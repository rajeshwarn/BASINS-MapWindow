//********************************************************************************************************
//File name: Shapefile.h
//Description: Declaration of the CShapefile
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

#pragma once
#include "MapWinGis.h"
#include <comsvcs.h>
#include <vector>
#include <map>
#include <io.h>

#include "IndexSearching.h"

#include "Enumerations.h"
#include "DrawingOptions.h"
#include "RotatedRectangle.h"
#include "ShapeDrawingOptions.h"
#include "ShapefileCategories.h"
#include "Shape.h"
#include "Chart.h"
#include "ShapeWrapper.h"
#include "Expression.h"
#include "QTree.h"
#include "GeometryConverter.h"
#include "ShapeData.h"
#include "ogr_spatialref.h"
#include "cpl_minixml.h"

//Shapefile File Info
#define HEADER_BYTES_16 50
#define HEADER_BYTES_32 100
#define FILE_CODE      9994
#define VERSION        1000
#define UNUSEDSIZE 5
#define UNUSEDVAL		0     	    
#define RECORD_HEADER_LENGTH_32 8
#define RECORD_SHAPE_TYPE_32 8

// ****************************************************************
//		ShapeInfo()
// ****************************************************************
// Stores all the information about a single shape from the shapefile
struct ShapeData
{
	ShapeData()
	{
		shape = NULL;
		fastData = NULL;
		chart = NULL;

		category = -1;
		handle = -1;
		size = 0;
		
		selected = false;
		isVisible = false;		// TODO: remove; size property is enough
		//isDrawn = false;
	}

	~ShapeData()
	{
		if (shape)
		{
			shape->Release();
			shape = NULL;
		}

		if (chart)
		{
			delete chart;
			chart = NULL;
		}

		if (fastData)
		{
			delete fastData;
			fastData = NULL;
		}
	}
	
	// shape
	IShape* shape;
	CShapeData* fastData;	// fast non-edit mode
	CChartInfo* chart;
	
	int handle;			// the unique handle of shape, assigned in the increasing order
	int category;		// the ShapefileCategory to which the shape belongs: < 0 - default options
	int size;			// size in pixels at current scale; the shape will be labeled or not considering this size
	
	bool selected;
	bool isVisible;		// because of the visibility expression 		
	//bool isDrawn;		// including categories options and visiblity expression; is set after each redraw

	// labels	
	// TODO: in case of sychronized labels, they should be stored here; unsynchronized labels will be stored in labels class
};

