//********************************************************************************************************
//File name: Utils.cpp
//Description: Implementation of CUtils.
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
//3-28-2005 dpa - Added some interpolation functionality and fixed the perimeter calculator.
//6-6-2005 angela - fixed ClipPolygon, GPCPolygonToShape, ShapeToGPCPolygon.
//11-8-2004 angela - fixed is_clockwise and get_Area.
//12-18-2005 cdm - Added Hillshade code from Rob Cairns and Matt Perry
//3-Apr-2006 Rob Cairns - Added TranslateRaster - wrapper for gdal_translate
//29 jul 2009 Sergei Leschinski (lsu) - a fix for ClipPolygon subfunctions
//06-aug-2009 lsu - ShapesIntersection and NextResultShape functions based on OGR/GEOS;
//					shifted implementation of area, length, perimeter properties to CShape 
//********************************************************************************************************
#include "stdafx.h"
#include "Utils.h"

#include "atlsafe.h"
#include <stack>
#include <comdef.h>

#include "gdal_priv.h"
#include "gdal.h"
#include "gdal_alg.h"
#include "cpl_conv.h"
#include "ogr_spatialref.h"
#include "ogr_api.h"
#include "ogr_srs_api.h"
#include "cpl_vsi.h"
#include "cpl_string.h"
#include "vrtdataset.h"
#include "direct.h"

#include "colour.h"
#include "Projections.h"
#include "GeometryConverter.h"
#include "LineBresenham.h"
#include "Image.h"
#include "Shapefile.h"
#include "GeoProjection.h"
#include "macros.h"

#pragma warning(disable:4996)

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

// CUtils
STDMETHODIMP CUtils::PointInPolygon(IShape *Shape, IPoint *TestPoint, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if( Shape != NULL )
	{	
		pip_cache_parts.clear();
		pip_cache_pointsX.clear();
		pip_cache_pointsY.clear();
	
		ShpfileType shptype;
		Shape->get_ShapeType(&shptype);

		if(shptype != SHP_POLYGON && shptype != SHP_POLYGONZ && shptype != SHP_POLYGONM )
		{	
			*retval = FALSE;
			lastErrorCode = tkINCOMPATIBLE_SHAPE_TYPE;
			if( globalCallback != NULL )
				globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
			return S_OK;
		}

		double noval;
		IExtents * box = NULL;
		Shape->get_Extents(&box);
		box->GetBounds(&pip_left,&pip_bottom,&noval,&pip_right,&pip_top,&noval);
		box->Release();

		long numParts = 0;
		long numPoints = 0;
		Shape->get_NumParts(&numParts);
		Shape->get_NumPoints(&numPoints);
		
		if(numParts == 0)//ah 11/22/05, for in-memory shapes that have no parts
		{
			pip_cache_parts.push_back(0);			
		}
		else
		{
			long part =0;
			for( int j = 0; j < numParts; j++ )
			{	
				Shape->get_Part(j,&part);
				pip_cache_parts.push_back(part);
			}
		}
		
		double pointX = 0.0;
		double pointY = 0.0;
		for( int i = 0; i < numPoints; i++ )
		{	
			IPoint * ipnt = NULL;
			Shape->get_Point(i,&ipnt);
			ipnt->get_X(&pointX);
			ipnt->get_Y(&pointY);
			pip_cache_pointsX.push_back(pointX);
			pip_cache_pointsY.push_back(pointY);
			ipnt->Release();
		}
	}
	
	if( TestPoint != NULL )
	{
		double test_pointX, test_pointY;	
		TestPoint->get_X(&test_pointX);
		TestPoint->get_Y(&test_pointY);
			
		//Initial Test on Bounds
		if( test_pointX < pip_left )
		{	
			*retval = FALSE;
			return S_OK;
		}
		else if( test_pointX > pip_right )
		{	
			*retval = FALSE;
			return S_OK;
		}
		if( test_pointY < pip_bottom )
		{	
			*retval = FALSE;
			return S_OK;
		}
		else if( test_pointY > pip_top )
		{	
			*retval = FALSE;
			return S_OK;
		}
			
		//X = U;
		//Y = V;
		//Always drop Z Coordinate and Project on XY Plane
		int beg_part = 0;
		int end_part = 0;

		int number_in_polygons = 0;
		int numCacheParts = (int)pip_cache_parts.size();//ah 6/6/05
		
		for( int j = 0; j < numCacheParts; j++ )
		{
			beg_part = pip_cache_parts[j];
			if( (numCacheParts - 1) > j )
				end_part = pip_cache_parts[j+1];
			else
				end_part = pip_cache_pointsX.size();

			int SH = 0;
			int NSH = 0;
			int NC = 0;		//number_crossings

			for( int i = beg_part; i < end_part - 1; i++ )
			{	
				long corner_two_index = i + 1;
				if( i == pip_cache_pointsX.size() - 1 )
					corner_two_index = beg_part;
				
				//Translate points to origin centered on test_point
				double corner_oneX = pip_cache_pointsX[i] - test_pointX;
				double corner_oneY = pip_cache_pointsY[i] - test_pointY;
				double corner_twoX = pip_cache_pointsX[corner_two_index] - test_pointX;
				double corner_twoY = pip_cache_pointsY[corner_two_index] - test_pointY;
				
				set_sign( corner_oneY, SH );
				set_sign( corner_twoY, NSH );
			
				if( does_cross( SH, NSH, corner_oneX, corner_oneY, corner_twoX, corner_twoY ) )
					NC++;	
				SH = NSH;		
			}
			//ODD Crossings = point_in_part
			if( NC % 2 != 0 )
				number_in_polygons++;		
		}

		//cout<<number_in_polygons<<endl;
		//ODD Part Crossings = point_in_polygon
		if( number_in_polygons% 2 != 0 )
			*retval = VARIANT_TRUE;
		else
			*retval = VARIANT_FALSE;
	}
	else
		*retval = VARIANT_FALSE;
	
	return S_OK;
}

inline bool CUtils::does_cross( int SH, int NSH, double corner_oneX, double corner_oneY, double corner_twoX, double corner_twoY )
{	
	if( SH != NSH )
	{	if( corner_oneX > 0 && corner_twoX > 0 )
			return true;
		else if( corner_oneX > 0 || corner_twoX > 0 )
		{	
			//b = v - u*m
			double m = ( corner_twoX - corner_oneX )/					
					   ( corner_twoY - corner_oneY );

			if( ( corner_oneX - corner_oneY*m ) > 0 )
				return true;
			else
				return false;
		}
		else
			return false;
	}
	else
		return false;
}

inline void CUtils::set_sign( double val, int & SH )
{	if( val < 0 )
		SH = -1;
	else
		SH = 1;
}

STDMETHODIMP CUtils::GridReplace(IGrid *Grid, VARIANT OldValue, VARIANT NewValue, ICallback *cBack, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	if( Grid == NULL )
	{	
		*retval = NULL;
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( cBack != NULL )
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		else if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}

	long ncols = 0, nrows = 0;
	IGridHeader * header = NULL;
	Grid->get_Header(&header);
	header->get_NumberCols(&ncols);
	header->get_NumberRows(&nrows);
	if( ncols <= 0 || nrows <= 0 )
	{	
		*retval = FALSE;
		lastErrorCode = tkZERO_ROWS_OR_COLS;
		if( cBack != NULL )
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		else if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}
	header->Release();

	double oldValue = 0;
	dVal(OldValue,oldValue);
	
	VARIANT vval;
	VariantInit(&vval); //added by Rob Cairns 4-Jan-06
	double val;

	long percent = 0, newpercent = 0;
	double total = nrows * ncols;

	for( int j = 0; j < nrows; j++ )
	{	
		for( int i = 0; i < ncols; i++ )
		{	
			Grid->get_Value(i,j,&vval);
			dVal(vval,val);
			if( val == oldValue )
				Grid->put_Value(i,j,NewValue);

			newpercent = (long)((( j*ncols + i )/total)*100);
			if( newpercent > percent )
			{	
				percent = newpercent;
				if( cBack != NULL )
					cBack->Progress(OLE2BSTR(key), percent, A2BSTR("GridReplace"));
				else if( globalCallback != NULL )
					globalCallback->Progress(OLE2BSTR(key), percent, A2BSTR("GridReplace"));
			}
		}
	}
	
	VariantClear(&vval); //added by Rob Cairns 4-Jan-06
	return S_OK;
}

STDMETHODIMP CUtils::GridInterpolateNoData(IGrid *Grid, ICallback *cBack, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if( Grid == NULL )
	{
		*retval = NULL;
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( cBack != NULL )
		{
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		}
		else if( globalCallback != NULL )
		{
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		}//else if
		*retval = VARIANT_FALSE;
		return S_OK;
	}//if

	long ncols = 0, nrows = 0;
	double nodatavalue;
	IGridHeader * header = NULL;
	VARIANT nodataval_variant;
	VariantInit(&nodataval_variant); //added by Rob Cairns 4-Jan-06
	Grid->get_Header(&header);
	header->get_NumberCols(&ncols);
	header->get_NumberRows(&nrows);
	header->get_NodataValue(&nodataval_variant);
	dVal(nodataval_variant, nodatavalue);
	VariantClear(&nodataval_variant); //added by Rob Cairns 4-Jan-06

	if(ncols <= 0 || nrows <= 0)
	{
		*retval = FALSE;
		lastErrorCode = tkZERO_ROWS_OR_COLS;
		if(cBack != NULL)
		{
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		}
		else if(globalCallback != NULL)
		{
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		}//else
		return S_OK;
	}//if

	GridInterpolate gi(Grid,nodatavalue,nrows,ncols);
	
	if( cBack != NULL )
	{
		cBack->Progress(OLE2BSTR(key), 0, A2BSTR("GridInterpolateNoData"));
	}
	else if( globalCallback != NULL )
	{
		globalCallback->Progress(OLE2BSTR(key), 0, A2BSTR("GridInterpolateNoData"));
	}//else if

	gi.Interpolate(0,0);
	
	if( cBack != NULL )
	{
		cBack->Progress(OLE2BSTR(key), 25, A2BSTR("GridInterpolateNoData"));
	}
	else if( globalCallback != NULL )
	{
		globalCallback->Progress(OLE2BSTR(key), 25, A2BSTR("GridInterpolateNoData"));
	}//else if
	
	gi.Interpolate(0,ncols-1);
	
	if( cBack != NULL )
	{
		cBack->Progress(OLE2BSTR(key), 50, A2BSTR("GridInterpolateNoData"));
	}
	else if( globalCallback != NULL )
	{
		globalCallback->Progress(OLE2BSTR(key), 50, A2BSTR("GridInterpolateNoData"));
	}//else if
	
	gi.Interpolate(nrows-1,0);
	
	if( cBack != NULL )
	{
		cBack->Progress(OLE2BSTR(key), 75, A2BSTR("GridInterpolateNoData"));
	}
	else if( globalCallback != NULL )
	{
		globalCallback->Progress(OLE2BSTR(key), 75, A2BSTR("GridInterpolateNoData"));
	}//else if
	
	gi.Interpolate(nrows-1,ncols-1);
	
	if( cBack != NULL )
	{
		cBack->Progress(OLE2BSTR(key), 100, A2BSTR("GridInterpolateNoData"));
	}
	else if( globalCallback != NULL )
	{
		globalCallback->Progress(OLE2BSTR(key), 100, A2BSTR("GridInterpolateNoData"));
	}//else if

	*retval = VARIANT_TRUE;

	return S_OK;
}

STDMETHODIMP CUtils::RemoveColinearPoints(IShapefile * Shapes, double LinearTolerance, ICallback *cBack, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if( Shapes == NULL )
	{
		*retval = NULL;
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}

	ShpfileType shptype;
	Shapes->get_ShapefileType(&shptype);

	if( shptype != SHP_POLYLINE && shptype != SHP_POLYGON )
	{
		*retval = NULL;
		if( shptype == SHP_POLYLINEZ || shptype == SHP_POLYGONZ ||
			shptype == SHP_POLYLINEM || shptype == SHP_POLYGONM )
			lastErrorCode = tkUNSUPPORTED_SHAPEFILE_TYPE;
		else
			lastErrorCode = tkINCOMPATIBLE_SHAPEFILE_TYPE;

		if( cBack != NULL )
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		else if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}

	VARIANT_BOOL vbretval;
	Shapes->StartEditingShapes(FALSE,cBack,&vbretval);
	long numShapes;
	Shapes->get_NumShapes(&numShapes);
	
	long percent = 0, cnt = 0;
	double total = 2*numShapes;
		
	XRedBlack rb;

	if( shptype == SHP_POLYLINE )
	{
	}
	else if( shptype == SHP_POLYGON )
	{	
		for( int currentShape = 0; currentShape < numShapes; currentShape++ )
		{	
			IShape * shape = NULL;
			Shapes->get_Shape(currentShape,&shape);

			long numPoints = 0, forward_index = 0, backward_index = 0;
			shape->get_NumPoints(&numPoints);
							
			for( int point_index = 0; point_index < numPoints; point_index++ )
			{	
				IPoint * pnt = NULL;
				shape->get_Point(point_index,&pnt);
				double x, y;
				pnt->get_X(&x);
				pnt->get_Y(&y);
				pnt->Release();
				
				POINT p;
				p.x = (LONG)x;
				p.y = (LONG)y;
				rb.Insert( p );

				forward_index = point_index + 1;
				backward_index = point_index - 1;
				if( forward_index >= numPoints )
					forward_index = 0;
				if( backward_index < 0 )
					backward_index = numPoints - 1;				

				IPoint * ione = NULL;
				IPoint * itwo = NULL;
				IPoint * ithree = NULL;

				shape->get_Point(backward_index,&ione);
				shape->get_Point(forward_index,&itwo);
				shape->get_Point(point_index,&ithree);

				double onex, oney;
				ione->get_X(&onex);
				ione->get_Y(&oney);
				double twox, twoy;
				itwo->get_X(&twox);
				itwo->get_Y(&twoy);				
				double threex, threey;
				ithree->get_X(&threex);
				ithree->get_Y(&threey);
					
				POINT one;
				one.x = (LONG)onex;
				one.y = (LONG)oney;

				POINT two;
				two.x = (LONG)twox;
				two.y = (LONG)twoy;

				POINT three;
				three.x = (LONG)threex;
				three.y = (LONG)threey;

				YRedBlackNode * prbn = rb.GetNode( three );
				if( prbn != NULL )
				{
					if( prbn->canSetColinear )
					{
						if( isColinear( one, two, three, LinearTolerance ) )
							prbn->isColinear = true;									
						else
						{	prbn->isColinear = false;
							prbn->canSetColinear = false;
						}
					}
				}
			}			
				
			cnt++;
			int newpercent = (int)((cnt/total)*100);
			if( newpercent > percent )
			{	percent = newpercent;
				if( cBack != NULL )
					cBack->Progress(OLE2BSTR(key), percent, A2BSTR("RemoveColinearPoints"));
				else if( globalCallback != NULL )
					globalCallback->Progress(OLE2BSTR(key), percent, A2BSTR("RemoveColinearPoints"));
			}			
				
			std::deque< POINT > PointsToKeep;
			for( currentShape = 0; currentShape < numShapes; currentShape++ )
			{	
				for( int point_index = 0; point_index < numPoints; point_index++ )
				{	
					IPoint * pnt = NULL;
					shape->get_Point(point_index,&pnt);
					double x, y;
					pnt->get_X(&x);
					pnt->get_Y(&y);
					pnt->Release();
					
					POINT p;
					p.x = (LONG)x; p.y = (LONG)y;
					rb.Insert( p );

					YRedBlackNode * prbn = rb.GetNode( p );
					if( prbn != NULL )
					{	
						if( prbn->isColinear == true && prbn->useCount < 2 )
						{	//Don't Keep the Point
						}
						else
							PointsToKeep.push_back( prbn->Element );
					}
				}
				
				for( int ns = 0; ns < numPoints; ns++ )
					shape->DeletePoint(0,&vbretval);
				
				PointsToKeep.push_back( PointsToKeep[0] );
				for( int i = 0; i < (int)PointsToKeep.size(); i++ )
				{	
					IPoint * pnt = NULL;
					m_factory.pointFactory->CreateInstance(NULL, IID_IPoint, (void**)&pnt);
					//CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&pnt);
					pnt->put_X( PointsToKeep[i].x );
					pnt->put_Y( PointsToKeep[i].y );
					long pntpos = i;
					shape->InsertPoint( pnt, &pntpos, &vbretval );
				}			
				
				PointsToKeep.clear();

				cnt++;
				int newpercent = (int)((cnt/total)*100);
				if( newpercent > percent )
				{	
					percent = newpercent;
					if( cBack != NULL )
						cBack->Progress(OLE2BSTR(key), percent, A2BSTR("RemoveColinearPoints"));
					else if( globalCallback != NULL )
						globalCallback->Progress(OLE2BSTR(key), percent, A2BSTR("RemoveColinearPoints"));
				}				
			}
			
			shape->Release();
			shape = NULL;
		}		
	}	

	Shapes->StopEditingShapes(TRUE,FALSE,cBack,&vbretval);

	return S_OK;
}

bool CUtils::isColinear( POINT one, POINT two, POINT test, double tolerance )
{
	double dx = two.x - one.x;
	double dy = two.y - one.y;
	if( dx != 0 )
	{	//Test to see if the slopes are equal
		double m1 = dy/dx;
		double dx2 = two.x - test.x;
		double dy2 = two.y - test.y;
		if( dx2 != 0 )
		{	
			double m2 = dy2/dx2;
			if( ( m1 > 0 && m2 > 0 ) || ( m1 < 0 && m2 < 0 ) )
			{
				if( fabs( m1 - m2 ) < tolerance )
					return true;
				else
					return false;
			}
			else if( m1 == 0 && m2 == 0 )
				return true;
			else
				return false;
		}
		else
			return false;
	}
	//Vertical Line
	//Check the X's
	else
	{	if( one.x == test.x )
			return true;
		else
			return false;
	}
}

STDMETHODIMP CUtils::get_Length(IShape *Shape, double *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if( Shape == NULL )
	{	
		*pVal = 0.0;
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}
	
	Shape->get_Length(pVal);	// lsu 27-jul-2009
	return S_OK;
}

STDMETHODIMP CUtils::get_Perimeter(IShape *Shape, double *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	if( Shape == NULL )
	{	*pVal = 0.0;
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}
	
	Shape->get_Perimeter(pVal);	// lsu 27-jul-2009
	return S_OK;
}

STDMETHODIMP CUtils::get_Area(IShape *Shape, double *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	if( Shape == NULL )	
	{	
		*pVal = 0.0;
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}
	
	Shape->get_Area(pVal);	// lsu 27-jul-2009
	return S_OK;
}




//ah 2/24/06
//v2: Checks to see if a polygon is oriented clockwise about the normal.
//In multi-part polygons, clockwise parts are filled while counter-clockwise parts are holes.
//Returns true if clockwise, false if counter-clockwise.
bool CUtils::is_clockwise(Poly *polygon)
{
	int numPoints = polygon->polyX.size();
	double area = 0;
	for(int i = 0; i <= numPoints-2; i++)
	{
		double oneX = polygon->polyX[i];
		double oneY = polygon->polyY[i];
		double twoX = polygon->polyX[i+1];		
		double twoY = polygon->polyY[i+1];
			
		double trap_area = ((oneX * twoY) - (twoX * oneY));
		area += trap_area;
	}
	if(area > 0)
	{
		//if area is positive, polygon vertices are oriented
		//counter-clockwise about the normal.
		return false;
	}
	else
	{
		//polygon vertices are oriented clockwise about the normal.
		return true;
	}
}
//ah 2/24/06
//v3: Checks to see if a polygon is oriented clockwise about the normal.
//In multi-part polygons, clockwise parts are filled while counter-clockwise parts are holes.
//Returns true if clockwise, false if counter-clockwise.
bool CUtils::is_clockwise(IShape *Shape)
{
	long numPoints = 0;
	Shape->get_NumPoints(&numPoints);
	double area = 0;

	//calculate cross product for all vertices except last one (repeat of first)
	for(int i = 0; i <= numPoints-2; i++)
	{
		IPoint* pt0 = NULL;
		IPoint* pt1 = NULL;
		Shape->get_Point(i, &pt0);
		Shape->get_Point(i+1, &pt1);

		double oneX = 0;
		double oneY = 0;
		double twoX = 0;
		double twoY = 0;

		pt0->get_X(&oneX);
		pt0->get_Y(&oneY);
		pt0->Release();
		pt1->get_X(&twoX);
		pt1->get_Y(&twoY);
		pt1->Release();
			
		double trap_area = ((oneX * twoY) - (twoX * oneY));
		area += trap_area;
	}
	if(area > 0)
	{
		//if area is positive, polygon vertices are oriented
		//counter-clockwise about the normal.
		return false;
	}
	else
	{
		//polygon vertices are oriented clockwise about the normal.
		return true;
	}
}

bool CUtils::is_clockwise( double x0, double y0, double x1, double y1, double x2, double y2)
{
	//See http://astronomy.swin.edu.au/~pbourke/geometry/clockwise/, by Paul Bourke
	//for better explanation of using cross product to find clockwiseness.
	double crossProduct = (x1 - x0)*(y2 - y1) - (y1 - y0)*(x2 - x1);

	//a negative cross product means that we have a clockwise polygon
	if( crossProduct < 0 )
		return true;
	else
		return false;
}

STDMETHODIMP CUtils::get_LastErrorCode(long *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = lastErrorCode;
	lastErrorCode = tkNO_ERROR;

	return S_OK;
}

STDMETHODIMP CUtils::get_ErrorMsg(long ErrorCode, BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	*pVal = A2BSTR(ErrorMsg(ErrorCode));

	return S_OK;
}

STDMETHODIMP CUtils::get_GlobalCallback(ICallback **pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*pVal = globalCallback;
	if( globalCallback != NULL )
	{	
		globalCallback->AddRef();
	}
	return S_OK;
}

STDMETHODIMP CUtils::put_GlobalCallback(ICallback *newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	put_ComReference(newVal, (IDispatch**)&globalCallback);
	return S_OK;
}

STDMETHODIMP CUtils::get_Key(BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	*pVal = OLE2BSTR(key);

	return S_OK;
}

STDMETHODIMP CUtils::put_Key(BSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	::SysFreeString(key);
	key = OLE2BSTR(newVal);

	return S_OK;
}



