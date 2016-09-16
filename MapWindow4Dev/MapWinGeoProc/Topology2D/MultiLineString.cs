using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D 
{
    /// <summary>
    /// This is a collection of line-strings, similar to a multipart polyline.
    /// In reality, this just exposes a convenient list of linestrings with some
    /// simple methods.
    /// </summary>
    public class MultiLineString: GeometryCollection
    {
        #region Constructors

        /// <summary>
        /// Creates a new empty instance of a geometry collection specifically for linestrings
        /// </summary>
        public MultiLineString()
        {
            this.GeometryN = new List<Geometry>();
        }

        /// <summary>
        /// Creates a MultiLineString instance using a Late-bound MapWinGIS.Shape object
        /// </summary>
        /// <param name="MapWinGIS_Shape">A MapWinGIS.Shape to turn into a multi-line string</param>
        public MultiLineString(object MapWinGIS_Shape)
        {
            this.GeometryN = new List<Geometry>();
            Topology2D.MultiLineString MLS = new MultiLineString();
            MLS = GeometryFactory.CreateMultiLineString(MapWinGIS_Shape);
            this.GeometryN = MLS.GeometryN;
        }
        #endregion




        #region Methods


        
        #endregion

       


    }
}
