using System;
using System.Diagnostics;
using System.Text;

using MapWinGeoProc.NTS.Topology.Geometries;

namespace MapWinGeoProc.NTS.Topology.Features
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Feature
    {

        #region Fields      

        private Geometry geometry = null;

        /// <summary>
        /// Geometry representation of the feature.
        /// </summary>
        public virtual Geometry Geometry
        {
            get { return geometry; }
            set { geometry = value; }
        }

        private IAttributesTable attributes = null;

        /// <summary>
        /// Attributes table of the feature.
        /// </summary>
        public virtual IAttributesTable Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="attributes"></param>
        public Feature(Geometry geometry, IAttributesTable attributes) : this()
        {
            this.geometry = geometry;
            this.attributes = attributes;
        }

        /// <summary>
        /// 
        /// </summary>
        public Feature() { }

        #endregion

    }
}
