//********************************************************************************************************
//File name: OGR2MWShape.cpp
//Description: functions for conversion between OGRGeomerty Class and MapWinGis Shape Representation
//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specific language governing rights and 
//limitations under the License. 
//
//The initial version of this code was adapted from GDAL/OGR library by Sergei Leschinski on 31 jul 2009
//See OGRGeometry inheritance diagram is here: http://gdal.org/ogr/classOGRGeometry.html
//See discussion of here: http://www.mapwindow.org/phorum/read.php?5,13428
//Contributor(s): (Open source contributors should list themselves and their modifications here). 

#include "stdafx.h"
#include "GeometryConverter.h"

// *********************************************************************
//			Shape2GEOSGeom()
// *********************************************************************
//  Converts MapWinGis shape to GEOS geometry
GEOSGeom GeometryConverter::Shape2GEOSGeom(IShape* shp)
{
	OGRGeometry* oGeom = ShapeToGeometry(shp);
	if (oGeom != NULL)
	{
		GEOSGeometry* result = oGeom->exportToGEOS();
		delete oGeom;
		return result;
	}
	else 
		return NULL;
}

// *********************************************************************
//		GEOSGeom2Shape()
// *********************************************************************
// Converts GEOSGeom to MapWinGIS shapes
bool GeometryConverter::GEOSGeomToShapes(GEOSGeom gsGeom, vector<IShape*>* vShapes)
{
	OGRGeometry* oGeom = OGRGeometryFactory::createFromGEOS(gsGeom);
	if (oGeom)
	{
		bool result = GeometryToShapes(oGeom, vShapes);
		delete oGeom;
		return result;
	}
	else
	{
		return false;
	}
}

//**********************************************************************
//							ExplodePolygon()			               
//**********************************************************************
bool GeometryConverter::MultiPolygon2Polygons(OGRGeometry* geom, vector<OGRGeometry*>* results)
{
	if (!geom || !results)
		return false;

	OGRwkbGeometryType oType = geom->getGeometryType();
	if (oType == wkbMultiPolygon || oType == wkbMultiPolygon25D)
	{
		OGRMultiPolygon* oMPoly = (OGRMultiPolygon *) geom;

		for(int iGeom=0; iGeom < oMPoly->getNumGeometries(); iGeom++ )
		{
			OGRGeometry* oPoly = oMPoly->getGeometryRef(iGeom);
			results->push_back(oPoly);
		}
		return (results->size() > 0);
	}
	else
		return false;
}

