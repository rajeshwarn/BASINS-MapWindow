using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Toolbox
{
    /// <summary>
    /// Instead of a plugin interface being required, this extends the ocx capabilities
    /// to allow some activities that normally would only work through a plugin
    /// </summary>
    public class Toolbox
    {

        #region Variables
       
        AxMapWinGIS.AxMap m_Map; // A local pointer to the map this toolbox works with
       
        
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new toolbox that is tuned to a specific ActiveX Map
        /// </summary>
        /// <param name="Map">This specifies an ActiveX Map for the tools to work on.</param>
        public Toolbox(AxMapWinGIS.AxMap Map)
        {
            m_Map = Map; // Set the local copy
        }

        #endregion


        /// <summary>
        /// This is specifically designed
        /// </summary>
        public void Buffer()
        {

        }
        

        /// <summary>
        /// Launches a GeoProcDialog with Union Options for a Non-Static instance
        /// </summary>
        public void Union()
        {
            MapWinGeoProc.Dialogs.GeoProcDialog GPD = new MapWinGeoProc.Dialogs.GeoProcDialog();
            
            string[] LayerNames = new string[m_Map.NumLayers];
            int NumShapeLayers = 0;
            for (int lyr = 0; lyr < m_Map.NumLayers; lyr++)
            {
                // Only consider Shapefile Layers for the Union operation
                object ob = m_Map.get_GetObject(lyr);
                if (ob.GetType() != typeof(MapWinGIS.Shapefile)) continue;

                string name = m_Map.get_LayerName(lyr);
                if (name != null)
                {
                    LayerNames[lyr] = name;
                }
                else
                {
                    MapWinGIS.Shapefile sf = ob as MapWinGIS.Shapefile;
                    LayerNames[lyr] = sf.Filename;
                }
                NumShapeLayers++;
            }
            
            
            // First Input Layer or File
            MapWinGeoProc.Dialogs.LayerFileElement LF1 = GPD.Add_LayerFileElement(MapWinGeoProc.Dialogs.GeoProcDialog.ElementTypes.OpenShapefile);
            LF1.Caption = "Shapefile or Shape Layer Input";
            if (NumShapeLayers > 0)
            {

                // They can't use selected shapes unless this is a layer
                LF1.LayerNames = LayerNames;
                MapWinGeoProc.Dialogs.BooleanElement BE = GPD.Add_BooleanElement();
                // If the name is not a layer name, then this will be disabled
                LF1.LayerOnlyBoolElements.Add(BE);
                BE.Text = "Use Selected Shapes";
            }
            
            // Second Input Layer or File
            MapWinGeoProc.Dialogs.LayerFileElement LF2 = GPD.Add_LayerFileElement(MapWinGeoProc.Dialogs.GeoProcDialog.ElementTypes.OpenShapefile);
            if (NumShapeLayers > 0)
            {

                // They can't use selected shapes unless this is a layer
                LF2.LayerNames = LayerNames;
                MapWinGeoProc.Dialogs.BooleanElement BE2 = GPD.Add_BooleanElement();
                // If the name is not a layer name, then this will be disabled
                LF2.LayerOnlyBoolElements.Add(BE2);
                BE2.Text = "Use Selected Shapes";
            }
            
            MapWinGeoProc.Dialogs.FileElement Fout = GPD.Add_FileElement(MapWinGeoProc.Dialogs.GeoProcDialog.ElementTypes.SaveShapefile);

            

            GPD.ShowDialog();
        }

    }
}
