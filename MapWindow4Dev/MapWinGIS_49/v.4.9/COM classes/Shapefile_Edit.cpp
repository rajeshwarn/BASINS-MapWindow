//********************************************************************************************************
//File name: Shapefile.cpp
//Description: Implementation of the CShapefile (see other cpp files as well)
//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Open Source. 
//
//The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
//Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
//public domain in March 2004.  
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
// -------------------------------------------------------------------------------------------------------
// lsu 3-02-2011: split the initial Shapefile.cpp file to make entities of the reasonble size
#pragma once
#include "stdafx.h"
#include "Shapefile.h"
#include "TableClass.h"
#include "Charts.h"

#pragma region StartEditing

// ************************************************************
//		StartEditingShapes()
// ************************************************************
STDMETHODIMP CShapefile::StartEditingShapes(VARIANT_BOOL StartEditTable, ICallback *cBack, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retval = VARIANT_FALSE;
	
	bool callbackIsNull = (globalCallback == NULL);
	if(cBack != NULL && globalCallback == NULL)
	{
		globalCallback = cBack;	
		globalCallback->AddRef();
	}
	
	if( dbf == NULL || m_sourceType == sstUninitialized)
	{	
		// Error: shapefile is not initialized
		ErrorMessage(tkSHAPEFILE_UNINITIALIZED);
	}
	else if( _isEditingShapes )
	{	
		// editing is going on already
		*retval = VARIANT_TRUE;
	}
	else if (m_writing) 
	{
		ErrorMessage(tkSHP_READ_VIOLATION);
	}
	else
	{
		// quad tree generation
		IExtents * box = NULL;	
		double xm,ym,zm,xM,yM,zM;
		if(useQTree)
		{
			if (qtree)
			{
				delete qtree;
				qtree = NULL;
			}
			IExtents * box = NULL;	
			this->get_Extents(&box);
			box->GetBounds(&xm,&ym,&zm,&xM,&yM,&zM);
			box->Release();
			qtree = new QTree(QTreeExtent(xm,xM,yM,ym));
		}
		
		// reading shapes into memory
		IShape * shp = NULL;
		lastErrorCode = tkNO_ERROR;			// TODO: why?
		long percent = 0, newpercent = 0; 
		
		int size = (int)_shapeData.size();
		for( int i = 0; i < size; i++) //_numShapes; i++ )
		{	
			this->get_Shape(i, &shp);

			if( lastErrorCode != tkNO_ERROR )
			{	
				ErrorMessage(lastErrorCode);
				ReleaseMemoryShapes();
				return S_OK;
			}
			
			_shapeData[i]->shape = shp;
			
			// TODO: can be optimized wihtout using IExtents
			this->QuickExtents(i, &box);
			box->GetBounds(&xm,&ym,&zm,&xM,&yM,&zM);
			box->Release();

			// Neio 2009 07 21 QuadTree
			if(useQTree)
			{
				QTreeNode node;
				node.Extent.left = xm;
				node.Extent.right= xM;
				node.Extent.top = yM;
				node.Extent.bottom = ym;
				node.index = i;
				qtree->AddNode(node);
			}
			
			newpercent = (long)((i + 1.0)/size * 100); //_numShapes * 100);
			if( newpercent > percent )
			{	
				percent = newpercent;
				if( globalCallback != NULL )
					globalCallback->Progress(OLE2BSTR(key),percent,A2BSTR("Reading shapes into memory"));
			}
		}
		*retval = VARIANT_TRUE;
	
		// releasing data for the fast non-edit mode
		if (_fastMode )
		{
			for (unsigned int i = 0; i < _shapeData.size(); i++)
			{
				ASSERT(_shapeData[i]->fastData);
				if (_shapeData[i]->fastData)
				{
					delete _shapeData[i]->fastData;
					_shapeData[i]->fastData = NULL;
				}
			}
		}

		// ------------------------------------------
		// reading table into memory
		// ------------------------------------------
		if(StartEditTable != VARIANT_FALSE)
		{
			StartEditingTable(globalCallback,retval);
		}
		
		if (*retval == VARIANT_FALSE)
		{
			ErrorMessage(dbf->get_LastErrorCode(&lastErrorCode));
			ReleaseMemoryShapes();
		}
		else
		{
			_isEditingShapes = TRUE;
		}
	}

	if (callbackIsNull)
	{
		globalCallback = NULL;
	}
	return S_OK;
}
#pragma endregion