//**********************************************************************
//							ShapeToGeometry()			               
//**********************************************************************
//  Converts MapWinGis shape object to OGR geometry object.
OGRGeometry* GeometryConverter::ShapeToGeometry(IShape* shp)
{
	if( shp == NULL)
		return NULL;
	
	OGRGeometry * oGeom = NULL;
		
	long numPoints, numParts;
	long beg_part, end_part;
	ShpfileType shptype;
	IPoint* pnt;
	double x,y,z;
	
	shp->get_ShapeType(&shptype);
	shp->get_NumParts(&numParts);
	shp->get_NumPoints(&numPoints);
	
	if (numPoints == 0)
		return NULL;

	
	if (shptype == SHP_POINT || shptype == SHP_POINTM || shptype == SHP_POINTZ)
	{
		shp->get_Point(0, &pnt);

		pnt->get_X(&x);
		pnt->get_Y(&y);
		pnt->get_Z(&z);
		pnt->Release();
		
		oGeom = new OGRPoint(x,y,z);
	}

	else if (shptype == SHP_MULTIPOINT || shptype == SHP_MULTIPOINTM || shptype == SHP_MULTIPOINTZ)
	{
		if (numPoints == 0)
			oGeom = NULL;
		else
		{
			OGRMultiPoint *oMPnt = new OGRMultiPoint();
		
			for( int i = 0; i < numPoints; i++ )
			{
				shp->get_Point(i, &pnt);
				pnt->get_X(&x);
				pnt->get_Y(&y);
				pnt->get_Z(&z);
				pnt->Release();

				OGRPoint* oPnt = new OGRPoint(x,y,z);
				oMPnt->addGeometryDirectly(oPnt);		// no memory release is needed when "direct" methods are used
			}
			oGeom = oMPnt;
		}
	}
	else if (shptype == SHP_POLYLINE || shptype == SHP_POLYLINEM || shptype == SHP_POLYLINEZ)
	{	
		if (numParts == 0)
			oGeom = NULL;
		
		else if (numParts == 1)
		{	
			OGRLineString *oLine = new OGRLineString();
			for( int i = 0; i < numPoints; i++ )
			{
				shp->get_Point(i, &pnt);
				pnt->get_X(&x);
				pnt->get_Y(&y);
				pnt->get_Z(&z);
				pnt->Release();
				oLine->addPoint(x,y,z);
			}
			oGeom = oLine;
		}
		else
		{	
			OGRMultiLineString* oMLine = new OGRMultiLineString();
			
			for( int j = 0; j < numParts; j++ )
			{
				shp->get_Part(j, &beg_part);
				if( numParts - 1 > j )
					shp->get_Part(j +1, &end_part);
				else
					end_part = numPoints;
				
				OGRLineString* oLine = new OGRLineString();
				for (int i = beg_part; i < end_part; i++)
				{
					shp->get_Point(i, &pnt);
					pnt->get_X(&x);
					pnt->get_Y(&y);
					pnt->get_Z(&z);
					pnt->Release();
					oLine->addPoint(x,y,z);
				}
				oMLine->addGeometryDirectly(oLine);
			}
			oGeom = oMLine;
		}
	}
		
	else if (shptype == SHP_POLYGON || shptype == SHP_POLYGONM || shptype == SHP_POLYGONZ)
	{	
		
		if (numParts == 0)
			return NULL;

		VARIANT_BOOL vbretval;
		if (numParts == 1)
		{	
			
			OGRLinearRing* oRing = new OGRLinearRing();
			for( int i = 0; i < numPoints; i++ )
			{
				shp->get_XY(i, &x, &y, &vbretval);
				oRing->addPoint(x,y,0); //z);
			}

			OGRPolygon* oPoly = new OGRPolygon();
			oPoly->addRingDirectly(oRing);
			oGeom = oPoly;
		}
		else
		{	
			OGRPolygon** tabPolygons = new OGRPolygon*[numParts];
			
			for( int j = 0; j < numParts; j++ )
			{			
				/*shp->get_PartIsClockWise(j, &vbretval);
				IShape* part = NULL;
				shp->get_PartAsShape(j, &part);
				CString s = vbretval?"true\n":"false\n";
				double area; 
				part->get_Area(&area);
				part->Release();
				CString s2;
				s2.Format("%f", area);
				AfxMessageBox(s + s2);*/

				shp->get_Part(j, &beg_part);
				if( numParts - 1 > j )
					shp->get_Part(j +1, &end_part);
				else
					end_part = numPoints;
				
				OGRLinearRing* oRing = new OGRLinearRing();

				for (int i = beg_part; i < end_part; i++)
				{
					shp->get_XY(i, &x, &y, &vbretval);
					oRing->addPoint(x,y,0);
				}
				
				tabPolygons[j] = new OGRPolygon();
				tabPolygons[j]->addRingDirectly(oRing);
			}				
			int isValidGeometry;
			const char* papszOptions[] = {"METHOD=ONLY_CCW", NULL};
				
			oGeom = OGRGeometryFactory::organizePolygons
					((OGRGeometry**)tabPolygons, numParts, &isValidGeometry, papszOptions);
			delete[] tabPolygons;
			//if (!isValidGeometry) all polygons will be contained in one multipolygon;
		}
	}
	else
		return NULL;	//	other types aren't supported
	
	if (oGeom != NULL)
	{		
		if (shptype == SHP_POINT || shptype == SHP_MULTIPOINT || shptype == SHP_POLYLINE || shptype == SHP_POLYGON)
			oGeom->setCoordinateDimension(2);
		else
			oGeom->setCoordinateDimension(3);
	}

	return oGeom;
}

/***********************************************************************/
/*							GeometryToShapes()			               */
/***********************************************************************/

/*  Converts OGR geometry object to MapWinGis shape objects. Designed
 *  to handle geometry collections which can store geometry objects of 
 *  different types.
 *  
 *	@param oGeom	input OGR geometry
 *	@param vShapes  vector to return resulting shapes
 *
 *  @return true when at least one shape was created, and false otherwise
 */
