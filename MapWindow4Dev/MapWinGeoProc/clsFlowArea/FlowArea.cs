//********************************************************************************************************
//File name: FlowArea.cs
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
//06/06/06 ARA  Allen Anselmo   Created
//********************************************************************************************************

using System;
using System.Diagnostics;
using System.Collections;

namespace MapWinGeoProc
{
    /// <summary>
    /// FlowArea provides users the ability to form flow areas from a D8 grid, source polygon, and stream network
    /// </summary>
    ///
    public class FlowArea
    {
        /// <summary>
        /// Generates a flow area analysis polygon from a given D8 grid, source polygon, and stream network
        /// <param name="currD8Path">The string path to the D8 grid</param>
        /// <param name="sourcePath">The string path to the source polygon shapefile</param>
        /// <param name="idxSelected">The index of the shape in the sourcePath shapefile which is to be used as the initiation zone</param>
        /// <param name="currStreamPath">The string path to the stream network polyline shapefile</param>
        /// <param name="demPath">The string path to the DEM used to calculate average slope of flow area polygons</param>
        /// <param name="zFactor">The z factor used for the slope calculations</param>
        /// <param name="outFlowShapePath">The output polygon shapefile path which will have the flow areas</param>
        /// <param name="callback">A callback object which provides progress and status messages</param>
        /// </summary>
        ///
        public static void FormFlowAreas(string currD8Path, string sourcePath, int idxSelected, string currStreamPath, string demPath, double zFactor, string outFlowShapePath, MapWinGIS.ICallback callback)
        {
            MapWinUtility.Logger.Dbg("FormFlowAreas(currD8Path: " + currD8Path + ",\n" +
                                     "              sourcePath: " + sourcePath + ",\n" + 
                                     "              idxSelected: " + idxSelected.ToString() + ",\n" +
                                     "              currStreamPath: " + currStreamPath + ",\n" + 
                                     "              demPath: " + demPath + ",\n" + 
                                     "              zFactor: " + zFactor.ToString() + ",\n" + 
                                     "              outFlowShapePath: " + outFlowShapePath.ToString() + ",\n" +
                                     "              ICallback)");
            int Prog = 0;
            int OldProg = 0;
            string newStreamPath;
            MapWinGIS.Grid gridD8 = new MapWinGIS.Grid();
            MapWinGIS.Grid gridNew = new MapWinGIS.Grid();
            MapWinGIS.Grid gridStream = new MapWinGIS.Grid();
            MapWinGIS.Shapefile sf = new MapWinGIS.Shapefile();
            MapWinGIS.Utils u = new MapWinGIS.Utils();
            MapWinGIS.GridHeader head = new MapWinGIS.GridHeader();
            int d8Type;
            double x;
            double y;
            int currCol;
            int currRow;
            string flowGridPath;
            double dXHalf;
            double dYHalf;
            FlowArea flowArea = new FlowArea();
            if(callback!= null)callback.Progress("Status", 0, "Processing Files...");
            MapWinUtility.Logger.Progress("Processing Files...", Prog, OldProg);
            
            flowGridPath = System.IO.Path.GetDirectoryName(outFlowShapePath) + "\\" + System.IO.Path.GetFileNameWithoutExtension(outFlowShapePath) + "g.tif";
            DataManagement.DeleteGrid(ref flowGridPath);
            DataManagement.DeleteShapefile(ref outFlowShapePath);
            flowArea.createStreamGrid(currStreamPath, currD8Path, out newStreamPath, callback);
            gridStream.Open(newStreamPath, MapWinGIS.GridDataType.DoubleDataType, true, MapWinGIS.GridFileType.Binary, callback);
            
            if(callback!= null)callback.Progress("Status", 0, "Creating Flow Area Grid...");
            MapWinUtility.Logger.Progress("Creating Flow Area Grid...", Prog, OldProg);

            gridD8.Open(currD8Path, MapWinGIS.GridDataType.ShortDataType, true,MapWinGIS.GridFileType.UseExtension, callback);
            d8Type = System.Convert.ToInt32( gridD8.Maximum);
            // Chris M 12/14/2006 head = gridD8.Header;
            head.CopyFrom(gridD8.Header);
            head.NodataValue = -1;
            gridNew.CreateNew(flowGridPath, head, MapWinGIS.GridDataType.DoubleDataType, -1,true, MapWinGIS.GridFileType.UseExtension, callback);
            gridNew.Save(flowGridPath,MapWinGIS.GridFileType.UseExtension, callback);

            sf.Open(sourcePath, callback);
            sf.BeginPointInShapefile();
            if(callback != null)callback.Progress("Status", 0, "Forming Flow Areas...");
            MapWinUtility.Logger.Progress("Forming Flow Areas...", Prog, OldProg);

            int rowStart;
            int rowStop;
            int colStart;
            int colStop;
            gridD8.ProjToCell(sf.get_Shape(idxSelected).Extents.xMin, sf.get_Shape(idxSelected).Extents.yMax, out colStart, out rowStart);
            gridD8.ProjToCell(sf.get_Shape(idxSelected).Extents.xMax, sf.get_Shape(idxSelected).Extents.yMin, out colStop, out rowStop);
            dXHalf = gridD8.Header.dX / 2;
            dYHalf = gridD8.Header.dY / 2;
            
            for (int i = rowStart - 1; i <= rowStop + 1; i++)
            {
                Prog = System.Convert.ToInt32(((System.Convert.ToDouble(i) - System.Convert.ToDouble(rowStart - 1)) / (System.Convert.ToDouble(rowStop + 1) - System.Convert.ToDouble(rowStart - 1))) * 100.0);
                if(Prog > OldProg)
                {
                    if (callback != null) callback.Progress("Progress", Prog, "Forming Flow Areas..." + Prog + "% Complete");
                     MapWinUtility.Logger.Progress("Forming Flow Areas..." + Prog + "% Complete", Prog, OldProg);
                     OldProg = Prog;
                }
                
               
                currRow = i;
                for (int j = colStart - 1; j <= colStop + 1; j++)
                {
                    currCol = j;
                    gridD8.CellToProj(currCol, currRow, out x, out y);
                    if (sf.PointInShape(idxSelected, x, y) | sf.PointInShape(idxSelected, x - dXHalf, y - dYHalf) | sf.PointInShape(idxSelected, x - dXHalf, y + dYHalf) | sf.PointInShape(idxSelected, x + dXHalf, y - dYHalf) | sf.PointInShape(idxSelected, x + dXHalf, y + dYHalf))
                    {
                        gridNew.set_Value(currCol, currRow, 1);
                        flowArea.setDownStreamArea(gridNew, gridD8, gridStream, currCol, currRow, d8Type);
                        flowArea.setUpStreamArea(gridNew, gridD8, currCol, currRow, d8Type);
                    }
                }
            }
            Prog = 0;
            OldProg = 0;

            sf.EndPointInShapefile();
            gridD8.Close();
            gridStream.Close();
            gridNew.Save(flowGridPath,MapWinGIS.GridFileType.UseExtension,callback);
            gridNew.Close();
            if(callback != null)callback.Progress("Status", 0, "Flow Grid to Shapefile");
            MapWinUtility.Logger.Progress("Converting Flow Grid To Shapefile...", Prog, OldProg);
            flowArea.SimpleRasterToPolygon(flowGridPath, outFlowShapePath, callback);
            flowArea.splitAreasOnStreams(outFlowShapePath, currStreamPath, callback);
            flowArea.findAverageSlope(demPath, zFactor, outFlowShapePath, callback);
            if(callback != null)callback.Progress("Status", 0, "");
            MapWinUtility.Logger.Progress("", 0, 0);
            MapWinUtility.Logger.Dbg("Finished FlowAreas");
        }

