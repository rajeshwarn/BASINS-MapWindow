//********************************************************************************************************
//File name: DPSimplification.cs
//Description: Generalize(Simplify) a line shapefile using the Douglas-Peucker line simplification algorithm.
//             This algorithm has a 'tolerance' parameter which specifies the maximum allowed deviation of the
//             simplified polyline from the original polyline. This class utilizes methods of the
//             MapWinGeoProc.NTS.Topology.Simplify.DouglasPeuckerLineSimplifier class. Currently the output of the
//             class can be a point shapefile or a list of (x,y, z) points.
//               
//********************************************************************************************************
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//01/21/2009 Jiri Kadlec provided the initial implementation						
//********************************************************************************************************


using System;
using System.Collections.Generic;
using MapWinGeoProc.NTS.Topology.Geometries;
using MapWinGeoProc.NTS.Topology.Simplify;
using MapWindow.Interfaces.Geometries;
using System.Text;

namespace MapWinGeoProc.Generalization
{
    /// <summary>
    /// This class provides methods for generalization of polyline
    /// shapefiles using the Douglas-Peucker line simplification
    /// algorithm
    /// </summary>
    public class DPSimplification
    {
        /// <summary>
        /// Generalization of polyline
        /// shapefiles using the Douglas-Peucker line simplification
        /// algorithm. This method will output a line shapefile.
        /// </summary>
        /// <param name="inFileName">Input shapefile</param>
        /// <param name="outFileName">Output shapefile</param>
        /// <param name="tolerance">tolerance parameter - 
        /// specfies the maximum allowed distance between original polyline
        /// and simplified polyline</param>
        /// <param name="cback">Use this parameter for reporting progress. Set to null if not needed</param>
        public static void Generalize(string inFileName, string outFileName, double tolerance, MapWinGIS.ICallback cback)
        {
            MapWinGIS.Shapefile oldSF = new MapWinGIS.Shapefile();
            if (!oldSF.Open(inFileName, null))
            {
                throw new ArgumentException(string.Format("Shapefile {0} could not be opened. Error: {1}",
                    inFileName, oldSF.get_ErrorMsg(oldSF.LastErrorCode)));
            }

            //Check if it's a line shapefile
            if (!(oldSF.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYLINE ||
                oldSF.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYLINEM ||
                oldSF.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYLINEZ))
            {
                throw new ArgumentException(string.Format("Shapefile {0} must be a polyline shapefile.", inFileName));
            }

            int numShapes = oldSF.NumShapes;
            int numFields = oldSF.NumFields;
            
            //create a new output shapefile
            MapWinGIS.Shapefile newSF = new MapWinGIS.Shapefile();
            MapWinGIS.ShpfileType sftype = MapWinGIS.ShpfileType.SHP_POLYLINE;

            // if shapefile exists - open it and clear all shapes
            if (System.IO.File.Exists(outFileName))
            {
                try
                {
                    //TODO: ask for overwriting..
                    bool deleted = MapWinGeoProc.DataManagement.DeleteShapefile(ref outFileName);
                }
                finally
                {

                }
            }
                
                if (!newSF.CreateNew(outFileName, sftype))
                {
                    throw new InvalidOperationException
                        ("Error creating shapefile " + outFileName + " " + newSF.get_ErrorMsg(newSF.LastErrorCode));
                }
                newSF.StartEditingShapes(true, cback);

            //Copy all fields
            if (!Globals.CopyFields(ref oldSF, ref newSF))
            {
                throw new InvalidOperationException(string.Format("Error copying fields from {0} to {1}",
                    oldSF.Filename, newSF.Filename));
            }

            int newShapeIndex = 0;
            for (int shpIdx = 0; shpIdx < numShapes; ++shpIdx)
            {
                MapWinGIS.Shape shp = oldSF.get_Shape(shpIdx);

                // convert each part of the polyline shape to a 'geometry' object
                Geometry geom = MapWinGeoProc.NTS_Adapter.ShapeToGeometry(shp);
                for (int partIdx = 0; partIdx < geom.NumGeometries; ++partIdx)
                {
                    Geometry geomPart = (Geometry)geom.GetGeometryN(partIdx);

                    //do the simplification
                    ICoordinate[] oldCoords = geomPart.Coordinates;
                    DouglasPeuckerLineSimplifier simplifier = new DouglasPeuckerLineSimplifier(oldCoords);
                    simplifier.DistanceTolerance = tolerance;
                    ICoordinate[] newCoords = simplifier.Simplify();

                    //convert the coordinates back to a geometry
                    Geometry newGeom = new LineString(newCoords);

                    //convert the geometry back to a shape
                    MapWinGIS.Shape newShape = MapWinGeoProc.NTS_Adapter.GeometryToShape(newGeom);
                    
                    //add the shape to the new shapefile
                    newShapeIndex = newSF.NumShapes;
                    if (newSF.EditInsertShape(newShape, ref newShapeIndex) == false)
                    {
                        throw new InvalidOperationException("Error inserting shape: " +
                            newSF.get_ErrorMsg(newSF.LastErrorCode));
                    }
                    //add attribute values
                    for (int fldIdx = 0; fldIdx < numFields; ++fldIdx)
                    {
                        object val = oldSF.get_CellValue(fldIdx, shpIdx);
                        if (newSF.EditCellValue(fldIdx, newSF.NumShapes - 1, val) == false)
                        {
                            throw new InvalidOperationException("Error editing cell value: " +
                                newSF.get_ErrorMsg(newSF.LastErrorCode));
                        }
                    }
                } 
            }
            //close the old shapefile
            oldSF.Close();

            //stop editing and close the new shapefile
            newSF.StopEditingShapes(true, true, cback);
            newSF.Close();
        }
    }
}