bool GeometryConverter::GeometryToShapes(OGRGeometry* oGeom, vector<IShape*>* vShapes)
{
	IShape* shp;
	if (!vShapes->empty()) vShapes->clear();

	if (oGeom == NULL) return false;
	
	bool bPoly =false;		bool bPoly25 =false;
	bool bLine = false;		bool bLine25 = false;
	bool bPoint = false;	bool bPoint25 = false;

	OGRwkbGeometryType oType;
	oType = oGeom->getGeometryType();

	if (oType == wkbGeometryCollection || oType == wkbGeometryCollection25D)
	{
		OGRGeometryCollection* oGCol = (OGRGeometryCollection *) oGeom;
		OGRGeometry* oTest;
		
		for (int i =0; i < oGCol->getNumGeometries(); i++)		
		{	
			oTest =  (OGRGeometry *) oGCol->getGeometryRef(i);
			oType = oTest->getGeometryType(); 
			
			if		(oType == wkbPoint)				bPoint = true;
			else if (oType == wkbPoint25D)			bPoint25 = true;
			else if (oType == wkbLineString)		bLine = true;
			else if (oType == wkbLineString25D)		bLine25  = true;
			else if (oType == wkbPolygon)			bPoly = true;
			else if (oType == wkbPolygon25D)		bPoly25 = true;
			else return false;		// we don't accept other types for now
		}
		if (bPoly25)		
		{	shp = GeometryConverter::GeometryToShape(oGeom, wkbMultiPolygon25D);
			if (shp != NULL) vShapes->push_back(shp);
		}
		else if (bPoly)		
		{	shp = GeometryConverter::GeometryToShape(oGeom, wkbMultiPolygon);
			if (shp != NULL) vShapes->push_back(shp);
		}
		
		if (bLine25)	
		{	shp = GeometryConverter::GeometryToShape(oGeom, wkbMultiLineString25D);
			if (shp != NULL) vShapes->push_back(shp);
		}
		else if (bLine)		
		{	shp = GeometryConverter::GeometryToShape(oGeom, wkbMultiLineString);
			if (shp != NULL) vShapes->push_back(shp);
		}
		
		if (bPoint25)	
		{	shp = GeometryConverter::GeometryToShape(oGeom, wkbMultiPoint25D);
			if (shp != NULL) vShapes->push_back(shp);
		}
		else if (bPoint)	
		{	shp = GeometryConverter::GeometryToShape(oGeom, wkbMultiPoint);
			if (shp != NULL) vShapes->push_back(shp);
		}
	}
	else
	{
		shp = GeometryConverter::GeometryToShape(oGeom);
		if (shp != NULL) vShapes->push_back(shp);
	}
	if (vShapes->size() > 0)	return true;
	else						return false;
}

/***********************************************************************/
/*							GeometryToShape()			               */
/***********************************************************************/

/*  Converts OGR geometry object to a single MapWinGis shape object. In 
 *  case of geometry collections return only type of geometries specified
 *	by parameter.
 *  
 *	@param oGeom	 input OGR geometry
 *	@param oBaseType sub type of geometry collection. Geometry collection 
 *					 will be treated as object of this geometry type
 *
 *  @return pointer to shape on succes, or NULL otherwise
 */
