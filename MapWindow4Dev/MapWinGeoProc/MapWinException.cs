using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Threading;
using System.Diagnostics;

namespace MapWinGeoProc
{
    /// <summary>
    /// This class can be used to efficiently specify an appropriate exception
    /// based on the error code returned by any of the MapWinGIS classes
    /// </summary>
    [Serializable]
    public class MapWinException: ApplicationException
    {
        private int m_ErrorCode;

        #region Constructors

        /// <summary>
        /// This creates a new MapWinException given an error code from one of the MapWinGIS objects
        /// </summary>
        /// <param name="errorCode">int The error code returned from the LastErrorCode method</param>
        public MapWinException(int errorCode) : base (GetErrorMessage(errorCode))
        {
            // We are logging an error message here... but this won't tell us where the error was 
            // called from, only what the contents of the error were.  Propper use should
            // actually call Logger.Dbg from the offending location.
            MapWinUtility.Logger.Dbg(GetErrorMessage(errorCode));
        }

        

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the numerical equivalent for the Error Code based on the MapWinGIS
        /// </summary>
        int ErrorCode
        {
            get { return m_ErrorCode; }
        }


        #endregion

        #region ISerializable Implementation

        /// <summary>
        /// Creates a MapWinException from Serialized data
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected MapWinException(SerializationInfo info, 
            StreamingContext context) : base (info, context)
        {
            // get the custom property out of the serialization stream and 
            // set the object's property
            m_ErrorCode = info.GetInt32 ("ErrorCode");
        }

        /// <summary>
        /// Serializes this object
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData (SerializationInfo info, 
            StreamingContext context)
        {
            // add the custom property into the serialization stream
            info.AddValue ("ErrorCode", m_ErrorCode);
           
            // call the base exception class to ensure proper serialization
            base.GetObjectData (info, context);
        }

        

        
        #endregion