STDMETHODIMP CUtils::GridMerge(VARIANT Grids, BSTR MergeFilename, VARIANT_BOOL InRam, GridFileType GrdFileType, ICallback *cBack, IGrid **retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	BSTR final_projection = A2BSTR("");

	//Check the Array Type
	if( Grids.vt != (VT_ARRAY|VT_DISPATCH) )
	{	
		*retval = NULL;
		lastErrorCode = tkINVALID_VARIANT_TYPE;
		if( cBack != NULL )
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		else if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}

	//Check the Array Size
	SAFEARRAY * arr = Grids.parray;
	SAFEARRAYBOUND arraybound = arr->rgsabound[0];
	if( arraybound.cElements <= 0 )
	{	
		*retval = NULL;
		return S_OK;
	}

	std::deque<IGrid*> allGrids;
	int gridSize = 0; //size of allGrids -- ah 6/6/03
	
	int elements = (int)arraybound.cElements; //ah 6/6/03
	
	for( int index = 0; index < elements; index++ )
	{	
		IUnknown * unknown = NULL;
		long ind = index;
		SafeArrayGetElement( arr, &ind, &unknown );
		
		if( unknown == NULL )
			continue;

		//Determine if the IUnknown supports the IGrid Interface
		IGrid * grid = NULL;		
		if( unknown->QueryInterface( IID_IGrid, (void**)&grid) != S_OK )
		{	
			*retval = NULL;			
			lastErrorCode = tkINTERFACE_NOT_SUPPORTED;
			if( cBack != NULL )
				cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
			else if( globalCallback != NULL )
				globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
			
			gridSize = (int)allGrids.size(); //ah 6/6/05
			for( int c = 0; c < gridSize; c++ )
			{	
				allGrids[c]->Release();
				allGrids[c] = NULL;
			}
			return S_OK;
		}
		else
			allGrids.push_back( grid );		
	}

	if( allGrids.size() <= 0 )
	{	
		*retval = NULL;
		return S_OK;
	}

	gridSize = (int)allGrids.size();
	ProjectionTools * pt = new ProjectionTools();
	for( int i = 0; i < gridSize; i++ )
	{	
		char * currentComparingProj = NULL;
		if( i == 0 )
		{	
			IGridHeader * header = NULL;
			allGrids[i]->get_Header(&header);
			header->get_Projection(&final_projection);
			currentComparingProj = OLE2A(final_projection);
			header->Release();
		}
		else
		{
			BSTR bstrProj;
			IGridHeader * header = NULL;
			allGrids[i]->get_Header(&header);
			header->get_Projection(&bstrProj);
			char * nextProj = OLE2A(bstrProj);
			if (!pt->IsSameProjection(currentComparingProj, nextProj))
			{
				if( cBack != NULL )
					cBack->Error(A2BSTR("GridMerge"), A2BSTR("Warning: Projection mismatch on merged grids"));
				else if( globalCallback != NULL )
					globalCallback->Error(A2BSTR("GridMerge"), A2BSTR("Warning: Projection mismatch on merged grids"));
			}
			header->Release();
		}
	}

	delete pt;

	double final_xllcenter;
	double final_yllcenter;
	double final_xurcenter;
	double final_yurcenter;
	double final_dx = 1;
	double final_dy = 1;
	GridDataType final_dType = UnknownDataType;
	long final_ncols, final_nrows;

	double ind_xllcenter;
	double ind_yllcenter;
	double ind_xurcenter;
	double ind_yurcenter;
	double ind_dx = 1;
	double ind_dy = 1;
	GridDataType ind_dType = UnknownDataType;
	long ind_ncols, ind_nrows;
	VARIANT vndv;
	VariantInit(&vndv); //added by Rob Cairns 4-Jan-06
	double ndv;

	long percent = 0, cnt = 0;
	double total = 0.0;
	
	//Get the bounds and DataType for the final grid
	gridSize = (int)allGrids.size();
	for( int i = 0; i < gridSize; i++ )
	{	if( i == 0 )
		{	
			IGridHeader * header = NULL;
			allGrids[i]->get_Header(&header);
			header->get_dX(&final_dx);
			header->get_dY(&final_dy);
			header->get_XllCenter(&final_xllcenter);
			header->get_YllCenter(&final_yllcenter);
			header->get_NodataValue(&vndv);
			dVal(vndv,ndv);
			
			header->get_NumberCols(&final_ncols);
			header->get_NumberRows(&final_nrows);

			final_xurcenter = final_xllcenter + (final_ncols-1)*final_dx;
			final_yurcenter = final_yllcenter + (final_nrows-1)*final_dy;
			total = final_ncols*final_nrows;
			header->Release();
			allGrids[i]->get_DataType(&final_dType);
		}
		else
		{	
			IGridHeader * header = NULL;
			allGrids[i]->get_Header(&header);
			header->get_dX(&ind_dx);
			header->get_dY(&ind_dy);
			header->get_XllCenter(&ind_xllcenter);
			header->get_YllCenter(&ind_yllcenter);
			
			header->get_NumberCols(&ind_ncols);
			header->get_NumberRows(&ind_nrows);

			ind_xurcenter = ind_xllcenter + (ind_ncols-1)*ind_dx;
			ind_yurcenter = ind_yllcenter + (ind_nrows-1)*ind_dy;
			header->Release();
			allGrids[i]->get_DataType(&ind_dType);

			if( ind_dType > final_dType && ind_dType != UnknownDataType )
				final_dType = ind_dType;

			if( ind_dx != final_dx )
			{	
				lastErrorCode = tkINCOMPATIBLE_DX;
				*retval = NULL;
				if( cBack != NULL )
					cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
				else if( globalCallback != NULL )
					globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
				
				gridSize = (int)allGrids.size();
				for( int c = 0; c < gridSize; c++ )
				{	allGrids[c]->Release();
					allGrids[c] = NULL;
				}
				VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
				return S_OK;				
			}
			else if( ind_dy != final_dy )
			{	lastErrorCode = tkINCOMPATIBLE_DY;
				*retval = NULL;
				if( cBack != NULL )
					cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
				else if( globalCallback != NULL )
					globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
				
				gridSize = (int)allGrids.size();
				for( int c = 0; c < gridSize; c++ )
				{	allGrids[c]->Release();
					allGrids[c] = NULL;
				}
				VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
				return S_OK;	
			}
	
			if( ind_xllcenter < final_xllcenter )
				final_xllcenter = ind_xllcenter;
			if( ind_yllcenter < final_yllcenter )
				final_yllcenter = ind_yllcenter;

			if( ind_xurcenter > final_xurcenter )
				final_xurcenter = ind_xurcenter;
			if( ind_yurcenter > final_yurcenter )
				final_yurcenter = ind_yurcenter;
			total += ind_ncols*ind_nrows;
		}
	}

	if( final_dType == UnknownDataType || final_dType == InvalidDataType )
	{	
		lastErrorCode = tkINVALID_DATA_TYPE;
		*retval = NULL;
		if( cBack != NULL )
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		else if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));

		gridSize = (int)allGrids.size();
		
		for( int c = 0; c < gridSize; c++ )
		{	allGrids[c]->Release();
			allGrids[c] = NULL;
		}
		VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
		return S_OK;
	}

	if( total <= 0 )
	{	
		*retval = NULL;
		gridSize = (int)allGrids.size();
		
		for( int c = 0; c < gridSize; c++ )
		{
			allGrids[c]->Release();
			allGrids[c] = NULL;
		}
		VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
		return S_OK;
	}

	IGridHeader * final_header = NULL;
	CoCreateInstance( CLSID_GridHeader, NULL, CLSCTX_INPROC_SERVER, IID_IGridHeader, (void**)&final_header );
	final_header->put_NumberCols( (long)((final_xurcenter - final_xllcenter)/final_dx)+1 );
	final_header->put_NumberRows( (long)((final_yurcenter - final_yllcenter)/final_dy)+1 );
	final_header->put_dX( final_dx );
	final_header->put_dY( final_dy );
	final_header->put_NodataValue( vndv );
	final_header->put_XllCenter( final_xllcenter );
	final_header->put_YllCenter( final_yllcenter );
	final_header->put_Projection(final_projection);
		
	VARIANT_BOOL vbretval = FALSE;
	CoCreateInstance(CLSID_Grid,NULL,CLSCTX_INPROC_SERVER,IID_IGrid,(void**)retval);
	(*retval)->CreateNew( MergeFilename, final_header, final_dType, vndv, InRam, GrdFileType, cBack, &vbretval ); 
	final_header->Release();

	if( vbretval == FALSE )
	{
		*retval = NULL;
		gridSize = (int)allGrids.size();
		for( int c = 0; c < gridSize; c++ )
		{	allGrids[c]->Release();
			allGrids[c] = NULL;
		}
		VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
		return S_OK;
	}

	//Merge the grids
	long xoffset = 0, yoffset = 0;
	double val, nodata_value;
	VARIANT vval;
	VariantInit(&vval); //added by Rob Cairns 4-Jan-06

	gridSize = (int)allGrids.size();
	for( int n = gridSize - 1; n >= 0; n-- )
	{	
		IGridHeader * header = NULL;
		allGrids[n]->get_Header(&header);
		header->get_XllCenter(&ind_xllcenter);
		header->get_YllCenter(&ind_yllcenter);		
		header->get_NumberCols(&ind_ncols);
		header->get_NumberRows(&ind_nrows);		
		header->get_NodataValue(&vndv);
		dVal(vndv,nodata_value);
		header->Release();

		(*retval)->ProjToCell(ind_xllcenter,ind_yllcenter+(ind_nrows-1)*final_dy,&xoffset,&yoffset);
		
		for( int j = 0; j < ind_nrows; j++ )
		{	
			for( int i = 0; i < ind_ncols; i++ )
			{	
				allGrids[n]->get_Value( i, j, &vval );
				dVal( vval, val );
			
				if( val != nodata_value && !(val < -2147483640 && nodata_value < -2147483640))
					(*retval)->put_Value( i + xoffset, j + yoffset, vval );				

				int newpercent = (int)((++cnt/total)*100);
				if( newpercent > percent )
				{	percent = newpercent;
					if( cBack != NULL )
						cBack->Progress(OLE2BSTR(key),percent,A2BSTR("GridMerge"));
					else if( globalCallback != NULL )
						globalCallback->Progress(OLE2BSTR(key),percent,A2BSTR("GridMerge"));
				}
			}
		}
	}	

	gridSize = (int)allGrids.size();
	for( int c = 0; c < gridSize; c++ )
	{	allGrids[c]->Release();
		allGrids[c] = NULL;
	}

	VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
	VariantClear(&vval); //added by Rob Cairns 4-Jan-06
	
	return S_OK;
}

STDMETHODIMP CUtils::ShapeMerge(IShapefile *Shapes, long IndexOne, long IndexTwo, ICallback *cBack, IShape **retval)
{
	//This function is being replaced by the GPC clip function either in the UTILS object or in 
	//a separate dll... 
	//dpa 6/3/05
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	AfxMessageBox("Not Implemented");

	return S_OK;
}

STDMETHODIMP CUtils::GridToImage(IGrid *Grid, IGridColorScheme *ci, ICallback *cBack, IImage ** retval)
{
	// Depending on the memory situation, call the appropriate
	// function. Don't do this test in one merged function,
	// because the number of "if"s performed in loops would be
	// detrimental to performance.

	IGridHeader * gridheader = NULL;
	Grid->get_Header(&gridheader);
	long ncols, nrows;
	gridheader->get_NumberCols(&ncols);
	gridheader->get_NumberRows(&nrows);
	gridheader->Release();

	//ofstream out("C:\\profile.txt", ios::app);
	//Init_Timer();
	//Start_Timer();
	
	// Required memory -- colums times rows times size of colour struct	
	// Check against increasing sizes to prevent double wraparound
	if (MemoryAvailable(ncols * nrows) && MemoryAvailable(ncols * nrows * 2) && MemoryAvailable(static_cast<long>((ncols * nrows * sizeof(colour)))))
	{
		bool b = (GridToImage_InRAM(Grid, ci, cBack, retval) == TRUE);
		//Stop_Timer();
		//Print_Timer(out,"GridToImage : RAM");
		return b;
	}
	else
	{
		bool b = (GridToImage_DiskBased(Grid, ci, cBack, retval) == TRUE);
		//Stop_Timer();
		//Print_Timer(out,"GridToImage : Disk");
		return b;
	}
}

STDMETHODIMP CUtils::GridToImage_DiskBased(IGrid *Grid, IGridColorScheme *ci, ICallback *cBack, IImage ** retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	
	if( Grid == NULL || ci == NULL )
	{	*retval = NULL;
		
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( cBack != NULL )
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
		else if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));

		return S_OK;
	}

	IGridHeader * gridheader = NULL;
	Grid->get_Header(&gridheader);

	long ncols, nrows;
	double xll, yll, nodata;
	double dx = 1.0, dy = 1.0;
	VARIANT vndv;
	VariantInit(&vndv); //added by Rob Cairns 4-Jan-06

	gridheader->get_NumberCols(&ncols);
	gridheader->get_NumberRows(&nrows);

	if( ncols <= 0 || nrows <= 0 )
	{	*retval = NULL;
		
		lastErrorCode = tkZERO_ROWS_OR_COLS;
		if( cBack != NULL )
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
		else if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		
		return S_OK;
	}

	gridheader->get_XllCenter(&xll);
	gridheader->get_YllCenter(&yll);
	gridheader->get_NodataValue(&vndv);
	dVal(vndv, nodata);	
	gridheader->get_dX(&dx);
	gridheader->get_dY(&dy);
	gridheader->Release();
	
	//Hard code csize, so that the vectors are normal
	double csize = 30;
	double val = nodata;
	VARIANT vval;
	VariantInit(&vval); //added by Rob Cairns 4-Jan-06
	long break_index = 0;

	//Hillshade code
	double leftPercent = 0.0;
	double rightPercent = 0.0;
	long cnt = 0;

	OLE_COLOR nodataColor;
	ci->get_NoDataColor(&nodataColor);

	int nodataColor_R = GetRValue(nodataColor);
	int nodataColor_G = GetGValue(nodataColor);
	int nodataColor_B = GetBValue(nodataColor);

	double ka = .7;
	double kd = .8;

	double ai = 0.0, li = 0.0;
	ci->get_AmbientIntensity(&ai);
	ci->get_LightSourceIntensity(&li);
	double lsi, lsj, lsk;					
	IVector * v = NULL;
	ci->GetLightSource(&v);
	v->get_i(&lsi);
	v->get_j(&lsj);
	v->get_k(&lsk);
	v->Release();
	cppVector lightsource(lsi,lsj,lsk);

	VARIANT_BOOL vbretval;
	BSTR bImageFile;
	Grid->get_Filename(&bImageFile);
	CString ImageFile = OLE2CA(bImageFile);
	int LocationOfPeriod = ImageFile.ReverseFind('.');
	ImageFile = ImageFile.Left(LocationOfPeriod);
	CString WorldFile = ImageFile;
	WorldFile += ".bpw";
	ImageFile += ".bmp";

	if (MemoryAvailable(ncols * (sizeof(_int32) * 3)))
		CanScanlineBuffer = true;
	else
		CanScanlineBuffer = false;

	CreateBitmap(ImageFile.GetBuffer(), ncols, nrows, &vbretval);
	
	std::deque<BreakVal> bvals;
	long numBreaks = 0;
	ci->get_NumBreaks(&numBreaks);
	double lowval, highval;
	for( int i = 0; i < numBreaks; i++ )
	{	
		IGridColorBreak * bi = NULL;
		ci->get_Break(i, &bi);
		bi->get_LowValue(&lowval);
		bi->get_HighValue(&highval);
		bi->Release();

		BreakVal bv;
		bv.lowVal = lowval;
		bv.highVal = highval;
		bvals.push_back( bv );
	}

	double total = nrows * ncols;
	long newpercent = 0, percent = 0;

	for( int j = nrows-1; j >= 0; j-- )
	{				
		for( int i = 0; i < ncols; i++ )
		{	
			newpercent = (long)((((nrows - j - 1)*ncols + i)/total)*100);
			if( newpercent > percent )
			{	
				percent = newpercent;
				if( cBack != NULL )
					cBack->Progress(OLE2BSTR(key),percent,A2BSTR("GridToImage"));		
				else if( globalCallback != NULL )
					globalCallback->Progress(OLE2BSTR(key),percent,A2BSTR("GridToImage"));				
			}

			Grid->get_Value( i, j, &vval );
			dVal( vval, val );
			
			//The value is nodata
			if( val == nodata )
			{	
				PutBitmapValue(i, j, nodataColor_R, nodataColor_G, nodataColor_B, ncols);
				continue;
			}

			//Find the break
			break_index = findBreak( bvals, val );

			//A break is not defined for this value
			if( break_index < 0 )
			{	
				PutBitmapValue(i, j, nodataColor_R, nodataColor_G, nodataColor_B, ncols);
				continue;
			}

			IGridColorBreak * bi = NULL;
			ci->get_Break( break_index, &bi );

			OLE_COLOR hiColor, lowColor;
			bi->get_HighColor(&hiColor);
			bi->get_LowColor(&lowColor);
			double hiVal, lowVal;
			bi->get_HighValue(&hiVal);
			bi->get_LowValue(&lowVal);
			double biRange = hiVal - lowVal;
			if( biRange <= 0.0 )
				biRange = 1.0;

			ColoringType colortype;
			bi->get_ColoringType(&colortype);
			GradientModel gradmodel;
			bi->get_GradientModel(&gradmodel);
			bi->Release();
			
			if( colortype == Hillshade )
			{
				double yone = 0, ytwo = 0, ythree = 0;
				VARIANT vyone, vytwo, vythree;
				VariantInit(&vyone); //added by Rob Cairns 4-Jan-06
				VariantInit(&vytwo); //added by Rob Cairns 4-Jan-06
				VariantInit(&vythree); //added by Rob Cairns 4-Jan-06

				//Cannot Compute Polygon ... Make the best guess
				if( i >= ncols - 1 || j <= 0 )
				{	
					if( i >= ncols - 1 && j <= 0)
					{	
						yone = val;
						ytwo = val;
						ythree = val;
					}
					else if( i >= ncols - 1 )
					{	Grid->get_Value( i - 1, j, &vyone );
						Grid->get_Value( i, j - 1, &vytwo );
						Grid->get_Value( i - 1, j - 1, &vythree );
						dVal(vyone,yone);
						dVal(vytwo,ytwo);
						dVal(vythree,ythree);
					}
					else if( j <= 0 )
					{	
						Grid->get_Value( i, j + 1, &vyone );
						Grid->get_Value( i + 1, j, &vytwo );						
						dVal(vyone,yone);
						dVal(vytwo,ytwo);						
						ythree = val;
					}
				}
				else
				{	
					yone = val;
					Grid->get_Value( i + 1, j - 1, &vytwo );
					Grid->get_Value( i, j - 1, &vythree );
					dVal(vytwo,ytwo);
					dVal(vythree,ythree);					
				}

				double xone = xll + csize*j;
				double xtwo = xone + csize;
				double xthree = xone;
				
				double zone = yll + csize*i;
				double ztwo = zone;
				double zthree = zone - csize;

				//check for nodata on triangle corners
				if( yone == nodata || ytwo == nodata || ythree == nodata )
				{	
					PutBitmapValue(i, j, nodataColor_R, nodataColor_G, nodataColor_B, ncols);
					continue;
				}
				else
				{	
					//Make Two Vectors
					cppVector one;
					one.seti( xone - xtwo );
					one.setj( yone - ytwo );
					one.setk( zone - ztwo );
					one.Normalize();
					cppVector two;
					two.seti( xone - xthree );
					two.setj( yone - ythree );
					two.setk( zone - zthree );
					two.Normalize();

					//Compute Normal
					cppVector normal = two.crossProduct( one );					

					//Compute I
					double I = ai*ka + li*kd*( lightsource.dot( normal ) );
					//Compute Gradient * I
					if( I > 1.0 )
						I = 1.0;

					//Two Color Gradient					
					//Linear
					if( gradmodel == Linear )
					{	rightPercent = ( ( val - lowVal ) / biRange );
						leftPercent = 1.0 - rightPercent;					
					}
					//Log
					else if( gradmodel == Logorithmic )
					{	
						double dLog = 0.0;
						double ht = val;
						if( ht < 1 )
							ht = 1.0;
						if( biRange > 1.0 && ht - lowVal > 1.0 )
						{	rightPercent = ( log( ht - lowVal)/log(biRange) );
							leftPercent = 1.0 - rightPercent;							
						}					
						else
						{	rightPercent = 0.0;
							leftPercent = 1.0;							
						}
							
					}
					//Exp
					else if( gradmodel == Exponential )
					{	
						double dLog = 0.0;
						double ht = val;
						if( ht < 1 )
							ht = 1.0;
						if( biRange > 1.0 )
						{	rightPercent = ( pow( ht - lowVal, 2)/pow(biRange, 2) );
							leftPercent = 1.0 - rightPercent;						
						}					
						else
						{	rightPercent = 0.0;
							leftPercent = 1.0;							
						}		
					}

					// The less declarations, the better.
					// Soak up any speed wherever possible

					//int finalColorR = (int)(((double)GetRValue(lowColor)*leftPercent + (double)GetRValue(hiColor)*rightPercent )*I) %256;
					//int finalColorG = (int)(((double)GetGValue(lowColor)*leftPercent + (double)GetGValue(hiColor)*rightPercent )*I) %256;
					//int finalColorB = (int)(((double)GetBValue(lowColor)*leftPercent + (double)GetBValue(hiColor)*rightPercent )*I) %256;
								
					PutBitmapValue(i, j, (int)(((double)GetRValue(lowColor)*leftPercent + (double)GetRValue(hiColor)*rightPercent )*I) %256, (int)(((double)GetGValue(lowColor)*leftPercent + (double)GetGValue(hiColor)*rightPercent )*I) %256, (int)(((double)GetBValue(lowColor)*leftPercent + (double)GetBValue(hiColor)*rightPercent )*I) %256, ncols);
				}
				
				VariantClear(&vyone); //added by Rob Cairns 4-Jan-06
				VariantClear(&vytwo); //added by Rob Cairns 4-Jan-06
				VariantClear(&vythree); //added by Rob Cairns 4-Jan-06

			}			
			else if( colortype == Gradient )
			{
				//Linear
				if( gradmodel == Linear )
				{	
					rightPercent = ( ( val - lowVal ) / biRange );
					leftPercent = 1.0 - rightPercent;			
				}
				//Log
				else if( gradmodel == Logorithmic )
				{	
					double dLog = 0.0;
					double ht = val;
					if( ht < 1 )
						ht = 1.0;
					if( biRange > 1.0 && ht - lowVal > 1.0 )
					{	rightPercent = ( log( ht - lowVal)/log(biRange) );
						leftPercent = 1.0 - rightPercent;					
					}					
					else
					{	
						rightPercent = 0.0;
						leftPercent = 1.0;						
					}						
				}
				//Exp
				else if( gradmodel == Exponential )
				{	
					double dLog = 0.0;
					double ht = val;
					if( ht < 1 )
						ht = 1.0;
					if( biRange > 1.0 )
					{
						rightPercent = ( pow( ht - lowVal, 2)/pow(biRange, 2) );
						leftPercent = 1.0 - rightPercent;
					}					
					else
					{
						rightPercent = 0.0;
						leftPercent = 1.0;						
					}		
				}

				// Less declaratinos to pass to inline function directly

				//int finalColorR = (int)((double)GetRValue(lowColor)*leftPercent + (double)GetRValue(hiColor)*rightPercent ) %256;
				//int finalColorG = (int)((double)GetGValue(lowColor)*leftPercent + (double)GetGValue(hiColor)*rightPercent ) %256;
				//int finalColorB = (int)((double)GetBValue(lowColor)*leftPercent + (double)GetBValue(hiColor)*rightPercent ) %256;
					
				PutBitmapValue(i, j, (int)((double)GetRValue(lowColor)*leftPercent + (double)GetRValue(hiColor)*rightPercent ) %256, (int)((double)GetGValue(lowColor)*leftPercent + (double)GetGValue(hiColor)*rightPercent ) %256, (int)((double)GetBValue(lowColor)*leftPercent + (double)GetBValue(hiColor)*rightPercent ) %256, ncols);

			}
			else if( colortype == Random )
				PutBitmapValue(i, j, GetRValue(lowColor), GetGValue(lowColor), GetBValue(lowColor), ncols);
		}		
	}
	
	FinalizeAndCloseBitmap(ncols);

	FILE* fout = fopen(WorldFile, "w");
	
	if( !fout )
	{	lastErrorCode = tkCANT_WRITE_WORLD_FILE;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)) );
		return VARIANT_FALSE;
	}
	
	fprintf(fout,"%.14f\n",dx);
	fprintf(fout,"%.14f\n",0.0);
	fprintf(fout,"%.14f\n",0.0);
	fprintf(fout,"%.14f\n",dy*-1.0);
	
	//convert lower left to upper left pixel
	double xupLeft = xll;
	double yupLeft = yll + ( dy*(nrows-1));
	
	fprintf(fout,"%.14f\n",xupLeft);
	fprintf(fout,"%.14f\n",yupLeft);
	
	fprintf(fout,"%s\n","[tkImageCom]");
	fprintf(fout,"%s %s\n","ImageFile",ImageFile);
	fflush(fout);
	fclose(fout);
	
	
	if (rasterDataset != NULL)
	{
		rasterDataset->FlushCache();
		GDALClose(rasterDataset);
		rasterDataset=NULL;
	}

	CoCreateInstance(CLSID_Image,NULL,CLSCTX_INPROC_SERVER,IID_IImage,(void**)retval);

	// Now open the retval bitmap so that life can proceed.
	VARIANT_BOOL openRetval;
	(*retval)->Open(A2BSTR(ImageFile.GetBuffer()), BITMAP_FILE, false, NULL, &openRetval);

	VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
	VariantClear(&vval); //added by Rob Cairns 4-Jan-06

	if (openRetval == VARIANT_TRUE)
	{
		return true;
	}
	else
		return false;
}