IShape * GeometryConverter::GeometryToShape(OGRGeometry* oGeom, OGRwkbGeometryType oBaseType)
{
	if (oGeom == NULL)
		return NULL;
	
	IShape* shp = NULL;

	IPoint* pnt;
	ShpfileType shptype;
	VARIANT_BOOL retval;
	long partIndex = 0;
	long pointIndex = 0;
	
	OGRwkbGeometryType oType;
	oType = oGeom->getGeometryType();

	if (oType == wkbGeometryCollection || oType == wkbGeometryCollection25D)
	{
		if ( oBaseType == wkbMultiPolygon || oBaseType == wkbMultiPolygon25D ||
			 oBaseType == wkbMultiLineString || oBaseType == wkbMultiLineString25D ||
			 oBaseType == wkbMultiPoint || oBaseType == wkbMultiPoint25D)
	
			oType = oBaseType;
		else
		{
			// to return objects of all types from geometry collection use GeometryToShapes
			return NULL;	
		}
	}

	if (oType == wkbPoint || oType == wkbPoint25D) 
	{
		if		(oType == wkbPoint) 	shptype = SHP_POINT;
		else if (oType == wkbPoint25D)	shptype = SHP_POINTZ;
		
		OGRPoint* oPnt = (OGRPoint *) oGeom;

		CoCreateInstance(CLSID_Shape,NULL,CLSCTX_INPROC_SERVER,IID_IShape,(void**)&shp);
		shp->put_ShapeType(shptype);

		m_factory.pointFactory->CreateInstance(NULL, IID_IPoint, (void**)&pnt);
		//CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&pnt);
		pnt->put_X(oPnt->getX());
		pnt->put_Y(oPnt->getY());
		pnt->put_Z(oPnt->getZ());
		shp->InsertPoint(pnt,&pointIndex,&retval);
		pnt->Release();
	}
	
	else if (oType == wkbMultiPoint || oType == wkbMultiPoint25D)
	{
		if		(oType == wkbMultiPoint) 	shptype = SHP_MULTIPOINT;
		else if (oType == wkbMultiPoint25D)	shptype = SHP_MULTIPOINTZ;
		
		OGRMultiPoint* oMPnt = (OGRMultiPoint* ) oGeom;
		if (oMPnt->getNumGeometries() == 0)
			return NULL;
		
		CoCreateInstance(CLSID_Shape,NULL,CLSCTX_INPROC_SERVER,IID_IShape,(void**)&shp);
		shp->put_ShapeType(shptype);

		for(long i = 0; i < oMPnt->getNumGeometries(); i++ )
		{
			OGRPoint* oPnt = (OGRPoint *) oMPnt->getGeometryRef(i);
			if (oPnt != NULL)
			{
				if (oPnt->getGeometryType() == wkbPoint || oPnt->getGeometryType() == wkbPoint25D)
				{
					m_factory.pointFactory->CreateInstance(NULL, IID_IPoint, (void**)&pnt);
					//CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&pnt);
					pnt->put_X(oPnt->getX());
					pnt->put_Y(oPnt->getY());
					pnt->put_Z(oPnt->getZ());
					shp->InsertPoint(pnt,&i,&retval);
					pnt->Release();
				}
			}
		}
	}
	
	else if (oType == wkbLineString || oType == wkbLineString25D)
	{
		if		(oType == wkbLineString) 	shptype = SHP_POLYLINE;
		else if (oType == wkbLineString25D)	shptype = SHP_POLYLINEZ;
		
		OGRLineString* oLine = (OGRLineString *) oGeom;
		if (oLine->getNumPoints() == 0)
			return NULL;

		CoCreateInstance(CLSID_Shape,NULL,CLSCTX_INPROC_SERVER,IID_IShape,(void**)&shp);
		shp->put_ShapeType(shptype);
		shp->InsertPart(0,&partIndex,&retval);
	
		for(long i = 0; i < oLine->getNumPoints(); i++ )
		{
			m_factory.pointFactory->CreateInstance(NULL, IID_IPoint, (void**)&pnt);
			//CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&pnt);
			pnt->put_X(oLine->getX(i));
			pnt->put_Y(oLine->getY(i));
			pnt->put_Z(oLine->getZ(i));
			shp->InsertPoint(pnt,&i,&retval);
			pnt->Release();
		}
	}
	
	else if (oType == wkbMultiLineString || oType == wkbMultiLineString25D)
	{
		if		(oType == wkbMultiLineString) 		shptype = SHP_POLYLINE;
		else if (oType == wkbMultiLineString25D)	shptype = SHP_POLYLINEZ;

		OGRMultiLineString * oMLine = (OGRMultiLineString *) oGeom;
		if (oMLine->getNumGeometries() == 0) 
			return NULL;

		CoCreateInstance(CLSID_Shape,NULL,CLSCTX_INPROC_SERVER,IID_IShape,(void**)&shp);
		shp->put_ShapeType(shptype);
		
		long count = 0;
		for (long j=0; j < oMLine->getNumGeometries(); j++)
		{	
			OGRLineString *oLine;
			oLine = (OGRLineString *) oMLine->getGeometryRef(j);
			
			if (!oLine == NULL)
			{
				if (oLine->getGeometryType() == wkbLineString || oLine->getGeometryType() == wkbLineString25D)
				{
					shp->InsertPart(count,&j,&retval);

					for(long i = 0; i < oLine->getNumPoints(); i++ )
					{
						m_factory.pointFactory->CreateInstance(NULL, IID_IPoint, (void**)&pnt);
						//CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&pnt);
						pnt->put_X(oLine->getX(i));
						pnt->put_Y(oLine->getY(i));
						pnt->put_Z(oLine->getZ(i));
						shp->InsertPoint(pnt,&count,&retval);
						pnt->Release();
						if (retval) count++;
					}
				}
			}
		}
	}
	
	// (meant to be part of poly; but we'll treat it alone also)
	else if (oType == wkbLinearRing)	
	{
		OGRLinearRing* oRing;
		
		shptype = SHP_POLYGON;
		if (oGeom->getCoordinateDimension() == 3)	shptype = SHP_POLYGONZ;
		else										shptype = SHP_POLYGON;
		
		oRing = (OGRLinearRing*) oGeom;
		if (oRing->getNumPoints() == 0) 
			return NULL;
		
		CoCreateInstance(CLSID_Shape,NULL,CLSCTX_INPROC_SERVER,IID_IShape,(void**)&shp);
		shp->put_ShapeType(shptype);
		shp->InsertPart(0,&partIndex,&retval);

		for(long i=0; i< oRing->getNumPoints(); i++)
		{
			m_factory.pointFactory->CreateInstance(NULL, IID_IPoint, (void**)&pnt);
			//CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&pnt);
			pnt->put_X(oRing->getX(i));
			pnt->put_Y(oRing->getY(i));
			pnt->put_Z(oRing->getZ(i));
			shp->InsertPoint(pnt,&i,&retval);
			pnt->Release();
		}
	}

	else if (oType == wkbPolygon || oType == wkbPolygon25D || oType == wkbMultiPolygon || oType == wkbMultiPolygon25D)
	{
		OGRPolygon*      oPoly;
		OGRLinearRing**  papoRings=NULL;
		int nRings = 0;
		
		if		(oType == wkbPolygon || oType == wkbMultiPolygon)			shptype = SHP_POLYGON;
		else if (oType == wkbPolygon25D || oType == wkbMultiPolygon25D)		shptype = SHP_POLYGONZ;
	
		if (oType == wkbPolygon || oType == wkbPolygon25D)
		{
			oPoly = (OGRPolygon *) oGeom;
			
			// there is no use of holes if we have no poly
			if( oPoly->getExteriorRing() == NULL)		return NULL;	
			if (oPoly->getExteriorRing()->IsEmpty())	return NULL;
			
			int nSrcRings = oPoly->getNumInteriorRings()+1;
			papoRings = (OGRLinearRing **) malloc(sizeof(void*)* nSrcRings); //CPLMalloc(sizeof(void*)* nSrcRings);

			for(int iRing = 0; iRing < nSrcRings; iRing++ )
			{
				if( iRing == 0 )	papoRings[nRings] = oPoly->getExteriorRing();
				else				papoRings[nRings] = oPoly->getInteriorRing(iRing-1);
				
				if (!papoRings[nRings]->IsEmpty()) 
					nRings ++;
			}
		}
				
		else if (oType == wkbMultiPolygon || oType == wkbMultiPolygon25D)
		{
			OGRMultiPolygon* oMPoly = (OGRMultiPolygon *) oGeom;

			for(int iGeom=0; iGeom < oMPoly->getNumGeometries(); iGeom++ )
			{
				oPoly = (OGRPolygon *) oMPoly->getGeometryRef(iGeom);
				
				if (oPoly->getGeometryType() == wkbPolygon || oPoly->getGeometryType() == wkbPolygon25D)
				{					
					if( oPoly->getExteriorRing() == NULL)		continue;
					if (oPoly->getExteriorRing()->IsEmpty())	continue;
							
					papoRings = (OGRLinearRing **) realloc(papoRings, sizeof(void*)* (nRings + oPoly->getNumInteriorRings() + 1));
					for(int iRing = 0; iRing < oPoly->getNumInteriorRings()+1; iRing++ )
					{
						if(iRing == 0)	papoRings[nRings] = oPoly->getExteriorRing();
						else			papoRings[nRings] = oPoly->getInteriorRing( iRing-1 );
						
						if (!papoRings[nRings]->IsEmpty())
						nRings ++;
					}
				}
			}
		}
		
		if (nRings == 0)
		{	
			if (papoRings !=NULL) free(papoRings);
			return NULL;
		}

		CoCreateInstance(CLSID_Shape,NULL,CLSCTX_INPROC_SERVER,IID_IShape,(void**)&shp);
		shp->put_ShapeType(shptype);
		
		OGRLinearRing *oRing;
		long count = 0;

		for(long iRing = 0; iRing < nRings; iRing++ )	
		{
			oRing = papoRings[iRing];
			shp->InsertPart(count,&iRing,&retval);
			
			for(long i = 0; i < oRing->getNumPoints(); i++ )		
			{
				m_factory.pointFactory->CreateInstance(NULL, IID_IPoint, (void**)&pnt);
				//CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&pnt);
				pnt->put_X(oRing->getX(i));
				pnt->put_Y(oRing->getY(i));
				//pnt->put_Z(oRing->getZ(i));
				shp->InsertPoint(pnt,&count,&retval);
				pnt->Release();
				pnt = NULL;
				if (retval) 
					count++;
			}
		}
		//CPLFree(papoRings);
		free(papoRings);
	}
	else	
		return NULL;	//(oType == wkbUnknown || oType == wkbNone)

	if (!shp == NULL)
	{	long numPoints = 0;
		shp->get_NumPoints(&numPoints);
		if (numPoints == 0) 
		{
			shp->Release();
			shp = NULL;
		}
	}
	return shp;
}
 
