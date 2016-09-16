using System;
using System.Collections.Generic;
using System.Text;

namespace MapWindow
{
	namespace Interfaces
    {
        public class ShapefileFillStippleScheme
        {
            public long FieldHandle = -1;
            public long LayerHandle = -1;

            private System.Collections.Hashtable ValueHatchMap = new System.Collections.Hashtable();

            public void AddHatch(string Value, ShapefileFillStippleBreak Break)
            {
                if (ValueHatchMap.Contains(Value))
                    ValueHatchMap[Value] = Break;
                else
                    ValueHatchMap.Add(Value, Break);
            }

            public void RemoveHatch(string Value)
            {
                if (ValueHatchMap.Contains(Value)) ValueHatchMap.Remove(Value);
            }

            public void ClearHatches()
            {
                ValueHatchMap.Clear();
            }

            public ShapefileFillStippleBreak GetHatch(string Value)
            {
                if (ValueHatchMap.Contains(Value))
                    return (ShapefileFillStippleBreak)ValueHatchMap[Value];
                else
                    return null;
            }

            public System.Collections.IEnumerator GetHatchesEnumerator()
            {
                return ValueHatchMap.GetEnumerator();
            }

            public int NumHatches()
            {
                return ValueHatchMap.Count;
            }
        }

        public class ShapefileFillStippleBreak
        {
            public string Value;
            public bool Transparent;
            public System.Drawing.Color LineColor;

        }
    }
}
