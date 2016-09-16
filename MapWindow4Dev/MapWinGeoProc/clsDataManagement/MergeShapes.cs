// ********************************************************************************************************
// <copyright file="MergeShapes.cs" company="MapWindow.org">
//     Copyright (c) MapWindow Development team. All rights reserved.
// </copyright>
// Description: Merges shapefiles
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
// 12/03/08 - Ted Dunsford (Shade1974) - Provided initial API and parameter descriptions
// 12/22/10 - Paul Meems - Made this class StyleCop and ReSharper compliant and cleaned up the code
// ********************************************************************************************************

namespace MapWinGeoProc
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Dialogs;
    using MapWinGIS;
    using MapWinUtility;

    /// <summary>
    /// Class to merge shapefiles
    /// </summary>
    /// <remarks>PM: If possible rename this to MergeShapefiles</remarks>
    public class clsMergeShapefiles
    {
        #region Variables

        /// <summary>The filename of the first shapefile</summary>
        private string inFile1;

        /// <summary>The filename of the second shapefile</summary>
        private string inFile2;

        /// <summary>The first shapefile</summary>
        private Shapefile inSF1;

        /// <summary>The second shapefile</summary>
        private Shapefile inSF2;

        /// <summary>The filename of the output shapefile</summary>
        private string outFile;

        /// <summary>The output shapefile</summary>
        private Shapefile outSF;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        // TODO Make this class so it works without dialogboxes as well

        /// <summary>
        /// Actually engages the append shapes process
        /// </summary>
        public void DoMergeShapefiles()
        {
            Logger.Dbg("DoMergeShapefiles()");
            string errorMessage;
            this.inSF1 = new Shapefile();
            this.inSF2 = new Shapefile();
            this.outSF = new Shapefile();

            this.GetFilenames();

            if (!this.OpenFiles(out errorMessage))
            {
                Logger.Message(errorMessage, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error, DialogResult.OK);
                return;
            }

            if (!this.outSF.StartEditingShapes(true, null))
            {
                this.CloseMessage(this.outSF.get_ErrorMsg(this.outSF.LastErrorCode));
                return;
            }

            // Determine a list of unique fields to add to the output shapefile.
            if (!this.CombineFields(out errorMessage))
            {
                this.CloseMessage(errorMessage);
                return;
            }

            if (!this.AddShapes(this.inSF1, out errorMessage))
            {
                this.CloseMessage(errorMessage);
                return;
            }

            if (!this.AddShapes(this.inSF2, out errorMessage))
            {
                this.CloseMessage(errorMessage);
                return;
            }

            this.inSF2.Close();
            this.inSF1.Close();
            if (!this.outSF.StopEditingShapes(true, true, null))
            {
                Logger.Message(
                    this.outSF.get_ErrorMsg(this.outSF.LastErrorCode), 
                    "File Error", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error, 
                    DialogResult.OK);
                return;
            }

            this.outSF.Close();
            Logger.Message(
                "Finished Merging Shapes", 
                "File Error", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information,
                DialogResult.OK);
        }

        /// <summary>
        /// Close files and show an error message 
        /// </summary>
        /// <param name="message">The error message</param>
        private void CloseMessage(string message)
        {
            if (this.outSF != null)
            {
                this.outSF.Close();
            }

            if (this.inSF1 != null)
            {
                this.inSF1.Close();
            }

            if (this.inSF2 != null)
            {
                this.inSF2.Close();
            }

            Logger.Message(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, DialogResult.OK);
        }

        /// <summary>
        /// Use a file dialog to obtain the filenames 
        /// </summary>
        private void GetFilenames()
        {
            var dialog = new GeoProcDialog();

            // File Element 1
            var file1 = dialog.Add_FileElement(GeoProcDialog.ElementTypes.OpenShapefile);
            file1.Caption = "First input shapefile";
            file1.HelpButtonVisible = false;

            // File Element 2
            var file2 = dialog.Add_FileElement(GeoProcDialog.ElementTypes.OpenShapefile);
            file2.Caption = "Second input shapefile";
            file2.HelpButtonVisible = false;

            // Out File Element
            using (var addFileElement = dialog.Add_FileElement(GeoProcDialog.ElementTypes.SaveShapefile))
            {
                addFileElement.Caption = "Output Shapefile";
                addFileElement.HelpButtonVisible = false;

                // Show a dialog to get the filenames to work with
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                this.inFile1 = file1.Filename;
                this.inFile2 = file2.Filename;
                this.outFile = addFileElement.Filename;
            }

            Logger.Dbg("input file 1: " + this.inFile1);
            Logger.Dbg("input file 2: " + this.inFile2);
            Logger.Dbg("output file: " + this.outFile);
        }

        /// <summary>
        /// Try to open the shapefiles
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        /// <returns>True on success</returns>
        private bool OpenFiles(out string errorMessage)
        {
            errorMessage = "No Error";

            // Try to open the filenames and test that they are the same type of shapefile
            if (!this.inSF1.Open(this.inFile1, null))
            {
                errorMessage = this.inSF1.get_ErrorMsg(this.inSF1.LastErrorCode);
                return false;
            }

            if (!this.inSF2.Open(this.inFile2, null))
            {
                this.inSF1.Close();
                errorMessage = this.inSF2.get_ErrorMsg(this.inSF1.LastErrorCode);
                return false;
            }

            if (this.inSF1.ShapefileType != this.inSF2.ShapefileType)
            {
                this.inSF1.Close();
                this.inSF2.Close();
                errorMessage = "Shape types were incompatible: " + this.inSF1.ShapefileType + " != " +
                               this.inSF2.ShapefileType;
                return false;
            }

            // Create the output shapefile and prepare it for editing
            if (!this.outSF.CreateNew(this.outFile, this.inSF1.ShapefileType))
            {
                this.inSF1.Close();
                this.inSF2.Close();
                errorMessage = this.outSF.get_ErrorMsg(this.outSF.LastErrorCode);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Build a list of unique fields for the output file 
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        /// <returns>True on success</returns>
        private bool CombineFields(out string errorMessage)
        {
            errorMessage = "No Error";
            var fields = new List<Field>();
            var names = new List<string>();
            for (var fld = 0; fld < this.inSF1.NumFields; fld++)
            {
                var field = this.inSF1.get_Field(fld);
                fields.Add(field);
                names.Add(field.Name);
            }

            for (var fld = 0; fld < this.inSF2.NumFields; fld++)
            {
                var field = this.inSF2.get_Field(fld);
                var name = field.Name;
                if (!names.Contains(name))
                {
                    fields.Add(field);
                    names.Add(name);
                }
                else
                {
                    if (fields[names.IndexOf(name)].Type != field.Type)
                    {
                        errorMessage = "Fields with a common name [" + name + "] did not share the same type.";
                        return false;
                    }
                }
            }

            var fi = 0;
            foreach (var field in fields)
            {
                if (!this.outSF.EditInsertField(field, ref fi, null))
                {
                    errorMessage = this.outSF.get_ErrorMsg(this.outSF.LastErrorCode);
                    return false;
                }

                fi++;
            }

            return true;
        }

        /// <summary>
        /// Add the shapes one by one 
        /// </summary>
        /// <param name="inSF">The shapefile to add the shapes to</param>
        /// <param name="errorMessage">The error message</param>
        /// <returns>True on success</returns>
        private bool AddShapes(IShapefile inSF, out string errorMessage)
        {
            errorMessage = "No Error.";
            var outShp = this.outSF.NumShapes;

            // Add all the shapes and attributes from the first shapefile
            for (var shp = 0; shp < inSF.NumShapes; shp++)
            {
                var shape = inSF.get_Shape(shp);
                this.outSF.EditInsertShape(shape, ref outShp);
                for (var fld = 0; fld < inSF.NumFields; fld++)
                {
                    var name = inSF.get_Field(fld).Name;
                    var index = this.IndexOf(name);
                    if (index == -1)
                    {
                        errorMessage = "Could not find a field in the output shapefile: [" + name + "]";
                        return false;
                    }

                    if (!this.outSF.EditCellValue(index, outShp, inSF.get_CellValue(fld, shp)))
                    {
                        errorMessage = this.outSF.get_ErrorMsg(this.outSF.LastErrorCode);
                        return false;
                    }
                }

                outShp++;
            }

            return true;
        }

        /// <summary>
        /// Use the string name to test each field in the output shapefile to determine the correct index 
        /// </summary>
        /// <param name="fieldName">The field to test</param>
        /// <returns>The index of the field</returns>
        private int IndexOf(string fieldName)
        {
            for (var fld = 0; fld < this.outSF.NumFields; fld++)
            {
                if (this.outSF.get_Field(fld).Name == fieldName)
                {
                    return fld;
                }
            }

            return -1;
        }

        #endregion
    }
}