/***********************************************************************/
/*						Read_OGR_Layer()				               */
/***********************************************************************/

/*  Reads OGR layer from the disk into memory shapefile. Format of input 
 *	layer is determined automatically by OGR library. Type of resulting
 *  shapefile is read from layer or specified as a parameter for formats
 *  which can store features of different types simultaneously (MapInfo 
 *  *.tab format for example). 
 *  
 *	@param Filename	 name of the file with a layer info on the disk
 *	@param shpType	 type of shapefile which will be created. Will be
 *					 read from layer if none was specified.
 *  @return resulting shapefile pointer on success, or NULL otherwise
 */
IShapefile* GeometryConverter::Read_OGR_Layer(BSTR Filename, ShpfileType shpType)
{
	USES_CONVERSION;
	
	CString fname = W2CA(Filename);
	if (fname.GetLength() == 0) return NULL;

	OGRRegisterAll();
    
	OGRDataSource*	oData;
    oData = OGRSFDriverRegistrar::Open(fname, FALSE);
    if( oData == NULL ) return NULL;

	OGRLayer*	oLayer;
	OGRFeature* oFeature;
    
	oLayer = oData->GetLayer(0);
	OGRFeatureDefn* oLDefn = oLayer->GetLayerDefn();
	oLayer->ResetReading();

/* ----------------------------------------------------------------- */
/*		Creation of shapefile										 */
/* ----------------------------------------------------------------- */
	IShapefile* sf;
	VARIANT_BOOL vbretval;
	
	if (shpType == SHP_NULLSHAPE)
		shpType = GeometryType2ShapeType(oLDefn->GetGeomType());
	
	CoCreateInstance(CLSID_Shapefile,NULL,CLSCTX_INPROC_SERVER,IID_IShapefile,(void**)&sf);
	sf->CreateNew(A2BSTR(""), shpType, &vbretval);
    
/* ----------------------------------------------------------------- */
/*		Converting of fields										 */
/* ----------------------------------------------------------------- */
    for(long iFld = 0; iFld < oLDefn->GetFieldCount(); iFld++ )
    {
		IField * fld = NULL;
		CoCreateInstance(CLSID_Field,NULL,CLSCTX_INPROC_SERVER,IID_IField,(void**)&fld);
		
		OGRFieldDefn* oField = oLDefn->GetFieldDefn(iFld);
		OGRFieldType type = oField->GetType();
		
		if( type == OFTInteger )	fld->put_Type(INTEGER_FIELD);
        else if(type == OFTReal )	fld->put_Type(DOUBLE_FIELD);
        else if(type == OFTString )	fld->put_Type(STRING_FIELD);

		fld->put_Name(A2BSTR(oField->GetNameRef()));
		fld->put_Width((long)oField->GetWidth());
		fld->put_Precision((long)oField->GetPrecision());

		sf->EditInsertField(fld, &iFld, NULL, &vbretval);
		fld->Release();
    }
	
/* ----------------------------------------------------------------- */
/*		Converting of the shapes and cellvalues						 */
/* ----------------------------------------------------------------- */
	while( (oFeature = oLayer->GetNextFeature()) != NULL )
    {
		OGRGeometry *oGeom;
        oGeom = oFeature->GetGeometryRef();
		if(oGeom == NULL) continue;
		
		IShape* shp =NULL;
		shp = GeometryConverter::GeometryToShape(oGeom);
		
		long numShapes;
		sf->get_NumShapes(&numShapes);
		sf->EditInsertShape(shp, &numShapes, &vbretval);

        for(int iFld = 0; iFld < oLDefn->GetFieldCount(); iFld++ )
        {
            OGRFieldDefn* oField = oLDefn->GetFieldDefn(iFld);
			OGRFieldType type = oField->GetType();
			VARIANT val;
			VariantInit(&val);

			if(type == OFTInteger)	
			{
				val.vt = VT_I4;
				val.lVal = oFeature->GetFieldAsInteger(iFld);
			}
            else if(type == OFTReal)
			{
				val.vt = VT_R8;
				val.dblVal = oFeature->GetFieldAsDouble(iFld);
			}
		    else //if (type == OFTString )
			{	
				val.vt = VT_BSTR;
				val.bstrVal = A2BSTR(oFeature->GetFieldAsString(iFld));	
			}
            sf->EditCellValue(iFld, numShapes, val, &vbretval);
			VariantClear(&val);
        }
        OGRFeature::DestroyFeature(oFeature);
    }
	
    OGRDataSource::DestroyDataSource(oData);

	sf->RefreshExtents(&vbretval);
	return sf;
}

