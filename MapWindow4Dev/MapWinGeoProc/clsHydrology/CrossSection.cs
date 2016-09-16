using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;
using MapWinGeoProc.Topology2D;

namespace MapWinGeoProc
{
    /// <summary>
    /// This class stores the functions dealing with cross sections of elevation grids.
    /// </summary>
    public static class CrossSection
    {
        /// <summary>
        /// Contains options for the CrossSection function
        /// </summary>
        public enum CrossSectionTypes
        {
            /// <summary>
            /// Specifies the output should be a polyline with z values for each vertex
            /// </summary>
            PolyLineWithZ,
            /// <summary>
            /// Specifies the output should be a series of points with both Z valued elevation and an additional elevation field.
            /// </summary>
            PointsWithZAndElevField
        }

        /// <summary>
        /// This overload of GetCrossSection launches a dialog to obtain the required parameters from the user.
        /// </summary>
        public static void GetCrossSection()
        {
            MapWinUtility.Logger.Dbg("GetCrossSection()");
            CrossSectionTypes CrossSectionType;
            Dialogs.GeoProcDialog myDialog = new MapWinGeoProc.Dialogs.GeoProcDialog();
            Dialogs.FileElement grid = myDialog.Add_FileElement(MapWinGeoProc.Dialogs.GeoProcDialog.ElementTypes.OpenGridFile);
            Dialogs.FileElement shapefile = myDialog.Add_FileElement(MapWinGeoProc.Dialogs.GeoProcDialog.ElementTypes.OpenShapefile);
            Dialogs.FileElement outfile = myDialog.Add_FileElement(MapWinGeoProc.Dialogs.GeoProcDialog.ElementTypes.SaveShapefile);
            Dialogs.BooleanElement points = myDialog.Add_BooleanElement();
            grid.Caption = "Elevation Grid File name";
            shapefile.Caption = "Cross-Section Input Polyline Shapefile";
            outfile.Caption = "Cross-Section Output Shapefile name";
            points.Caption = "Output cross sections as points";
            myDialog.DialogHelpText = "This function will use a vector polyline shapefile to determine elevations as cross sections.";
            myDialog.DialogHelpTitle = "Get Cross Section";
            try
            {
                //ResourceManager rm = new ResourceManager("clsHydrology.CrossSection", Assembly.GetExecutingAssembly());
                //System.Drawing.Bitmap b = (System.Drawing.Bitmap)rm.GetObject("CrossSectionHelp");
                
                //myDialog.DialogHelpImage = (System.Drawing.Image)b;
                //myDialog.HelpImage = (System.Drawing.Image)b;
            }
            catch(Exception ex)
            {
                MapWinUtility.Logger.Message(ex.Message, "Exception Thrown", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            myDialog.ShowDialog();
            if (myDialog.DialogResult != System.Windows.Forms.DialogResult.OK) return;
            CrossSectionType = CrossSectionTypes.PolyLineWithZ;
            if(points.Value == true)CrossSectionType = CrossSectionTypes.PointsWithZAndElevField;
            GetCrossSection(grid.Filename, shapefile.Filename, outfile.Filename, CrossSectionType);
            MapWinUtility.Logger.Dbg("Finished GetCrossSection");
        }

        /// <summary>
        /// This creates a new shapefile that has Z values and follows along the same line segments.
        /// The boundaries for grid cells are marked with vertices and the segment is given a Z value
        /// that corresponds to the grid elevation it intersects.
        /// </summary>
        /// <param name="ElevGrid">A string filename for the grid that contains the elevations.</param>
        /// <param name="PolyLine">A string filename for a polyline shapefile that shows the pathways of the cross sections in the X-Y direction.</param>
        /// <param name="OutFileName">A string containing the full path of the desired output shapefile.  The extension should be *.shp</param>
        /// <param name="CrossSectionType">Clarifies the type of output</param>
        /// <remarks>This function throws Argument or Application exceptions on errors, so it's recommended that coders enclose it in a try catch block.</remarks>
        public static void GetCrossSection(string ElevGrid, string PolyLine, string OutFileName, CrossSectionTypes CrossSectionType)
        {
            GetCrossSection(ElevGrid, PolyLine, OutFileName, CrossSectionType, null);
        }

        /// <summary>
        /// This creates a new shapefile that has Z values and follows along the same line segments.
        /// The boundaries for grid cells are marked with vertices and the segment is given a Z value
        /// that corresponds to the grid elevation it intersects.
        /// </summary>
        /// <param name="ElevGrid">A string filename for the grid that contains the elevations.</param>
        /// <param name="PolyLine">A string filename for a polyline shapefile that shows the pathways of the cross sections in the X-Y direction.</param>
        /// <param name="OutFileName">A string containing the full path of the desired output shapefile.  The extension should be *.shp</param>
        /// <param name="CrossSectionType">Clarifies the type of output</param>
        /// <param name="ICallBack">A MapWinGIS.ICallback for progress messages. [Optional]</param>
        /// <remarks>This function throws Argument or Application exceptions on errors, so it's recommended that coders enclose it in a try catch block.</remarks>
        public static void GetCrossSection(string ElevGrid, string PolyLine, string OutFileName, CrossSectionTypes CrossSectionType, MapWinGIS.ICallback ICallBack)
        {
            bool res;

            // Load the grid 
            if (ElevGrid == null) throw new ArgumentException("ElevGrid cannot be null.");
            if (System.IO.File.Exists(ElevGrid) == false) throw new ArgumentException("The file " + ElevGrid + " does not exist.");
            MapWinGIS.Grid mwGrid = new MapWinGIS.Grid();
            res = mwGrid.Open(ElevGrid, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, ICallBack);
            if (res == false) throw new ApplicationException(mwGrid.get_ErrorMsg(mwGrid.LastErrorCode));

            // Load the Shapefile
            if (PolyLine == null) throw new ArgumentException("PolyLine cannot be null.");
            if (System.IO.File.Exists(PolyLine) == false) throw new ArgumentException("The file " + PolyLine + " does not exist.");
            MapWinGIS.Shapefile mwPolyLine = new MapWinGIS.Shapefile();
            res = mwPolyLine.Open(PolyLine, ICallBack);
            if (res == false) throw new ApplicationException(mwPolyLine.get_ErrorMsg(mwPolyLine.LastErrorCode));

            GetCrossSection(mwGrid, mwPolyLine, OutFileName, CrossSectionType, ICallBack);
        }

        /// <summary>
        /// This creates a new shapefile that has Z values and follows along the same line segments.
        /// The boundaries for grid cells are marked with vertices and the segment is given a Z value
        /// that corresponds to the grid elevation it intersects.
        /// </summary>
        /// <param name="mwElevGrid">A MapWinGIS Grid that contains the elevations.</param>
        /// <param name="mwPolyLine">A MapWinGIS Shapefile that shows the pathways of the cross sections in the X-Y direction.</param>
        /// <param name="OutFileName">A string containing the full path of the desired output shapefile.  The extension should be *.shp</param>
        /// <param name="CrossSectionType">Clarifies the type of output</param>
        /// <remarks>This function throws Argument or Application exceptions on errors, so it's recommended that coders enclose it in a try catch block.</remarks>
        public static void GetCrossSection(MapWinGIS.Grid mwElevGrid, MapWinGIS.Shapefile mwPolyLine, string OutFileName, CrossSectionTypes CrossSectionType)
        {
            GetCrossSection(mwElevGrid, mwPolyLine, OutFileName, CrossSectionType, null);
        }

        /// <summary>
        /// This creates a new shapefile that has Z values and follows along the same line segments.
        /// The boundaries for grid cells are marked with vertices and the segment is given a Z value
        /// that corresponds to the grid elevation it intersects.
        /// </summary>
        /// <param name="mwElevGrid">A MapWinGIS Grid that contains the elevations.</param>
        /// <param name="mwPolyLine">A MapWinGIS Shapefile that shows the pathways of the cross sections in the X-Y direction.</param>
        /// <param name="OutFileName">A string containing the full path of the desired output shapefile.  The extension should be *.shp</param>
        /// <param name="CrossSectionType">Clarifies the type of output.  default = PolyLineWithZ</param>
        /// <param name="ICallBack">A MapWinGIS.ICallback for progress messages. [Optional]</param>
        /// <remarks>This function throws Argument or Application exceptions on errors, so it's recommended that coders enclose it in a try catch block.</remarks>
        public static void GetCrossSection(MapWinGIS.Grid mwElevGrid, MapWinGIS.Shapefile mwPolyLine, string OutFileName, CrossSectionTypes CrossSectionType, MapWinGIS.ICallback ICallBack)
        {
            MapWinUtility.Logger.Dbg("GetCrossSection(mwElevGrid: " + Macro.ParamName(mwElevGrid) + ",\n" +
                                     "                mwPolyLine: " + Macro.ParamName(mwPolyLine) + ",\n" +
                                     "                OutFileName: " + OutFileName + ",\n" +
                                     "                CrossSectionType: " + CrossSectionType.ToString() + ",\n" +
                                     "                ICallback)");
            bool res;
            int Prog = 0;
            int OldProg = 0;
            double dS, dX, dY, XllCenter, YllCenter;
            int NumRows, NumCols, ElevField, IDField;
            ElevField = 1;
            IDField = 0;
            // Test to be sure that the elevation grid and shapefile are not null
            if (mwElevGrid == null)
            {
                MapWinUtility.Logger.Dbg("Argument Exception: Elevation grid mwElevGrid can't be null.");
                throw new ArgumentException("Elevation grid mwElevGrid can't be null.");
            }
            if (mwPolyLine == null)
            {
                MapWinUtility.Logger.Dbg("Argument Exception: The shapefile of input cross sections mwPolyLine can't be null.");
                throw new ArgumentException("The shapefile of input cross sections mwPolyLine can't be null.");
            }

            // Clear any existing shapefile output filenames that might cause problems if they exist.
            string fn;
            if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(OutFileName)) == false)
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(OutFileName));
            }
            if (System.IO.File.Exists(OutFileName)) System.IO.File.Delete(OutFileName);
            fn = System.IO.Path.ChangeExtension(OutFileName, ".dbf");
            if (System.IO.File.Exists(fn)) System.IO.File.Delete(fn);
            fn = System.IO.Path.ChangeExtension(OutFileName, ".shx");
            if (System.IO.File.Exists(fn)) System.IO.File.Delete(fn);
            fn = System.IO.Path.ChangeExtension(OutFileName, ".prj");
            if (System.IO.File.Exists(fn)) System.IO.File.Delete(fn);
            // Since we are given the lower left coordinate, just make sure dX and dY are positive
            dX = Math.Abs(mwElevGrid.Header.dX);
            if (dX == 0) throw new ApplicationException("mwElevGrid.Header.dX cannot be 0.");
            dY = Math.Abs(mwElevGrid.Header.dY);
            if (dY == 0) throw new ApplicationException("mwElevGrid.Header.dY cannot be 0.");