STDMETHODIMP CUtils::GridToImage_InRAM(IGrid *Grid, IGridColorScheme *ci, ICallback *cBack, IImage ** retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	
	if( Grid == NULL || ci == NULL )
	{	*retval = NULL;
		
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( cBack != NULL )
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
		else if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));

		return S_OK;
	}

	IGridHeader * gridheader = NULL;
	Grid->get_Header(&gridheader);

	long ncols, nrows;
	double xll, yll, nodata;
	double dx = 1.0, dy = 1.0;
	VARIANT vndv;
	VariantInit(&vndv); //added by Rob Cairns 4-Jan-06

	gridheader->get_NumberCols(&ncols);
	gridheader->get_NumberRows(&nrows);

	if( ncols <= 0 || nrows <= 0 )
	{	*retval = NULL;
		
		lastErrorCode = tkZERO_ROWS_OR_COLS;
		if( cBack != NULL )
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
		else if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		
		return S_OK;
	}

	gridheader->get_XllCenter(&xll);
	gridheader->get_YllCenter(&yll);
	gridheader->get_NodataValue(&vndv);
	dVal(vndv, nodata);	
	gridheader->get_dX(&dx);
	gridheader->get_dY(&dy);
	gridheader->Release();
	
	//Hard code csize, so that the vectors are normal
	double csize = 30;
	double val = nodata;
	VARIANT vval;
	VariantInit(&vval); //added by Rob Cairns 4-Jan-06
	long break_index = 0;

	//Hillshade code
	double leftPercent = 0.0;
	double rightPercent = 0.0;
	long cnt = 0;

	OLE_COLOR nodataColor;
	ci->get_NoDataColor(&nodataColor);

	double ka = .7;
	double kd = .8;

	double ai = 0.0, li = 0.0;
	ci->get_AmbientIntensity(&ai);
	ci->get_LightSourceIntensity(&li);
	double lsi, lsj, lsk;					
	IVector * v = NULL;
	ci->GetLightSource(&v);
	v->get_i(&lsi);
	v->get_j(&lsj);
	v->get_k(&lsk);
	v->Release();
	cppVector lightsource(lsi,lsj,lsk);

	CoCreateInstance(CLSID_Image,NULL,CLSCTX_INPROC_SERVER,IID_IImage,(void**)retval);
	VARIANT_BOOL vbretval;
	(*retval)->CreateNew( ncols, nrows, &vbretval );
	
	std::deque<BreakVal> bvals;
	long numBreaks = 0;
	ci->get_NumBreaks(&numBreaks);
	double lowval, highval;
	for( int i = 0; i < numBreaks; i++ )
	{	
		IGridColorBreak * bi = NULL;
		ci->get_Break(i, &bi);
		bi->get_LowValue(&lowval);
		bi->get_HighValue(&highval);
		bi->Release();

		BreakVal bv;
		bv.lowVal = lowval;
		bv.highVal = highval;
		bvals.push_back( bv );
	}

	double total = nrows * ncols;
	long newpercent = 0, percent = 0;

	for( int j = nrows-1; j >= 0; j-- )
	{				
		for( int i = 0; i < ncols; i++ )
		{	
			newpercent = (long)((((nrows - j - 1)*ncols + i)/total)*100);
			if( newpercent > percent )
			{	
				percent = newpercent;
				if( cBack != NULL )
					cBack->Progress(OLE2BSTR(key),percent,A2BSTR("GridToImage"));		
				else if( globalCallback != NULL )
					globalCallback->Progress(OLE2BSTR(key),percent,A2BSTR("GridToImage"));				
			}

			Grid->get_Value( i, j, &vval );
			dVal( vval, val );
			
			//The value is nodata
			if( val == nodata )
			{	
				(*retval)->put_Value( j, i, nodataColor );
				continue;
			}

			//Find the break
			break_index = findBreak( bvals, val );

			//A break is not defined for this value
			if( break_index < 0 )
			{	(*retval)->put_Value( j, i, nodataColor );
				continue;
			}

			IGridColorBreak * bi = NULL;
			ci->get_Break( break_index, &bi );

			OLE_COLOR hiColor, lowColor;
			bi->get_HighColor(&hiColor);
			bi->get_LowColor(&lowColor);
			double hiVal, lowVal;
			bi->get_HighValue(&hiVal);
			bi->get_LowValue(&lowVal);
			double biRange = hiVal - lowVal;
			if( biRange <= 0.0 )
				biRange = 1.0;

			ColoringType colortype;
			bi->get_ColoringType(&colortype);
			GradientModel gradmodel;
			bi->get_GradientModel(&gradmodel);
			bi->Release();
			
			if( colortype == Hillshade )
			{
				double yone = 0, ytwo = 0, ythree = 0;
				VARIANT vyone, vytwo, vythree;
				VariantInit(&vyone); //added by Rob Cairns 4-Jan-06
				VariantInit(&vytwo); //added by Rob Cairns 4-Jan-06
				VariantInit(&vythree); //added by Rob Cairns 4-Jan-06

				//Cannot Compute Polygon ... Make the best guess
				if( i >= ncols - 1 || j <= 0 )
				{	
					if( i >= ncols - 1 && j <= 0)
					{	
						yone = val;
						ytwo = val;
						ythree = val;
					}
					else if( i >= ncols - 1 )
					{	Grid->get_Value( i - 1, j, &vyone );
						Grid->get_Value( i, j - 1, &vytwo );
						Grid->get_Value( i - 1, j - 1, &vythree );
						dVal(vyone,yone);
						dVal(vytwo,ytwo);
						dVal(vythree,ythree);
					}
					else if( j <= 0 )
					{	
						Grid->get_Value( i, j + 1, &vyone );
						Grid->get_Value( i + 1, j, &vytwo );						
						dVal(vyone,yone);
						dVal(vytwo,ytwo);						
						ythree = val;
					}
				}
				else
				{	
					yone = val;
					Grid->get_Value( i + 1, j - 1, &vytwo );
					Grid->get_Value( i, j - 1, &vythree );
					dVal(vytwo,ytwo);
					dVal(vythree,ythree);					
				}

				double xone = xll + csize*j;
				double xtwo = xone + csize;
				double xthree = xone;
				
				double zone = yll + csize*i;
				double ztwo = zone;
				double zthree = zone - csize;

				//check for nodata on triangle corners
				if( yone == nodata || ytwo == nodata || ythree == nodata )
				{	
					(*retval)->put_Value( j, i, nodataColor );
					continue;
				}
				else
				{	
					//Make Two Vectors
					cppVector one;
					one.seti( xone - xtwo );
					one.setj( yone - ytwo );
					one.setk( zone - ztwo );
					one.Normalize();
					cppVector two;
					two.seti( xone - xthree );
					two.setj( yone - ythree );
					two.setk( zone - zthree );
					two.Normalize();

					//Compute Normal
					cppVector normal = two.crossProduct( one );					

					//Compute I
					double I = ai*ka + li*kd*( lightsource.dot( normal ) );
					//Compute Gradient * I
					if( I > 1.0 )
						I = 1.0;

					//Two Color Gradient					
					//Linear
					if( gradmodel == Linear )
					{	rightPercent = ( ( val - lowVal ) / biRange );
						leftPercent = 1.0 - rightPercent;					
					}
					//Log
					else if( gradmodel == Logorithmic )
					{	
						double dLog = 0.0;
						double ht = val;
						if( ht < 1 )
							ht = 1.0;
						if( biRange > 1.0 && ht - lowVal > 1.0 )
						{	rightPercent = ( log( ht - lowVal)/log(biRange) );
							leftPercent = 1.0 - rightPercent;							
						}					
						else
						{	rightPercent = 0.0;
							leftPercent = 1.0;							
						}
							
					}
					//Exp
					else if( gradmodel == Exponential )
					{	
						double dLog = 0.0;
						double ht = val;
						if( ht < 1 )
							ht = 1.0;
						if( biRange > 1.0 )
						{	rightPercent = ( pow( ht - lowVal, 2)/pow(biRange, 2) );
							leftPercent = 1.0 - rightPercent;						
						}					
						else
						{	rightPercent = 0.0;
							leftPercent = 1.0;							
						}		
					}

					int finalColorR = (int)(((double)GetRValue(lowColor)*leftPercent + (double)GetRValue(hiColor)*rightPercent )*I) %256;
					int finalColorG = (int)(((double)GetGValue(lowColor)*leftPercent + (double)GetGValue(hiColor)*rightPercent )*I) %256;
					int finalColorB = (int)(((double)GetBValue(lowColor)*leftPercent + (double)GetBValue(hiColor)*rightPercent )*I) %256;
					
					(*retval)->put_Value( j, i, RGB(finalColorR, finalColorG, finalColorB) );
									
				}
				
				VariantClear(&vyone); //added by Rob Cairns 4-Jan-06
				VariantClear(&vytwo); //added by Rob Cairns 4-Jan-06
				VariantClear(&vythree); //added by Rob Cairns 4-Jan-06

			}			
			else if( colortype == Gradient )
			{
				//Linear
				if( gradmodel == Linear )
				{	
					rightPercent = ( ( val - lowVal ) / biRange );
					leftPercent = 1.0 - rightPercent;			
				}
				//Log
				else if( gradmodel == Logorithmic )
				{	
					double dLog = 0.0;
					double ht = val;
					if( ht < 1 )
						ht = 1.0;
					if( biRange > 1.0 && ht - lowVal > 1.0 )
					{	rightPercent = ( log( ht - lowVal)/log(biRange) );
						leftPercent = 1.0 - rightPercent;					
					}					
					else
					{	
						rightPercent = 0.0;
						leftPercent = 1.0;						
					}						
				}
				//Exp
				else if( gradmodel == Exponential )
				{	
					double dLog = 0.0;
					double ht = val;
					if( ht < 1 )
						ht = 1.0;
					if( biRange > 1.0 )
					{
						rightPercent = ( pow( ht - lowVal, 2)/pow(biRange, 2) );
						leftPercent = 1.0 - rightPercent;
					}					
					else
					{
						rightPercent = 0.0;
						leftPercent = 1.0;						
					}		
				}

				int finalColorR = (int)((double)GetRValue(lowColor)*leftPercent + (double)GetRValue(hiColor)*rightPercent ) %256;
				int finalColorG = (int)((double)GetGValue(lowColor)*leftPercent + (double)GetGValue(hiColor)*rightPercent ) %256;
				int finalColorB = (int)((double)GetBValue(lowColor)*leftPercent + (double)GetBValue(hiColor)*rightPercent ) %256;
					
				(*retval)->put_Value( j, i, RGB(finalColorR, finalColorG, finalColorB) );
			}
			else if( colortype == Random )
				(*retval)->put_Value( j, i, lowColor );			
		}		
	}
		
	(*retval)->put_XllCenter(xll);
	(*retval)->put_YllCenter(yll);
	(*retval)->put_dX(dx);
	(*retval)->put_dY(dy);
	
	VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
	VariantClear(&vval); //added by Rob Cairns 4-Jan-06
	
	return S_OK;
}


inline long CUtils::findBreak( std::deque<BreakVal> & bVals, double val )
{

	int sizeBVals = (int)bVals.size();
	for( int i = 0; i < sizeBVals; i++ )
	{	
		if( val >= bVals[i].lowVal &&
			val <= bVals[i].highVal )
			return i;
	}

	return -1;
}

STDMETHODIMP CUtils::GridToGrid(IGrid *Grid, GridDataType OutDataType, ICallback *cBack, IGrid **retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	if( Grid == NULL )
	{	
		*retval = NULL;
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( cBack != NULL )
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		else if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}

	long ncols = 0, nrows = 0;

	IGridHeader * header = NULL;
	Grid->get_Header(&header);
	header->get_NumberCols(&ncols);
	header->get_NumberRows(&nrows);

	if( ncols <= 0 || nrows <= 0 )
	{	
		*retval = NULL;
		lastErrorCode = tkZERO_ROWS_OR_COLS;
		if( cBack != NULL )
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		else if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}

	VARIANT vndv;
	VariantInit(&vndv); //added by Rob Cairns 4-Jan-06
	header->get_NodataValue(&vndv);

	CoCreateInstance(CLSID_Grid,NULL,CLSCTX_INPROC_SERVER,IID_IGrid,(void**)retval);
	VARIANT_BOOL vbretval;
	(*retval)->CreateNew(A2BSTR(""),header,OutDataType,vndv,TRUE,UseExtension,cBack,&vbretval);
	header->Release();

	VARIANT val;
	VariantInit(&val); //added by Rob Cairns 4-Jan-06
	long percent = 0, newpercent = 0;
	double total = nrows*ncols;
	
	for( int j = 0; j < nrows; j++ )
	{	for( int i = 0; i < ncols; i++ )
		{	
			Grid->get_Value(i,j,&val);
			(*retval)->put_Value(i,j,val);	
		}
		newpercent = (long)((j / nrows)*100);
		if( newpercent > percent )
		{	
			percent = newpercent;
			if( cBack != NULL )
				cBack->Progress(OLE2BSTR(key), percent, A2BSTR("Grid2Grid"));
			else if( globalCallback != NULL )
				globalCallback->Progress(OLE2BSTR(key), percent, A2BSTR("Grid2Grid"));
		}
	}	

	VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
	VariantClear(&val); //added by Rob Cairns 4-Jan-06

	return S_OK;
}

STDMETHODIMP CUtils::ShapeToShapeZ(IShapefile * Shapefile, IGrid *Grid, ICallback *cBack, IShapefile **retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	if( Shapefile == NULL || Grid == NULL )
	{	
		*retval = NULL;
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}
	
	long ncols = 0, nrows = 0;
	IGridHeader * header = NULL;
	Grid->get_Header(&header);
	header->get_NumberCols(&ncols);
	header->get_NumberRows(&nrows);
	header->Release();

	double left=0, bottom=0, right=0, top=0;
	Grid->CellToProj(0,0,&left,&bottom);
	Grid->CellToProj(ncols-1,nrows-1,&right,&top);

	IExtents * box = NULL;
	Shapefile->get_Extents(&box);
	double s_left=0,s_bottom=0,s_right=0,s_top=0,nval;
	box->GetBounds(&s_left,&s_bottom,&nval,&s_right,&s_top,&nval);
	box->Release();

	//Check the bounds
	if( s_left < left || s_right > right || s_bottom < bottom || s_top < top )
	{	*retval = NULL;
		lastErrorCode = tkSHAPEFILE_LARGER_THAN_GRID;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}

	VARIANT_BOOL vbretval;
	ShpfileType shapetype = SHP_NULLSHAPE;
	Shapefile->get_ShapefileType(&shapetype);
	CoCreateInstance(CLSID_Shapefile,NULL,CLSCTX_INPROC_SERVER,IID_IShapefile,(void**)retval);
	(*retval)->CreateNew(A2BSTR(""),shapetype,&vbretval);
	
	long numFields = 0;
	Shapefile->get_NumFields(&numFields);
	for( int i = 0; i < numFields; i++ )
	{	
		IField * field = NULL;
		Shapefile->get_Field(i,&field);
		long fpos = i;
		(*retval)->EditInsertField(field,&fpos,cBack,&vbretval);
		field->Release();
	}

	long numShapes = 0;
	Shapefile->get_NumShapes(&numShapes);
	for( int j = 0; j < numShapes; j++ )
	{	
		IShape * shape = NULL;
		Shapefile->get_Shape(j,&shape);
		
		long numPoints = 0;
		shape->get_NumPoints(&numPoints);
		int p = 0;
		for( p = 0; p < numPoints; p++ )
		{	IPoint * point = NULL;
			shape->get_Point(p,&point);
			long col = 0, row = 0;
			double x = 0, y = 0;
			point->get_X(&x);
			point->get_Y(&y);
			Grid->ProjToCell(x,y,&col,&row);
			VARIANT val;
			VariantInit(&val); //added by Rob Cairns 4-Jan-06
			Grid->get_Value(col,row,&val);
			double z = 0;
			dVal(val,z);
			point->put_Z(z);			
			point->Release();
			VariantClear(&val); //added by Rob Cairns 4-Jan-06
		}
		long spos = p;
		(*retval)->EditInsertShape(shape,&spos,&vbretval);
		shape->Release();

		VARIANT cval;
		VariantInit(&cval); //added by Rob Cairns 4-Jan-06
		for( int k = 0; k < numFields; k++ )
		{	
			Shapefile->get_CellValue(k,j,&cval);
			(*retval)->EditCellValue(k,j,cval,&vbretval);
		}
		VariantClear(&cval); //added by Rob Cairns 4-Jan-06
	}

	return S_OK;
}

STDMETHODIMP CUtils::TinToShapefile(ITin *Tin, ShpfileType Type, ICallback *cBack, IShapefile **retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	if( Tin == NULL )
	{	
		*retval = NULL;
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}

	long numTriangles = 0;
	Tin->get_NumTriangles(&numTriangles);
	long numVertices = 0;
	Tin->get_NumVertices(&numVertices);
	long percent = 0, newpercent = 0;
	double total = numTriangles;

	CoCreateInstance(CLSID_Shapefile,NULL,CLSCTX_INPROC_SERVER,IID_IShapefile,(void**)retval);

	if( Type == SHP_POLYGON || Type == SHP_POLYGONZ )
	{	
		//Create shapefile
		VARIANT_BOOL vbretval;
		(*retval)->CreateNew(A2BSTR(""),Type,&vbretval);
		
		long pos = 0;
		IField * field = NULL;
		CoCreateInstance(CLSID_Field,NULL,CLSCTX_INPROC_SERVER,IID_IField,(void**)&field);
		field->put_Name(A2BSTR("Tin"));
		field->put_Type(INTEGER_FIELD);
		field->put_Width(5);
		(*retval)->EditInsertField(field,&pos,cBack,&vbretval);
		field->Release();
		field = NULL;		
		
		long vtx1, vtx2, vtx3;
		double x1, y1, z1;
		double x2, y2, z2;
		double x3, y3, z3;

		for( long i = 0; i < numTriangles; i++ )
		{			
			Tin->Triangle(i,&vtx1,&vtx2,&vtx3);
			Tin->Vertex(vtx1,&x1,&y1,&z1);
			Tin->Vertex(vtx2,&x3,&y3,&z3);
			Tin->Vertex(vtx3,&x2,&y2,&z2);
				
			IShape * shape = NULL;
			CoCreateInstance(CLSID_Shape,NULL,CLSCTX_INPROC_SERVER,IID_IShape,(void**)&shape);
			shape->Create(Type,&vbretval);

			pos = 0;
			shape->InsertPart(0,&pos,&vbretval);

			IPoint * point = NULL;
			CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&point);
			point->put_X(x1);
			point->put_Y(y1);
			point->put_Z(z1);
			pos = 0;
			shape->InsertPoint(point,&pos,&vbretval);
			point->Release();
			point = NULL;

			CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&point);
			point->put_X(x2);
			point->put_Y(y2);
			point->put_Z(z2);
			pos = 1;
			shape->InsertPoint(point,&pos,&vbretval);
			point->Release();
			point = NULL;
						
			CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&point);
			point->put_X(x3);
			point->put_Y(y3);
			point->put_Z(z3);
			pos = 2;
			shape->InsertPoint(point,&pos,&vbretval);
			point->Release();
			point = NULL;

			CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&point);
			point->put_X(x1);
			point->put_Y(y1);
			point->put_Z(z1);
			pos = 3;
			shape->InsertPoint(point,&pos,&vbretval);
			point->Release();
			point = NULL;
			
			pos = i;
			(*retval)->EditInsertShape(shape,&pos,&vbretval);
			shape->Release();
			shape = NULL;

			VARIANT val;
			VariantInit(&val); //added by Rob Cairns 4-Jan-06
			val.vt = VT_I4;
			val.lVal = i;
			(*retval)->EditCellValue(0,pos,val,&vbretval);
			
			newpercent = (long)((i/total)*100);
			if( newpercent > percent )
			{	percent = newpercent;
				if( cBack != NULL )
					cBack->Progress(OLE2BSTR(key), percent, A2BSTR("TinToShapefile"));
				else if( globalCallback != NULL )
					globalCallback->Progress(OLE2BSTR(key), percent, A2BSTR("TinToShapefile"));
			}

			VariantClear(&val); //added by Rob Cairns 4-Jan-06

		}		
	}
	else if( Type == SHP_POINT || Type == SHP_POINTZ )
	{	
		//Create shapefile
		VARIANT_BOOL vbretval;
		(*retval)->CreateNew(A2BSTR(""),Type,&vbretval);
		
		long pos = 0;
		IField * field = NULL;
		CoCreateInstance(CLSID_Field,NULL,CLSCTX_INPROC_SERVER,IID_IField,(void**)&field);
		field->put_Name(A2BSTR("Vertex"));
		field->put_Type(INTEGER_FIELD);
		field->put_Width(5);
		(*retval)->EditInsertField(field,&pos,cBack,&vbretval);
		field->Release();
		field = NULL;		
		
		double x1, y1, z1;
		
		for( long i = 0; i < numVertices; i++ )
		{	
			Tin->Vertex(i,&x1,&y1,&z1);
				
			IShape * shape = NULL;
			CoCreateInstance(CLSID_Shape,NULL,CLSCTX_INPROC_SERVER,IID_IShape,(void**)&shape);
			shape->Create(Type,&vbretval);

			IPoint * point = NULL;
			CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&point);
			point->put_X(x1);
			point->put_Y(y1);
			point->put_Z(z1);
			pos = 0;
			shape->InsertPoint(point,&pos,&vbretval);
			point->Release();
			point = NULL;

			(*retval)->EditInsertShape(shape,&i,&vbretval);
			shape->Release();
			shape = NULL;

			VARIANT val;
			VariantInit(&val); //added by Rob Cairns 4-Jan-06
			val.vt = VT_I4;
			val.lVal = i;
			(*retval)->EditCellValue(0,i,val,&vbretval);
			
			newpercent = (long)((i/total)*100);
			if( newpercent > percent )
			{	
				percent = newpercent;
				if( cBack != NULL )
					cBack->Progress(OLE2BSTR(key), percent, A2BSTR("TinToShapefile"));
				else if( globalCallback != NULL )
					globalCallback->Progress(OLE2BSTR(key), percent, A2BSTR("TinToShapefile"));
			}
	
			VariantClear(&val); //added by Rob Cairns 4-Jan-06

		}
	}

	return S_OK;
}


///\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
///\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
//			POLYGONAL ALGORITHM
///\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
///\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/

# ifndef GRID_TO_SHAPES
# define GRID_TO_SHAPES

# define BORDER             -2225
# define DOUBLE_BORDER      -2226
# define DECISION           -2227
# define TRACE_BORDER       -2228
# define CURRENT_POLYGON	-2229

	enum DIRECTION
	{ NONE,
	  RIGHT,
	  UPRIGHT,
	  UP,
	  UPLEFT,
	  LEFT,
	  DOWNLEFT,
	  DOWN,
	  DOWNRIGHT
	};

# endif