// *********************************************************************
// CShapefile declaration
// *********************************************************************
class ATL_NO_VTABLE CShapefile : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CShapefile, &CLSID_Shapefile>,
	public IDispatchImpl<IShapefile, &IID_IShapefile, &LIBID_MapWinGIS, /*wMajor =*/ VERSION_MAJOR, /*wMinor =*/ VERSION_MINOR>
{
public:
	CShapefile()
	{	
		_stopExecution = NULL;

		_selectionTransparency = 180;
		_selectionAppearance = saSelectionColor;
		_selectionColor = RGB(255, 255, 0);
		_collisionMode = LocalList;
		
		_geometryEngine = engineGeos;
		
		m_sourceType = sstUninitialized;
		
		m_writing = false;
		m_reading = false;
		
		_isEditingShapes = FALSE;
		_fastMode = m_globalSettings.shapefileFastMode ? TRUE : FALSE;
		_minDrawingSize = 1;

	    useSpatialIndex = TRUE;
	    hasSpatialIndex = FALSE;
	    spatialIndexLoaded = FALSE;
		spatialIndexMaxAreaPercent = 0.5;
		spatialIndexNodeCapacity = 100;

		//Neio 20090721
		useQTree = VARIANT_FALSE;
		cacheExtents = FALSE;
		qtree = NULL;

		_shpfile = NULL;
		_shxfile = NULL;

		_shpfiletype = SHP_NULLSHAPE;
		//_numShapes = 0;
		_nextShapeHandle = 0;

		_minX = 0.0;
		_minY = 0.0;
		_minZ = 0.0;
		_maxX = 0.0;
		_maxY = 0.0;
		_maxZ = 0.0;
		_minM = 0.0;
		_maxM = 0.0;
		
		USES_CONVERSION;
		key = A2BSTR("");
		_expression = A2BSTR("");
		globalCallback = NULL;
		lastErrorCode = tkNO_ERROR;
		dbf = NULL;
		
		// creation of children classes
		m_selectDrawOpt = NULL;
		m_defaultDrawOpt = NULL;
		m_labels = NULL;
		m_categories = NULL;
		m_charts = NULL;
		m_geoProjection = NULL;

		CoCreateInstance(CLSID_ShapeDrawingOptions,NULL,CLSCTX_INPROC_SERVER,IID_IShapeDrawingOptions,(void**)&m_selectDrawOpt);
		CoCreateInstance(CLSID_ShapeDrawingOptions,NULL,CLSCTX_INPROC_SERVER,IID_IShapeDrawingOptions,(void**)&m_defaultDrawOpt);
		CoCreateInstance(CLSID_ShapefileCategories,NULL,CLSCTX_INPROC_SERVER,IID_IShapefileCategories,(void**)&m_categories);
		CoCreateInstance(CLSID_Labels,NULL,CLSCTX_INPROC_SERVER,IID_ILabels,(void**)&m_labels);
		CoCreateInstance(CLSID_Charts,NULL,CLSCTX_INPROC_SERVER,IID_ICharts,(void**)&m_charts);
		HRESULT res = CoCreateInstance(CLSID_GeoProjection,NULL,CLSCTX_INPROC_SERVER,IID_IGeoProjection,(void**)&m_geoProjection);

		put_ReferenceToLabels();
		put_ReferenceToCategories();
		put_ReferenceToCharts();
	}
	~CShapefile()
	{			
		::SysFreeString(key);
		::SysFreeString(_expression);

		//Close the files
		if( _shpfile != NULL )	fclose(_shpfile);
		_shpfile = NULL;
		
		if( _shxfile != NULL )	fclose(_shxfile);
		_shxfile = NULL;
		
		if( dbf != NULL )		dbf->Release();
		dbf = NULL;		

		if (spatialIndexLoaded)
				unloadSpatialIndex(spatialIndexID);

		ReleaseMemoryShapes();
		
		for (unsigned int i = 0; i < _shapeData.size(); i++)
		{
			delete _shapeData[i];
		}
		_shapeData.clear();

		if (m_selectDrawOpt != NULL)
		{
			m_selectDrawOpt->Release();
			m_selectDrawOpt = NULL;
		}

		if (m_defaultDrawOpt != NULL)
		{
			m_defaultDrawOpt->Release();
			m_defaultDrawOpt = NULL;
		}

		if (m_labels != NULL)
		{
			put_ReferenceToLabels(true);	// labels class maybe referenced by client and won't be deleted as a result
			m_labels->Release();			// therefore we must clear the reference to the parent as it will be invalid
			m_labels = NULL;
		}

		if (m_categories != NULL)
		{
			put_ReferenceToCategories(true);
			m_categories->Release();
			m_categories = NULL;
		}

		if (m_charts != NULL)
		{
			put_ReferenceToCharts(true);
			m_charts->Release();
			m_charts = NULL;
		}

		if (_stopExecution)
		{
			_stopExecution->Release();
			_stopExecution = NULL;
		}

		if (m_geoProjection)
		{
			m_geoProjection->Release();
		}
	}

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		return S_OK;
	}
	
	void FinalRelease() 
	{
	}

	DECLARE_REGISTRY_RESOURCEID(IDR_SHAPEFILE)

	DECLARE_NOT_AGGREGATABLE(CShapefile)

	BEGIN_COM_MAP(CShapefile)
		COM_INTERFACE_ENTRY(IShapefile)
		COM_INTERFACE_ENTRY(IDispatch)
	END_COM_MAP()


