
namespace MapWinGIS
{
    using System;

    class ILabels
    {
        #region ILabels Members

        public LabelCategory AddCategory(string Name)
        {
            throw new NotImplementedException();
        }

        public void AddLabel(string Text, double x, double y, double Rotation, int Category)
        {
            throw new NotImplementedException();
        }

        public void AddPart(int Index, string Text, double x, double y, double Rotation, int Category)
        {
            throw new NotImplementedException();
        }

        public tkLabelAlignment Alignment
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

        public void ApplyCategories()
        {
            throw new NotImplementedException();
        }

        public void ApplyColorScheme(tkColorSchemeType Type, ColorScheme ColorScheme)
        {
            throw new NotImplementedException();
        }

        public void ApplyColorScheme2(tkColorSchemeType Type, ColorScheme ColorScheme, tkLabelElements Element)
        {
            throw new NotImplementedException();
        }

        public void ApplyColorScheme3(tkColorSchemeType Type, ColorScheme ColorScheme, tkLabelElements Element, int CategoryStartIndex, int CategoryEndIndex)
        {
            throw new NotImplementedException();
        }

        public bool AutoOffset
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

        public bool AvoidCollisions
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

        public double BasicScale
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

        public int ClassificationField
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

        public void ClearCategories()
        {
            throw new NotImplementedException();
        }

        public int CollisionBuffer
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

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public void Deserialize(string newVal)
        {
            throw new NotImplementedException();
        }

        public bool DynamicVisibility
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

        public string Expression
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

        public bool FontBold
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

        public uint FontColor
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

        public uint FontColor2
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

        public tkLinearGradientMode FontGradientMode
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

        public bool FontItalic
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

        public uint FontOutlineColor
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

        public bool FontOutlineVisible
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

        public int FontOutlineWidth
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

        public int FontSize
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

        public bool FontStrikeOut
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

        public int FontTransparency
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

        public bool FontUnderline
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

        public uint FrameBackColor
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

        public uint FrameBackColor2
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

        public tkLinearGradientMode FrameGradientMode
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

        public uint FrameOutlineColor
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

        public tkDashStyle FrameOutlineStyle
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

        public int FrameOutlineWidth
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

        public int FramePaddingX
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

        public int FramePaddingY
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

        public int FrameTransparency
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

        public tkLabelFrameType FrameType
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

        public bool FrameVisible
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

        public int Generate(string Expression, tkLabelPositioning Method, bool LargestPartOnly)
        {
            throw new NotImplementedException();
        }

        public bool GenerateCategories(int FieldIndex, tkClassificationType ClassificationType, int numClasses)
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

        public uint HaloColor
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

        public int HaloSize
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

        public bool HaloVisible
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

        public tkLabelAlignment InboxAlignment
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

        public LabelCategory InsertCategory(int Index, string Name)
        {
            throw new NotImplementedException();
        }

        public bool InsertLabel(int Index, string Text, double x, double y, double Rotation, int Category)
        {
            throw new NotImplementedException();
        }

        public bool InsertPart(int Index, int Part, string Text, double x, double y, double Rotation, int Category)
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

        public tkLineLabelOrientation LineOrientation
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

        public bool LoadFromDbf(bool loadText, bool loadCategory)
        {
            throw new NotImplementedException();
        }

        public bool LoadFromDbf2(string xField, string yField, string angleField, string textField, string categoryField, bool loadText, bool loadCategory)
        {
            throw new NotImplementedException();
        }

        public bool LoadFromXML(string Filename)
        {
            throw new NotImplementedException();
        }

        public double MaxVisibleScale
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

        public int MinDrawingSize
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

        public double MinVisibleScale
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

        public bool MoveCategoryDown(int Index)
        {
            throw new NotImplementedException();
        }

        public bool MoveCategoryUp(int Index)
        {
            throw new NotImplementedException();
        }

        public int NumCategories
        {
            get { throw new NotImplementedException(); }
        }

        public double OffsetX
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

        public double OffsetY
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

        public LabelCategory Options
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

        public tkLabelPositioning Positioning
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

        public bool RemoveCategory(int Index)
        {
            throw new NotImplementedException();
        }

        public bool RemoveDuplicates
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

        public bool RemoveLabel(int Index)
        {
            throw new NotImplementedException();
        }

        public bool RemovePart(int Index, int Part)
        {
            throw new NotImplementedException();
        }

        public bool SaveToDbf(bool saveText, bool saveCategory)
        {
            throw new NotImplementedException();
        }

        public bool SaveToDbf2(string xField, string yField, string angleField, string textField, string categoryField, bool saveText, bool saveCategory)
        {
            throw new NotImplementedException();
        }

        public bool SaveToXML(string Filename)
        {
            throw new NotImplementedException();
        }

        public tkSavingMode SavingMode
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

        public bool ScaleLabels
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

        public bool Select(Extents BoundingBox, int Tolerance, SelectMode SelectMode, ref object LabelIndices, ref object PartIndices)
        {
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        public uint ShadowColor
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

        public int ShadowOffsetX
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

        public int ShadowOffsetY
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

        public bool ShadowVisible
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

        public bool Synchronized
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

        public bool UseGdiPlus
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

        public bool UseWidthLimits
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

        public tkVerticalPosition VerticalPosition
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

        public string VisibilityExpression
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

        public LabelCategory get_Category(int Index)
        {
            throw new NotImplementedException();
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public Label get_Label(int Index, int Part)
        {
            throw new NotImplementedException();
        }

        public int get_NumParts(int Index)
        {
            throw new NotImplementedException();
        }

        public void set_Category(int Index, LabelCategory pVal)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