/***********************************************************************/
/*			GeometryType2ShapeType()/ShapeType2GeometryType			   */
/***********************************************************************/

/*  Establish correspondance between the types of MapWinGis shapefile  
 *	layer and the types of ogr layers.
 */
ShpfileType GeometryConverter::GeometryType2ShapeType(OGRwkbGeometryType oType)
{
	switch ( oType )
	{
		case wkbPoint:				return SHP_POINT;
		case wkbPoint25D:			return SHP_POINTZ;
		case wkbMultiPoint:			return SHP_MULTIPOINT;
		case wkbMultiPoint25D:		return SHP_MULTIPOINTZ;
		case wkbLineString:			return SHP_POLYLINE;
		case wkbLineString25D:		return SHP_POLYLINEZ;
		case wkbPolygon:			return SHP_POLYGON;
		case wkbPolygon25D:			return SHP_POLYGONZ;
		case wkbMultiPolygon:		return SHP_POLYGON;
		case wkbMultiPolygon25D:	return SHP_POLYGONZ;
		case wkbNone:				return SHP_NULLSHAPE;
	}
	return SHP_NULLSHAPE;
}
OGRwkbGeometryType GeometryConverter::ShapeType2GeometryType(ShpfileType shpType)
{
	switch( shpType )
	{
		case SHP_POINT:			return wkbPoint;
		case SHP_POINTM:		return wkbPoint;
		case SHP_POINTZ:		return wkbPoint25D;
		case SHP_MULTIPOINT:	return wkbMultiPoint;
		case SHP_MULTIPOINTM:	return wkbMultiPoint;
		case SHP_MULTIPOINTZ:	return wkbMultiPoint25D;
		case SHP_POLYLINE:		return wkbLineString;
		case SHP_POLYLINEM:		return wkbLineString;
		case SHP_POLYLINEZ:		return wkbLineString25D;
		case SHP_POLYGON:		return wkbPolygon;
		case SHP_POLYGONM:		return wkbPolygon;
		case SHP_POLYGONZ:		return wkbPolygon25D;
		case SHP_NULLSHAPE:		return wkbNone;
	}
	return wkbNone;
}

