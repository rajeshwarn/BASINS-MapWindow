
namespace MapWinGIS
{
    using System;
    
    /// <summary>
    /// A class which encapsulates all the operation with ESRI shapefiles, for both file-based and in-memory mode.
    /// Facilitates creation, editing, quering and geoprocessing of shapefiles. Shapefile holds geometry of objects 
    /// (.shp and .shx part) and their attribute values (.dbf part)
    /// </summary>
    public class IShapefile
    {
        #region IShapefile Members

        /// <summary>
        /// Creates a new instance shapefile class with shapes aggregated based on the value of the specified field.
        /// Aggregation means creation of a single multi-part shape from several single-part or multi-part input shapes.
        /// </summary>
        /// <param name="SelectedOnly">Only selected shapes will be included in the RESULT</param>
        /// <param name="FieldIndex">The index of field to group shapes by (all the shapes with the same value of this field
        /// will form one aggregated shape).</param>
        /// <returns>Reference to the shapefile on success or NULL reference on failure. 
        /// Use Shapefile.ErrorMsg(Shapefile.LastErrorCode) to find out the reason of failure.</returns>
        public MapWinGIS.Shapefile AggregateShapes(bool SelectedOnly, int FieldIndex)
        {
            throw new NotImplementedException();
        }

        public bool BeginPointInShapefile()
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Shapefile BufferByDistance(double Distance, int nSegments, bool SelectedOnly, bool MergeResults)
        {
            throw new NotImplementedException();
        }

        public bool CacheExtents
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

        /// <summary>
        /// Gets or sets an instance of ShapefileCategories class associated with shapefile. 
        /// The property can't be set to NULL (there is always an instance ShapefileCategories class associated with it).
        /// </summary>
        public MapWinGIS.ShapefileCategories Categories
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

        /// <summary>
        /// Gets expression to be set for OpenFileDialog.Filter property to select ESRI shapefiles.
        /// </summary>
        public string CdlgFilter
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets an instance of Charts class associated with shapefile. 
        /// The property can't be set to NULL (there is always an instance of Charts class associated with it).
        /// </summary>
        public MapWinGIS.Charts Charts
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

        public MapWinGIS.Shapefile Clip(bool SelectedOnlySubject, MapWinGIS.Shapefile sfOverlay, bool SelectedOnlyOverlay)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates in-memory copy of the shapefile of the same type and with the same fields in attribute table.
        /// Shapes and their attributes aren't copied.
        /// </summary>
        public MapWinGIS.Shapefile Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Closes shapefile and releases all the resources. In case shapefile was in editing mode 
        /// (Shapefile.EditingShapes = true), all the edits will be discarded.
        /// </summary>
        /// <returns>Returns true on correct closing of editing mode. This value can be skipped.</returns>
        public bool Close()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets collision mode for shapefile labels and charts.
        /// </summary>
        ///<value></value>
        public MapWinGIS.tkCollisionMode CollisionMode
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

