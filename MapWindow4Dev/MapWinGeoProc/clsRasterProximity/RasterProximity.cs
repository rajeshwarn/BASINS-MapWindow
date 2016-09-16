//********************************************************************************************************
// File name: RasterProximity.cs
// Description: For each raster cell, compute the distance (proximity) to the nearest 'target' cell.
//
//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specific language governing rights and 
//limitations under the License. 
//
// Author: Jiri Kadlec 8th June 2009
//
// Contributor(s):
//				
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using MapWinGIS;

namespace MapWinGeoProc
{
    /// <summary>
    /// Methods for calculating raster distance (proximity). The algorithms used are based on the GDAL tools.
    /// For each raster cell, the shortest euclidean distance to the nearest 'target' cell is calculated. 
    /// The 'target' cells are all cells that have a user-specified value.
    /// </summary>
    public class RasterProximity
    {
        
        /// <summary>
        /// For each raster cell calculate the distance to the nearest 'target' ('source') cell.
        /// </summary>
        /// <param name="input">The input raster file name</param>
        /// <param name="output">The output raster file name</param>
        /// <param name="maxDistance">The maximum distance to be calculated</param>
        /// <param name="targetValue">all cells having this value will be considered 'target cells'</param>
        /// <param name="cback">Object for reporting progress</param>
        /// <returns>true if successfull, false otherwise</returns>
        public static bool ComputeProximity(string input, string output, double maxDistance, double targetValue, MapWinGIS.ICallback cback)
        {
            // declare jagged arrays for storing the raster working copy and the (Rx, Ry) vectors. 
            //rX is distance from current cell to nearest target cell measured in the x-direction
            //rY is distance from current cell to nearest target cell measured in the y-direction
            // the actual distance can be calculated as sqrt(rX^2 + rY^2).
            // in the resulting distance raster we store the squared distance as well as the rX, rY relative coordinates
            // to improve computation speed

            int[][] aRx = null;     //x-element of distance vector for each cell
            int[][] aRy = null;     //y-element of distance vector for each cell
            int[][] aSqDist= null; //the working copy of the raster. 
                                                  //The squared distances from each cell to nearest target cell are stored here.

            
            //Reads the input raster into a jagged in-memory array.
            aSqDist = ReadInput(input, targetValue, cback);


            int numRows = aSqDist.Length;
            int numColumns = aSqDist[0].Length;
            int lastUpdate = 0;

            int infD = int.MaxValue; //initial distance value
            int targetVal = 0;       //the new value of target cells in the array

            //initialize the aRx, aRy arrays
            aRx = new int[numRows][];
            aRy = new int[numRows][];
            for (int i = 0; i < numRows; i++)
            {
                aRx[i] = new int[numColumns];
                aRy[i] = new int[numColumns];
            }

            // *******************************************************************
            // raster distance calculation PASS ONE - top to bottom, left to right
            // *******************************************************************
            
            int[] aNcels = new int[4]; //the values of four neighbouring cells (W, NW, N, NE)
            int[] aDiff = new int[4]; //the calculated distances of the four neighbouring cells to nearest target cell

            for(int row = 1; row < numRows; row++)
            {
                for (int col = 1; col < numColumns - 1; col ++)
                {
                    int val = aSqDist[row][col];
                    
                    //Continue processing only if the current cell is not a target
                    if (val == targetVal)
                    {
                        continue;
                    }
                    
                    //read the values of the cell's neighbours
                    aNcels[0] = aSqDist[row][col - 1]; // W
                    aNcels[1] = aSqDist[row-1][col - 1]; // NW
                    aNcels[2] = aSqDist[row-1][col];     // N
                    aNcels[3] = aSqDist[row-1][col + 1]; // NE

                    //calculate the squared euclidean distances to each neighbouring cell and to the nearest target cell
                    aDiff[0] = (aNcels[0] < infD) ? aNcels[0] + 2 * aRx[row][col - 1] + 1 : infD;
                    aDiff[1] = (aNcels[1] < infD) ? aNcels[1] + 2 * (aRx[row - 1][col - 1] + aRy[row - 1][col - 1] + 1) : infD;
                    aDiff[2] = (aNcels[2] < infD) ? aNcels[2] + 2 * aRy[row - 1][col] + 1 : infD;
                    aDiff[3] = (aNcels[3] < infD) ? aNcels[3] + 2 * (aRx[row - 1][col + 1] + aRy[row - 1][col + 1] + 1) : infD;

                    //find neighbouring cell with minimum distance difference
                    int minDiff = aDiff[0];
                    int minDiffCell = 0;
                    for (int i = 1; i < 4; i++)
                    {
                        if(aDiff[i] < minDiff)
                        {
                            minDiff = aDiff[i];
                            minDiffCell = i;
                        }
                    }

                    //if a neighbouring cell is a target cell or if it's a cell with a distance previously calculated:
                    if (minDiff < infD)
                    {
                        //assign the minimum euclidean distance
                        aSqDist[row][col] = minDiff;
                        
                        //update the (rX, rY) cell-to-nearest-target vector
                        switch(minDiffCell)
                        {
                            case 0: // W
                                aRx[row][col] = aRx[row][col - 1] + 1;
                                aRy[row][col] = aRy[row][col - 1];
                                break;
                            case 1: // NW
                                aRx[row][col] = aRx[row-1][col-1] + 1;
                                aRy[row][col] = aRy[row-1][col-1] + 1;
                                break;
                            case 2: // N
                                aRx[row][col] = aRx[row-1][col];
                                aRy[row][col] = aRy[row-1][col] + 1;
                                break;
                            case 3: // NE
                                aRx[row][col] = aRx[row-1][col+1] + 1;
                                aRy[row][col] = aRy[row-1][col+1] + 1;
                                break;
                        }
                    } 
                    //end of update (rX, rY) cell-to-nearest-target vector
                    
                } 
                
                //end or current row processing - report progress
                int percent = (int)((double)row / (double)numRows * 100f);
                if (percent > lastUpdate)
                {
                    lastUpdate += 1;
                    if (cback != null)
                    {
                        cback.Progress("", lastUpdate, "Pass 1: " + lastUpdate.ToString() + "% complete");
                    }
                }
            }
            // *******************************************************************
            // end of first pass loop
            // *******************************************************************

            // *******************************************************************
            // raster distance calculation PASS TWO - bottom to top, right to left
            // *******************************************************************
            lastUpdate = 0;
            for (int row = numRows - 2; row > 0; row--)
            {
                for (int col = numColumns - 2; col > 0; col--)
                {
                    int val = aSqDist[row][col];

                    //Continue processing only if the current cell is not a target
                    if (val == targetVal)
                    {
                        continue;
                    }

                    //read the values of the cell's neighbours
                    aNcels[0] = aSqDist[row][col + 1];     // E
                    aNcels[1] = aSqDist[row + 1][col + 1]; // SE
                    aNcels[2] = aSqDist[row + 1][col];     // S
                    aNcels[3] = aSqDist[row + 1][col - 1]; // SW

                    //calculate the squared euclidean distances to each neighbouring cell and to the nearest target cell
                    aDiff[0] = (aNcels[0] < infD) ? aNcels[0] + 2 * aRx[row][col + 1] + 1 : infD;
                    aDiff[1] = (aNcels[1] < infD) ? aNcels[1] + 2 * (aRx[row + 1][col + 1] + aRy[row + 1][col + 1] + 1) : infD;
                    aDiff[2] = (aNcels[2] < infD) ? aNcels[2] + 2 * aRy[row + 1][col] + 1 : infD;
                    aDiff[3] = (aNcels[3] < infD) ? aNcels[3] + 2 * (aRx[row + 1][col - 1] + aRy[row + 1][col - 1] + 1) : infD;

                    //find neighbouring cell with minimum distance difference
                    int minDiff = aDiff[0];
                    int minDiffCell = 0;
                    for (int i = 1; i < 4; i++)
                    {
                        if (aDiff[i] < minDiff)
                        {
                            minDiff = aDiff[i];
                            minDiffCell = i;
                        }
                    }

                    //if a neighbouring cell with known distance smaller than current known distance was found:
                    if (minDiff < val)
                    {
                        //assign the minimum euclidean distance
                        aSqDist[row][col] = minDiff;

                        //update the (rX, rY) cell-to-nearest-target vector
                        switch (minDiffCell)
                        {
                            case 0: // E
                                aRx[row][col] = aRx[row][col + 1] + 1;
                                aRy[row][col] = aRy[row][col + 1];
                                break;
                            case 1: // SE
                                aRx[row][col] = aRx[row + 1][col + 1] + 1;
                                aRy[row][col] = aRy[row + 1][col + 1] + 1;
                                break;
                            case 2: // S
                                aRx[row][col] = aRx[row + 1][col];
                                aRy[row][col] = aRy[row + 1][col] + 1;
                                break;
                            case 3: // SW
                                aRx[row][col] = aRx[row + 1][col - 1] + 1;
                                aRy[row][col] = aRy[row + 1][col - 1] + 1;
                                break;
                        }
                    }
                    //end of update (rX, rY) cell-to-nearest-target vector

                }
                
                //Write row to output raster
                //writeOutputRow(outRaster, row, aSqDist[row]);

                //Report progress
                int percent = (int)((double)row / (double)numRows * 100f);
                if (percent > lastUpdate)
                {
                    lastUpdate += 1;
                    lastUpdate += 1;
                    if (cback != null)
                    {
                        cback.Progress("", lastUpdate, "Pass 1: " + lastUpdate.ToString() + "% complete");
                    }
                }
            }
            // *******************************************************************
            // end of second pass proximity calculation loop
            // *******************************************************************

            //********************************************************************
            // write the output raster
            //********************************************************************
            WriteOutput(input, output, ref aSqDist, (int)maxDistance, cback);

            return true;
        }

