using System;
using System.Collections;
using System.Text;

using MapWinGeoProc.NTS.Topology.Geometries;
using MapWindow.Interfaces.Geometries;
namespace MapWinGeoProc.NTS.Topology.Utilities
{
    /// <summary>
    /// Computes various kinds of common geometric shapes.
    /// Allows various ways of specifying the location and extent of the shapes,
    /// as well as number of line segments used to form them.
    /// </summary>
    public class GeometricShapeFactory
    {
        private GeometryFactory geomFact;
        private Dimensions dim = new Dimensions();
        private int nPts = 100;

        /// <summary>
        /// Create a shape factory which will create shapes using the default GeometryFactory.
        /// </summary>
        public GeometricShapeFactory() : this(new GeometryFactory()) { }

        /// <summary>
        /// Create a shape factory which will create shapes using the given GeometryFactory.
        /// </summary>
        /// <param name="geomFact">The factory to use.</param>
        public GeometricShapeFactory(GeometryFactory geomFact)
        {
            this.geomFact = geomFact;
        }

        /// <summary>
        /// Gets/Sets the location of the shape by specifying the base coordinate
        /// (which in most cases is the
        /// lower left point of the envelope containing the shape).
        /// </summary>
        public virtual Coordinate Base  
        {
            get
            {
                return dim.Base;
            }
            set
            {
                dim.Base = value;
            }
        }

        /// <summary>
        /// Gets/Sets the location of the shape by specifying the centre of
        /// the shape's bounding box.
        /// </summary>
        public virtual Coordinate Centre
        {
            get
            {
                return dim.Centre;
            }
            set
            {
                dim.Centre = value;
            }
        }

        /// <summary>
        /// Gets/Sets the total number of points in the created Geometry.
        /// </summary>
        public virtual int NumPoints
        {
            get
            {
                return nPts;
            }
            set
            {
                nPts = value;
            }
        }

        /// <summary>
        /// Gets/Sets the size of the extent of the shape in both x and y directions.        
        /// </summary>                
        public virtual double Size
        {
            get
            {
                return dim.Size;
            }
            set
            {
                dim.Size = value;
            }
        }

        /// <summary>
        /// Gets/Sets the width of the shape.
        /// </summary>
        public virtual double Width
        {
            get
            {
                return dim.Width;
            }
            set
            {
                dim.Width = value;
            }
        }

        /// <summary>
        /// Gets/Sets the height of the shape.
        /// </summary>
        public virtual double Height
        {
            get
            {
                return dim.Height;
            }
            set
            {
                dim.Height = value;
            }
        }

        /// <summary>
        /// Creates a rectangular <c>Polygon</c>.
        /// </summary>
        /// <returns>A rectangular polygon.</returns>
        public virtual IPolygon CreateRectangle()
        {
            int i;
            int ipt = 0;
            int nSide = nPts / 4;
            if (nSide < 1) nSide = 1;
            double XsegLen = dim.Envelope.Width / nSide;
            double YsegLen = dim.Envelope.Height / nSide;

            Coordinate[] pts = new Coordinate[4 * nSide + 1];
            Envelope env = dim.Envelope;            

            for (i = 0; i < nSide; i++) 
            {
                double x = env.MinX + i * XsegLen;
                double y = env.MinY;
                pts[ipt++] = new Coordinate(x, y);
            }
            for (i = 0; i < nSide; i++) 
            {
                double x = env.MaxX;
                double y = env.MinY + i * YsegLen;
                pts[ipt++] = new Coordinate(x, y);
            }
            for (i = 0; i < nSide; i++) 
            {
                double x = env.MaxX - i * XsegLen;
                double y = env.MaxY;
                pts[ipt++] = new Coordinate(x, y);
            }
            for (i = 0; i < nSide; i++) 
            {
                double x = env.MinX;
                double y = env.MaxY - i * YsegLen;
                pts[ipt++] = new Coordinate(x, y);
            }
            pts[ipt++] = new Coordinate(pts[0]);

            ILinearRing ring = geomFact.CreateLinearRing(pts);
            IPolygon poly = geomFact.CreatePolygon(ring, null);
            return poly;
        }