#pragma region StopEditing

// ********************************************************
//		StopEditingShapes()
// ********************************************************
STDMETHODIMP CShapefile::StopEditingShapes(VARIANT_BOOL ApplyChanges, VARIANT_BOOL StopEditTable, ICallback *cBack, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retval = VARIANT_FALSE;

	bool callbackIsNull = (globalCallback == NULL);
	if(cBack != NULL && globalCallback == NULL)
	{
		globalCallback = cBack;	
		globalCallback->AddRef();
	}
	
	if( dbf == NULL || m_sourceType == sstUninitialized)
	{
		ErrorMessage(tkSHAPEFILE_UNINITIALIZED, cBack);
	}
	else if( _isEditingShapes == FALSE )
	{	
		*retval = VARIANT_TRUE;
	}
	else if ( m_writing )
	{
		ErrorMessage(tkSHP_WRITE_VIOLATION, cBack);
	}
	else if ( m_sourceType == sstInMemory )
	{
		// shapefile wan't saved before
		if(_shpfileName.GetLength() > 0)
		{
			this->Save(cBack, retval);
			
			if (*retval)
			{
				// TODO: stop editing with discarding changes
				_isEditingShapes = VARIANT_FALSE;
			}
		}
		else
		{
			// there is no file name; only SaveAs method will help
			// Error: the name for in-memory shapefile wasn't specified
			ErrorMessage(tkINVALID_FILENAME, cBack);
		}
	}
	else
	{
		USES_CONVERSION;

		if( ApplyChanges )
		{	
			m_writing = true;
			
			// verify Shapefile Integrity
			if( verifyMemShapes(cBack) == FALSE )
			{	
				// error Code is set in function
			}
			else
			{
				_shpfile = freopen(_shpfileName,"wb+", _shpfile);
				_shxfile = freopen(_shxfileName,"wb+",_shxfile);

				if( _shpfile == NULL || _shxfile == NULL )
				{	
					if( _shxfile != NULL )
					{	
						fclose( _shxfile );
						_shxfile = NULL;
						
					}
					if( _shpfile != NULL )
					{	
						fclose( _shpfile );
						_shpfile = NULL;
						ErrorMessage(tkCANT_OPEN_SHP, cBack);
					}
				}
				else
				{
					// force computation of Extents
					VARIANT_BOOL vbretval;
					this->RefreshExtents(&vbretval);

					writeShp(_shpfile,cBack);
					writeShx(_shxfile,cBack);

					_shpfile = freopen(_shpfileName,"rb+", _shpfile);
					_shxfile = freopen(_shxfileName,"rb+",_shxfile);
					
					if( _shpfile == NULL || _shxfile == NULL )
					{	
						if( _shxfile != NULL )
						{	
							fclose( _shxfile );
							_shxfile = NULL;
							ErrorMessage(tkCANT_OPEN_SHX, cBack);
						}
						if( _shpfile != NULL )
						{	
							fclose( _shpfile );
							_shpfile = NULL;
							ErrorMessage(tkCANT_OPEN_SHP, cBack);
						}
					}
					else
					{	
						_isEditingShapes = FALSE;
						ReleaseMemoryShapes();
						*retval = VARIANT_TRUE;

						if(StopEditTable != VARIANT_FALSE)
							StopEditingTable(ApplyChanges,cBack,retval);
					}
				}
			}
			m_writing = false;
		}
		else
		{	
			// -----------------------------------------------------
			// discard the Changes
			// -----------------------------------------------------
			_isEditingShapes = FALSE;
			ReleaseMemoryShapes();

			// TODO: is it needed ?
			// reload the shx file
			this->readShx();

			if(StopEditTable != VARIANT_FALSE)
			{
				StopEditingTable(ApplyChanges,cBack,retval);
			}

			// TODO: the number of shapes can be different; labels, charts, etc will be lost as a result
			// the initial state should be copied somewhere for not loosing drawing options
			// TODO: is it needed?
			/*if (_shapeData.size() != _numShapes)
			{
				_shapeData.clear();
				_shapeData.resize(_numShapes);
			}*/
			*retval = VARIANT_TRUE;
		}
		
		// restoring fast mode
		if (*retval == VARIANT_TRUE && _fastMode)
		{
			this->put_FastMode(VARIANT_FALSE);
			this->put_FastMode(VARIANT_TRUE);
		}
	}
	
	// restoring callback state
	if (callbackIsNull)
		globalCallback = NULL;

	return S_OK;
}
#pragma endregion

