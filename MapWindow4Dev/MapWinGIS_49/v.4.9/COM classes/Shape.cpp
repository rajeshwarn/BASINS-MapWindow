//********************************************************************************************************
//File name: Shape.cpp
//Description: Implementation of the CShape
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
//27-jul-2009 Sergei Leschinski (lsu) - added centroid function; 
//										shifted area, length, perimeter properties from CUtils;
//06-aug-2009 lsu - added OGR/GEOS geoprocessing functions;
//********************************************************************************************************
#include "stdafx.h"
#include "Shape.h"
#include "GeometryOperations.h"
#include "Templates.h"

#ifdef MY_DEBUG
#include <fstream>
#include <iostream>
using namespace std;
#endif

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#pragma region DataConversions

// **********************************************
//		CShape::get_fastMode
// **********************************************
bool CShape::get_fastMode()
{
	return _useFastMode;
}

void CShape::put_fastModeAdd(bool state)
{
	_useFastMode = state;
}

// **********************************************
//		InitShapeWrapper()
// **********************************************
CShapeWrapper* CShape::InitShapeWrapper(CShapeWrapperCOM* shpOld)
{
	ASSERT(shpOld != NULL);
	
	ShpfileType type = shpOld->get_ShapeType();

	bool isZ = false, isM = false;
	isM = (type == SHP_MULTIPOINTM || type == SHP_POINTM || type == SHP_POLYGONM || type == SHP_POLYLINEM);
	isZ = (type == SHP_MULTIPOINTZ || type == SHP_POINTZ || type == SHP_POLYGONZ || type == SHP_POLYLINEZ);
	
	double x, y, z, m;

	// referencing the old one
	std::deque<IPoint*>* allPoints = &(shpOld->_allPoints);
	std::deque<long>* allParts = &(shpOld->_allParts);
	
	// creating new wrapper
	CShapeWrapper* shpNew = new CShapeWrapper(type);

	// passing points
	unsigned int size = allPoints->size();
	for (unsigned int i = 0; i < size; i++)
	{
		(*allPoints)[i]->get_X(&x);
		(*allPoints)[i]->get_Y(&y);
		shpNew->_points.push_back(pointEx(x, y));

		if (isZ)
		{ 
			(*allPoints)[i]->get_Z(&z);
			shpNew->_pointsZ.push_back(z);
		}
		else if (isM)
		{
			(*allPoints)[i]->get_Z(&z);
			(*allPoints)[i]->get_M(&m);
			shpNew->_pointsZ.push_back(z);
			shpNew->_pointsM.push_back(m);
		}
	}

	// passing parts
	for (unsigned int i = 0; i < (*allParts).size(); i++)
	{
		shpNew->_parts.push_back((*allParts)[i]);
	}

	return shpNew;
}

// **********************************************
//		InitComWrapper()
// **********************************************
CShapeWrapperCOM* CShape::InitComWrapper(CShapeWrapper* shpOld)
{
	ASSERT(shpOld != NULL);
	
	ShpfileType type = shpOld->get_ShapeType();

	bool isZ = false, isM = false;
	isM = (type == SHP_MULTIPOINTM || type == SHP_POINTM || type == SHP_POLYGONM || type == SHP_POLYLINEM);
	isZ = (type == SHP_MULTIPOINTZ || type == SHP_POINTZ || type == SHP_POLYGONZ || type == SHP_POLYLINEZ);
	
	// creating the new wrapper
	CShapeWrapperCOM* shpNew = new CShapeWrapperCOM(type);
	std::deque<IPoint*>* allPoints = &(shpNew->_allPoints);
	std::deque<long>* allParts = &(shpNew->_allParts);
	
	// passing points
	IPoint* pnt = NULL;
	unsigned int size = shpOld->_points.size();
	
	for (unsigned int i = 0; i < size; i++)
	{
		m_factory.pointFactory->CreateInstance(NULL, IID_IPoint, (void**)&pnt);
		//CoCreateInstance( CLSID_Point, NULL, CLSCTX_INPROC_SERVER, IID_IPoint, (void**)&pnt);
		pnt->put_X(shpOld->_points[i].X);
		pnt->put_Y(shpOld->_points[i].Y);
		if (isZ || isM)
		{
			pnt->put_Z(shpOld->_pointsZ[i]);
		}
		if (isM)
		{
			pnt->put_M(shpOld->_pointsM[i]);
		}
		allPoints->push_back(pnt);
	}
	
	// passing parts
	size = shpOld->_parts.size();
	for (unsigned int i = 0; i < size; i++)
	{
		allParts->push_back(shpOld->_parts[i]);
	}

	return shpNew;
}

// **********************************************
//		put_FastEditMode
// **********************************************
// Toggles between fast (CShapeWrapper) and regular mode (_allPoints)
void CShape::put_fastMode(bool state)
{
	ASSERT(_shp != NULL);
	
	if (state && !_useFastMode )
	{
		// COM wrapper -> CShapeWrapper

		// referencing the old one
		CShapeWrapperCOM* shpOld = (CShapeWrapperCOM*)_shp;
		
		// converting
		CShapeWrapper* shpNew = InitShapeWrapper(shpOld);

		// deleting the old wrapper
		delete shpOld;

		// storing the new one
		_shp = shpNew;
	}
	else if ( !state && _useFastMode )
	{
		// CShapeWrapper -> COM wrapper

		// referencing the old one
		CShapeWrapper* shpOld = (CShapeWrapper*)_shp;
		
		CShapeWrapperCOM* shpNew = InitComWrapper(shpOld);

		// deleting the old wrapper
		delete shpOld;

		// storing the new one
		_shp = shpNew;
	}
	_useFastMode = state;
}

#pragma endregion

#pragma region Genereal

// *************************************************************
//		get_LastErrorCode
// *************************************************************
STDMETHODIMP CShape::get_LastErrorCode(long *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	*pVal = _lastErrorCode;
	_lastErrorCode = tkNO_ERROR;

	return S_OK;
}

// *************************************************************
//		get_ErrorMsg
// *************************************************************
STDMETHODIMP CShape::get_ErrorMsg(long ErrorCode, BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	USES_CONVERSION;
	*pVal = A2BSTR(ErrorMsg(ErrorCode));

	return S_OK;
}

// *************************************************************
//		get/put_globalCallback
// *************************************************************
STDMETHODIMP CShape::get_GlobalCallback(ICallback **pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*pVal = _globalCallback;
	if( _globalCallback != NULL )
		_globalCallback->AddRef();
	return S_OK;
}
STDMETHODIMP CShape::put_GlobalCallback(ICallback *newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	put_ComReference(newVal, (IDispatch**)&_globalCallback);
	return S_OK;
}

// *************************************************************
//		get/put__key
// *************************************************************
STDMETHODIMP CShape::get_Key(BSTR *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	*pVal = OLE2BSTR(_key);
	return S_OK;
}
STDMETHODIMP CShape::put_Key(BSTR newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	::SysFreeString(_key);
	_key = OLE2BSTR(newVal);
	return S_OK;
}

//***************************************************************
//		ErrorMessage()								           
//***************************************************************
inline void CShape::ErrorMessage(long ErrorCode)
{
	_lastErrorCode = ErrorCode;
	if( _globalCallback != NULL) _globalCallback->Error(OLE2BSTR(_key),A2BSTR(ErrorMsg(_lastErrorCode)));
	return;
}

// *************************************************************
//		Create
// *************************************************************
STDMETHODIMP CShape::Create(ShpfileType ShpType, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;

	if(	ShpType == SHP_MULTIPATCH )
	{	
		ErrorMessage(tkUNSUPPORTED_SHAPEFILE_TYPE);
		*retval = VARIANT_FALSE;
	}
	else
	{
		_shp->Clear();
		_shp->put_ShapeType(ShpType);		// the only error here (multipatch shape) is checked above
											// so we don't control return value of function here
		*retval = VARIANT_TRUE;
	}
	return S_OK;
}

// **********************************************************
//		get_ShapeType
// **********************************************************
STDMETHODIMP CShape::get_ShapeType(ShpfileType *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*pVal = _shp->get_ShapeType();
	return S_OK;
}

// *************************************************************
//		put_ShapeType
// *************************************************************
STDMETHODIMP CShape::put_ShapeType(ShpfileType newVal)
{	
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	if (!_shp->put_ShapeType(newVal))
	{
		ErrorMessage(_shp->get_LastErrorCode());
	}
	return S_OK;
}