// **********************************************************************
// *						Write_OGR_Layer()				               
// **********************************************************************
//  Writes memory shapefile to the file of specified format.
//  Isn't implemented yet.
bool GeometryConverter::Write_OGR_Layer(IShapefile* sf, BSTR Filename)
{
	return false;
}

#pragma region Clipper

// *********************************************************************
//			Shape2ClipperPolygon()
// *********************************************************************
// Converts shape clipper polygon 
using namespace clipper;
TPolyPolygon* GeometryConverter::Shape2ClipperPolygon(IShape* shp)
{
	if (!shp) 
		return NULL;

	ShpfileType shpType;
	shp->get_ShapeType(&shpType);
	if (shpType != SHP_POLYGON && shpType != SHP_POLYGONM && shpType != SHP_POLYGONZ)
		return NULL;
	
	long numParts, numPoints;
	shp->get_NumParts(&numParts);
	shp->get_NumPoints(&numPoints);
	
	if (numPoints == 0 || numParts == 0)
		return NULL;
	
	TPolyPolygon* retval = new TPolyPolygon();
	for (int i = 0; i < numParts; i++)
	{
		TPolygon polygon;
		
		long begin, end;
		shp->get_Part(i, &begin);
		shp->get_EndOfPart(i, &end);
		
		VARIANT_BOOL vbretval;
		for (int j = begin; j <= end - 1; j++ )			// -1 added
		{
			TDoublePoint pnt;
			shp->get_XY(j, &(pnt.X), &(pnt.Y), &vbretval);
			if (conversionFactor != 1.0)
			{
				pnt.X *= conversionFactor;
				pnt.Y *= conversionFactor;
			}
			polygon.push_back(pnt);
		}
		retval->push_back(polygon);
	}

	if (retval->size() > 0)
	{
		return retval;
	}
	else
	{
		delete retval;
		return NULL;
	}
}

// ******************************************************************
//		ClipperPolygon2Shape()
// ******************************************************************
// Converts clipper polygon to shape
// TODO: perhaps include polygon type as a parameter
IShape* GeometryConverter::ClipperPolygon2Shape(TPolyPolygon* polygon)
{
	bool pointsExist = false;
	for (long i = 0; i < (long)polygon->size(); i++)
	{
		TPolygon* poly = &((*polygon)[i]);
		if (poly->size() > 0)
		{
			pointsExist = true;
			break;
		}
	}

	if (!pointsExist)
		return NULL;
	
	IShape* shp = NULL;
	CoCreateInstance(CLSID_Shape,NULL,CLSCTX_INPROC_SERVER,IID_IShape,(void**)&shp);
	if (!shp) 
		return NULL;
	
	VARIANT_BOOL vbretval;
	shp->Create(SHP_POLYGON, &vbretval);

	long cnt = 0;
	long part = 0;
	for (long i = 0; i < (long)polygon->size(); i++)
	{
		TPolygon* poly = &((*polygon)[i]);
		if (poly->size() > 0)
		{
			shp->InsertPart(cnt, &part, &vbretval);
			part++;
			
			//for (unsigned int j = 0; j < poly->size(); j++)
			int j = poly->size() - 1;
			for (; j >= 0; j--)
			{
				IPoint* pnt = NULL;
				m_factory.pointFactory->CreateInstance(NULL, IID_IPoint, (void**)&pnt);
				//CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&pnt);
				
				if (conversionFactor != 1.0)
				{
					(*poly)[j].X  /= conversionFactor;
					(*poly)[j].Y  /= conversionFactor;
				}
				
				pnt->put_X((*poly)[j].X );
				pnt->put_Y((*poly)[j].Y );
				
				shp->InsertPoint(pnt, &cnt, &vbretval);
				pnt->Release();
				cnt++;
			}
			
			// the first and the last point of the part must be the same
			int size = poly->size() - 1;
			if (size > 0)
			{
				if (((*poly)[0]).X != ((*poly)[size]).X ||
					((*poly)[0]).Y != ((*poly)[size]).Y)
				{
					IPoint* pnt = NULL;
					m_factory.pointFactory->CreateInstance(NULL, IID_IPoint, (void**)&pnt);
					//CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&pnt);
					pnt->put_X((*poly)[size].X );
					pnt->put_Y((*poly)[size].Y );
					shp->InsertPoint(pnt, &cnt, &vbretval);
					pnt->Release();
					cnt++;
				}
			}
		}
	}
	return shp;
}