#pragma region Operations

// ***********************************************************
//		EditInsertShape()
// ***********************************************************
STDMETHODIMP CShapefile::EditInsertShape(IShape *Shape, long *ShapeIndex, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retval = VARIANT_FALSE;
	
 	if( dbf == NULL || m_sourceType == sstUninitialized )
	{	
		ErrorMessage(tkSHAPEFILE_UNINITIALIZED);
	}
	else if(!_isEditingShapes)
	{
		ErrorMessage(tkSHPFILE_NOT_IN_EDIT_MODE);
	}
	else
	{
		VARIANT_BOOL isEditingTable;
		dbf->get_EditingTable(&isEditingTable);
		
		if(!isEditingTable)
		{
			ErrorMessage(tkDBF_NOT_IN_EDIT_MODE);
		}
		else
		{
			if( Shape == NULL )
			{	
				ErrorMessage(tkUNEXPECTED_NULL_PARAMETER);
			}
			else
			{
				ShpfileType shapetype;
				Shape->get_ShapeType(&shapetype);
				
				if( shapetype != SHP_NULLSHAPE && shapetype != _shpfiletype)
				{	
					ErrorMessage(tkINCOMPATIBLE_SHAPEFILE_TYPE);
				}
				else
				{
					// wrong index will be corrected
					if( *ShapeIndex < 0 )
					{
						*ShapeIndex = 0;
					}
					else if( *ShapeIndex > (int)_shapeData.size() )
					{
						*ShapeIndex = _shapeData.size();
					}
					
					// adding the row in table
					dbf->EditInsertRow( ShapeIndex, retval );
					
					if( *retval == VARIANT_FALSE )
					{	
						dbf->get_LastErrorCode(&lastErrorCode);
						ErrorMessage(lastErrorCode);
					}			
					else
					{	
						VARIANT_BOOL bSynchronized;
						m_labels->get_Synchronized(&bSynchronized);

						ShapeData* data = new ShapeData();
						Shape->AddRef();
						data->shape = Shape;
						_shapeData.insert(_shapeData.begin() + *ShapeIndex, data);

						// shape must have corrct underlying data structure
						// shapes not bound to shapefile all use CShapeWrapperCOM underlying class
						// and if fast mode is set to true, CShapeWrapper class is expected
						if (this->_fastMode != ((CShape*)Shape)->get_fastMode())
						{
							((CShape*)Shape)->put_fastMode(this->_fastMode?true:false);
						}

						// updating labels
						if (dbf) 
						{
							double x = 0.0, y = 0.0, rotation = 0.0;
							VARIANT_BOOL vbretval;

							bool chartsExist = ((CCharts*)m_charts)->_chartsExist;
							if (bSynchronized || chartsExist)
							{
								// position
								tkLabelPositioning positioning;
								m_labels->get_Positioning(&positioning);

								tkLineLabelOrientation orientation;
								m_labels->get_LineOrientation(&orientation);
								
								((CShape*)Shape)->get_LabelPosition(positioning, x, y, rotation, orientation);
							}
							
							if (bSynchronized)
							{
								// it doesn't make sense to recalculate expression as dbf cells are empty all the same
								CString text;
								m_labels->InsertLabel(*ShapeIndex, A2BSTR(text), x, y, rotation, -1, &vbretval);
							}

							if (chartsExist)
							{
								if (!_shapeData[*ShapeIndex]->chart)
								{
									_shapeData[*ShapeIndex]->chart = new CChartInfo();
									_shapeData[*ShapeIndex]->chart->x = x;
									_shapeData[*ShapeIndex]->chart->y = y;
								}
							}
						}
						
						// extending the bounds of the shapefile we don't care if the bounds became less
						// it's necessarry to call RefreshExtents in this case, for zoom to layer working right
						IExtents * box;
						Shape->get_Extents(&box);
						double xm,ym,zm,xM,yM,zM;
						box->GetBounds(&xm,&ym,&zm,&xM,&yM,&zM);

						if (_shapeData.size() == 1)
						{
							_minX = xm;
							_maxX = xM;
							_minY = ym;
							_maxY = yM;
							_minZ = zm;
							_maxZ = zM;
						}
						else
						{
							
							if (xm < _minX) _minX = xm;
							if (xM > _maxX) _maxX = xM;
							if (ym < _minY) _minY = ym;
							if (yM > _maxY) _maxY = yM;
							if (zm < _minZ) _minZ = zm;
							if (zM > _maxZ) _maxZ = zM;
						}
						box->Release();

						// Neio 07/23/2009 - add qtree
						if(useQTree)
						{
							QTreeNode node;
							
							//node.index = memShapes.size() - 1;
							node.index = *ShapeIndex;
							node.Extent.left = xm;
							node.Extent.right = xM;
							node.Extent.top = yM;
							node.Extent.bottom = ym;
							qtree->AddNode(node);
						}
						*retval = VARIANT_TRUE;
					}
					
					((CTableClass*)dbf)->set_IndexValue(*ShapeIndex);
				
					// lsu: 11 aug 2011: substituted by the line above

					//long nFields = 0;
					//dbf->get_NumFields(&nFields);
					//for (int z = 0; z < nFields; z++)
					//{
					//	IField * fld;
					//	BSTR fldName;

					//	dbf->get_Field(z, &fld);
					//	fld->get_Name(&fldName);
					//	fld->Release();
					//	
					//	USES_CONVERSION;
					//	if (_stricmp(W2A(fldName), "MWShapeID") == 0)
					//	{
					//		VARIANT_BOOL rt;
					//		VARIANT val;
					//		VariantInit(&val);
					//		val.vt = VT_I4;
					//		val.lVal = FindNewShapeID(z);
					//		EditCellValue(z, *ShapeIndex, val, &rt);
					//		VariantClear(&val);
					//		SysFreeString(fldName); // tws 6/6/7
					//		break;
					//	}
					//	SysFreeString(fldName);		// tws 6/6/7
					//}
				}
			}
		}
	}
	return S_OK;
}

