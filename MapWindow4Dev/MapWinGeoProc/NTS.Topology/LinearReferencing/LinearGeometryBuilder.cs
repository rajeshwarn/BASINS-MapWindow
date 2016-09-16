using System;
using System.Collections.Generic;

using MapWinGeoProc.NTS.Topology.Geometries;
using MapWindow.Interfaces.Geometries;
namespace MapWinGeoProc.NTS.Topology.LinearReferencing
{
    /// <summary>
    /// Builds a linear geometry (<see cref="LineString" /> or <see cref="MultiLineString" />)
    /// incrementally (point-by-point).
    /// </summary>
    public class LinearGeometryBuilder
    {
        private IGeometryFactory geomFact = null;
        private List<IGeometry> lines = new List<IGeometry>();
        private CoordinateList coordList = null;

        private bool ignoreInvalidLines = false;
        private bool fixInvalidLines = false;

        private ICoordinate lastPt = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geomFact"></param>
        public LinearGeometryBuilder(IGeometryFactory geomFact)
        {
            this.geomFact = geomFact;
        }

        /// <summary>
        /// Allows invalid lines to be fixed rather than causing Exceptions.
        /// An invalid line is one which has only one unique point.
        /// </summary>
        public virtual bool FixInvalidLines
        {
            get
            {
                return fixInvalidLines;
            }
            set
            {
                fixInvalidLines = value;
            }
        }

        /// <summary>
        /// Allows invalid lines to be ignored rather than causing Exceptions.
        /// An invalid line is one which has only one unique point.
        /// </summary>
        public virtual bool IgnoreInvalidLines
        {
            get
            {
                return ignoreInvalidLines;
            }
            set
            {
                ignoreInvalidLines = value;
            }
        }

        /// <summary>
        /// Adds a point to the current line.
        /// </summary>
        /// <param name="pt">The <see cref="Coordinate" /> to add.</param>
        public virtual void Add(ICoordinate pt)
        {
            Add(pt, true);
        }

        /// <summary>
        /// Adds a point to the current line.
        /// </summary>
        /// <param name="pt">The <see cref="Coordinate" /> to add.</param>
        /// <param name="allowRepeatedPoints">If <c>true</c>, allows the insertions of repeated points.</param>
        public void Add(ICoordinate pt, bool allowRepeatedPoints)
        {
            if (coordList == null)
                coordList = new CoordinateList();
            coordList.Add(pt, allowRepeatedPoints);
            lastPt = pt;
        }

        /// <summary>
        /// 
        /// </summary>
        public ICoordinate LastCoordinate
        {
            get
            {
                return lastPt;
            }
        }

        /// <summary>
        /// Terminate the current <see cref="LineString" />.
        /// </summary>
        public void EndLine()
        {
            if (coordList == null)
                return;
            
            if (ignoreInvalidLines && coordList.Count < 2)
            {
                coordList = null;
                return;
            }

            Coordinate[] rawPts = coordList.ToCoordinateArray();
            Coordinate[] pts = rawPts;
            if (FixInvalidLines)
                pts = ValidCoordinateSequence(rawPts);

            coordList = null;
            ILineString line = null;
            try
            {
                line = geomFact.CreateLineString(pts);
            }
            catch (ArgumentException ex)
            {
                // exception is due to too few points in line.
                // only propagate if not ignoring short lines
                if (!IgnoreInvalidLines)
                    throw ex;
            }

            if (line != null) 
                lines.Add(line);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        private Coordinate[] ValidCoordinateSequence(Coordinate[] pts)
        {
            if (pts.Length >= 2) 
                return pts;
            Coordinate[] validPts = new Coordinate[] { pts[0], pts[0] };
            return validPts;
        }

        /// <summary>
        /// Builds and returns the <see cref="Geometry" />.
        /// </summary>
        /// <returns></returns>
        public IGeometry GetGeometry()
        {
            // end last line in case it was not done by user
            EndLine();
            return geomFact.BuildGeometry(lines);
        }

    }
}
