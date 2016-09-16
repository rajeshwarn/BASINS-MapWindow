////********************************************************************************************************
////File name: Projections.cs
////Description: Public class, provides methods for recording and retrieving errors.
////********************************************************************************************************
////The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
////you may not use this file except in compliance with the License. You may obtain a copy of the License at
////http://www.mozilla.org/MPL/
////Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
////ANY KIND, either express or implied. See the License for the specific language governing rights and
////limitations under the License.
////
////The Original Code is MapWindow Open Source.
////
////Contributor(s): (Open source contributors should list themselves and their modifications here).
////25-04-2010 DG - Dave Gilmore - Created Projections Class
////23-05-2011 Matthew K - Referenced DotSpatial.Projections implementation.
////********************************************************************************************************

//namespace MapWinGeoProc.Projections
//{
//    using System;
//    using System.Collections;
//    using System.Collections.Generic;

//    /// <summary>
//    /// Projection class used for all projection tasks
//    /// </summary>
//    public class Projections
//    {
//        #region Constructor Method

//        /// <summary>
//        /// Initalizes a new instance of the Projections class.
//        /// </summary>
//        public Projections()
//        {
//        }

//        #endregion Constructor Method

//        #region Methods

//        /// <summary>
//        /// Method that gets all the Major Projection categories
//        /// </summary>
//        /// <param name="geographic">Parameter to determine the type of projection - Set to false for Projected</param>
//        /// <returns>ArrayList of Major categories</returns>
//        public IEnumerable<string> GetMajorCategories(bool geographic)
//        {
//            string[] names;

//            if (geographic)
//            {
//                names = KnownCoordinateSystems.Geographic.Names;
//            }
//            else
//            {
//                names = KnownCoordinateSystems.Projected.Names;
//            }

//            // Sort in place
//            Array.Sort(names);

//            // Return Array
//            return names;
//        }

//        /// <summary>
//        /// Gets a list of all Minor Projection categories based on Selected Major Category
//        /// </summary>
//        /// <param name="majorCategory">Current Selected Major Category</param>
//        /// <param name="geographic">Parameter to determine the type of projection - Set to false for Projected</param>
//        /// <returns>ArrayList of Minor categories</returns>
//        public IEnumerable<string> GetMinorCategories(string majorCategory, bool geographic)
//        {
//            CoordinateSystemCategory csc;
//            if (geographic)
//            {
//                csc = KnownCoordinateSystems.Geographic.GetCategory(majorCategory);
//            }
//            else
//            {
//                csc = KnownCoordinateSystems.Projected.GetCategory(majorCategory);
//            }

//            // Sort in place
//            Array.Sort(csc.Names);

//            //// Return
//            return csc.Names;
//        }

//        /// <summary>
//        /// Projection Selection Dialogue
//        /// </summary>
//        /// <returns>Selected Coordinate System PROJ4 String</returns>
//        public string SelectProjection()
//        {
//            //// Instantiate Form Object
//            using (var projectionDialogue = new ProjectionSelectDialog())
//            {
//                //// Show Projection Selection
//                projectionDialogue.ShowDialog();
//                if (projectionDialogue.DialogResult == System.Windows.Forms.DialogResult.OK)
//                {
//                    return projectionDialogue.SelectedCoordinateSystem.ToProj4String();
//                }
//                else
//                {
//                    projectionDialogue.Close();
//                    return String.Empty;
//                }
//            }
//        }

//        #endregion Methods
//    }
//}