// *************************************************************
//			get_IsValid	
// *************************************************************
// Checking validity of the geometry
STDMETHODIMP CShape::get_IsValid(VARIANT_BOOL* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retval = VARIANT_FALSE;
		
	if(_shp->get_PointCount() == 0)
	{
		_isValidReason = "Shape hasn't got points";
		return S_OK;
	}
	
	ShpfileType shptype = _shp->get_ShapeType();

	if (shptype == SHP_POLYGON || shptype == SHP_POLYGONM || shptype == SHP_POLYGONZ || 
		shptype == SHP_POLYLINE || shptype == SHP_POLYLINEM || shptype == SHP_POLYLINEZ)
	{
		if (_shp->get_PartCount() == 0)
		{
			_isValidReason = "Shape hasn't got parts";
			return S_OK;
		}
	}
	
	if (shptype == SHP_POLYGON || shptype == SHP_POLYGONM || shptype == SHP_POLYGONZ)
	{
		int beg_part, end_part;
		double x1, x2, y1, y2;

		for(long i = 0; i < _shp->get_PartCount(); i++)
		{
			beg_part = _shp->get_PartStartPoint(i);
			end_part = _shp->get_PartEndPoint(i);
			
			_shp->get_PointXY(beg_part, x1, y1);
			_shp->get_PointXY(end_part, x2, y2);

			if (x1 != x2 || y1!= y2)
			{
				_isValidReason = "The first and the last point of the polygon part must be the same";
				return S_OK;
			}
		}
	}
	
	// -----------------------------------------------
	//  check through GEOS (common for both modes)
	// -----------------------------------------------
	OGRGeometry* oGeom = NULL;

	oGeom = GeometryConverter::ShapeToGeometry(this);
	if (oGeom == NULL) 
	{
		_isValidReason = "Failed to convert to OGR geometry";
		return S_OK;
	}

	// added code
	GEOSGeom hGeosGeom = NULL;	
	hGeosGeom = oGeom->exportToGEOS();
	delete oGeom;

	if (hGeosGeom == NULL)
	{
		_isValidReason = "Failed to convert to GEOS geometry";
		return S_OK;
	}

	if (!GEOSisValid( hGeosGeom ))
	{
		_isValidReason = GEOSisValidReason(hGeosGeom);
	}
	else
	{
		*retval = VARIANT_TRUE;
	}
	GEOSGeom_destroy( hGeosGeom );
	
	return S_OK;
}
#pragma endregion

#pragma region Parts
// **********************************************************
//		get_NumParts
// **********************************************************
STDMETHODIMP CShape::get_NumParts(long *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*pVal = _shp->get_PartCount();
	return S_OK;
}

// *************************************************************
//		get_Part
// *************************************************************
STDMETHODIMP CShape::get_Part(long PartIndex, long* pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*pVal = _shp->get_PartStartPoint(PartIndex);
	if (*pVal == -1)
	{
		ErrorMessage(_shp->get_LastErrorCode());
	}
	return S_OK;
}

// *************************************************************
//		put_Part
// *************************************************************
STDMETHODIMP CShape::put_Part(long PartIndex, long newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	
	if (!_shp->put_PartStartPoint(PartIndex, newVal))
	{
		ErrorMessage(_shp->get_LastErrorCode());
	}
	return S_OK;
}

/***********************************************************************/
/*		get_EndOfPart()
/***********************************************************************/
//  Returns last point of the part
STDMETHODIMP CShape::get_EndOfPart(long PartIndex, long* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retval = _shp->get_PartEndPoint(PartIndex);
	if (*retval == VARIANT_FALSE)
	{
		ErrorMessage(_shp->get_LastErrorCode());
	}
	return S_OK;
};

// *************************************************************
//		InsertPart
// *************************************************************
STDMETHODIMP CShape::InsertPart(long PointIndex, long *PartIndex, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	*retval = (VARIANT_BOOL)_shp->InsertPart(*PartIndex , PointIndex);
	if (*retval == VARIANT_FALSE)
	{
		ErrorMessage(_shp->get_LastErrorCode());
	}
	return S_OK;
}
	
// *************************************************************
//		DeletePart
// *************************************************************
STDMETHODIMP CShape::DeletePart(long PartIndex, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	*retval = (VARIANT_BOOL)_shp->DeletePart(PartIndex);
	if (*retval == VARIANT_FALSE)
	{
		ErrorMessage(_shp->get_LastErrorCode());
	}
	return S_OK;
}

//*********************************************************************
//		CShape::PartIsClockWise()									
//*********************************************************************
//  Returns true if points of shape's part are in clockwise direction
//  and false otherwise
STDMETHODIMP CShape::get_PartIsClockWise(long PartIndex, VARIANT_BOOL* retval)
{
	*retval = VARIANT_FALSE;
	
	long numParts, numPoints;
	this->get_NumParts(&numParts);

	if ((PartIndex >= numParts) || (PartIndex < 0))
	{
		ErrorMessage(tkINDEX_OUT_OF_BOUNDS);
		return S_OK;
	}
	
	long beg_part, end_part;
	this->get_NumPoints(&numPoints);
	this->get_Part(PartIndex,&beg_part);
	
	if( numParts - 1 > PartIndex )	
		this->get_Part(PartIndex+1, &end_part);
	else							
		end_part = numPoints;
	
	//lsu: we need to calcuate area of part to determine clockwiseness
	double x1, x2, y1, y2;
	VARIANT_BOOL vbretval;
	double area = 0;

	for(long i = beg_part; i < end_part - 1; i++)
	{
		this->get_XY(i, &x1, &y1, &vbretval);
		this->get_XY(i + 1, &x2, &y2, &vbretval);
		area += (x1 * y2) - (x2 * y1);
	}
    
    if (area < 0) *retval = VARIANT_TRUE;
	
	return S_OK;
}

//***********************************************************************/
//		CShape::ReversePointsOrder()
//***********************************************************************/
//  Changes the order of points for shape's part
STDMETHODIMP CShape::ReversePointsOrder(long PartIndex, VARIANT_BOOL* retval)
{
	*retval = VARIANT_FALSE;
	
	long numParts, numPoints;
	this->get_NumParts(&numParts);

	if ((PartIndex >= numParts) || (PartIndex < 0))
	{
		ErrorMessage(tkINDEX_OUT_OF_BOUNDS);
		return S_OK;
	}
	
	long beg_part, end_part;
	this->get_NumPoints(&numPoints);
	this->get_Part(PartIndex,&beg_part);
	
	if( numParts - 1 > PartIndex )	
	{
		this->get_Part(PartIndex+1, &end_part);
	}
	else							
	{
		end_part = numPoints;
	}
	
	if (_useFastMode)
	{
		CShapeWrapper* shp = (CShapeWrapper*) _shp;
		if (shp->_points.size() > 1)
		{
			std::vector<pointEx>::iterator iter1 = shp->_points.begin();
			std::vector<pointEx>::iterator iter2 = shp->_points.begin();
			iter1 +=beg_part;
			iter2 +=end_part;
			reverse( iter1, iter2);
		}
		
		if (shp->_pointsZ.size() > 1)
		{
			std::vector<double>::iterator iterZ1 = shp->_pointsZ.begin();
			std::vector<double>::iterator iterZ2 = shp->_pointsZ.begin();
			iterZ1 +=beg_part;
			iterZ2 +=end_part;
			reverse( iterZ1, iterZ2);
		}
		
		if (shp->_pointsM.size() > 1)
		{
			std::vector<double>::iterator iterM1 = shp->_pointsM.begin();
			std::vector<double>::iterator iterM2 = shp->_pointsM.begin();
			iterM1 +=beg_part;
			iterM2 +=end_part;
			reverse( iterM1, iterM2);
		}
	}
	else
	{
		CShapeWrapperCOM* shp = (CShapeWrapperCOM*) _shp;
		std::deque<IPoint* > allPoints = shp->_allPoints;
		deque<IPoint *>::iterator iter1 = allPoints.begin();
		deque<IPoint *>::iterator iter2 = allPoints.begin();
		iter1 +=beg_part;
		iter2 +=end_part;
		reverse( iter1, iter2);
	}

	*retval = VARIANT_TRUE;
	return S_OK;
}

// ***************************************************************
//		PartAsShape()
// ***************************************************************
//  Returns part of the shape as new shape; new points are created
STDMETHODIMP CShape::get_PartAsShape(long PartIndex, IShape **pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	
	long beg_part, end_part;
	this->get_Part(PartIndex, &beg_part);
	this->get_EndOfPart(PartIndex, &end_part);
	
	if (beg_part == -1 || end_part == -1) 
	{
		*pVal = NULL; 
		return S_OK;
	}
	
	IShape * shp = NULL;
	CoCreateInstance( CLSID_Shape, NULL, CLSCTX_INPROC_SERVER, IID_IShape, (void**)&shp );
	
	ShpfileType shptype = _shp->get_ShapeType();
	shp->put_ShapeType(shptype);

	long part  = 0; 
	VARIANT_BOOL vbretval;
	shp->InsertPart(0, &part, &vbretval);
	
	long cnt = 0;
	IPoint* pntOld = NULL;
	IPoint* pntNew = NULL;
	for (int i = beg_part; i <=end_part; i++)
	{
		this->get_Point(i, &pntOld);
		pntOld->Clone(&pntNew);
		shp->InsertPoint(pntNew, &cnt, &vbretval);
		pntOld->Release();
		cnt++;
	}
	*pVal = shp;
	return S_OK;
}

#pragma endregion	

