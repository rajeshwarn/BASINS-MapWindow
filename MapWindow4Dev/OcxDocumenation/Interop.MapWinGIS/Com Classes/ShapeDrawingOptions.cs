
namespace MapWinGIS
{
    using System;

    class IShapeDrawingOptions
    {
        #region IShapeDrawingOptions Members

        public ShapeDrawingOptions Clone()
        {
            throw new NotImplementedException();
        }

        public void Deserialize(string newVal)
        {
            throw new NotImplementedException();
        }

        public bool DrawLine(IntPtr hdc, float x, float y, int Width, int Height, bool drawVertices, int clipWidth, int clipHeight, uint BackColor)
        {
            throw new NotImplementedException();
        }

        public bool DrawLineVB(int hdc, float x, float y, int Width, int Height, bool drawVertices, int clipWidth, int clipHeight, uint BackColor)
        {
            throw new NotImplementedException();
        }

        public bool DrawPoint(IntPtr hdc, float x, float y, int clipWidth, int clipHeight, uint BackColor)
        {
            throw new NotImplementedException();
        }

        public bool DrawPointVB(int hdc, float x, float y, int clipWidth, int clipHeight, uint BackColor)
        {
            throw new NotImplementedException();
        }

        public bool DrawRectangle(IntPtr hdc, float x, float y, int Width, int Height, bool drawVertices, int clipWidth, int clipHeight, uint BackColor)
        {
            throw new NotImplementedException();
        }

        public bool DrawRectangleVB(int hdc, float x, float y, int Width, int Height, bool drawVertices, int clipWidth, int clipHeight, uint BackColor)
        {
            throw new NotImplementedException();
        }

        public bool DrawShape(IntPtr hdc, float x, float y, Shape Shape, bool drawVertices, int clipWidth, int clipHeight, uint BackColor)
        {
            throw new NotImplementedException();
        }

        public bool DrawShapeVB(int hdc, float x, float y, Shape Shape, bool drawVertices, int clipWidth, int clipHeight, uint BackColor)
        {
            throw new NotImplementedException();
        }

        public tkVectorDrawingMode DrawingMode
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public uint FillBgColor
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool FillBgTransparent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public uint FillColor
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public uint FillColor2
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public tkGradientBounds FillGradientBounds
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public tkGradientType FillGradientType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public tkGDIPlusHatchStyle FillHatchStyle
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double FillRotation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public float FillTransparency
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public tkFillType FillType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool FillVisible
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string FontName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int LastErrorCode
        {
            get { throw new NotImplementedException(); }
        }

        public uint LineColor
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public LinePattern LinePattern
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public tkDashStyle LineStipple
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public float LineTransparency
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool LineVisible
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public float LineWidth
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Image Picture
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double PictureScaleX
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double PictureScaleY
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public short PointCharacter
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double PointRotation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public tkPointShapeType PointShape
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int PointSidesCount
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public float PointSidesRatio
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public float PointSize
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public tkPointSymbolType PointType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        public void SetDefaultPointSymbol(tkDefaultPointSymbol symbol)
        {
            throw new NotImplementedException();
        }

        public void SetGradientFill(uint Color, short range)
        {
            throw new NotImplementedException();
        }

        public string Tag
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool UseLinePattern
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public uint VerticesColor
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool VerticesFillVisible
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int VerticesSize
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public tkVertexType VerticesType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool VerticesVisible
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Visible
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
