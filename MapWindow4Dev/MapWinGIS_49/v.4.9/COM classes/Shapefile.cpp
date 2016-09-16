//********************************************************************************************************
//File name: Shapefile.cpp
//Description: Implementation of the CShapefile
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
// 06/06/2007 - T Shanley (tws) - fixed some memory leaks in EditInsertShape
// 07/03/2009 - Sergei Leschinski (lsu) - caching of shape extents for faster redraw in edit mode 
//				(see http://www.mapwindow.org/phorum/read.php?5,13065).
//				New properties/methods: RefreshShapeExtents, RefreshExtents, CacheExtents.
//				Works only in edit mode (Shapefile.EditingShapes = true). Set Shapefile.CacheExtents = true 
//				to activate this functionality. When changes are made to certain shapes RefreshShapeExtents 
//				must be used to update cached information. RefreshExtents is used to update extents for 
//				a whole shapefile. shpBounds vector is used to store extents. 
// 07/23/2009 - �ܴϻ� (Neio Zhou) - 
//              QuadTree support in Edit mode, see (http://www.mapwindow.org/phorum/read.php?5,13404)
//			    use UseQTree property to switch if use the quadtree.
//              The performance is much better when use both cacheExtents = true and UseQTree = true.
//              Add Save Method to save without exiting edit mode
//              Change the bahavior of SaveAs (This beed to be discussed)
//              Change the selectShapes in Edit Mode in order to fix a bug that cannot select inserted shapes
// 08-24-2009 (sm) Fixes to performance issues with the spatial index trees as per 
//				http://www.mapwindow.org/phorum/read.php?5,13738
// 27 aug 2009 lsu Added support for selection. GetIntersection, SpatialQuery, SelectShapesAlt functions.
                       
#include "stdafx.h"
#include "Shapefile.h"
#include "Varh.h"
#include "Projections.h"		// ProjectionTools
//#include "UtilityFunctions.h"
#include "Labels.h"
#include "Charts.h"
#include "GeoProjection.h"

#ifdef _DEBUG
	#define new DEBUG_NEW
	#undef THIS_FILE
	static char THIS_FILE[] = __FILE__;
#endif

#pragma region Properties	
// ************************************************************
//		get_EditingShapes()
// ************************************************************
STDMETHODIMP CShapefile::get_EditingShapes(VARIANT_BOOL *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*pVal = _isEditingShapes?VARIANT_TRUE:VARIANT_FALSE;
	return S_OK;
}

// ************************************************************
//		get_LastErrorCode()
// ************************************************************
STDMETHODIMP CShapefile::get_LastErrorCode(long *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*pVal = lastErrorCode;
	lastErrorCode = tkNO_ERROR;
	return S_OK;
}

// ************************************************************
//		get_CdlgFilter()
// ************************************************************
STDMETHODIMP CShapefile::get_CdlgFilter(BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	*pVal = A2BSTR("ESRI Shapefiles (*.shp)|*.shp");
	return S_OK;
}

// ************************************************************
//		get/put_GlobalCallback()
// ************************************************************
STDMETHODIMP CShapefile::get_GlobalCallback(ICallback **pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = globalCallback;
	if( globalCallback != NULL )
		globalCallback->AddRef();
	return S_OK;
}
STDMETHODIMP CShapefile::put_GlobalCallback(ICallback *newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	put_ComReference(newVal, (IDispatch**)&globalCallback);
	if( dbf != NULL )
		dbf->put_GlobalCallback(newVal);

	return S_OK;
}

// ************************************************************
//		get/put_Key()
// ************************************************************
STDMETHODIMP CShapefile::get_Key(BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	*pVal = OLE2BSTR(key);
	return S_OK;
}
STDMETHODIMP CShapefile::put_Key(BSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	::SysFreeString(key);
	key = OLE2BSTR(newVal);
	return S_OK;
}

// ************************************************************
//		get/put_VisibilityExpression
// ************************************************************
STDMETHODIMP CShapefile::get_VisibilityExpression(BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	*pVal = OLE2BSTR(_expression);
	return S_OK;
}
STDMETHODIMP CShapefile::put_VisibilityExpression(BSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	::SysFreeString(_expression);
	_expression = OLE2BSTR(newVal);
	return S_OK;
}

// *****************************************************************
//		get_NumShapes()
// *****************************************************************
STDMETHODIMP CShapefile::get_NumShapes(long *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*pVal = _shapeData.size(); //_numShapes;
	return S_OK;
}

// **************************************************************
//		get_NumFields()
// **************************************************************
STDMETHODIMP CShapefile::get_NumFields(long *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	if( dbf != NULL )
		dbf->get_NumFields(pVal);
	else
	{	
		ErrorMessage(tkFILE_NOT_OPEN);
		*pVal = 0;
	}
	return S_OK;
}

// ************************************************************
//		get_ShapefileType()
// ************************************************************
STDMETHODIMP CShapefile::get_ShapefileType(ShpfileType *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*pVal = _shpfiletype;
	return S_OK;
}

// *****************************************************************
//	   get_ErrorMsg()
// *****************************************************************
STDMETHODIMP CShapefile::get_ErrorMsg(long ErrorCode, BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	*pVal = A2BSTR(ErrorMsg(ErrorCode));
	return S_OK;
}

// *****************************************************************
//	   get_FileHandle()
// *****************************************************************
STDMETHODIMP CShapefile::get_FileHandle(long * pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	if( _shpfile != NULL )
	{	
		int handle = _fileno(_shpfile);
		*pVal = _dup(handle);				
	}
	else
		*pVal = -1;

	return S_OK;
}

// **************************************************************
//	   get_Filename()
// **************************************************************
STDMETHODIMP CShapefile::get_Filename(BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = A2BSTR(_shpfileName);

	return S_OK;
}

// **************************************************************
//		ErrorMessage()
// **************************************************************
void CShapefile::ErrorMessage(long ErrorCode)
{
	lastErrorCode = ErrorCode;
	if( globalCallback != NULL) globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
	return;
}
void CShapefile::ErrorMessage(long ErrorCode, ICallback* cBack)
{
	lastErrorCode = ErrorCode;
	if( globalCallback != NULL) globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
	// in case additional callback was provided we shall use it as well
	if( cBack != NULL) cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
	return;
}

// ************************************************************
//		get_MinDrawingSize()
// ************************************************************
STDMETHODIMP CShapefile::get_MinDrawingSize(LONG* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = _minDrawingSize;
	return S_OK;
}
STDMETHODIMP CShapefile::put_MinDrawingSize(LONG newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	_minDrawingSize = newVal;
	return S_OK;
}

// ************************************************************
//		get_SourceType()
// ************************************************************
STDMETHODIMP CShapefile::get_SourceType(tkShapefileSourceType* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = m_sourceType;
	return S_OK;
}

#pragma endregion

#pragma region CreateAndOpen
// *****************************************************
//		ApplyRandomDrawingOptions()
// *****************************************************
void CShapefile::ApplyRandomDrawingOptions()
{
	ShpfileType type = _shpfiletype;
	
	// setting default colors for shapefile layer
	IShapeDrawingOptions* options = NULL;
	this->get_DefaultDrawingOptions(&options);
	if (options)
	{
		//srand( (unsigned)time( NULL ));
		unsigned char r1, r2, g1, g2, b1, b2;

		if( type == SHP_POLYLINE || type == SHP_POLYLINEZ || type == SHP_POLYLINEM )
		{
			r1 = g1 = b1 = 75;	 // dark colors
			r2 = g2 = b2 = 150;
			unsigned char r = r1 + unsigned char(double(rand()/double(RAND_MAX) * (r2 - r1)));
			unsigned char g = g1 + unsigned char(double(rand()/double(RAND_MAX) * (g2 - g1)));
			unsigned char b = b1 + unsigned char(double(rand()/double(RAND_MAX) * (b2 - b1)));
			options->put_LineColor(RGB(r,g,b));
			options->put_FillColor(RGB(255,255,255));
		}
		else if( type == SHP_POLYGON || type == SHP_POLYGONZ || type == SHP_POLYGONM )
		{
			r1 = g1 = b1 = 180;	// light colors
			r2 = g2 = b2 = 230;
			unsigned char r = r1 + unsigned char(double(rand()/double(RAND_MAX) * (r2 - r1)));
			unsigned char g = g1 + unsigned char(double(rand()/double(RAND_MAX) * (g2 - g1)));
			unsigned char b = b1 + unsigned char(double(rand()/double(RAND_MAX) * (b2 - b1)));
			options->put_FillColor(RGB(r,g,b));

			// grey color for outlines
			options->put_LineColor(RGB(150,150,150));
		}
		else   // point and multipoints
		{
			r1 = g1 = b1 = 50;
			r2 = g2 = b2 = 150;
			unsigned char r = r1 + unsigned char(double(rand()/double(RAND_MAX) * (r2 - r1)));
			unsigned char g = g1 + unsigned char(double(rand()/double(RAND_MAX) * (g2 - g1)));
			unsigned char b = b1 + unsigned char(double(rand()/double(RAND_MAX) * (b2 - b1)));
			options->put_FillColor(RGB(r,g,b));

			// grey color for outlines
			options->put_LineColor(RGB(150,150,150));
		}
		options->Release();
		options = NULL;
	}

	this->get_SelectionDrawingOptions(&options);
	if (options)
	{
		if( type == SHP_POLYLINE || type == SHP_POLYLINEZ || type == SHP_POLYLINEM )
		{
			options->put_LineColor(RGB(255,255,0));
		}
		else if( type == SHP_POLYGON || type == SHP_POLYGONZ || type == SHP_POLYGONM )
		{
			options->put_FillColor(RGB(255,255,0));
			// grey color for outlines
			options->put_LineColor(RGB(150,150,150));
		}
		else   // point and multipoints
		{
			options->put_FillColor(RGB(255, 255,0));
			// grey color for outlines
			options->put_LineColor(RGB(150,150,150));
		}
		options->Release();
		options = NULL;
	}

	ILabels* labels;
	this->get_Labels(&labels);
	if (labels)
	{
		if (type == SHP_POINT || type == SHP_POINTZ || type == SHP_POINTM)
		{
			labels->put_Alignment(laCenterRight);
		}
		labels->Release();
	}
}

