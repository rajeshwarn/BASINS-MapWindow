// ********************************************************************************************************
// <copyright file="ScaleTools.cs" company="MapWindow.org">
//     Copyright (c) MapWindow Development team. All rights reserved.
// </copyright>
// Description: Class to return an extent based on a passed in scale and centerPoint
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specific language governing rights and 
// limitations under the License. 
//
// The Original Code is MapWindow Open Source.
//
// For more information about the Bresenham algorithm, see the Wikipedia 
// (http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm) 
//
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// 02/09/09 - Brian Marchionni  - Provided initial implementation
// 12/22/10 - Paul Meems        - Made this class StyleCop and ReSharper compliant and cleaned up the code
// ********************************************************************************************************

namespace MapWinGeoProc
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class with scale tools
    /// </summary>
    public static class ScaleTools
    {
        /// <summary>
        /// Returns an extent based on a passed in scale and centerPoint
        /// </summary>
        /// <param name="scale">The scale to use in the form of 1:scale</param>
        /// <param name="centerPoint">The center location to calculate the extents</param>
        /// <param name="mapWinUnits">The units that the map is in</param>
        /// <param name="mapWidthInPixels">Width of the map in pixels</param>
        /// <param name="mapHeightInPixels">Height of the map in pixels</param>
        /// <returns>The new extents. Returns null if something went wrong</returns>
        public static MapWinGIS.Extents ExtentFromScale(int scale, MapWinGIS.Point centerPoint, string mapWinUnits, int mapWidthInPixels, int mapHeightInPixels)
        {
            var convert = GetConversionFactor(mapWinUnits, centerPoint);
            if (convert == 0)
            {
                return null;
            }

            if (scale == 0)
            {
                return new MapWinGIS.Extents();
            }

            // 96 is the standard dpi for windows theres no easy way of getting it from the system so its just hard coded here
            // Technically this is incorrect but hey nobody will notice.
            var screenInchWidth = mapWidthInPixels / 96.0;
            var mapInchWidth = scale * screenInchWidth;
            var mapProjWidth = mapInchWidth / convert;

            var screenInchHeight = mapHeightInPixels / 96.0;
            var mapInchHeight = scale * screenInchHeight;
            var mapProjHeight = mapInchHeight / convert;

            var ext = new MapWinGIS.Extents();
            ext.SetBounds(centerPoint.x - (mapProjWidth / 2), centerPoint.y - (mapProjHeight / 2), 0, centerPoint.x + (mapProjWidth / 2), centerPoint.y + (mapProjHeight / 2), 0);
            return ext;
        }

        /// <summary>
        /// This returns a scale string based on an extent and MapUnits
        /// </summary>
        /// <param name="extent">The extent of the map</param>
        /// <param name="mapWinUnits">The units of the map</param>
        /// <param name="mapWidthInPixels">The width of the map</param>
        /// <param name="mapHeightInPixels">The height of the map</param>
        /// <returns>The calculated scale</returns>
        public static double CalcScale(MapWinGIS.Extents extent, string mapWinUnits, int mapWidthInPixels, int mapHeightInPixels)
        {
            // Enables the combo box if the map is in a know unit disables it if not            
            var pt = new MapWinGIS.Point
                     {
                         x = extent.xMin + ((extent.xMax - extent.xMin) / 2),
                         y = extent.yMin + ((extent.yMax - extent.yMin) / 2)
                     };
            var convert = GetConversionFactor(mapWinUnits, pt);

            // Get the height of the screen in map units
            var mapProjHeight = Math.Abs(extent.yMax - extent.yMin);

            // Get the width of the map and the screen in inches 
            // (This is an estimation based on OS parameters its almost certainly incorrect)
            var mapInchHeight = mapProjHeight * convert;
            var screenInchHeight = mapHeightInPixels / 96.0;
            return mapInchHeight / screenInchHeight;
        }

        /// <summary>
        /// Returns the conversion factor between the map units and inches
        /// </summary>
        /// <param name="mapWinUnits">A string represing the MapUnits</param>
        /// <param name="centerPoint">The center coodinates of the screen in MapUnits</param>
        /// <returns>A double representing the conversion factor between MapUnits and inches. If something goes wrong we return 0</returns>
        public static double GetConversionFactor(string mapWinUnits, MapWinGIS.Point centerPoint)
        {
            switch (mapWinUnits.ToLower())
            {
                case "lat/long":
                    return 4366141.73;
                case "meters":
                    return 39.3700787;
                case "centimeters":
                    return 0.393700787;
                case "feet":
                    return 12;
                case "inches":
                    return 1;
                case "kilometers":
                    return 39370.0787;
                case "miles":
                    return 63360;
                case "millimeters":
                    return 0.0393700787;
                case "yards":
                    return 36;
                case "us-ft":
                    return 12;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Returns the conversion factor if the map is in degrees
        /// </summary>
        /// <param name="centerPoint">The center point</param>
        /// <returns>The width in meters</returns>
        public static double WidthOfLongitude(MapWinGIS.Point centerPoint)
        {
            // Gets the latitude of the center of the screen in radians
            var centerLat = centerPoint.y * Math.PI / 180;

            // The define the spheroid of the earth in meters
            const double A = 6378137;
            const double B = 6356752.3;

            // Calculates the Earth's Merdional Radius (mR) at a specific latitude
            var numerator = (Math.Pow(A, 4) * Math.Pow(Math.Cos(centerLat), 2)) + (Math.Pow(B, 4) * Math.Pow(Math.Sin(centerLat), 2));
            var denominator = Math.Pow(A * Math.Cos(centerLat), 2) + Math.Pow(B * Math.Sin(centerLat), 2);
            var mR = Math.Sqrt(numerator / denominator);

            // Returns width of a degree at a specific latitude in inches
            return Math.PI / 180 * Math.Cos(centerLat) * mR;
        }

        /// <summary>
        /// Converts the project map units to MapWinGIS.tkUnitsOfMeasure
        /// </summary>
        /// <param name="projectMapUnits">The project map units, derived from the projection</param>
        /// <returns>The map units as MapWinGIS.tkUnitsOfMeasure</returns>
        public static MapWinGIS.tkUnitsOfMeasure GetMapUnits(string projectMapUnits)
        {
            switch (projectMapUnits.ToLower())
            {
                case "lat/long":
                    return MapWinGIS.tkUnitsOfMeasure.umDecimalDegrees;
                case "meters":
                    return MapWinGIS.tkUnitsOfMeasure.umMeters;
                case "centimeters":
                    return MapWinGIS.tkUnitsOfMeasure.umCentimeters;
                case "feet":
                    return MapWinGIS.tkUnitsOfMeasure.umFeets;
                case "inches":
                    return MapWinGIS.tkUnitsOfMeasure.umInches;
                case "kilometers":
                    return MapWinGIS.tkUnitsOfMeasure.umKilometers;
                case "miles":
                    return MapWinGIS.tkUnitsOfMeasure.umMiles;
                case "millimeters":
                    return MapWinGIS.tkUnitsOfMeasure.umMiliMeters;
                case "yards":
                    return MapWinGIS.tkUnitsOfMeasure.umYards;
                case "us-ft":
                    return MapWinGIS.tkUnitsOfMeasure.umFeets;
                default:
                    return MapWinGIS.tkUnitsOfMeasure.umMeters;
            }
        }
    }
}