// *********************************************************************
//		EditDeleteShape()
// *********************************************************************
STDMETHODIMP CShapefile::EditDeleteShape(long ShapeIndex, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retval = VARIANT_FALSE;

	if( dbf == NULL || m_sourceType == sstUninitialized )
	{	
		ErrorMessage(tkSHAPEFILE_UNINITIALIZED);
	}
	else if(!_isEditingShapes)
	{
		ErrorMessage(tkSHPFILE_NOT_IN_EDIT_MODE);
	}
	else
	{
		VARIANT_BOOL isEditingTable;
		dbf->get_EditingTable(&isEditingTable);
		
		if(!isEditingTable)
		{
			ErrorMessage(tkDBF_NOT_IN_EDIT_MODE);
		}
		else if( ShapeIndex < 0 || ShapeIndex >= (int)_shapeData.size() )
		{	
			ErrorMessage(tkINDEX_OUT_OF_BOUNDS);
		}
		else
		{
			VARIANT_BOOL vbretval;
			dbf->EditDeleteRow( ShapeIndex, &vbretval);
			
			if(!vbretval)
			{	
				dbf->get_LastErrorCode(&lastErrorCode);
				ErrorMessage(lastErrorCode);
			}			
			else
			{	
				delete _shapeData[ShapeIndex];
				_shapeData.erase( _shapeData.begin() + ShapeIndex );
				//_numShapes--;	// todo: remove
				
				// TODO: rewrite
				VARIANT_BOOL bSynchronized;
				if(m_labels->get_Synchronized(&bSynchronized))
					m_labels->RemoveLabel(ShapeIndex, &vbretval);
				
				*retval = VARIANT_TRUE;
			}
		}
	}
	return S_OK;
}