STDMETHODIMP CUtils::GridToShapefile(IGrid *Grid, IGrid *ConnectionGrid, ICallback *cBack, IShapefile **retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	if( Grid == NULL )
	{	*retval = NULL;
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( cBack != NULL )
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		else if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}

	//Create a grid that is twice the size of the original
	IGridHeader * header = NULL;
	Grid->get_Header(&header);
	long cols = 0, rows = 0;
	header->get_NumberCols(&cols);
	header->get_NumberRows(&rows);
	
	GridDataType dType = UnknownDataType;
	Grid->get_DataType(&dType);

	double dx, dy, xllCenter, yllCenter;

	VARIANT vndv;
	VariantInit(&vndv); //added by Rob Cairns 4-Jan-06
	header->get_NodataValue(&vndv);
	header->get_dX(&dx);
	header->get_dY(&dy);
	header->get_XllCenter(&xllCenter);
	header->get_YllCenter(&yllCenter);
	header->Release();
	header = NULL;

	if( cols <= 0 || rows <= 0 )
	{	*retval = NULL;
		lastErrorCode = tkZERO_ROWS_OR_COLS;
		if( cBack != NULL )
			cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));	
		else if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
		return S_OK;
	}
	
	long exp_cols = 2*cols + 1;
	long exp_rows = 2*rows + 1;

	long percent = 0, newpercent = 0;	
	double polygon_id = 0;
	double nodata_value = -1;
	dVal(vndv,nodata_value);

	double expand_nodata_value = nodata_value;
	double total = rows;
	
	VARIANT_BOOL vbretval = FALSE;
	
	CoCreateInstance(CLSID_Grid,NULL,CLSCTX_INPROC_SERVER,IID_IGrid,(void**)&expand_grid);
	IGridHeader * expand_header = NULL;
	CoCreateInstance(CLSID_GridHeader,NULL,CLSCTX_INPROC_SERVER,IID_IGridHeader,(void**)&expand_header);
	expand_header->put_dX( dx*.5 );
	expand_header->put_dY( dy*.5 );
	expand_header->put_XllCenter( xllCenter - dx*.5 );
	expand_header->put_YllCenter( yllCenter - dy*.5 );
	expand_header->put_NodataValue( vndv );
	expand_header->put_NumberCols( exp_cols );
	expand_header->put_NumberRows( exp_rows );
	
	char * tmpgname = new char[512];
	char * tmpfname = new char[512];
	char * tmppath = new char[260 + 512 + 1];
	_getcwd(tmppath,260);
	tmpnam(tmpfname);
	sprintf(tmpgname, "%s.bgd", tmpfname);

	expand_grid->CreateNew(A2BSTR(tmpgname),expand_header,dType,vndv,VARIANT_TRUE,UseExtension,cBack,&vbretval);

	expand_header->Release();
	expand_header = NULL;

	connection_grid = ConnectionGrid;
	
	double expand_value = 0;
	for( int y = rows - 1; y>= 0; y-- )
	{	for( int x = 0; x < cols; x++ )
		{	
			polygon_id = getValue( Grid, x, y );

			//Expand as the center cell
			int expand_x = 2*x + 1;
			int expand_y = 2*y + 1;

			//Expanded Cell arrangement
			//4 3 2
			//5 0 1
			//6 7 8

			//Mark the cell as a border if it already has a value
			//Cell 0
			expand_value = getValue( expand_grid, expand_x, expand_y );
			//Set the polygon_id
			if( expand_value == expand_nodata_value )
				setValue( expand_grid, expand_x,     expand_y,     polygon_id );
			
			//Cell 1
			expand_value = getValue( expand_grid, expand_x + 1, expand_y );
			//set the polygon id
			if( expand_value == expand_nodata_value )
				setValue( expand_grid, expand_x + 1, expand_y,     polygon_id );
			//polygon_id already exists and is the same... reset it
			else if( expand_value == polygon_id )
				setValue( expand_grid, expand_x + 1, expand_y,     polygon_id );										
			//set the cell as a border
			else if( expand_value >= 0 )
				setValue(expand_grid,  expand_x + 1, expand_y,     BORDER );
			
			//Cell 2
			expand_value = getValue( expand_grid, expand_x + 1, expand_y + 1 );
			if( expand_value == expand_nodata_value )
				setValue( expand_grid, expand_x + 1, expand_y + 1, polygon_id );
			else if( expand_value == polygon_id )
				setValue( expand_grid, expand_x + 1, expand_y + 1, polygon_id );									
			else if( expand_value >= 0 )
				setValue( expand_grid, expand_x + 1, expand_y + 1, BORDER );
			else if( expand_value == BORDER )
			{	expand_value -= 1;
				setValue( expand_grid, expand_x + 1, expand_y + 1,		expand_value );
			}
			else if( expand_value == DOUBLE_BORDER )
			{	if( is_decision( expand_grid, expand_x + 1, expand_y + 1 ) )
					setValue( expand_grid, expand_x + 1, expand_y + 1, DECISION );	
				else
					setValue( expand_grid, expand_x + 1, expand_y + 1, BORDER );	
			}

			//Cell 3
			expand_value = getValue( expand_grid, expand_x, expand_y + 1 );
			//set the polygon id
			if( expand_value == expand_nodata_value )
				setValue( expand_grid, expand_x,     expand_y + 1, polygon_id );
			//polygon_id already exists and is the same... reset it
			else if( expand_value == polygon_id )
				setValue( expand_grid, expand_x,     expand_y + 1, polygon_id );							
			//set the cell as a border
			else if( expand_value >= 0 )
				setValue( expand_grid, expand_x,     expand_y + 1, BORDER );
			
			//Cell 4
			expand_value = getValue( expand_grid, expand_x - 1, expand_y + 1 );
			if( expand_value == expand_nodata_value )
				setValue( expand_grid, expand_x - 1, expand_y + 1, polygon_id );
			else if( expand_value == polygon_id )
				setValue( expand_grid, expand_x - 1, expand_y + 1, polygon_id );				
			else if( expand_value >= 0 )
				setValue( expand_grid, expand_x - 1, expand_y + 1, BORDER );
			else if( expand_value == BORDER )
			{	expand_value -= 1;
				setValue( expand_grid, expand_x - 1, expand_y + 1,	expand_value );
			}
			else if( expand_value == DOUBLE_BORDER )
			{	if( is_decision( expand_grid, expand_x - 1, expand_y + 1 ) )
					setValue( expand_grid, expand_x - 1, expand_y + 1, DECISION );	
				else
					setValue( expand_grid, expand_x - 1, expand_y + 1, BORDER );	
			}

			//Cell 5
			expand_value = getValue( expand_grid, expand_x - 1, expand_y );
			//set the polygon id
			if( expand_value == expand_nodata_value )
				setValue( expand_grid, expand_x - 1, expand_y, polygon_id );
			//polygon_id already exists and is the same... reset it
			else if( expand_value == polygon_id )
				setValue( expand_grid, expand_x - 1, expand_y, polygon_id );						
			//set the cell as a border
			else if( expand_value >= 0 )
				setValue( expand_grid, expand_x - 1, expand_y, BORDER );
			
			//Cell 6
			expand_value = getValue( expand_grid, expand_x - 1, expand_y - 1 );
			if( expand_value == expand_nodata_value )
				setValue( expand_grid, expand_x - 1,     expand_y - 1, polygon_id );
			else if( expand_value == polygon_id )
				setValue( expand_grid, expand_x - 1,     expand_y - 1, polygon_id );								
			else if( expand_value >= 0 )
				setValue( expand_grid, expand_x - 1,     expand_y - 1, BORDER );
			else if( expand_value == BORDER )
			{	expand_value -= 1;
				setValue( expand_grid, expand_x - 1,	  expand_y - 1,	expand_value );	
			}
			else if( expand_value == DOUBLE_BORDER )
			{	if( is_decision( expand_grid, expand_x - 1, expand_y - 1 ) )
					setValue( expand_grid, expand_x - 1, expand_y - 1, DECISION );	
				else
					setValue( expand_grid, expand_x - 1, expand_y - 1, BORDER );	
			}

			//Cell 7
			expand_value = getValue( expand_grid, expand_x, expand_y - 1 );
			//set the polygon id
			if( expand_value == expand_nodata_value )
				setValue( expand_grid, expand_x, expand_y - 1, polygon_id );
			//polygon_id already exists and is the same... reset it
			else if( expand_value == polygon_id )
				setValue( expand_grid, expand_x, expand_y - 1, polygon_id );						
			//set the cell as a border
			else if( expand_value >= 0 )
				setValue( expand_grid, expand_x, expand_y - 1, BORDER );
			
			//Cell 8
			expand_value = getValue( expand_grid, expand_x + 1, expand_y - 1 );
			if( expand_value == expand_nodata_value )
				setValue( expand_grid, expand_x + 1, expand_y - 1, polygon_id );
			else if( expand_value == polygon_id )
				setValue( expand_grid, expand_x + 1, expand_y - 1, polygon_id );					
			else if( expand_value >= 0 )
				setValue( expand_grid, expand_x + 1, expand_y - 1, BORDER );		
			else if( expand_value == BORDER )
			{	expand_value -= 1;
				setValue( expand_grid, expand_x + 1, expand_y - 1,	expand_value );	
			}
			else if( expand_value == DOUBLE_BORDER )
			{	if( is_decision( expand_grid, expand_x + 1, expand_y - 1 ) )
					setValue( expand_grid, expand_x + 1, expand_y - 1, DECISION );	
				else
					setValue( expand_grid, expand_x + 1, expand_y - 1, BORDER );	
			}
		}			
		
		newpercent = (long)(( (double)( rows - y - 1 )/(total) )*100);
		if( newpercent > percent )
		{	percent = newpercent;
			if( cBack != NULL )
				cBack->Progress(OLE2BSTR(key),percent,A2BSTR("Polygon Creation: Expanding Grid" ));
			else if( globalCallback != NULL )
				globalCallback->Progress(OLE2BSTR(key),percent,A2BSTR("Polygon Creation: Expanding Grid" ));
		}			
	}

	//Mark edges of the expanded grid that have a polygon id as BORDER
	//	and reset DOUBLE_BORDER to BORDER
	//Mark edges of a polygon that have a nodata_value by them
	newpercent = 0;
	percent = 0;
	total = exp_rows;
	for( int j = 0; j < exp_rows; j++ )
	{
		for( int i = 0; i < exp_cols; i++ )
		{
			polygon_id = getValue( expand_grid, i, j );
			if( polygon_id != expand_nodata_value )
			{
				if( j == 0 )
					setValue( expand_grid, i, 0, BORDER );
				else if( j == exp_rows - 1 )
					setValue( expand_grid, i, exp_rows - 1, BORDER );
				else if( i == 0 )
					setValue( expand_grid, 0, j, BORDER );
				else if( i == exp_cols - 1 )
					setValue( expand_grid, exp_cols - 1, j, BORDER );
				else 
				{					
					if( polygon_id == DOUBLE_BORDER )
						setValue( expand_grid, i, j, BORDER );
					//Convex polygon test
					else if( polygon_id == DECISION )
					{	
						if (connection_grid != NULL)
						{
							long contract_x = ( i - 1 )/2;
							long contract_y = ( j - 1 )/2;

							IGridHeader * cHeader = NULL;
							connection_grid->get_Header(&cHeader);
							VARIANT cndv;
							VariantInit(&cndv); //added by Rob Cairns 4-Jan-06
							cHeader->get_NodataValue(&cndv);
							cHeader->Release();
							cHeader = NULL;

							double nodata = -1;
							dVal(cndv,nodata);
							
							VariantClear(&cndv); //added by Rob Cairns 4-Jan-06
							
							double decision = getValue( connection_grid, contract_x, contract_y );
							if( decision == nodata )
							{	expand_grid->Close(&vbretval);							
								*retval = NULL;
								lastErrorCode = tkCONCAVE_POLYGONS;
								if( cBack != NULL )
									cBack->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));	
								else if( globalCallback != NULL )
									globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
								return S_OK;					
							}
						}
					}
					//Test to see if it is on a nodata border
					else
					{	//4 3 2
						//5 0 1
						//6 7 8

						//Cell 1
						double cell1 = getValue( expand_grid, i+1, j );
						//Cell 2
						double cell2 = getValue( expand_grid, i+1, j-1 );
						//Cell 3
						double cell3 = getValue( expand_grid, i, j-1 );
						//Cell 4
						double cell4 = getValue( expand_grid, i-1, j-1 );
						//Cell 5
						double cell5 = getValue( expand_grid, i-1, j );
						//Cell 6
						double cell6 = getValue( expand_grid, i-1, j+1 );
						//Cell 7
						double cell7 = getValue( expand_grid, i, j+1 );
						//Cell 8
						double cell8 = getValue( expand_grid, i+1, j+1 );

						if( cell1 == expand_nodata_value || 
							cell2 == expand_nodata_value || 
							cell3 == expand_nodata_value || 
							cell4 == expand_nodata_value || 
							cell5 == expand_nodata_value || 
							cell6 == expand_nodata_value || 
							cell7 == expand_nodata_value || 
							cell8 == expand_nodata_value )
						{
							if( i%2 == 0 && j%2 == 0 )
							{	if( is_decision( expand_grid, i, j ) )
									setValue( expand_grid, i, j, DECISION );
								else
									setValue( expand_grid, i, j, BORDER );
							}
							else
								setValue( expand_grid, i, j, BORDER );
						}
					}					
				}
			}			
		}

		newpercent = (long)(( j/total )*100);
		if( newpercent > percent )
		{	percent = newpercent;
			if( cBack != NULL )
				cBack->Progress(OLE2BSTR(key),percent,A2BSTR("Polygon Creation: Marking Borders" ));
			else if( globalCallback != NULL )
				globalCallback->Progress(OLE2BSTR(key),percent,A2BSTR("Polygon Creation: Marking Borders" ));
		}	
	}

	//Create the shapefile
	CoCreateInstance(CLSID_Shapefile,NULL,CLSCTX_INPROC_SERVER,IID_IShapefile,(void**)retval);
	(*retval)->CreateNew(A2BSTR(""),SHP_POLYGON,&vbretval);
	
	long fieldpos = 0;
	IField * field = NULL;
	CoCreateInstance(CLSID_Field,NULL,CLSCTX_INPROC_SERVER,IID_IField,(void**)&field);
	field->put_Name(A2BSTR("PolygonID"));
	field->put_Type(INTEGER_FIELD);
	field->put_Width(10);
	(*retval)->EditInsertField(field,&fieldpos,cBack,&vbretval);
	field->Release();
	field = NULL;

	//Create the polygons
	//Only look at the center value of the cells
	std::deque< RasterPoint > polygon;	
	int number_polygons = 0;
	double xllcenter = xllCenter - dx*.5;
	double yllcenter = yllCenter - dy*.5;
	double nodata = expand_nodata_value;
	
	total = rows;
	newpercent = 0;
	percent = 0;
	for( int py = 0; py < rows; py++) //rows - 1; py>= 0; py-- )
	{	for( int px = 0; px < cols; px++ )
		{
			//Expand as the center cell
			long expand_x = 2*px + 1;
			long expand_y = 2*py + 1;
			polygon_id = getValue( expand_grid, expand_x, expand_y );		

			//Check to see if polygon id is a specially marked cell or nodata
			if( polygon_id >= 0 )
			//if( polygon_id == 148 )
			{
				//Clear the polygon list
				polygon.clear();				

				//Mark the edge with
				// a special value.
				mark_edge( polygon_id, expand_x - 1, expand_y - 1 );

				//Always start Trace from Cell 3
				trace_polygon( expand_x - 1, expand_y - 1, polygon );
				
				//Flood fill the polygon with nodata
				scan_fill_to_edge( nodata, expand_x, expand_y + 1 );
				
				if( polygon.size() > 3 )
				{
					long endIndex = polygon.size()-1;

					double x1 = polygon[0].column,
						x2 = polygon[1].column,
						y1 = polygon[0].row, 
						y2 = polygon[1].row;

					cppVector v1(x2 - x1,y2 - y1,0.0);
					
					x1 = polygon[endIndex].column;
					x2 = polygon[0].column;
					y1 = polygon[endIndex].row;
					y2 = polygon[0].row;
					
					cppVector v2(x2 - x1,y2 - y1,0.0);
					
					cppVector cross = v1.crossProduct(v2);

					//Invert Polygon to Clockwise if necessary					
					//Counter ClockWise
					if( cross.getk() > 0.0 )
					{	
						std::deque<RasterPoint> reverse_polygon;
						for( int point = 0; point < (int)polygon.size(); point++ )
							reverse_polygon.push_front( polygon[point] );
						polygon = reverse_polygon;
					}
				
					
					//Write the final polygon
					IShape * shape = NULL;
					CoCreateInstance(CLSID_Shape,NULL,CLSCTX_INPROC_SERVER,IID_IShape,(void**)&shape);
					shape->Create(SHP_POLYGON,&vbretval);
					long ppos = 0;
					shape->InsertPart(0,&ppos,&vbretval);

					double poly_x = 0, poly_y = 0;
					long ras_x = 0, ras_y = 0;
					long last_point_index = -1;
					int point = 0;
					for( point = 0; point < (int)polygon.size(); point++) //(int)polygon.size()-1; point >= 0; point-- )
					{	
						ras_x = polygon[point].column;
						ras_y = polygon[point].row;

						//Check if the raster_point is a joint

						//Find the four cells in the flow grid
						//Constrict to center cell of connection_grid
						//( x, y ) == Cell 0
						//4 3 2
						//5 0 1
						//6 7 8

						//Cell 4
						double cell4_x = ( (ras_x-1) - 1 )/2;
						double cell4_y = ( (ras_y-1) - 1 )/2;
						//Cell 6
						double cell6_x = ( (ras_x-1) - 1 )/2;
						double cell6_y = ( (ras_y+1) - 1 )/2;
						//Cell 2
						double cell2_x = ( (ras_x+1) - 1 )/2;
						double cell2_y = ( (ras_y-1) - 1 )/2;
						//Cell 8			
						double cell8_x = ( (ras_x+1) - 1 )/2;
						double cell8_y = ( (ras_y+1) - 1 )/2;

						double cell2 = getValue( Grid, (long)cell2_x, (long)cell2_y );
						double cell8 = getValue( Grid, (long)cell8_x, (long)cell8_y );
						double cell4 = getValue( Grid, (long)cell4_x, (long)cell4_y );
						double cell6 = getValue( Grid, (long)cell6_x, (long)cell6_y );

						bool write_point = true;
						if( is_joint( cell2, cell8, cell4, cell6 ) )
							write_point = true;
						else if( ras_x == 0 || ras_x == exp_cols - 1 )
						{	write_point = true;
						}
						else if( ras_y == 0 || ras_y == exp_rows - 1 )
						{	write_point = true;
						}
						//Remove the Jagged Points if it isn't a joint
						else
						{	long ras_behind_x = 0, ras_behind_y = 0;
							long ras_front_x = 0, ras_front_y = 0;

							if( point == 0 )
							{	ras_behind_x = polygon[polygon.size()-1].column;
								ras_behind_y = polygon[polygon.size()-1].row;
							}
							else
							{	ras_behind_x = polygon[point-1].column;
								ras_behind_y = polygon[point-1].row;
							}

							if( point == polygon.size() -1 )
							{	ras_front_x = polygon[0].column;
								ras_front_y = polygon[0].row;
							}
							else
							{	ras_front_x = polygon[point+1].column;
								ras_front_y = polygon[point+1].row;
							}
							
							if( abs( ras_behind_x - ras_front_x ) == 1 &&
								abs( ras_behind_y - ras_front_y ) == 1 )
									write_point = false;
						}

						//Write the point if it's a joint or not a jaggy
						if( write_point == true )
						{	if( last_point_index < 0 )
								last_point_index = point;
							expand_grid->CellToProj( polygon[point].column, polygon[point].row, &poly_x, &poly_y );
							IPoint * ipoint = NULL;
							CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&ipoint);
							ipoint->put_X(poly_x);
							ipoint->put_Y(poly_y);
							long pindex = point;
							shape->InsertPoint( ipoint, &pindex, &vbretval ); 						
							ipoint->Release();
							ipoint = NULL;
						}
					}
					//Write the last point again
					expand_grid->CellToProj( polygon[last_point_index].column, polygon[last_point_index].row, &poly_x, &poly_y );
					IPoint * ipoint = NULL;
					CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&ipoint);
					ipoint->put_X(poly_x);
					ipoint->put_Y(poly_y);
					long pindex = point + 1;
					shape->InsertPoint( ipoint, &pindex, &vbretval );
					ipoint->Release();
					ipoint = NULL;

					long sindex = number_polygons;
					(*retval)->EditInsertShape(shape,&sindex,&vbretval);

					shape->Release();
					shape = NULL;

					VARIANT cval;
					VariantInit(&cval); //added by Rob Cairns 4-Jan-06
					cval.vt = VT_I4;
					cval.lVal = (LONG)polygon_id;
					(*retval)->EditCellValue(fieldpos,sindex,cval,&vbretval);				
					number_polygons++;
					VariantClear(&cval); //added by Rob Cairns 4-Jan-06
				}
			}					
		}

		newpercent = (long)((double)py/(double)rows*100);
		if( newpercent > percent )
		{	percent = newpercent;
			if( cBack != NULL )
				cBack->Progress(OLE2BSTR(key),percent,A2BSTR("Polygon Creation: Creating Polygons"));
			else if( globalCallback != NULL )
				globalCallback->Progress(OLE2BSTR(key),percent,A2BSTR("Polygon Creation: Creating Polygons"));
		}	
	}

	expand_grid->Close(&vbretval);
	expand_grid->Release();
	expand_grid = NULL;
	
	VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
	
	return S_OK;
}

//Determine if a given index of [] map is a DECISION SPOT . . .
//DECISION SPOT is a point where multiple path options are available
inline bool CUtils::is_decision( IGrid * g, int x, int y )
{	
	// ( x, y ) corresponds to 0

	//4 3 2
	//5 0 1
	//6 7 8

	//Check if cells (2 & 6) or (4 & 8) have the same polygon id.
	//If they do then this is a DECISION SPOT if there are three
	//unique values.
	
	double four = getValue( g, x - 1, y - 1 );
	double eight = getValue( g, x + 1, y + 1 );
	double two = getValue( g, x + 1, y - 1 );
	double six = getValue( g, x - 1, y + 1 );
	
	IGridHeader * header = NULL;
	g->get_Header(&header);
	VARIANT vval;
	VariantInit(&vval); //added by Rob Cairns 4-Jan-06
	header->get_NodataValue(&vval);
	double nodata = -1;
	dVal(vval,nodata);
	VariantClear(&vval); //added by Rob Cairns 4-Jan-06
	header->Release();
	header = NULL;

	if( two == six )
	{	if( two == four || two == eight )
			return false;
		else if( six == four || six == eight )
			return false;

		return true;
	}
	else if( four == eight )
	{	if( four == two || four == six )
			return false;
		else if( eight == two || eight == six )
			return false;
		return true;
	}

	return false;
}

