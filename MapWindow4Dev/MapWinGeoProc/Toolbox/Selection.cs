using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MapWinGeoProc.Toolbox
{
    class Selection
    {
        #region Variables
        AxMapWinGIS.AxMap m_Map;
        
        // Appearance
        System.Drawing.Color m_SelectionColor; // Default Cyan
        int m_SelectionLineThickness; // Default = 3
        bool m_ShowSelection; // Defualt = true
        // Layer information
        int m_CurrentLayer;
        List<int>[] m_SelectedShapes; // One list for each layer;
        Hashtable[] m_OriginalThickness; // Stores the original line thicknesses
        Hashtable[] m_OriginalLineColor; // Stores the original line color
        Hashtable[] m_OriginalFillColor; // Stores the original color
        Hashtable[] m_OriginalPointColor; // Stores the original point color
       
        // Mouse Handling 
        System.Drawing.Point m_MouseDownLoc;
        bool m_ShiftPressed;
        MapWinGIS.SelectMode m_SelectionMode;


        #endregion
        
        #region Constructor

        /// <summary>
        /// Creates a new instance of the selection object tuned to a specific map
        /// </summary>
        /// <param name="Map"></param>
        public Selection(AxMapWinGIS.AxMap Map)
        {
            m_Map = Map;

            // Appearance
            m_SelectionColor = System.Drawing.Color.Cyan;
            m_SelectionLineThickness = 3;
            m_ShowSelection = true;
            m_SelectionMode = MapWinGIS.SelectMode.INTERSECTION;
            // No current layer assumes that we should select shapes from all layers
            m_CurrentLayer = -1;
            m_Map.MouseDownEvent += new AxMapWinGIS._DMapEvents_MouseDownEventHandler(m_Map_MouseDownEvent);
            m_Map.MouseUpEvent += new AxMapWinGIS._DMapEvents_MouseUpEventHandler(m_Map_MouseUpEvent);
            m_Map.SelectBoxFinal += new AxMapWinGIS._DMapEvents_SelectBoxFinalEventHandler(m_Map_SelectBoxFinal);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color to draw the selection with.  The default is cyan.
        /// </summary>
        public System.Drawing.Color SelectionColor
        {
            get
            {
                return m_SelectionColor;
            }
            set
            {
                m_SelectionColor = value;
            }
        }
        /// <summary>
        /// Gets or sets the thickness of selected lines
        /// </summary>
        public int SelectionLineThickness
        {
            get
            {
                return m_SelectionLineThickness;
            }
            set
            {
                m_SelectionLineThickness = value;
            }
        }
        /// <summary>
        /// Gets or Sets whether the selection should be indicated by changes in color or line thickness.
        /// </summary>
        public bool ShowSelection
        {
            get
            {
                return m_ShowSelection;
            }
            set
            {
                m_ShowSelection = value;
            }
        }
        /// <summary>
        /// Gets or sets the selection mode.  Options are Intersection and Inclusion
        /// </summary>
        public MapWinGIS.SelectMode SelectionMode
        {
            get
            {
                return m_SelectionMode;
            }
            set
            {
                m_SelectionMode = value;
            }
        }

        /// <summary>
        /// Gets or Sets a separate list of selected shape indecies for each layer in the map.
        /// </summary>
        public List<int>[] SelectedShapes
        {
            get
            {
                return m_SelectedShapes;
            }
            set
            {
                if (value.GetUpperBound(0) >= m_Map.NumLayers)
                {
                    throw new ArgumentException("There were more layers specified in the selection than exist in the map.");
                }
                
                m_SelectedShapes = new List<int>[m_Map.NumLayers];
                // We create a selection for each layer regardless of what they send us.
                for (int i = 0; i < m_Map.NumLayers; i++)
                {
                    if (value != null)
                    {
                        if (value[i] != null)
                        {
                            if (i <= value.GetUpperBound(0))
                            {
                                m_SelectedShapes[i] = value[i];
                                continue;
                            }
                        }
                    }
                    m_SelectedShapes[i] = new List<int>();
                }
            }
        }

        #endregion

        /// <summary>
        /// Clears the values in the selection for all layers
        /// </summary>
        public void ClearSelectedShapes()
        {

            for (int lyr = 0; lyr < m_SelectedShapes.GetUpperBound(0); lyr++)
            {
                // Return the drawing to the original colors
                Restore_Layer(lyr);

                m_SelectedShapes[lyr].Clear();
            }
        }

        /// <summary>
        /// Clears the shapes selected from a specific layer
        /// </summary>
        /// <param name="Layer"></param>
        public void ClearSelectedShapes(int Layer)
        {
            if (Layer > m_SelectedShapes.GetUpperBound(0))
            {
                throw new ArgumentException("The Layer index specified was greater than the number of layers in the current selection.");
            }
            m_SelectedShapes[Layer].Clear();
        }

        /// <summary>
        /// Returns the selected shapes for a specific layer
        /// </summary>
        /// <param name="Layer"></param>
        /// <returns></returns>
        public List<int> GetSelectedShapes(int Layer)
        {
            return m_SelectedShapes[Layer];
        }

        /// <summary>
        /// Sets the selection for a specific layer.
        /// </summary>
        /// <param name="Layer">The Layer index to set the selection for</param>
        /// <param name="Shapes">An array of integer shape values</param>
        void SetSelectedShapes(int Layer, int[] Shapes)
        {
            // the reason for passing an integer is to match with the "SelectShapes" function on axMap
            m_SelectedShapes[Layer] = new List<int>();
            for (int i = 0; i <= Shapes.GetUpperBound(0); i++)
            {
                m_SelectedShapes[Layer].Add(Shapes[i]);
            }
        }

        /// <summary>
        /// Only starts selecting shapes for a single layer
        /// </summary>
        /// <param name="Layer">The layer index for the map</param>
        public void StartSelectingShapes(int Layer)
        {
            m_Map.CursorMode = MapWinGIS.tkCursorMode.cmSelection;
            m_Map.SendMouseDown = true;
            m_Map.SendMouseUp = true;
            m_Map.SendSelectBoxFinal = true;
            m_CurrentLayer = Layer;
        }

        /// <summary>
        /// Selects shapes for all layers
        /// </summary>
        public void StartSelectingShapes()
        {
            m_Map.CursorMode = MapWinGIS.tkCursorMode.cmSelection;
            m_Map.SendMouseDown = true;   
            m_Map.SendMouseUp = true;
            m_Map.SendSelectBoxFinal = true;
        }

        /// <summary>
        /// This does not restore anything, but simply deactivates the mouse responsiveness
        /// </summary>
        public void StopSelectingShapes()
        {
            m_Map.SendMouseDown = false;
            m_Map.SendMouseUp = false;
            m_Map.SendSelectBoxFinal = false;
        }

        #region Event handlers
        
        
        void m_Map_MouseDownEvent(object sender, AxMapWinGIS._DMapEvents_MouseDownEvent e)
        {
            m_MouseDownLoc = new System.Drawing.Point(e.x, e.y);
            if (e.shift == 1)
            {
                m_ShiftPressed = true;
            }
            else
            {
                m_ShiftPressed = false;
            }
        }

        // This is the full blown select for a drag style extent
        void m_Map_SelectBoxFinal(object sender, AxMapWinGIS._DMapEvents_SelectBoxFinalEvent e)
        {
            // First get the projected coordinates
            double top = 0;
            double left = 0;
            double bottom = 0;
            double right = 0;
            m_Map.PixelToProj(e.left, e.top, ref left, ref top);
            m_Map.PixelToProj(e.right, e.bottom, ref right, ref bottom);

            MapWinGIS.Extents ext = new MapWinGIS.Extents();
            ext.SetBounds(left, bottom, 0, right, top, 0);
            SelectExtents(ext);
        }

        void m_Map_MouseUpEvent(object sender, AxMapWinGIS._DMapEvents_MouseUpEvent e)
        {
            // Select using an approximation of 5 pixels about e
            double top = 0;
            double left = 0;
            double bottom = 0;
            double right = 0;
            m_Map.PixelToProj(e.x - 5, e.y - 5, ref left, ref top);
            m_Map.PixelToProj(e.x + 5, e.y + 5, ref right, ref bottom);

            MapWinGIS.Extents ext = new MapWinGIS.Extents();
            ext.SetBounds(left, bottom, 0, right, top, 0);
            SelectExtents(ext);
        }

        private void SelectExtents(MapWinGIS.Extents ext)
        {
            object[] Shapes = new object[m_Map.NumLayers];
            if (m_CurrentLayer == -1 || m_CurrentLayer >= m_Map.NumLayers)
            {
                for (int lyr = 0; lyr < m_Map.NumLayers; lyr++)
                {
                    object ob = m_Map.get_GetObject(lyr);
                    if (ob.GetType() != typeof(MapWinGIS.Shapefile)) return;
                    MapWinGIS.Shapefile sf = ob as MapWinGIS.Shapefile;
                    sf.SelectShapes(ext, 0, MapWinGIS.SelectMode.INTERSECTION, ref Shapes[m_CurrentLayer]);

                }
            }
            else
            {
                object ob = m_Map.get_GetObject(m_CurrentLayer);
                if (ob.GetType() != typeof(MapWinGIS.Shapefile)) return;
                MapWinGIS.Shapefile sf = ob as MapWinGIS.Shapefile;
                sf.SelectShapes(ext, 0, MapWinGIS.SelectMode.INTERSECTION, ref Shapes[m_CurrentLayer]);
            }

            m_Map.SuspendLayout();
            // If shift is down then we append to the selection
            if (m_ShiftPressed == false)
            {
                // Clear the selection first

                ClearSelectedShapes();

            }

            //Append selected shapes
            for (int lyr = 0; lyr < m_Map.NumLayers; lyr++)
            {
                int[] myShapes = Shapes[lyr] as int[];
                for (int shp = 0; shp <= myShapes.GetUpperBound(0); shp++)
                {
                    if (m_SelectedShapes[lyr].Contains(myShapes[shp])) continue;
                    m_SelectedShapes[lyr].Add(myShapes[shp]);
                }
            }

            for (int lyr = 0; lyr < m_SelectedShapes.GetUpperBound(0); lyr++)
            {
                Highlight_Layer(lyr);
            }

            m_Map.ResumeLayout();
        }

        

       

        #endregion

        #region Coloration


        /// <summary>
        /// Use this function to update the entire map to be consistent with the current selection.
        /// </summary>
        public void Highlight_Selection()
        {
            m_Map.SuspendLayout(); // Don't actually draw the changes until we are done
            // Revert everything back to the original color so that we can start over
            
            for (int lyr = 0; lyr < m_SelectedShapes.GetUpperBound(0); lyr++)
            {
                Restore_Layer(lyr);

                Highlight_Layer(lyr);
            }

            m_Map.ResumeLayout();
        }

        /// <summary>
        /// This handles the coloring aspects of un-selecting all the shapes in a layer
        /// </summary>
        public void Restore_Layer(int Layer)
        {
            IDictionaryEnumerator Enumerator;
            object ob = m_Map.get_GetObject(Layer);
            if (ob.GetType() != typeof(MapWinGIS.Shapefile))return;
            
            MapWinGIS.Shapefile sf = ob as MapWinGIS.Shapefile;
            if (sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYGON ||
                sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYGONM ||
                sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
            {
                
                Enumerator =  m_OriginalFillColor[Layer].GetEnumerator();
                while (Enumerator.MoveNext())
                {
                    m_Map.set_ShapeFillColor(Layer, (int)Enumerator.Key, (uint)Enumerator.Value);
                }

                Enumerator = m_OriginalLineColor[Layer].GetEnumerator();
                while (Enumerator.MoveNext())
                {
                    m_Map.set_ShapeLineColor(Layer, (int)Enumerator.Key, (uint)Enumerator.Value);
                }
            }
            else if (sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYLINE ||
                     sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYLINEM ||
                     sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
            {

                Enumerator = m_OriginalLineColor[Layer].GetEnumerator();
                while (Enumerator.MoveNext())
                {
                    m_Map.set_ShapeLineColor(Layer, (int)Enumerator.Key, (uint)Enumerator.Value);
                }
                Enumerator = m_OriginalThickness[Layer].GetEnumerator();
                while (Enumerator.MoveNext())
                {
                    m_Map.set_ShapeLineWidth(Layer, (int)Enumerator.Key, (float)Enumerator.Value);
                }
               
            }
            else if (sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POINT ||
               sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POINTM ||
               sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POINTZ ||
               sf.ShapefileType == MapWinGIS.ShpfileType.SHP_MULTIPOINT ||
               sf.ShapefileType == MapWinGIS.ShpfileType.SHP_MULTIPOINTM ||
               sf.ShapefileType == MapWinGIS.ShpfileType.SHP_MULTIPOINTZ)
            {
                Enumerator = m_OriginalThickness[Layer].GetEnumerator();
                while (Enumerator.MoveNext())
                {
                    m_Map.set_ShapeLineWidth(Layer, (int)Enumerator.Key, (float)Enumerator.Value);
                }
                Enumerator = m_OriginalPointColor[Layer].GetEnumerator();
                while (Enumerator.MoveNext())
                {
                    m_Map.set_ShapePointColor(Layer, (int)Enumerator.Key, (uint)Enumerator.Value);
                }
            }
            
        }

        /// <summary>
        /// This handles the coloring aspects of selecting all the shapes in a shapefile layer
        /// </summary>
        public void Highlight_Layer(int Layer)
        {
            object ob = m_Map.get_GetObject(Layer);
            if (ob.GetType() != typeof(MapWinGIS.Shapefile)) return; // Don't bother selecting images

            int numLayers = m_Map.NumLayers;
            m_OriginalThickness = new Hashtable[numLayers];
            m_OriginalLineColor = new Hashtable[numLayers];
            m_OriginalPointColor = new Hashtable[numLayers];
            m_OriginalFillColor = new Hashtable[numLayers];

            UInt32 SelCol = Convert.ToUInt32(m_SelectionColor.B) * 256*256 + 
                            Convert.ToUInt32(m_SelectionColor.G) * 256 +
                            Convert.ToUInt32(m_SelectionColor.R);

            MapWinGIS.Shapefile sf = ob as MapWinGIS.Shapefile;
            if (sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYGON ||
                sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYGONM ||
                sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
            {
                for (int shp = 0; shp < m_SelectedShapes[Layer].Count; shp++)
                {
                    int Shape = m_SelectedShapes[Layer][shp];
                    m_OriginalFillColor[Layer].Add(Shape, m_Map.get_ShapeFillColor(Layer, Shape));
                    m_Map.set_ShapeFillColor(Layer, Shape, SelCol);
                    m_OriginalLineColor[Layer].Add(Shape, m_Map.get_ShapeLineColor(Layer, Shape));
                    m_Map.set_ShapeLineColor(Layer, Shape, SelCol);
                }
            }
            else if (sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYLINE ||
                     sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYLINEM ||
                     sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
            {
                for (int shp = 0; shp < m_SelectedShapes[Layer].Count; shp++)
                {
                    int Shape = m_SelectedShapes[Layer][shp];
                    m_OriginalThickness[Layer].Add(Shape, m_Map.get_ShapeLineWidth(Layer, Shape));
                    m_Map.set_ShapeLineWidth(Layer, Shape, 3);
                    m_OriginalLineColor[Layer].Add(Shape, m_Map.get_ShapeLineColor(Layer, Shape));
                    m_Map.set_ShapeLineColor(Layer, Shape, SelCol);
                }
            }
            else if (sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POINT ||
               sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POINTM ||
               sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POINTZ ||
               sf.ShapefileType == MapWinGIS.ShpfileType.SHP_MULTIPOINT ||
               sf.ShapefileType == MapWinGIS.ShpfileType.SHP_MULTIPOINTM ||
               sf.ShapefileType == MapWinGIS.ShpfileType.SHP_MULTIPOINTZ)
            {
                for (int shp = 0; shp < m_SelectedShapes[Layer].Count; shp++)
                {
                    int Shape = m_SelectedShapes[Layer][shp];
                    float Size = m_Map.get_ShapePointSize(Layer, Shape);
                    m_OriginalThickness[Layer].Add(Shape, Size);
                    m_Map.set_ShapePointSize(Layer, Shape, Size + 2);
                    m_OriginalPointColor[Layer].Add(Shape, m_Map.get_ShapePointColor(Layer, Shape));
                    m_Map.set_ShapePointColor(Layer, Shape, SelCol);
                }
            }

        }

        #endregion
    }
}