        #region Private Methods

        /// <summary>
        /// Writes the output raster. The output will have the same data type, cell size and extents as the input.
        /// </summary>
        /// <param name="input">input raster file name</param>
        /// <param name="output">output raster file name</param>
        /// <param name="array">array of raster values (squared distances to nearest target cell)</param>
        /// <param name="maxDistance">maximum distance to be included in the output raster</param>
        /// <param name="cback">for reporting progress</param>
        /// <returns>true if successful, false otherwise</returns>
        private static bool WriteOutput(string input, string output, ref int[][] array, int maxDistance, ICallback cback)
        {
            //open input raster file
            MapWinGIS.Grid inputGrid = new MapWinGIS.Grid();
            if (!inputGrid.Open(input, GridDataType.UnknownDataType, false, GridFileType.UseExtension, cback)) return false;
            GridHeader hd = new GridHeader();
            hd.CopyFrom(inputGrid.Header);
            inputGrid.Close();

            //create output grid object
            if (System.IO.File.Exists(output))
            {
                System.IO.File.Delete(output);
            }
            MapWinGIS.Grid outputGrid = new MapWinGIS.Grid();
            if (!outputGrid.CreateNew(output, hd, GridDataType.FloatDataType, -1.0, true, GridFileType.Binary, cback))
            {
                if (cback != null)
                {
                    string msg = outputGrid.get_ErrorMsg(outputGrid.LastErrorCode);
                    cback.Error("", msg);
                }
                return false;
            }
            
            int numRows = hd.NumberRows;
            int numCols = hd.NumberCols;
            double cellSize = hd.dX;

            float noData = Convert.ToSingle(hd.NodataValue);

            float[] vals = new float[numCols];

            float maxSquareDist = (float)(maxDistance * maxDistance);

            //read values into the row
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    int val = array[row][col];

                    if (val > maxSquareDist) //distance greater then max.distance
                    {
                        vals[col] = noData;
                    }
                    else if (val > 0)
                    {
                        //here we compute the actual distance
                        vals[col] = (float)(Math.Sqrt((double)val) * cellSize);
                    }
                    else
                    {
                        vals[col] = 0.0f;
                    }
                }
                //write the row to output grid
                outputGrid.PutRow(row, ref vals[0]);

            }