//Mark the edges so the polygon can be traced
void CUtils::mark_edge( double & polygon_id, long x, long y )
{	
	std::stack< RasterPoint > stack;
	std::vector< RasterPoint > decisions;
	RasterPoint pt;
	//Change the BORDERS to TRACE_BORDERS
	stack.push( RasterPoint( x, y ) );
	int i = x, j = y;
	while( stack.size() > 0 )
	{		
		pt = stack.top();
		i = pt.column;
		j = pt.row;
		//i = stack[0].column;
		//j = stack[0].row;
		stack.pop(); //_front();
		//4 3 2
		//5 0 1
		//6 7 8
		//Cell 1
		double cell1 = getValue( expand_grid, i+1, j );		
		//Cell 2
		double cell2 = getValue( expand_grid, i+1, j-1 );		
		//Cell 3
		double cell3 = getValue( expand_grid, i, j-1 );		
		//Cell 4
		double cell4 = getValue( expand_grid, i-1, j-1 );		
		//Cell 5
		double cell5 = getValue( expand_grid, i-1, j );		
		//Cell 6
		double cell6 = getValue( expand_grid, i-1, j+1 );		
		//Cell 7
		double cell7 = getValue( expand_grid, i, j+1 );		
		//Cell 8
		double cell8 = getValue( expand_grid, i+1, j+1 );		

		if( cell1 == polygon_id || 
			cell2 == polygon_id || 
			cell3 == polygon_id || 
			cell4 == polygon_id || 
			cell5 == polygon_id || 
			cell6 == polygon_id || 
			cell7 == polygon_id || 
			cell8 == polygon_id )
		{	
			if( cell8 == BORDER )
				stack.push( RasterPoint( i+1, j+1 ));
			if( cell7 == BORDER )
				stack.push( RasterPoint( i, j+1 ));
			if( cell6 == BORDER )
				stack.push( RasterPoint( i-1, j+1 ));
			if( cell5 == BORDER )
				stack.push( RasterPoint( i-1, j ));
			if( cell4 == BORDER )
				stack.push( RasterPoint( i-1, j-1 ));
			if( cell3 == BORDER )
				stack.push( RasterPoint( i, j-1 ));
			if( cell2 == BORDER )
				stack.push( RasterPoint( i+1, j-1 ));
			if( cell1 == BORDER )
				stack.push( RasterPoint( i+1, j ));

			//Don't reset if DECISION
			if( getValue( expand_grid, i, j ) == BORDER )
				setValue( expand_grid, i, j, TRACE_BORDER );
			//Push diagonals if this is a DECISION
			else if( getValue( expand_grid, i, j ) == DECISION )
			{	stack.push( RasterPoint( i+1, j+1 ) );
				stack.push( RasterPoint( i-1, j+1 ) );
				stack.push( RasterPoint( i-1, j-1 ) );
				stack.push( RasterPoint( i+1, j-1 ) );
			}
		}
	}

	//Mark Polygon as the CURRENT_POLYGON
	stack.push( RasterPoint( x+1, y+1 ) );
	while( stack.size() > 0 )
	{	
		pt = stack.top();
		i = pt.column;
		j = pt.row;
		//i = stack[0].column;
		//j = stack[0].row;
		stack.pop();
		//4 3 2
		//5 0 1
		//6 7 8
		//Cell 1
		double cell1 = getValue( expand_grid, i+1, j );
		if( cell1 == polygon_id || cell1 == DECISION )
			stack.push( RasterPoint( i+1, j ));
		//Cell 2
		double cell2 = getValue( expand_grid, i+1, j-1 );
		if( cell2 == polygon_id || cell2 == DECISION  )
			stack.push( RasterPoint( i+1, j-1 ));
		//Cell 3
		double cell3 = getValue( expand_grid, i, j-1 );
		if( cell3 == polygon_id || cell3 == DECISION  )
			stack.push( RasterPoint( i, j-1 ));
		//Cell 4
		double cell4 = getValue( expand_grid, i-1, j-1 );
		if( cell4 == polygon_id || cell4 == DECISION  )
			stack.push( RasterPoint( i-1, j-1 ));
		//Cell 5
		double cell5 = getValue( expand_grid, i-1, j );
		if( cell5 == polygon_id || cell5 == DECISION  )
			stack.push( RasterPoint( i-1, j ));
		//Cell 6
		double cell6 = getValue( expand_grid, i-1, j+1 );
		if( cell6 == polygon_id || cell6 == DECISION  )
			stack.push( RasterPoint( i-1, j+1 ));
		//Cell 7
		double cell7 = getValue( expand_grid, i, j+1 );
		if( cell7 == polygon_id || cell7 == DECISION  )
			stack.push( RasterPoint( i, j+1 ));
		//Cell 8
		double cell8 = getValue( expand_grid, i+1, j+1 );
		if( cell8 == polygon_id || cell8 == DECISION  )
			stack.push( RasterPoint( i+1, j+1 ));

		//Don't reset if DECISION
		if( getValue( expand_grid, i, j ) == polygon_id )
			setValue( expand_grid, i, j, CURRENT_POLYGON );
		//Push diagonals if this is a DECISION
		else if( getValue( expand_grid, i, j ) == DECISION )
		{	setValue( expand_grid, i, j, CURRENT_POLYGON );
			decisions.push_back( RasterPoint( i, j ) );
			stack.push( RasterPoint( i+1, j+1 ) );
			stack.push( RasterPoint( i-1, j+1 ) );
			stack.push( RasterPoint( i-1, j-1 ) );
			stack.push( RasterPoint( i+1, j-1 ) );
		}			
	}
	
	for( int d1 = 0; d1 < (int)decisions.size(); d1++ )
		setValue( expand_grid, decisions[d1].column, decisions[d1].row, DECISION );
}	

//Erase the CurrentPolygon
void CUtils::scan_fill_to_edge( double & nodata, long x, long y )
{	
	std::stack< RasterPoint > stack;
	std::vector< RasterPoint > decisions;
	RasterPoint pt;

	//Erase the Polygon
	int i = x, j = y;
	stack.push( RasterPoint( x, y-1 ) );
	while( stack.size() > 0 )
	{	
		pt = stack.top();
		i = pt.column;
		j = pt.row;
		stack.pop();
		//4 3 2
		//5 0 1
		//6 7 8
		//Cell 1
		double cell1 = getValue( expand_grid, i+1, j );
		if( cell1 == CURRENT_POLYGON || cell1 == DECISION )
			stack.push( RasterPoint( i+1, j ));
		//Cell 2
		double cell2 = getValue( expand_grid, i+1, j-1 );
		if( cell2 == CURRENT_POLYGON || cell2 == DECISION  )
			stack.push( RasterPoint( i+1, j-1 ));
		//Cell 3
		double cell3 = getValue( expand_grid, i, j-1 );
		if( cell3 == CURRENT_POLYGON || cell3 == DECISION  )
			stack.push( RasterPoint( i, j-1 ));
		//Cell 4
		double cell4 = getValue( expand_grid, i-1, j-1 );
		if( cell4 == CURRENT_POLYGON || cell4 == DECISION  )
			stack.push( RasterPoint( i-1, j-1 ));
		//Cell 5
		double cell5 = getValue( expand_grid, i-1, j );
		if( cell5 == CURRENT_POLYGON || cell5 == DECISION  )
			stack.push( RasterPoint( i-1, j ));
		//Cell 6
		double cell6 = getValue( expand_grid, i-1, j+1 );
		if( cell6 == CURRENT_POLYGON || cell6 == DECISION  )
			stack.push( RasterPoint( i-1, j+1 ));
		//Cell 7
		double cell7 = getValue( expand_grid, i, j+1 );
		if( cell7 == CURRENT_POLYGON || cell7 == DECISION  )
			stack.push( RasterPoint( i, j+1 ));
		//Cell 8
		double cell8 = getValue( expand_grid, i+1, j+1 );
		if( cell8 == CURRENT_POLYGON || cell8 == DECISION  )
			stack.push( RasterPoint( i+1, j+1 ));

		//Don't reset if DECISION
		if( getValue( expand_grid, i, j ) == CURRENT_POLYGON )
			setValue( expand_grid, i, j, nodata );
		//Push diagonals if this is a DECISION
		else if( getValue( expand_grid, i, j ) == DECISION )
		{	setValue( expand_grid, i, j, nodata );
			decisions.push_back( RasterPoint( i, j ) );
			stack.push( RasterPoint( i+1, j+1 ) );
			stack.push( RasterPoint( i-1, j+1 ) );
			stack.push( RasterPoint( i-1, j-1 ) );
			stack.push( RasterPoint( i+1, j-1 ) );
		}			
	}
	
	for( int d1 = 0; d1 < (int)decisions.size(); d1++ )
		setValue( expand_grid, decisions[d1].column, decisions[d1].row, DECISION );
}	

inline double CUtils::getValue( IGrid * Grid, long column, long row )
{	VARIANT vval;
	VariantInit(&vval); //added by Rob Cairns 4-Jan-06
	Grid->get_Value(column,row,&vval);
	double val = 0;
	dVal(vval,val);
	VariantClear(&vval); //added by Rob Cairns 4-Jan-06
	return val;
}

inline void CUtils::setValue( IGrid * Grid, long column, long row, double val )
{	VARIANT vval;
	VariantInit(&vval); //added by Rob Cairns 4-Jan-06
	vval.vt = VT_R8;
	vval.dblVal = val;
	Grid->put_Value(column,row,vval);
	VariantClear(&vval); //added by Rob Cairns 4-Jan-06
}

void CUtils::trace_polygon( long x, long y, std::deque<RasterPoint> & polygon )
{	
	polygon.push_back( RasterPoint( x, y ) );

	std::stack<RasterPoint> stack;
	RasterPoint pt;

	stack.push(RasterPoint(x,y));

	while ( stack.size() > 0)
	{
		pt = stack.top();
		x = pt.column;
		y = pt.row;
		//x = stack[0].column;
		//y = stack[0].row;
		stack.pop();

		

		//Reset the current grid value back to BORDER
		if( getValue( expand_grid, x, y ) == TRACE_BORDER )
			setValue( expand_grid, x, y, BORDER );
		
		// ( x, y ) corresponds to 0
		//4 3 2
		//5 0 1
		//6 7 8
		
		bool moved_turtle = false;
		//Restrict to Cardinal Directions
		//Cell 1
		if( getValue( expand_grid, x + 1, y ) == TRACE_BORDER )
		{	
			moved_turtle = true;
			//trace_polygon( x + 1, y, polygon );
			stack.push(RasterPoint(x + 1, y));
			polygon.push_back(RasterPoint(x + 1, y));
		}
		//Cell 3
		else if( getValue( expand_grid, x, y - 1 ) == TRACE_BORDER )
		{	
			moved_turtle = true;
			//trace_polygon( x, y - 1, polygon );
			stack.push(RasterPoint(x, y - 1));
			polygon.push_back(RasterPoint(x, y - 1));
		}
		//Cell 5
		else if( getValue( expand_grid, x - 1, y ) == TRACE_BORDER )
		{	
			moved_turtle = true;
			//trace_polygon( x - 1, y, polygon );
			stack.push(RasterPoint(x - 1, y));
			polygon.push_back(RasterPoint(x - 1, y));
		}
		//Cell 7
		else if( getValue( expand_grid, x, y + 1 ) == TRACE_BORDER )
		{	
			moved_turtle = true;
			//trace_polygon( x, y + 1, polygon );
			stack.push(RasterPoint(x, y + 1));
			polygon.push_back(RasterPoint(x, y + 1));
		}
		
		//Look for a Decision Node ...
		//Cell 1
		if( !moved_turtle )
		{
			if( getValue( expand_grid, x + 1, y ) == DECISION )
			{
				if( connection_grid == NULL )
				{
					polygon.clear();
					return;
				}
				
				//Find the four cells in the flow grid
				//Constrict to center cell of connection_grid
				//( x, y ) now == Cell 5 
				//4 3 2
				//5 0 1
				//6 7 8
				
				//Cell 2
				cell2_x = ( (x+2) - 1 )/2;
				cell2_y = ( (y-1) - 1 )/2;
				//Cell 4
				cell4_x = ( (x) - 1 )/2;
				cell4_y = ( (y-1) - 1 )/2;
				//Cell 6
				cell6_x = ( (x) - 1 )/2;
				cell6_y = ( (y+1) - 1 )/2;			
				//Cell 8			
				cell8_x = ( (x+2) - 1 )/2;
				cell8_y = ( (y+1) - 1 )/2;
				
				flow2 = getValue( connection_grid, cell2_x, cell2_y );
				flow8 = getValue( connection_grid, cell8_x, cell8_y );
				flow4 = getValue( connection_grid, cell4_x, cell4_y );
				flow6 = getValue( connection_grid, cell6_x, cell6_y );
				
				//Move to Up-Right ... maybe
				if( flow2 == 6 || flow6 == 2 )
				{	
					if( getValue( expand_grid, x + 1, y - 1 ) == TRACE_BORDER )
					{
						moved_turtle = true;
						//trace_polygon( x + 1, y - 1, polygon );
						stack.push(RasterPoint( x + 1, y - 1));
						polygon.push_back(RasterPoint(x + 1, y - 1));
					}
				}
				//Move to Down-Right ... maybe
				else if( flow4 == 8 || flow8 == 4 )
				{	
					if( getValue( expand_grid, x + 1, y + 1 ) == TRACE_BORDER )
					{
						moved_turtle = true;
						//trace_polygon( x + 1, y + 1, polygon );
						stack.push(RasterPoint(x + 1, y + 1));
						polygon.push_back(RasterPoint(x + 1, y + 1));
					}
				}
				//Cannot be determined by flow directions
				else
				{	
					if( getValue( expand_grid, x, y - 1 ) == CURRENT_POLYGON )
					{
						moved_turtle = true;
						//trace_polygon( x + 1, y - 1, polygon );
						stack.push(RasterPoint(x + 1, y - 1));
						polygon.push_back(RasterPoint(x + 1, y - 1));
					}
					else
					{
						moved_turtle = true;
						//trace_polygon( x + 1, y + 1, polygon );
						stack.push(RasterPoint(x + 1, y + 1));
						polygon.push_back(RasterPoint(x + 1, y + 1));
					}
				}
			}
		}
		
		//Cell 3
		if( !moved_turtle )
		{
			if( getValue( expand_grid, x, y - 1 ) == DECISION )
			{
				if( connection_grid == NULL )
				{
					polygon.clear();
					return;
				}
				
				//Find the four cells in the flow grid
				//Constrict to center cell of connection_grid
				//( x, y ) now == Cell 7
				//4 3 2
				//5 0 1
				//6 7 8
				
				//Cell 2
				cell2_x = ( (x+1) - 1 )/2;
				cell2_y = ( (y-2) - 1 )/2;
				//Cell 4
				cell4_x = ( (x-1) - 1 )/2;
				cell4_y = ( (y-2) - 1 )/2;
				//Cell 6
				cell6_x = ( (x-1) - 1 )/2;
				cell6_y = ( (y) - 1 )/2;			
				//Cell 8			
				cell8_x = ( (x+1) - 1 )/2;
				cell8_y = ( (y) - 1 )/2;
				
				flow2 = getValue( connection_grid, cell2_x, cell2_y );
				flow8 = getValue( connection_grid, cell8_x, cell8_y );
				flow4 = getValue( connection_grid, cell4_x, cell4_y );
				flow6 = getValue( connection_grid, cell6_x, cell6_y );
				
				//Move to Up-Right ... maybe
				if( flow2 == 6 || flow6 == 2 )
				{	
					if( getValue( expand_grid, x + 1, y - 1 ) == TRACE_BORDER )
					{
						moved_turtle = true;
						//trace_polygon( x + 1, y - 1, polygon );
						stack.push(RasterPoint(x + 1, y - 1));
						polygon.push_back(RasterPoint(x + 1, y - 1));
					}
				}
				//Move to Up-Left ... maybe
				else if( flow4 == 8 || flow8 == 4 )
				{
					if( getValue( expand_grid, x - 1, y - 1 ) == TRACE_BORDER )
					{	
						moved_turtle = true;
						//trace_polygon( x - 1, y - 1, polygon );
						stack.push(RasterPoint(x - 1, y - 1));
						polygon.push_back(RasterPoint(x - 1, y - 1));
					}
				}
				//Cannot be determined by flow directions
				else
				{	
					if( getValue( expand_grid, x - 1, y ) == CURRENT_POLYGON )
					{
						moved_turtle = true;
						//trace_polygon( x - 1, y - 1, polygon );
						stack.push(RasterPoint(x - 1, y - 1));
						polygon.push_back(RasterPoint(x - 1, y - 1));
					}
					else
					{
						moved_turtle = true;
						//trace_polygon( x + 1, y - 1, polygon );
						stack.push(RasterPoint(x + 1, y - 1));
						polygon.push_back(RasterPoint(x + 1, y - 1));
					}
				}
			}
		}
		
		//Cell 5
		if( !moved_turtle )
		{
			if( getValue( expand_grid, x - 1, y ) == DECISION )
			{
				if( connection_grid == NULL )
				{
					polygon.clear();
					return;
				}
				
				//Find the four cells in the flow grid
				//Constrict to center cell of connection_grid
				//( x, y ) now == Cell 1
				//4 3 2
				//5 0 1
				//6 7 8
				
				//Cell 2
				cell2_x = ( (x) - 1 )/2;
				cell2_y = ( (y-1) - 1 )/2;
				//Cell 4
				cell4_x = ( (x-2) - 1 )/2;
				cell4_y = ( (y-1) - 1 )/2;
				//Cell 6
				cell6_x = ( (x-2) - 1 )/2;
				cell6_y = ( (y+1) - 1 )/2;			
				//Cell 8			
				cell8_x = ( (x) - 1 )/2;
				cell8_y = ( (y+1) - 1 )/2;
				
				flow2 = getValue( connection_grid, cell2_x, cell2_y );
				flow8 = getValue( connection_grid, cell8_x, cell8_y );
				flow4 = getValue( connection_grid, cell4_x, cell4_y );
				flow6 = getValue( connection_grid, cell6_x, cell6_y );
				
				//Move to Down-Left ... maybe
				if( flow2 == 6 || flow6 == 2 )
				{	
					if( getValue( expand_grid, x - 1, y + 1 ) == TRACE_BORDER )
					{
						moved_turtle = true;
						//trace_polygon( x - 1, y + 1, polygon );
						stack.push(RasterPoint(x - 1, y + 1));
						polygon.push_back(RasterPoint(x - 1, y + 1));
					}
				}
				//Move to Up-Left ... maybe
				else if( flow4 == 8 || flow8 == 4 )
				{	
					if( getValue( expand_grid, x - 1, y - 1 ) == TRACE_BORDER )
					{
						moved_turtle = true;
						//trace_polygon( x - 1, y - 1, polygon );
						stack.push(RasterPoint(x - 1, y - 1));
						polygon.push_back(RasterPoint(x - 1, y - 1));
					}
				}
				//Cannot be determined by flow directions
				else
				{	
					if( getValue( expand_grid, x, y - 1 ) == CURRENT_POLYGON )
					{	
						moved_turtle = true;
						//trace_polygon( x - 1, y - 1, polygon );
						stack.push(RasterPoint(x - 1, y - 1));
						polygon.push_back(RasterPoint(x - 1, y - 1));
					}
					else
					{
						moved_turtle = true;
						//trace_polygon( x - 1, y + 1, polygon );
						stack.push(RasterPoint(x - 1, y + 1));
						polygon.push_back(RasterPoint(x - 1, y + 1));
					}
				}
			}	
		}
		
		//Cell 7
		if( !moved_turtle )	{	
			if( getValue( expand_grid, x, y + 1 ) == DECISION )
			{
				if( connection_grid == NULL )
				{
					polygon.clear();
					return;
				}
				
				//Find the four cells in the flow grid
				//Constrict to center cell of connection_grid
				//( x, y ) now == Cell 3
				//4 3 2
				//5 0 1
				//6 7 8
				
				//Cell 2
				cell2_x = ( (x+1) - 1 )/2;
				cell2_y = ( (y) - 1 )/2;
				//Cell 4
				cell4_x = ( (x-1) - 1 )/2;
				cell4_y = ( (y) - 1 )/2;
				//Cell 6
				cell6_x = ( (x-1) - 1 )/2;
				cell6_y = ( (y+2) - 1 )/2;			
				//Cell 8			
				cell8_x = ( (x+1) - 1 )/2;
				cell8_y = ( (y+2) - 1 )/2;
				
				flow2 = getValue( connection_grid, cell2_x, cell2_y );
				flow8 = getValue( connection_grid, cell8_x, cell8_y );
				flow4 = getValue( connection_grid, cell4_x, cell4_y );
				flow6 = getValue( connection_grid, cell6_x, cell6_y );
				
				//Move to Down-Left ... maybe
				if( flow2 == 6 || flow6 == 2 )
				{	
					if( getValue( expand_grid, x - 1, y + 1 ) == TRACE_BORDER )
					{	
						moved_turtle = true;
						//trace_polygon( x - 1, y + 1, polygon );
						stack.push(RasterPoint(x - 1, y + 1));
						polygon.push_back(RasterPoint(x - 1, y + 1));
					}
				}
				//Move to Down-Right ... maybe
				else if( flow4 == 8 || flow8 == 4 )
				{	
					if( getValue( expand_grid, x + 1, y + 1 ) == TRACE_BORDER )
					{	
						moved_turtle = true;
						//trace_polygon( x + 1, y + 1, polygon );
						stack.push(RasterPoint( x + 1, y + 1));
						polygon.push_back(RasterPoint(x + 1, y + 1));
					}
				}
				//Cannot be determined by flow directions
				else
				{	
					if( getValue( expand_grid, x - 1, y ) == CURRENT_POLYGON )
					{
						moved_turtle = true;
						//trace_polygon( x - 1, y + 1, polygon );
						stack.push(RasterPoint(x - 1, y + 1));
						polygon.push_back(RasterPoint(x - 1, y + 1));
					}
					else
					{	
						moved_turtle = true;
						//trace_polygon( x + 1, y + 1, polygon );
						stack.push(RasterPoint(x + 1, y + 1));
						polygon.push_back(RasterPoint(x + 1, y + 1));
					}
				}
			}
		}
		
	}
}

inline bool CUtils::is_joint( double cell2, double cell8, double cell4, double cell6 )
{	
	int number_unique = 1;
	if( cell2 != cell8 && cell2 != cell4 && cell2 != cell6 )
		number_unique++;
	if( cell8 != cell2 && cell8 != cell4 && cell8 != cell6 )
		number_unique++;
	if( cell4 != cell2 && cell4 != cell8 && cell4 != cell6 )
		number_unique++;
	if( cell6 != cell2 && cell6 != cell8 && cell6 != cell4 )
		number_unique++;

	if( number_unique > 2 )
		return true;
	return false;
}


///\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
///\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
//			POLYGONAL ALGORITHM
///\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/
///\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/

#ifndef ROUND
#define ROUND(X)  (long)(X + .5)
#endif

STDMETHODIMP CUtils::ShapefileToGrid(IShapefile * Shpfile, VARIANT_BOOL UseShapefileBounds, IGridHeader * GridHeader, double Cellsize, VARIANT_BOOL UseShapeNumber, short SingleValue,IGrid ** retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	USES_CONVERSION;

	if( Shpfile == NULL )
	{	*retval = NULL;
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return S_OK;
	}
	
	CoCreateInstance(CLSID_Grid,NULL,CLSCTX_INPROC_SERVER,IID_IGrid,(void**)retval);

	if ((*retval) == NULL)
	{
		lastErrorCode = tkCANT_COCREATE_COM_INSTANCE;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		
		return S_OK;

	}
	
	VARIANT_BOOL result = VARIANT_FALSE;
	VARIANT vndv; //no data value
	VariantInit(&vndv); //added by Rob Cairns 4-Jan-06
	vndv.vt = VT_I2;
	short NoData;

	if (UseShapefileBounds == VARIANT_FALSE)
	{
		if ( GridHeader == NULL)
		{	//if the user doesn't want to use the shapefilebounds, then they MUST
			//give us a GridHeader...so, if this is null, fail
			(*retval)->Release();
			*retval = NULL;
			lastErrorCode = tkINVALID_PARAMETER_VALUE;
			if( globalCallback != NULL )
				globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
			VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
			return S_OK;
		}
		

		//attempt to create a new grid of the correct size
		GridHeader->get_NodataValue(&vndv);
		sVal(vndv,NoData);

		if (UseShapeNumber == VARIANT_FALSE && NoData == SingleValue)
		{
			//we must fail because we can't use the nodatavalue from the grid
			//as the singlevalue of the shapefile being converted to a grid;
			(*retval)->Release();
			*retval = NULL;
			lastErrorCode = tkINVALID_PARAMETER_VALUE;
			if( globalCallback != NULL )
				globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
			VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
			return S_OK;

		}

		(*retval)->CreateNew(A2BSTR(""),GridHeader,ShortDataType ,vndv,VARIANT_TRUE,UseExtension,globalCallback,&result);

		if ( result == VARIANT_FALSE)
		{ //failed to create new grid
			(*retval)->get_LastErrorCode(&lastErrorCode);
			(*retval)->Release();
			*retval = NULL;
			if( globalCallback != NULL )
				globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
			VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
			return S_OK;
		}

		long NumShapes;

		Shpfile->get_NumShapes(&NumShapes);

		IShape * shape = NULL;
		
		short cellValue;

		//loop through each shape and examine all points within the shape
		for(short CurShape = 0; CurShape < NumShapes; CurShape++)
		{
			if (UseShapeNumber == VARIANT_FALSE)
				cellValue = SingleValue;
			else
				cellValue = CurShape;

			Shpfile->get_Shape(CurShape,&shape);
			if ( PolygonToGrid(shape,retval,cellValue) == false)
				break;
			shape->Release();

		}

		
	}
	else
	{

		return NULL; //not complete

		IGridHeader * LocalGridHeader;
		(*retval)->get_Header(&LocalGridHeader);

		//attempt to create a new grid of the correct size
		LocalGridHeader->get_NodataValue(&vndv);
		sVal(vndv,NoData);

		//make sure that the singlevalue they want to use isn't the nodata value we are using
		if (UseShapeNumber == VARIANT_FALSE && NoData == SingleValue)
		{
			NoData = NoData - 1;
			vndv.vt = VT_I2;
			vndv.iVal = NoData;

			LocalGridHeader->put_NodataValue(vndv);
		}

		//Create a header that uses the minimum size allowed
		IExtents * bndbox;
		Shpfile->get_Extents(&bndbox);

		double xllcenter = 0, yllcenter = 0;
		double xurcenter = 0, yurcenter = 0;
		bndbox->get_xMin(&xllcenter);
		bndbox->get_yMin(&yllcenter);
		bndbox->get_xMax(&xurcenter);
		bndbox->get_yMax(&yurcenter);
		bndbox->Release();
		
		if( Cellsize <= 1 )
		{	*retval = NULL;
			lastErrorCode = tkINVALID_PARAMETER_VALUE;
			if( globalCallback != NULL )
				globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));			
			return S_OK;
		}

		long ncols = ROUND( ( xurcenter - xllcenter ) / Cellsize ) + 1;
		long nrows = ROUND( ( yurcenter - yllcenter )/ Cellsize ) + 1;

		LocalGridHeader->put_dX(Cellsize);
		LocalGridHeader->put_dY(Cellsize);
		VARIANT vndv;
		VariantInit(&vndv); //added by Rob Cairns 4-Jan-06 - is this code reachable?
		vndv.iVal = NoData;
		LocalGridHeader->put_NodataValue(vndv);
		LocalGridHeader->put_NumberCols(ncols);
		LocalGridHeader->put_NumberRows(nrows);
		LocalGridHeader->put_XllCenter(xllcenter );
		LocalGridHeader->put_YllCenter(yllcenter);
		
		
		(*retval)->CreateNew(A2BSTR(""),LocalGridHeader,ShortDataType ,vndv,TRUE,UseExtension,globalCallback,&result);

		bndbox->Release();
		LocalGridHeader->Release();

		if ( result == VARIANT_FALSE)
		{ //failed to create new grid
			(*retval)->get_LastErrorCode(&lastErrorCode);
			(*retval)->Release();
			*retval = NULL;
			if( globalCallback != NULL )
				globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
			return S_OK;
		}

		long NumShapes;

		Shpfile->get_NumShapes(&NumShapes);

		IShape * shape = NULL;
		
		short cellValue;

		//loop through each shape and examine all points within the shape
		for(short CurShape = 0; CurShape < NumShapes; CurShape++)
		{
			if (UseShapeNumber == FALSE)
				cellValue = SingleValue;
			else
				cellValue = CurShape;

			Shpfile->get_Shape(CurShape,&shape);
			if ( PolygonToGrid(shape,retval,cellValue) == false)
				break;
			shape->Release();
		}

	}

	VariantClear(&vndv); //added by Rob Cairns 4-Jan-06
	return S_OK;
}

