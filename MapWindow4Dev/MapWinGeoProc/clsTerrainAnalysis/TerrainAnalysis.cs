//********************************************************************************************************
//File name: TerrainAnalysis.cs
//Description: Public class, future home of many MapWinGIS.Utils functions;
//******************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specific language governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Open Source. 
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//06/08/06 ARA  Allen Anselmo   Created
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc
{
    /// <summary>
    /// TerrainAnalysis provides functionality for terrain analysis which is not directly part of the Hydrologic analysis found in HydroProc.cls
    /// </summary>
    ///
    public class TerrainAnalysis
    {
        /// <summary>
        /// Slope will take a DEM and an output path and form a slope grid using the 3rd Order Finite Difference slope algorithm found in Bolstad GIS Fundamentals
        /// </summary>
        /// <param name="inDEMPath">Path to the input DEM</param>
        /// <param name="outSlopePath">Path to the output slope grid</param>
        /// <param name="slopeInPercent">True if output should be in percent, false if not</param>
        /// <param name="callback">Callback object for progress and status messages</param>
        public static bool Slope(string inDEMPath, string outSlopePath, bool slopeInPercent, MapWinGIS.ICallback callback)
        {
            return doSlope(inDEMPath, 1.0, outSlopePath, slopeInPercent, callback);
        }

        /// <summary>
        /// Overload of slope using Z Factor
        /// </summary>
        /// <param name="inDEMPath"></param>
        /// <param name="inZFactor"></param>
        /// <param name="outSlopePath"></param>
        /// <param name="slopeInPercent"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool Slope(string inDEMPath, double inZFactor, string outSlopePath, bool slopeInPercent, MapWinGIS.ICallback callback)
        {
            return doSlope(inDEMPath, inZFactor, outSlopePath, slopeInPercent, callback);
        }

        private static bool doSlope(string inDEMPath, double zFactor, string outSlopePath, bool slopeInPercent, MapWinGIS.ICallback callback)
        {
            MapWinUtility.Logger.Dbg("doSlope(inDEmPath: " + inDEMPath + ",\n" +
                                     "        zFactor: " + zFactor.ToString() + ",\n" +
                                     "        outSlopePath: " + outSlopePath + ",\n" +
                                     "        slopeInPercent: " + slopeInPercent.ToString() + ",\n" +
                                     "        ICallback)");
            bool Result;
            MapWinGIS.Grid gridDem = new MapWinGIS.Grid();
            MapWinGIS.Grid gridSlope = new MapWinGIS.Grid();
            MapWinGIS.GridHeader head = new MapWinGIS.GridHeader();
            int Prog = 0;
            int OldProg = 0;
            double z1, z2, z3, z4, z5, z6, z7, z8, dZ_dx, dZ_dy, slope;

            if (!gridDem.Open(inDEMPath, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, callback))
            {
                MapWinUtility.Logger.Message(gridDem.get_ErrorMsg( gridDem.LastErrorCode), "Error opening Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }
            // This is a very bad thing. --> head = gridDem.Header;
            // Instead:
            head.CopyFrom(gridDem.Header);
            head.NodataValue = -9999;
            DataManagement.DeleteGrid(ref outSlopePath);
            Result = gridSlope.CreateNew(outSlopePath, head, MapWinGIS.GridDataType.DoubleDataType, head.NodataValue, true, MapWinGIS.GridFileType.UseExtension, callback);
            if (Result == false)
            {
                MapWinUtility.Logger.Message(gridSlope.get_ErrorMsg(gridSlope.LastErrorCode), "Error Creating Slope Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }

            MapWinUtility.Logger.Status("Computing Slope from " + System.IO.Path.GetFileName(inDEMPath));

            int LastRow = head.NumberRows - 2;
            int LastCol = head.NumberCols - 2;
            for (int row = 1; row <= LastRow; row++)
            {
                MapWinUtility.Logger.Progress(row, LastRow);
                if (callback != null)
                {
                    Prog = (int)((row * 100) / LastRow);
                    if (Prog > OldProg)
                    {
                        callback.Progress("Progress", Prog, "Calculating Slope..." + Prog.ToString() + "% Complete");
                        OldProg = Prog;
                    }
                }
                for (int col = 1; col <= LastCol; col++)
                {
                    z1 = Convert.ToDouble(gridDem.get_Value(col - 1, row - 1));
                    z2 = Convert.ToDouble(gridDem.get_Value(col, row - 1));
                    z3 = Convert.ToDouble(gridDem.get_Value(col + 1, row - 1));
                    z4 = Convert.ToDouble(gridDem.get_Value(col - 1, row));
                    z5 = Convert.ToDouble(gridDem.get_Value(col + 1, row));
                    z6 = Convert.ToDouble(gridDem.get_Value(col - 1, row + 1));
                    z7 = Convert.ToDouble(gridDem.get_Value(col, row + 1));
                    z8 = Convert.ToDouble(gridDem.get_Value(col + 1, row + 1));

                    dZ_dx = (zFactor*((z3 - z1) + 2 * (z5 - z4) + (z8 - z6))) / (8 * head.dX);
                    dZ_dy = (zFactor*((z1 - z6) + 2 * (z2 - z7) + (z3 - z8))) / (8 * head.dY);

                    //dZ_dx = (z5 - z4) / (2 * head.dX);
                    //dZ_dy = (z2 - z7) / (2 * head.dY);
                    slope = Math.Atan(Math.Sqrt((dZ_dx * dZ_dx) + (dZ_dy * dZ_dy))) * (180 / Math.PI);
                    if (slopeInPercent)
                    {
                        slope = Math.Tan(slope * (Math.PI / 180)) * 100;
                    }
                    gridSlope.set_Value(col, row, slope);
                }
            }

            gridDem.Close();
            Result = gridSlope.Save(outSlopePath, MapWinGIS.GridFileType.UseExtension, callback);
            if (Result == false)
            {
                MapWinUtility.Logger.Message(gridSlope.get_ErrorMsg(gridSlope.LastErrorCode), "Error Saving Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }
            
        
            gridSlope.Close();
            if (callback != null)
            {
                callback.Progress("status", 0, "");
                callback.Progress("Progress", 0, "");
            }
            MapWinUtility.Logger.Progress("", 0, 0);
            return true;
        }


        /// <summary>
        /// A function to calculate the slope, aspect, profile curvature, and plan curvature grids, which all come from the same base calculations
        /// Slope uses Bolstad algorithm from GIS Fundamentals First Edition, page 294. 
        /// Aspect uses modification of bolstad's aspect to match the ESRI specification at http://support.esri.com/index.cfm?fa=knowledgebase.techarticles.articleShow >d equal sign> 21345
        /// Profile and Plan curvature use the algorithms defined in Bolstad GIS Fundamentals Second Edition, in the terrain analysis chapter
        /// </summary>
        /// <param name="inDEMPath"></param>
        /// <param name="zFactor"></param>
        /// <param name="outSlopePath"></param>
        /// <param name="slopeInPercent"></param>
        /// <param name="outAspectPath"></param>
        /// <param name="outProfileCurvePath"></param>
        /// <param name="outPlanCurvePath"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool SlopeAspectCurvature(string inDEMPath, float zFactor, string outSlopePath, bool slopeInPercent, string outAspectPath, string outProfileCurvePath, string outPlanCurvePath, MapWinGIS.ICallback callback)
        {
            MapWinUtility.Logger.Dbg("SlopeAspectCurvature(inDEmPath: " + inDEMPath + ",\n" +
                                     "        zFactor: " + zFactor.ToString() + ",\n" +
                                     "        outSlopePath: " + outSlopePath + ",\n" +
                                     "        slopeInPercent: " + slopeInPercent.ToString() + ",\n" +
                                     "        outAspectPath: " + outAspectPath + ",\n" +
                                     "        outProfileCurvePath: " + outProfileCurvePath + ",\n" +
                                     "        outPlanCurvePath: " + outPlanCurvePath + ",\n" +
                                     "        ICallback)");
            bool Result;
            int Prog = 0;
            float z0, z1, z2, z3, z4, z5, z6, z7, z8, dZ_dx, dZ_dy, slope, aspect;
            double D, E, F, G, H, planCurve, profCurve;
            MapWinGIS.Grid gridDem = new MapWinGIS.Grid();
            MapWinGIS.Grid gridSlope = new MapWinGIS.Grid();
            MapWinGIS.GridHeader slopehead = new MapWinGIS.GridHeader();
            MapWinGIS.Grid gridAspect = new MapWinGIS.Grid();
            MapWinGIS.GridHeader asphead = new MapWinGIS.GridHeader();
            MapWinGIS.Grid gridProfCurve = new MapWinGIS.Grid();
            MapWinGIS.GridHeader profhead = new MapWinGIS.GridHeader();
            MapWinGIS.Grid gridPlanCurve = new MapWinGIS.Grid();
            MapWinGIS.GridHeader planhead = new MapWinGIS.GridHeader();


            if (!gridDem.Open(inDEMPath, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, callback))
            {
                MapWinUtility.Logger.Message(gridDem.get_ErrorMsg(gridDem.LastErrorCode), "Error opening Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }

            slopehead.CopyFrom(gridDem.Header);
            slopehead.NodataValue = float.MinValue;
            DataManagement.DeleteGrid(ref outSlopePath);
            Result = gridSlope.CreateNew(outSlopePath, slopehead, MapWinGIS.GridDataType.FloatDataType, slopehead.NodataValue, true, MapWinGIS.GridFileType.UseExtension, callback);
            if (Result == false)
            {
                MapWinUtility.Logger.Message(gridSlope.get_ErrorMsg(gridSlope.LastErrorCode), "Error Creating Slope Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }
            gridSlope.Header.Projection = gridDem.Header.Projection;

            asphead.CopyFrom(gridDem.Header);
            asphead.NodataValue = float.MinValue;
            DataManagement.DeleteGrid(ref outAspectPath);
            Result = gridAspect.CreateNew(outAspectPath, asphead, MapWinGIS.GridDataType.FloatDataType, asphead.NodataValue, true, MapWinGIS.GridFileType.UseExtension, callback);
            if (Result == false)
            {
                MapWinUtility.Logger.Message(gridAspect.get_ErrorMsg(gridAspect.LastErrorCode), "Error Creating Aspect Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }
            gridAspect.Header.Projection = gridDem.Header.Projection;

            profhead.CopyFrom(gridDem.Header);
            profhead.NodataValue = float.MinValue;
            DataManagement.DeleteGrid(ref outProfileCurvePath);
            Result = gridProfCurve.CreateNew(outProfileCurvePath, profhead, MapWinGIS.GridDataType.FloatDataType, profhead.NodataValue, true, MapWinGIS.GridFileType.UseExtension, callback);
            if (Result == false)
            {
                MapWinUtility.Logger.Message(gridProfCurve.get_ErrorMsg(gridProfCurve.LastErrorCode), "Error Creating Profile Curvature Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }
            gridProfCurve.Header.Projection = gridDem.Header.Projection;

            planhead.CopyFrom(gridDem.Header);
            planhead.NodataValue = float.MinValue;
            DataManagement.DeleteGrid(ref outPlanCurvePath);
            Result = gridPlanCurve.CreateNew(outPlanCurvePath, planhead, MapWinGIS.GridDataType.FloatDataType, planhead.NodataValue, true, MapWinGIS.GridFileType.UseExtension, callback);
            if (Result == false)
            {
                MapWinUtility.Logger.Message(gridPlanCurve.get_ErrorMsg(gridPlanCurve.LastErrorCode), "Error Creating Plan Curvature Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }
            gridPlanCurve.Header.Projection = gridDem.Header.Projection;

            int nr = slopehead.NumberRows;
            int nc = slopehead.NumberCols;
            double dX = slopehead.dX;
            double dY = slopehead.dY;
           
            float[][] rowBuffer = new float[3][];
            rowBuffer[0] = new float[nc];
            rowBuffer[1] = new float[nc];
            rowBuffer[2] = new float[nc];

            for (int row = 1; row < nr - 1; row++)
            {
                if (callback != null)
                {
                    Prog = (int)((row * 100) / (nr));
                    callback.Progress("Progress", Prog, "Calc Window Row: " + row.ToString());
                }
                MapWinUtility.Logger.Progress(Prog, nr);

                gridDem.GetRow(row - 1, ref rowBuffer[0][0]);
                gridDem.GetRow(row, ref rowBuffer[1][0]);
                gridDem.GetRow(row + 1, ref rowBuffer[2][0]);

                for (int col = 1; col < nc - 1; col++)
                {
                    //if (callback != null)
                    //{
                    //    //Prog = (int)((row * 100) / (nr));
                    //    callback.Progress("Progress", Prog, "Calc Window Row: " + row.ToString() + " Col: " + col.ToString());
                    //}
                    z1 = rowBuffer[0][col - 1];
                    z2 = rowBuffer[0][col];
                    z3 = rowBuffer[0][col + 1];
                    z4 = rowBuffer[1][col - 1];
                    z0 = rowBuffer[1][col];
                    z5 = rowBuffer[1][col + 1];
                    z6 = rowBuffer[2][col - 1];
                    z7 = rowBuffer[2][col];
                    z8 = rowBuffer[2][col + 1];

                    //a b c  1 2 3
                    //d e f  4 0 5
                    //g h i  6 7 8
                    //dz_dx esri
                    //((c + 2f + i) - (a + 2d + g)) / 8
                    //(c + 2f + i - a - 2d - g) / 8
                    //equiv bolstad 
                    //((c - a) + 2 (f - d) + (i - g))/8*dx
                    //(c + 2f + i - a - 2d -g)/8*dx
                    //dz_dy esri
                    //((g + 2h + i) - (a + 2b + c)) / 8 
                    //(g + 2h + i - a - 2b - c) / 8
                    //equiv bolstad 
                    //((a - g) + 2 (b - h) + (c - i))/8*dy
                    //(a + 2b + c - g - 2h - i)/8*dy (negative of esri)
                    
                    dZ_dx = (zFactor * ((z3 - z1) + 2 * (z5 - z4) + (z8 - z6))) / (8 * Convert.ToSingle(dX));
                    dZ_dy = (zFactor * ((z1 - z6) + 2 * (z2 - z7) + (z3 - z8))) / (8 * Convert.ToSingle(dY));

                    slope = Convert.ToSingle(Math.Atan(Math.Sqrt((dZ_dx * dZ_dx) + (dZ_dy * dZ_dy))) * (180 / Math.PI));
                    if (slopeInPercent)
                    {
                        slope = Convert.ToSingle(Math.Tan(slope * (Math.PI / 180)) * 100);
                    }

                    //neg them both and use atan2 to match ESRI algorithm because dz_dy is negative of esri and esri negs the dz_dx
                    aspect = Convert.ToSingle(Math.Atan2(-dZ_dy, -dZ_dx) * (180 / Math.PI));
                    if (dZ_dx == 0 && dZ_dy == 0) //have to do this to match esri output for flat areas
                    {
                        aspect = -1;
                    }
                    else if (aspect < 0)
                    {
                        aspect = Convert.ToSingle(90.0 - aspect);
                    }
                    else if (aspect > 90)
                    {
                        aspect = Convert.ToSingle(360.0 - aspect + 90.0);
                    }
                    else
                    {
                        aspect = Convert.ToSingle(90.0 - aspect);
                    }

                    //bolstad second edition coefficients match ESRI coefficients
                    D = (((z4 + z5) / 2) - z0) / Math.Pow(dX, 2);
                    E = (((z2 + z7) / 2) - z0) / Math.Pow(dX, 2);
                    F = (z3 - z1 + z6 - z8) / (4 * Math.Pow(dX, 2));
                    G = (z5 - z4) / (2 * dX);
                    H = (z2 - z7) / (2 * dX);

                    if (G == 0 && H == 0)
                    {
                        planCurve = 0;
                        profCurve = 0;
                    }
                    else
                    {
                        planCurve = (-200 * (D * Math.Pow(H, 2) + E * Math.Pow(G, 2) - F * G * H)) / (Math.Pow(G, 2) + Math.Pow(H, 2));
                        profCurve = (200 * (D * Math.Pow(G, 2) + E * Math.Pow(H, 2) + F * G * H)) / (Math.Pow(G, 2) + Math.Pow(H, 2));
                    }

                    gridSlope.set_Value(col, row, slope);
                    gridAspect.set_Value(col, row, aspect);
                    gridProfCurve.set_Value(col, row, Convert.ToSingle(profCurve));
                    gridPlanCurve.set_Value(col, row, Convert.ToSingle(planCurve));
                }
            }

            gridDem.Close();
            Result = gridSlope.Save(outSlopePath, MapWinGIS.GridFileType.UseExtension, callback);
            if (Result == false)
            {
                MapWinUtility.Logger.Message(gridSlope.get_ErrorMsg(gridSlope.LastErrorCode), "Error Saving Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }

            Result = gridAspect.Save(outAspectPath, MapWinGIS.GridFileType.UseExtension, callback);
            if (Result == false)
            {
                MapWinUtility.Logger.Message(gridAspect.get_ErrorMsg(gridAspect.LastErrorCode), "Error Saving Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }
            
            Result = gridProfCurve.Save(outProfileCurvePath, MapWinGIS.GridFileType.UseExtension, callback);
            if (Result == false)
            {
                MapWinUtility.Logger.Message(gridProfCurve.get_ErrorMsg(gridProfCurve.LastErrorCode), "Error Saving Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }

            Result = gridPlanCurve.Save(outPlanCurvePath, MapWinGIS.GridFileType.UseExtension, callback);
            if (Result == false)
            {
                MapWinUtility.Logger.Message(gridPlanCurve.get_ErrorMsg(gridPlanCurve.LastErrorCode), "Error Saving Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }

            gridSlope.Close();
            gridAspect.Close();
            gridPlanCurve.Close();
            gridProfCurve.Close();
            if (callback != null)
            {
                callback.Progress("status", 0, "");
                callback.Progress("Progress", 0, "");
            }
            MapWinUtility.Logger.Progress("", 0, 0);
            return true;
            
        }

        /// <summary>
        /// A function to calculate the slope, aspect, profile curvature, and plan curvature grids, which all come from the same base calculations
        /// Slope uses Bolstad algorithm from GIS Fundamentals First Edition, page 294. 
        /// Aspect uses modification of bolstad's aspect to match the ESRI specification at http://support.esri.com/index.cfm?fa=knowledgebase.techarticles.articleShow >d equal sign> 21345
        /// Profile and Plan curvature use the algorithms defined in Bolstad GIS Fundamentals Second Edition, in the terrain analysis chapter 
        /// </summary>
        /// <param name="inDEMPath"></param>
        /// <param name="zFactor"></param>
        /// <param name="outSlopePath"></param>
        /// <param name="slopeInPercent"></param>
        /// <param name="outAspectPath"></param>
        /// <param name="outProfileCurvePath"></param>
        /// <param name="outPlanCurvePath"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool SlopeAspectCurvature2(string inDEMPath, float zFactor, string outSlopePath, bool slopeInPercent, string outAspectPath, string outProfileCurvePath, string outPlanCurvePath, MapWinGIS.ICallback callback)
        {
            List<MovingWindow> windowList = new List<MovingWindow>();

            SlopeMovingWindow slopeWin = new SlopeMovingWindow();
            slopeWin.slopeInPercent = slopeInPercent;
            slopeWin.zFactor = zFactor;
            slopeWin.OutputPath = outSlopePath;
            windowList.Add(slopeWin);

            AspectMovingWindow aspectWin = new AspectMovingWindow();
            aspectWin.zFactor = zFactor;
            aspectWin.OutputPath = outAspectPath;
            windowList.Add(aspectWin);

            ProfileCurvatureMovingWindow profWin = new ProfileCurvatureMovingWindow();
            profWin.OutputPath = outProfileCurvePath;
            windowList.Add(profWin);

            PlanCurvatureMovingWindow planWin = new PlanCurvatureMovingWindow();
            planWin.OutputPath = outPlanCurvePath;
            windowList.Add(planWin);

            RunWindow(inDEMPath, windowList, callback);

            return true;
        }

        /// <summary>
        /// A slope using moving window instead of cell by cell association
        /// </summary>
        /// <param name="inDEMPath"></param>
        /// <param name="zFactor"></param>
        /// <param name="outSlopePath"></param>
        /// <param name="slopeInPercent"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool Slope2(string inDEMPath, float zFactor, string outSlopePath, bool slopeInPercent, MapWinGIS.ICallback callback)
        {
            List<MovingWindow> windowList = new List<MovingWindow>();

            SlopeMovingWindow slopeWin = new SlopeMovingWindow();
            slopeWin.slopeInPercent = slopeInPercent;
            slopeWin.zFactor = zFactor;
            slopeWin.OutputPath = outSlopePath;
            windowList.Add(slopeWin);

            RunWindow(inDEMPath, windowList, callback);

            return true;
        }


        /// <summary>
        /// Run window takes a list of moving windows to execute on a single input grid
        /// </summary>
        /// <param name="inGridPath"></param>
        /// <param name="inWindows"></param>
        /// <returns></returns>
        public static bool RunWindow(string inGridPath, List<MovingWindow> inWindows)
        {
            return doRunWindow(inGridPath, inWindows, "", null);
        }

        /// <summary>
        /// Run window takes a list of moving windows to execute on a single input grid
        /// </summary>
        /// <param name="inGridPath"></param>
        /// <param name="inWindows"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static bool RunWindow(string inGridPath, List<MovingWindow> inWindows, string Message)
        {
            return doRunWindow(inGridPath, inWindows, Message, null);
        }

        /// <summary>
        /// Run window takes a list of moving windows to execute on a single input grid
        /// </summary>
        /// <param name="inGridPath"></param>
        /// <param name="inWindows"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool RunWindow(string inGridPath, List<MovingWindow> inWindows, MapWinGIS.ICallback callback)
        {
            return doRunWindow(inGridPath, inWindows, "", callback);
        }

        /// <summary>
        /// Run window takes a list of moving windows to execute on a single input grid
        /// </summary>
        /// <param name="inGridPath"></param>
        /// <param name="inWindows"></param>
        /// <param name="Message"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static bool RunWindow(string inGridPath, List<MovingWindow> inWindows, string Message, MapWinGIS.ICallback callback)
        {
            return doRunWindow(inGridPath, inWindows, Message, callback);
        }
        
        private static bool doRunWindow(string inGridPath, List<MovingWindow> inWindows, string Message, MapWinGIS.ICallback callback)
        {
            MapWinUtility.Logger.Dbg("RunWindow(inGridPath: " + inGridPath + ",\n" +
                                     "        inWindows Count: " +inWindows.Count.ToString() + ",\n" +
                                     "        Message: " + Message + ",\n" +
                                     "        ICallback)");

            if (callback != null)
            {
                callback.Progress("Progress", 0, "Initializing " + Message + "Window");
            }
            bool Result;
            int Prog = 0;
            int OldProg = 0;
            int maxWinHeight = -10000;
            int currRealRow;

            //Open Input grid
            MapWinGIS.Grid inGrid = new MapWinGIS.Grid();
            if (!inGrid.Open(inGridPath, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension,  null))
            {
                MapWinUtility.Logger.Message(inGrid.get_ErrorMsg(inGrid.LastErrorCode), "Error opening Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }
            MapWinGIS.GridHeader inGridHeader = inGrid.Header;

            //Initialize (i.e. create output grids) each of the moving windows.
            foreach (MovingWindow win in inWindows)
            {
                Result = win.Initialize(win.OutputPath, inGridHeader);
                if (Result == false)
                {
                    foreach (MovingWindow win2 in inWindows)
                    {
                        win2.Cleanup();
                    }
                    return false;
                }
                if (maxWinHeight < win.WindowHeight)
                {
                    maxWinHeight = win.WindowHeight;
                }
            }

            int nr = inGridHeader.NumberRows;
            int nc = inGridHeader.NumberCols;
            double dX = inGridHeader.dX;
            double dY = inGridHeader.dY;

            //Create a rowbuffer that is big enough for the maximum moving window height
            float[][] rowBuffer = new float[maxWinHeight][];
            for (int i = 0; i < maxWinHeight; i++)
            {
                rowBuffer[i] = new float[nc];
            }

            int winCenterRow  = maxWinHeight / 2;

            //Make sure each of the moving windows knows what the center row is
            foreach (MovingWindow win in inWindows)
            {
                win.WindowCenterRow = winCenterRow;
            }
            try
            {
                for (int row = 0; row < nr; row++)
                {
                    if (callback != null)
                    {
                        Prog = (int)((row * 100) / (nr));
                        MapWinUtility.Logger.Progress("Calculate " + Message + "Window..." + Prog.ToString() + "% Complete", Prog, OldProg);
                        callback.Progress("Progress", Prog, "Calculate " + Message + "Window Row: " + (row + 1).ToString() + "/" + nr.ToString());
                    }

                    //Get the rows above the center row. 
                    for (int i = 1; i <= winCenterRow; i++)
                    {
                        currRealRow = row - i;
                        if (currRealRow < 0)
                        {
                            rowBuffer[winCenterRow - i] = new float[nc];
                        }
                        else
                        {
                            inGrid.GetRow(currRealRow, ref rowBuffer[winCenterRow - i][0]);
                        }
                    }
                    
                    //Get the center row
                    inGrid.GetRow(row, ref rowBuffer[winCenterRow][0]);
                    
                    //Get the rows below the center row
                    for (int i = 1; i <= winCenterRow; i++)
                    {
                        currRealRow = row + i;
                        if (currRealRow > nr)
                        {
                            rowBuffer[winCenterRow + i] = new float[nc];
                        }
                        else
                        {
                            inGrid.GetRow(currRealRow, ref rowBuffer[winCenterRow + i][0]);
                        }
                    }

                    //Make sure all the windows know what the current row is and has a reference to the row data.
                    foreach (MovingWindow win in inWindows)
                    {
                        win.CurrRow = row;
                        win._WindowRowData = rowBuffer;
                    }

                    for (int col = 0; col < nc; col++)
                    {
                        //if (callback != null)
                        //{
                        //    callback.Progress("Progress", Prog, "Calc Window Row: " + row.ToString() + "  Col: " + col.ToString());
                        //}
                        foreach (MovingWindow win in inWindows)
                        {
                            //Make sure the window knows the current column
                            win.CurrCol = col;

                            //Call the calculate, which should call the correct type via polymorphism and set the center cell correctly.
                            Result = win.Calculate();
                            if (Result == false)
                            {
                                inGrid.Close();
                                foreach (MovingWindow win2 in inWindows)
                                {
                                    win2.Cleanup();
                                }
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                inGrid.Close();
                foreach (MovingWindow win2 in inWindows)
                {
                    win2.Cleanup();
                }
                MapWinUtility.Logger.Message("An exception was thrown while running the moving window calculations: " + Ex.Message, "Error Calculating Window Values", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }
            if (callback != null)
            {
                callback.Progress("Progress", 0, "Saving Window Results");
            }
            inGrid.Close();

            //Save and close the grids.
            foreach (MovingWindow win in inWindows)
            {
                Result = win.Finish();
                if (Result == false)
                {
                    foreach (MovingWindow win2 in inWindows)
                    {
                        win2.Cleanup();
                    }
                    return false;
                }
            }

            if (callback != null)
            {
                callback.Progress("Progress", 0, "");
            }
            MapWinUtility.Logger.Progress("", 0, 0);
            return true;
        }

        

        #region "NOAA Solar Position"
        
        /// <summary>
        /// A function to calculate the azimuth and altitude of the estimated sun 
        ///  position at a given time and date as based on the NOAA javascript 
        ///  solar calculations used at 
        ///  http://www.srrb.noaa.gov/highlights/sunrise/azel.html which, in 
        ///  turn, are based on Astronomical Algorithms, by Jean Meeus
        /// </summary>
        /// <param name="Month">Month in numeric form January = 1, December = 12</param>
        /// <param name="Day">Numeric Day of month</param>
        /// <param name="Year">Numeric Year AD. Won't work as well for years before 1800</param>
        /// <param name="Hour">Hour in 24 hour time</param>
        /// <param name="Minute">Minute</param>
        /// <param name="Second">Seconds</param>
        /// <param name="OffsetToUniversalTime">Offset to UTC. Positive for west, negative for east. MST is 7.</param>
        /// <param name="DaylightSavingsTime">True if using DST for current region</param>
        /// <param name="Latitude">Standard latitude in decimal degrees. West is negative.</param>
        /// <param name="Longitude">Standard longitude in decimal degrees. South is negative</param>
        /// <param name="Altitude">Output for solar altitude in degrees clockwise from north</param>
        /// <param name="Azimuth">Output for solar azimuth in degrees in degrees up from horizon</param>
        public static void CalculateSunPosition(int Month, int Day, int Year, int Hour, int Minute, int Second, double OffsetToUniversalTime, bool DaylightSavingsTime, double Latitude, double Longitude, out double Altitude, out double Azimuth)
        {
            double eqTimeRes = -999.0, solarDecRes = -999.0, azimuthRes = -999.0, elevationRes = -999.0, cosZenRes = -999.0;

            //Reverse the standard Longitude because this algorithm assumes west is positive, east is negative
            Longitude = -Longitude;

            if (isValidPositionInputs(Month, Day, Year, Hour, Minute, Second))
            {
                if ((Latitude >= -90) && (Latitude < -89.8))
                {
                    //alert("All latitudes between 89.8 and 90 S\n will be set to -89.8.");
                    Latitude = -89.8;
                }
                if ((Latitude <= 90) && (Latitude > 89.8))
                {
                    //alert("All latitudes between 89.8 and 90 N\n will be set to 89.8.");
                    Latitude = 89.8;
                }

                //*****	Get calc date/time
                //double julDay = calcJD(Year, Month, Day);
                int daySavings = DaylightSavingsTime ? 60 : 0;
                double zone = OffsetToUniversalTime;
                if (zone > 12 || zone < -12.5)
                {
                    //alert("The offset must be between -12.5 and 12.  \n Setting \"Off-Set\"=0");
                    zone = 0;
                }
                int mm = Minute;
                int hh = Hour - (daySavings / 60);
                int ss = Second;
                while (hh > 23)
                {
                    hh -= 24;
                }

                // timenow is GMT time for calculation
                double timenow = hh + mm / 60 + ss / 3600 + zone;	// in hours since 0Z
                double JD = calcJD(Year, Month, Day);
                string dow = calcDayOfWeek(JD);
                double doy = calcDayOfYear(Month, Day, isLeapYear(Year));
                double T = calcTimeJulianCent(JD + timenow/24.0);
                double R = calcSunRadVector(T);
                double alpha = calcSunRtAscension(T);
                double theta = calcSunDeclination(T);
                double Etime = calcEquationOfTime(T);
                double eqTime = Etime;
                double solarDec = theta; // in degrees
                
                eqTimeRes = Math.Floor(100.0*Etime)/100.0;
                solarDecRes = Math.Floor(100.0*theta)/100.0; // in degrees
                
                double earthRadVec = R;
                double solarTimeFix = eqTime - 4.0 * Longitude + 60.0 * zone;
                double trueSolarTime = hh * 60.0 + mm + ss / 60.0 + solarTimeFix; //in minutes
                while (trueSolarTime > 1440)
                {
                    trueSolarTime -= 1440;
                }

                double hourAngle = trueSolarTime / 4.0 - 180.0;
                if (hourAngle < -180)
                {
                  hourAngle += 360.0;
                }

                double haRad = degToRad(hourAngle);

                double csz = Math.Sin(degToRad(Latitude)) * Math.Sin(degToRad(solarDec)) + Math.Cos(degToRad(Latitude)) * Math.Cos(degToRad(solarDec)) * Math.Cos(haRad);
                if (csz > 1.0)
                {
                    csz = 1.0;
                }
                else if (csz < -1.0)
                {
                    csz = -1.0;
                }

                double zenith = radToDeg(Math.Acos(csz));
                double azDenom = (Math.Acos(degToRad(Latitude)) * Math.Sin(degToRad(zenith)));
                double azRad;
                if (Math.Abs(azDenom) > 0.001)
                {
                    azRad = ((Math.Sin(degToRad(Latitude)) * Math.Cos(degToRad(zenith))) - Math.Sin(degToRad(solarDec))) / azDenom;
                    if (Math.Abs(azRad) > 1.0)
                    {
                        if (azRad < 0)
                        {
                            azRad = -1.0;
                        }
                        else
                        {
                            azRad = 1.0;
                        }
                    }

                    azimuthRes = 180.0 - radToDeg(Math.Acos(azRad));

                    if (hourAngle > 0.0)
                    {
                        azimuthRes = -azimuthRes;
                    }
                }
                else
                {
                    if (Latitude > 0.0)
                    {
                        azimuthRes = 180.0;
                    }
                    else
                    {
                        azimuthRes = 0.0;
                    }
                }
                if (azimuthRes < 0.0)
                {
                    azimuthRes += 360.0;
                }

                double te, refractionCorrection;
                double exoatmElevation = 90.0 - zenith;
                if (exoatmElevation > 85.0)
                {
                    refractionCorrection = 0.0;
                }
                else
                {
                    te = Math.Tan(degToRad(exoatmElevation));
                    if (exoatmElevation > 5.0)
                    {
                        refractionCorrection = 58.1 / te - 0.07 / (te * te * te) + 0.000086 / (te * te * te * te * te);
                    }
                    else if (exoatmElevation > -0.575)
                    {
                        refractionCorrection = 1735.0 + exoatmElevation * (-518.2 + exoatmElevation * (103.4 + exoatmElevation * (-12.79 + exoatmElevation * 0.711)));
                    }
                    else
                    {
                        refractionCorrection = -20.774 / te;
                    }
                    refractionCorrection = refractionCorrection / 3600.0;
                }

                double solarZen = zenith - refractionCorrection;

                if (solarZen < 108.0)
                { // astronomical twilight
                    azimuthRes = Math.Floor(100.0 * azimuthRes) / 100.0;
                    elevationRes = Math.Floor(100.0 * (90.0 - solarZen)) / 100.0;

                    if (solarZen < 90.0)
                    {
                        cosZenRes = Math.Floor(10000.0 * (Math.Cos(degToRad(solarZen)))) / 10000.0;
                    }
                    else
                    {
                        cosZenRes = 0.0;
                    }
                }
                else
                { //darkness
                    elevationRes = -999.0;
                    azimuthRes = -999.0;
                }
            }
            Altitude = elevationRes;
            Azimuth = azimuthRes;

        }

        /// <summary>
        /// A function to calculate the sunrise and sunset dates of a given date and lat/lon
        /// based on the NOAA javascript solar calculations used at
        /// http://www.srrb.noaa.gov/highlights/sunrise/sunrise.html
        /// </summary>
        /// <param name="Month">Month in numeric form January = 1, December = 12</param>
        /// <param name="Day">Numeric Day of month</param>
        /// <param name="Year">Numeric Year AD. Won't work as well for years before 1800</param>
        /// <param name="OffsetToUniversalTime">Offset to UTC. Positive for west, negative for east. MST is 7.</param>
        /// <param name="DaylightSavingsTime">True if using DST for current region</param>
        /// <param name="Latitude">Standard latitude in decimal degrees. West is negative.</param>
        /// <param name="Longitude">Standard longitude in decimal degrees. South is negative</param>
        /// <param name="Sunrise">Sunrise time. A local time in minutes in format of double. -1.0 if an error occurs. Can be converted to readable format with various timeString functions</param>
        /// <param name="Noon">Solar noon time. A local time in minutes in format of double. -1.0 if an error occurs or if there is no sunset currently. Can be converted to readable format with various timeString functions</param>
        /// <param name="Sunset">Sunset time. A local time in format of double. -1.0 if an error occurs. Can be converted to readable format with various timeString functions</param>
        /// <param name="RiseDate">Will be -1.0 normally, but in northern and southern regions, there isn't a daily sunrise and sunset and so this date will be set to show the last day a sunrise occurred. In Julian day format.</param>
        /// <param name="SetDate">Will be -1.0 normally, but in northern and southern regions, there isn't a daily sunrise and sunset and so this date will be set to show the next day a sunset occurred. In Julian day format.</param>
        public static void CalculateSunRiseNoonSet(int Month, int Day, int Year, double OffsetToUniversalTime, bool DaylightSavingsTime, double Latitude, double Longitude, out double Sunrise, out double Noon, out double Sunset, out double RiseDate, out double SetDate) 
	    {
            Sunrise = -1;
            Noon = -1;
            Sunset = -1;
            RiseDate = -1;
            SetDate = -1;
            //Reverse the standard Longitude because this algorithm assumes west is positive, east is negative
            Longitude = -Longitude;

            if (isValidPositionInputs(Month, Day, Year, 0, 0, 0))
            {
                if ((Latitude >= -90) && (Latitude < -89))
                {
                    //alert("All Latitudes between 89 and 90 S\n will be set to -89");
                    Latitude = -89;
                }
                if ((Latitude <= 90) && (Latitude > 89))
                {
                    //alert("All Latitudes between 89 and 90 N\n will be set to 89");
                    Latitude = 89;
                }

                //*****	Calculate the time of sunrise			

                //*********************************************************************/
                //****************   NEW STUFF   ******   January, 2001   ****************
                //*********************************************************************/

                double JD = calcJD(Year, Month, Day);
                string dow = calcDayOfWeek(JD);
                double doy = calcDayOfYear(Month, Day, isLeapYear(Year));
                double T = calcTimeJulianCent(JD);

                double alpha = calcSunRtAscension(T);
                double theta = calcSunDeclination(T);
                double Etime = calcEquationOfTime(T);

                //*********************************************************************/

                double eqTime = Etime;
                double solarDec = theta;

                // Calculate sunrise for this date
                // if no sunrise is found, set flag nosunrise

                bool nosunrise = false;

                double riseTimeGMT = calcSunriseUTC(JD, Latitude, Longitude);
                if (double.IsNaN(riseTimeGMT))
                {
                    nosunrise = true;
                }

                // Calculate sunset for this date
                // if no sunset is found, set flag nosunset

                bool nosunset = false;
                double setTimeGMT = calcSunsetUTC(JD, Latitude, Longitude);
                if (double.IsNaN(setTimeGMT))
                {
                    nosunset = true;
                }

                int daySavings;  // = 0 (no) or 60 (yes)
                if (DaylightSavingsTime)
                {
                    daySavings = 60;
                }
                else
                {
                    daySavings = 0;
                }
                double zone = OffsetToUniversalTime;
                if (zone > 12 || zone < -12.5)
                {
                    //alert("The offset must be between -12.5 and 12.  \n Setting \"Off-Set\"=0");
                    zone = 0;
                }

                if (!nosunrise)		// Sunrise was found
                {
                    double riseTimeLST = Convert.ToDouble(riseTimeGMT) - (60 * zone) + daySavings;
                    //	in minutes
                    //double riseStr = timeStringShortAMPM(riseTimeLST, JD);
                    //double utcRiseStr = timeStringDate(riseTimeGMT, JD);
                    Sunrise = riseTimeLST;
                }

                if (!nosunset)		// Sunset was found
                {
                    double setTimeLST = Convert.ToDouble(setTimeGMT) - (60 * zone) + daySavings;
                    //double setStr = timeStringShortAMPM(setTimeLST, JD);
                    //double utcSetStr = timeStringDate(setTimeGMT, JD);
                    Sunset = setTimeLST;
                }

                // Calculate solar noon for this date

                double solNoonGMT = calcSolNoonUTC(T, Longitude);
                double solNoonLST = solNoonGMT - (60 * zone) + daySavings;

                //double solnStr = timeString(solNoonLST);
                //double utcSolnStr = timeString(solNoonGMT);
                Noon = solNoonLST;

                double tsnoon = calcTimeJulianCent(calcJDFromJulianCent(T) - 0.5 + Convert.ToDouble(solNoonGMT) / 1440.0);

                eqTime = calcEquationOfTime(tsnoon);
                solarDec = calcSunDeclination(tsnoon);

                // report special cases of no sunrise
                double newjd, newtime;
                if (nosunrise)
                {
                    // if Northern hemisphere and spring or summer, OR  
                    // if Southern hemisphere and fall or winter, use 
                    // previous sunrise and next sunset

                    if (((Latitude > 66.4) && (doy > 79) && (doy < 267)) ||
                       ((Latitude < -66.4) && ((doy < 83) || (doy > 263))))
                    {
                        newjd = findRecentSunrise(JD, Latitude, Longitude);
                        newtime = Convert.ToDouble(calcSunriseUTC(newjd, Latitude, Longitude)) - (60 * zone) + daySavings;
                        if (newtime > 1440)
                        {
                            newtime -= 1440;
                            newjd += 1.0;
                        }
                        if (newtime < 0)
                        {
                            newtime += 1440;
                            newjd -= 1.0;
                        }
                        //timeStringAMPMDate(newtime, newjd);
                        Sunrise = newtime;
                        RiseDate = newjd;
                    }

                    // if Northern hemisphere and fall or winter, OR 
                    // if Southern hemisphere and spring or summer, use 
                    // next sunrise and previous sunset

                    else if (((Latitude > 66.4) && ((doy < 83) || (doy > 263))) ||
                        ((Latitude < -66.4) && (doy > 79) && (doy < 267)))
                    {
                        newjd = findNextSunrise(JD, Latitude, Longitude);
                        newtime = Convert.ToDouble(calcSunriseUTC(newjd, Latitude, Longitude)) - (60 * zone) + daySavings;
                        if (newtime > 1440)
                        {
                            newtime -= 1440;
                            newjd += 1.0;
                        }
                        if (newtime < 0)
                        {
                            newtime += 1440;
                            newjd -= 1.0;
                        }
                        //timeStringAMPMDate(newtime, newjd);
                        Sunrise = newtime;
                        RiseDate = newjd;
                    }
                    else
                    {
                        //alert("Cannot Find Sunrise!");
                    }
                }

                if (nosunset)
                {
                    // if Northern hemisphere and spring or summer, OR
                    // if Southern hemisphere and fall or winter, use 
                    // previous sunrise and next sunset

                    if (((Latitude > 66.4) && (doy > 79) && (doy < 267)) ||
                       ((Latitude < -66.4) && ((doy < 83) || (doy > 263))))
                    {
                        newjd = findNextSunset(JD, Latitude, Longitude);
                        newtime = Convert.ToDouble(calcSunsetUTC(newjd, Latitude, Longitude)) - (60 * zone) + daySavings;
                        if (newtime > 1440)
                        {
                            newtime -= 1440;
                            newjd += 1.0;
                        }
                        if (newtime < 0)
                        {
                            newtime += 1440;
                            newjd -= 1.0;
                        }
                        //timeStringAMPMDate(newtime, newjd);
                        Sunset = newtime;
                        SetDate = newjd;
                        Noon = -1;
                    }

                    // if Northern hemisphere and fall or winter, OR
                    // if Southern hemisphere and spring or summer, use 
                    // next sunrise and last sunset

                    else if (((Latitude > 66.4) && ((doy < 83) || (doy > 263))) ||
                        ((Latitude < -66.4) && (doy > 79) && (doy < 267)))
                    {
                        newjd = findRecentSunset(JD, Latitude, Longitude);
                        newtime = Convert.ToDouble(calcSunsetUTC(newjd, Latitude, Longitude)) - (60 * zone) + daySavings;
                        if (newtime > 1440)
                        {
                            newtime -= 1440;
                            newjd += 1.0;
                        }
                        if (newtime < 0)
                        {
                            newtime += 1440;
                            newjd -= 1.0;
                        }
                        //timeStringAMPMDate(newtime, newjd);
                        Sunset = newtime;
                        SetDate = newjd;
                        Noon = -1;
                    }

                    else
                    {
                        //alert("Cannot Find Sunset!");
                    }
                }
            }
        }

        /// <summary>
        /// A function convert time of day in minutes to hours, minutes, and seconds
        /// </summary>
        /// <param name="minutes">time of day in minutes</param>
        /// <param name="hour">Output integer of hours in military time</param>
        /// <param name="minute">Output integer of minutes</param>
        /// <param name="second">Output integer of seconds</param>
        public static void timeConvert(double minutes, out int hour, out int minute, out int second)
        {
            double floatHour = minutes / 60.0;
            hour = Convert.ToInt32(Math.Floor(floatHour));
            double floatMinute = 60.0 * (floatHour - Math.Floor(floatHour));
            minute = Convert.ToInt32(Math.Floor(floatMinute));
            double floatSec = 60.0 * (floatMinute - Math.Floor(floatMinute));
            second = Convert.ToInt32(Math.Floor(floatSec + 0.5));
        }
        
        /// <summary>
        /// A function convert time of day in minutes to a zero-padded string
        ///		suitable for printing to the form text fields
	    /// </summary>
	    /// <param name="minutes">time of day in minutes</param>
        /// <returns>string of the format HH:MM:SS, minutes and seconds are zero padded</returns>
        // timeString returns a zero-padded string (HH:MM:SS) given time in minutes
        public static string timeString(double minutes)
	    {
		    double floatHour = minutes / 60.0;
		    double hour = Math.Floor(floatHour);
		    double floatMinute = 60.0 * (floatHour - Math.Floor(floatHour));
		    double minute = Math.Floor(floatMinute);
		    double floatSec = 60.0 * (floatMinute - Math.Floor(floatMinute));
		    double second = Math.Floor(floatSec + 0.5);

		    string timeStr = hour.ToString() + ":";
		    if (minute < 10)	//	i.e. only one digit
			    timeStr += "0" + minute.ToString() + ":";
		    else
			    timeStr += minute.ToString() + ":";
		    if (second < 10)	//	i.e. only one digit
			    timeStr += "0" + second.ToString();
		    else
			    timeStr += second.ToString();

		    return timeStr;
	    }
        
        /// <summary>
        /// A function to convert time of day in minutes to a zero-padded string
        ///		suitable for printing to the form text fields.  If time	
        ///		crosses a day boundary, date is appended.
        /// </summary>
        /// <param name="minutes">time of day in minutes</param>
        /// <param name="JD">julian day</param>
        /// <returns>string of the format HH:MM[AM/PM] (DDMon)</returns>
        // timeStringShortAMPM returns a zero-padded string (HH:MM *M) given time in 
        // minutes and appends short date if time is > 24 or < 0, resp.
        public static string timeStringShortAMPM(double minutes, double JD)
	    {
		    double julianday = JD;
		    double floatHour = minutes / 60.0;
		    double hour = Math.Floor(floatHour);
		    double floatMinute = 60.0 * (floatHour - Math.Floor(floatHour));
		    double minute = Math.Floor(floatMinute);
		    double floatSec = 60.0 * (floatMinute - Math.Floor(floatMinute));
		    double second = Math.Floor(floatSec + 0.5);
		    bool PM = false;

		    minute += (second >= 30)? 1 : 0;

		    if (minute >= 60) 
		    {
			    minute -= 60;
			    hour ++;
		    }

		    bool daychange = false;
		    if (hour > 23) 
		    {
			    hour -= 24;
			    daychange = true;
			    julianday += 1.0;
		    }

		    if (hour < 0)
		    {
			    hour += 24;
			    daychange = true;
			    julianday -= 1.0;
		    }

		    if (hour > 12)
		    {
			    hour -= 12;
			    PM = true;
		    }

                if (hour == 12)
		    {
                  PM = true;
                }

		    if (hour == 0)
		    {
			    PM = false;
			    hour = 12;
		    }

		    string timeStr = hour.ToString() + ":";
		    if (minute < 10)	//	i.e. only one digit
                timeStr += "0" + minute.ToString() + ((PM) ? "PM" : "AM");
		    else
                timeStr += "" + minute.ToString() + ((PM) ? "PM" : "AM");

		    if (daychange) return timeStr + " " + calcDayFromJD(julianday);
		    return timeStr;
	    }

        /// <summary>
        /// A function to convert time of day in minutes to a zero-padded string
        ///		suitable for printing to the form text fields, and appends	
        ///		the date.
        /// </summary>
        /// <param name="minutes">time of day in minutes</param>
        /// <param name="JD">julian day</param>
        /// <returns>string of the format HH:MM[AM/PM] DDMon</returns>
        // timeStringAMPMDate returns a zero-padded string (HH:MM[AM/PM]) given time 
        // in minutes and julian day, and appends the short date
        public static string timeStringAMPMDate(double minutes, double JD)
	    {
		    double julianday = JD;
		    double floatHour = minutes / 60.0;
		    double hour = Math.Floor(floatHour);
		    double floatMinute = 60.0 * (floatHour - Math.Floor(floatHour));
		    double minute = Math.Floor(floatMinute);
		    double floatSec = 60.0 * (floatMinute - Math.Floor(floatMinute));
		    double second = Math.Floor(floatSec + 0.5);

		    minute += (second >= 30)? 1 : 0;

		    if (minute >= 60) 
		    {
			    minute -= 60;
			    hour ++;
		    }

		    if (hour > 23) 
		    {
			    hour -= 24;
			    julianday += 1.0;
		    }

		    if (hour < 0)
		    {
			    hour += 24;
			    julianday -= 1.0;
		    }

		    bool PM = false;
		    if (hour > 12)
		    {
			    hour -= 12;
			    PM = true;
		    }

            if (hour == 12)
		    {
                PM = true;
            }

		    if (hour == 0)
		    {
			    PM = false;
			    hour = 12;
		    }

		    string timeStr = hour + ":";
		    if (minute < 10)	//	i.e. only one digit
                timeStr += "0" + minute.ToString() + ((PM) ? "PM" : "AM");
		    else
                timeStr += minute.ToString() + ((PM) ? "PM" : "AM");

		    return timeStr + " " + calcDayFromJD(julianday);
	    }

        /// <summary>
        /// A function to convert time of day in minutes to a zero-padded 24hr time	
        ///		suitable for printing to the form text fields.  If time	
        ///		crosses a day boundary, date is appended.
        /// </summary>
        /// <param name="minutes">time of day in minutes</param>
        /// <param name="JD">julian day</param>
        /// <returns>string of the format HH:MM (DDMon)</returns>
        // timeStringDate returns a zero-padded string (HH:MM) given time in minutes
        // and julian day, and appends the short date if time crosses a day boundary
        public static string timeStringDate(double minutes, double JD)
	    {
		    double julianday = JD;
		    double floatHour = minutes / 60.0;
		    double hour = Math.Floor(floatHour);
		    double floatMinute = 60.0 * (floatHour - Math.Floor(floatHour));
		    double minute = Math.Floor(floatMinute);
		    double floatSec = 60.0 * (floatMinute - Math.Floor(floatMinute));
		    double second = Math.Floor(floatSec + 0.5);

		    minute += (second >= 30)? 1 : 0;

		    if (minute >= 60) 
		    {
			    minute -= 60;
			    hour ++;
		    }

		    bool daychange = false;
		    if (hour > 23) 
		    {
			    hour -= 24;
			    julianday += 1.0;
			    daychange = true;
		    }

		    if (hour < 0)
		    {
			    hour += 24;
			    julianday -= 1.0;
			    daychange = true;
		    }

            string timeStr = hour.ToString() + ":";
		    if (minute < 10)	//	i.e. only one digit
                timeStr += "0" + minute.ToString();
		    else
                timeStr += minute.ToString();

		    if (daychange) return timeStr + " " + calcDayFromJD(julianday);
		    return timeStr;
	    }


        private static bool isValidPositionInputs(int Month, int Day, int Year, int Hour, int Minute, int Second)
        {
            if (Month < 1 || Month > 12)
            {
                return false;
            }

            if (Day < 1 || Day > 31)
            {
                return false;
            }

            if (Year < 0)
            {
                return false;
            }

            if (Hour < 0 || Hour > 24)
            {
                return false;
            }

            if (Minute < 0 || Minute > 59)
            {
                return false;
            }

            if (Second < 0 || Second > 59)
            {
                return false;
            }

            return true;
        }

        //	'isLeapYear' returns '1' if the yr is a leap year, '0' if it is not.
        private static int isLeapYear(int yr)
        {
            if ((yr % 4 == 0 && yr % 100 != 0) || yr % 400 == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


        // Convert radian angle to degrees
        private static double radToDeg(double angleRad)
        {
            return (180.0 * angleRad / Math.PI);
        }

        // Convert degree angle to radians
        private static double degToRad(double angleDeg) 
        {
            return (Math.PI * angleDeg / 180.0);
        }

        //***********************************************************************/
        //* Name:    calcDayOfYear								*/
        //* Type:    Function									*/
        //* Purpose: Finds numerical day-of-year from mn, day and lp year info  */
        //* Arguments:										*/
        //*   month: January = 1								*/
        //*   day  : 1 - 31									*/
        //*   lpyr : 1 if leap year, 0 if not						*/
        //* Return value:										*/
        //*   The numerical day of year							*/
        //***********************************************************************/
        private static int calcDayOfYear(int mn, int dy, int lpyr)
        {
            int k = (lpyr==1 ? 1 : 2);
            int doy = Convert.ToInt32(Math.Floor((275 * mn) / 9.0) - k * Math.Floor((mn + 9) / 12.0) + dy - 30);
            return doy;
        }

        //***********************************************************************/
        //* Name:    calcDayOfWeek								*/
        //* Type:    Function									*/
        //* Purpose: Derives weekday from Julian Day					*/
        //* Arguments:										*/
        //*   juld : Julian Day									*/
        //* Return value:										*/
        //*   String containing name of weekday						*/
        //***********************************************************************/
        private static string calcDayOfWeek(double juld)
        {
            int A = Convert.ToInt32((juld + 1.5) % 7);
            string DOW = (A==0)?"Sunday":(A==1)?"Monday":(A==2)?"Tuesday":(A==3)?"Wednesday":(A==4)?"Thursday":(A==5)?"Friday":"Saturday";
            return DOW;
        }
        
        //***********************************************************************/
        //* Name:    calcJD									*/
        //* Type:    Function									*/
        //* Purpose: Julian day from calendar day						*/
        //* Arguments:										*/
        //*   year : 4 digit year								*/
        //*   month: January = 1								*/
        //*   day  : 1 - 31									*/
        //* Return value:										*/
        //*   The Julian day corresponding to the date					*/
        //* Note:											*/
        //*   Number is returned for start of day.  Fractional days should be	*/
        //*   added later.									*/
        //***********************************************************************/
        private static double calcJD(int year, int month, int day)
        {
            if (month <= 2) {
                year -= 1;
                month += 12;
            }
            double A = Math.Floor(year/100.0);
            double B = 2 - A + Math.Floor(A/4);

            double JD = Math.Floor(365.25*(year + 4716)) + Math.Floor(30.6001*(month+1)) + day + B - 1524.5;
            return JD;
        }
        
        //***********************************************************************/
        //* Name:    calcDateFromJD								*/
        //* Type:    Function									*/
        //* Purpose: Calendar date from Julian Day					*/
        //* Arguments:										*/
        //*   jd   : Julian Day									*/
        //* Return value:										*/
        //*   String date in the form DD-MONTHNAME-YYYY					*/
        //* Note:											*/
        //***********************************************************************/
        private static string calcDateFromJD(double jd)
        {
            double A, alpha;
            double z = Math.Floor(jd + 0.5);
            double f = (jd + 0.5) - z;

            if (z < 2299161.0)
            {
                A = z;
            }
            else
            {
                alpha = Math.Floor((z - 1867216.25) / 36524.25);
                A = z + 1 + alpha - Math.Floor(alpha / 4.0);
            }

            double B = A + 1524;
            double C = Math.Floor((B - 122.1) / 365.25);
            double D = Math.Floor(365.25 * C);
            double E = Math.Floor((B - D) / 30.6001);

            int day = Convert.ToInt32(B - D - Math.Floor(30.6001 * E) + f);
            int month = Convert.ToInt32((E < 14) ? E - 1 : E - 13);
            int year = Convert.ToInt32((month > 2) ? C - 4716 : C - 4715);

            return (day + "-" + monthName(month) + "-" + year);
        }

        private static string monthName(int month)
        {
            return (month == 1) ? "January" : (month == 2) ? "February" : (month == 3) ? "March" : (month == 4) ? "April" : (month == 5) ? "May" : (month == 6) ? "June" : (month == 7) ? "July" : (month == 8) ? "August" : (month == 9) ? "September" : (month == 10) ? "October" : (month == 11) ? "November" : (month == 12) ? "December" : "Unknown";
        }
        
        //***********************************************************************/
        //* Name:    calcDayFromJD								*/
        //* Type:    Function									*/
        //* Purpose: Calendar day (minus year) from Julian Day			*/
        //* Arguments:										*/
        //*   jd   : Julian Day									*/
        //* Return value:										*/
        //*   String date in the form DD-MONTH						*/
        //***********************************************************************/
        private static string calcDayFromJD(double jd)
        {
            double z = Math.Floor(jd + 0.5);
            double f = (jd + 0.5) - z;
            double A, alpha;

            if (z < 2299161.0) {
                A = z;
            } else {
                alpha = Math.Floor((z - 1867216.25)/36524.25);
                A = z + 1 + alpha - Math.Floor(alpha/4.0);
            }

            double B = A + 1524;
            double C = Math.Floor((B - 122.1)/365.25);
            double D = Math.Floor(365.25 * C);
            double E = Math.Floor((B - D)/30.6001);

            int day = Convert.ToInt32(B - D - Math.Floor(30.6001 * E) + f);
            int month = Convert.ToInt32((E < 14) ? E - 1 : E - 13);
            int year = Convert.ToInt32((month > 2) ? C - 4716 : C - 4715);

            return ((day<10 ? "0" : "") + day + monthAbbrev(month));
        }

        private static string monthAbbrev(int month)
        {
            return (month == 1) ? "JAN" : (month == 2) ? "FEB" : (month == 3) ? "MAR" : (month == 4) ? "APR" : (month == 5) ? "MAY" : (month == 6) ? "JUN" : (month == 7) ? "JUL" : (month == 8) ? "AUG" : (month == 9) ? "SEP" : (month == 10) ? "OCT" : (month == 11) ? "NOV" : (month == 12) ? "DEC" : "UNK";
        }

        //***********************************************************************/
        //* Name:    calcTimeJulianCent							*/
        //* Type:    Function									*/
        //* Purpose: convert Julian Day to centuries since J2000.0.			*/
        //* Arguments:										*/
        //*   jd : the Julian Day to convert						*/
        //* Return value:										*/
        //*   the T value corresponding to the Julian Day				*/
        //***********************************************************************/
        private static double calcTimeJulianCent(double jd)
        {
            double T = (jd - 2451545.0)/36525.0;
            return T;
        }

        //***********************************************************************/
        //* Name:    calcJDFromJulianCent							*/
        //* Type:    Function									*/
        //* Purpose: convert centuries since J2000.0 to Julian Day.			*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   the Julian Day corresponding to the t value				*/
        //***********************************************************************/
        private static double calcJDFromJulianCent(double t)
        {
            double JD = t * 36525.0 + 2451545.0;
            return JD;
        }
        
        //***********************************************************************/
        //* Name:    calGeomMeanLongSun							*/
        //* Type:    Function									*/
        //* Purpose: calculate the Geometric Mean Longitude of the Sun		*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   the Geometric Mean Longitude of the Sun in degrees			*/
        //***********************************************************************/
        private static double calcGeomMeanLongSun(double t)
        {
            double L0 = 280.46646 + t * (36000.76983 + 0.0003032 * t);
            while(L0 > 360.0)
            {
                L0 -= 360.0;
            }
            while(L0 < 0.0)
            {
                L0 += 360.0;
            }
            return L0;		// in degrees
        }
        
        //***********************************************************************/
        //* Name:    calGeomAnomalySun							*/
        //* Type:    Function									*/
        //* Purpose: calculate the Geometric Mean Anomaly of the Sun		*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   the Geometric Mean Anomaly of the Sun in degrees			*/
        //***********************************************************************/
        private static double calcGeomMeanAnomalySun(double t)
        {
            double M = 357.52911 + t * (35999.05029 - 0.0001537 * t);
            return M;		// in degrees
        }

        //***********************************************************************/
        //* Name:    calcEccentricityEarthOrbit						*/
        //* Type:    Function									*/
        //* Purpose: calculate the eccentricity of earth's orbit			*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   the unitless eccentricity							*/
        //***********************************************************************/
        private static double calcEccentricityEarthOrbit(double t)
        {
            double e = 0.016708634 - t * (0.000042037 + 0.0000001267 * t);
            return e;		// unitless
        }

        //***********************************************************************/
        //* Name:    calcSunEqOfCenter							*/
        //* Type:    Function									*/
        //* Purpose: calculate the equation of center for the sun			*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   in degrees										*/
        //***********************************************************************/
        private static double calcSunEqOfCenter(double t)
        {
            double m = calcGeomMeanAnomalySun(t);

            double mrad = degToRad(m);
            double sinm = Math.Sin(mrad);
            double sin2m = Math.Sin(mrad + mrad);
            double sin3m = Math.Sin(mrad + mrad + mrad);

            double C = sinm * (1.914602 - t * (0.004817 + 0.000014 * t)) + sin2m * (0.019993 - 0.000101 * t) + sin3m * 0.000289;
            return C;		// in degrees
        }

        //***********************************************************************/
        //* Name:    calcSunTrueLong								*/
        //* Type:    Function									*/
        //* Purpose: calculate the true longitude of the sun				*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   sun's true longitude in degrees						*/
        //***********************************************************************/
        private static double calcSunTrueLong(double t)
        {
            double l0 = calcGeomMeanLongSun(t);
            double c = calcSunEqOfCenter(t);

            double O = l0 + c;
            return O;		// in degrees
        }

        //***********************************************************************/
        //* Name:    calcSunTrueAnomaly							*/
        //* Type:    Function									*/
        //* Purpose: calculate the true anamoly of the sun				*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   sun's true anamoly in degrees							*/
        //***********************************************************************/
        private static double calcSunTrueAnomaly(double t)
        {
            double m = calcGeomMeanAnomalySun(t);
            double c = calcSunEqOfCenter(t);

            double v = m + c;
            return v;		// in degrees
        }

        //***********************************************************************/
        //* Name:    calcSunRadVector								*/
        //* Type:    Function									*/
        //* Purpose: calculate the distance to the sun in AU				*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   sun radius vector in AUs							*/
        //***********************************************************************/
        private static double calcSunRadVector(double t)
        {
            double v = calcSunTrueAnomaly(t);
            double e = calcEccentricityEarthOrbit(t);

            double R = (1.000001018 * (1 - e * e)) / (1 + e * Math.Cos(degToRad(v)));
            return R;		// in AUs
        }

        //***********************************************************************/
        //* Name:    calcSunApparentLong							*/
        //* Type:    Function									*/
        //* Purpose: calculate the apparent longitude of the sun			*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   sun's apparent longitude in degrees						*/
        //***********************************************************************/
        private static double calcSunApparentLong(double t)
        {
            double o = calcSunTrueLong(t);

            double omega = 125.04 - 1934.136 * t;
            double lambda = o - 0.00569 - 0.00478 * Math.Sin(degToRad(omega));
            return lambda;		// in degrees
        }

        //***********************************************************************/
        //* Name:    calcMeanObliquityOfEcliptic						*/
        //* Type:    Function									*/
        //* Purpose: calculate the mean obliquity of the ecliptic			*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   mean obliquity in degrees							*/
        //***********************************************************************/
        private static double calcMeanObliquityOfEcliptic(double t)
        {
            double seconds = 21.448 - t*(46.8150 + t*(0.00059 - t*(0.001813)));
            double e0 = 23.0 + (26.0 + (seconds/60.0))/60.0;
            return e0;		// in degrees
        }

        //***********************************************************************/
        //* Name:    calcObliquityCorrection						*/
        //* Type:    Function									*/
        //* Purpose: calculate the corrected obliquity of the ecliptic		*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   corrected obliquity in degrees						*/
        //***********************************************************************/
        private static double calcObliquityCorrection(double t)
        {
            double e0 = calcMeanObliquityOfEcliptic(t);

            double omega = 125.04 - 1934.136 * t;
            double e = e0 + 0.00256 * Math.Cos(degToRad(omega));
            return e;		// in degrees
        }

        //***********************************************************************/
        //* Name:    calcSunRtAscension							*/
        //* Type:    Function									*/
        //* Purpose: calculate the right ascension of the sun				*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   sun's right ascension in degrees						*/
        //***********************************************************************/
        private static double calcSunRtAscension(double t)
        {
            double e = calcObliquityCorrection(t);
            double lambda = calcSunApparentLong(t);

            double tananum = (Math.Cos(degToRad(e)) * Math.Sin(degToRad(lambda)));
            double tanadenom = (Math.Cos(degToRad(lambda)));
            double alpha = radToDeg(Math.Atan2(tananum, tanadenom));
            return alpha;		// in degrees
        }

        //***********************************************************************/
        //* Name:    calcSunDeclination							*/
        //* Type:    Function									*/
        //* Purpose: calculate the declination of the sun				*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   sun's declination in degrees							*/
        //***********************************************************************/
        private static double calcSunDeclination(double t)
        {
            double e = calcObliquityCorrection(t);
            double lambda = calcSunApparentLong(t);

            double sint = Math.Sin(degToRad(e)) * Math.Sin(degToRad(lambda));
            double theta = radToDeg(Math.Asin(sint));
            return theta;		// in degrees
        }

        //***********************************************************************/
        //* Name:    calcEquationOfTime							*/
        //* Type:    Function									*/
        //* Purpose: calculate the difference between true solar time and mean	*/
        //*		solar time									*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //* Return value:										*/
        //*   equation of time in minutes of time						*/
        //***********************************************************************/
        private static double calcEquationOfTime(double t)
        {
            double epsilon = calcObliquityCorrection(t);
            double l0 = calcGeomMeanLongSun(t);
            double e = calcEccentricityEarthOrbit(t);
            double m = calcGeomMeanAnomalySun(t);

            double y = Math.Tan(degToRad(epsilon) / 2.0);
            y *= y;

            double sin2l0 = Math.Sin(2.0 * degToRad(l0));
            double sinm = Math.Sin(degToRad(m));
            double cos2l0 = Math.Cos(2.0 * degToRad(l0));
            double sin4l0 = Math.Sin(4.0 * degToRad(l0));
            double sin2m = Math.Sin(2.0 * degToRad(m));

            double Etime = y * sin2l0 - 2.0 * e * sinm + 4.0 * e * y * sinm * cos2l0 - 0.5 * y * y * sin4l0 - 1.25 * e * e * sin2m;

            return radToDeg(Etime)*4.0;	// in minutes of time
        }

        //***********************************************************************/
        //* Name:    calcHourAngleSunrise							*/
        //* Type:    Function									*/
        //* Purpose: calculate the hour angle of the sun at sunrise for the	*/
        //*			latitude								*/
        //* Arguments:										*/
        //*   lat : latitude of observer in degrees					*/
        //*	solarDec : declination angle of sun in degrees				*/
        //* Return value:										*/
        //*   hour angle of sunrise in radians						*/
        //***********************************************************************/
	    private static double calcHourAngleSunrise(double lat, double solarDec)
	    {
		    double latRad = degToRad(lat);
            double sdRad = degToRad(solarDec);

            double HAarg = (Math.Cos(degToRad(90.833)) / (Math.Cos(latRad) * Math.Cos(sdRad)) - Math.Tan(latRad) * Math.Tan(sdRad));

            double HA = (Math.Acos(Math.Cos(degToRad(90.833)) / (Math.Cos(latRad) * Math.Cos(sdRad)) - Math.Tan(latRad) * Math.Tan(sdRad)));

		    return HA;		// in radians
	    }

        //***********************************************************************/
        //* Name:    calcHourAngleSunset							*/
        //* Type:    private static double									*/
        //* Purpose: calculate the hour angle of the sun at sunset for the	*/
        //*			latitude								*/
        //* Arguments:										*/
        //*   lat : latitude of observer in degrees					*/
        //*	solarDec : declination angle of sun in degrees				*/
        //* Return value:										*/
        //*   hour angle of sunset in radians						*/
        //***********************************************************************/
        private static double calcHourAngleSunset(double lat, double solarDec)
            {
                double latRad = degToRad(lat);
                double sdRad = degToRad(solarDec);

                double HAarg = (Math.Cos(degToRad(90.833))/(Math.Cos(latRad)*Math.Cos(sdRad))-Math.Tan(latRad) * Math.Tan(sdRad));

                double HA = (Math.Acos(Math.Cos(degToRad(90.833))/(Math.Cos(latRad)*Math.Cos(sdRad))-Math.Tan(latRad) * Math.Tan(sdRad)));

                return -HA;		// in radians
            }
        
        //***********************************************************************/
        //* Name:    calcSunriseUTC								*/
        //* Type:    private static double									*/
        //* Purpose: calculate the Universal Coordinated Time (UTC) of sunrise	*/
        //*			for the given day at the given location on earth	*/
        //* Arguments:										*/
        //*   JD  : julian day									*/
        //*   latitude : latitude of observer in degrees				*/
        //*   longitude : longitude of observer in degrees				*/
        //* Return value:										*/
        //*   time in minutes from zero Z							*/
        //***********************************************************************/
        private static double calcSunriseUTC(double JD, double latitude, double longitude)
        {
            double t = calcTimeJulianCent(JD);

            // *** Find the time of solar noon at the location, and use
            //     that declination. This is better than start of the 
            //     Julian day

            double noonmin = Convert.ToDouble(calcSolNoonUTC(t, longitude));
            double tnoon = calcTimeJulianCent (JD+noonmin/1440.0);

            // *** First pass to approximate sunrise (using solar noon)

            double eqTime = calcEquationOfTime(tnoon);
            double solarDec = calcSunDeclination(tnoon);
            double hourAngle = calcHourAngleSunrise(latitude, solarDec);

            double delta = longitude - radToDeg(hourAngle);
            double timeDiff = 4 * delta;	// in minutes of time
            double timeUTC = 720 + timeDiff - eqTime;	// in minutes

            // alert("eqTime = " + eqTime + "\nsolarDec = " + solarDec + "\ntimeUTC = " + timeUTC);

            // *** Second pass includes fractional jday in gamma calc

            double newt = calcTimeJulianCent(calcJDFromJulianCent(t) + timeUTC/1440.0); 
            eqTime = calcEquationOfTime(newt);
            solarDec = calcSunDeclination(newt);
            hourAngle = calcHourAngleSunrise(latitude, solarDec);
            delta = longitude - radToDeg(hourAngle);
            timeDiff = 4 * delta;
            timeUTC = 720 + timeDiff - eqTime; // in minutes

            // alert("eqTime = " + eqTime + "\nsolarDec = " + solarDec + "\ntimeUTC = " + timeUTC);

            return timeUTC;
        }

        //***********************************************************************/
        //* Name:    calcSolNoonUTC								*/
        //* Type:    private static double									*/
        //* Purpose: calculate the Universal Coordinated Time (UTC) of solar	*/
        //*		noon for the given day at the given location on earth		*/
        //* Arguments:										*/
        //*   t : number of Julian centuries since J2000.0				*/
        //*   longitude : longitude of observer in degrees				*/
        //* Return value:										*/
        //*   time in minutes from zero Z							*/
        //***********************************************************************/
        private static double calcSolNoonUTC(double t, double longitude)
            {
                // First pass uses approximate solar noon to calculate eqtime
                double tnoon = calcTimeJulianCent(calcJDFromJulianCent(t) + longitude/360.0);
                double eqTime = calcEquationOfTime(tnoon);
                double solNoonUTC = 720 + (longitude * 4) - eqTime; // min

                double newt = calcTimeJulianCent(calcJDFromJulianCent(t) -0.5 + solNoonUTC/1440.0); 

                eqTime = calcEquationOfTime(newt);
                // double solarNoonDec = calcSunDeclination(newt);
                solNoonUTC = 720 + (longitude * 4) - eqTime; // min
        		
                return solNoonUTC;
            }

        //***********************************************************************/
        //* Name:    calcSunsetUTC								*/
        //* Type:    Function									*/
        //* Purpose: calculate the Universal Coordinated Time (UTC) of sunset	*/
        //*			for the given day at the given location on earth	*/
        //* Arguments:										*/
        //*   JD  : julian day									*/
        //*   latitude : latitude of observer in degrees				*/
        //*   longitude : longitude of observer in degrees				*/
        //* Return value:										*/
        //*   time in minutes from zero Z							*/
        //***********************************************************************/
        private static double calcSunsetUTC(double JD, double latitude, double longitude)
        {
            double t = calcTimeJulianCent(JD);

            // *** Find the time of solar noon at the location, and use
            //     that declination. This is better than start of the 
            //     Julian day

            double noonmin = Convert.ToDouble(calcSolNoonUTC(t, longitude));
            double tnoon = calcTimeJulianCent(JD + noonmin / 1440.0);

            // First calculates sunrise and approx length of day

            double eqTime = calcEquationOfTime(tnoon);
            double solarDec = calcSunDeclination(tnoon);
            double hourAngle = calcHourAngleSunset(latitude, solarDec);

            double delta = longitude - radToDeg(hourAngle);
            double timeDiff = 4 * delta;
            double timeUTC = 720 + timeDiff - eqTime;

            // first pass used to include fractional day in gamma calc

            double newt = calcTimeJulianCent(calcJDFromJulianCent(t) + timeUTC / 1440.0);
            eqTime = calcEquationOfTime(newt);
            solarDec = calcSunDeclination(newt);
            hourAngle = calcHourAngleSunset(latitude, solarDec);

            delta = longitude - radToDeg(hourAngle);
            timeDiff = 4 * delta;
            timeUTC = 720 + timeDiff - eqTime; // in minutes

            return timeUTC;
        }
                
        //***********************************************************************/
        //* Name:    findRecentSunrise							*/
        //* Type:    Function									*/
        //* Purpose: calculate the julian day of the most recent sunrise		*/
        //*		starting from the given day at the given location on earth	*/
        //* Arguments:										*/
        //*   JD  : julian day									*/
        //*   latitude : latitude of observer in degrees				*/
        //*   longitude : longitude of observer in degrees				*/
        //* Return value:										*/
        //*   julian day of the most recent sunrise					*/
        //***********************************************************************/
        private static double findRecentSunrise(double jd, double latitude, double longitude)
        {
            double julianday = jd;

            double time = calcSunriseUTC(julianday, latitude, longitude);
            while (double.IsNaN(time))
            {
                julianday -= 1.0;
                time = calcSunriseUTC(julianday, latitude, longitude);
            }

            return julianday;
        }
        
        //***********************************************************************/
        //* Name:    findRecentSunset								*/
        //* Type:    Function									*/
        //* Purpose: calculate the julian day of the most recent sunset		*/
        //*		starting from the given day at the given location on earth	*/
        //* Arguments:										*/
        //*   JD  : julian day									*/
        //*   latitude : latitude of observer in degrees				*/
        //*   longitude : longitude of observer in degrees				*/
        //* Return value:										*/
        //*   julian day of the most recent sunset					*/
        //***********************************************************************/
        private static double findRecentSunset(double jd, double latitude, double longitude)
        {
            double julianday = jd;

            double time = calcSunsetUTC(julianday, latitude, longitude);
            while (double.IsNaN(time))
            {
                julianday -= 1.0;
                time = calcSunsetUTC(julianday, latitude, longitude);
            }

            return julianday;
        }
        
        //***********************************************************************/
        //* Name:    findNextSunrise								*/
        //* Type:    Function									*/
        //* Purpose: calculate the julian day of the next sunrise			*/
        //*		starting from the given day at the given location on earth	*/
        //* Arguments:										*/
        //*   JD  : julian day									*/
        //*   latitude : latitude of observer in degrees				*/
        //*   longitude : longitude of observer in degrees				*/
        //* Return value:										*/
        //*   julian day of the next sunrise						*/
        //***********************************************************************/
        private static double findNextSunrise(double jd, double latitude, double longitude)
        {
            double julianday = jd;

            double time = calcSunriseUTC(julianday, latitude, longitude);
            while (double.IsNaN(time))
            {
                julianday += 1.0;
                time = calcSunriseUTC(julianday, latitude, longitude);
            }

            return julianday;
        }
        
        //***********************************************************************/
        //* Name:    findNextSunset								*/
        //* Type:    Function									*/
        //* Purpose: calculate the julian day of the next sunset			*/
        //*		starting from the given day at the given location on earth	*/
        //* Arguments:										*/
        //*   JD  : julian day									*/
        //*   latitude : latitude of observer in degrees				*/
        //*   longitude : longitude of observer in degrees				*/
        //* Return value:										*/
        //*   julian day of the next sunset							*/
        //***********************************************************************/
        private static double findNextSunset(double jd, double latitude, double longitude)
        {
            double julianday = jd;

            double time = calcSunsetUTC(julianday, latitude, longitude);
            while (double.IsNaN(time))
            {
                julianday += 1.0;
                time = calcSunsetUTC(julianday, latitude, longitude);
            }

            return julianday;
        }


        #endregion

    }

    #region "Moving Windows"

    /// <summary>
    /// A moving window class is a base structure for a variable size moving window terrain analysis. the default is a 3 by 3 mean window
    /// </summary>
    public class MovingWindow : Object
    {
        /// <summary>
        /// height of the window
        /// </summary>
        public virtual int WindowHeight
        {
            get
            {
                return 3;
            }
        }
        
        /// <summary>
        /// width of the window
        /// </summary>
        public virtual int WindowWidth
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// data array of the values from the grid populated for the current window
        /// </summary>
        public float[][] _WindowRowData;
       
        /// <summary>
        /// index of the vertical center row of the windows
        /// </summary>
        protected int _WindowCenterRow = -1;
        /// <summary>
        /// Same
        /// </summary>
        public int WindowCenterRow
        {
            get
            {
                return _WindowCenterRow;
            }
            set
            {
                _WindowCenterRow = value;
            }
        }

        /// <summary>
        /// Current column being examined
        /// </summary>
        protected int _CurrCol = -1;
        /// <summary>
        /// same
        /// </summary>
        public int CurrCol
        {
            get
            {
                return _CurrCol;
            }
            set
            {
                _CurrCol = value;
            }
        }

        /// <summary>
        /// Current row from total grid being examined
        /// </summary>
        protected int _CurrRow = -1;
        /// <summary>
        /// Same
        /// </summary>
        public int CurrRow
        {
            get
            {
                return _CurrRow;
            }
            set
            {
                _CurrRow = value;
            }
        }

        /// <summary>
        /// total number of cols
        /// </summary>
        protected int _NumberCols = -1;
        /// <summary>
        /// total number of rows
        /// </summary>
        protected int _NumberRows = -1;
        /// <summary>
        /// cellsize in x
        /// </summary>
        protected double _DX = -1;
        /// <summary>
        /// cellsize in y
        /// </summary>
        protected double _DY = -1;
        /// <summary>
        /// input nodata value
        /// </summary>
        protected object _InNoDataValue = -1;
        /// <summary>
        /// output nodata value
        /// </summary>
        protected object _OutNoDataValue = -1;

        /// <summary>
        /// output path for result data
        /// </summary>
        protected string _OutputPath = "";
        /// <summary>
        /// same
        /// </summary>
        public string OutputPath
        {
            get
            {
                return _OutputPath;
            }
            set
            {
                _OutputPath = value;
            }
        }

        /// <summary>
        /// flag for whether to us inram or not
        /// </summary>
        protected bool _useInRam = true;
        /// <summary>
        /// Same
        /// </summary>
        public bool UseInRam
        {
            get
            {
                return _useInRam;
            }
            set
            {
                _useInRam = value;
            }
        }

        /// <summary>
        /// Output grid object
        /// </summary>
        protected MapWinGIS.Grid _OutGrid;

        /// <summary>
        /// Whether to invert the input or not (used for depth grids which have data values increasing as they go down, which can screw up some terrain analysis)
        /// </summary>
        protected bool _InvertInput = false;
        /// <summary>
        /// same
        /// </summary>
        public bool InvertInput
        {
            get
            {
                return _InvertInput;
            }
            set
            {
                _InvertInput = value;
            }
        }
        
        /// <summary>
        /// Funtion to initialize the window with output path and header
        /// </summary>
        /// <param name="OutPath"></param>
        /// <param name="InHead"></param>
        /// <returns></returns>
        public bool Initialize(string OutPath, MapWinGIS.GridHeader InHead)
        {
            bool Result;
            if (OutPath != "" && InHead != null)
            {
                _OutputPath = OutPath;
                _OutGrid = new MapWinGIS.Grid();
                MapWinGIS.GridHeader outHead = new MapWinGIS.GridHeader();

                outHead.CopyFrom(InHead);
                outHead.NodataValue = -999999;
                Result = DataManagement.DeleteGrid(ref OutPath);
                if (Result == false)
                {
                    MapWinUtility.Logger.Message("That grid path could not be overwritten.", "Error Creating Window Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                    return false;
                }
                Result = _OutGrid.CreateNew(OutPath, outHead, MapWinGIS.GridDataType.FloatDataType, outHead.NodataValue, _useInRam, MapWinGIS.GridFileType.UseExtension, null);
                if (Result == false)
                {
                    MapWinUtility.Logger.Message(_OutGrid.get_ErrorMsg(_OutGrid.LastErrorCode), "Error Creating Window Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                    return false;
                }
                _OutGrid.Header.Projection = InHead.Projection;
                _DX = outHead.dX;
                _DY = outHead.dY;
                _NumberCols = outHead.NumberCols;
                _NumberRows = outHead.NumberRows;
                _OutNoDataValue = outHead.NodataValue;
                _InNoDataValue = InHead.NodataValue;
            }
            else
            {
                MapWinUtility.Logger.Message("Invalid grid path.", "Error Creating Window Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Overwritable function where actual calculations occur for each column/row
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnCalculate()
        {
            float z1 = _WindowRowData[_WindowCenterRow - 1][_CurrCol - 1];
            float z2 = _WindowRowData[_WindowCenterRow - 1][_CurrCol];
            float z3 = _WindowRowData[_WindowCenterRow - 1][_CurrCol + 1];
            float z4 = _WindowRowData[_WindowCenterRow][_CurrCol - 1];
            float z0 = _WindowRowData[_WindowCenterRow][_CurrCol];
            float z5 = _WindowRowData[_WindowCenterRow][_CurrCol + 1];
            float z6 = _WindowRowData[_WindowCenterRow + 1][_CurrCol - 1];
            float z7 = _WindowRowData[_WindowCenterRow + 1][_CurrCol];
            float z8 = _WindowRowData[_WindowCenterRow + 1][_CurrCol + 1];

            if (_InvertInput)
            {
                z1 = -z1; z2 = -z2; z3 = -z3; z4 = -z4; z0 = -z0; z5 = -z5; z6 = -z6; z7 = -z7; z8 = -z8; 
            }

            float avg = (z1 + z2 + z3 + z4 + z0 + z5 + z6 + z7 + z8)/9;

            _OutGrid.set_Value(_CurrCol, _CurrRow, avg);

            return true;
        }

        /// <summary>
        /// Overwritable function triggered when finished
        /// </summary>
        /// <returns></returns>
        protected virtual bool OnFinish()
        {
            //Do nothing by default. Allow inheriting to overwrite for special functionality
            return true;
        }

        /// <summary>
        /// Main processing function which calls OnCalculate and handles base errors
        /// </summary>
        /// <returns></returns>
        public bool Calculate()
        {
            if (_OutGrid != null)
            {
                if (_WindowRowData != null)
                {
                    if (_WindowCenterRow != -1)
                    {
                        if (_CurrCol != -1 && _CurrRow != -1)
                        {
                            return OnCalculate();
                        }
                        else
                        {
                            MapWinUtility.Logger.Message("The current row and column have not been initialized yet.", "Error Calculating Window Value", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                            return false;
                        }
                    }
                    else
                    {
                        MapWinUtility.Logger.Message("The current window center row has not been initialized yet.", "Error Calculating Window Value", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                        return false;
                    }
                }
                else
                {
                    MapWinUtility.Logger.Message("The window data has not been initialized yet.", "Error Calculating Window Value", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                    return false;
                }                
            }
            else
            {
                MapWinUtility.Logger.Message("The window output grid has not been initialized yet.", "Error Calculating Window Value", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }
        }

        /// <summary>
        /// Finish function which handles output and calls on finish at the end of processing
        /// </summary>
        /// <returns></returns>
        public bool Finish()
        {
            bool Result;
            if (_OutGrid != null)
            {
                Result = _OutGrid.Save(_OutputPath, MapWinGIS.GridFileType.UseExtension, null);
                if (Result == false)
                {
                    MapWinUtility.Logger.Message(_OutGrid.get_ErrorMsg(_OutGrid.LastErrorCode), "Error Saving Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                    return false;
                }
                _OutGrid.Close();
                _OutGrid = null;
            }
            else
            {
                MapWinUtility.Logger.Message("The window output grid has not been initialized yet.", "Error Saving Grid", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                return false;
            }
            
            return OnFinish();
        }

        /// <summary>
        /// Function which cleans up after the moving window has finished
        /// </summary>
        public void Cleanup()
        {
            _OutGrid.Close();
        }
    }

    /// <summary>
    /// 8 by 8 mean moving window
    /// </summary>
    public class Mean5MovingWindow : MovingWindow
    {
        /// <summary>
        /// overwritten height
        /// </summary>
        public override int WindowHeight
        {
            get
            {
                return 5;
            }
        }
        /// <summary>
        /// overwritten width
        /// </summary>
        public override int WindowWidth
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        /// Overwritten calculate for this data
        /// </summary>
        /// <returns></returns>
        protected override bool OnCalculate()
        {
            if (_CurrRow != 0 && _CurrRow != 1 && _CurrRow != (_NumberRows - 2) && _CurrRow != (_NumberRows - 1) && _CurrCol != 0 && _CurrCol != 1 && _CurrCol != (_NumberCols - 2) && _CurrCol != (_NumberCols - 1))
            {
                float total = 0;
                int count = 0;
                for (int tmprow = _WindowCenterRow - 2; tmprow <= _WindowCenterRow + 2; tmprow++)
                {
                    for (int tmpcol = _CurrCol - 2; tmpcol <= _CurrCol + 2; tmpcol++)
                    {
                        float currVal = _WindowRowData[tmprow][tmpcol];
                        float fInNoData = Convert.ToSingle(_InNoDataValue);
                        float fOutNoData = Convert.ToSingle(_OutNoDataValue);
                        if (currVal == fInNoData)
                        {
                            _OutGrid.set_Value(_CurrCol, _CurrRow, fOutNoData);
                            return true;
                        }
                        else
                        {
                            total += currVal;
                            count++;
                        }
                    }
                }

                if (count > 1)
                {
                    float mean = total / count;
                    _OutGrid.set_Value(_CurrCol, _CurrRow, mean);
                }

            }
            else
            {
                _OutGrid.set_Value(_CurrCol, _CurrRow, _OutNoDataValue);
            }

            return true;
        }
    }

    /// <summary>
    /// Slope calculations
    /// </summary>
    public class SlopeMovingWindow : MovingWindow
    {
        private float _zFactor = Convert.ToSingle(1.0);
        /// <summary>
        /// Public zfactor
        /// </summary>
        public float zFactor
        {
            get
            {
                return _zFactor;
            }
            set
            {
                _zFactor = value;
            }
        }

        private bool _slopeInPercent = false;
        /// <summary>
        /// Public boolean for whether to output percent slope or not
        /// </summary>
        public bool slopeInPercent
        {
            get
            {
                return _slopeInPercent;
            }
            set
            {
                _slopeInPercent = value;
            }
        }

        private double total = 0;
        private double sqrTotal = 0;
        private int count = 0;


        /// <summary>
        /// Overwritten Calculate
        /// </summary>
        /// <returns></returns>
        protected override bool OnCalculate()
        {
            float fInNoData = Convert.ToSingle(_InNoDataValue);
            float fOutNoData = Convert.ToSingle(_OutNoDataValue);
            float z0, z1, z2, z3, z4, z5, z6, z7, z8;

            //float z1 = _WindowRowData[_WindowCenterRow - 1][_CurrCol - 1];
            //float z2 = _WindowRowData[_WindowCenterRow - 1][_CurrCol];
            //float z3 = _WindowRowData[_WindowCenterRow - 1][_CurrCol + 1];
            //float z4 = _WindowRowData[_WindowCenterRow][_CurrCol - 1];
            //float z0 = _WindowRowData[_WindowCenterRow][_CurrCol];
            //float z5 = _WindowRowData[_WindowCenterRow][_CurrCol + 1];
            //float z6 = _WindowRowData[_WindowCenterRow + 1][_CurrCol - 1];
            //float z7 = _WindowRowData[_WindowCenterRow + 1][_CurrCol];
            //float z8 = _WindowRowData[_WindowCenterRow + 1][_CurrCol + 1];

            z0 = _WindowRowData[_WindowCenterRow][_CurrCol];

            if (_CurrRow == 0)
            {
                if (_CurrCol == 0)
                {
                    z4 = z0;
                    z5 = _WindowRowData[_WindowCenterRow][_CurrCol + 1];
                    z6 = z0;
                    z7 = _WindowRowData[_WindowCenterRow + 1][_CurrCol];
                    z8 = _WindowRowData[_WindowCenterRow + 1][_CurrCol + 1];
                }
                else if (_CurrCol == (_NumberCols - 1))
                {
                    z4 = _WindowRowData[_WindowCenterRow][_CurrCol - 1];
                    z5 = z0;
                    z6 = _WindowRowData[_WindowCenterRow + 1][_CurrCol - 1];
                    z7 = _WindowRowData[_WindowCenterRow + 1][_CurrCol];
                    z8 = z0;
                }
                else
                {
                    z4 = _WindowRowData[_WindowCenterRow][_CurrCol - 1];
                    z5 = _WindowRowData[_WindowCenterRow][_CurrCol + 1];
                    z6 = _WindowRowData[_WindowCenterRow + 1][_CurrCol - 1];
                    z7 = _WindowRowData[_WindowCenterRow + 1][_CurrCol];
                    z8 = _WindowRowData[_WindowCenterRow + 1][_CurrCol + 1];
                }

                z1 = z0; z2 = z0; z3 = z0;
            }
            else if (_CurrRow == (_NumberRows - 1))
            {
                if (_CurrCol == 0)
                {
                    z1 = z0;
                    z2 = _WindowRowData[_WindowCenterRow - 1][_CurrCol];
                    z3 = _WindowRowData[_WindowCenterRow - 1][_CurrCol + 1];
                    z4 = z0;
                    z5 = _WindowRowData[_WindowCenterRow][_CurrCol + 1];
                }
                else if (_CurrCol == (_NumberCols - 1))
                {
                    z1 = _WindowRowData[_WindowCenterRow - 1][_CurrCol - 1];
                    z2 = _WindowRowData[_WindowCenterRow - 1][_CurrCol];
                    z3 = z0;
                    z4 = _WindowRowData[_WindowCenterRow][_CurrCol - 1];
                    z5 = z0;
                }
                else
                {
                    z1 = _WindowRowData[_WindowCenterRow - 1][_CurrCol - 1];
                    z2 = _WindowRowData[_WindowCenterRow - 1][_CurrCol];
                    z3 = _WindowRowData[_WindowCenterRow - 1][_CurrCol + 1];
                    z4 = _WindowRowData[_WindowCenterRow][_CurrCol - 1];
                    z5 = _WindowRowData[_WindowCenterRow][_CurrCol + 1];
                }

                z6 = z0; z7 = z0; z8 = z0;
            }
            else
            {
                if (_CurrCol == 0)
                {
                    z1 = z0;
                    z2 = _WindowRowData[_WindowCenterRow - 1][_CurrCol];
                    z3 = _WindowRowData[_WindowCenterRow - 1][_CurrCol + 1];
                    z4 = z0;
                    z5 = _WindowRowData[_WindowCenterRow][_CurrCol + 1];
                    z6 = z0;
                    z7 = _WindowRowData[_WindowCenterRow + 1][_CurrCol];
                    z8 = _WindowRowData[_WindowCenterRow + 1][_CurrCol + 1];
                }
                else if (_CurrCol == (_NumberCols - 1))
                {
                    z1 = _WindowRowData[_WindowCenterRow - 1][_CurrCol - 1];
                    z2 = _WindowRowData[_WindowCenterRow - 1][_CurrCol];
                    z3 = z0;
                    z4 = _WindowRowData[_WindowCenterRow][_CurrCol - 1];
                    z5 = z0;
                    z6 = _WindowRowData[_WindowCenterRow + 1][_CurrCol - 1];
                    z7 = _WindowRowData[_WindowCenterRow + 1][_CurrCol];
                    z8 = z0;
                }
                else
                {
                    z1 = _WindowRowData[_WindowCenterRow - 1][_CurrCol - 1];
                    z2 = _WindowRowData[_WindowCenterRow - 1][_CurrCol];
                    z3 = _WindowRowData[_WindowCenterRow - 1][_CurrCol + 1];
                    z4 = _WindowRowData[_WindowCenterRow][_CurrCol - 1];
                    z5 = _WindowRowData[_WindowCenterRow][_CurrCol + 1];
                    z6 = _WindowRowData[_WindowCenterRow + 1][_CurrCol - 1];
                    z7 = _WindowRowData[_WindowCenterRow + 1][_CurrCol];
                    z8 = _WindowRowData[_WindowCenterRow + 1][_CurrCol + 1];
                }
            }

            if (z0 == fInNoData)
            {
                _OutGrid.set_Value(_CurrCol, _CurrRow, _OutNoDataValue);
            }
            else
            {

                if (z1 == fInNoData) z1 = z0;
                if (z2 == fInNoData) z2 = z0;
                if (z3 == fInNoData) z3 = z0;
                if (z4 == fInNoData) z4 = z0;
                if (z5 == fInNoData) z5 = z0;
                if (z6 == fInNoData) z6 = z0;
                if (z7 == fInNoData) z7 = z0;
                if (z8 == fInNoData) z8 = z0;

                if (_InvertInput)
                {
                    z1 = -z1; z2 = -z2; z3 = -z3; z4 = -z4; z0 = -z0; z5 = -z5; z6 = -z6; z7 = -z7; z8 = -z8;
                }
                //a b c  1 2 3
                //d e f  4 0 5
                //g h i  6 7 8
                //dz_dx esri
                //((c + 2f + i) - (a + 2d + g)) / 8*dx
                //(c + 2f + i - a - 2d - g) / 8*dx
                //equiv bolstad 
                //((c - a) + 2 (f - d) + (i - g))/8*dx
                //(c + 2f + i - a - 2d -g)/8*dx
                //dz_dy esri
                //((g + 2h + i) - (a + 2b + c)) / 8*dx
                //(g + 2h + i - a - 2b - c) / 8*dx
                //equiv bolstad 
                //((a - g) + 2 (b - h) + (c - i))/8*dy
                //(a + 2b + c - g - 2h - i)/8*dy (negative of esri thereby the same after squaring)
                float dX = Convert.ToSingle(_DX);
                float dY = Convert.ToSingle(_DY);
                float dZ_dx = (_zFactor * ((z3 - z1) + 2 * (z5 - z4) + (z8 - z6))) / (8 * dX);
                float dZ_dy = (_zFactor * ((z1 - z6) + 2 * (z2 - z7) + (z3 - z8))) / (8 * dY);

                float slope = Convert.ToSingle(Math.Atan(Math.Sqrt((dZ_dx * dZ_dx) + (dZ_dy * dZ_dy))) * (180 / Math.PI));

                if (_slopeInPercent)
                {
                    slope = Convert.ToSingle(Math.Tan(slope * (Math.PI / 180)) * 100);
                }

                _OutGrid.set_Value(_CurrCol, _CurrRow, slope);
                total += slope;
                sqrTotal += slope * slope;
                count++;
            }
            
            return true;
        }

        /// <summary>
        /// Overwritten finish
        /// </summary>
        /// <returns></returns>
        protected override bool OnFinish()
        {
            float StdDeviation = (float)Math.Sqrt((sqrTotal / count) - (total / count) * (total / count));
            System.IO.StreamWriter output = new System.IO.StreamWriter(_OutputPath + "_stdev.txt", false);
            output.Write(StdDeviation);
            output.Close();

            double mean = total / count;
            output = new System.IO.StreamWriter(_OutputPath + "_mean.txt", false);
            output.Write(mean);
            output.Close();

            return true;
        }
    }

    /// <summary>
    /// Aspect calculation
    /// </summary>
    public class AspectMovingWindow : MovingWindow
    {
        private float _zFactor = Convert.ToSingle(1.0);
        /// <summary>
        /// Public zfactor
        /// </summary>
        public float zFactor
        {
            get
            {
                return _zFactor;
            }
            set
            {
                _zFactor = value;
            }
        }

        /// <summary>
        /// Overwritten Calculate
        /// </summary>
        /// <returns></returns>
        protected override bool OnCalculate()
        {
            
            if (_CurrRow != 0 && _CurrRow != (_NumberRows - 1) && _CurrCol != 0 && _CurrCol != (_NumberCols - 1))
            {
                float z1 = _WindowRowData[_WindowCenterRow - 1][_CurrCol - 1];
                float z2 = _WindowRowData[_WindowCenterRow - 1][_CurrCol];
                float z3 = _WindowRowData[_WindowCenterRow - 1][_CurrCol + 1];
                float z4 = _WindowRowData[_WindowCenterRow][_CurrCol - 1];
                float z0 = _WindowRowData[_WindowCenterRow][_CurrCol];
                float z5 = _WindowRowData[_WindowCenterRow][_CurrCol + 1];
                float z6 = _WindowRowData[_WindowCenterRow + 1][_CurrCol - 1];
                float z7 = _WindowRowData[_WindowCenterRow + 1][_CurrCol];
                float z8 = _WindowRowData[_WindowCenterRow + 1][_CurrCol + 1];

                float fInNoData = Convert.ToSingle(_InNoDataValue);
                if (z1 == fInNoData || z2 == fInNoData || z3 == fInNoData || z4 == fInNoData || z0 == fInNoData || z5 == fInNoData || z6 == fInNoData || z7 == fInNoData || z8 == fInNoData)
                {
                    _OutGrid.set_Value(_CurrCol, _CurrRow, _OutNoDataValue);
                }
                else
                {
                    if (_InvertInput)
                    {
                        z1 = -z1; z2 = -z2; z3 = -z3; z4 = -z4; z0 = -z0; z5 = -z5; z6 = -z6; z7 = -z7; z8 = -z8;
                    }
                    float dX = Convert.ToSingle(_DX);
                    float dY = Convert.ToSingle(_DY);
                    float dZ_dx = (_zFactor * ((z3 - z1) + 2 * (z5 - z4) + (z8 - z6))) / (8 * dX);
                    float dZ_dy = (_zFactor * ((z1 - z6) + 2 * (z2 - z7) + (z3 - z8))) / (8 * dY);

                    float aspect = Convert.ToSingle(Math.Atan2(-dZ_dy, -dZ_dx) * (180 / Math.PI));

                    if (dZ_dx == 0 && dZ_dy == 0) //have to do this to match esri output for flat areas
                    {
                        aspect = -1;
                    }
                    else if (aspect < 0)
                    {
                        aspect = Convert.ToSingle(90.0 - aspect);
                    }
                    else if (aspect > 90)
                    {
                        aspect = Convert.ToSingle(360.0 - aspect + 90.0);
                    }
                    else
                    {
                        aspect = Convert.ToSingle(90.0 - aspect);
                    }


                    _OutGrid.set_Value(_CurrCol, _CurrRow, aspect);
                }
            }
            else
            {
                _OutGrid.set_Value(_CurrCol, _CurrRow, _OutNoDataValue);
            }
        
            return true;
        }
    }

    /// <summary>
    /// Profile curvature calculation
    /// </summary>
    public class ProfileCurvatureMovingWindow : MovingWindow
    {
        private double total = 0;
        private double sqrTotal = 0;
        private int count = 0;

        /// <summary>
        /// Overwritten Calculate
        /// </summary>
        /// <returns></returns>
        protected override bool OnCalculate()
        {
            if (_CurrRow != 0 && _CurrRow != (_NumberRows - 1) && _CurrCol != 0 && _CurrCol != (_NumberCols - 1))
            {
                float z1 = _WindowRowData[_WindowCenterRow - 1][_CurrCol - 1];
                float z2 = _WindowRowData[_WindowCenterRow - 1][_CurrCol];
                float z3 = _WindowRowData[_WindowCenterRow - 1][_CurrCol + 1];
                float z4 = _WindowRowData[_WindowCenterRow][_CurrCol - 1];
                float z0 = _WindowRowData[_WindowCenterRow][_CurrCol];
                float z5 = _WindowRowData[_WindowCenterRow][_CurrCol + 1];
                float z6 = _WindowRowData[_WindowCenterRow + 1][_CurrCol - 1];
                float z7 = _WindowRowData[_WindowCenterRow + 1][_CurrCol];
                float z8 = _WindowRowData[_WindowCenterRow + 1][_CurrCol + 1];

                float fNoData = Convert.ToSingle(_InNoDataValue);
                if (z1 == fNoData || z2 == fNoData || z3 == fNoData || z4 == fNoData || z0 == fNoData || z5 == fNoData || z6 == fNoData || z7 == fNoData || z8 == fNoData)
                {
                    _OutGrid.set_Value(_CurrCol, _CurrRow, _OutNoDataValue);
                }
                else
                {
                    if (_InvertInput)
                    {
                        z1 = -z1; z2 = -z2; z3 = -z3; z4 = -z4; z0 = -z0; z5 = -z5; z6 = -z6; z7 = -z7; z8 = -z8;
                    }
                    float dX = Convert.ToSingle(_DX);
                    //bolstad second edition coefficients match ESRI coefficients
                    double D = (((z4 + z5) / 2) - z0) / Math.Pow(dX, 2);
                    double E = (((z2 + z7) / 2) - z0) / Math.Pow(dX, 2);
                    double F = (z3 - z1 + z6 - z8) / (4 * Math.Pow(dX, 2));
                    double G = (z5 - z4) / (2 * dX);
                    double H = (z2 - z7) / (2 * dX);
                    double profCurve;

                    if (G == 0 && H == 0)
                    {
                        profCurve = 0;
                    }
                    else
                    {
                        profCurve = (200 * (D * Math.Pow(G, 2) + E * Math.Pow(H, 2) + F * G * H)) / (Math.Pow(G, 2) + Math.Pow(H, 2));
                    }
                    
                    _OutGrid.set_Value(_CurrCol, _CurrRow, Convert.ToSingle(profCurve));
                    total += profCurve;
                    sqrTotal += profCurve * profCurve;
                    count++;
                }
            }
            else
            {
                _OutGrid.set_Value(_CurrCol, _CurrRow, _OutNoDataValue);
            }
        
            return true;
        }

        /// <summary>
        /// Overwritten finish
        /// </summary>
        /// <returns></returns>
        protected override bool OnFinish()
        {
            float StdDeviation = (float)Math.Sqrt((sqrTotal / count) - (total / count) * (total / count));
            System.IO.StreamWriter output = new System.IO.StreamWriter(_OutputPath + "_stdev.txt", false);
            output.Write(StdDeviation);
            output.Close();
            return true;
        }
    }

    /// <summary>
    /// Planiform curvature calculation
    /// </summary>
    public class PlanCurvatureMovingWindow : MovingWindow
    {
        private double total = 0;
        private double sqrTotal = 0;
        private int count = 0;

        /// <summary>
        /// Overwritten Calculate
        /// </summary>
        /// <returns></returns>
        protected override bool OnCalculate()
        {            
            if (_CurrRow != 0 && _CurrRow != (_NumberRows - 1) && _CurrCol != 0 && _CurrCol != (_NumberCols - 1))
            {
                float z1 = _WindowRowData[_WindowCenterRow - 1][_CurrCol - 1];
                float z2 = _WindowRowData[_WindowCenterRow - 1][_CurrCol];
                float z3 = _WindowRowData[_WindowCenterRow - 1][_CurrCol + 1];
                float z4 = _WindowRowData[_WindowCenterRow][_CurrCol - 1];
                float z0 = _WindowRowData[_WindowCenterRow][_CurrCol];
                float z5 = _WindowRowData[_WindowCenterRow][_CurrCol + 1];
                float z6 = _WindowRowData[_WindowCenterRow + 1][_CurrCol - 1];
                float z7 = _WindowRowData[_WindowCenterRow + 1][_CurrCol];
                float z8 = _WindowRowData[_WindowCenterRow + 1][_CurrCol + 1];

                float fNoData = Convert.ToSingle(_InNoDataValue);
                if (z1 == fNoData || z2 == fNoData || z3 == fNoData || z4 == fNoData || z0 == fNoData || z5 == fNoData || z6 == fNoData || z7 == fNoData || z8 == fNoData)
                {
                    _OutGrid.set_Value(_CurrCol, _CurrRow, _OutNoDataValue);
                }
                else
                {
                    if (_InvertInput)
                    {
                        z1 = -z1; z2 = -z2; z3 = -z3; z4 = -z4; z0 = -z0; z5 = -z5; z6 = -z6; z7 = -z7; z8 = -z8;
                    }
                    float dX = Convert.ToSingle(_DX);
                    //bolstad second edition coefficients match ESRI coefficients
                    double D = (((z4 + z5) / 2) - z0) / Math.Pow(dX, 2);
                    double E = (((z2 + z7) / 2) - z0) / Math.Pow(dX, 2);
                    double F = (z3 - z1 + z6 - z8) / (4 * Math.Pow(dX, 2));
                    double G = (z5 - z4) / (2 * dX);
                    double H = (z2 - z7) / (2 * dX);
                    double planCurve;

                    if (G == 0 && H == 0)
                    {
                        planCurve = 0;
                    }
                    else
                    {
                        planCurve = (-200 * (D * Math.Pow(H, 2) + E * Math.Pow(G, 2) - F * G * H)) / (Math.Pow(G, 2) + Math.Pow(H, 2));
                    }

                    _OutGrid.set_Value(_CurrCol, _CurrRow, Convert.ToSingle(planCurve));
                    total += planCurve;
                    sqrTotal += planCurve * planCurve;
                    count++;
                }
            }
            else
            {
                _OutGrid.set_Value(_CurrCol, _CurrRow, _OutNoDataValue);
            }

            return true;
        }

        /// <summary>
        /// Overwritten finish
        /// </summary>
        /// <returns></returns>
        protected override bool OnFinish()
        {
            float StdDeviation = (float)Math.Sqrt((sqrTotal / count) - (total / count) * (total / count));
            System.IO.StreamWriter output = new System.IO.StreamWriter(_OutputPath +  "_stdev.txt", false);
            output.Write(StdDeviation);
            output.Close();
            return true;
        }
    }

    /// <summary>
    /// Hillshade calculation
    /// </summary>
    public class HillshadeMovingWindow : MovingWindow
    {
        private double total = 0;
        private double sqrTotal = 0;
        private int count = 0;

        private float _Altitude = Convert.ToSingle(-1.0);
        /// <summary>
        /// Solar altitude input
        /// </summary>
        public float Altitude
        {
            get
            {
                return _Altitude;
            }
            set
            {
                _Altitude = value;
            }
        }

        private float _Azimuth = Convert.ToSingle(-1.0);
        /// <summary>
        /// Solar azimuth input
        /// </summary>
        public float Azimuth
        {
            get
            {
                return _Azimuth;
            }
            set
            {
                _Azimuth = value;
            }
        }

        /// <summary>
        /// Overwritten Calculate
        /// </summary>
        /// <returns></returns>
        protected override bool OnCalculate()
        {
            if (_Altitude != -1.0 && _Azimuth != -1.0)
            {                
                if (_CurrRow != 0 && _CurrRow != (_NumberRows - 1) && _CurrCol != 0 && _CurrCol != (_NumberCols - 1))
                {
                    float z1 = _WindowRowData[_WindowCenterRow - 1][_CurrCol - 1];
                    float z2 = _WindowRowData[_WindowCenterRow - 1][_CurrCol];
                    float z3 = _WindowRowData[_WindowCenterRow - 1][_CurrCol + 1];
                    float z4 = _WindowRowData[_WindowCenterRow][_CurrCol - 1];
                    float z0 = _WindowRowData[_WindowCenterRow][_CurrCol];
                    float z5 = _WindowRowData[_WindowCenterRow][_CurrCol + 1];
                    float z6 = _WindowRowData[_WindowCenterRow + 1][_CurrCol - 1];
                    float z7 = _WindowRowData[_WindowCenterRow + 1][_CurrCol];
                    float z8 = _WindowRowData[_WindowCenterRow + 1][_CurrCol + 1];

                    float fNoData = Convert.ToSingle(_InNoDataValue);
                    if (z1 == fNoData || z2 == fNoData || z3 == fNoData || z4 == fNoData || z0 == fNoData || z5 == fNoData || z6 == fNoData || z7 == fNoData || z8 == fNoData)
                    {
                        _OutGrid.set_Value(_CurrCol, _CurrRow, _OutNoDataValue);
                    }
                    else
                    {
                        if (_InvertInput)
                        {
                            z1 = -z1; z2 = -z2; z3 = -z3; z4 = -z4; z0 = -z0; z5 = -z5; z6 = -z6; z7 = -z7; z8 = -z8;
                        }
                        //a b c  1 2 3
                        //d e f  4 0 5
                        //g h i  6 7 8
                        //dz_dx esri
                        //((c + 2f + i) - (a + 2d + g)) / 8*dx
                        //(c + 2f + i - a - 2d - g) / 8*dx
                        //equiv bolstad 
                        //((c - a) + 2 (f - d) + (i - g))/8*dx
                        //(c + 2f + i - a - 2d -g)/8*dx
                        //dz_dy esri
                        //((g + 2h + i) - (a + 2b + c)) / 8*dx
                        //(g + 2h + i - a - 2b - c) / 8*dx
                        //equiv bolstad 
                        //((a - g) + 2 (b - h) + (c - i))/8*dy
                        //(a + 2b + c - g - 2h - i)/8*dy (negative of esri thereby the same after squaring)
                        double dX = _DX;
                        double dY = _DY;
                        double dZ_dx = ((z3 + 2 * z5 + z8) - (z1 + 2 * z4 + z6)) / (8 * dX);
                        double dZ_dy = ((z6 + 2 * z7 + z8) - (z1 + 2 * z2 + z3)) / (8 * dY);

                        double slope_rad = Math.Atan(Math.Sqrt((dZ_dx * dZ_dx) + (dZ_dy * dZ_dy)));

                        double aspect_rad = Math.Atan2(dZ_dy, -dZ_dx);

                        if (dZ_dx != 0)
                        {
                            if (aspect_rad < 0)
                            {
                                aspect_rad = 2 * Math.PI + aspect_rad;
                            }
                        }
                        else
                        {
                            if (dZ_dy > 0)
                            {
                                aspect_rad = Math.PI / 2;
                            }
                            else if (dZ_dy < 0)
                            {
                                aspect_rad = 2 * Math.PI - Math.PI / 2;
                            }
                            else
                            {
                                aspect_rad = -1;
                            }
                        }


                        //taken from http://webhelp.esri.com/arcgisdesktop/9.2/index.cfm?TopicName=How%20Hillshade%20works
                        double zenith_rad = (90.0 - _Altitude) * (Math.PI / 180.0);
                        double azimuth_math = 360.0 - _Azimuth + 90.0;
                        if (azimuth_math >= 360.0)
                        {
                            azimuth_math = azimuth_math - 360.0;
                        }
                        double azimuth_rad = azimuth_math * (Math.PI / 180.0);

                        //255.0 * ( ( cos(Zenith_rad) * cos(Slope_rad) ) + ( sin(Zenith_rad) * sin(Slope_rad) * cos(Azimuth_rad - Aspect_rad) ) )
                        float hillshade = Convert.ToSingle(Math.Floor(255.0 * ((Math.Cos(zenith_rad) * Math.Cos(slope_rad)) + (Math.Sin(zenith_rad) * Math.Sin(slope_rad) * Math.Cos(azimuth_rad - aspect_rad)))));

                        if (hillshade >= 0)
                        {
                            _OutGrid.set_Value(_CurrCol, _CurrRow, hillshade);
                            total += hillshade;
                            sqrTotal += hillshade * hillshade;
                            count++;
                        }
                        else
                        {
                            _OutGrid.set_Value(_CurrCol, _CurrRow, _OutNoDataValue);
                        }
                    }
                }
                else
                {
                    _OutGrid.set_Value(_CurrCol, _CurrRow, _OutNoDataValue);
                }

                return true;
            }
            else
            {
                //pass message that azi/alt not set
                return false;
            }
        }

        /// <summary>
        /// Overwritten finish
        /// </summary>
        /// <returns></returns>
        protected override bool OnFinish()
        {
            float StdDeviation = (float)Math.Sqrt((sqrTotal / count) - (total / count) * (total / count));
            System.IO.StreamWriter output = new System.IO.StreamWriter(_OutputPath + "_stdev.txt", false);
            output.Write(StdDeviation);
            output.Close();

            double mean = total / count;
            output = new System.IO.StreamWriter(_OutputPath + "_mean.txt", false);
            output.Write(mean);
            output.Close();
            return true;
        }
    }

    #endregion
}