        /// <summary>
        /// Returns the error message that belongs to a particular error code
        /// </summary>
        /// <param name="ErrorCode">Integer, the numeric code for the error</param>
        /// <returns>String, a Message for the specified error</returns>
        public static string GetErrorMessage(int ErrorCode)
        {
            string Message = "Invalid Error Code";
            switch(ErrorCode)
            {
                // 1 - 200 -  Common Errors
                case 0: 
                    Message = "No Error.";
                    break;
                case 1: 
                    Message = "Index out of bounds.";
                    break;
                case 2:
                    Message = "Unexpected Null Parameter.";
                    break;
                case 3: 
                    Message = "Invalid File Extension.";
                    break;
                case 4: 
                    Message = "Invalid Filename.";
                    break;
                case 5: 
                    Message = "Unrecoverable Error.";
                    break;
                case 6: 
                    Message = "File Not Open.";
                    break;
                case 7: 
                    Message = "Zero Length String.";
                    break;
                case 8: 
                    Message = "Incorrect Variant Type.";
                    break;
                case 9: 
                    Message = "Invalid Parameter Value.";
                    break;
                case 10: 
                    Message = "Interface Not Supported.";
                    break;
                case 11: 
                    Message = "Unavailable In Disk Mode.";
                    break;
                case 12: 
                    Message = "Cant Open File";
                    break;
                case 13: 
                    Message = "Unsupported File Extension";
                    break;
                case 14: 
                    Message = "Cant Create File";
                    break;
                case 15:
                    Message = "Invalid File.";
                    break;
                case 16:
                    Message = "Invalid Variant Type.";
                    break;
                case 17:
                    Message = "Out of Range 0 to 1.";
                    break;
                case 18:
                    Message = "Can't Cocreate Com Instance.";
                    break;
                
                // 201 - 400 = Shapefile or Shape Errors
                
                case 201:
                    Message = "Unsupported Shapefile Type.";
                    break;
                case 202:
                    Message = "Incompatible Shapefile Type.";
                    break;
                case 203:
                    Message = "Can't Open Shapefile (*.shp).";
                    break;
                case 204:
                    Message = "Can't Open SHX file (*.shx).";
                    break;
                case 205:
                    Message = "Invalid Shapefile (*.shp).";
                    break;
                case 206:
                    Message = "Invalid SHX file (*.shx).";
                    break;
                case 207:
                    Message = "Shapefile In Edit Mode.";
                    break;
                case 208:
                    Message = "Shapefile Not In Edit Mode.";
                    break;
                case 209:
                    Message = "Can't Create Shapefile (*.shp).";
                    break;
                case 210:
                    Message = "Can't Create SHX file (*.shx).";
                    break;
                case 211:
                    Message = "Shapefile Exists (*.shp).";
                    break;
                case 212:
                    Message = "SHX File Exists (*.shx).";
                    break;
                case 213:
                    Message = "Incompatible Shape Type.";
                    break;
                
                // 401 - 600 - Grid Errors
                case 401:
                    Message = "Grid Not Initialized.";
                    break;
                case 402:
                    Message = "Invalid Data Type.";
                    break;
                case 403:
                    Message = "Invalid Grid File Type.";
                    break;
                case 404:
                    Message = "Zero Rows or Cols.";
                    break;
                case 405:
                    Message = "Incompatible Data Type.";
                    break;
                case 406:
                    Message = "ESRI dll Not Initialized.";
                    break;
                case 407:
                    Message = "ESRI Invalid Bounds.";
                    break;
                case 408:
                    Message = "ESRI Access Window Set.";
                    break;
                case 409:
                    Message = "Can't Allocate Memory.";
                    break;
                case 410:
                    Message = "ESRI Layer Open.";
                    break;
                case 411:
                    Message = "ESRI Layer Create.";
                    break;
                case 412:
                    Message = "ESRI Can't Delete File.";
                    break;
                case 413:
                    Message = "SDTS Bad File Header.";
                    break;

                // 601 - 800 Image Errors 
                case 601:
                    Message = "Can't Write World File.";
                    break;
                case 602:
                    Message = "Invalid Width or Height.";
                    break;
                case 603:
                    Message = "Invalid Dy.";
                    break;
                case 604:
                    Message = "Invalid Dx.";
                    break;
                
                // 1001 - 1200 = Utils
                case 1001:
                    Message = "Out of Range 0 to 180.";
                    break;
                case 1002:
                    Message = "Out of Range -360 To 360.";
                    break;
                case 1003:
                    Message = "Shapefile Larger than Grid.";
                    break;
                case 1004:
                    Message = "Concave Polygons.";
                    break;
                case 1005:
                    Message = "Incompatible Dx.";
                    break;
                case 1006:
                    Message = "Incompatible Dy.";
                    break;
                case 1007:
                    Message = "Invalid Final Point Index.";
                    break;
                case 1008:
                    Message = "Tolerance Too Large.";
                    break;
                case 1009:
                    Message = "Not Aligned.";
                    break;
                case 1010:
                    Message = "Invalid Node.";
                    break;
                case 1011:
                    Message = "Node At Outlet.";
                    break;
                case 1012:
                    Message = "No Network.";
                    break;
                case 1013:
                    Message = "Can't Change Outlet Parent.";
                    break;
                case 1014:
                    Message = "Net Loop.";
                    break;
                case 1015:
                    Message = "Missing Field.";
                    break;
                case 1016:
                    Message = "Invalid Field.";
                    break;
                case 1017:
                    Message = "Invalid Field Value.";
                    break;

                // 1201 - 1400 Map Errors

                case 1201:
                    Message = "Invalid Layer Handle.";
                    break;
                case 1202:
                    Message = "Invalid Draw Handle.";
                    break;
                case 1203:
                    Message = "Window locked.";
                    break;
                case 1204:
                    Message = "Invalid Layer Position.";
                    break;
                case 1205:
                    Message = "Init Invalid DC.";
                    break;
                case 1206:
                    Message = "Init Can't Setup Pixel Format.";
                    break;
                case 1207:
                    Message = "Init Can't Create Context.";
                    break;
                case 1208:
                    Message = "Init Can't Make Current.";
                    break;
                case 1209:
                    Message = "Unexpected Layer Type.";
                    break;
                case 1210:
                    Message = "Map Not Initialized.";
                    break;
                case 1211:
                    Message = "Invalid MapState.";
                    break;
                case 1212:
                    Message = "MapState Layer Load Failed.";
                    break;

                    // 1401 - 1600 Invuc 
                case 1401:
                     Message = "Value Must be 2 to N";
                     break;
                case 1402:
                    Message = "Not Initialized";
                    break;

            }
            return Message;
        }
    }
}