        private void setDownStreamArea( MapWinGIS.Grid gridNew,  MapWinGIS.Grid gridD8, MapWinGIS.Grid gridStream, int col, int row, int d8Type)
        {
            System.Collections.Stack stkDownCell = new System.Collections.Stack();
            int currRow;
            int currCol;
            int cnt;
            System.Collections.ArrayList lstDownCells = new System.Collections.ArrayList();
            double nodata;
            nodata = System.Convert.ToDouble(gridNew.Header.NodataValue);                
            cnt = 0;
            getDownstreamCell(gridD8, col, row, lstDownCells, d8Type);
            for (int i = 0; i <= lstDownCells.Count - 1; i++)
            {
                stkDownCell.Push(lstDownCells[i]);
            }
            while (stkDownCell.Count > 0)
            {
                currRow = System.Convert.ToInt32(stkDownCell.Pop());
                currCol = System.Convert.ToInt32(stkDownCell.Pop());
                if (System.Convert.ToInt32(gridNew.get_Value(currCol, currRow)) != 1)
                {
                    cnt = cnt + 1;
                    gridNew.set_Value(currCol, currRow, 2);
                    if (System.Convert.ToInt32(gridStream.get_Value(currCol, currRow)) == -1)
                    {
                        getDownstreamCell(gridD8, currCol, currRow, lstDownCells, d8Type);
                        if (lstDownCells.Count == 0)
                        {
                            cnt = 0;
                        }
                        for (int i = 0; i <= lstDownCells.Count - 1; i++)
                        {
                            stkDownCell.Push(lstDownCells[i]);
                        }
                    }
                    else
                    {
                        backtrackDownStreamArea(gridNew, gridD8, currCol, currRow, d8Type, System.Convert.ToDouble(gridStream.get_Value(currCol, currRow)));
                    }
                }
            }
        }

        private void getDownstreamCell( MapWinGIS.Grid gridD8, int col, int row, System.Collections.ArrayList lstDownCells, int d8Type)
        {
            lstDownCells.Clear();
            int currVal;
            currVal = System.Convert.ToInt32(gridD8.get_Value(col, row));
            if (d8Type <= 8)
            {
                if (currVal == 1)
                {
                    lstDownCells.Add(col + 1);
                    lstDownCells.Add(row);
                }
                else if (currVal == 2)
                {
                    lstDownCells.Add(col + 1);
                    lstDownCells.Add(row - 1);
                }
                else if (currVal == 3)
                {
                    lstDownCells.Add(col);
                    lstDownCells.Add(row - 1);
                }
                else if (currVal == 4)
                {
                    lstDownCells.Add(col - 1);
                    lstDownCells.Add(row - 1);
                }
                else if (currVal == 5)
                {
                    lstDownCells.Add(col - 1);
                    lstDownCells.Add(row);
                }
                else if (currVal == 6)
                {
                    lstDownCells.Add(col - 1);
                    lstDownCells.Add(row + 1);
                }
                else if (currVal == 7)
                {
                    lstDownCells.Add(col);
                    lstDownCells.Add(row + 1);
                }
                else if (currVal == 8)
                {
                    lstDownCells.Add(col + 1);
                    lstDownCells.Add(row + 1);
                }
            }
            else
            {
                if (currVal == 1)
                {
                    lstDownCells.Add(col + 1);
                    lstDownCells.Add(row);
                }
                else if (currVal == 128)
                {
                    lstDownCells.Add(col + 1);
                    lstDownCells.Add(row - 1);
                }
                else if (currVal == 64)
                {
                    lstDownCells.Add(col);
                    lstDownCells.Add(row - 1);
                }
                else if (currVal == 32)
                {
                    lstDownCells.Add(col - 1);
                    lstDownCells.Add(row - 1);
                }
                else if (currVal == 16)
                {
                    lstDownCells.Add(col - 1);
                    lstDownCells.Add(row);
                }
                else if (currVal == 8)
                {
                    lstDownCells.Add(col - 1);
                    lstDownCells.Add(row + 1);
                }
                else if (currVal == 4)
                {
                    lstDownCells.Add(col);
                    lstDownCells.Add(row + 1);
                }
                else if (currVal == 2)
                {
                    lstDownCells.Add(col + 1);
                    lstDownCells.Add(row + 1);
                }
            }
        }

        private void backtrackDownStreamArea(MapWinGIS.Grid gridNew, MapWinGIS.Grid gridD8, int col, int row, int d8Type, double setVal)
        {
            System.Collections.Stack stkBackCell = new System.Collections.Stack();
            int currRow;
            int currCol;
            int cnt;
            ArrayList lstBackCells = new ArrayList();
            cnt = 0;
            gridNew.set_Value(col, row, setVal);
            getBacktrackCell(gridD8, gridNew, col, row, lstBackCells, d8Type);
            for (int i = 0; i <= lstBackCells.Count - 1; i++)
            {
                stkBackCell.Push(lstBackCells[i]);
            }
            while (stkBackCell.Count > 0)
            {
                currRow = System.Convert.ToInt32(stkBackCell.Pop());
                currCol = System.Convert.ToInt32(stkBackCell.Pop());
                if (System.Convert.ToInt32(gridNew.get_Value(currCol, currRow)) == 2)
                {
                    cnt = cnt + 1;
                    gridNew.set_Value(currCol, currRow, setVal);
                    getBacktrackCell(gridD8, gridNew, currCol, currRow, lstBackCells, d8Type);
                    if (lstBackCells.Count == 0)
                    {
                        cnt = 0;
                    }
                    for (int i = 0; i <= lstBackCells.Count - 1; i++)
                    {
                        stkBackCell.Push(lstBackCells[i]);
                    }
                }
            }
        }

