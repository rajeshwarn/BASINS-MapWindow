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
//1/29/2005 - Code is identical to public domain version.
//********************************************************************************************************
using System;

namespace ShapefileEditor
{
    /// <summary>
    /// Summary description for AddShapefileClass.
    /// </summary>
    public class AddShapefileClass
    {
        private GlobalFunctions m_globals;

        public AddShapefileClass(GlobalFunctions g)
        {
            //
            // TODO: Add constructor logic here
            //
            m_globals = g;
            m_globals.Events.AddHandler(new MapWindow.Events.ItemClickedEvent(ItemClicked));
        }

        private void ItemClicked(string name, ref bool handled)
        {
            if (name == GlobalFunctions.c_NewButton)
                AddNewShapefile();
        }

        /// <summary>
        /// Prompts the user for information needed to create a new shapefile, then creates it.
        /// </summary>
        public void AddNewShapefile()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(AddShapefileForm));
            using (var dlg = new AddShapefileForm())
            {
                if (dlg.ShowDialog(this.m_globals.MapWindowForm) != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                MapWinGIS.Shapefile sf = new MapWinGIS.ShapefileClass();

                // mmj 09.03.2010: Changes needed by internationalization:
                //  dlg.cmbType.Text returns language specific text, so the select case with 
                //  const ("Point", "Line", "Polygon") only works if English is selected language
                
                // Paul Meems - 17 Oct 2011, New shapefiles are already in edit mode:
                // sf.StartEditingShapes(true, null);
                if (dlg.cmbType.Text == resources.GetString("cmbType.Items"))
                {
                    if (!sf.CreateNewWithShapeID(dlg.txtFilename.Text, MapWinGIS.ShpfileType.SHP_POINT))
                    {
                        MapWinUtility.Logger.Message(
                            "Cannot create new shapefile:  " + sf.get_ErrorMsg(sf.LastErrorCode),
                            "Important Information!",
                            System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Information,
                            System.Windows.Forms.DialogResult.OK);
                        return;
                    }
                }
                else
                {
                    if (dlg.cmbType.Text == resources.GetString("cmbType.Items1"))
                    {
                        if (!sf.CreateNewWithShapeID(dlg.txtFilename.Text, MapWinGIS.ShpfileType.SHP_POLYLINE))
                        {
                            MapWinUtility.Logger.Message(
                                "Cannot create new shapefile:  " + sf.get_ErrorMsg(sf.LastErrorCode),
                                "Important Information!",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information,
                                System.Windows.Forms.DialogResult.OK);
                            return;
                        }
                    }
                    else if (dlg.cmbType.Text == resources.GetString("cmbType.Items2"))
                    {
                        if (!sf.CreateNewWithShapeID(dlg.txtFilename.Text, MapWinGIS.ShpfileType.SHP_POLYGON))
                        {
                            MapWinUtility.Logger.Message(
                                "Cannot create new shapefile:  " + sf.get_ErrorMsg(sf.LastErrorCode),
                                "Important Information!",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information,
                                System.Windows.Forms.DialogResult.OK);
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                // Paul Meems - 17 Oct 2011, The file will be saved again in the next few lines:
                // sf.StopEditingShapes(true, true, null);

                // Paul Meems, 3 Oct. 2011 Add projection:
                if (!string.IsNullOrEmpty(this.m_globals.MapWin.Project.ProjectProjection))
                {
                    sf.Projection = this.m_globals.MapWin.Project.ProjectProjection;
                }

                if (!sf.SaveAs(dlg.txtFilename.Text, null))
                {
                    MapWinUtility.Logger.Message(
                        "Could not save the new shapefile: " + sf.get_ErrorMsg(sf.LastErrorCode),
                        "Important Information!",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Information,
                        System.Windows.Forms.DialogResult.OK);
                    return;
                }

                sf.Close();
                this.m_globals.MapWin.Layers.Add(dlg.txtFilename.Text);
                MapWinUtility.Logger.Message(
                    "An empty shapefile has been created. Before adding shapes, make sure that your extents are set properly. To make sure extents are correct load an image, grid or shapefile that is located in the same area.",
                    "Important Information!",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information,
                    System.Windows.Forms.DialogResult.OK);
            }
        }
    }
}
