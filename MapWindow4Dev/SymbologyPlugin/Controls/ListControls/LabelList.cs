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

namespace mwSymbology.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Drawing;
    using System.Xml;
    using System.Windows.Forms;
    using System.Drawing.Text;

    /// <summary>
    /// A control to show the list of available line pattern styles
    /// </summary>
    [ToolboxItem(true)]
    public partial class LabelList : ListControl
    {
        #region Declaration
        // The list of icons
        List<LabelStyle> m_styles = new List<LabelStyle>();

        private Graphics m_graphics = null;
        #endregion

        #region Properties
        public Graphics Graphics
        {
            get { return m_graphics; }
            set { m_graphics = value; }
        }

        public TextRenderingHint TextRenderingHint
        {
            get { return m_graphics.TextRenderingHint; }
            set 
            { 
                m_graphics.TextRenderingHint = value;
                this.Invalidate();
            }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Creates a new instance of the LinePatternControl
        /// </summary>
        public LabelList()
        {
            m_graphics = Graphics.FromHwnd(base.Handle);

            this.CellWidth = 96;
            this.CellHeight = 32;
            this.ItemCount = 0;
            this.OnDrawItem += new OnDrawItemDelegate(Control_OnDrawItem);
            this.EnabledChanged += delegate(object sender, EventArgs e)
            {
                base.Redraw();
            };

            this.AddDefaultStyles();
        }

        /// <summary>
        /// Adds default patterns to the list
        /// </summary>
        private void AddDefaultStyles()
        {
            m_styles.Clear();

            LabelStyle style = new LabelStyle(new MapWinGIS.LabelCategory());
            m_styles.Add(style);

            this.ItemCount = m_styles.Count;
        }

        /// <summary>
        /// Adds pattern to the list
        /// </summary>
        internal void AddStyle(LabelStyle style)
        {
            if (style != null)
            {
                m_styles.Add(style);
                this.ItemCount = m_styles.Count;
            }
        }

        /// <summary>
        /// Removes given pattern from the list
        /// </summary>
        internal void RemoveStyle(int index)
        {
            if (index >= 0 && index < m_styles.Count)
            {
                m_styles.RemoveAt(index);
                this.ItemCount = m_styles.Count;
            }
        }

        /// <summary>
        /// Gets the selected pattern or null if there is no one
        /// </summary>
        internal LabelStyle SelectedStyle
        {
            get
            {
                if (this.ItemCount == 0 || this.SelectedIndex < 0 || this.SelectedIndex >= m_styles.Count)
                {
                    return null;
                }
                else
                {
                    return m_styles[this.SelectedIndex];
                }
            }
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Saves list of styles to XML
        /// </summary>
        public bool SaveToXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<MapWindow version= '" + "'></MapWindow>");     // TODO: add version

            XmlElement xelRoot = xmlDoc.DocumentElement;

            XmlAttribute attr = xmlDoc.CreateAttribute("FileVersion");
            attr.InnerText = "0";
            xelRoot.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("FileType");
            attr.InnerText = "LabelStyles";
            xelRoot.Attributes.Append(attr);

            XmlElement xelSchemes = xmlDoc.CreateElement("LabelStyles");

            foreach (LabelStyle style in m_styles)
            {
                XmlElement xelStyle = xmlDoc.CreateElement("LabelStyle");
                xelStyle.InnerText = style.Serialize();
                xelSchemes.AppendChild(xelStyle);
            }
            xelRoot.AppendChild(xelSchemes);

            string filename = this.get_FileName();
            string path = Path.GetDirectoryName(filename);
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    SymbologyPlugin.MessageBoxError("Failed to create directory: " + path + Environment.NewLine + ex.Message);
                    return false;
                }
            }

            if (Directory.Exists(path))
            {
                try
                {
                    xmlDoc.Save(filename);
                }
                catch (Exception ex)
                {
                    SymbologyPlugin.MessageBoxError("Failed to save line patterns: " + path + Environment.NewLine + ex.Message);
                    return false;
                }
            }

            m_styles.Clear();
            this.ItemCount = m_styles.Count;

            return true;
        }

        /// <summary>
        /// Loads all the icons form the current path
        /// </summary>
        /// <param name="path"></param>
        public void LoadFromXML()
        {
            m_styles.Clear();

            XmlDocument xmlDoc = new XmlDocument();
            string filename = this.get_FileName();

            // reading from the file
            if (System.IO.File.Exists(filename))
            {
                xmlDoc.Load(filename);

                XmlElement xelSchemes = xmlDoc.DocumentElement["LabelStyles"];
                if (xelSchemes != null)
                {
                    foreach (XmlNode nodeStyle in xelSchemes.ChildNodes)
                    {
                        LabelStyle style = new LabelStyle(new MapWinGIS.LabelCategory());
                        style.Deserialize(nodeStyle.InnerText);
                        m_styles.Add(style);
                    }
                }
            }

            // load some default ones if none were loaded
            if (m_styles.Count == 0)
            {
                this.AddDefaultStyles();
            }

            this.ItemCount = m_styles.Count;

            if (this.ItemCount > 0)
                this.SelectedIndex = 0;
        }

        /// <summary>
        /// Retuns the name of file to serialize patterns in
        /// </summary>
        private string get_FileName()
        {
            string filename = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return Directory.GetParent(filename).FullName + "\\Styles\\labels.xml";
        }
        #endregion

        #region Drawing
        /// <summary>
        /// Draws the next icon from the list
        /// </summary>
        void Control_OnDrawItem(Graphics graphics, RectangleF rect, int itemIndex, bool selected)
        {
            m_graphics = graphics;
            
            LabelStyle style = m_styles[itemIndex];
            if (style != null)
            {
                Rectangle rectNew = new Rectangle((int)(rect.X + rect.Width * 0.1), (int)(rect.Y + rect.Height * 0.1), 
                                                  (int)(rect.Width * 0.8), (int)(rect.Height * 0.8));
                
                int fontTransp = style.FontTransparency;
                int frameTransp = style.FrameTransparency;
                if (!this.Enabled)
                {
                    style.FontTransparency = 50;
                    style.FrameTransparency = 50;
                }

                Rectangle r = new Rectangle((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
                style.TextRenderingHint = this.TextRenderingHint;
                graphics.FillRectangle(new SolidBrush(Color.White), rect);
                graphics.DrawRectangle(new Pen(base.GridColor), r);
                style.Draw(graphics, rectNew, "Style", 20);

                if (!this.Enabled)
                {
                    style.FontTransparency = fontTransp;
                    style.FrameTransparency = frameTransp;
                }
            }
        }
        #endregion
    }
}
