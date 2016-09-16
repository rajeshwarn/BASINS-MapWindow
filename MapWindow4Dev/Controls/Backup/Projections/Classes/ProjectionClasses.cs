// ----------------------------------------------------------------------------
// MapWindow.Controls.Projections: store controls to work with EPSG projections
// database
// Author: Sergei Leschinski
// ----------------------------------------------------------------------------

namespace MapWindow.Controls.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;

    /// <summary>
    /// The type of geographic coordinate system
    /// </summary>
    public enum GeographicalCSType
    {
        /// <summary>
        /// Coordinate system used for a single country
        /// </summary>
        Local = 0,
        
        /// <summary>
        /// Coordinate system used inside certain region
        /// </summary>
        Regional = 1,
        
        /// <summary>
        /// Coordinate system used around the World
        /// </summary>
        Global = 2,
    }

    /// <summary>
    /// Structure to hold information about region
    /// </summary>
    public class Region
    {
        /// <summary>
        /// The name of the region
        /// </summary>
        public string Name;
        
        /// <summary>
        /// The code of the region
        /// </summary>
        public int Code;
        
        /// <summary>
        /// The code of the parent region
        /// </summary>
        public int ParentCode;
        
        /// <summary>
        /// The list of countries belonging to the region
        /// </summary>
        public List<Country> Countries = new List<Country>();

       
    }

    /// <summary>
    /// Structure to hold information about country
    /// </summary>
    public class Country: Territory 
    {
        /// <summary>
        /// A code of region, the country belong to
        /// </summary>
        public int RegionCode;
        
        /// <summary>
        /// List of Geographical coordinate systems for the country
        /// </summary>
        public List<GeographicCS> GeographicCS = new List<GeographicCS>();
        
        /// <summary>
        /// EPSG codes of projetcted coordinate systems for the region (references can be obtained through GCS list)
        /// </summary>
        public List<int> ProjectedCS = new List<int>();
    }

    public class CoordinateSystem: Territory 
    {
        /// <summary>
        /// A string describing the scope from EPSG database, typically names of countries
        /// </summary>
        public string Scope;

        /// <summary>
        /// Text description of the area coordinate system applies to
        /// </summary>
        public string AreaName;

        /// <summary>
        /// Remarks on coordinate system
        /// </summary>
        public string Remarks;

        /// <summary>
        /// Settings name as string representation
        /// </summary>
        public override string ToString()
        {
            return base.Name == "" ? "not defined" : base.Name;
        }

        /// <summary>
        /// Proj4 string for the coordinate system
        /// </summary>
        public string proj4;

        /// <summary>
        /// List of alternative proj4 formulations of the given projection
        /// </summary>
        public List<string> Dialects;

        /// <summary>
        /// Creates a new instance of the CoordinateSystem class
        /// </summary>
        public CoordinateSystem()
        {
            Dialects = new List<string>();
        }

        /// <summary>
        /// Gets the extents where coordinate system is applicable (decimal degrees)
        /// </summary>
        public MapWinGIS.Extents Extents
        {
            get
            {
                MapWinGIS.Extents extents = new MapWinGIS.Extents();
                extents.SetBounds(this.Left, this.Bottom, 0.0, this.Right, this.Top, 0.0);
                return extents;
            }
        }
    }

    /// <summary>
    /// Structure to hold information about GCS
    /// </summary>
    public class GeographicCS : CoordinateSystem
    {
        /// <summary>
        /// List of projections for geographical coordinate system
        /// </summary>
        public List<ProjectedCS> Projections = new List<ProjectedCS>();
        
        /// <summary>
        /// The type of geogrphical coordinate system
        /// </summary>
        public GeographicalCSType Type;
        
        /// <summary>
        /// The code area from EPSG database
        /// </summary>
        public int AreaCode;
        
        /// <summary>
        /// The code of region coordinate system belongs to (for regional systems only)
        /// </summary>
        public int RegionCode;

        private Hashtable m_dctProjections = null;
        
        /// <summary>
        /// Fast search for projection by it's code (hashtable)
        /// </summary>
        /// <param name="pcsCode"></param>
        public ProjectedCS ProjectionByCode(int pcsCode)
        {
            if (m_dctProjections == null)
            {
                m_dctProjections = new Hashtable();
                foreach (ProjectedCS pcs in Projections)
                {
                    m_dctProjections.Add(pcs.Code, pcs);
                }
            }

            if (m_dctProjections.ContainsKey(pcsCode))
                return (ProjectedCS)m_dctProjections[pcsCode];
            else
                return null;
        }

        /// <summary>
        /// Settings name as string representation
        /// </summary>
        //public override string ToString()
        //{
        //    return base.Name == "" ? "not defined" : base.Name;
        //}
    }

    /// <summary>
    /// Structure to hold information about PCS
    /// </summary>
    public class ProjectedCS : CoordinateSystem
    {
        /// <summary>
        /// EPSG code of source geographic coordinate system
        /// </summary>
        public int SourceCode;
        
        /// <summary>
        /// The type of projection (custom clasification for particular systems)
        /// </summary>
        public string ProjectionType;
        
        /// <summary>
        /// Units of measure
        /// </summary>
        public int Units;
        
        /// <summary>
        /// Marks local projection, that should be shown under country node only
        /// </summary>
        public bool Local;

        /// <summary>
        /// Settings name as string representation
        /// </summary>
        //public override string ToString()
        //{
        //    return base.Name == "" ? "not defined" : base.Name;
        //}
    }
    
    /// <summary>
    /// Base class fro coordinate systems and countries
    /// </summary>
    public class Territory
    {
        /// <summary>
        /// The code of territory (coordinate system , country or region)
        /// </summary>
        public int Code;
        
        /// <summary>
        /// The name of territory
        /// </summary>
        public string Name;
        
        /// <summary>
        /// The left bound
        /// </summary>
        public double Left;
        
        /// <summary>
        /// The right bound
        /// </summary>
        public double Right;
        
        /// <summary>
        /// The top bound
        /// </summary>
        public double Top;

        /// <summary>
        /// The bottom bound
        /// </summary>
        public double Bottom;
        internal bool IsActive;   // fall within bounds

        /// <summary>
        /// Settings name as string representation
        /// </summary>
        public override string ToString()
        {
            return this.Name == "" ? "not defined" : this.Name;
        }
    }
}