            // Determine the stepping distance from the grid coordintes
            dS = dX / 2;
            if (dY < dX) dS = dY / 2;


            XllCenter = mwElevGrid.Header.XllCenter;
            YllCenter = mwElevGrid.Header.YllCenter;
            NumRows = mwElevGrid.Header.NumberRows;
            NumCols = mwElevGrid.Header.NumberCols;

            // Test for intersection between the entire shapefile and the grid
            double left, right, top, bottom;
            left = XllCenter - dX / 2;
            right = XllCenter + NumCols * dX - dX / 2;
            bottom = YllCenter - dY / 2;
            top = YllCenter + NumRows * dY - dY / 2;
            MapWinGeoProc.Topology2D.Envelope gExt = new MapWinGeoProc.Topology2D.Envelope(left, right, bottom, top);
            MapWinGeoProc.Topology2D.Envelope pExt = new MapWinGeoProc.Topology2D.Envelope(mwPolyLine.Extents);
            if (gExt.Intersects(pExt) == false)
            {
                MapWinUtility.Logger.Dbg("Application Exception: The shapefile doesn't overlap the grid, so no cross sections were found.");
                throw new ApplicationException("The shapefile doesn't overlap the grid, so no cross sections were found.");
            }


            // Setup the output shapefile and the basic shape objects
            MapWinGIS.Shape mwShape;
            MapWinGIS.Shapefile sfOut = new MapWinGIS.Shapefile();
            sfOut.Projection = mwPolyLine.Projection;
            