        private void getBacktrackCell(MapWinGIS.Grid gridD8, MapWinGIS.Grid gridNew, int col, int row, System.Collections.ArrayList lstBackCells, int d8Type)
        {
            lstBackCells.Clear();
            if (d8Type <= 8)
            {
                if (System.Convert.ToInt32(gridD8.get_Value(col - 1, row)) == 1 && System.Convert.ToInt32(gridNew.get_Value(col - 1, row)) == 2)
                {
                    lstBackCells.Add(col - 1);
                    lstBackCells.Add(row);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col - 1, row + 1)) == 2 && System.Convert.ToInt32(gridNew.get_Value(col - 1, row + 1)) == 2)
                {
                    lstBackCells.Add(col - 1);
                    lstBackCells.Add(row + 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col, row + 1)) == 3 && System.Convert.ToInt32(gridNew.get_Value(col, row + 1)) == 2)
                {
                    lstBackCells.Add(col);
                    lstBackCells.Add(row + 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col + 1, row + 1)) == 4 && System.Convert.ToInt32(gridNew.get_Value(col + 1, row + 1)) == 2)
                {
                    lstBackCells.Add(col + 1);
                    lstBackCells.Add(row + 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col + 1, row)) == 5 && System.Convert.ToInt32(gridNew.get_Value(col + 1, row)) == 2)
                {
                    lstBackCells.Add(col + 1);
                    lstBackCells.Add(row);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col + 1, row - 1)) == 6 && System.Convert.ToInt32(gridNew.get_Value(col + 1, row - 1)) == 2)
                {
                    lstBackCells.Add(col + 1);
                    lstBackCells.Add(row - 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col, row - 1)) == 7 && System.Convert.ToInt32(gridNew.get_Value(col, row - 1)) == 2)
                {
                    lstBackCells.Add(col);
                    lstBackCells.Add(row - 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col - 1, row - 1)) == 8 && System.Convert.ToInt32(gridNew.get_Value(col - 1, row - 1)) == 2)
                {
                    lstBackCells.Add(col - 1);
                    lstBackCells.Add(row - 1);
                }
            }
            else
            {
                if (System.Convert.ToInt32(gridD8.get_Value(col - 1, row)) == 1 && System.Convert.ToInt32(gridNew.get_Value(col - 1, row)) == 2)
                {
                    lstBackCells.Add(col - 1);
                    lstBackCells.Add(row);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col - 1, row + 1)) == 128 && System.Convert.ToInt32(gridNew.get_Value(col - 1, row + 1)) == 2)
                {
                    lstBackCells.Add(col - 1);
                    lstBackCells.Add(row + 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col, row + 1)) == 64 && System.Convert.ToInt32(gridNew.get_Value(col, row + 1)) == 2)
                {
                    lstBackCells.Add(col);
                    lstBackCells.Add(row + 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col + 1, row + 1)) == 32 && System.Convert.ToInt32(gridNew.get_Value(col + 1, row + 1)) == 2)
                {
                    lstBackCells.Add(col + 1);
                    lstBackCells.Add(row + 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col + 1, row)) == 16 && System.Convert.ToInt32(gridNew.get_Value(col + 1, row)) == 2)
                {
                    lstBackCells.Add(col + 1);
                    lstBackCells.Add(row);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col + 1, row - 1)) == 8 && System.Convert.ToInt32(gridNew.get_Value(col + 1, row - 1)) == 2)
                {
                    lstBackCells.Add(col + 1);
                    lstBackCells.Add(row - 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col, row - 1)) == 4 && System.Convert.ToInt32(gridNew.get_Value(col, row - 1)) == 2)
                {
                    lstBackCells.Add(col);
                    lstBackCells.Add(row - 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col - 1, row - 1)) == 2 && System.Convert.ToInt32(gridNew.get_Value(col - 1, row - 1)) == 2)
                {
                    lstBackCells.Add(col - 1);
                    lstBackCells.Add(row - 1);
                }
            }
        }

        private void setUpStreamArea( MapWinGIS.Grid gridNew,  MapWinGIS.Grid gridD8, int col, int row, int d8Type)
        {
            System.Collections.Stack stkUpCell = new System.Collections.Stack();
            int currRow;
            int currCol;
            ArrayList lstUpCells = new ArrayList();
            getUpstreamCells(gridD8, col, row, lstUpCells, d8Type);
            for (int i = 0; i <= lstUpCells.Count - 1; i++)
            {
                stkUpCell.Push(lstUpCells[i]);
            }
            while (stkUpCell.Count > 0)
            {
                currRow = System.Convert.ToInt32(stkUpCell.Pop());
                currCol = System.Convert.ToInt32(stkUpCell.Pop());
                if (System.Convert.ToInt32(gridNew.get_Value(currCol, currRow)) != 3 && System.Convert.ToInt32(gridNew.get_Value(currCol, currRow)) != 1)
                {
                    gridNew.set_Value(currCol, currRow,3);
                    getUpstreamCells(gridD8, currCol, currRow, lstUpCells, d8Type);
                    for (int i = 0; i <= lstUpCells.Count - 1; i++)
                    {
                        stkUpCell.Push(lstUpCells[i]);
                    }
                }
            }
        }

        private void getUpstreamCells( MapWinGIS.Grid gridD8, int col, int row, System.Collections.ArrayList lstUpCells, int d8Type)
        {
            lstUpCells.Clear();
            if (d8Type <= 8)
            {
                if (System.Convert.ToInt32(gridD8.get_Value(col - 1, row)) == 1)
                {
                    lstUpCells.Add(col - 1);
                    lstUpCells.Add(row);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col - 1, row + 1)) == 2)
                {
                    lstUpCells.Add(col - 1);
                    lstUpCells.Add(row + 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col, row + 1)) == 3)
                {
                    lstUpCells.Add(col);
                    lstUpCells.Add(row + 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col + 1, row + 1)) == 4)
                {
                    lstUpCells.Add(col + 1);
                    lstUpCells.Add(row + 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col + 1, row)) == 5)
                {
                    lstUpCells.Add(col + 1);
                    lstUpCells.Add(row);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col + 1, row - 1)) == 6)
                {
                    lstUpCells.Add(col + 1);
                    lstUpCells.Add(row - 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col, row - 1)) == 7)
                {
                    lstUpCells.Add(col);
                    lstUpCells.Add(row - 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col - 1, row - 1)) == 8)
                {
                    lstUpCells.Add(col - 1);
                    lstUpCells.Add(row - 1);
                }
            }
            else
            {
                if (System.Convert.ToInt32(gridD8.get_Value(col - 1, row)) == 1)
                {
                    lstUpCells.Add(col - 1);
                    lstUpCells.Add(row);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col - 1, row + 1)) == 128)
                {
                    lstUpCells.Add(col - 1);
                    lstUpCells.Add(row + 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col, row + 1)) == 64)
                {
                    lstUpCells.Add(col);
                    lstUpCells.Add(row + 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col + 1, row + 1)) == 32)
                {
                    lstUpCells.Add(col + 1);
                    lstUpCells.Add(row + 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col + 1, row)) == 16)
                {
                    lstUpCells.Add(col + 1);
                    lstUpCells.Add(row);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col + 1, row - 1)) == 8)
                {
                    lstUpCells.Add(col + 1);
                    lstUpCells.Add(row - 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col, row - 1)) == 4)
                {
                    lstUpCells.Add(col);
                    lstUpCells.Add(row - 1);
                }
                if (System.Convert.ToInt32(gridD8.get_Value(col - 1, row - 1)) == 2)
                {
                    lstUpCells.Add(col - 1);
                    lstUpCells.Add(row - 1);
                }
            }
        }

        private void createStreamGrid(string currStreamPath, string currD8Path, out string newStreamPath, MapWinGIS.ICallback callback)
        {
            string strStreamGridPath;
            MapWinGIS.Shapefile lineShape = new MapWinGIS.Shapefile();
            MapWinGIS.Grid gridOut = new MapWinGIS.Grid();
            MapWinGIS.Grid gridD8 = new MapWinGIS.Grid();
            MapWinGIS.Utils u = new MapWinGIS.Utils();
            int y0;
            int y1;
            int x0;
            int x1;
            int swap;
            int deltax;
            int deltay;
            int err;
            int deltaerr;
            int y;
            int ystep;
            double currID;
            bool steep;
            double perc;
            MapWinGIS.Shape tmpShape;
            MapWinGIS.GridHeader head = new MapWinGIS.GridHeader();
            strStreamGridPath = System.IO.Path.GetDirectoryName(currStreamPath) + "\\" + System.IO.Path.GetFileNameWithoutExtension(currStreamPath) + "_grid.bgd";
            DataManagement.DeleteGrid(ref strStreamGridPath);
            lineShape.Open(currStreamPath, callback);
            gridD8.Open(currD8Path, MapWinGIS.GridDataType.ShortDataType, true, MapWinGIS.GridFileType.UseExtension, callback);
            // Chris M 12/14/2006 head = gridD8.Header;
            head.CopyFrom(gridD8.Header);
            head.NodataValue = -1;
            gridOut.CreateNew(strStreamGridPath, head, MapWinGIS.GridDataType.DoubleDataType, -1,true, MapWinGIS.GridFileType.Binary,callback);
            for (int i = 0; i <= lineShape.NumShapes - 1; i++)
            {
                tmpShape = lineShape.get_Shape(i);
                currID = System.Convert.ToDouble(lineShape.get_CellValue(0, i));
                perc = (System.Convert.ToDouble(i) / System.Convert.ToDouble(lineShape.NumShapes - 1)) * 100.0;
                callback.Progress("Progress", System.Convert.ToInt32(perc), "");
                for (int j = 1; j <= tmpShape.numPoints - 1; j++)
                {
                    gridOut.ProjToCell(tmpShape.get_Point(j - 1).x, tmpShape.get_Point(j - 1).y, out x0, out y0);
                    gridOut.ProjToCell(tmpShape.get_Point(j).x, tmpShape.get_Point(j).y, out x1, out y1);
                    steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
                    if (steep)
                    {
                        swap = x0;
                        x0 = y0;
                        y0 = swap;
                        swap = x1;
                        x1 = y1;
                        y1 = swap;
                    }
                    if (x0 > x1)
                    {
                        swap = x0;
                        x0 = x1;
                        x1 = swap;
                        swap = y0;
                        y0 = y1;
                        y1 = swap;
                    }
                    deltax = x1 - x0;
                    deltay = Math.Abs(y1 - y0);
                    err = 0;
                    deltaerr = deltay;
                    y = y0;
                    if (y0 < y1)
                    {
                        ystep = 1;
                    }
                    else
                    {
                        ystep = -1;
                    }
                    for (int x = x0; x <= x1; x++)
                    {
                        if (steep)
                        {
                            gridOut.set_Value(y, x, currID);
                        }
                        else
                        {
                            gridOut.set_Value(x, y, currID);
                        }
                        err = err + deltaerr;
                        if (2 * err >= deltax)
                        {
                            y = y + ystep;
                            err = err - deltax;
                        }
                    }
                }
            }
            newStreamPath = strStreamGridPath;
            gridOut.Save(strStreamGridPath, MapWinGIS.GridFileType.UseExtension, callback);
            gridD8.Close();
            lineShape.Close();
            gridOut.Close();
        }

        private void splitAreasOnStreams(string flowAreaPath, string currStreamPath, MapWinGIS.ICallback callback)
        {
            MapWinGIS.Shapefile sfFlow = new MapWinGIS.Shapefile();
            MapWinGIS.Shapefile sfStream = new MapWinGIS.Shapefile();
            MapWinGIS.Shapefile sfTemp = new MapWinGIS.Shapefile();
            MapWinGIS.Shapefile sfClip;
            MapWinGIS.Shape tmp1, tmp2;
            string strTempPath, currCOMID;
            int currPoly, currSegment, tmpNum;
            int perc;

            callback.Progress("Status", 0, "Splitting Areas on Streams");
            sfFlow.Open(flowAreaPath, callback);
            sfStream.Open(currStreamPath, callback);

            strTempPath = System.IO.Path.GetDirectoryName(flowAreaPath) + "\\" + System.IO.Path.GetFileNameWithoutExtension(flowAreaPath) + "_trim.shp";
            DataManagement.DeleteShapefile(ref strTempPath);
            //CDM 8/4/2006 sfTemp.CreateNew(strTempPath, MapWinGIS.ShpfileType.SHP_POLYGON);
            Globals.PrepareResultSF(ref strTempPath, ref sfTemp, MapWinGIS.ShpfileType.SHP_POLYGON,true);
            sfTemp.Projection = sfFlow.Projection;
            sfTemp.StartEditingShapes(true, callback);
            sfTemp.StartEditingTable(callback);
            MapWinGIS.Field tmpValField = new MapWinGIS.Field();
            tmpValField.Name = "Value";
            tmpValField.Type = MapWinGIS.FieldType.INTEGER_FIELD;
            tmpNum = sfTemp.NumFields;
            sfTemp.EditInsertField(tmpValField, ref tmpNum, callback);
            MapWinGIS.Field tmpZoneField = new MapWinGIS.Field();
            tmpZoneField.Name = "Zone";
            tmpZoneField.Type = MapWinGIS.FieldType.STRING_FIELD;
            tmpNum = sfTemp.NumFields;
            sfTemp.EditInsertField(tmpZoneField, ref tmpNum, callback);
            MapWinGIS.Field tmpAreaField = new MapWinGIS.Field();
            tmpAreaField.Name = "Area";
            tmpAreaField.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
            tmpNum = sfTemp.NumFields;
            sfTemp.EditInsertField(tmpAreaField, ref tmpNum, callback);
            MapWinGIS.Field tmpCOMIDField = new MapWinGIS.Field();
            tmpCOMIDField.Name = "COMID";
            tmpCOMIDField.Type = MapWinGIS.FieldType.STRING_FIELD;
            tmpNum = sfTemp.NumFields;
            sfTemp.EditInsertField(tmpCOMIDField, ref tmpNum, callback);
            MapWinGIS.Field tmpSlopeField = new MapWinGIS.Field();
            tmpSlopeField.Name = "AveSlope";
            tmpSlopeField.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
            tmpNum = sfTemp.NumFields;
            sfTemp.EditInsertField(tmpSlopeField, ref tmpNum, callback);


            for (int i = 0; i < sfFlow.NumShapes; i++)
            {
                perc = System.Convert.ToInt32((System.Convert.ToDouble(i) / System.Convert.ToDouble(sfFlow.NumShapes + 1)) * 100.0);
                callback.Progress("Progress", perc, "Splitting Area on Streams");
                currPoly = i;
                currCOMID = System.Convert.ToString(sfFlow.get_CellValue(3, i));
                if ( currCOMID != "")
                {
                    currSegment = -1;
                    for (int j = 0; j < sfStream.NumShapes; j++)
                    {
                        if (System.Convert.ToString(sfStream.get_CellValue(0, j)) == currCOMID)
                        {
                            currSegment = j;
                            break;
                        }
                    }
                    if (currSegment != -1)
                    {
                        tmpNum = sfFlow.NumShapes;
                        tmp1 = sfFlow.get_Shape(currPoly);
                        tmp2 = sfStream.get_Shape(currSegment);
                        try
                        {
                            if (SpatialOperations.ClipPolygonWithLine(ref tmp1, ref tmp2, out sfClip) == true)
                            {
                                tmp1 = sfClip.get_Shape(0);
                                tmp2 = sfClip.get_Shape(1);
                                double area1 = Utils.Area(ref tmp1);
                                double area2 = Utils.Area(ref tmp2);
                                if (area1 > area2)
                                {
                                    sfTemp.EditInsertShape(tmp1, ref tmpNum);
                                    sfTemp.EditCellValue(2, tmpNum, area1);
                                }
                                else
                                {
                                    sfTemp.EditInsertShape(tmp2, ref tmpNum);
                                    sfTemp.EditCellValue(2, tmpNum, area2);
                                }
                                sfTemp.EditCellValue(0, tmpNum, sfFlow.get_CellValue(0, currPoly));
                                sfTemp.EditCellValue(1, tmpNum, sfFlow.get_CellValue(1, currPoly));
                                sfTemp.EditCellValue(3, tmpNum, sfFlow.get_CellValue(3, currPoly));
                            }
                            else
                            {
                                tmp1 = sfFlow.get_Shape(currPoly);
                                tmpNum = sfTemp.NumShapes;
                                sfTemp.EditInsertShape(tmp1, ref tmpNum);
                                sfTemp.EditCellValue(0, tmpNum, sfFlow.get_CellValue(0, currPoly));
                                sfTemp.EditCellValue(1, tmpNum, sfFlow.get_CellValue(1, currPoly));
                                sfTemp.EditCellValue(2, tmpNum, sfFlow.get_CellValue(2, currPoly));
                                sfTemp.EditCellValue(3, tmpNum, sfFlow.get_CellValue(3, currPoly));
                            }
                        }
                        catch
                        {
                            tmp1 = sfFlow.get_Shape(currPoly);
                            tmpNum = sfTemp.NumShapes;
                            sfTemp.EditInsertShape(tmp1, ref tmpNum);
                            sfTemp.EditCellValue(0, tmpNum, sfFlow.get_CellValue(0, currPoly));
                            sfTemp.EditCellValue(1, tmpNum, sfFlow.get_CellValue(1, currPoly));
                            sfTemp.EditCellValue(2, tmpNum, sfFlow.get_CellValue(2, currPoly));
                            sfTemp.EditCellValue(3, tmpNum, sfFlow.get_CellValue(3, currPoly));
                        }
                    }
                }
                else
                {
                    tmp1 = sfFlow.get_Shape(currPoly);
                    tmpNum = sfTemp.NumShapes;
                    sfTemp.EditInsertShape(tmp1, ref tmpNum);
                    sfTemp.EditCellValue(0, tmpNum, sfFlow.get_CellValue(0, currPoly));
                    sfTemp.EditCellValue(1, tmpNum, sfFlow.get_CellValue(1, currPoly));
                    sfTemp.EditCellValue(2, tmpNum, sfFlow.get_CellValue(2, currPoly));
                    sfTemp.EditCellValue(3, tmpNum, sfFlow.get_CellValue(3, currPoly));
                }
            }
            sfTemp.StopEditingShapes(true, false, callback);
            sfTemp.StopEditingTable(true,callback);

            sfFlow.Close();
            sfStream.Close();

            DataManagement.DeleteShapefile(ref flowAreaPath);
            sfTemp.SaveAs(flowAreaPath,callback);
            sfTemp.Close();
            DataManagement.DeleteShapefile(ref strTempPath);
        }

        private void findAverageSlope(string demPath, double zFactor, string flowPolyShapefilePath, MapWinGIS.ICallback callback)
        {
            MapWinGIS.Grid slopeGrid = new MapWinGIS.Grid();
            MapWinGIS.Shapefile polyShape = new MapWinGIS.Shapefile();
            MapWinGIS.GridHeader head;
            int colStart, colStop, rowStart, rowStop, shapeCount;
            double xCent, yCent, dxHalf, dyHalf;
            string slopePath;
            int perc = 0;

            if (System.IO.Path.GetFileName(demPath) != "sta.adf")
            {
                slopePath = System.IO.Path.GetDirectoryName(demPath) + "\\" + System.IO.Path.GetFileNameWithoutExtension(demPath) + "_slope.bgd";
            }
            else
            {
                slopePath = System.IO.Path.GetDirectoryName(demPath) + "_slope.bgd";
            }

            TerrainAnalysis.Slope(demPath, zFactor, slopePath, true, callback);

            slopeGrid.Open(slopePath, MapWinGIS.GridDataType.DoubleDataType, true, MapWinGIS.GridFileType.UseExtension, callback);
            head = slopeGrid.Header;

            polyShape.Open(flowPolyShapefilePath, callback);
            shapeCount = polyShape.NumShapes;            
            int [] areaCount = new int[shapeCount];
            double [] areaTotal = new double[shapeCount];
            double[] areaAve = new double[shapeCount];

            dxHalf = head.dX / 2;
            dyHalf = head.dY / 2;

            callback.Progress("Status", 0, "Calculating Average Slopes");
            slopeGrid.ProjToCell(polyShape.Extents.xMin, polyShape.Extents.yMax, out colStart, out rowStart);
            slopeGrid.ProjToCell(polyShape.Extents.xMax, polyShape.Extents.yMin, out colStop, out rowStop);
            for (int row = rowStart - 1; row < rowStop + 1; row++)
            {
                perc = System.Convert.ToInt32(((System.Convert.ToDouble(row) - System.Convert.ToDouble(rowStart - 1)) / (System.Convert.ToDouble(rowStop + 1) - System.Convert.ToDouble(rowStart - 1))) * 100.0);
                callback.Progress("Progress", perc, "");
                for (int col = colStart - 1; col < colStop + 1; col++)
                {
                    slopeGrid.CellToProj(col, row, out xCent, out yCent);
                    for (int shpindx = 0; shpindx < polyShape.NumShapes; shpindx++)
                    {
                        if (polyShape.PointInShape(shpindx, xCent, yCent) || polyShape.PointInShape(shpindx, xCent - dxHalf, yCent - dyHalf) || polyShape.PointInShape(shpindx, xCent + dxHalf, yCent - dyHalf) || polyShape.PointInShape(shpindx, xCent + dxHalf, yCent + dyHalf) || polyShape.PointInShape(shpindx, xCent - dxHalf, yCent + dyHalf))
                        {
                            areaCount[shpindx]++;
                            areaTotal[shpindx] += Convert.ToDouble(slopeGrid.get_Value(col, row))/100;
                        }
                    }
                }
            }
            polyShape.StartEditingTable(callback);
            for (int shpindx = 0; shpindx < polyShape.NumShapes; shpindx++)
            {
                if (areaCount[shpindx] == 0)
                {
                    areaAve[shpindx] = 0;
                }
                else
                {
                    areaAve[shpindx] = areaTotal[shpindx] / areaCount[shpindx];
                }                
                polyShape.EditCellValue(4, shpindx, areaAve[shpindx]);
            }
            polyShape.StopEditingTable(true, callback);
            callback.Progress("Status", 0, "");

            polyShape.Close();
            slopeGrid.Close();
        }


        #region Simple Raster to Polygon

        private void SimpleRasterToPolygon(string strInRast, string strOutPoly, MapWinGIS.ICallback callback)
        {
            int maxX;
            int maxY;
            double noData;
            double currVal;
            double currTrack;
            string strTrackPath;
            MapWinGIS.Grid gridIn = new MapWinGIS.Grid();
            MapWinGIS.Grid gridTrack = new MapWinGIS.Grid();
            MapWinGIS.GridHeader headIn;
            MapWinGIS.Shapefile polyOut = new MapWinGIS.Shapefile();

            callback.Progress("Status", 0, "Convert Flow Areas to Shapes");

            gridIn.Open(strInRast, MapWinGIS.GridDataType.DoubleDataType, true, MapWinGIS.GridFileType.UseExtension, callback);
            headIn = gridIn.Header;
            maxX = headIn.NumberCols - 1;
            maxY = headIn.NumberRows - 1;
            noData = -1;

            strTrackPath = System.IO.Path.GetDirectoryName(strInRast) + "\\" + System.IO.Path.GetFileNameWithoutExtension(strInRast) + "_track.bgd";
            DataManagement.DeleteGrid(ref strTrackPath);
            gridTrack.CreateNew(strTrackPath, headIn, MapWinGIS.GridDataType.LongDataType, 0, true, MapWinGIS.GridFileType.UseExtension, callback);

            DataManagement.DeleteShapefile(ref strOutPoly);
            //CDM 8/4/2006 polyOut.CreateNew(strOutPoly, MapWinGIS.ShpfileType.SHP_POLYGON);
            Globals.PrepareResultSF(ref strOutPoly, ref polyOut, MapWinGIS.ShpfileType.SHP_POLYGON,true);
            polyOut.Projection = gridIn.Header.Projection;
            polyOut.StartEditingTable(callback);
            MapWinGIS.Field tmpValField = new MapWinGIS.Field();
            tmpValField.Name = "Value";
            tmpValField.Type = MapWinGIS.FieldType.INTEGER_FIELD;
            int tmpNum = polyOut.NumFields;
            polyOut.EditInsertField(tmpValField, ref tmpNum, callback);
            MapWinGIS.Field tmpZoneField = new MapWinGIS.Field();
            tmpZoneField.Name = "Zone";
            tmpZoneField.Type = MapWinGIS.FieldType.STRING_FIELD;
            tmpNum = polyOut.NumFields;
            polyOut.EditInsertField(tmpZoneField, ref tmpNum, callback);
            MapWinGIS.Field tmpAreaField = new MapWinGIS.Field();
            tmpAreaField.Name = "Area";
            tmpAreaField.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
            tmpNum = polyOut.NumFields;
            polyOut.EditInsertField(tmpAreaField, ref tmpNum, callback);
            MapWinGIS.Field tmpCOMIDField = new MapWinGIS.Field();
            tmpCOMIDField.Name = "COMID";
            tmpCOMIDField.Type = MapWinGIS.FieldType.STRING_FIELD;
            tmpNum = polyOut.NumFields;
            polyOut.EditInsertField(tmpCOMIDField, ref tmpNum, callback);
            MapWinGIS.Field tmpSlopeField = new MapWinGIS.Field();
            tmpSlopeField.Name = "AveSlope";
            tmpSlopeField.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
            tmpNum = polyOut.NumFields;
            polyOut.EditInsertField(tmpSlopeField, ref tmpNum, callback);
            polyOut.StopEditingTable(true, callback);
            int perc = 0;
            for (int loopY = 0; loopY <= maxY; loopY++)
            {
                perc = System.Convert.ToInt32((System.Convert.ToDouble(loopY) / System.Convert.ToDouble(maxY)) * 100);
                callback.Progress("Progress", perc, "Convert Flow Areas to Shapes");
                for (int loopX = 0; loopX <= maxX; loopX++)
                {
                    currVal = System.Convert.ToInt32(gridIn.get_Value(loopX, loopY));
                    currTrack = System.Convert.ToInt32(gridTrack.get_Value(loopX, loopY));
                    if (currVal == noData)
                    {
                        gridTrack.set_Value(loopX, loopY, 1);
                    }
                    else if (currTrack == 1)
                    {
                    }
                    else
                    {
                        formPolyFromCell(gridIn, gridTrack, loopX, loopY, polyOut, callback);
                    }
                }
            }
            gridIn.Close();
            gridTrack.Close();
            polyOut.Close();
            callback.Progress("Status", 0, "");
        }

        private void formPolyFromCell(MapWinGIS.Grid gridMain, MapWinGIS.Grid gridTrack, int startX, int startY, MapWinGIS.Shapefile polyOut, MapWinGIS.ICallback callback)
        {
            int nextDir = -1;
            int nextX = -1;
            int nextY = -1;
            int startDir = -1;
            ArrayList lstPoints = new ArrayList();
            MapWinGIS.Point startPoint;
            MapWinGIS.Point endPoint = new MapWinGIS.Point();
            lstPoints.Clear();
            startDir = 1;
            cycleCellDirForPoly(gridMain, gridTrack, startX, startY, startDir, System.Convert.ToDouble(gridMain.get_Value(startX, startY)), lstPoints, ref nextDir, ref nextX, ref nextY);
            if (nextDir != -1)
            {
                startPoint = (MapWinGIS.Point)lstPoints[0];
                if (lstPoints.Count > 1)
                {
                    endPoint = (MapWinGIS.Point)lstPoints[lstPoints.Count - 1];
                }
                else
                {
                    endPoint.x = -1;
                    endPoint.y = -1;
                }
                while (startPoint.x != endPoint.x | startPoint.y != endPoint.y)
                {
                    cycleCellDirForPoly(gridMain, gridTrack, nextX, nextY, nextDir, System.Convert.ToDouble(gridMain.get_Value(nextX, nextY)), lstPoints, ref nextDir, ref nextX, ref nextY);
                    endPoint = (MapWinGIS.Point)lstPoints[lstPoints.Count - 1];
                }
            }
            addPolyFromPointsList(polyOut, lstPoints, System.Convert.ToDouble(gridMain.get_Value(startX, startY)), callback);
            int tmpNum = polyOut.NumShapes - 1;
            fillTrackGrid(gridTrack, polyOut, tmpNum);
        }

        private void cycleCellDirForPoly(MapWinGIS.Grid gridMain, MapWinGIS.Grid gridTrack, int startX, int startY, int startDir, double checkVal, ArrayList lstPoints, ref int nextDir, ref int nextX, ref int nextY)
        {
            MapWinGIS.Point pntCurr;
            int currDir = -1;
            int cellX = -1;
            int cellY = -1;
            double cellVal = -1;
            MapWinGIS.Point startPoint;
            bool breakFlag;
            nextDir = -1;
            nextX = -1;
            nextY = -1;
            gridTrack.set_Value(startX, startY, 1);
            if (startDir == 2 | startDir == 4 | startDir == 6 | startDir == 8)
            {
                currDir = ((startDir + 5) % 8) + 1;
            }
            else
            {
                currDir = ((startDir + 6) % 8) + 1;
            }
            for (int i = 1; i <= 9; i++)
            {
                breakFlag = false;
                if (currDir == 2 | currDir == 4 | currDir == 6 | currDir == 8)
                {
                    pntCurr = getCellCornerPoint(gridMain, startX, startY, currDir);
                    lstPoints.Add(pntCurr);
                    if (lstPoints.Count > 1)
                    {
                        startPoint = (MapWinGIS.Point)lstPoints[0];
                        if (pntCurr.x == startPoint.x & pntCurr.y == startPoint.y)
                        {
                            breakFlag = true;
                        }
                    }
                }
                getValAndCellInDir(gridMain, startX, startY, currDir, ref cellVal, ref cellX, ref cellY);
                if (cellVal == checkVal)
                {
                    nextDir = currDir;
                    nextX = cellX;
                    nextY = cellY;
                    break;
                }
                currDir = currDir % 8 + 1;
                if (breakFlag)
                {
                    break;
                }
            }
        }

        private void getValAndCellInDir(MapWinGIS.Grid gridMain, int startX, int startY, int currDir, ref double dirVal, ref int dirX, ref int dirY)
        {
            if (currDir == 1)
            {
                dirVal = System.Convert.ToInt32(gridMain.get_Value(startX + 1, startY));
                dirX = startX + 1;
                dirY = startY;
            }
            else if (currDir == 2)
            {
                dirVal = System.Convert.ToInt32(gridMain.get_Value(startX + 1, startY + 1));
                dirX = startX + 1;
                dirY = startY + 1;
            }
            else if (currDir == 3)
            {
                dirVal = System.Convert.ToInt32(gridMain.get_Value(startX, startY + 1));
                dirX = startX;
                dirY = startY + 1;
            }
            else if (currDir == 4)
            {
                dirVal = System.Convert.ToInt32(gridMain.get_Value(startX - 1, startY + 1));
                dirX = startX - 1;
                dirY = startY + 1;
            }
            else if (currDir == 5)
            {
                dirVal = System.Convert.ToInt32(gridMain.get_Value(startX - 1, startY));
                dirX = startX - 1;
                dirY = startY;
            }
            else if (currDir == 6)
            {
                dirVal = System.Convert.ToInt32(gridMain.get_Value(startX - 1, startY - 1));
                dirX = startX - 1;
                dirY = startY - 1;
            }
            else if (currDir == 7)
            {
                dirVal = System.Convert.ToInt32(gridMain.get_Value(startX, startY - 1));
                dirX = startX;
                dirY = startY - 1;
            }
            else if (currDir == 8)
            {
                dirVal = System.Convert.ToInt32(gridMain.get_Value(startX + 1, startY - 1));
                dirX = startX + 1;
                dirY = startY - 1;
            }
        }

        private MapWinGIS.Point getCellCornerPoint(MapWinGIS.Grid g, int x, int y, int corner)
        {
            double centerX;
            double centerY;
            double cellwidth;
            double cellheight;
            double cornerX;
            double cornerY;
            MapWinGIS.Point tmpPoint = new MapWinGIS.Point();
            cellwidth = g.Header.dX;
            cellheight = g.Header.dY;
            g.CellToProj(x, y, out centerX, out centerY);
            if (corner == 2)
            {
                cornerX = centerX + cellwidth / 2;
                cornerY = centerY - cellheight / 2;
            }
            else if (corner == 4)
            {
                cornerX = centerX - cellwidth / 2;
                cornerY = centerY - cellheight / 2;
            }
            else if (corner == 6)
            {
                cornerX = centerX - cellwidth / 2;
                cornerY = centerY + cellheight / 2;
            }
            else if (corner == 8)
            {
                cornerX = centerX + cellwidth / 2;
                cornerY = centerY + cellheight / 2;
            }
            else
            {
                cornerX = -1;
                cornerY = -1;
            }
            tmpPoint.x = cornerX;
            tmpPoint.y = cornerY;
            return tmpPoint;
        }

        private void addPolyFromPointsList(MapWinGIS.Shapefile polyOut, ArrayList lstPoints, double currVal, MapWinGIS.ICallback callback)
        {
            MapWinGIS.Point tmpPoint;
            MapWinGIS.Shape tmpPoly = new MapWinGIS.Shape();
            int tmpNum = -1;
            bool tmpbool;
            if (lstPoints.Count > 1)
            {
                tmpbool = polyOut.StartEditingShapes(true, callback);
                tmpbool = polyOut.StartEditingTable(callback);
                tmpbool = tmpPoly.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
                for (int i = 0; i <= lstPoints.Count - 1; i++)
                {
                    tmpPoint = (MapWinGIS.Point)lstPoints[i];
                    tmpNum = tmpPoly.numPoints;
                    tmpbool = tmpPoly.InsertPoint(tmpPoint, ref tmpNum);
                }
                tmpNum = polyOut.NumShapes;
                if (polyOut.EditInsertShape(tmpPoly, ref tmpNum) == false)
                {
                    System.Windows.Forms.MessageBox.Show(polyOut.get_ErrorMsg(polyOut.LastErrorCode));
                }

                tmpbool = polyOut.EditCellValue(2, polyOut.NumShapes - 1, Utils.Area(ref tmpPoly));
                if (currVal == 1)
                {
                    tmpbool = polyOut.EditCellValue(0, polyOut.NumShapes - 1, 1);
                    tmpbool = polyOut.EditCellValue(1, polyOut.NumShapes - 1, "Initiation");
                }
                else if (currVal == 3)
                {
                    tmpbool = polyOut.EditCellValue(0, polyOut.NumShapes - 1, 3);
                    tmpbool = polyOut.EditCellValue(1, polyOut.NumShapes - 1, "In-flow");
                }
                else if (currVal > 3)
                {
                    tmpbool = polyOut.EditCellValue(0, polyOut.NumShapes - 1, 2);
                    tmpbool = polyOut.EditCellValue(1, polyOut.NumShapes - 1, "Out-flow");
                    tmpbool = polyOut.EditCellValue(3, polyOut.NumShapes - 1, System.Convert.ToString(currVal));
                }
                polyOut.StopEditingTable(true, callback);
                polyOut.StopEditingShapes(true, false, callback);
            }
        }

        private void fillTrackGrid(MapWinGIS.Grid gridTrack, MapWinGIS.Shapefile polyOut, int idxToFill)
        {
            int rowStart;
            int rowStop;
            int colStart;
            int colStop;
            double currxproj;
            double curryproj;
            polyOut.BeginPointInShapefile();
            gridTrack.ProjToCell(polyOut.get_Shape(idxToFill).Extents.xMin, polyOut.get_Shape(idxToFill).Extents.yMax, out colStart, out rowStart);
            gridTrack.ProjToCell(polyOut.get_Shape(idxToFill).Extents.xMax, polyOut.get_Shape(idxToFill).Extents.yMin, out colStop, out rowStop);
            for (int rowcell = rowStart; rowcell <= rowStop; rowcell++)
            {
                for (int colcell = colStart; colcell <= colStop; colcell++)
                {
                    gridTrack.CellToProj(colcell, rowcell, out currxproj, out curryproj);
                    if (polyOut.PointInShape(idxToFill, currxproj, curryproj))
                    {
                        gridTrack.set_Value(colcell, rowcell, 1);
                    }
                }
            }
            polyOut.EndPointInShapefile();
        }

        private bool isNotEdgeCell(MapWinGIS.Grid gridIn, MapWinGIS.Grid gridTrack, int startX, int startY, double noData)
        {
            bool isNotEdgeCell = false;
            if (System.Convert.ToInt32(gridIn.get_Value(startX - 1, startY - 1)) == noData | System.Convert.ToInt32(gridTrack.get_Value(startX - 1, startY - 1)) == 1)
            {
                isNotEdgeCell = false;
            }
            else if (System.Convert.ToInt32(gridIn.get_Value(startX, startY - 1)) == noData | System.Convert.ToInt32(gridTrack.get_Value(startX, startY - 1)) == 1)
            {
                isNotEdgeCell = false;
            }
            else if (System.Convert.ToInt32(gridIn.get_Value(startX + 1, startY - 1)) == noData | System.Convert.ToInt32(gridTrack.get_Value(startX + 1, startY - 1)) == 1)
            {
                isNotEdgeCell = false;
            }
            else if (System.Convert.ToInt32(gridIn.get_Value(startX + 1, startY)) == noData | System.Convert.ToInt32(gridTrack.get_Value(startX + 1, startY)) == 1)
            {
                isNotEdgeCell = false;
            }
            else if (System.Convert.ToInt32(gridIn.get_Value(startX + 1, startY + 1)) == noData | System.Convert.ToInt32(gridTrack.get_Value(startX + 1, startY + 1)) == 1)
            {
                isNotEdgeCell = false;
            }
            else if (System.Convert.ToInt32(gridIn.get_Value(startX, startY + 1)) == noData | System.Convert.ToInt32(gridTrack.get_Value(startX, startY + 1)) == 1)
            {
                isNotEdgeCell = false;
            }
            else if (System.Convert.ToInt32(gridIn.get_Value(startX - 1, startY + 1)) == noData | System.Convert.ToInt32(gridTrack.get_Value(startX - 1, startY + 1)) == 1)
            {
                isNotEdgeCell = false;
            }
            else if (System.Convert.ToInt32(gridIn.get_Value(startX - 1, startY)) == noData | System.Convert.ToInt32(gridTrack.get_Value(startX - 1, startY)) == 1)
            {
                isNotEdgeCell = false;
            }
            else
            {
                isNotEdgeCell = true;
                gridTrack.set_Value(startX, startY, 1);
            }
            return isNotEdgeCell;
        }
        #endregion

    }
}
