using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Compatibility
{
    /// <summary>
    /// Not yet implemented
    /// </summary>
    public class RasterCatalog: MapWindow.Interfaces.Raster.IRasterCatalog
    {




        #region IRasterCatalog Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Raster"></param>
        /// <returns></returns>
        public int Add(MapWindow.Interfaces.Raster.IRaster Raster)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public object GetValue(double X, double Y)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Value"></param>
        public void PutValue(double X, double Y, object Value)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        public void Remove(int ID)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
