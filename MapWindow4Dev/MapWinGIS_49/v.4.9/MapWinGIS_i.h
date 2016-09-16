

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0500 */
/* at Mon Oct 17 14:48:04 2011
 */
/* Compiler settings for .\MapWinGIS.odl:
    Oicf, W0, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __MapWinGIS_i_h__
#define __MapWinGIS_i_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IGrid_FWD_DEFINED__
#define __IGrid_FWD_DEFINED__
typedef interface IGrid IGrid;
#endif 	/* __IGrid_FWD_DEFINED__ */


#ifndef __IGridHeader_FWD_DEFINED__
#define __IGridHeader_FWD_DEFINED__
typedef interface IGridHeader IGridHeader;
#endif 	/* __IGridHeader_FWD_DEFINED__ */


#ifndef __IESRIGridManager_FWD_DEFINED__
#define __IESRIGridManager_FWD_DEFINED__
typedef interface IESRIGridManager IESRIGridManager;
#endif 	/* __IESRIGridManager_FWD_DEFINED__ */


#ifndef __IImage_FWD_DEFINED__
#define __IImage_FWD_DEFINED__
typedef interface IImage IImage;
#endif 	/* __IImage_FWD_DEFINED__ */


#ifndef __IShapefile_FWD_DEFINED__
#define __IShapefile_FWD_DEFINED__
typedef interface IShapefile IShapefile;
#endif 	/* __IShapefile_FWD_DEFINED__ */


#ifndef __IShape_FWD_DEFINED__
#define __IShape_FWD_DEFINED__
typedef interface IShape IShape;
#endif 	/* __IShape_FWD_DEFINED__ */


#ifndef __IExtents_FWD_DEFINED__
#define __IExtents_FWD_DEFINED__
typedef interface IExtents IExtents;
#endif 	/* __IExtents_FWD_DEFINED__ */


#ifndef __IPoint_FWD_DEFINED__
#define __IPoint_FWD_DEFINED__
typedef interface IPoint IPoint;
#endif 	/* __IPoint_FWD_DEFINED__ */


#ifndef __ITable_FWD_DEFINED__
#define __ITable_FWD_DEFINED__
typedef interface ITable ITable;
#endif 	/* __ITable_FWD_DEFINED__ */


#ifndef __IField_FWD_DEFINED__
#define __IField_FWD_DEFINED__
typedef interface IField IField;
#endif 	/* __IField_FWD_DEFINED__ */


#ifndef __IShapeNetwork_FWD_DEFINED__
#define __IShapeNetwork_FWD_DEFINED__
typedef interface IShapeNetwork IShapeNetwork;
#endif 	/* __IShapeNetwork_FWD_DEFINED__ */


#ifndef __ICallback_FWD_DEFINED__
#define __ICallback_FWD_DEFINED__
typedef interface ICallback ICallback;
#endif 	/* __ICallback_FWD_DEFINED__ */


#ifndef __IStopExecution_FWD_DEFINED__
#define __IStopExecution_FWD_DEFINED__
typedef interface IStopExecution IStopExecution;
#endif 	/* __IStopExecution_FWD_DEFINED__ */


#ifndef __IUtils_FWD_DEFINED__
#define __IUtils_FWD_DEFINED__
typedef interface IUtils IUtils;
#endif 	/* __IUtils_FWD_DEFINED__ */


#ifndef __IVector_FWD_DEFINED__
#define __IVector_FWD_DEFINED__
typedef interface IVector IVector;
#endif 	/* __IVector_FWD_DEFINED__ */


#ifndef __IGridColorScheme_FWD_DEFINED__
#define __IGridColorScheme_FWD_DEFINED__
typedef interface IGridColorScheme IGridColorScheme;
#endif 	/* __IGridColorScheme_FWD_DEFINED__ */


#ifndef __IGridColorBreak_FWD_DEFINED__
#define __IGridColorBreak_FWD_DEFINED__
typedef interface IGridColorBreak IGridColorBreak;
#endif 	/* __IGridColorBreak_FWD_DEFINED__ */


#ifndef __ITin_FWD_DEFINED__
#define __ITin_FWD_DEFINED__
typedef interface ITin ITin;
#endif 	/* __ITin_FWD_DEFINED__ */


#ifndef __IShapeDrawingOptions_FWD_DEFINED__
#define __IShapeDrawingOptions_FWD_DEFINED__
typedef interface IShapeDrawingOptions IShapeDrawingOptions;
#endif 	/* __IShapeDrawingOptions_FWD_DEFINED__ */


#ifndef __ILabel_FWD_DEFINED__
#define __ILabel_FWD_DEFINED__
typedef interface ILabel ILabel;
#endif 	/* __ILabel_FWD_DEFINED__ */


#ifndef __ILabels_FWD_DEFINED__
#define __ILabels_FWD_DEFINED__
typedef interface ILabels ILabels;
#endif 	/* __ILabels_FWD_DEFINED__ */


#ifndef __ILabelCategory_FWD_DEFINED__
#define __ILabelCategory_FWD_DEFINED__
typedef interface ILabelCategory ILabelCategory;
#endif 	/* __ILabelCategory_FWD_DEFINED__ */


#ifndef __IShapefileCategories_FWD_DEFINED__
#define __IShapefileCategories_FWD_DEFINED__
typedef interface IShapefileCategories IShapefileCategories;
#endif 	/* __IShapefileCategories_FWD_DEFINED__ */


#ifndef __IShapefileCategory_FWD_DEFINED__
#define __IShapefileCategory_FWD_DEFINED__
typedef interface IShapefileCategory IShapefileCategory;
#endif 	/* __IShapefileCategory_FWD_DEFINED__ */


#ifndef __ICharts_FWD_DEFINED__
#define __ICharts_FWD_DEFINED__
typedef interface ICharts ICharts;
#endif 	/* __ICharts_FWD_DEFINED__ */


#ifndef __IChart_FWD_DEFINED__
#define __IChart_FWD_DEFINED__
typedef interface IChart IChart;
#endif 	/* __IChart_FWD_DEFINED__ */


#ifndef __IColorScheme_FWD_DEFINED__
#define __IColorScheme_FWD_DEFINED__
typedef interface IColorScheme IColorScheme;
#endif 	/* __IColorScheme_FWD_DEFINED__ */


#ifndef __IChartField_FWD_DEFINED__
#define __IChartField_FWD_DEFINED__
typedef interface IChartField IChartField;
#endif 	/* __IChartField_FWD_DEFINED__ */


#ifndef __ILinePattern_FWD_DEFINED__
#define __ILinePattern_FWD_DEFINED__
typedef interface ILinePattern ILinePattern;
#endif 	/* __ILinePattern_FWD_DEFINED__ */


#ifndef __ILineSegment_FWD_DEFINED__
#define __ILineSegment_FWD_DEFINED__
typedef interface ILineSegment ILineSegment;
#endif 	/* __ILineSegment_FWD_DEFINED__ */


#ifndef __IGeoProjection_FWD_DEFINED__
#define __IGeoProjection_FWD_DEFINED__
typedef interface IGeoProjection IGeoProjection;
#endif 	/* __IGeoProjection_FWD_DEFINED__ */


#ifndef __IGlobalSettings_FWD_DEFINED__
#define __IGlobalSettings_FWD_DEFINED__
typedef interface IGlobalSettings IGlobalSettings;
#endif 	/* __IGlobalSettings_FWD_DEFINED__ */


#ifndef __ITiles_FWD_DEFINED__
#define __ITiles_FWD_DEFINED__
typedef interface ITiles ITiles;
#endif 	/* __ITiles_FWD_DEFINED__ */


#ifndef ___DMap_FWD_DEFINED__
#define ___DMap_FWD_DEFINED__
typedef interface _DMap _DMap;
#endif 	/* ___DMap_FWD_DEFINED__ */


#ifndef ___DMapEvents_FWD_DEFINED__
#define ___DMapEvents_FWD_DEFINED__
typedef interface _DMapEvents _DMapEvents;
#endif 	/* ___DMapEvents_FWD_DEFINED__ */


#ifndef __IShapefileColorScheme_FWD_DEFINED__
#define __IShapefileColorScheme_FWD_DEFINED__
typedef interface IShapefileColorScheme IShapefileColorScheme;
#endif 	/* __IShapefileColorScheme_FWD_DEFINED__ */


#ifndef __IShapefileColorBreak_FWD_DEFINED__
#define __IShapefileColorBreak_FWD_DEFINED__
typedef interface IShapefileColorBreak IShapefileColorBreak;
#endif 	/* __IShapefileColorBreak_FWD_DEFINED__ */


#ifndef __Map_FWD_DEFINED__
#define __Map_FWD_DEFINED__

#ifdef __cplusplus
typedef class Map Map;
#else
typedef struct Map Map;
#endif /* __cplusplus */

#endif 	/* __Map_FWD_DEFINED__ */


#ifndef __ShapefileColorScheme_FWD_DEFINED__
#define __ShapefileColorScheme_FWD_DEFINED__

#ifdef __cplusplus
typedef class ShapefileColorScheme ShapefileColorScheme;
#else
typedef struct ShapefileColorScheme ShapefileColorScheme;
#endif /* __cplusplus */

#endif 	/* __ShapefileColorScheme_FWD_DEFINED__ */


#ifndef __ShapefileColorBreak_FWD_DEFINED__
#define __ShapefileColorBreak_FWD_DEFINED__

#ifdef __cplusplus
typedef class ShapefileColorBreak ShapefileColorBreak;
#else
typedef struct ShapefileColorBreak ShapefileColorBreak;
#endif /* __cplusplus */

#endif 	/* __ShapefileColorBreak_FWD_DEFINED__ */


#ifndef __Grid_FWD_DEFINED__
#define __Grid_FWD_DEFINED__

#ifdef __cplusplus
typedef class Grid Grid;
#else
typedef struct Grid Grid;
#endif /* __cplusplus */

#endif 	/* __Grid_FWD_DEFINED__ */


#ifndef __GridHeader_FWD_DEFINED__
#define __GridHeader_FWD_DEFINED__

#ifdef __cplusplus
typedef class GridHeader GridHeader;
#else
typedef struct GridHeader GridHeader;
#endif /* __cplusplus */

#endif 	/* __GridHeader_FWD_DEFINED__ */


#ifndef __ESRIGridManager_FWD_DEFINED__
#define __ESRIGridManager_FWD_DEFINED__

#ifdef __cplusplus
typedef class ESRIGridManager ESRIGridManager;
#else
typedef struct ESRIGridManager ESRIGridManager;
#endif /* __cplusplus */

#endif 	/* __ESRIGridManager_FWD_DEFINED__ */


#ifndef __Image_FWD_DEFINED__
#define __Image_FWD_DEFINED__

#ifdef __cplusplus
typedef class Image Image;
#else
typedef struct Image Image;
#endif /* __cplusplus */

#endif 	/* __Image_FWD_DEFINED__ */


#ifndef __Shapefile_FWD_DEFINED__
#define __Shapefile_FWD_DEFINED__

#ifdef __cplusplus
typedef class Shapefile Shapefile;
#else
typedef struct Shapefile Shapefile;
#endif /* __cplusplus */

#endif 	/* __Shapefile_FWD_DEFINED__ */


#ifndef __Shape_FWD_DEFINED__
#define __Shape_FWD_DEFINED__

#ifdef __cplusplus
typedef class Shape Shape;
#else
typedef struct Shape Shape;
#endif /* __cplusplus */

#endif 	/* __Shape_FWD_DEFINED__ */


#ifndef __Extents_FWD_DEFINED__
#define __Extents_FWD_DEFINED__

#ifdef __cplusplus
typedef class Extents Extents;
#else
typedef struct Extents Extents;
#endif /* __cplusplus */

#endif 	/* __Extents_FWD_DEFINED__ */


#ifndef __Point_FWD_DEFINED__
#define __Point_FWD_DEFINED__

#ifdef __cplusplus
typedef class Point Point;
#else
typedef struct Point Point;
#endif /* __cplusplus */

#endif 	/* __Point_FWD_DEFINED__ */


#ifndef __Table_FWD_DEFINED__
#define __Table_FWD_DEFINED__

#ifdef __cplusplus
typedef class Table Table;
#else
typedef struct Table Table;
#endif /* __cplusplus */

#endif 	/* __Table_FWD_DEFINED__ */


#ifndef __Field_FWD_DEFINED__
#define __Field_FWD_DEFINED__

#ifdef __cplusplus
typedef class Field Field;
#else
typedef struct Field Field;
#endif /* __cplusplus */

#endif 	/* __Field_FWD_DEFINED__ */


#ifndef __ShapeNetwork_FWD_DEFINED__
#define __ShapeNetwork_FWD_DEFINED__

#ifdef __cplusplus
typedef class ShapeNetwork ShapeNetwork;
#else
typedef struct ShapeNetwork ShapeNetwork;
#endif /* __cplusplus */

#endif 	/* __ShapeNetwork_FWD_DEFINED__ */


#ifndef __Utils_FWD_DEFINED__
#define __Utils_FWD_DEFINED__

#ifdef __cplusplus
typedef class Utils Utils;
#else
typedef struct Utils Utils;
#endif /* __cplusplus */

#endif 	/* __Utils_FWD_DEFINED__ */


#ifndef __Vector_FWD_DEFINED__
#define __Vector_FWD_DEFINED__

#ifdef __cplusplus
typedef class Vector Vector;
#else
typedef struct Vector Vector;
#endif /* __cplusplus */

#endif 	/* __Vector_FWD_DEFINED__ */


#ifndef __GridColorScheme_FWD_DEFINED__
#define __GridColorScheme_FWD_DEFINED__

#ifdef __cplusplus
typedef class GridColorScheme GridColorScheme;
#else
typedef struct GridColorScheme GridColorScheme;
#endif /* __cplusplus */

#endif 	/* __GridColorScheme_FWD_DEFINED__ */


#ifndef __GridColorBreak_FWD_DEFINED__
#define __GridColorBreak_FWD_DEFINED__

#ifdef __cplusplus
typedef class GridColorBreak GridColorBreak;
#else
typedef struct GridColorBreak GridColorBreak;
#endif /* __cplusplus */

#endif 	/* __GridColorBreak_FWD_DEFINED__ */


#ifndef __Tin_FWD_DEFINED__
#define __Tin_FWD_DEFINED__

#ifdef __cplusplus
typedef class Tin Tin;
#else
typedef struct Tin Tin;
#endif /* __cplusplus */

#endif 	/* __Tin_FWD_DEFINED__ */


#ifndef __ShapeDrawingOptions_FWD_DEFINED__
#define __ShapeDrawingOptions_FWD_DEFINED__

#ifdef __cplusplus
typedef class ShapeDrawingOptions ShapeDrawingOptions;
#else
typedef struct ShapeDrawingOptions ShapeDrawingOptions;
#endif /* __cplusplus */

#endif 	/* __ShapeDrawingOptions_FWD_DEFINED__ */


#ifndef __Labels_FWD_DEFINED__
#define __Labels_FWD_DEFINED__

#ifdef __cplusplus
typedef class Labels Labels;
#else
typedef struct Labels Labels;
#endif /* __cplusplus */

#endif 	/* __Labels_FWD_DEFINED__ */


#ifndef __LabelCategory_FWD_DEFINED__
#define __LabelCategory_FWD_DEFINED__

#ifdef __cplusplus
typedef class LabelCategory LabelCategory;
#else
typedef struct LabelCategory LabelCategory;
#endif /* __cplusplus */

#endif 	/* __LabelCategory_FWD_DEFINED__ */


#ifndef __Label_FWD_DEFINED__
#define __Label_FWD_DEFINED__

#ifdef __cplusplus
typedef class Label Label;
#else
typedef struct Label Label;
#endif /* __cplusplus */

#endif 	/* __Label_FWD_DEFINED__ */


#ifndef __ShapefileCategories_FWD_DEFINED__
#define __ShapefileCategories_FWD_DEFINED__

#ifdef __cplusplus
typedef class ShapefileCategories ShapefileCategories;
#else
typedef struct ShapefileCategories ShapefileCategories;
#endif /* __cplusplus */

#endif 	/* __ShapefileCategories_FWD_DEFINED__ */


#ifndef __ShapefileCategory_FWD_DEFINED__
#define __ShapefileCategory_FWD_DEFINED__

#ifdef __cplusplus
typedef class ShapefileCategory ShapefileCategory;
#else
typedef struct ShapefileCategory ShapefileCategory;
#endif /* __cplusplus */

#endif 	/* __ShapefileCategory_FWD_DEFINED__ */


#ifndef __Charts_FWD_DEFINED__
#define __Charts_FWD_DEFINED__

#ifdef __cplusplus
typedef class Charts Charts;
#else
typedef struct Charts Charts;
#endif /* __cplusplus */

#endif 	/* __Charts_FWD_DEFINED__ */


#ifndef __Chart_FWD_DEFINED__
#define __Chart_FWD_DEFINED__

#ifdef __cplusplus
typedef class Chart Chart;
#else
typedef struct Chart Chart;
#endif /* __cplusplus */

#endif 	/* __Chart_FWD_DEFINED__ */


#ifndef __ColorScheme_FWD_DEFINED__
#define __ColorScheme_FWD_DEFINED__

#ifdef __cplusplus
typedef class ColorScheme ColorScheme;
#else
typedef struct ColorScheme ColorScheme;
#endif /* __cplusplus */

#endif 	/* __ColorScheme_FWD_DEFINED__ */


#ifndef __ChartField_FWD_DEFINED__
#define __ChartField_FWD_DEFINED__

#ifdef __cplusplus
typedef class ChartField ChartField;
#else
typedef struct ChartField ChartField;
#endif /* __cplusplus */

#endif 	/* __ChartField_FWD_DEFINED__ */


#ifndef __LinePattern_FWD_DEFINED__
#define __LinePattern_FWD_DEFINED__

#ifdef __cplusplus
typedef class LinePattern LinePattern;
#else
typedef struct LinePattern LinePattern;
#endif /* __cplusplus */

#endif 	/* __LinePattern_FWD_DEFINED__ */


#ifndef __LineSegment_FWD_DEFINED__
#define __LineSegment_FWD_DEFINED__

#ifdef __cplusplus
typedef class LineSegment LineSegment;
#else
typedef struct LineSegment LineSegment;
#endif /* __cplusplus */

#endif 	/* __LineSegment_FWD_DEFINED__ */


#ifndef __GeoProjection_FWD_DEFINED__
#define __GeoProjection_FWD_DEFINED__

#ifdef __cplusplus
typedef class GeoProjection GeoProjection;
#else
typedef struct GeoProjection GeoProjection;
#endif /* __cplusplus */

#endif 	/* __GeoProjection_FWD_DEFINED__ */


#ifndef __GlobalSettings_FWD_DEFINED__
#define __GlobalSettings_FWD_DEFINED__

#ifdef __cplusplus
typedef class GlobalSettings GlobalSettings;
#else
typedef struct GlobalSettings GlobalSettings;
#endif /* __cplusplus */

#endif 	/* __GlobalSettings_FWD_DEFINED__ */


#ifndef __Tiles_FWD_DEFINED__
#define __Tiles_FWD_DEFINED__

#ifdef __cplusplus
typedef class Tiles Tiles;
#else
typedef struct Tiles Tiles;
#endif /* __cplusplus */

#endif 	/* __Tiles_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_MapWinGIS_0000_0000 */
/* [local] */ 
































typedef /* [helpstring][uuid] */  DECLSPEC_UUID("FD17FF91-8B93-47a2-A517-B4039579B549") 
enum tkCursor
    {	crsrMapDefault	= 0,
	crsrAppStarting	= 1,
	crsrArrow	= 2,
	crsrCross	= 3,
	crsrHelp	= 4,
	crsrIBeam	= 5,
	crsrNo	= 6,
	crsrSizeAll	= 7,
	crsrSizeNESW	= 8,
	crsrSizeNS	= 9,
	crsrSizeNWSE	= 10,
	crsrSizeWE	= 11,
	crsrUpArrow	= 12,
	crsrWait	= 13,
	crsrUserDefined	= 14
    } 	tkCursor;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("BCDBD4E0-8B7C-11DA-A72B-0800200C9A66") 
enum tkResizeBehavior
    {	rbClassic	= 0,
	rbModern	= 1,
	rbIntuitive	= 2,
	rbWarp	= 3,
	rbKeepScale	= 4
    } 	tkResizeBehavior;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("9106CF0F-8A9A-4040-A4B0-D60B72B46504") 
enum tkCursorMode
    {	cmZoomIn	= 0,
	cmZoomOut	= 1,
	cmPan	= 2,
	cmSelection	= 3,
	cmNone	= 4
    } 	tkCursorMode;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("F4FB70AE-68F3-45d4-945F-78EE26A28F1D") 
enum tkLineStipple
    {	lsNone	= 0,
	lsDotted	= 1,
	lsDashed	= 2,
	lsDashDotDash	= 3,
	lsDoubleSolid	= 4,
	lsDoubleSolidPlusDash	= 5,
	lsTrainTracks	= 6,
	lsCustom	= 7,
	lsDashDotDot	= 8
    } 	tkLineStipple;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("546FF8CF-249A-48e6-AD00-7015854D77B1") 
enum tkFillStipple
    {	fsNone	= 0,
	fsVerticalBars	= 1,
	fsHorizontalBars	= 2,
	fsDiagonalDownRight	= 3,
	fsDiagonalDownLeft	= 4,
	fsPolkaDot	= 5,
	fsCustom	= 6,
	fsCross	= 7,
	fsRaster	= 8
    } 	tkFillStipple;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("320AC432-2396-4e9f-9BCB-EC87DE8449BE") 
enum tkDrawReferenceList
    {	dlScreenReferencedList	= 0,
	dlSpatiallyReferencedList	= 1
    } 	tkDrawReferenceList;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("FAB764C1-87FC-402a-AE3D-9C15476C1571") 
enum tkDrawMode
    {	dmPoints	= 0,
	dmLines	= 1,
	dmLineLoop	= 2,
	dmLineStrip	= 3,
	dmTriangles	= 4,
	dmTriangleStrip	= 5,
	dmTriangleFan	= 6,
	dmQuads	= 7,
	dmQuadStrip	= 8,
	dmPolygon	= 9
    } 	tkDrawMode;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("C2095580-06B9-41f5-B06E-908B6FC0C8A3") 
enum tkPointType
    {	ptSquare	= 0,
	ptCircle	= 1,
	ptDiamond	= 2,
	ptTriangleUp	= 3,
	ptTriangleDown	= 4,
	ptTriangleLeft	= 5,
	ptTriangleRight	= 6,
	ptUserDefined	= 7,
	ptImageList	= 8,
	ptFontChar	= 9
    } 	tkPointType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("5C462DAA-5CC2-4b5c-9D5A-8BA1EC1774B7") 
enum tkLockMode
    {	lmUnlock	= 0,
	lmLock	= 1
    } 	tkLockMode;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("6EE497FB-B03B-4bba-914F-C05199BE0F0D") 
enum tkHJustification
    {	hjLeft	= 0,
	hjCenter	= 1,
	hjRight	= 2,
	hjNone	= 3,
	hjRaw	= 4
    } 	tkHJustification;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("B4EA9A5D-C2DB-4da9-AE5B-A70E57C66C5C") 
enum SplitMethod
    {	InscribedRadius	= 0,
	AngleDeviation	= 1
    } 	SplitMethod;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("6F09E672-EA03-47dc-BC25-4A165DACC148") 
enum PolygonOperation
    {	DIFFERENCE_OPERATION	= 0,
	INTERSECTION_OPERATION	= 1,
	EXCLUSIVEOR_OPERATION	= 2,
	UNION_OPERATION	= 3
    } 	PolygonOperation;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("5FDEB35E-865A-445a-A499-0BED8218A521") 
enum ColoringType
    {	Hillshade	= 0,
	Gradient	= 1,
	Random	= 2
    } 	ColoringType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("35E7AF86-3942-4f7c-8164-D11942522AC3") 
enum GradientModel
    {	Logorithmic	= 0,
	Linear	= 1,
	Exponential	= 2
    } 	GradientModel;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("DB5BD81D-8DC0-401b-A78B-8738F53F4810") 
enum PredefinedColorScheme
    {	FallLeaves	= 0,
	SummerMountains	= 1,
	Desert	= 2,
	Glaciers	= 3,
	Meadow	= 4,
	ValleyFires	= 5,
	DeadSea	= 6,
	Highway1	= 7
    } 	PredefinedColorScheme;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("3E52C14E-3F39-4286-B630-AF8988A8BDD2") 
enum tkSpatialRelation
    {	srContains	= 0,
	srCrosses	= 1,
	srDisjoint	= 2,
	srEquals	= 3,
	srIntersects	= 4,
	srOverlaps	= 5,
	srTouches	= 6,
	srWithin	= 7
    } 	tkSpatialRelation;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("85C1F392-6405-4b9a-82F4-43D4D54E4264") 
enum tkClipOperation
    {	clDifference	= 0,
	clIntersection	= 1,
	clSymDifference	= 2,
	clUnion	= 3,
	clClip	= 4
    } 	tkClipOperation;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("f3f936dd-eb4a-4ec4-a30c-4ec91a83c99b") 
enum tkShapeDrawingMethod
    {	dmStandard	= 0,
	dmNewWithSelection	= 1,
	dmNewWithLabels	= 2,
	dmNewSymbology	= 3
    } 	tkShapeDrawingMethod;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("54594EDE-BAB4-43d8-AD06-462900348496") 
enum tkLabelAlignment
    {	laTopLeft	= 0,
	laTopCenter	= 1,
	laTopRight	= 2,
	laCenterLeft	= 3,
	laCenter	= 4,
	laCenterRight	= 5,
	laBottomLeft	= 6,
	laBottomCenter	= 7,
	laBottomRight	= 8
    } 	tkLabelAlignment;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("7A778F18-9CD1-45ae-ABFC-92B8E6C1579A") 
enum tkLabelPositioning
    {	lpCenter	= 0,
	lpCentroid	= 1,
	lpInteriorPoint	= 2,
	lpFirstSegment	= 3,
	lpLastSegment	= 4,
	lpMiddleSegment	= 5,
	lpLongestSegement	= 6,
	lpNone	= 7
    } 	tkLabelPositioning;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("0B972D4B-3D6F-4f92-BCF8-ECD424310E26") 
enum tkVerticalPosition
    {	vpAboveParentLayer	= 0,
	vpAboveAllLayers	= 1
    } 	tkVerticalPosition;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("343CAE84-D677-4bc6-A7F4-4C7E5096D776") 
enum tkClassificationType
    {	ctNaturalBreaks	= 0,
	ctUniqueValues	= 1,
	ctEqualIntervals	= 2,
	ctEqualCount	= 3,
	ctStandardDeviation	= 4,
	ctEqualSumOfValues	= 5
    } 	tkClassificationType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("8ED72532-E5F9-4424-AA90-BF0904086689") 
enum tkColorSchemeType
    {	ctSchemeRandom	= 0,
	ctSchemeGraduated	= 1
    } 	tkColorSchemeType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("83FFE275-E602-4832-B44E-241EBA27917D") 
enum tkLineLabelOrientation
    {	lorHorizontal	= 0,
	lorParallel	= 1,
	lorPerpindicular	= 2
    } 	tkLineLabelOrientation;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("C1A63978-A342-4d37-8E5F-93B3D1F2F582") 
enum tkLabelFrameType
    {	lfRectangle	= 0,
	lfRoundedRectangle	= 1,
	lfPointedRectangle	= 2
    } 	tkLabelFrameType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("57D081F9-12F3-47b6-9336-A42DBFDFA847") 
enum tkUnitsOfMeasure
    {	umDecimalDegrees	= 0,
	umMiliMeters	= 1,
	umCentimeters	= 2,
	umInches	= 3,
	umFeets	= 4,
	umYards	= 5,
	umMeters	= 6,
	umMiles	= 7,
	umKilometers	= 8
    } 	tkUnitsOfMeasure;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("15E8581C-003E-4f7a-80E4-89BE29BD91A8") 
enum tkLabelElements
    {	leFont	= 0,
	leFontOutline	= 1,
	leShadow	= 2,
	leHalo	= 3,
	leFrameBackground	= 4,
	leFrameOutline	= 5,
	leDefault	= 6
    } 	tkLabelElements;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("C914C27A-A74E-4831-AF78-6DB9E7696111") 
enum tkShapeElements
    {	shElementDefault	= 0,
	shElementFill	= 1,
	shElementFill2	= 2,
	shElementLines	= 3,
	shElementFillBackground	= 4
    } 	tkShapeElements;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("D54E7336-4AEC-4ff1-8681-566C61B04DD8") 
enum tkLinearGradientMode
    {	gmHorizontal	= 0,
	gmVertical	= 1,
	gmForwardDiagonal	= 2,
	gmBackwardDiagonal	= 3,
	gmNone	= 4
    } 	tkLinearGradientMode;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("5DC0755F-EDA3-40cb-AE03-1AB5D3197623") 
enum tkInterpolationMode
    {	imBilinear	= 3,
	imBicubic	= 4,
	imNone	= 5,
	imHighQualityBilinear	= 6,
	imHighQualityBicubic	= 7
    } 	tkInterpolationMode;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("3BDEA45D-F8D9-4dca-A58B-9BB7F689263A") 
enum tkGDALResamplingMethod
    {	grmNone	= 0,
	grmNearest	= 1,
	grmGauss	= 2,
	grmBicubic	= 3,
	grmAverage	= 4
    } 	tkGDALResamplingMethod;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("63FBAB4B-A262-466a-AA4C-52F0E13817B4") 
enum tkResamplingType
    {	rtNone	= 0,
	rtLinear	= 1,
	rtCubic	= 2,
	rtLanczos	= 3
    } 	tkResamplingType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("85E2F305-667D-4868-9B2D-A5E9AD35C6D8") 
enum tkGradientType
    {	gtLinear	= 0,
	gtRectangular	= 1,
	gtCircle	= 2
    } 	tkGradientType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("BE7EA6B6-8667-4e87-BA91-8131314762B1") 
enum tkGDIPlusHatchStyle
    {	hsNone	= -1,
	hsHorizontal	= 0,
	hsVertical	= 1,
	hsForwardDiagonal	= 2,
	hsBackwardDiagonal	= 3,
	hsCross	= 4,
	hsDiagonalCross	= 5,
	hsPercent05	= 6,
	hsPercent10	= 7,
	hsPercent20	= 8,
	hsPercent25	= 9,
	hsPercent30	= 10,
	hsPercent40	= 11,
	hsPercent50	= 12,
	hsPercent60	= 13,
	hsPercent70	= 14,
	hsPercent75	= 15,
	hsPercent80	= 16,
	hsPercent90	= 17,
	hsLightDownwardDiagonal	= 18,
	hsLightUpwardDiagonal	= 19,
	hsDarkDownwardDiagonal	= 20,
	hsDarkUpwardDiagonal	= 21,
	hsWideDownwardDiagonal	= 22,
	hsWideUpwardDiagonal	= 23,
	hsLightVertical	= 24,
	hsLightHorizontal	= 25,
	hsNarrowVertical	= 26,
	hsNarrowHorizontal	= 27,
	hsDarkVertical	= 28,
	hsDarkHorizontal	= 29,
	hsDashedDownwardDiagonal	= 30,
	hsDashedUpwardDiagonal	= 31,
	hsDashedHorizontal	= 32,
	hsDashedVertical	= 33,
	hsSmallConfetti	= 34,
	hsLargeConfetti	= 35,
	hsZigZag	= 36,
	hsWave	= 37,
	hsDiagonalBrick	= 38,
	hsHorizontalBrick	= 39,
	hsWeave	= 40,
	hsPlaid	= 41,
	hsDivot	= 42,
	hsDottedGrid	= 43,
	hsDottedDiamond	= 44,
	hsShingle	= 45,
	hsTrellis	= 46,
	hsSphere	= 47,
	hsSmallGrid	= 48,
	hsSmallCheckerBoard	= 49,
	hsLargeCheckerBoard	= 50,
	hsOutlinedDiamond	= 51,
	hsSolidDiamond	= 52
    } 	tkGDIPlusHatchStyle;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("84E461DF-53D1-4c36-AF85-D70F2214424F") 
enum tkPointSymbolType
    {	ptSymbolStandard	= 0,
	ptSymbolFontCharacter	= 1,
	ptSymbolPicture	= 2
    } 	tkPointSymbolType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("D7BBAD28-38EE-45d1-B4E8-BF4C292EF522") 
enum tkFillType
    {	ftStandard	= 0,
	ftHatch	= 1,
	ftGradient	= 2,
	ftPicture	= 3
    } 	tkFillType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("15CCFD69-B193-493e-AC04-8D676D8B4ECF") 
enum tkPointShapeType
    {	ptShapeRegular	= 0,
	ptShapeCross	= 1,
	ptShapeStar	= 2,
	ptShapeCircle	= 3,
	ptShapeArrow	= 4,
	ptShapeFlag	= 5
    } 	tkPointShapeType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("64FA8C3E-F0DE-4674-87D5-614C14B310F9") 
enum tkDefaultPointSymbol
    {	dpsSquare	= 0,
	dpsCircle	= 1,
	dpsDiamond	= 2,
	dpsTriangleUp	= 3,
	dpsTriangleDown	= 4,
	dpsTriangleLeft	= 5,
	dpsTriangleRight	= 6,
	dpsCross	= 7,
	dpsXCross	= 8,
	dpsStar	= 9,
	dpsPentagon	= 10,
	dpsArrowUp	= 11,
	dpsArrowDown	= 12,
	dpsArrowLeft	= 13,
	dpsArrowRight	= 14,
	dpsAsterisk	= 15,
	dpsFlag	= 16
    } 	tkDefaultPointSymbol;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("FB3763C2-1D27-419f-A7CF-5A7B350E00E1") 
enum tkGradientBounds
    {	gbWholeLayer	= 0,
	gbPerShape	= 1
    } 	tkGradientBounds;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("EDD7EA9B-EABB-4efb-A621-BF2C9265F1C6") 
enum tkVectorDrawingMode
    {	vdmGDI	= 0,
	vdmGDIMixed	= 1,
	vdmGDIPlus	= 2
    } 	tkVectorDrawingMode;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("EBBC71F6-D747-485f-AEA0-51BF87432F15") 
enum tkChartType
    {	chtBarChart	= 0,
	chtPieChart	= 1
    } 	tkChartType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("1C045CDE-4913-47db-9001-81AF2EB35910") 
enum tkGeometryEngine
    {	engineGeos	= 0,
	engineClipper	= 1
    } 	tkGeometryEngine;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("9E717D9C-AEF5-4124-8339-AF9B46D5370A") 
enum tkSelectionAppearance
    {	saSelectionColor	= 0,
	saDrawingOptions	= 1
    } 	tkSelectionAppearance;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("EF5AFCDA-91D5-4b81-A13C-0D3EB24E4E97") 
enum tkCollisionMode
    {	AllowCollisions	= 0,
	LocalList	= 1,
	GlobalList	= 2
    } 	tkCollisionMode;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("CD43915D-38BB-410e-A24F-3C1967617151") 
enum tkTextRenderingHint
    {	SystemDefault	= 0,
	SingleBitPerPixelGridFit	= ( SystemDefault + 1 ) ,
	SingleBitPerPixel	= ( SingleBitPerPixelGridFit + 1 ) ,
	AntiAliasGridFit	= ( SingleBitPerPixel + 1 ) ,
	HintAntiAlias	= ( AntiAliasGridFit + 1 ) ,
	ClearTypeGridFit	= ( HintAntiAlias + 1 ) 
    } 	tkTextRenderingHint;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("BC397A60-966F-45d6-B65C-7BC59F2DFFA1") 
enum tkSmoothingMode
    {	DefaultMode	= 0,
	HighSpeedMode	= ( DefaultMode + 1 ) ,
	HighQualityMode	= ( HighSpeedMode + 1 ) ,
	None	= ( HighQualityMode + 1 ) ,
	AntiAlias	= ( None + 1 ) 
    } 	tkSmoothingMode;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("86786053-9C59-41fb-825A-5D0382603C4B") 
enum tkCompositingQuality
    {	Default	= 0,
	HighSpeed	= ( Default + 1 ) ,
	HighQuality	= ( HighSpeed + 1 ) ,
	GammaCorrected	= ( HighQuality + 1 ) ,
	AssumeLinear	= ( GammaCorrected + 1 ) 
    } 	tkCompositingQuality;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("DB604729-D315-4af3-AD35-9277EE06D1C1") 
enum tkMapColor
    {	AliceBlue	= 0xfff0f8ff,
	AntiqueWhite	= 0xfffaebd7,
	Aqua	= 0xff00ffff,
	Aquamarine	= 0xff7fffd4,
	Azure	= 0xfff0ffff,
	Beige	= 0xfff5f5dc,
	Bisque	= 0xffffe4c4,
	Black	= 0xff000000,
	BlanchedAlmond	= 0xffffebcd,
	Blue	= 0xff0000ff,
	BlueViolet	= 0xff8a2be2,
	Brown	= 0xffa52a2a,
	BurlyWood	= 0xffdeb887,
	CadetBlue	= 0xff5f9ea0,
	Chartreuse	= 0xff7fff00,
	Chocolate	= 0xffd2691e,
	Coral	= 0xffff7f50,
	CornflowerBlue	= 0xff6495ed,
	Cornsilk	= 0xfffff8dc,
	Crimson	= 0xffdc143c,
	Cyan	= 0xff00ffff,
	DarkBlue	= 0xff00008b,
	DarkCyan	= 0xff008b8b,
	DarkGoldenrod	= 0xffb8860b,
	DarkGray	= 0xffa9a9a9,
	DarkGreen	= 0xff006400,
	DarkKhaki	= 0xffbdb76b,
	DarkMagenta	= 0xff8b008b,
	DarkOliveGreen	= 0xff556b2f,
	DarkOrange	= 0xffff8c00,
	DarkOrchid	= 0xff9932cc,
	DarkRed	= 0xff8b0000,
	DarkSalmon	= 0xffe9967a,
	DarkSeaGreen	= 0xff8fbc8b,
	DarkSlateBlue	= 0xff483d8b,
	DarkSlateGray	= 0xff2f4f4f,
	DarkTurquoise	= 0xff00ced1,
	DarkViolet	= 0xff9400d3,
	DeepPink	= 0xffff1493,
	DeepSkyBlue	= 0xff00bfff,
	DimGray	= 0xff696969,
	DodgerBlue	= 0xff1e90ff,
	Firebrick	= 0xffb22222,
	FloralWhite	= 0xfffffaf0,
	ForestGreen	= 0xff228b22,
	Fuchsia	= 0xffff00ff,
	Gainsboro	= 0xffdcdcdc,
	GhostWhite	= 0xfff8f8ff,
	Gold	= 0xffffd700,
	Goldenrod	= 0xffdaa520,
	Gray	= 0xff808080,
	Green	= 0xff008000,
	GreenYellow	= 0xffadff2f,
	Honeydew	= 0xfff0fff0,
	HotPink	= 0xffff69b4,
	IndianRed	= 0xffcd5c5c,
	Indigo	= 0xff4b0082,
	Ivory	= 0xfffffff0,
	Khaki	= 0xfff0e68c,
	Lavender	= 0xffe6e6fa,
	LavenderBlush	= 0xfffff0f5,
	LawnGreen	= 0xff7cfc00,
	LemonChiffon	= 0xfffffacd,
	LightBlue	= 0xffadd8e6,
	LightCoral	= 0xfff08080,
	LightCyan	= 0xffe0ffff,
	LightGoldenrodYellow	= 0xfffafad2,
	LightGray	= 0xffd3d3d3,
	LightGreen	= 0xff90ee90,
	LightPink	= 0xffffb6c1,
	LightSalmon	= 0xffffa07a,
	LightSeaGreen	= 0xff20b2aa,
	LightSkyBlue	= 0xff87cefa,
	LightSlateGray	= 0xff778899,
	LightSteelBlue	= 0xffb0c4de,
	LightYellow	= 0xffffffe0,
	Lime	= 0xff00ff00,
	LimeGreen	= 0xff32cd32,
	Linen	= 0xfffaf0e6,
	Magenta	= 0xffff00ff,
	Maroon	= 0xff800000,
	MediumAquamarine	= 0xff66cdaa,
	MediumBlue	= 0xff0000cd,
	MediumOrchid	= 0xffba55d3,
	MediumPurple	= 0xff9370db,
	MediumSeaGreen	= 0xff3cb371,
	MediumSlateBlue	= 0xff7b68ee,
	MediumSpringGreen	= 0xff00fa9a,
	MediumTurquoise	= 0xff48d1cc,
	MediumVioletRed	= 0xffc71585,
	MidnightBlue	= 0xff191970,
	MintCream	= 0xfff5fffa,
	MistyRose	= 0xffffe4e1,
	Moccasin	= 0xffffe4b5,
	NavajoWhite	= 0xffffdead,
	Navy	= 0xff000080,
	OldLace	= 0xfffdf5e6,
	Olive	= 0xff808000,
	OliveDrab	= 0xff6b8e23,
	Orange	= 0xffffa500,
	OrangeRed	= 0xffff4500,
	Orchid	= 0xffda70d6,
	PaleGoldenrod	= 0xffeee8aa,
	PaleGreen	= 0xff98fb98,
	PaleTurquoise	= 0xffafeeee,
	PaleVioletRed	= 0xffdb7093,
	PapayaWhip	= 0xffffefd5,
	PeachPuff	= 0xffffdab9,
	Peru	= 0xffcd853f,
	Pink	= 0xffffc0cb,
	Plum	= 0xffdda0dd,
	PowderBlue	= 0xffb0e0e6,
	Purple	= 0xff800080,
	Red	= 0xffff0000,
	RosyBrown	= 0xffbc8f8f,
	RoyalBlue	= 0xff4169e1,
	SaddleBrown	= 0xff8b4513,
	Salmon	= 0xfffa8072,
	SandyBrown	= 0xfff4a460,
	SeaGreen	= 0xff2e8b57,
	SeaShell	= 0xfffff5ee,
	Sienna	= 0xffa0522d,
	Silver	= 0xffc0c0c0,
	SkyBlue	= 0xff87ceeb,
	SlateBlue	= 0xff6a5acd,
	SlateGray	= 0xff708090,
	Snow	= 0xfffffafa,
	SpringGreen	= 0xff00ff7f,
	SteelBlue	= 0xff4682b4,
	Tan	= 0xffd2b48c,
	Teal	= 0xff008080,
	Thistle	= 0xffd8bfd8,
	Tomato	= 0xffff6347,
	Transparent	= 0xffffff,
	Turquoise	= 0xff40e0d0,
	Violet	= 0xffee82ee,
	Wheat	= 0xfff5deb3,
	White	= 0xffffffff,
	WhiteSmoke	= 0xfff5f5f5,
	Yellow	= 0xffffff00,
	YellowGreen	= 0xff9acd32
    } 	tkMapColor;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("79F928D4-6E5A-4312-A519-F6AB83691086") 
enum tkDashStyle
    {	dsSolid	= 0,
	dsDash	= 1,
	dsDot	= 2,
	dsDashDot	= 3,
	dsDashDotDot	= 4,
	dsCustom	= 5
    } 	tkDashStyle;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("119B042B-9EC9-45e4-8B83-526079198279") 
enum tkVertexType
    {	vtSquare	= 0,
	vtCircle	= 1
    } 	tkVertexType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("F4B17059-5214-477a-99ED-7D30C9722941") 
enum tkLineType
    {	lltSimple	= 0,
	lltMarker	= 1
    } 	tkLineType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("C5C7A7BF-17D4-4953-9CBC-4068B0FC59E6") 
enum tkChartValuesStyle
    {	vsHorizontal	= 0,
	vsVertical	= 1
    } 	tkChartValuesStyle;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("0536E234-CBD1-4975-9B2B-2EC1CD37F84A") 
enum tkShapefileSourceType
    {	sstUninitialized	= 0,
	sstDiskBased	= 1,
	sstInMemory	= 2
    } 	tkShapefileSourceType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("2EA95843-840A-457d-8D9A-CFFD731B89D0") 
enum tkImageSourceType
    {	istUninitialized	= 0,
	istDiskBased	= 1,
	istInMemory	= 2,
	istGDALBased	= 3,
	istGDIPlus	= 4
    } 	tkImageSourceType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("E214CC65-00DC-4511-8E9E-C262433C1AAD") 
enum tkSavingMode
    {	modeNone	= 0,
	modeStandard	= 1,
	modeXML	= 2,
	modeDBF	= 3,
	modeXMLOverwrite	= 4
    } 	tkSavingMode;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("6470A5C5-E726-40e3-8C7B-E5C1E11E8A48") 
enum tkProjectionParameter
    {	LatitudeOfOrigin	= 0,
	CentralMeridian	= 1,
	ScaleFactor	= 2,
	FalseEasting	= 3,
	FalseNorthing	= 4,
	LongitudeOfOrigin	= 5
    } 	tkProjectionParameter;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("F80983AA-F448-418b-A791-5B64795E57D2") 
enum tkGeogCSParameter
    {	SemiMajor	= 0,
	SemiMinor	= 1,
	InverseFlattening	= 2,
	PrimeMeridian	= 3,
	AngularUnit	= 4
    } 	tkGeogCSParameter;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("AF13E014-9E40-4ee2-9641-3BA4BE865DBA") 
enum tkGroupOperation
    {	operMin	= 0,
	operMax	= 1,
	operCount	= 2,
	operSum	= 3,
	operAverage	= 4
    } 	tkGroupOperation;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("CDF57781-4FE1-46ed-AC51-59CD3C89B4C8") 
enum ShpfileType
    {	SHP_NULLSHAPE	= 0,
	SHP_POINT	= 1,
	SHP_POLYLINE	= 3,
	SHP_POLYGON	= 5,
	SHP_MULTIPOINT	= 8,
	SHP_POINTZ	= 11,
	SHP_POLYLINEZ	= 13,
	SHP_POLYGONZ	= 15,
	SHP_MULTIPOINTZ	= 18,
	SHP_POINTM	= 21,
	SHP_POLYLINEM	= 23,
	SHP_POLYGONM	= 25,
	SHP_MULTIPOINTM	= 28,
	SHP_MULTIPATCH	= 31
    } 	ShpfileType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("44E55993-60B9-4f67-9500-073A3FCA2249") 
enum SelectMode
    {	INTERSECTION	= 0,
	INCLUSION	= 1
    } 	SelectMode;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("485EEBC8-5F16-48bc-BD18-DBBDA0CA6E4A") 
enum ImageType
    {	BITMAP_FILE	= 0,
	GIF_FILE	= 1,
	USE_FILE_EXTENSION	= 2,
	TIFF_FILE	= 3,
	JPEG_FILE	= 4,
	PNG_FILE	= 5,
	PPM_FILE	= 7,
	ECW_FILE	= 8,
	JPEG2000_FILE	= 9,
	SID_FILE	= 10,
	PNM_FILE	= 11,
	PGM_FILE	= 12,
	BIL_FILE	= 13,
	ADF_FILE	= 14,
	GRD_FILE	= 15,
	IMG_FILE	= 16,
	ASC_FILE	= 17,
	BT_FILE	= 18,
	MAP_FILE	= 19,
	LF2_FILE	= 20,
	KAP_FILE	= 21,
	DEM_FILE	= 22
    } 	ImageType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("5AD363AD-E860-4789-87E8-F3100AF3707D") 
enum FieldType
    {	STRING_FIELD	= 0,
	INTEGER_FIELD	= 1,
	DOUBLE_FIELD	= 2
    } 	FieldType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("F02C004B-FD7D-4ace-B672-BDB8A41632BC") 
enum GridDataType
    {	ShortDataType	= 0,
	LongDataType	= 1,
	FloatDataType	= 2,
	DoubleDataType	= 3,
	InvalidDataType	= -1,
	UnknownDataType	= 4,
	ByteDataType	= 5
    } 	GridDataType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("50814193-87DC-45f0-9682-F64C5B153AAC") 
enum GridFileType
    {	Ascii	= 0,
	Binary	= 1,
	Esri	= 2,
	GeoTiff	= 3,
	Sdts	= 4,
	PAux	= 5,
	PCIDsk	= 6,
	DTed	= 7,
	Bil	= 8,
	Ecw	= 9,
	MrSid	= 10,
	Flt	= 11,
	Dem	= 12,
	UseExtension	= 13,
	InvalidGridFileType	= -1
    } 	GridFileType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("05D8AC58-5435-4957-B94B-8DF7155D5F98") 
enum AmbiguityResolution
    {	Z_VALUE	= 0,
	DISTANCE_TO_OUTLET	= 1,
	NO_RESOLUTION	= 2
    } 	AmbiguityResolution;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("6DC3F142-CFCB-40d6-99D0-EE197334360C") 
enum tkValueType
    {	vtDouble	= 0,
	vtString	= 1,
	vtBoolean	= 2
    } 	tkValueType;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("76BCEF79-8841-440c-BA0B-B8C8B236935E") 
enum tkCoordinateSystem
    {	csAbidjan_1987	= 4143,
	csAccra	= 4168,
	csAdindan	= 4201,
	csAfgooye	= 4205,
	csAgadez	= 4206,
	csAGD66	= 4202,
	csAGD84	= 4203,
	csAin_el_Abd	= 4204,
	csAlbanian_1987	= 4191,
	csAmerican_Samoa_1962	= 4169,
	csAmersfoort	= 4289,
	csAmmassalik_1958	= 4196,
	csAnguilla_1957	= 4600,
	csAntigua_1943	= 4601,
	csAratu	= 4208,
	csArc_1950	= 4209,
	csArc_1960	= 4210,
	csAscension_Island_1958	= 4712,
	csATF_Paris	= 4901,
	csATS77	= 4122,
	csAustralian_Antarctic	= 4176,
	csAyabelle_Lighthouse	= 4713,
	csAzores_Central_1948	= 4183,
	csAzores_Central_1995	= 4665,
	csAzores_Occidental_1939	= 4182,
	csAzores_Oriental_1940	= 4184,
	csAzores_Oriental_1995	= 4664,
	csBarbados_1938	= 4212,
	csBatavia	= 4211,
	csBatavia_Jakarta	= 4813,
	csBDA2000	= 4762,
	csBeduaram	= 4213,
	csBeijing_1954	= 4214,
	csBelge_1950	= 4215,
	csBelge_1950_Brussels	= 4809,
	csBelge_1972	= 4313,
	csBellevue	= 4714,
	csBermuda_1957	= 4216,
	csBern_1898_Bern	= 4801,
	csBern_1938	= 4306,
	csBissau	= 4165,
	csBogota_1975	= 4218,
	csBogota_1975_Bogota	= 4802,
	csBukit_Rimpah	= 4219,
	csCadastre_1997	= 4475,
	csCamacupa	= 4220,
	csCamp_Area_Astro	= 4715,
	csCampo_Inchauspe	= 4221,
	csCape	= 4222,
	csCape_Canaveral	= 4717,
	csCarthage	= 4223,
	csCarthage_Paris	= 4816,
	csCH1903	= 4149,
	csCH1903_plus	= 4150,
	csChatham_Islands_1971	= 4672,
	csChatham_Islands_1979	= 4673,
	csChina_Geodetic_Coordinate_System_2000	= 4490,
	csChos_Malal_1914	= 4160,
	csCHTRF95	= 4151,
	csChua	= 4224,
	csCocos_Islands_1965	= 4708,
	csCombani_1950	= 4632,
	csConakry_1905	= 4315,
	csCorrego_Alegre	= 4225,
	csCSG67	= 4623,
	csDabola_1981	= 4155,
	csDatum_73	= 4274,
	csDealul_Piscului_1930	= 4316,
	csDeception_Island	= 4736,
	csDeir_ez_Zor	= 4227,
	csDGN95	= 4755,
	csDHDN	= 4314,
	csDiego_Garcia_1969	= 4724,
	csDominica_1945	= 4602,
	csDouala_1948	= 4192,
	csDRUKREF_03	= 5264,
	csEaster_Island_1967	= 4719,
	csED50	= 4230,
	csED50_ED77	= 4154,
	csED79	= 4668,
	csED87	= 4231,
	csEgypt_1907	= 4229,
	csEgypt_1930	= 4199,
	csEgypt_Gulf_of_Suez_S_650_TL	= 4706,
	csELD79	= 4159,
	csEST92	= 4133,
	csEST97	= 4180,
	csETRS89	= 4258,
	csFahud	= 4232,
	csFatu_Iva_72	= 4688,
	csFD54	= 4741,
	csFD58	= 4132,
	csFiji_1956	= 4721,
	csFiji_1986	= 4720,
	csfk89	= 4753,
	csFort_Marigot	= 4621,
	csGan_1970	= 4684,
	csGaroua	= 4197,
	csGDA94	= 4283,
	csGDBD2009	= 5246,
	csGDM2000	= 4742,
	csGGRS87	= 4121,
	csGR96	= 4747,
	csGrand_Cayman_1959	= 4723,
	csGrand_Comoros	= 4646,
	csGreek	= 4120,
	csGreek_Athens	= 4815,
	csGrenada_1953	= 4603,
	csGuadeloupe_1948	= 4622,
	csGuam_1963	= 4675,
	csGulshan_303	= 4682,
	csHanoi_1972	= 4147,
	csHartebeesthoek94	= 4148,
	csHD1909	= 3819,
	csHD72	= 4237,
	csHelle_1954	= 4660,
	csHerat_North	= 4255,
	csHito_XVIII_1963	= 4254,
	csHjorsey_1955	= 4658,
	csHong_Kong_1963	= 4738,
	csHong_Kong_1963_67	= 4739,
	csHong_Kong_1980	= 4611,
	csHTRS96	= 4761,
	csHu_Tzu_Shan_1950	= 4236,
	csID74	= 4238,
	csIGC_1962_6th_Parallel_South	= 4697,
	csIGCB_1955	= 4701,
	csIGM95	= 4670,
	csIGN_1962_Kerguelen	= 4698,
	csIGN_Astro_1960	= 4700,
	csIGN53_Mare	= 4641,
	csIGN56_Lifou	= 4633,
	csIGN63_Hiva_Oa	= 4689,
	csIGN72_Grande_Terre	= 4662,
	csIGN72_Nuku_Hiva	= 4630,
	csIGRS	= 3889,
	csIKBD_92	= 4667,
	csIndian_1954	= 4239,
	csIndian_1960	= 4131,
	csIndian_1975	= 4240,
	csIRENET95	= 4173,
	csISN93	= 4659,
	csIsrael	= 4141,
	csIwo_Jima_1945	= 4709,
	csJAD2001	= 4758,
	csJAD69	= 4242,
	csJamaica_1875	= 4241,
	csJGD2000	= 4612,
	csJohnston_Island_1961	= 4725,
	csJouik_1961	= 4679,
	csKalianpur_1880	= 4243,
	csKalianpur_1937	= 4144,
	csKalianpur_1962	= 4145,
	csKalianpur_1975	= 4146,
	csKandawala	= 4244,
	csKarbala_1979	= 4743,
	csKasai_1953	= 4696,
	csKatanga_1955	= 4695,
	csKertau_RSO	= 4751,
	csKertau_1968	= 4245,
	csKKJ	= 4123,
	csKOC	= 4246,
	csKorea_2000	= 4737,
	csKorean_1985	= 4162,
	csKorean_1995	= 4166,
	csKousseri	= 4198,
	csKUDAMS	= 4319,
	csKusaie_1951	= 4735,
	csLa_Canoa	= 4247,
	csLake	= 4249,
	csLao_1993	= 4677,
	csLao_1997	= 4678,
	csLe_Pouce_1934	= 4699,
	csLeigon	= 4250,
	csLGD2006	= 4754,
	csLiberia_1964	= 4251,
	csLisbon	= 4207,
	csLisbon_Lisbon	= 4803,
	csLisbon_1890	= 4666,
	csLisbon_1890_Lisbon	= 4904,
	csLittle_Cayman_1961	= 4726,
	csLKS92	= 4661,
	csLKS94	= 4669,
	csLocodjo_1965	= 4142,
	csLoma_Quintana	= 4288,
	csLome	= 4252,
	csLuxembourg_1930	= 4181,
	csLuzon_1911	= 4253,
	csMadrid_1870_Madrid	= 4903,
	csMadzansua	= 4128,
	csMAGNA_SIRGAS	= 4686,
	csMahe_1971	= 4256,
	csMakassar	= 4257,
	csMakassar_Jakarta	= 4804,
	csMalongo_1987	= 4259,
	csManoca_1962	= 4193,
	csMarcus_Island_1952	= 4711,
	csMarshall_Islands_1960	= 4732,
	csMartinique_1938	= 4625,
	csMassawa	= 4262,
	csMaupiti_83	= 4692,
	csMauritania_1999	= 4702,
	csMerchich	= 4261,
	csMexican_Datum_of_1993	= 4483,
	csMGI	= 4312,
	csMGI_Ferro	= 4805,
	csMGI_1901	= 3906,
	csMhast_offshore	= 4705,
	csMhast_onshore	= 4704,
	csMhast_1951	= 4703,
	csMidway_1961	= 4727,
	csMinna	= 4263,
	csMOLDREF99	= 4023,
	csMonte_Mario	= 4265,
	csMonte_Mario_Rome	= 4806,
	csMontserrat_1958	= 4604,
	csMoorea_87	= 4691,
	csMOP78	= 4639,
	csMount_Dillon	= 4157,
	csMoznet	= 4130,
	csMporaloko	= 4266,
	csNAD27	= 4267,
	csNAD27_Michigan	= 4268,
	csNAD27_76	= 4608,
	csNAD27_CGQ77	= 4609,
	csNAD83	= 4269,
	csNAD83_CSRS	= 4617,
	csNAD83_HARN	= 4152,
	csNAD83_NSRS2007	= 4759,
	csNahrwan_1934	= 4744,
	csNahrwan_1967	= 4270,
	csNakhl_e_Ghanem	= 4693,
	csNaparima_1955	= 4158,
	csNaparima_1972	= 4271,
	csNEA74_Noumea	= 4644,
	csNew_Beijing	= 4555,
	csNGN	= 4318,
	csNGO_1948	= 4273,
	csNGO_1948_Oslo	= 4817,
	csNord_Sahara_1959	= 4307,
	csNouakchott_1965	= 4680,
	csNSWC_9Z_2	= 4276,
	csNTF	= 4275,
	csNTF_Paris	= 4807,
	csNZGD2000	= 4167,
	csNZGD49	= 4272,
	csObservatario	= 4129,
	csOld_Hawaiian	= 4135,
	csOS_SN80	= 4279,
	csOSGB_1936	= 4277,
	csOSGB70	= 4278,
	csOSNI_1952	= 4188,
	csPadang	= 4280,
	csPadang_Jakarta	= 4808,
	csPalestine_1923	= 4281,
	csPampa_del_Castillo	= 4161,
	csPD_83	= 4746,
	csPerroud_1950	= 4637,
	csPetrels_1972	= 4636,
	csPhoenix_Islands_1966	= 4716,
	csPico_de_las_Nieves_1984	= 4728,
	csPitcairn_1967	= 4729,
	csPitcairn_2006	= 4763,
	csPoint_58	= 4620,
	csPointe_Noire	= 4282,
	csPorto_Santo	= 4615,
	csPorto_Santo_1995	= 4663,
	csPOSGAR_94	= 4694,
	csPOSGAR_98	= 4190,
	csPrincipe	= 4824,
	csPRS92	= 4683,
	csPSAD56	= 4248,
	csPSD93	= 4134,
	csPTRA08	= 5013,
	csPuerto_Rico	= 4139,
	csPulkovo_1942	= 4284,
	csPulkovo_1942_58	= 4179,
	csPulkovo_1942_83	= 4178,
	csPulkovo_1995	= 4200,
	csPZ_90	= 4740,
	csQatar_1948	= 4286,
	csQatar_1974	= 4285,
	csQND95	= 4614,
	csQornoq_1927	= 4194,
	csRassadiran	= 4153,
	csRD_83	= 4745,
	csREGCAN95	= 4081,
	csREGVEN	= 4189,
	csReunion_1947	= 4626,
	csReykjavik_1900	= 4657,
	csRGF93	= 4171,
	csRGFG95	= 4624,
	csRGM04	= 4470,
	csRGNC91_93	= 4749,
	csRGPF	= 4687,
	csRGR92	= 4627,
	csRGRDC_2005	= 4046,
	csRGSPM06	= 4463,
	csRRAF_1991	= 4558,
	csRSRGD2000	= 4764,
	csRT38	= 4308,
	csRT38_Stockholm	= 4814,
	csRT90	= 4124,
	csSAD69	= 4618,
	csSaint_Pierre_et_Miquelon_1950	= 4638,
	csSanto_1965	= 4730,
	csSao_Tome	= 4823,
	csSapper_Hill_1943	= 4292,
	csSchwarzeck	= 4293,
	csScoresbysund_1952	= 4195,
	csSegara	= 4613,
	csSegara_Jakarta	= 4820,
	csSelvagem_Grande	= 4616,
	csSerindung	= 4295,
	csSierra_Leone_1924	= 4174,
	csSierra_Leone_1968	= 4175,
	csSIRGAS_1995	= 4170,
	csSIRGAS_2000	= 4674,
	csS_JTSK	= 4156,
	csS_JTSK_Ferro	= 4818,
	csS_JTSK_05	= 5228,
	csS_JTSK_05_Ferro	= 5229,
	csSLD99	= 5233,
	csSlovenia_1996	= 4765,
	csSolomon_1968	= 4718,
	csSouth_Georgia_1968	= 4722,
	csSouth_Yemen	= 4164,
	csSREF98	= 4075,
	csSt_George_Island	= 4138,
	csSt_Helena_1971	= 4710,
	csSt_Kitts_1955	= 4605,
	csSt_Lawrence_Island	= 4136,
	csSt_Lucia_1955	= 4606,
	csSt_Paul_Island	= 4137,
	csSt_Vincent_1945	= 4607,
	csST71_Belep	= 4643,
	csST84_Ile_des_Pins	= 4642,
	csST87_Ouvea	= 4750,
	csSVY21	= 4757,
	csSWEREF99	= 4619,
	csTahaa_54	= 4629,
	csTahiti_52	= 4628,
	csTahiti_79	= 4690,
	csTananarive	= 4297,
	csTananarive_Paris	= 4810,
	csTC_1948	= 4303,
	csTern_Island_1961	= 4707,
	csTete	= 4127,
	csTimbalai_1948	= 4298,
	csTM65	= 4299,
	csTM75	= 4300,
	csTokyo	= 4301,
	csTokyo_1892	= 5132,
	csTrinidad_1903	= 4302,
	csTristan_1968	= 4734,
	csTUREF	= 5252,
	csTWD67	= 3821,
	csTWD97	= 3824,
	csVanua_Levu_1915	= 4748,
	csVientiane_1982	= 4676,
	csViti_Levu_1912	= 4752,
	csVN_2000	= 4756,
	csVoirol_1875	= 4304,
	csVoirol_1875_Paris	= 4811,
	csVoirol_1879	= 4671,
	csVoirol_1879_Paris	= 4821,
	csWake_Island_1952	= 4733,
	csWGS_66	= 4760,
	csWGS_72	= 4322,
	csWGS_72BE	= 4324,
	csWGS_84	= 4326,
	csXian_1980	= 4610,
	csYacare	= 4309,
	csYemen_NGN96	= 4163,
	csYoff	= 4310,
	csZanderij	= 4311
    } 	tkCoordinateSystem;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("32617F7E-44F7-4ecd-8228-85924A4DF5B7") 
enum tkNad83Projection
    {	Nad83_Kentucky_North	= 2205,
	Nad83_Arizona_East_ft	= 2222,
	Nad83_Arizona_Central_ft	= 2223,
	Nad83_Arizona_West_ft	= 2224,
	Nad83_California_zone_1_ftUS	= 2225,
	Nad83_California_zone_2_ftUS	= 2226,
	Nad83_California_zone_3_ftUS	= 2227,
	Nad83_California_zone_4_ftUS	= 2228,
	Nad83_California_zone_5_ftUS	= 2229,
	Nad83_California_zone_6_ftUS	= 2230,
	Nad83_Colorado_North_ftUS	= 2231,
	Nad83_Colorado_Central_ftUS	= 2232,
	Nad83_Colorado_South_ftUS	= 2233,
	Nad83_Connecticut_ftUS	= 2234,
	Nad83_Delaware_ftUS	= 2235,
	Nad83_Florida_East_ftUS	= 2236,
	Nad83_Florida_West_ftUS	= 2237,
	Nad83_Florida_North_ftUS	= 2238,
	Nad83_Georgia_East_ftUS	= 2239,
	Nad83_Georgia_West_ftUS	= 2240,
	Nad83_Idaho_East_ftUS	= 2241,
	Nad83_Idaho_Central_ftUS	= 2242,
	Nad83_Idaho_West_ftUS	= 2243,
	Nad83_Kentucky_North_ftUS	= 2246,
	Nad83_Kentucky_South_ftUS	= 2247,
	Nad83_Maryland_ftUS	= 2248,
	Nad83_Massachusetts_Mainland_ftUS	= 2249,
	Nad83_Massachusetts_Island_ftUS	= 2250,
	Nad83_Michigan_North_ft	= 2251,
	Nad83_Michigan_Central_ft	= 2252,
	Nad83_Michigan_South_ft	= 2253,
	Nad83_Mississippi_East_ftUS	= 2254,
	Nad83_Mississippi_West_ftUS	= 2255,
	Nad83_Montana_ft	= 2256,
	Nad83_New_Mexico_East_ftUS	= 2257,
	Nad83_New_Mexico_Central_ftUS	= 2258,
	Nad83_New_Mexico_West_ftUS	= 2259,
	Nad83_New_York_East_ftUS	= 2260,
	Nad83_New_York_Central_ftUS	= 2261,
	Nad83_New_York_West_ftUS	= 2262,
	Nad83_New_York_Long_Island_ftUS	= 2263,
	Nad83_North_Carolina_ftUS	= 2264,
	Nad83_North_Dakota_North_ft	= 2265,
	Nad83_North_Dakota_South_ft	= 2266,
	Nad83_Oklahoma_North_ftUS	= 2267,
	Nad83_Oklahoma_South_ftUS	= 2268,
	Nad83_Oregon_North_ft	= 2269,
	Nad83_Oregon_South_ft	= 2270,
	Nad83_Pennsylvania_North_ftUS	= 2271,
	Nad83_Pennsylvania_South_ftUS	= 2272,
	Nad83_South_Carolina_ft	= 2273,
	Nad83_Tennessee_ftUS	= 2274,
	Nad83_Texas_North_ftUS	= 2275,
	Nad83_Texas_North_Central_ftUS	= 2276,
	Nad83_Texas_Central_ftUS	= 2277,
	Nad83_Texas_South_Central_ftUS	= 2278,
	Nad83_Texas_South_ftUS	= 2279,
	Nad83_Utah_North_ft	= 2280,
	Nad83_Utah_Central_ft	= 2281,
	Nad83_Utah_South_ft	= 2282,
	Nad83_Virginia_North_ftUS	= 2283,
	Nad83_Virginia_South_ftUS	= 2284,
	Nad83_Washington_North_ftUS	= 2285,
	Nad83_Washington_South_ftUS	= 2286,
	Nad83_Wisconsin_North_ftUS	= 2287,
	Nad83_Wisconsin_Central_ftUS	= 2288,
	Nad83_Wisconsin_South_ftUS	= 2289,
	Nad83_Indiana_East_ftUS	= 2965,
	Nad83_Indiana_West_ftUS	= 2966,
	Nad83_Oregon_Lambert	= 2991,
	Nad83_Oregon_Lambert_ft	= 2992,
	Nad83_BC_Albers	= 3005,
	Nad83_Wisconsin_Transverse_Mercator	= 3070,
	Nad83_Maine_CS2000_East	= 3072,
	Nad83_Maine_CS2000_West	= 3074,
	Nad83_Michigan_Oblique_Mercator	= 3078,
	Nad83_Texas_State_Mapping_System	= 3081,
	Nad83_Texas_Centric_Lambert_Conformal	= 3082,
	Nad83_Texas_Centric_Albers_Equal_Area	= 3083,
	Nad83_Florida_GDL_Albers	= 3086,
	Nad83_Kentucky_Single_Zone	= 3088,
	Nad83_Kentucky_Single_Zone_ftUS	= 3089,
	Nad83_Ontario_MNR_Lambert	= 3161,
	Nad83_Great_Lakes_Albers	= 3174,
	Nad83_Great_Lakes_and_St_Lawrence_Albers	= 3175,
	Nad83_California_Albers	= 3310,
	Nad83_Alaska_Albers	= 3338,
	Nad83_Statistics_Canada_Lambert	= 3347,
	Nad83_Alberta_10_TM_Forest	= 3400,
	Nad83_Alberta_10_TM_Resource	= 3401,
	Nad83_Iowa_North_ft_US	= 3417,
	Nad83_Iowa_South_ft_US	= 3418,
	Nad83_Kansas_North_ft_US	= 3419,
	Nad83_Kansas_South_ft_US	= 3420,
	Nad83_Nevada_East_ft_US	= 3421,
	Nad83_Nevada_Central_ft_US	= 3422,
	Nad83_Nevada_West_ft_US	= 3423,
	Nad83_New_Jersey_ft_US	= 3424,
	Nad83_Arkansas_North_ftUS	= 3433,
	Nad83_Arkansas_South_ftUS	= 3434,
	Nad83_Illinois_East_ftUS	= 3435,
	Nad83_Illinois_West_ftUS	= 3436,
	Nad83_New_Hampshire_ftUS	= 3437,
	Nad83_Rhode_Island_ftUS	= 3438,
	Nad83_Louisiana_North_ftUS	= 3451,
	Nad83_Louisiana_South_ftUS	= 3452,
	Nad83_Louisiana_Offshore_ftUS	= 3453,
	Nad83_South_Dakota_South_ftUS	= 3455,
	Nad83_Maine_CS2000_Central	= 3463,
	Nad83_Utah_North_ftUS	= 3560,
	Nad83_Utah_Central_ftUS	= 3566,
	Nad83_Utah_South_ftUS	= 3567,
	Nad83_Yukon_Albers	= 3578,
	Nad83_NWT_Lambert	= 3580,
	Nad83_Ohio_North_ftUS	= 3734,
	Nad83_Ohio_South_ftUS	= 3735,
	Nad83_Wyoming_East_ftUS	= 3736,
	Nad83_Wyoming_East_Central_ftUS	= 3737,
	Nad83_Wyoming_West_Central_ftUS	= 3738,
	Nad83_Wyoming_West_ftUS	= 3739,
	Nad83_Hawaii_zone_3_ftUS	= 3759,
	Nad83_Alberta_3TM_ref_merid_111_W	= 3775,
	Nad83_Alberta_3TM_ref_merid_114_W	= 3776,
	Nad83_Alberta_3TM_ref_merid_117_W	= 3777,
	Nad83_MTQ_Lambert	= 3798,
	Nad83_Alberta_3TM_ref_merid_120_W	= 3801,
	Nad83_Mississippi_TM	= 3814,
	Nad83_Virginia_Lambert	= 3968,
	Nad83_Canada_Atlas_Lambert	= 3978,
	Nad83_BLM_59N_ftUS	= 4217,
	Nad83_BLM_60N_ftUS	= 4420,
	Nad83_BLM_1N_ftUS	= 4421,
	Nad83_BLM_2N_ftUS	= 4422,
	Nad83_BLM_3N_ftUS	= 4423,
	Nad83_BLM_4N_ftUS	= 4424,
	Nad83_BLM_5N_ftUS	= 4425,
	Nad83_BLM_6N_ftUS	= 4426,
	Nad83_BLM_7N_ftUS	= 4427,
	Nad83_BLM_8N_ftUS	= 4428,
	Nad83_BLM_9N_ftUS	= 4429,
	Nad83_BLM_10N_ftUS	= 4430,
	Nad83_BLM_11N_ftUS	= 4431,
	Nad83_BLM_12N_ftUS	= 4432,
	Nad83_BLM_13N_ftUS	= 4433,
	Nad83_BLM_18N_ftUS	= 4438,
	Nad83_BLM_19N_ftUS	= 4439,
	Nad83_South_Dakota_North_ftUS	= 4457,
	Nad83_Conus_Albers	= 5070,
	Nad83_Teranet_Ontario_Lambert	= 5320,
	Nad83_Maine_East_ftUS	= 26847,
	Nad83_Maine_West_ftUS	= 26848,
	Nad83_Minnesota_North_ftUS	= 26849,
	Nad83_Minnesota_Central_ftUS	= 26850,
	Nad83_Minnesota_South_ftUS	= 26851,
	Nad83_Nebraska_ftUS	= 26852,
	Nad83_West_Virginia_North_ftUS	= 26853,
	Nad83_West_Virginia_South_ftUS	= 26854,
	Nad83_UTM_zone_59N	= 3372,
	Nad83_UTM_zone_60N	= 3373,
	Nad83_UTM_zone_1N	= 26901,
	Nad83_UTM_zone_2N	= 26902,
	Nad83_UTM_zone_3N	= 26903,
	Nad83_UTM_zone_4N	= 26904,
	Nad83_UTM_zone_5N	= 26905,
	Nad83_UTM_zone_6N	= 26906,
	Nad83_UTM_zone_7N	= 26907,
	Nad83_UTM_zone_8N	= 26908,
	Nad83_UTM_zone_9N	= 26909,
	Nad83_UTM_zone_10N	= 26910,
	Nad83_UTM_zone_11N	= 26911,
	Nad83_UTM_zone_12N	= 26912,
	Nad83_UTM_zone_13N	= 26913,
	Nad83_UTM_zone_14N	= 26914,
	Nad83_UTM_zone_15N	= 26915,
	Nad83_UTM_zone_16N	= 26916,
	Nad83_UTM_zone_17N	= 26917,
	Nad83_UTM_zone_18N	= 26918,
	Nad83_UTM_zone_19N	= 26919,
	Nad83_UTM_zone_20N	= 26920,
	Nad83_UTM_zone_21N	= 26921,
	Nad83_UTM_zone_22N	= 26922,
	Nad83_UTM_zone_23N	= 26923,
	Nad83_BLM_14N_ftUS	= 32164,
	Nad83_BLM_15N_ftUS	= 32165,
	Nad83_BLM_16N_ftUS	= 32166,
	Nad83_BLM_17N_ftUS	= 32167,
	Nad83_Alabama_East	= 26929,
	Nad83_Alabama_West	= 26930,
	Nad83_Alaska_zone_1	= 26931,
	Nad83_Alaska_zone_2	= 26932,
	Nad83_Alaska_zone_3	= 26933,
	Nad83_Alaska_zone_4	= 26934,
	Nad83_Alaska_zone_5	= 26935,
	Nad83_Alaska_zone_6	= 26936,
	Nad83_Alaska_zone_7	= 26937,
	Nad83_Alaska_zone_8	= 26938,
	Nad83_Alaska_zone_9	= 26939,
	Nad83_Alaska_zone_10	= 26940,
	Nad83_California_zone_1	= 26941,
	Nad83_California_zone_2	= 26942,
	Nad83_California_zone_3	= 26943,
	Nad83_California_zone_4	= 26944,
	Nad83_California_zone_5	= 26945,
	Nad83_California_zone_6	= 26946,
	Nad83_Arizona_East	= 26948,
	Nad83_Arizona_Central	= 26949,
	Nad83_Arizona_West	= 26950,
	Nad83_Arkansas_North	= 26951,
	Nad83_Arkansas_South	= 26952,
	Nad83_Colorado_North	= 26953,
	Nad83_Colorado_Central	= 26954,
	Nad83_Colorado_South	= 26955,
	Nad83_Connecticut	= 26956,
	Nad83_Delaware	= 26957,
	Nad83_Florida_East	= 26958,
	Nad83_Florida_West	= 26959,
	Nad83_Florida_North	= 26960,
	Nad83_Hawaii_zone_1	= 26961,
	Nad83_Hawaii_zone_2	= 26962,
	Nad83_Hawaii_zone_3	= 26963,
	Nad83_Hawaii_zone_4	= 26964,
	Nad83_Hawaii_zone_5	= 26965,
	Nad83_Georgia_East	= 26966,
	Nad83_Georgia_West	= 26967,
	Nad83_Idaho_East	= 26968,
	Nad83_Idaho_Central	= 26969,
	Nad83_Idaho_West	= 26970,
	Nad83_Illinois_East	= 26971,
	Nad83_Illinois_West	= 26972,
	Nad83_Indiana_East	= 26973,
	Nad83_Indiana_West	= 26974,
	Nad83_Iowa_North	= 26975,
	Nad83_Iowa_South	= 26976,
	Nad83_Kansas_North	= 26977,
	Nad83_Kansas_South	= 26978,
	Nad83_Kentucky_South	= 26980,
	Nad83_Louisiana_North	= 26981,
	Nad83_Louisiana_South	= 26982,
	Nad83_Maine_East	= 26983,
	Nad83_Maine_West	= 26984,
	Nad83_Maryland	= 26985,
	Nad83_Massachusetts_Mainland	= 26986,
	Nad83_Massachusetts_Island	= 26987,
	Nad83_Michigan_North	= 26988,
	Nad83_Michigan_Central	= 26989,
	Nad83_Michigan_South	= 26990,
	Nad83_Minnesota_North	= 26991,
	Nad83_Minnesota_Central	= 26992,
	Nad83_Minnesota_South	= 26993,
	Nad83_Mississippi_East	= 26994,
	Nad83_Mississippi_West	= 26995,
	Nad83_Missouri_East	= 26996,
	Nad83_Missouri_Central	= 26997,
	Nad83_Missouri_West	= 26998,
	Nad83_Montana	= 32100,
	Nad83_Nebraska	= 32104,
	Nad83_Nevada_East	= 32107,
	Nad83_Nevada_Central	= 32108,
	Nad83_Nevada_West	= 32109,
	Nad83_New_Hampshire	= 32110,
	Nad83_New_Jersey	= 32111,
	Nad83_New_Mexico_East	= 32112,
	Nad83_New_Mexico_Central	= 32113,
	Nad83_New_Mexico_West	= 32114,
	Nad83_New_York_East	= 32115,
	Nad83_New_York_Central	= 32116,
	Nad83_New_York_West	= 32117,
	Nad83_New_York_Long_Island	= 32118,
	Nad83_North_Carolina	= 32119,
	Nad83_North_Dakota_North	= 32120,
	Nad83_North_Dakota_South	= 32121,
	Nad83_Ohio_North	= 32122,
	Nad83_Ohio_South	= 32123,
	Nad83_Oklahoma_North	= 32124,
	Nad83_Oklahoma_South	= 32125,
	Nad83_Oregon_North	= 32126,
	Nad83_Oregon_South	= 32127,
	Nad83_Pennsylvania_North	= 32128,
	Nad83_Pennsylvania_South	= 32129,
	Nad83_Rhode_Island	= 32130,
	Nad83_South_Carolina	= 32133,
	Nad83_South_Dakota_North	= 32134,
	Nad83_South_Dakota_South	= 32135,
	Nad83_Tennessee	= 32136,
	Nad83_Texas_North	= 32137,
	Nad83_Texas_North_Central	= 32138,
	Nad83_Texas_Central	= 32139,
	Nad83_Texas_South_Central	= 32140,
	Nad83_Texas_South	= 32141,
	Nad83_Utah_North	= 32142,
	Nad83_Utah_Central	= 32143,
	Nad83_Utah_South	= 32144,
	Nad83_Vermont	= 32145,
	Nad83_Virginia_North	= 32146,
	Nad83_Virginia_South	= 32147,
	Nad83_Washington_North	= 32148,
	Nad83_Washington_South	= 32149,
	Nad83_West_Virginia_North	= 32150,
	Nad83_West_Virginia_South	= 32151,
	Nad83_Wisconsin_North	= 32152,
	Nad83_Wisconsin_Central	= 32153,
	Nad83_Wisconsin_South	= 32154,
	Nad83_Wyoming_East	= 32155,
	Nad83_Wyoming_East_Central	= 32156,
	Nad83_Wyoming_West_Central	= 32157,
	Nad83_Wyoming_West	= 32158,
	Nad83_Puerto_Rico_and_Virgin_Is	= 32161,
	Nad83_SCoPQ_zone_2	= 32180,
	Nad83_MTM_zone_1	= 32181,
	Nad83_MTM_zone_2	= 32182,
	Nad83_MTM_zone_3	= 32183,
	Nad83_MTM_zone_4	= 32184,
	Nad83_MTM_zone_5	= 32185,
	Nad83_MTM_zone_6	= 32186,
	Nad83_MTM_zone_7	= 32187,
	Nad83_MTM_zone_8	= 32188,
	Nad83_MTM_zone_9	= 32189,
	Nad83_MTM_zone_10	= 32190,
	Nad83_MTM_zone_11	= 32191,
	Nad83_MTM_zone_12	= 32192,
	Nad83_MTM_zone_13	= 32193,
	Nad83_MTM_zone_14	= 32194,
	Nad83_MTM_zone_15	= 32195,
	Nad83_MTM_zone_16	= 32196,
	Nad83_MTM_zone_17	= 32197,
	Nad83_Quebec_Lambert	= 32198,
	Nad83_Louisiana_Offshore	= 32199
    } 	tkNad83Projection;

typedef /* [helpstring][uuid] */  DECLSPEC_UUID("71C4AFC7-1527-4931-B031-3309C30F3213") 
enum tkWgs84Projection
    {	Wgs84_World_Mercator	= 3395,
	Wgs84_PDC_Mercator	= 3832,
	Wgs84_Pseudo_Mercator	= 3857,
	Wgs84_Mercator_41	= 3994,
	Wgs84_World_Equidistant_Cylindrical	= 4087,
	Wgs84_UPS_North_EN	= 5041,
	Wgs84_UPS_South_EN	= 5042,
	Wgs84_UTM_grid_system_northern_hemisphere	= 32600,
	Wgs84_UTM_zone_1N	= 32601,
	Wgs84_UTM_zone_2N	= 32602,
	Wgs84_UTM_zone_3N	= 32603,
	Wgs84_UTM_zone_4N	= 32604,
	Wgs84_UTM_zone_5N	= 32605,
	Wgs84_UTM_zone_6N	= 32606,
	Wgs84_UTM_zone_7N	= 32607,
	Wgs84_UTM_zone_8N	= 32608,
	Wgs84_UTM_zone_9N	= 32609,
	Wgs84_UTM_zone_10N	= 32610,
	Wgs84_UTM_zone_11N	= 32611,
	Wgs84_UTM_zone_12N	= 32612,
	Wgs84_UTM_zone_13N	= 32613,
	Wgs84_UTM_zone_14N	= 32614,
	Wgs84_UTM_zone_15N	= 32615,
	Wgs84_UTM_zone_16N	= 32616,
	Wgs84_UTM_zone_17N	= 32617,
	Wgs84_UTM_zone_18N	= 32618,
	Wgs84_UTM_zone_19N	= 32619,
	Wgs84_UTM_zone_20N	= 32620,
	Wgs84_UTM_zone_21N	= 32621,
	Wgs84_UTM_zone_22N	= 32622,
	Wgs84_UTM_zone_23N	= 32623,
	Wgs84_UTM_zone_24N	= 32624,
	Wgs84_UTM_zone_25N	= 32625,
	Wgs84_UTM_zone_26N	= 32626,
	Wgs84_UTM_zone_27N	= 32627,
	Wgs84_UTM_zone_28N	= 32628,
	Wgs84_UTM_zone_29N	= 32629,
	Wgs84_UTM_zone_30N	= 32630,
	Wgs84_UTM_zone_31N	= 32631,
	Wgs84_UTM_zone_32N	= 32632,
	Wgs84_UTM_zone_33N	= 32633,
	Wgs84_UTM_zone_34N	= 32634,
	Wgs84_UTM_zone_35N	= 32635,
	Wgs84_UTM_zone_36N	= 32636,
	Wgs84_UTM_zone_37N	= 32637,
	Wgs84_UTM_zone_38N	= 32638,
	Wgs84_UTM_zone_39N	= 32639,
	Wgs84_UTM_zone_40N	= 32640,
	Wgs84_UTM_zone_41N	= 32641,
	Wgs84_UTM_zone_42N	= 32642,
	Wgs84_UTM_zone_43N	= 32643,
	Wgs84_UTM_zone_44N	= 32644,
	Wgs84_UTM_zone_45N	= 32645,
	Wgs84_UTM_zone_46N	= 32646,
	Wgs84_UTM_zone_47N	= 32647,
	Wgs84_UTM_zone_48N	= 32648,
	Wgs84_UTM_zone_49N	= 32649,
	Wgs84_UTM_zone_50N	= 32650,
	Wgs84_UTM_zone_51N	= 32651,
	Wgs84_UTM_zone_52N	= 32652,
	Wgs84_UTM_zone_53N	= 32653,
	Wgs84_UTM_zone_54N	= 32654,
	Wgs84_UTM_zone_55N	= 32655,
	Wgs84_UTM_zone_56N	= 32656,
	Wgs84_UTM_zone_57N	= 32657,
	Wgs84_UTM_zone_58N	= 32658,
	Wgs84_UTM_zone_59N	= 32659,
	Wgs84_UTM_zone_60N	= 32660,
	Wgs84_UPS_North_NE	= 32661,
	Wgs84_BLM_14N_ftUS	= 32664,
	Wgs84_BLM_15N_ftUS	= 32665,
	Wgs84_BLM_16N_ftUS	= 32666,
	Wgs84_BLM_17N_ftUS	= 32667,
	Wgs84_UTM_grid_system_southern_hemisphere	= 32700,
	Wgs84_UTM_zone_1S	= 32701,
	Wgs84_UTM_zone_2S	= 32702,
	Wgs84_UTM_zone_3S	= 32703,
	Wgs84_UTM_zone_4S	= 32704,
	Wgs84_UTM_zone_5S	= 32705,
	Wgs84_UTM_zone_6S	= 32706,
	Wgs84_UTM_zone_7S	= 32707,
	Wgs84_UTM_zone_8S	= 32708,
	Wgs84_UTM_zone_9S	= 32709,
	Wgs84_UTM_zone_10S	= 32710,
	Wgs84_UTM_zone_11S	= 32711,
	Wgs84_UTM_zone_12S	= 32712,
	Wgs84_UTM_zone_13S	= 32713,
	Wgs84_UTM_zone_14S	= 32714,
	Wgs84_UTM_zone_15S	= 32715,
	Wgs84_UTM_zone_16S	= 32716,
	Wgs84_UTM_zone_17S	= 32717,
	Wgs84_UTM_zone_18S	= 32718,
	Wgs84_UTM_zone_19S	= 32719,
	Wgs84_UTM_zone_20S	= 32720,
	Wgs84_UTM_zone_21S	= 32721,
	Wgs84_UTM_zone_22S	= 32722,
	Wgs84_UTM_zone_23S	= 32723,
	Wgs84_UTM_zone_24S	= 32724,
	Wgs84_UTM_zone_25S	= 32725,
	Wgs84_UTM_zone_26S	= 32726,
	Wgs84_UTM_zone_27S	= 32727,
	Wgs84_UTM_zone_28S	= 32728,
	Wgs84_UTM_zone_29S	= 32729,
	Wgs84_UTM_zone_30S	= 32730,
	Wgs84_UTM_zone_31S	= 32731,
	Wgs84_UTM_zone_32S	= 32732,
	Wgs84_UTM_zone_33S	= 32733,
	Wgs84_UTM_zone_34S	= 32734,
	Wgs84_UTM_zone_35S	= 32735,
	Wgs84_UTM_zone_36S	= 32736,
	Wgs84_UTM_zone_37S	= 32737,
	Wgs84_UTM_zone_38S	= 32738,
	Wgs84_UTM_zone_39S	= 32739,
	Wgs84_UTM_zone_40S	= 32740,
	Wgs84_UTM_zone_41S	= 32741,
	Wgs84_UTM_zone_42S	= 32742,
	Wgs84_UTM_zone_43S	= 32743,
	Wgs84_UTM_zone_44S	= 32744,
	Wgs84_UTM_zone_45S	= 32745,
	Wgs84_UTM_zone_46S	= 32746,
	Wgs84_UTM_zone_47S	= 32747,
	Wgs84_UTM_zone_48S	= 32748,
	Wgs84_UTM_zone_49S	= 32749,
	Wgs84_UTM_zone_50S	= 32750,
	Wgs84_UTM_zone_51S	= 32751,
	Wgs84_UTM_zone_52S	= 32752,
	Wgs84_UTM_zone_53S	= 32753,
	Wgs84_UTM_zone_54S	= 32754,
	Wgs84_UTM_zone_55S	= 32755,
	Wgs84_UTM_zone_56S	= 32756,
	Wgs84_UTM_zone_57S	= 32757,
	Wgs84_UTM_zone_58S	= 32758,
	Wgs84_UTM_zone_59S	= 32759,
	Wgs84_UTM_zone_60S	= 32760,
	Wgs84_UPS_South_NE	= 32761
    } 	tkWgs84Projection;



extern RPC_IF_HANDLE __MIDL_itf_MapWinGIS_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_MapWinGIS_0000_0000_v0_0_s_ifspec;

#ifndef __IGrid_INTERFACE_DEFINED__
#define __IGrid_INTERFACE_DEFINED__

/* interface IGrid */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IGrid;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("18DFB64A-9E72-4CBE-AFD6-A5B7421DD0CB")
    IGrid : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Header( 
            /* [retval][out] */ IGridHeader **pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Value( 
            /* [in] */ long Column,
            /* [in] */ long Row,
            /* [retval][out] */ VARIANT *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Value( 
            /* [in] */ long Column,
            /* [in] */ long Row,
            /* [in] */ VARIANT newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_InRam( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Maximum( 
            /* [retval][out] */ VARIANT *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Minimum( 
            /* [retval][out] */ VARIANT *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_DataType( 
            /* [retval][out] */ GridDataType *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Filename( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Open( 
            /* [in] */ BSTR Filename,
            /* [defaultvalue][optional][in] */ GridDataType DataType,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL InRam,
            /* [defaultvalue][optional][in] */ GridFileType FileType,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CreateNew( 
            /* [in] */ BSTR Filename,
            /* [in] */ IGridHeader *Header,
            /* [in] */ GridDataType DataType,
            /* [in] */ VARIANT InitialValue,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL InRam,
            /* [defaultvalue][optional][in] */ GridFileType FileType,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Close( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Save( 
            /* [optional][in] */ BSTR Filename,
            /* [defaultvalue][optional][in] */ GridFileType GridFileType,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clear( 
            /* [in] */ VARIANT ClearValue,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ProjToCell( 
            /* [in] */ double x,
            /* [in] */ double y,
            /* [out] */ long *Column,
            /* [out] */ long *Row) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CellToProj( 
            /* [in] */ long Column,
            /* [in] */ long Row,
            /* [out] */ double *x,
            /* [out] */ double *y) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CdlgFilter( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AssignNewProjection( 
            /* [in] */ BSTR projection,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_RasterColorTableColoringScheme( 
            /* [retval][out] */ IGridColorScheme **pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetRow( 
            /* [in] */ long Row,
            /* [out][in] */ float *Vals,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PutRow( 
            /* [in] */ long Row,
            /* [out][in] */ float *Vals,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetFloatWindow( 
            /* [in] */ long StartRow,
            /* [in] */ long EndRow,
            /* [in] */ long StartCol,
            /* [in] */ long EndCol,
            /* [out][in] */ float *Vals,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PutFloatWindow( 
            /* [in] */ long StartRow,
            /* [in] */ long EndRow,
            /* [in] */ long StartCol,
            /* [in] */ long EndCol,
            /* [out][in] */ float *Vals,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetInvalidValuesToNodata( 
            /* [in] */ double MinThresholdValue,
            /* [in] */ double MaxThresholdValue,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Resource( 
            /* [in] */ BSTR newSrcPath,
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IGridVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IGrid * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IGrid * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IGrid * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IGrid * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IGrid * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IGrid * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IGrid * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Header )( 
            IGrid * This,
            /* [retval][out] */ IGridHeader **pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Value )( 
            IGrid * This,
            /* [in] */ long Column,
            /* [in] */ long Row,
            /* [retval][out] */ VARIANT *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Value )( 
            IGrid * This,
            /* [in] */ long Column,
            /* [in] */ long Row,
            /* [in] */ VARIANT newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_InRam )( 
            IGrid * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Maximum )( 
            IGrid * This,
            /* [retval][out] */ VARIANT *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Minimum )( 
            IGrid * This,
            /* [retval][out] */ VARIANT *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DataType )( 
            IGrid * This,
            /* [retval][out] */ GridDataType *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Filename )( 
            IGrid * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IGrid * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IGrid * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IGrid * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IGrid * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IGrid * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IGrid * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Open )( 
            IGrid * This,
            /* [in] */ BSTR Filename,
            /* [defaultvalue][optional][in] */ GridDataType DataType,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL InRam,
            /* [defaultvalue][optional][in] */ GridFileType FileType,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CreateNew )( 
            IGrid * This,
            /* [in] */ BSTR Filename,
            /* [in] */ IGridHeader *Header,
            /* [in] */ GridDataType DataType,
            /* [in] */ VARIANT InitialValue,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL InRam,
            /* [defaultvalue][optional][in] */ GridFileType FileType,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Close )( 
            IGrid * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Save )( 
            IGrid * This,
            /* [optional][in] */ BSTR Filename,
            /* [defaultvalue][optional][in] */ GridFileType GridFileType,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clear )( 
            IGrid * This,
            /* [in] */ VARIANT ClearValue,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ProjToCell )( 
            IGrid * This,
            /* [in] */ double x,
            /* [in] */ double y,
            /* [out] */ long *Column,
            /* [out] */ long *Row);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CellToProj )( 
            IGrid * This,
            /* [in] */ long Column,
            /* [in] */ long Row,
            /* [out] */ double *x,
            /* [out] */ double *y);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CdlgFilter )( 
            IGrid * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AssignNewProjection )( 
            IGrid * This,
            /* [in] */ BSTR projection,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_RasterColorTableColoringScheme )( 
            IGrid * This,
            /* [retval][out] */ IGridColorScheme **pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetRow )( 
            IGrid * This,
            /* [in] */ long Row,
            /* [out][in] */ float *Vals,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PutRow )( 
            IGrid * This,
            /* [in] */ long Row,
            /* [out][in] */ float *Vals,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetFloatWindow )( 
            IGrid * This,
            /* [in] */ long StartRow,
            /* [in] */ long EndRow,
            /* [in] */ long StartCol,
            /* [in] */ long EndCol,
            /* [out][in] */ float *Vals,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PutFloatWindow )( 
            IGrid * This,
            /* [in] */ long StartRow,
            /* [in] */ long EndRow,
            /* [in] */ long StartCol,
            /* [in] */ long EndCol,
            /* [out][in] */ float *Vals,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetInvalidValuesToNodata )( 
            IGrid * This,
            /* [in] */ double MinThresholdValue,
            /* [in] */ double MaxThresholdValue,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Resource )( 
            IGrid * This,
            /* [in] */ BSTR newSrcPath,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        END_INTERFACE
    } IGridVtbl;

    interface IGrid
    {
        CONST_VTBL struct IGridVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IGrid_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IGrid_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IGrid_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IGrid_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IGrid_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IGrid_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IGrid_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IGrid_get_Header(This,pVal)	\
    ( (This)->lpVtbl -> get_Header(This,pVal) ) 

#define IGrid_get_Value(This,Column,Row,pVal)	\
    ( (This)->lpVtbl -> get_Value(This,Column,Row,pVal) ) 

#define IGrid_put_Value(This,Column,Row,newVal)	\
    ( (This)->lpVtbl -> put_Value(This,Column,Row,newVal) ) 

#define IGrid_get_InRam(This,pVal)	\
    ( (This)->lpVtbl -> get_InRam(This,pVal) ) 

#define IGrid_get_Maximum(This,pVal)	\
    ( (This)->lpVtbl -> get_Maximum(This,pVal) ) 

#define IGrid_get_Minimum(This,pVal)	\
    ( (This)->lpVtbl -> get_Minimum(This,pVal) ) 

#define IGrid_get_DataType(This,pVal)	\
    ( (This)->lpVtbl -> get_DataType(This,pVal) ) 

#define IGrid_get_Filename(This,pVal)	\
    ( (This)->lpVtbl -> get_Filename(This,pVal) ) 

#define IGrid_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IGrid_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IGrid_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IGrid_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IGrid_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IGrid_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IGrid_Open(This,Filename,DataType,InRam,FileType,cBack,retval)	\
    ( (This)->lpVtbl -> Open(This,Filename,DataType,InRam,FileType,cBack,retval) ) 

#define IGrid_CreateNew(This,Filename,Header,DataType,InitialValue,InRam,FileType,cBack,retval)	\
    ( (This)->lpVtbl -> CreateNew(This,Filename,Header,DataType,InitialValue,InRam,FileType,cBack,retval) ) 

#define IGrid_Close(This,retval)	\
    ( (This)->lpVtbl -> Close(This,retval) ) 

#define IGrid_Save(This,Filename,GridFileType,cBack,retval)	\
    ( (This)->lpVtbl -> Save(This,Filename,GridFileType,cBack,retval) ) 

#define IGrid_Clear(This,ClearValue,retval)	\
    ( (This)->lpVtbl -> Clear(This,ClearValue,retval) ) 

#define IGrid_ProjToCell(This,x,y,Column,Row)	\
    ( (This)->lpVtbl -> ProjToCell(This,x,y,Column,Row) ) 

#define IGrid_CellToProj(This,Column,Row,x,y)	\
    ( (This)->lpVtbl -> CellToProj(This,Column,Row,x,y) ) 

#define IGrid_get_CdlgFilter(This,pVal)	\
    ( (This)->lpVtbl -> get_CdlgFilter(This,pVal) ) 

#define IGrid_AssignNewProjection(This,projection,retval)	\
    ( (This)->lpVtbl -> AssignNewProjection(This,projection,retval) ) 

#define IGrid_get_RasterColorTableColoringScheme(This,pVal)	\
    ( (This)->lpVtbl -> get_RasterColorTableColoringScheme(This,pVal) ) 

#define IGrid_GetRow(This,Row,Vals,retval)	\
    ( (This)->lpVtbl -> GetRow(This,Row,Vals,retval) ) 

#define IGrid_PutRow(This,Row,Vals,retval)	\
    ( (This)->lpVtbl -> PutRow(This,Row,Vals,retval) ) 

#define IGrid_GetFloatWindow(This,StartRow,EndRow,StartCol,EndCol,Vals,retval)	\
    ( (This)->lpVtbl -> GetFloatWindow(This,StartRow,EndRow,StartCol,EndCol,Vals,retval) ) 

#define IGrid_PutFloatWindow(This,StartRow,EndRow,StartCol,EndCol,Vals,retval)	\
    ( (This)->lpVtbl -> PutFloatWindow(This,StartRow,EndRow,StartCol,EndCol,Vals,retval) ) 

#define IGrid_SetInvalidValuesToNodata(This,MinThresholdValue,MaxThresholdValue,retval)	\
    ( (This)->lpVtbl -> SetInvalidValuesToNodata(This,MinThresholdValue,MaxThresholdValue,retval) ) 

#define IGrid_Resource(This,newSrcPath,pVal)	\
    ( (This)->lpVtbl -> Resource(This,newSrcPath,pVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IGrid_INTERFACE_DEFINED__ */


#ifndef __IGridHeader_INTERFACE_DEFINED__
#define __IGridHeader_INTERFACE_DEFINED__

/* interface IGridHeader */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IGridHeader;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("E42814D1-6269-41B1-93C2-AA848F00E459")
    IGridHeader : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumberCols( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_NumberCols( 
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumberRows( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_NumberRows( 
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NodataValue( 
            /* [retval][out] */ VARIANT *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_NodataValue( 
            /* [in] */ VARIANT newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_dX( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_dX( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_dY( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_dY( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_XllCenter( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_XllCenter( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_YllCenter( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_YllCenter( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Projection( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Projection( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Notes( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Notes( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Owner( 
            /* [in] */ int *t,
            /* [in] */ int *d,
            /* [in] */ int *s,
            /* [in] */ int *l,
            /* [in] */ int *f) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CopyFrom( 
            /* [in] */ IGridHeader *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ColorTable( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ColorTable( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GeoProjection( 
            /* [retval][out] */ IGeoProjection **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GeoProjection( 
            /* [in] */ IGeoProjection *newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IGridHeaderVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IGridHeader * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IGridHeader * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IGridHeader * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IGridHeader * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IGridHeader * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IGridHeader * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IGridHeader * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumberCols )( 
            IGridHeader * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_NumberCols )( 
            IGridHeader * This,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumberRows )( 
            IGridHeader * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_NumberRows )( 
            IGridHeader * This,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NodataValue )( 
            IGridHeader * This,
            /* [retval][out] */ VARIANT *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_NodataValue )( 
            IGridHeader * This,
            /* [in] */ VARIANT newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_dX )( 
            IGridHeader * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_dX )( 
            IGridHeader * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_dY )( 
            IGridHeader * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_dY )( 
            IGridHeader * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_XllCenter )( 
            IGridHeader * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_XllCenter )( 
            IGridHeader * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_YllCenter )( 
            IGridHeader * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_YllCenter )( 
            IGridHeader * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Projection )( 
            IGridHeader * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Projection )( 
            IGridHeader * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Notes )( 
            IGridHeader * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Notes )( 
            IGridHeader * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IGridHeader * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IGridHeader * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IGridHeader * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IGridHeader * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IGridHeader * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IGridHeader * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Owner )( 
            IGridHeader * This,
            /* [in] */ int *t,
            /* [in] */ int *d,
            /* [in] */ int *s,
            /* [in] */ int *l,
            /* [in] */ int *f);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CopyFrom )( 
            IGridHeader * This,
            /* [in] */ IGridHeader *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ColorTable )( 
            IGridHeader * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ColorTable )( 
            IGridHeader * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GeoProjection )( 
            IGridHeader * This,
            /* [retval][out] */ IGeoProjection **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GeoProjection )( 
            IGridHeader * This,
            /* [in] */ IGeoProjection *newVal);
        
        END_INTERFACE
    } IGridHeaderVtbl;

    interface IGridHeader
    {
        CONST_VTBL struct IGridHeaderVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IGridHeader_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IGridHeader_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IGridHeader_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IGridHeader_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IGridHeader_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IGridHeader_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IGridHeader_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IGridHeader_get_NumberCols(This,pVal)	\
    ( (This)->lpVtbl -> get_NumberCols(This,pVal) ) 

#define IGridHeader_put_NumberCols(This,newVal)	\
    ( (This)->lpVtbl -> put_NumberCols(This,newVal) ) 

#define IGridHeader_get_NumberRows(This,pVal)	\
    ( (This)->lpVtbl -> get_NumberRows(This,pVal) ) 

#define IGridHeader_put_NumberRows(This,newVal)	\
    ( (This)->lpVtbl -> put_NumberRows(This,newVal) ) 

#define IGridHeader_get_NodataValue(This,pVal)	\
    ( (This)->lpVtbl -> get_NodataValue(This,pVal) ) 

#define IGridHeader_put_NodataValue(This,newVal)	\
    ( (This)->lpVtbl -> put_NodataValue(This,newVal) ) 

#define IGridHeader_get_dX(This,pVal)	\
    ( (This)->lpVtbl -> get_dX(This,pVal) ) 

#define IGridHeader_put_dX(This,newVal)	\
    ( (This)->lpVtbl -> put_dX(This,newVal) ) 

#define IGridHeader_get_dY(This,pVal)	\
    ( (This)->lpVtbl -> get_dY(This,pVal) ) 

#define IGridHeader_put_dY(This,newVal)	\
    ( (This)->lpVtbl -> put_dY(This,newVal) ) 

#define IGridHeader_get_XllCenter(This,pVal)	\
    ( (This)->lpVtbl -> get_XllCenter(This,pVal) ) 

#define IGridHeader_put_XllCenter(This,newVal)	\
    ( (This)->lpVtbl -> put_XllCenter(This,newVal) ) 

#define IGridHeader_get_YllCenter(This,pVal)	\
    ( (This)->lpVtbl -> get_YllCenter(This,pVal) ) 

#define IGridHeader_put_YllCenter(This,newVal)	\
    ( (This)->lpVtbl -> put_YllCenter(This,newVal) ) 

#define IGridHeader_get_Projection(This,pVal)	\
    ( (This)->lpVtbl -> get_Projection(This,pVal) ) 

#define IGridHeader_put_Projection(This,newVal)	\
    ( (This)->lpVtbl -> put_Projection(This,newVal) ) 

#define IGridHeader_get_Notes(This,pVal)	\
    ( (This)->lpVtbl -> get_Notes(This,pVal) ) 

#define IGridHeader_put_Notes(This,newVal)	\
    ( (This)->lpVtbl -> put_Notes(This,newVal) ) 

#define IGridHeader_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IGridHeader_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IGridHeader_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IGridHeader_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IGridHeader_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IGridHeader_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IGridHeader_put_Owner(This,t,d,s,l,f)	\
    ( (This)->lpVtbl -> put_Owner(This,t,d,s,l,f) ) 

#define IGridHeader_CopyFrom(This,pVal)	\
    ( (This)->lpVtbl -> CopyFrom(This,pVal) ) 

#define IGridHeader_get_ColorTable(This,pVal)	\
    ( (This)->lpVtbl -> get_ColorTable(This,pVal) ) 

#define IGridHeader_put_ColorTable(This,newVal)	\
    ( (This)->lpVtbl -> put_ColorTable(This,newVal) ) 

#define IGridHeader_get_GeoProjection(This,pVal)	\
    ( (This)->lpVtbl -> get_GeoProjection(This,pVal) ) 

#define IGridHeader_put_GeoProjection(This,newVal)	\
    ( (This)->lpVtbl -> put_GeoProjection(This,newVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IGridHeader_INTERFACE_DEFINED__ */


#ifndef __IESRIGridManager_INTERFACE_DEFINED__
#define __IESRIGridManager_INTERFACE_DEFINED__

/* interface IESRIGridManager */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IESRIGridManager;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("55B3F2DA-EB09-4FA9-B74B-9A1B3E457318")
    IESRIGridManager : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CanUseESRIGrids( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DeleteESRIGrids( 
            /* [in] */ BSTR Filename,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE IsESRIGrid( 
            /* [in] */ BSTR Filename,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IESRIGridManagerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IESRIGridManager * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IESRIGridManager * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IESRIGridManager * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IESRIGridManager * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IESRIGridManager * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IESRIGridManager * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IESRIGridManager * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IESRIGridManager * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IESRIGridManager * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IESRIGridManager * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IESRIGridManager * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CanUseESRIGrids )( 
            IESRIGridManager * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DeleteESRIGrids )( 
            IESRIGridManager * This,
            /* [in] */ BSTR Filename,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *IsESRIGrid )( 
            IESRIGridManager * This,
            /* [in] */ BSTR Filename,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        END_INTERFACE
    } IESRIGridManagerVtbl;

    interface IESRIGridManager
    {
        CONST_VTBL struct IESRIGridManagerVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IESRIGridManager_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IESRIGridManager_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IESRIGridManager_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IESRIGridManager_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IESRIGridManager_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IESRIGridManager_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IESRIGridManager_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IESRIGridManager_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IESRIGridManager_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IESRIGridManager_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IESRIGridManager_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IESRIGridManager_CanUseESRIGrids(This,retval)	\
    ( (This)->lpVtbl -> CanUseESRIGrids(This,retval) ) 

#define IESRIGridManager_DeleteESRIGrids(This,Filename,retval)	\
    ( (This)->lpVtbl -> DeleteESRIGrids(This,Filename,retval) ) 

#define IESRIGridManager_IsESRIGrid(This,Filename,retval)	\
    ( (This)->lpVtbl -> IsESRIGrid(This,Filename,retval) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IESRIGridManager_INTERFACE_DEFINED__ */


#ifndef __IImage_INTERFACE_DEFINED__
#define __IImage_INTERFACE_DEFINED__

/* interface IImage */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IImage;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("79C5F83E-FB53-4189-9EC4-4AC25440D825")
    IImage : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Open( 
            /* [in] */ BSTR ImageFileName,
            /* [defaultvalue][optional][in] */ ImageType FileType,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL InRam,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Save( 
            /* [in] */ BSTR ImageFileName,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL WriteWorldFile,
            /* [defaultvalue][optional][in] */ ImageType FileType,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CreateNew( 
            /* [in] */ long NewWidth,
            /* [in] */ long NewHeight,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Close( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clear( 
            /* [defaultvalue][optional][in] */ OLE_COLOR CanvasColor,
            /* [optional][in] */ ICallback *CBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetRow( 
            /* [in] */ long Row,
            /* [out][in] */ long *Vals,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Width( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Height( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_YllCenter( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_YllCenter( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_XllCenter( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_XllCenter( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_dY( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_dY( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_dX( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_dX( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Value( 
            /* [in] */ long row,
            /* [in] */ long col,
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Value( 
            /* [in] */ long row,
            /* [in] */ long col,
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IsInRam( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_TransparencyColor( 
            /* [retval][out] */ OLE_COLOR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_TransparencyColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_UseTransparencyColor( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_UseTransparencyColor( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CdlgFilter( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [hidden][helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FileHandle( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ImageType( 
            /* [retval][out] */ ImageType *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Picture( 
            /* [retval][out] */ IPictureDisp **pVal) = 0;
        
        virtual /* [helpstring][id][propputref] */ HRESULT STDMETHODCALLTYPE putref_Picture( 
            /* [in] */ IPictureDisp *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Filename( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetImageBitsDC( 
            /* [in] */ long hDC,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetImageBitsDC( 
            /* [in] */ long hDC,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [hidden][helpstring][id] */ HRESULT STDMETHODCALLTYPE SetVisibleExtents( 
            /* [in] */ double newMinX,
            /* [in] */ double newMinY,
            /* [in] */ double newMaxX,
            /* [in] */ double newMaxY,
            /* [in] */ long newPixelsInView,
            /* [in] */ float transPercent) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetProjection( 
            /* [in] */ BSTR Proj4,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetProjection( 
            /* [retval][out] */ BSTR *Proj4) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_OriginalWidth( 
            /* [retval][out] */ LONG *OriginalWidth) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_OriginalHeight( 
            /* [retval][out] */ LONG *OriginalHeight) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Resource( 
            /* [in] */ BSTR newImgPath,
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE _pushSchemetkRaster( 
            /* [in] */ IGridColorScheme *cScheme,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [hidden][helpstring][id] */ HRESULT STDMETHODCALLTYPE GetOriginalXllCenter( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [hidden][helpstring][id] */ HRESULT STDMETHODCALLTYPE GetOriginalYllCenter( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [hidden][helpstring][id] */ HRESULT STDMETHODCALLTYPE GetOriginal_dX( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [hidden][helpstring][id] */ HRESULT STDMETHODCALLTYPE GetOriginal_dY( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [hidden][helpstring][id] */ HRESULT STDMETHODCALLTYPE GetOriginalHeight( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [hidden][helpstring][id] */ HRESULT STDMETHODCALLTYPE GetOriginalWidth( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_AllowHillshade( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_AllowHillshade( 
            /* [in] */ VARIANT_BOOL newValue) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SetToGrey( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_SetToGrey( 
            /* [in] */ VARIANT_BOOL newValue) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_UseHistogram( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_UseHistogram( 
            /* [in] */ VARIANT_BOOL newValue) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_HasColorTable( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PaletteInterpretation( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_BufferSize( 
            /* [retval][out] */ int *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_BufferSize( 
            /* [in] */ int newValue) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NoBands( 
            /* [retval][out] */ int *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ImageColorScheme( 
            /* [retval][out] */ PredefinedColorScheme *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ImageColorScheme( 
            /* [in] */ PredefinedColorScheme newValue) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_DrawingMethod( 
            /* [retval][out] */ int *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_DrawingMethod( 
            /* [in] */ int newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE BuildOverviews( 
            /* [in] */ tkGDALResamplingMethod ResamplingMethod,
            /* [in] */ int NumOverviews,
            /* [in] */ SAFEARRAY * OverviewList,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ClearGDALCache( 
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ClearGDALCache( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_TransparencyPercent( 
            /* [retval][out] */ double *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_TransparencyPercent( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_TransparencyColor2( 
            /* [retval][out] */ OLE_COLOR *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_TransparencyColor2( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_DownsamplingMode( 
            /* [retval][out] */ tkInterpolationMode *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_DownsamplingMode( 
            /* [in] */ tkInterpolationMode newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_UpsamplingMode( 
            /* [retval][out] */ tkInterpolationMode *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_UpsamplingMode( 
            /* [in] */ tkInterpolationMode newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Labels( 
            /* [retval][out] */ ILabels **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Labels( 
            /* [in] */ ILabels *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Extents( 
            /* [retval][out] */ IExtents **pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ProjectionToImage( 
            /* [in] */ double ProjX,
            /* [in] */ double ProjY,
            /* [out] */ long *ImageX,
            /* [out] */ long *ImageY) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ImageToProjection( 
            /* [in] */ long ImageX,
            /* [in] */ long ImageY,
            /* [out] */ double *ProjX,
            /* [out] */ double *ProjY) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ProjectionToBuffer( 
            /* [in] */ double ProjX,
            /* [in] */ double ProjY,
            /* [out] */ long *BufferX,
            /* [out] */ long *BufferY) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE BufferToProjection( 
            /* [in] */ long BufferX,
            /* [in] */ long BufferY,
            /* [out] */ double *ProjX,
            /* [out] */ double *ProjY) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CanUseGrouping( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_CanUseGrouping( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_OriginalXllCenter( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_OriginalXllCenter( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_OriginalYllCenter( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_OriginalYllCenter( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_OriginalDX( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_OriginalDX( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_OriginalDY( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_OriginalDY( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetUniqueColors( 
            /* [in] */ double MaxBufferSizeMB,
            /* [out] */ VARIANT *Colors,
            /* [out] */ VARIANT *Frequencies,
            /* [retval][out] */ LONG *Count) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetNoDataValue( 
            double Value,
            VARIANT_BOOL *Result) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumOverviews( 
            /* [retval][out] */ int *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE LoadBuffer( 
            /* [defaultvalue][optional][in] */ double maxBufferSize,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SourceType( 
            /* [retval][out] */ tkImageSourceType *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Serialize( 
            /* [in] */ VARIANT_BOOL SerializePixels,
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Deserialize( 
            /* [in] */ BSTR newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IImageVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IImage * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IImage * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IImage * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IImage * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IImage * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IImage * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IImage * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Open )( 
            IImage * This,
            /* [in] */ BSTR ImageFileName,
            /* [defaultvalue][optional][in] */ ImageType FileType,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL InRam,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Save )( 
            IImage * This,
            /* [in] */ BSTR ImageFileName,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL WriteWorldFile,
            /* [defaultvalue][optional][in] */ ImageType FileType,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CreateNew )( 
            IImage * This,
            /* [in] */ long NewWidth,
            /* [in] */ long NewHeight,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Close )( 
            IImage * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clear )( 
            IImage * This,
            /* [defaultvalue][optional][in] */ OLE_COLOR CanvasColor,
            /* [optional][in] */ ICallback *CBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetRow )( 
            IImage * This,
            /* [in] */ long Row,
            /* [out][in] */ long *Vals,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Width )( 
            IImage * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Height )( 
            IImage * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_YllCenter )( 
            IImage * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_YllCenter )( 
            IImage * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_XllCenter )( 
            IImage * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_XllCenter )( 
            IImage * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_dY )( 
            IImage * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_dY )( 
            IImage * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_dX )( 
            IImage * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_dX )( 
            IImage * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Value )( 
            IImage * This,
            /* [in] */ long row,
            /* [in] */ long col,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Value )( 
            IImage * This,
            /* [in] */ long row,
            /* [in] */ long col,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsInRam )( 
            IImage * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_TransparencyColor )( 
            IImage * This,
            /* [retval][out] */ OLE_COLOR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_TransparencyColor )( 
            IImage * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_UseTransparencyColor )( 
            IImage * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_UseTransparencyColor )( 
            IImage * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IImage * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IImage * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CdlgFilter )( 
            IImage * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IImage * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IImage * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IImage * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IImage * This,
            /* [in] */ BSTR newVal);
        
        /* [hidden][helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FileHandle )( 
            IImage * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ImageType )( 
            IImage * This,
            /* [retval][out] */ ImageType *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Picture )( 
            IImage * This,
            /* [retval][out] */ IPictureDisp **pVal);
        
        /* [helpstring][id][propputref] */ HRESULT ( STDMETHODCALLTYPE *putref_Picture )( 
            IImage * This,
            /* [in] */ IPictureDisp *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Filename )( 
            IImage * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetImageBitsDC )( 
            IImage * This,
            /* [in] */ long hDC,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetImageBitsDC )( 
            IImage * This,
            /* [in] */ long hDC,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [hidden][helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetVisibleExtents )( 
            IImage * This,
            /* [in] */ double newMinX,
            /* [in] */ double newMinY,
            /* [in] */ double newMaxX,
            /* [in] */ double newMaxY,
            /* [in] */ long newPixelsInView,
            /* [in] */ float transPercent);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetProjection )( 
            IImage * This,
            /* [in] */ BSTR Proj4,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetProjection )( 
            IImage * This,
            /* [retval][out] */ BSTR *Proj4);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_OriginalWidth )( 
            IImage * This,
            /* [retval][out] */ LONG *OriginalWidth);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_OriginalHeight )( 
            IImage * This,
            /* [retval][out] */ LONG *OriginalHeight);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Resource )( 
            IImage * This,
            /* [in] */ BSTR newImgPath,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *_pushSchemetkRaster )( 
            IImage * This,
            /* [in] */ IGridColorScheme *cScheme,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [hidden][helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetOriginalXllCenter )( 
            IImage * This,
            /* [retval][out] */ double *pVal);
        
        /* [hidden][helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetOriginalYllCenter )( 
            IImage * This,
            /* [retval][out] */ double *pVal);
        
        /* [hidden][helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetOriginal_dX )( 
            IImage * This,
            /* [retval][out] */ double *pVal);
        
        /* [hidden][helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetOriginal_dY )( 
            IImage * This,
            /* [retval][out] */ double *pVal);
        
        /* [hidden][helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetOriginalHeight )( 
            IImage * This,
            /* [retval][out] */ long *pVal);
        
        /* [hidden][helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetOriginalWidth )( 
            IImage * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_AllowHillshade )( 
            IImage * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_AllowHillshade )( 
            IImage * This,
            /* [in] */ VARIANT_BOOL newValue);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SetToGrey )( 
            IImage * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_SetToGrey )( 
            IImage * This,
            /* [in] */ VARIANT_BOOL newValue);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_UseHistogram )( 
            IImage * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_UseHistogram )( 
            IImage * This,
            /* [in] */ VARIANT_BOOL newValue);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_HasColorTable )( 
            IImage * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PaletteInterpretation )( 
            IImage * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_BufferSize )( 
            IImage * This,
            /* [retval][out] */ int *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_BufferSize )( 
            IImage * This,
            /* [in] */ int newValue);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NoBands )( 
            IImage * This,
            /* [retval][out] */ int *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ImageColorScheme )( 
            IImage * This,
            /* [retval][out] */ PredefinedColorScheme *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ImageColorScheme )( 
            IImage * This,
            /* [in] */ PredefinedColorScheme newValue);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DrawingMethod )( 
            IImage * This,
            /* [retval][out] */ int *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DrawingMethod )( 
            IImage * This,
            /* [in] */ int newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *BuildOverviews )( 
            IImage * This,
            /* [in] */ tkGDALResamplingMethod ResamplingMethod,
            /* [in] */ int NumOverviews,
            /* [in] */ SAFEARRAY * OverviewList,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ClearGDALCache )( 
            IImage * This,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ClearGDALCache )( 
            IImage * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_TransparencyPercent )( 
            IImage * This,
            /* [retval][out] */ double *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_TransparencyPercent )( 
            IImage * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_TransparencyColor2 )( 
            IImage * This,
            /* [retval][out] */ OLE_COLOR *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_TransparencyColor2 )( 
            IImage * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DownsamplingMode )( 
            IImage * This,
            /* [retval][out] */ tkInterpolationMode *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DownsamplingMode )( 
            IImage * This,
            /* [in] */ tkInterpolationMode newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_UpsamplingMode )( 
            IImage * This,
            /* [retval][out] */ tkInterpolationMode *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_UpsamplingMode )( 
            IImage * This,
            /* [in] */ tkInterpolationMode newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Labels )( 
            IImage * This,
            /* [retval][out] */ ILabels **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Labels )( 
            IImage * This,
            /* [in] */ ILabels *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Extents )( 
            IImage * This,
            /* [retval][out] */ IExtents **pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ProjectionToImage )( 
            IImage * This,
            /* [in] */ double ProjX,
            /* [in] */ double ProjY,
            /* [out] */ long *ImageX,
            /* [out] */ long *ImageY);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ImageToProjection )( 
            IImage * This,
            /* [in] */ long ImageX,
            /* [in] */ long ImageY,
            /* [out] */ double *ProjX,
            /* [out] */ double *ProjY);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ProjectionToBuffer )( 
            IImage * This,
            /* [in] */ double ProjX,
            /* [in] */ double ProjY,
            /* [out] */ long *BufferX,
            /* [out] */ long *BufferY);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *BufferToProjection )( 
            IImage * This,
            /* [in] */ long BufferX,
            /* [in] */ long BufferY,
            /* [out] */ double *ProjX,
            /* [out] */ double *ProjY);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CanUseGrouping )( 
            IImage * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_CanUseGrouping )( 
            IImage * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_OriginalXllCenter )( 
            IImage * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_OriginalXllCenter )( 
            IImage * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_OriginalYllCenter )( 
            IImage * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_OriginalYllCenter )( 
            IImage * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_OriginalDX )( 
            IImage * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_OriginalDX )( 
            IImage * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_OriginalDY )( 
            IImage * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_OriginalDY )( 
            IImage * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetUniqueColors )( 
            IImage * This,
            /* [in] */ double MaxBufferSizeMB,
            /* [out] */ VARIANT *Colors,
            /* [out] */ VARIANT *Frequencies,
            /* [retval][out] */ LONG *Count);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetNoDataValue )( 
            IImage * This,
            double Value,
            VARIANT_BOOL *Result);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumOverviews )( 
            IImage * This,
            /* [retval][out] */ int *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *LoadBuffer )( 
            IImage * This,
            /* [defaultvalue][optional][in] */ double maxBufferSize,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SourceType )( 
            IImage * This,
            /* [retval][out] */ tkImageSourceType *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Serialize )( 
            IImage * This,
            /* [in] */ VARIANT_BOOL SerializePixels,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Deserialize )( 
            IImage * This,
            /* [in] */ BSTR newVal);
        
        END_INTERFACE
    } IImageVtbl;

    interface IImage
    {
        CONST_VTBL struct IImageVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IImage_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IImage_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IImage_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IImage_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IImage_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IImage_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IImage_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IImage_Open(This,ImageFileName,FileType,InRam,cBack,retval)	\
    ( (This)->lpVtbl -> Open(This,ImageFileName,FileType,InRam,cBack,retval) ) 

#define IImage_Save(This,ImageFileName,WriteWorldFile,FileType,cBack,retval)	\
    ( (This)->lpVtbl -> Save(This,ImageFileName,WriteWorldFile,FileType,cBack,retval) ) 

#define IImage_CreateNew(This,NewWidth,NewHeight,retval)	\
    ( (This)->lpVtbl -> CreateNew(This,NewWidth,NewHeight,retval) ) 

#define IImage_Close(This,retval)	\
    ( (This)->lpVtbl -> Close(This,retval) ) 

#define IImage_Clear(This,CanvasColor,CBack,retval)	\
    ( (This)->lpVtbl -> Clear(This,CanvasColor,CBack,retval) ) 

#define IImage_GetRow(This,Row,Vals,retval)	\
    ( (This)->lpVtbl -> GetRow(This,Row,Vals,retval) ) 

#define IImage_get_Width(This,pVal)	\
    ( (This)->lpVtbl -> get_Width(This,pVal) ) 

#define IImage_get_Height(This,pVal)	\
    ( (This)->lpVtbl -> get_Height(This,pVal) ) 

#define IImage_get_YllCenter(This,pVal)	\
    ( (This)->lpVtbl -> get_YllCenter(This,pVal) ) 

#define IImage_put_YllCenter(This,newVal)	\
    ( (This)->lpVtbl -> put_YllCenter(This,newVal) ) 

#define IImage_get_XllCenter(This,pVal)	\
    ( (This)->lpVtbl -> get_XllCenter(This,pVal) ) 

#define IImage_put_XllCenter(This,newVal)	\
    ( (This)->lpVtbl -> put_XllCenter(This,newVal) ) 

#define IImage_get_dY(This,pVal)	\
    ( (This)->lpVtbl -> get_dY(This,pVal) ) 

#define IImage_put_dY(This,newVal)	\
    ( (This)->lpVtbl -> put_dY(This,newVal) ) 

#define IImage_get_dX(This,pVal)	\
    ( (This)->lpVtbl -> get_dX(This,pVal) ) 

#define IImage_put_dX(This,newVal)	\
    ( (This)->lpVtbl -> put_dX(This,newVal) ) 

#define IImage_get_Value(This,row,col,pVal)	\
    ( (This)->lpVtbl -> get_Value(This,row,col,pVal) ) 

#define IImage_put_Value(This,row,col,newVal)	\
    ( (This)->lpVtbl -> put_Value(This,row,col,newVal) ) 

#define IImage_get_IsInRam(This,pVal)	\
    ( (This)->lpVtbl -> get_IsInRam(This,pVal) ) 

#define IImage_get_TransparencyColor(This,pVal)	\
    ( (This)->lpVtbl -> get_TransparencyColor(This,pVal) ) 

#define IImage_put_TransparencyColor(This,newVal)	\
    ( (This)->lpVtbl -> put_TransparencyColor(This,newVal) ) 

#define IImage_get_UseTransparencyColor(This,pVal)	\
    ( (This)->lpVtbl -> get_UseTransparencyColor(This,pVal) ) 

#define IImage_put_UseTransparencyColor(This,newVal)	\
    ( (This)->lpVtbl -> put_UseTransparencyColor(This,newVal) ) 

#define IImage_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IImage_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IImage_get_CdlgFilter(This,pVal)	\
    ( (This)->lpVtbl -> get_CdlgFilter(This,pVal) ) 

#define IImage_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IImage_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IImage_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IImage_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IImage_get_FileHandle(This,pVal)	\
    ( (This)->lpVtbl -> get_FileHandle(This,pVal) ) 

#define IImage_get_ImageType(This,pVal)	\
    ( (This)->lpVtbl -> get_ImageType(This,pVal) ) 

#define IImage_get_Picture(This,pVal)	\
    ( (This)->lpVtbl -> get_Picture(This,pVal) ) 

#define IImage_putref_Picture(This,newVal)	\
    ( (This)->lpVtbl -> putref_Picture(This,newVal) ) 

#define IImage_get_Filename(This,pVal)	\
    ( (This)->lpVtbl -> get_Filename(This,pVal) ) 

#define IImage_GetImageBitsDC(This,hDC,retval)	\
    ( (This)->lpVtbl -> GetImageBitsDC(This,hDC,retval) ) 

#define IImage_SetImageBitsDC(This,hDC,retval)	\
    ( (This)->lpVtbl -> SetImageBitsDC(This,hDC,retval) ) 

#define IImage_SetVisibleExtents(This,newMinX,newMinY,newMaxX,newMaxY,newPixelsInView,transPercent)	\
    ( (This)->lpVtbl -> SetVisibleExtents(This,newMinX,newMinY,newMaxX,newMaxY,newPixelsInView,transPercent) ) 

#define IImage_SetProjection(This,Proj4,retval)	\
    ( (This)->lpVtbl -> SetProjection(This,Proj4,retval) ) 

#define IImage_GetProjection(This,Proj4)	\
    ( (This)->lpVtbl -> GetProjection(This,Proj4) ) 

#define IImage_get_OriginalWidth(This,OriginalWidth)	\
    ( (This)->lpVtbl -> get_OriginalWidth(This,OriginalWidth) ) 

#define IImage_get_OriginalHeight(This,OriginalHeight)	\
    ( (This)->lpVtbl -> get_OriginalHeight(This,OriginalHeight) ) 

#define IImage_Resource(This,newImgPath,pVal)	\
    ( (This)->lpVtbl -> Resource(This,newImgPath,pVal) ) 

#define IImage__pushSchemetkRaster(This,cScheme,retval)	\
    ( (This)->lpVtbl -> _pushSchemetkRaster(This,cScheme,retval) ) 

#define IImage_GetOriginalXllCenter(This,pVal)	\
    ( (This)->lpVtbl -> GetOriginalXllCenter(This,pVal) ) 

#define IImage_GetOriginalYllCenter(This,pVal)	\
    ( (This)->lpVtbl -> GetOriginalYllCenter(This,pVal) ) 

#define IImage_GetOriginal_dX(This,pVal)	\
    ( (This)->lpVtbl -> GetOriginal_dX(This,pVal) ) 

#define IImage_GetOriginal_dY(This,pVal)	\
    ( (This)->lpVtbl -> GetOriginal_dY(This,pVal) ) 

#define IImage_GetOriginalHeight(This,pVal)	\
    ( (This)->lpVtbl -> GetOriginalHeight(This,pVal) ) 

#define IImage_GetOriginalWidth(This,pVal)	\
    ( (This)->lpVtbl -> GetOriginalWidth(This,pVal) ) 

#define IImage_get_AllowHillshade(This,pVal)	\
    ( (This)->lpVtbl -> get_AllowHillshade(This,pVal) ) 

#define IImage_put_AllowHillshade(This,newValue)	\
    ( (This)->lpVtbl -> put_AllowHillshade(This,newValue) ) 

#define IImage_get_SetToGrey(This,pVal)	\
    ( (This)->lpVtbl -> get_SetToGrey(This,pVal) ) 

#define IImage_put_SetToGrey(This,newValue)	\
    ( (This)->lpVtbl -> put_SetToGrey(This,newValue) ) 

#define IImage_get_UseHistogram(This,pVal)	\
    ( (This)->lpVtbl -> get_UseHistogram(This,pVal) ) 

#define IImage_put_UseHistogram(This,newValue)	\
    ( (This)->lpVtbl -> put_UseHistogram(This,newValue) ) 

#define IImage_get_HasColorTable(This,pVal)	\
    ( (This)->lpVtbl -> get_HasColorTable(This,pVal) ) 

#define IImage_get_PaletteInterpretation(This,pVal)	\
    ( (This)->lpVtbl -> get_PaletteInterpretation(This,pVal) ) 

#define IImage_get_BufferSize(This,pVal)	\
    ( (This)->lpVtbl -> get_BufferSize(This,pVal) ) 

#define IImage_put_BufferSize(This,newValue)	\
    ( (This)->lpVtbl -> put_BufferSize(This,newValue) ) 

#define IImage_get_NoBands(This,pVal)	\
    ( (This)->lpVtbl -> get_NoBands(This,pVal) ) 

#define IImage_get_ImageColorScheme(This,pVal)	\
    ( (This)->lpVtbl -> get_ImageColorScheme(This,pVal) ) 

#define IImage_put_ImageColorScheme(This,newValue)	\
    ( (This)->lpVtbl -> put_ImageColorScheme(This,newValue) ) 

#define IImage_get_DrawingMethod(This,retVal)	\
    ( (This)->lpVtbl -> get_DrawingMethod(This,retVal) ) 

#define IImage_put_DrawingMethod(This,newVal)	\
    ( (This)->lpVtbl -> put_DrawingMethod(This,newVal) ) 

#define IImage_BuildOverviews(This,ResamplingMethod,NumOverviews,OverviewList,retval)	\
    ( (This)->lpVtbl -> BuildOverviews(This,ResamplingMethod,NumOverviews,OverviewList,retval) ) 

#define IImage_get_ClearGDALCache(This,retVal)	\
    ( (This)->lpVtbl -> get_ClearGDALCache(This,retVal) ) 

#define IImage_put_ClearGDALCache(This,newVal)	\
    ( (This)->lpVtbl -> put_ClearGDALCache(This,newVal) ) 

#define IImage_get_TransparencyPercent(This,retVal)	\
    ( (This)->lpVtbl -> get_TransparencyPercent(This,retVal) ) 

#define IImage_put_TransparencyPercent(This,newVal)	\
    ( (This)->lpVtbl -> put_TransparencyPercent(This,newVal) ) 

#define IImage_get_TransparencyColor2(This,retVal)	\
    ( (This)->lpVtbl -> get_TransparencyColor2(This,retVal) ) 

#define IImage_put_TransparencyColor2(This,newVal)	\
    ( (This)->lpVtbl -> put_TransparencyColor2(This,newVal) ) 

#define IImage_get_DownsamplingMode(This,retVal)	\
    ( (This)->lpVtbl -> get_DownsamplingMode(This,retVal) ) 

#define IImage_put_DownsamplingMode(This,newVal)	\
    ( (This)->lpVtbl -> put_DownsamplingMode(This,newVal) ) 

#define IImage_get_UpsamplingMode(This,retVal)	\
    ( (This)->lpVtbl -> get_UpsamplingMode(This,retVal) ) 

#define IImage_put_UpsamplingMode(This,newVal)	\
    ( (This)->lpVtbl -> put_UpsamplingMode(This,newVal) ) 

#define IImage_get_Labels(This,pVal)	\
    ( (This)->lpVtbl -> get_Labels(This,pVal) ) 

#define IImage_put_Labels(This,newVal)	\
    ( (This)->lpVtbl -> put_Labels(This,newVal) ) 

#define IImage_get_Extents(This,pVal)	\
    ( (This)->lpVtbl -> get_Extents(This,pVal) ) 

#define IImage_ProjectionToImage(This,ProjX,ProjY,ImageX,ImageY)	\
    ( (This)->lpVtbl -> ProjectionToImage(This,ProjX,ProjY,ImageX,ImageY) ) 

#define IImage_ImageToProjection(This,ImageX,ImageY,ProjX,ProjY)	\
    ( (This)->lpVtbl -> ImageToProjection(This,ImageX,ImageY,ProjX,ProjY) ) 

#define IImage_ProjectionToBuffer(This,ProjX,ProjY,BufferX,BufferY)	\
    ( (This)->lpVtbl -> ProjectionToBuffer(This,ProjX,ProjY,BufferX,BufferY) ) 

#define IImage_BufferToProjection(This,BufferX,BufferY,ProjX,ProjY)	\
    ( (This)->lpVtbl -> BufferToProjection(This,BufferX,BufferY,ProjX,ProjY) ) 

#define IImage_get_CanUseGrouping(This,pVal)	\
    ( (This)->lpVtbl -> get_CanUseGrouping(This,pVal) ) 

#define IImage_put_CanUseGrouping(This,newVal)	\
    ( (This)->lpVtbl -> put_CanUseGrouping(This,newVal) ) 

#define IImage_get_OriginalXllCenter(This,pVal)	\
    ( (This)->lpVtbl -> get_OriginalXllCenter(This,pVal) ) 

#define IImage_put_OriginalXllCenter(This,newVal)	\
    ( (This)->lpVtbl -> put_OriginalXllCenter(This,newVal) ) 

#define IImage_get_OriginalYllCenter(This,pVal)	\
    ( (This)->lpVtbl -> get_OriginalYllCenter(This,pVal) ) 

#define IImage_put_OriginalYllCenter(This,newVal)	\
    ( (This)->lpVtbl -> put_OriginalYllCenter(This,newVal) ) 

#define IImage_get_OriginalDX(This,pVal)	\
    ( (This)->lpVtbl -> get_OriginalDX(This,pVal) ) 

#define IImage_put_OriginalDX(This,newVal)	\
    ( (This)->lpVtbl -> put_OriginalDX(This,newVal) ) 

#define IImage_get_OriginalDY(This,pVal)	\
    ( (This)->lpVtbl -> get_OriginalDY(This,pVal) ) 

#define IImage_put_OriginalDY(This,newVal)	\
    ( (This)->lpVtbl -> put_OriginalDY(This,newVal) ) 

#define IImage_GetUniqueColors(This,MaxBufferSizeMB,Colors,Frequencies,Count)	\
    ( (This)->lpVtbl -> GetUniqueColors(This,MaxBufferSizeMB,Colors,Frequencies,Count) ) 

#define IImage_SetNoDataValue(This,Value,Result)	\
    ( (This)->lpVtbl -> SetNoDataValue(This,Value,Result) ) 

#define IImage_get_NumOverviews(This,retval)	\
    ( (This)->lpVtbl -> get_NumOverviews(This,retval) ) 

#define IImage_LoadBuffer(This,maxBufferSize,retVal)	\
    ( (This)->lpVtbl -> LoadBuffer(This,maxBufferSize,retVal) ) 

#define IImage_get_SourceType(This,pVal)	\
    ( (This)->lpVtbl -> get_SourceType(This,pVal) ) 

#define IImage_Serialize(This,SerializePixels,retVal)	\
    ( (This)->lpVtbl -> Serialize(This,SerializePixels,retVal) ) 

#define IImage_Deserialize(This,newVal)	\
    ( (This)->lpVtbl -> Deserialize(This,newVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IImage_INTERFACE_DEFINED__ */


#ifndef __IShapefile_INTERFACE_DEFINED__
#define __IShapefile_INTERFACE_DEFINED__

/* interface IShapefile */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IShapefile;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("5DC72405-C39C-4755-8CFC-9876A89225BC")
    IShapefile : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumShapes( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumFields( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Extents( 
            /* [retval][out] */ IExtents **pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShapefileType( 
            /* [retval][out] */ ShpfileType *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Shape( 
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ IShape **pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_EditingShapes( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CdlgFilter( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Open( 
            /* [in] */ BSTR ShapefileName,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CreateNew( 
            /* [in] */ BSTR ShapefileName,
            /* [in] */ ShpfileType ShapefileType,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SaveAs( 
            /* [in] */ BSTR ShapefileName,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Close( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditClear( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditInsertShape( 
            /* [in] */ IShape *Shape,
            /* [out][in] */ long *ShapeIndex,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditDeleteShape( 
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SelectShapes( 
            /* [in] */ IExtents *BoundBox,
            /* [defaultvalue][optional][in] */ double Tolerance,
            /* [defaultvalue][optional][in] */ SelectMode SelectMode,
            /* [optional][out][in] */ VARIANT *Result,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE StartEditingShapes( 
            /* [defaultvalue][optional][in] */ VARIANT_BOOL StartEditTable,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE StopEditingShapes( 
            /* [defaultvalue][optional][in] */ VARIANT_BOOL ApplyChanges,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL StopEditTable,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditInsertField( 
            /* [in] */ IField *NewField,
            /* [out][in] */ long *FieldIndex,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditDeleteField( 
            /* [in] */ long FieldIndex,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditCellValue( 
            /* [in] */ long FieldIndex,
            /* [in] */ long ShapeIndex,
            /* [in] */ VARIANT NewVal,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE StartEditingTable( 
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE StopEditingTable( 
            /* [defaultvalue][optional][in] */ VARIANT_BOOL ApplyChanges,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Field( 
            /* [in] */ long FieldIndex,
            /* [retval][out] */ IField **pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CellValue( 
            /* [in] */ long FieldIndex,
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ VARIANT *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_EditingTable( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [hidden][helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FileHandle( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Filename( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE QuickPoint( 
            /* [in] */ long ShapeIndex,
            /* [in] */ long PointIndex,
            /* [retval][out] */ IPoint **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE QuickExtents( 
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ IExtents **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE QuickPoints( 
            /* [in] */ long ShapeIndex,
            /* [out][in] */ long *NumPoints,
            /* [retval][out] */ SAFEARRAY * *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PointInShape( 
            /* [in] */ LONG ShapeIndex,
            /* [in] */ DOUBLE x,
            /* [in] */ DOUBLE y,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PointInShapefile( 
            /* [in] */ DOUBLE x,
            /* [in] */ DOUBLE y,
            /* [retval][out] */ LONG *ShapeIndex) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE BeginPointInShapefile( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EndPointInShapefile( void) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Projection( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Projection( 
            /* [in] */ BSTR proj4String) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FieldByName( 
            /* [in] */ BSTR Fieldname,
            /* [retval][out] */ IField **pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumPoints( 
            /* [in] */ long Shapeindex,
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CreateNewWithShapeID( 
            /* [in] */ BSTR ShapefileName,
            /* [in] */ ShpfileType ShapefileType,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_UseSpatialIndex( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_UseSpatialIndex( 
            /* [in] */ VARIANT_BOOL pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CreateSpatialIndex( 
            /* [in] */ BSTR ShapefileName,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_HasSpatialIndex( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_HasSpatialIndex( 
            /* [in] */ VARIANT_BOOL __MIDL__IShapefile0000) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Resource( 
            /* [in] */ BSTR newShpPath,
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CacheExtents( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_CacheExtents( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE RefreshExtents( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE RefreshShapeExtents( 
            /* [in] */ LONG ShapeId,
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_UseQTree( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_UseQTree( 
            /* [defaultvalue][in] */ VARIANT_BOOL pVal = TRUE) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Save( 
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE IsSpatialIndexValid( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_SpatialIndexMaxAreaPercent( 
            /* [in] */ DOUBLE newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SpatialIndexMaxAreaPercent( 
            /* [retval][out] */ DOUBLE *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CanUseSpatialIndex( 
            /* [in] */ IExtents *pArea,
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetIntersection( 
            /* [in] */ VARIANT_BOOL SelectedOnlyOfThis,
            /* [in] */ IShapefile *sf,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [in] */ ShpfileType fileType,
            /* [defaultvalue][optional][in] */ ICallback *cBack,
            /* [retval][out] */ IShapefile **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SelectByShapefile( 
            /* [in] */ IShapefile *sf,
            /* [in] */ tkSpatialRelation Relation,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [out][in] */ VARIANT *Result,
            /* [defaultvalue][optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumSelected( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShapeSelected( 
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ShapeSelected( 
            /* [in] */ long ShapeIndex,
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SelectionDrawingOptions( 
            /* [retval][out] */ IShapeDrawingOptions **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_SelectionDrawingOptions( 
            /* [in] */ IShapeDrawingOptions *newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SelectAll( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SelectNone( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE InvertSelection( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Dissolve( 
            /* [in] */ long FieldIndex,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [retval][out] */ IShapefile **sf) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Labels( 
            /* [retval][out] */ ILabels **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Labels( 
            /* [in] */ ILabels *newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GenerateLabels( 
            /* [in] */ long FieldIndex,
            /* [in] */ tkLabelPositioning Method,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL LargestPartOnly,
            /* [retval][out] */ long *count) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clone( 
            /* [retval][out] */ IShapefile **retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_DefaultDrawingOptions( 
            /* [retval][out] */ IShapeDrawingOptions **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_DefaultDrawingOptions( 
            /* [in] */ IShapeDrawingOptions *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Categories( 
            /* [retval][out] */ IShapefileCategories **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Categories( 
            /* [in] */ IShapefileCategories *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Charts( 
            /* [retval][out] */ ICharts **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Charts( 
            /* [in] */ ICharts *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShapeCategory( 
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ShapeCategory( 
            /* [in] */ long ShapeIndex,
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Table( 
            /* [retval][out] */ ITable **retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_VisibilityExpression( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_VisibilityExpression( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FastMode( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FastMode( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MinDrawingSize( 
            /* [retval][out] */ LONG *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MinDrawingSize( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SourceType( 
            /* [retval][out] */ tkShapefileSourceType *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE BufferByDistance( 
            /* [in] */ double Distance,
            /* [in] */ LONG nSegments,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [in] */ VARIANT_BOOL MergeResults,
            /* [retval][out] */ IShapefile **sf) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GeometryEngine( 
            /* [retval][out] */ tkGeometryEngine *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GeometryEngine( 
            /* [in] */ tkGeometryEngine newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Difference( 
            /* [in] */ VARIANT_BOOL SelectedOnlySubject,
            /* [in] */ IShapefile *sfOverlay,
            /* [in] */ VARIANT_BOOL SelectedOnlyOverlay,
            /* [retval][out] */ IShapefile **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clip( 
            /* [in] */ VARIANT_BOOL SelectedOnlySubject,
            /* [in] */ IShapefile *sfOverlay,
            /* [in] */ VARIANT_BOOL SelectedOnlyOverlay,
            /* [retval][out] */ IShapefile **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SymmDifference( 
            /* [in] */ VARIANT_BOOL SelectedOnlySubject,
            /* [in] */ IShapefile *sfOverlay,
            /* [in] */ VARIANT_BOOL SelectedOnlyOverlay,
            /* [retval][out] */ IShapefile **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Union( 
            /* [in] */ VARIANT_BOOL SelectedOnlySubject,
            /* [in] */ IShapefile *sfOverlay,
            /* [in] */ VARIANT_BOOL SelectedOnlyOverlay,
            /* [retval][out] */ IShapefile **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ExplodeShapes( 
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [retval][out] */ IShapefile **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AggregateShapes( 
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [defaultvalue][optional][in] */ LONG FieldIndex,
            /* [retval][out] */ IShapefile **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ExportSelection( 
            /* [retval][out] */ IShapefile **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Sort( 
            /* [in] */ LONG FieldIndex,
            /* [in] */ VARIANT_BOOL Ascending,
            /* [retval][out] */ IShapefile **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Merge( 
            /* [in] */ VARIANT_BOOL SelectedOnlyThis,
            /* [in] */ IShapefile *sf,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [retval][out] */ IShapefile **retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SelectionColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_SelectionColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SelectionAppearance( 
            /* [retval][out] */ tkSelectionAppearance *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_SelectionAppearance( 
            /* [in] */ tkSelectionAppearance newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CollisionMode( 
            /* [retval][out] */ tkCollisionMode *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_CollisionMode( 
            /* [in] */ tkCollisionMode newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SelectionTransparency( 
            /* [retval][out] */ BYTE *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_SelectionTransparency( 
            /* [in] */ BYTE newVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_StopExecution( 
            /* [in] */ IStopExecution *stopper) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Serialize( 
            /* [in] */ VARIANT_BOOL SaveSelection,
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Deserialize( 
            /* [in] */ VARIANT_BOOL LoadSelection,
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GeoProjection( 
            /* [retval][out] */ IGeoProjection **retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GeoProjection( 
            /* [in] */ IGeoProjection *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Reproject( 
            /* [in] */ IGeoProjection *newProjection,
            /* [out][in] */ LONG *reprojectedCount,
            /* [retval][out] */ IShapefile **retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ReprojectInPlace( 
            /* [in] */ IGeoProjection *newProjection,
            /* [out][in] */ LONG *reprojectedCount,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SimplifyLines( 
            /* [in] */ DOUBLE Tolerance,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [retval][out] */ IShapefile **retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE FixUpShapes( 
            /* [out] */ IShapefile **retVal,
            /* [retval][out] */ VARIANT_BOOL *fixed) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IShapefileVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IShapefile * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IShapefile * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IShapefile * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IShapefile * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IShapefile * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IShapefile * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IShapefile * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumShapes )( 
            IShapefile * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumFields )( 
            IShapefile * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Extents )( 
            IShapefile * This,
            /* [retval][out] */ IExtents **pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShapefileType )( 
            IShapefile * This,
            /* [retval][out] */ ShpfileType *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Shape )( 
            IShapefile * This,
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ IShape **pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_EditingShapes )( 
            IShapefile * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IShapefile * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CdlgFilter )( 
            IShapefile * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IShapefile * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IShapefile * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IShapefile * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IShapefile * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Open )( 
            IShapefile * This,
            /* [in] */ BSTR ShapefileName,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CreateNew )( 
            IShapefile * This,
            /* [in] */ BSTR ShapefileName,
            /* [in] */ ShpfileType ShapefileType,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SaveAs )( 
            IShapefile * This,
            /* [in] */ BSTR ShapefileName,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Close )( 
            IShapefile * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditClear )( 
            IShapefile * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditInsertShape )( 
            IShapefile * This,
            /* [in] */ IShape *Shape,
            /* [out][in] */ long *ShapeIndex,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditDeleteShape )( 
            IShapefile * This,
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SelectShapes )( 
            IShapefile * This,
            /* [in] */ IExtents *BoundBox,
            /* [defaultvalue][optional][in] */ double Tolerance,
            /* [defaultvalue][optional][in] */ SelectMode SelectMode,
            /* [optional][out][in] */ VARIANT *Result,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *StartEditingShapes )( 
            IShapefile * This,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL StartEditTable,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *StopEditingShapes )( 
            IShapefile * This,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL ApplyChanges,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL StopEditTable,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditInsertField )( 
            IShapefile * This,
            /* [in] */ IField *NewField,
            /* [out][in] */ long *FieldIndex,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditDeleteField )( 
            IShapefile * This,
            /* [in] */ long FieldIndex,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditCellValue )( 
            IShapefile * This,
            /* [in] */ long FieldIndex,
            /* [in] */ long ShapeIndex,
            /* [in] */ VARIANT NewVal,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *StartEditingTable )( 
            IShapefile * This,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *StopEditingTable )( 
            IShapefile * This,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL ApplyChanges,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Field )( 
            IShapefile * This,
            /* [in] */ long FieldIndex,
            /* [retval][out] */ IField **pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CellValue )( 
            IShapefile * This,
            /* [in] */ long FieldIndex,
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ VARIANT *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_EditingTable )( 
            IShapefile * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IShapefile * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [hidden][helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FileHandle )( 
            IShapefile * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Filename )( 
            IShapefile * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *QuickPoint )( 
            IShapefile * This,
            /* [in] */ long ShapeIndex,
            /* [in] */ long PointIndex,
            /* [retval][out] */ IPoint **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *QuickExtents )( 
            IShapefile * This,
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ IExtents **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *QuickPoints )( 
            IShapefile * This,
            /* [in] */ long ShapeIndex,
            /* [out][in] */ long *NumPoints,
            /* [retval][out] */ SAFEARRAY * *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PointInShape )( 
            IShapefile * This,
            /* [in] */ LONG ShapeIndex,
            /* [in] */ DOUBLE x,
            /* [in] */ DOUBLE y,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PointInShapefile )( 
            IShapefile * This,
            /* [in] */ DOUBLE x,
            /* [in] */ DOUBLE y,
            /* [retval][out] */ LONG *ShapeIndex);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *BeginPointInShapefile )( 
            IShapefile * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EndPointInShapefile )( 
            IShapefile * This);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Projection )( 
            IShapefile * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Projection )( 
            IShapefile * This,
            /* [in] */ BSTR proj4String);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FieldByName )( 
            IShapefile * This,
            /* [in] */ BSTR Fieldname,
            /* [retval][out] */ IField **pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumPoints )( 
            IShapefile * This,
            /* [in] */ long Shapeindex,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CreateNewWithShapeID )( 
            IShapefile * This,
            /* [in] */ BSTR ShapefileName,
            /* [in] */ ShpfileType ShapefileType,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_UseSpatialIndex )( 
            IShapefile * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_UseSpatialIndex )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CreateSpatialIndex )( 
            IShapefile * This,
            /* [in] */ BSTR ShapefileName,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_HasSpatialIndex )( 
            IShapefile * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_HasSpatialIndex )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL __MIDL__IShapefile0000);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Resource )( 
            IShapefile * This,
            /* [in] */ BSTR newShpPath,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CacheExtents )( 
            IShapefile * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_CacheExtents )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *RefreshExtents )( 
            IShapefile * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *RefreshShapeExtents )( 
            IShapefile * This,
            /* [in] */ LONG ShapeId,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_UseQTree )( 
            IShapefile * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_UseQTree )( 
            IShapefile * This,
            /* [defaultvalue][in] */ VARIANT_BOOL pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Save )( 
            IShapefile * This,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *IsSpatialIndexValid )( 
            IShapefile * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_SpatialIndexMaxAreaPercent )( 
            IShapefile * This,
            /* [in] */ DOUBLE newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SpatialIndexMaxAreaPercent )( 
            IShapefile * This,
            /* [retval][out] */ DOUBLE *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CanUseSpatialIndex )( 
            IShapefile * This,
            /* [in] */ IExtents *pArea,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetIntersection )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL SelectedOnlyOfThis,
            /* [in] */ IShapefile *sf,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [in] */ ShpfileType fileType,
            /* [defaultvalue][optional][in] */ ICallback *cBack,
            /* [retval][out] */ IShapefile **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SelectByShapefile )( 
            IShapefile * This,
            /* [in] */ IShapefile *sf,
            /* [in] */ tkSpatialRelation Relation,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [out][in] */ VARIANT *Result,
            /* [defaultvalue][optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumSelected )( 
            IShapefile * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShapeSelected )( 
            IShapefile * This,
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ShapeSelected )( 
            IShapefile * This,
            /* [in] */ long ShapeIndex,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SelectionDrawingOptions )( 
            IShapefile * This,
            /* [retval][out] */ IShapeDrawingOptions **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_SelectionDrawingOptions )( 
            IShapefile * This,
            /* [in] */ IShapeDrawingOptions *newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SelectAll )( 
            IShapefile * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SelectNone )( 
            IShapefile * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *InvertSelection )( 
            IShapefile * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Dissolve )( 
            IShapefile * This,
            /* [in] */ long FieldIndex,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [retval][out] */ IShapefile **sf);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Labels )( 
            IShapefile * This,
            /* [retval][out] */ ILabels **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Labels )( 
            IShapefile * This,
            /* [in] */ ILabels *newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GenerateLabels )( 
            IShapefile * This,
            /* [in] */ long FieldIndex,
            /* [in] */ tkLabelPositioning Method,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL LargestPartOnly,
            /* [retval][out] */ long *count);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clone )( 
            IShapefile * This,
            /* [retval][out] */ IShapefile **retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DefaultDrawingOptions )( 
            IShapefile * This,
            /* [retval][out] */ IShapeDrawingOptions **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DefaultDrawingOptions )( 
            IShapefile * This,
            /* [in] */ IShapeDrawingOptions *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Categories )( 
            IShapefile * This,
            /* [retval][out] */ IShapefileCategories **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Categories )( 
            IShapefile * This,
            /* [in] */ IShapefileCategories *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Charts )( 
            IShapefile * This,
            /* [retval][out] */ ICharts **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Charts )( 
            IShapefile * This,
            /* [in] */ ICharts *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShapeCategory )( 
            IShapefile * This,
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ShapeCategory )( 
            IShapefile * This,
            /* [in] */ long ShapeIndex,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Table )( 
            IShapefile * This,
            /* [retval][out] */ ITable **retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_VisibilityExpression )( 
            IShapefile * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_VisibilityExpression )( 
            IShapefile * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FastMode )( 
            IShapefile * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FastMode )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MinDrawingSize )( 
            IShapefile * This,
            /* [retval][out] */ LONG *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MinDrawingSize )( 
            IShapefile * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SourceType )( 
            IShapefile * This,
            /* [retval][out] */ tkShapefileSourceType *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *BufferByDistance )( 
            IShapefile * This,
            /* [in] */ double Distance,
            /* [in] */ LONG nSegments,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [in] */ VARIANT_BOOL MergeResults,
            /* [retval][out] */ IShapefile **sf);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GeometryEngine )( 
            IShapefile * This,
            /* [retval][out] */ tkGeometryEngine *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GeometryEngine )( 
            IShapefile * This,
            /* [in] */ tkGeometryEngine newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Difference )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL SelectedOnlySubject,
            /* [in] */ IShapefile *sfOverlay,
            /* [in] */ VARIANT_BOOL SelectedOnlyOverlay,
            /* [retval][out] */ IShapefile **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clip )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL SelectedOnlySubject,
            /* [in] */ IShapefile *sfOverlay,
            /* [in] */ VARIANT_BOOL SelectedOnlyOverlay,
            /* [retval][out] */ IShapefile **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SymmDifference )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL SelectedOnlySubject,
            /* [in] */ IShapefile *sfOverlay,
            /* [in] */ VARIANT_BOOL SelectedOnlyOverlay,
            /* [retval][out] */ IShapefile **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Union )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL SelectedOnlySubject,
            /* [in] */ IShapefile *sfOverlay,
            /* [in] */ VARIANT_BOOL SelectedOnlyOverlay,
            /* [retval][out] */ IShapefile **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ExplodeShapes )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [retval][out] */ IShapefile **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AggregateShapes )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [defaultvalue][optional][in] */ LONG FieldIndex,
            /* [retval][out] */ IShapefile **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ExportSelection )( 
            IShapefile * This,
            /* [retval][out] */ IShapefile **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Sort )( 
            IShapefile * This,
            /* [in] */ LONG FieldIndex,
            /* [in] */ VARIANT_BOOL Ascending,
            /* [retval][out] */ IShapefile **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Merge )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL SelectedOnlyThis,
            /* [in] */ IShapefile *sf,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [retval][out] */ IShapefile **retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SelectionColor )( 
            IShapefile * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_SelectionColor )( 
            IShapefile * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SelectionAppearance )( 
            IShapefile * This,
            /* [retval][out] */ tkSelectionAppearance *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_SelectionAppearance )( 
            IShapefile * This,
            /* [in] */ tkSelectionAppearance newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CollisionMode )( 
            IShapefile * This,
            /* [retval][out] */ tkCollisionMode *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_CollisionMode )( 
            IShapefile * This,
            /* [in] */ tkCollisionMode newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SelectionTransparency )( 
            IShapefile * This,
            /* [retval][out] */ BYTE *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_SelectionTransparency )( 
            IShapefile * This,
            /* [in] */ BYTE newVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_StopExecution )( 
            IShapefile * This,
            /* [in] */ IStopExecution *stopper);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Serialize )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL SaveSelection,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Deserialize )( 
            IShapefile * This,
            /* [in] */ VARIANT_BOOL LoadSelection,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GeoProjection )( 
            IShapefile * This,
            /* [retval][out] */ IGeoProjection **retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GeoProjection )( 
            IShapefile * This,
            /* [in] */ IGeoProjection *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Reproject )( 
            IShapefile * This,
            /* [in] */ IGeoProjection *newProjection,
            /* [out][in] */ LONG *reprojectedCount,
            /* [retval][out] */ IShapefile **retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ReprojectInPlace )( 
            IShapefile * This,
            /* [in] */ IGeoProjection *newProjection,
            /* [out][in] */ LONG *reprojectedCount,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SimplifyLines )( 
            IShapefile * This,
            /* [in] */ DOUBLE Tolerance,
            /* [in] */ VARIANT_BOOL SelectedOnly,
            /* [retval][out] */ IShapefile **retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *FixUpShapes )( 
            IShapefile * This,
            /* [out] */ IShapefile **retVal,
            /* [retval][out] */ VARIANT_BOOL *fixed);
        
        END_INTERFACE
    } IShapefileVtbl;

    interface IShapefile
    {
        CONST_VTBL struct IShapefileVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IShapefile_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IShapefile_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IShapefile_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IShapefile_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IShapefile_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IShapefile_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IShapefile_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IShapefile_get_NumShapes(This,pVal)	\
    ( (This)->lpVtbl -> get_NumShapes(This,pVal) ) 

#define IShapefile_get_NumFields(This,pVal)	\
    ( (This)->lpVtbl -> get_NumFields(This,pVal) ) 

#define IShapefile_get_Extents(This,pVal)	\
    ( (This)->lpVtbl -> get_Extents(This,pVal) ) 

#define IShapefile_get_ShapefileType(This,pVal)	\
    ( (This)->lpVtbl -> get_ShapefileType(This,pVal) ) 

#define IShapefile_get_Shape(This,ShapeIndex,pVal)	\
    ( (This)->lpVtbl -> get_Shape(This,ShapeIndex,pVal) ) 

#define IShapefile_get_EditingShapes(This,pVal)	\
    ( (This)->lpVtbl -> get_EditingShapes(This,pVal) ) 

#define IShapefile_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IShapefile_get_CdlgFilter(This,pVal)	\
    ( (This)->lpVtbl -> get_CdlgFilter(This,pVal) ) 

#define IShapefile_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IShapefile_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IShapefile_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IShapefile_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IShapefile_Open(This,ShapefileName,cBack,retval)	\
    ( (This)->lpVtbl -> Open(This,ShapefileName,cBack,retval) ) 

#define IShapefile_CreateNew(This,ShapefileName,ShapefileType,retval)	\
    ( (This)->lpVtbl -> CreateNew(This,ShapefileName,ShapefileType,retval) ) 

#define IShapefile_SaveAs(This,ShapefileName,cBack,retval)	\
    ( (This)->lpVtbl -> SaveAs(This,ShapefileName,cBack,retval) ) 

#define IShapefile_Close(This,retval)	\
    ( (This)->lpVtbl -> Close(This,retval) ) 

#define IShapefile_EditClear(This,retval)	\
    ( (This)->lpVtbl -> EditClear(This,retval) ) 

#define IShapefile_EditInsertShape(This,Shape,ShapeIndex,retval)	\
    ( (This)->lpVtbl -> EditInsertShape(This,Shape,ShapeIndex,retval) ) 

#define IShapefile_EditDeleteShape(This,ShapeIndex,retval)	\
    ( (This)->lpVtbl -> EditDeleteShape(This,ShapeIndex,retval) ) 

#define IShapefile_SelectShapes(This,BoundBox,Tolerance,SelectMode,Result,retval)	\
    ( (This)->lpVtbl -> SelectShapes(This,BoundBox,Tolerance,SelectMode,Result,retval) ) 

#define IShapefile_StartEditingShapes(This,StartEditTable,cBack,retval)	\
    ( (This)->lpVtbl -> StartEditingShapes(This,StartEditTable,cBack,retval) ) 

#define IShapefile_StopEditingShapes(This,ApplyChanges,StopEditTable,cBack,retval)	\
    ( (This)->lpVtbl -> StopEditingShapes(This,ApplyChanges,StopEditTable,cBack,retval) ) 

#define IShapefile_EditInsertField(This,NewField,FieldIndex,cBack,retval)	\
    ( (This)->lpVtbl -> EditInsertField(This,NewField,FieldIndex,cBack,retval) ) 

#define IShapefile_EditDeleteField(This,FieldIndex,cBack,retval)	\
    ( (This)->lpVtbl -> EditDeleteField(This,FieldIndex,cBack,retval) ) 

#define IShapefile_EditCellValue(This,FieldIndex,ShapeIndex,NewVal,retval)	\
    ( (This)->lpVtbl -> EditCellValue(This,FieldIndex,ShapeIndex,NewVal,retval) ) 

#define IShapefile_StartEditingTable(This,cBack,retval)	\
    ( (This)->lpVtbl -> StartEditingTable(This,cBack,retval) ) 

#define IShapefile_StopEditingTable(This,ApplyChanges,cBack,retval)	\
    ( (This)->lpVtbl -> StopEditingTable(This,ApplyChanges,cBack,retval) ) 

#define IShapefile_get_Field(This,FieldIndex,pVal)	\
    ( (This)->lpVtbl -> get_Field(This,FieldIndex,pVal) ) 

#define IShapefile_get_CellValue(This,FieldIndex,ShapeIndex,pVal)	\
    ( (This)->lpVtbl -> get_CellValue(This,FieldIndex,ShapeIndex,pVal) ) 

#define IShapefile_get_EditingTable(This,pVal)	\
    ( (This)->lpVtbl -> get_EditingTable(This,pVal) ) 

#define IShapefile_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IShapefile_get_FileHandle(This,pVal)	\
    ( (This)->lpVtbl -> get_FileHandle(This,pVal) ) 

#define IShapefile_get_Filename(This,pVal)	\
    ( (This)->lpVtbl -> get_Filename(This,pVal) ) 

#define IShapefile_QuickPoint(This,ShapeIndex,PointIndex,retval)	\
    ( (This)->lpVtbl -> QuickPoint(This,ShapeIndex,PointIndex,retval) ) 

#define IShapefile_QuickExtents(This,ShapeIndex,retval)	\
    ( (This)->lpVtbl -> QuickExtents(This,ShapeIndex,retval) ) 

#define IShapefile_QuickPoints(This,ShapeIndex,NumPoints,retval)	\
    ( (This)->lpVtbl -> QuickPoints(This,ShapeIndex,NumPoints,retval) ) 

#define IShapefile_PointInShape(This,ShapeIndex,x,y,retval)	\
    ( (This)->lpVtbl -> PointInShape(This,ShapeIndex,x,y,retval) ) 

#define IShapefile_PointInShapefile(This,x,y,ShapeIndex)	\
    ( (This)->lpVtbl -> PointInShapefile(This,x,y,ShapeIndex) ) 

#define IShapefile_BeginPointInShapefile(This,retval)	\
    ( (This)->lpVtbl -> BeginPointInShapefile(This,retval) ) 

#define IShapefile_EndPointInShapefile(This)	\
    ( (This)->lpVtbl -> EndPointInShapefile(This) ) 

#define IShapefile_get_Projection(This,pVal)	\
    ( (This)->lpVtbl -> get_Projection(This,pVal) ) 

#define IShapefile_put_Projection(This,proj4String)	\
    ( (This)->lpVtbl -> put_Projection(This,proj4String) ) 

#define IShapefile_get_FieldByName(This,Fieldname,pVal)	\
    ( (This)->lpVtbl -> get_FieldByName(This,Fieldname,pVal) ) 

#define IShapefile_get_NumPoints(This,Shapeindex,pVal)	\
    ( (This)->lpVtbl -> get_NumPoints(This,Shapeindex,pVal) ) 

#define IShapefile_CreateNewWithShapeID(This,ShapefileName,ShapefileType,retval)	\
    ( (This)->lpVtbl -> CreateNewWithShapeID(This,ShapefileName,ShapefileType,retval) ) 

#define IShapefile_get_UseSpatialIndex(This,pVal)	\
    ( (This)->lpVtbl -> get_UseSpatialIndex(This,pVal) ) 

#define IShapefile_put_UseSpatialIndex(This,pVal)	\
    ( (This)->lpVtbl -> put_UseSpatialIndex(This,pVal) ) 

#define IShapefile_CreateSpatialIndex(This,ShapefileName,retval)	\
    ( (This)->lpVtbl -> CreateSpatialIndex(This,ShapefileName,retval) ) 

#define IShapefile_get_HasSpatialIndex(This,pVal)	\
    ( (This)->lpVtbl -> get_HasSpatialIndex(This,pVal) ) 

#define IShapefile_put_HasSpatialIndex(This,__MIDL__IShapefile0000)	\
    ( (This)->lpVtbl -> put_HasSpatialIndex(This,__MIDL__IShapefile0000) ) 

#define IShapefile_Resource(This,newShpPath,pVal)	\
    ( (This)->lpVtbl -> Resource(This,newShpPath,pVal) ) 

#define IShapefile_get_CacheExtents(This,pVal)	\
    ( (This)->lpVtbl -> get_CacheExtents(This,pVal) ) 

#define IShapefile_put_CacheExtents(This,newVal)	\
    ( (This)->lpVtbl -> put_CacheExtents(This,newVal) ) 

#define IShapefile_RefreshExtents(This,pVal)	\
    ( (This)->lpVtbl -> RefreshExtents(This,pVal) ) 

#define IShapefile_RefreshShapeExtents(This,ShapeId,pVal)	\
    ( (This)->lpVtbl -> RefreshShapeExtents(This,ShapeId,pVal) ) 

#define IShapefile_get_UseQTree(This,pVal)	\
    ( (This)->lpVtbl -> get_UseQTree(This,pVal) ) 

#define IShapefile_put_UseQTree(This,pVal)	\
    ( (This)->lpVtbl -> put_UseQTree(This,pVal) ) 

#define IShapefile_Save(This,cBack,retval)	\
    ( (This)->lpVtbl -> Save(This,cBack,retval) ) 

#define IShapefile_IsSpatialIndexValid(This,pVal)	\
    ( (This)->lpVtbl -> IsSpatialIndexValid(This,pVal) ) 

#define IShapefile_put_SpatialIndexMaxAreaPercent(This,newVal)	\
    ( (This)->lpVtbl -> put_SpatialIndexMaxAreaPercent(This,newVal) ) 

#define IShapefile_get_SpatialIndexMaxAreaPercent(This,pVal)	\
    ( (This)->lpVtbl -> get_SpatialIndexMaxAreaPercent(This,pVal) ) 

#define IShapefile_get_CanUseSpatialIndex(This,pArea,pVal)	\
    ( (This)->lpVtbl -> get_CanUseSpatialIndex(This,pArea,pVal) ) 

#define IShapefile_GetIntersection(This,SelectedOnlyOfThis,sf,SelectedOnly,fileType,cBack,retval)	\
    ( (This)->lpVtbl -> GetIntersection(This,SelectedOnlyOfThis,sf,SelectedOnly,fileType,cBack,retval) ) 

#define IShapefile_SelectByShapefile(This,sf,Relation,SelectedOnly,Result,cBack,retval)	\
    ( (This)->lpVtbl -> SelectByShapefile(This,sf,Relation,SelectedOnly,Result,cBack,retval) ) 

#define IShapefile_get_NumSelected(This,pVal)	\
    ( (This)->lpVtbl -> get_NumSelected(This,pVal) ) 

#define IShapefile_get_ShapeSelected(This,ShapeIndex,pVal)	\
    ( (This)->lpVtbl -> get_ShapeSelected(This,ShapeIndex,pVal) ) 

#define IShapefile_put_ShapeSelected(This,ShapeIndex,newVal)	\
    ( (This)->lpVtbl -> put_ShapeSelected(This,ShapeIndex,newVal) ) 

#define IShapefile_get_SelectionDrawingOptions(This,pVal)	\
    ( (This)->lpVtbl -> get_SelectionDrawingOptions(This,pVal) ) 

#define IShapefile_put_SelectionDrawingOptions(This,newVal)	\
    ( (This)->lpVtbl -> put_SelectionDrawingOptions(This,newVal) ) 

#define IShapefile_SelectAll(This)	\
    ( (This)->lpVtbl -> SelectAll(This) ) 

#define IShapefile_SelectNone(This)	\
    ( (This)->lpVtbl -> SelectNone(This) ) 

#define IShapefile_InvertSelection(This)	\
    ( (This)->lpVtbl -> InvertSelection(This) ) 

#define IShapefile_Dissolve(This,FieldIndex,SelectedOnly,sf)	\
    ( (This)->lpVtbl -> Dissolve(This,FieldIndex,SelectedOnly,sf) ) 

#define IShapefile_get_Labels(This,pVal)	\
    ( (This)->lpVtbl -> get_Labels(This,pVal) ) 

#define IShapefile_put_Labels(This,newVal)	\
    ( (This)->lpVtbl -> put_Labels(This,newVal) ) 

#define IShapefile_GenerateLabels(This,FieldIndex,Method,LargestPartOnly,count)	\
    ( (This)->lpVtbl -> GenerateLabels(This,FieldIndex,Method,LargestPartOnly,count) ) 

#define IShapefile_Clone(This,retVal)	\
    ( (This)->lpVtbl -> Clone(This,retVal) ) 

#define IShapefile_get_DefaultDrawingOptions(This,pVal)	\
    ( (This)->lpVtbl -> get_DefaultDrawingOptions(This,pVal) ) 

#define IShapefile_put_DefaultDrawingOptions(This,newVal)	\
    ( (This)->lpVtbl -> put_DefaultDrawingOptions(This,newVal) ) 

#define IShapefile_get_Categories(This,pVal)	\
    ( (This)->lpVtbl -> get_Categories(This,pVal) ) 

#define IShapefile_put_Categories(This,newVal)	\
    ( (This)->lpVtbl -> put_Categories(This,newVal) ) 

#define IShapefile_get_Charts(This,pVal)	\
    ( (This)->lpVtbl -> get_Charts(This,pVal) ) 

#define IShapefile_put_Charts(This,newVal)	\
    ( (This)->lpVtbl -> put_Charts(This,newVal) ) 

#define IShapefile_get_ShapeCategory(This,ShapeIndex,pVal)	\
    ( (This)->lpVtbl -> get_ShapeCategory(This,ShapeIndex,pVal) ) 

#define IShapefile_put_ShapeCategory(This,ShapeIndex,newVal)	\
    ( (This)->lpVtbl -> put_ShapeCategory(This,ShapeIndex,newVal) ) 

#define IShapefile_get_Table(This,retVal)	\
    ( (This)->lpVtbl -> get_Table(This,retVal) ) 

#define IShapefile_get_VisibilityExpression(This,pVal)	\
    ( (This)->lpVtbl -> get_VisibilityExpression(This,pVal) ) 

#define IShapefile_put_VisibilityExpression(This,newVal)	\
    ( (This)->lpVtbl -> put_VisibilityExpression(This,newVal) ) 

#define IShapefile_get_FastMode(This,pVal)	\
    ( (This)->lpVtbl -> get_FastMode(This,pVal) ) 

#define IShapefile_put_FastMode(This,newVal)	\
    ( (This)->lpVtbl -> put_FastMode(This,newVal) ) 

#define IShapefile_get_MinDrawingSize(This,pVal)	\
    ( (This)->lpVtbl -> get_MinDrawingSize(This,pVal) ) 

#define IShapefile_put_MinDrawingSize(This,newVal)	\
    ( (This)->lpVtbl -> put_MinDrawingSize(This,newVal) ) 

#define IShapefile_get_SourceType(This,pVal)	\
    ( (This)->lpVtbl -> get_SourceType(This,pVal) ) 

#define IShapefile_BufferByDistance(This,Distance,nSegments,SelectedOnly,MergeResults,sf)	\
    ( (This)->lpVtbl -> BufferByDistance(This,Distance,nSegments,SelectedOnly,MergeResults,sf) ) 

#define IShapefile_get_GeometryEngine(This,pVal)	\
    ( (This)->lpVtbl -> get_GeometryEngine(This,pVal) ) 

#define IShapefile_put_GeometryEngine(This,newVal)	\
    ( (This)->lpVtbl -> put_GeometryEngine(This,newVal) ) 

#define IShapefile_Difference(This,SelectedOnlySubject,sfOverlay,SelectedOnlyOverlay,retval)	\
    ( (This)->lpVtbl -> Difference(This,SelectedOnlySubject,sfOverlay,SelectedOnlyOverlay,retval) ) 

#define IShapefile_Clip(This,SelectedOnlySubject,sfOverlay,SelectedOnlyOverlay,retval)	\
    ( (This)->lpVtbl -> Clip(This,SelectedOnlySubject,sfOverlay,SelectedOnlyOverlay,retval) ) 

#define IShapefile_SymmDifference(This,SelectedOnlySubject,sfOverlay,SelectedOnlyOverlay,retval)	\
    ( (This)->lpVtbl -> SymmDifference(This,SelectedOnlySubject,sfOverlay,SelectedOnlyOverlay,retval) ) 

#define IShapefile_Union(This,SelectedOnlySubject,sfOverlay,SelectedOnlyOverlay,retval)	\
    ( (This)->lpVtbl -> Union(This,SelectedOnlySubject,sfOverlay,SelectedOnlyOverlay,retval) ) 

#define IShapefile_ExplodeShapes(This,SelectedOnly,retval)	\
    ( (This)->lpVtbl -> ExplodeShapes(This,SelectedOnly,retval) ) 

#define IShapefile_AggregateShapes(This,SelectedOnly,FieldIndex,retval)	\
    ( (This)->lpVtbl -> AggregateShapes(This,SelectedOnly,FieldIndex,retval) ) 

#define IShapefile_ExportSelection(This,retval)	\
    ( (This)->lpVtbl -> ExportSelection(This,retval) ) 

#define IShapefile_Sort(This,FieldIndex,Ascending,retval)	\
    ( (This)->lpVtbl -> Sort(This,FieldIndex,Ascending,retval) ) 

#define IShapefile_Merge(This,SelectedOnlyThis,sf,SelectedOnly,retval)	\
    ( (This)->lpVtbl -> Merge(This,SelectedOnlyThis,sf,SelectedOnly,retval) ) 

#define IShapefile_get_SelectionColor(This,retval)	\
    ( (This)->lpVtbl -> get_SelectionColor(This,retval) ) 

#define IShapefile_put_SelectionColor(This,newVal)	\
    ( (This)->lpVtbl -> put_SelectionColor(This,newVal) ) 

#define IShapefile_get_SelectionAppearance(This,retval)	\
    ( (This)->lpVtbl -> get_SelectionAppearance(This,retval) ) 

#define IShapefile_put_SelectionAppearance(This,newVal)	\
    ( (This)->lpVtbl -> put_SelectionAppearance(This,newVal) ) 

#define IShapefile_get_CollisionMode(This,retval)	\
    ( (This)->lpVtbl -> get_CollisionMode(This,retval) ) 

#define IShapefile_put_CollisionMode(This,newVal)	\
    ( (This)->lpVtbl -> put_CollisionMode(This,newVal) ) 

#define IShapefile_get_SelectionTransparency(This,retval)	\
    ( (This)->lpVtbl -> get_SelectionTransparency(This,retval) ) 

#define IShapefile_put_SelectionTransparency(This,newVal)	\
    ( (This)->lpVtbl -> put_SelectionTransparency(This,newVal) ) 

#define IShapefile_put_StopExecution(This,stopper)	\
    ( (This)->lpVtbl -> put_StopExecution(This,stopper) ) 

#define IShapefile_Serialize(This,SaveSelection,retVal)	\
    ( (This)->lpVtbl -> Serialize(This,SaveSelection,retVal) ) 

#define IShapefile_Deserialize(This,LoadSelection,newVal)	\
    ( (This)->lpVtbl -> Deserialize(This,LoadSelection,newVal) ) 

#define IShapefile_get_GeoProjection(This,retVal)	\
    ( (This)->lpVtbl -> get_GeoProjection(This,retVal) ) 

#define IShapefile_put_GeoProjection(This,pVal)	\
    ( (This)->lpVtbl -> put_GeoProjection(This,pVal) ) 

#define IShapefile_Reproject(This,newProjection,reprojectedCount,retVal)	\
    ( (This)->lpVtbl -> Reproject(This,newProjection,reprojectedCount,retVal) ) 

#define IShapefile_ReprojectInPlace(This,newProjection,reprojectedCount,retVal)	\
    ( (This)->lpVtbl -> ReprojectInPlace(This,newProjection,reprojectedCount,retVal) ) 

#define IShapefile_SimplifyLines(This,Tolerance,SelectedOnly,retVal)	\
    ( (This)->lpVtbl -> SimplifyLines(This,Tolerance,SelectedOnly,retVal) ) 

#define IShapefile_FixUpShapes(This,retVal,fixed)	\
    ( (This)->lpVtbl -> FixUpShapes(This,retVal,fixed) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE IShapefile_FixUpShapes_Proxy( 
    IShapefile * This,
    /* [out] */ IShapefile **retVal,
    /* [retval][out] */ VARIANT_BOOL *fixed);


void __RPC_STUB IShapefile_FixUpShapes_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IShapefile_INTERFACE_DEFINED__ */


#ifndef __IShape_INTERFACE_DEFINED__
#define __IShape_INTERFACE_DEFINED__

/* interface IShape */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IShape;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("5FA550E3-2044-4034-BAAA-B4CCE90A0C41")
    IShape : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumPoints( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumParts( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShapeType( 
            /* [retval][out] */ ShpfileType *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ShapeType( 
            /* [in] */ ShpfileType newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Point( 
            /* [in] */ long PointIndex,
            /* [retval][out] */ IPoint **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Point( 
            /* [in] */ long PointIndex,
            /* [in] */ IPoint *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Part( 
            /* [in] */ long PartIndex,
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Part( 
            /* [in] */ long PartIndex,
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Create( 
            /* [in] */ ShpfileType ShpType,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE InsertPoint( 
            /* [in] */ IPoint *NewPoint,
            /* [out][in] */ long *PointIndex,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DeletePoint( 
            /* [in] */ long PointIndex,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE InsertPart( 
            /* [in] */ long PointIndex,
            /* [out][in] */ long *PartIndex,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DeletePart( 
            /* [in] */ long PartIndex,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Extents( 
            /* [retval][out] */ IExtents **pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SerializeToString( 
            /* [retval][out] */ BSTR *Serialized) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CreateFromString( 
            /* [in] */ BSTR Serialized,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PointInThisPoly( 
            /* [in] */ IPoint *pt,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Centroid( 
            /* [retval][out] */ IPoint **pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Length( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Perimeter( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Area( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Relates( 
            /* [in] */ IShape *Shape,
            /* [in] */ tkSpatialRelation Relation,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Distance( 
            /* [in] */ IShape *Shape,
            /* [retval][out] */ DOUBLE *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Buffer( 
            /* [in] */ DOUBLE Distance,
            /* [in] */ long nQuadSegments,
            /* [retval][out] */ IShape **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clip( 
            /* [in] */ IShape *Shape,
            /* [in] */ tkClipOperation Operation,
            /* [retval][out] */ IShape **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Contains( 
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Crosses( 
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Disjoint( 
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Equals( 
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Intersects( 
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Overlaps( 
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Touches( 
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Within( 
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Boundry( 
            /* [retval][out] */ IShape **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ConvexHull( 
            /* [retval][out] */ IShape **retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IsValid( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_XY( 
            /* [in] */ long PointIndex,
            /* [out][in] */ double *x,
            /* [out][in] */ double *y,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PartIsClockWise( 
            /* [in] */ long PartIndex,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ReversePointsOrder( 
            /* [in] */ long PartIndex,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetIntersection( 
            /* [in] */ IShape *Shape,
            /* [out][out] */ VARIANT *Results,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Center( 
            /* [retval][out] */ IPoint **pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_EndOfPart( 
            /* [in] */ long PartIndex,
            /* [retval][out] */ long *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PartAsShape( 
            /* [in] */ long PartIndex,
            /* [retval][out] */ IShape **retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IsValidReason( 
            /* [retval][out] */ BSTR *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_InteriorPoint( 
            /* [retval][out] */ IPoint **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clone( 
            /* [retval][out] */ IShape **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Explode( 
            /* [out][in] */ VARIANT *Results,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE put_XY( 
            /* [in] */ LONG pointIndex,
            /* [in] */ DOUBLE x,
            /* [in] */ DOUBLE y,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ExportToBinary( 
            /* [out][in] */ VARIANT *bytesArray,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ImportFromBinary( 
            /* [in] */ VARIANT bytesArray,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE FixUp( 
            /* [out] */ IShape **retval) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IShapeVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IShape * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IShape * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IShape * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IShape * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IShape * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IShape * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IShape * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumPoints )( 
            IShape * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumParts )( 
            IShape * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShapeType )( 
            IShape * This,
            /* [retval][out] */ ShpfileType *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ShapeType )( 
            IShape * This,
            /* [in] */ ShpfileType newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Point )( 
            IShape * This,
            /* [in] */ long PointIndex,
            /* [retval][out] */ IPoint **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Point )( 
            IShape * This,
            /* [in] */ long PointIndex,
            /* [in] */ IPoint *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Part )( 
            IShape * This,
            /* [in] */ long PartIndex,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Part )( 
            IShape * This,
            /* [in] */ long PartIndex,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IShape * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IShape * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IShape * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IShape * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IShape * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IShape * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Create )( 
            IShape * This,
            /* [in] */ ShpfileType ShpType,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *InsertPoint )( 
            IShape * This,
            /* [in] */ IPoint *NewPoint,
            /* [out][in] */ long *PointIndex,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DeletePoint )( 
            IShape * This,
            /* [in] */ long PointIndex,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *InsertPart )( 
            IShape * This,
            /* [in] */ long PointIndex,
            /* [out][in] */ long *PartIndex,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DeletePart )( 
            IShape * This,
            /* [in] */ long PartIndex,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Extents )( 
            IShape * This,
            /* [retval][out] */ IExtents **pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SerializeToString )( 
            IShape * This,
            /* [retval][out] */ BSTR *Serialized);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CreateFromString )( 
            IShape * This,
            /* [in] */ BSTR Serialized,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PointInThisPoly )( 
            IShape * This,
            /* [in] */ IPoint *pt,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Centroid )( 
            IShape * This,
            /* [retval][out] */ IPoint **pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Length )( 
            IShape * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Perimeter )( 
            IShape * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Area )( 
            IShape * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Relates )( 
            IShape * This,
            /* [in] */ IShape *Shape,
            /* [in] */ tkSpatialRelation Relation,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Distance )( 
            IShape * This,
            /* [in] */ IShape *Shape,
            /* [retval][out] */ DOUBLE *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Buffer )( 
            IShape * This,
            /* [in] */ DOUBLE Distance,
            /* [in] */ long nQuadSegments,
            /* [retval][out] */ IShape **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clip )( 
            IShape * This,
            /* [in] */ IShape *Shape,
            /* [in] */ tkClipOperation Operation,
            /* [retval][out] */ IShape **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Contains )( 
            IShape * This,
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Crosses )( 
            IShape * This,
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Disjoint )( 
            IShape * This,
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Equals )( 
            IShape * This,
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Intersects )( 
            IShape * This,
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Overlaps )( 
            IShape * This,
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Touches )( 
            IShape * This,
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Within )( 
            IShape * This,
            /* [in] */ IShape *Shape,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Boundry )( 
            IShape * This,
            /* [retval][out] */ IShape **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ConvexHull )( 
            IShape * This,
            /* [retval][out] */ IShape **retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsValid )( 
            IShape * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_XY )( 
            IShape * This,
            /* [in] */ long PointIndex,
            /* [out][in] */ double *x,
            /* [out][in] */ double *y,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PartIsClockWise )( 
            IShape * This,
            /* [in] */ long PartIndex,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ReversePointsOrder )( 
            IShape * This,
            /* [in] */ long PartIndex,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetIntersection )( 
            IShape * This,
            /* [in] */ IShape *Shape,
            /* [out][out] */ VARIANT *Results,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Center )( 
            IShape * This,
            /* [retval][out] */ IPoint **pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_EndOfPart )( 
            IShape * This,
            /* [in] */ long PartIndex,
            /* [retval][out] */ long *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PartAsShape )( 
            IShape * This,
            /* [in] */ long PartIndex,
            /* [retval][out] */ IShape **retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsValidReason )( 
            IShape * This,
            /* [retval][out] */ BSTR *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_InteriorPoint )( 
            IShape * This,
            /* [retval][out] */ IPoint **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clone )( 
            IShape * This,
            /* [retval][out] */ IShape **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Explode )( 
            IShape * This,
            /* [out][in] */ VARIANT *Results,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *put_XY )( 
            IShape * This,
            /* [in] */ LONG pointIndex,
            /* [in] */ DOUBLE x,
            /* [in] */ DOUBLE y,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ExportToBinary )( 
            IShape * This,
            /* [out][in] */ VARIANT *bytesArray,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ImportFromBinary )( 
            IShape * This,
            /* [in] */ VARIANT bytesArray,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *FixUp )( 
            IShape * This,
            /* [out] */ IShape **retval);
        
        END_INTERFACE
    } IShapeVtbl;

    interface IShape
    {
        CONST_VTBL struct IShapeVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IShape_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IShape_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IShape_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IShape_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IShape_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IShape_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IShape_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IShape_get_NumPoints(This,pVal)	\
    ( (This)->lpVtbl -> get_NumPoints(This,pVal) ) 

#define IShape_get_NumParts(This,pVal)	\
    ( (This)->lpVtbl -> get_NumParts(This,pVal) ) 

#define IShape_get_ShapeType(This,pVal)	\
    ( (This)->lpVtbl -> get_ShapeType(This,pVal) ) 

#define IShape_put_ShapeType(This,newVal)	\
    ( (This)->lpVtbl -> put_ShapeType(This,newVal) ) 

#define IShape_get_Point(This,PointIndex,pVal)	\
    ( (This)->lpVtbl -> get_Point(This,PointIndex,pVal) ) 

#define IShape_put_Point(This,PointIndex,newVal)	\
    ( (This)->lpVtbl -> put_Point(This,PointIndex,newVal) ) 

#define IShape_get_Part(This,PartIndex,pVal)	\
    ( (This)->lpVtbl -> get_Part(This,PartIndex,pVal) ) 

#define IShape_put_Part(This,PartIndex,newVal)	\
    ( (This)->lpVtbl -> put_Part(This,PartIndex,newVal) ) 

#define IShape_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IShape_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IShape_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IShape_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IShape_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IShape_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IShape_Create(This,ShpType,retval)	\
    ( (This)->lpVtbl -> Create(This,ShpType,retval) ) 

#define IShape_InsertPoint(This,NewPoint,PointIndex,retval)	\
    ( (This)->lpVtbl -> InsertPoint(This,NewPoint,PointIndex,retval) ) 

#define IShape_DeletePoint(This,PointIndex,retval)	\
    ( (This)->lpVtbl -> DeletePoint(This,PointIndex,retval) ) 

#define IShape_InsertPart(This,PointIndex,PartIndex,retval)	\
    ( (This)->lpVtbl -> InsertPart(This,PointIndex,PartIndex,retval) ) 

#define IShape_DeletePart(This,PartIndex,retval)	\
    ( (This)->lpVtbl -> DeletePart(This,PartIndex,retval) ) 

#define IShape_get_Extents(This,pVal)	\
    ( (This)->lpVtbl -> get_Extents(This,pVal) ) 

#define IShape_SerializeToString(This,Serialized)	\
    ( (This)->lpVtbl -> SerializeToString(This,Serialized) ) 

#define IShape_CreateFromString(This,Serialized,retval)	\
    ( (This)->lpVtbl -> CreateFromString(This,Serialized,retval) ) 

#define IShape_PointInThisPoly(This,pt,retval)	\
    ( (This)->lpVtbl -> PointInThisPoly(This,pt,retval) ) 

#define IShape_get_Centroid(This,pVal)	\
    ( (This)->lpVtbl -> get_Centroid(This,pVal) ) 

#define IShape_get_Length(This,pVal)	\
    ( (This)->lpVtbl -> get_Length(This,pVal) ) 

#define IShape_get_Perimeter(This,pVal)	\
    ( (This)->lpVtbl -> get_Perimeter(This,pVal) ) 

#define IShape_get_Area(This,pVal)	\
    ( (This)->lpVtbl -> get_Area(This,pVal) ) 

#define IShape_Relates(This,Shape,Relation,retval)	\
    ( (This)->lpVtbl -> Relates(This,Shape,Relation,retval) ) 

#define IShape_Distance(This,Shape,retval)	\
    ( (This)->lpVtbl -> Distance(This,Shape,retval) ) 

#define IShape_Buffer(This,Distance,nQuadSegments,retval)	\
    ( (This)->lpVtbl -> Buffer(This,Distance,nQuadSegments,retval) ) 

#define IShape_Clip(This,Shape,Operation,retval)	\
    ( (This)->lpVtbl -> Clip(This,Shape,Operation,retval) ) 

#define IShape_Contains(This,Shape,retval)	\
    ( (This)->lpVtbl -> Contains(This,Shape,retval) ) 

#define IShape_Crosses(This,Shape,retval)	\
    ( (This)->lpVtbl -> Crosses(This,Shape,retval) ) 

#define IShape_Disjoint(This,Shape,retval)	\
    ( (This)->lpVtbl -> Disjoint(This,Shape,retval) ) 

#define IShape_Equals(This,Shape,retval)	\
    ( (This)->lpVtbl -> Equals(This,Shape,retval) ) 

#define IShape_Intersects(This,Shape,retval)	\
    ( (This)->lpVtbl -> Intersects(This,Shape,retval) ) 

#define IShape_Overlaps(This,Shape,retval)	\
    ( (This)->lpVtbl -> Overlaps(This,Shape,retval) ) 

#define IShape_Touches(This,Shape,retval)	\
    ( (This)->lpVtbl -> Touches(This,Shape,retval) ) 

#define IShape_Within(This,Shape,retval)	\
    ( (This)->lpVtbl -> Within(This,Shape,retval) ) 

#define IShape_Boundry(This,retval)	\
    ( (This)->lpVtbl -> Boundry(This,retval) ) 

#define IShape_ConvexHull(This,retval)	\
    ( (This)->lpVtbl -> ConvexHull(This,retval) ) 

#define IShape_get_IsValid(This,retval)	\
    ( (This)->lpVtbl -> get_IsValid(This,retval) ) 

#define IShape_get_XY(This,PointIndex,x,y,retval)	\
    ( (This)->lpVtbl -> get_XY(This,PointIndex,x,y,retval) ) 

#define IShape_get_PartIsClockWise(This,PartIndex,retval)	\
    ( (This)->lpVtbl -> get_PartIsClockWise(This,PartIndex,retval) ) 

#define IShape_ReversePointsOrder(This,PartIndex,retval)	\
    ( (This)->lpVtbl -> ReversePointsOrder(This,PartIndex,retval) ) 

#define IShape_GetIntersection(This,Shape,Results,retval)	\
    ( (This)->lpVtbl -> GetIntersection(This,Shape,Results,retval) ) 

#define IShape_get_Center(This,pVal)	\
    ( (This)->lpVtbl -> get_Center(This,pVal) ) 

#define IShape_get_EndOfPart(This,PartIndex,retval)	\
    ( (This)->lpVtbl -> get_EndOfPart(This,PartIndex,retval) ) 

#define IShape_get_PartAsShape(This,PartIndex,retval)	\
    ( (This)->lpVtbl -> get_PartAsShape(This,PartIndex,retval) ) 

#define IShape_get_IsValidReason(This,retval)	\
    ( (This)->lpVtbl -> get_IsValidReason(This,retval) ) 

#define IShape_get_InteriorPoint(This,retval)	\
    ( (This)->lpVtbl -> get_InteriorPoint(This,retval) ) 

#define IShape_Clone(This,retval)	\
    ( (This)->lpVtbl -> Clone(This,retval) ) 

#define IShape_Explode(This,Results,retval)	\
    ( (This)->lpVtbl -> Explode(This,Results,retval) ) 

#define IShape_put_XY(This,pointIndex,x,y,retVal)	\
    ( (This)->lpVtbl -> put_XY(This,pointIndex,x,y,retVal) ) 

#define IShape_ExportToBinary(This,bytesArray,retVal)	\
    ( (This)->lpVtbl -> ExportToBinary(This,bytesArray,retVal) ) 

#define IShape_ImportFromBinary(This,bytesArray,retVal)	\
    ( (This)->lpVtbl -> ImportFromBinary(This,bytesArray,retVal) ) 

#define IShape_FixUp(This,retval)	\
    ( (This)->lpVtbl -> FixUp(This,retval) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IShape_INTERFACE_DEFINED__ */


#ifndef __IExtents_INTERFACE_DEFINED__
#define __IExtents_INTERFACE_DEFINED__

/* interface IExtents */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IExtents;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("A5692259-035E-487A-8D89-509DD6DD0F64")
    IExtents : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetBounds( 
            /* [in] */ double xMin,
            /* [in] */ double yMin,
            /* [in] */ double zMin,
            /* [in] */ double xMax,
            /* [in] */ double yMax,
            /* [in] */ double zMax) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetBounds( 
            /* [out] */ double *xMin,
            /* [out] */ double *yMin,
            /* [out] */ double *zMin,
            /* [out] */ double *xMax,
            /* [out] */ double *yMax,
            /* [out] */ double *zMax) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_xMin( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_xMax( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_yMin( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_yMax( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_zMin( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_zMax( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_mMin( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_mMax( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetMeasureBounds( 
            /* [out] */ double *mMin,
            /* [out] */ double *mMax) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetMeasureBounds( 
            /* [in] */ double mMin,
            /* [in] */ double mMax) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IExtentsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IExtents * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IExtents * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IExtents * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IExtents * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IExtents * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IExtents * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IExtents * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetBounds )( 
            IExtents * This,
            /* [in] */ double xMin,
            /* [in] */ double yMin,
            /* [in] */ double zMin,
            /* [in] */ double xMax,
            /* [in] */ double yMax,
            /* [in] */ double zMax);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetBounds )( 
            IExtents * This,
            /* [out] */ double *xMin,
            /* [out] */ double *yMin,
            /* [out] */ double *zMin,
            /* [out] */ double *xMax,
            /* [out] */ double *yMax,
            /* [out] */ double *zMax);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_xMin )( 
            IExtents * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_xMax )( 
            IExtents * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_yMin )( 
            IExtents * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_yMax )( 
            IExtents * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_zMin )( 
            IExtents * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_zMax )( 
            IExtents * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_mMin )( 
            IExtents * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_mMax )( 
            IExtents * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetMeasureBounds )( 
            IExtents * This,
            /* [out] */ double *mMin,
            /* [out] */ double *mMax);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetMeasureBounds )( 
            IExtents * This,
            /* [in] */ double mMin,
            /* [in] */ double mMax);
        
        END_INTERFACE
    } IExtentsVtbl;

    interface IExtents
    {
        CONST_VTBL struct IExtentsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IExtents_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IExtents_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IExtents_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IExtents_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IExtents_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IExtents_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IExtents_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IExtents_SetBounds(This,xMin,yMin,zMin,xMax,yMax,zMax)	\
    ( (This)->lpVtbl -> SetBounds(This,xMin,yMin,zMin,xMax,yMax,zMax) ) 

#define IExtents_GetBounds(This,xMin,yMin,zMin,xMax,yMax,zMax)	\
    ( (This)->lpVtbl -> GetBounds(This,xMin,yMin,zMin,xMax,yMax,zMax) ) 

#define IExtents_get_xMin(This,pVal)	\
    ( (This)->lpVtbl -> get_xMin(This,pVal) ) 

#define IExtents_get_xMax(This,pVal)	\
    ( (This)->lpVtbl -> get_xMax(This,pVal) ) 

#define IExtents_get_yMin(This,pVal)	\
    ( (This)->lpVtbl -> get_yMin(This,pVal) ) 

#define IExtents_get_yMax(This,pVal)	\
    ( (This)->lpVtbl -> get_yMax(This,pVal) ) 

#define IExtents_get_zMin(This,pVal)	\
    ( (This)->lpVtbl -> get_zMin(This,pVal) ) 

#define IExtents_get_zMax(This,pVal)	\
    ( (This)->lpVtbl -> get_zMax(This,pVal) ) 

#define IExtents_get_mMin(This,pVal)	\
    ( (This)->lpVtbl -> get_mMin(This,pVal) ) 

#define IExtents_get_mMax(This,pVal)	\
    ( (This)->lpVtbl -> get_mMax(This,pVal) ) 

#define IExtents_GetMeasureBounds(This,mMin,mMax)	\
    ( (This)->lpVtbl -> GetMeasureBounds(This,mMin,mMax) ) 

#define IExtents_SetMeasureBounds(This,mMin,mMax)	\
    ( (This)->lpVtbl -> SetMeasureBounds(This,mMin,mMax) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IExtents_INTERFACE_DEFINED__ */


#ifndef __IPoint_INTERFACE_DEFINED__
#define __IPoint_INTERFACE_DEFINED__

/* interface IPoint */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IPoint;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("74F07889-1380-43EE-837A-BBB268311005")
    IPoint : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_X( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_X( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Y( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Y( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Z( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Z( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_M( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_M( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clone( 
            /* [retval][out] */ IPoint **retVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IPointVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IPoint * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IPoint * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IPoint * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IPoint * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IPoint * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IPoint * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IPoint * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_X )( 
            IPoint * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_X )( 
            IPoint * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Y )( 
            IPoint * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Y )( 
            IPoint * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Z )( 
            IPoint * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Z )( 
            IPoint * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IPoint * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IPoint * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IPoint * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IPoint * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IPoint * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IPoint * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_M )( 
            IPoint * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_M )( 
            IPoint * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clone )( 
            IPoint * This,
            /* [retval][out] */ IPoint **retVal);
        
        END_INTERFACE
    } IPointVtbl;

    interface IPoint
    {
        CONST_VTBL struct IPointVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IPoint_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IPoint_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IPoint_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IPoint_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IPoint_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IPoint_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IPoint_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IPoint_get_X(This,pVal)	\
    ( (This)->lpVtbl -> get_X(This,pVal) ) 

#define IPoint_put_X(This,newVal)	\
    ( (This)->lpVtbl -> put_X(This,newVal) ) 

#define IPoint_get_Y(This,pVal)	\
    ( (This)->lpVtbl -> get_Y(This,pVal) ) 

#define IPoint_put_Y(This,newVal)	\
    ( (This)->lpVtbl -> put_Y(This,newVal) ) 

#define IPoint_get_Z(This,pVal)	\
    ( (This)->lpVtbl -> get_Z(This,pVal) ) 

#define IPoint_put_Z(This,newVal)	\
    ( (This)->lpVtbl -> put_Z(This,newVal) ) 

#define IPoint_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IPoint_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IPoint_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IPoint_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IPoint_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IPoint_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IPoint_get_M(This,pVal)	\
    ( (This)->lpVtbl -> get_M(This,pVal) ) 

#define IPoint_put_M(This,newVal)	\
    ( (This)->lpVtbl -> put_M(This,newVal) ) 

#define IPoint_Clone(This,retVal)	\
    ( (This)->lpVtbl -> Clone(This,retVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IPoint_INTERFACE_DEFINED__ */


#ifndef __ITable_INTERFACE_DEFINED__
#define __ITable_INTERFACE_DEFINED__

/* interface ITable */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ITable;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4365A8A1-2E46-4223-B2DC-65764262D88B")
    ITable : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumRows( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumFields( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Field( 
            /* [in] */ long FieldIndex,
            /* [retval][out] */ IField **pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CellValue( 
            /* [in] */ long FieldIndex,
            /* [in] */ long RowIndex,
            /* [retval][out] */ VARIANT *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_EditingTable( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CdlgFilter( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Open( 
            /* [in] */ BSTR dbfFilename,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CreateNew( 
            /* [in] */ BSTR dbfFilename,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SaveAs( 
            /* [in] */ BSTR dbfFilename,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Close( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditClear( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditInsertField( 
            /* [in] */ IField *Field,
            /* [out][in] */ long *FieldIndex,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditReplaceField( 
            /* [in] */ long FieldIndex,
            /* [in] */ IField *newField,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditDeleteField( 
            /* [in] */ long FieldIndex,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditInsertRow( 
            /* [out][in] */ long *RowIndex,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditCellValue( 
            /* [in] */ long FieldIndex,
            /* [in] */ long RowIndex,
            /* [in] */ VARIANT newVal,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE StartEditingTable( 
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE StopEditingTable( 
            /* [defaultvalue][optional][in] */ VARIANT_BOOL ApplyChanges,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE EditDeleteRow( 
            /* [in] */ long RowIndex,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Save( 
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MinValue( 
            /* [in] */ long FieldIndex,
            /* [retval][out] */ VARIANT *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MaxValue( 
            /* [in] */ long FieldIndex,
            /* [retval][out] */ VARIANT *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MeanValue( 
            /* [in] */ long FieldIndex,
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_StandardDeviation( 
            /* [in] */ long FieldIndex,
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ParseExpression( 
            /* [in] */ BSTR Expression,
            /* [out][in] */ BSTR *ErrorString,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Query( 
            /* [in] */ BSTR Expression,
            /* [out][in] */ VARIANT *Result,
            /* [out][in] */ BSTR *ErrorString,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FieldIndexByName( 
            /* [in] */ BSTR FieldName,
            /* [retval][out] */ long *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE TestExpression( 
            /* [in] */ BSTR Expression,
            /* [in] */ tkValueType ReturnType,
            /* [out][in] */ BSTR *ErrorString,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Calculate( 
            /* [in] */ BSTR Expression,
            /* [in] */ LONG RowIndex,
            /* [out] */ VARIANT *Result,
            /* [out] */ BSTR *ErrorString,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ITableVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ITable * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ITable * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ITable * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ITable * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ITable * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ITable * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ITable * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumRows )( 
            ITable * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumFields )( 
            ITable * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Field )( 
            ITable * This,
            /* [in] */ long FieldIndex,
            /* [retval][out] */ IField **pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CellValue )( 
            ITable * This,
            /* [in] */ long FieldIndex,
            /* [in] */ long RowIndex,
            /* [retval][out] */ VARIANT *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_EditingTable )( 
            ITable * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            ITable * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            ITable * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CdlgFilter )( 
            ITable * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            ITable * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            ITable * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            ITable * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            ITable * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Open )( 
            ITable * This,
            /* [in] */ BSTR dbfFilename,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CreateNew )( 
            ITable * This,
            /* [in] */ BSTR dbfFilename,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SaveAs )( 
            ITable * This,
            /* [in] */ BSTR dbfFilename,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Close )( 
            ITable * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditClear )( 
            ITable * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditInsertField )( 
            ITable * This,
            /* [in] */ IField *Field,
            /* [out][in] */ long *FieldIndex,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditReplaceField )( 
            ITable * This,
            /* [in] */ long FieldIndex,
            /* [in] */ IField *newField,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditDeleteField )( 
            ITable * This,
            /* [in] */ long FieldIndex,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditInsertRow )( 
            ITable * This,
            /* [out][in] */ long *RowIndex,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditCellValue )( 
            ITable * This,
            /* [in] */ long FieldIndex,
            /* [in] */ long RowIndex,
            /* [in] */ VARIANT newVal,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *StartEditingTable )( 
            ITable * This,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *StopEditingTable )( 
            ITable * This,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL ApplyChanges,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *EditDeleteRow )( 
            ITable * This,
            /* [in] */ long RowIndex,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Save )( 
            ITable * This,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MinValue )( 
            ITable * This,
            /* [in] */ long FieldIndex,
            /* [retval][out] */ VARIANT *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MaxValue )( 
            ITable * This,
            /* [in] */ long FieldIndex,
            /* [retval][out] */ VARIANT *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MeanValue )( 
            ITable * This,
            /* [in] */ long FieldIndex,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_StandardDeviation )( 
            ITable * This,
            /* [in] */ long FieldIndex,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ParseExpression )( 
            ITable * This,
            /* [in] */ BSTR Expression,
            /* [out][in] */ BSTR *ErrorString,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Query )( 
            ITable * This,
            /* [in] */ BSTR Expression,
            /* [out][in] */ VARIANT *Result,
            /* [out][in] */ BSTR *ErrorString,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FieldIndexByName )( 
            ITable * This,
            /* [in] */ BSTR FieldName,
            /* [retval][out] */ long *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *TestExpression )( 
            ITable * This,
            /* [in] */ BSTR Expression,
            /* [in] */ tkValueType ReturnType,
            /* [out][in] */ BSTR *ErrorString,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Calculate )( 
            ITable * This,
            /* [in] */ BSTR Expression,
            /* [in] */ LONG RowIndex,
            /* [out] */ VARIANT *Result,
            /* [out] */ BSTR *ErrorString,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        END_INTERFACE
    } ITableVtbl;

    interface ITable
    {
        CONST_VTBL struct ITableVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ITable_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ITable_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ITable_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ITable_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ITable_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ITable_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ITable_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ITable_get_NumRows(This,pVal)	\
    ( (This)->lpVtbl -> get_NumRows(This,pVal) ) 

#define ITable_get_NumFields(This,pVal)	\
    ( (This)->lpVtbl -> get_NumFields(This,pVal) ) 

#define ITable_get_Field(This,FieldIndex,pVal)	\
    ( (This)->lpVtbl -> get_Field(This,FieldIndex,pVal) ) 

#define ITable_get_CellValue(This,FieldIndex,RowIndex,pVal)	\
    ( (This)->lpVtbl -> get_CellValue(This,FieldIndex,RowIndex,pVal) ) 

#define ITable_get_EditingTable(This,pVal)	\
    ( (This)->lpVtbl -> get_EditingTable(This,pVal) ) 

#define ITable_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define ITable_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define ITable_get_CdlgFilter(This,pVal)	\
    ( (This)->lpVtbl -> get_CdlgFilter(This,pVal) ) 

#define ITable_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define ITable_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define ITable_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define ITable_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define ITable_Open(This,dbfFilename,cBack,retval)	\
    ( (This)->lpVtbl -> Open(This,dbfFilename,cBack,retval) ) 

#define ITable_CreateNew(This,dbfFilename,retval)	\
    ( (This)->lpVtbl -> CreateNew(This,dbfFilename,retval) ) 

#define ITable_SaveAs(This,dbfFilename,cBack,retval)	\
    ( (This)->lpVtbl -> SaveAs(This,dbfFilename,cBack,retval) ) 

#define ITable_Close(This,retval)	\
    ( (This)->lpVtbl -> Close(This,retval) ) 

#define ITable_EditClear(This,retval)	\
    ( (This)->lpVtbl -> EditClear(This,retval) ) 

#define ITable_EditInsertField(This,Field,FieldIndex,cBack,retval)	\
    ( (This)->lpVtbl -> EditInsertField(This,Field,FieldIndex,cBack,retval) ) 

#define ITable_EditReplaceField(This,FieldIndex,newField,cBack,retval)	\
    ( (This)->lpVtbl -> EditReplaceField(This,FieldIndex,newField,cBack,retval) ) 

#define ITable_EditDeleteField(This,FieldIndex,cBack,retval)	\
    ( (This)->lpVtbl -> EditDeleteField(This,FieldIndex,cBack,retval) ) 

#define ITable_EditInsertRow(This,RowIndex,retval)	\
    ( (This)->lpVtbl -> EditInsertRow(This,RowIndex,retval) ) 

#define ITable_EditCellValue(This,FieldIndex,RowIndex,newVal,retval)	\
    ( (This)->lpVtbl -> EditCellValue(This,FieldIndex,RowIndex,newVal,retval) ) 

#define ITable_StartEditingTable(This,cBack,retval)	\
    ( (This)->lpVtbl -> StartEditingTable(This,cBack,retval) ) 

#define ITable_StopEditingTable(This,ApplyChanges,cBack,retval)	\
    ( (This)->lpVtbl -> StopEditingTable(This,ApplyChanges,cBack,retval) ) 

#define ITable_EditDeleteRow(This,RowIndex,retval)	\
    ( (This)->lpVtbl -> EditDeleteRow(This,RowIndex,retval) ) 

#define ITable_Save(This,cBack,retval)	\
    ( (This)->lpVtbl -> Save(This,cBack,retval) ) 

#define ITable_get_MinValue(This,FieldIndex,retval)	\
    ( (This)->lpVtbl -> get_MinValue(This,FieldIndex,retval) ) 

#define ITable_get_MaxValue(This,FieldIndex,retval)	\
    ( (This)->lpVtbl -> get_MaxValue(This,FieldIndex,retval) ) 

#define ITable_get_MeanValue(This,FieldIndex,retval)	\
    ( (This)->lpVtbl -> get_MeanValue(This,FieldIndex,retval) ) 

#define ITable_get_StandardDeviation(This,FieldIndex,retval)	\
    ( (This)->lpVtbl -> get_StandardDeviation(This,FieldIndex,retval) ) 

#define ITable_ParseExpression(This,Expression,ErrorString,retVal)	\
    ( (This)->lpVtbl -> ParseExpression(This,Expression,ErrorString,retVal) ) 

#define ITable_Query(This,Expression,Result,ErrorString,retval)	\
    ( (This)->lpVtbl -> Query(This,Expression,Result,ErrorString,retval) ) 

#define ITable_get_FieldIndexByName(This,FieldName,retval)	\
    ( (This)->lpVtbl -> get_FieldIndexByName(This,FieldName,retval) ) 

#define ITable_TestExpression(This,Expression,ReturnType,ErrorString,retVal)	\
    ( (This)->lpVtbl -> TestExpression(This,Expression,ReturnType,ErrorString,retVal) ) 

#define ITable_Calculate(This,Expression,RowIndex,Result,ErrorString,retVal)	\
    ( (This)->lpVtbl -> Calculate(This,Expression,RowIndex,Result,ErrorString,retVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ITable_INTERFACE_DEFINED__ */


#ifndef __IField_INTERFACE_DEFINED__
#define __IField_INTERFACE_DEFINED__

/* interface IField */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IField;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("3F3751A5-4CF8-4AC3-AFC2-60DE8B90FC7B")
    IField : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Name( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Name( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Width( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Width( 
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Precision( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Precision( 
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Type( 
            /* [retval][out] */ FieldType *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Type( 
            /* [in] */ FieldType newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clone( 
            /* [retval][out] */ IField **retVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IFieldVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IField * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IField * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IField * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IField * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IField * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IField * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IField * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Name )( 
            IField * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Name )( 
            IField * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Width )( 
            IField * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Width )( 
            IField * This,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Precision )( 
            IField * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Precision )( 
            IField * This,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Type )( 
            IField * This,
            /* [retval][out] */ FieldType *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Type )( 
            IField * This,
            /* [in] */ FieldType newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IField * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IField * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IField * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IField * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IField * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IField * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clone )( 
            IField * This,
            /* [retval][out] */ IField **retVal);
        
        END_INTERFACE
    } IFieldVtbl;

    interface IField
    {
        CONST_VTBL struct IFieldVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IField_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IField_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IField_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IField_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IField_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IField_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IField_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IField_get_Name(This,pVal)	\
    ( (This)->lpVtbl -> get_Name(This,pVal) ) 

#define IField_put_Name(This,newVal)	\
    ( (This)->lpVtbl -> put_Name(This,newVal) ) 

#define IField_get_Width(This,pVal)	\
    ( (This)->lpVtbl -> get_Width(This,pVal) ) 

#define IField_put_Width(This,newVal)	\
    ( (This)->lpVtbl -> put_Width(This,newVal) ) 

#define IField_get_Precision(This,pVal)	\
    ( (This)->lpVtbl -> get_Precision(This,pVal) ) 

#define IField_put_Precision(This,newVal)	\
    ( (This)->lpVtbl -> put_Precision(This,newVal) ) 

#define IField_get_Type(This,pVal)	\
    ( (This)->lpVtbl -> get_Type(This,pVal) ) 

#define IField_put_Type(This,newVal)	\
    ( (This)->lpVtbl -> put_Type(This,newVal) ) 

#define IField_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IField_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IField_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IField_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IField_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IField_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IField_Clone(This,retVal)	\
    ( (This)->lpVtbl -> Clone(This,retVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IField_INTERFACE_DEFINED__ */


#ifndef __IShapeNetwork_INTERFACE_DEFINED__
#define __IShapeNetwork_INTERFACE_DEFINED__

/* interface IShapeNetwork */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IShapeNetwork;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("2D4968F2-40D9-4F25-8BE6-B51B959CC1B0")
    IShapeNetwork : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Build( 
            /* [in] */ IShapefile *Shapefile,
            /* [in] */ long ShapeIndex,
            /* [in] */ long FinalPointIndex,
            /* [in] */ double Tolerance,
            /* [in] */ AmbiguityResolution ar,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ long *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DeleteShape( 
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE RasterizeD8( 
            /* [in] */ VARIANT_BOOL UseNetworkBounds,
            /* [optional][in] */ IGridHeader *Header,
            /* [defaultvalue][optional][in] */ double Cellsize,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IGrid **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE MoveUp( 
            /* [in] */ long UpIndex,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE MoveDown( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE MoveTo( 
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE MoveToOutlet( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Shapefile( 
            /* [retval][out] */ IShapefile **pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CurrentShape( 
            /* [retval][out] */ IShape **pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CurrentShapeIndex( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_DistanceToOutlet( 
            /* [in] */ long PointIndex,
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumDirectUps( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NetworkSize( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_AmbigShapeIndex( 
            /* [in] */ long Index,
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ParentIndex( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ParentIndex( 
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Open( 
            /* [in] */ IShapefile *sf,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Close( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IShapeNetworkVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IShapeNetwork * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IShapeNetwork * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IShapeNetwork * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IShapeNetwork * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IShapeNetwork * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IShapeNetwork * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IShapeNetwork * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Build )( 
            IShapeNetwork * This,
            /* [in] */ IShapefile *Shapefile,
            /* [in] */ long ShapeIndex,
            /* [in] */ long FinalPointIndex,
            /* [in] */ double Tolerance,
            /* [in] */ AmbiguityResolution ar,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ long *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DeleteShape )( 
            IShapeNetwork * This,
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *RasterizeD8 )( 
            IShapeNetwork * This,
            /* [in] */ VARIANT_BOOL UseNetworkBounds,
            /* [optional][in] */ IGridHeader *Header,
            /* [defaultvalue][optional][in] */ double Cellsize,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IGrid **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *MoveUp )( 
            IShapeNetwork * This,
            /* [in] */ long UpIndex,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *MoveDown )( 
            IShapeNetwork * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *MoveTo )( 
            IShapeNetwork * This,
            /* [in] */ long ShapeIndex,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *MoveToOutlet )( 
            IShapeNetwork * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Shapefile )( 
            IShapeNetwork * This,
            /* [retval][out] */ IShapefile **pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CurrentShape )( 
            IShapeNetwork * This,
            /* [retval][out] */ IShape **pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CurrentShapeIndex )( 
            IShapeNetwork * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DistanceToOutlet )( 
            IShapeNetwork * This,
            /* [in] */ long PointIndex,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumDirectUps )( 
            IShapeNetwork * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NetworkSize )( 
            IShapeNetwork * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_AmbigShapeIndex )( 
            IShapeNetwork * This,
            /* [in] */ long Index,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IShapeNetwork * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IShapeNetwork * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IShapeNetwork * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IShapeNetwork * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IShapeNetwork * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IShapeNetwork * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ParentIndex )( 
            IShapeNetwork * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ParentIndex )( 
            IShapeNetwork * This,
            /* [in] */ long newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Open )( 
            IShapeNetwork * This,
            /* [in] */ IShapefile *sf,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Close )( 
            IShapeNetwork * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        END_INTERFACE
    } IShapeNetworkVtbl;

    interface IShapeNetwork
    {
        CONST_VTBL struct IShapeNetworkVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IShapeNetwork_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IShapeNetwork_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IShapeNetwork_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IShapeNetwork_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IShapeNetwork_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IShapeNetwork_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IShapeNetwork_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IShapeNetwork_Build(This,Shapefile,ShapeIndex,FinalPointIndex,Tolerance,ar,cBack,retval)	\
    ( (This)->lpVtbl -> Build(This,Shapefile,ShapeIndex,FinalPointIndex,Tolerance,ar,cBack,retval) ) 

#define IShapeNetwork_DeleteShape(This,ShapeIndex,retval)	\
    ( (This)->lpVtbl -> DeleteShape(This,ShapeIndex,retval) ) 

#define IShapeNetwork_RasterizeD8(This,UseNetworkBounds,Header,Cellsize,cBack,retval)	\
    ( (This)->lpVtbl -> RasterizeD8(This,UseNetworkBounds,Header,Cellsize,cBack,retval) ) 

#define IShapeNetwork_MoveUp(This,UpIndex,retval)	\
    ( (This)->lpVtbl -> MoveUp(This,UpIndex,retval) ) 

#define IShapeNetwork_MoveDown(This,retval)	\
    ( (This)->lpVtbl -> MoveDown(This,retval) ) 

#define IShapeNetwork_MoveTo(This,ShapeIndex,retval)	\
    ( (This)->lpVtbl -> MoveTo(This,ShapeIndex,retval) ) 

#define IShapeNetwork_MoveToOutlet(This,retval)	\
    ( (This)->lpVtbl -> MoveToOutlet(This,retval) ) 

#define IShapeNetwork_get_Shapefile(This,pVal)	\
    ( (This)->lpVtbl -> get_Shapefile(This,pVal) ) 

#define IShapeNetwork_get_CurrentShape(This,pVal)	\
    ( (This)->lpVtbl -> get_CurrentShape(This,pVal) ) 

#define IShapeNetwork_get_CurrentShapeIndex(This,pVal)	\
    ( (This)->lpVtbl -> get_CurrentShapeIndex(This,pVal) ) 

#define IShapeNetwork_get_DistanceToOutlet(This,PointIndex,pVal)	\
    ( (This)->lpVtbl -> get_DistanceToOutlet(This,PointIndex,pVal) ) 

#define IShapeNetwork_get_NumDirectUps(This,pVal)	\
    ( (This)->lpVtbl -> get_NumDirectUps(This,pVal) ) 

#define IShapeNetwork_get_NetworkSize(This,pVal)	\
    ( (This)->lpVtbl -> get_NetworkSize(This,pVal) ) 

#define IShapeNetwork_get_AmbigShapeIndex(This,Index,pVal)	\
    ( (This)->lpVtbl -> get_AmbigShapeIndex(This,Index,pVal) ) 

#define IShapeNetwork_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IShapeNetwork_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IShapeNetwork_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IShapeNetwork_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IShapeNetwork_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IShapeNetwork_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IShapeNetwork_get_ParentIndex(This,pVal)	\
    ( (This)->lpVtbl -> get_ParentIndex(This,pVal) ) 

#define IShapeNetwork_put_ParentIndex(This,newVal)	\
    ( (This)->lpVtbl -> put_ParentIndex(This,newVal) ) 

#define IShapeNetwork_Open(This,sf,cBack,retval)	\
    ( (This)->lpVtbl -> Open(This,sf,cBack,retval) ) 

#define IShapeNetwork_Close(This,retval)	\
    ( (This)->lpVtbl -> Close(This,retval) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IShapeNetwork_INTERFACE_DEFINED__ */


#ifndef __ICallback_INTERFACE_DEFINED__
#define __ICallback_INTERFACE_DEFINED__

/* interface ICallback */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_ICallback;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("90E6BBF7-A956-49be-A5CD-A4640C263AB6")
    ICallback : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Progress( 
            /* [in] */ BSTR KeyOfSender,
            /* [in] */ long Percent,
            /* [in] */ BSTR Message) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Error( 
            /* [in] */ BSTR KeyOfSender,
            /* [in] */ BSTR ErrorMsg) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ICallbackVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ICallback * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ICallback * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ICallback * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ICallback * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ICallback * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ICallback * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ICallback * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Progress )( 
            ICallback * This,
            /* [in] */ BSTR KeyOfSender,
            /* [in] */ long Percent,
            /* [in] */ BSTR Message);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Error )( 
            ICallback * This,
            /* [in] */ BSTR KeyOfSender,
            /* [in] */ BSTR ErrorMsg);
        
        END_INTERFACE
    } ICallbackVtbl;

    interface ICallback
    {
        CONST_VTBL struct ICallbackVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ICallback_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ICallback_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ICallback_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ICallback_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ICallback_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ICallback_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ICallback_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ICallback_Progress(This,KeyOfSender,Percent,Message)	\
    ( (This)->lpVtbl -> Progress(This,KeyOfSender,Percent,Message) ) 

#define ICallback_Error(This,KeyOfSender,ErrorMsg)	\
    ( (This)->lpVtbl -> Error(This,KeyOfSender,ErrorMsg) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ICallback_INTERFACE_DEFINED__ */


#ifndef __IStopExecution_INTERFACE_DEFINED__
#define __IStopExecution_INTERFACE_DEFINED__

/* interface IStopExecution */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_IStopExecution;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("52A29829-BB46-4d76-8082-55551E538BDA")
    IStopExecution : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE StopFunction( 
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IStopExecutionVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IStopExecution * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IStopExecution * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IStopExecution * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IStopExecution * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IStopExecution * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IStopExecution * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IStopExecution * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *StopFunction )( 
            IStopExecution * This,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        END_INTERFACE
    } IStopExecutionVtbl;

    interface IStopExecution
    {
        CONST_VTBL struct IStopExecutionVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IStopExecution_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IStopExecution_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IStopExecution_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IStopExecution_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IStopExecution_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IStopExecution_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IStopExecution_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IStopExecution_StopFunction(This,retVal)	\
    ( (This)->lpVtbl -> StopFunction(This,retVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IStopExecution_INTERFACE_DEFINED__ */


#ifndef __IUtils_INTERFACE_DEFINED__
#define __IUtils_INTERFACE_DEFINED__

/* interface IUtils */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IUtils;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("360BEC33-7703-4693-B6CA-90FEA22CF1B7")
    IUtils : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PointInPolygon( 
            /* [in] */ IShape *Shp,
            /* [in] */ IPoint *TestPoint,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GridReplace( 
            /* [in] */ IGrid *Grd,
            /* [in] */ VARIANT OldValue,
            /* [in] */ VARIANT NewValue,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GridInterpolateNoData( 
            /* [in] */ IGrid *Grd,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE RemoveColinearPoints( 
            /* [in] */ IShapefile *Shapes,
            /* [in] */ double LinearTolerance,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Length( 
            /* [in] */ IShape *Shape,
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Perimeter( 
            /* [in] */ IShape *Shape,
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Area( 
            /* [in] */ IShape *Shape,
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ClipPolygon( 
            /* [in] */ PolygonOperation op,
            /* [in] */ IShape *SubjectPolygon,
            /* [in] */ IShape *ClipPolygon,
            /* [retval][out] */ IShape **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GridMerge( 
            /* [in] */ VARIANT Grids,
            /* [in] */ BSTR MergeFilename,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL InRam,
            /* [defaultvalue][optional][in] */ GridFileType GrdFileType,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IGrid **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ShapeMerge( 
            /* [in] */ IShapefile *Shapes,
            /* [in] */ long IndexOne,
            /* [in] */ long IndexTwo,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IShape **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GridToImage( 
            /* [in] */ IGrid *Grid,
            /* [in] */ IGridColorScheme *cScheme,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IImage **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GridToShapefile( 
            /* [in] */ IGrid *Grid,
            /* [defaultvalue][optional][in] */ IGrid *ConnectionGrid,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IShapefile **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GridToGrid( 
            /* [in] */ IGrid *Grid,
            /* [in] */ GridDataType OutDataType,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IGrid **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ShapeToShapeZ( 
            /* [in] */ IShapefile *Shapefile,
            /* [in] */ IGrid *Grid,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IShapefile **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE TinToShapefile( 
            /* [in] */ ITin *Tin,
            /* [in] */ ShpfileType Type,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IShapefile **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ShapefileToGrid( 
            /* [in] */ IShapefile *Shpfile,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL UseShapefileBounds,
            /* [defaultvalue][optional][in] */ IGridHeader *GrdHeader,
            /* [defaultvalue][optional][in] */ double Cellsize,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL UseShapeNumber,
            /* [defaultvalue][optional][in] */ short SingleValue,
            /* [retval][out] */ IGrid **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE hBitmapToPicture( 
            /* [in] */ long hBitmap,
            /* [retval][out] */ IPictureDisp **retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GenerateHillShade( 
            /* [in] */ BSTR bstrGridFilename,
            /* [in] */ BSTR bstrShadeFilename,
            /* [defaultvalue][optional][in] */ float z,
            /* [defaultvalue][optional][in] */ float scale,
            /* [defaultvalue][optional][in] */ float az,
            /* [defaultvalue][optional][in] */ float alt,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GenerateContour( 
            /* [in] */ BSTR pszSrcFilename,
            /* [in] */ BSTR pszDstFilename,
            /* [in] */ double dfInterval,
            /* [defaultvalue][optional][in] */ double dfNoData,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL Is3D,
            /* [defaultvalue][optional][in] */ VARIANT dblFLArray,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE TranslateRaster( 
            /* [in] */ BSTR bstrSrcFilename,
            /* [in] */ BSTR bstrDstFilename,
            /* [in] */ BSTR bstrOptions,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OGRLayerToShapefile( 
            /* [in] */ BSTR Filename,
            /* [defaultvalue][optional][in] */ ShpfileType shpType,
            /* [defaultvalue][optional][in] */ ICallback *cBack,
            /* [retval][out] */ IShapefile **sf) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE MergeImages( 
            /* [in] */ SAFEARRAY * InputNames,
            /* [in] */ BSTR OutputName,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ReprojectShapefile( 
            /* [in] */ IShapefile *sf,
            /* [in] */ IGeoProjection *source,
            /* [in] */ IGeoProjection *target,
            /* [retval][out] */ IShapefile **result) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ColorByName( 
            /* [in] */ tkMapColor name,
            /* [retval][out] */ OLE_COLOR *retVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IUtilsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IUtils * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IUtils * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IUtils * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IUtils * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IUtils * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IUtils * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IUtils * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PointInPolygon )( 
            IUtils * This,
            /* [in] */ IShape *Shp,
            /* [in] */ IPoint *TestPoint,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GridReplace )( 
            IUtils * This,
            /* [in] */ IGrid *Grd,
            /* [in] */ VARIANT OldValue,
            /* [in] */ VARIANT NewValue,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GridInterpolateNoData )( 
            IUtils * This,
            /* [in] */ IGrid *Grd,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *RemoveColinearPoints )( 
            IUtils * This,
            /* [in] */ IShapefile *Shapes,
            /* [in] */ double LinearTolerance,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Length )( 
            IUtils * This,
            /* [in] */ IShape *Shape,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Perimeter )( 
            IUtils * This,
            /* [in] */ IShape *Shape,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Area )( 
            IUtils * This,
            /* [in] */ IShape *Shape,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IUtils * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IUtils * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IUtils * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IUtils * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IUtils * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IUtils * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ClipPolygon )( 
            IUtils * This,
            /* [in] */ PolygonOperation op,
            /* [in] */ IShape *SubjectPolygon,
            /* [in] */ IShape *ClipPolygon,
            /* [retval][out] */ IShape **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GridMerge )( 
            IUtils * This,
            /* [in] */ VARIANT Grids,
            /* [in] */ BSTR MergeFilename,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL InRam,
            /* [defaultvalue][optional][in] */ GridFileType GrdFileType,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IGrid **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ShapeMerge )( 
            IUtils * This,
            /* [in] */ IShapefile *Shapes,
            /* [in] */ long IndexOne,
            /* [in] */ long IndexTwo,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IShape **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GridToImage )( 
            IUtils * This,
            /* [in] */ IGrid *Grid,
            /* [in] */ IGridColorScheme *cScheme,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IImage **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GridToShapefile )( 
            IUtils * This,
            /* [in] */ IGrid *Grid,
            /* [defaultvalue][optional][in] */ IGrid *ConnectionGrid,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IShapefile **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GridToGrid )( 
            IUtils * This,
            /* [in] */ IGrid *Grid,
            /* [in] */ GridDataType OutDataType,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IGrid **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ShapeToShapeZ )( 
            IUtils * This,
            /* [in] */ IShapefile *Shapefile,
            /* [in] */ IGrid *Grid,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IShapefile **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *TinToShapefile )( 
            IUtils * This,
            /* [in] */ ITin *Tin,
            /* [in] */ ShpfileType Type,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ IShapefile **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ShapefileToGrid )( 
            IUtils * This,
            /* [in] */ IShapefile *Shpfile,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL UseShapefileBounds,
            /* [defaultvalue][optional][in] */ IGridHeader *GrdHeader,
            /* [defaultvalue][optional][in] */ double Cellsize,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL UseShapeNumber,
            /* [defaultvalue][optional][in] */ short SingleValue,
            /* [retval][out] */ IGrid **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *hBitmapToPicture )( 
            IUtils * This,
            /* [in] */ long hBitmap,
            /* [retval][out] */ IPictureDisp **retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GenerateHillShade )( 
            IUtils * This,
            /* [in] */ BSTR bstrGridFilename,
            /* [in] */ BSTR bstrShadeFilename,
            /* [defaultvalue][optional][in] */ float z,
            /* [defaultvalue][optional][in] */ float scale,
            /* [defaultvalue][optional][in] */ float az,
            /* [defaultvalue][optional][in] */ float alt,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GenerateContour )( 
            IUtils * This,
            /* [in] */ BSTR pszSrcFilename,
            /* [in] */ BSTR pszDstFilename,
            /* [in] */ double dfInterval,
            /* [defaultvalue][optional][in] */ double dfNoData,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL Is3D,
            /* [defaultvalue][optional][in] */ VARIANT dblFLArray,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *TranslateRaster )( 
            IUtils * This,
            /* [in] */ BSTR bstrSrcFilename,
            /* [in] */ BSTR bstrDstFilename,
            /* [in] */ BSTR bstrOptions,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *OGRLayerToShapefile )( 
            IUtils * This,
            /* [in] */ BSTR Filename,
            /* [defaultvalue][optional][in] */ ShpfileType shpType,
            /* [defaultvalue][optional][in] */ ICallback *cBack,
            /* [retval][out] */ IShapefile **sf);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *MergeImages )( 
            IUtils * This,
            /* [in] */ SAFEARRAY * InputNames,
            /* [in] */ BSTR OutputName,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ReprojectShapefile )( 
            IUtils * This,
            /* [in] */ IShapefile *sf,
            /* [in] */ IGeoProjection *source,
            /* [in] */ IGeoProjection *target,
            /* [retval][out] */ IShapefile **result);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ColorByName )( 
            IUtils * This,
            /* [in] */ tkMapColor name,
            /* [retval][out] */ OLE_COLOR *retVal);
        
        END_INTERFACE
    } IUtilsVtbl;

    interface IUtils
    {
        CONST_VTBL struct IUtilsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IUtils_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IUtils_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IUtils_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IUtils_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IUtils_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IUtils_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IUtils_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IUtils_PointInPolygon(This,Shp,TestPoint,retval)	\
    ( (This)->lpVtbl -> PointInPolygon(This,Shp,TestPoint,retval) ) 

#define IUtils_GridReplace(This,Grd,OldValue,NewValue,cBack,retval)	\
    ( (This)->lpVtbl -> GridReplace(This,Grd,OldValue,NewValue,cBack,retval) ) 

#define IUtils_GridInterpolateNoData(This,Grd,cBack,retval)	\
    ( (This)->lpVtbl -> GridInterpolateNoData(This,Grd,cBack,retval) ) 

#define IUtils_RemoveColinearPoints(This,Shapes,LinearTolerance,cBack,retval)	\
    ( (This)->lpVtbl -> RemoveColinearPoints(This,Shapes,LinearTolerance,cBack,retval) ) 

#define IUtils_get_Length(This,Shape,pVal)	\
    ( (This)->lpVtbl -> get_Length(This,Shape,pVal) ) 

#define IUtils_get_Perimeter(This,Shape,pVal)	\
    ( (This)->lpVtbl -> get_Perimeter(This,Shape,pVal) ) 

#define IUtils_get_Area(This,Shape,pVal)	\
    ( (This)->lpVtbl -> get_Area(This,Shape,pVal) ) 

#define IUtils_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IUtils_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IUtils_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IUtils_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IUtils_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IUtils_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IUtils_ClipPolygon(This,op,SubjectPolygon,ClipPolygon,retval)	\
    ( (This)->lpVtbl -> ClipPolygon(This,op,SubjectPolygon,ClipPolygon,retval) ) 

#define IUtils_GridMerge(This,Grids,MergeFilename,InRam,GrdFileType,cBack,retval)	\
    ( (This)->lpVtbl -> GridMerge(This,Grids,MergeFilename,InRam,GrdFileType,cBack,retval) ) 

#define IUtils_ShapeMerge(This,Shapes,IndexOne,IndexTwo,cBack,retval)	\
    ( (This)->lpVtbl -> ShapeMerge(This,Shapes,IndexOne,IndexTwo,cBack,retval) ) 

#define IUtils_GridToImage(This,Grid,cScheme,cBack,retval)	\
    ( (This)->lpVtbl -> GridToImage(This,Grid,cScheme,cBack,retval) ) 

#define IUtils_GridToShapefile(This,Grid,ConnectionGrid,cBack,retval)	\
    ( (This)->lpVtbl -> GridToShapefile(This,Grid,ConnectionGrid,cBack,retval) ) 

#define IUtils_GridToGrid(This,Grid,OutDataType,cBack,retval)	\
    ( (This)->lpVtbl -> GridToGrid(This,Grid,OutDataType,cBack,retval) ) 

#define IUtils_ShapeToShapeZ(This,Shapefile,Grid,cBack,retval)	\
    ( (This)->lpVtbl -> ShapeToShapeZ(This,Shapefile,Grid,cBack,retval) ) 

#define IUtils_TinToShapefile(This,Tin,Type,cBack,retval)	\
    ( (This)->lpVtbl -> TinToShapefile(This,Tin,Type,cBack,retval) ) 

#define IUtils_ShapefileToGrid(This,Shpfile,UseShapefileBounds,GrdHeader,Cellsize,UseShapeNumber,SingleValue,retval)	\
    ( (This)->lpVtbl -> ShapefileToGrid(This,Shpfile,UseShapefileBounds,GrdHeader,Cellsize,UseShapeNumber,SingleValue,retval) ) 

#define IUtils_hBitmapToPicture(This,hBitmap,retval)	\
    ( (This)->lpVtbl -> hBitmapToPicture(This,hBitmap,retval) ) 

#define IUtils_GenerateHillShade(This,bstrGridFilename,bstrShadeFilename,z,scale,az,alt,retval)	\
    ( (This)->lpVtbl -> GenerateHillShade(This,bstrGridFilename,bstrShadeFilename,z,scale,az,alt,retval) ) 

#define IUtils_GenerateContour(This,pszSrcFilename,pszDstFilename,dfInterval,dfNoData,Is3D,dblFLArray,cBack,retval)	\
    ( (This)->lpVtbl -> GenerateContour(This,pszSrcFilename,pszDstFilename,dfInterval,dfNoData,Is3D,dblFLArray,cBack,retval) ) 

#define IUtils_TranslateRaster(This,bstrSrcFilename,bstrDstFilename,bstrOptions,cBack,retval)	\
    ( (This)->lpVtbl -> TranslateRaster(This,bstrSrcFilename,bstrDstFilename,bstrOptions,cBack,retval) ) 

#define IUtils_OGRLayerToShapefile(This,Filename,shpType,cBack,sf)	\
    ( (This)->lpVtbl -> OGRLayerToShapefile(This,Filename,shpType,cBack,sf) ) 

#define IUtils_MergeImages(This,InputNames,OutputName,retVal)	\
    ( (This)->lpVtbl -> MergeImages(This,InputNames,OutputName,retVal) ) 

#define IUtils_ReprojectShapefile(This,sf,source,target,result)	\
    ( (This)->lpVtbl -> ReprojectShapefile(This,sf,source,target,result) ) 

#define IUtils_ColorByName(This,name,retVal)	\
    ( (This)->lpVtbl -> ColorByName(This,name,retVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IUtils_INTERFACE_DEFINED__ */


#ifndef __IVector_INTERFACE_DEFINED__
#define __IVector_INTERFACE_DEFINED__

/* interface IVector */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IVector;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("C60625AB-AD4C-405E-8CA2-62E36E4B3F73")
    IVector : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_i( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_i( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_j( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_j( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_k( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_k( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Normalize( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Dot( 
            /* [in] */ IVector *V,
            /* [retval][out] */ double *result) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CrossProduct( 
            /* [in] */ IVector *V,
            /* [retval][out] */ IVector **result) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IVectorVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IVector * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IVector * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IVector * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IVector * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IVector * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IVector * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IVector * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_i )( 
            IVector * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_i )( 
            IVector * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_j )( 
            IVector * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_j )( 
            IVector * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_k )( 
            IVector * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_k )( 
            IVector * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Normalize )( 
            IVector * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Dot )( 
            IVector * This,
            /* [in] */ IVector *V,
            /* [retval][out] */ double *result);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CrossProduct )( 
            IVector * This,
            /* [in] */ IVector *V,
            /* [retval][out] */ IVector **result);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IVector * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IVector * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IVector * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IVector * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IVector * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IVector * This,
            /* [in] */ BSTR newVal);
        
        END_INTERFACE
    } IVectorVtbl;

    interface IVector
    {
        CONST_VTBL struct IVectorVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IVector_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IVector_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IVector_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IVector_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IVector_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IVector_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IVector_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IVector_get_i(This,pVal)	\
    ( (This)->lpVtbl -> get_i(This,pVal) ) 

#define IVector_put_i(This,newVal)	\
    ( (This)->lpVtbl -> put_i(This,newVal) ) 

#define IVector_get_j(This,pVal)	\
    ( (This)->lpVtbl -> get_j(This,pVal) ) 

#define IVector_put_j(This,newVal)	\
    ( (This)->lpVtbl -> put_j(This,newVal) ) 

#define IVector_get_k(This,pVal)	\
    ( (This)->lpVtbl -> get_k(This,pVal) ) 

#define IVector_put_k(This,newVal)	\
    ( (This)->lpVtbl -> put_k(This,newVal) ) 

#define IVector_Normalize(This)	\
    ( (This)->lpVtbl -> Normalize(This) ) 

#define IVector_Dot(This,V,result)	\
    ( (This)->lpVtbl -> Dot(This,V,result) ) 

#define IVector_CrossProduct(This,V,result)	\
    ( (This)->lpVtbl -> CrossProduct(This,V,result) ) 

#define IVector_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IVector_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IVector_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IVector_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IVector_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IVector_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IVector_INTERFACE_DEFINED__ */


#ifndef __IGridColorScheme_INTERFACE_DEFINED__
#define __IGridColorScheme_INTERFACE_DEFINED__

/* interface IGridColorScheme */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IGridColorScheme;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("1C43B56D-2065-4953-9138-31AFE8470FF5")
    IGridColorScheme : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumBreaks( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_AmbientIntensity( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_AmbientIntensity( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LightSourceIntensity( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LightSourceIntensity( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LightSourceAzimuth( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LightSourceElevation( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetLightSource( 
            /* [in] */ double Azimuth,
            /* [in] */ double Elevation) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE InsertBreak( 
            /* [in] */ IGridColorBreak *BrkInfo) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Break( 
            /* [in] */ long Index,
            /* [retval][out] */ IGridColorBreak **pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DeleteBreak( 
            /* [in] */ long Index) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clear( void) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NoDataColor( 
            /* [retval][out] */ OLE_COLOR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_NoDataColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE UsePredefined( 
            /* [in] */ double LowValue,
            /* [in] */ double HighValue,
            /* [defaultvalue][optional][in] */ PredefinedColorScheme Preset = SummerMountains) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GetLightSource( 
            /* [retval][out] */ IVector **result) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE InsertAt( 
            /* [in] */ int Position,
            /* [in] */ IGridColorBreak *Break) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Serialize( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Deserialize( 
            /* [in] */ BSTR newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IGridColorSchemeVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IGridColorScheme * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IGridColorScheme * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IGridColorScheme * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IGridColorScheme * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IGridColorScheme * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IGridColorScheme * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IGridColorScheme * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumBreaks )( 
            IGridColorScheme * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_AmbientIntensity )( 
            IGridColorScheme * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_AmbientIntensity )( 
            IGridColorScheme * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LightSourceIntensity )( 
            IGridColorScheme * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LightSourceIntensity )( 
            IGridColorScheme * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LightSourceAzimuth )( 
            IGridColorScheme * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LightSourceElevation )( 
            IGridColorScheme * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetLightSource )( 
            IGridColorScheme * This,
            /* [in] */ double Azimuth,
            /* [in] */ double Elevation);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *InsertBreak )( 
            IGridColorScheme * This,
            /* [in] */ IGridColorBreak *BrkInfo);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Break )( 
            IGridColorScheme * This,
            /* [in] */ long Index,
            /* [retval][out] */ IGridColorBreak **pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DeleteBreak )( 
            IGridColorScheme * This,
            /* [in] */ long Index);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clear )( 
            IGridColorScheme * This);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NoDataColor )( 
            IGridColorScheme * This,
            /* [retval][out] */ OLE_COLOR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_NoDataColor )( 
            IGridColorScheme * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *UsePredefined )( 
            IGridColorScheme * This,
            /* [in] */ double LowValue,
            /* [in] */ double HighValue,
            /* [defaultvalue][optional][in] */ PredefinedColorScheme Preset);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GetLightSource )( 
            IGridColorScheme * This,
            /* [retval][out] */ IVector **result);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IGridColorScheme * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IGridColorScheme * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IGridColorScheme * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IGridColorScheme * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IGridColorScheme * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IGridColorScheme * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *InsertAt )( 
            IGridColorScheme * This,
            /* [in] */ int Position,
            /* [in] */ IGridColorBreak *Break);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Serialize )( 
            IGridColorScheme * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Deserialize )( 
            IGridColorScheme * This,
            /* [in] */ BSTR newVal);
        
        END_INTERFACE
    } IGridColorSchemeVtbl;

    interface IGridColorScheme
    {
        CONST_VTBL struct IGridColorSchemeVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IGridColorScheme_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IGridColorScheme_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IGridColorScheme_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IGridColorScheme_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IGridColorScheme_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IGridColorScheme_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IGridColorScheme_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IGridColorScheme_get_NumBreaks(This,pVal)	\
    ( (This)->lpVtbl -> get_NumBreaks(This,pVal) ) 

#define IGridColorScheme_get_AmbientIntensity(This,pVal)	\
    ( (This)->lpVtbl -> get_AmbientIntensity(This,pVal) ) 

#define IGridColorScheme_put_AmbientIntensity(This,newVal)	\
    ( (This)->lpVtbl -> put_AmbientIntensity(This,newVal) ) 

#define IGridColorScheme_get_LightSourceIntensity(This,pVal)	\
    ( (This)->lpVtbl -> get_LightSourceIntensity(This,pVal) ) 

#define IGridColorScheme_put_LightSourceIntensity(This,newVal)	\
    ( (This)->lpVtbl -> put_LightSourceIntensity(This,newVal) ) 

#define IGridColorScheme_get_LightSourceAzimuth(This,pVal)	\
    ( (This)->lpVtbl -> get_LightSourceAzimuth(This,pVal) ) 

#define IGridColorScheme_get_LightSourceElevation(This,pVal)	\
    ( (This)->lpVtbl -> get_LightSourceElevation(This,pVal) ) 

#define IGridColorScheme_SetLightSource(This,Azimuth,Elevation)	\
    ( (This)->lpVtbl -> SetLightSource(This,Azimuth,Elevation) ) 

#define IGridColorScheme_InsertBreak(This,BrkInfo)	\
    ( (This)->lpVtbl -> InsertBreak(This,BrkInfo) ) 

#define IGridColorScheme_get_Break(This,Index,pVal)	\
    ( (This)->lpVtbl -> get_Break(This,Index,pVal) ) 

#define IGridColorScheme_DeleteBreak(This,Index)	\
    ( (This)->lpVtbl -> DeleteBreak(This,Index) ) 

#define IGridColorScheme_Clear(This)	\
    ( (This)->lpVtbl -> Clear(This) ) 

#define IGridColorScheme_get_NoDataColor(This,pVal)	\
    ( (This)->lpVtbl -> get_NoDataColor(This,pVal) ) 

#define IGridColorScheme_put_NoDataColor(This,newVal)	\
    ( (This)->lpVtbl -> put_NoDataColor(This,newVal) ) 

#define IGridColorScheme_UsePredefined(This,LowValue,HighValue,Preset)	\
    ( (This)->lpVtbl -> UsePredefined(This,LowValue,HighValue,Preset) ) 

#define IGridColorScheme_GetLightSource(This,result)	\
    ( (This)->lpVtbl -> GetLightSource(This,result) ) 

#define IGridColorScheme_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IGridColorScheme_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IGridColorScheme_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IGridColorScheme_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IGridColorScheme_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IGridColorScheme_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IGridColorScheme_InsertAt(This,Position,Break)	\
    ( (This)->lpVtbl -> InsertAt(This,Position,Break) ) 

#define IGridColorScheme_Serialize(This,retVal)	\
    ( (This)->lpVtbl -> Serialize(This,retVal) ) 

#define IGridColorScheme_Deserialize(This,newVal)	\
    ( (This)->lpVtbl -> Deserialize(This,newVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IGridColorScheme_INTERFACE_DEFINED__ */


#ifndef __IGridColorBreak_INTERFACE_DEFINED__
#define __IGridColorBreak_INTERFACE_DEFINED__

/* interface IGridColorBreak */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IGridColorBreak;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("1C6ECF5D-04FA-43C4-97B1-22D5FFB55FBD")
    IGridColorBreak : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_HighColor( 
            /* [retval][out] */ OLE_COLOR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_HighColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LowColor( 
            /* [retval][out] */ OLE_COLOR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LowColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_HighValue( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_HighValue( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LowValue( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LowValue( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ColoringType( 
            /* [retval][out] */ ColoringType *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ColoringType( 
            /* [in] */ ColoringType newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GradientModel( 
            /* [retval][out] */ GradientModel *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GradientModel( 
            /* [in] */ GradientModel newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Caption( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Caption( 
            /* [in] */ BSTR newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IGridColorBreakVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IGridColorBreak * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IGridColorBreak * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IGridColorBreak * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IGridColorBreak * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IGridColorBreak * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IGridColorBreak * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IGridColorBreak * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_HighColor )( 
            IGridColorBreak * This,
            /* [retval][out] */ OLE_COLOR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_HighColor )( 
            IGridColorBreak * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LowColor )( 
            IGridColorBreak * This,
            /* [retval][out] */ OLE_COLOR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LowColor )( 
            IGridColorBreak * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_HighValue )( 
            IGridColorBreak * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_HighValue )( 
            IGridColorBreak * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LowValue )( 
            IGridColorBreak * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LowValue )( 
            IGridColorBreak * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ColoringType )( 
            IGridColorBreak * This,
            /* [retval][out] */ ColoringType *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ColoringType )( 
            IGridColorBreak * This,
            /* [in] */ ColoringType newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GradientModel )( 
            IGridColorBreak * This,
            /* [retval][out] */ GradientModel *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GradientModel )( 
            IGridColorBreak * This,
            /* [in] */ GradientModel newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IGridColorBreak * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IGridColorBreak * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IGridColorBreak * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IGridColorBreak * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IGridColorBreak * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IGridColorBreak * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Caption )( 
            IGridColorBreak * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Caption )( 
            IGridColorBreak * This,
            /* [in] */ BSTR newVal);
        
        END_INTERFACE
    } IGridColorBreakVtbl;

    interface IGridColorBreak
    {
        CONST_VTBL struct IGridColorBreakVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IGridColorBreak_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IGridColorBreak_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IGridColorBreak_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IGridColorBreak_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IGridColorBreak_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IGridColorBreak_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IGridColorBreak_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IGridColorBreak_get_HighColor(This,pVal)	\
    ( (This)->lpVtbl -> get_HighColor(This,pVal) ) 

#define IGridColorBreak_put_HighColor(This,newVal)	\
    ( (This)->lpVtbl -> put_HighColor(This,newVal) ) 

#define IGridColorBreak_get_LowColor(This,pVal)	\
    ( (This)->lpVtbl -> get_LowColor(This,pVal) ) 

#define IGridColorBreak_put_LowColor(This,newVal)	\
    ( (This)->lpVtbl -> put_LowColor(This,newVal) ) 

#define IGridColorBreak_get_HighValue(This,pVal)	\
    ( (This)->lpVtbl -> get_HighValue(This,pVal) ) 

#define IGridColorBreak_put_HighValue(This,newVal)	\
    ( (This)->lpVtbl -> put_HighValue(This,newVal) ) 

#define IGridColorBreak_get_LowValue(This,pVal)	\
    ( (This)->lpVtbl -> get_LowValue(This,pVal) ) 

#define IGridColorBreak_put_LowValue(This,newVal)	\
    ( (This)->lpVtbl -> put_LowValue(This,newVal) ) 

#define IGridColorBreak_get_ColoringType(This,pVal)	\
    ( (This)->lpVtbl -> get_ColoringType(This,pVal) ) 

#define IGridColorBreak_put_ColoringType(This,newVal)	\
    ( (This)->lpVtbl -> put_ColoringType(This,newVal) ) 

#define IGridColorBreak_get_GradientModel(This,pVal)	\
    ( (This)->lpVtbl -> get_GradientModel(This,pVal) ) 

#define IGridColorBreak_put_GradientModel(This,newVal)	\
    ( (This)->lpVtbl -> put_GradientModel(This,newVal) ) 

#define IGridColorBreak_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IGridColorBreak_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IGridColorBreak_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IGridColorBreak_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IGridColorBreak_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IGridColorBreak_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IGridColorBreak_get_Caption(This,pVal)	\
    ( (This)->lpVtbl -> get_Caption(This,pVal) ) 

#define IGridColorBreak_put_Caption(This,newVal)	\
    ( (This)->lpVtbl -> put_Caption(This,newVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IGridColorBreak_INTERFACE_DEFINED__ */


#ifndef __ITin_INTERFACE_DEFINED__
#define __ITin_INTERFACE_DEFINED__

/* interface ITin */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ITin;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("55DD824E-332E-41CA-B40C-C8DC81EE209C")
    ITin : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Open( 
            /* [in] */ BSTR TinFile,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CreateNew( 
            /* [in] */ IGrid *Grid,
            /* [in] */ double Deviation,
            /* [in] */ SplitMethod SplitTest,
            /* [in] */ double STParam,
            /* [in] */ long MeshDivisions,
            /* [defaultvalue][optional][in] */ long MaximumTriangles,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Save( 
            /* [in] */ BSTR TinFilename,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Close( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Select( 
            /* [out][in] */ long *TriangleHint,
            /* [in] */ double X,
            /* [in] */ double Y,
            /* [out] */ double *Z,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumTriangles( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumVertices( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CdlgFilter( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Triangle( 
            /* [in] */ long TriIndex,
            /* [out] */ long *vtx1Index,
            /* [out] */ long *vtx2Index,
            /* [out] */ long *vtx3Index) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Vertex( 
            /* [in] */ long VtxIndex,
            /* [out] */ double *X,
            /* [out] */ double *Y,
            /* [out] */ double *Z) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Max( 
            /* [out] */ double *X,
            /* [out] */ double *Y,
            /* [out] */ double *Z) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Min( 
            /* [out] */ double *X,
            /* [out] */ double *Y,
            /* [out] */ double *Z) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Filename( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IsNDTriangle( 
            /* [in] */ long TriIndex,
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE TriangleNeighbors( 
            /* [in] */ long TriIndex,
            /* [out][in] */ long *triIndex1,
            /* [out][in] */ long *triIndex2,
            /* [out][in] */ long *triIndex3) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CreateTinFromPoints( 
            /* [in] */ SAFEARRAY * Points,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ITinVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ITin * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ITin * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ITin * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ITin * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ITin * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ITin * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ITin * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Open )( 
            ITin * This,
            /* [in] */ BSTR TinFile,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CreateNew )( 
            ITin * This,
            /* [in] */ IGrid *Grid,
            /* [in] */ double Deviation,
            /* [in] */ SplitMethod SplitTest,
            /* [in] */ double STParam,
            /* [in] */ long MeshDivisions,
            /* [defaultvalue][optional][in] */ long MaximumTriangles,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Save )( 
            ITin * This,
            /* [in] */ BSTR TinFilename,
            /* [optional][in] */ ICallback *cBack,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Close )( 
            ITin * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Select )( 
            ITin * This,
            /* [out][in] */ long *TriangleHint,
            /* [in] */ double X,
            /* [in] */ double Y,
            /* [out] */ double *Z,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumTriangles )( 
            ITin * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumVertices )( 
            ITin * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            ITin * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            ITin * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CdlgFilter )( 
            ITin * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            ITin * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            ITin * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            ITin * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            ITin * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Triangle )( 
            ITin * This,
            /* [in] */ long TriIndex,
            /* [out] */ long *vtx1Index,
            /* [out] */ long *vtx2Index,
            /* [out] */ long *vtx3Index);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Vertex )( 
            ITin * This,
            /* [in] */ long VtxIndex,
            /* [out] */ double *X,
            /* [out] */ double *Y,
            /* [out] */ double *Z);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Max )( 
            ITin * This,
            /* [out] */ double *X,
            /* [out] */ double *Y,
            /* [out] */ double *Z);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Min )( 
            ITin * This,
            /* [out] */ double *X,
            /* [out] */ double *Y,
            /* [out] */ double *Z);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Filename )( 
            ITin * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsNDTriangle )( 
            ITin * This,
            /* [in] */ long TriIndex,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *TriangleNeighbors )( 
            ITin * This,
            /* [in] */ long TriIndex,
            /* [out][in] */ long *triIndex1,
            /* [out][in] */ long *triIndex2,
            /* [out][in] */ long *triIndex3);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CreateTinFromPoints )( 
            ITin * This,
            /* [in] */ SAFEARRAY * Points,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        END_INTERFACE
    } ITinVtbl;

    interface ITin
    {
        CONST_VTBL struct ITinVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ITin_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ITin_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ITin_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ITin_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ITin_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ITin_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ITin_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ITin_Open(This,TinFile,cBack,retval)	\
    ( (This)->lpVtbl -> Open(This,TinFile,cBack,retval) ) 

#define ITin_CreateNew(This,Grid,Deviation,SplitTest,STParam,MeshDivisions,MaximumTriangles,cBack,retval)	\
    ( (This)->lpVtbl -> CreateNew(This,Grid,Deviation,SplitTest,STParam,MeshDivisions,MaximumTriangles,cBack,retval) ) 

#define ITin_Save(This,TinFilename,cBack,retval)	\
    ( (This)->lpVtbl -> Save(This,TinFilename,cBack,retval) ) 

#define ITin_Close(This,retval)	\
    ( (This)->lpVtbl -> Close(This,retval) ) 

#define ITin_Select(This,TriangleHint,X,Y,Z,retval)	\
    ( (This)->lpVtbl -> Select(This,TriangleHint,X,Y,Z,retval) ) 

#define ITin_get_NumTriangles(This,pVal)	\
    ( (This)->lpVtbl -> get_NumTriangles(This,pVal) ) 

#define ITin_get_NumVertices(This,pVal)	\
    ( (This)->lpVtbl -> get_NumVertices(This,pVal) ) 

#define ITin_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define ITin_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define ITin_get_CdlgFilter(This,pVal)	\
    ( (This)->lpVtbl -> get_CdlgFilter(This,pVal) ) 

#define ITin_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define ITin_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define ITin_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define ITin_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define ITin_Triangle(This,TriIndex,vtx1Index,vtx2Index,vtx3Index)	\
    ( (This)->lpVtbl -> Triangle(This,TriIndex,vtx1Index,vtx2Index,vtx3Index) ) 

#define ITin_Vertex(This,VtxIndex,X,Y,Z)	\
    ( (This)->lpVtbl -> Vertex(This,VtxIndex,X,Y,Z) ) 

#define ITin_Max(This,X,Y,Z)	\
    ( (This)->lpVtbl -> Max(This,X,Y,Z) ) 

#define ITin_Min(This,X,Y,Z)	\
    ( (This)->lpVtbl -> Min(This,X,Y,Z) ) 

#define ITin_get_Filename(This,pVal)	\
    ( (This)->lpVtbl -> get_Filename(This,pVal) ) 

#define ITin_get_IsNDTriangle(This,TriIndex,pVal)	\
    ( (This)->lpVtbl -> get_IsNDTriangle(This,TriIndex,pVal) ) 

#define ITin_TriangleNeighbors(This,TriIndex,triIndex1,triIndex2,triIndex3)	\
    ( (This)->lpVtbl -> TriangleNeighbors(This,TriIndex,triIndex1,triIndex2,triIndex3) ) 

#define ITin_CreateTinFromPoints(This,Points,retval)	\
    ( (This)->lpVtbl -> CreateTinFromPoints(This,Points,retval) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ITin_INTERFACE_DEFINED__ */


#ifndef __IShapeDrawingOptions_INTERFACE_DEFINED__
#define __IShapeDrawingOptions_INTERFACE_DEFINED__

/* interface IShapeDrawingOptions */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IShapeDrawingOptions;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("7399B752-61D9-4A23-973F-1033431DD009")
    IShapeDrawingOptions : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FillVisible( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FillVisible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LineVisible( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LineVisible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LineTransparency( 
            /* [retval][out] */ float *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LineTransparency( 
            /* [in] */ float newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FillColor( 
            /* [retval][out] */ OLE_COLOR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FillColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LineColor( 
            /* [retval][out] */ OLE_COLOR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LineColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_DrawingMode( 
            /* [retval][out] */ tkVectorDrawingMode *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_DrawingMode( 
            /* [in] */ tkVectorDrawingMode newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FillHatchStyle( 
            /* [retval][out] */ tkGDIPlusHatchStyle *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FillHatchStyle( 
            /* [in] */ tkGDIPlusHatchStyle newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LineStipple( 
            /* [retval][out] */ tkDashStyle *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LineStipple( 
            /* [in] */ tkDashStyle newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PointShape( 
            /* [retval][out] */ tkPointShapeType *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PointShape( 
            /* [in] */ tkPointShapeType newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FillTransparency( 
            /* [retval][out] */ float *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FillTransparency( 
            /* [in] */ float newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LineWidth( 
            /* [retval][out] */ float *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LineWidth( 
            /* [in] */ float newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PointSize( 
            /* [retval][out] */ float *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PointSize( 
            /* [in] */ float newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FillBgTransparent( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FillBgTransparent( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FillBgColor( 
            /* [retval][out] */ OLE_COLOR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FillBgColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Picture( 
            /* [retval][out] */ IImage **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Picture( 
            /* [in] */ IImage *newValue) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Visible( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Visible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FillType( 
            /* [retval][out] */ tkFillType *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FillType( 
            /* [in] */ tkFillType newValue) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FillGradientType( 
            /* [retval][out] */ tkGradientType *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FillGradientType( 
            /* [in] */ tkGradientType newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PointType( 
            /* [retval][out] */ tkPointSymbolType *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PointType( 
            /* [in] */ tkPointSymbolType newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FillColor2( 
            /* [retval][out] */ OLE_COLOR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FillColor2( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PointRotation( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PointRotation( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PointSidesCount( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PointSidesCount( 
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PointSidesRatio( 
            /* [retval][out] */ float *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PointSidesRatio( 
            /* [in] */ float newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FillRotation( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FillRotation( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FillGradientBounds( 
            /* [retval][out] */ tkGradientBounds *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FillGradientBounds( 
            /* [in] */ tkGradientBounds newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PictureScaleX( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PictureScaleX( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PictureScaleY( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PictureScaleY( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DrawShape( 
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ IShape *shape,
            /* [in] */ VARIANT_BOOL drawVertices,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PointCharacter( 
            /* [retval][out] */ short *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PointCharacter( 
            /* [in] */ short newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontName( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontName( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clone( 
            /* [retval][out] */ IShapeDrawingOptions **retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DrawRectangle( 
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int width,
            /* [in] */ int height,
            /* [in] */ VARIANT_BOOL drawVertices,
            /* [defaultvalue][optional][in] */ int clipWidth,
            /* [defaultvalue][optional][in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DrawPoint( 
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [defaultvalue][optional][in] */ int clipWidth,
            /* [defaultvalue][optional][in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_VerticesVisible( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_VerticesVisible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_VerticesType( 
            /* [retval][out] */ tkVertexType *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_VerticesType( 
            /* [in] */ tkVertexType newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_VerticesColor( 
            /* [retval][out] */ OLE_COLOR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_VerticesColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_VerticesSize( 
            /* [retval][out] */ LONG *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_VerticesSize( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_VerticesFillVisible( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_VerticesFillVisible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DrawLine( 
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int width,
            /* [in] */ int height,
            /* [in] */ VARIANT_BOOL drawVertices,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LinePattern( 
            /* [retval][out] */ ILinePattern **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LinePattern( 
            /* [in] */ ILinePattern *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Tag( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Tag( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetGradientFill( 
            /* [in] */ OLE_COLOR color,
            /* [in] */ short range) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetDefaultPointSymbol( 
            /* [in] */ tkDefaultPointSymbol symbol) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_UseLinePattern( 
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_UseLinePattern( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Serialize( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Deserialize( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DrawPointVB( 
            /* [in] */ int hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [defaultvalue][optional][in] */ int clipWidth,
            /* [defaultvalue][optional][in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DrawLineVB( 
            /* [in] */ int hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int width,
            /* [in] */ int height,
            /* [in] */ VARIANT_BOOL drawVertices,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DrawRectangleVB( 
            /* [in] */ int hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int width,
            /* [in] */ int height,
            /* [in] */ VARIANT_BOOL drawVertices,
            /* [defaultvalue][optional][in] */ int clipWidth,
            /* [defaultvalue][optional][in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DrawShapeVB( 
            /* [in] */ int hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ IShape *shape,
            /* [in] */ VARIANT_BOOL drawVertices,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IShapeDrawingOptionsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IShapeDrawingOptions * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IShapeDrawingOptions * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IShapeDrawingOptions * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IShapeDrawingOptions * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IShapeDrawingOptions * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IShapeDrawingOptions * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IShapeDrawingOptions * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FillVisible )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FillVisible )( 
            IShapeDrawingOptions * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LineVisible )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LineVisible )( 
            IShapeDrawingOptions * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LineTransparency )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ float *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LineTransparency )( 
            IShapeDrawingOptions * This,
            /* [in] */ float newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FillColor )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ OLE_COLOR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FillColor )( 
            IShapeDrawingOptions * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LineColor )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ OLE_COLOR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LineColor )( 
            IShapeDrawingOptions * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DrawingMode )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ tkVectorDrawingMode *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DrawingMode )( 
            IShapeDrawingOptions * This,
            /* [in] */ tkVectorDrawingMode newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FillHatchStyle )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ tkGDIPlusHatchStyle *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FillHatchStyle )( 
            IShapeDrawingOptions * This,
            /* [in] */ tkGDIPlusHatchStyle newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LineStipple )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ tkDashStyle *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LineStipple )( 
            IShapeDrawingOptions * This,
            /* [in] */ tkDashStyle newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PointShape )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ tkPointShapeType *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PointShape )( 
            IShapeDrawingOptions * This,
            /* [in] */ tkPointShapeType newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FillTransparency )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ float *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FillTransparency )( 
            IShapeDrawingOptions * This,
            /* [in] */ float newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LineWidth )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ float *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LineWidth )( 
            IShapeDrawingOptions * This,
            /* [in] */ float newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PointSize )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ float *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PointSize )( 
            IShapeDrawingOptions * This,
            /* [in] */ float newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FillBgTransparent )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FillBgTransparent )( 
            IShapeDrawingOptions * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FillBgColor )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ OLE_COLOR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FillBgColor )( 
            IShapeDrawingOptions * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Picture )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ IImage **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Picture )( 
            IShapeDrawingOptions * This,
            /* [in] */ IImage *newValue);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Visible )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Visible )( 
            IShapeDrawingOptions * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FillType )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ tkFillType *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FillType )( 
            IShapeDrawingOptions * This,
            /* [in] */ tkFillType newValue);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FillGradientType )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ tkGradientType *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FillGradientType )( 
            IShapeDrawingOptions * This,
            /* [in] */ tkGradientType newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PointType )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ tkPointSymbolType *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PointType )( 
            IShapeDrawingOptions * This,
            /* [in] */ tkPointSymbolType newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FillColor2 )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ OLE_COLOR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FillColor2 )( 
            IShapeDrawingOptions * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PointRotation )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PointRotation )( 
            IShapeDrawingOptions * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PointSidesCount )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PointSidesCount )( 
            IShapeDrawingOptions * This,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PointSidesRatio )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ float *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PointSidesRatio )( 
            IShapeDrawingOptions * This,
            /* [in] */ float newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FillRotation )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FillRotation )( 
            IShapeDrawingOptions * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FillGradientBounds )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ tkGradientBounds *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FillGradientBounds )( 
            IShapeDrawingOptions * This,
            /* [in] */ tkGradientBounds newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PictureScaleX )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PictureScaleX )( 
            IShapeDrawingOptions * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PictureScaleY )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PictureScaleY )( 
            IShapeDrawingOptions * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DrawShape )( 
            IShapeDrawingOptions * This,
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ IShape *shape,
            /* [in] */ VARIANT_BOOL drawVertices,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PointCharacter )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ short *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PointCharacter )( 
            IShapeDrawingOptions * This,
            /* [in] */ short newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontName )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontName )( 
            IShapeDrawingOptions * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clone )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ IShapeDrawingOptions **retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IShapeDrawingOptions * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DrawRectangle )( 
            IShapeDrawingOptions * This,
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int width,
            /* [in] */ int height,
            /* [in] */ VARIANT_BOOL drawVertices,
            /* [defaultvalue][optional][in] */ int clipWidth,
            /* [defaultvalue][optional][in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DrawPoint )( 
            IShapeDrawingOptions * This,
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [defaultvalue][optional][in] */ int clipWidth,
            /* [defaultvalue][optional][in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_VerticesVisible )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_VerticesVisible )( 
            IShapeDrawingOptions * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_VerticesType )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ tkVertexType *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_VerticesType )( 
            IShapeDrawingOptions * This,
            /* [in] */ tkVertexType newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_VerticesColor )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ OLE_COLOR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_VerticesColor )( 
            IShapeDrawingOptions * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_VerticesSize )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ LONG *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_VerticesSize )( 
            IShapeDrawingOptions * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_VerticesFillVisible )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_VerticesFillVisible )( 
            IShapeDrawingOptions * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DrawLine )( 
            IShapeDrawingOptions * This,
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int width,
            /* [in] */ int height,
            /* [in] */ VARIANT_BOOL drawVertices,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LinePattern )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ ILinePattern **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LinePattern )( 
            IShapeDrawingOptions * This,
            /* [in] */ ILinePattern *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Tag )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Tag )( 
            IShapeDrawingOptions * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetGradientFill )( 
            IShapeDrawingOptions * This,
            /* [in] */ OLE_COLOR color,
            /* [in] */ short range);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetDefaultPointSymbol )( 
            IShapeDrawingOptions * This,
            /* [in] */ tkDefaultPointSymbol symbol);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_UseLinePattern )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_UseLinePattern )( 
            IShapeDrawingOptions * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Serialize )( 
            IShapeDrawingOptions * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Deserialize )( 
            IShapeDrawingOptions * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DrawPointVB )( 
            IShapeDrawingOptions * This,
            /* [in] */ int hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [defaultvalue][optional][in] */ int clipWidth,
            /* [defaultvalue][optional][in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DrawLineVB )( 
            IShapeDrawingOptions * This,
            /* [in] */ int hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int width,
            /* [in] */ int height,
            /* [in] */ VARIANT_BOOL drawVertices,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DrawRectangleVB )( 
            IShapeDrawingOptions * This,
            /* [in] */ int hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int width,
            /* [in] */ int height,
            /* [in] */ VARIANT_BOOL drawVertices,
            /* [defaultvalue][optional][in] */ int clipWidth,
            /* [defaultvalue][optional][in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DrawShapeVB )( 
            IShapeDrawingOptions * This,
            /* [in] */ int hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ IShape *shape,
            /* [in] */ VARIANT_BOOL drawVertices,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        END_INTERFACE
    } IShapeDrawingOptionsVtbl;

    interface IShapeDrawingOptions
    {
        CONST_VTBL struct IShapeDrawingOptionsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IShapeDrawingOptions_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IShapeDrawingOptions_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IShapeDrawingOptions_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IShapeDrawingOptions_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IShapeDrawingOptions_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IShapeDrawingOptions_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IShapeDrawingOptions_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IShapeDrawingOptions_get_FillVisible(This,pVal)	\
    ( (This)->lpVtbl -> get_FillVisible(This,pVal) ) 

#define IShapeDrawingOptions_put_FillVisible(This,newVal)	\
    ( (This)->lpVtbl -> put_FillVisible(This,newVal) ) 

#define IShapeDrawingOptions_get_LineVisible(This,pVal)	\
    ( (This)->lpVtbl -> get_LineVisible(This,pVal) ) 

#define IShapeDrawingOptions_put_LineVisible(This,newVal)	\
    ( (This)->lpVtbl -> put_LineVisible(This,newVal) ) 

#define IShapeDrawingOptions_get_LineTransparency(This,pVal)	\
    ( (This)->lpVtbl -> get_LineTransparency(This,pVal) ) 

#define IShapeDrawingOptions_put_LineTransparency(This,newVal)	\
    ( (This)->lpVtbl -> put_LineTransparency(This,newVal) ) 

#define IShapeDrawingOptions_get_FillColor(This,pVal)	\
    ( (This)->lpVtbl -> get_FillColor(This,pVal) ) 

#define IShapeDrawingOptions_put_FillColor(This,newVal)	\
    ( (This)->lpVtbl -> put_FillColor(This,newVal) ) 

#define IShapeDrawingOptions_get_LineColor(This,pVal)	\
    ( (This)->lpVtbl -> get_LineColor(This,pVal) ) 

#define IShapeDrawingOptions_put_LineColor(This,newVal)	\
    ( (This)->lpVtbl -> put_LineColor(This,newVal) ) 

#define IShapeDrawingOptions_get_DrawingMode(This,pVal)	\
    ( (This)->lpVtbl -> get_DrawingMode(This,pVal) ) 

#define IShapeDrawingOptions_put_DrawingMode(This,newVal)	\
    ( (This)->lpVtbl -> put_DrawingMode(This,newVal) ) 

#define IShapeDrawingOptions_get_FillHatchStyle(This,pVal)	\
    ( (This)->lpVtbl -> get_FillHatchStyle(This,pVal) ) 

#define IShapeDrawingOptions_put_FillHatchStyle(This,newVal)	\
    ( (This)->lpVtbl -> put_FillHatchStyle(This,newVal) ) 

#define IShapeDrawingOptions_get_LineStipple(This,pVal)	\
    ( (This)->lpVtbl -> get_LineStipple(This,pVal) ) 

#define IShapeDrawingOptions_put_LineStipple(This,newVal)	\
    ( (This)->lpVtbl -> put_LineStipple(This,newVal) ) 

#define IShapeDrawingOptions_get_PointShape(This,pVal)	\
    ( (This)->lpVtbl -> get_PointShape(This,pVal) ) 

#define IShapeDrawingOptions_put_PointShape(This,newVal)	\
    ( (This)->lpVtbl -> put_PointShape(This,newVal) ) 

#define IShapeDrawingOptions_get_FillTransparency(This,pVal)	\
    ( (This)->lpVtbl -> get_FillTransparency(This,pVal) ) 

#define IShapeDrawingOptions_put_FillTransparency(This,newVal)	\
    ( (This)->lpVtbl -> put_FillTransparency(This,newVal) ) 

#define IShapeDrawingOptions_get_LineWidth(This,pVal)	\
    ( (This)->lpVtbl -> get_LineWidth(This,pVal) ) 

#define IShapeDrawingOptions_put_LineWidth(This,newVal)	\
    ( (This)->lpVtbl -> put_LineWidth(This,newVal) ) 

#define IShapeDrawingOptions_get_PointSize(This,pVal)	\
    ( (This)->lpVtbl -> get_PointSize(This,pVal) ) 

#define IShapeDrawingOptions_put_PointSize(This,newVal)	\
    ( (This)->lpVtbl -> put_PointSize(This,newVal) ) 

#define IShapeDrawingOptions_get_FillBgTransparent(This,pVal)	\
    ( (This)->lpVtbl -> get_FillBgTransparent(This,pVal) ) 

#define IShapeDrawingOptions_put_FillBgTransparent(This,newVal)	\
    ( (This)->lpVtbl -> put_FillBgTransparent(This,newVal) ) 

#define IShapeDrawingOptions_get_FillBgColor(This,pVal)	\
    ( (This)->lpVtbl -> get_FillBgColor(This,pVal) ) 

#define IShapeDrawingOptions_put_FillBgColor(This,newVal)	\
    ( (This)->lpVtbl -> put_FillBgColor(This,newVal) ) 

#define IShapeDrawingOptions_get_Picture(This,pVal)	\
    ( (This)->lpVtbl -> get_Picture(This,pVal) ) 

#define IShapeDrawingOptions_put_Picture(This,newValue)	\
    ( (This)->lpVtbl -> put_Picture(This,newValue) ) 

#define IShapeDrawingOptions_get_Visible(This,pVal)	\
    ( (This)->lpVtbl -> get_Visible(This,pVal) ) 

#define IShapeDrawingOptions_put_Visible(This,newVal)	\
    ( (This)->lpVtbl -> put_Visible(This,newVal) ) 

#define IShapeDrawingOptions_get_FillType(This,pVal)	\
    ( (This)->lpVtbl -> get_FillType(This,pVal) ) 

#define IShapeDrawingOptions_put_FillType(This,newValue)	\
    ( (This)->lpVtbl -> put_FillType(This,newValue) ) 

#define IShapeDrawingOptions_get_FillGradientType(This,pVal)	\
    ( (This)->lpVtbl -> get_FillGradientType(This,pVal) ) 

#define IShapeDrawingOptions_put_FillGradientType(This,newVal)	\
    ( (This)->lpVtbl -> put_FillGradientType(This,newVal) ) 

#define IShapeDrawingOptions_get_PointType(This,pVal)	\
    ( (This)->lpVtbl -> get_PointType(This,pVal) ) 

#define IShapeDrawingOptions_put_PointType(This,newVal)	\
    ( (This)->lpVtbl -> put_PointType(This,newVal) ) 

#define IShapeDrawingOptions_get_FillColor2(This,pVal)	\
    ( (This)->lpVtbl -> get_FillColor2(This,pVal) ) 

#define IShapeDrawingOptions_put_FillColor2(This,newVal)	\
    ( (This)->lpVtbl -> put_FillColor2(This,newVal) ) 

#define IShapeDrawingOptions_get_PointRotation(This,pVal)	\
    ( (This)->lpVtbl -> get_PointRotation(This,pVal) ) 

#define IShapeDrawingOptions_put_PointRotation(This,newVal)	\
    ( (This)->lpVtbl -> put_PointRotation(This,newVal) ) 

#define IShapeDrawingOptions_get_PointSidesCount(This,pVal)	\
    ( (This)->lpVtbl -> get_PointSidesCount(This,pVal) ) 

#define IShapeDrawingOptions_put_PointSidesCount(This,newVal)	\
    ( (This)->lpVtbl -> put_PointSidesCount(This,newVal) ) 

#define IShapeDrawingOptions_get_PointSidesRatio(This,pVal)	\
    ( (This)->lpVtbl -> get_PointSidesRatio(This,pVal) ) 

#define IShapeDrawingOptions_put_PointSidesRatio(This,newVal)	\
    ( (This)->lpVtbl -> put_PointSidesRatio(This,newVal) ) 

#define IShapeDrawingOptions_get_FillRotation(This,pVal)	\
    ( (This)->lpVtbl -> get_FillRotation(This,pVal) ) 

#define IShapeDrawingOptions_put_FillRotation(This,newVal)	\
    ( (This)->lpVtbl -> put_FillRotation(This,newVal) ) 

#define IShapeDrawingOptions_get_FillGradientBounds(This,pVal)	\
    ( (This)->lpVtbl -> get_FillGradientBounds(This,pVal) ) 

#define IShapeDrawingOptions_put_FillGradientBounds(This,newVal)	\
    ( (This)->lpVtbl -> put_FillGradientBounds(This,newVal) ) 

#define IShapeDrawingOptions_get_PictureScaleX(This,pVal)	\
    ( (This)->lpVtbl -> get_PictureScaleX(This,pVal) ) 

#define IShapeDrawingOptions_put_PictureScaleX(This,newVal)	\
    ( (This)->lpVtbl -> put_PictureScaleX(This,newVal) ) 

#define IShapeDrawingOptions_get_PictureScaleY(This,pVal)	\
    ( (This)->lpVtbl -> get_PictureScaleY(This,pVal) ) 

#define IShapeDrawingOptions_put_PictureScaleY(This,newVal)	\
    ( (This)->lpVtbl -> put_PictureScaleY(This,newVal) ) 

#define IShapeDrawingOptions_DrawShape(This,hdc,x,y,shape,drawVertices,clipWidth,clipHeight,backColor,retVal)	\
    ( (This)->lpVtbl -> DrawShape(This,hdc,x,y,shape,drawVertices,clipWidth,clipHeight,backColor,retVal) ) 

#define IShapeDrawingOptions_get_PointCharacter(This,pVal)	\
    ( (This)->lpVtbl -> get_PointCharacter(This,pVal) ) 

#define IShapeDrawingOptions_put_PointCharacter(This,newVal)	\
    ( (This)->lpVtbl -> put_PointCharacter(This,newVal) ) 

#define IShapeDrawingOptions_get_FontName(This,pVal)	\
    ( (This)->lpVtbl -> get_FontName(This,pVal) ) 

#define IShapeDrawingOptions_put_FontName(This,newVal)	\
    ( (This)->lpVtbl -> put_FontName(This,newVal) ) 

#define IShapeDrawingOptions_Clone(This,retval)	\
    ( (This)->lpVtbl -> Clone(This,retval) ) 

#define IShapeDrawingOptions_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IShapeDrawingOptions_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IShapeDrawingOptions_DrawRectangle(This,hdc,x,y,width,height,drawVertices,clipWidth,clipHeight,backColor,retVal)	\
    ( (This)->lpVtbl -> DrawRectangle(This,hdc,x,y,width,height,drawVertices,clipWidth,clipHeight,backColor,retVal) ) 

#define IShapeDrawingOptions_DrawPoint(This,hdc,x,y,clipWidth,clipHeight,backColor,retVal)	\
    ( (This)->lpVtbl -> DrawPoint(This,hdc,x,y,clipWidth,clipHeight,backColor,retVal) ) 

#define IShapeDrawingOptions_get_VerticesVisible(This,pVal)	\
    ( (This)->lpVtbl -> get_VerticesVisible(This,pVal) ) 

#define IShapeDrawingOptions_put_VerticesVisible(This,newVal)	\
    ( (This)->lpVtbl -> put_VerticesVisible(This,newVal) ) 

#define IShapeDrawingOptions_get_VerticesType(This,pVal)	\
    ( (This)->lpVtbl -> get_VerticesType(This,pVal) ) 

#define IShapeDrawingOptions_put_VerticesType(This,newVal)	\
    ( (This)->lpVtbl -> put_VerticesType(This,newVal) ) 

#define IShapeDrawingOptions_get_VerticesColor(This,pVal)	\
    ( (This)->lpVtbl -> get_VerticesColor(This,pVal) ) 

#define IShapeDrawingOptions_put_VerticesColor(This,newVal)	\
    ( (This)->lpVtbl -> put_VerticesColor(This,newVal) ) 

#define IShapeDrawingOptions_get_VerticesSize(This,pVal)	\
    ( (This)->lpVtbl -> get_VerticesSize(This,pVal) ) 

#define IShapeDrawingOptions_put_VerticesSize(This,newVal)	\
    ( (This)->lpVtbl -> put_VerticesSize(This,newVal) ) 

#define IShapeDrawingOptions_get_VerticesFillVisible(This,pVal)	\
    ( (This)->lpVtbl -> get_VerticesFillVisible(This,pVal) ) 

#define IShapeDrawingOptions_put_VerticesFillVisible(This,newVal)	\
    ( (This)->lpVtbl -> put_VerticesFillVisible(This,newVal) ) 

#define IShapeDrawingOptions_DrawLine(This,hdc,x,y,width,height,drawVertices,clipWidth,clipHeight,backColor,retVal)	\
    ( (This)->lpVtbl -> DrawLine(This,hdc,x,y,width,height,drawVertices,clipWidth,clipHeight,backColor,retVal) ) 

#define IShapeDrawingOptions_get_LinePattern(This,pVal)	\
    ( (This)->lpVtbl -> get_LinePattern(This,pVal) ) 

#define IShapeDrawingOptions_put_LinePattern(This,newVal)	\
    ( (This)->lpVtbl -> put_LinePattern(This,newVal) ) 

#define IShapeDrawingOptions_get_Tag(This,retVal)	\
    ( (This)->lpVtbl -> get_Tag(This,retVal) ) 

#define IShapeDrawingOptions_put_Tag(This,newVal)	\
    ( (This)->lpVtbl -> put_Tag(This,newVal) ) 

#define IShapeDrawingOptions_SetGradientFill(This,color,range)	\
    ( (This)->lpVtbl -> SetGradientFill(This,color,range) ) 

#define IShapeDrawingOptions_SetDefaultPointSymbol(This,symbol)	\
    ( (This)->lpVtbl -> SetDefaultPointSymbol(This,symbol) ) 

#define IShapeDrawingOptions_get_UseLinePattern(This,retVal)	\
    ( (This)->lpVtbl -> get_UseLinePattern(This,retVal) ) 

#define IShapeDrawingOptions_put_UseLinePattern(This,newVal)	\
    ( (This)->lpVtbl -> put_UseLinePattern(This,newVal) ) 

#define IShapeDrawingOptions_Serialize(This,retVal)	\
    ( (This)->lpVtbl -> Serialize(This,retVal) ) 

#define IShapeDrawingOptions_Deserialize(This,newVal)	\
    ( (This)->lpVtbl -> Deserialize(This,newVal) ) 

#define IShapeDrawingOptions_DrawPointVB(This,hdc,x,y,clipWidth,clipHeight,backColor,retVal)	\
    ( (This)->lpVtbl -> DrawPointVB(This,hdc,x,y,clipWidth,clipHeight,backColor,retVal) ) 

#define IShapeDrawingOptions_DrawLineVB(This,hdc,x,y,width,height,drawVertices,clipWidth,clipHeight,backColor,retVal)	\
    ( (This)->lpVtbl -> DrawLineVB(This,hdc,x,y,width,height,drawVertices,clipWidth,clipHeight,backColor,retVal) ) 

#define IShapeDrawingOptions_DrawRectangleVB(This,hdc,x,y,width,height,drawVertices,clipWidth,clipHeight,backColor,retVal)	\
    ( (This)->lpVtbl -> DrawRectangleVB(This,hdc,x,y,width,height,drawVertices,clipWidth,clipHeight,backColor,retVal) ) 

#define IShapeDrawingOptions_DrawShapeVB(This,hdc,x,y,shape,drawVertices,clipWidth,clipHeight,backColor,retVal)	\
    ( (This)->lpVtbl -> DrawShapeVB(This,hdc,x,y,shape,drawVertices,clipWidth,clipHeight,backColor,retVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IShapeDrawingOptions_INTERFACE_DEFINED__ */


#ifndef __ILabel_INTERFACE_DEFINED__
#define __ILabel_INTERFACE_DEFINED__

/* interface ILabel */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ILabel;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4B341A36-CFA6-4421-9D08-FD5B06097307")
    ILabel : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Visible( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Visible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Rotation( 
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Rotation( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Text( 
            /* [retval][out] */ BSTR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Text( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_X( 
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_X( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Y( 
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Y( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IsDrawn( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Category( 
            /* [retval][out] */ long *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Category( 
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ScreenExtents( 
            /* [retval][out] */ IExtents **retval) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ILabelVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ILabel * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ILabel * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ILabel * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ILabel * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ILabel * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ILabel * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ILabel * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Visible )( 
            ILabel * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Visible )( 
            ILabel * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Rotation )( 
            ILabel * This,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Rotation )( 
            ILabel * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Text )( 
            ILabel * This,
            /* [retval][out] */ BSTR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Text )( 
            ILabel * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_X )( 
            ILabel * This,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_X )( 
            ILabel * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Y )( 
            ILabel * This,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Y )( 
            ILabel * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsDrawn )( 
            ILabel * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Category )( 
            ILabel * This,
            /* [retval][out] */ long *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Category )( 
            ILabel * This,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ScreenExtents )( 
            ILabel * This,
            /* [retval][out] */ IExtents **retval);
        
        END_INTERFACE
    } ILabelVtbl;

    interface ILabel
    {
        CONST_VTBL struct ILabelVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ILabel_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ILabel_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ILabel_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ILabel_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ILabel_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ILabel_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ILabel_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ILabel_get_Visible(This,retval)	\
    ( (This)->lpVtbl -> get_Visible(This,retval) ) 

#define ILabel_put_Visible(This,newVal)	\
    ( (This)->lpVtbl -> put_Visible(This,newVal) ) 

#define ILabel_get_Rotation(This,retval)	\
    ( (This)->lpVtbl -> get_Rotation(This,retval) ) 

#define ILabel_put_Rotation(This,newVal)	\
    ( (This)->lpVtbl -> put_Rotation(This,newVal) ) 

#define ILabel_get_Text(This,retval)	\
    ( (This)->lpVtbl -> get_Text(This,retval) ) 

#define ILabel_put_Text(This,newVal)	\
    ( (This)->lpVtbl -> put_Text(This,newVal) ) 

#define ILabel_get_X(This,retval)	\
    ( (This)->lpVtbl -> get_X(This,retval) ) 

#define ILabel_put_X(This,newVal)	\
    ( (This)->lpVtbl -> put_X(This,newVal) ) 

#define ILabel_get_Y(This,retval)	\
    ( (This)->lpVtbl -> get_Y(This,retval) ) 

#define ILabel_put_Y(This,newVal)	\
    ( (This)->lpVtbl -> put_Y(This,newVal) ) 

#define ILabel_get_IsDrawn(This,retval)	\
    ( (This)->lpVtbl -> get_IsDrawn(This,retval) ) 

#define ILabel_get_Category(This,retval)	\
    ( (This)->lpVtbl -> get_Category(This,retval) ) 

#define ILabel_put_Category(This,newVal)	\
    ( (This)->lpVtbl -> put_Category(This,newVal) ) 

#define ILabel_get_ScreenExtents(This,retval)	\
    ( (This)->lpVtbl -> get_ScreenExtents(This,retval) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ILabel_INTERFACE_DEFINED__ */


#ifndef __ILabels_INTERFACE_DEFINED__
#define __ILabels_INTERFACE_DEFINED__

/* interface ILabels */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ILabels;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("A73AF37E-3A6A-4532-B48F-FA53309FA117")
    ILabels : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_VerticalPosition( 
            /* [retval][out] */ tkVerticalPosition *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_VerticalPosition( 
            /* [in] */ tkVerticalPosition newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Category( 
            /* [in] */ long Index,
            /* [retval][out] */ ILabelCategory **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Category( 
            /* [in] */ long Index,
            /* [in] */ ILabelCategory *newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AddLabel( 
            /* [in] */ BSTR Text,
            /* [in] */ double x,
            /* [in] */ double y,
            /* [defaultvalue][optional][in] */ double Rotation = 0,
            /* [defaultvalue][optional][in] */ long Category = -1) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE InsertLabel( 
            /* [in] */ long Index,
            /* [in] */ BSTR Text,
            /* [in] */ double x,
            /* [in] */ double y,
            /* [defaultvalue][optional][in] */ double Rotation,
            /* [defaultvalue][optional][in] */ long Category,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE RemoveLabel( 
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AddPart( 
            /* [in] */ long index,
            /* [in] */ BSTR Text,
            /* [in] */ double x,
            /* [in] */ double y,
            /* [defaultvalue][optional][in] */ double Rotation = 0,
            /* [defaultvalue][optional][in] */ long Category = -1) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE InsertPart( 
            /* [in] */ long Index,
            /* [in] */ long Part,
            /* [in] */ BSTR Text,
            /* [in] */ double x,
            /* [in] */ double y,
            /* [defaultvalue][optional][in] */ double Rotation,
            /* [defaultvalue][optional][in] */ long Category,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE RemovePart( 
            /* [in] */ long Index,
            /* [in] */ long Part,
            /* [retval][out] */ VARIANT_BOOL *vbretval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AddCategory( 
            /* [in] */ BSTR Name,
            /* [retval][out] */ ILabelCategory **retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE InsertCategory( 
            /* [in] */ long Index,
            /* [in] */ BSTR Name,
            /* [retval][out] */ ILabelCategory **retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE RemoveCategory( 
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *vbretval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clear( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ClearCategories( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Select( 
            /* [in] */ IExtents *BoundingBox,
            /* [defaultvalue][optional][in] */ long Tolerance,
            /* [defaultvalue][optional][in] */ SelectMode SelectMode,
            /* [out][optional][in] */ VARIANT *LabelIndices,
            /* [out][optional][in] */ VARIANT *PartIndices,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Count( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumParts( 
            /* [in] */ long Index,
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumCategories( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Label( 
            /* [in] */ long Index,
            /* [in] */ long Part,
            /* [retval][out] */ ILabel **pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Synchronized( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Synchronized( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ScaleLabels( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ScaleLabels( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_BasicScale( 
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_BasicScale( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MaxVisibleScale( 
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MaxVisibleScale( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MinVisibleScale( 
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MinVisibleScale( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_DynamicVisibility( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_DynamicVisibility( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_AvoidCollisions( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_AvoidCollisions( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CollisionBuffer( 
            /* [retval][out] */ long *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_CollisionBuffer( 
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_UseWidthLimits( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_UseWidthLimits( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_RemoveDuplicates( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_RemoveDuplicates( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_UseGdiPlus( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_UseGdiPlus( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Visible( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Visible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_OffsetX( 
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_OffsetX( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_OffsetY( 
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_OffsetY( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Alignment( 
            /* [retval][out] */ tkLabelAlignment *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Alignment( 
            /* [in] */ tkLabelAlignment newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LineOrientation( 
            /* [retval][out] */ tkLineLabelOrientation *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LineOrientation( 
            /* [in] */ tkLineLabelOrientation newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontName( 
            /* [retval][out] */ BSTR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontName( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontSize( 
            /* [retval][out] */ long *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontSize( 
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontItalic( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontItalic( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontBold( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontBold( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontUnderline( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontUnderline( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontStrikeOut( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontStrikeOut( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontColor2( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontColor2( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontGradientMode( 
            /* [retval][out] */ tkLinearGradientMode *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontGradientMode( 
            /* [in] */ tkLinearGradientMode newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontTransparency( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontTransparency( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontOutlineVisible( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontOutlineVisible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShadowVisible( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ShadowVisible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_HaloVisible( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_HaloVisible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontOutlineColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontOutlineColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShadowColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ShadowColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_HaloColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_HaloColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontOutlineWidth( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontOutlineWidth( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShadowOffsetX( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ShadowOffsetX( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShadowOffsetY( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ShadowOffsetY( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_HaloSize( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_HaloSize( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameType( 
            /* [retval][out] */ tkLabelFrameType *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameType( 
            /* [in] */ tkLabelFrameType newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameOutlineColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameOutlineColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameBackColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameBackColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameBackColor2( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameBackColor2( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameGradientMode( 
            /* [retval][out] */ tkLinearGradientMode *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameGradientMode( 
            /* [in] */ tkLinearGradientMode newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameOutlineStyle( 
            /* [retval][out] */ tkDashStyle *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameOutlineStyle( 
            /* [in] */ tkDashStyle newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameOutlineWidth( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameOutlineWidth( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FramePaddingX( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FramePaddingX( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FramePaddingY( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FramePaddingY( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameTransparency( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameTransparency( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_InboxAlignment( 
            /* [retval][out] */ tkLabelAlignment *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_InboxAlignment( 
            /* [in] */ tkLabelAlignment newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ClassificationField( 
            /* [retval][out] */ long *FieldIndex) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ClassificationField( 
            /* [in] */ long FieldIndex) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE GenerateCategories( 
            /* [in] */ long FieldIndex,
            /* [in] */ tkClassificationType ClassificationType,
            /* [in] */ long numClasses,
            /* [retval][out] */ VARIANT_BOOL *vbretval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ApplyCategories( void) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Options( 
            /* [retval][out] */ ILabelCategory **retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Options( 
            /* [in] */ ILabelCategory *newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ApplyColorScheme( 
            /* [in] */ tkColorSchemeType Type,
            /* [in] */ IColorScheme *ColorScheme) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ApplyColorScheme2( 
            /* [in] */ tkColorSchemeType Type,
            /* [in] */ IColorScheme *ColorScheme,
            /* [in] */ tkLabelElements Element) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ApplyColorScheme3( 
            /* [in] */ tkColorSchemeType Type,
            /* [in] */ IColorScheme *ColorScheme,
            /* [in] */ tkLabelElements Element,
            /* [in] */ long CategoryStartIndex,
            /* [in] */ long CategoryEndIndex) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameVisible( 
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameVisible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_VisibilityExpression( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_VisibilityExpression( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MinDrawingSize( 
            /* [retval][out] */ LONG *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MinDrawingSize( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE MoveCategoryUp( 
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE MoveCategoryDown( 
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_AutoOffset( 
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_AutoOffset( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Serialize( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Deserialize( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Expression( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Expression( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SaveToXML( 
            /* [in] */ BSTR filename,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE LoadFromXML( 
            /* [in] */ BSTR filename,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SaveToDbf( 
            /* [defaultvalue][optional][in] */ VARIANT_BOOL saveText,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL saveCategory,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SaveToDbf2( 
            /* [defaultvalue][optional][in] */ BSTR xField,
            /* [defaultvalue][optional][in] */ BSTR yField,
            /* [defaultvalue][optional][in] */ BSTR angleField,
            /* [defaultvalue][optional][in] */ BSTR textField,
            /* [defaultvalue][optional][in] */ BSTR categoryField,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL saveText,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL saveCategory,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE LoadFromDbf( 
            /* [defaultvalue][optional][in] */ VARIANT_BOOL loadText,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL loadCategory,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE LoadFromDbf2( 
            /* [defaultvalue][optional][in] */ BSTR xField,
            /* [defaultvalue][optional][in] */ BSTR yField,
            /* [defaultvalue][optional][in] */ BSTR angleField,
            /* [defaultvalue][optional][in] */ BSTR textField,
            /* [defaultvalue][optional][in] */ BSTR categoryField,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL loadText,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL loadCategory,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Generate( 
            /* [in] */ BSTR Expression,
            /* [in] */ tkLabelPositioning Method,
            /* [in] */ VARIANT_BOOL LargestPartOnly,
            /* [retval][out] */ long *Count) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SavingMode( 
            /* [retval][out] */ tkSavingMode *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_SavingMode( 
            /* [in] */ tkSavingMode newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Positioning( 
            /* [retval][out] */ tkLabelPositioning *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Positioning( 
            /* [in] */ tkLabelPositioning newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_TextRenderingHint( 
            /* [retval][out] */ tkTextRenderingHint *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_TextRenderingHint( 
            /* [in] */ tkTextRenderingHint newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CompositingQuality( 
            /* [retval][out] */ tkCompositingQuality *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_CompositingQuality( 
            /* [in] */ tkCompositingQuality newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SmoothingMode( 
            /* [retval][out] */ tkSmoothingMode *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_SmoothingMode( 
            /* [in] */ tkSmoothingMode newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ILabelsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ILabels * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ILabels * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ILabels * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ILabels * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ILabels * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ILabels * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ILabels * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            ILabels * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            ILabels * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            ILabels * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            ILabels * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            ILabels * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            ILabels * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_VerticalPosition )( 
            ILabels * This,
            /* [retval][out] */ tkVerticalPosition *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_VerticalPosition )( 
            ILabels * This,
            /* [in] */ tkVerticalPosition newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Category )( 
            ILabels * This,
            /* [in] */ long Index,
            /* [retval][out] */ ILabelCategory **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Category )( 
            ILabels * This,
            /* [in] */ long Index,
            /* [in] */ ILabelCategory *newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AddLabel )( 
            ILabels * This,
            /* [in] */ BSTR Text,
            /* [in] */ double x,
            /* [in] */ double y,
            /* [defaultvalue][optional][in] */ double Rotation,
            /* [defaultvalue][optional][in] */ long Category);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *InsertLabel )( 
            ILabels * This,
            /* [in] */ long Index,
            /* [in] */ BSTR Text,
            /* [in] */ double x,
            /* [in] */ double y,
            /* [defaultvalue][optional][in] */ double Rotation,
            /* [defaultvalue][optional][in] */ long Category,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *RemoveLabel )( 
            ILabels * This,
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AddPart )( 
            ILabels * This,
            /* [in] */ long index,
            /* [in] */ BSTR Text,
            /* [in] */ double x,
            /* [in] */ double y,
            /* [defaultvalue][optional][in] */ double Rotation,
            /* [defaultvalue][optional][in] */ long Category);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *InsertPart )( 
            ILabels * This,
            /* [in] */ long Index,
            /* [in] */ long Part,
            /* [in] */ BSTR Text,
            /* [in] */ double x,
            /* [in] */ double y,
            /* [defaultvalue][optional][in] */ double Rotation,
            /* [defaultvalue][optional][in] */ long Category,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *RemovePart )( 
            ILabels * This,
            /* [in] */ long Index,
            /* [in] */ long Part,
            /* [retval][out] */ VARIANT_BOOL *vbretval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AddCategory )( 
            ILabels * This,
            /* [in] */ BSTR Name,
            /* [retval][out] */ ILabelCategory **retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *InsertCategory )( 
            ILabels * This,
            /* [in] */ long Index,
            /* [in] */ BSTR Name,
            /* [retval][out] */ ILabelCategory **retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *RemoveCategory )( 
            ILabels * This,
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *vbretval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clear )( 
            ILabels * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ClearCategories )( 
            ILabels * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Select )( 
            ILabels * This,
            /* [in] */ IExtents *BoundingBox,
            /* [defaultvalue][optional][in] */ long Tolerance,
            /* [defaultvalue][optional][in] */ SelectMode SelectMode,
            /* [out][optional][in] */ VARIANT *LabelIndices,
            /* [out][optional][in] */ VARIANT *PartIndices,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Count )( 
            ILabels * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumParts )( 
            ILabels * This,
            /* [in] */ long Index,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumCategories )( 
            ILabels * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Label )( 
            ILabels * This,
            /* [in] */ long Index,
            /* [in] */ long Part,
            /* [retval][out] */ ILabel **pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Synchronized )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Synchronized )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ScaleLabels )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ScaleLabels )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_BasicScale )( 
            ILabels * This,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_BasicScale )( 
            ILabels * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MaxVisibleScale )( 
            ILabels * This,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MaxVisibleScale )( 
            ILabels * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MinVisibleScale )( 
            ILabels * This,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MinVisibleScale )( 
            ILabels * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DynamicVisibility )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DynamicVisibility )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_AvoidCollisions )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_AvoidCollisions )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CollisionBuffer )( 
            ILabels * This,
            /* [retval][out] */ long *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_CollisionBuffer )( 
            ILabels * This,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_UseWidthLimits )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_UseWidthLimits )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_RemoveDuplicates )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_RemoveDuplicates )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_UseGdiPlus )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_UseGdiPlus )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Visible )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Visible )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_OffsetX )( 
            ILabels * This,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_OffsetX )( 
            ILabels * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_OffsetY )( 
            ILabels * This,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_OffsetY )( 
            ILabels * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Alignment )( 
            ILabels * This,
            /* [retval][out] */ tkLabelAlignment *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Alignment )( 
            ILabels * This,
            /* [in] */ tkLabelAlignment newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LineOrientation )( 
            ILabels * This,
            /* [retval][out] */ tkLineLabelOrientation *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LineOrientation )( 
            ILabels * This,
            /* [in] */ tkLineLabelOrientation newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontName )( 
            ILabels * This,
            /* [retval][out] */ BSTR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontName )( 
            ILabels * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontSize )( 
            ILabels * This,
            /* [retval][out] */ long *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontSize )( 
            ILabels * This,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontItalic )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontItalic )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontBold )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontBold )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontUnderline )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontUnderline )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontStrikeOut )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontStrikeOut )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontColor )( 
            ILabels * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontColor )( 
            ILabels * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontColor2 )( 
            ILabels * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontColor2 )( 
            ILabels * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontGradientMode )( 
            ILabels * This,
            /* [retval][out] */ tkLinearGradientMode *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontGradientMode )( 
            ILabels * This,
            /* [in] */ tkLinearGradientMode newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontTransparency )( 
            ILabels * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontTransparency )( 
            ILabels * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontOutlineVisible )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontOutlineVisible )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShadowVisible )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ShadowVisible )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_HaloVisible )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_HaloVisible )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontOutlineColor )( 
            ILabels * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontOutlineColor )( 
            ILabels * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShadowColor )( 
            ILabels * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ShadowColor )( 
            ILabels * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_HaloColor )( 
            ILabels * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_HaloColor )( 
            ILabels * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontOutlineWidth )( 
            ILabels * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontOutlineWidth )( 
            ILabels * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShadowOffsetX )( 
            ILabels * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ShadowOffsetX )( 
            ILabels * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShadowOffsetY )( 
            ILabels * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ShadowOffsetY )( 
            ILabels * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_HaloSize )( 
            ILabels * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_HaloSize )( 
            ILabels * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameType )( 
            ILabels * This,
            /* [retval][out] */ tkLabelFrameType *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameType )( 
            ILabels * This,
            /* [in] */ tkLabelFrameType newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameOutlineColor )( 
            ILabels * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameOutlineColor )( 
            ILabels * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameBackColor )( 
            ILabels * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameBackColor )( 
            ILabels * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameBackColor2 )( 
            ILabels * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameBackColor2 )( 
            ILabels * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameGradientMode )( 
            ILabels * This,
            /* [retval][out] */ tkLinearGradientMode *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameGradientMode )( 
            ILabels * This,
            /* [in] */ tkLinearGradientMode newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameOutlineStyle )( 
            ILabels * This,
            /* [retval][out] */ tkDashStyle *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameOutlineStyle )( 
            ILabels * This,
            /* [in] */ tkDashStyle newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameOutlineWidth )( 
            ILabels * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameOutlineWidth )( 
            ILabels * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FramePaddingX )( 
            ILabels * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FramePaddingX )( 
            ILabels * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FramePaddingY )( 
            ILabels * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FramePaddingY )( 
            ILabels * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameTransparency )( 
            ILabels * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameTransparency )( 
            ILabels * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_InboxAlignment )( 
            ILabels * This,
            /* [retval][out] */ tkLabelAlignment *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_InboxAlignment )( 
            ILabels * This,
            /* [in] */ tkLabelAlignment newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ClassificationField )( 
            ILabels * This,
            /* [retval][out] */ long *FieldIndex);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ClassificationField )( 
            ILabels * This,
            /* [in] */ long FieldIndex);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *GenerateCategories )( 
            ILabels * This,
            /* [in] */ long FieldIndex,
            /* [in] */ tkClassificationType ClassificationType,
            /* [in] */ long numClasses,
            /* [retval][out] */ VARIANT_BOOL *vbretval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ApplyCategories )( 
            ILabels * This);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Options )( 
            ILabels * This,
            /* [retval][out] */ ILabelCategory **retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Options )( 
            ILabels * This,
            /* [in] */ ILabelCategory *newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ApplyColorScheme )( 
            ILabels * This,
            /* [in] */ tkColorSchemeType Type,
            /* [in] */ IColorScheme *ColorScheme);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ApplyColorScheme2 )( 
            ILabels * This,
            /* [in] */ tkColorSchemeType Type,
            /* [in] */ IColorScheme *ColorScheme,
            /* [in] */ tkLabelElements Element);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ApplyColorScheme3 )( 
            ILabels * This,
            /* [in] */ tkColorSchemeType Type,
            /* [in] */ IColorScheme *ColorScheme,
            /* [in] */ tkLabelElements Element,
            /* [in] */ long CategoryStartIndex,
            /* [in] */ long CategoryEndIndex);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameVisible )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameVisible )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_VisibilityExpression )( 
            ILabels * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_VisibilityExpression )( 
            ILabels * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MinDrawingSize )( 
            ILabels * This,
            /* [retval][out] */ LONG *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MinDrawingSize )( 
            ILabels * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *MoveCategoryUp )( 
            ILabels * This,
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *MoveCategoryDown )( 
            ILabels * This,
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_AutoOffset )( 
            ILabels * This,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_AutoOffset )( 
            ILabels * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Serialize )( 
            ILabels * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Deserialize )( 
            ILabels * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Expression )( 
            ILabels * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Expression )( 
            ILabels * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SaveToXML )( 
            ILabels * This,
            /* [in] */ BSTR filename,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *LoadFromXML )( 
            ILabels * This,
            /* [in] */ BSTR filename,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SaveToDbf )( 
            ILabels * This,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL saveText,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL saveCategory,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SaveToDbf2 )( 
            ILabels * This,
            /* [defaultvalue][optional][in] */ BSTR xField,
            /* [defaultvalue][optional][in] */ BSTR yField,
            /* [defaultvalue][optional][in] */ BSTR angleField,
            /* [defaultvalue][optional][in] */ BSTR textField,
            /* [defaultvalue][optional][in] */ BSTR categoryField,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL saveText,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL saveCategory,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *LoadFromDbf )( 
            ILabels * This,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL loadText,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL loadCategory,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *LoadFromDbf2 )( 
            ILabels * This,
            /* [defaultvalue][optional][in] */ BSTR xField,
            /* [defaultvalue][optional][in] */ BSTR yField,
            /* [defaultvalue][optional][in] */ BSTR angleField,
            /* [defaultvalue][optional][in] */ BSTR textField,
            /* [defaultvalue][optional][in] */ BSTR categoryField,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL loadText,
            /* [defaultvalue][optional][in] */ VARIANT_BOOL loadCategory,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Generate )( 
            ILabels * This,
            /* [in] */ BSTR Expression,
            /* [in] */ tkLabelPositioning Method,
            /* [in] */ VARIANT_BOOL LargestPartOnly,
            /* [retval][out] */ long *Count);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SavingMode )( 
            ILabels * This,
            /* [retval][out] */ tkSavingMode *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_SavingMode )( 
            ILabels * This,
            /* [in] */ tkSavingMode newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Positioning )( 
            ILabels * This,
            /* [retval][out] */ tkLabelPositioning *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Positioning )( 
            ILabels * This,
            /* [in] */ tkLabelPositioning newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_TextRenderingHint )( 
            ILabels * This,
            /* [retval][out] */ tkTextRenderingHint *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_TextRenderingHint )( 
            ILabels * This,
            /* [in] */ tkTextRenderingHint newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CompositingQuality )( 
            ILabels * This,
            /* [retval][out] */ tkCompositingQuality *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_CompositingQuality )( 
            ILabels * This,
            /* [in] */ tkCompositingQuality newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SmoothingMode )( 
            ILabels * This,
            /* [retval][out] */ tkSmoothingMode *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_SmoothingMode )( 
            ILabels * This,
            /* [in] */ tkSmoothingMode newVal);
        
        END_INTERFACE
    } ILabelsVtbl;

    interface ILabels
    {
        CONST_VTBL struct ILabelsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ILabels_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ILabels_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ILabels_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ILabels_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ILabels_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ILabels_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ILabels_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ILabels_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define ILabels_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define ILabels_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define ILabels_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define ILabels_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define ILabels_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define ILabels_get_VerticalPosition(This,retval)	\
    ( (This)->lpVtbl -> get_VerticalPosition(This,retval) ) 

#define ILabels_put_VerticalPosition(This,newVal)	\
    ( (This)->lpVtbl -> put_VerticalPosition(This,newVal) ) 

#define ILabels_get_Category(This,Index,pVal)	\
    ( (This)->lpVtbl -> get_Category(This,Index,pVal) ) 

#define ILabels_put_Category(This,Index,newVal)	\
    ( (This)->lpVtbl -> put_Category(This,Index,newVal) ) 

#define ILabels_AddLabel(This,Text,x,y,Rotation,Category)	\
    ( (This)->lpVtbl -> AddLabel(This,Text,x,y,Rotation,Category) ) 

#define ILabels_InsertLabel(This,Index,Text,x,y,Rotation,Category,retval)	\
    ( (This)->lpVtbl -> InsertLabel(This,Index,Text,x,y,Rotation,Category,retval) ) 

#define ILabels_RemoveLabel(This,Index,retval)	\
    ( (This)->lpVtbl -> RemoveLabel(This,Index,retval) ) 

#define ILabels_AddPart(This,index,Text,x,y,Rotation,Category)	\
    ( (This)->lpVtbl -> AddPart(This,index,Text,x,y,Rotation,Category) ) 

#define ILabels_InsertPart(This,Index,Part,Text,x,y,Rotation,Category,retval)	\
    ( (This)->lpVtbl -> InsertPart(This,Index,Part,Text,x,y,Rotation,Category,retval) ) 

#define ILabels_RemovePart(This,Index,Part,vbretval)	\
    ( (This)->lpVtbl -> RemovePart(This,Index,Part,vbretval) ) 

#define ILabels_AddCategory(This,Name,retVal)	\
    ( (This)->lpVtbl -> AddCategory(This,Name,retVal) ) 

#define ILabels_InsertCategory(This,Index,Name,retVal)	\
    ( (This)->lpVtbl -> InsertCategory(This,Index,Name,retVal) ) 

#define ILabels_RemoveCategory(This,Index,vbretval)	\
    ( (This)->lpVtbl -> RemoveCategory(This,Index,vbretval) ) 

#define ILabels_Clear(This)	\
    ( (This)->lpVtbl -> Clear(This) ) 

#define ILabels_ClearCategories(This)	\
    ( (This)->lpVtbl -> ClearCategories(This) ) 

#define ILabels_Select(This,BoundingBox,Tolerance,SelectMode,LabelIndices,PartIndices,retval)	\
    ( (This)->lpVtbl -> Select(This,BoundingBox,Tolerance,SelectMode,LabelIndices,PartIndices,retval) ) 

#define ILabels_get_Count(This,pVal)	\
    ( (This)->lpVtbl -> get_Count(This,pVal) ) 

#define ILabels_get_NumParts(This,Index,pVal)	\
    ( (This)->lpVtbl -> get_NumParts(This,Index,pVal) ) 

#define ILabels_get_NumCategories(This,pVal)	\
    ( (This)->lpVtbl -> get_NumCategories(This,pVal) ) 

#define ILabels_get_Label(This,Index,Part,pVal)	\
    ( (This)->lpVtbl -> get_Label(This,Index,Part,pVal) ) 

#define ILabels_get_Synchronized(This,retval)	\
    ( (This)->lpVtbl -> get_Synchronized(This,retval) ) 

#define ILabels_put_Synchronized(This,newVal)	\
    ( (This)->lpVtbl -> put_Synchronized(This,newVal) ) 

#define ILabels_get_ScaleLabels(This,retval)	\
    ( (This)->lpVtbl -> get_ScaleLabels(This,retval) ) 

#define ILabels_put_ScaleLabels(This,newVal)	\
    ( (This)->lpVtbl -> put_ScaleLabels(This,newVal) ) 

#define ILabels_get_BasicScale(This,retval)	\
    ( (This)->lpVtbl -> get_BasicScale(This,retval) ) 

#define ILabels_put_BasicScale(This,newVal)	\
    ( (This)->lpVtbl -> put_BasicScale(This,newVal) ) 

#define ILabels_get_MaxVisibleScale(This,retval)	\
    ( (This)->lpVtbl -> get_MaxVisibleScale(This,retval) ) 

#define ILabels_put_MaxVisibleScale(This,newVal)	\
    ( (This)->lpVtbl -> put_MaxVisibleScale(This,newVal) ) 

#define ILabels_get_MinVisibleScale(This,retval)	\
    ( (This)->lpVtbl -> get_MinVisibleScale(This,retval) ) 

#define ILabels_put_MinVisibleScale(This,newVal)	\
    ( (This)->lpVtbl -> put_MinVisibleScale(This,newVal) ) 

#define ILabels_get_DynamicVisibility(This,retval)	\
    ( (This)->lpVtbl -> get_DynamicVisibility(This,retval) ) 

#define ILabels_put_DynamicVisibility(This,newVal)	\
    ( (This)->lpVtbl -> put_DynamicVisibility(This,newVal) ) 

#define ILabels_get_AvoidCollisions(This,retval)	\
    ( (This)->lpVtbl -> get_AvoidCollisions(This,retval) ) 

#define ILabels_put_AvoidCollisions(This,newVal)	\
    ( (This)->lpVtbl -> put_AvoidCollisions(This,newVal) ) 

#define ILabels_get_CollisionBuffer(This,retval)	\
    ( (This)->lpVtbl -> get_CollisionBuffer(This,retval) ) 

#define ILabels_put_CollisionBuffer(This,newVal)	\
    ( (This)->lpVtbl -> put_CollisionBuffer(This,newVal) ) 

#define ILabels_get_UseWidthLimits(This,retval)	\
    ( (This)->lpVtbl -> get_UseWidthLimits(This,retval) ) 

#define ILabels_put_UseWidthLimits(This,newVal)	\
    ( (This)->lpVtbl -> put_UseWidthLimits(This,newVal) ) 

#define ILabels_get_RemoveDuplicates(This,retval)	\
    ( (This)->lpVtbl -> get_RemoveDuplicates(This,retval) ) 

#define ILabels_put_RemoveDuplicates(This,newVal)	\
    ( (This)->lpVtbl -> put_RemoveDuplicates(This,newVal) ) 

#define ILabels_get_UseGdiPlus(This,retval)	\
    ( (This)->lpVtbl -> get_UseGdiPlus(This,retval) ) 

#define ILabels_put_UseGdiPlus(This,newVal)	\
    ( (This)->lpVtbl -> put_UseGdiPlus(This,newVal) ) 

#define ILabels_get_Visible(This,retval)	\
    ( (This)->lpVtbl -> get_Visible(This,retval) ) 

#define ILabels_put_Visible(This,newVal)	\
    ( (This)->lpVtbl -> put_Visible(This,newVal) ) 

#define ILabels_get_OffsetX(This,retval)	\
    ( (This)->lpVtbl -> get_OffsetX(This,retval) ) 

#define ILabels_put_OffsetX(This,newVal)	\
    ( (This)->lpVtbl -> put_OffsetX(This,newVal) ) 

#define ILabels_get_OffsetY(This,retval)	\
    ( (This)->lpVtbl -> get_OffsetY(This,retval) ) 

#define ILabels_put_OffsetY(This,newVal)	\
    ( (This)->lpVtbl -> put_OffsetY(This,newVal) ) 

#define ILabels_get_Alignment(This,retval)	\
    ( (This)->lpVtbl -> get_Alignment(This,retval) ) 

#define ILabels_put_Alignment(This,newVal)	\
    ( (This)->lpVtbl -> put_Alignment(This,newVal) ) 

#define ILabels_get_LineOrientation(This,retval)	\
    ( (This)->lpVtbl -> get_LineOrientation(This,retval) ) 

#define ILabels_put_LineOrientation(This,newVal)	\
    ( (This)->lpVtbl -> put_LineOrientation(This,newVal) ) 

#define ILabels_get_FontName(This,retval)	\
    ( (This)->lpVtbl -> get_FontName(This,retval) ) 

#define ILabels_put_FontName(This,newVal)	\
    ( (This)->lpVtbl -> put_FontName(This,newVal) ) 

#define ILabels_get_FontSize(This,retval)	\
    ( (This)->lpVtbl -> get_FontSize(This,retval) ) 

#define ILabels_put_FontSize(This,newVal)	\
    ( (This)->lpVtbl -> put_FontSize(This,newVal) ) 

#define ILabels_get_FontItalic(This,retval)	\
    ( (This)->lpVtbl -> get_FontItalic(This,retval) ) 

#define ILabels_put_FontItalic(This,newVal)	\
    ( (This)->lpVtbl -> put_FontItalic(This,newVal) ) 

#define ILabels_get_FontBold(This,retval)	\
    ( (This)->lpVtbl -> get_FontBold(This,retval) ) 

#define ILabels_put_FontBold(This,newVal)	\
    ( (This)->lpVtbl -> put_FontBold(This,newVal) ) 

#define ILabels_get_FontUnderline(This,retval)	\
    ( (This)->lpVtbl -> get_FontUnderline(This,retval) ) 

#define ILabels_put_FontUnderline(This,newVal)	\
    ( (This)->lpVtbl -> put_FontUnderline(This,newVal) ) 

#define ILabels_get_FontStrikeOut(This,retval)	\
    ( (This)->lpVtbl -> get_FontStrikeOut(This,retval) ) 

#define ILabels_put_FontStrikeOut(This,newVal)	\
    ( (This)->lpVtbl -> put_FontStrikeOut(This,newVal) ) 

#define ILabels_get_FontColor(This,retval)	\
    ( (This)->lpVtbl -> get_FontColor(This,retval) ) 

#define ILabels_put_FontColor(This,newVal)	\
    ( (This)->lpVtbl -> put_FontColor(This,newVal) ) 

#define ILabels_get_FontColor2(This,retval)	\
    ( (This)->lpVtbl -> get_FontColor2(This,retval) ) 

#define ILabels_put_FontColor2(This,newVal)	\
    ( (This)->lpVtbl -> put_FontColor2(This,newVal) ) 

#define ILabels_get_FontGradientMode(This,retval)	\
    ( (This)->lpVtbl -> get_FontGradientMode(This,retval) ) 

#define ILabels_put_FontGradientMode(This,newVal)	\
    ( (This)->lpVtbl -> put_FontGradientMode(This,newVal) ) 

#define ILabels_get_FontTransparency(This,retval)	\
    ( (This)->lpVtbl -> get_FontTransparency(This,retval) ) 

#define ILabels_put_FontTransparency(This,newVal)	\
    ( (This)->lpVtbl -> put_FontTransparency(This,newVal) ) 

#define ILabels_get_FontOutlineVisible(This,retval)	\
    ( (This)->lpVtbl -> get_FontOutlineVisible(This,retval) ) 

#define ILabels_put_FontOutlineVisible(This,newVal)	\
    ( (This)->lpVtbl -> put_FontOutlineVisible(This,newVal) ) 

#define ILabels_get_ShadowVisible(This,retval)	\
    ( (This)->lpVtbl -> get_ShadowVisible(This,retval) ) 

#define ILabels_put_ShadowVisible(This,newVal)	\
    ( (This)->lpVtbl -> put_ShadowVisible(This,newVal) ) 

#define ILabels_get_HaloVisible(This,retval)	\
    ( (This)->lpVtbl -> get_HaloVisible(This,retval) ) 

#define ILabels_put_HaloVisible(This,newVal)	\
    ( (This)->lpVtbl -> put_HaloVisible(This,newVal) ) 

#define ILabels_get_FontOutlineColor(This,retval)	\
    ( (This)->lpVtbl -> get_FontOutlineColor(This,retval) ) 

#define ILabels_put_FontOutlineColor(This,newVal)	\
    ( (This)->lpVtbl -> put_FontOutlineColor(This,newVal) ) 

#define ILabels_get_ShadowColor(This,retval)	\
    ( (This)->lpVtbl -> get_ShadowColor(This,retval) ) 

#define ILabels_put_ShadowColor(This,newVal)	\
    ( (This)->lpVtbl -> put_ShadowColor(This,newVal) ) 

#define ILabels_get_HaloColor(This,retval)	\
    ( (This)->lpVtbl -> get_HaloColor(This,retval) ) 

#define ILabels_put_HaloColor(This,newVal)	\
    ( (This)->lpVtbl -> put_HaloColor(This,newVal) ) 

#define ILabels_get_FontOutlineWidth(This,retval)	\
    ( (This)->lpVtbl -> get_FontOutlineWidth(This,retval) ) 

#define ILabels_put_FontOutlineWidth(This,newVal)	\
    ( (This)->lpVtbl -> put_FontOutlineWidth(This,newVal) ) 

#define ILabels_get_ShadowOffsetX(This,retval)	\
    ( (This)->lpVtbl -> get_ShadowOffsetX(This,retval) ) 

#define ILabels_put_ShadowOffsetX(This,newVal)	\
    ( (This)->lpVtbl -> put_ShadowOffsetX(This,newVal) ) 

#define ILabels_get_ShadowOffsetY(This,retval)	\
    ( (This)->lpVtbl -> get_ShadowOffsetY(This,retval) ) 

#define ILabels_put_ShadowOffsetY(This,newVal)	\
    ( (This)->lpVtbl -> put_ShadowOffsetY(This,newVal) ) 

#define ILabels_get_HaloSize(This,retval)	\
    ( (This)->lpVtbl -> get_HaloSize(This,retval) ) 

#define ILabels_put_HaloSize(This,newVal)	\
    ( (This)->lpVtbl -> put_HaloSize(This,newVal) ) 

#define ILabels_get_FrameType(This,retval)	\
    ( (This)->lpVtbl -> get_FrameType(This,retval) ) 

#define ILabels_put_FrameType(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameType(This,newVal) ) 

#define ILabels_get_FrameOutlineColor(This,retval)	\
    ( (This)->lpVtbl -> get_FrameOutlineColor(This,retval) ) 

#define ILabels_put_FrameOutlineColor(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameOutlineColor(This,newVal) ) 

#define ILabels_get_FrameBackColor(This,retval)	\
    ( (This)->lpVtbl -> get_FrameBackColor(This,retval) ) 

#define ILabels_put_FrameBackColor(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameBackColor(This,newVal) ) 

#define ILabels_get_FrameBackColor2(This,retval)	\
    ( (This)->lpVtbl -> get_FrameBackColor2(This,retval) ) 

#define ILabels_put_FrameBackColor2(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameBackColor2(This,newVal) ) 

#define ILabels_get_FrameGradientMode(This,retval)	\
    ( (This)->lpVtbl -> get_FrameGradientMode(This,retval) ) 

#define ILabels_put_FrameGradientMode(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameGradientMode(This,newVal) ) 

#define ILabels_get_FrameOutlineStyle(This,retval)	\
    ( (This)->lpVtbl -> get_FrameOutlineStyle(This,retval) ) 

#define ILabels_put_FrameOutlineStyle(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameOutlineStyle(This,newVal) ) 

#define ILabels_get_FrameOutlineWidth(This,retval)	\
    ( (This)->lpVtbl -> get_FrameOutlineWidth(This,retval) ) 

#define ILabels_put_FrameOutlineWidth(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameOutlineWidth(This,newVal) ) 

#define ILabels_get_FramePaddingX(This,retval)	\
    ( (This)->lpVtbl -> get_FramePaddingX(This,retval) ) 

#define ILabels_put_FramePaddingX(This,newVal)	\
    ( (This)->lpVtbl -> put_FramePaddingX(This,newVal) ) 

#define ILabels_get_FramePaddingY(This,retval)	\
    ( (This)->lpVtbl -> get_FramePaddingY(This,retval) ) 

#define ILabels_put_FramePaddingY(This,newVal)	\
    ( (This)->lpVtbl -> put_FramePaddingY(This,newVal) ) 

#define ILabels_get_FrameTransparency(This,retval)	\
    ( (This)->lpVtbl -> get_FrameTransparency(This,retval) ) 

#define ILabels_put_FrameTransparency(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameTransparency(This,newVal) ) 

#define ILabels_get_InboxAlignment(This,retval)	\
    ( (This)->lpVtbl -> get_InboxAlignment(This,retval) ) 

#define ILabels_put_InboxAlignment(This,newVal)	\
    ( (This)->lpVtbl -> put_InboxAlignment(This,newVal) ) 

#define ILabels_get_ClassificationField(This,FieldIndex)	\
    ( (This)->lpVtbl -> get_ClassificationField(This,FieldIndex) ) 

#define ILabels_put_ClassificationField(This,FieldIndex)	\
    ( (This)->lpVtbl -> put_ClassificationField(This,FieldIndex) ) 

#define ILabels_GenerateCategories(This,FieldIndex,ClassificationType,numClasses,vbretval)	\
    ( (This)->lpVtbl -> GenerateCategories(This,FieldIndex,ClassificationType,numClasses,vbretval) ) 

#define ILabels_ApplyCategories(This)	\
    ( (This)->lpVtbl -> ApplyCategories(This) ) 

#define ILabels_get_Options(This,retVal)	\
    ( (This)->lpVtbl -> get_Options(This,retVal) ) 

#define ILabels_put_Options(This,newVal)	\
    ( (This)->lpVtbl -> put_Options(This,newVal) ) 

#define ILabels_ApplyColorScheme(This,Type,ColorScheme)	\
    ( (This)->lpVtbl -> ApplyColorScheme(This,Type,ColorScheme) ) 

#define ILabels_ApplyColorScheme2(This,Type,ColorScheme,Element)	\
    ( (This)->lpVtbl -> ApplyColorScheme2(This,Type,ColorScheme,Element) ) 

#define ILabels_ApplyColorScheme3(This,Type,ColorScheme,Element,CategoryStartIndex,CategoryEndIndex)	\
    ( (This)->lpVtbl -> ApplyColorScheme3(This,Type,ColorScheme,Element,CategoryStartIndex,CategoryEndIndex) ) 

#define ILabels_get_FrameVisible(This,retVal)	\
    ( (This)->lpVtbl -> get_FrameVisible(This,retVal) ) 

#define ILabels_put_FrameVisible(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameVisible(This,newVal) ) 

#define ILabels_get_VisibilityExpression(This,retVal)	\
    ( (This)->lpVtbl -> get_VisibilityExpression(This,retVal) ) 

#define ILabels_put_VisibilityExpression(This,newVal)	\
    ( (This)->lpVtbl -> put_VisibilityExpression(This,newVal) ) 

#define ILabels_get_MinDrawingSize(This,retVal)	\
    ( (This)->lpVtbl -> get_MinDrawingSize(This,retVal) ) 

#define ILabels_put_MinDrawingSize(This,newVal)	\
    ( (This)->lpVtbl -> put_MinDrawingSize(This,newVal) ) 

#define ILabels_MoveCategoryUp(This,Index,retval)	\
    ( (This)->lpVtbl -> MoveCategoryUp(This,Index,retval) ) 

#define ILabels_MoveCategoryDown(This,Index,retval)	\
    ( (This)->lpVtbl -> MoveCategoryDown(This,Index,retval) ) 

#define ILabels_get_AutoOffset(This,retVal)	\
    ( (This)->lpVtbl -> get_AutoOffset(This,retVal) ) 

#define ILabels_put_AutoOffset(This,newVal)	\
    ( (This)->lpVtbl -> put_AutoOffset(This,newVal) ) 

#define ILabels_Serialize(This,retVal)	\
    ( (This)->lpVtbl -> Serialize(This,retVal) ) 

#define ILabels_Deserialize(This,newVal)	\
    ( (This)->lpVtbl -> Deserialize(This,newVal) ) 

#define ILabels_get_Expression(This,retVal)	\
    ( (This)->lpVtbl -> get_Expression(This,retVal) ) 

#define ILabels_put_Expression(This,newVal)	\
    ( (This)->lpVtbl -> put_Expression(This,newVal) ) 

#define ILabels_SaveToXML(This,filename,retVal)	\
    ( (This)->lpVtbl -> SaveToXML(This,filename,retVal) ) 

#define ILabels_LoadFromXML(This,filename,retVal)	\
    ( (This)->lpVtbl -> LoadFromXML(This,filename,retVal) ) 

#define ILabels_SaveToDbf(This,saveText,saveCategory,retVal)	\
    ( (This)->lpVtbl -> SaveToDbf(This,saveText,saveCategory,retVal) ) 

#define ILabels_SaveToDbf2(This,xField,yField,angleField,textField,categoryField,saveText,saveCategory,retVal)	\
    ( (This)->lpVtbl -> SaveToDbf2(This,xField,yField,angleField,textField,categoryField,saveText,saveCategory,retVal) ) 

#define ILabels_LoadFromDbf(This,loadText,loadCategory,retVal)	\
    ( (This)->lpVtbl -> LoadFromDbf(This,loadText,loadCategory,retVal) ) 

#define ILabels_LoadFromDbf2(This,xField,yField,angleField,textField,categoryField,loadText,loadCategory,retVal)	\
    ( (This)->lpVtbl -> LoadFromDbf2(This,xField,yField,angleField,textField,categoryField,loadText,loadCategory,retVal) ) 

#define ILabels_Generate(This,Expression,Method,LargestPartOnly,Count)	\
    ( (This)->lpVtbl -> Generate(This,Expression,Method,LargestPartOnly,Count) ) 

#define ILabels_get_SavingMode(This,retVal)	\
    ( (This)->lpVtbl -> get_SavingMode(This,retVal) ) 

#define ILabels_put_SavingMode(This,newVal)	\
    ( (This)->lpVtbl -> put_SavingMode(This,newVal) ) 

#define ILabels_get_Positioning(This,pVal)	\
    ( (This)->lpVtbl -> get_Positioning(This,pVal) ) 

#define ILabels_put_Positioning(This,newVal)	\
    ( (This)->lpVtbl -> put_Positioning(This,newVal) ) 

#define ILabels_get_TextRenderingHint(This,pVal)	\
    ( (This)->lpVtbl -> get_TextRenderingHint(This,pVal) ) 

#define ILabels_put_TextRenderingHint(This,newVal)	\
    ( (This)->lpVtbl -> put_TextRenderingHint(This,newVal) ) 

#define ILabels_get_CompositingQuality(This,pVal)	\
    ( (This)->lpVtbl -> get_CompositingQuality(This,pVal) ) 

#define ILabels_put_CompositingQuality(This,newVal)	\
    ( (This)->lpVtbl -> put_CompositingQuality(This,newVal) ) 

#define ILabels_get_SmoothingMode(This,pVal)	\
    ( (This)->lpVtbl -> get_SmoothingMode(This,pVal) ) 

#define ILabels_put_SmoothingMode(This,newVal)	\
    ( (This)->lpVtbl -> put_SmoothingMode(This,newVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ILabels_put_ClassificationField_Proxy( 
    ILabels * This,
    /* [in] */ long FieldIndex);


void __RPC_STUB ILabels_put_ClassificationField_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_GenerateCategories_Proxy( 
    ILabels * This,
    /* [in] */ long FieldIndex,
    /* [in] */ tkClassificationType ClassificationType,
    /* [in] */ long numClasses,
    /* [retval][out] */ VARIANT_BOOL *vbretval);


void __RPC_STUB ILabels_GenerateCategories_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_ApplyCategories_Proxy( 
    ILabels * This);


void __RPC_STUB ILabels_ApplyCategories_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ILabels_get_Options_Proxy( 
    ILabels * This,
    /* [retval][out] */ ILabelCategory **retVal);


void __RPC_STUB ILabels_get_Options_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ILabels_put_Options_Proxy( 
    ILabels * This,
    /* [in] */ ILabelCategory *newVal);


void __RPC_STUB ILabels_put_Options_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_ApplyColorScheme_Proxy( 
    ILabels * This,
    /* [in] */ tkColorSchemeType Type,
    /* [in] */ IColorScheme *ColorScheme);


void __RPC_STUB ILabels_ApplyColorScheme_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_ApplyColorScheme2_Proxy( 
    ILabels * This,
    /* [in] */ tkColorSchemeType Type,
    /* [in] */ IColorScheme *ColorScheme,
    /* [in] */ tkLabelElements Element);


void __RPC_STUB ILabels_ApplyColorScheme2_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_ApplyColorScheme3_Proxy( 
    ILabels * This,
    /* [in] */ tkColorSchemeType Type,
    /* [in] */ IColorScheme *ColorScheme,
    /* [in] */ tkLabelElements Element,
    /* [in] */ long CategoryStartIndex,
    /* [in] */ long CategoryEndIndex);


void __RPC_STUB ILabels_ApplyColorScheme3_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ILabels_get_FrameVisible_Proxy( 
    ILabels * This,
    /* [retval][out] */ VARIANT_BOOL *retVal);


void __RPC_STUB ILabels_get_FrameVisible_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ILabels_put_FrameVisible_Proxy( 
    ILabels * This,
    /* [in] */ VARIANT_BOOL newVal);


void __RPC_STUB ILabels_put_FrameVisible_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ILabels_get_VisibilityExpression_Proxy( 
    ILabels * This,
    /* [retval][out] */ BSTR *retVal);


void __RPC_STUB ILabels_get_VisibilityExpression_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ILabels_put_VisibilityExpression_Proxy( 
    ILabels * This,
    /* [in] */ BSTR newVal);


void __RPC_STUB ILabels_put_VisibilityExpression_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ILabels_get_MinDrawingSize_Proxy( 
    ILabels * This,
    /* [retval][out] */ LONG *retVal);


void __RPC_STUB ILabels_get_MinDrawingSize_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ILabels_put_MinDrawingSize_Proxy( 
    ILabels * This,
    /* [in] */ LONG newVal);


void __RPC_STUB ILabels_put_MinDrawingSize_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_MoveCategoryUp_Proxy( 
    ILabels * This,
    /* [in] */ long Index,
    /* [retval][out] */ VARIANT_BOOL *retval);


void __RPC_STUB ILabels_MoveCategoryUp_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_MoveCategoryDown_Proxy( 
    ILabels * This,
    /* [in] */ long Index,
    /* [retval][out] */ VARIANT_BOOL *retval);


void __RPC_STUB ILabels_MoveCategoryDown_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ILabels_get_AutoOffset_Proxy( 
    ILabels * This,
    /* [retval][out] */ VARIANT_BOOL *retVal);


void __RPC_STUB ILabels_get_AutoOffset_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ILabels_put_AutoOffset_Proxy( 
    ILabels * This,
    /* [in] */ VARIANT_BOOL newVal);


void __RPC_STUB ILabels_put_AutoOffset_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_Serialize_Proxy( 
    ILabels * This,
    /* [retval][out] */ BSTR *retVal);


void __RPC_STUB ILabels_Serialize_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_Deserialize_Proxy( 
    ILabels * This,
    /* [in] */ BSTR newVal);


void __RPC_STUB ILabels_Deserialize_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ILabels_get_Expression_Proxy( 
    ILabels * This,
    /* [retval][out] */ BSTR *retVal);


void __RPC_STUB ILabels_get_Expression_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ILabels_put_Expression_Proxy( 
    ILabels * This,
    /* [in] */ BSTR newVal);


void __RPC_STUB ILabels_put_Expression_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_SaveToXML_Proxy( 
    ILabels * This,
    /* [in] */ BSTR filename,
    /* [retval][out] */ VARIANT_BOOL *retVal);


void __RPC_STUB ILabels_SaveToXML_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_LoadFromXML_Proxy( 
    ILabels * This,
    /* [in] */ BSTR filename,
    /* [retval][out] */ VARIANT_BOOL *retVal);


void __RPC_STUB ILabels_LoadFromXML_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_SaveToDbf_Proxy( 
    ILabels * This,
    /* [defaultvalue][optional][in] */ VARIANT_BOOL saveText,
    /* [defaultvalue][optional][in] */ VARIANT_BOOL saveCategory,
    /* [retval][out] */ VARIANT_BOOL *retVal);


void __RPC_STUB ILabels_SaveToDbf_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_SaveToDbf2_Proxy( 
    ILabels * This,
    /* [defaultvalue][optional][in] */ BSTR xField,
    /* [defaultvalue][optional][in] */ BSTR yField,
    /* [defaultvalue][optional][in] */ BSTR angleField,
    /* [defaultvalue][optional][in] */ BSTR textField,
    /* [defaultvalue][optional][in] */ BSTR categoryField,
    /* [defaultvalue][optional][in] */ VARIANT_BOOL saveText,
    /* [defaultvalue][optional][in] */ VARIANT_BOOL saveCategory,
    /* [retval][out] */ VARIANT_BOOL *retVal);


void __RPC_STUB ILabels_SaveToDbf2_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_LoadFromDbf_Proxy( 
    ILabels * This,
    /* [defaultvalue][optional][in] */ VARIANT_BOOL loadText,
    /* [defaultvalue][optional][in] */ VARIANT_BOOL loadCategory,
    /* [retval][out] */ VARIANT_BOOL *retVal);


void __RPC_STUB ILabels_LoadFromDbf_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_LoadFromDbf2_Proxy( 
    ILabels * This,
    /* [defaultvalue][optional][in] */ BSTR xField,
    /* [defaultvalue][optional][in] */ BSTR yField,
    /* [defaultvalue][optional][in] */ BSTR angleField,
    /* [defaultvalue][optional][in] */ BSTR textField,
    /* [defaultvalue][optional][in] */ BSTR categoryField,
    /* [defaultvalue][optional][in] */ VARIANT_BOOL loadText,
    /* [defaultvalue][optional][in] */ VARIANT_BOOL loadCategory,
    /* [retval][out] */ VARIANT_BOOL *retVal);


void __RPC_STUB ILabels_LoadFromDbf2_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ILabels_Generate_Proxy( 
    ILabels * This,
    /* [in] */ BSTR Expression,
    /* [in] */ tkLabelPositioning Method,
    /* [in] */ VARIANT_BOOL LargestPartOnly,
    /* [retval][out] */ long *Count);


void __RPC_STUB ILabels_Generate_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ILabels_get_SavingMode_Proxy( 
    ILabels * This,
    /* [retval][out] */ tkSavingMode *retVal);


void __RPC_STUB ILabels_get_SavingMode_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ILabels_put_SavingMode_Proxy( 
    ILabels * This,
    /* [in] */ tkSavingMode newVal);


void __RPC_STUB ILabels_put_SavingMode_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ILabels_get_Positioning_Proxy( 
    ILabels * This,
    /* [retval][out] */ tkLabelPositioning *pVal);


void __RPC_STUB ILabels_get_Positioning_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ILabels_put_Positioning_Proxy( 
    ILabels * This,
    /* [in] */ tkLabelPositioning newVal);


void __RPC_STUB ILabels_put_Positioning_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ILabels_get_TextRenderingHint_Proxy( 
    ILabels * This,
    /* [retval][out] */ tkTextRenderingHint *pVal);


void __RPC_STUB ILabels_get_TextRenderingHint_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ILabels_put_TextRenderingHint_Proxy( 
    ILabels * This,
    /* [in] */ tkTextRenderingHint newVal);


void __RPC_STUB ILabels_put_TextRenderingHint_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ILabels_get_CompositingQuality_Proxy( 
    ILabels * This,
    /* [retval][out] */ tkCompositingQuality *pVal);


void __RPC_STUB ILabels_get_CompositingQuality_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ILabels_put_CompositingQuality_Proxy( 
    ILabels * This,
    /* [in] */ tkCompositingQuality newVal);


void __RPC_STUB ILabels_put_CompositingQuality_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE ILabels_get_SmoothingMode_Proxy( 
    ILabels * This,
    /* [retval][out] */ tkSmoothingMode *pVal);


void __RPC_STUB ILabels_get_SmoothingMode_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE ILabels_put_SmoothingMode_Proxy( 
    ILabels * This,
    /* [in] */ tkSmoothingMode newVal);


void __RPC_STUB ILabels_put_SmoothingMode_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __ILabels_INTERFACE_DEFINED__ */


#ifndef __ILabelCategory_INTERFACE_DEFINED__
#define __ILabelCategory_INTERFACE_DEFINED__

/* interface ILabelCategory */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ILabelCategory;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4BB3D2B2-A72D-4538-A092-9E1E69ED6001")
    ILabelCategory : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Priority( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Priority( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Name( 
            /* [retval][out] */ BSTR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Name( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Expression( 
            /* [retval][out] */ BSTR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Expression( 
            /* [in] */ BSTR retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MinValue( 
            /* [retval][out] */ VARIANT *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MinValue( 
            /* [in] */ VARIANT newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MaxValue( 
            /* [retval][out] */ VARIANT *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MaxValue( 
            /* [in] */ VARIANT newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Visible( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Visible( 
            /* [in] */ VARIANT_BOOL retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_OffsetX( 
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_OffsetX( 
            /* [in] */ double retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_OffsetY( 
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_OffsetY( 
            /* [in] */ double retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Alignment( 
            /* [retval][out] */ tkLabelAlignment *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Alignment( 
            /* [in] */ tkLabelAlignment retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LineOrientation( 
            /* [retval][out] */ tkLineLabelOrientation *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LineOrientation( 
            /* [in] */ tkLineLabelOrientation newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontName( 
            /* [retval][out] */ BSTR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontName( 
            /* [in] */ BSTR retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontSize( 
            /* [retval][out] */ long *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontSize( 
            /* [in] */ long retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontItalic( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontItalic( 
            /* [in] */ VARIANT_BOOL retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontBold( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontBold( 
            /* [in] */ VARIANT_BOOL retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontUnderline( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontUnderline( 
            /* [in] */ VARIANT_BOOL retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontStrikeOut( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontStrikeOut( 
            /* [in] */ VARIANT_BOOL retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontColor( 
            /* [in] */ OLE_COLOR retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontColor2( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontColor2( 
            /* [in] */ OLE_COLOR retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontGradientMode( 
            /* [retval][out] */ tkLinearGradientMode *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontGradientMode( 
            /* [in] */ tkLinearGradientMode retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontTransparency( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontTransparency( 
            /* [in] */ LONG retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontOutlineVisible( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontOutlineVisible( 
            /* [in] */ VARIANT_BOOL retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShadowVisible( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ShadowVisible( 
            /* [in] */ VARIANT_BOOL retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_HaloVisible( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_HaloVisible( 
            /* [in] */ VARIANT_BOOL retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontOutlineColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontOutlineColor( 
            /* [in] */ OLE_COLOR retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShadowColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ShadowColor( 
            /* [in] */ OLE_COLOR retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_HaloColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_HaloColor( 
            /* [in] */ OLE_COLOR retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FontOutlineWidth( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FontOutlineWidth( 
            /* [in] */ LONG retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShadowOffsetX( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ShadowOffsetX( 
            /* [in] */ LONG retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShadowOffsetY( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ShadowOffsetY( 
            /* [in] */ LONG retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_HaloSize( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_HaloSize( 
            /* [in] */ LONG retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameType( 
            /* [retval][out] */ tkLabelFrameType *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameType( 
            /* [in] */ tkLabelFrameType newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameOutlineColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameOutlineColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameBackColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameBackColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameBackColor2( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameBackColor2( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameGradientMode( 
            /* [retval][out] */ tkLinearGradientMode *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameGradientMode( 
            /* [in] */ tkLinearGradientMode retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameOutlineStyle( 
            /* [retval][out] */ tkDashStyle *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameOutlineStyle( 
            /* [in] */ tkDashStyle retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameOutlineWidth( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameOutlineWidth( 
            /* [in] */ LONG retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FramePaddingX( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FramePaddingX( 
            /* [in] */ LONG retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FramePaddingY( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FramePaddingY( 
            /* [in] */ LONG retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameTransparency( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameTransparency( 
            /* [in] */ LONG retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_InboxAlignment( 
            /* [retval][out] */ tkLabelAlignment *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_InboxAlignment( 
            /* [in] */ tkLabelAlignment newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FrameVisible( 
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FrameVisible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [hidden][helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Enabled( 
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [hidden][helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Enabled( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Serialize( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Deserialize( 
            /* [in] */ BSTR newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ILabelCategoryVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ILabelCategory * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ILabelCategory * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ILabelCategory * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ILabelCategory * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ILabelCategory * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ILabelCategory * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ILabelCategory * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Priority )( 
            ILabelCategory * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Priority )( 
            ILabelCategory * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Name )( 
            ILabelCategory * This,
            /* [retval][out] */ BSTR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Name )( 
            ILabelCategory * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Expression )( 
            ILabelCategory * This,
            /* [retval][out] */ BSTR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Expression )( 
            ILabelCategory * This,
            /* [in] */ BSTR retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MinValue )( 
            ILabelCategory * This,
            /* [retval][out] */ VARIANT *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MinValue )( 
            ILabelCategory * This,
            /* [in] */ VARIANT newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MaxValue )( 
            ILabelCategory * This,
            /* [retval][out] */ VARIANT *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MaxValue )( 
            ILabelCategory * This,
            /* [in] */ VARIANT newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Visible )( 
            ILabelCategory * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Visible )( 
            ILabelCategory * This,
            /* [in] */ VARIANT_BOOL retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_OffsetX )( 
            ILabelCategory * This,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_OffsetX )( 
            ILabelCategory * This,
            /* [in] */ double retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_OffsetY )( 
            ILabelCategory * This,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_OffsetY )( 
            ILabelCategory * This,
            /* [in] */ double retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Alignment )( 
            ILabelCategory * This,
            /* [retval][out] */ tkLabelAlignment *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Alignment )( 
            ILabelCategory * This,
            /* [in] */ tkLabelAlignment retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LineOrientation )( 
            ILabelCategory * This,
            /* [retval][out] */ tkLineLabelOrientation *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LineOrientation )( 
            ILabelCategory * This,
            /* [in] */ tkLineLabelOrientation newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontName )( 
            ILabelCategory * This,
            /* [retval][out] */ BSTR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontName )( 
            ILabelCategory * This,
            /* [in] */ BSTR retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontSize )( 
            ILabelCategory * This,
            /* [retval][out] */ long *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontSize )( 
            ILabelCategory * This,
            /* [in] */ long retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontItalic )( 
            ILabelCategory * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontItalic )( 
            ILabelCategory * This,
            /* [in] */ VARIANT_BOOL retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontBold )( 
            ILabelCategory * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontBold )( 
            ILabelCategory * This,
            /* [in] */ VARIANT_BOOL retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontUnderline )( 
            ILabelCategory * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontUnderline )( 
            ILabelCategory * This,
            /* [in] */ VARIANT_BOOL retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontStrikeOut )( 
            ILabelCategory * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontStrikeOut )( 
            ILabelCategory * This,
            /* [in] */ VARIANT_BOOL retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontColor )( 
            ILabelCategory * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontColor )( 
            ILabelCategory * This,
            /* [in] */ OLE_COLOR retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontColor2 )( 
            ILabelCategory * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontColor2 )( 
            ILabelCategory * This,
            /* [in] */ OLE_COLOR retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontGradientMode )( 
            ILabelCategory * This,
            /* [retval][out] */ tkLinearGradientMode *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontGradientMode )( 
            ILabelCategory * This,
            /* [in] */ tkLinearGradientMode retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontTransparency )( 
            ILabelCategory * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontTransparency )( 
            ILabelCategory * This,
            /* [in] */ LONG retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontOutlineVisible )( 
            ILabelCategory * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontOutlineVisible )( 
            ILabelCategory * This,
            /* [in] */ VARIANT_BOOL retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShadowVisible )( 
            ILabelCategory * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ShadowVisible )( 
            ILabelCategory * This,
            /* [in] */ VARIANT_BOOL retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_HaloVisible )( 
            ILabelCategory * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_HaloVisible )( 
            ILabelCategory * This,
            /* [in] */ VARIANT_BOOL retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontOutlineColor )( 
            ILabelCategory * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontOutlineColor )( 
            ILabelCategory * This,
            /* [in] */ OLE_COLOR retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShadowColor )( 
            ILabelCategory * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ShadowColor )( 
            ILabelCategory * This,
            /* [in] */ OLE_COLOR retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_HaloColor )( 
            ILabelCategory * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_HaloColor )( 
            ILabelCategory * This,
            /* [in] */ OLE_COLOR retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FontOutlineWidth )( 
            ILabelCategory * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FontOutlineWidth )( 
            ILabelCategory * This,
            /* [in] */ LONG retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShadowOffsetX )( 
            ILabelCategory * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ShadowOffsetX )( 
            ILabelCategory * This,
            /* [in] */ LONG retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShadowOffsetY )( 
            ILabelCategory * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ShadowOffsetY )( 
            ILabelCategory * This,
            /* [in] */ LONG retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_HaloSize )( 
            ILabelCategory * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_HaloSize )( 
            ILabelCategory * This,
            /* [in] */ LONG retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameType )( 
            ILabelCategory * This,
            /* [retval][out] */ tkLabelFrameType *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameType )( 
            ILabelCategory * This,
            /* [in] */ tkLabelFrameType newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameOutlineColor )( 
            ILabelCategory * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameOutlineColor )( 
            ILabelCategory * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameBackColor )( 
            ILabelCategory * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameBackColor )( 
            ILabelCategory * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameBackColor2 )( 
            ILabelCategory * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameBackColor2 )( 
            ILabelCategory * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameGradientMode )( 
            ILabelCategory * This,
            /* [retval][out] */ tkLinearGradientMode *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameGradientMode )( 
            ILabelCategory * This,
            /* [in] */ tkLinearGradientMode retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameOutlineStyle )( 
            ILabelCategory * This,
            /* [retval][out] */ tkDashStyle *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameOutlineStyle )( 
            ILabelCategory * This,
            /* [in] */ tkDashStyle retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameOutlineWidth )( 
            ILabelCategory * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameOutlineWidth )( 
            ILabelCategory * This,
            /* [in] */ LONG retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FramePaddingX )( 
            ILabelCategory * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FramePaddingX )( 
            ILabelCategory * This,
            /* [in] */ LONG retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FramePaddingY )( 
            ILabelCategory * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FramePaddingY )( 
            ILabelCategory * This,
            /* [in] */ LONG retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameTransparency )( 
            ILabelCategory * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameTransparency )( 
            ILabelCategory * This,
            /* [in] */ LONG retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_InboxAlignment )( 
            ILabelCategory * This,
            /* [retval][out] */ tkLabelAlignment *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_InboxAlignment )( 
            ILabelCategory * This,
            /* [in] */ tkLabelAlignment newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FrameVisible )( 
            ILabelCategory * This,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FrameVisible )( 
            ILabelCategory * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [hidden][helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Enabled )( 
            ILabelCategory * This,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [hidden][helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Enabled )( 
            ILabelCategory * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Serialize )( 
            ILabelCategory * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Deserialize )( 
            ILabelCategory * This,
            /* [in] */ BSTR newVal);
        
        END_INTERFACE
    } ILabelCategoryVtbl;

    interface ILabelCategory
    {
        CONST_VTBL struct ILabelCategoryVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ILabelCategory_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ILabelCategory_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ILabelCategory_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ILabelCategory_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ILabelCategory_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ILabelCategory_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ILabelCategory_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ILabelCategory_get_Priority(This,retval)	\
    ( (This)->lpVtbl -> get_Priority(This,retval) ) 

#define ILabelCategory_put_Priority(This,newVal)	\
    ( (This)->lpVtbl -> put_Priority(This,newVal) ) 

#define ILabelCategory_get_Name(This,retval)	\
    ( (This)->lpVtbl -> get_Name(This,retval) ) 

#define ILabelCategory_put_Name(This,newVal)	\
    ( (This)->lpVtbl -> put_Name(This,newVal) ) 

#define ILabelCategory_get_Expression(This,retval)	\
    ( (This)->lpVtbl -> get_Expression(This,retval) ) 

#define ILabelCategory_put_Expression(This,retval)	\
    ( (This)->lpVtbl -> put_Expression(This,retval) ) 

#define ILabelCategory_get_MinValue(This,pVal)	\
    ( (This)->lpVtbl -> get_MinValue(This,pVal) ) 

#define ILabelCategory_put_MinValue(This,newVal)	\
    ( (This)->lpVtbl -> put_MinValue(This,newVal) ) 

#define ILabelCategory_get_MaxValue(This,pVal)	\
    ( (This)->lpVtbl -> get_MaxValue(This,pVal) ) 

#define ILabelCategory_put_MaxValue(This,newVal)	\
    ( (This)->lpVtbl -> put_MaxValue(This,newVal) ) 

#define ILabelCategory_get_Visible(This,retval)	\
    ( (This)->lpVtbl -> get_Visible(This,retval) ) 

#define ILabelCategory_put_Visible(This,retval)	\
    ( (This)->lpVtbl -> put_Visible(This,retval) ) 

#define ILabelCategory_get_OffsetX(This,retval)	\
    ( (This)->lpVtbl -> get_OffsetX(This,retval) ) 

#define ILabelCategory_put_OffsetX(This,retval)	\
    ( (This)->lpVtbl -> put_OffsetX(This,retval) ) 

#define ILabelCategory_get_OffsetY(This,retval)	\
    ( (This)->lpVtbl -> get_OffsetY(This,retval) ) 

#define ILabelCategory_put_OffsetY(This,retval)	\
    ( (This)->lpVtbl -> put_OffsetY(This,retval) ) 

#define ILabelCategory_get_Alignment(This,retval)	\
    ( (This)->lpVtbl -> get_Alignment(This,retval) ) 

#define ILabelCategory_put_Alignment(This,retval)	\
    ( (This)->lpVtbl -> put_Alignment(This,retval) ) 

#define ILabelCategory_get_LineOrientation(This,retval)	\
    ( (This)->lpVtbl -> get_LineOrientation(This,retval) ) 

#define ILabelCategory_put_LineOrientation(This,newVal)	\
    ( (This)->lpVtbl -> put_LineOrientation(This,newVal) ) 

#define ILabelCategory_get_FontName(This,retval)	\
    ( (This)->lpVtbl -> get_FontName(This,retval) ) 

#define ILabelCategory_put_FontName(This,retval)	\
    ( (This)->lpVtbl -> put_FontName(This,retval) ) 

#define ILabelCategory_get_FontSize(This,retval)	\
    ( (This)->lpVtbl -> get_FontSize(This,retval) ) 

#define ILabelCategory_put_FontSize(This,retval)	\
    ( (This)->lpVtbl -> put_FontSize(This,retval) ) 

#define ILabelCategory_get_FontItalic(This,retval)	\
    ( (This)->lpVtbl -> get_FontItalic(This,retval) ) 

#define ILabelCategory_put_FontItalic(This,retval)	\
    ( (This)->lpVtbl -> put_FontItalic(This,retval) ) 

#define ILabelCategory_get_FontBold(This,retval)	\
    ( (This)->lpVtbl -> get_FontBold(This,retval) ) 

#define ILabelCategory_put_FontBold(This,retval)	\
    ( (This)->lpVtbl -> put_FontBold(This,retval) ) 

#define ILabelCategory_get_FontUnderline(This,retval)	\
    ( (This)->lpVtbl -> get_FontUnderline(This,retval) ) 

#define ILabelCategory_put_FontUnderline(This,retval)	\
    ( (This)->lpVtbl -> put_FontUnderline(This,retval) ) 

#define ILabelCategory_get_FontStrikeOut(This,retval)	\
    ( (This)->lpVtbl -> get_FontStrikeOut(This,retval) ) 

#define ILabelCategory_put_FontStrikeOut(This,retval)	\
    ( (This)->lpVtbl -> put_FontStrikeOut(This,retval) ) 

#define ILabelCategory_get_FontColor(This,retval)	\
    ( (This)->lpVtbl -> get_FontColor(This,retval) ) 

#define ILabelCategory_put_FontColor(This,retval)	\
    ( (This)->lpVtbl -> put_FontColor(This,retval) ) 

#define ILabelCategory_get_FontColor2(This,retval)	\
    ( (This)->lpVtbl -> get_FontColor2(This,retval) ) 

#define ILabelCategory_put_FontColor2(This,retval)	\
    ( (This)->lpVtbl -> put_FontColor2(This,retval) ) 

#define ILabelCategory_get_FontGradientMode(This,retval)	\
    ( (This)->lpVtbl -> get_FontGradientMode(This,retval) ) 

#define ILabelCategory_put_FontGradientMode(This,retval)	\
    ( (This)->lpVtbl -> put_FontGradientMode(This,retval) ) 

#define ILabelCategory_get_FontTransparency(This,retval)	\
    ( (This)->lpVtbl -> get_FontTransparency(This,retval) ) 

#define ILabelCategory_put_FontTransparency(This,retval)	\
    ( (This)->lpVtbl -> put_FontTransparency(This,retval) ) 

#define ILabelCategory_get_FontOutlineVisible(This,retval)	\
    ( (This)->lpVtbl -> get_FontOutlineVisible(This,retval) ) 

#define ILabelCategory_put_FontOutlineVisible(This,retval)	\
    ( (This)->lpVtbl -> put_FontOutlineVisible(This,retval) ) 

#define ILabelCategory_get_ShadowVisible(This,retval)	\
    ( (This)->lpVtbl -> get_ShadowVisible(This,retval) ) 

#define ILabelCategory_put_ShadowVisible(This,retval)	\
    ( (This)->lpVtbl -> put_ShadowVisible(This,retval) ) 

#define ILabelCategory_get_HaloVisible(This,retval)	\
    ( (This)->lpVtbl -> get_HaloVisible(This,retval) ) 

#define ILabelCategory_put_HaloVisible(This,retval)	\
    ( (This)->lpVtbl -> put_HaloVisible(This,retval) ) 

#define ILabelCategory_get_FontOutlineColor(This,retval)	\
    ( (This)->lpVtbl -> get_FontOutlineColor(This,retval) ) 

#define ILabelCategory_put_FontOutlineColor(This,retval)	\
    ( (This)->lpVtbl -> put_FontOutlineColor(This,retval) ) 

#define ILabelCategory_get_ShadowColor(This,retval)	\
    ( (This)->lpVtbl -> get_ShadowColor(This,retval) ) 

#define ILabelCategory_put_ShadowColor(This,retval)	\
    ( (This)->lpVtbl -> put_ShadowColor(This,retval) ) 

#define ILabelCategory_get_HaloColor(This,retval)	\
    ( (This)->lpVtbl -> get_HaloColor(This,retval) ) 

#define ILabelCategory_put_HaloColor(This,retval)	\
    ( (This)->lpVtbl -> put_HaloColor(This,retval) ) 

#define ILabelCategory_get_FontOutlineWidth(This,retval)	\
    ( (This)->lpVtbl -> get_FontOutlineWidth(This,retval) ) 

#define ILabelCategory_put_FontOutlineWidth(This,retval)	\
    ( (This)->lpVtbl -> put_FontOutlineWidth(This,retval) ) 

#define ILabelCategory_get_ShadowOffsetX(This,retval)	\
    ( (This)->lpVtbl -> get_ShadowOffsetX(This,retval) ) 

#define ILabelCategory_put_ShadowOffsetX(This,retval)	\
    ( (This)->lpVtbl -> put_ShadowOffsetX(This,retval) ) 

#define ILabelCategory_get_ShadowOffsetY(This,retval)	\
    ( (This)->lpVtbl -> get_ShadowOffsetY(This,retval) ) 

#define ILabelCategory_put_ShadowOffsetY(This,retval)	\
    ( (This)->lpVtbl -> put_ShadowOffsetY(This,retval) ) 

#define ILabelCategory_get_HaloSize(This,retval)	\
    ( (This)->lpVtbl -> get_HaloSize(This,retval) ) 

#define ILabelCategory_put_HaloSize(This,retval)	\
    ( (This)->lpVtbl -> put_HaloSize(This,retval) ) 

#define ILabelCategory_get_FrameType(This,retval)	\
    ( (This)->lpVtbl -> get_FrameType(This,retval) ) 

#define ILabelCategory_put_FrameType(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameType(This,newVal) ) 

#define ILabelCategory_get_FrameOutlineColor(This,retval)	\
    ( (This)->lpVtbl -> get_FrameOutlineColor(This,retval) ) 

#define ILabelCategory_put_FrameOutlineColor(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameOutlineColor(This,newVal) ) 

#define ILabelCategory_get_FrameBackColor(This,retval)	\
    ( (This)->lpVtbl -> get_FrameBackColor(This,retval) ) 

#define ILabelCategory_put_FrameBackColor(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameBackColor(This,newVal) ) 

#define ILabelCategory_get_FrameBackColor2(This,retval)	\
    ( (This)->lpVtbl -> get_FrameBackColor2(This,retval) ) 

#define ILabelCategory_put_FrameBackColor2(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameBackColor2(This,newVal) ) 

#define ILabelCategory_get_FrameGradientMode(This,retval)	\
    ( (This)->lpVtbl -> get_FrameGradientMode(This,retval) ) 

#define ILabelCategory_put_FrameGradientMode(This,retval)	\
    ( (This)->lpVtbl -> put_FrameGradientMode(This,retval) ) 

#define ILabelCategory_get_FrameOutlineStyle(This,retval)	\
    ( (This)->lpVtbl -> get_FrameOutlineStyle(This,retval) ) 

#define ILabelCategory_put_FrameOutlineStyle(This,retval)	\
    ( (This)->lpVtbl -> put_FrameOutlineStyle(This,retval) ) 

#define ILabelCategory_get_FrameOutlineWidth(This,retval)	\
    ( (This)->lpVtbl -> get_FrameOutlineWidth(This,retval) ) 

#define ILabelCategory_put_FrameOutlineWidth(This,retval)	\
    ( (This)->lpVtbl -> put_FrameOutlineWidth(This,retval) ) 

#define ILabelCategory_get_FramePaddingX(This,retval)	\
    ( (This)->lpVtbl -> get_FramePaddingX(This,retval) ) 

#define ILabelCategory_put_FramePaddingX(This,retval)	\
    ( (This)->lpVtbl -> put_FramePaddingX(This,retval) ) 

#define ILabelCategory_get_FramePaddingY(This,retval)	\
    ( (This)->lpVtbl -> get_FramePaddingY(This,retval) ) 

#define ILabelCategory_put_FramePaddingY(This,retval)	\
    ( (This)->lpVtbl -> put_FramePaddingY(This,retval) ) 

#define ILabelCategory_get_FrameTransparency(This,retval)	\
    ( (This)->lpVtbl -> get_FrameTransparency(This,retval) ) 

#define ILabelCategory_put_FrameTransparency(This,retval)	\
    ( (This)->lpVtbl -> put_FrameTransparency(This,retval) ) 

#define ILabelCategory_get_InboxAlignment(This,retval)	\
    ( (This)->lpVtbl -> get_InboxAlignment(This,retval) ) 

#define ILabelCategory_put_InboxAlignment(This,newVal)	\
    ( (This)->lpVtbl -> put_InboxAlignment(This,newVal) ) 

#define ILabelCategory_get_FrameVisible(This,retVal)	\
    ( (This)->lpVtbl -> get_FrameVisible(This,retVal) ) 

#define ILabelCategory_put_FrameVisible(This,newVal)	\
    ( (This)->lpVtbl -> put_FrameVisible(This,newVal) ) 

#define ILabelCategory_get_Enabled(This,retVal)	\
    ( (This)->lpVtbl -> get_Enabled(This,retVal) ) 

#define ILabelCategory_put_Enabled(This,newVal)	\
    ( (This)->lpVtbl -> put_Enabled(This,newVal) ) 

#define ILabelCategory_Serialize(This,retVal)	\
    ( (This)->lpVtbl -> Serialize(This,retVal) ) 

#define ILabelCategory_Deserialize(This,newVal)	\
    ( (This)->lpVtbl -> Deserialize(This,newVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ILabelCategory_INTERFACE_DEFINED__ */


#ifndef __IShapefileCategories_INTERFACE_DEFINED__
#define __IShapefileCategories_INTERFACE_DEFINED__

/* interface IShapefileCategories */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IShapefileCategories;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("EC594CB1-FA55-469C-B662-192F7A464C23")
    IShapefileCategories : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Add( 
            /* [in] */ BSTR Name,
            /* [retval][out] */ IShapefileCategory **retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Insert( 
            /* [in] */ long Index,
            /* [in] */ BSTR Name,
            /* [retval][out] */ IShapefileCategory **retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Remove( 
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *vbretval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clear( void) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Item( 
            /* [in] */ long Index,
            /* [retval][out] */ IShapefileCategory **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Item( 
            /* [in] */ long Index,
            /* [in] */ IShapefileCategory *newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Generate( 
            /* [in] */ long FieldIndex,
            /* [in] */ tkClassificationType ClassificationType,
            /* [in] */ long numClasses,
            /* [retval][out] */ VARIANT_BOOL *vbretval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Count( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Shapefile( 
            /* [retval][out] */ IShapefile **retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ApplyExpressions( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ApplyExpression( 
            /* [in] */ long CategoryIndex) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ApplyColorScheme( 
            /* [in] */ tkColorSchemeType Type,
            /* [in] */ IColorScheme *ColorScheme) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ApplyColorScheme2( 
            /* [in] */ tkColorSchemeType Type,
            /* [in] */ IColorScheme *ColorScheme,
            /* [in] */ tkShapeElements ShapeElement) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ApplyColorScheme3( 
            /* [in] */ tkColorSchemeType Type,
            /* [in] */ IColorScheme *ColorScheme,
            /* [in] */ tkShapeElements ShapeElement,
            /* [in] */ long CategoryStartIndex,
            /* [in] */ long CategoryEndIndex) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Caption( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Caption( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE MoveUp( 
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE MoveDown( 
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Serialize( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Deserialize( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AddRange( 
            /* [in] */ long FieldIndex,
            /* [in] */ tkClassificationType ClassificationType,
            /* [in] */ long numClasses,
            /* [in] */ VARIANT minValue,
            /* [in] */ VARIANT maxValue,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IShapefileCategoriesVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IShapefileCategories * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IShapefileCategories * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IShapefileCategories * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IShapefileCategories * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IShapefileCategories * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IShapefileCategories * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IShapefileCategories * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IShapefileCategories * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IShapefileCategories * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IShapefileCategories * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IShapefileCategories * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IShapefileCategories * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IShapefileCategories * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Add )( 
            IShapefileCategories * This,
            /* [in] */ BSTR Name,
            /* [retval][out] */ IShapefileCategory **retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Insert )( 
            IShapefileCategories * This,
            /* [in] */ long Index,
            /* [in] */ BSTR Name,
            /* [retval][out] */ IShapefileCategory **retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Remove )( 
            IShapefileCategories * This,
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *vbretval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clear )( 
            IShapefileCategories * This);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Item )( 
            IShapefileCategories * This,
            /* [in] */ long Index,
            /* [retval][out] */ IShapefileCategory **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Item )( 
            IShapefileCategories * This,
            /* [in] */ long Index,
            /* [in] */ IShapefileCategory *newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Generate )( 
            IShapefileCategories * This,
            /* [in] */ long FieldIndex,
            /* [in] */ tkClassificationType ClassificationType,
            /* [in] */ long numClasses,
            /* [retval][out] */ VARIANT_BOOL *vbretval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Count )( 
            IShapefileCategories * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Shapefile )( 
            IShapefileCategories * This,
            /* [retval][out] */ IShapefile **retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ApplyExpressions )( 
            IShapefileCategories * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ApplyExpression )( 
            IShapefileCategories * This,
            /* [in] */ long CategoryIndex);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ApplyColorScheme )( 
            IShapefileCategories * This,
            /* [in] */ tkColorSchemeType Type,
            /* [in] */ IColorScheme *ColorScheme);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ApplyColorScheme2 )( 
            IShapefileCategories * This,
            /* [in] */ tkColorSchemeType Type,
            /* [in] */ IColorScheme *ColorScheme,
            /* [in] */ tkShapeElements ShapeElement);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ApplyColorScheme3 )( 
            IShapefileCategories * This,
            /* [in] */ tkColorSchemeType Type,
            /* [in] */ IColorScheme *ColorScheme,
            /* [in] */ tkShapeElements ShapeElement,
            /* [in] */ long CategoryStartIndex,
            /* [in] */ long CategoryEndIndex);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Caption )( 
            IShapefileCategories * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Caption )( 
            IShapefileCategories * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *MoveUp )( 
            IShapefileCategories * This,
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *MoveDown )( 
            IShapefileCategories * This,
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Serialize )( 
            IShapefileCategories * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Deserialize )( 
            IShapefileCategories * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AddRange )( 
            IShapefileCategories * This,
            /* [in] */ long FieldIndex,
            /* [in] */ tkClassificationType ClassificationType,
            /* [in] */ long numClasses,
            /* [in] */ VARIANT minValue,
            /* [in] */ VARIANT maxValue,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        END_INTERFACE
    } IShapefileCategoriesVtbl;

    interface IShapefileCategories
    {
        CONST_VTBL struct IShapefileCategoriesVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IShapefileCategories_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IShapefileCategories_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IShapefileCategories_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IShapefileCategories_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IShapefileCategories_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IShapefileCategories_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IShapefileCategories_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IShapefileCategories_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IShapefileCategories_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IShapefileCategories_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IShapefileCategories_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IShapefileCategories_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IShapefileCategories_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IShapefileCategories_Add(This,Name,retVal)	\
    ( (This)->lpVtbl -> Add(This,Name,retVal) ) 

#define IShapefileCategories_Insert(This,Index,Name,retVal)	\
    ( (This)->lpVtbl -> Insert(This,Index,Name,retVal) ) 

#define IShapefileCategories_Remove(This,Index,vbretval)	\
    ( (This)->lpVtbl -> Remove(This,Index,vbretval) ) 

#define IShapefileCategories_Clear(This)	\
    ( (This)->lpVtbl -> Clear(This) ) 

#define IShapefileCategories_get_Item(This,Index,pVal)	\
    ( (This)->lpVtbl -> get_Item(This,Index,pVal) ) 

#define IShapefileCategories_put_Item(This,Index,newVal)	\
    ( (This)->lpVtbl -> put_Item(This,Index,newVal) ) 

#define IShapefileCategories_Generate(This,FieldIndex,ClassificationType,numClasses,vbretval)	\
    ( (This)->lpVtbl -> Generate(This,FieldIndex,ClassificationType,numClasses,vbretval) ) 

#define IShapefileCategories_get_Count(This,pVal)	\
    ( (This)->lpVtbl -> get_Count(This,pVal) ) 

#define IShapefileCategories_get_Shapefile(This,retVal)	\
    ( (This)->lpVtbl -> get_Shapefile(This,retVal) ) 

#define IShapefileCategories_ApplyExpressions(This)	\
    ( (This)->lpVtbl -> ApplyExpressions(This) ) 

#define IShapefileCategories_ApplyExpression(This,CategoryIndex)	\
    ( (This)->lpVtbl -> ApplyExpression(This,CategoryIndex) ) 

#define IShapefileCategories_ApplyColorScheme(This,Type,ColorScheme)	\
    ( (This)->lpVtbl -> ApplyColorScheme(This,Type,ColorScheme) ) 

#define IShapefileCategories_ApplyColorScheme2(This,Type,ColorScheme,ShapeElement)	\
    ( (This)->lpVtbl -> ApplyColorScheme2(This,Type,ColorScheme,ShapeElement) ) 

#define IShapefileCategories_ApplyColorScheme3(This,Type,ColorScheme,ShapeElement,CategoryStartIndex,CategoryEndIndex)	\
    ( (This)->lpVtbl -> ApplyColorScheme3(This,Type,ColorScheme,ShapeElement,CategoryStartIndex,CategoryEndIndex) ) 

#define IShapefileCategories_get_Caption(This,pVal)	\
    ( (This)->lpVtbl -> get_Caption(This,pVal) ) 

#define IShapefileCategories_put_Caption(This,newVal)	\
    ( (This)->lpVtbl -> put_Caption(This,newVal) ) 

#define IShapefileCategories_MoveUp(This,Index,retval)	\
    ( (This)->lpVtbl -> MoveUp(This,Index,retval) ) 

#define IShapefileCategories_MoveDown(This,Index,retval)	\
    ( (This)->lpVtbl -> MoveDown(This,Index,retval) ) 

#define IShapefileCategories_Serialize(This,retVal)	\
    ( (This)->lpVtbl -> Serialize(This,retVal) ) 

#define IShapefileCategories_Deserialize(This,newVal)	\
    ( (This)->lpVtbl -> Deserialize(This,newVal) ) 

#define IShapefileCategories_AddRange(This,FieldIndex,ClassificationType,numClasses,minValue,maxValue,retVal)	\
    ( (This)->lpVtbl -> AddRange(This,FieldIndex,ClassificationType,numClasses,minValue,maxValue,retVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IShapefileCategories_INTERFACE_DEFINED__ */


#ifndef __IShapefileCategory_INTERFACE_DEFINED__
#define __IShapefileCategory_INTERFACE_DEFINED__

/* interface IShapefileCategory */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IShapefileCategory;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("688EB3FF-CF7A-490C-9BC7-BE47CEB32C59")
    IShapefileCategory : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Name( 
            /* [retval][out] */ BSTR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Name( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Expression( 
            /* [retval][out] */ BSTR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Expression( 
            /* [in] */ BSTR retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_DrawingOptions( 
            /* [retval][out] */ IShapeDrawingOptions **retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_DrawingOptions( 
            /* [in] */ IShapeDrawingOptions *retval) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IShapefileCategoryVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IShapefileCategory * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IShapefileCategory * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IShapefileCategory * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IShapefileCategory * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IShapefileCategory * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IShapefileCategory * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IShapefileCategory * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Name )( 
            IShapefileCategory * This,
            /* [retval][out] */ BSTR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Name )( 
            IShapefileCategory * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Expression )( 
            IShapefileCategory * This,
            /* [retval][out] */ BSTR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Expression )( 
            IShapefileCategory * This,
            /* [in] */ BSTR retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DrawingOptions )( 
            IShapefileCategory * This,
            /* [retval][out] */ IShapeDrawingOptions **retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DrawingOptions )( 
            IShapefileCategory * This,
            /* [in] */ IShapeDrawingOptions *retval);
        
        END_INTERFACE
    } IShapefileCategoryVtbl;

    interface IShapefileCategory
    {
        CONST_VTBL struct IShapefileCategoryVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IShapefileCategory_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IShapefileCategory_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IShapefileCategory_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IShapefileCategory_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IShapefileCategory_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IShapefileCategory_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IShapefileCategory_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IShapefileCategory_get_Name(This,retval)	\
    ( (This)->lpVtbl -> get_Name(This,retval) ) 

#define IShapefileCategory_put_Name(This,newVal)	\
    ( (This)->lpVtbl -> put_Name(This,newVal) ) 

#define IShapefileCategory_get_Expression(This,retval)	\
    ( (This)->lpVtbl -> get_Expression(This,retval) ) 

#define IShapefileCategory_put_Expression(This,retval)	\
    ( (This)->lpVtbl -> put_Expression(This,retval) ) 

#define IShapefileCategory_get_DrawingOptions(This,retval)	\
    ( (This)->lpVtbl -> get_DrawingOptions(This,retval) ) 

#define IShapefileCategory_put_DrawingOptions(This,retval)	\
    ( (This)->lpVtbl -> put_DrawingOptions(This,retval) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IShapefileCategory_INTERFACE_DEFINED__ */


#ifndef __ICharts_INTERFACE_DEFINED__
#define __ICharts_INTERFACE_DEFINED__

/* interface ICharts */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ICharts;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("D98BB982-8D47-47BC-81CA-0EFA15D1B4F6")
    ICharts : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Visible( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Visible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_AvoidCollisions( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_AvoidCollisions( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ChartType( 
            /* [retval][out] */ tkChartType *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ChartType( 
            /* [in] */ tkChartType newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_BarWidth( 
            /* [retval][out] */ LONG *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_BarWidth( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_BarHeight( 
            /* [retval][out] */ LONG *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_BarHeight( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PieRadius( 
            /* [retval][out] */ LONG *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PieRadius( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PieRotation( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PieRotation( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumFields( 
            /* [retval][out] */ LONG *newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AddField2( 
            /* [in] */ LONG FieldIndex,
            /* [in] */ OLE_COLOR Color) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE InsertField2( 
            /* [in] */ LONG Index,
            /* [in] */ LONG FieldIndex,
            /* [in] */ OLE_COLOR Color,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE RemoveField( 
            /* [in] */ LONG Index,
            /* [retval][out] */ VARIANT_BOOL *vbretval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ClearFields( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE MoveField( 
            /* [in] */ LONG OldIndex,
            /* [in] */ LONG NewIndex,
            /* [retval][out] */ VARIANT_BOOL *vbretval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Generate( 
            /* [in] */ tkLabelPositioning Type,
            /* [retval][out] */ VARIANT_BOOL *vbretval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clear( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DrawChart( 
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ VARIANT_BOOL hideLabels,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Tilt( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Tilt( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Thickness( 
            /* [retval][out] */ double *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Thickness( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PieRadius2( 
            /* [retval][out] */ LONG *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PieRadius2( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SizeField( 
            /* [retval][out] */ LONG *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_SizeField( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NormalizationField( 
            /* [retval][out] */ LONG *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_NormalizationField( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_UseVariableRadius( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_UseVariableRadius( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Use3DMode( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Use3DMode( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Transparency( 
            /* [retval][out] */ SHORT *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Transparency( 
            /* [in] */ SHORT newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LineColor( 
            /* [retval][out] */ OLE_COLOR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LineColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_VerticalPosition( 
            /* [retval][out] */ tkVerticalPosition *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_VerticalPosition( 
            /* [in] */ tkVerticalPosition newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Chart( 
            /* [in] */ long Chart,
            /* [retval][out] */ IChart **retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Field( 
            /* [in] */ long FieldIndex,
            /* [retval][out] */ IChartField **retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AddField( 
            /* [in] */ IChartField *Field,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE InsertField( 
            /* [in] */ long Index,
            /* [in] */ IChartField *Field,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Count( 
            /* [retval][out] */ long *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MaxVisibleScale( 
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MaxVisibleScale( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MinVisibleScale( 
            /* [retval][out] */ double *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MinVisibleScale( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_DynamicVisibility( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_DynamicVisibility( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IconWidth( 
            /* [retval][out] */ long *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IconHeight( 
            /* [retval][out] */ long *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Caption( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Caption( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ValuesFontName( 
            /* [retval][out] */ BSTR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ValuesFontName( 
            /* [in] */ BSTR retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ValuesFontSize( 
            /* [retval][out] */ long *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ValuesFontSize( 
            /* [in] */ long retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ValuesFontItalic( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ValuesFontItalic( 
            /* [in] */ VARIANT_BOOL retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ValuesFontBold( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ValuesFontBold( 
            /* [in] */ VARIANT_BOOL retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ValuesFontColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ValuesFontColor( 
            /* [in] */ OLE_COLOR retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ValuesFrameVisible( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ValuesFrameVisible( 
            /* [in] */ VARIANT_BOOL retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ValuesFrameColor( 
            /* [retval][out] */ OLE_COLOR *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ValuesFrameColor( 
            /* [in] */ OLE_COLOR retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ValuesVisible( 
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ValuesVisible( 
            /* [in] */ VARIANT_BOOL retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ValuesStyle( 
            /* [retval][out] */ tkChartValuesStyle *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ValuesStyle( 
            /* [in] */ tkChartValuesStyle retval) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Select( 
            /* [in] */ IExtents *BoundingBox,
            /* [in] */ long Tolerance,
            /* [in] */ SelectMode SelectMode,
            /* [out][in] */ VARIANT *Indices,
            /* [retval][out] */ VARIANT_BOOL *retval) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_VisibilityExpression( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_VisibilityExpression( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CollisionBuffer( 
            /* [retval][out] */ long *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_CollisionBuffer( 
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_OffsetX( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_OffsetX( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_OffsetY( 
            /* [retval][out] */ LONG *retval) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_OffsetY( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Serialize( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Deserialize( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SaveToXML( 
            /* [in] */ BSTR Filename,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE LoadFromXML( 
            /* [in] */ BSTR Filename,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SavingMode( 
            /* [retval][out] */ tkSavingMode *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_SavingMode( 
            /* [in] */ tkSavingMode newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IChartsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ICharts * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ICharts * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ICharts * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ICharts * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ICharts * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ICharts * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ICharts * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            ICharts * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            ICharts * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Visible )( 
            ICharts * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Visible )( 
            ICharts * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_AvoidCollisions )( 
            ICharts * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_AvoidCollisions )( 
            ICharts * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ChartType )( 
            ICharts * This,
            /* [retval][out] */ tkChartType *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ChartType )( 
            ICharts * This,
            /* [in] */ tkChartType newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_BarWidth )( 
            ICharts * This,
            /* [retval][out] */ LONG *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_BarWidth )( 
            ICharts * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_BarHeight )( 
            ICharts * This,
            /* [retval][out] */ LONG *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_BarHeight )( 
            ICharts * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PieRadius )( 
            ICharts * This,
            /* [retval][out] */ LONG *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PieRadius )( 
            ICharts * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PieRotation )( 
            ICharts * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PieRotation )( 
            ICharts * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumFields )( 
            ICharts * This,
            /* [retval][out] */ LONG *newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AddField2 )( 
            ICharts * This,
            /* [in] */ LONG FieldIndex,
            /* [in] */ OLE_COLOR Color);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *InsertField2 )( 
            ICharts * This,
            /* [in] */ LONG Index,
            /* [in] */ LONG FieldIndex,
            /* [in] */ OLE_COLOR Color,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *RemoveField )( 
            ICharts * This,
            /* [in] */ LONG Index,
            /* [retval][out] */ VARIANT_BOOL *vbretval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ClearFields )( 
            ICharts * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *MoveField )( 
            ICharts * This,
            /* [in] */ LONG OldIndex,
            /* [in] */ LONG NewIndex,
            /* [retval][out] */ VARIANT_BOOL *vbretval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Generate )( 
            ICharts * This,
            /* [in] */ tkLabelPositioning Type,
            /* [retval][out] */ VARIANT_BOOL *vbretval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clear )( 
            ICharts * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DrawChart )( 
            ICharts * This,
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ VARIANT_BOOL hideLabels,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Tilt )( 
            ICharts * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Tilt )( 
            ICharts * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Thickness )( 
            ICharts * This,
            /* [retval][out] */ double *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Thickness )( 
            ICharts * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PieRadius2 )( 
            ICharts * This,
            /* [retval][out] */ LONG *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PieRadius2 )( 
            ICharts * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SizeField )( 
            ICharts * This,
            /* [retval][out] */ LONG *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_SizeField )( 
            ICharts * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NormalizationField )( 
            ICharts * This,
            /* [retval][out] */ LONG *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_NormalizationField )( 
            ICharts * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_UseVariableRadius )( 
            ICharts * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_UseVariableRadius )( 
            ICharts * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Use3DMode )( 
            ICharts * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Use3DMode )( 
            ICharts * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Transparency )( 
            ICharts * This,
            /* [retval][out] */ SHORT *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Transparency )( 
            ICharts * This,
            /* [in] */ SHORT newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LineColor )( 
            ICharts * This,
            /* [retval][out] */ OLE_COLOR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LineColor )( 
            ICharts * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_VerticalPosition )( 
            ICharts * This,
            /* [retval][out] */ tkVerticalPosition *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_VerticalPosition )( 
            ICharts * This,
            /* [in] */ tkVerticalPosition newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Chart )( 
            ICharts * This,
            /* [in] */ long Chart,
            /* [retval][out] */ IChart **retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Field )( 
            ICharts * This,
            /* [in] */ long FieldIndex,
            /* [retval][out] */ IChartField **retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AddField )( 
            ICharts * This,
            /* [in] */ IChartField *Field,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *InsertField )( 
            ICharts * This,
            /* [in] */ long Index,
            /* [in] */ IChartField *Field,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            ICharts * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            ICharts * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            ICharts * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            ICharts * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Count )( 
            ICharts * This,
            /* [retval][out] */ long *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MaxVisibleScale )( 
            ICharts * This,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MaxVisibleScale )( 
            ICharts * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MinVisibleScale )( 
            ICharts * This,
            /* [retval][out] */ double *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MinVisibleScale )( 
            ICharts * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DynamicVisibility )( 
            ICharts * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DynamicVisibility )( 
            ICharts * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IconWidth )( 
            ICharts * This,
            /* [retval][out] */ long *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IconHeight )( 
            ICharts * This,
            /* [retval][out] */ long *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Caption )( 
            ICharts * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Caption )( 
            ICharts * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ValuesFontName )( 
            ICharts * This,
            /* [retval][out] */ BSTR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ValuesFontName )( 
            ICharts * This,
            /* [in] */ BSTR retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ValuesFontSize )( 
            ICharts * This,
            /* [retval][out] */ long *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ValuesFontSize )( 
            ICharts * This,
            /* [in] */ long retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ValuesFontItalic )( 
            ICharts * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ValuesFontItalic )( 
            ICharts * This,
            /* [in] */ VARIANT_BOOL retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ValuesFontBold )( 
            ICharts * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ValuesFontBold )( 
            ICharts * This,
            /* [in] */ VARIANT_BOOL retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ValuesFontColor )( 
            ICharts * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ValuesFontColor )( 
            ICharts * This,
            /* [in] */ OLE_COLOR retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ValuesFrameVisible )( 
            ICharts * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ValuesFrameVisible )( 
            ICharts * This,
            /* [in] */ VARIANT_BOOL retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ValuesFrameColor )( 
            ICharts * This,
            /* [retval][out] */ OLE_COLOR *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ValuesFrameColor )( 
            ICharts * This,
            /* [in] */ OLE_COLOR retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ValuesVisible )( 
            ICharts * This,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ValuesVisible )( 
            ICharts * This,
            /* [in] */ VARIANT_BOOL retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ValuesStyle )( 
            ICharts * This,
            /* [retval][out] */ tkChartValuesStyle *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ValuesStyle )( 
            ICharts * This,
            /* [in] */ tkChartValuesStyle retval);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Select )( 
            ICharts * This,
            /* [in] */ IExtents *BoundingBox,
            /* [in] */ long Tolerance,
            /* [in] */ SelectMode SelectMode,
            /* [out][in] */ VARIANT *Indices,
            /* [retval][out] */ VARIANT_BOOL *retval);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_VisibilityExpression )( 
            ICharts * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_VisibilityExpression )( 
            ICharts * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CollisionBuffer )( 
            ICharts * This,
            /* [retval][out] */ long *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_CollisionBuffer )( 
            ICharts * This,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_OffsetX )( 
            ICharts * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_OffsetX )( 
            ICharts * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_OffsetY )( 
            ICharts * This,
            /* [retval][out] */ LONG *retval);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_OffsetY )( 
            ICharts * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Serialize )( 
            ICharts * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Deserialize )( 
            ICharts * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SaveToXML )( 
            ICharts * This,
            /* [in] */ BSTR Filename,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *LoadFromXML )( 
            ICharts * This,
            /* [in] */ BSTR Filename,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SavingMode )( 
            ICharts * This,
            /* [retval][out] */ tkSavingMode *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_SavingMode )( 
            ICharts * This,
            /* [in] */ tkSavingMode newVal);
        
        END_INTERFACE
    } IChartsVtbl;

    interface ICharts
    {
        CONST_VTBL struct IChartsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ICharts_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ICharts_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ICharts_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ICharts_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ICharts_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ICharts_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ICharts_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ICharts_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define ICharts_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define ICharts_get_Visible(This,pVal)	\
    ( (This)->lpVtbl -> get_Visible(This,pVal) ) 

#define ICharts_put_Visible(This,newVal)	\
    ( (This)->lpVtbl -> put_Visible(This,newVal) ) 

#define ICharts_get_AvoidCollisions(This,pVal)	\
    ( (This)->lpVtbl -> get_AvoidCollisions(This,pVal) ) 

#define ICharts_put_AvoidCollisions(This,newVal)	\
    ( (This)->lpVtbl -> put_AvoidCollisions(This,newVal) ) 

#define ICharts_get_ChartType(This,pVal)	\
    ( (This)->lpVtbl -> get_ChartType(This,pVal) ) 

#define ICharts_put_ChartType(This,newVal)	\
    ( (This)->lpVtbl -> put_ChartType(This,newVal) ) 

#define ICharts_get_BarWidth(This,pVal)	\
    ( (This)->lpVtbl -> get_BarWidth(This,pVal) ) 

#define ICharts_put_BarWidth(This,newVal)	\
    ( (This)->lpVtbl -> put_BarWidth(This,newVal) ) 

#define ICharts_get_BarHeight(This,pVal)	\
    ( (This)->lpVtbl -> get_BarHeight(This,pVal) ) 

#define ICharts_put_BarHeight(This,newVal)	\
    ( (This)->lpVtbl -> put_BarHeight(This,newVal) ) 

#define ICharts_get_PieRadius(This,pVal)	\
    ( (This)->lpVtbl -> get_PieRadius(This,pVal) ) 

#define ICharts_put_PieRadius(This,newVal)	\
    ( (This)->lpVtbl -> put_PieRadius(This,newVal) ) 

#define ICharts_get_PieRotation(This,pVal)	\
    ( (This)->lpVtbl -> get_PieRotation(This,pVal) ) 

#define ICharts_put_PieRotation(This,newVal)	\
    ( (This)->lpVtbl -> put_PieRotation(This,newVal) ) 

#define ICharts_get_NumFields(This,newVal)	\
    ( (This)->lpVtbl -> get_NumFields(This,newVal) ) 

#define ICharts_AddField2(This,FieldIndex,Color)	\
    ( (This)->lpVtbl -> AddField2(This,FieldIndex,Color) ) 

#define ICharts_InsertField2(This,Index,FieldIndex,Color,retVal)	\
    ( (This)->lpVtbl -> InsertField2(This,Index,FieldIndex,Color,retVal) ) 

#define ICharts_RemoveField(This,Index,vbretval)	\
    ( (This)->lpVtbl -> RemoveField(This,Index,vbretval) ) 

#define ICharts_ClearFields(This)	\
    ( (This)->lpVtbl -> ClearFields(This) ) 

#define ICharts_MoveField(This,OldIndex,NewIndex,vbretval)	\
    ( (This)->lpVtbl -> MoveField(This,OldIndex,NewIndex,vbretval) ) 

#define ICharts_Generate(This,Type,vbretval)	\
    ( (This)->lpVtbl -> Generate(This,Type,vbretval) ) 

#define ICharts_Clear(This)	\
    ( (This)->lpVtbl -> Clear(This) ) 

#define ICharts_DrawChart(This,hdc,x,y,hideLabels,backColor,retVal)	\
    ( (This)->lpVtbl -> DrawChart(This,hdc,x,y,hideLabels,backColor,retVal) ) 

#define ICharts_get_Tilt(This,pVal)	\
    ( (This)->lpVtbl -> get_Tilt(This,pVal) ) 

#define ICharts_put_Tilt(This,newVal)	\
    ( (This)->lpVtbl -> put_Tilt(This,newVal) ) 

#define ICharts_get_Thickness(This,pVal)	\
    ( (This)->lpVtbl -> get_Thickness(This,pVal) ) 

#define ICharts_put_Thickness(This,newVal)	\
    ( (This)->lpVtbl -> put_Thickness(This,newVal) ) 

#define ICharts_get_PieRadius2(This,pVal)	\
    ( (This)->lpVtbl -> get_PieRadius2(This,pVal) ) 

#define ICharts_put_PieRadius2(This,newVal)	\
    ( (This)->lpVtbl -> put_PieRadius2(This,newVal) ) 

#define ICharts_get_SizeField(This,pVal)	\
    ( (This)->lpVtbl -> get_SizeField(This,pVal) ) 

#define ICharts_put_SizeField(This,newVal)	\
    ( (This)->lpVtbl -> put_SizeField(This,newVal) ) 

#define ICharts_get_NormalizationField(This,pVal)	\
    ( (This)->lpVtbl -> get_NormalizationField(This,pVal) ) 

#define ICharts_put_NormalizationField(This,newVal)	\
    ( (This)->lpVtbl -> put_NormalizationField(This,newVal) ) 

#define ICharts_get_UseVariableRadius(This,pVal)	\
    ( (This)->lpVtbl -> get_UseVariableRadius(This,pVal) ) 

#define ICharts_put_UseVariableRadius(This,newVal)	\
    ( (This)->lpVtbl -> put_UseVariableRadius(This,newVal) ) 

#define ICharts_get_Use3DMode(This,pVal)	\
    ( (This)->lpVtbl -> get_Use3DMode(This,pVal) ) 

#define ICharts_put_Use3DMode(This,newVal)	\
    ( (This)->lpVtbl -> put_Use3DMode(This,newVal) ) 

#define ICharts_get_Transparency(This,pVal)	\
    ( (This)->lpVtbl -> get_Transparency(This,pVal) ) 

#define ICharts_put_Transparency(This,newVal)	\
    ( (This)->lpVtbl -> put_Transparency(This,newVal) ) 

#define ICharts_get_LineColor(This,pVal)	\
    ( (This)->lpVtbl -> get_LineColor(This,pVal) ) 

#define ICharts_put_LineColor(This,newVal)	\
    ( (This)->lpVtbl -> put_LineColor(This,newVal) ) 

#define ICharts_get_VerticalPosition(This,pVal)	\
    ( (This)->lpVtbl -> get_VerticalPosition(This,pVal) ) 

#define ICharts_put_VerticalPosition(This,newVal)	\
    ( (This)->lpVtbl -> put_VerticalPosition(This,newVal) ) 

#define ICharts_get_Chart(This,Chart,retVal)	\
    ( (This)->lpVtbl -> get_Chart(This,Chart,retVal) ) 

#define ICharts_get_Field(This,FieldIndex,retVal)	\
    ( (This)->lpVtbl -> get_Field(This,FieldIndex,retVal) ) 

#define ICharts_AddField(This,Field,retVal)	\
    ( (This)->lpVtbl -> AddField(This,Field,retVal) ) 

#define ICharts_InsertField(This,Index,Field,retVal)	\
    ( (This)->lpVtbl -> InsertField(This,Index,Field,retVal) ) 

#define ICharts_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define ICharts_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define ICharts_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define ICharts_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define ICharts_get_Count(This,retVal)	\
    ( (This)->lpVtbl -> get_Count(This,retVal) ) 

#define ICharts_get_MaxVisibleScale(This,retval)	\
    ( (This)->lpVtbl -> get_MaxVisibleScale(This,retval) ) 

#define ICharts_put_MaxVisibleScale(This,newVal)	\
    ( (This)->lpVtbl -> put_MaxVisibleScale(This,newVal) ) 

#define ICharts_get_MinVisibleScale(This,retval)	\
    ( (This)->lpVtbl -> get_MinVisibleScale(This,retval) ) 

#define ICharts_put_MinVisibleScale(This,newVal)	\
    ( (This)->lpVtbl -> put_MinVisibleScale(This,newVal) ) 

#define ICharts_get_DynamicVisibility(This,retval)	\
    ( (This)->lpVtbl -> get_DynamicVisibility(This,retval) ) 

#define ICharts_put_DynamicVisibility(This,newVal)	\
    ( (This)->lpVtbl -> put_DynamicVisibility(This,newVal) ) 

#define ICharts_get_IconWidth(This,retval)	\
    ( (This)->lpVtbl -> get_IconWidth(This,retval) ) 

#define ICharts_get_IconHeight(This,retval)	\
    ( (This)->lpVtbl -> get_IconHeight(This,retval) ) 

#define ICharts_get_Caption(This,retVal)	\
    ( (This)->lpVtbl -> get_Caption(This,retVal) ) 

#define ICharts_put_Caption(This,newVal)	\
    ( (This)->lpVtbl -> put_Caption(This,newVal) ) 

#define ICharts_get_ValuesFontName(This,retval)	\
    ( (This)->lpVtbl -> get_ValuesFontName(This,retval) ) 

#define ICharts_put_ValuesFontName(This,retval)	\
    ( (This)->lpVtbl -> put_ValuesFontName(This,retval) ) 

#define ICharts_get_ValuesFontSize(This,retval)	\
    ( (This)->lpVtbl -> get_ValuesFontSize(This,retval) ) 

#define ICharts_put_ValuesFontSize(This,retval)	\
    ( (This)->lpVtbl -> put_ValuesFontSize(This,retval) ) 

#define ICharts_get_ValuesFontItalic(This,retval)	\
    ( (This)->lpVtbl -> get_ValuesFontItalic(This,retval) ) 

#define ICharts_put_ValuesFontItalic(This,retval)	\
    ( (This)->lpVtbl -> put_ValuesFontItalic(This,retval) ) 

#define ICharts_get_ValuesFontBold(This,retval)	\
    ( (This)->lpVtbl -> get_ValuesFontBold(This,retval) ) 

#define ICharts_put_ValuesFontBold(This,retval)	\
    ( (This)->lpVtbl -> put_ValuesFontBold(This,retval) ) 

#define ICharts_get_ValuesFontColor(This,retval)	\
    ( (This)->lpVtbl -> get_ValuesFontColor(This,retval) ) 

#define ICharts_put_ValuesFontColor(This,retval)	\
    ( (This)->lpVtbl -> put_ValuesFontColor(This,retval) ) 

#define ICharts_get_ValuesFrameVisible(This,retval)	\
    ( (This)->lpVtbl -> get_ValuesFrameVisible(This,retval) ) 

#define ICharts_put_ValuesFrameVisible(This,retval)	\
    ( (This)->lpVtbl -> put_ValuesFrameVisible(This,retval) ) 

#define ICharts_get_ValuesFrameColor(This,retval)	\
    ( (This)->lpVtbl -> get_ValuesFrameColor(This,retval) ) 

#define ICharts_put_ValuesFrameColor(This,retval)	\
    ( (This)->lpVtbl -> put_ValuesFrameColor(This,retval) ) 

#define ICharts_get_ValuesVisible(This,retval)	\
    ( (This)->lpVtbl -> get_ValuesVisible(This,retval) ) 

#define ICharts_put_ValuesVisible(This,retval)	\
    ( (This)->lpVtbl -> put_ValuesVisible(This,retval) ) 

#define ICharts_get_ValuesStyle(This,retval)	\
    ( (This)->lpVtbl -> get_ValuesStyle(This,retval) ) 

#define ICharts_put_ValuesStyle(This,retval)	\
    ( (This)->lpVtbl -> put_ValuesStyle(This,retval) ) 

#define ICharts_Select(This,BoundingBox,Tolerance,SelectMode,Indices,retval)	\
    ( (This)->lpVtbl -> Select(This,BoundingBox,Tolerance,SelectMode,Indices,retval) ) 

#define ICharts_get_VisibilityExpression(This,retVal)	\
    ( (This)->lpVtbl -> get_VisibilityExpression(This,retVal) ) 

#define ICharts_put_VisibilityExpression(This,newVal)	\
    ( (This)->lpVtbl -> put_VisibilityExpression(This,newVal) ) 

#define ICharts_get_CollisionBuffer(This,retval)	\
    ( (This)->lpVtbl -> get_CollisionBuffer(This,retval) ) 

#define ICharts_put_CollisionBuffer(This,newVal)	\
    ( (This)->lpVtbl -> put_CollisionBuffer(This,newVal) ) 

#define ICharts_get_OffsetX(This,retval)	\
    ( (This)->lpVtbl -> get_OffsetX(This,retval) ) 

#define ICharts_put_OffsetX(This,newVal)	\
    ( (This)->lpVtbl -> put_OffsetX(This,newVal) ) 

#define ICharts_get_OffsetY(This,retval)	\
    ( (This)->lpVtbl -> get_OffsetY(This,retval) ) 

#define ICharts_put_OffsetY(This,newVal)	\
    ( (This)->lpVtbl -> put_OffsetY(This,newVal) ) 

#define ICharts_Serialize(This,retVal)	\
    ( (This)->lpVtbl -> Serialize(This,retVal) ) 

#define ICharts_Deserialize(This,newVal)	\
    ( (This)->lpVtbl -> Deserialize(This,newVal) ) 

#define ICharts_SaveToXML(This,Filename,retVal)	\
    ( (This)->lpVtbl -> SaveToXML(This,Filename,retVal) ) 

#define ICharts_LoadFromXML(This,Filename,retVal)	\
    ( (This)->lpVtbl -> LoadFromXML(This,Filename,retVal) ) 

#define ICharts_get_SavingMode(This,retVal)	\
    ( (This)->lpVtbl -> get_SavingMode(This,retVal) ) 

#define ICharts_put_SavingMode(This,newVal)	\
    ( (This)->lpVtbl -> put_SavingMode(This,newVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ICharts_INTERFACE_DEFINED__ */


#ifndef __IChart_INTERFACE_DEFINED__
#define __IChart_INTERFACE_DEFINED__

/* interface IChart */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IChart;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("34613D99-DDAB-48CA-AB5D-CAD805E7986C")
    IChart : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PositionX( 
            /* [retval][out] */ double *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PositionX( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_PositionY( 
            /* [retval][out] */ double *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_PositionY( 
            /* [in] */ double newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Visible( 
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Visible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IsDrawn( 
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_IsDrawn( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ScreenExtents( 
            /* [retval][out] */ IExtents **retVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IChartVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IChart * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IChart * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IChart * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IChart * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IChart * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IChart * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IChart * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PositionX )( 
            IChart * This,
            /* [retval][out] */ double *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PositionX )( 
            IChart * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PositionY )( 
            IChart * This,
            /* [retval][out] */ double *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_PositionY )( 
            IChart * This,
            /* [in] */ double newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Visible )( 
            IChart * This,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Visible )( 
            IChart * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsDrawn )( 
            IChart * This,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_IsDrawn )( 
            IChart * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ScreenExtents )( 
            IChart * This,
            /* [retval][out] */ IExtents **retVal);
        
        END_INTERFACE
    } IChartVtbl;

    interface IChart
    {
        CONST_VTBL struct IChartVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IChart_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IChart_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IChart_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IChart_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IChart_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IChart_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IChart_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IChart_get_PositionX(This,retVal)	\
    ( (This)->lpVtbl -> get_PositionX(This,retVal) ) 

#define IChart_put_PositionX(This,newVal)	\
    ( (This)->lpVtbl -> put_PositionX(This,newVal) ) 

#define IChart_get_PositionY(This,retVal)	\
    ( (This)->lpVtbl -> get_PositionY(This,retVal) ) 

#define IChart_put_PositionY(This,newVal)	\
    ( (This)->lpVtbl -> put_PositionY(This,newVal) ) 

#define IChart_get_Visible(This,retVal)	\
    ( (This)->lpVtbl -> get_Visible(This,retVal) ) 

#define IChart_put_Visible(This,newVal)	\
    ( (This)->lpVtbl -> put_Visible(This,newVal) ) 

#define IChart_get_IsDrawn(This,retVal)	\
    ( (This)->lpVtbl -> get_IsDrawn(This,retVal) ) 

#define IChart_put_IsDrawn(This,newVal)	\
    ( (This)->lpVtbl -> put_IsDrawn(This,newVal) ) 

#define IChart_get_ScreenExtents(This,retVal)	\
    ( (This)->lpVtbl -> get_ScreenExtents(This,retVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IChart_INTERFACE_DEFINED__ */


#ifndef __IColorScheme_INTERFACE_DEFINED__
#define __IColorScheme_INTERFACE_DEFINED__

/* interface IColorScheme */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IColorScheme;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("D2334B3C-0779-4F5F-8771-2F857F0D601E")
    IColorScheme : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetColors( 
            /* [in] */ OLE_COLOR Color1,
            /* [in] */ OLE_COLOR Color2) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetColors2( 
            /* [in] */ tkMapColor Color1,
            /* [in] */ tkMapColor Color2) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetColors3( 
            /* [in] */ SHORT MinRed,
            /* [in] */ SHORT MinGreen,
            /* [in] */ SHORT MinBlue,
            /* [in] */ SHORT MaxRed,
            /* [in] */ SHORT MaxGreen,
            /* [in] */ SHORT MaxBlue) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetColors4( 
            /* [in] */ PredefinedColorScheme Scheme) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AddBreak( 
            /* [in] */ double Value,
            /* [in] */ OLE_COLOR Color) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Remove( 
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clear( void) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NumBreaks( 
            /* [retval][out] */ long *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_RandomColor( 
            /* [in] */ double Value,
            /* [retval][out] */ OLE_COLOR *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GraduatedColor( 
            /* [in] */ double Value,
            /* [retval][out] */ OLE_COLOR *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_BreakColor( 
            /* [in] */ long Index,
            /* [retval][out] */ OLE_COLOR *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_BreakColor( 
            /* [in] */ long Index,
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_BreakValue( 
            /* [in] */ long Index,
            /* [retval][out] */ double *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IColorSchemeVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IColorScheme * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IColorScheme * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IColorScheme * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IColorScheme * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IColorScheme * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IColorScheme * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IColorScheme * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetColors )( 
            IColorScheme * This,
            /* [in] */ OLE_COLOR Color1,
            /* [in] */ OLE_COLOR Color2);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetColors2 )( 
            IColorScheme * This,
            /* [in] */ tkMapColor Color1,
            /* [in] */ tkMapColor Color2);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetColors3 )( 
            IColorScheme * This,
            /* [in] */ SHORT MinRed,
            /* [in] */ SHORT MinGreen,
            /* [in] */ SHORT MinBlue,
            /* [in] */ SHORT MaxRed,
            /* [in] */ SHORT MaxGreen,
            /* [in] */ SHORT MaxBlue);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetColors4 )( 
            IColorScheme * This,
            /* [in] */ PredefinedColorScheme Scheme);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AddBreak )( 
            IColorScheme * This,
            /* [in] */ double Value,
            /* [in] */ OLE_COLOR Color);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Remove )( 
            IColorScheme * This,
            /* [in] */ long Index,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clear )( 
            IColorScheme * This);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NumBreaks )( 
            IColorScheme * This,
            /* [retval][out] */ long *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_RandomColor )( 
            IColorScheme * This,
            /* [in] */ double Value,
            /* [retval][out] */ OLE_COLOR *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GraduatedColor )( 
            IColorScheme * This,
            /* [in] */ double Value,
            /* [retval][out] */ OLE_COLOR *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_BreakColor )( 
            IColorScheme * This,
            /* [in] */ long Index,
            /* [retval][out] */ OLE_COLOR *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_BreakColor )( 
            IColorScheme * This,
            /* [in] */ long Index,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_BreakValue )( 
            IColorScheme * This,
            /* [in] */ long Index,
            /* [retval][out] */ double *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IColorScheme * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IColorScheme * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IColorScheme * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IColorScheme * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IColorScheme * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IColorScheme * This,
            /* [in] */ BSTR newVal);
        
        END_INTERFACE
    } IColorSchemeVtbl;

    interface IColorScheme
    {
        CONST_VTBL struct IColorSchemeVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IColorScheme_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IColorScheme_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IColorScheme_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IColorScheme_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IColorScheme_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IColorScheme_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IColorScheme_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IColorScheme_SetColors(This,Color1,Color2)	\
    ( (This)->lpVtbl -> SetColors(This,Color1,Color2) ) 

#define IColorScheme_SetColors2(This,Color1,Color2)	\
    ( (This)->lpVtbl -> SetColors2(This,Color1,Color2) ) 

#define IColorScheme_SetColors3(This,MinRed,MinGreen,MinBlue,MaxRed,MaxGreen,MaxBlue)	\
    ( (This)->lpVtbl -> SetColors3(This,MinRed,MinGreen,MinBlue,MaxRed,MaxGreen,MaxBlue) ) 

#define IColorScheme_SetColors4(This,Scheme)	\
    ( (This)->lpVtbl -> SetColors4(This,Scheme) ) 

#define IColorScheme_AddBreak(This,Value,Color)	\
    ( (This)->lpVtbl -> AddBreak(This,Value,Color) ) 

#define IColorScheme_Remove(This,Index,retVal)	\
    ( (This)->lpVtbl -> Remove(This,Index,retVal) ) 

#define IColorScheme_Clear(This)	\
    ( (This)->lpVtbl -> Clear(This) ) 

#define IColorScheme_get_NumBreaks(This,retVal)	\
    ( (This)->lpVtbl -> get_NumBreaks(This,retVal) ) 

#define IColorScheme_get_RandomColor(This,Value,retVal)	\
    ( (This)->lpVtbl -> get_RandomColor(This,Value,retVal) ) 

#define IColorScheme_get_GraduatedColor(This,Value,retVal)	\
    ( (This)->lpVtbl -> get_GraduatedColor(This,Value,retVal) ) 

#define IColorScheme_get_BreakColor(This,Index,retVal)	\
    ( (This)->lpVtbl -> get_BreakColor(This,Index,retVal) ) 

#define IColorScheme_put_BreakColor(This,Index,newVal)	\
    ( (This)->lpVtbl -> put_BreakColor(This,Index,newVal) ) 

#define IColorScheme_get_BreakValue(This,Index,retVal)	\
    ( (This)->lpVtbl -> get_BreakValue(This,Index,retVal) ) 

#define IColorScheme_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IColorScheme_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IColorScheme_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IColorScheme_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IColorScheme_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IColorScheme_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IColorScheme_INTERFACE_DEFINED__ */


#ifndef __IChartField_INTERFACE_DEFINED__
#define __IChartField_INTERFACE_DEFINED__

/* interface IChartField */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IChartField;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("A9C1AFEB-8CC6-4A36-8E41-E643C1302E6F")
    IChartField : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Index( 
            /* [retval][out] */ LONG *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Index( 
            /* [in] */ LONG newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Color( 
            /* [retval][out] */ OLE_COLOR *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Color( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Name( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Name( 
            /* [in] */ BSTR newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IChartFieldVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IChartField * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IChartField * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IChartField * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IChartField * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IChartField * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IChartField * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IChartField * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Index )( 
            IChartField * This,
            /* [retval][out] */ LONG *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Index )( 
            IChartField * This,
            /* [in] */ LONG newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Color )( 
            IChartField * This,
            /* [retval][out] */ OLE_COLOR *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Color )( 
            IChartField * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Name )( 
            IChartField * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Name )( 
            IChartField * This,
            /* [in] */ BSTR newVal);
        
        END_INTERFACE
    } IChartFieldVtbl;

    interface IChartField
    {
        CONST_VTBL struct IChartFieldVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IChartField_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IChartField_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IChartField_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IChartField_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IChartField_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IChartField_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IChartField_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IChartField_get_Index(This,retVal)	\
    ( (This)->lpVtbl -> get_Index(This,retVal) ) 

#define IChartField_put_Index(This,newVal)	\
    ( (This)->lpVtbl -> put_Index(This,newVal) ) 

#define IChartField_get_Color(This,retVal)	\
    ( (This)->lpVtbl -> get_Color(This,retVal) ) 

#define IChartField_put_Color(This,newVal)	\
    ( (This)->lpVtbl -> put_Color(This,newVal) ) 

#define IChartField_get_Name(This,retVal)	\
    ( (This)->lpVtbl -> get_Name(This,retVal) ) 

#define IChartField_put_Name(This,newVal)	\
    ( (This)->lpVtbl -> put_Name(This,newVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IChartField_INTERFACE_DEFINED__ */


#ifndef __ILinePattern_INTERFACE_DEFINED__
#define __ILinePattern_INTERFACE_DEFINED__

/* interface ILinePattern */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ILinePattern;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("54EB7DD1-CEC2-4165-8DBA-13115B079DF1")
    ILinePattern : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Line( 
            /* [in] */ int Index,
            /* [retval][out] */ ILineSegment **retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Line( 
            /* [in] */ int Index,
            /* [in] */ ILineSegment *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Count( 
            /* [retval][out] */ int *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AddLine( 
            /* [in] */ OLE_COLOR color,
            /* [in] */ float width,
            /* [in] */ tkDashStyle style) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE InsertLine( 
            int Index,
            /* [in] */ OLE_COLOR color,
            /* [in] */ float width,
            /* [in] */ tkDashStyle style,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AddMarker( 
            /* [in] */ tkDefaultPointSymbol marker,
            /* [retval][out] */ ILineSegment **retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE InsertMarker( 
            /* [in] */ int Index,
            /* [in] */ tkDefaultPointSymbol marker,
            /* [retval][out] */ ILineSegment **retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE RemoveItem( 
            /* [in] */ int Index,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clear( void) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Draw( 
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Transparency( 
            /* [retval][out] */ BYTE *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Transparency( 
            /* [in] */ BYTE newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Serialize( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Deserialize( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DrawVB( 
            /* [in] */ int hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ILinePatternVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ILinePattern * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ILinePattern * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ILinePattern * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ILinePattern * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ILinePattern * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ILinePattern * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ILinePattern * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            ILinePattern * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            ILinePattern * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            ILinePattern * This,
            /* [retval][out] */ ICallback **retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            ILinePattern * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            ILinePattern * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            ILinePattern * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Line )( 
            ILinePattern * This,
            /* [in] */ int Index,
            /* [retval][out] */ ILineSegment **retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Line )( 
            ILinePattern * This,
            /* [in] */ int Index,
            /* [in] */ ILineSegment *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Count )( 
            ILinePattern * This,
            /* [retval][out] */ int *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AddLine )( 
            ILinePattern * This,
            /* [in] */ OLE_COLOR color,
            /* [in] */ float width,
            /* [in] */ tkDashStyle style);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *InsertLine )( 
            ILinePattern * This,
            int Index,
            /* [in] */ OLE_COLOR color,
            /* [in] */ float width,
            /* [in] */ tkDashStyle style,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AddMarker )( 
            ILinePattern * This,
            /* [in] */ tkDefaultPointSymbol marker,
            /* [retval][out] */ ILineSegment **retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *InsertMarker )( 
            ILinePattern * This,
            /* [in] */ int Index,
            /* [in] */ tkDefaultPointSymbol marker,
            /* [retval][out] */ ILineSegment **retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *RemoveItem )( 
            ILinePattern * This,
            /* [in] */ int Index,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clear )( 
            ILinePattern * This);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Draw )( 
            ILinePattern * This,
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Transparency )( 
            ILinePattern * This,
            /* [retval][out] */ BYTE *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Transparency )( 
            ILinePattern * This,
            /* [in] */ BYTE newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Serialize )( 
            ILinePattern * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Deserialize )( 
            ILinePattern * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DrawVB )( 
            ILinePattern * This,
            /* [in] */ int hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        END_INTERFACE
    } ILinePatternVtbl;

    interface ILinePattern
    {
        CONST_VTBL struct ILinePatternVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ILinePattern_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ILinePattern_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ILinePattern_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ILinePattern_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ILinePattern_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ILinePattern_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ILinePattern_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ILinePattern_get_Key(This,retVal)	\
    ( (This)->lpVtbl -> get_Key(This,retVal) ) 

#define ILinePattern_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define ILinePattern_get_GlobalCallback(This,retVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,retVal) ) 

#define ILinePattern_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define ILinePattern_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define ILinePattern_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define ILinePattern_get_Line(This,Index,retVal)	\
    ( (This)->lpVtbl -> get_Line(This,Index,retVal) ) 

#define ILinePattern_put_Line(This,Index,newVal)	\
    ( (This)->lpVtbl -> put_Line(This,Index,newVal) ) 

#define ILinePattern_get_Count(This,retVal)	\
    ( (This)->lpVtbl -> get_Count(This,retVal) ) 

#define ILinePattern_AddLine(This,color,width,style)	\
    ( (This)->lpVtbl -> AddLine(This,color,width,style) ) 

#define ILinePattern_InsertLine(This,Index,color,width,style,retVal)	\
    ( (This)->lpVtbl -> InsertLine(This,Index,color,width,style,retVal) ) 

#define ILinePattern_AddMarker(This,marker,retVal)	\
    ( (This)->lpVtbl -> AddMarker(This,marker,retVal) ) 

#define ILinePattern_InsertMarker(This,Index,marker,retVal)	\
    ( (This)->lpVtbl -> InsertMarker(This,Index,marker,retVal) ) 

#define ILinePattern_RemoveItem(This,Index,retVal)	\
    ( (This)->lpVtbl -> RemoveItem(This,Index,retVal) ) 

#define ILinePattern_Clear(This)	\
    ( (This)->lpVtbl -> Clear(This) ) 

#define ILinePattern_Draw(This,hdc,x,y,clipWidth,clipHeight,backColor,retVal)	\
    ( (This)->lpVtbl -> Draw(This,hdc,x,y,clipWidth,clipHeight,backColor,retVal) ) 

#define ILinePattern_get_Transparency(This,retVal)	\
    ( (This)->lpVtbl -> get_Transparency(This,retVal) ) 

#define ILinePattern_put_Transparency(This,newVal)	\
    ( (This)->lpVtbl -> put_Transparency(This,newVal) ) 

#define ILinePattern_Serialize(This,retVal)	\
    ( (This)->lpVtbl -> Serialize(This,retVal) ) 

#define ILinePattern_Deserialize(This,newVal)	\
    ( (This)->lpVtbl -> Deserialize(This,newVal) ) 

#define ILinePattern_DrawVB(This,hdc,x,y,clipWidth,clipHeight,backColor,retVal)	\
    ( (This)->lpVtbl -> DrawVB(This,hdc,x,y,clipWidth,clipHeight,backColor,retVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ILinePattern_INTERFACE_DEFINED__ */


#ifndef __ILineSegment_INTERFACE_DEFINED__
#define __ILineSegment_INTERFACE_DEFINED__

/* interface ILineSegment */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ILineSegment;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("56A5439F-F550-434E-B6C5-0508A6461F47")
    ILineSegment : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Color( 
            /* [retval][out] */ OLE_COLOR *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Color( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LineWidth( 
            /* [retval][out] */ float *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LineWidth( 
            /* [in] */ float newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LineStyle( 
            /* [retval][out] */ tkDashStyle *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LineStyle( 
            /* [in] */ tkDashStyle newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LineType( 
            /* [retval][out] */ tkLineType *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LineType( 
            /* [in] */ tkLineType newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Marker( 
            /* [retval][out] */ tkDefaultPointSymbol *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Marker( 
            /* [in] */ tkDefaultPointSymbol newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MarkerSize( 
            /* [retval][out] */ float *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MarkerSize( 
            /* [in] */ float newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MarkerInterval( 
            /* [retval][out] */ float *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MarkerInterval( 
            /* [in] */ float newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MarkerOrientation( 
            /* [retval][out] */ tkLineLabelOrientation *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MarkerOrientation( 
            /* [in] */ tkLineLabelOrientation newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MarkerFlipFirst( 
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MarkerFlipFirst( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MarkerOffset( 
            /* [retval][out] */ float *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MarkerOffset( 
            /* [in] */ float newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Draw( 
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MarkerOutlineColor( 
            /* [retval][out] */ OLE_COLOR *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MarkerOutlineColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE DrawVB( 
            /* [in] */ int hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ILineSegmentVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ILineSegment * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ILineSegment * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ILineSegment * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ILineSegment * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ILineSegment * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ILineSegment * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ILineSegment * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Color )( 
            ILineSegment * This,
            /* [retval][out] */ OLE_COLOR *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Color )( 
            ILineSegment * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LineWidth )( 
            ILineSegment * This,
            /* [retval][out] */ float *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LineWidth )( 
            ILineSegment * This,
            /* [in] */ float newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LineStyle )( 
            ILineSegment * This,
            /* [retval][out] */ tkDashStyle *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LineStyle )( 
            ILineSegment * This,
            /* [in] */ tkDashStyle newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LineType )( 
            ILineSegment * This,
            /* [retval][out] */ tkLineType *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LineType )( 
            ILineSegment * This,
            /* [in] */ tkLineType newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Marker )( 
            ILineSegment * This,
            /* [retval][out] */ tkDefaultPointSymbol *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Marker )( 
            ILineSegment * This,
            /* [in] */ tkDefaultPointSymbol newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MarkerSize )( 
            ILineSegment * This,
            /* [retval][out] */ float *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MarkerSize )( 
            ILineSegment * This,
            /* [in] */ float newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MarkerInterval )( 
            ILineSegment * This,
            /* [retval][out] */ float *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MarkerInterval )( 
            ILineSegment * This,
            /* [in] */ float newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MarkerOrientation )( 
            ILineSegment * This,
            /* [retval][out] */ tkLineLabelOrientation *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MarkerOrientation )( 
            ILineSegment * This,
            /* [in] */ tkLineLabelOrientation newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MarkerFlipFirst )( 
            ILineSegment * This,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MarkerFlipFirst )( 
            ILineSegment * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MarkerOffset )( 
            ILineSegment * This,
            /* [retval][out] */ float *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MarkerOffset )( 
            ILineSegment * This,
            /* [in] */ float newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Draw )( 
            ILineSegment * This,
            /* [in] */ int **hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MarkerOutlineColor )( 
            ILineSegment * This,
            /* [retval][out] */ OLE_COLOR *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MarkerOutlineColor )( 
            ILineSegment * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *DrawVB )( 
            ILineSegment * This,
            /* [in] */ int hdc,
            /* [in] */ float x,
            /* [in] */ float y,
            /* [in] */ int clipWidth,
            /* [in] */ int clipHeight,
            /* [defaultvalue][optional][in] */ OLE_COLOR backColor,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        END_INTERFACE
    } ILineSegmentVtbl;

    interface ILineSegment
    {
        CONST_VTBL struct ILineSegmentVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ILineSegment_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ILineSegment_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ILineSegment_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ILineSegment_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ILineSegment_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ILineSegment_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ILineSegment_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ILineSegment_get_Color(This,retVal)	\
    ( (This)->lpVtbl -> get_Color(This,retVal) ) 

#define ILineSegment_put_Color(This,newVal)	\
    ( (This)->lpVtbl -> put_Color(This,newVal) ) 

#define ILineSegment_get_LineWidth(This,retVal)	\
    ( (This)->lpVtbl -> get_LineWidth(This,retVal) ) 

#define ILineSegment_put_LineWidth(This,newVal)	\
    ( (This)->lpVtbl -> put_LineWidth(This,newVal) ) 

#define ILineSegment_get_LineStyle(This,retVal)	\
    ( (This)->lpVtbl -> get_LineStyle(This,retVal) ) 

#define ILineSegment_put_LineStyle(This,newVal)	\
    ( (This)->lpVtbl -> put_LineStyle(This,newVal) ) 

#define ILineSegment_get_LineType(This,retVal)	\
    ( (This)->lpVtbl -> get_LineType(This,retVal) ) 

#define ILineSegment_put_LineType(This,newVal)	\
    ( (This)->lpVtbl -> put_LineType(This,newVal) ) 

#define ILineSegment_get_Marker(This,retVal)	\
    ( (This)->lpVtbl -> get_Marker(This,retVal) ) 

#define ILineSegment_put_Marker(This,newVal)	\
    ( (This)->lpVtbl -> put_Marker(This,newVal) ) 

#define ILineSegment_get_MarkerSize(This,retVal)	\
    ( (This)->lpVtbl -> get_MarkerSize(This,retVal) ) 

#define ILineSegment_put_MarkerSize(This,newVal)	\
    ( (This)->lpVtbl -> put_MarkerSize(This,newVal) ) 

#define ILineSegment_get_MarkerInterval(This,retVal)	\
    ( (This)->lpVtbl -> get_MarkerInterval(This,retVal) ) 

#define ILineSegment_put_MarkerInterval(This,newVal)	\
    ( (This)->lpVtbl -> put_MarkerInterval(This,newVal) ) 

#define ILineSegment_get_MarkerOrientation(This,retVal)	\
    ( (This)->lpVtbl -> get_MarkerOrientation(This,retVal) ) 

#define ILineSegment_put_MarkerOrientation(This,newVal)	\
    ( (This)->lpVtbl -> put_MarkerOrientation(This,newVal) ) 

#define ILineSegment_get_MarkerFlipFirst(This,retVal)	\
    ( (This)->lpVtbl -> get_MarkerFlipFirst(This,retVal) ) 

#define ILineSegment_put_MarkerFlipFirst(This,newVal)	\
    ( (This)->lpVtbl -> put_MarkerFlipFirst(This,newVal) ) 

#define ILineSegment_get_MarkerOffset(This,retVal)	\
    ( (This)->lpVtbl -> get_MarkerOffset(This,retVal) ) 

#define ILineSegment_put_MarkerOffset(This,newVal)	\
    ( (This)->lpVtbl -> put_MarkerOffset(This,newVal) ) 

#define ILineSegment_Draw(This,hdc,x,y,clipWidth,clipHeight,backColor,retVal)	\
    ( (This)->lpVtbl -> Draw(This,hdc,x,y,clipWidth,clipHeight,backColor,retVal) ) 

#define ILineSegment_get_MarkerOutlineColor(This,retVal)	\
    ( (This)->lpVtbl -> get_MarkerOutlineColor(This,retVal) ) 

#define ILineSegment_put_MarkerOutlineColor(This,newVal)	\
    ( (This)->lpVtbl -> put_MarkerOutlineColor(This,newVal) ) 

#define ILineSegment_DrawVB(This,hdc,x,y,clipWidth,clipHeight,backColor,retVal)	\
    ( (This)->lpVtbl -> DrawVB(This,hdc,x,y,clipWidth,clipHeight,backColor,retVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ILineSegment_INTERFACE_DEFINED__ */


#ifndef __IGeoProjection_INTERFACE_DEFINED__
#define __IGeoProjection_INTERFACE_DEFINED__

/* interface IGeoProjection */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IGeoProjection;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("AED5318E-9E3D-4276-BE03-71EDFEDC0F1F")
    IGeoProjection : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ExportToProj4( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ImportFromProj4( 
            /* [in] */ BSTR proj,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ImportFromESRI( 
            /* [in] */ BSTR proj,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ImportFromEPSG( 
            /* [in] */ LONG projCode,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ExportToWKT( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ImportFromWKT( 
            /* [in] */ BSTR proj,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetWellKnownGeogCS( 
            /* [in] */ tkCoordinateSystem newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IsGeographic( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IsProjected( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IsLocal( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IsSame( 
            /* [in] */ IGeoProjection *proj,
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IsSameGeogCS( 
            /* [in] */ IGeoProjection *proj,
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_InverseFlattening( 
            /* [retval][out] */ DOUBLE *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SemiMajor( 
            /* [retval][out] */ DOUBLE *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SemiMinor( 
            /* [retval][out] */ DOUBLE *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ProjectionParam( 
            /* [in] */ tkProjectionParameter name,
            /* [in] */ double *value,
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IsEmpty( 
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CopyFrom( 
            /* [in] */ IGeoProjection *sourceProj,
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Name( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ProjectionName( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GeogCSName( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GeogCSParam( 
            /* [in] */ tkGeogCSParameter name,
            /* [in] */ DOUBLE *pVal,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetGeographicCS( 
            /* [in] */ tkCoordinateSystem coordinateSystem) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetWgs84Projection( 
            /* [in] */ tkWgs84Projection projection) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE SetNad83Projection( 
            /* [in] */ tkNad83Projection projection) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_IsSameExt( 
            /* [in] */ IGeoProjection *proj,
            /* [in] */ IExtents *bounds,
            /* [defaultvalue][optional][in] */ int numSamplingPoints,
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ReadFromFile( 
            /* [in] */ BSTR filename,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE WriteToFile( 
            /* [in] */ BSTR filename,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE ImportFromAutoDetect( 
            /* [in] */ BSTR proj,
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IGeoProjectionVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IGeoProjection * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IGeoProjection * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IGeoProjection * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IGeoProjection * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IGeoProjection * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IGeoProjection * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IGeoProjection * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ExportToProj4 )( 
            IGeoProjection * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ImportFromProj4 )( 
            IGeoProjection * This,
            /* [in] */ BSTR proj,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ImportFromESRI )( 
            IGeoProjection * This,
            /* [in] */ BSTR proj,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ImportFromEPSG )( 
            IGeoProjection * This,
            /* [in] */ LONG projCode,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ExportToWKT )( 
            IGeoProjection * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ImportFromWKT )( 
            IGeoProjection * This,
            /* [in] */ BSTR proj,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IGeoProjection * This,
            /* [retval][out] */ ICallback **retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IGeoProjection * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IGeoProjection * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IGeoProjection * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IGeoProjection * This,
            /* [retval][out] */ BSTR *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IGeoProjection * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetWellKnownGeogCS )( 
            IGeoProjection * This,
            /* [in] */ tkCoordinateSystem newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsGeographic )( 
            IGeoProjection * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsProjected )( 
            IGeoProjection * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsLocal )( 
            IGeoProjection * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsSame )( 
            IGeoProjection * This,
            /* [in] */ IGeoProjection *proj,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsSameGeogCS )( 
            IGeoProjection * This,
            /* [in] */ IGeoProjection *proj,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_InverseFlattening )( 
            IGeoProjection * This,
            /* [retval][out] */ DOUBLE *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SemiMajor )( 
            IGeoProjection * This,
            /* [retval][out] */ DOUBLE *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SemiMinor )( 
            IGeoProjection * This,
            /* [retval][out] */ DOUBLE *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ProjectionParam )( 
            IGeoProjection * This,
            /* [in] */ tkProjectionParameter name,
            /* [in] */ double *value,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsEmpty )( 
            IGeoProjection * This,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CopyFrom )( 
            IGeoProjection * This,
            /* [in] */ IGeoProjection *sourceProj,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Name )( 
            IGeoProjection * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ProjectionName )( 
            IGeoProjection * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GeogCSName )( 
            IGeoProjection * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GeogCSParam )( 
            IGeoProjection * This,
            /* [in] */ tkGeogCSParameter name,
            /* [in] */ DOUBLE *pVal,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetGeographicCS )( 
            IGeoProjection * This,
            /* [in] */ tkCoordinateSystem coordinateSystem);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetWgs84Projection )( 
            IGeoProjection * This,
            /* [in] */ tkWgs84Projection projection);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *SetNad83Projection )( 
            IGeoProjection * This,
            /* [in] */ tkNad83Projection projection);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsSameExt )( 
            IGeoProjection * This,
            /* [in] */ IGeoProjection *proj,
            /* [in] */ IExtents *bounds,
            /* [defaultvalue][optional][in] */ int numSamplingPoints,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ReadFromFile )( 
            IGeoProjection * This,
            /* [in] */ BSTR filename,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *WriteToFile )( 
            IGeoProjection * This,
            /* [in] */ BSTR filename,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *ImportFromAutoDetect )( 
            IGeoProjection * This,
            /* [in] */ BSTR proj,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        END_INTERFACE
    } IGeoProjectionVtbl;

    interface IGeoProjection
    {
        CONST_VTBL struct IGeoProjectionVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IGeoProjection_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IGeoProjection_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IGeoProjection_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IGeoProjection_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IGeoProjection_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IGeoProjection_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IGeoProjection_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IGeoProjection_ExportToProj4(This,retVal)	\
    ( (This)->lpVtbl -> ExportToProj4(This,retVal) ) 

#define IGeoProjection_ImportFromProj4(This,proj,retVal)	\
    ( (This)->lpVtbl -> ImportFromProj4(This,proj,retVal) ) 

#define IGeoProjection_ImportFromESRI(This,proj,retVal)	\
    ( (This)->lpVtbl -> ImportFromESRI(This,proj,retVal) ) 

#define IGeoProjection_ImportFromEPSG(This,projCode,retVal)	\
    ( (This)->lpVtbl -> ImportFromEPSG(This,projCode,retVal) ) 

#define IGeoProjection_ExportToWKT(This,retVal)	\
    ( (This)->lpVtbl -> ExportToWKT(This,retVal) ) 

#define IGeoProjection_ImportFromWKT(This,proj,retVal)	\
    ( (This)->lpVtbl -> ImportFromWKT(This,proj,retVal) ) 

#define IGeoProjection_get_GlobalCallback(This,retVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,retVal) ) 

#define IGeoProjection_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IGeoProjection_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IGeoProjection_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IGeoProjection_get_Key(This,retVal)	\
    ( (This)->lpVtbl -> get_Key(This,retVal) ) 

#define IGeoProjection_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IGeoProjection_SetWellKnownGeogCS(This,newVal)	\
    ( (This)->lpVtbl -> SetWellKnownGeogCS(This,newVal) ) 

#define IGeoProjection_get_IsGeographic(This,pVal)	\
    ( (This)->lpVtbl -> get_IsGeographic(This,pVal) ) 

#define IGeoProjection_get_IsProjected(This,pVal)	\
    ( (This)->lpVtbl -> get_IsProjected(This,pVal) ) 

#define IGeoProjection_get_IsLocal(This,pVal)	\
    ( (This)->lpVtbl -> get_IsLocal(This,pVal) ) 

#define IGeoProjection_get_IsSame(This,proj,pVal)	\
    ( (This)->lpVtbl -> get_IsSame(This,proj,pVal) ) 

#define IGeoProjection_get_IsSameGeogCS(This,proj,pVal)	\
    ( (This)->lpVtbl -> get_IsSameGeogCS(This,proj,pVal) ) 

#define IGeoProjection_get_InverseFlattening(This,pVal)	\
    ( (This)->lpVtbl -> get_InverseFlattening(This,pVal) ) 

#define IGeoProjection_get_SemiMajor(This,pVal)	\
    ( (This)->lpVtbl -> get_SemiMajor(This,pVal) ) 

#define IGeoProjection_get_SemiMinor(This,pVal)	\
    ( (This)->lpVtbl -> get_SemiMinor(This,pVal) ) 

#define IGeoProjection_get_ProjectionParam(This,name,value,pVal)	\
    ( (This)->lpVtbl -> get_ProjectionParam(This,name,value,pVal) ) 

#define IGeoProjection_get_IsEmpty(This,retVal)	\
    ( (This)->lpVtbl -> get_IsEmpty(This,retVal) ) 

#define IGeoProjection_CopyFrom(This,sourceProj,pVal)	\
    ( (This)->lpVtbl -> CopyFrom(This,sourceProj,pVal) ) 

#define IGeoProjection_get_Name(This,pVal)	\
    ( (This)->lpVtbl -> get_Name(This,pVal) ) 

#define IGeoProjection_get_ProjectionName(This,pVal)	\
    ( (This)->lpVtbl -> get_ProjectionName(This,pVal) ) 

#define IGeoProjection_get_GeogCSName(This,pVal)	\
    ( (This)->lpVtbl -> get_GeogCSName(This,pVal) ) 

#define IGeoProjection_get_GeogCSParam(This,name,pVal,retVal)	\
    ( (This)->lpVtbl -> get_GeogCSParam(This,name,pVal,retVal) ) 

#define IGeoProjection_SetGeographicCS(This,coordinateSystem)	\
    ( (This)->lpVtbl -> SetGeographicCS(This,coordinateSystem) ) 

#define IGeoProjection_SetWgs84Projection(This,projection)	\
    ( (This)->lpVtbl -> SetWgs84Projection(This,projection) ) 

#define IGeoProjection_SetNad83Projection(This,projection)	\
    ( (This)->lpVtbl -> SetNad83Projection(This,projection) ) 

#define IGeoProjection_get_IsSameExt(This,proj,bounds,numSamplingPoints,pVal)	\
    ( (This)->lpVtbl -> get_IsSameExt(This,proj,bounds,numSamplingPoints,pVal) ) 

#define IGeoProjection_ReadFromFile(This,filename,retVal)	\
    ( (This)->lpVtbl -> ReadFromFile(This,filename,retVal) ) 

#define IGeoProjection_WriteToFile(This,filename,retVal)	\
    ( (This)->lpVtbl -> WriteToFile(This,filename,retVal) ) 

#define IGeoProjection_ImportFromAutoDetect(This,proj,retVal)	\
    ( (This)->lpVtbl -> ImportFromAutoDetect(This,proj,retVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IGeoProjection_INTERFACE_DEFINED__ */


#ifndef __IGlobalSettings_INTERFACE_DEFINED__
#define __IGlobalSettings_INTERFACE_DEFINED__

/* interface IGlobalSettings */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IGlobalSettings;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("97A80176-EE9A-461E-B494-F4F168F16ECA")
    IGlobalSettings : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MinPolygonArea( 
            /* [retval][out] */ DOUBLE *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MinPolygonArea( 
            /* [in] */ DOUBLE newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_MinAreaToPerimeterRatio( 
            /* [retval][out] */ DOUBLE *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_MinAreaToPerimeterRatio( 
            /* [in] */ DOUBLE newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ClipperGcsMultiplicationFactor( 
            /* [retval][out] */ DOUBLE *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ClipperGcsMultiplicationFactor( 
            /* [in] */ DOUBLE newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShapefileFastMode( 
            /* [retval][out] */ VARIANT_BOOL *retVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ShapefileFastMode( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ShapefileFastUnion( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ShapefileFastUnion( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IGlobalSettingsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IGlobalSettings * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IGlobalSettings * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IGlobalSettings * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IGlobalSettings * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IGlobalSettings * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IGlobalSettings * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IGlobalSettings * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MinPolygonArea )( 
            IGlobalSettings * This,
            /* [retval][out] */ DOUBLE *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MinPolygonArea )( 
            IGlobalSettings * This,
            /* [in] */ DOUBLE newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MinAreaToPerimeterRatio )( 
            IGlobalSettings * This,
            /* [retval][out] */ DOUBLE *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_MinAreaToPerimeterRatio )( 
            IGlobalSettings * This,
            /* [in] */ DOUBLE newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ClipperGcsMultiplicationFactor )( 
            IGlobalSettings * This,
            /* [retval][out] */ DOUBLE *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ClipperGcsMultiplicationFactor )( 
            IGlobalSettings * This,
            /* [in] */ DOUBLE newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShapefileFastMode )( 
            IGlobalSettings * This,
            /* [retval][out] */ VARIANT_BOOL *retVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ShapefileFastMode )( 
            IGlobalSettings * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ShapefileFastUnion )( 
            IGlobalSettings * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ShapefileFastUnion )( 
            IGlobalSettings * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        END_INTERFACE
    } IGlobalSettingsVtbl;

    interface IGlobalSettings
    {
        CONST_VTBL struct IGlobalSettingsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IGlobalSettings_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IGlobalSettings_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IGlobalSettings_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IGlobalSettings_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IGlobalSettings_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IGlobalSettings_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IGlobalSettings_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IGlobalSettings_get_MinPolygonArea(This,retVal)	\
    ( (This)->lpVtbl -> get_MinPolygonArea(This,retVal) ) 

#define IGlobalSettings_put_MinPolygonArea(This,newVal)	\
    ( (This)->lpVtbl -> put_MinPolygonArea(This,newVal) ) 

#define IGlobalSettings_get_MinAreaToPerimeterRatio(This,retVal)	\
    ( (This)->lpVtbl -> get_MinAreaToPerimeterRatio(This,retVal) ) 

#define IGlobalSettings_put_MinAreaToPerimeterRatio(This,newVal)	\
    ( (This)->lpVtbl -> put_MinAreaToPerimeterRatio(This,newVal) ) 

#define IGlobalSettings_get_ClipperGcsMultiplicationFactor(This,pVal)	\
    ( (This)->lpVtbl -> get_ClipperGcsMultiplicationFactor(This,pVal) ) 

#define IGlobalSettings_put_ClipperGcsMultiplicationFactor(This,newVal)	\
    ( (This)->lpVtbl -> put_ClipperGcsMultiplicationFactor(This,newVal) ) 

#define IGlobalSettings_get_ShapefileFastMode(This,retVal)	\
    ( (This)->lpVtbl -> get_ShapefileFastMode(This,retVal) ) 

#define IGlobalSettings_put_ShapefileFastMode(This,newVal)	\
    ( (This)->lpVtbl -> put_ShapefileFastMode(This,newVal) ) 

#define IGlobalSettings_get_ShapefileFastUnion(This,pVal)	\
    ( (This)->lpVtbl -> get_ShapefileFastUnion(This,pVal) ) 

#define IGlobalSettings_put_ShapefileFastUnion(This,newVal)	\
    ( (This)->lpVtbl -> put_ShapefileFastUnion(This,newVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IGlobalSettings_INTERFACE_DEFINED__ */


#ifndef __ITiles_INTERFACE_DEFINED__
#define __ITiles_INTERFACE_DEFINED__

/* interface ITiles */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ITiles;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("6BC1A3D4-74B0-426E-8BE8-01AE26A4F470")
    ITiles : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Clear( void) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Count( 
            /* [retval][out] */ LONG *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Visible( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Visible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Add( 
            /* [in] */ VARIANT bytesArray,
            /* [in] */ double xLon,
            /* [in] */ double yLat,
            /* [in] */ double widthLon,
            /* [in] */ double heightLat) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ITilesVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ITiles * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ITiles * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ITiles * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ITiles * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ITiles * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ITiles * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ITiles * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Clear )( 
            ITiles * This);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Count )( 
            ITiles * This,
            /* [retval][out] */ LONG *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Visible )( 
            ITiles * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Visible )( 
            ITiles * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Add )( 
            ITiles * This,
            /* [in] */ VARIANT bytesArray,
            /* [in] */ double xLon,
            /* [in] */ double yLat,
            /* [in] */ double widthLon,
            /* [in] */ double heightLat);
        
        END_INTERFACE
    } ITilesVtbl;

    interface ITiles
    {
        CONST_VTBL struct ITilesVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ITiles_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ITiles_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ITiles_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ITiles_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ITiles_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ITiles_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ITiles_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ITiles_Clear(This)	\
    ( (This)->lpVtbl -> Clear(This) ) 

#define ITiles_get_Count(This,pVal)	\
    ( (This)->lpVtbl -> get_Count(This,pVal) ) 

#define ITiles_get_Visible(This,pVal)	\
    ( (This)->lpVtbl -> get_Visible(This,pVal) ) 

#define ITiles_put_Visible(This,newVal)	\
    ( (This)->lpVtbl -> put_Visible(This,newVal) ) 

#define ITiles_Add(This,bytesArray,xLon,yLat,widthLon,heightLat)	\
    ( (This)->lpVtbl -> Add(This,bytesArray,xLon,yLat,widthLon,heightLat) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ITiles_INTERFACE_DEFINED__ */



#ifndef __MapWinGIS_LIBRARY_DEFINED__
#define __MapWinGIS_LIBRARY_DEFINED__

/* library MapWinGIS */
/* [control][helpstring][helpfile][version][uuid] */ 


EXTERN_C const IID LIBID_MapWinGIS;

#ifndef ___DMap_DISPINTERFACE_DEFINED__
#define ___DMap_DISPINTERFACE_DEFINED__

/* dispinterface _DMap */
/* [hidden][version][helpstring][uuid] */ 


EXTERN_C const IID DIID__DMap;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("1D077739-E866-46A0-B256-8AECC04F2312")
    _DMap : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DMapVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DMap * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DMap * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DMap * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DMap * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DMap * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DMap * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DMap * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _DMapVtbl;

    interface _DMap
    {
        CONST_VTBL struct _DMapVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DMap_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DMap_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DMap_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DMap_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DMap_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DMap_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DMap_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DMap_DISPINTERFACE_DEFINED__ */


#ifndef ___DMapEvents_DISPINTERFACE_DEFINED__
#define ___DMapEvents_DISPINTERFACE_DEFINED__

/* dispinterface _DMapEvents */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__DMapEvents;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("ABEA1545-08AB-4D5C-A594-D3017211EA95")
    _DMapEvents : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _DMapEventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _DMapEvents * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _DMapEvents * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _DMapEvents * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _DMapEvents * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _DMapEvents * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _DMapEvents * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _DMapEvents * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _DMapEventsVtbl;

    interface _DMapEvents
    {
        CONST_VTBL struct _DMapEventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _DMapEvents_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _DMapEvents_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _DMapEvents_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _DMapEvents_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _DMapEvents_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _DMapEvents_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _DMapEvents_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___DMapEvents_DISPINTERFACE_DEFINED__ */


#ifndef __IShapefileColorScheme_INTERFACE_DEFINED__
#define __IShapefileColorScheme_INTERFACE_DEFINED__

/* interface IShapefileColorScheme */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_IShapefileColorScheme;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("FAE1B21A-10C5-4C33-8DC2-931EDC9FBF82")
    IShapefileColorScheme : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE NumBreaks( 
            /* [retval][out] */ long *result) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Remove( 
            /* [in] */ long Index) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Add( 
            /* [in] */ IShapefileColorBreak *Break,
            /* [retval][out] */ long *result) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ColorBreak( 
            /* [in] */ long Index,
            /* [retval][out] */ IShapefileColorBreak **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_ColorBreak( 
            /* [in] */ long Index,
            /* [in] */ IShapefileColorBreak *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LayerHandle( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_LayerHandle( 
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_FieldIndex( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_FieldIndex( 
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_LastErrorCode( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_ErrorMsg( 
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_GlobalCallback( 
            /* [retval][out] */ ICallback **pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_GlobalCallback( 
            /* [in] */ ICallback *newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Key( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Key( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE InsertAt( 
            /* [in] */ int Position,
            /* [in] */ IShapefileColorBreak *Break,
            /* [retval][out] */ long *result) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IShapefileColorSchemeVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IShapefileColorScheme * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IShapefileColorScheme * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IShapefileColorScheme * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IShapefileColorScheme * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IShapefileColorScheme * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IShapefileColorScheme * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IShapefileColorScheme * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *NumBreaks )( 
            IShapefileColorScheme * This,
            /* [retval][out] */ long *result);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Remove )( 
            IShapefileColorScheme * This,
            /* [in] */ long Index);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Add )( 
            IShapefileColorScheme * This,
            /* [in] */ IShapefileColorBreak *Break,
            /* [retval][out] */ long *result);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ColorBreak )( 
            IShapefileColorScheme * This,
            /* [in] */ long Index,
            /* [retval][out] */ IShapefileColorBreak **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ColorBreak )( 
            IShapefileColorScheme * This,
            /* [in] */ long Index,
            /* [in] */ IShapefileColorBreak *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LayerHandle )( 
            IShapefileColorScheme * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LayerHandle )( 
            IShapefileColorScheme * This,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_FieldIndex )( 
            IShapefileColorScheme * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_FieldIndex )( 
            IShapefileColorScheme * This,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastErrorCode )( 
            IShapefileColorScheme * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ErrorMsg )( 
            IShapefileColorScheme * This,
            /* [in] */ long ErrorCode,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_GlobalCallback )( 
            IShapefileColorScheme * This,
            /* [retval][out] */ ICallback **pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_GlobalCallback )( 
            IShapefileColorScheme * This,
            /* [in] */ ICallback *newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Key )( 
            IShapefileColorScheme * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Key )( 
            IShapefileColorScheme * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *InsertAt )( 
            IShapefileColorScheme * This,
            /* [in] */ int Position,
            /* [in] */ IShapefileColorBreak *Break,
            /* [retval][out] */ long *result);
        
        END_INTERFACE
    } IShapefileColorSchemeVtbl;

    interface IShapefileColorScheme
    {
        CONST_VTBL struct IShapefileColorSchemeVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IShapefileColorScheme_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IShapefileColorScheme_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IShapefileColorScheme_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IShapefileColorScheme_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IShapefileColorScheme_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IShapefileColorScheme_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IShapefileColorScheme_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IShapefileColorScheme_NumBreaks(This,result)	\
    ( (This)->lpVtbl -> NumBreaks(This,result) ) 

#define IShapefileColorScheme_Remove(This,Index)	\
    ( (This)->lpVtbl -> Remove(This,Index) ) 

#define IShapefileColorScheme_Add(This,Break,result)	\
    ( (This)->lpVtbl -> Add(This,Break,result) ) 

#define IShapefileColorScheme_get_ColorBreak(This,Index,pVal)	\
    ( (This)->lpVtbl -> get_ColorBreak(This,Index,pVal) ) 

#define IShapefileColorScheme_put_ColorBreak(This,Index,newVal)	\
    ( (This)->lpVtbl -> put_ColorBreak(This,Index,newVal) ) 

#define IShapefileColorScheme_get_LayerHandle(This,pVal)	\
    ( (This)->lpVtbl -> get_LayerHandle(This,pVal) ) 

#define IShapefileColorScheme_put_LayerHandle(This,newVal)	\
    ( (This)->lpVtbl -> put_LayerHandle(This,newVal) ) 

#define IShapefileColorScheme_get_FieldIndex(This,pVal)	\
    ( (This)->lpVtbl -> get_FieldIndex(This,pVal) ) 

#define IShapefileColorScheme_put_FieldIndex(This,newVal)	\
    ( (This)->lpVtbl -> put_FieldIndex(This,newVal) ) 

#define IShapefileColorScheme_get_LastErrorCode(This,pVal)	\
    ( (This)->lpVtbl -> get_LastErrorCode(This,pVal) ) 

#define IShapefileColorScheme_get_ErrorMsg(This,ErrorCode,pVal)	\
    ( (This)->lpVtbl -> get_ErrorMsg(This,ErrorCode,pVal) ) 

#define IShapefileColorScheme_get_GlobalCallback(This,pVal)	\
    ( (This)->lpVtbl -> get_GlobalCallback(This,pVal) ) 

#define IShapefileColorScheme_put_GlobalCallback(This,newVal)	\
    ( (This)->lpVtbl -> put_GlobalCallback(This,newVal) ) 

#define IShapefileColorScheme_get_Key(This,pVal)	\
    ( (This)->lpVtbl -> get_Key(This,pVal) ) 

#define IShapefileColorScheme_put_Key(This,newVal)	\
    ( (This)->lpVtbl -> put_Key(This,newVal) ) 

#define IShapefileColorScheme_InsertAt(This,Position,Break,result)	\
    ( (This)->lpVtbl -> InsertAt(This,Position,Break,result) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IShapefileColorScheme_INTERFACE_DEFINED__ */


#ifndef __IShapefileColorBreak_INTERFACE_DEFINED__
#define __IShapefileColorBreak_INTERFACE_DEFINED__

/* interface IShapefileColorBreak */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_IShapefileColorBreak;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("E6D4EB7A-3E8F-45B2-A514-90EF7B2F5C0A")
    IShapefileColorBreak : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_StartValue( 
            /* [retval][out] */ VARIANT *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_StartValue( 
            /* [in] */ VARIANT newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_EndValue( 
            /* [retval][out] */ VARIANT *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_EndValue( 
            /* [in] */ VARIANT newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_StartColor( 
            /* [retval][out] */ OLE_COLOR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_StartColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_EndColor( 
            /* [retval][out] */ OLE_COLOR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_EndColor( 
            /* [in] */ OLE_COLOR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Caption( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Caption( 
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Visible( 
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Visible( 
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IShapefileColorBreakVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IShapefileColorBreak * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IShapefileColorBreak * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IShapefileColorBreak * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IShapefileColorBreak * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IShapefileColorBreak * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IShapefileColorBreak * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IShapefileColorBreak * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_StartValue )( 
            IShapefileColorBreak * This,
            /* [retval][out] */ VARIANT *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_StartValue )( 
            IShapefileColorBreak * This,
            /* [in] */ VARIANT newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_EndValue )( 
            IShapefileColorBreak * This,
            /* [retval][out] */ VARIANT *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_EndValue )( 
            IShapefileColorBreak * This,
            /* [in] */ VARIANT newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_StartColor )( 
            IShapefileColorBreak * This,
            /* [retval][out] */ OLE_COLOR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_StartColor )( 
            IShapefileColorBreak * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_EndColor )( 
            IShapefileColorBreak * This,
            /* [retval][out] */ OLE_COLOR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_EndColor )( 
            IShapefileColorBreak * This,
            /* [in] */ OLE_COLOR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Caption )( 
            IShapefileColorBreak * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Caption )( 
            IShapefileColorBreak * This,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Visible )( 
            IShapefileColorBreak * This,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Visible )( 
            IShapefileColorBreak * This,
            /* [in] */ VARIANT_BOOL newVal);
        
        END_INTERFACE
    } IShapefileColorBreakVtbl;

    interface IShapefileColorBreak
    {
        CONST_VTBL struct IShapefileColorBreakVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IShapefileColorBreak_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IShapefileColorBreak_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IShapefileColorBreak_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IShapefileColorBreak_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IShapefileColorBreak_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IShapefileColorBreak_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IShapefileColorBreak_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IShapefileColorBreak_get_StartValue(This,pVal)	\
    ( (This)->lpVtbl -> get_StartValue(This,pVal) ) 

#define IShapefileColorBreak_put_StartValue(This,newVal)	\
    ( (This)->lpVtbl -> put_StartValue(This,newVal) ) 

#define IShapefileColorBreak_get_EndValue(This,pVal)	\
    ( (This)->lpVtbl -> get_EndValue(This,pVal) ) 

#define IShapefileColorBreak_put_EndValue(This,newVal)	\
    ( (This)->lpVtbl -> put_EndValue(This,newVal) ) 

#define IShapefileColorBreak_get_StartColor(This,pVal)	\
    ( (This)->lpVtbl -> get_StartColor(This,pVal) ) 

#define IShapefileColorBreak_put_StartColor(This,newVal)	\
    ( (This)->lpVtbl -> put_StartColor(This,newVal) ) 

#define IShapefileColorBreak_get_EndColor(This,pVal)	\
    ( (This)->lpVtbl -> get_EndColor(This,pVal) ) 

#define IShapefileColorBreak_put_EndColor(This,newVal)	\
    ( (This)->lpVtbl -> put_EndColor(This,newVal) ) 

#define IShapefileColorBreak_get_Caption(This,pVal)	\
    ( (This)->lpVtbl -> get_Caption(This,pVal) ) 

#define IShapefileColorBreak_put_Caption(This,newVal)	\
    ( (This)->lpVtbl -> put_Caption(This,newVal) ) 

#define IShapefileColorBreak_get_Visible(This,pVal)	\
    ( (This)->lpVtbl -> get_Visible(This,pVal) ) 

#define IShapefileColorBreak_put_Visible(This,newVal)	\
    ( (This)->lpVtbl -> put_Visible(This,newVal) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IShapefileColorBreak_INTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_Map;

#ifdef __cplusplus

class DECLSPEC_UUID("54F4C2F7-ED40-43B7-9D6F-E45965DF7F95")
Map;
#endif

EXTERN_C const CLSID CLSID_ShapefileColorScheme;

#ifdef __cplusplus

class DECLSPEC_UUID("A038D3E9-46CB-4F95-A40A-88826BF71BA6")
ShapefileColorScheme;
#endif

EXTERN_C const CLSID CLSID_ShapefileColorBreak;

#ifdef __cplusplus

class DECLSPEC_UUID("700A2AAA-0D28-4943-92EC-08AA9682617A")
ShapefileColorBreak;
#endif

EXTERN_C const CLSID CLSID_Grid;

#ifdef __cplusplus

class DECLSPEC_UUID("B4A353E3-D3DF-455C-8E4D-CFC937800820")
Grid;
#endif

EXTERN_C const CLSID CLSID_GridHeader;

#ifdef __cplusplus

class DECLSPEC_UUID("044AFE79-D3DE-4500-A14B-DECEA635B497")
GridHeader;
#endif

EXTERN_C const CLSID CLSID_ESRIGridManager;

#ifdef __cplusplus

class DECLSPEC_UUID("86E02063-602C-47F2-9778-81E6979E3267")
ESRIGridManager;
#endif

EXTERN_C const CLSID CLSID_Image;

#ifdef __cplusplus

class DECLSPEC_UUID("0DB362E3-6F79-4226-AF19-47B67B27E99B")
Image;
#endif

EXTERN_C const CLSID CLSID_Shapefile;

#ifdef __cplusplus

class DECLSPEC_UUID("C0EAC9EB-1D02-4BD9-8DAB-4BF922C8CD13")
Shapefile;
#endif

EXTERN_C const CLSID CLSID_Shape;

#ifdef __cplusplus

class DECLSPEC_UUID("CE7E6869-6F74-4E9D-9F07-3DCBADAB6299")
Shape;
#endif

EXTERN_C const CLSID CLSID_Extents;

#ifdef __cplusplus

class DECLSPEC_UUID("03F9B3DB-637B-4544-BF7A-2F190F821F0D")
Extents;
#endif

EXTERN_C const CLSID CLSID_Point;

#ifdef __cplusplus

class DECLSPEC_UUID("CE63AD29-C5EB-4865-B143-E0AC35ED6FBC")
Point;
#endif

EXTERN_C const CLSID CLSID_Table;

#ifdef __cplusplus

class DECLSPEC_UUID("97EFB80F-3638-4BDC-9128-C5A30194C257")
Table;
#endif

EXTERN_C const CLSID CLSID_Field;

#ifdef __cplusplus

class DECLSPEC_UUID("C2C71E09-3DEB-4E6C-B54A-D5613986BFFE")
Field;
#endif

EXTERN_C const CLSID CLSID_ShapeNetwork;

#ifdef __cplusplus

class DECLSPEC_UUID("B655545F-1D9C-4D81-A73C-205FC2C3C4AB")
ShapeNetwork;
#endif

EXTERN_C const CLSID CLSID_Utils;

#ifdef __cplusplus

class DECLSPEC_UUID("B898877F-DC9E-4FBF-B997-B65DC97B72E9")
Utils;
#endif

EXTERN_C const CLSID CLSID_Vector;

#ifdef __cplusplus

class DECLSPEC_UUID("D226C4B1-C97C-469D-8CBC-8E3DF2139612")
Vector;
#endif

EXTERN_C const CLSID CLSID_GridColorScheme;

#ifdef __cplusplus

class DECLSPEC_UUID("ECEB5841-F84E-4DFD-8C96-32216C69C818")
GridColorScheme;
#endif

EXTERN_C const CLSID CLSID_GridColorBreak;

#ifdef __cplusplus

class DECLSPEC_UUID("B82B0EB0-05B6-4FF2-AA16-BCD33FDE6568")
GridColorBreak;
#endif

EXTERN_C const CLSID CLSID_Tin;

#ifdef __cplusplus

class DECLSPEC_UUID("677B1AF6-A28D-4FAB-8A5F-0F8763D88638")
Tin;
#endif

EXTERN_C const CLSID CLSID_ShapeDrawingOptions;

#ifdef __cplusplus

class DECLSPEC_UUID("58804A7F-2C75-41AF-9D32-5BD08DB1BAF6")
ShapeDrawingOptions;
#endif

EXTERN_C const CLSID CLSID_Labels;

#ifdef __cplusplus

class DECLSPEC_UUID("CEA6B369-F2EC-4927-BD8C-F0F6A4066EC6")
Labels;
#endif

EXTERN_C const CLSID CLSID_LabelCategory;

#ifdef __cplusplus

class DECLSPEC_UUID("92ADD941-94C2-4A57-A058-E9999F21D6BF")
LabelCategory;
#endif

EXTERN_C const CLSID CLSID_Label;

#ifdef __cplusplus

class DECLSPEC_UUID("4D745AC7-D623-4F51-BA01-18793FC778A6")
Label;
#endif

EXTERN_C const CLSID CLSID_ShapefileCategories;

#ifdef __cplusplus

class DECLSPEC_UUID("1A3B0D02-9265-41B0-84BB-9E09F262FF82")
ShapefileCategories;
#endif

EXTERN_C const CLSID CLSID_ShapefileCategory;

#ifdef __cplusplus

class DECLSPEC_UUID("51464A2A-69F7-4CAD-8728-9608580210A3")
ShapefileCategory;
#endif

EXTERN_C const CLSID CLSID_Charts;

#ifdef __cplusplus

class DECLSPEC_UUID("1176C871-4C0B-48CF-85B6-926A7948E0F7")
Charts;
#endif

EXTERN_C const CLSID CLSID_Chart;

#ifdef __cplusplus

class DECLSPEC_UUID("A109A2A1-775F-4FBF-B0C7-F703F8B0BC90")
Chart;
#endif

EXTERN_C const CLSID CLSID_ColorScheme;

#ifdef __cplusplus

class DECLSPEC_UUID("60409E71-BBB8-491C-A48B-ADA7F383CB6E")
ColorScheme;
#endif

EXTERN_C const CLSID CLSID_ChartField;

#ifdef __cplusplus

class DECLSPEC_UUID("8C429C40-4F0F-479A-B492-98819424801D")
ChartField;
#endif

EXTERN_C const CLSID CLSID_LinePattern;

#ifdef __cplusplus

class DECLSPEC_UUID("FF695B0C-4977-4D9E-88DD-0DF4FF7082BC")
LinePattern;
#endif

EXTERN_C const CLSID CLSID_LineSegment;

#ifdef __cplusplus

class DECLSPEC_UUID("03A98C90-70FF-40C7-AD93-6BF8B41B170F")
LineSegment;
#endif

EXTERN_C const CLSID CLSID_GeoProjection;

#ifdef __cplusplus

class DECLSPEC_UUID("B0828DB2-3354-419F-82B0-AC0478DDB00D")
GeoProjection;
#endif

EXTERN_C const CLSID CLSID_GlobalSettings;

#ifdef __cplusplus

class DECLSPEC_UUID("80CDFEE0-576F-4141-906E-877638A2AEF3")
GlobalSettings;
#endif

EXTERN_C const CLSID CLSID_Tiles;

#ifdef __cplusplus

class DECLSPEC_UUID("9C7B823D-B1BE-4C39-A552-8B148C008FC5")
Tiles;
#endif
#endif /* __MapWinGIS_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  BSTR_UserSize(     unsigned long *, unsigned long            , BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserMarshal(  unsigned long *, unsigned char *, BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserUnmarshal(unsigned long *, unsigned char *, BSTR * ); 
void                      __RPC_USER  BSTR_UserFree(     unsigned long *, BSTR * ); 

unsigned long             __RPC_USER  LPSAFEARRAY_UserSize(     unsigned long *, unsigned long            , LPSAFEARRAY * ); 
unsigned char * __RPC_USER  LPSAFEARRAY_UserMarshal(  unsigned long *, unsigned char *, LPSAFEARRAY * ); 
unsigned char * __RPC_USER  LPSAFEARRAY_UserUnmarshal(unsigned long *, unsigned char *, LPSAFEARRAY * ); 
void                      __RPC_USER  LPSAFEARRAY_UserFree(     unsigned long *, LPSAFEARRAY * ); 

unsigned long             __RPC_USER  VARIANT_UserSize(     unsigned long *, unsigned long            , VARIANT * ); 
unsigned char * __RPC_USER  VARIANT_UserMarshal(  unsigned long *, unsigned char *, VARIANT * ); 
unsigned char * __RPC_USER  VARIANT_UserUnmarshal(unsigned long *, unsigned char *, VARIANT * ); 
void                      __RPC_USER  VARIANT_UserFree(     unsigned long *, VARIANT * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


