//********************************************************************************************************
//File name: UnitConverter.cs
//Description: Methods for length and area unit conversion
//********************************************************************************************************
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
//28.Aug. 2008 - jk - Jiri Kadlec - Initial version						
//********************************************************************************************************

using System;
using MapWindow.Interfaces;

namespace MapWinGeoProc
{
    /// <summary>
    /// Class for converting between different distance / area units
    /// </summary>
    public class UnitConverter
    {
        #region Public Methods
        /// <summary>
        /// Distance unit conversion
        /// </summary>
        /// <param name="convertFrom">The original unit to convert from</param>
        /// <param name="convertTo">The new unit to convert to</param>
        /// <param name="orgMeasurement">The measured distance to convert from (in original units)</param>
        /// <returns>The conversion result (in the new units)</returns>
        public static double ConvertLength(UnitOfMeasure convertFrom, UnitOfMeasure convertTo, double orgMeasurement)
        {
            // acres and hectares are not distance units
            if (convertTo == UnitOfMeasure.Acres || convertTo == UnitOfMeasure.Hectares ||
                convertFrom == UnitOfMeasure.Acres || convertFrom == UnitOfMeasure.Hectares)
            {
                throw new ArgumentException("Area units such as acres and hectares are not supported by " +
                    "the ConvertLength method. Please use ConvertArea() instead.");
            }
            
            return orgMeasurement * CalcConversionFactor(convertFrom, convertTo, MeasurementTypes.Length);
        }

        /// <summary>
        /// Area unit conversion
        /// </summary>
        /// <param name="convertFrom">The original unit to convert from</param>
        /// <param name="convertTo">The new unit to convert to</param>
        /// <param name="orgMeasurement">The measured area to convert from (in original units)</param>
        /// <returns>The conversion result (in the new units)</returns>
        public static double ConvertArea(UnitOfMeasure convertFrom, UnitOfMeasure convertTo, double orgMeasurement)
        {
            return orgMeasurement * CalcConversionFactor(convertFrom, convertTo, MeasurementTypes.Area);
        }