// ************************************************************
//		Open()
// ************************************************************
STDMETHODIMP CShapefile::Open(BSTR ShapefileName, ICallback *cBack, VARIANT_BOOL *retval)
{	
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retval = VARIANT_FALSE;
	VARIANT_BOOL vbretval;

	if(globalCallback == NULL && cBack !=NULL)
	{
		globalCallback = cBack;
		globalCallback->AddRef();
	}
	
	USES_CONVERSION;
	CString tmp_shpfileName = OLE2CA(ShapefileName);

	if (tmp_shpfileName.GetLength() == 0)
	{
		// better to use CreateNew directly, but this call will be preserved for backward compatibility
		this->CreateNew(A2BSTR(""), _shpfiletype, &vbretval);
	}
	else if( tmp_shpfileName.GetLength() <= 3 )
	{	
		ErrorMessage(tkINVALID_FILENAME);
	}
	else
	{
		// close the opened shapefile
		this->Close(&vbretval);

		if( vbretval == VARIANT_FALSE )
		{
			// error code in the function
			return S_FALSE;
		}
		
		// saving the provided names; 
		// from now on we must clean the class varaibles in case the operation won't succeed
		_shpfileName = tmp_shpfileName;
		_shxfileName = tmp_shpfileName.Left(tmp_shpfileName.GetLength() - 3) + "shx";
		_dbffileName = tmp_shpfileName.Left(tmp_shpfileName.GetLength() - 3) + "dbf";
		_prjfileName = tmp_shpfileName.Left(tmp_shpfileName.GetLength() - 3) + "prj";

		// Chris Michaelis 12/19/2005 - Windows 98 doesn't support unicode and will thus crash and burn on _wfopen.
		OSVERSIONINFO OSversion;
		OSversion.dwOSVersionInfoSize=sizeof(OSVERSIONINFO);
		::GetVersionEx(&OSversion);

		if (OSversion.dwPlatformId == VER_PLATFORM_WIN32_WINDOWS)
		{
			// Running Windows 95, 98, ME
			_shpfile = fopen(_shpfileName, "rb");
			_shxfile = fopen(_shxfileName,"rb");
		}
		else
		{
			// Running 2k, XP, NT, or other future versions
			////Changes made to use _wfopen to support Assian character file names ---Lailin Chen 11/5/2005
			_shpfile = _wfopen( T2W(_shpfileName), L"rb");
			_shxfile = _wfopen(T2W(_shxfileName),L"rb");
		}
		
		// opening dbf
		ASSERT(dbf == null);
		CoCreateInstance(CLSID_Table,NULL,CLSCTX_INPROC_SERVER,IID_ITable,(void**)&dbf);
		dbf->put_GlobalCallback(globalCallback);
		dbf->Open(A2BSTR(_dbffileName), cBack, &vbretval);

		if( _shpfile == NULL )
		{
			ErrorMessage(tkCANT_OPEN_SHP);
			this->Close(&vbretval);
		}
		else if( _shxfile == NULL )
		{	
			ErrorMessage(tkCANT_OPEN_SHX);
			this->Close(&vbretval);
		}
		else if( vbretval == VARIANT_FALSE )
		{	
			dbf->get_LastErrorCode(&lastErrorCode);
			ErrorMessage(lastErrorCode);
			this->Close(&vbretval);
		}		
		else
		{	
			if( !readShx())		// shapefile header is read here as well
			{	
				ErrorMessage(tkINVALID_SHX_FILE);
				this->Close(&vbretval);
			}
			else
			{
				//Check for supported types
				if( _shpfiletype != SHP_NULLSHAPE &&
					_shpfiletype != SHP_POINT &&
					_shpfiletype != SHP_POLYLINE &&
					_shpfiletype != SHP_POLYGON &&
					_shpfiletype != SHP_POINTZ &&
					_shpfiletype != SHP_POLYLINEZ &&
					_shpfiletype != SHP_POLYGONZ &&
					_shpfiletype != SHP_MULTIPOINT &&
					_shpfiletype != SHP_MULTIPOINTZ &&
					_shpfiletype != SHP_POLYLINEM &&
					_shpfiletype != SHP_POLYGONM &&
					_shpfiletype != SHP_POINTM &&
					_shpfiletype != SHP_MULTIPOINTM )
				{
					ErrorMessage(tkUNSUPPORTED_SHAPEFILE_TYPE);
					this->Close(&vbretval);
				}
				else
				{
					//_shapeData.resize(_numShapes);
					_shapeData.resize(shpOffsets.size());
					for (unsigned int i = 0; i < _shapeData.size(); i++)
					{
						_shapeData[i] = new ShapeData();
					}

					if (_fastMode)
					{
						_fastMode = FALSE;
						this->put_FastMode(VARIANT_TRUE);
					}

					m_sourceType = sstDiskBased;

					// reading projection
					m_geoProjection->ReadFromFile(A2BSTR(_prjfileName), &vbretval);

					this->ApplyRandomDrawingOptions();

					*retval = VARIANT_TRUE;
				}
			}
		}		
	}
	UpdateLabelsPositioning();
	return S_OK;
}