// TODO: add methods to set Z and M; write method put_XY
#pragma region Points
// **********************************************************
//		get_NumPoints
// **********************************************************
STDMETHODIMP CShape::get_NumPoints(long *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*pVal = _shp->get_PointCount();
	return S_OK;
}

// *************************************************************
//		get_Point
// *************************************************************
STDMETHODIMP CShape::get_Point(long PointIndex, IPoint **pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*pVal = _shp->get_Point(PointIndex);
	if ((*pVal) == NULL)
	{
		ErrorMessage(_shp->get_LastErrorCode());
	}
	return S_OK;
}

// *************************************************************
//		put_Point
// *************************************************************
STDMETHODIMP CShape::put_Point(long PointIndex, IPoint *newVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	if( newVal = NULL )
	{	
		ErrorMessage(tkUNEXPECTED_NULL_PARAMETER);
	}
	else
	{
		double x, y;	
		newVal->get_X(&x);
		newVal->get_Y(&y);
		newVal->Release();
		if (!_shp->put_PointXY(PointIndex, x, y))
		{
			ErrorMessage(_shp->get_LastErrorCode());
		}
	}
	return S_OK;
}

// *************************************************************
//		InsertPoint
// *************************************************************
STDMETHODIMP CShape::InsertPoint(IPoint *NewPoint, long *PointIndex, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	*retval = (VARIANT_BOOL)_shp->InsertPoint(*PointIndex, NewPoint);
	if (*retval == VARIANT_FALSE)
	{
		ErrorMessage(_shp->get_LastErrorCode());
	}
	return S_OK;
}

// *************************************************************
//		DeletePoint
// *************************************************************
STDMETHODIMP CShape::DeletePoint(long PointIndex, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	*retval = (VARIANT_BOOL)_shp->DeletePoint(PointIndex);
	if (*retval == VARIANT_FALSE)
	{
		ErrorMessage(_shp->get_LastErrorCode());
	}
	return S_OK;
}

// *************************************************************
//		get_XY
// *************************************************************
STDMETHODIMP CShape::get_XY(long PointIndex, double* x, double* y, VARIANT_BOOL* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retval = (VARIANT_BOOL)_shp->get_PointXY(PointIndex, *x, *y);
	if (*retval == VARIANT_FALSE)
	{
		ErrorMessage(_shp->get_LastErrorCode());
	}
	return S_OK;
}

// **********************************************
//   put_XY()
// **********************************************
STDMETHODIMP CShape::put_XY(LONG pointIndex, double x, double y, VARIANT_BOOL* retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retVal = (VARIANT_BOOL)_shp->put_PointXY(pointIndex, x, y);
	if (*retVal == VARIANT_FALSE)
	{
		ErrorMessage(_shp->get_LastErrorCode());
	}
	return S_OK;
}

#pragma endregion	

#pragma region SpatialProperties

// *****************************************************************
//			get_Center()
// *****************************************************************
//  Returns center of shape (crossing of diagonals of bounding box)
STDMETHODIMP CShape::get_Center(IPoint **pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	double xMin, xMax, yMin, yMax;
	double x, y;
	
	//CShapeWrapper* shp = (CShapeWrapper*)_shp;
	//std::vector<pointEx>* points = &shp->_points;

	for( int i = 0; i < (int)_shp->get_PointCount(); i++ )
	{	
		_shp->get_PointXY(i, x, y);

		if( i == 0 )
		{	
			xMin = x; xMax = x;
			yMin = y; yMax = y;
		}
		else
		{	
			if		(x < xMin)	xMin = x;
			else if (x > xMax)	xMax = x;
			if		(y < yMin)	yMin = y;
			else if (y > yMax)	yMax = y;
		}
	}	
	
	x = xMin + (xMax - xMin)/2;
	y = yMin + (yMax - yMin)/2;

	IPoint * pnt = NULL;
	CoCreateInstance( CLSID_Point, NULL, CLSCTX_INPROC_SERVER, IID_IPoint, (void**)&pnt );
	pnt->put_X(x);pnt->put_Y(y);
	*pVal = pnt;
	
	return S_OK;
}

// *************************************************************
//		get_Length
// *************************************************************
// TODO: it's possible to optimize it for fast mode
STDMETHODIMP CShape::get_Length(double *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	ShpfileType shptype;
	this->get_ShapeType(&shptype);

	if( shptype != SHP_POLYLINE && shptype != SHP_POLYLINEZ && shptype != SHP_POLYLINEM )
	{	
		*pVal = 0.0;
		ErrorMessage(tkINCOMPATIBLE_SHAPE_TYPE);
		return S_OK;
	}

	double length = 0.0;
	long numParts = 0, numPoints = 0;
	this->get_NumParts(&numParts);
	this->get_NumPoints(&numPoints);
	
	long beg_part = 0;
	long end_part = 0;
	for( int j = 0; j < numParts; j++ )
	{	
		this->get_Part(j,&beg_part);
		if( numParts - 1 > j )
		{
			this->get_Part(j+1,&end_part);
		}
		else
		{
			end_part = numPoints;
		}

		for( int i = beg_part; i < end_part-1; i++ )
		{	
			IPoint * pnt1 = NULL;
			IPoint * pnt2 = NULL;
			this->get_Point(i,&pnt1);
			this->get_Point(i+1,&pnt2);

			double oneX, oneY;
			double twoX, twoY;
			pnt1->get_X(&oneX);
			pnt1->get_Y(&oneY);
			pnt2->get_X(&twoX);
			pnt2->get_Y(&twoY);
			
			length += sqrt( pow( twoX - oneX, 2 ) + pow( twoY - oneY, 2 ) );

			pnt1->Release();
			pnt2->Release();
		}
	}
	
	*pVal = length;
	return S_OK;
}

// *************************************************************
//		get_Perimeter
// *************************************************************
// TODO: it's possible to optimize it for fast mode
STDMETHODIMP CShape::get_Perimeter(double *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())

	ShpfileType shptype;
	this->get_ShapeType(&shptype);

	if( shptype != SHP_POLYGON && shptype != SHP_POLYGONZ && shptype != SHP_POLYGONM )
	{	
		*pVal = 0.0;
		ErrorMessage(tkINCOMPATIBLE_SHAPE_TYPE);
		return S_OK;
	}

	double perimeter = 0.0;
	long numParts = 0, numPoints = 0;
	this->get_NumParts(&numParts);
	this->get_NumPoints(&numPoints);

	long beg_part = 0;
	long end_part = 0;
	for( int j = 0; j < numParts; j++ )
	{	
		this->get_Part(j,&beg_part);
		if( numParts - 1 > j )
			this->get_Part(j+1,&end_part);
		else
			end_part = numPoints;
		
		double px1, py1, px2, py2;
		for( int i = beg_part; i < end_part-1; i++ )
		{	
			IPoint * pnt1 = NULL;
			IPoint * pnt2 = NULL;
			this->get_Point(i,&pnt1);
			this->get_Point(i+1,&pnt2);
			pnt1->get_X(&px1);
			pnt1->get_Y(&py1);
			pnt2->get_X(&px2);
			pnt2->get_Y(&py2);
			pnt1->Release();
			pnt2->Release();
			perimeter += sqrt( pow(px2 - px1, 2) + pow(py2 - py1,2));
		}
		
	}

	*pVal = perimeter;
	return S_OK;
}

// *************************************************************
//		get_Extents
// *************************************************************
STDMETHODIMP CShape::get_Extents(IExtents **pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	double xMin = 0.0, xMax = 0.0,
		   yMin = 0.0, yMax = 0.0,
		   zMin = 0.0, zMax = 0.0,
		   mMin = 0.0, mMax = 0.0;
		
	if (_shp->get_ShapeType2D() == SHP_POINT)
	{
		_shp->get_PointXY(0, xMin, yMin);
		xMax = xMin; yMax = yMin;
	}
	else
	{
		_shp->get_Bounds(xMin, xMax, yMin, yMax, zMin, zMax, mMin, mMax);
	}

	IExtents * bBox = NULL;
	CoCreateInstance( CLSID_Extents, NULL, CLSCTX_INPROC_SERVER, IID_IExtents, (void**)&bBox );
	bBox->SetBounds(xMin,yMin,zMin,xMax,yMax,zMax);
	bBox->SetMeasureBounds(mMin,mMax);
	*pVal = bBox;
	return S_OK;
}

// *************************************************************
//		get_Area
// *************************************************************
// TODO: it's possible to optimize it for fast mode

	struct Poly
	{	
	public:
		Poly(){}
		std::vector<double> polyX;
		std::vector<double> polyY;
	};