        /// <summary>
        /// Creates a circular <c>Polygon</c>.
        /// </summary>
        /// <returns>A circular polygon.</returns>
        public virtual IPolygon CreateCircle()
        {

            Envelope env = dim.Envelope;
            double xRadius = env.Width / 2.0;
            double yRadius = env.Height / 2.0;

            double centreX = env.MinX + xRadius;
            double centreY = env.MinY + yRadius;

            Coordinate[] pts = new Coordinate[nPts + 1];
            int iPt = 0;
            for (int i = 0; i < nPts; i++) 
            {
                double ang = i * (2 * Math.PI / nPts);
                double x = xRadius * Math.Cos(ang) + centreX;
                double y = yRadius * Math.Sin(ang) + centreY;
                Coordinate pt = new Coordinate(x, y);
                pts[iPt++] = pt;
            }
            pts[iPt] = pts[0];

            ILinearRing ring = geomFact.CreateLinearRing(pts);
            IPolygon poly = geomFact.CreatePolygon(ring, null);
            return poly;
        }

        /// <summary>
        /// Creates a elliptical arc, as a LineString.
        /// </summary>
        /// <param name="startAng"></param>
        /// <param name="endAng"></param>
        /// <returns></returns>
        public virtual ILineString CreateArc(double startAng, double endAng)
        {
            Envelope env = dim.Envelope;
            double xRadius = env.Width / 2.0;
            double yRadius = env.Height / 2.0;

            double centreX = env.MinX + xRadius;
            double centreY = env.MinY + yRadius;

            double angSize = (endAng - startAng);
            if (angSize <= 0.0 || angSize > 2 * Math.PI)
                angSize = 2 * Math.PI;
            double angInc = angSize / nPts;

            Coordinate[] pts = new Coordinate[nPts];
            int iPt = 0;
            for (int i = 0; i < nPts; i++) 
            {
                double ang = startAng + i * angInc;
                double x = xRadius * Math.Cos(ang) + centreX;
                double y = yRadius * Math.Sin(ang) + centreY;
                Coordinate pt = new Coordinate(x, y);
                new PrecisionModel(geomFact.PrecisionModel).MakePrecise(pt);
                pts[iPt++] = pt;
            }
            ILineString line = geomFact.CreateLineString(pts);
            return line;
        }

        /// <summary>
        /// 
        /// </summary>
        private class Dimensions
        {
            private Coordinate basecoord;

            /// <summary>
            /// 
            /// </summary>
            public virtual Coordinate Base
            {
                get { return basecoord; }
                set { basecoord = value; }
            }

            private Coordinate centre;

            /// <summary>
            /// 
            /// </summary>
            public virtual Coordinate Centre
            {
                get { return centre; }
                set { centre = value; }
            }

            private double width;

            /// <summary>
            /// 
            /// </summary>
            public virtual double Width
            {
                get { return width; }
                set { width = value; }
            }

            private double height;

            /// <summary>
            /// 
            /// </summary>
            public virtual double Height
            {
                get { return height; }
                set { height = value; }
            }                                  

            /// <summary>
            /// 
            /// </summary>
            public virtual double Size
            {
                get
                {
                    return Math.Max(Width, Height);
                }
                set
                {
                    Height = value;
                    Width = value;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public virtual Envelope Envelope
            {
                get
                {
                    if (Base != null)
                        return new Envelope(Base.X, Base.X + Width, Base.Y, Base.Y + Height);                    
                    if (Centre != null)
                        return new Envelope(Centre.X - Width / 2, Centre.X + Width / 2,
                                            Centre.Y - Height / 2, Centre.Y + Height / 2);                    
                    return new Envelope(0, Width, 0, Height);
                }
            }
        }
    }
}