        /// <summary>
        /// Try to detect the units from a "proj4" string
        /// </summary>
        /// <param name="prj4"></param>
        /// <returns></returns>
        public static UnitOfMeasure GetShapefileUnits(string prj4)
        {
            //Also look at bug #732: MapUnits aren't correctly used in GISTools->Calculate area
            //This function should be available through the core.
            try
            {
                //Try to detect the unit from the proj4 string;
                if (prj4 == null || prj4 == "")
                    throw new ArgumentNullException("No projection provided.");

                if (prj4.ToLower().Contains("+units="))
                {
                    string[] components = prj4.ToLower().Substring(prj4.ToLower().IndexOf("+units=")).Split(Convert.ToChar(32));
                    string[] unitPart = components[0].Split(Convert.ToChar(61));
                    if (unitPart.Length > 1 && unitPart[0] == "+units")
                    {
                        if (unitPart[1] == "m")
                            return UnitOfMeasure.Meters;
                        if (unitPart[1] == "us-ft")
                            return UnitOfMeasure.Feet;
                    }
                }
                else
                {
                    //Is it latlong?
                    if (prj4.ToLower().Contains("longlat") || prj4.ToLower().Contains("latlong"))
                    {
                        return UnitOfMeasure.DecimalDegrees;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in getSFUnits: \n" + ex.ToString());
            }
            return UnitOfMeasure.Meters;
        }

        /// <summary>
        /// try to detect the distance/area unit name from it's type
        /// </summary>
        /// <param name="unitName">the unit name (must be MapWindow.Interfaces.UnitOfMeasure.ToString()</param>
        /// <returns>the unit as a MapWindow.Interfaces.UnitOfMeasure enumeration member</returns>
        public static UnitOfMeasure StringToUOM(string unitName)
        {
            try
            {
                string tmp = unitName.Replace(" Squared", "");
                tmp = tmp.ToLower();

                switch (tmp)
                {
                    case "centimeters":
                        return UnitOfMeasure.Centimeters;
                        
                    case "feet":
                        return UnitOfMeasure.Feet;
                       
                    case "inches":
                        return UnitOfMeasure.Inches;
                        
                    case "kilometers":
                        return UnitOfMeasure.Kilometers;
                        
                    case "meters":
                        return UnitOfMeasure.Meters;
                        
                    case "miles":
                        return UnitOfMeasure.Miles;
                       
                    case "millimeters":
                        return UnitOfMeasure.Millimeters;
                        
                    case "yards":
                        return UnitOfMeasure.Yards;
                        
                    case "nauticalmiles":
                    case "nautical miles":
                        return UnitOfMeasure.NauticalMiles;
                        
                    case "decimaldegrees":
                    case "lat/long":
                    case "deg":
                    case "deg.":
                        return UnitOfMeasure.DecimalDegrees;
                        
                    default:
                        return UnitOfMeasure.Meters;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in stringToUOM: \n" + ex.ToString());
            }

        }
        #endregion

        #region Private Methods
        //This is a simpler approach then the one used in the current GIS-Tool
        //But it's working ;)
        
        /// <summary>
        /// Calculate a conversion factor between the units
        /// (Original code by Paul Meems)
        /// </summary>
        /// <returns></returns>
        private static double CalcConversionFactor(UnitOfMeasure orgUnits, UnitOfMeasure calcUnits, MeasurementTypes measurementTypes)
        {
            try
            {
                // First we're going to determine the conversion factor from the orgUnits to meters
                // Second we're going to determine the conversion factor from the calcUnits to meters
                // Thirth the overall conversion factor is (conversion factor orgUnits) / (conversion factor calcUnits)

                Exception DecDegException = new Exception("Conversion To/From Decimal Degrees requires the use of 'FromDecimalDegrees' function or 'ToDecimalDegrees' function");
                double cfMapUnits = 1, cfCalcUnits = 1;
                // Step 1: convert to mapUnits to meters:
                switch (orgUnits)
                {
                    case UnitOfMeasure.Centimeters:
                        cfMapUnits = ConversionFactorsSQUnits.CentimetersToMeters;
                        break;
                    case UnitOfMeasure.DecimalDegrees:
                        // lsu: probably applicable only while measuring along the meridians on sphere
                        // but is useful approximation in case of equirectangular projection (decimal degrees as map units)
                        cfCalcUnits = ConversionFactorsSQUnits.DegreesToMeters;
                        //cfMapUnits = 1; //no conversion
                        break;
                    case UnitOfMeasure.Feet:
                        cfMapUnits = ConversionFactorsSQUnits.FeetToMeters;
                        break;
                    case UnitOfMeasure.Inches:
                        cfMapUnits = ConversionFactorsSQUnits.InchesToMeters;
                        break;
                    case UnitOfMeasure.Kilometers:
                        cfMapUnits = ConversionFactorsSQUnits.KilometersToMeters;
                        break;
                    case UnitOfMeasure.Miles:
                        cfMapUnits = ConversionFactorsSQUnits.MilesToMeters;
                        break;
                    case UnitOfMeasure.Millimeters:
                        cfMapUnits = ConversionFactorsSQUnits.MillimeterToMeters;
                        break;
                    case UnitOfMeasure.Yards:
                        cfMapUnits = ConversionFactorsSQUnits.YardsToMeters;
                        break;
                    case UnitOfMeasure.NauticalMiles:
                        cfMapUnits = ConversionFactorsSQUnits.NauticalMilesToMeters;
                        break;
                    case UnitOfMeasure.Acres:
                        cfMapUnits = ConversionFactorsSQUnits.AcresToMeters;   
                    // When using acres feet are more likely to be wanted for perimeter, thanks to Jack for pointing this out.
                        if (measurementTypes != MeasurementTypes.Area)
                            cfMapUnits = ConversionFactorsSQUnits.FeetToMeters;
                        break;
                    case UnitOfMeasure.Hectares:
                        cfMapUnits = ConversionFactorsSQUnits.HectaresToMeters;
                        if (measurementTypes != MeasurementTypes.Area)
                            cfMapUnits = ConversionFactorsSQUnits.MetersToMeters;
                        break;
                    default:
                        cfMapUnits = 1;
                        break;
                }
                if (measurementTypes == MeasurementTypes.Area && !(orgUnits == UnitOfMeasure.Acres || orgUnits == UnitOfMeasure.Hectares))
                    cfMapUnits *= cfMapUnits; // Squared

                // Step 2: convert to calcUnits to meters:
                switch (calcUnits)
                {
                    case UnitOfMeasure.Centimeters:
                        cfCalcUnits = ConversionFactorsSQUnits.CentimetersToMeters;
                        break;
                    case UnitOfMeasure.DecimalDegrees:
                        // lsu: probably applicable only while measuring along the meridians on sphere
                        // but is useful approximation in case of equirectangular projection (decimal degrees as map units)
                        cfCalcUnits = ConversionFactorsSQUnits.DegreesToMeters;
                        //cfCalcUnits = 1; //no conversion
                        break;
                    case UnitOfMeasure.Feet:
                        cfCalcUnits = ConversionFactorsSQUnits.FeetToMeters;
                        break;
                    case UnitOfMeasure.Inches:
                        cfCalcUnits = ConversionFactorsSQUnits.InchesToMeters;
                        break;
                    case UnitOfMeasure.Kilometers:
                        cfCalcUnits = ConversionFactorsSQUnits.KilometersToMeters;
                        break;
                    case UnitOfMeasure.Miles:
                        cfCalcUnits = ConversionFactorsSQUnits.MilesToMeters;
                        break;
                    case UnitOfMeasure.NauticalMiles:
                        cfCalcUnits = ConversionFactorsSQUnits.NauticalMilesToMeters;
                        break;
                    case UnitOfMeasure.Millimeters:
                        cfCalcUnits = ConversionFactorsSQUnits.MillimeterToMeters;
                        break;
                    case UnitOfMeasure.Yards:
                        cfCalcUnits = ConversionFactorsSQUnits.YardsToMeters;
                        break;
                    case UnitOfMeasure.Acres:
                        cfCalcUnits = ConversionFactorsSQUnits.AcresToMeters;
                        if (measurementTypes != MeasurementTypes.Area)
                            cfCalcUnits = ConversionFactorsSQUnits.FeetToMeters;
                        break;
                    case UnitOfMeasure.Hectares:
                        cfCalcUnits = ConversionFactorsSQUnits.HectaresToMeters;
                        if (measurementTypes != MeasurementTypes.Area)
                            cfCalcUnits = ConversionFactorsSQUnits.MetersToMeters;
                        break;
                    default:
                        cfCalcUnits = 1;
                        break;
                }
                if (measurementTypes == MeasurementTypes.Area && !(calcUnits == UnitOfMeasure.Acres || calcUnits == UnitOfMeasure.Hectares))
                    cfCalcUnits *= cfCalcUnits; // Squared
                return cfMapUnits / cfCalcUnits;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in convertUnits: \n" + ex.ToString());
            }
        }
        #endregion

        
        /// <summary>
        /// The unit conversion factors
        /// (original code by Paul Meems)
        /// </summary>
        static class ConversionFactorsSQUnits
        {
            public const double KilometersToMeters = 1000;
            public const double CentimetersToMeters = 0.01;
            public const double MillimeterToMeters = 0.001;
            public const double MilesToMeters = 1609.344;
            public const double YardsToMeters = 0.9144;
            public const double FeetToMeters = 0.3048;
            public const double InchesToMeters = 0.0254;
            public const double NauticalMilesToMeters = 1852.000;
            public const double AcresToMeters = 4046.856;
            public const double HectaresToMeters = 10000;
            public const double MetersToMeters = 1;
            public const double DegreesToMeters = 110899.999942;    // degrees to inches from MapWinGIS is taken and multiplied by 0.0254
        }

        enum MeasurementTypes
        {
            Length,
            Area
        }
    }
}