// *********************************************************
//		CreateNew()
// *********************************************************
STDMETHODIMP CShapefile::CreateNew(BSTR ShapefileName, ShpfileType ShapefileType, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retval = VARIANT_FALSE;
	VARIANT_BOOL vbretval;

	// check for supported types
	if( ShapefileType != SHP_NULLSHAPE &&
		ShapefileType != SHP_POINT &&
		ShapefileType != SHP_POLYLINE &&
		ShapefileType != SHP_POLYGON &&
		ShapefileType != SHP_POINTZ &&
		ShapefileType != SHP_POLYLINEZ &&
		ShapefileType != SHP_POLYGONZ &&
		ShapefileType != SHP_MULTIPOINT &&
		ShapefileType != SHP_MULTIPOINTZ &&
		ShapefileType != SHP_POLYLINEM &&
		ShapefileType != SHP_POLYGONM &&
		ShapefileType != SHP_MULTIPOINTM )
	{
		ErrorMessage(tkUNSUPPORTED_SHAPEFILE_TYPE);
	}
	else
	{
		USES_CONVERSION;
		CString tmp_shpfileName = OLE2CA(ShapefileName);
		
		// ----------------------------------------------
		// in memory shapefile (without name)
		// ----------------------------------------------
		if (tmp_shpfileName.GetLength() == 0)
		{
			// closing the old shapefile (error code inside the function)
			Close(&vbretval);
			
			if( vbretval == VARIANT_TRUE )
			{	
				CoCreateInstance(CLSID_Table,NULL,CLSCTX_INPROC_SERVER,IID_ITable,(void**)&dbf);
				if (!dbf)
				{
					ErrorMessage(tkCANT_COCREATE_COM_INSTANCE);
				}
				else
				{
					BSTR newdbfName = A2BSTR("");
					dbf->CreateNew(newdbfName, &vbretval);
					
					if (!vbretval)
					{
						long error;
						dbf->get_LastErrorCode(&error);
						ErrorMessage(error);
						dbf->Release();
						dbf = NULL;
					}
					else
					{
						_shpfiletype = ShapefileType;
						_isEditingShapes = true;
						m_sourceType = sstInMemory;

						this->ApplyRandomDrawingOptions();

						*retval = VARIANT_TRUE;
					}
				}
			}
		}
		else if (tmp_shpfileName.GetLength() <= 3)
		{
			ErrorMessage(tkINVALID_FILENAME);
		}
		else
		{
			// ----------------------------------------------
			// in memory shapefile (name specified, is acceptable and available)
			// ----------------------------------------------
			CString shpName, shxName, dbfName, prjName;
			shpName = tmp_shpfileName;
			shxName = tmp_shpfileName.Left(tmp_shpfileName.GetLength() - 3) + "shx";
			dbfName = tmp_shpfileName.Left(tmp_shpfileName.GetLength() - 3) + "dbf";
			prjName = tmp_shpfileName.Left(tmp_shpfileName.GetLength() - 3) + "prj";
			
			// new file is created, so there must not be any files with this names
			if( Utility::fileExists(shpName) != FALSE )
			{	
				ErrorMessage(tkSHP_FILE_EXISTS);
				return S_OK;
			}
			if( Utility::fileExists(shxName) != FALSE )
			{	
				ErrorMessage(tkSHX_FILE_EXISTS);
				return S_OK;
			}
			if( Utility::fileExists(dbfName) != FALSE )
			{	
				ErrorMessage(tkDBF_FILE_EXISTS);
				return S_OK;
			}
			if( Utility::fileExists(prjName) != FALSE )
			{	
				ErrorMessage(tkPRJ_FILE_EXISTS);	// lsu: probably it's ok to overwite it blindly ?
				return S_OK;
			}

			// closing the old shapefile (error code inside the function)
			this->Close(&vbretval);
			
			if( vbretval == VARIANT_TRUE )
			{	
				CoCreateInstance(CLSID_Table,NULL,CLSCTX_INPROC_SERVER,IID_ITable,(void**)&dbf);
				
				if (!dbf)
				{
					ErrorMessage(tkCANT_COCREATE_COM_INSTANCE);
				}
				else
				{
					dbf->put_GlobalCallback(globalCallback);
					
					CString newDbfName = tmp_shpfileName.Left(tmp_shpfileName.GetLength() - 3) + "dbf";
					dbf->CreateNew(A2BSTR(newDbfName), &vbretval);
					
					if (!vbretval)
					{
						dbf->get_LastErrorCode(&lastErrorCode);
						ErrorMessage(lastErrorCode);
						dbf->Release();
						dbf = NULL;
					}
					else
					{
						_shpfileName = tmp_shpfileName;
						_shxfileName = tmp_shpfileName.Left(tmp_shpfileName.GetLength() - 3) + "shx";
						_dbffileName = tmp_shpfileName.Left(tmp_shpfileName.GetLength() - 3) + "dbf";
						_prjfileName = tmp_shpfileName.Left(tmp_shpfileName.GetLength() - 3) + "prj";
						
						_shpfiletype = ShapefileType;
						_isEditingShapes = true;
						m_sourceType = sstInMemory;
						this->ApplyRandomDrawingOptions();

						*retval = VARIANT_TRUE;
					}
				}
			}
		}
	}
	UpdateLabelsPositioning();
	return S_OK;
}

// *********************************************************
//		CreateNewWithShapeID()
// *********************************************************
STDMETHODIMP CShapefile::CreateNewWithShapeID(BSTR ShapefileName, ShpfileType ShapefileType, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	CreateNew(ShapefileName, ShapefileType, retval);
	
	if (*retval == VARIANT_TRUE)
	{
		IField * shapeIDField = NULL;
		CoCreateInstance(CLSID_Field,NULL,CLSCTX_INPROC_SERVER,IID_IField,(void**)&shapeIDField);
		
		shapeIDField->put_Name(A2BSTR("MWShapeID"));
		shapeIDField->put_Type(INTEGER_FIELD);
		shapeIDField->put_Width(10);
		shapeIDField->put_Precision(10);

		long fldIndex = 0;
		VARIANT_BOOL retVal;
		EditInsertField(shapeIDField, &fldIndex, NULL, &retVal);

		shapeIDField->Release();
	}
	return S_OK;
}
#pragma endregion

#pragma region SaveAndClose
// *****************************************************************
//		Close()
// *****************************************************************
STDMETHODIMP CShapefile::Close(VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if( _isEditingShapes )
	{
		// just stop editing shapes, if the shape is in open status
		StopEditingShapes(VARIANT_FALSE, VARIANT_TRUE, NULL, retval);
	}
	
	// stop editing table in case only it have been edited
	VARIANT_BOOL isEditingTable = VARIANT_FALSE;
	if( dbf != NULL )
	{
		dbf->get_EditingTable(&isEditingTable);
		if (isEditingTable)
		{
			StopEditingTable(VARIANT_FALSE,globalCallback,retval);
			dbf->get_EditingTable(&isEditingTable);
		}
	}
	
	m_sourceType = sstUninitialized;
	_shpfiletype = SHP_NULLSHAPE;
	_shpfileName = "";
	_shxfileName = "";
	_dbffileName = "";
	
	//_numShapes = 0;
	
	_minX = 0.0;
	_minY = 0.0;
	_minZ = 0.0;
	_maxX = 0.0;
	_maxY = 0.0;
	_maxZ = 0.0;
	_minM = 0.0;
	_maxM = 0.0;
	
	// removing shape data
	for (unsigned int i = 0; i < _shapeData.size(); i++)
	{
		delete _shapeData[i];
	}
	_shapeData.clear();
	
	if( _shpfile != NULL) fclose(_shpfile);
	_shpfile = NULL;
	
	if( _shxfile != NULL) fclose(_shxfile);
	_shxfile = NULL;
	
	if( dbf != NULL )
	{
		dbf->Release();
		dbf = NULL;
	}

	*retval = !_isEditingShapes && !isEditingTable?VARIANT_TRUE:VARIANT_FALSE;
	UpdateLabelsPositioning();
	return S_OK;
}

// **********************************************************
//		SaveAs()
// **********************************************************
// Saves shapefile to the new or the same location. Doesn't change the editing state (source type) 
STDMETHODIMP CShapefile::SaveAs(BSTR ShapefileName, ICallback *cBack, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retval = VARIANT_FALSE;

	bool callbackIsNull = (globalCallback == NULL);
	if(globalCallback == NULL && cBack != NULL)
	{
		globalCallback = cBack;
		globalCallback->AddRef();
	}

	if (dbf == NULL || m_sourceType == sstUninitialized)
	{
		ErrorMessage(tkSHAPEFILE_UNINITIALIZED);
		return S_FALSE;
	}

	// in case someone else is writing, we leave
	if (m_writing)
	{
		ErrorMessage(tkSHP_WRITE_VIOLATION);
		return S_FALSE;
	}
	
	USES_CONVERSION;
	CString sa_shpfileName;
	sa_shpfileName = OLE2CA(ShapefileName);

	if( sa_shpfileName.GetLength() <= 3 )
	{	
		ErrorMessage(tkINVALID_FILENAME);
	}
	else
	{
		CString sa_shxfileName = sa_shpfileName.Left(sa_shpfileName.GetLength() - 3) + "shx";
		CString sa_dbffileName = sa_shpfileName.Left(sa_shpfileName.GetLength() - 3) + "dbf";
	
		// -----------------------------------------------
		// it's not allowed to rewrite the existing files
		// -----------------------------------------------
		if( Utility::fileExists(sa_shpfileName) )
		{	
			ErrorMessage(tkSHP_FILE_EXISTS);
			return S_OK;
		}
		if( Utility::fileExists(sa_shxfileName) )
		{	
			ErrorMessage(tkSHX_FILE_EXISTS);
			return S_OK;
		}
		if( Utility::fileExists(sa_dbffileName) )
		{	
			ErrorMessage(tkDBF_FILE_EXISTS);
			return S_OK;
		}

		// -----------------------------------------------
		//   checking in-memory shapes
		// -----------------------------------------------
		if( _isEditingShapes )
		{	
			if( verifyMemShapes(cBack) == FALSE )
			{	
				// error Code is set in function
				return S_OK;
			}

			// refresh extents
			VARIANT_BOOL retVal;
			this->RefreshExtents(&retVal);
		}		
		
		// -----------------------------------------------
		// opening files
		// -----------------------------------------------
		FILE * sa_shpfile = fopen( sa_shpfileName, "wb+" );		
		if( sa_shpfile == NULL )
		{	
			ErrorMessage(tkCANT_CREATE_SHP);
			return S_OK;
		}
		
		//shx
		FILE * sa_shxfile = fopen( sa_shxfileName, "wb+" );
		if( sa_shxfile == NULL )
		{	
			fclose( sa_shpfile );
			sa_shpfile = NULL;
			ErrorMessage(tkCANT_CREATE_SHX);
			return S_OK;
		}

		// ------------------------------------------------
		//	writing the files
		// ------------------------------------------------
		writeShp(sa_shpfile,cBack);
		writeShx(sa_shxfile,cBack);

		fclose(sa_shpfile);
		fclose(sa_shxfile);

		sa_shpfile = fopen( sa_shpfileName, "rb" );		
		sa_shxfile = fopen( sa_shxfileName, "rb" );		

		// ------------------------------------------------
		//	saving dbf table
		// ------------------------------------------------
		dbf->SaveAs(sa_dbffileName.AllocSysString(), cBack, retval);
		if( *retval == FALSE )
		{	
			dbf->get_LastErrorCode(&lastErrorCode);
			fclose(sa_shpfile);
			fclose(sa_shxfile);
			_unlink(sa_shpfileName);
			_unlink(sa_shxfileName);
			return S_OK;
		}
		
		// -------------------------------------------------
		//	switching to the new files
		// -------------------------------------------------
		if( _shpfile != NULL )	fclose(_shpfile);
		_shpfile = sa_shpfile;
		
		if( _shxfile != NULL )	fclose(_shxfile);
		_shxfile = sa_shxfile;
	
		// update all filenames:
		_shpfileName = sa_shpfileName;	// lsu: saving of shp filename should be done before writing the projection;
		_shxfileName = sa_shxfileName;	// otherwise projection string will be written to the memory
		_dbffileName = sa_dbffileName;
		_prjfileName = sa_shpfileName.Left(sa_shpfileName.GetLength() - 3) + "prj";

		// projection will be written to the disk after this
		m_sourceType = sstDiskBased;	

		// saving projection in new format
		VARIANT_BOOL vbretval;
		m_geoProjection->WriteToFile(A2BSTR(_prjfileName), &vbretval);

		if (useQTree)
			GenerateQTree();

		*retval = VARIANT_TRUE;
	}
	
	// restoring callback
	if (callbackIsNull)
		globalCallback = NULL;
	return S_OK;
}