bool CUtils::PolygonToGrid(IShape * shape, IGrid ** grid, short cellValue)
{
LineBresenham Line;
std::vector<lbPoint> ListOfPoints;
lbPoint StartPoint, EndPoint;
//IShape * shape = NULL;
IPoint * shpPoint = NULL;
long NumPoints = 0;
double CurX;
double CurY;

	ListOfPoints.clear();
	
	if (shape == NULL || grid == NULL)
	{
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( globalCallback != NULL )
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
		return false;
	}

	
	VARIANT CellValue;
	VariantInit(&CellValue); //added by Rob Cairns 4-Jan-06
	
	//the variant needs to be declared as a short
	CellValue.vt = VT_I2;
	
	//set it equal to the value specified in parameter list
	CellValue.iVal = cellValue;

	shape->get_NumPoints(&NumPoints);
	for(int CurPoint = 0; CurPoint <= NumPoints; CurPoint++)
	{
	
		if ( CurPoint < NumPoints)
		{
			shape->get_Point(CurPoint,&shpPoint);
		}
		else 
		{//we need to make sure to connect the first point to the last point
			ShpfileType shapetype;
			shape->get_ShapeType(&shapetype);

			if (shapetype == SHP_POLYGON || shapetype == SHP_POLYGONZ || shapetype == SHP_POLYGONM )
			{
				shape->get_Point(0,&shpPoint);
			}
			else
				break;//jump out of loop, we are done

		}
		
		if ( shpPoint == NULL)
		{
			shape->get_LastErrorCode(&lastErrorCode);
			if( globalCallback != NULL )
				globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
			
			VariantClear(&CellValue); //added by Rob Cairns 4-Jan-06
			
			return false;
		}
		
		//if we got to this point, then we have a valid shape and a valid shpPoint
		if ( CurPoint == 0  )
		{//we are one the first point, so just store the point and continue
			shpPoint->get_X(&CurX);
			shpPoint->get_Y(&CurY);
			shpPoint->Release();
			
			//this next line uses the grid to calculate the location of 
			//the current point within the grid
			(*grid)->ProjToCell(CurX,CurY,&(EndPoint.x),&(EndPoint.y));
			
		}
		else
		{
			//set the previous point equal to the currentPoint
			//then get the new Current point
			StartPoint = EndPoint;
			
			shpPoint->get_X(&CurX);
			shpPoint->get_Y(&CurY);
			shpPoint->Release();
			
			//this next line uses the grid to calculate the location of 
			//the current point within the grid
			(*grid)->ProjToCell(CurX,CurY,&(EndPoint.x),&(EndPoint.y));
			
			//now run the bresenham on the two points so far
			ListOfPoints = Line.ComputeLinePoints(StartPoint,EndPoint);
			
			for (int i = 0; i < (int)ListOfPoints.size(); i++)
				(*grid)->put_Value(ListOfPoints[i].x,ListOfPoints[i].y, CellValue);
		}

	VariantClear(&CellValue); //added by Rob Cairns 4-Jan-06

	}//end for(int CurPoint = 0; CurPoint < NumPoints; CurPoint++)

	return true;
}

STDMETHODIMP CUtils::hBitmapToPicture(long hBitmap, IPictureDisp **retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	HBITMAP bitmap = (HBITMAP)hBitmap;
	PICTDESC pictdesc;
	pictdesc.cbSizeofstruct = sizeof(PICTDESC);
	pictdesc.picType = PICTYPE_BITMAP;
	pictdesc.bmp.hbitmap = bitmap;
	pictdesc.bmp.hpal = NULL;
	*retval = NULL;
	OleCreatePictureIndirect(&pictdesc,IID_IPictureDisp,TRUE,(void**)retval);

	return S_OK;
}

STDMETHODIMP CUtils::GenerateHillShade(BSTR  bstrGridFilename, BSTR  bstrShadeFilename, 
						 float z, float scale, float az, float alt, VARIANT_BOOL *retval) 
/*  Purpose:
		Hillshade generates a shaded relief map from any GDAL-supported elevation raster
    Credits:
		This code was swiped from Matt Perry
		perrygeo@gmail.com
		http://www.perrygeo.net
		Published in Gdal-dev Digest, Vol 19, Issue 20
	Usage:
		hillshade input_dem output_hillshade
		[-z ZFactor (default=1)] [-s scale* (default=1)]
		[-az Azimuth (default=315)] [-alt Altitude (default=45)]
	Notes : 
		Scale for Feet:Latlong use scale=370400, for Meters:LatLong use scale=111120
*/

{ 
	USES_CONVERSION;
	const char  *pszGridFilename = OLE2A(bstrGridFilename);
	const char  *pszShadeFilename = OLE2A(bstrShadeFilename);

    GDALDataset *poDataset;  
    const float radiansToDegrees = (float)(180.0 / 3.14159);  
    const float degreesToRadians = (float)(3.14159 / 180.0); 
    double      adfGeoTransf[6];
    float       *win;
    float       *shadeBuf;
	float  	    x;
	float		y;
    float       aspect;
	float		slope;
	float       cang;
    int         i;
    int         j;
    int         n;
    int         containsNull;
	const char *pszFormat = "GTiff";
	bool		retVal = false;
    
	try
	{	
		/* ----------------------------------- 
		* Default Values 
		*/

		if (z == NULL) z = 1.0; 
		if (scale == NULL) scale = 1.0;
		if (az == NULL) az = 315.0;
		if (alt == NULL) alt = 45.0;
		
		/*---------------------------------------
		* Open Dataset and get raster band (assuming it is band #1)
		*/
		
		GDALAllRegister(); 

		poDataset = (GDALDataset *) GDALOpen( pszGridFilename, GA_ReadOnly );
		if( poDataset == NULL )
		{
			printf( "Couldn't open dataset %s\n", 
					pszGridFilename );
			return retVal;
		}
		GDALRasterBand  *poBnd;       
		poBnd = poDataset->GetRasterBand( 1 );
		poDataset->GetGeoTransform( adfGeoTransf );

		/* -------------------------------------
		* Get variables from input dataset
		*/
		const double   nsres = adfGeoTransf[5];
		const double   ewres = adfGeoTransf[1];	
		const float    nullValue = (float) poBnd->GetNoDataValue( );
		const int      nXSize = poBnd->GetXSize();
		const int      nYSize = poBnd->GetYSize();
		shadeBuf       = (float *) CPLMalloc(sizeof(float)*nXSize);
		win            = (float *) CPLMalloc(sizeof(float)*9);

		/* -----------------------------------------
		* Create the output dataset and copy over relevant metadata
		*/
		GDALDriver *poDriver;
		poDriver = GetGDALDriverManager()->GetDriverByName(pszFormat);
		GDALDataset      *poShadeDS;    
		GDALRasterBand   *poShadeBand; 
		char **papszOptions = NULL;
		
		printf("XSize %d\n",nXSize);

		poShadeDS = poDriver->Create(pszShadeFilename,nXSize,nYSize,1,GDT_Byte, 
									papszOptions );

		if (poShadeDS == NULL)
			return retVal;
		poShadeDS->SetGeoTransform( adfGeoTransf );    
		poShadeDS->SetProjection( poDataset->GetProjectionRef() );
		poShadeBand = poShadeDS->GetRasterBand(1);
		poShadeBand->SetNoDataValue( poBnd->GetNoDataValue( ) );    
		/* ------------------------------------------
		* Move a 3x3 window over each cell 
		* (where the cell in question is #4)
		*
		*                 0 1 2
		*                 3 4 5
		*                 6 7 8
		*
		*/
		for ( i = 0; i < nYSize; i++) {
			for ( j = 0; j < nXSize; j++) {
				containsNull = 0;

				// Exclude the edges 
				if (i == 0 || j == 0 || i == nYSize-1 || j == nXSize-1 ) 
				{
					// We are at the edge so write nullValue and move on
					shadeBuf[j] = nullValue;
					continue;
				}

				// Read in 3x3 window
				poBnd->RasterIO( GF_Read, j-1, i-1, 3, 3,
							win, 3, 3, GDT_Float32, 
							0, 0 ); 

				// Check if window has null value
				for ( n = 0; n <= 8; n++) {
				if(win[n] == nullValue) {
					containsNull = 1;
					break;
				}
				}

				if (containsNull == 1) {
					// We have nulls so write nullValue and move on
					shadeBuf[j] = nullValue;
					continue;
				} 
				else 
				{
					// We have a valid 3x3 window. 
	         
					/* ---------------------------------------
					* Compute Hillshade
					*/

					// First Slope ...
					x = (float)(((z*win[0] + z*win[3] + z*win[3] + z*win[6]) - 
						(z*win[2] + z*win[5] + z*win[5] + z*win[8])) /
						(8.0 * ewres * scale));

					y = (float)(((z*win[6] + z*win[7] + z*win[7] + z*win[8]) - 
						(z*win[0] + z*win[1] + z*win[1] + z*win[2])) /
						(8.0 * nsres * scale));

					slope = (float)90.0 - atan(sqrt(x*x + y*y))*radiansToDegrees;
					
					// ... then aspect...
					aspect = atan2(x,y);			
					
					// ... then the shade value
					cang = (float)(sin(alt*degreesToRadians) * sin(slope*degreesToRadians) + 
						cos(alt*degreesToRadians) * cos(slope*degreesToRadians) * 
						cos((az-90.0)*degreesToRadians - aspect));

					if (cang <= 0.0) 
						cang = nullValue;
					else
						cang = static_cast<float>(255.0 * cang);

					shadeBuf[j] = cang;
					
				}
			}

			/* -----------------------------------------
			* Write Line to Raster
			*/
			poShadeBand->RasterIO( GF_Write, 0, i, nXSize, 1, 
						shadeBuf, nXSize, 1, GDT_Float32, 0, 0 );   
		}

		delete poShadeDS;
		retVal = true;
	}	
	
	catch(exception e)
	{
		retVal = false;
	}

	if (retVal)
		return S_OK;
	else
		return S_FALSE;

}

void CUtils::CreateBitmap(char * filename, long cols, long rows, VARIANT_BOOL * retval)
{
	GDALAllRegister();

    GDALDriver *poDriver;
    char **papszOptions = NULL;

    poDriver = GetGDALDriverManager()->GetDriverByName("BMP");

	if( poDriver == NULL )
	{
        *retval = S_FALSE;
		return;
	}

    rasterDataset = poDriver->Create( filename, cols, rows, 3, GDT_Byte, papszOptions );

	if (rasterDataset)
	{
		poBand_R = rasterDataset->GetRasterBand(1);
		poBand_G = rasterDataset->GetRasterBand(2);
		poBand_B = rasterDataset->GetRasterBand(3);
		if (poBand_R != NULL && poBand_G != NULL && poBand_B != NULL)
		{
			*retval = S_OK;
			return;
		}
	}

	*retval = S_FALSE;
}

inline void CUtils::PutBitmapValue(long col, long row, _int32 Rvalue, _int32 Gvalue, _int32 Bvalue, long totalWidth)
{
	if (BufferANum == row)
	{
		BufferA_R[col] = Rvalue;
		BufferA_G[col] = Gvalue;
		BufferA_B[col] = Bvalue;
		BufferLastUsed = 'A';
	}
	else if (BufferBNum == row)
	{
		BufferB_R[col] = Rvalue;
		BufferB_G[col] = Gvalue;
		BufferB_B[col] = Bvalue;
		BufferLastUsed = 'B';
	}
	else if (CanScanlineBuffer && BufferLastUsed != 'A') // If A wasn't the last used, replace it.
	{
		BufferLastUsed = 'A';
		// Write to row BufferANum
		poBand_R->RasterIO( GF_Write, 0, BufferANum, totalWidth, 1, BufferA_R, totalWidth, 1, GDT_Int32, 0, 0 );
		poBand_G->RasterIO( GF_Write, 0, BufferANum, totalWidth, 1, BufferA_G, totalWidth, 1, GDT_Int32, 0, 0 );
		poBand_B->RasterIO( GF_Write, 0, BufferANum, totalWidth, 1, BufferA_B, totalWidth, 1, GDT_Int32, 0, 0 );

		// Now that we're loading a different number into buffer,
		// reset BufferANum
		BufferANum = row;

		// Fetch the buffer rather than creating anew; data may have been written to it out of order.
		if (BufferA_R != NULL)
		{
			CPLFree(BufferA_R);
			BufferA_R = NULL;
		}

		BufferA_R = (_int32*) CPLMalloc( sizeof(_int32)*totalWidth);
		poBand_R->RasterIO( GF_Read, 0, row, totalWidth, 1, BufferA_R, totalWidth, 1, GDT_Int32, 0, 0 );

		if (BufferA_G != NULL)
		{
			CPLFree(BufferA_G);
			BufferA_G = NULL;
		}

		BufferA_G = (_int32*) CPLMalloc( sizeof(_int32)*totalWidth);
		poBand_G->RasterIO( GF_Read, 0, row, totalWidth, 1, BufferA_G, totalWidth, 1, GDT_Int32, 0, 0 );

		if (BufferA_B != NULL)
		{
			CPLFree(BufferA_B);
			BufferA_B = NULL;
		}

		BufferA_B = (_int32*) CPLMalloc( sizeof(_int32)*totalWidth);
		poBand_B->RasterIO( GF_Read, 0, row, totalWidth, 1, BufferA_B, totalWidth, 1, GDT_Int32, 0, 0 );

		// Finally, put the value.
		BufferA_R[col] = Rvalue;
		BufferA_G[col] = Gvalue;
		BufferA_B[col] = Bvalue;
	}
	else if (CanScanlineBuffer && BufferLastUsed != 'B') // If B wasn't the last used, replace it.
	{
		BufferLastUsed = 'B';
		// Write to row BufferANum
		poBand_R->RasterIO( GF_Write, 0, BufferBNum, totalWidth, 1, BufferB_R, totalWidth, 1, GDT_Int32, 0, 0 );
		poBand_G->RasterIO( GF_Write, 0, BufferBNum, totalWidth, 1, BufferB_G, totalWidth, 1, GDT_Int32, 0, 0 );
		poBand_B->RasterIO( GF_Write, 0, BufferBNum, totalWidth, 1, BufferB_B, totalWidth, 1, GDT_Int32, 0, 0 );

		// Now that we're loading a different number into buffer,
		// reset BufferBNum
		BufferBNum = row;

		// Fetch the buffer rather than creating anew; data may have been written to it out of order.
		if (BufferB_R != NULL)
		{
			CPLFree(BufferB_R);
			BufferB_R = NULL;
		}

		BufferB_R = (_int32*) CPLMalloc( sizeof(_int32)*totalWidth);
		poBand_R->RasterIO( GF_Read, 0, row, totalWidth, 1, BufferB_R, totalWidth, 1, GDT_Int32, 0, 0 );

		if (BufferB_G != NULL)
		{
			CPLFree(BufferB_G);
			BufferB_G = NULL;
		}

		BufferB_G = (_int32*) CPLMalloc( sizeof(_int32)*totalWidth);
		poBand_G->RasterIO( GF_Read, 0, row, totalWidth, 1, BufferB_G, totalWidth, 1, GDT_Int32, 0, 0 );

		if (BufferB_B != NULL)
		{
			CPLFree(BufferB_B);
			BufferB_B = NULL;
		}

		BufferB_B = (_int32*) CPLMalloc( sizeof(_int32)*totalWidth);
		poBand_B->RasterIO( GF_Read, 0, row, totalWidth, 1, BufferB_B, totalWidth, 1, GDT_Int32, 0, 0 );

		// Finally, put the value.
		BufferB_R[col] = Rvalue;
		BufferB_G[col] = Gvalue;
		BufferB_B[col] = Bvalue;
	}
	else
	{
		// Write directly to file
		poBand_R->RasterIO( GF_Write, col, row, 1, 1, &Rvalue, 1, 1, GDT_Int32, 0, 0 );
		poBand_G->RasterIO( GF_Write, col, row, 1, 1, &Gvalue, 1, 1, GDT_Int32, 0, 0 );
		poBand_B->RasterIO( GF_Write, col, row, 1, 1, &Bvalue, 1, 1, GDT_Int32, 0, 0 );
	}
}

void CUtils::FinalizeAndCloseBitmap(int totalWidth)
{
	if (BufferA_R != NULL && BufferA_G != NULL && BufferA_B != NULL && BufferANum != -1)
	{
		poBand_R->RasterIO( GF_Write, 0, BufferANum, totalWidth, 1, BufferA_R, totalWidth, 1, GDT_Int32, 0, 0 );
		poBand_G->RasterIO( GF_Write, 0, BufferANum, totalWidth, 1, BufferA_G, totalWidth, 1, GDT_Int32, 0, 0 );
		poBand_B->RasterIO( GF_Write, 0, BufferANum, totalWidth, 1, BufferA_B, totalWidth, 1, GDT_Int32, 0, 0 );
	}
	if (BufferB_R != NULL && BufferB_G != NULL && BufferB_B != NULL && BufferBNum != -1)
	{
		poBand_R->RasterIO( GF_Write, 0, BufferBNum, totalWidth, 1, BufferB_R, totalWidth, 1, GDT_Int32, 0, 0 );
		poBand_G->RasterIO( GF_Write, 0, BufferBNum, totalWidth, 1, BufferB_G, totalWidth, 1, GDT_Int32, 0, 0 );
		poBand_B->RasterIO( GF_Write, 0, BufferBNum, totalWidth, 1, BufferB_B, totalWidth, 1, GDT_Int32, 0, 0 );
	}

	if (BufferA_R != NULL)
	{
		CPLFree(BufferA_R);
		BufferA_R = NULL;
	}
	if (BufferA_G != NULL)
	{
		CPLFree(BufferA_G);
		BufferA_G = NULL;
	}
	if (BufferA_B != NULL)
	{
		CPLFree(BufferA_B);
		BufferA_B = NULL;
	}
	if (BufferB_R != NULL)
	{
		CPLFree(BufferB_R);
		BufferB_R = NULL;
	}
	if (BufferB_G != NULL)
	{
		CPLFree(BufferB_G);
		BufferB_G = NULL;
	}
	if (BufferB_B != NULL)
	{
		CPLFree(BufferB_B);
		BufferB_B = NULL;
	}

	if (rasterDataset != NULL)
	{
		delete rasterDataset;
		rasterDataset = NULL;
	}
}

bool CUtils::MemoryAvailable(double bytes)
{
  MEMORYSTATUS stat;

  GlobalMemoryStatus (&stat);

  if (stat.dwAvailPhys >= bytes)
	  return true;

  return false;
}

// Generate Contour
// This was swiped from the GDAL Tools generate_contour, written by Frank Warmerdam.
/******************************************************************************
 * $Id: gdal_contour.cpp,v 1.10 2005/02/03 17:28:37 fwarmerdam Exp $
 *
 * Project:  Contour Generator
 * Purpose:  Contour Generator mainline.
 * Author:   Frank Warmerdam <warmerdam@pobox.com>
 *
 ******************************************************************************
 * Copyright (c) 2003, Applied Coherent Technology (www.actgate.com). 
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 ******************************************************************************/

STDMETHODIMP CUtils::GenerateContour(BSTR pszSrcFilename, BSTR pszDstFilename, double dfInterval, double dfNoData, VARIANT_BOOL Is3D, VARIANT dblFLArray, ICallback * cBack, VARIANT_BOOL *retval)
{
	USES_CONVERSION;

	GDALDatasetH hSrcDS;
    int b3D = FALSE;
    int nBandIn = 1;
    double dfOffset = 0.0;
    const char *pszElevAttrib = "Grid_value";
    const char *pszFormat = "ESRI Shapefile";
    double adfFixedLevels[1000];
    int    nFixedLevelCount = 0;

	if (Is3D == VARIANT_TRUE)
		b3D = TRUE;

    GDALAllRegister();
    OGRRegisterAll();

	//Added by Rob Cairns 4-Mar-06
	if ( dblFLArray.vt == (VT_ARRAY | VT_R8) )
	{	
		long Dims = SafeArrayGetDim(dblFLArray.parray);
		long UpperBounds;
		long LowerBounds;
		SafeArrayGetLBound(dblFLArray.parray, 1, &LowerBounds);
		SafeArrayGetUBound(dblFLArray.parray, 1, &UpperBounds);
		if (Dims ==1 && UpperBounds > 0) 
		{
			double *arrVal;
			SafeArrayAccessData (dblFLArray.parray, (void**)&arrVal);
			for (int i = 0; i < UpperBounds; i++) 
			{
				adfFixedLevels[i] = arrVal[i];
				nFixedLevelCount++;
				arrVal[i] = 0;
			}
			SafeArrayUnaccessData (dblFLArray.parray);
		}
	}


    if( dfInterval == 0.0 && nFixedLevelCount == 0 )
    {
		(*retval) = VARIANT_FALSE;
        return S_FALSE;
    }

/* -------------------------------------------------------------------- */
/*      Open source raster file.                                        */
/* -------------------------------------------------------------------- */
    GDALRasterBandH hBand;

    hSrcDS = GDALOpen( OLE2A(pszSrcFilename), GA_ReadOnly );
    if( hSrcDS == NULL )
	{
		(*retval) = VARIANT_FALSE;
		return S_FALSE;
	}

    hBand = GDALGetRasterBand( hSrcDS, nBandIn );
    if( hBand == NULL )
    {
        CPLError( CE_Failure, CPLE_AppDefined, 
                  "Band %d does not exist on dataset.", 
                  nBandIn );
		(*retval) = VARIANT_FALSE;
		return S_FALSE;
    }

    //if( !bNoDataSet && !bIgnoreNoData )
	/*int bNoDataSet = FALSE;
    double val = GDALGetRasterNoDataValue( hBand, &bNoDataSet );
	if (bNoDataSet)	
		dfNoData = val;*/

/* -------------------------------------------------------------------- */
/*      Try to get a coordinate system from the raster.                 */
/* -------------------------------------------------------------------- */
    OGRSpatialReferenceH hSRS = NULL;

    const char *pszWKT = GDALGetProjectionRef( hBand );

    if( pszWKT != NULL && _tcslen(pszWKT) != 0 )
        hSRS = OSRNewSpatialReference( pszWKT );

/* -------------------------------------------------------------------- */
/*      Create the outputfile.                                          */
/* -------------------------------------------------------------------- */
    OGRDataSourceH hDS;
    OGRSFDriverH hDriver = OGRGetDriverByName( pszFormat );
    OGRFieldDefnH hFld;
    OGRLayerH hLayer;
    int nElevField = -1;

    if( hDriver == NULL )
    {
        //fprintf( stderr, "Unable to find format driver named %s.\n", 
        //         pszFormat );
		(*retval) = VARIANT_FALSE;
        return S_FALSE;
    }

    hDS = OGR_Dr_CreateDataSource( hDriver, OLE2A(pszDstFilename), NULL );
    if( hDS == NULL )
	{
		(*retval) = VARIANT_FALSE;
        return S_FALSE;
	}

    hLayer = OGR_DS_CreateLayer( hDS, "contour", hSRS, 
                                 b3D ? wkbLineString25D : wkbLineString,
                                 NULL );
    if( hLayer == NULL )
	{
		(*retval) = VARIANT_FALSE;
        return S_FALSE;
	}

    hFld = OGR_Fld_Create( "ID", OFTInteger );
    OGR_Fld_SetWidth( hFld, 8 );
    OGR_L_CreateField( hLayer, hFld, FALSE );
    OGR_Fld_Destroy( hFld );

    if( pszElevAttrib )
    {
        hFld = OGR_Fld_Create( pszElevAttrib, OFTReal );
        OGR_Fld_SetWidth( hFld, 12 );
        OGR_Fld_SetPrecision( hFld, 3 );
        OGR_L_CreateField( hLayer, hFld, FALSE );
        OGR_Fld_Destroy( hFld );
        nElevField = 1;
    }

/* -------------------------------------------------------------------- */
/*      Invoke.                                                         */
/* -------------------------------------------------------------------- */
    CPLErr eErr;
    
	bool bNoDataSet = true;
    eErr = GDALContourGenerate( hBand, dfInterval, dfOffset, 
                                nFixedLevelCount, adfFixedLevels,
                                bNoDataSet, dfNoData, 
                                hLayer, 0, nElevField,
                                GDALTermProgress, NULL );

    OGR_DS_Destroy( hDS );
    GDALClose( hSrcDS );

	(*retval) = VARIANT_TRUE;
	return S_OK;
}