STDMETHODIMP CShape::get_Area(double *pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	ShpfileType shptype;
	this->get_ShapeType(&shptype);

	if( shptype != SHP_POLYGON && shptype != SHP_POLYGONZ && shptype != SHP_POLYGONM )
	{	
		*pVal = 0.0;
		ErrorMessage(tkINCOMPATIBLE_SHAPE_TYPE);
		return S_OK;
	}

	double total_area = 0.0;
	double indiv_area = 0.0;

	long numParts = 0, numPoints = 0;
	this->get_NumParts(&numParts);
	this->get_NumPoints(&numPoints);
	
	if(numParts > 1)
	{
		//Create new polygons from the different parts (simplified on 2/10/06 by Angela Hillier)
		std::deque<Poly> all_polygons;

		long beg_part = 0;
		long end_part = 0;
		for( int j = 0; j < numParts; j++ )
		{	
			this->get_Part(j, &beg_part);
			if( numParts - 1 > j )
				this->get_Part(j+1, &end_part);
			else
				end_part = numPoints;
			
			double px, py;
			Poly polygon;
			for( int i = beg_part; i < end_part; i++ )
			{	
				IPoint * pnt = NULL;
				this->get_Point(i,&pnt);
				pnt->get_X(&px);
				pnt->get_Y(&py);
				pnt->Release();

				polygon.polyX.push_back(px);
				polygon.polyY.push_back(py);			
			}
			all_polygons.push_back( polygon );		
		}
		
		for( int p = 0; p < (int)all_polygons.size(); p++ )
		{	
			indiv_area = 0.0;

			//Calculate individual area of each part
			for( int a = 0; a < (int)all_polygons[p].polyX.size() - 1; a++)
			{	
				double oneX = all_polygons[p].polyX[a];
				double oneY = all_polygons[p].polyY[a];
				double twoX = all_polygons[p].polyX[a+1];		
				double twoY = all_polygons[p].polyY[a+1];
			
				double trap_area = ((oneX * twoY) - (twoX * oneY));
				indiv_area += trap_area;
			}		
			
			total_area += indiv_area;
		}
		total_area = fabs(total_area) * .5;
		all_polygons.clear();
	}
	else
	{
		for(int i= 0; i <= numPoints-2; i++)
		{
			double oneX, oneY, twoX, twoY;
			IPoint * pnt1 = NULL;
			IPoint * pnt2 = NULL;
			this->get_Point(i,&pnt1);
			this->get_Point(i+1,&pnt2);
			pnt1->get_X(&oneX);
			pnt1->get_Y(&oneY);
			pnt2->get_X(&twoX);
			pnt2->get_Y(&twoY);
			pnt1->Release();
			pnt2->Release();

			double trap_area = ((oneX * twoY) - (twoX * oneY));
			total_area += trap_area;
		}
		total_area = fabs(total_area) * .5;
	}

	*pVal = total_area;
	return S_OK;
}

// *************************************************************
//		get_Centroid
// *************************************************************
// TODO: it's possible to optimize it for fast mode
STDMETHODIMP CShape::get_Centroid(IPoint** pVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	
	ShpfileType shptype = _shp->get_ShapeType();
	if( shptype != SHP_POLYGON && shptype != SHP_POLYGONZ && shptype != SHP_POLYGONM )
	{	
		ErrorMessage(tkINCOMPATIBLE_SHAPE_TYPE);
		return S_OK;
	}
	
	long numPoints, numParts;
	long beg_part, end_part;
	double totalArea = 0.0;
    double area;
	double xSum, ySum;
	double XShift = 0.0;
	double YShift = 0.0; 
	double xPart, yPart;	// centroid of part
	double x = 0.0;			// final centroid
	double y = 0.0;

	IExtents* box;
	double xMin, yMin, xMax, yMax,zMin, zMax;
	this->get_Extents(&box);
	box->GetBounds(&xMin, &yMin, &zMin, &xMax, &yMax, &zMax);
	box->Release();

	if (xMin < 0 && xMax > 0) XShift = xMax - xMin;
	if (yMin < 0 && yMax > 0) YShift = yMax - yMin;

	this->get_NumPoints(&numPoints);
	this->get_NumParts(&numParts);
	
	for( int j = 0; j < numParts; j++ )
	{	
		area = xSum = ySum = 0.0;
		
		this->get_Part(j, &beg_part);
		if( numParts - 1 > j )
			this->get_Part(j+1, &end_part);
		else
			end_part = numPoints;

		for (int i = beg_part; i < end_part -1; i++)
		{
			IPoint * pnt1 = NULL;
			IPoint * pnt2 = NULL;
			this->get_Point(i,&pnt1);
			this->get_Point(i+1,&pnt2);

			double oneX, oneY, twoX, twoY;
			pnt1->get_X(&oneX);
			pnt1->get_Y(&oneY);
			pnt2->get_X(&twoX);
			pnt2->get_Y(&twoY);
			pnt1->Release();
			pnt2->Release();
			
			double cProduct = ((oneX + XShift) * (twoY + YShift)) - ((twoX + XShift) * (oneY + YShift));
			xSum += ((oneX + XShift) + (twoX + XShift)) * cProduct;
			ySum += ((oneY + YShift) + (twoY + YShift)) * cProduct;
			
			area += (oneX * twoY) - (twoX * oneY);
		}

		area = fabs(area) * .5;
		if (area!=0)
		{	xPart = xSum / (6 * area);
			yPart = ySum / (6 * area);
		}

		// corrects for shapes in quadrants other than 1 or clockwise/counterclocwise sign errors
		if (xMax + XShift < 0 && xPart > 0)  xPart = -1 * xPart;
		if (xMin + XShift > 0 && xPart < 0)  xPart = -1 * xPart;
		if (yMax + YShift < 0 && yPart > 0)  yPart = -1 * yPart;
		if (yMin + YShift > 0 && yPart < 0)  yPart = -1 * yPart;
	    
		// Adjust centroid if we calculated it using an X or Y shift
		xPart -= XShift;
		yPart -= YShift;

		x += xPart * area;
        y += yPart * area;
		totalArea += area;
	}
	if (totalArea != 0)
	{	
		x = x / totalArea;
		y = y / totalArea;
	}
	
	IPoint* pnt = NULL;
	m_factory.pointFactory->CreateInstance(NULL, IID_IPoint, (void**)&pnt);
	//CoCreateInstance(CLSID_Point,NULL,CLSCTX_INPROC_SERVER,IID_IPoint,(void**)&pnt);	
	pnt->put_X(x);
	pnt->put_Y(y);
	*pVal = pnt;
	return S_OK;
}
#pragma endregion

#pragma region GEOSGeoprocessing

// *************************************************************
//		Relates()
// *************************************************************
STDMETHODIMP CShape::Relates(IShape* Shape, tkSpatialRelation Relation, VARIANT_BOOL* retval)
{
    AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retval = VARIANT_FALSE;	

	if( Shape == NULL)
	{	
		ErrorMessage(tkUNEXPECTED_NULL_PARAMETER);
		return S_OK;
	} 
	
	// if extents don't cross, no need to seek further
	if (!(Relation == srDisjoint))	
	{	
		if (RelateExtents(this, Shape) == erNone)		
			return S_OK;
	}

	OGRGeometry* oGeom1 = NULL;
	OGRGeometry* oGeom2 = NULL;
	
	oGeom1 = GeometryConverter::ShapeToGeometry(this);
	if (oGeom1 == NULL) return S_OK;

	oGeom2 = GeometryConverter::ShapeToGeometry(Shape);
	if (oGeom2 == NULL) 
	{	
		delete oGeom1;
		return S_OK;
	}
	
	OGRBoolean res;
	
	switch (Relation)
	{
		case srContains:	res = oGeom1->Contains(oGeom2);
		case srCrosses:		res = oGeom1->Crosses(oGeom2);
		case srDisjoint:	res = oGeom1->Disjoint(oGeom2);
		case srEquals:		res = oGeom1->Equal(oGeom2);
		case srIntersects:	res = oGeom1->Intersect(oGeom2);
		case srOverlaps:	res = oGeom1->Overlaps(oGeom2);
		case srTouches:		res = oGeom1->Touches(oGeom2);
		case srWithin:		res = oGeom1->Within(oGeom2);
	}
	
	delete oGeom1;
	delete oGeom2;

	*retval = res;
	return S_OK;
}

// *************************************************************
//		Relations
// *************************************************************
STDMETHODIMP CShape::Contains(IShape* Shape, VARIANT_BOOL* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	this->Relates(Shape, srContains, retval);
	return S_OK;
}
STDMETHODIMP CShape::Crosses(IShape* Shape, VARIANT_BOOL* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	this->Relates(Shape, srCrosses, retval);
	return S_OK;
}
STDMETHODIMP CShape::Disjoint(IShape* Shape, VARIANT_BOOL* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	this->Relates(Shape, srDisjoint, retval);
	return S_OK;
}
STDMETHODIMP CShape::Equals(IShape* Shape, VARIANT_BOOL* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	this->Relates(Shape, srEquals, retval);
	return S_OK;
}
STDMETHODIMP CShape::Intersects(IShape* Shape, VARIANT_BOOL* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	this->Relates(Shape, srIntersects, retval);
	return S_OK;
}
STDMETHODIMP CShape::Overlaps(IShape* Shape, VARIANT_BOOL* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	this->Relates(Shape, srOverlaps, retval);
	return S_OK;
}
STDMETHODIMP CShape::Touches(IShape* Shape, VARIANT_BOOL* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	this->Relates(Shape, srTouches, retval);
	return S_OK;
}
STDMETHODIMP CShape::Within(IShape* Shape, VARIANT_BOOL* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	this->Relates(Shape, srWithin, retval);
	return S_OK;
}