// **************************************************************
//		Save()
// **************************************************************
//Neio, 2009/07/23, add "Save" Method for saving without exiting edit mode
STDMETHODIMP CShapefile::Save(ICallback *cBack, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retval = VARIANT_FALSE;

	if (globalCallback == NULL && cBack != NULL)
	{
		globalCallback = cBack;
		globalCallback->AddRef();
	}
	
	// no edits were made; it doesn't make sense to save it
	if( _isEditingShapes == FALSE )
	{	
		ErrorMessage( tkSHPFILE_NOT_IN_EDIT_MODE );
		return S_OK;
	}

	if( verifyMemShapes(globalCallback) == FALSE )
	{	
		// error Code is set in function
		return S_OK;
	}
	
	// compute the extents
	VARIANT_BOOL res;
	RefreshExtents(&res);

	// -------------------------------------------------
	//	Reopen the files in the write mode
	// -------------------------------------------------
	if (_shpfile && _shxfile)	// TODO: check the condition
	{
		_shpfile = freopen(_shpfileName,"wb+",_shpfile);
		_shxfile = freopen(_shxfileName,"wb+",_shxfile);
	}
	else
	{
		_shpfile = fopen(_shpfileName,"wb+");
		_shxfile = fopen(_shxfileName,"wb+");
	}

	if( _shpfile == NULL || _shxfile == NULL )
	{	
		if( _shxfile != NULL )
		{	
			fclose( _shxfile );
			_shxfile = NULL;
			lastErrorCode = tkCANT_OPEN_SHX;
		}
		if( _shpfile != NULL )
		{	
			fclose( _shpfile );
			_shpfile = NULL;
			lastErrorCode = tkCANT_OPEN_SHP;
		}
		*retval = FALSE;
		
		ErrorMessage(lastErrorCode);
		
	}
	else
	{
		m_writing = true;

		// -------------------------------------------------
		//	Writing the files
		// -------------------------------------------------
		writeShp(_shpfile,cBack);
		writeShx(_shxfile,cBack);
		
		if (useQTree)
			GenerateQTree();

		// -------------------------------------------------
		//	Reopen the updated files
		// -------------------------------------------------
		_shpfile = freopen(_shpfileName,"rb+",_shpfile);
		_shxfile = freopen(_shxfileName,"rb+",_shxfile);
		
		if( _shpfile == NULL || _shxfile == NULL )
		{	
			if( _shxfile != NULL )
			{	
				fclose( _shxfile );
				_shxfile = NULL;
				lastErrorCode = tkCANT_OPEN_SHX;
			}
			if( _shpfile != NULL )
			{	
				fclose( _shpfile );
				_shpfile = NULL;
				lastErrorCode = tkCANT_OPEN_SHP;
			}
			*retval = FALSE;
			
			ErrorMessage( lastErrorCode );
		}
		else
		{
			//Save the table file
			dbf->Save(cBack,retval);
			
			// projection will be written to the disk after this
			m_sourceType = sstDiskBased;	

			// saving projection in new format
			VARIANT_BOOL vbretval;
			m_geoProjection->WriteToFile(A2BSTR(_prjfileName), &vbretval);

			*retval = VARIANT_TRUE;
		}
	}
	m_writing = false;
	return S_OK;
}

// ************************************************************
//		Resource()
// ************************************************************
STDMETHODIMP CShapefile::Resource(BSTR newShpPath, VARIANT_BOOL *retval)
{
	USES_CONVERSION;
	Close(retval);
	Open(newShpPath, NULL, retval);
	return S_OK;
}
#pragma endregion

#pragma region Clone
// ***********************************************************************
//		Clone()
// ***********************************************************************
//  Creates new shapefile with the same type and fields as existing one
 STDMETHODIMP CShapefile::Clone(IShapefile** retVal)
{
	IShapefile* sf = NULL;
	CoCreateInstance(CLSID_Shapefile,NULL,CLSCTX_INPROC_SERVER,IID_IShapefile,(void**)&sf);
	
	ShpfileType shpType;
	VARIANT_BOOL vbretval;
	this->get_ShapefileType(&shpType);
	sf->CreateNew(A2BSTR(""), shpType, &vbretval);
	
	// copying the projection string
	BSTR pVal;
	this->get_Projection(&pVal);
	if (pVal != NULL)
		sf->put_Projection(pVal);

	long numFields;
	this->get_NumFields(&numFields);

	for (long i = 0; i < numFields; i++)
	{
		// extracting field properties			
		IField * fld = NULL;
		BSTR name;
		long precision, width;
		FieldType type;
		
		this->get_Field(i,&fld);
		fld->get_Name(&name);
		fld->get_Precision(&precision);
		fld->get_Type(&type);
		fld->get_Width(&width);
		fld->Release();
		
		// creating new field and passing values			
		IField * fldNew = NULL;
		CoCreateInstance(CLSID_Field,NULL,CLSCTX_INPROC_SERVER,IID_IField,(void**)&fldNew);
		fldNew->put_Name(name); SysFreeString(name);
		fldNew->put_Precision(precision);
		fldNew->put_Type(type);
		fldNew->put_Width(width);
		sf->EditInsertField(fldNew, &i, NULL, &vbretval);

		if (!vbretval)
		{
			sf->Release(); sf = NULL; break;
		}
	}
	*retVal = sf;
	return S_OK;
}
#pragma endregion

#pragma region Extents
// ************************************************************
//		get_Extents()
// ************************************************************
STDMETHODIMP CShapefile::get_Extents(IExtents **pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	IExtents * bBox = NULL;
	CoCreateInstance( CLSID_Extents, NULL, CLSCTX_INPROC_SERVER, IID_IExtents, (void**)&bBox );
	
	// Extents could change because of the moving of points of a single shape
	// It's difficult to track such changes, so we still need to recalculate them 
	// here to enforce proper drawing; _fastMode mode - for those who want 
	// to call refresh extents theirselfs
	if (!_fastMode)
	{
		VARIANT_BOOL vbretval;
		this->RefreshExtents(&vbretval);
	}

	bBox->SetBounds(_minX,_minY,_minZ,_maxX,_maxY,_maxZ);
	bBox->SetMeasureBounds(_minM,_maxM);
	*pVal = bBox;

	return S_OK;
}
#pragma endregion