            outputGrid.Save(output, GridFileType.Binary, cback);
            outputGrid.Close();
            return true;
        }


        /// <summary>
        /// Reads the input raster file into a jagged in-memory array. The cell values are converted
        /// to 0 (target cell) or int.maxValue (all other cells)
        /// </summary>
        /// <param name="input">input raster file</param>
        /// <param name="targetValue">all cells with this value are considered 'target cells'</param>
        /// <param name="cback">for reporting progress</param>
        private static int[][] ReadInput(string input, double targetValue, ICallback cback)
        {
            //open input raster file
            Grid grd = new Grid();
            if (!grd.Open(input, GridDataType.UnknownDataType, true, GridFileType.UseExtension, cback))
            {
                cback.Error("", "Unable to open grid " + input);
                return null;
            }

            //initialize the array 
            int numRows = grd.Header.NumberRows;
            int numCols = grd.Header.NumberCols;
            int[][] array = new int[numRows][];
            
            for (int i = 0; i < numRows; i++)
            {
                array[i] = new int[numCols];
            }

            //read each raster row into the array. Target cells will be assigned a zero value.
            //other cells will be assigned an int.MaxValue value.
            float fTargetVal = (float)targetValue;
            int noTarget = int.MaxValue;
            float[] vals = new float[numCols];

            for (int row = 0; row < numRows; row++)
            {
                grd.GetRow(row, ref vals[0]);

                for (int col = 0; col < numCols; col++)
                {
                    if (vals[col] == fTargetVal)
                    {
                        array[row][col] = 0;
                    }
                    else
                    {
                        array[row][col] = noTarget;
                    }
                }
            }
            grd.Close();
            grd = null;
            return array;
        }

        #endregion
    }
}
