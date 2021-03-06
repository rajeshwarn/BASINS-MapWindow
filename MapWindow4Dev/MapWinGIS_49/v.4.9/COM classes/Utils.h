//********************************************************************************************************
//File name: Utils.h
//Description: Declaration of the CUtils.
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
//3-28-2005 dpa - Added include for the grid interpolator.
//********************************************************************************************************

#pragma once
#include "MapWinGis.h"

#include <comsvcs.h>
#include <deque>
#include <vector>
#include <math.h>

#include "gdal_priv.h"
#include "ogr_geometry.h"

#include "varH.h"
#include "XRedBlackTree.h"
#include "YRedBlackTree.h"
#include "Vector.h"

#include "GridInterpolate.h"
#include "GeometryConverter.h"

extern "C"
{	
//# include "gpc.h"
}

struct BreakVal
{	
	double lowVal;
	double highVal;
};

struct RasterPoint
{	
public:
	RasterPoint()
	{	column = 0;
		row = 0;
	}
	RasterPoint( long c, long r )
	{	column = c;
		row = r;
	}

	long row;
	long column;
};

struct Poly
{	
public:
	Poly()
	{
	}

	std::vector<double> polyX;
	std::vector<double> polyY;
};



// CUtils

class ATL_NO_VTABLE CUtils : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CUtils, &CLSID_Utils>,
	public IDispatchImpl<IUtils, &IID_IUtils, &LIBID_MapWinGIS, /*wMajor =*/ VERSION_MAJOR, /*wMinor =*/ VERSION_MINOR>
{
public:
	CUtils()
	{
		USES_CONVERSION;

		pip_left = 0;
		pip_right = 0;
		pip_top = 0;
		pip_bottom = 0;		

		lastErrorCode = tkNO_ERROR;
		globalCallback = NULL;
		key = A2BSTR("");
			
		BufferA_R = NULL;
		BufferA_G = NULL;
		BufferA_B = NULL;
		BufferB_R = NULL;
		BufferB_G = NULL;
		BufferB_B = NULL;
		BufferLastUsed = 'Z';
		BufferANum = -1;
		BufferBNum = -1;
		rasterDataset = NULL;
	}
	~CUtils()
	{
		::SysFreeString(key);
	}

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}
	
	void FinalRelease() 
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_UTILS)

DECLARE_NOT_AGGREGATABLE(CUtils)

BEGIN_COM_MAP(CUtils)
	COM_INTERFACE_ENTRY(IUtils)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()


