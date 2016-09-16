﻿// ********************************************************************************************************
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
    #region
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using MapWinGIS;
    using MapWinUtility;
using System.Drawing.Text;
    #endregion

    /// <summary>
    /// A class to encapsulating label properties and drawing
    /// </summary>
    public class LabelStyle : MapWinGIS.LabelCategoryClass
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the LabelStyle class
        /// </summary>
        /// <param name="cat">Label category to take the options from</param>
        public LabelStyle(LabelCategory cat)
        {
            PropertyInfo[] props = cat.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                PropertyInfo propDest = this.GetType().GetProperty(prop.Name);
                propDest.SetValue(this, prop.GetValue(cat, null), null);
            }
        }
        #endregion

        #region Properties
        TextRenderingHint m_textRenderingHint = TextRenderingHint.SystemDefault;

        public TextRenderingHint TextRenderingHint
        {
            get { return m_textRenderingHint; }
            set { m_textRenderingHint = value; }
        }
        #endregion

        /// <summary>
        ///  Returns the size of the string drawn with current options
        /// </summary>
        /// <param name="g">Graphics object on which the drawing is performed</param>
        /// <param name="s">Text string to measure</param>
        /// <param name="maxFontSize">This is maximum font size that will be used, larger values will be reduced to the given value</param>
        /// <returns>The size needed to draw the string</returns>
        public Size MeasureString(Graphics g, string s, int maxFontSize)
        {
            int fontSize = this.FontSize;
            if (maxFontSize > 0 && maxFontSize < fontSize)
            {
                fontSize = maxFontSize;
            }

            // font options
            FontStyle style = FontStyle.Regular;
            if (this.FontUnderline)
            {
                style |= FontStyle.Underline;
            }
            
            if (this.FontBold)
            {
                style |= FontStyle.Bold;
            }
            
            if (this.FontItalic)
            {
                style |= FontStyle.Italic;
            }
            
            if (this.FontStrikeOut)
            {
                style |= FontStyle.Strikeout;
            }

            Font font = new Font(this.FontName, fontSize, style);
            StringFormat format = this.StringFormatByAlignment(this.InboxAlignment);

            SizeF sizef = g.MeasureString(s, font);
            Size size = new Size((int)sizef.Width, (int)sizef.Height);
            size.Width += 1;
            size.Height += 1;

            if (this.FrameVisible)
            {
                size.Width += this.FramePaddingX;
                size.Height += this.FramePaddingY;
            }

            return size;
        }

        /// <summary>
        /// Returns Color object initialized with the given OLE_COLOR and alpha value
        /// </summary>
        /// <param name="color">OLE COLOR as unsigned interger</param>
        /// <param name="alpha">alpha value</param>
        /// <returns>Color object</returns>
        public Color GetColor(uint color, int alpha)
        {
            if (alpha != 255)
            {
                return Color.FromArgb(alpha, Colors.IntegerToColor(color));
            }
            else
            {
                return Colors.IntegerToColor(color);
            }
        }

        #region Region drawing
        /// <summary>
        /// Draws label style in specified rectangle, alignment by center is used. Clipping is made if required.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pntOrigin"></param>
        /// <param name="s"></param>
        /// <param name="useAlignment"></param>
        /// <param name="maxFontSize"></param>
        public void Draw(Graphics g, Rectangle rect, string s, int maxFontSize)
        {
            this.DrawCore(g, new System.Drawing.Point(0, 0), rect, s, false, maxFontSize);
        }
        
        /// <summary>
        /// Drawing of label using the label category options
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        /// <param name="pntOrigin">The position to start drawing</param>
        /// <param name="s">A string to draw</param>
        /// <param name="useAlignment">Toggles usage of alignment options</param>
        /// <param name="maxFontSize">This is maximum font size that will be used, larger values will be reduced to the given value</param>
        public void Draw(Graphics g, System.Drawing.Point pntOrigin, string s, bool useAlignment, int maxFontSize)
        {
            System.Drawing.Rectangle rect = new Rectangle(0,0,0,0);
            this.DrawCore(g, pntOrigin, rect, s, useAlignment, maxFontSize);
        }

        /// <summary>
        /// Core drawing routine
        /// </summary>
        private void DrawCore(Graphics g, System.Drawing.Point pntOrigin, System.Drawing.Rectangle bounds, string s, bool useAlignment, int maxFontSize)
        {
            if (s == "")
                return;

            int fontSize = this.FontSize;
            if (maxFontSize > 0 && maxFontSize < fontSize)
            {
                fontSize = maxFontSize;
            }

            // font options
            FontStyle style = FontStyle.Regular;
            if (this.FontUnderline)
            {
                style |= FontStyle.Underline;
            }

            if (this.FontBold)
            {
                style |= FontStyle.Bold;
            }

            if (this.FontItalic)
            {
                style |= FontStyle.Italic;
            }

            if (this.FontStrikeOut)
            {
                style |= FontStyle.Strikeout;
            }

            Font font = null;
            try
            {
                font = new Font(this.FontName, fontSize, style);
            }
            catch
            {
                font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Regular);
                s = "not supported";
            }
            StringFormat format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            
            Rectangle rect = new Rectangle();
            bool ignoreBounds = bounds.Width == 0 && bounds.Height == 0;    // no bounds were passed, so there is no need for trimming

            // bounds should be calculated
            if (ignoreBounds)
            {
                format = this.StringFormatByAlignment(this.InboxAlignment);
                SizeF sizef = g.MeasureString(s, font);
                Size size = new Size((int)sizef.Width, (int)sizef.Height);
                rect = new Rectangle(pntOrigin, size);
                rect.Height += 1;   // to avoid clipping the letters in some cases
                rect.Width += 1;
            }
            else
            {
                SizeF sizef = g.MeasureString(s, font);
                Size size = new Size((int)sizef.Width, (int)sizef.Height);
                System.Drawing.Point pntCenter = new System.Drawing.Point();

                pntCenter.X = bounds.X + bounds.Width / 2;
                pntCenter.Y = bounds.Y + bounds.Height / 2;
                rect = new Rectangle(pntCenter.X - size.Width/2, pntCenter.Y - size.Height/2, size.Width, size.Height);
                rect.Height += 1;   // to avoid clipping the letters in some cases
                rect.Width += 1;
            }

            if (useAlignment)
            {
                this.AlignRectangle(ref rect, this.Alignment);

                // offset
                rect.X += (int)this.OffsetX;
                rect.Y += (int)this.OffsetY;
            }

            if (this.FrameVisible)
            {
                rect.Width += this.FramePaddingX;
                rect.Height += this.FramePaddingY;
                rect.X -= this.FramePaddingX / 2;
                rect.Y -= this.FramePaddingY / 2;
            }

            // clipping
            if (!ignoreBounds)
            {
                if (rect.Width > bounds.Width)
                    rect.Width = bounds.Width;

                if (rect.Height > bounds.Height)
                    rect.Height = bounds.Height;

                if (rect.X < bounds.X)
                    rect.X = bounds.X;

                if (rect.Y < bounds.Y)
                    rect.Y = bounds.Y;
            }

            // drawing a frame
            if ((this.FrameTransparency != 0) && this.FrameVisible)
            {
                Pen penFrame = new Pen(this.GetColor(this.FrameOutlineColor, this.FrameTransparency), this.FrameOutlineWidth);  // Colors.IntegerToColor(base.FrameOutlineColor));
                penFrame.DashStyle = (DashStyle)this.FrameOutlineStyle;
                if (this.FrameGradientMode != MapWinGIS.tkLinearGradientMode.gmNone)
                {
                    LinearGradientBrush lgb = new LinearGradientBrush(
                                                                      rect,
                                                                      this.GetColor(this.FrameBackColor, this.FrameTransparency),
                                                                      this.GetColor(this.FrameBackColor2, this.FrameTransparency),
                                                                      (LinearGradientMode)this.FrameGradientMode);
                    this.DrawLabelFrame(g, lgb, penFrame, rect);
                    lgb.Dispose();
                }
                else
                {
                    SolidBrush brush = new SolidBrush(this.GetColor(this.FrameBackColor, this.FrameTransparency));
                    this.DrawLabelFrame(g, brush, penFrame, rect);
                    brush.Dispose();
                }

                penFrame.Dispose();
            }


            // drawing the label
            if (this.FontTransparency != 0)
            {
                g.TextRenderingHint = m_textRenderingHint;

                Brush brush = null;
                if (this.FontGradientMode != MapWinGIS.tkLinearGradientMode.gmNone)
                {
                    brush = new LinearGradientBrush(
                                                    rect,
                                                    this.GetColor(this.FontColor, this.FontTransparency),
                                                    this.GetColor(this.FontColor2, this.FontTransparency),
                                                    (LinearGradientMode)this.FontGradientMode);
                }
                else
                {
                    brush = new SolidBrush(this.GetColor(this.FontColor, this.FontTransparency));
                }
                
                if (this.ShadowVisible || this.HaloVisible || this.FontOutlineVisible)
                {
                    GraphicsPath path = new GraphicsPath();
                    path.StartFigure();
                    path.AddString(s, font.FontFamily, (int)font.Style, (float)fontSize * 96f / 72f, rect, format);
                    path.CloseFigure();

                    // shadow
                    if (this.ShadowVisible)
                    {
                        SolidBrush brushShadow = new SolidBrush(this.GetColor(this.ShadowColor, this.FontTransparency));
                        Matrix mtx = new Matrix();
                        mtx.Translate(this.ShadowOffsetX, this.ShadowOffsetY);
                        path.Transform(mtx);
                        g.FillPath(brushShadow, path);
                        mtx.Translate(-2 * this.ShadowOffsetX, -2 * this.ShadowOffsetY);
                        path.Transform(mtx);
                        mtx.Dispose();
                    }

                    // halo
                    if (this.HaloVisible)
                    {
                        float width = (float)font.Size / 16.0f * (float)this.HaloSize;
                        Pen penHalo = new Pen(this.GetColor(this.HaloColor, this.FontTransparency), width);
                        penHalo.LineJoin = LineJoin.Round;
                        g.DrawPath(penHalo, path);
                        penHalo.Dispose();
                    }

                    // font outline
                    if (this.FontOutlineVisible)
                    {
                        Pen penOutline = new Pen(this.GetColor(this.FontOutlineColor, this.FontTransparency), this.FontOutlineWidth);
                        penOutline.LineJoin = LineJoin.Round;
                        g.DrawPath(penOutline, path);
                        penOutline.Dispose();
                    }

                    // the font itself
                    g.FillPath(brush, path);

                    path.Dispose();
                }
                else
                {
                    
                    g.DrawString(s, font, brush, rect, format); //new System.Drawing.Point(rect.Left, rect.Top));
                }
                brush.Dispose();

            }   // (fontTransparency != 0)
        }

        /// <summary>
        /// Returns string format for the text to draw based upon label alignment option
        /// </summary>
        /// <param name="alignment">MapWinGIS to convert</param>
        /// <returns>Net string format</returns>
        private StringFormat StringFormatByAlignment(MapWinGIS.tkLabelAlignment alignment)
        {
            StringFormat fmt = new StringFormat();
            switch (alignment)
            {
                case MapWinGIS.tkLabelAlignment.laCenter: 
                    fmt.Alignment = StringAlignment.Center; 
                    fmt.LineAlignment = StringAlignment.Center; 
                    break;
                case MapWinGIS.tkLabelAlignment.laCenterLeft: 
                    fmt.Alignment = StringAlignment.Near; 
                    fmt.LineAlignment = StringAlignment.Center; 
                    break;
                case MapWinGIS.tkLabelAlignment.laCenterRight: 
                    fmt.Alignment = StringAlignment.Far; 
                    fmt.LineAlignment = StringAlignment.Center; 
                    break;
                case MapWinGIS.tkLabelAlignment.laBottomCenter: 
                    fmt.Alignment = StringAlignment.Center; 
                    fmt.LineAlignment = StringAlignment.Far; 
                    break;
                case MapWinGIS.tkLabelAlignment.laBottomLeft: 
                    fmt.Alignment = StringAlignment.Near; 
                    fmt.LineAlignment = StringAlignment.Far; 
                    break;
                case MapWinGIS.tkLabelAlignment.laBottomRight: 
                    fmt.Alignment = StringAlignment.Far; 
                    fmt.LineAlignment = StringAlignment.Far; 
                    break;
                case MapWinGIS.tkLabelAlignment.laTopCenter: 
                    fmt.Alignment = StringAlignment.Center; 
                    fmt.LineAlignment = StringAlignment.Near; 
                    break;
                case MapWinGIS.tkLabelAlignment.laTopLeft: 
                    fmt.Alignment = StringAlignment.Near; 
                    fmt.LineAlignment = StringAlignment.Near; 
                    break;
                case MapWinGIS.tkLabelAlignment.laTopRight: 
                    fmt.Alignment = StringAlignment.Far; 
                    fmt.LineAlignment = StringAlignment.Near; 
                    break;
            }
            
            return fmt;
        }

        /// <summary>
        /// Aligning the label rectangle around the point of origin
        /// </summary>
        /// <param name="r">Rectangle to align</param>
        /// <param name="alignment">Alignment option to apply</param>
        private void AlignRectangle(ref Rectangle r, tkLabelAlignment alignment)
        {
            switch (alignment)
            {
                case tkLabelAlignment.laTopLeft:
                                r.X -= r.Width;
                                r.Y -= r.Height;
                                break;                
                case tkLabelAlignment.laTopCenter:
                                r.X -= r.Width / 2;
                                r.Y -= r.Height;
                                break;                        
                case tkLabelAlignment.laTopRight:
                                r.X += 0;
                                r.Y -= r.Height;
                                break;            
                case tkLabelAlignment.laCenterLeft:
                                r.X -= r.Width;
                                r.Y -= r.Height / 2;
                                break;
                case tkLabelAlignment.laCenter:
                                r.X -= r.Width / 2;
                                r.Y -= r.Height / 2;
                                break;
                case tkLabelAlignment.laCenterRight:
                                r.X += 0;
                                r.Y -= r.Height / 2;
                                break;            
                case tkLabelAlignment.laBottomLeft:
                                r.X -= r.Width;
                                r.Y += 0;
                                break;            
                case tkLabelAlignment.laBottomCenter:
                                r.X -= r.Width / 2;
                                r.Y += 0;
                                break;
                case tkLabelAlignment.laBottomRight:
                                // rect.MoveToXY(0, 0);
                                break;            
            }
            return;
        }

        /// <summary>
        /// Draws a frame for the label
        /// </summary>
        /// <param name="g">Graphics object to draw on</param>
        /// <param name="brush">Brush object to draw frame background</param>
        /// <param name="pen">Pen object to draw frame outline</param>
        /// <param name="rect">Rectangle to draw in</param>
        private void DrawLabelFrame(Graphics g, Brush brush, Pen pen, Rectangle rect)
        {
            switch (this.FrameType)
            {
                case tkLabelFrameType.lfRectangle:
                    {
                        g.FillRectangle(brush, rect);
                        g.DrawRectangle(pen, rect);
                        break;
                    }

                case tkLabelFrameType.lfRoundedRectangle:
                    {
                        int left = rect.X;
                        int right = rect.X + rect.Width;
                        int top = rect.Y;
                        int bottom = rect.Y + rect.Height;

                        GraphicsPath path = new GraphicsPath();
                        path.StartFigure();

                        path.AddLine(left + rect.Height, top, right - rect.Height, top);
                        path.AddArc(right - rect.Height, top, rect.Height, rect.Height, -90.0f, 180.0f);
                        path.AddLine(right - rect.Height, bottom, left + rect.Height, bottom);
                        path.AddArc(left, top, rect.Height, rect.Height, 90.0f, 180.0f);
                        path.CloseFigure();
                        g.FillPath(brush, path);
                        g.DrawPath(pen, path);
                        path.Dispose();
                        break;
                    }

                case tkLabelFrameType.lfPointedRectangle:
                    {
                        float left = rect.X;
                        float right = rect.X + rect.Width;
                        float top = rect.Y;
                        float bottom = rect.Y + rect.Height;

                        GraphicsPath path = new GraphicsPath();
                        path.StartFigure();
                        path.AddLine(left + (rect.Height / 4), top, right - (rect.Height / 4), top);

                        path.AddLine(right - (rect.Height / 4), top, right, (top + bottom) / 2);
                        path.AddLine(right, (top + bottom) / 2, right - (rect.Height / 4), bottom);

                        path.AddLine(right - (rect.Height / 4), bottom, left + (rect.Height / 4), bottom);

                        path.AddLine(left + (rect.Height / 4), bottom, left, (top + bottom) / 2);
                        path.AddLine(left, (top + bottom) / 2, left + (rect.Height / 4), top);

                        path.CloseFigure();
                        g.FillPath(brush, path);
                        g.DrawPath(pen, path);
                        path.Dispose();
                        break;
                    }
            }
        }
        #endregion
    }
}