// *************************************************************
//		Clip
// *************************************************************
STDMETHODIMP CShape::Clip(IShape* Shape, tkClipOperation Operation, IShape** retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retval = NULL;

	if( Shape == NULL)
	{	
		ErrorMessage(tkUNEXPECTED_NULL_PARAMETER);
		return S_OK;
	} 
	
	if (Operation != clUnion)
	{
		if (RelateExtents(this, Shape) == erNone)	
			return S_OK;
	}

	OGRGeometry* oGeom1 = NULL;
	OGRGeometry* oGeom2 = NULL;

	oGeom1 = GeometryConverter::ShapeToGeometry(this);
	if (oGeom1 == NULL) 
		return S_OK;

	OGRwkbGeometryType oReturnType = oGeom1->getGeometryType();
	
	oGeom2 = GeometryConverter::ShapeToGeometry(Shape);
	if (oGeom2 == NULL) 
	{	
		delete oGeom1;
		return S_OK;
	}
	
	OGRGeometry* oGeom3 = NULL;

	switch (Operation)
	{
		case clUnion:			
			oGeom3 = oGeom1->Union(oGeom2);
			break;
		case clDifference:
			oGeom3 = oGeom1->Difference(oGeom2);
			break;
		case clIntersection:
			oGeom3 = oGeom1->Intersection(oGeom2);
			break;
		case clSymDifference:
			oGeom3 = oGeom1->SymmetricDifference(oGeom2);
			break;
		default:
			break;
	}
	
	delete oGeom1;
	delete oGeom2;
	
	if (oGeom3 == NULL) 
		return S_OK;

	IShape* shp;
	shp = GeometryConverter::GeometryToShape(oGeom3, oReturnType);
	
	delete oGeom3;

	*retval = shp;
	return S_OK;
}

// *************************************************************
//		Distance
// *************************************************************
STDMETHODIMP CShape::Distance(IShape* Shape, double* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	
	*retval = 0.0;

	if( Shape == NULL)
	{	
		ErrorMessage(tkUNEXPECTED_NULL_PARAMETER);
		return S_OK;
	} 
	
	OGRGeometry* oGeom1 = NULL;
	OGRGeometry* oGeom2 = NULL;
	
	oGeom1 = GeometryConverter::ShapeToGeometry(this);
	if (oGeom1 == NULL) return S_OK;

	oGeom2 = GeometryConverter::ShapeToGeometry(Shape);
	if (oGeom2 == NULL) 
	{	
		delete oGeom1;
		return S_OK;
	}

	*retval = oGeom1->Distance(oGeom2);

	delete oGeom1;
	delete oGeom2;
	return S_OK;
}

OGRGeometry* DoBuffer(DOUBLE Distance, long nQuadSegments, OGRGeometry* geomSource)
{
	__try
	{
		return geomSource->Buffer(Distance, nQuadSegments);
	}
	__except(1)
	{
		return NULL;
	}
}

// *************************************************************
//		Buffer
// *************************************************************
STDMETHODIMP CShape::Buffer(DOUBLE Distance, long nQuadSegments, IShape** retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	*retval = NULL;

	OGRGeometry* oGeom1 = NULL;
	OGRGeometry* oGeom2 = NULL;

	oGeom1 = GeometryConverter::ShapeToGeometry(this);
	if (oGeom1 == NULL) return S_FALSE;
		
	//oGeom2 = oGeom1->Buffer(Distance, (int)nQuadSegments);
	oGeom2 = DoBuffer(Distance, nQuadSegments, oGeom1);
	if (oGeom2)
	{
		#ifdef MY_DEBUG
		IShapefile* sf = NULL;
		CoCreateInstance( CLSID_Shapefile, NULL, CLSCTX_INPROC_SERVER, IID_IShapefile, (void**)&sf );
		VARIANT_BOOL vbretval;
		long index = 0;
		sf->CreateNewWithShapeID(A2BSTR(""), ShpfileType::SHP_POLYGON, &vbretval);
		sf->EditInsertShape(this, &index, &vbretval);
		sf->SaveAs(A2BSTR("c:\\temp.shp"), NULL, &vbretval);
		sf->Close(&vbretval);
		sf->Release();
		#endif
	}

	delete oGeom1;
	if (oGeom2 == NULL)	return S_FALSE;
	
	IShape* shp;
	shp = GeometryConverter::GeometryToShape(oGeom2);

	*retval = shp;
	delete oGeom2;
	
	return S_OK;
}

// *************************************************************
//		Boundry
// *************************************************************
STDMETHODIMP CShape::Boundry(IShape** retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	*retval = NULL;

	OGRGeometry* oGeom1 = NULL;
	OGRGeometry* oGeom2 = NULL;

	oGeom1 = GeometryConverter::ShapeToGeometry(this);
	if (oGeom1 == NULL) return S_OK;
	
	oGeom2 = oGeom1->getBoundary();
	delete oGeom1;

	if (oGeom2 == NULL)	return S_OK;
	
	IShape* shp;
	shp = GeometryConverter::GeometryToShape(oGeom2);

	*retval = shp;
	delete oGeom2;
	
	return S_OK;
}

// *************************************************************
//		ConvexHull
// *************************************************************
STDMETHODIMP CShape::ConvexHull(IShape** retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	*retval = NULL;

	OGRGeometry* oGeom1 = NULL;
	OGRGeometry* oGeom2 = NULL;

	oGeom1 = GeometryConverter::ShapeToGeometry(this);
	if (oGeom1 == NULL) return S_OK;
	
	oGeom2 = oGeom1->ConvexHull();
	delete oGeom1;

	if (oGeom2 == NULL)	return S_OK;
	
	IShape* shp;
	shp = GeometryConverter::GeometryToShape(oGeom2);

	*retval = shp;
	delete oGeom2;
	
	return S_OK;
}

// *************************************************************
//		get_isValidReason
// *************************************************************
STDMETHODIMP CShape::get_IsValidReason(BSTR* retval)
{
	*retval = A2BSTR(_isValidReason);
	_isValidReason = "";
	return S_OK;
};

/***********************************************************************/
/*		CShape::GetIntersection()
/***********************************************************************/
//  Returns intersection of 2 shapes as an safearray of shapes
 STDMETHODIMP CShape::GetIntersection(IShape* Shape, VARIANT* Results, VARIANT_BOOL* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retval = VARIANT_FALSE;

	if( Shape == NULL)
	{	
		ErrorMessage(tkUNEXPECTED_NULL_PARAMETER);
		return S_OK;
	} 
	
	if (RelateExtents(this, Shape) == erNone)	
		return S_OK;

	OGRGeometry* oGeom1 = NULL;
	OGRGeometry* oGeom2 = NULL;

	oGeom1 = GeometryConverter::ShapeToGeometry(this);
	if (oGeom1 == NULL) return S_OK;
	
	oGeom2 = GeometryConverter::ShapeToGeometry(Shape);
	if (oGeom2 == NULL) 
	{	
		delete oGeom1;
		return S_OK;
	}
	
	OGRGeometry* oGeom3 = NULL;
	oGeom3 = oGeom1->Intersection(oGeom2);

	delete oGeom1;
	delete oGeom2;
	
	if (oGeom3 == NULL) return S_OK;
	
	std::vector<IShape*> vShapes;
	if (!GeometryConverter::GeometryToShapes(oGeom3, &vShapes))return S_OK;
	delete oGeom3;

	if (vShapes.size()!=0) 
	{
		if (Templates::Vector2SafeArray(&vShapes, Results))
			*retval = VARIANT_TRUE;
		vShapes.clear();
	}
	return S_OK;
}

