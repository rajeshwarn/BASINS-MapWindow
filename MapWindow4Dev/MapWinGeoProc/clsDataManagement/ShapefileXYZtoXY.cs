// ********************************************************************************************************
// <copyright file="ShapefileXYZtoXY.cs" company="MapWindow.org">
//     Copyright (c) MapWindow Development team. All rights reserved.
// </copyright>
// Description: Converts a shapefile Z into a regular shapefile without Z values.
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
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// 01/22/08 - Jiri Kadlec - Provided initial implementation
// 12/21/10 - Paul Meems  - Made this class StyleCop and ReSharper compliant and cleaned up the code
// ********************************************************************************************************

namespace MapWinGeoProc
{
    using System;
    using System.IO;

    /// <summary>
    /// Class to convert a shapefile Z into a regular shapefile without Z values
    /// </summary>
    public class ShapefileXYZtoXY
    {
        /// <summary>
        /// Converts a shapefile Z into a regular shapefile without Z values
        /// </summary>
        /// <param name="inputFilename">The name of the input shapefile</param>
        /// <param name="outputFilename">The name of the output shapefile</param>
        /// <returns>True on success</returns>
        public bool Convert(string inputFilename, string outputFilename)
        {
            // Opens the shapefiles for processing
            var inputSF = new MapWinGIS.Shapefile();
            var outputSF = new MapWinGIS.Shapefile();

            if (!inputSF.Open(inputFilename, null))
            {
                inputSF.Close();
                throw new ApplicationException(inputSF.get_ErrorMsg(inputSF.LastErrorCode));
            }

            if (!outputSF.CreateNew(outputFilename, inputSF.ShapefileType))
            {
                inputSF.Close();
                throw new ApplicationException(outputSF.get_ErrorMsg(outputSF.LastErrorCode));
            }

            outputSF.StartEditingShapes(true, null);

            // Loops through all the shapes to modify them
            var numShapes = inputSF.NumShapes;
            for (var shpIndex = 0; shpIndex < numShapes; shpIndex++)
            {
                var oldShape = inputSF.get_Shape(shpIndex);
                var resultShape = new MapWinGIS.Shape();
                resultShape.Create(outputSF.ShapefileType);
                var numPoints = 0;
                var numParts = inputSF.get_Shape(shpIndex).NumParts;
                var points = (double[])inputSF.QuickPoints(shpIndex, ref numPoints);

                // Deal with multi-part shapes
                if (numParts > 1)
                {
                    for (var j = 0; j <= numParts - 1; j++)
                    {
                        var begPart = oldShape.get_Part(j);
                        int endPart;
                        if (j < numParts - 1)
                        {
                            endPart = oldShape.get_Part(j + 1);
                        }
                        else
                        {
                            endPart = oldShape.numPoints;
                        }

                        var partIndex = j;
                        resultShape.InsertPart(begPart, ref partIndex);

                        // Project each point of the current part
                        for (var k = begPart; k <= endPart - 1; k++)
                        {
                            var pt = new MapWinGIS.PointClass
                                     {
                                         x = points[2 * j],
                                         y = points[(2 * j) + 1]
                                     };

                            if (!resultShape.InsertPoint(pt, ref j))
                            {
                                inputSF.Close();
                                outputSF.Close();
                                throw new ApplicationException(resultShape.get_ErrorMsg(resultShape.LastErrorCode));
                            }
                        } // end of looping through points in part
                    } // end of looping through parts of a multi-part shape
                }
                else 
                {
                    // not a multi-part shape
                    for (var j = 0; j < numPoints; j++)
                    {
                        var pt = new MapWinGIS.PointClass
                                     {
                                         x = points[2 * j],
                                         y = points[(2 * j) + 1]
                                     };

                        if (!resultShape.InsertPoint(pt, ref j))
                        {
                            inputSF.Close();
                            outputSF.Close();
                            throw new ApplicationException(resultShape.get_ErrorMsg(resultShape.LastErrorCode));
                        }
                    }
                }

                // TODO Check if shape is valid:

                // Adds the shape to the shapefile
                if (!outputSF.EditInsertShape(resultShape, ref shpIndex))
                {
                    inputSF.Close();
                    outputSF.Close();
                    throw new ApplicationException(outputSF.get_ErrorMsg(outputSF.LastErrorCode));
                }
            } // end of looping through shapes

            // We close everything
            inputSF.Close();
            outputSF.StopEditingShapes(true, true, null);
            outputSF.Close();

            // Creates a copy of the attribute table so that we don't have to do it in code, much faster this way
            DataManagement.TryCopy(Path.ChangeExtension(inputFilename, ".dbf"), Path.ChangeExtension(outputFilename, ".dbf"));

            return true;
        }
    }
}