// *********************************************************************
//		IShapefile interface
// *********************************************************************
public:
	STDMETHOD(QuickPoints)(/*[in]*/ long ShapeIndex, /*[in, out]*/ long * NumPoints, /*[out, retval]*/ SAFEARRAY ** retval);
	STDMETHOD(QuickExtents)(/*[in]*/ long ShapeIndex, /*[out, retval]*/ IExtents ** retval);
	STDMETHOD(QuickPoint)(/*[in]*/ long ShapeIndex, /*[in]*/ long PointIndex, /*[out, retval]*/ IPoint ** retval);
	STDMETHOD(get_Filename)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(get_FileHandle)(/*[out, retval]*/ long *pVal);
	STDMETHOD(get_EditingTable)(/*[out, retval]*/ VARIANT_BOOL *pVal);
	STDMETHOD(get_CellValue)(/*[in]*/ long FieldIndex, /*[in]*/ long ShapeIndex, /*[out, retval]*/ VARIANT *pVal);
	STDMETHOD(get_Field)(/*[in]*/ long FieldIndex, /*[out, retval]*/ IField * *pVal);
	STDMETHOD(get_FieldByName)(/*[in]*/ BSTR Fieldname, /*[out, retval] */ IField * *pVal);
	STDMETHOD(StopEditingTable)(/*[in, optional, defaultvalue(TRUE)]*/ VARIANT_BOOL ApplyChanges, /*[in, optional]*/ ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(StartEditingTable)(/*[in, optional]*/ ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(EditCellValue)(/*[in]*/ long FieldIndex, /*[in]*/ long ShapeIndex, /*[in]*/ VARIANT NewVal, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(EditDeleteField)(/*[in]*/ long FieldIndex, /*[in, optional]*/ ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(EditInsertField)(/*[in]*/ IField * NewField, /*[in,out]*/long * FieldIndex, /*[in, optional]*/ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(get_ErrorMsg)(/*[in]*/ long ErrorCode, /*[out, retval]*/ BSTR *pVal);
	STDMETHOD(StopEditingShapes)(/*[in, optional, defaultvalue(TRUE)]*/VARIANT_BOOL ApplyChanges,/*[in,optional,defaultvalue(TRUE)]*/VARIANT_BOOL StopEditTable, /*[in, optional]*/ ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(StartEditingShapes)(/*[in,optional,defaultvalue(TRUE)]*/VARIANT_BOOL StartEditTable,/*[in, optional]*/ ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(SelectShapes)(/*[in]*/ IExtents * BoundBox, /*[in, optional, defaultvalue(0.0)]*/double Tolerance, /*[in, optional, defaultvalue(INTERSECTION)]*/ SelectMode SelectMode, /*[in, out]*/ VARIANT * Result, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(EditDeleteShape)(/*[in]*/long ShapeIndex, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(EditInsertShape)(/*[in]*/IShape * Shape, /*[in, out]*/ long * ShapeIndex, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(EditClear)(/*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(Close)(/*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(SaveAs)(/*[in]*/ BSTR ShapefileName, /*[in, optional]*/ ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(CreateNew)(/*[in]*/ BSTR ShapefileName, /*[in]*/ ShpfileType ShapefileType, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(CreateNewWithShapeID)(/*[in]*/ BSTR ShapefileName, /*[in]*/ ShpfileType ShapefileType, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(Open)(/*[in]*/ BSTR ShapefileName, /*[in, optional]*/ICallback * cBack, /*[out, retval]*/VARIANT_BOOL * retval);
	STDMETHOD(get_Key)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_Key)(/*[in]*/ BSTR newVal);
	STDMETHOD(get_GlobalCallback)(/*[out, retval]*/ ICallback * *pVal);
	STDMETHOD(put_GlobalCallback)(/*[in]*/ ICallback * newVal);
	STDMETHOD(get_CdlgFilter)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(get_LastErrorCode)(/*[out, retval]*/ long *pVal);
	STDMETHOD(get_EditingShapes)(/*[out, retval]*/ VARIANT_BOOL *pVal);
	STDMETHOD(get_Shape)(/*[in]*/long ShapeIndex, /*[out, retval]*/ IShape * *pVal);
	STDMETHOD(get_ShapefileType)(/*[out, retval]*/ ShpfileType *pVal);
	STDMETHOD(get_Extents)(/*[out, retval]*/ IExtents * *pVal);
	STDMETHOD(get_NumFields)(/*[out, retval]*/ long *pVal);
	STDMETHOD(get_NumShapes)(/*[out, retval]*/ long *pVal);
	STDMETHOD(get_Projection)(/*[out, retval]*/ BSTR *pVal);
	STDMETHOD(put_Projection)(/*[in]*/BSTR proj4String);
	STDMETHOD(get_NumPoints)(/*[in]*/ long ShapeIndex, /*[out, retval]*/ long *pVal);
	STDMETHOD(get_UseSpatialIndex)(/*[out, retval]*/VARIANT_BOOL *pVal);
	STDMETHOD(put_UseSpatialIndex)(/*[in]*/VARIANT_BOOL pVal);
 	STDMETHOD(get_HasSpatialIndex)(/*[out, retval]*/VARIANT_BOOL *pVal);
	STDMETHOD(put_HasSpatialIndex)(/*[in]*/VARIANT_BOOL pVal);
    STDMETHOD(CreateSpatialIndex)(/*[in]*/BSTR ShapefileName, /*[out, retval]*/ VARIANT_BOOL  *pVal);
	STDMETHOD(Resource)(/*[in]*/ BSTR newSrcPath, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(IsSpatialIndexValid)(/*[out, retval]*/ VARIANT_BOOL  *pVal);
	STDMETHOD(put_SpatialIndexMaxAreaPercent)(/*[in]*/ DOUBLE newVal);
	STDMETHOD(get_SpatialIndexMaxAreaPercent)(/*[out, retval]*/ DOUBLE* pVal);
	STDMETHOD(get_CanUseSpatialIndex)(/*[in]*/ IExtents* pArea, /*[out, retval]*/ VARIANT_BOOL* pVal);
	STDMETHOD(PointInShape)(LONG ShapeIndex, DOUBLE x, DOUBLE y, VARIANT_BOOL* retval);
	STDMETHOD(PointInShapefile)(DOUBLE x, DOUBLE y, LONG* ShapeIndex);
	STDMETHOD(BeginPointInShapefile)(VARIANT_BOOL* retval);
	STDMETHOD(EndPointInShapefile)(void);
	STDMETHOD(get_CacheExtents)(VARIANT_BOOL * pVal);
	STDMETHOD(put_CacheExtents)(VARIANT_BOOL newVal);
	STDMETHOD(RefreshExtents)(VARIANT_BOOL * pVal);
	STDMETHOD(RefreshShapeExtents)(LONG ShapeId, VARIANT_BOOL *pVal);
	//Neio 2009/07/21 QTree Mode
	STDMETHOD(QuickQueryInEditMode)(/*[in]*/IExtents * BoundBox,int ** Result, int* ResultCount);
	STDMETHOD(get_UseQTree)(VARIANT_BOOL * pVal);
	STDMETHOD(put_UseQTree)(VARIANT_BOOL pVal);
	STDMETHOD(Save)(/*[in, optional]*/ ICallback * cBack, /*[out, retval]*/ VARIANT_BOOL * retval);
	STDMETHOD(GetIntersection)(/*[in]*/VARIANT_BOOL SelectedOnlyOfThis, /*[in]*/IShapefile* sf, /*[in]*/VARIANT_BOOL SelectedOnly, /*[in]*/ ShpfileType fileType, /*[in, optional, defaultvalue(NULL)]*/ ICallback * cBack, /*[out, retval]*/ IShapefile** retval);
	STDMETHOD(SelectByShapefile)(/*[in]*/IShapefile* sf,  /*[in]*/tkSpatialRelation Relation, /*[in]*/VARIANT_BOOL SelectedOnly, /*[in, out]*/ VARIANT *Result, /*[in, optional, defaultvalue(NULL)]*/ ICallback* cBack, /*[out, retval]*/VARIANT_BOOL *retval);
	STDMETHOD(get_SelectionDrawingOptions)(/*[out, retval]*/IShapeDrawingOptions** pVal);
	STDMETHOD(put_SelectionDrawingOptions)(/*[in]*/IShapeDrawingOptions* newVal);
	STDMETHOD(get_ShapeSelected)(/*[in]*/long ShapeIndex,/*[out, retval]*/ VARIANT_BOOL* pVal);
	STDMETHOD(put_ShapeSelected)(/*[in]*/long ShapeIndex, /*[in]*/ VARIANT_BOOL newVal);
	STDMETHOD(get_NumSelected)(/*[out, retval]*/long *pVal);
	STDMETHOD(SelectAll)();
	STDMETHOD(SelectNone)();
	STDMETHOD(InvertSelection)();
	STDMETHOD(Dissolve)(long FieldIndex, VARIANT_BOOL SelectedOnly, IShapefile** sf);
	STDMETHOD(get_Labels)(ILabels** pVal);
	STDMETHOD(put_Labels)(ILabels* newVal);
	STDMETHOD(GenerateLabels)(long FieldIndex, tkLabelPositioning Method, VARIANT_BOOL LargestPartOnly, long* Count);
	STDMETHOD(Clone)(IShapefile** retVal);
	STDMETHOD(get_DefaultDrawingOptions)(IShapeDrawingOptions** pVal);
	STDMETHOD(put_DefaultDrawingOptions)(IShapeDrawingOptions* newVal);
	STDMETHOD(get_Categories)(IShapefileCategories** pVal);
	STDMETHOD(put_Categories)(IShapefileCategories* newVal);
	STDMETHOD(get_Charts)(ICharts** pVal);
	STDMETHOD(put_Charts)(ICharts* newVal);
	STDMETHOD(get_ShapeCategory)(/*[in]*/long ShapeIndex,/*[out, retval]*/ long* pVal);
	STDMETHOD(put_ShapeCategory)(/*[in]*/long ShapeIndex, /*[in]*/ long newVal);
	STDMETHOD(get_Table)(ITable** retVal);
	STDMETHOD(get_VisibilityExpression)(BSTR* retval);
	STDMETHOD(put_VisibilityExpression)(BSTR newVal);
	STDMETHOD(get_FastMode)(VARIANT_BOOL* retval);		// in fast editing mode CShapeWrapper class is used to store shape points
	STDMETHOD(put_FastMode)(VARIANT_BOOL newVal);		// there are some restictions on editing this mode though
	STDMETHOD(get_MinDrawingSize)(LONG* pVal);
	STDMETHOD(put_MinDrawingSize)(LONG newVal);
	STDMETHOD(get_SourceType)(tkShapefileSourceType* pVal);
	STDMETHOD(BufferByDistance)(double Distance, LONG nSegments, VARIANT_BOOL SelectedOnly, VARIANT_BOOL MergeResults, IShapefile** sf);

	STDMETHOD(get_GeometryEngine)(tkGeometryEngine* pVal);
	STDMETHOD(put_GeometryEngine)(tkGeometryEngine pVal);
	
	STDMETHOD(Difference)(VARIANT_BOOL SelectedOnlySubject, IShapefile* sfOverlay, VARIANT_BOOL SelectedOnlyOverlay, IShapefile** retval);
	STDMETHOD(Clip)(VARIANT_BOOL SelectedOnlySubject, IShapefile* sfOverlay, VARIANT_BOOL SelectedOnlyOverlay, IShapefile** retval);
	STDMETHOD(SymmDifference)(VARIANT_BOOL SelectedOnlySubject, IShapefile* sfOverlay, VARIANT_BOOL SelectedOnlyOverlay, IShapefile** retval);
	STDMETHOD(Union)(VARIANT_BOOL SelectedOnlySubject, IShapefile* sfOverlay, VARIANT_BOOL SelectedOnlyOverlay, IShapefile** retval);
	
	STDMETHOD(ExplodeShapes)(VARIANT_BOOL SelectedOnly, IShapefile** retval);
	STDMETHOD(AggregateShapes)(VARIANT_BOOL SelectedOnly, LONG FieldIndex, IShapefile** retval);
	STDMETHOD(ExportSelection)(IShapefile** retval);
	STDMETHOD(Sort)(LONG FieldIndex, VARIANT_BOOL Ascending, IShapefile** retval);
	STDMETHOD(Merge)(VARIANT_BOOL SelectedOnlyThis, IShapefile* sf, VARIANT_BOOL SelectedOnly, IShapefile** retval);

	STDMETHOD(get_SelectionColor)(OLE_COLOR* retval);
	STDMETHOD(put_SelectionColor)(OLE_COLOR newVal);
	STDMETHOD(get_SelectionAppearance)(tkSelectionAppearance* retval);
	STDMETHOD(put_SelectionAppearance)(tkSelectionAppearance newVal);
	STDMETHOD(get_CollisionMode)(tkCollisionMode* retval);
	STDMETHOD(put_CollisionMode)(tkCollisionMode newVal);

	STDMETHOD(get_SelectionTransparency)(BYTE* retval);
	STDMETHOD(put_SelectionTransparency)(BYTE newVal);

	STDMETHOD(put_StopExecution)(IStopExecution* stopper)
	{
		put_ComReference((IDispatch*)stopper, (IDispatch**)&_stopExecution, true);
		return S_OK;
	}

	STDMETHOD(Serialize)(VARIANT_BOOL SaveSelection, BSTR* retVal);
	STDMETHOD(Deserialize)(VARIANT_BOOL LoadSelection, BSTR newVal);

	STDMETHOD(get_GeoProjection)(IGeoProjection** retVal);
	STDMETHOD(put_GeoProjection)(IGeoProjection* pVal);
	STDMETHOD(Reproject)(IGeoProjection* newProjection, LONG* reprojectedCount, IShapefile** retVal);
	STDMETHOD(ReprojectInPlace)(IGeoProjection* newProjection, LONG* reprojectedCount, VARIANT_BOOL* retVal);
	STDMETHOD(SimplifyLines)(DOUBLE Tolerance, VARIANT_BOOL SelectedOnly, IShapefile** retVal);
	STDMETHOD(FixUpShapes)(IShapefile** retVal, VARIANT_BOOL* fixed);

	bool DeserializeCore(VARIANT_BOOL LoadSelection, CPLXMLNode* node);
	CPLXMLNode* SerializeCore(VARIANT_BOOL SaveSelection, CString ElementName);
	
	void CShapefile::CopyFields(IShapefile* target);
	OGRSpatialReference* CShapefile::get_OGRSpatialReference();
	void CShapefile::UpdateLabelsPositioning();

	// Returns underlying shapefile if any
	FILE* CShapefile::get_File()
	{
		return _shpfile;
	}
private:
	void CShapefile::ApplyRandomDrawingOptions();

	// GEOPROCESSING
	void CShapefile::DissolveClipper(long FieldIndex, VARIANT_BOOL SelectedOnly, IShapefile* sf);
	void CShapefile::DissolveGEOS(long FieldIndex, VARIANT_BOOL SelectedOnly, IShapefile* sf);
	
	void CShapefile::DoClipOperation(VARIANT_BOOL SelectedOnlySubject, IShapefile* sfOverlay, 
									 VARIANT_BOOL SelectedOnlyOverlay, IShapefile** retval, 
									 tkClipOperation operation, ShpfileType returnType = SHP_NULLSHAPE);

	// intersection
	void CShapefile::IntersectionGEOS(VARIANT_BOOL SelectedOnlySubject, IShapefile* sfClip, VARIANT_BOOL SelectedOnlyClip, 
									  IShapefile* sfResult, map<long, long>* fieldMap = NULL, 
									  std::set<int>* subjectShapesToSkip = NULL,  std::set<int>* clippingShapesToSkip = NULL );

	void CShapefile::IntersectionClipper(VARIANT_BOOL SelectedOnlySubject, IShapefile* sfClip, VARIANT_BOOL SelectedOnlyClip, 
										 IShapefile* sfResult, map<long, long>* fieldMap = NULL,
										 std::set<int>* subjectShapesToSkip = NULL,  std::set<int>* clippingShapesToSkip = NULL );
	// difference
	void CShapefile::DifferenceGEOS(IShapefile* sfSubject, VARIANT_BOOL SelectedOnlySubject, IShapefile* sfOverlay, VARIANT_BOOL SelectedOnlyOverlay,  
									IShapefile* sfResult, map<long, long>* fieldMap = NULL, std::set<int>* shapesToSkip = NULL);
	void CShapefile::DifferenceClipper(IShapefile* sfSubject, VARIANT_BOOL SelectedOnlySubject, IShapefile* sfClip, VARIANT_BOOL SelectedOnlyClip, 
									IShapefile* sfResult, map<long, long>* fieldMap = NULL, std::set<int>* shapesToSkip = NULL);

	// clip
	void CShapefile::ClipGEOS(VARIANT_BOOL SelectedOnlySubject, IShapefile* sfOverlay, VARIANT_BOOL SelectedOnlyOverlay, IShapefile* sfResult);
	void CShapefile::ClipClipper(VARIANT_BOOL SelectedOnlySubject, IShapefile* sfOverlay, VARIANT_BOOL SelectedOnlyOverlay, IShapefile* sfResult);
	
	// utilities
	void CShapefile::InsertShapesVector(IShapefile* sf, vector<IShape* >& vShapes, 
									IShapefile* sfSubject, long subjectId, std::map<long, long>* fieldMapSubject = NULL,
									IShapefile* sfClip = NULL, long clipId = -1, std::map<long, long>* fieldMapClip = NULL);

	void CShapefile::CopyFields(IShapefile* source, IShapefile* target);
	
	void CShapefile::CopyFields(IShapefile* sfSubject, IShapefile* sfOverlay, IShapefile* sfResult, map<long, long>& fieldMap, bool mergeFields = false);
	bool CShapefile::FieldsAreEqual(IField* field1, IField* field2);

	bool CShapefile::ReprojectCore(IGeoProjection* newProjection, LONG* reprojectedCount, IShapefile** retVal, bool reprojectInPlace);
	
	// MEMBER VARIABLES
	IGeoProjection* m_geoProjection;
	
	IStopExecution* _stopExecution;

	// collision modre for point shapefiles
	tkCollisionMode _collisionMode;
	
	// GEOS or Clipper
	tkGeometryEngine _geometryEngine;

	// table is initialized in CreateNew or Open methods
	// it is is destroyed in Close() method
	// in case table is null, shapefile will be considered uninitialized
	ITable * dbf;
	
	// flag showing that shapefile is currently writing to the file
	bool m_writing;

	// flag showing that shapefile is currently reading into memory
	bool m_reading;
	
	// is it disk-based or in-memory?
	tkShapefileSourceType m_sourceType;
	
	// visibility expression
	BSTR _expression;
	
	//Flag for Disk vs. Memory
	BOOL _isEditingShapes;
	
	// When this flag is on CShapeWrapper will be used in the Shape class to stre the points
	// otherwise usual COM points
	BOOL _fastMode;
	
	// objects which are less than this value in pixels for current scale, will drawn as point
	int _minDrawingSize;

	//Flags for Spatial Indexing
	BOOL useSpatialIndex;
	BOOL hasSpatialIndex;
	CSpatialIndexID spatialIndexID;
	BOOL spatialIndexLoaded;	
	DOUBLE spatialIndexMaxAreaPercent;
	int spatialIndexNodeCapacity;
	
	// drawing options
	IShapeDrawingOptions* m_selectDrawOpt;	
	IShapeDrawingOptions* m_defaultDrawOpt;
	tkSelectionAppearance _selectionAppearance;
	OLE_COLOR _selectionColor;
	unsigned char _selectionTransparency;

	// charts
	ICharts* m_charts;

	// extents won't be recalculated in each get_Extents call
	BOOL cacheExtents;

	//Neio (07/21/2009) shapes are stored in QTree (EDIT MODE)
	QTree* qtree;
	BOOL useQTree;
	vector<int> deleteElement;

	//Disk access
	FILE * _shpfile;
	FILE * _shxfile;

	//
	ShpfileType _shpfiletype;
	//long _numShapes;
	long _nextShapeHandle;		// the next unique handle to assign
	
	// data for point in shapefile test
	struct ShapeHeader 
	{
		double MinX, MinY;
		double MaxX, MaxY;
		int NumParts;
		int NumPoints;
	};
	struct PolygonShapefile
	{
		ShapeHeader shpHeader;
		std::vector<Point2D> Points;
		std::vector<int> Parts;
	};
	std::vector<PolygonShapefile> m_PolySF;

	//Extent Information
	double _minX;
	double _minY;
	double _minZ;
	double _maxX;
	double _maxY;
	double _maxZ;
	double _minM;
	double _maxM;

	BSTR key;
	long lastErrorCode;
	ICallback * globalCallback;
	
	std::vector<ShapeData*> _shapeData;
	std::vector<long> shpOffsets;		//(32 bit words)
	
	//Trio of Filenames
	CString _shpfileName;
	CString _shxfileName;
	CString _dbffileName;
	CString _prjfileName;
	
	ILabels* m_labels;
	//CString m_projection;	// projection string
	
	IShapefileCategories* m_categories;

	// -------------------------------------------------------------
	//	private functions
	// -------------------------------------------------------------
	BOOL ReleaseMemoryShapes();
	BOOL verifyMemShapes(ICallback * cBack);
	long FindNewShapeID(long FieldIndex);

	//Read Write Functions
	BOOL readShx();
	BOOL writeShx(FILE * _shxfile, ICallback * cBack);
	BOOL writeShp(FILE * shpfile, ICallback * cBack);	

	// selection Functions
	BOOL defineShapeBounds(long ShapeIndex, ShpfileType & ShapeType, double &s_minX, double &s_minY, double &s_maxX, double &s_maxY );	
	BOOL defineShapePoints(long ShapeIndex, ShpfileType & ShapeType, std::vector<long> & parts, std::vector<double> & xPts, std::vector<double> & yPts );
	BOOL pointInPolygon( long ShapeIndex, double x, double y );
	
	//Neio 2009 07 22
	void GenerateQTree();
	void TrimMemShapes();
	
	bool UniqueFieldNames(IShapefile* sf);
	
	QTree* GenerateLocalQTree(IShapefile* sf, bool SelectedOnly);
	VARIANT_BOOL SelectShapesAlt(IExtents *BoundBox, double Tolerance, SelectMode SelectMode, VARIANT* arr);
	//void CShapefile::get_LabelPosition(IShape* shp, tkLabelPositioning method, double& x, double& y, double& rotation);
	void put_ReferenceToLabels(bool bNullReference = false);
	void put_ReferenceToCategories(bool bNullReference = false);
	void put_ReferenceToCharts(bool bNullReference = false);
	int get_OuterRingIndex(int ShapeIndex, int PartIndex);
	
public:
	// -----------------------------------------------------------------
	//	public functions
	// -----------------------------------------------------------------
	void ErrorMessage(long ErrorCode);
	void ErrorMessage(long ErrorCode, ICallback* cBack);
	int get_ShapeCategory(int ShapeIndex);
	
	void SetChartsPositions(tkLabelPositioning Method);
	bool ReadChartFields(std::vector<double*>* values);
	bool ReadChartField(std::vector<double>* values, int FieldIndex);
	void ClearChartFrames();
	
	// fast access
	std::vector<ShapeData*>* get_ShapeVector()
	{
		return &_shapeData;
	}
	IShapeWrapper* get_ShapeWrapper(int ShapeIndex)
	{
		return ((CShape*)_shapeData[ShapeIndex]->shape)->get_ShapeWrapper();
	}
	IShapeData* get_ShapeData(int ShapeIndex)
	{
		return (_shapeData[ShapeIndex])->fastData;
	}
	IExtents* get_ShapeExtents(int ShapeIndex)
	{
		double xMin, xMax, yMin, yMax;
		IExtents* ext = NULL;
		if (_fastMode)
		{
			_shapeData[ShapeIndex]->fastData->get_BoundsXY(xMin, xMax, yMin, yMax);
			CoCreateInstance(CLSID_Extents,NULL,CLSCTX_INPROC_SERVER,IID_IExtents,(void**)&ext);
			ext->SetBounds(xMin, yMin, 0, xMax, yMax, 0);
			ext->AddRef();
		}
		else
		{
			_shapeData[ShapeIndex]->shape->get_Extents(&ext);
		}
		return ext;
	}


	
	
};
OBJECT_ENTRY_AUTO(__uuidof(Shapefile), CShapefile)
