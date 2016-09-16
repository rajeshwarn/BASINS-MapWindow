
namespace AxMapWinGIS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// MapWinGIS Map component.
    /// </summary>
    public class AxMap  //: MapWinGIS._DMap
    {
        #region _DMap Members
        /// <summary>
        /// Adds label to the specified drawing layer 
        /// </summary>
        /// <param name="DrawHandle"></param>
        /// <param name="Text"></param>
        /// <param name="Color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="hJustification"></param>
        public void AddDrawingLabel(int DrawHandle, string Text, uint Color, double x, double y, MapWinGIS.tkHJustification hJustification)
        {
            throw new NotImplementedException();
        }

        public void AddDrawingLabelEx(int DrawHandle, string Text, uint Color, double x, double y, MapWinGIS.tkHJustification hJustification, double Rotation)
        {
            throw new NotImplementedException();
        }

        public void AddLabel(int LayerHandle, string Text, uint Color, double x, double y, MapWinGIS.tkHJustification hJustification)
        {
            throw new NotImplementedException();
        }

        public void AddLabelEx(int LayerHandle, string Text, uint Color, double x, double y, MapWinGIS.tkHJustification hJustification, double Rotation)
        {
            throw new NotImplementedException();
        }

        public int AddLayer(object Object, bool Visible)
        {
            throw new NotImplementedException();
        }

        public bool AdjustLayerExtents(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool ApplyLegendColors(object Legend)
        {
            throw new NotImplementedException();
        }

        public uint BackColor
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool CanUseImageGrouping
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void ClearDrawing(int DrawHandle)
        {
            throw new NotImplementedException();
        }

        public void ClearDrawingLabels(int DrawHandle)
        {
            throw new NotImplementedException();
        }

        public void ClearDrawings()
        {
            throw new NotImplementedException();
        }

        public void ClearLabels(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public void ClearUDPointImageList(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public double CurrentScale
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public MapWinGIS.tkCursorMode CursorMode
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool DeserializeLayer(int LayerHandle, string newVal)
        {
            throw new NotImplementedException();
        }

        public bool DeserializeMapState(string State, bool LoadLayers, string BasePath)
        {
            throw new NotImplementedException();
        }

        public bool DisableWaitCursor
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool DoubleBuffer
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void DrawBackBuffer(IntPtr hdc, int ImageWidth, int ImageHeight)
        {
            throw new NotImplementedException();
        }

        public void DrawCircle(double x, double y, double pixelRadius, uint Color, bool fill)
        {
            throw new NotImplementedException();
        }

        public void DrawCircleEx(int LayerHandle, double x, double y, double pixelRadius, uint Color, bool fill)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(double x1, double y1, double x2, double y2, int pixelWidth, uint Color)
        {
            throw new NotImplementedException();
        }

        public void DrawLineEx(int LayerHandle, double x1, double y1, double x2, double y2, int pixelWidth, uint Color)
        {
            throw new NotImplementedException();
        }

        public void DrawPoint(double x, double y, int pixelSize, uint Color)
        {
            throw new NotImplementedException();
        }

        public void DrawPointEx(int LayerHandle, double x, double y, int pixelSize, uint Color)
        {
            throw new NotImplementedException();
        }

        public void DrawPolygon(ref object xPoints, ref object yPoints, int numPoints, uint Color, bool fill)
        {
            throw new NotImplementedException();
        }

        public void DrawPolygonEx(int LayerHandle, ref object xPoints, ref object yPoints, int numPoints, uint Color, bool fill)
        {
            throw new NotImplementedException();
        }

        public void DrawWideCircle(double x, double y, double pixelRadius, uint Color, bool fill, short Width)
        {
            throw new NotImplementedException();
        }

        public void DrawWideCircleEx(int LayerHandle, double x, double y, double radius, uint Color, bool fill, short OutlineWidth)
        {
            throw new NotImplementedException();
        }

        public void DrawWidePolygon(ref object xPoints, ref object yPoints, int numPoints, uint Color, bool fill, short Width)
        {
            throw new NotImplementedException();
        }

        public void DrawWidePolygonEx(int LayerHandle, ref object xPoints, ref object yPoints, int numPoints, uint Color, bool fill, short OutlineWidth)
        {
            throw new NotImplementedException();
        }

        public void DrawingFont(int DrawHandle, string FontName, int FontSize)
        {
            throw new NotImplementedException();
        }

        public int ExtentHistory
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double ExtentPad
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object Extents
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public MapWinGIS.Point GetBaseProjectionPoint(double rotX, double rotY)
        {
            throw new NotImplementedException();
        }

        public object GetColorScheme(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public void GetDrawingStandardViewWidth(int DrawHandle, ref double Width)
        {
            throw new NotImplementedException();
        }

        public void GetLayerStandardViewWidth(int LayerHandle, ref double Width)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Extents GetRotatedExtent()
        {
            throw new NotImplementedException();
        }

        public object GlobalCallback
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int HWnd()
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.tkLockMode IsLocked
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSameProjection(string proj4_a, string proj4_b)
        {
            throw new NotImplementedException();
        }

        public bool IsTIFFGrid(string Filename)
        {
            throw new NotImplementedException();
        }

        public string Key
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void LabelColor(int LayerHandle, uint LabelFontColor)
        {
            throw new NotImplementedException();
        }

        public int LastErrorCode
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void LayerFont(int LayerHandle, string FontName, int FontSize)
        {
            throw new NotImplementedException();
        }

        public void LayerFontEx(int LayerHandle, string FontName, int FontSize, bool isBold, bool isItalic, bool isUnderline)
        {
            throw new NotImplementedException();
        }

        public int LineSeparationFactor
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool LoadLayerOptions(int LayerHandle, string OptionsName, ref string Description)
        {
            throw new NotImplementedException();
        }

        public bool LoadMapState(string Filename, object Callback)
        {
            throw new NotImplementedException();
        }

        public void LockWindow(MapWinGIS.tkLockMode LockMode)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.tkCursor MapCursor
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public MapWinGIS.tkResizeBehavior MapResizeBehavior
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double MapRotationAngle
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string MapState
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public MapWinGIS.tkUnitsOfMeasure MapUnits
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public MapWinGIS.Extents MaxExtents
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double MouseWheelSpeed
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool MoveLayer(int InitialPosition, int TargetPosition)
        {
            throw new NotImplementedException();
        }

        public bool MoveLayerBottom(int InitialPosition)
        {
            throw new NotImplementedException();
        }

        public bool MoveLayerDown(int InitialPosition)
        {
            throw new NotImplementedException();
        }

        public bool MoveLayerTop(int InitialPosition)
        {
            throw new NotImplementedException();
        }

        public bool MoveLayerUp(int InitialPosition)
        {
            throw new NotImplementedException();
        }

        public bool MultilineLabels
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int NewDrawing(MapWinGIS.tkDrawReferenceList Projection)
        {
            throw new NotImplementedException();
        }

        public int NumLayers
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void PixelToProj(double pixelX, double pixelY, ref double projX, ref double projY)
        {
            throw new NotImplementedException();
        }

        public double PixelsPerDegree
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void ProjToPixel(double projX, double projY, ref double pixelX, ref double pixelY)
        {
            throw new NotImplementedException();
        }

        public void ReSourceLayer(int LayerHandle, string newSrcPath)
        {
            throw new NotImplementedException();
        }

        public void Redraw()
        {
            throw new NotImplementedException();
        }

        public void RemoveAllLayers()
        {
            throw new NotImplementedException();
        }

        public void RemoveLayer(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool RemoveLayerOptions(int LayerHandle, string OptionsName)
        {
            throw new NotImplementedException();
        }

        public void RemoveLayerWithoutClosing(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public void Resize(int Width, int Height)
        {
            throw new NotImplementedException();
        }

        public bool SaveLayerOptions(int LayerHandle, string OptionsName, bool Overwrite, string Description)
        {
            throw new NotImplementedException();
        }

        public bool SaveMapState(string Filename, bool RelativePaths, bool Overwrite)
        {
            throw new NotImplementedException();
        }

        public bool SendMouseDown
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool SendMouseMove
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool SendMouseUp
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool SendOnDrawBackBuffer
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool SendSelectBoxDrag
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool SendSelectBoxFinal
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string SerialNumber
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string SerializeLayer(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public string SerializeMapState(bool RelativePaths, string BasePath)
        {
            throw new NotImplementedException();
        }

        public void SetDrawingLayerVisible(int LayerHandle, bool Visiable)
        {
            throw new NotImplementedException();
        }

        public void SetDrawingStandardViewWidth(int DrawHandle, double Width)
        {
            throw new NotImplementedException();
        }

        public bool SetImageLayerColorScheme(int LayerHandle, object ColorScheme)
        {
            throw new NotImplementedException();
        }

        public void SetLayerStandardViewWidth(int LayerHandle, double Width)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.tkShapeDrawingMethod ShapeDrawingMethod
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool ShowRedrawTime
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void ShowToolTip(string Text, int Milliseconds)
        {
            throw new NotImplementedException();
        }

        public bool ShowVersionNumber
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object SnapShot(object BoundBox)
        {
            throw new NotImplementedException();
        }

        public object SnapShot2(int ClippingLayerNbr, double Zoom, int pWidth)
        {
            throw new NotImplementedException();
        }

        public object SnapShot3(double left, double right, double top, double bottom, int Width)
        {
            throw new NotImplementedException();
        }

        public bool SnapShotToDC(IntPtr hdc, MapWinGIS.Extents Extents, int Width)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Tiles Tiles
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool TrapRMouseDown
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int UDCursorHandle
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void UpdateImage(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool UseSeamlessPan
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int VersionNumber
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void ZoomIn(double Percent)
        {
            throw new NotImplementedException();
        }

        public void ZoomOut(double Percent)
        {
            throw new NotImplementedException();
        }

        public double ZoomPercent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void ZoomToLayer(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public void ZoomToMaxExtents()
        {
            throw new NotImplementedException();
        }

        public void ZoomToMaxVisibleExtents()
        {
            throw new NotImplementedException();
        }

        public int ZoomToPrev()
        {
            throw new NotImplementedException();
        }

        public void ZoomToShape(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public string get_DrawingKey(int DrawHandle)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Labels get_DrawingLabels(int DrawingLayerIndex)
        {
            throw new NotImplementedException();
        }

        public int get_DrawingLabelsOffset(int DrawHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_DrawingLabelsScale(int DrawHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_DrawingLabelsShadow(int DrawHandle)
        {
            throw new NotImplementedException();
        }

        public uint get_DrawingLabelsShadowColor(int DrawHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_DrawingLabelsVisible(int DrawHandle)
        {
            throw new NotImplementedException();
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public object get_GetObject(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public string get_GridFileName(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Image get_Image(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public float get_ImageLayerPercentTransparent(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public string get_LayerDescription(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_LayerDynamicVisibility(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public int get_LayerHandle(int LayerPosition)
        {
            throw new NotImplementedException();
        }

        public string get_LayerKey(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Labels get_LayerLabels(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public int get_LayerLabelsOffset(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_LayerLabelsScale(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_LayerLabelsShadow(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public uint get_LayerLabelsShadowColor(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_LayerLabelsVisible(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public double get_LayerMaxVisibleScale(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public double get_LayerMinVisibleScale(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public string get_LayerName(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public int get_LayerPosition(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_LayerSkipOnSaving(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_LayerVisible(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_ShapeDrawFill(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public bool get_ShapeDrawLine(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public bool get_ShapeDrawPoint(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public uint get_ShapeFillColor(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.tkFillStipple get_ShapeFillStipple(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public float get_ShapeFillTransparency(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public bool get_ShapeLayerDrawFill(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_ShapeLayerDrawLine(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_ShapeLayerDrawPoint(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public uint get_ShapeLayerFillColor(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.tkFillStipple get_ShapeLayerFillStipple(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public float get_ShapeLayerFillTransparency(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public uint get_ShapeLayerLineColor(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.tkLineStipple get_ShapeLayerLineStipple(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public float get_ShapeLayerLineWidth(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public uint get_ShapeLayerPointColor(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public float get_ShapeLayerPointSize(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.tkPointType get_ShapeLayerPointType(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public uint get_ShapeLayerStippleColor(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_ShapeLayerStippleTransparent(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public uint get_ShapeLineColor(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.tkLineStipple get_ShapeLineStipple(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public float get_ShapeLineWidth(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public uint get_ShapePointColor(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public int get_ShapePointFontCharListID(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public int get_ShapePointImageListID(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public float get_ShapePointSize(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.tkPointType get_ShapePointType(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public uint get_ShapeStippleColor(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public bool get_ShapeStippleTransparent(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public bool get_ShapeVisible(int LayerHandle, int Shape)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Shapefile get_Shapefile(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public int get_UDFillStipple(int LayerHandle, int StippleRow)
        {
            throw new NotImplementedException();
        }

        public int get_UDLineStipple(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public int get_UDPointImageListCount(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public object get_UDPointImageListItem(int LayerHandle, int ImageIndex)
        {
            throw new NotImplementedException();
        }

        public object get_UDPointType(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_UseDrawingLabelCollision(int DrawHandle)
        {
            throw new NotImplementedException();
        }

        public bool get_UseLabelCollision(int LayerHandle)
        {
            throw new NotImplementedException();
        }

        public void set_DrawingKey(int DrawHandle, string __p2)
        {
            throw new NotImplementedException();
        }

        public void set_DrawingLabels(int DrawingLayerIndex, MapWinGIS.Labels __p2)
        {
            throw new NotImplementedException();
        }

        public void set_DrawingLabelsOffset(int DrawHandle, int __p2)
        {
            throw new NotImplementedException();
        }

        public void set_DrawingLabelsScale(int DrawHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_DrawingLabelsShadow(int DrawHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_DrawingLabelsShadowColor(int DrawHandle, uint __p2)
        {
            throw new NotImplementedException();
        }

        public void set_DrawingLabelsVisible(int DrawHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_GridFileName(int LayerHandle, string __p2)
        {
            throw new NotImplementedException();
        }

        public void set_Image(int LayerHandle, MapWinGIS.Image __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ImageLayerPercentTransparent(int LayerHandle, float __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerDescription(int LayerHandle, string __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerDynamicVisibility(int LayerHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerKey(int LayerHandle, string __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerLabels(int LayerHandle, MapWinGIS.Labels __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerLabelsOffset(int LayerHandle, int __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerLabelsScale(int LayerHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerLabelsShadow(int LayerHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerLabelsShadowColor(int LayerHandle, uint __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerLabelsVisible(int LayerHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerMaxVisibleScale(int LayerHandle, double __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerMinVisibleScale(int LayerHandle, double __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerName(int LayerHandle, string __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerSkipOnSaving(int LayerHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_LayerVisible(int LayerHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeDrawFill(int LayerHandle, int Shape, bool __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeDrawLine(int LayerHandle, int Shape, bool __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeDrawPoint(int LayerHandle, int Shape, bool __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeFillColor(int LayerHandle, int Shape, uint __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeFillStipple(int LayerHandle, int Shape, MapWinGIS.tkFillStipple __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeFillTransparency(int LayerHandle, int Shape, float __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerDrawFill(int LayerHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerDrawLine(int LayerHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerDrawPoint(int LayerHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerFillColor(int LayerHandle, uint __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerFillStipple(int LayerHandle, MapWinGIS.tkFillStipple __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerFillTransparency(int LayerHandle, float __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerLineColor(int LayerHandle, uint __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerLineStipple(int LayerHandle, MapWinGIS.tkLineStipple __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerLineWidth(int LayerHandle, float __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerPointColor(int LayerHandle, uint __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerPointSize(int LayerHandle, float __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerPointType(int LayerHandle, MapWinGIS.tkPointType __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerStippleColor(int LayerHandle, uint __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLayerStippleTransparent(int LayerHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLineColor(int LayerHandle, int Shape, uint __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLineStipple(int LayerHandle, int Shape, MapWinGIS.tkLineStipple __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeLineWidth(int LayerHandle, int Shape, float __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapePointColor(int LayerHandle, int Shape, uint __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapePointFontCharListID(int LayerHandle, int Shape, int __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapePointImageListID(int LayerHandle, int Shape, int __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapePointSize(int LayerHandle, int Shape, float __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapePointType(int LayerHandle, int Shape, MapWinGIS.tkPointType __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeStippleColor(int LayerHandle, int Shape, uint __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeStippleTransparent(int LayerHandle, int Shape, bool __p3)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeVisible(int LayerHandle, int Shape, bool __p3)
        {
            throw new NotImplementedException();
        }

        public void set_Shapefile(int LayerHandle, MapWinGIS.Shapefile __p2)
        {
            throw new NotImplementedException();
        }

        public void set_UDFillStipple(int LayerHandle, int StippleRow, int __p3)
        {
            throw new NotImplementedException();
        }

        public void set_UDLineStipple(int LayerHandle, int __p2)
        {
            throw new NotImplementedException();
        }

        public void set_UDPointFontCharFont(int LayerHandle, string FontName, float FontSize, bool isBold, bool isItalic, bool isUnderline)
        {
            throw new NotImplementedException();
        }

        public void set_UDPointFontCharFontSize(int LayerHandle, float FontSize)
        {
            throw new NotImplementedException();
        }

        public int set_UDPointFontCharListAdd(int LayerHandle, int newValue, uint Color)
        {
            throw new NotImplementedException();
        }

        public int set_UDPointImageListAdd(int LayerHandle, object newValue)
        {
            throw new NotImplementedException();
        }

        public void set_UDPointType(int LayerHandle, object __p2)
        {
            throw new NotImplementedException();
        }

        public void set_UseDrawingLabelCollision(int DrawHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        public void set_UseLabelCollision(int LayerHandle, bool __p2)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    
}