#pragma region AttributeTable
// ****************************************************************
//	  EditInsertField()
// ****************************************************************
STDMETHODIMP CShapefile::EditInsertField(IField *NewField, long *FieldIndex, ICallback *cBack, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if(cBack == NULL && globalCallback!=NULL)
		cBack = globalCallback;	

	if( dbf != NULL )
	{
		dbf->EditInsertField(NewField,FieldIndex,cBack,retval);
	}
	else
	{	
		*retval = VARIANT_FALSE;
		lastErrorCode = tkFILE_NOT_OPEN;
		if( cBack != NULL )
			cBack->Error( OLE2BSTR(key),  A2BSTR(ErrorMsg(lastErrorCode) ) );
		return S_OK;
	}

	if( *retval == VARIANT_FALSE )
	{	
		dbf->get_LastErrorCode(&lastErrorCode);
		*retval = VARIANT_FALSE;
	}

	return S_OK;
}

// ****************************************************************
//	  EditDeleteField()
// ****************************************************************
STDMETHODIMP CShapefile::EditDeleteField(long FieldIndex, ICallback *cBack, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if(globalCallback == NULL && cBack != NULL)
	{
		globalCallback = cBack;	
		globalCallback->AddRef();
	}

	if( dbf != NULL )
	{
		dbf->EditDeleteField(FieldIndex,cBack,retval);
	}
	else
	{	
		*retval = VARIANT_FALSE;
		ErrorMessage(tkFILE_NOT_OPEN);
		return S_OK;
	}

	if( *retval == VARIANT_FALSE )
	{	
		dbf->get_LastErrorCode(&lastErrorCode);
		*retval = VARIANT_FALSE;
	}

	return S_OK;
}

// ****************************************************************
//	  EditCellValue()
// ****************************************************************
STDMETHODIMP CShapefile::EditCellValue(long FieldIndex, long ShapeIndex, VARIANT NewVal, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if( dbf != NULL )
	{
		dbf->EditCellValue(FieldIndex,ShapeIndex,NewVal,retval);
	}
	else
	{	
		*retval = VARIANT_FALSE;
		ErrorMessage(tkFILE_NOT_OPEN);
		return S_OK;
	}

	if( *retval == VARIANT_FALSE )
	{	
		dbf->get_LastErrorCode(&lastErrorCode);
		*retval = VARIANT_FALSE;
	}

	return S_OK;
}

// ****************************************************************
//	  StartEditingTable()
// ****************************************************************
STDMETHODIMP CShapefile::StartEditingTable(ICallback *cBack, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	if(globalCallback == NULL && cBack != NULL)
	{
		globalCallback = cBack;
		globalCallback->AddRef();
	}

	if( dbf != NULL )
	{
		dbf->StartEditingTable(cBack,retval);
	}
	else
	{	
		*retval = VARIANT_FALSE;
		ErrorMessage(tkFILE_NOT_OPEN);
		return S_OK;
	}

	if( *retval == VARIANT_FALSE )
	{	
		dbf->get_LastErrorCode(&lastErrorCode);
		*retval = VARIANT_FALSE;
	}

	return S_OK;
}

// ****************************************************************
//	  EditCellValue()
// ****************************************************************
STDMETHODIMP CShapefile::StopEditingTable(VARIANT_BOOL ApplyChanges, ICallback *cBack, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	if(globalCallback == NULL && cBack!=NULL)
	{
		globalCallback = cBack;
		globalCallback->AddRef();
	}

	if( dbf != NULL )
	{
		dbf->StopEditingTable(ApplyChanges,cBack,retval);
	}
	else
	{	
		*retval = VARIANT_FALSE;
		ErrorMessage(tkFILE_NOT_OPEN);
		return S_OK;
	}

	if( *retval == FALSE )
	{	
		dbf->get_LastErrorCode(&lastErrorCode);
		*retval = VARIANT_FALSE;
	}

	return S_OK;
}

// *****************************************************************
//	   get_Field()
// *****************************************************************
STDMETHODIMP CShapefile::get_Field(long FieldIndex, IField **pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if( dbf != NULL )
	{
		dbf->get_Field(FieldIndex,pVal);	
		if(*pVal != NULL)
		{
			// we need to report error from field class, and will use callback from this class for it
			ICallback* cBack = NULL;
			if ((*pVal)->get_GlobalCallback(&cBack) == NULL && this->globalCallback != NULL)
				(*pVal)->put_GlobalCallback(globalCallback);
			
			if (cBack != NULL)
				cBack->Release();	// we put a reference in field class so must release it here
		}
	}
	else
	{	
		ErrorMessage(tkFILE_NOT_OPEN);
		return S_OK;
	}

	return S_OK;
}

// *****************************************************************
//	   get_FieldByName()
// *****************************************************************
STDMETHODIMP CShapefile::get_FieldByName(BSTR Fieldname, IField **pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	
    long max;
	BSTR Testname;
	CString strTestname;
	CString strFieldname;
	IField *testVal;
	
	dbf->get_NumFields(&max);
	if( dbf != NULL )
	{
        if( _tcslen( OLE2CA(Fieldname) ) > 0 )
		{
			strFieldname = OLE2A(Fieldname);
		}
		else
		{
			ErrorMessage(tkZERO_LENGTH_STRING);
		}

		for (int fld=0; fld < max; fld++)
		{
			dbf->get_Field(fld,&testVal);
			testVal->get_Name(&Testname);
			strTestname = OLE2A(Testname);
			if(strTestname == strFieldname)
			{
				*pVal = testVal;
				return S_OK;
			}
		}
	}
	else
	{	
		lastErrorCode = tkFILE_NOT_OPEN;
		if( globalCallback != NULL )
			globalCallback->Error( OLE2BSTR(key),  A2BSTR(ErrorMsg(lastErrorCode) ) );
		return S_OK;
	}
	
	// we did not have a file error, but we also didn't match the name
	pVal = NULL;
	return S_OK;	
}

// *****************************************************************
//	   FindNewShapeID()
// *****************************************************************
long CShapefile::FindNewShapeID(long FieldIndex)
{
    if (dbf == NULL)
	{
		return 0;
	}
	else
	{
		long rt = -1;
		long lo = 0;

		int size = _shapeData.size();
		for (int i = 0; i < size - 1; i++)
		{
			VARIANT pVal;
			VariantInit(&pVal);
			dbf->get_CellValue(FieldIndex, i, &pVal);
			lVal(pVal, lo);
			VariantClear(&pVal);
			if (lo > rt) rt = lo;
		}
		return rt + 1;
	}
}

// *****************************************************************
//	   get_CellValue()
// *****************************************************************
STDMETHODIMP CShapefile::get_CellValue(long FieldIndex, long ShapeIndex, VARIANT *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if( dbf != NULL )
	{
		dbf->get_CellValue(FieldIndex,ShapeIndex,pVal);
	}
	else
	{	
		ErrorMessage(tkFILE_NOT_OPEN);
		return S_OK;
	}

	return S_OK;
}

// *****************************************************************
//	   get_EditingTable()
// *****************************************************************
STDMETHODIMP CShapefile::get_EditingTable(VARIANT_BOOL *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	if( dbf != NULL )
	{
		dbf->get_EditingTable(pVal);
	}
	else
	{	
		*pVal = VARIANT_FALSE;
		ErrorMessage(tkFILE_NOT_OPEN);
		return S_OK;
	}

	return S_OK;
}

// *************************************************************
//		get_Table()
// *************************************************************
STDMETHODIMP CShapefile::get_Table(ITable** retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retVal = dbf;
	if ( dbf )
	{
		dbf->AddRef();
	}
	return S_OK;
}

//*********************************************************************
//*						UniqueFieldNames()				              
//*********************************************************************
// Makes name of fields in dbf table unique.	 
// In case of duplicated names adds _# to them	 
bool CShapefile::UniqueFieldNames(IShapefile* sf)
{
	VARIANT_BOOL editing;
	USES_CONVERSION;

	// Do we need edit mode for editing of the field names?
	// Yes we do, shapelib doesn't allow it otherwise ;)
	sf->get_EditingShapes(&editing);	
	if (!editing) 
		return false;
	
	long numFields;
	sf->get_NumFields(&numFields);
	
	set<CString> fields;

	for(long i = 0; i< numFields; i++)
	{
		BSTR name;
		IField* fld;
		sf->get_Field(i, &fld);
		fld->get_Name(&name);

		if (fields.find(OLE2CA(name)) == fields.end())
		{
			fields.insert(OLE2CA(name));
		}
		else
		{	
			bool found = false;
			for(int j =0; !found ;j++)
			{
				CString temp = OLE2CA(name);
				temp.AppendFormat("_%d", j);
				if (fields.find(temp) == fields.end())
				{	
					fields.insert(temp);
					name = temp.AllocSysString();
					fld->put_Name(name);
					found = true;
				}
			}
		}
		fld->Release();
	}
	fields.clear();
	return true;
}
#pragma endregion