// ******************************************************************
//		ClipPolygon()
// ******************************************************************
IShape* GeometryConverter::ClipPolygon(IShape* shapeClip, IShape* shapeSubject, PolygonOperation operation)
{
	TClipType operNew;
	switch(operation)
	{
		case DIFFERENCE_OPERATION:
			operNew = ctDifference;
			break;
		case INTERSECTION_OPERATION:
			operNew = ctIntersection;
			break;
		case EXCLUSIVEOR_OPERATION:
			operNew = ctXor;
			break;
		case UNION_OPERATION:
			operNew = ctUnion;
			break;
		default: 
			return NULL;
	}
	
	VARIANT_BOOL vbretval;
	CString s;
	
	shapeSubject->get_PartIsClockWise(0, &vbretval);
	s = "Subject: ";
	s += vbretval ? "CW" : "CCW";
	//AfxMessageBox(s);
	
	shapeClip->get_PartIsClockWise(0, &vbretval);
	s = "Clip: ";
	s += vbretval ? "CW" : "CCW";
	//AfxMessageBox(s);

	//shapeSubject
	GeometryConverter ogr;
	TPolyPolygon* poly1 = ogr.Shape2ClipperPolygon(shapeClip);
	TPolyPolygon* poly2 = ogr.Shape2ClipperPolygon(shapeSubject);
	if (poly1 && poly2)
	{
		
		TPolyPolygon* result = ClipPolygon(poly1, poly2, operNew);
		if (result)
		{
			IShape* shp = ogr.ClipperPolygon2Shape(result);
			delete result;
			return shp;
		}
		else
		{
			return NULL;
		}
	}
	else
	{
		return NULL;
	}
}

// ******************************************************************
//		ClipPolygon()
// ******************************************************************
TPolyPolygon* GeometryConverter::ClipPolygon(TPolyPolygon* polyClip, TPolyPolygon* polySubject, TClipType operation)
{
	if (polyClip && polySubject)
	{
		if (polyClip->size() > 0 && polySubject->size() > 0)
		{
			clipper::Clipper* c = new clipper::Clipper();
			TPolyPolygon* solution = new TPolyPolygon();
			
			for (unsigned int i = 0; i < polySubject->size(); i++)
			{
				c->AddPolygon((*polySubject)[i], ptSubject);
			}

			for (unsigned int i = 0; i < polyClip->size(); i++)
			{
				c->AddPolygon((*polyClip)[i], ptClip);
			}
			
			c->Execute(operation, *solution);		// ctIntersection
			c->Clear();
			delete c;
			return solution;
		}
		else
		{
			return NULL;
		}
	}
	else
	{
		return NULL;
	}
}
#pragma endregion

// ********************************************************************
//		MergeGeosGeometries
// ********************************************************************
// Returns GEOS geometry which is result of union of the geomtries passed
// Initial geomtries are deleted in the process !!!
GEOSGeometry* GeometryConverter::MergeGeosGeometries( std::vector<GEOSGeometry*>& data, ICallback* callback, bool deleteInput )
{
	if (data.size() == 0)
		return NULL;
	
	USES_CONVERSION;
	GEOSGeometry* g1 = NULL;
	GEOSGeometry* g2 = NULL;
	
	bool stop = false;
	int count = 0;	// number of union operation performed
	long percent = 0;
	
	int size = data.size();
	int depth = 0;

	while (!stop)
	{
		stop = true;

		for (int i = 0; i < size; i++)
		{
			if (data[i] != NULL)
			{
				bool doUnion = false;
				if (!g1)
				{
					g1 = data[i];
					data[i] = NULL;
				}
				else
				{
					g2 = data[i];
					data[i] = NULL;
				}

				if (g2 != NULL)
				{
					GEOSGeometry* geom = GEOSUnion(g1, g2);
					data[i] = geom;		// placing the resulting geometry back for further processing
					
					if (deleteInput || depth > 0)	// in clipping operation geometries used several times
													// so the intial geometries should be intact (depth == 0)
													// in other cases (Buffer, Dissolve) the geometries can be deleted in place
													// all cases
					{
						GEOSGeom_destroy(g1);
						GEOSGeom_destroy(g2);
					}
					
					g1 = NULL;
					g2 = NULL;
					count++;
					stop = false;		// in case there is at least one union occured, we shall run once more

					if( callback) 
					{
						long newpercent = (long)(((double)count/size)*100); 
						if( newpercent > percent )
						{	
							percent = newpercent;
							callback->Progress(A2BSTR(""),percent,A2BSTR("Merging shapes..."));
						}
					}
				}
				
				// it the last geometry, unpaired one, not the only one, it's the initial and must not be deleted
				if (i == size -1 && stop == false && g2 == NULL && g1 != NULL && depth == 0 && !deleteInput)
				{
					// we need to clone it, to be able to apply unified memory management afterwards
					// when depth > 0 all interim geometries are deleted, while this one should be preserved
					GEOSGeometry* geomTemp = GEOSGeom_clone(g1);
					g1 = geomTemp;
				}
			}
		}
		depth++;
	}
	return g1;
}