// ***************************************************************
//		get_InteriorPoint
// ***************************************************************
// Returns interior point - located on the horizontal line which goes
// through vertical center of the shapes extents.
STDMETHODIMP CShape::get_InteriorPoint(IPoint** retval)
{
	*retval = NULL;
	
	IExtents* ext = NULL;
	this->get_Extents(&ext);
	if (ext == NULL) 
		return S_OK;

	double xMin, xMax, yMin, yMax;
	ext->get_xMin(&xMin);	ext->get_xMax(&xMax);
	ext->get_yMin(&yMin);	ext->get_yMax(&yMax);
	
	OGRGeometry* oGeom = GeometryConverter::ShapeToGeometry(this);
	if (oGeom == NULL)
	{
		ErrorMessage(tkCANT_CONVERT_SHAPE_GEOS);
		return S_OK;
	}

	OGRLineString* oLine = new OGRLineString();
	oLine->addPoint(xMin,(yMax + yMin)/2);
	oLine->addPoint(xMax,(yMax + yMin)/2);
	
	OGRGeometry* oResult = oGeom->Intersection(oLine);
	if (oResult == NULL)
	{
		// TODO: add error code
		delete oLine; return S_OK;
	}
	
	// Intersection can be line or point; for polygons we are interested
	//   in the longest line, and for polylines - in point which is the closest
	//   to the center of extents
	double x = DBL_MIN;
	double y = DBL_MIN;
	
	ShpfileType shptype = _shp->get_ShapeType();

	if( shptype == SHP_POLYLINE || shptype == SHP_POLYLINEZ || shptype == SHP_POLYLINEM )
	{
		// TODO: write implementation
	}
	if( shptype == SHP_POLYGON || shptype == SHP_POLYGONZ || shptype == SHP_POLYGONM )
	{
		OGRwkbGeometryType oType = oResult->getGeometryType();
		if (oType == wkbLineString || oType == wkbLineString25D)
		{
			OGRLineString* oSubLine = (OGRLineString*) oResult;
			x = (oSubLine->getX(0) + oSubLine->getX(1))/2;
			y = (oSubLine->getY(0) + oSubLine->getY(1))/2;
		}
		else if (oType == wkbGeometryCollection || oType == wkbGeometryCollection25D ||
				 oType == wkbMultiLineString || oType == wkbMultiLineString25D)
		{
			double maxLength = -1;

			OGRGeometryCollection* oColl = (OGRGeometryCollection *) oResult;
			for (long i=0; i < oColl->getNumGeometries(); i++)
			{	
				OGRGeometry* oPart = oColl->getGeometryRef(i);
				if (oPart->getGeometryType() == wkbLineString || oPart->getGeometryType() == wkbLineString25D)
				{
					
					OGRLineString* oSubLine = (OGRLineString*)oPart;
					double length = oSubLine->get_Length();
					if (length > maxLength)
					{
						x = (oSubLine->getX(0) + oSubLine->getX(1))/2;
						y = (oSubLine->getY(0) + oSubLine->getY(1))/2;
						maxLength = length;
					}
				}
			}
		}
	}
	
	if (x != 0 || y !=0 )
	{
		CoCreateInstance( CLSID_Point, NULL, CLSCTX_INPROC_SERVER, IID_IPoint, (void**)retval );
		(*retval)->put_X(x);
		(*retval)->put_Y(y);
	}
	
	// cleaning
	if (oLine)	delete oLine; 
	if (oGeom)	delete oGeom; 
	if (oResult)delete oResult;
	return S_OK;
}
#pragma endregion

#pragma region Serialization
// *************************************************************
//		SerializeToString
// *************************************************************
STDMETHODIMP CShape::SerializeToString(BSTR * Serialized)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	//	fast editing mode
	ShpfileType shptype = _shp->get_ShapeType();

	CString builder = "";
	char cbuf[20];
	double dbuf;
	double dbuf1;
	_itoa(shptype, cbuf, 10);
	builder.Append(cbuf);
	builder.Append(";");

	for(int i = 0; i < (int)_shp->get_PartCount(); i++ )
	{
		sprintf(cbuf, "%d;", _shp->get_PartStartPoint(i));
		builder.Append(cbuf);
	}
	
	for(int i = 0; i < (int)_shp->get_PointCount(); i++ )
	{	
		_shp->get_PointXY(i, dbuf, dbuf1);
		
		sprintf(cbuf, "%f|", dbuf);
		builder.Append(cbuf);

		sprintf(cbuf, "%f|", dbuf1);
		builder.Append(cbuf);

		if (shptype == SHP_MULTIPOINTM || shptype == SHP_POLYGONM || shptype == SHP_POLYLINEM || 
			shptype == SHP_MULTIPOINTZ || shptype == SHP_POLYGONZ || shptype == SHP_POLYLINEZ)
		{
			_shp->get_PointZ(i, dbuf);
			sprintf(cbuf, "%f|", dbuf);
			builder.Append(cbuf);
		}
		if (shptype == SHP_MULTIPOINTM || shptype == SHP_POLYGONM || shptype == SHP_POLYLINEM)
		{
			_shp->get_PointM(i, dbuf);
			sprintf(cbuf, "%f|", dbuf);
			builder.Append(cbuf);
		}
	}
	*Serialized = builder.AllocSysString();
	
	return S_OK;
}

// *************************************************************
//		CreateFromString
// *************************************************************
STDMETHODIMP CShape::CreateFromString(BSTR Serialized, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	USES_CONVERSION;
	
	VARIANT_BOOL rt;
	CString ser = OLE2A(Serialized);
	CString next;
	next = ser.Mid(ser.Find(";")+1);

	ShpfileType newShpType = (ShpfileType)atoi(ser.Left(ser.Find(";")));

	//Test the ShpType
	if(	newShpType == SHP_MULTIPATCH )
	{	
		ErrorMessage(tkUNSUPPORTED_SHAPEFILE_TYPE);
		*retval = VARIANT_FALSE;
	}
	else
	{
		//Discard the old shape information
		_shp->Clear();
	
		_shp->put_ShapeType(newShpType);

		while (next.Find(";") != -1)
		{
			long nextID = _shp->get_PartCount();
			this->InsertPart(atol(next.Left(next.Find(";"))), &nextID, &rt);
			next = next.Mid(next.Find(";")+1);
		}
		
		double x,y,z,m;
		while (next.Find("|") != -1)
		{
			x = Utility::atof_custom(next.Left(next.Find("|")));
			next = next.Mid(next.Find("|")+1);
			y = Utility::atof_custom(next.Left(next.Find("|")));
			
			long nextID = _shp->get_PointCount();
			_shp->InsertPointXY(nextID, x, y);
			
			ShpfileType shptype = _shp->get_ShapeType();

			// Z
			if (shptype == SHP_MULTIPOINTM || shptype == SHP_POLYGONM || shptype == SHP_POLYLINEM || shptype == SHP_MULTIPOINTZ || shptype == SHP_POLYGONZ || shptype == SHP_POLYLINEZ)
			{
				next = next.Mid(next.Find("|")+1);
				z = Utility::atof_custom(next.Left(next.Find("|")));
				_shp->put_PointZ(nextID, z);
			}
			// M
			if (shptype == SHP_MULTIPOINTM || shptype == SHP_POLYGONM || shptype == SHP_POLYLINEM)
			{
				next = next.Mid(next.Find("|")+1);
				m = Utility::atof_custom(next.Left(next.Find("|")));
				_shp->put_PointM(nextID, m);
			}

			next = next.Mid(next.Find("|")+1);
		}
		*retval = VARIANT_TRUE;
	}
	return S_OK;
}
#pragma endregion

#pragma region PointInPolygon

// *****************************************************************
//		PointInThisPoly()
// *****************************************************************
STDMETHODIMP CShape::PointInThisPoly(IPoint * pt, VARIANT_BOOL *retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	bool result;
	if (_useFastMode)
	{
		result = PointInThisPolyFast(pt);
	}
	else
	{
		result = PointInThisPolyRegular(pt);
	}
	*retval = result?VARIANT_TRUE:VARIANT_FALSE;
	return S_OK;
}

// *****************************************************************
//		PointInThisPolyFast()
// *****************************************************************
bool CShape::PointInThisPolyFast(IPoint * pt)
{
	if (!_useFastMode)
	{
		AfxMessageBox("Wrong shape fast edit mode usage");
		return false;
	}

	ShpfileType shptype = _shp->get_ShapeType();
	
	if (!( shptype == SHP_POLYGON || shptype == SHP_POLYGONZ || shptype == SHP_POLYGONM ))
	{
		return false;
	}

	double x;
	double y;
	pt->get_X(&x);
	pt->get_Y(&y);

	double dbuf, dbuf1;

	static double minx = -1;
	static double miny = -1;
	static double maxx = -1;
	static double maxy = -1;
	static bool foundMinsMaxs = false;
	
	if (!foundMinsMaxs)
	{
		if (_shp->get_PointCount() > 0)
		{
			_shp->get_PointXY(0, dbuf, dbuf1);
			minx = maxx = dbuf;
			miny = maxy = dbuf1;
			
			for (int i = 1; i < _shp->get_PointCount(); i++)
			{
				_shp->get_PointXY(i, dbuf, dbuf1);
				if (dbuf > maxx) maxx = dbuf;
				if (dbuf < minx) minx = dbuf;
				
				if (dbuf1 > maxy) maxy = dbuf1;
				if (dbuf1 < miny) miny = dbuf1;
			}
		}
		else
		{
			return false;
		}
		foundMinsMaxs = true;
	}

	if(x < minx || y < miny || x > maxx || y > maxy)
	{
		return false;
	}

	int CrossCount = 0;

	double x1, y1, x2, y2;
	
	for(int nPart = 0; nPart < _shp->get_PartCount(); nPart++)
	{
		int nPointMax = _shp->get_PointCount() - 1;
		
		if (_shp->get_PartCount() - 1 > nPart)
			nPointMax = _shp->get_PartStartPoint(nPart+1) - 1;
		
		for(int nPoint = _shp->get_PartStartPoint(nPart); nPoint < nPointMax; nPoint++)
		{
			_shp->get_PointXY(nPoint, x1, y1);
			x1 -= x;
			y1 -= y;

			_shp->get_PointXY(nPoint + 1, x2, y2);
			x2 -= x;
			y2 -= y;

			register double y1y2 = y1*y2;

			if(y1y2 > 0.0) // If the signs are the same
			{
				// Then it does not cross
				continue;
			}
			else if(y1y2 == 0.0) // Then it has intesected a vertex
			{
				if(y1 == 0.0)
				{
					if( y2 > 0.0 )
						continue;
				}
				else if( y1 > 0.0 )
					continue;
			}

			if( x1 > 0.0 && x2 > 0.0 )
			{
				CrossCount++;
				continue;
			}

			// Calculate Intersection
			if((x1 - y1*((x2 - x1)/(y2 - y1))) > 0.0)
				CrossCount++;
		}
	}

	return (CrossCount&1);
}