#pragma region DrawingOptions
// *************************************************************
//		get_ShapeCategory()
// *************************************************************
STDMETHODIMP CShapefile::get_ShapeCategory(long ShapeIndex, long* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	if( ShapeIndex < 0 || ShapeIndex >= (long)_shapeData.size()) //_numShapes)
	{	
		*pVal = -1;
		ErrorMessage(tkINDEX_OUT_OF_BOUNDS);
	}
	else
		*pVal = _shapeData[ShapeIndex]->category; 
	return S_OK;
}
STDMETHODIMP CShapefile::put_ShapeCategory(long ShapeIndex, long newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	if( ShapeIndex < 0 || ShapeIndex >= (long)_shapeData.size()) //_numShapes )
	{	
		ErrorMessage(tkINDEX_OUT_OF_BOUNDS);
	}
	else
		_shapeData[ShapeIndex]->category = (int)newVal;

	return S_OK;
}

// *******************************************************************
//  	SelectionDrawingOptions()
// *******************************************************************
//  Returns and sets parameters used to draw selection for the shapefile.
STDMETHODIMP CShapefile::get_SelectionDrawingOptions(IShapeDrawingOptions** pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = m_selectDrawOpt;
	if (m_selectDrawOpt)
		m_selectDrawOpt->AddRef();
	return S_OK;
}
STDMETHODIMP CShapefile::put_SelectionDrawingOptions(IShapeDrawingOptions* newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	if (!newVal)
	{
		ErrorMessage(tkINVALID_PARAMETER_VALUE); 
	}
	else
	{
		put_ComReference(newVal, (IDispatch**)&m_selectDrawOpt, false);
	}
	return S_OK;
}

// *******************************************************************
//		DeafultDrawingOptions()				          
// *******************************************************************
// Returns and sets parameters used to draw shapefile by default.
STDMETHODIMP CShapefile::get_DefaultDrawingOptions(IShapeDrawingOptions** pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*pVal = m_defaultDrawOpt;
	if (m_defaultDrawOpt)
		m_defaultDrawOpt->AddRef();
	return S_OK;
}
STDMETHODIMP CShapefile::put_DefaultDrawingOptions(IShapeDrawingOptions* newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	
	if (!newVal)
	{
		ErrorMessage(tkINVALID_PARAMETER_VALUE); 
	}
	else
	{
		put_ComReference(newVal, (IDispatch**)&m_defaultDrawOpt);
	}
	return S_OK;
}

/***********************************************************************/
/*		put_ReferenceToCategories
/***********************************************************************/
void CShapefile::put_ReferenceToCategories(bool bNullReference)
{
	if (m_categories == NULL) return;
	CShapefileCategories* coCategories = static_cast<CShapefileCategories*>(m_categories);
	//CShapefile* coShapefile = static_cast<CShapefile*>(this);
	if (!bNullReference)
		coCategories->put_ParentShapefile(this);
	else
		coCategories->put_ParentShapefile(NULL);
};

/***********************************************************************/
/*		get/put_Categories
/***********************************************************************/
STDMETHODIMP CShapefile::get_Categories(IShapefileCategories** pVal)
{
	*pVal = m_categories;
	if (m_categories != NULL)
		m_categories->AddRef();
	return S_OK;
}
STDMETHODIMP CShapefile::put_Categories(IShapefileCategories* newVal)
{
	if (put_ComReference((IDispatch*)newVal, (IDispatch**) &m_categories, false))	
	{
		((CShapefileCategories*)m_categories)->put_ParentShapefile(this);
	}
	return S_OK;
}
#pragma endregion

// ********************************************************************
//		get_SelectionColor
// ********************************************************************
STDMETHODIMP CShapefile::get_SelectionColor(OLE_COLOR* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retval = _selectionColor;
	return S_OK;
}
STDMETHODIMP CShapefile::put_SelectionColor(OLE_COLOR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	_selectionColor = newVal;
	return S_OK;
}

// ********************************************************************
//		get_SelectionTransparency
// ********************************************************************
STDMETHODIMP CShapefile::get_SelectionTransparency (BYTE* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retval = _selectionTransparency;
	return S_OK;
}
STDMETHODIMP CShapefile::put_SelectionTransparency (BYTE newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	if (newVal > 255) newVal = 255;
	if (newVal < 0) newVal = 0;
	_selectionTransparency = newVal;
	return S_OK;
}

// ********************************************************************
//		get_SelectionAppearance
// ********************************************************************
STDMETHODIMP CShapefile::get_SelectionAppearance(tkSelectionAppearance* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retval = _selectionAppearance;
	return S_OK;
}
STDMETHODIMP CShapefile::put_SelectionAppearance(tkSelectionAppearance newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	_selectionAppearance = newVal;
	return S_OK;
}

// ********************************************************************
//		get_PointCollisionMode
// ********************************************************************
STDMETHODIMP CShapefile::get_CollisionMode(tkCollisionMode* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retval = _collisionMode;
	return S_OK;
}
STDMETHODIMP CShapefile::put_CollisionMode(tkCollisionMode newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	_collisionMode = newVal;
	return S_OK;
}

#pragma region "Seialization"
// ********************************************************
//     Serialize()
// ********************************************************
STDMETHODIMP CShapefile::Serialize(VARIANT_BOOL SaveSelection, BSTR* retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	CPLXMLNode* psTree = this->SerializeCore(VARIANT_TRUE, "ShapefileClass");
	if (!psTree)
	{
		*retVal = A2BSTR("");
	}
	else
	{
		CString str = CPLSerializeXMLTree(psTree);	
		*retVal = A2BSTR(str);
	}
	return S_OK;
}

// ********************************************************
//     SerializeCore()
// ********************************************************
 CPLXMLNode* CShapefile::SerializeCore(VARIANT_BOOL SaveSelection, CString ElementName)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	CPLXMLNode* psTree = CPLCreateXMLNode( NULL, CXT_Element, ElementName );
	
	if (psTree)
	{
		CString s =  OLE2CA(_expression);
		if (s != "")
			CPLCreateXMLAttributeAndValue(psTree, "VisibilityExpression", s);

		if (_fastMode != FALSE)
			CPLCreateXMLAttributeAndValue(psTree, "FastMode", CPLString().Printf("%d", (int)_fastMode));

		if (useQTree != FALSE)
			CPLCreateXMLAttributeAndValue(psTree, "UseQTree", CPLString().Printf("%d", (int)useQTree));

		if (_collisionMode != LocalList )
			CPLCreateXMLAttributeAndValue(psTree, "CollisionMode", CPLString().Printf("%d", (int)_collisionMode));

		if (_selectionAppearance != saSelectionColor)
			CPLCreateXMLAttributeAndValue(psTree, "SelectionAppearance", CPLString().Printf("%d", (int)_selectionAppearance));

		if (_selectionColor != RGB(255, 255, 0))
			CPLCreateXMLAttributeAndValue(psTree, "SelectionColor", CPLString().Printf("%d", (int)_selectionColor));

		if (_selectionTransparency != 180)
			CPLCreateXMLAttributeAndValue(psTree, "SelectionTransparency", CPLString().Printf("%d", (int)_selectionTransparency));

		if (_minDrawingSize != 1)
		CPLCreateXMLAttributeAndValue(psTree, "MinDrawingSize", CPLString().Printf("%d", _minDrawingSize));

		// drawing options
		CPLXMLNode* node = ((CShapeDrawingOptions*)m_defaultDrawOpt)->SerializeCore("DefaultDrawingOptions");
		if (node)
		{
			CPLAddXMLChild(psTree, node);
		}
		
		if(_selectionAppearance == saDrawingOptions)
		{
			CPLXMLNode* node = ((CShapeDrawingOptions*)m_selectDrawOpt)->SerializeCore("SelectionDrawingOptions");
			if (node)
			{
				CPLAddXMLChild(psTree, node);
			}
		}

		// categories
		node = ((CShapefileCategories*)m_categories)->SerializeCore("ShapefileCategoriesClass");
		if (node)
		{
			CPLAddXMLChild(psTree, node);
		}

		// labels
		CPLXMLNode* psLabels = ((CLabels*)m_labels)->SerializeCore("LabelsClass");
		if (psLabels)
		{
			CPLAddXMLChild(psTree, psLabels);
		}

		// charts
		CPLXMLNode* psCharts = ((CCharts*)m_charts)->SerializeCore("ChartsClass");
		if (psCharts)
		{
			CPLAddXMLChild(psTree, psCharts);
		}

		// selection
		long numSelected;
		this->get_NumSelected(&numSelected);
		
		if (numSelected > 0 && SaveSelection)
		{
			char* selection = new char[_shapeData.size() + 1];
			selection[_shapeData.size()] = '\0';
			for (unsigned int i = 0; i < _shapeData.size(); i++)
			{
				selection[i] = _shapeData[i]->selected ? '1' : '0';
			}
		
			CPLXMLNode* nodeSelection = CPLCreateXMLElementAndValue(psTree, "Selection", selection);
			if (nodeSelection)
			{
				CPLCreateXMLAttributeAndValue(nodeSelection, "TotalCount", CPLString().Printf("%d", _shapeData.size()));
				CPLCreateXMLAttributeAndValue(nodeSelection, "SelectedCount", CPLString().Printf("%d", numSelected));
			}
			delete[] selection;
		}
	}
	return psTree;
}

