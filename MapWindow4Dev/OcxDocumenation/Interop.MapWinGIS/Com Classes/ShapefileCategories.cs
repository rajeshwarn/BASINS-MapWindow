
namespace MapWinGIS
{
    using System;

    class IShapefileCategories
    {
        #region IShapefileCategories Members

        public ShapefileCategory Add(string Name)
        {
            throw new NotImplementedException();
        }

        public bool AddRange(int FieldIndex, tkClassificationType ClassificationType, int numClasses, object MinValue, object MaxValue)
        {
            throw new NotImplementedException();
        }

        public void ApplyColorScheme(tkColorSchemeType Type, ColorScheme ColorScheme)
        {
            throw new NotImplementedException();
        }

        public void ApplyColorScheme2(tkColorSchemeType Type, ColorScheme ColorScheme, tkShapeElements ShapeElement)
        {
            throw new NotImplementedException();
        }

        public void ApplyColorScheme3(tkColorSchemeType Type, ColorScheme ColorScheme, tkShapeElements ShapeElement, int CategoryStartIndex, int CategoryEndIndex)
        {
            throw new NotImplementedException();
        }

        public void ApplyExpression(int CategoryIndex)
        {
            throw new NotImplementedException();
        }

        public void ApplyExpressions()
        {
            throw new NotImplementedException();
        }

        public string Caption
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

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public void Deserialize(string newVal)
        {
            throw new NotImplementedException();
        }

        public bool Generate(int FieldIndex, tkClassificationType ClassificationType, int numClasses)
        {
            throw new NotImplementedException();
        }

        public ICallback GlobalCallback
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

        public ShapefileCategory Insert(int Index, string Name)
        {
            throw new NotImplementedException();
        }

        public string Key
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

        public bool MoveDown(int Index)
        {
            throw new NotImplementedException();
        }

        public bool MoveUp(int Index)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int Index)
        {
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        public Shapefile Shapefile
        {
            get { throw new NotImplementedException(); }
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public ShapefileCategory get_Item(int Index)
        {
            throw new NotImplementedException();
        }

        public void set_Item(int Index, ShapefileCategory pVal)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