            if (CrossSectionType == CrossSectionTypes.PointsWithZAndElevField)
            {
                res = sfOut.CreateNew(OutFileName, MapWinGIS.ShpfileType.SHP_POINTZ);
                if (res != true)
                {
                    MapWinUtility.Logger.Dbg("Application Exception: " + sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                    throw new ApplicationException(sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                }
                res = sfOut.StartEditingShapes(true, ICallBack);
                if (res != true)
                {
                    MapWinUtility.Logger.Dbg("Application Exception: " + sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                    throw new ApplicationException(sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                }
                MapWinGIS.Field ID = new MapWinGIS.Field();
                ID.Name = "ID";
                ID.Type = MapWinGIS.FieldType.INTEGER_FIELD;
                res = sfOut.EditInsertField(ID, ref IDField, ICallBack);
                if (res != true)
                {
                    MapWinUtility.Logger.Dbg("Application Exception: " + sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                    throw new ApplicationException(sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                }
                MapWinGIS.Field Elev = new MapWinGIS.Field();
                Elev.Name = "Elevation";
                Elev.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
                res = sfOut.EditInsertField(Elev, ref ElevField, ICallBack);
                if (res != true)
                {
                    MapWinUtility.Logger.Dbg("Application Exception: " + sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                    throw new ApplicationException(sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                }
            }
            else
            {
                res = sfOut.CreateNew(OutFileName, MapWinGIS.ShpfileType.SHP_POLYLINEZ);
                if (res != true)
                {
                    MapWinUtility.Logger.Dbg("Application Exception: " + sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                    throw new ApplicationException(sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                }
                res = sfOut.StartEditingShapes(true, ICallBack);
                if (res != true)
                {
                    MapWinUtility.Logger.Dbg("Application Exception: " + sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                    throw new ApplicationException(sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                }
                MapWinGIS.Field ID = new MapWinGIS.Field();
                ID.Name = "ID";
                ID.Type = MapWinGIS.FieldType.INTEGER_FIELD;
                res = sfOut.EditInsertField(ID, ref IDField, ICallBack);
                if (res != true)
                {
                    MapWinUtility.Logger.Dbg("Application Exception: " + sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                    throw new ApplicationException(sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                }
            }

            

            MapWinGIS.Shape mwOutShape;
            // Loop through all the shapes in the polyline shapefile
            int shIndx = -1;
            for (int shp = 0; shp < mwPolyLine.NumShapes; shp++)
            {

                mwShape = mwPolyLine.get_Shape(shp);

                // By turning multi-part polylines into multiple linestrings, we avoid the annoying multi-part logic
                MultiLineString MLS = GeometryFactory.CreateMultiLineString(mwShape);

                for (int ls = 0; ls < MLS.NumGeometries; ls++)
                {
                    LineString lsSource = MLS.GeometryN[ls] as LineString;
                    for (int pt = 0; pt < lsSource.Coordinates.Count - 1; pt++)
                    {


                        Coordinate start = lsSource.Coordinates[pt];
                        Coordinate end = lsSource.Coordinates[pt + 1];

                        // Crop the parts of each segment that do not overlap with the grid.
                        if (start.X < left)
                        {
                            if (end.X < left)
                            {
                                // this segment is outside the grid
                                continue;
                            }
                            // crop this segment to only the portion on the grid
                            start.X = left;
                        }
                        if (end.X < left)
                        {
                            // crop this segment to only the portion on the grid
                            end.X = left;
                        }
                        if (start.X > right)
                        {
                            if (end.X > right)
                            {
                                // this segment is outside the grid
                                continue;
                            }
                            // crop to grid
                            start.X = right;
                        }
                        if (end.X > right)
                        {
                            // crop to the grid
                            end.X = right;
                        }


                        double length = Math.Sqrt((end.X - start.X) * (end.X - start.X) + (end.Y - start.Y) * (end.Y - start.Y));
                        int NumSteps = (int)Math.Floor(length / dS);
                        double segDx = (end.X - start.X) / NumSteps;
                        double segDy = (end.Y - start.Y) / NumSteps;
                        mwOutShape = new MapWinGIS.Shape();
                        if (CrossSectionType == CrossSectionTypes.PolyLineWithZ)
                        {
                            mwOutShape.Create(MapWinGIS.ShpfileType.SHP_POLYLINEZ);
                        }

                        // step by dS and get the grid value at that point at each step
                        int p = 0;
                        for (int I = 0; I < NumSteps; I++)
                        {
                            int row, col;
                            object Elev;
                            double X = start.X + segDx * I;
                            double Y = start.Y + segDy * I;
                            mwElevGrid.ProjToCell(X, Y, out col, out row);
                            Elev = mwElevGrid.get_Value(col, row);
                            MapWinGIS.Point pnt = new MapWinGIS.Point();
                            pnt.x = X;
                            pnt.y = Y;
                            pnt.Z = (double)Elev;
                            if (CrossSectionType == CrossSectionTypes.PointsWithZAndElevField)
                            {
                                p = 0;
                                mwOutShape = new MapWinGIS.Shape();
                                mwOutShape.Create(MapWinGIS.ShpfileType.SHP_POINTZ);
                                res = mwOutShape.InsertPoint(pnt, ref p);
                                if (res == false) throw new ApplicationException(mwOutShape.get_ErrorMsg(mwOutShape.LastErrorCode));
                                res = sfOut.EditInsertShape(mwOutShape, ref shIndx);
                                if (res != true) throw new ApplicationException(sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                                res = sfOut.EditCellValue(IDField, shIndx, shIndx);
                                if (res != true) throw new ApplicationException(sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                                res = sfOut.EditCellValue(ElevField, shIndx, Elev);
                                if (res != true) throw new ApplicationException(sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                                shIndx++;
                            }
                            else
                            {
                                res = mwOutShape.InsertPoint(pnt, ref p);
                                p++;
                                if (res == false) throw new ApplicationException(mwOutShape.get_ErrorMsg(mwOutShape.LastErrorCode));
                            }
                        }
                        if (CrossSectionType == CrossSectionTypes.PolyLineWithZ)
                        {
                            if (mwOutShape.numPoints > 0)
                            {

                                res = sfOut.EditInsertShape(mwOutShape, ref shIndx);
                                if (res != true) throw new ApplicationException(sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                                res = sfOut.EditCellValue(IDField, shIndx, shIndx);
                                if (res != true) throw new ApplicationException(sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                                shIndx++;
                            }
                            
                        }
                    }
                }
                Prog = Convert.ToInt32(shp * 100 / mwPolyLine.NumShapes);
                if (Prog > OldProg)
                {
                    MapWinUtility.Logger.Progress("Evaluating Cross Section..." + Prog.ToString() + "% Complete.", Prog, OldProg);
                    OldProg = Prog;
                }
                
            }
            res = sfOut.StopEditingShapes(true, true, ICallBack);
            if (res != true)
            {
                MapWinUtility.Logger.Dbg("Application Exception: " + sfOut.get_ErrorMsg(sfOut.LastErrorCode));
                throw new ApplicationException(sfOut.get_ErrorMsg(sfOut.LastErrorCode));
            }
        }

    }
}