/* ****************************************************************************
 * $Id: gdal_translate.cpp,v 1.32 2005/05/17 19:04:47 fwarmerdam Exp $
 *
 * Project:  GDAL Utilities
 * Purpose:  GDAL Image Translator Program
 * Author:   Frank Warmerdam, warmerdam@pobox.com
 *
 * ****************************************************************************
 * Copyright (c) 1998, 2002, Frank Warmerdam
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 * ****************************************************************************/

STDMETHODIMP CUtils::TranslateRaster(BSTR bstrSrcFilename, BSTR bstrDstFilename, BSTR bstrOptions, ICallback * cBack, VARIANT_BOOL *retval)
{
	USES_CONVERSION;
    GDALDatasetH	hDataset, hOutDS;
    int				i;
    int				nRasterXSize, nRasterYSize;
    const char		*pszSource=NULL, *pszDest=NULL, *pszFormat = "GTiff";
    GDALDriverH		hDriver;
    int				*panBandList = NULL, nBandCount = 0, bDefBands = TRUE;
    double			adfGeoTransform[6];
    GDALDataType	eOutputType = GDT_Unknown;
    int				nOXSize = 0, nOYSize = 0;
    char			*pszOXSize=NULL, *pszOYSize=NULL;
    char            **papszCreateOptions = NULL;
    int             anSrcWin[4], bStrict = TRUE;
    const char      *pszProjection;
    int             bScale = FALSE, bHaveScaleSrc = FALSE;
    double	        dfScaleSrcMin=0.0, dfScaleSrcMax=255.0;
    double          dfScaleDstMin=0.0, dfScaleDstMax=255.0;
    double          dfULX, dfULY, dfLRX, dfLRY;
    char            **papszMetadataOptions = NULL;
    char            *pszOutputSRS = NULL;
    int             bQuiet = FALSE, bGotBounds = FALSE;
	GDALProgressFunc    pfnProgress = GDALTermProgress;
    int             nGCPCount = 0;
    GDAL_GCP        *pasGCPs = NULL;
    int             iSrcFileArg = -1, iDstFileArg = -1;
    int             bCopySubDatasets = FALSE;
    double          adfULLR[4];
    int             bSetNoData = FALSE;
    double			dfNoDataReal = 0.0;
	int				opts=0;

	
	(*retval) = VARIANT_FALSE;
	try
	{
		anSrcWin[0] = 0;
		anSrcWin[1] = 0;
		anSrcWin[2] = 0;
		anSrcWin[3] = 0;

		dfULX = dfULY = dfLRX = dfLRY = 0.0;

	/* -------------------------------------------------------------------- */
	/*      Register standard GDAL drivers, and process generic GDAL        */
	/*      command options.                                                */
	/* -------------------------------------------------------------------- */
		GDALAllRegister();
	    
		Parse(OLE2CA(bstrOptions), OLE2CA(bstrSrcFilename), OLE2CA(bstrDstFilename), &opts);

		if( opts < 1 )
		{	(*retval) = VARIANT_FALSE;
			return S_OK;
		}
	 
	/* -------------------------------------------------------------------- */
	/*      Handle command line arguments.                                  */
	/* -------------------------------------------------------------------- */
		for( i = 1; i < opts; i++ )
		{
       		if( _stricmp(sArr[i],"-of")==0 && i < opts-1 )
				pszFormat = sArr[++i];

			else if( _stricmp(sArr[i],"-quiet")==0 )
			{
				bQuiet = TRUE;
				pfnProgress = GDALDummyProgress;
			}

			else if( _stricmp(sArr[i],"-ot")==0 && i < opts-1 )
			{
				int	iType;
	            
				for( iType = 1; iType < GDT_TypeCount; iType++ )
				{
					if( GDALGetDataTypeName((GDALDataType)iType) != NULL
						&& _stricmp(GDALGetDataTypeName((GDALDataType)iType),
								sArr[i+1])==0 )
					{
						eOutputType = (GDALDataType) iType;
					}
				}

				if( eOutputType == GDT_Unknown )
				{
					CString tmpCstr = "Unknown output pixel type: " + sArr[i+1] +"\n";
					if (!bQuiet)
						Usage(tmpCstr);
					GDALDestroyDriverManager();
					(*retval) = VARIANT_FALSE;
					return S_OK;
				}
				i++;
			}
			else if( _stricmp(sArr[i],"-b")==0 && i < opts-1 )
			{
				if( atoi(sArr[i+1]) < 1 )
				{
					CString tmpCstr = "Unrecognizable band number: " + sArr[i+1] + "\n";
					if (!bQuiet)
						Usage(tmpCstr);
					GDALDestroyDriverManager();
					(*retval) = VARIANT_FALSE;
					return S_OK;
				}

				nBandCount++;
				panBandList = (int *) 
					CPLRealloc(panBandList, sizeof(int) * nBandCount);
				panBandList[nBandCount-1] = atoi(sArr[++i]);

				if( panBandList[nBandCount-1] != nBandCount )
					bDefBands = FALSE;
			}
			else if( _stricmp(sArr[i],"-not_strict")==0  )
				bStrict = FALSE;
	            
			else if( _stricmp(sArr[i],"-sds")==0  )
				bCopySubDatasets = TRUE;
	            
			else if( _stricmp(sArr[i],"-gcp")==0 && i < opts - 4 )
			{
				/* -gcp pixel line easting northing [elev] */

				nGCPCount++;
				pasGCPs = (GDAL_GCP *) 
					CPLRealloc( pasGCPs, sizeof(GDAL_GCP) * nGCPCount );
				GDALInitGCPs( 1, pasGCPs + nGCPCount - 1 );

				pasGCPs[nGCPCount-1].dfGCPPixel = atof(sArr[++i]);
				pasGCPs[nGCPCount-1].dfGCPLine = atof(sArr[++i]);
				pasGCPs[nGCPCount-1].dfGCPX = atof(sArr[++i]);
				pasGCPs[nGCPCount-1].dfGCPY = atof(sArr[++i]);
				char * tmpStr = (char *)(LPCTSTR) sArr[i+1];
				if( tmpStr != NULL 
					&& (atof(tmpStr) != 0.0 || tmpStr[0] == '0') )
					pasGCPs[nGCPCount-1].dfGCPZ = atof(tmpStr);
				//if( sArr[i+1] != NULL 
				//    && (atof(sArr[i+1]) != 0.0 || sArr[i+1][0] == '0') )
				//    pasGCPs[nGCPCount-1].dfGCPZ = atof(sArr[++i]);

				/* should set id and info? */
			}   

			else if( _stricmp(sArr[i],"-a_nodata")==0 && i < opts - 1 )
			{
				bSetNoData = TRUE;
				dfNoDataReal = atof(sArr[i+1]);
				i += 1;
			}   

			else if( _stricmp(sArr[i],"-a_ullr")==0 && i < opts - 4 )
			{
				adfULLR[0] = atof(sArr[i+1]);
				adfULLR[1] = atof(sArr[i+2]);
				adfULLR[2] = atof(sArr[i+3]);
				adfULLR[3] = atof(sArr[i+4]);

				bGotBounds = TRUE;
	            
				i += 4;
			}   

			else if( _stricmp(sArr[i],"-co")==0 && i < opts-1 )
			{
				papszCreateOptions = CSLAddString( papszCreateOptions, sArr[++i] );
			}   

			else if( _stricmp(sArr[i],"-scale")==0 )
			{
				bScale = TRUE;
				if( i < opts-2 && ArgIsNumeric(sArr[i+1]) )
				{
					bHaveScaleSrc = TRUE;
					dfScaleSrcMin = atof(sArr[i+1]);
					dfScaleSrcMax = atof(sArr[i+2]);
					i += 2;
				}
				if( i < opts-2 && bHaveScaleSrc && ArgIsNumeric(sArr[i+1]) )
				{
					dfScaleDstMin = atof(sArr[i+1]);
					dfScaleDstMax = atof(sArr[i+2]);
					i += 2;
				}
				else
				{
					dfScaleDstMin = 0.0;
					dfScaleDstMax = 255.999;
				}
			}   

			else if( _stricmp(sArr[i],"-mo")==0 && i < opts-1 )
			{
				papszMetadataOptions = CSLAddString( papszMetadataOptions,
													sArr[++i] );
			}

			else if( _stricmp(sArr[i],"-outsize")==0 && i < opts-2 )
			{
				pszOXSize = (char *) (LPCTSTR) sArr[++i];
				pszOYSize = (char *) (LPCTSTR) sArr[++i];
			}   

			else if( _stricmp(sArr[i],"-srcwin")==0 && i < opts-4 )
			{
				anSrcWin[0] = atoi(sArr[++i]);
				anSrcWin[1] = atoi(sArr[++i]);
				anSrcWin[2] = atoi(sArr[++i]);
				anSrcWin[3] = atoi(sArr[++i]);
			}   

			else if( _stricmp(sArr[i],"-projwin")==0 && i < opts-4 )
			{
				dfULX = atof(sArr[++i]);
				dfULY = atof(sArr[++i]);
				dfLRX = atof(sArr[++i]);
				dfLRY = atof(sArr[++i]);
			}   

			else if( _stricmp(sArr[i],"-a_srs")==0 && i < opts-1 )
			{
				OGRSpatialReference oOutputSRS;

				if( oOutputSRS.SetFromUserInput( sArr[i+1] ) != OGRERR_NONE )
				{
					fprintf( stderr, "Failed to process SRS definition: %s\n", 
							sArr[i+1] );
					GDALDestroyDriverManager();
					(*retval) = VARIANT_FALSE;
					return S_OK;
				}

				oOutputSRS.exportToWkt( &pszOutputSRS );
				i++;
			}   

			else if( sArr[i][0] == '-' )
			{
				CString tmpCstr = "Option " + sArr[i] + "incomplete, or not recognised.\n";
				if (!bQuiet)
					Usage(tmpCstr);
				GDALDestroyDriverManager();
				(*retval) = VARIANT_FALSE;
				return S_OK;
			}
			else if( pszSource == NULL )
			{
				iSrcFileArg = i;
				pszSource = (char *) (LPCTSTR) sArr[i];
			}
			else if( pszDest == NULL )
			{
 				pszDest = (char *) (LPCTSTR) sArr[i];
				iDstFileArg = i;
			}

			else
			{
				CString tmpCstr = "Too many command options.\n";
				if (!bQuiet)
					Usage(tmpCstr);
				GDALDestroyDriverManager();
				(*retval) = VARIANT_FALSE;
				return S_OK;
			}
		}

		if( pszDest == NULL )
		{
 			if (!bQuiet)
				Usage("Destination file name is missing\n");
			GDALDestroyDriverManager();
			(*retval) = VARIANT_FALSE;
			return S_OK;
		}

	/* -------------------------------------------------------------------- */
	/*      Attempt to open source file.                                    */
	/* -------------------------------------------------------------------- */

		hDataset = GDALOpenShared( pszSource, GA_ReadOnly );
	    
		if( hDataset == NULL )
		{
			fprintf( stderr,
					"GDALOpen failed - %d\n%s\n",
					CPLGetLastErrorNo(), CPLGetLastErrorMsg() );
			GDALDestroyDriverManager();
			(*retval) = VARIANT_FALSE;
			return S_OK;
		}

	/* -------------------------------------------------------------------- */
	/*      Handle subdatasets.                                             */
	/* -------------------------------------------------------------------- */
		if( CSLCount(GDALGetMetadata( hDataset, "SUBDATASETS" )) > 0 )
		{
			if( !bCopySubDatasets )
			{
				fprintf( stderr,
						"Input file contains subdatasets. Please, select one of them for reading.\n" );
			}
			else
			{
				char **papszSubdatasets = GDALGetMetadata(hDataset,"SUBDATASETS");
				char *pszSubDest = (char *) CPLMalloc(_tcslen(pszDest)+32);
				int i;
				bool bOldSubCall = bSubCall;

				//sArr[iDstFileArg] = pszSubDest;
				sArr.RemoveAt(iDstFileArg);
				bstrDstFilename = (_bstr_t) pszSubDest;

				bSubCall = TRUE;
				for( i = 0; papszSubdatasets[i] != NULL; i += 2 )
				{
					//sArr.[iSrcFileArg] = strstr(papszSubdatasets[i],"=")+1;
            		sArr.RemoveAt(iSrcFileArg);
					char * tmpChar = strstr(papszSubdatasets[i],"=")+1;
					bstrSrcFilename = _bstr_t( tmpChar);
				
					sprintf( pszSubDest, "%s%d", pszDest, i/2 + 1 );
					VARIANT_BOOL * tmpBool = 0;
					TranslateRaster(bstrSrcFilename, bstrDstFilename, bstrOptions, NULL, tmpBool );
					if (tmpBool != 0 )
						break;
				}

				bSubCall = bOldSubCall;
				CPLFree( pszSubDest );
			}

			GDALClose( hDataset );

			if( !bSubCall )
			{
				GDALDumpOpenDatasets( stderr );
				GDALDestroyDriverManager();
			}
			return 1;
		}

	/* -------------------------------------------------------------------- */
	/*      Collect some information from the source file.                  */
	/* -------------------------------------------------------------------- */
		nRasterXSize = GDALGetRasterXSize( hDataset );
		nRasterYSize = GDALGetRasterYSize( hDataset );

		if( !bQuiet )
			printf( "Input file size is %d, %d\n", nRasterXSize, nRasterYSize );

		if( anSrcWin[2] == 0 && anSrcWin[3] == 0 )
		{
			anSrcWin[2] = nRasterXSize;
			anSrcWin[3] = nRasterYSize;
		}

	/* -------------------------------------------------------------------- */
	/*	Build band list to translate					*/
	/* -------------------------------------------------------------------- */
		if( nBandCount == 0 )
		{
			nBandCount = GDALGetRasterCount( hDataset );
			panBandList = (int *) CPLMalloc(sizeof(int)*nBandCount);
			for( i = 0; i < nBandCount; i++ )
				panBandList[i] = i+1;
		}
		else
		{
			for( i = 0; i < nBandCount; i++ )
			{
				if( panBandList[i] < 1 || panBandList[i] > GDALGetRasterCount(hDataset) )
				{
					fprintf( stderr, 
							"Band %d requested, but only bands 1 to %d available.\n",
							panBandList[i], GDALGetRasterCount(hDataset) );
					GDALDestroyDriverManager();
					(*retval) = VARIANT_FALSE;
					return S_OK;
				}
			}

			if( nBandCount != GDALGetRasterCount( hDataset ) )
				bDefBands = FALSE;
		}

	/* -------------------------------------------------------------------- */
	/*      Compute the source window from the projected source window      */
	/*      if the projected coordinates were provided.  Note that the      */
	/*      projected coordinates are in ulx, uly, lrx, lry format,         */
	/*      while the anSrcWin is xoff, yoff, xsize, ysize with the         */
	/*      xoff,yoff being the ulx, uly in pixel/line.                     */
	/* -------------------------------------------------------------------- */
		if( dfULX != 0.0 || dfULY != 0.0 
			|| dfLRX != 0.0 || dfLRY != 0.0 )
		{
			double	adfGeoTransform[6];

			GDALGetGeoTransform( hDataset, adfGeoTransform );

			if( adfGeoTransform[2] != 0.0 || adfGeoTransform[4] != 0.0 )
			{
				fprintf( stderr, 
						"The -projwin option was used, but the geotransform is\n"
						"rotated.  This configuration is not supported.\n" );
				GDALClose( hDataset );
				CPLFree( panBandList );
				GDALDestroyDriverManager();
				(*retval) = VARIANT_FALSE;
				return S_OK;
			}

			anSrcWin[0] = (int) 
				((dfULX - adfGeoTransform[0]) / adfGeoTransform[1] + 0.001);
			anSrcWin[1] = (int) 
				((dfULY - adfGeoTransform[3]) / adfGeoTransform[5] + 0.001);

			anSrcWin[2] = (int) ((dfLRX - dfULX) / adfGeoTransform[1] + 0.5);
			anSrcWin[3] = (int) ((dfLRY - dfULY) / adfGeoTransform[5] + 0.5);

			if( !bQuiet )
				fprintf( stdout, 
						"Computed -srcwin %d %d %d %d from projected window.\n",
						anSrcWin[0], 
						anSrcWin[1], 
						anSrcWin[2], 
						anSrcWin[3] );
	        
			if( anSrcWin[0] < 0 || anSrcWin[1] < 0 
				|| anSrcWin[0] + anSrcWin[2] > GDALGetRasterXSize(hDataset) 
				|| anSrcWin[1] + anSrcWin[3] > GDALGetRasterYSize(hDataset) )
			{
				fprintf( stderr, 
						"Computed -srcwin falls outside raster size of %dx%d.\n",
						GDALGetRasterXSize(hDataset), 
						GDALGetRasterYSize(hDataset) );
				(*retval) = VARIANT_FALSE;
				return S_OK;
			}
		}

	/* -------------------------------------------------------------------- */
	/*      Verify source window.                                           */
	/* -------------------------------------------------------------------- */
		if( anSrcWin[0] < 0 || anSrcWin[1] < 0 
			|| anSrcWin[2] <= 0 || anSrcWin[3] <= 0
			|| anSrcWin[0] + anSrcWin[2] > GDALGetRasterXSize(hDataset) 
			|| anSrcWin[1] + anSrcWin[3] > GDALGetRasterYSize(hDataset) )
		{
			fprintf( stderr, 
					"-srcwin %d %d %d %d falls outside raster size of %dx%d\n"
					"or is otherwise illegal.\n",
					anSrcWin[0],
					anSrcWin[1],
					anSrcWin[2],
					anSrcWin[3],
					GDALGetRasterXSize(hDataset), 
					GDALGetRasterYSize(hDataset) );
		(*retval) = VARIANT_FALSE;
			return S_OK;
		}

	/* -------------------------------------------------------------------- */
	/*      Find the output driver.                                         */
	/* -------------------------------------------------------------------- */
		hDriver = GDALGetDriverByName( pszFormat );
		if( hDriver == NULL )
		{
			int	iDr;
	        
			printf( "Output driver `%s' not recognised.\n", pszFormat );
			printf( "The following format drivers are configured and support output:\n" );
			for( iDr = 0; iDr < GDALGetDriverCount(); iDr++ )
			{
				GDALDriverH hDriver = GDALGetDriver(iDr);

				if( GDALGetMetadataItem( hDriver, GDAL_DCAP_CREATE, NULL ) != NULL
					|| GDALGetMetadataItem( hDriver, GDAL_DCAP_CREATECOPY,
											NULL ) != NULL )
				{
					printf( "  %s: %s\n",
							GDALGetDriverShortName( hDriver  ),
							GDALGetDriverLongName( hDriver ) );
				}
			}
			printf( "\n" );
			if (!bQuiet)
				Usage("Output driver not recognised\n");
	        
			GDALClose( hDataset );
			CPLFree( panBandList );
			GDALDestroyDriverManager();
			//CSLDestroy( sArr );
			int j = 0;
			while (j < sArr.GetSize() )
			{
				delete sArr.GetAt( j++ );
			}
			sArr.RemoveAll();

			CSLDestroy( papszCreateOptions );
			(*retval) = VARIANT_FALSE;
			return S_OK;
		}

	/* -------------------------------------------------------------------- */
	/*      The short form is to CreateCopy().  We use this if the input    */
	/*      matches the whole dataset.  Eventually we should rewrite        */
	/*      this entire program to use virtual datasets to construct a      */
	/*      virtual input source to copy from.                              */
	/* -------------------------------------------------------------------- */
		if( eOutputType == GDT_Unknown 
			&& !bScale && CSLCount(papszMetadataOptions) == 0 && bDefBands 
			&& anSrcWin[0] == 0 && anSrcWin[1] == 0 
			&& anSrcWin[2] == GDALGetRasterXSize(hDataset)
			&& anSrcWin[3] == GDALGetRasterYSize(hDataset) 
			&& pszOXSize == NULL && pszOYSize == NULL 
			&& nGCPCount == 0 && !bGotBounds
			&& pszOutputSRS == NULL && !bSetNoData )
		{
	        
			hOutDS = GDALCreateCopy( hDriver, pszDest, hDataset, 
									bStrict, papszCreateOptions, 
									pfnProgress, NULL );

			if( hOutDS != NULL )
			{
				GDALClose( hOutDS );
				(*retval) = VARIANT_TRUE;
			}
			else
			{
				(*retval) = VARIANT_FALSE;
			}
	        
			GDALClose( hDataset );

			CPLFree( panBandList );

			if( !bSubCall )
			{
				GDALDumpOpenDatasets( stderr );
				GDALDestroyDriverManager();
			}

			//CSLDestroy( sArr );
			int j = 0;
			while (j < sArr.GetSize() )
			{
				delete sArr.GetAt( j++ );
			}
			sArr.RemoveAll();

			CSLDestroy( papszCreateOptions );
			
			return S_OK;
		}

	/* -------------------------------------------------------------------- */
	/*      Establish some parameters.                                      */
	/* -------------------------------------------------------------------- */
		if( pszOXSize == NULL )
		{
			nOXSize = anSrcWin[2];
			nOYSize = anSrcWin[3];
		}
		else
		{
			nOXSize = (int) ((pszOXSize[_tcslen(pszOXSize)-1]=='%' 
							? atof(pszOXSize)/100*anSrcWin[2] : atoi(pszOXSize)));
			nOYSize = (int) ((pszOYSize[_tcslen(pszOYSize)-1]=='%' 
							? atof(pszOYSize)/100*anSrcWin[3] : atoi(pszOYSize)));
		}
	    
	/* ==================================================================== */
	/*      Create a virtual dataset.                                       */
	/* ==================================================================== */
		VRTDataset *poVDS;
	        
	/* -------------------------------------------------------------------- */
	/*      Make a virtual clone.                                           */
	/* -------------------------------------------------------------------- */
		poVDS = new VRTDataset( nOXSize, nOYSize );

		if( nGCPCount == 0 )
		{
			if( pszOutputSRS != NULL )
			{
				poVDS->SetProjection( pszOutputSRS );
			}
			else
			{
				pszProjection = GDALGetProjectionRef( hDataset );
				if( pszProjection != NULL && _tcslen(pszProjection) > 0 )
					poVDS->SetProjection( pszProjection );
			}
		}

		if( bGotBounds )
		{
			adfGeoTransform[0] = adfULLR[0];
			adfGeoTransform[1] = (adfULLR[2] - adfULLR[0]) / nOXSize;
			adfGeoTransform[2] = 0.0;
			adfGeoTransform[3] = adfULLR[1];
			adfGeoTransform[4] = 0.0;
			adfGeoTransform[5] = (adfULLR[3] - adfULLR[1]) / nOYSize;

			poVDS->SetGeoTransform( adfGeoTransform );
		}

		else if( GDALGetGeoTransform( hDataset, adfGeoTransform ) == CE_None 
			&& nGCPCount == 0 )
		{
			adfGeoTransform[0] += anSrcWin[0] * adfGeoTransform[1]
				+ anSrcWin[1] * adfGeoTransform[2];
			adfGeoTransform[3] += anSrcWin[0] * adfGeoTransform[4]
				+ anSrcWin[1] * adfGeoTransform[5];
	        
			adfGeoTransform[1] *= anSrcWin[2] / (double) nOXSize;
			adfGeoTransform[2] *= anSrcWin[3] / (double) nOYSize;
			adfGeoTransform[4] *= anSrcWin[2] / (double) nOXSize;
			adfGeoTransform[5] *= anSrcWin[3] / (double) nOYSize;
	        
			poVDS->SetGeoTransform( adfGeoTransform );
		}

		if( nGCPCount != 0 )
		{
			const char *pszGCPProjection = pszOutputSRS;

			if( pszGCPProjection == NULL )
				pszGCPProjection = GDALGetGCPProjection( hDataset );
			if( pszGCPProjection == NULL )
				pszGCPProjection = "";

			poVDS->SetGCPs( nGCPCount, pasGCPs, pszGCPProjection );

			GDALDeinitGCPs( nGCPCount, pasGCPs );
			CPLFree( pasGCPs );
		}

		else if( GDALGetGCPCount( hDataset ) > 0 )
		{
			GDAL_GCP *pasGCPs;
			int       nGCPs = GDALGetGCPCount( hDataset );

			pasGCPs = GDALDuplicateGCPs( nGCPs, GDALGetGCPs( hDataset ) );

			for( i = 0; i < nGCPs; i++ )
			{
				pasGCPs[i].dfGCPPixel -= anSrcWin[0];
				pasGCPs[i].dfGCPLine  -= anSrcWin[1];
				pasGCPs[i].dfGCPPixel *= (nOXSize / (double) anSrcWin[2] );
				pasGCPs[i].dfGCPLine  *= (nOYSize / (double) anSrcWin[3] );
			}
	            
			poVDS->SetGCPs( nGCPs, pasGCPs,
							GDALGetGCPProjection( hDataset ) );

			GDALDeinitGCPs( nGCPs, pasGCPs );
			CPLFree( pasGCPs );
		}

		poVDS->SetMetadata( ((GDALDataset*)hDataset)->GetMetadata() );
		AttachMetadata( (GDALDatasetH) poVDS, papszMetadataOptions );

		for( i = 0; i < nBandCount; i++ )
		{
			VRTSourcedRasterBand   *poVRTBand;
			GDALRasterBand  *poSrcBand;
			GDALDataType    eBandType;

			poSrcBand = ((GDALDataset *) 
						hDataset)->GetRasterBand(panBandList[i]);

	/* -------------------------------------------------------------------- */
	/*      Select output data type to match source.                        */
	/* -------------------------------------------------------------------- */
			if( eOutputType == GDT_Unknown )
				eBandType = poSrcBand->GetRasterDataType();
			else
				eBandType = eOutputType;

	/* -------------------------------------------------------------------- */
	/*      Create this band.                                               */
	/* -------------------------------------------------------------------- */
			poVDS->AddBand( eBandType, NULL );
			poVRTBand = (VRTSourcedRasterBand *) poVDS->GetRasterBand( i+1 );
	            
	/* -------------------------------------------------------------------- */
	/*      Do we need to collect scaling information?                      */
	/* -------------------------------------------------------------------- */
			double dfScale=1.0, dfOffset=0.0;

			if( bScale && !bHaveScaleSrc )
			{
				double	adfCMinMax[2];
				GDALComputeRasterMinMax( poSrcBand, TRUE, adfCMinMax );
				dfScaleSrcMin = adfCMinMax[0];
				dfScaleSrcMax = adfCMinMax[1];
			}

			if( bScale )
			{
				if( dfScaleSrcMax == dfScaleSrcMin )
					dfScaleSrcMax += 0.1;
				if( dfScaleDstMax == dfScaleDstMin )
					dfScaleDstMax += 0.1;

				dfScale = (dfScaleDstMax - dfScaleDstMin) 
					/ (dfScaleSrcMax - dfScaleSrcMin);
				dfOffset = -1 * dfScaleSrcMin * dfScale + dfScaleDstMin;
			}

	/* -------------------------------------------------------------------- */
	/*      Create a simple or complex data source depending on the         */
	/*      translation type required.                                      */
	/* -------------------------------------------------------------------- */
				poVRTBand->AddSimpleSource( poSrcBand,
											anSrcWin[0], anSrcWin[1], 
											anSrcWin[2], anSrcWin[3], 
											0, 0, nOXSize, nOYSize );

	/* -------------------------------------------------------------------- */
	/*      copy over some other information of interest.                   */
	/* -------------------------------------------------------------------- */
			poVRTBand->CopyCommonInfoFrom( poSrcBand );

	/* -------------------------------------------------------------------- */
	/*      Set a forcable nodata value?                                    */
	/* -------------------------------------------------------------------- */
			if( bSetNoData )
				poVRTBand->SetNoDataValue( dfNoDataReal );
		}

	/* -------------------------------------------------------------------- */
	/*      Write to the output file using CopyCreate().                    */
	/* -------------------------------------------------------------------- */
		hOutDS = GDALCreateCopy( hDriver, pszDest, (GDALDatasetH) poVDS,
								bStrict, papszCreateOptions, 
								pfnProgress, NULL );
		if( hOutDS != NULL )
		{
			GDALClose( hOutDS );
			(*retval) = VARIANT_TRUE;
		}
		else
		{
			(*retval) = VARIANT_FALSE;
		}
		GDALClose( (GDALDatasetH) poVDS );
	        
		GDALClose( hDataset );

		CPLFree( panBandList );

		if( !bSubCall )
		{
			GDALDumpOpenDatasets( stderr );
			GDALDestroyDriverManager();
		}

		//CSLDestroy( sArr );
		int j = 0;
		while (j < sArr.GetSize() )
		{
			delete sArr.GetAt( j++ );
		}
		sArr.RemoveAll();

		CSLDestroy( papszCreateOptions );
		return S_OK;
	}
	catch(exception e)
	{
		if( hOutDS != NULL )
			GDALClose( hOutDS );
		if( hDataset != NULL )
			GDALClose( hDataset );
		CPLFree( panBandList );
		if( !bSubCall )
		{
			GDALDumpOpenDatasets( stderr );
			GDALDestroyDriverManager();
		}
		int j = 0;
		while (j < sArr.GetSize() )
		{
			delete sArr.GetAt( j++ );
		}
		sArr.RemoveAll();
		(*retval) = VARIANT_FALSE;
		return S_OK;
	}

}