// ********************************************************
//     Deserialize()
// ********************************************************
STDMETHODIMP CShapefile::Deserialize(VARIANT_BOOL LoadSelection, BSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	CString s = OLE2CA(newVal);
	CPLXMLNode* node = CPLParseXMLString(s.GetString());
	if (node)
	{
		node = CPLGetXMLNode(node, "=ShapefileClass");
		if (node)
		{
			this->DeserializeCore(VARIANT_TRUE, node);
		}
	}
	return S_OK;
}

// ********************************************************
//     DeserializeCore()
// ********************************************************
bool CShapefile::DeserializeCore(VARIANT_BOOL LoadSelection, CPLXMLNode* node)
{
	USES_CONVERSION;

	if (!node )
		return false;

	CString s;
	s = CPLGetXMLValue( node, "VisibilityExpression", NULL );
	_expression = A2BSTR(s);
	
	s = CPLGetXMLValue( node, "FastMode", NULL );
	bool fastMode = (s != "") ? atoi(s.GetString()) == 0 ? false : true : false;
	this->put_FastMode(fastMode);

	s = CPLGetXMLValue( node, "UseQTree", NULL );
	useQTree = (s != "") ? (BOOL)atoi(s.GetString()) : FALSE;

	s = CPLGetXMLValue( node, "CollisionMode", NULL );
	_collisionMode = (s != "") ? (tkCollisionMode)atoi(s.GetString()) : LocalList;

	s = CPLGetXMLValue( node, "SelectionAppearance", NULL );
	_selectionAppearance = (s != "") ? (tkSelectionAppearance)atoi(s.GetString()): saSelectionColor;

	s = CPLGetXMLValue( node, "SelectionColor", NULL );
	_selectionColor = (s != "") ? (OLE_COLOR)atoi(s.GetString()) : RGB(255, 255, 0);

	s = CPLGetXMLValue( node, "SelectionTransparency", NULL );
	_selectionTransparency = (s != "") ? (unsigned char)atoi(s.GetString()) : 180;

	s = CPLGetXMLValue( node, "MinDrawingSize", NULL );
	_minDrawingSize = (s != "") ? atoi(s.GetString()) : 1;

	// drawing options
	CPLXMLNode* psChild = CPLGetXMLNode(node, "DefaultDrawingOptions");
	if (psChild)
	{
		IShapeDrawingOptions* options = NULL;
		this->get_DefaultDrawingOptions(&options);
		((CShapeDrawingOptions*)options)->DeserializeCore(psChild);
		options->Release();
	}

	if (_selectionAppearance == saDrawingOptions)
	{
		CPLXMLNode* psChild = CPLGetXMLNode(node, "SelectionDrawingOptions");
		if (psChild)
		{
			IShapeDrawingOptions* options = NULL;
			this->get_SelectionDrawingOptions(&options);
			((CShapeDrawingOptions*)options)->DeserializeCore(psChild);
			options->Release();
		}
	}
	
	// Categories
	psChild = CPLGetXMLNode(node, "ShapefileCategoriesClass");
	if (psChild)
	{
		((CShapefileCategories*)m_categories)->DeserializeCore(psChild);
	}

	// Labels
	psChild = CPLGetXMLNode(node, "LabelsClass");
	if (psChild)
	{
		((CLabels*)m_labels)->DeserializeCore(psChild);
	}

	// Charts
	psChild = CPLGetXMLNode(node, "ChartsClass");
	if (psChild)
	{
		((CCharts*)m_charts)->DeserializeCore(psChild);
	}
	
	// selection
	CPLXMLNode* nodeSelection = CPLGetXMLNode(node, "Selection");
	if (nodeSelection && LoadSelection)
	{
		this->SelectNone();

		CString s = CPLGetXMLValue(nodeSelection, "TotalCount", "0");
		long count = atoi(s);
		s = CPLGetXMLValue(nodeSelection, "=Selection", "");
		if (s.GetLength() == count && s.GetLength() == _shapeData.size())
		{
			char* selection = s.GetBuffer();
			for (unsigned int i = 0; i < _shapeData.size(); i++)
			{
				if (selection[i] == '1')
				{
					_shapeData[i]->selected =  true;
				}
			}
		}
	}
	return true;
}
#pragma endregion

// ******************************************************* 
//		UpdateLabelsPositioning()
// ******************************************************* 
// Should be called after change of shapefile type (CreateNew, Open, Resource, Close)
void CShapefile::UpdateLabelsPositioning()
{
	ShpfileType type = Utility::ShapeTypeConvert2D(_shpfiletype);
	
	if (m_labels)
	{
		if (type == SHP_POINT || type == SHP_MULTIPOINT)
		{
			m_labels->put_Positioning(lpCenter);
		}
		else if (type == SHP_POLYLINE)
		{
			m_labels->put_Positioning(lpLongestSegement);
		}
		else if (type == SHP_POLYGON)
		{
			m_labels->put_Positioning(lpCentroid);
		}
		else
		{
			m_labels->put_Positioning(lpNone);
		}
	}
}

#pragma region Projection
// *****************************************************************
//		get_Projection()
// *****************************************************************
// Chris Michaelis Sept 19 2005
STDMETHODIMP CShapefile::get_Projection(BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	m_geoProjection->ExportToProj4(pVal);
	return S_OK;

	//if ( m_sourceType == sstDiskBased ) //&& _shpfileName != "" )
	//{
	//	try
	//	{
	//		char * prj4 = NULL;
	//		ProjectionTools * p = new ProjectionTools();
	//		p->GetProj4FromPRJFile(_prjfileName.GetBuffer(), &prj4);
	//		delete p;

	//		if (prj4 != NULL)
	//		{
	//			*pVal = A2BSTR(prj4);
	//			CPLFree(prj4);
	//		}
	//		else
	//		{
	//			*pVal = A2BSTR("");
	//		}
	//	}
	//	catch(...)
	//	{
	//	}
	//}
	//else
	//{
	//	// in-memory shapefile
	//	*pVal = A2BSTR(m_projection);
	//}
}

// *****************************************************************
//		get_OGRSpatialReference()
// *****************************************************************
OGRSpatialReference* CShapefile::get_OGRSpatialReference()
{
	USES_CONVERSION;
	BSTR proj;
	this->get_Projection(&proj);
	CString str = OLE2CA(proj);
	SysFreeString(proj);

	OGRSpatialReference* reference = new OGRSpatialReference;
	
	if (str != "")
	{
		reference->importFromProj4(str.GetString());
		return reference;
	}
	else
	{
		return NULL;
	}
}

// *****************************************************************
//		put_Projection()
// *****************************************************************
STDMETHODIMP CShapefile::put_Projection(BSTR proj4Projection)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	
	VARIANT_BOOL vbretval;
	m_geoProjection->ImportFromProj4(proj4Projection, &vbretval);
	if (vbretval)
	{
		m_geoProjection->WriteToFile(A2BSTR(_prjfileName), &vbretval);
	}
	return S_OK;

	//if( m_sourceType == sstDiskBased ) //&& _shpfileName != "")
	//{
	//	CString s = W2A(proj4Projection);
	//	if (s == "")
	//		return S_FALSE;		// we won't write empty file
	//	
	//	try
	//	{
	//		if (_prjfileName.CompareNoCase("") == 0)
	//			return S_FALSE;

	//		FILE * prjFile;
	//		char * buffer = NULL;
	//		prjFile = fopen(_prjfileName, "wb");
	//		if (prjFile)
	//		{
	//			char * wkt = NULL;
	//			ProjectionTools * p = new ProjectionTools();
	//			p->ToESRIWKTFromProj4(&wkt, W2A(proj4Projection));

	//			if (wkt != NULL)
	//			{		// copy into the buffer
	//				fprintf(prjFile, "%s", wkt);
	//			}

	//			fclose(prjFile);
	//			prjFile = NULL;
	//			delete p;
	//			p = NULL;
	//		}
	//		return S_OK;
	//	}
	//	catch(...)
	//	{
	//		return S_FALSE;
	//	}
	//}
	//else
	//{
	//	// in-memory shapefile
	//	m_projection = W2A(proj4Projection);
	//	return S_OK;
	//}
}

