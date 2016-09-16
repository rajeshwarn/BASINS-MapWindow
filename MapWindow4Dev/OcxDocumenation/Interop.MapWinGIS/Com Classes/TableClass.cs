
namespace MapWinGIS
{
    using System;

    class ITable
    {
        #region ITable Members

        public bool Calculate(string Expression, int RowIndex, out object Result, out string ErrorString)
        {
            throw new NotImplementedException();
        }

        public string CdlgFilter
        {
            get { throw new NotImplementedException(); }
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public bool CreateNew(string dbfFilename)
        {
            throw new NotImplementedException();
        }

        public bool EditCellValue(int FieldIndex, int RowIndex, object newVal)
        {
            throw new NotImplementedException();
        }

        public bool EditClear()
        {
            throw new NotImplementedException();
        }

        public bool EditDeleteField(int FieldIndex, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool EditDeleteRow(int RowIndex)
        {
            throw new NotImplementedException();
        }

        public bool EditInsertField(Field Field, ref int FieldIndex, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool EditInsertRow(ref int RowIndex)
        {
            throw new NotImplementedException();
        }

        public bool EditReplaceField(int FieldIndex, Field NewField, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool EditingTable
        {
            get { throw new NotImplementedException(); }
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

        public int NumFields
        {
            get { throw new NotImplementedException(); }
        }

        public int NumRows
        {
            get { throw new NotImplementedException(); }
        }

        public bool Open(string dbfFilename, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool ParseExpression(string Expression, ref string ErrorString)
        {
            throw new NotImplementedException();
        }

        public bool Query(string Expression, ref object Result, ref string ErrorString)
        {
            throw new NotImplementedException();
        }

        public bool Save(ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool SaveAs(string dbfFilename, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool StartEditingTable(ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool StopEditingTable(bool ApplyChanges, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool TestExpression(string Expression, tkValueType ReturnType, ref string ErrorString)
        {
            throw new NotImplementedException();
        }

        public object get_CellValue(int FieldIndex, int RowIndex)
        {
            throw new NotImplementedException();
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public Field get_Field(int FieldIndex)
        {
            throw new NotImplementedException();
        }

        public int get_FieldIndexByName(string Fieldname)
        {
            throw new NotImplementedException();
        }

        public object get_MaxValue(int FieldIndex)
        {
            throw new NotImplementedException();
        }

        public double get_MeanValue(int FieldIndex)
        {
            throw new NotImplementedException();
        }

        public object get_MinValue(int FieldIndex)
        {
            throw new NotImplementedException();
        }

        public double get_StandardDeviation(int FieldIndex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