/************************************************************************/
/*                            ArgIsNumeric()                            */
/************************************************************************/

bool CUtils::ArgIsNumeric( const char *pszArg )

{
    if( pszArg[0] == '-' )
        pszArg++;

    if( *pszArg == '\0' )
        return false;

    while( *pszArg != '\0' )
    {
        if( (*pszArg < '0' || *pszArg > '9') && *pszArg != '.' )
            return false;
        pszArg++;
    }
        
    return true;
}

/************************************************************************/
/*                           AttachMetadata()                           */
/************************************************************************/

void CUtils::AttachMetadata( GDALDatasetH hDS, char **papszMetadataOptions )
{
    int nCount = CSLCount(papszMetadataOptions);
    int i;

    for( i = 0; i < nCount; i++ )
    {
        char    *pszKey = NULL;
        const char *pszValue;
        
        pszValue = CPLParseNameValue( papszMetadataOptions[i], &pszKey );
        GDALSetMetadataItem(hDS,pszKey,pszValue,NULL);
        CPLFree( pszKey );
    }

    CSLDestroy( papszMetadataOptions );
}

/************************************************************************/
/*                           Parse()                                    */
/************************************************************************/
void CUtils::Parse(CString sOrig, CString inFile, CString outFile, int * opts)
{
	CString sTemp, sTrans, sStore;
	int m, i; 
	char chSeps[] = " ";
	
	//set an initial max array size
	sArr.SetSize(sOrig.GetLength(),25);
	sArr[0] = "Dummy value at 0";
	i=0;
	while(1)
	{	
		i++;
		m = sOrig.FindOneOf( (LPCTSTR)chSeps );
		if (m != -1)
		{
			sTemp = sOrig.Mid(0, m);
			sArr[i] = sTemp;
			sTrans = sOrig.Mid(m+1, sOrig.GetLength());
			sOrig = sTrans;
		}
		else
		{
			sArr[i]=sOrig;
			break;	
		}
	}
	sArr[i+1]=inFile;
	sArr[i+2]=outFile;
	sArr.SetSize(i+3,25);
	sArr.FreeExtra();

	*opts = (int) sArr.GetSize();
	
	//for (i = 0; i < sArr.GetSize() ;i++)
	//{
	//	//cArr[i+1] = (char *)(LPCTSTR) sArr[i].GetString(); 
	//	AfxMessageBox((LPCTSTR)sArr[i]);
	//}
}
/*  ******************************************************************* */
/*                               Usage()                                */
/* ******************************************************************** */

void CUtils::Usage(CString additional)

{
	additional = additional + "\nUsage: TranslateRaster - MapWinGIS wrapper for gdal_translate\n" 
			"       src_dataset, dst_dataset,\n"
            "       [-ot {Byte/Int16/UInt16/UInt32/Int32/Float32/Float64/\n"
            "             CInt16/CInt32/CFloat32/CFloat64}] [-not_strict]\n"
            "       [-of format] [-b band] [-outsize xsize[%%] ysize[%%]]\n"
            "       [-scale [src_min src_max [dst_min dst_max]]]\n"
            "       [-srcwin xoff yoff xsize ysize] [-projwin ulx uly lrx lry]\n"
            "       [-a_srs srs_def] [-a_ullr ulx uly lrx lry] [-a_nodata value]\n"
            "       [-gcp pixel line easting northing [elevation]]*\n" 
            "       [-mo \"META-TAG=VALUE\"]* [-quiet] [-sds]\n"
            "       [-co \"NAME=VALUE\"]*\n\n"
			"For more information please see: http://www.remotesensing.org/gdal/gdal_translate.html";


	AfxMessageBox( (LPCTSTR) additional);
}

// ***********************************************************
//	  OGRLayerToShapefile()
// ***********************************************************
STDMETHODIMP CUtils::OGRLayerToShapefile(BSTR Filename, ShpfileType shpType, ICallback *cBack, IShapefile** sf)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	(*sf) = GeometryConverter::Read_OGR_Layer(Filename, shpType);
	return S_OK;
}

// ***********************************************************
//	  ClipPolygon
// ***********************************************************
// New implementation, based on GEOS
STDMETHODIMP CUtils::ClipPolygon(PolygonOperation op, IShape* SubjectPolygon, IShape* ClipPolygon, IShape** retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retval = NULL;
	
	if( SubjectPolygon == NULL || ClipPolygon == NULL )
	{	
		lastErrorCode = tkUNEXPECTED_NULL_PARAMETER;
		if( globalCallback != NULL ) 
			globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
		return S_OK;
	}

	ShpfileType shptype;
	SubjectPolygon->get_ShapeType (&shptype);
	if( shptype != SHP_POLYGON && shptype != SHP_POLYGONM && shptype != SHP_POLYGONZ )
	{	
		lastErrorCode = tkINCOMPATIBLE_SHAPE_TYPE;
		if( globalCallback != NULL ) globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
		return S_OK;
	}

	ClipPolygon->get_ShapeType(&shptype);
	if( shptype != SHP_POLYGON && shptype != SHP_POLYGONM && shptype != SHP_POLYGONZ )
	{	
		lastErrorCode = tkINCOMPATIBLE_SHAPE_TYPE;
		if( globalCallback != NULL ) globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));		
		return S_OK;
	}
	
	bool geos = true;

	if (geos)
	{
		OGRGeometry* geomSubject = GeometryConverter::ShapeToGeometry(SubjectPolygon);
		if (geomSubject == NULL) return S_OK;

		OGRGeometry* geomClip = GeometryConverter::ShapeToGeometry(ClipPolygon);
		if (geomClip == NULL)
		{
			delete geomSubject;
			return S_OK;
		}
		
		OGRGeometry* oGeom = NULL;
		switch(op)
		{
			case DIFFERENCE_OPERATION:
				oGeom = geomSubject->Difference(geomClip);
				break;
			case INTERSECTION_OPERATION:
				oGeom = geomSubject->Intersection(geomClip);
				break;
			case EXCLUSIVEOR_OPERATION:
				oGeom = geomSubject->SymmetricDifference(geomClip);
				break;
			case UNION_OPERATION:
				oGeom = geomSubject->Union(geomClip);
				break;
		}
		
		delete geomClip;
		delete geomSubject;

		if (oGeom != NULL)
		{
			vector<IShape* > vShapes;
			if (GeometryConverter::GeometryToShapes(oGeom, &vShapes))
			{
				*retval = vShapes[0];
				
				ASSERT(vShapes.size() < 2);

				// if there are more than one poly, we should release them
				for (int i = 1; i < (int)vShapes.size(); i++) 
				{
					if (vShapes[i] != NULL)
					{
						vShapes[i]->Release();
					}
				}
			}
			delete oGeom;
		}
	}
	else
	{
		IShape* shp = GeometryConverter::ClipPolygon(ClipPolygon, SubjectPolygon, op);
		*retval = shp;
	}
	return S_OK;
}

#pragma region MergeImages
// ******************************************************************
//		MergeImages
// ******************************************************************
STDMETHODIMP CUtils::MergeImages(/*[in]*/SAFEARRAY* InputNames, /*[in]*/BSTR OutputName, /*out,retval*/VARIANT_BOOL* retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retVal = VARIANT_FALSE;
	USES_CONVERSION;
	
	// Check dimensions of the array.
    if (SafeArrayGetDim(InputNames) != 1)
	{
        // most likely this error will be caught while marshalling the array
		// AfxThrowOleDispatchException(1002, "Type Mismatch in Parameter. Pass a one-dimensional array");
		AfxMessageBox("Failed to read the names of input files");
		return S_OK;
	}

	LONG cElements, lLBound, lUBound;
	HRESULT hr;
	hr = SafeArrayGetLBound(InputNames, 1, &lLBound);
    if (FAILED(hr))
	{
        AfxMessageBox("Failed to read the names of input files");
		return S_OK;
	}
    
	hr = SafeArrayGetUBound(InputNames, 1, &lUBound);
    if (FAILED(hr))
	{
		AfxMessageBox("Failed to read the names of input files");
		return S_OK;
	}
	
	// TODO: add check that we have an array of BSTR and not the other data type
	BSTR HUGEP *pbstr;
	hr = SafeArrayAccessData(InputNames, (void HUGEP* FAR*)&pbstr);
    if (FAILED(hr))
	{
        // TODO: report error        
		AfxMessageBox("Failed to read the names of input files");
		return S_OK;
	}
	
	if( globalCallback != NULL )
		globalCallback->Progress(OLE2BSTR(key), 0, A2BSTR("Loading input images..."));

	VARIANT_BOOL vbretval;
	std::vector<IImage*> images;
	cElements = lUBound-lLBound + 1;
    for (int i = 0; i < cElements; i++)
    {
		//AfxMessageBox(OLE2A(s));
		IImage* img = NULL;
		CoCreateInstance(CLSID_Image,NULL,CLSCTX_INPROC_SERVER,IID_IImage,(void**)&img);
		img->Open(pbstr[i], USE_FILE_EXTENSION, VARIANT_FALSE, NULL, &vbretval);
		
		if (vbretval)
		{
			images.push_back(img);
		}
		else
		{
			img->Release();
			img = NULL;
		}
	}
	
	LONG width, height;
	if (images.size() <= 1)
	{
		// TODO: report error: less than 2 valid images were provided
		AfxMessageBox("Less than 2 valid images were provided");
		goto Cleaning;
	}
	else
	{
		// all the images must have the same size
		LONG w, h;
		for (unsigned int i = 0; i < images.size(); i++ )
		{
			images[i]->get_OriginalWidth(&w);
			images[i]->get_OriginalHeight(&h);

			if (i == 0)
			{
				width = w;
				height = h;
			}
			else if ( w != width || h != height)
			{
				AfxMessageBox("All the images must have the same size");
				// TODO: report error: all the images ust have the same size
				goto Cleaning;
			}
		}
	}

	// saving and copying the pixels
	colour* pixels = new colour[width * height];
	for (unsigned int i = 0; i < images.size(); i++ )
	{
		CString s; BSTR name;
		images[i]->get_Filename(&name);
		s.Format("Processing image %d: %s", i + 1, OLE2A(name));
		int percent = (int)((double)i/(double)(images.size() - 1)*100.0);
		if( globalCallback != NULL )
			globalCallback->Progress(OLE2BSTR(key), percent, A2BSTR(s));
		
		CImageClass* img  = (CImageClass*)images[i];
		img->SaveNotNullPixels(true);
		
		DataPixels* source = img->m_pixels;
		for (int j = 0; j < img->m_pixelsCount; j++)
		{
			pixels[(source + j)->position] = (source + j)->value;
		}

		img->ClearNotNullPixels();
	}
	
	// saving the results
	if( globalCallback != NULL )
		globalCallback->Progress(OLE2BSTR(key), 0, A2BSTR("Saving result..."));
	
	unsigned char* bits = reinterpret_cast<unsigned char*>(pixels);
	SaveBitmap(width, height, bits, OutputName);

Cleaning:
	// cleaning
	if( globalCallback != NULL )
		globalCallback->Progress(OLE2BSTR(key), 0, A2BSTR(""));

	for (unsigned int i = 0; i < images.size(); i++)
	{
		images[i]->Release();
	}
	
	if (pixels) 
	{
		delete[] pixels;
		pixels = NULL;
	}
	return S_OK;
}

// --------------------------------------------------
//	SaveBitmap
// --------------------------------------------------
// Saves provided array of pixels as png image (uses GDI+)
bool CUtils::SaveBitmap(int width, int height, unsigned char* pixels, BSTR outputName)
{
	int pad = (width * 24) % 32;
	if(pad != 0)
	{	pad = 32 - pad;
		pad /= 8;
	}

	BITMAPINFOHEADER bih;
	bih.biCompression=0;
	bih.biXPelsPerMeter=0;
	bih.biYPelsPerMeter=0;
	bih.biClrUsed=0;
	bih.biClrImportant=0;
	bih.biPlanes=1;
	bih.biSize=sizeof(BITMAPINFOHEADER);

	bih.biBitCount=24;
	bih.biWidth= width;
	bih.biHeight= height;
	bih.biSizeImage= (width * 3 + pad) * height;
	
	BITMAPINFO bif;
	bif.bmiHeader = bih;
	
	int nBytesInRow = width * 3 + pad;
	
	// copying bits
	unsigned char* bitsNew = new unsigned char[nBytesInRow * height];
	for(int i = 0; i < height; i++)		
		memcpy(&bitsNew[i * nBytesInRow], &pixels[i * width * 3], width * 3);
	
	// saing the image
	Gdiplus::Bitmap* bmp = new Gdiplus::Bitmap(&bif, (void*)bitsNew); 
	CLSID pngClsid;
	GetEncoderClsid(L"png", &pngClsid);	// perhaps some other formats ?
	USES_CONVERSION;
	Gdiplus::Status status = bmp->Save(OLE2W(outputName), &pngClsid, NULL);

	if (bmp) 
	{
		delete bmp;
	}
	if (bitsNew)
	{
		delete[] bitsNew;
	}

	return (status == Gdiplus::Ok);
}

// ***********************************************************
//		GetEncoderClsid()
// ***********************************************************
// Returns encoder for the specified image format
// The following call should be used for PNG fromat, for example: GetEncoderClsid(L"png", &pngClsid);
int CUtils::GetEncoderClsid(const WCHAR* format, CLSID* pClsid)
{
   UINT  num = 0;          // number of image encoders
   UINT  size = 0;         // size of the image encoder array in bytes
	
   
   Gdiplus::ImageCodecInfo* pImageCodecInfo = NULL;

   Gdiplus::GetImageEncodersSize(&num, &size);
   if(size == 0)
      return -1;  // Failure

   pImageCodecInfo = (Gdiplus::ImageCodecInfo*)(malloc(size));
   if(pImageCodecInfo == NULL)
      return -1;  // Failure

   GetImageEncoders(num, size, pImageCodecInfo);

   for(UINT j = 0; j < num; ++j)
   {
      if( wcscmp(pImageCodecInfo[j].MimeType, format) == 0 )
      {
         *pClsid = pImageCodecInfo[j].Clsid;
         free(pImageCodecInfo);
         return j;  // Success
      }    
   }

   free(pImageCodecInfo);
   return -1;  // Failure
}
#pragma endregion

// ***********************************************************
//		ReprojectShapefile()
// ***********************************************************
STDMETHODIMP CUtils::ReprojectShapefile(IShapefile* sf, IGeoProjection* source, IGeoProjection* target, IShapefile** result)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	if (!sf || !source || !target)
	{
		ErrorMessage(tkUNEXPECTED_NULL_PARAMETER);
		*result = NULL;
		return S_OK;
	}

	OGRSpatialReference* ref1 = ((CGeoProjection*)source)->get_SpatialReference();
	OGRSpatialReference* ref2 = ((CGeoProjection*)target)->get_SpatialReference();

	OGRCoordinateTransformation* transf = OGRCreateCoordinateTransformation( ref1, ref2 );
	if (!transf)
	{
		ErrorMessage(tkFAILED_TO_REPROJECT);
		*result = NULL;
		return S_FALSE;
	}

	// creating a copy of the shapefile
	sf->Clone(result);

	// do reprojection
	VARIANT_BOOL vbretval;
	CComVariant var;
	long numShapes, count = 0;
	sf->get_NumShapes(&numShapes);

	long numFields, percent = 0;
	sf->get_NumFields(&numFields);
	
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
		sf->get_Shape(i, &shp);
		
		IShape* shpNew = NULL;
		shp->Clone(&shpNew);
		
		if (shpNew)
		{
			long numPoints;
			shpNew->get_NumPoints(&numPoints);
			
			double x, y;
			int success = -1;
			IPoint* pnt = NULL;
			for (long j = 0; j < numPoints; j++)
			{
				shpNew->get_XY(j, &x, &y, &vbretval);

				// will work faster after embedding to the CShape class
				BOOL res = transf->Transform( 1, &x, &y);
				if (res)	
				{
					shpNew->put_XY(j, x, y, &vbretval);
				}
				else
				{
					// if there is at least one failed point, reprojection will be interupted
					shpNew->Release();	
					(*result)->Release();
					(*result) = NULL;
					return S_FALSE;	// TODO: report error code
				}
			}

			(*result)->get_NumShapes(&count);
			(*result)->EditInsertShape(shpNew, &count, &vbretval);

			// copying attributes
			for (long j = 0; j < numFields; j++)
			{
				sf->get_CellValue(j, i, &var);
				(*result)->EditCellValue(j, i, var, &vbretval);
			}
		}
	}

	// TODO: set projection string for the resulting shapefile

	if( globalCallback != NULL )
	{
		globalCallback->Progress(OLE2BSTR(key),100,A2BSTR(""));
	}
	return S_OK;
}

// **************************************************************
//		ErrorMessage()
// **************************************************************
void CUtils::ErrorMessage(long ErrorCode)
{
	lastErrorCode = ErrorCode;
	if( globalCallback != NULL) 
		globalCallback->Error(OLE2BSTR(key),A2BSTR(ErrorMsg(lastErrorCode)));
	return;
}

STDMETHODIMP CUtils::ColorByName(tkMapColor name, OLE_COLOR* retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retVal = BGR_TO_RGB(name);
	return S_OK;
}