        /// <summary>
        /// Initializes in-memory shapefile of the specified type. An EditingShapes property for the new shapefile will be set to true. 
        /// Any shapefile opened in this instance of class before this call, will be closed without saving the changes (if any).
        /// </summary>
        /// <param name="ShapefileName">The name of the new shapefile. An empty string should be passed here.</param>
        /// <param name="ShapefileType">Type of the shapefile to create.</param>
        /// <returns>Returns true on success and false otherwise.</returns>
        /// Use Shapefile.ErrorMsg(Shapefile.LastErrorCode) to find out the reason of failure.</returns>
        public bool CreateNew(string ShapefileName, MapWinGIS.ShpfileType ShapefileType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes in-memory shapefile of the specified type in the same way as Shapefile.CreateNew.
        /// Besides [MWShapeID] field will be added to attribute table. A unique positive interger value will be set 
        /// in this field for each new shape added to shapefile.
        /// </summary>
        /// <param name="ShapefileName">The name of the new shapefile. An empty string should be passed here.</param>
        /// <param name="ShapefileType">Type of the shapefile to create.</param>
        /// <returns>Returns true on success and false otherwise.</returns>
        /// Use Shapefile.ErrorMsg(Shapefile.LastErrorCode) to find out the reason of failure.</returns>
        public bool CreateNewWithShapeID(string ShapefileName, MapWinGIS.ShpfileType ShapefileType)
        {
            throw new NotImplementedException();
        }

        public bool CreateSpatialIndex(string ShapefileName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets an instance of ShapeDrawingOptions class which holds default drawing options. 
        /// Default options are applied for every shape, which doesn't belong to any shapefile category and doesn't fall into selection.
        /// The property can't be set to NULL (there is always an instance ShapeDrawingOptions class associated with it).
        /// </summary>
        public MapWinGIS.ShapeDrawingOptions DefaultDrawingOptions
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

        /// <summary>
        /// Restores the state of shapefile from provided string. The string must be generated by Shapefile.Serialize method.
        /// 
        /// </summary>
        /// <param name="LoadSelection">Determines whether selection state of individual shapes shuld be loaded.</param>
        /// <param name="newVal">A string to restore values from.</param>
        public void Deserialize(bool LoadSelection, string newVal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the state of the shapefile in XML-formatted string. Serialization covers shapefile properties and all child classes 
        /// (drawing options, labels, charts, categories). Geometry of shapes and values of attribute table will not be serialized.
        /// </summary>
        /// <param name="SaveSelection">Determines with selection state of individual shapes shuld be saved.</param>
        /// <returns>XML-formatted string or empty string on failure.
        /// Use Shapefile.ErrorMsg(Shapefile.LastErrorCode) to find out the reason of failure.</returns>
        public string Serialize(bool SaveSelection)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Shapefile Difference(bool SelectedOnlySubject, MapWinGIS.Shapefile sfOverlay, bool SelectedOnlyOverlay)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Shapefile Dissolve(int FieldIndex, bool SelectedOnly)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the new value for particular cell in attribute table. The table must be in editing mode 
        /// (see Shapefile.EditingTable propety).
        /// </summary>
        /// <param name="FieldIndex">The index of field in attribute table</param>
        /// <param name="ShapeIndex">The index of shape (row number) in attribute table</param>
        /// <param name="newVal">The variant value to be passed (integer, double and string values are accepted depending on field type)</param>
        /// <returns></returns>
        public bool EditCellValue(int FieldIndex, int ShapeIndex, object newVal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes all the shapes from shapefile and cooresponding records from the attribute table. 
        /// Both shapefile and attribute table should be in editing mode. See (see Shapefile.EditingTable and Shapefile.EditingShapes properties).
        /// </summary>
        /// <returns>Returns true on success and false otherwise.</returns>
        public bool EditClear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a field with the specified index from the attribute table. The table must be in editing mode 
        /// (see Shapefile.EditingTable propety).
        /// </summary>
        /// <param name="FieldIndex">An index of field to delete.</param>
        /// <param name="cBack">An instance of class implementing ICallback interface. Use Shapefile.GlobalCallback property instead.</param>
        /// <returns>Returns true on success and false otherwise</returns>
        public bool EditDeleteField(int FieldIndex, MapWinGIS.ICallback cBack)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete shape with the specified index from shapefile. 
        /// Both shapefile and attribute table should be in editing mode. See (see Shapefile.EditingTable and Shapefile.EditingShapes properties).
        /// </summary>
        /// <param name="ShapeIndex">The index of shape to delete.</param>
        /// <returns>Returns true on success and false otherwise</returns>
        public bool EditDeleteShape(int ShapeIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts a new field in the shapefile attribute table.
        /// </summary>
        /// <param name="NewField">A new instance of field object to insert.</param>
        /// <param name="FieldIndex">A position to insert the new field. An ivalid index will be automatically substituted with 0 ot numFields</param>
        /// <param name="cBack">An instance of class implementing ICallback interface. Pass NULL and use Shapefile.GlobalCallback property if needed.</param>
        /// <returns>True on success and false otherwise</returns>
        public bool EditInsertField(MapWinGIS.Field NewField, ref int FieldIndex, MapWinGIS.ICallback cBack)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts a new shape in the shapefile.
        /// </summary>
        /// <param name="NewField">A new instance of shape object to insert.</param>
        /// <param name="FieldIndex">A position to insert the new shape. An ivalid index will be automatically substituted with 0 ot numShapes</param>
        /// <param name="cBack">An instance of class implementing ICallback interface. Pass NULL and use Shapefile.GlobalCallback property if needed.</param>
        /// <returns>True on success and false otherwise</returns>
        public bool EditInsertShape(MapWinGIS.Shape Shape, ref int ShapeIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value indicating whether editing operation are allowed for shapefile.
        /// Shapefile.EditInsertShape, Shapefile.EditDeleteShape, Shapefile.EditClear are affected by this property.
        /// Use Shapefile.StartEditingShapes and Shapefile.StopEditingShapes to control editing mode.
        /// </summary>
        public bool EditingShapes
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the value indicating whether editing operation are allowed for shapefile attribute table.
        /// EditInsertField, EditDeleteField, EditInsertShape, EditDeleteShape, EditClear methods of shapefile class
        /// are affected by this property.
        /// Use Shapefile.StartEditingShapes and Shapefile.StopEditingShapes to control editing mode.
        /// </summary>
        public bool EditingTable
        {
            get { throw new NotImplementedException(); }
        }

        public void EndPointInShapefile()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new instance of shapefile class with single-part shapes produced from the multi-part shapes of the input shapefile. 
        /// Single input shapes are moved to the output shapefile without changed as well as the values from attribute table.
        /// </summary>
        /// <param name="SelectedOnly">Indicates whether the operation should be applied only to the selected shapes.</param>
        /// <returns>A new instance of Shapefile class with resultant shapes or NULL reference on failure.</returns>
        public MapWinGIS.Shapefile ExplodeShapes(bool SelectedOnly)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new instance of the shapefile class and copies selected shapes of the input shapefile to it.
        /// </summary>
        /// <returns>A new instance of shapefile class with selected shapes or NULL reference on failure</returns>
        public MapWinGIS.Shapefile ExportSelection()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets bounding box which holds all the shapes in the shapefile.
        /// When fast mode is set on, Shapefile.RefreshExtents call is needed to get the correct 
        /// extents after edits where made.
        /// </summary>
        public MapWinGIS.Extents Extents
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets or sets the fast mode for the shapefile. The mode ensures faster drawing but makes the user responsible 
        /// for the refreshing of shapefile extents after editing operation (see Shapefile.RefreshExtents and Shape.RefreshExtents).
        /// It's highly recomended to use the mode for large shapefile.
        /// </summary>
        public bool FastMode
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

        public int FileHandle
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the filename of the file with .shp extention on disk which plays as a source for this instance of Shapefile class.
        /// The property should be used for disk-based shapefiles only (see Shapefile.SourceType property).
        /// </summary>
        public string Filename
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Generates labels for the shapefile based on values of the specified field in attribute table.
        /// </summary>
        /// <param name="FieldIndex">The index of field to take the values from</param>
        /// <param name="Method">The method for calculation of label position.</param>
        /// <param name="LargestPartOnly">Determines whether all the parts of the multi-part shape should be supplied with individual label or only the largest (longest) one</param>
        /// <returns>The number of labels generated. Normally it must be equal to the number of shapes.</returns>
        public int GenerateLabels(int FieldIndex, MapWinGIS.tkLabelPositioning Method, bool LargestPartOnly)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get or sets an instance of GeoProjection class, which holds information about Shapefile coordinate system and projection.
        /// GeoProjection can't be set to NULL (there is always an instance GeoProjection class associated with shapefile).
        /// </summary>
        public MapWinGIS.GeoProjection GeoProjection
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

        /// <summary>
        /// Gets or sets the engine (library) to use for geoprocessing operations.
        /// The following methods are affected by this property: 
        /// TODO: make list
        /// </summary>
        public MapWinGIS.tkGeometryEngine GeometryEngine
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

        /// <summary>
        /// Creates a new instance of shapefile class which holdes the results of per-shape intersection of 2 input shapefiles.
        /// Attribute table of resulting shapefile holds fields from both input shapefile. 
        /// </summary>
        /// <param name="SelectedOnlyOfThis">Indicates whether operation will be applied to the selected shapes of the current shapefile only.</param>
        /// <param name="sf">The second shapefile to perfrom intersection.</param>
        /// <param name="SelectedOnly">Indicates whether operation will be applied to the selected shapes of the current shapefile only.</param>
        /// <param name="fileType">The type of output shapefiles. SHP_NULLSHAPE value should be passed for automatic choosing of type. See remarks.</param>
        /// <param name="cBack">An instance of class implementing ICallback interface. It's recommedded to pass NULL and use Shapefile.GlobalCallback property if needed.</param>
        /// <returns>Reference to the resulting shapefile or NULL reference on failure</returns>
        /// <remarks>Intesection operation can generate shapes of different types. For example, the intersection of 2 polygons can be
        /// a polygon, a polyline, a point or any combination of those. With SHP_NULLSHAPE fileType paramater the most obvious type will be used:
        /// for example, SHP_POLYGON for 2 polygon shapefiles, etc. Specify you own fileType if the default value isn't returning the expected type</remarks>
        public MapWinGIS.Shapefile GetIntersection(bool SelectedOnlyOfThis, MapWinGIS.Shapefile sf, bool SelectedOnly, MapWinGIS.ShpfileType fileType, MapWinGIS.ICallback cBack)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets an instance of class implementing ICallback interface, which handles progress and error messages of the Shapefile class.
        /// It's recommended to set it in case time consuming operation will be used (geoprocessing, generation of labels, etc).
        /// The property is equal to NULL by default.
        /// </summary>
        public MapWinGIS.ICallback GlobalCallback
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

        /// <summary>
        /// Gets the value indicating whether spatial index exists for the shapefile.
        /// The index is stored in <TODO: write formats> files.
        /// The set part of property does nothing. Use Shapefile.CreateSpatialIndex to createa the new index.
        /// </summary>
        public bool HasSpatialIndex
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

        /// <summary>
        /// Inverts selection of the shapefile, i.e. selection state of every shape is changed to the opposite one.
        /// See Shapefile.get_ShapeSelected property.
        /// </summary>
        public void InvertSelection()
        {
            throw new NotImplementedException();
        }

        public bool IsSpatialIndexValid()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A text string associated with shapefile. Any value can be stored by developer in this property.
        /// </summary>
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

        /// <summary>
        /// Gets or sets an instance of Labels class which hold information about text, position and appearance of labels associated with shapefile. 
        /// The property can't be set to NULL (there is always an instance Labels class associated with it).
        /// </summary>
        public MapWinGIS.Labels Labels
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

        /// <summary>
        /// Gets the code of last error which took place inside this instance of Shapefile class.
        /// To retrieve text description of error, use Shapefile.get_ErrorMsg(Shapefile.LastErrorCode).
        /// Check this value if a certaine method has failed.
        /// </summary>
        public int LastErrorCode
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Creates a new instance of shapefile class which holds shapes from 2 input shapefiles. No changes to geometry of individual shapes are made.
        /// Fields and values from attribute values of both input shapefile will be passed to the resuting one.
        /// </summary>
        /// <param name="SelectedOnlyThis">Indicates whether the operation will be applied only to the selected shapes of the current shapefile</param>
        /// <param name="sf">The second shapefile to take shapes from</param>
        /// <param name="SelectedOnly">Indicates whether the operation will be applied only to the selected shapes of the second shapefile</param>
        /// <returns></returns>
        public MapWinGIS.Shapefile Merge(bool SelectedOnlyThis, MapWinGIS.Shapefile sf, bool SelectedOnly)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the value which controls the drawing of small objects on screen. The objects with scaled size less
        /// than this value will be drawn as a single dot. This can noticeably increase perfromance for large shapefiles at full scale.
        /// But with the increase of speed the quality of drawing will be deteriorating. The default value is 1.
        /// </summary>
        public int MinDrawingSize
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

        /// <summary>
        /// Gets the number of fields in attribute table of the shapefile.
        /// </summary>
        public int NumFields
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the number of selected shapes in the shapefile. See Shapefile.ShapeSelected property.
        /// </summary>
        public int NumSelected
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the number of shapes in the shapefile.
        /// </summary>
        public int NumShapes
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Opens shapefile from the disk. Any other shapefile currently opened will be closed wihout saving the changes.
        /// Shapefile.SourceType property will be set to sstDiskBased.
        /// </summary>
        /// <param name="ShapefileName">The name of file with .shp extention to open.</param>
        /// <param name="cBack">A instance of class which implements ICallback interface to report errors and progrees information</param>
        /// <returns>True on success and false otherwise.</returns>
        public bool Open(string ShapefileName, MapWinGIS.ICallback cBack)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns boolean value which indicates whether the given point is located whithin the specified shape.
        /// The operation is applicable for polygon shapefiles only.
        /// </summary>
        /// <param name="ShapeIndex">The index shape (polygon) to perfrom the test</param>
        /// <param name="x">X coordinate of the point</param>
        /// <param name="y">Y coordinate of the point</param>
        /// <returns>True in case the point is located within polygon and false otherwise</returns>
        public bool PointInShape(int ShapeIndex, double x, double y)
        {
            throw new NotImplementedException();
        }


        public int PointInShapefile(double x, double y)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the projection string for shapefile. String in proj4 and ESRI WKT formats are supported.
        /// This property is deprecated, use Shapefile.GeoProjection property instead.
        /// </summary>
        public string Projection
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

        public MapWinGIS.Extents QuickExtents(int ShapeIndex)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Point QuickPoint(int ShapeIndex, int PointIndex)
        {
            throw new NotImplementedException();
        }

        public System.Array QuickPoints(int ShapeIndex, ref int numPoints)
        {
            throw new NotImplementedException();
        }

        public bool RefreshExtents()
        {
            throw new NotImplementedException();
        }

        public bool RefreshShapeExtents(int ShapeId)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Shapefile Reproject(MapWinGIS.GeoProjection newProjection, ref int reprojectedCount)
        {
            throw new NotImplementedException();
        }

        public bool ReprojectInPlace(MapWinGIS.GeoProjection newProjection, ref int reprojectedCount)
        {
            throw new NotImplementedException();
        }

        public bool Resource(string newShpPath)
        {
            throw new NotImplementedException();
        }

        public bool Save(MapWinGIS.ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool SaveAs(string ShapefileName, MapWinGIS.ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public void SelectAll()
        {
            throw new NotImplementedException();
        }

        public bool SelectByShapefile(MapWinGIS.Shapefile sf, MapWinGIS.tkSpatialRelation Relation, bool SelectedOnly, ref object Result, MapWinGIS.ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public void SelectNone()
        {
            throw new NotImplementedException();
        }

        public bool SelectShapes(MapWinGIS.Extents BoundBox, double Tolerance, MapWinGIS.SelectMode SelectMode, ref object Result)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.tkSelectionAppearance SelectionAppearance
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

        public uint SelectionColor
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

        public MapWinGIS.ShapeDrawingOptions SelectionDrawingOptions
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

        public byte SelectionTransparency
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

        

        public MapWinGIS.ShpfileType ShapefileType
        {
            get { throw new NotImplementedException(); }
        }

        public MapWinGIS.Shapefile Sort(int FieldIndex, bool Ascending)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.tkShapefileSourceType SourceType
        {
            get { throw new NotImplementedException(); }
        }

        public double SpatialIndexMaxAreaPercent
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

        public bool StartEditingShapes(bool StartEditTable, MapWinGIS.ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool StartEditingTable(MapWinGIS.ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool StopEditingShapes(bool ApplyChanges, bool StopEditTable, MapWinGIS.ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool StopEditingTable(bool ApplyChanges, MapWinGIS.ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.IStopExecution StopExecution
        {
            set { throw new NotImplementedException(); }
        }

        public MapWinGIS.Shapefile SymmDifference(bool SelectedOnlySubject, MapWinGIS.Shapefile sfOverlay, bool SelectedOnlyOverlay)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Table Table
        {
            get { throw new NotImplementedException(); }
        }

        public MapWinGIS.Shapefile Union(bool SelectedOnlySubject, MapWinGIS.Shapefile sfOverlay, bool SelectedOnlyOverlay)
        {
            throw new NotImplementedException();
        }

        public bool UseQTree
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

        public bool UseSpatialIndex
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

        public string VisibilityExpression
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

        public bool get_CanUseSpatialIndex(MapWinGIS.Extents pArea)
        {
            throw new NotImplementedException();
        }

        public object get_CellValue(int FieldIndex, int ShapeIndex)
        {
            throw new NotImplementedException();
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Field get_Field(int FieldIndex)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Field get_FieldByName(string Fieldname)
        {
            throw new NotImplementedException();
        }

        public MapWinGIS.Shape get_Shape(int ShapeIndex)
        {
            throw new NotImplementedException();
        }

        public int get_ShapeCategory(int ShapeIndex)
        {
            throw new NotImplementedException();
        }

        public bool get_ShapeSelected(int ShapeIndex)
        {
            throw new NotImplementedException();
        }

        public int get_numPoints(int ShapeIndex)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeCategory(int ShapeIndex, int pVal)
        {
            throw new NotImplementedException();
        }

        public void set_ShapeSelected(int ShapeIndex, bool pVal)
        {
            throw new NotImplementedException();
        }

        public bool FixUpShapes(out Shapefile retval)
        {
            throw new NotImplementedException();
        }

        public Shapefile SimplifyLines(double Tolerance, bool SelectedOnly)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}