// ***********************************************************
//		EditClear()
// ***********************************************************
STDMETHODIMP CShapefile::EditClear(VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retval = VARIANT_FALSE;

	if (dbf == NULL || m_sourceType == sstUninitialized)
	{
		ErrorMessage(tkSHAPEFILE_UNINITIALIZED);
	}
	else if( _isEditingShapes == FALSE )
	{	
		ErrorMessage(tkSHPFILE_NOT_IN_EDIT_MODE);
	}
	else
	{
		VARIANT_BOOL isEditingTable;
		dbf->get_EditingTable(&isEditingTable);
	
		if( isEditingTable == FALSE )
		{	
			ErrorMessage(tkDBF_NOT_IN_EDIT_MODE);
		}
		else
		{	
			dbf->EditClear(retval);
			if( *retval == VARIANT_FALSE )
			{	
				dbf->get_LastErrorCode(&lastErrorCode);
				ErrorMessage(lastErrorCode);
			}
			
			//_numShapes = 0;		// todo: remove
			
			for (unsigned int i = 0; i < _shapeData.size(); i++)
			{
				delete _shapeData[i];	// all the releasing done in the destructor
			}
			_shapeData.clear();
			
			if(useQTree == VARIANT_TRUE)
			{
				delete qtree;
				qtree = NULL;
			}

			// deleting the labels
			VARIANT_BOOL bSynchronized;
			m_labels->get_Synchronized(&bSynchronized);
			if (bSynchronized)
			{
				m_labels->Clear();
			}
			*retval = VARIANT_TRUE;
		}
	}
	return S_OK;
}
#pragma endregion

#pragma region CacheExtents
// ****************************************************************
//		get_CacheExtents()
// ****************************************************************
STDMETHODIMP CShapefile::get_CacheExtents(VARIANT_BOOL * pVal)
{
	// The property no longer used
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = VARIANT_FALSE;
	return S_OK;
}

// ****************************************************************
//		put_CacheExtents()
// ****************************************************************
STDMETHODIMP CShapefile::put_CacheExtents(VARIANT_BOOL newVal)
{
	// The property no longer used
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return S_OK;
}

// ********************************************************************
//		RefreshExtents()
// ********************************************************************
STDMETHODIMP CShapefile::RefreshExtents(VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	if (_isEditingShapes)
	{	
		IExtents * box=NULL;
		double Xmin, Ymin, Zmin, Mmin, Xmax, Ymax, Zmax, Mmax;
		
		_minX = 0.0, _maxX = 0.0;
		_minY = 0.0, _maxY = 0.0;
		_minZ = 0.0, _maxZ = 0.0;
		_minM = 0.0, _maxM = 0.0;
		
		for( int i = 0; i < (int)_shapeData.size(); i++ )
		{	
			// refresh shapes extents
			_shapeData[i]->shape->get_Extents(&box);
			box->GetBounds(&Xmin, &Ymin, &Zmin, &Xmax, &Ymax, &Zmax);
			box->GetMeasureBounds(&Mmin, &Mmax);
			box->Release();

			// refresh shapefile extents
			if (i==0)
			{
				_minX = Xmin, _maxX = Xmax;
				_minY = Ymin, _maxY = Ymax;
				_minZ = Zmin, _maxZ = Zmax;
				_minM = Mmin, _maxM = Mmax;
			}
			else	
			{	if( Xmin < _minX )	_minX = Xmin; 
				if( Xmax > _maxX )	_maxX = Xmax;
				if( Ymin < _minY )	_minY = Ymin;
				if( Ymax > _maxY )	_maxY = Ymax;
				if( Zmin < _minZ )	_minZ = Zmin;
				if( Zmax > _maxZ )	_maxZ = Zmax;
				if( Mmin < _minM )	_minM = Mmin;
				if( Mmax > _maxM )	_maxM = Mmax;
			}
		}
	}
	*retval = VARIANT_TRUE;
	return S_OK;
}

// ********************************************************************
//		RefreshShapeExtents()
// ********************************************************************
STDMETHODIMP CShapefile::RefreshShapeExtents(LONG ShapeId, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	// The method is no longer used
	*retval = VARIANT_TRUE;
	return S_OK;
}
#pragma endregion

#pragma region Utilities
// ********************************************************************
//		ReleaseMemoryShapes()
// ********************************************************************
BOOL CShapefile::ReleaseMemoryShapes()
{
	int size = (int)_shapeData.size();
	for( int i = 0; i < size; i++ )
	{	
		if (_shapeData[i]->shape)
		{
			_shapeData[i]->shape->Release();
			_shapeData[i]->shape = NULL;
		}
	}

	if(useQTree == VARIANT_TRUE)
	{
		delete qtree;
		qtree = NULL;
	}
	return S_OK;
}

