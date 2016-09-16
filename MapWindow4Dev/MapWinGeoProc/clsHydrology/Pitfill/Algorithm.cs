using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Pitfill
{
    /// <summary>
    /// A class for storing the meat of the PitFill algorithm
    /// </summary>
    public class Algorithm
    {
        /// <summary>
        /// Public structure for storing a row, column and Z value
        /// </summary>
        public struct mPoint
        {
            /// <summary>
            /// The Y coordinate or Row value
            /// </summary>
            public int Row;
            /// <summary>
            /// The X coordinate or Column value
            /// </summary>
            public int Col;
            /// <summary>
            /// The Z coordinate or elevation value
            /// </summary>
            public float Z;
            /// <summary>
            /// Creates a new instance of the mPoint structure
            /// </summary>
            /// <param name="NewRow">The Row or Y coordinate</param>
            /// <param name="NewCol">The Column or X coordinate</param>
            /// <param name="NewZ">The Z or elevation value</param>
            public mPoint(int NewRow, int NewCol, float NewZ)
            {
                Row = NewRow;
                Col = NewCol;
                Z = NewZ;
            }
        }
       
        const int UP = 1;
        const int DOWN = 2;
        const int LEFT = 3;
        const int RIGHT = 4;
        /// <summary>
        /// This is the core algorithm to fill the depressions in an image
        /// This concept is masterminded by the self proclaimed genius Ted Dunsford
        /// and is being prepared for publication.
        /// </summary>
        /// <param name="mwSourceGrid">The MapWinGIS.Grid to read the actual elevation values from</param>
        /// <param name="mwDestGrid">The MapWinGIS.Grid to write the elevations to</param>
        /// <param name="SpawnPoints">A list of points on the edges where the algorithm will start </param>
        /// <param name="myFrame">The current sub-division of the overall grid</param>
        /// <param name="DepsExist">A list of booleans for each direction indicating if dependencies were found in neighboring frames</param>
        /// <param name="myProgress">A Dialogs.ProgressDialog indicating the current progress of the algorithm.</param>
        /// <returns>TimeSpan indicating the time the function took to run the algorithm</returns>
        public static System.TimeSpan FloodDependencies(MapWinGIS.Grid mwSourceGrid, MapWinGIS.Grid mwDestGrid, List<Frame.Loc> SpawnPoints, Frame myFrame, bool[] DepsExist, Dialogs.ProgressDialog myProgress)
        {
            string strSpawnPoints = "null";
            string strFrame = "null";
            int Prog = 0;
            int OldProg = 0;
            
            if(SpawnPoints != null)
            {
                strSpawnPoints = SpawnPoints.Count.ToString();
            }
            if(myFrame != null)
            {
                strFrame = "Frame [" + myFrame.Width.ToString() + " x " + myFrame.Height.ToString() + "]";
            }

            MapWinUtility.Logger.Dbg("FloodDependencies(mwSourceGrid: " + Macro.ParamName(mwSourceGrid) + ",\n" +
                                     "                  mwDestGrid: " + Macro.ParamName(mwDestGrid) + ",\n" +
                                     "                  SpawnPoints: " + strSpawnPoints + " Locations,\n" +
                                     "                  myFrame: " + strSpawnPoints + ",\n" +
                                     "                  DepsExist: " + DepsExist.ToString() + 
                                     "                  ProgressDialog)");
            
            if (mwSourceGrid == null)
            {
                MapWinUtility.Logger.Dbg("Argument Exception: mwSourceGrid cannot be null.");
                throw new ArgumentException("mwSourceGrid cannot be null.");
            }

            if (mwDestGrid == null)
            {
                MapWinUtility.Logger.Dbg("Argument Exception: mwDestGrid cannot be null.");
                throw new ArgumentException("mwDestGrid cannot be null.");
            }
            if (SpawnPoints == null)
            {
                MapWinUtility.Logger.Dbg("Argument Exception: SpawnPoints cannot be null.");
                throw new ArgumentException("SpawnPoints cannot be null.");
            }
            if (myFrame == null)
            {
                MapWinUtility.Logger.Dbg("Argument Exception: myFrame cannot be null.");
                throw new ArgumentException("myFrame cannot be null.");
            }
            System.Drawing.Rectangle Window = myFrame.Rectangle;

            float[][] Source;
            float[][] Dest;
            int numRows = Window.Height;
            int numCols = Window.Width;
            mPoint pt;
            Frame.Loc ptStart;
            List<mPoint> DryCells = new List<mPoint>();
            List<mPoint> WetCells = new List<mPoint>();
            float[] Top = new float[numCols];
            float[] Bottom = new float[numCols];
            float[] Left = new float[numRows];
            float[] Right = new float[numRows];

            // Since we are evaluating this frame, we can clear its dependencies
            myFrame.Set_HasDependencies(UP, false);
            myFrame.Set_HasDependencies(DOWN, false);
            myFrame.Set_HasDependencies(LEFT, false);
            myFrame.Set_HasDependencies(RIGHT, false);
            
            myProgress.WriteMessage("Loading File...");
            MapWinUtility.Logger.Progress("Loading File...", Prog, OldProg);
            
            Source = myFrame.GetArrayFromWindow(mwSourceGrid, myProgress.ICallBack);
            // Initialize first time around.  We will be writing over the "copy" with
            // any changes we make, but this should save some time in file handling
            if (myFrame.Parent.First_Time(myFrame.X, myFrame.Y) == true)
            {
                if (myFrame.Parent.numCols * myFrame.Parent.numRows > 64000000)
                {
                    // in the case where we copied the file, we need the middle cells to be set 
                    // to the float maximum for the algorithm to work.
                    Dest = myFrame.GetBorders(mwSourceGrid, mwDestGrid, myProgress.ICallBack);
                    myFrame.Parent.set_First_Time(myFrame.X, myFrame.Y, false);
                }
                else
                {
                    Dest = myFrame.GetArrayFromWindow(mwDestGrid, myProgress.ICallBack);
                }
            }
            else
            {
                Dest = myFrame.GetArrayFromWindow(mwDestGrid, myProgress.ICallBack);
            }

            DateTime AlgStart = new DateTime();
            AlgStart = DateTime.Now;
            for (int iPoint = 0; iPoint < SpawnPoints.Count; iPoint++)
            {
                ptStart = SpawnPoints[iPoint];
                pt = new mPoint(ptStart.Y, ptStart.X, Dest[ptStart.Y][ptStart.X]);
                if (Dest[ptStart.Y][ptStart.X] == Source[ptStart.Y][ptStart.X])
                {
                    DryCells.Add(pt);
                }
                else
                {
                    WetCells.Add(pt);
                }
            }
            // Even though we have specific dependency locations for our source, we can potentially
            // effect any other cells on the perimeter
            
            for (int col = 0; col < numCols; col++)
            {
                Top[col] = Dest[0][col];
                Bottom[col] = Dest[numRows - 1][col];
            }
            for (int row = 0; row < numRows; row++)
            {
                Left[row] = Dest[row][0];
                Right[row] = Dest[row][numCols - 1];
            }

            if (DryCells.Count > 0)
            {
                WetCells.AddRange(DryUpward(Source, ref Dest, numRows, numCols, DryCells, myProgress.ICallBack));
            }

            FloodFill(Source, ref Dest, numRows, numCols, WetCells, myProgress.ICallBack);

            TimeSpan AlgTime = DateTime.Now - AlgStart;
            myProgress.WriteMessage("Saving Array to File...");
            MapWinUtility.Logger.Progress("Saving Array to File...", Prog, OldProg);
            myFrame.SaveArrayToWindow(Dest, mwDestGrid, myProgress.ICallBack);
            myProgress.WriteMessage("Done.");
            MapWinUtility.Logger.Progress("", Prog, OldProg);
            byte[][] Dependencies = new byte[5][];
           
            Dependencies[UP] = new byte[numCols];
            Dependencies[DOWN] = new byte[numCols];
            DepsExist[UP] = false;
            DepsExist[DOWN] = false;
            for (int col = 0; col < numCols; col++)
            {
                if (Top[col] != Dest[0][col])
                {
                    DepsExist[UP] = true;
                    Dependencies[UP][col] = (byte)DOWN;
                    //if (Dest[0][col] < Minima[UP]) Minima[UP] = Dest[0][col];
                }
                else
                {
                    Dependencies[UP][col] = (byte)0;
                }
                if (Bottom[col] != Dest[numRows - 1][col])
                {
                    DepsExist[DOWN] = true;
                    Dependencies[DOWN][col] = (byte)UP;
                    //if (Dest[numRows - 1][col] < Minima[DOWN]) Minima[DOWN] = Dest[numRows - 1][col];
                }
                else
                {
                    Dependencies[DOWN][col] = (byte)0;
                }
            }
            // Save Dependencies to the files
            myFrame.Write_Dependencies(Dependencies[UP], UP);
            myFrame.Write_Dependencies(Dependencies[DOWN], DOWN);
            Dependencies[LEFT] = new byte[numRows];
            Dependencies[RIGHT] = new byte[numRows];
            DepsExist[LEFT] = false;
            DepsExist[RIGHT] = false;
            for (int row = 0; row < numRows; row++)
            {

                if (Left[row] != Dest[row][0])
                {
                    DepsExist[LEFT] = true;
                    Dependencies[LEFT][row] = (byte)RIGHT;
                    //if (Dest[row][0] < Minima[LEFT]) Minima[LEFT] = Dest[row][0];
                }
                else
                {
                    Dependencies[LEFT][row] = (byte)0;
                }
                if (Right[row] != Dest[row][numCols - 1])
                {
                    DepsExist[RIGHT] = true;
                    Dependencies[RIGHT][row] = (byte)LEFT;
                    //if (Dest[row][numCols - 1] < Minima[RIGHT]) Minima[RIGHT] = Dest[row][numCols - 1];
                }
                else
                {
                    Dependencies[RIGHT][row] = (byte)0;
                }
            }
            // Save Vertical Dependencies to the file
            myFrame.Write_Dependencies(Dependencies[LEFT], LEFT);
            myFrame.Write_Dependencies(Dependencies[RIGHT], RIGHT);

            // Update the status of the neighbors
            myFrame.Update_Neighbor_Status(DepsExist);
            MapWinUtility.Logger.Dbg("Finished FloodDependencies");
            return AlgTime;
        }
        // Flood cells represent cells that were changed in the last iteration
        // initially this should be on the order of the perimeter in size but will get "larger"
        // as the "overwriting" takes over.  Flood areas overwrite with lower values, spreading the
        // lowest value as much as possible
        private static void FloodFill(float[][] Source, ref float[][] Dest, int numRows, int numCols, List<mPoint> WetCells, MapWinGIS.ICallback ICallBack)
        {
            mPoint pt;
            List<mPoint> NewCells;
            List<mPoint> DryCells;
            int[] dirX = { 1, 1, 0, -1, -1, -1, 0, 1 };
            int[] dirY = { 0, 1, 1, 1, 0, -1, -1, -1 };
            long Count = 0;
            int Percent;
            int OldPercent = 0;
            int Cycles = 0;
            while (WetCells.Count > 0)
            {
                
                NewCells = new List<mPoint>();
                for (int iCell = 0; iCell < WetCells.Count; iCell++)
                {
                    pt = WetCells[iCell];
                    if (Dest[pt.Row][pt.Col] < pt.Z) pt.Z = Dest[pt.Row][pt.Col];

                    int X, Y;
                    for (int dr = 0; dr < 8; dr++)
                    {
                        // ensure the neighbor is inside the image
                        Y = pt.Row + dirY[dr];
                        if (Y < 0 || Y > numRows - 1) continue;
                        X = pt.Col + dirX[dr];
                        if (X < 0 || X > numCols - 1) continue;
                        // if our neighbor is dry, don't do anything
                        if (Dest[Y][X] == Source[Y][X]) continue;
                        // if our neighbor's water level is below ours, change us
                        if (Dest[Y][X] < pt.Z)
                        {
                            // if we are wet, we might as well save time and drop our level to that level
                            if (pt.Z > Source[pt.Row][pt.Col])
                            {
                                // if our source is higher, dry us out
                                if (Source[pt.Row][pt.Col] >= Dest[Y][X])
                                {
                                    Dest[pt.Row][pt.Col] = Source[pt.Row][pt.Col];
                                    
                                    pt.Z = Source[pt.Row][pt.Col];
                                    DryCells = new List<mPoint>();
                                    DryCells.Add(pt);
                                    NewCells.AddRange(DryUpward(Source, ref Dest, numRows, numCols, DryCells, ICallBack));
                                    //We don't need to consider this direction for spreading since it was dry
                                }
                                else
                                {
                                    
                                    Dest[pt.Row][pt.Col] = Dest[Y][X];
                                    pt.Z = Dest[pt.Row][pt.Col];
                                    NewCells.Add(pt);
                                    break;

                                }
                            }
                            // if we made it here, we don't have to update ourself, but we don't 
                            // change our neighbor either
                            continue;
                        }

                        // if our neighbor's source is higher than the water level, dry our neighbor
                        if (Source[Y][X] >= pt.Z)
                        {
                            
                            Dest[Y][X] = Source[Y][X];
                            DryCells = new List<mPoint>();
                            DryCells.Add(new mPoint(Y, X, Source[Y][X]));
                            NewCells.AddRange(DryUpward(Source, ref Dest, numRows, numCols, DryCells, ICallBack));
                        }
                        else
                        {
                            if (Dest[Y][X] > pt.Z)
                            {
                                // if our neighbor's water level is higher than ours, lower it
                                
                                Dest[Y][X] = pt.Z;
                                NewCells.Add(new mPoint(Y, X, pt.Z));
                            }
                            else
                            {
                                // our neighbor's water level is the same as ours, so do nothing
                                continue;
                            }
                        }
                        // our neighbor was changed
                    }
                }
                WetCells = NewCells;
                if (ICallBack != null)
                {
                    // assume an average of 3*NumRows*NumCols
                    Count += NewCells.Count;
                    Cycles = NewCells.Count;
                    Percent = Convert.ToInt32(100 * Count / (2 * numCols * numRows));
                    if (Percent > OldPercent + 3)
                    {
                        if (Percent < 100)
                        {
                            ICallBack.Progress("status", Percent, "Count: " + Count + ",  Mem: " + Cycles);
                            OldPercent = Percent;
                        }
                        else
                        {
                            ICallBack.Progress("status", 100, "Count: " + Count + ",  Mem: " + Cycles);
                            OldPercent = Percent;
                        }
                    }
                }
                

            }
            // We succeeded in completely filling this image so we never have to come back
            return;
        }// End FloodFill

        // This handles situations where uphill values come in direct contact with locations directly below them that are dry, and 
        // therefore must be dry themselves. (And not a pit).
        private static List<mPoint> DryUpward(float[][] Source, ref float[][] Dest, int numRows, int numCols, List<mPoint> DryCells, MapWinGIS.ICallback ICallBack)
        {
            mPoint pt;
            List<mPoint> NewCells;
            List<mPoint> WetCells = new List<mPoint>();
            int[] dirX = { 1, 1, 0, -1, -1, -1, 0, 1 };
            int[] dirY = { 0, 1, 1, 1, 0, -1, -1, -1 };

            while (DryCells.Count > 0)
            {
                NewCells = new List<mPoint>();
                for (int iCell = 0; iCell < DryCells.Count; iCell++)
                {
                    pt = DryCells[iCell];
                    for (int dr = 0; dr < 8; dr++)
                    {
                        int X, Y;
                        // ensure the neighbor is inside the image
                        Y = pt.Row + dirY[dr];
                        if (Y < 0 || Y > numRows - 1) continue;
                        X = pt.Col + dirX[dr];
                        if (X < 0 || X > numCols - 1) continue;
                        // if our neighbor is dry, don't do anything
                        if (Dest[Y][X] == Source[Y][X]) continue;

                        // if our neighbor's source is higher than the water level, dry our neighbor
                        if (Source[Y][X] >= pt.Z)
                        {
                            Dest[Y][X] = Source[Y][X];
                            NewCells.Add(new mPoint(Y, X, Source[Y][X]));
                        }
                        else
                        {
                            // only tweak this if we will lower it!
                            if (Dest[Y][X] < pt.Z) continue;
                            // our neighbor can be dried to this level
                            Dest[Y][X] = pt.Z;
                            WetCells.Add(new mPoint(Y, X, pt.Z));
                        }
                    }
                }// End DryCell Loop
                DryCells = NewCells;
            }
            return WetCells;
        }// End FloodFill
    }

}