// IUtils
public:
	STDMETHOD(ClipPolygon)(/*[in]*/ PolygonOperation op, /*[in]*/ IShape * SubjectPolygon, /*[in]*/ IShape * ClipPolygon, /*[out, retval]*/ IShape ** retval);
	STDMETHOD(get_Key)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_Key)(/*[in]*/ BSTR newVal);
	STDMETHOD(get_GlobalCallback)(/*[out, retval]*/ ICallback * *pVal);
	STDMETHOD(put_GlobalCallback)(/*[in]*/ ICallback * newVal);
	STDMETHOD(get_ErrorMsg)(/*[in]*/ long ErrorCode, /*[out, retval]*/ BSTR *pVal);
	STDMETHOD(get_LastErrorCode)(/*[out, retval]*/ long *pVal);
	STDMETHOD(get_Area)(/*[in]*/ IShape * Shape, /*[out, retval]*/ double *pVal);
	STDMETHOD(get_Perimeter)(/*[in]*/ IShape * Shape, /*[out, retval]*/ double *pVal);
	STDMETHOD(get_Length)(/*[in]*/ IShape * Shape, /*[out, retval]*/ double *pVal);
	STDMETHOD(RemoveColinearPoints)(/*[in,out]*/ IShapefile * Shapes, /*[in]*/double LinearTolerance, /*[in, optional]*/ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(GridInterpolateNoData)(/*[in, out]*/ IGrid * Grid, /*[in, optional]*/ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(GridReplace)(/*[in, out]*/ IGrid * Grid, /*[in]*/ VARIANT OldValue, /*[in]*/ VARIANT NewValue, /*[in, optional]*/ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(PointInPolygon)(/*[in]*/ IShape * Shape, /*[in]*/IPoint * TestPoint, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(ShapeMerge)(/*[in]*/ IShapefile * Shapes, /*[in]*/long IndexOne, /*[in]*/long IndexTwo, /*[in, optional]*/ICallback * cBack, /*[out, retval]*/ IShape ** retval);
	STDMETHOD(GridMerge)(/*[in]*/ VARIANT Grids, /*[in]*/ BSTR MergeFilename, /*[in, optional, defaultvalue(TRUE)]*/ VARIANT_BOOL InRam, /*[in, optional, defaultvalue(UseExtension)]*/ GridFileType GrdFileType, /*[in, optional]*/ICallback * cBack, /*[out, retval]*/ IGrid ** retval);
	STDMETHOD(hBitmapToPicture)(/*[in]*/ long hBitmap, /*[out, retval]*/ IPictureDisp ** retval);
	STDMETHOD(ShapefileToGrid)(/*[in]*/ IShapefile * Shpfile,/* [in, optional, defaultvalue(TRUE)]*/ VARIANT_BOOL UseShapefileBounds, /*[in, optional, defaultvalue(NULL)]*/IGridHeader * GrdHeader, /*[in,optional, defaultvalue(30.0)]*/double Cellsize, /*[in,optional, defaultvalue(TRUE)]*/VARIANT_BOOL UseShapeNumber, /*[in,optional, defaultvalue(1)]*/short SingleValue,/*[out, retval]*/ IGrid ** retval);
	STDMETHOD(TinToShapefile)(/*[in]*/ ITin * Tin, /*[in]*/ ShpfileType Type, /*[in, optional]*/ICallback * cBack, /*[out, retval]*/ IShapefile ** retval);
	STDMETHOD(ShapeToShapeZ)(/*[in]*/ IShapefile * Shapefile, /*[in]*/ IGrid * Grid, /*[in, optional]*/ICallback * cBack, /*[out, retval]*/ IShapefile ** retval);
	STDMETHOD(GridToGrid)(/*[in]*/ IGrid * Grid, /*[in]*/ GridDataType OutDataType, /*[in, optional]*/ICallback * cBack, /*[out, retval]*/ IGrid ** retval);
	STDMETHOD(GridToShapefile)(/*[in]*/ IGrid * Grid, /*[in, optional, defaultvalue(NULL)]*/ IGrid * ConnectionGrid, /*[in, optional]*/ICallback * cBack, /*[out, retval]*/ IShapefile ** retval);
	STDMETHOD(GridToImage)(/*[in]*/ IGrid * Grid, /*[in]*/ IGridColorScheme * ci, /*[in, optional]*/ICallback * cBack, /*[out, retval]*/ IImage ** retval);
	STDMETHOD(GenerateHillShade)(/*[in]*/ BSTR bstrGridFilename, /*[in]*/ BSTR bstrShadeFilename, /*[in, optional, defaultvalue(1)]*/ float z, /*[in, optional, defaultvalue(1)]*/ float scale, /*[in, optional, defaultvalue(315)]*/ float az, /*[in, optional, defaultvalue(45)]*/ float alt, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(GenerateContour)(/*[in]*/ BSTR pszSrcFilename, /*[in]*/ BSTR pszDstFilename, /*[in]*/ double dfInterval, /*[in, optional, defaultvalue(0)]*/ double dfNoData, /*[in, optional, defaultvalue(FALSE)]*/ VARIANT_BOOL Is3D, /* [in, optional, defaultvalue(NULL)] */ VARIANT dblFLArray, /*[in, optional]*/ ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(TranslateRaster)(/*[in]*/ BSTR bstrSrcFilename, /*[in]*/ BSTR bstrDstFilename, /*[in]*/ BSTR bstrOptions, /*[in, optional]*/ ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(OGRLayerToShapefile)(/*[in]*/BSTR Filename, /*[in, optional, defaultvalue(SHP_NULLSHAPE)]*/ ShpfileType shpType, /*[in, optional, defaultvalue(NULL)]*/ICallback *cBack, /*[out, retval]*/IShapefile** sf);
	STDMETHOD(MergeImages)(/*[in]*/SAFEARRAY* InputNames, /*[in]*/BSTR OutputName, VARIANT_BOOL* retVal);
	STDMETHOD(ReprojectShapefile)(IShapefile* sf, IGeoProjection* source, IGeoProjection* target, IShapefile** result);
private:
	inline long findBreak( std::deque<BreakVal> & bvals, double val );
	bool PolygonToGrid(IShape * shape, IGrid ** grid, short cellValue);
	//Polygonal Algorithm
	//Used to minimize stack in recursive call for trace_polygon
	//Cell 4
	long cell4_x;
	long cell4_y;
	//Cell 6
	long cell6_x;
	long cell6_y;
	//Cell 2
	long cell2_x;
	long cell2_y;
	//Cell 8			
	long cell8_x;
	long cell8_y;
	//Flow Directions
	double flow2;
	double flow8;
	double flow4;
	double flow6;
	IGrid * expand_grid;
	IGrid * connection_grid;

	void trace_polygon( long x, long y, std::deque<RasterPoint> & polygon );
	inline bool is_joint( double cell2, double cell8, double cell4, double cell6 );
	inline double getValue( IGrid * Grid, long column, long row );
	inline void setValue( IGrid * Grid, long column, long row, double val );
	void scan_fill_to_edge( double & nodata, long x, long y );
	void mark_edge( double & polygon_id, long x, long y );
	inline bool is_decision( IGrid * g, int x, int y );
private:
	inline bool does_cross( int SH, int NSH, double corner_oneX, double corner_oneY, double corner_twoX, double corner_twoY );
	inline void set_sign( double val, int & SH );
	bool is_clockwise( double x0, double y0, double x1, double y1, double x2, double y2); //ah 11/8/05
	bool is_clockwise(IShape *Shape); //ah 6/3/05
	bool is_clockwise(Poly *polygon);//ah 6/3/05
	
	bool isColinear( POINT one, POINT two, POINT test, double tolerance );
	STDMETHODIMP CUtils::GridToImage_DiskBased(IGrid *Grid, IGridColorScheme *ci, ICallback *cBack, IImage ** retval);
	STDMETHODIMP CUtils::GridToImage_InRAM(IGrid *Grid, IGridColorScheme *ci, ICallback *cBack, IImage ** retval);
	inline void PutBitmapValue(long col, long row, _int32 Rvalue, _int32 Gvalue, _int32 Bvalue, long totalWidth);
	void CreateBitmap(char * filename, long cols, long rows, VARIANT_BOOL * retval);
	bool MemoryAvailable(double bytes);
	void FinalizeAndCloseBitmap(int totalWidth);
	int CUtils::GetEncoderClsid(const WCHAR* format, CLSID* pClsid);
	bool CUtils::SaveBitmap(int width, int height, unsigned char* pixels, BSTR outputName);

	bool CanScanlineBuffer;
	char BufferLastUsed;
	int BufferANum;
	int BufferBNum;
	_int32 * BufferA_R;
	_int32 * BufferA_G;
	_int32 * BufferA_B;
	_int32 * BufferB_R;
	_int32 * BufferB_G;
	_int32 * BufferB_B;
	GDALRasterBand * poBand_R;
	GDALRasterBand * poBand_G;
	GDALRasterBand * poBand_B;
	GDALDataset * rasterDataset;
	std::deque<long> pip_cache_parts;
	std::deque<double> pip_cache_pointsX;
	std::deque<double> pip_cache_pointsY;
	double pip_left;
	double pip_right;
	double pip_top;
	double pip_bottom;
	long lastErrorCode;
	ICallback * globalCallback;
	BSTR key;

	//For TranslateRaster
	bool CUtils::ArgIsNumeric( const char *pszArg );
	void CUtils::AttachMetadata( GDALDatasetH hDS, char **papszMetadataOptions );
	void Usage(CString additional);
	void CUtils::Parse(CString sOrig, CString inFile, CString outFile, int * opts);
	bool bSubCall;
	CStringArray sArr;
	
	void CUtils::ErrorMessage(long ErrorCode);
private:
// lsu 4-march-2010
// Polygon Clipping using gpc. Replaced by GEOS. Will be deleted when will be
// sure that new implementation works.

	//gpc_polygon * ShapeToGPCPolygon(IShape * shp);
	//IShape * GPCPolygonToShape(gpc_polygon * gpc,ShpfileType shptype);
	//bool is_clockwise(gpc_vertex_list *contour);//ah 6/3/05
public:
	
	
	
	STDMETHOD(ColorByName)(tkMapColor name, OLE_COLOR* retVal);
};

OBJECT_ENTRY_AUTO(__uuidof(Utils), CUtils)