// ****************************************************************
//		verifyMemShapes
// ****************************************************************
//Verify Shapefile Integrity
BOOL CShapefile::verifyMemShapes(ICallback * cBack)
{
	ShpfileType shapetype;
	long numPoints;
	long numParts;
	IPoint * firstPnt = NULL;
	IPoint * lastPnt = NULL;
	VARIANT_BOOL vbretval = VARIANT_FALSE;
	
	if (!globalCallback && cBack)
	{
		globalCallback = cBack;
		globalCallback->AddRef();
	}

	//for( int i = 0; i < (int)memShapes.size(); i++ )
	for( int i = 0; i < (int)_shapeData.size(); i++ )
	{						
		IShape* shp = _shapeData[i]->shape;
		if ( !shp ) 
			continue;
		
		shp->get_ShapeType(&shapetype);
		shp->get_NumPoints(&numPoints);
		shp->get_NumParts(&numParts);


		if( shapetype != SHP_NULLSHAPE && shapetype != _shpfiletype )
		{	
			
			ErrorMessage(tkINCOMPATIBLE_SHAPE_TYPE);
			return FALSE;
		}
		else if( shapetype == SHP_POINT || shapetype == SHP_POINTZ || shapetype == SHP_POINTM )
		{	
			if( numPoints == 0 )
			{	
				ShpfileType tmpshptype = SHP_NULLSHAPE;
				shp->put_ShapeType(tmpshptype);
			}
		}
		else if( shapetype == SHP_POLYLINE || shapetype == SHP_POLYLINEZ || shapetype == SHP_POLYLINEM )
		{	
			if( numPoints < 2 )
			{	
				ShpfileType tmpshptype = SHP_NULLSHAPE;
				shp->put_ShapeType(tmpshptype);
			}
			else if( numParts == 0 )
			{	
				long partindex = 0;
				shp->InsertPart(0,&partindex,&vbretval);						
			}
		}
		else if( shapetype == SHP_POLYGON || shapetype == SHP_POLYGONZ || shapetype == SHP_POLYGONM )
		{	
			if( numPoints < 2 )
			{	
				ShpfileType tmpshptype = SHP_NULLSHAPE;
				shp->put_ShapeType(tmpshptype);
			}
			else 
			{	
				if( numParts == 0 )
				{	
					long partindex = 0;
					shp->InsertPart(0,&partindex,&vbretval);
					numParts = 1;
				}
				
				//force the first and last point of a ring to be the same
				long partOffset = 0;
				for( int p = 0; p < numParts; p++ )
				{
					long startRing;
					shp->get_Part(p,&startRing);
					long endRing = 0;
					if( p == numParts - 1 )
						endRing = numPoints;	
					else
						shp->get_Part(p+1,&endRing);

					if( startRing < 0 || startRing >= numPoints + partOffset )
						startRing = 0;
					if( endRing < startRing || endRing >= numPoints + partOffset )
						endRing = numPoints + partOffset;
					
					shp->get_Point(startRing,&firstPnt);
					shp->get_Point(endRing - 1,&lastPnt);
					
					double x1, y1, z1;
					double x2, y2, z2;
					
					if ( firstPnt && lastPnt )
					{
						firstPnt->get_X(&x1);
						firstPnt->get_Y(&y1);
						firstPnt->get_Z(&z1);

						lastPnt->get_X(&x2);
						lastPnt->get_Y(&y2);
						lastPnt->get_Z(&z2);

						// make sure first and last point are the same for each part
						if( x1 != x2 || y1 != y2 || z1 != z2 )
						{	
							VARIANT_BOOL retval;
							shp->InsertPoint(firstPnt, &endRing, &retval);
							for( int t = p+1; t < numParts; t++ )
							{	
								shp->get_Part(t,&startRing);
								shp->put_Part(t,startRing+1);
								partOffset++;
							}
						}
					}
					if ( firstPnt )
					{
						firstPnt->Release();
						firstPnt = NULL;		
					}
					
					if ( lastPnt )
					{
						lastPnt->Release();
						lastPnt = NULL;
					}
				}
			}
		}
		else if( shapetype == SHP_MULTIPOINT || shapetype == SHP_MULTIPOINTZ || shapetype == SHP_MULTIPOINTM )
		{	
			if( numPoints == 0 )
			{	
				ShpfileType tmpshptype = SHP_NULLSHAPE;
				shp->put_ShapeType(tmpshptype);
			}
		}
	}
	return TRUE;
}
#pragma endregion