// *************************************************************
//		PointInThisPolyRegular()
// *************************************************************
bool CShape::PointInThisPolyRegular(IPoint * pt)
{
	ASSERT(_useFastMode == false);
	
	ShpfileType shptype = _shp->get_ShapeType();

	if (!( shptype == SHP_POLYGON || shptype == SHP_POLYGONZ || shptype == SHP_POLYGONM ))
	{
		return false;
	}

	double x;
	double y;
	pt->get_X(&x);
	pt->get_Y(&y);

	double dbuf;

	static double minx = -1;
	static double miny = -1;
	static double maxx = -1;
	static double maxy = -1;
	static bool foundMinsMaxs = false;
	
	CShapeWrapperCOM* shp = (CShapeWrapperCOM*)_shp;
	std::deque<IPoint*> allPoints = shp->_allPoints;
	std::deque<long> allParts = shp->_allParts;

	if (!foundMinsMaxs)
	{
		if (allPoints.size() > 0)
		{
			allPoints[0]->get_X(&dbuf);
			minx = dbuf;
			maxx = dbuf;
			allPoints[0]->get_Y(&dbuf);
			miny = dbuf;
			maxy = dbuf;
			for (register unsigned int i = 1; i < allPoints.size(); i++)
			{
				allPoints[i]->get_X(&dbuf);
				if (dbuf > maxx) maxx = dbuf;
				if (dbuf < minx) minx = dbuf;
				allPoints[i]->get_Y(&dbuf);
				if (dbuf > maxy) maxy = dbuf;
				if (dbuf < miny) miny = dbuf;
			}
		}
		else
		{
			return false;
		}
		foundMinsMaxs = true;
	}

	if(x < minx || y < miny || x > maxx || y > maxy)
	{
		return false;
	}

	register int CrossCount = 0;

	for(register unsigned int nPart = 0; nPart < allParts.size(); nPart++)
	{
		int nPointMax = allPoints.size() - 1;
		if (allParts.size() - 1 > nPart)
		{
			nPointMax = allParts[nPart+1] - 1;
		}
		for(register int nPoint = allParts[nPart]; nPoint < nPointMax; nPoint++)
		{
			register double x1;
			register double y1;
			register double x2;
			register double y2;
			allPoints[nPoint]->get_X(&x1);
			x1 -= x;
			allPoints[nPoint]->get_Y(&y1);
			y1 -= y;
			allPoints[nPoint+1]->get_X(&x2);
			x2 -= x;
			allPoints[nPoint+1]->get_Y(&y2);
			y2 -= y;

			register double y1y2 = y1*y2;

			if(y1y2 > 0.0) // If the signs are the same
			{
				// Then it does not cross
				continue;
			}
			else if(y1y2 == 0.0) // Then it has intesected a vertex
			{
				if(y1 == 0.0)
				{
					if( y2 > 0.0 )
						continue;
				}
				else if( y1 > 0.0 )
					continue;
			}

			if( x1 > 0.0 && x2 > 0.0 )
			{
				CrossCount++;
				continue;
			}

			// Calculate Intersection
			if((x1 - y1*((x2 - x1)/(y2 - y1))) > 0.0)
				CrossCount++;
		}
	}

	if(CrossCount&1)
	{
		return true;
	}

	return false;
}
#pragma endregion

//*******************************************************************
//		Get_SegmentAngle()
//*******************************************************************
// returns angle in degrees
double CShape::get_SegmentAngle( long segementIndex)
{
	double x1, y1, x2, y2, dx, dy;
	VARIANT_BOOL vbretval;
	long numPoints;
	this->get_NumPoints(&numPoints);
	if (segementIndex > numPoints - 2)
	{
		return 0.0;
	}
	
	this->get_XY(segementIndex, &x1, &y1, &vbretval);
	this->get_XY(segementIndex + 1, &x2, &y2, &vbretval);
	dx = x2 -x1; dy = y2 - y1;
	return GetPointAngle(dx, dy) / pi * 180.0;
}

// **********************************************
//   get_ShapeWrapper()
// **********************************************
IShapeWrapper* CShape::get_ShapeWrapper()
{ 
	return _shp; 
}

// **********************************************
//   put_ShapeWrapper()
// **********************************************
bool CShape::put_ShapeWrapper(CShapeWrapper* data)
{
	if (data == NULL )		// fast mode should be set explictly before using wrapper
	{
		return false;
	}
	else
	{
		if (_shp)
		{
			if (_useFastMode)
			{
				delete (CShapeWrapper*)_shp;
			}
			else
			{
				delete (CShapeWrapperCOM*)_shp;
			}
			_shp = NULL;
		}
		_shp = data;		// shp must never be null
		_useFastMode = true;
		return true;
	}
}


// **********************************************
//   Clone()
// **********************************************
STDMETHODIMP CShape::Clone(IShape** retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	
	(*retval) = NULL;

	IShape* shp = NULL;
	CoCreateInstance( CLSID_Shape, NULL, CLSCTX_INPROC_SERVER, IID_IShape, (void**)&shp);
	if (shp)
	{
		VARIANT_BOOL vbretval;
		ShpfileType shpType;
		this->get_ShapeType(&shpType);
		shp->Create(shpType, &vbretval);
		
		// we shall do all the work through interface; some optimizations for the fast mode - later
		long numParts;
		long numPoints;
		this->get_NumParts(&numParts);
		this->get_NumPoints(&numPoints);

		// copying parts
		for (long i = 0; i < numParts; i++)
		{
			long index;
			this->get_Part(i, &index);
			shp->InsertPart(index, &i, &vbretval);
		}

		// copying points
		for (long i = 0; i < numPoints; i++)
		{
			IPoint* pnt = NULL;
			this->get_Point(i, &pnt);
			shp->InsertPoint(pnt, &i, &vbretval);
		}
		
		shp->put_Key(_key);
		(*retval) = shp;
	}
	return S_OK;
}

// **********************************************
//   Explode()
// **********************************************
// Splits multi-part shapes in the single part ones
STDMETHODIMP CShape::Explode(VARIANT* Results, VARIANT_BOOL* retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState())
	*retval = VARIANT_FALSE;
	
	std::vector<IShape*> vShapes;
	if (ExplodeCore(vShapes))
	{
		// no need to release objects as we return them to the caller
		if (Templates::Vector2SafeArray(&vShapes, Results))
			*retval = VARIANT_TRUE;
	}
	return S_OK;
}

// **********************************************
//   ExplodeCore()
// **********************************************
bool CShape::ExplodeCore(std::vector<IShape*>& vShapes)
{
	vShapes.clear();
	
	ShpfileType shpType;
	this->get_ShapeType(&shpType);

	LONG numParts;
	this->get_NumParts(&numParts);
	if (numParts <= 1)
	{
		IShape* shp = NULL;
		this->Clone(&shp);
		vShapes.push_back(shp);
	}
	else if (shpType != SHP_POLYGON && shpType != SHP_POLYGONM && shpType != SHP_POLYGONZ)
	{
		// for every feature except polygons we just return the parts of shapes
		for (long i = 0; i < numParts; i++)
		{
			IShape* part = NULL;
			this->get_PartAsShape(i, &part);
			vShapes.push_back(part);
		}
	}
	else
	{
		// for polygons holes should be treated, the main problem here is to determine 
		// to which part the hole belong; OGR will be used for this
		OGRGeometry* geom = GeometryConverter::ShapeToGeometry(this);
		if (geom)
		{
			OGRwkbGeometryType type = geom->getGeometryType();
			if (type == wkbMultiPolygon || type == wkbMultiPolygon25D)
			{
				std::vector<OGRGeometry*> polygons;		// polygons shouldn't be deleted as they are only 
														// references to the parts of init multipolygon
				if (GeometryConverter::MultiPolygon2Polygons(geom, &polygons))
				{
					for (unsigned int i = 0; i < polygons.size(); i++)
					{
						IShape* poly = GeometryConverter::GeometryToShape(polygons[i]);
						if (poly)
						{
							vShapes.push_back(poly);
						}
					}
				}
			}
			else
			{
				IShape* shp = GeometryConverter::GeometryToShape(geom);
				if (shp)
				{
					vShapes.push_back(shp);
				}
			}
			delete geom;
		}
	}
	return vShapes.size() > 0;
}

