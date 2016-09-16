using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace MapWinGeoProc
{
    /// <summary>
    /// Class to hold interpolation functions
    /// </summary>
    public class Interpolation
    {

        /// <summary>
        /// A class to store interpolation point information
        /// </summary>
        private class InterpolationPoint
        {
            /// <summary>
            /// X value
            /// </summary>
            public double X;
            /// <summary>
            /// Y value
            /// </summary>
            public double Y;
            /// <summary>
            /// Value to interpolate
            /// </summary>
            public double Value;
            /// <summary>
            /// Changing distance value used as a marker for sorting
            /// </summary>
            public double Distance;

            /// <summary>
            /// Default constructor
            /// </summary>
            public InterpolationPoint()
            {
                Distance = 0;
            }

            /// <summary>
            /// Populating constructor that autofills as it creates a new point
            /// </summary>
            /// <param name="inX"></param>
            /// <param name="inY"></param>
            /// <param name="inValue"></param>
            /// <param name="inDistance"></param>
            public InterpolationPoint(double inX, double inY, double inValue, double inDistance)
            {
                X = inX;
                Y = inY;
                Value = inValue;
                Distance = inDistance;
            }
        }

        /// <summary>
        /// Sorter class for InterpolationPoint classes
        /// </summary>
        private class InterpolationPointSorter : System.Collections.Generic.IComparer<InterpolationPoint>
        {
            int IComparer<InterpolationPoint>.Compare(InterpolationPoint x, InterpolationPoint y)
            {
                InterpolationPoint pt1, pt2;
                pt1 = x as InterpolationPoint;
                pt2 = y as InterpolationPoint;

                if (pt1.Distance > pt2.Distance)
                {
                    return 1;
                }
                else if (pt1.Distance < pt2.Distance)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }

        }

        /// <summary>
        /// Type of neighborhood search to use. 
        /// </summary>
        public enum IDWNeighborhoodType
        {
            /// <summary>
            /// Variable will search for a certain number of nearest points. 
            /// </summary>
            Variable,
            /// <summary>
            /// Fixed will search for all points in a fixed radius.
            /// </summary>
            Fixed,
            /// <summary>
            /// Unknown type
            /// </summary>
            Unknown
        }


        /// <summary>
        /// Inverse Distance Weighting Interpolation
        /// </summary>
        /// <param name="InPointsPath">Input point shapefile path to interpolate</param>
        /// <param name="InValueFieldIndex">Input field index where interpolation value is stored</param>
        /// <param name="OutGridPath">Output grid path where interpolation is stored</param>
        public static void IDW(string InPointsPath, int InValueFieldIndex, string OutGridPath)
        {
            MapWinGIS.Shapefile sf = new MapWinGIS.Shapefile();
            sf.Open(InPointsPath, null);
            double height = Math.Abs(sf.Extents.yMax - sf.Extents.yMin);
            double width = Math.Abs(sf.Extents.xMax - sf.Extents.xMin);
            double defaultCellSize = -1;
            if (height <= width)
            {
                defaultCellSize = height / 250;
            }
            else
            {
                defaultCellSize = width / 250;
            }
            sf.Close();

            IDW(InPointsPath, InValueFieldIndex, OutGridPath, defaultCellSize);
        }

        /// <summary>
        /// Inverse Distance Weighting Interpolation
        /// </summary>
        /// <param name="InPointsPath">Input point shapefile path to interpolate</param>
        /// <param name="InValueFieldIndex">Input field index where interpolation value is stored</param>
        /// <param name="OutGridPath">Output grid path where interpolation is stored</param>
        /// <param name="CellSize">Cell Size for output grid. Default lower of points extent height or width divided by 250</param>
        public static void IDW(string InPointsPath, int InValueFieldIndex, string OutGridPath, double CellSize)
        {
            IDW(InPointsPath, InValueFieldIndex, OutGridPath, CellSize, 2.0);
        }

        /// <summary>
        /// Inverse Distance Weighting Interpolation
        /// </summary>
        /// <param name="InPointsPath">Input point shapefile path to interpolate</param>
        /// <param name="InValueFieldIndex">Input field index where interpolation value is stored</param>
        /// <param name="OutGridPath">Output grid path where interpolation is stored</param>
        /// <param name="CellSize">Cell Size for output grid. Default lower of points extent height or width divided by 250</param>
        /// <param name="NeighborhoodType">Variable is a variable number of nearest points. Fixed is all points in a fixed distance</param>
        public static void IDW(string InPointsPath, int InValueFieldIndex, string OutGridPath, double CellSize, IDWNeighborhoodType NeighborhoodType)
        {
            IDW(InPointsPath, InValueFieldIndex, OutGridPath, CellSize, 2.0, NeighborhoodType);
        }

        /// <summary>
        /// Inverse Distance Weighting Interpolation
        /// </summary>
        /// <param name="InPointsPath">Input point shapefile path to interpolate</param>
        /// <param name="InValueFieldIndex">Input field index where interpolation value is stored</param>
        /// <param name="OutGridPath">Output grid path where interpolation is stored</param>
        /// <param name="CellSize">Cell Size for output grid. Default lower of points extent height or width divided by 250</param>
        /// <param name="Power">Power variable for IDW algorithm. 0 lt p lt 1 will give sharp changes. >1 will give smoother. Default 2.</param>
        public static void IDW(string InPointsPath, int InValueFieldIndex, string OutGridPath, double CellSize, double Power)
        {
            IDW(InPointsPath, InValueFieldIndex, OutGridPath, CellSize, Power, IDWNeighborhoodType.Variable);
        }

        /// <summary>
        /// Inverse Distance Weighting Interpolation
        /// </summary>
        /// <param name="InPointsPath">Input point shapefile path to interpolate</param>
        /// <param name="InValueFieldIndex">Input field index where interpolation value is stored</param>
        /// <param name="OutGridPath">Output grid path where interpolation is stored</param>
        /// <param name="CellSize">Cell Size for output grid. Default lower of points extent height or width divided by 250</param>
        /// <param name="Power">Power variable for IDW algorithm. 0 lt p lt 1 will give sharp changes. >1 will give smoother. Default 2.</param>
        /// <param name="NeighborhoodType">Variable is a variable number of nearest points. Fixed is all points in a fixed distance</param>
        public static void IDW(string InPointsPath, int InValueFieldIndex, string OutGridPath, double CellSize, double Power, IDWNeighborhoodType NeighborhoodType)
        {
            if (NeighborhoodType == IDWNeighborhoodType.Variable)
            {
                IDW(InPointsPath, InValueFieldIndex, OutGridPath, CellSize, Power, NeighborhoodType, 12, -1, null);
            }
            else
            {
                IDW(InPointsPath, InValueFieldIndex, OutGridPath, CellSize, Power, NeighborhoodType, 0, 5*CellSize, null);
            }
        }

        /// <summary>
        /// Inverse Distance Weighting Interpolation
        /// </summary>
        /// <param name="InPointsPath">Input point shapefile path to interpolate</param>
        /// <param name="InValueFieldIndex">Input field index where interpolation value is stored</param>
        /// <param name="OutGridPath">Output grid path where interpolation is stored</param>
        /// <param name="CellSize">Cell Size for output grid. Default lower of points extent height or width divided by 250</param>
        /// <param name="Power">Power variable for IDW algorithm. 0 lt p lt 1 will give sharp changes. >1 will give smoother. Default 2.</param>
        /// <param name="NeighborhoodType">Variable is a variable number of nearest points. Fixed is all points in a fixed distance</param>
        /// <param name="NeighborhoodCount">Variable Count is either number of nearest points to use. Fixed Count is minimum points needed to find valid interpolation</param>
        /// <param name="NeighborhoodDistance">Variable Distance is a maximum distance of nearest points. Fixed Distance is distance to use to select points</param>
        /// <param name="callback">A callback for progress information</param>
        public static void IDW(string InPointsPath, int InValueFieldIndex, string OutGridPath, double CellSize, double Power, IDWNeighborhoodType NeighborhoodType, int NeighborhoodCount, double NeighborhoodDistance, MapWinGIS.ICallback callback)
        {
            int newperc = 0, oldperc = 0;

            KDTreeDLL.KDTree pointsTree;
            double[][] Points;
            string[] PointVals;
            string proj, projUnits;
            MapWinGIS.Extents pointsExtents;
            KDTreeCachePoints(InPointsPath, InValueFieldIndex, out pointsTree, out Points, out PointVals, out proj, out projUnits, out pointsExtents, callback);

            MapWinGIS.Grid outGrid;
            DataManagement.DeleteGrid(ref OutGridPath);
            double NoDataValue = -32768;
            CreateGridFromExtents(pointsExtents, CellSize, proj, NoDataValue, OutGridPath, out outGrid);

            //Cycle grid and find interpolated value for each cell

            int nr = outGrid.Header.NumberRows;
            int nc = outGrid.Header.NumberCols;
            double sumWeight, sumWeightAndVal, projX, projY;
            int[] neighbors;
            double[] CellPoint;
            newperc = 0;
            oldperc = 0;
            for (int row = 0; row < nr; row++)
            {
                newperc = Convert.ToInt32((Convert.ToDouble(row) / Convert.ToDouble(nr)) * 100);
                if (callback != null) callback.Progress("Status", newperc, "IDW Row " + row.ToString() + "/" + nr.ToString());
                oldperc = newperc;

                newperc = 0;
                oldperc = 0;
                for (int col = 0; col < nc; col++)
                {
                    outGrid.CellToProj(col, row, out projX, out projY);
                    CellPoint = new double[2];
                    CellPoint[0] = projX;
                    CellPoint[1] = projY;

                    if (GetIDWNeighbors(CellPoint, ref pointsTree, ref Points, out neighbors, NeighborhoodType, NeighborhoodCount, NeighborhoodDistance, projUnits) == 0)
                    {

                        sumWeightAndVal = 0;
                        sumWeight = 0;

                        for (int i = 0; i < neighbors.Length; i++)
                        {
                            sumWeightAndVal += GetIDWeight(CellPoint, Points[neighbors[i]], projUnits, Power) * double.Parse(PointVals[neighbors[i]]);
                            sumWeight += GetIDWeight(CellPoint, Points[neighbors[i]], projUnits, Power);
                        }
                        outGrid.set_Value(col, row, (sumWeightAndVal / sumWeight));
                    }                    
                }
            }

            outGrid.Save(OutGridPath, MapWinGIS.GridFileType.UseExtension, null);
            outGrid.Close();
            if (callback != null) callback.Progress("Status", 0, "");
        }

        /// <summary>
        /// A function which turns a point shapefile with a given value field into a KDTree and correspondingly indexed points and values arrays
        /// </summary>
        /// <param name="InPointsPath">The path to the points shapefile</param>
        /// <param name="InValueFieldIndex">The index to the field to use for values</param>
        /// <param name="PointsTree">The out reference for the KDTree</param>
        /// <param name="Points">The out reference for the double array of x and y values of the points</param>
        /// <param name="PointVals">The out reference for the value array of points</param>
        /// <param name="Projection">The projection string of the in file</param>
        /// <param name="ProjectionUnits">The projection units of the in file</param>
        /// <param name="PointsExtents">The extents of the points shapefile</param>
        /// <param name="callback">A callback for progress</param>
        public static void KDTreeCachePoints(string InPointsPath, int InValueFieldIndex, out KDTreeDLL.KDTree PointsTree, out double[][] Points, out string[] PointVals, out string Projection, out string ProjectionUnits, out MapWinGIS.Extents PointsExtents, MapWinGIS.ICallback callback)
        {
            int newperc = 0, oldperc = 0;
            MapWinGIS.Shapefile pointsf = new MapWinGIS.Shapefile();
            pointsf.Open(InPointsPath, null);

            PointsExtents = pointsf.Extents;
            Projection = pointsf.Projection;
            if (Projection != null)
            {
                ProjectionUnits = Projection.Substring(Projection.IndexOf("units=") + 6);
                ProjectionUnits = ProjectionUnits.Substring(0, ProjectionUnits.IndexOf("+")).Trim();
            }
            else
            {
                double tmpX = pointsf.Extents.xMax;
                string tmpstr = Math.Floor(tmpX).ToString();

                if (tmpstr.Length > 4)
                {
                    ProjectionUnits = "";
                }
                else
                {
                    ProjectionUnits = "lat/long";
                }
            }

            PointsTree = new KDTreeDLL.KDTree(2);
            
            MapWinGIS.Point currpt;
            int ns = pointsf.NumShapes;
            Points = new double[ns][];
            PointVals = new string[ns];
            int duplicates = 0;
            for (int i = 0; i < ns; i++)
            {
                Points[i] = new double[2];

                newperc = Convert.ToInt32((Convert.ToDouble(i) / Convert.ToDouble(ns)) * 100);
                if ((newperc > oldperc))
                {
                    if (callback != null) callback.Progress("Status", newperc, "IDW Caching " + i.ToString());
                    oldperc = newperc;
                }

                currpt = pointsf.get_Shape(i).get_Point(0);
                Points[i][0] = currpt.x;
                Points[i][1] = currpt.y;
                PointVals[i] = pointsf.get_CellValue(InValueFieldIndex, i).ToString();

                try
                {
                    if (PointsTree.search(Points[i]) == null)
                    {
                        PointsTree.insert(Points[i], i);
                    }
                }
                catch (KDTreeDLL.KeyDuplicateException)
                {
                    duplicates++;
                }
            }
            pointsf.Close();
        }

        private static int GetIDWNeighbors(double[] InCellPoint, ref KDTreeDLL.KDTree InPointsTree, ref double[][] InPoints, out int[] Neighbors, IDWNeighborhoodType NeighborhoodType, int NeighborhoodCount, double NeighborhoodDistance, string ProjUnits)
        {
            int tmpIdx;
            Neighbors = new int[1];
            if (NeighborhoodType == IDWNeighborhoodType.Variable) //Taking NeighborhoodCount number of nearest points
            {
                if (NeighborhoodDistance == -1) //Then take NeighborhoodCount number of nearest points
                {
                    Object[] ptIndexObjs = InPointsTree.nearest(InCellPoint, NeighborhoodCount);
                    if (ptIndexObjs.Length == 0) return -1;
                    Neighbors = new int[ptIndexObjs.Length];
                    for (int i = 0; i < ptIndexObjs.Length; i++)
                    {
                        Neighbors[i] = (int) ptIndexObjs[i];
                    }
                }
                else //Take NeighborhoodCount of nearest points that are a maximum of NeighborhoodDistance away
                {
                    Object[] ptIndexObjs = InPointsTree.nearest(InCellPoint, NeighborhoodCount);
                    if (ptIndexObjs.Length == 0) return -1;
                    Neighbors = new int[ptIndexObjs.Length];
                    for (int i = 0; i < ptIndexObjs.Length; i++)
                    {
                        tmpIdx = (int)ptIndexObjs[i];
                        if (SpatialOperations.Distance(InCellPoint[0], InCellPoint[1], InPoints[tmpIdx][0], InPoints[tmpIdx][1], ProjUnits) <= NeighborhoodDistance)
                        {
                            Neighbors[i] = tmpIdx;
                        }
                        else
                        {
                            Neighbors[i] = -1;
                        }
                    }
                }
            }
            else if (NeighborhoodType == IDWNeighborhoodType.Fixed) //Taking all points in fixed distance
            {
                //Something

                //if (OutNeighbors.Count < NeighborhoodCount) //Test for minimum number of points found
                //{
                //    return -1;
                //    //Error
                //}
            }
            else
            {
                return -1;
                //Error
            }
            return 0;
        }
        
        private static double GetIDWeight(double[] InCellPoint, double[] InTestPoint, String ProjUnits, double Power)
        {
            return (1 / Math.Pow(SpatialOperations.Distance(InCellPoint[0], InCellPoint[1], InTestPoint[0], InTestPoint[1], ProjUnits), Power));
        }






        /// <summary>
        /// Inverse Distance Weighting Interpolation
        /// </summary>
        /// <param name="InPointsPath">Input point shapefile path to interpolate</param>
        /// <param name="InValueFieldIndex">Input field index where interpolation value is stored</param>
        /// <param name="OutGridPath">Output grid path where interpolation is stored</param>
        /// <param name="CellSize">Cell Size for output grid. Default lower of points extent height or width divided by 250</param>
        /// <param name="Power">Power variable for IDW algorithm. 0 lt p lt 1 will give sharp changes. >1 will give smoother. Default 2.</param>
        /// <param name="NeighborhoodType">Variable is a variable number of nearest points. Fixed is all points in a fixed distance</param>
        /// <param name="NeighborhoodCount">Variable Count is either number of nearest points to use. Fixed Count is minimum points needed to find valid interpolation</param>
        /// <param name="NeighborhoodDistance">Variable Distance is a maximum distance of nearest points. Fixed Distance is distance to use to select points</param>
        /// <param name="callback">A callback for progress information</param>
        public static void IDWBrute(string InPointsPath, int InValueFieldIndex, string OutGridPath, double CellSize, double Power, IDWNeighborhoodType NeighborhoodType, int NeighborhoodCount, double NeighborhoodDistance, MapWinGIS.ICallback callback)
        {
            int newperc = 0, oldperc = 0;

            List<InterpolationPoint> pointsCache;
            string proj, projUnits;
            MapWinGIS.Extents pointsExtents;
            CachePointsBrute(InPointsPath, InValueFieldIndex, out pointsCache, out proj, out projUnits, out pointsExtents, callback);

            MapWinGIS.Grid outGrid;
            DataManagement.DeleteGrid(ref OutGridPath);
            double NoDataValue = -32768;
            CreateGridFromExtents(pointsExtents, CellSize, proj, NoDataValue, OutGridPath, out outGrid);

            //Cycle grid and find interpolated value for each cell
            List<InterpolationPoint> neighbors;
            InterpolationPoint cellPoint;
            int nr = outGrid.Header.NumberRows;
            int nc = outGrid.Header.NumberCols;
            double sumWeight, sumWeightAndVal;
            newperc = 0;
            oldperc = 0;
            for (int row = 0; row < nr; row++)
            {
                newperc = Convert.ToInt32((Convert.ToDouble(row) / Convert.ToDouble(nr)) * 100);
                if (callback != null) callback.Progress("Status", newperc, "IDW Row " + row);

                newperc = 0;
                oldperc = 0;
                for (int col = 0; col < nc; col++)
                {
                    newperc = Convert.ToInt32((Convert.ToDouble(col) / Convert.ToDouble(nc)) * 100);
                    if ((newperc > oldperc))
                    {
                        if (callback != null) callback.Progress("Status", newperc, "IDW Row " + row.ToString() + "  Col " + col.ToString());
                        oldperc = newperc;
                    }

                    cellPoint = new InterpolationPoint();
                    outGrid.CellToProj(col, row, out cellPoint.X, out cellPoint.Y);
                    GetIDWNeighborsBrute(cellPoint, ref pointsCache, out neighbors, NeighborhoodType, NeighborhoodCount, NeighborhoodDistance, projUnits);

                    sumWeightAndVal = 0;
                    sumWeight = 0;

                    foreach (InterpolationPoint npoint in neighbors)
                    {
                        sumWeightAndVal += GetIDWeightBrute(cellPoint, npoint, projUnits, Power) * npoint.Value;
                        sumWeight += GetIDWeightBrute(cellPoint, npoint, projUnits, Power);
                    }

                    outGrid.set_Value(col, row, (sumWeightAndVal / sumWeight));
                }
            }
 
            outGrid.Save(OutGridPath, MapWinGIS.GridFileType.UseExtension, null);
            outGrid.Close();
        }
        
        private static void CachePointsBrute(string InPointsPath, int InValueFieldIndex, out List<InterpolationPoint> PointsCache, out string Projection, out string ProjectionUnits, out MapWinGIS.Extents PointsExtents, MapWinGIS.ICallback callback)
        {
            int newperc = 0, oldperc = 0;
            MapWinGIS.Shapefile points = new MapWinGIS.Shapefile();
            points.Open(InPointsPath, null);

            PointsExtents = points.Extents;
            Projection = points.Projection;
            if (Projection != null)
            {
                ProjectionUnits = Projection.Substring(Projection.IndexOf("units=") + 6);
                ProjectionUnits = ProjectionUnits.Substring(0, ProjectionUnits.IndexOf("+")).Trim();
            }
            else
            {
                double tmpX = points.Extents.xMax;
                string tmpstr = Math.Floor(tmpX).ToString();

                if (tmpstr.Length > 4)
                {
                    ProjectionUnits = "";
                }
                else
                {
                    ProjectionUnits = "lat/long";
                }
            }

            PointsCache = new List<InterpolationPoint>();
            InterpolationPoint pt;
            MapWinGIS.Point currpt;
            int ns = points.NumShapes;
            for (int i = 0; i < ns; i++)
            {
                newperc = Convert.ToInt32((Convert.ToDouble(i) / Convert.ToDouble(ns)) * 100);
                if ((newperc > oldperc))
                {
                    if (callback != null) callback.Progress("Status", newperc, "IDW Caching " + i.ToString());
                    oldperc = newperc;
                }

                currpt = points.get_Shape(i).get_Point(0);

                pt = new InterpolationPoint(currpt.x, currpt.y, double.Parse(points.get_CellValue(InValueFieldIndex, i).ToString()), 0);
                PointsCache.Add(pt);
            }
            points.Close();
        }

        private static int GetIDWNeighborsBrute(InterpolationPoint InCellPoint, ref List<InterpolationPoint> InPointCache, out List<InterpolationPoint> OutNeighbors, IDWNeighborhoodType NeighborhoodType, int NeighborhoodCount, double NeighborhoodDistance, string ProjUnits)
        {
            SortInterpolationPointsByDistanceBrute(InCellPoint, ref InPointCache, ProjUnits);

            OutNeighbors = new List<InterpolationPoint>();
            int numPts = 0;

            if (NeighborhoodType == IDWNeighborhoodType.Variable) //Taking NeighborhoodCount number of nearest points
            {
                if (NeighborhoodDistance == -1) //Then take NeighborhoodCount number of nearest points
                {
                    foreach (InterpolationPoint npoint in InPointCache)
                    {
                        OutNeighbors.Add(new InterpolationPoint(npoint.X, npoint.Y, npoint.Value, npoint.Distance));
                        numPts++;
                        if (numPts >= NeighborhoodCount) break;
                    }
                }
                else //Take NeighborhoodCount of nearest points that are a maximum of NeighborhoodDistance away
                {
                    foreach (InterpolationPoint npoint in InPointCache)
                    {
                        if (npoint.Distance <= NeighborhoodDistance)
                        {
                            OutNeighbors.Add(new InterpolationPoint(npoint.X, npoint.Y, npoint.Value, npoint.Distance));
                            numPts++;
                            if (numPts >= NeighborhoodCount) break;
                        }
                    }
                }
            }
            else if (NeighborhoodType == IDWNeighborhoodType.Fixed) //Taking all points in fixed distance
            {
                foreach (InterpolationPoint npoint in InPointCache)
                {
                    if (npoint.Distance <= NeighborhoodDistance)
                    {
                        OutNeighbors.Add(new InterpolationPoint(npoint.X, npoint.Y, npoint.Value, npoint.Distance));
                    }
                }
                if (OutNeighbors.Count < NeighborhoodCount) //Test for minimum number of points found
                {
                    return -1;
                    //Error
                }
            }
            else
            {
                return -1;
                //Error
            }
            return 0;
        }

        private static void SortInterpolationPointsByDistanceBrute(InterpolationPoint InCellPoint, ref List<InterpolationPoint> InPointCache, string ProjUnits)
        {
            foreach (InterpolationPoint cpoint in InPointCache)
            {
                cpoint.Distance = SpatialOperations.Distance(InCellPoint.X, InCellPoint.Y, cpoint.X, cpoint.Y, ProjUnits);
            }
            InterpolationPointSorter sorter = new InterpolationPointSorter();
            InPointCache.Sort(sorter);
        }

        private static double GetIDWeightBrute(InterpolationPoint CellPoint, InterpolationPoint TestPoint, String ProjUnits, double Power)
        {
            return (1/Math.Pow(SpatialOperations.Distance(CellPoint.X, CellPoint.Y, TestPoint.X, TestPoint.Y, ProjUnits), Power));
        }






        private static void CreateGridFromExtents(MapWinGIS.Extents InExtents, double CellSize, String Projection, double NoDataValue, string OutGridPath, out MapWinGIS.Grid OutGrid)
        {
            double height = Math.Abs(InExtents.yMax - InExtents.yMin);
            double width = Math.Abs(InExtents.xMax - InExtents.xMin);

            OutGrid = new MapWinGIS.Grid();
            MapWinGIS.GridHeader hdr = new MapWinGIS.GridHeader();

            hdr.dX = CellSize;
            hdr.dY = CellSize;
            hdr.NodataValue = NoDataValue;
            hdr.NumberRows = int.Parse(Math.Ceiling(height / CellSize).ToString());
            hdr.NumberCols = int.Parse(Math.Ceiling(width / CellSize).ToString());
            hdr.Projection = Projection;
            hdr.XllCenter = InExtents.xMin + 0.5 * CellSize;
            hdr.YllCenter = InExtents.yMin + 0.5 * CellSize;

            OutGrid.CreateNew(OutGridPath, hdr, MapWinGIS.GridDataType.DoubleDataType, NoDataValue, true, MapWinGIS.GridFileType.UseExtension, null);
        }
    
    }

}

