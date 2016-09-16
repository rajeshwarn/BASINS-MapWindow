// ********************************************************************************************************
// <copyright file="mwSymbology.cs" company="MapWindow.org">
// Copyright (c) MapWindow.org. All rights reserved.
// </copyright>
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http:// Www.mozilla.org/MPL/ 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
// 
// The Initial Developer of this version of the Original Code is Sergei Leschinski
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Change Log: 
// Date            Changed By      Notes
// ********************************************************************************************************

namespace mwSymbology
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;
    using System.Text;
    using MapWinGIS;
    using mwSymbology.Classes;
    using System.Drawing.Text;
    
    public class LabelUtilities
    {
        /// <summary>
        /// Retunrns label string formed by the first record of attribute table
        /// </summary>
        internal static string get_LabelText(MapWinGIS.Shapefile sf, string expression)
        {
            if (expression.ToLower() == "<no expression>")
            {
                if (sf.Labels.Count > 0)
                {
                    return sf.Labels.get_Label(0, 0).Text;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                object obj; string err;
                expression = LabelUtilities.FixExpression(expression);
                if (sf.Table.Calculate(expression, 0, out obj, out err))
                {
                    return obj.ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Draws preview based on the category options and expression stored in the Labels class
        /// </summary>
        internal static void DrawPreview(LabelCategory category, MapWinGIS.Shapefile sf, System.Windows.Forms.PictureBox canvas, bool forceDrawing, Color backColor)
        {
            string expression = (sf.Labels.Expression == "" && sf.Labels.Count != 0) ? "<no expression>" : sf.Labels.Expression;
            LabelUtilities.DrawPreview(category, sf, canvas, expression, forceDrawing, backColor);
        }

        /// <summary>
        /// Draws preview based on the specified expression string
        /// </summary>
        internal static void DrawPreview(LabelCategory category, MapWinGIS.Shapefile sf, System.Windows.Forms.PictureBox canvas, string expression, bool forceDrawing, Color backColor)
        {
            // retrieving text
            string s = get_LabelText(sf, expression);
            if (s.Trim() == string.Empty)
            {
                s = "";
            }

            Bitmap img = new Bitmap((int)canvas.ClientRectangle.Width, (int)canvas.ClientRectangle.Height);
            Graphics g = Graphics.FromImage(img);
            g.Clear(backColor);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextContrast = 0;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;

            System.Drawing.Point pntOrigin = new System.Drawing.Point((canvas.ClientRectangle.Right + canvas.ClientRectangle.Left) / 2,
                                                                      (canvas.ClientRectangle.Bottom + canvas.ClientRectangle.Top) / 2);

            // drawing the label
            LabelStyle style = new LabelStyle(category);
            if (style != null && (sf.Labels.Count > 0 || forceDrawing))
            {
                // drawing base point
                Pen pen = new Pen(Color.Black, 2);
                Rectangle rect = new Rectangle((int)pntOrigin.X, (int)pntOrigin.Y, 1, 1);
                g.DrawEllipse(pen, rect);
                pen.Dispose();

                style.TextRenderingHint = (TextRenderingHint)sf.Labels.TextRenderingHint;
                style.Draw(g, pntOrigin, s, true, 0);
            }

            if (canvas.Image != null)
            {
                canvas.Image.Dispose();
            }

            canvas.Image = img;
        }

        /// <summary>
        /// Generate label categories for the given set of shapefile categories
        /// </summary>
        /// <param name="mapWin">The reference to MapWindow</param>
        /// <param name="layerHandle">The handle of the layer</param>
        internal static void GenerateCategories(MapWindow.Interfaces.IMapWin mapWin, int layerHandle)
        {
            MapWindow.Interfaces.Layer lyr = mapWin.Layers[layerHandle];
            Shapefile sf = lyr.GetObject() as MapWinGIS.Shapefile;
            MapWinGIS.Labels lb = sf.Labels;

            sf.Labels.ClearCategories();
            for (int i = 0; i < sf.Categories.Count; i++)
            {
                MapWinGIS.ShapefileCategory cat = sf.Categories.get_Item(i);
                MapWinGIS.LabelCategory labelCat = lb.AddCategory(cat.Name);
                labelCat.Expression = cat.Expression;
            }

            SymbologySettings settings = SymbologyPlugin.get_LayerSettings(layerHandle, mapWin);
            ColorBlend blend = (ColorBlend)settings.LabelsScheme;

            if (blend != null)
            {
                MapWinGIS.ColorScheme scheme = ColorSchemes.ColorBlend2ColorScheme(blend);
                if (settings.LabelsRandomColors)
                {
                    lb.ApplyColorScheme(MapWinGIS.tkColorSchemeType.ctSchemeRandom, scheme);
                }
                else
                {
                    lb.ApplyColorScheme(MapWinGIS.tkColorSchemeType.ctSchemeGraduated, scheme);
                }
            }

            if (settings.LabelsVariableSize)
            {
                for (int i = 0; i < lb.NumCategories; i++)
                {
                    lb.get_Category(i).FontSize = (int)((double)sf.Labels.FontSize +
                    (double)settings.LabelsSizeRange / ((double)lb.NumCategories - 1) * (double)i);
                }
            }

            // Expressions aren't supported by labels yet, therefore we need to copy indices from the symbology
            for (int i = 0; i < lb.Count; i++)
            {
                MapWinGIS.Label label = lb.get_Label(i, 0);
                label.Category = sf.get_ShapeCategory(i);
            }
        }

        /// <summary>
        /// Returns the expression which complies with the ocx parser rules
        /// The new line characters should be placed in quotes
        /// </summary>
        internal static string FixExpression(string s)
        {
            string res = "";
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\"')
                {
                    count++;
                }

                // there is new line character outside any brackets
                if (s[i] == '\n' && count % 2 == 0)
                {
                    res += "\"\n\"+";
                }
                else
                {
                    res += s[i];
                }
            }
            return res;
        }

        /// <summary>
        /// Returns the string without quotes around new line
        /// </summary>
        internal static string StripNewLineQuotes(string s)
        {
            string res = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (i < s.Length - 3)
                {
                    if (s.Substring(i, 4) == "\"\n\"+")
                    {
                        res += "\n";
                        i = i + 3;
                    }
                    else
                        res += s[i];
                }
                else
                    res += s[i];
            }
            return res;
        }
    }
}