/***********************************************************************/
/*			get_LabelPosition()
/***********************************************************************/
// sub-function for GenerateLabels
// returns coordinates of label and angle of segment rotation for polylines
void CShape::get_LabelPosition(tkLabelPositioning method, double& x, double& y, double& rotation, tkLineLabelOrientation orientation)
{
	x = y = 0.0;
	if (method == lpNone)
		return;

	IPoint* pnt = NULL;
	ShpfileType shpType;
	this->get_ShapeType(&shpType);
	VARIANT_BOOL vbretval;
	int segmentIndex = -1;	// for polylines

	shpType = Utility::ShapeTypeConvert2D(shpType);
	
	if (shpType == SHP_POINT || shpType == SHP_MULTIPOINT)
	{
		VARIANT_BOOL vbretval;
		this->get_XY(0, &x, &y, &vbretval);
	}
	if (shpType == SHP_POLYGON)
	{
		if (method == lpCenter)				this->get_Center(&pnt);
		else if (method == lpCentroid)		this->get_Centroid(&pnt);
		else if (method == lpInteriorPoint)	this->get_InteriorPoint(&pnt);
		else
		{	
			ErrorMessage(tkINVALID_PARAMETER_VALUE); 
			return;
		}
		
		// TODO: interior point can return no result; write some error handling
		if (pnt != NULL)
		{
			pnt->get_X(&x);
			pnt->get_Y(&y);
			pnt->Release(); pnt = NULL;
		}
	}
	else if (shpType == SHP_POLYLINE)
	{
		IPoint* pnt1 = NULL;
		IPoint* pnt2 = NULL;
		
		long numPoints;
		this->get_NumPoints(&numPoints);
		if (numPoints < 2) return;

		double x1, y1, x2, y2;

		if	(method == lpFirstSegment)
		{
			this->get_XY(0, &x, &y, &vbretval);
			segmentIndex = 0;
		}
		else if (method == lpLastSegment)
		{
			this->get_XY(numPoints - 1, &x, &y, &vbretval);
			segmentIndex = numPoints - 2;
		}
		else if (method == lpMiddleSegment)
		{
			double length = 0.0;
			double halfLength, delta;
			this->get_Length(&halfLength);
			halfLength /= 2.0;
			
			long i;
			for (i =0; i < numPoints; i++)
			{
				this->get_XY(i, &x1, &y1, &vbretval);
				this->get_XY(i + 1, &x2, &y2, &vbretval);
				delta = sqrt(pow(x1 - x2, 2.0) + pow(y1 - y2, 2.0));
				if (length + delta < halfLength)
					length += delta;
				else if (length + delta > halfLength)
				{
					double ratio = (halfLength - length)/ (delta);
					x = x1 + (x2 - x1) * ratio;
					y = y1 + (y2 - y1) * ratio;
					break;
				}
				else
				{
					x = x2; y = y2; break;
				}
			}
			segmentIndex = i;
		}
		else if (method == lpLongestSegement)
		{
			double maxLength = 0;
			double length;
			for (long i =0; i < numPoints - 1; i++)	
			{
				this->get_XY(i, &x1, &y1, &vbretval);
				this->get_XY(i + 1, &x2, &y2, &vbretval);
				length = sqrt(pow(x1 - x2, 2.0) + pow(y1 - y2, 2.0));
				if (length > maxLength)
				{
					maxLength = length;
					segmentIndex = i;
				}
			}
			if (segmentIndex != -1)
			{
				this->get_XY(segmentIndex, &x1, &y1, &vbretval);
				this->get_XY(segmentIndex + 1, &x2, &y2, &vbretval);
			}
			x = (x1 + x2)/2.0;
			y = (y1 + y2)/2.0;
		}
		else
		{	// the method is unsupported
			ErrorMessage(tkINVALID_PARAMETER_VALUE); return;
		}

		// defining angle for a segment with label
		if (orientation == lorHorizontal)
		{
			rotation = 0.0;
		}
		else
		{
			rotation = this->get_SegmentAngle(segmentIndex) - 90;
			if (orientation == lorPerpindicular)
			{
				rotation += 90.0;
			}
		}
	}
	else if (shpType == SHP_POINT)
	{
		// just return the point itself inspite of method
		this->get_XY(0, &x, &y, &vbretval);
	}
	else if (shpType == SHP_MULTIPOINT)
	{
		// TODO: return the first point for now; maybe implement several behaviours:
		// first point, last point, point closest to center of mass;
		this->get_XY(0, &x, &y, &vbretval);
	}
	return;
}

// ********************************************************************
//		Bytes2SafeArray()				               
// ********************************************************************
//  Creates safearray with numbers of shapes as long values
//  Returns true when created safearray has elements, and false otherwise
bool Bytes2SafeArray(unsigned char* data, int size, VARIANT* arr)
{
	SAFEARRAY FAR* psa = NULL;
	SAFEARRAYBOUND rgsabound[1];
	rgsabound[0].lLbound = 0;	

	if( size > 0 )
	{
		rgsabound[0].cElements = size;
		psa = SafeArrayCreate( VT_UI1, 1, rgsabound);
    			
		if( psa )
		{
			unsigned char* pchar = NULL;
			SafeArrayAccessData(psa,(void HUGEP* FAR*)(&pchar));
			
			memcpy(pchar,&(data[0]),sizeof(unsigned char)*size);
			
			SafeArrayUnaccessData(psa);
			
			arr->vt = VT_ARRAY|VT_UI1;
			arr->parray = psa;
			return true;
		}
	}
	else
	{
		rgsabound[0].cElements = 0;
		psa = SafeArrayCreate( VT_UI1, 1, rgsabound);
		arr->vt = VT_ARRAY|VT_UI1;
		arr->parray = psa;
	}
	return false;
}

/***********************************************************************/
/*			ExportToBinary()
/***********************************************************************/
STDMETHODIMP CShape::ExportToBinary(VARIANT* bytesArray, VARIANT_BOOL* retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	int* data = NULL;
	int contentLength;
	
	if (_useFastMode)
	{
		data = ((CShapeWrapper*)_shp)->get_ShapeData();
		contentLength = ((CShapeWrapper*)_shp)->get_ContentLength();
	}
	else
	{
		CShapeWrapperCOM* shp = ((CShapeWrapperCOM*)_shp);
		CShapeWrapper* shpNew = this->InitShapeWrapper(shp);
		data = shpNew->get_ShapeData();
		contentLength = shpNew->get_ContentLength();
		delete shpNew;
	}

	if (data)
	{
		unsigned char* buffer = reinterpret_cast<unsigned char*>(data);
		*retVal = Bytes2SafeArray(buffer, contentLength, bytesArray);
		return S_OK;
	}
	else
	{
		*retVal = NULL;
		return S_FALSE;
	}
}

//********************************************************************
//*		ImportFromBinary()
//********************************************************************
STDMETHODIMP CShape::ImportFromBinary(VARIANT bytesArray, VARIANT_BOOL* retVal)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	*retVal = VARIANT_FALSE;
	
	if (bytesArray.vt != (VT_ARRAY|VT_UI1))
		return S_FALSE;
	
	unsigned char* p = NULL;
	SafeArrayAccessData(bytesArray.parray,(void HUGEP* FAR*)(&p));
	char* data = reinterpret_cast<char*>(p);

	bool result = false;
	if (_useFastMode)
	{
		 result = ((CShapeWrapper*)_shp)->put_ShapeData(data);
	}
	else
	{
		CShapeWrapperCOM* shpOld = (CShapeWrapperCOM*)_shp;
		CShapeWrapper* shp = InitShapeWrapper(shpOld);
		if (shp)
		{
			if (shp->put_ShapeData(data))
			{
				CShapeWrapperCOM* shpNew = InitComWrapper(shp);
				if (shpNew)
				{
					_shp = shpNew;
					delete shpOld;
					result = true;
				}
			}
			delete shp;
		}
	}
	*retVal = result ? VARIANT_TRUE : VARIANT_FALSE;
	SafeArrayUnaccessData(bytesArray.parray);
	return S_OK;
}

//*****************************************************************
//*		FixUp()
//*****************************************************************
STDMETHODIMP CShape::FixUp(IShape** retval)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	VARIANT_BOOL vbretval;
	this->get_IsValid(&vbretval);
	if (vbretval)
	{
		this->Clone(retval);
	}
	else
	{
		this->Buffer(m_globalSettings.invalidShapesBufferDistance, 30, retval);
	}

	if (*retval)
	{
		(*retval)->get_IsValid(&vbretval);
		if (!vbretval)
		{
			(*retval)->Release();
			(*retval) = NULL;
		}
	}
	return S_OK;
}