// *****************************************************************
//		get_GeoProjection()
// *****************************************************************
STDMETHODIMP CShapefile::get_GeoProjection(IGeoProjection** retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	if (m_geoProjection)
		m_geoProjection->AddRef();

	*retVal = m_geoProjection;
	return S_OK;
}

// *****************************************************************
//		put_GeoProjection()
// *****************************************************************
STDMETHODIMP CShapefile::put_GeoProjection(IGeoProjection* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	put_ComReference((IDispatch*)pVal, (IDispatch**)&m_geoProjection, false);
	if (_prjfileName.GetLength() != 0)
	{
		VARIANT_BOOL vbretval;
		m_geoProjection->WriteToFile(A2BSTR(_prjfileName), &vbretval);
	}
	return S_OK;
}

// *****************************************************************
//		Reproject()
// *****************************************************************
STDMETHODIMP CShapefile::Reproject(IGeoProjection* newProjection, LONG* reprojectedCount, IShapefile** retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	
	if (!this->ReprojectCore(newProjection, reprojectedCount, retVal, false))
	{
		*retVal = NULL;
		return S_FALSE;
	}
	else
	{
		return S_OK;
	}
}

// *****************************************************************
//		ReprojectInPlace()
// *****************************************************************
STDMETHODIMP CShapefile::ReprojectInPlace(IGeoProjection* newProjection, LONG* reprojectedCount, VARIANT_BOOL* retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	if (!_isEditingShapes)
	{
		ErrorMessage(tkSHPFILE_NOT_IN_EDIT_MODE);
		*retVal = VARIANT_FALSE;
	}
	else
	{
		if (this->ReprojectCore(newProjection, reprojectedCount, NULL, true))
		{
			// spatial index must be deleted, as it became useful all the same
			CString name = _shpfileName.Left(_shpfileName.GetLength() - 3) + "mwd";
			if (Utility::fileExists(name))
			{
				remove(name);
			}
			name = _shpfileName.Left(_shpfileName.GetLength() - 3) + "mwx";
			if (Utility::fileExists(name))
			{
				remove(name);
			}
			
			// update qtree
			if (useQTree)
				GenerateQTree();

			VARIANT_BOOL vbretval;
			this->RefreshExtents(&vbretval);
			*retVal = VARIANT_TRUE;
			return S_OK;
		}
		else
		{
			*retVal = NULL;
			return S_FALSE;
		}
	}
	return S_OK;
}

// *****************************************************************
//		ReprojectCore()
// *****************************************************************
bool CShapefile::ReprojectCore(IGeoProjection* newProjection, LONG* reprojectedCount, IShapefile** retVal, bool reprojectInPlace)
{
	if (m_sourceType == sstUninitialized )
	{
		ErrorMessage(tkSHAPEFILE_UNINITIALIZED);
		return false;
	}

	if (!newProjection)
	{
		ErrorMessage(tkUNEXPECTED_NULL_PARAMETER);
		return false;
	}

	VARIANT_BOOL isEmpty1, isEmpty2;
	newProjection->get_IsEmpty(&isEmpty1);
	m_geoProjection->get_IsEmpty(&isEmpty2);
	if (isEmpty1 || isEmpty2)
	{
		ErrorMessage(tkPROJECTION_NOT_INITIALIZED);
		return false;
	}
	
	OGRSpatialReference* projSource = ((CGeoProjection*)m_geoProjection)->get_SpatialReference();
	OGRSpatialReference* projTarget = ((CGeoProjection*)newProjection)->get_SpatialReference();

	OGRCoordinateTransformation* transf = OGRCreateCoordinateTransformation( projSource, projTarget );
	if (!transf)
	{
		ErrorMessage(tkFAILED_TO_REPROJECT);
		return false;
	}

	// creating a copy of the shapefile
	if (!reprojectInPlace)
		this->Clone(retVal);

	// do reprojection
	CComVariant var;
	long numShapes = _shapeData.size();
	long count = 0;

	long numFields, percent = 0;
	this->get_NumFields(&numFields);
	
	VARIANT_BOOL vbretval = VARIANT_FALSE;
	*reprojectedCount = 0;

	for (long i = 0; i < numShapes; i++)
	{
		if( globalCallback != NULL )
		{
			long newpercent = (long)(((double)(i + 1)/numShapes)*100);
			if( newpercent > percent )
			{	
				percent = newpercent;
				globalCallback->Progress(OLE2BSTR(key),percent,A2BSTR("Projecting..."));
			}
		}
		
		IShape* shp = NULL;
		this->get_Shape(i, &shp);
		
		if (!reprojectInPlace)
		{
			IShape* shpNew = NULL;
			shp->Clone(&shpNew);
			shp->Release();
			shp = shpNew;
		}
		
		if (shp)
		{
			long numPoints;
			shp->get_NumPoints(&numPoints);
			
			if(numPoints > 0)
			{
				double* x = new double[numPoints];
				double* y = new double[numPoints];

				// extracting coordinates
				for (long j = 0; j < numPoints; j++)
				{
					shp->get_XY(j, x + j, y + j, &vbretval);
				}

				// will work faster after embedding to the CShape class
				BOOL res = transf->Transform( numPoints, x, y);
				if (res)	
				{
					// saving updated coordinates
					for (long j = 0; j < numPoints; j++)
					{
						shp->put_XY(j, x[j], y[j], &vbretval);
					}

					if (!reprojectInPlace)
					{
						(*retVal)->get_NumShapes(&count);
						(*retVal)->EditInsertShape(shp, &count, &vbretval);
						
						// copying attributes
						for (long j = 0; j < numFields; j++)
						{
							this->get_CellValue(j, i, &var);
							(*retVal)->EditCellValue(j, i, var, &vbretval);
						}
					}
					(*reprojectedCount)++;
				}
				delete[] x; delete[] y;
			}
			shp->Release();	
		}
	}
	
	if( globalCallback != NULL )
		globalCallback->Progress(OLE2BSTR(key),100,A2BSTR(""));

	// setting new projection
	if (reprojectInPlace)
	{
		m_geoProjection->CopyFrom(newProjection, &vbretval);
	}
	else
	{
		IGeoProjection* proj = NULL;
		(*retVal)->get_GeoProjection(&proj);
		if (proj)
		{
			proj->CopyFrom(newProjection, &vbretval);
			proj->Release();
		}
	}
	
	// it's critical to set correct projection, so false will be returned if it wasn't done
	return vbretval ? true : false;
}
#pragma endregion

// *****************************************************************
//		FixUpShapes()
// *****************************************************************
STDMETHODIMP CShapefile::FixUpShapes(IShapefile** retVal, VARIANT_BOOL* fixed)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*fixed = VARIANT_FALSE;
	this->Clone(retVal);
	
	VARIANT_BOOL vbretval;
	if (*retVal)
	{
		long numFields;
		this->get_NumFields(&numFields);
		long percent = 0;
		int numShapes = _shapeData.size();
		*fixed = VARIANT_TRUE;

		for (int i = 0; i < numShapes; i++)
		{
			if( globalCallback != NULL )
			{
				long newpercent = (long)(((double)(i + 1)/numShapes)*100);
				if( newpercent > percent )
				{	
					percent = newpercent;
					globalCallback->Progress(OLE2BSTR(key),percent,A2BSTR("Fixing..."));
				}
			}
			
			IShape* shp = NULL;
			this->get_Shape(i, &shp);
			if (shp)
			{
				IShape* shpNew = NULL;
				shp->FixUp(&shpNew);
				if (!shpNew)
				{
					shp->Clone(&shpNew);
					*fixed = VARIANT_FALSE;	// if at least one shape isn't fixed, false will be returned.
				}
				
				long shapeIndex;
				(*retVal)->get_NumShapes(&shapeIndex);
				(*retVal)->EditInsertShape(shpNew, &shapeIndex, &vbretval);
				
				if (vbretval)
				{
					// copy attributes
					CComVariant var;
					for (int iFld = 0; iFld < numFields; iFld++)
					{	
						this->get_CellValue(iFld, i, &var);
						(*retVal)->EditCellValue(iFld, shapeIndex, var, &vbretval);
					}
				}
				shp->Release();
			}
		}
	}

	if( globalCallback != NULL )
		globalCallback->Progress(OLE2BSTR(key),0,A2BSTR(""));

	return S_OK;
}
