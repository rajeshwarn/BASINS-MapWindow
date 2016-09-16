using System;
using System.Runtime.InteropServices;


using MapWindow.Interfaces.Geometries;


namespace MapWinGeoProc.NTS.Topology.Geometries
{
    /// <summary>
    /// Defines a rectangular region of the 2D coordinate plane.
    /// It is often used to represent the bounding box of a <c>Geometry</c>,
    /// e.g. the minimum and maximum x and y values of the <c>Coordinate</c>s.
    /// Note that Envelopes support infinite or half-infinite regions, by using the values of
    /// <c>Double.PositiveInfinity</c> and <c>Double.NegativeInfinity</c>.
    /// When Envelope objects are created or initialized,
    /// the supplies extent values are automatically sorted into the correct order.    
    /// </summary>
    [Serializable]
    public class Envelope : IEnvelope
    {
        /// <summary>
        /// Boolean, tests to see if all of the values are zero, and returns
        /// true if they are zero and false if any are not zero.
        /// </summary>
        /// <returns>Boolean</returns>
        public virtual bool AllZero()
        {
            //Null is a slightly stronger case of undefined
            if (IsNull) return true;
            if (minx == 0.0 && maxx == 0.0 && miny == 0.0 && maxy == 0.0) return true;
            return false;
        }


        /// <summary>
        /// Return HashCode.
        /// </summary>
        public override int GetHashCode()
        {
            int result = 17;            
            result = 37 * result + Coordinate.GetHashCode(minx);
            result = 37 * result + Coordinate.GetHashCode(maxx);
            result = 37 * result + Coordinate.GetHashCode(miny);
            result = 37 * result + Coordinate.GetHashCode(maxy);
            return result;
        }

        /// <summary>
        /// Test the point q to see whether it intersects the Envelope
        /// defined by p1-p2.
        /// </summary>
        /// <param name="p1">One extremal point of the envelope.</param>
        /// <param name="p2">Another extremal point of the envelope.</param>
        /// <param name="q">Point to test for intersection.</param>
        /// <returns><c>true</c> if q intersects the envelope p1-p2.</returns>
        public static bool Intersects(ICoordinate p1, ICoordinate p2, ICoordinate q)
        {
            if  (((q.X >= (p1.X < p2.X ? p1.X : p2.X))  && (q.X <= (p1.X > p2.X ? p1.X : p2.X))) &&
                 ((q.Y >= (p1.Y < p2.Y ? p1.Y : p2.Y))  && (q.Y <= (p1.Y > p2.Y ? p1.Y : p2.Y))))            
                return true;                        
            return false;
        }

        /// <summary>
        /// Test the envelope defined by p1-p2 for intersection
        /// with the envelope defined by q1-q2
        /// </summary>
        /// <param name="p1">One extremal point of the envelope Point.</param>
        /// <param name="p2">Another extremal point of the envelope Point.</param>
        /// <param name="q1">One extremal point of the envelope Q.</param>
        /// <param name="q2">Another extremal point of the envelope Q.</param>
        /// <returns><c>true</c> if Q intersects Point</returns>
        public static bool Intersects(ICoordinate p1, ICoordinate p2, ICoordinate q1, ICoordinate q2)
        {
            double minq = Math.Min(q1.X, q2.X);
            double maxq = Math.Max(q1.X, q2.X);
            double minp = Math.Min(p1.X, p2.X);
            double maxp = Math.Max(p1.X, p2.X);
            if(minp > maxq) return false;
            if(maxp < minq) return false;

            minq = Math.Min(q1.Y, q2.Y);
            maxq = Math.Max(q1.Y, q2.Y);
            minp = Math.Min(p1.Y, p2.Y);
            maxp = Math.Max(p1.Y, p2.Y);
            if( minp > maxq ) return false;
            if( maxp < minq ) return false;

            return true;
        }

        /*
        *  the minimum x-coordinate
        */
        private double minx;

        /*
        *  the maximum x-coordinate
        */
        private double maxx;

        /*
        * the minimum y-coordinate
        */
        private double miny;

        /*
        *  the maximum y-coordinate
        */
        private double maxy;

        /*
        *  the minimum z-coordinate
        */
        private double minz;

        /*
        *  the maximum z-coordinate
        */
        private double maxz;

        /// <summary>
        /// Creates a null <c>Envelope</c>.
        /// </summary>
        public Envelope()
        {
            Init();
        }

        /// <summary>
        /// Creates an <c>Envelope</c> for a region defined by maximum and minimum values.
        /// </summary>
        /// <param name="x1">The first x-value.</param>
        /// <param name="x2">The second x-value.</param>
        /// <param name="y1">The first y-value.</param>
        /// <param name="y2">The second y-value.</param>
        public Envelope(double x1, double x2, double y1, double y2)
        {
            Init(x1, x2, y1, y2);
        }

        /// <summary>
        /// Creates an <c>Envelope</c> for a region defined by an Vector.IEnvelope
        /// </summary>
        /// <param name="Ienvelope">The .IEnvelope to create an envelope from</param>
        public Envelope(IEnvelope Ienvelope)
        {
            Init(Ienvelope.MinX, Ienvelope.MaxX, Ienvelope.MinY, Ienvelope.MaxY);
        }

        /// <summary>
        /// Creates an <c>Envelope</c> for a region defined by two Coordinates.
        /// </summary>
        /// <param name="p1">The first Coordinate.</param>
        /// <param name="p2">The second Coordinate.</param>
        public Envelope(ICoordinate p1, ICoordinate p2)
        {
            Init(p1, p2);
        }

        /// <summary>
        /// Creates an <c>Envelope</c> for a region defined by a single Coordinate.
        /// </summary>
        /// <param name="p">The Coordinate.</param>
        public Envelope(ICoordinate p)
        {
            Init(p);
        }
      

        /// <summary>
        /// Initialize to a null <c>Envelope</c>.
        /// </summary>
        public virtual void Init()
        {
            SetToNull();
        }

        /// <summary>
        /// Initialize an <c>Envelope</c> for a region defined by maximum and minimum values.
        /// </summary>
        /// <param name="x1">The first x-value.</param>
        /// <param name="x2">The second x-value.</param>
        /// <param name="y1">The first y-value.</param>
        /// <param name="y2">The second y-value.</param>
        public virtual void Init(double x1, double x2, double y1, double y2)
        {
            if (x1 < x2)
            {
                minx = x1;
                maxx = x2;
            }
            else
            {
                minx = x2;
                maxx = x1;
            }

            if (y1 < y2)
            {
                miny = y1;
                maxy = y2;
            }
            else
            {
                miny = y2;
                maxy = y1;
            }
        }

        /// <summary>
        /// This will set all 3 dimensions.  Be warned, the Z dimensions are just place holders
        /// for any topology opperations and do not have any true functionality.  Whichever
        /// is smaller becomes the minimum and whichever is larger becomes the maximum.
        /// </summary>
        /// <param name="x1">An X coordinate </param>
        /// <param name="x2">Another X coordinate</param>
        /// <param name="y1">A Y coordinate</param>
        /// <param name="y2">Another Y coordinate</param>
        /// <param name="z1">A Z coordinate</param>
        /// <param name="z2">Another Z coordinate</param>
        public virtual void Init(double x1, double x2, double y1, double y2, double z1, double z2)
        {
            maxx = Math.Max(x1, x2);
            minx = Math.Min(x1, x2);
            maxy = Math.Max(y1, y2);
            miny = Math.Min(y1, y2);
            maxz = Math.Max(z1, z2);
            minz = Math.Min(z1, z2); 
        }

        /// <summary>
        /// Initialize an <c>Envelope</c> for a region defined by two Coordinates.
        /// </summary>
        /// <param name="p1">The first Coordinate.</param>
        /// <param name="p2">The second Coordinate.</param>
        public virtual void Init(ICoordinate p1, ICoordinate p2)
        {
            Init(p1.X, p2.X, p1.Y, p2.Y);
        }

        /// <summary>
        /// Initialize an <c>Envelope</c> for a region defined by a single Coordinate.
        /// </summary>
        /// <param name="p">The Coordinate.</param>
        public virtual void Init(ICoordinate p)
        {
            Init(p.X, p.X, p.Y, p.Y);
        }

        /// <summary>
        /// Initialize an <c>Envelope</c> from an existing Envelope.
        /// </summary>
        /// <param name="env">The Envelope to initialize from.</param>
        public virtual void Init(IEnvelope env)
        {
            this.minx = env.MinX;
            this.maxx = env.MaxX;
            this.miny = env.MinY;
            this.maxy = env.MaxY;
            this.minz = env.MinZ;
            this.maxz = env.MaxZ;
        }

        /// <summary>
        /// Makes this <c>Envelope</c> a "null" envelope..
        /// </summary>
        public virtual void SetToNull()
        {
            minx = 0;
            maxx = -1;
            miny = 0;
            maxy = -1;
        }

        /// <summary>
        /// Returns <c>true</c> if this <c>Envelope</c> is a "null" envelope.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this <c>Envelope</c> is uninitialized
        /// or is the envelope of the empty point.
        /// </returns>
        public virtual bool IsNull
        {
            get
            {
                return maxx < minx;
            }
        }

        /// <summary>
        /// Returns the difference between the maximum and minimum x values.
        /// </summary>
        /// <returns>max x - min x, or 0 if this is a null <c>Envelope</c>.</returns>
        public virtual double Width
        {
            get
            {
                if (IsNull)
                    return 0;                
                return maxx - minx;
            }
        }

        /// <summary>
        /// Returns the difference between the maximum and minimum y values.
        /// </summary>
        /// <returns>max y - min y, or 0 if this is a null <c>Envelope</c>.</returns>
        public virtual double Height
        {
            get
            {
                if (IsNull)
                    return 0;
                return maxy - miny;
            }
        }

        /// <summary>
        /// Gets or Sets the Minimum X value.  By setting the value,
        /// the current Minimum X is replaced, and the new value is
        /// compared with the current Maximum X.  If the new value
        /// is greater, then it becomes MaxX instead of MinX.
        /// </summary>
        /// <returns>The minimum x-coordinate.</returns>
        public virtual double MinX
        {
            get
            {
                return minx;
            }
            set
            {
                if (value > maxx)
                {
                    minx = maxx;
                    maxx = value;
                }
                else
                {
                    minx = value;
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Maximum X value.  By setting the value,
        /// the current Maximum X is replaced, and the new value is
        /// compared with the current Minimum X.  If the new value
        /// is less, then it becomes MinX instead of MaxX.
        /// </summary>
        /// <returns>The maximum x-coordinate.</returns>
        public virtual double MaxX
        {
            get
            {
                return maxx;
            }
            set
            {
                if (value < minx)
                {
                    maxx = minx;
                    minx = value;
                }
                else
                {
                    maxx = value;
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Minimum Y value.  By setting the value,
        /// the current Minimum Y is replaced, and the new value is
        /// compared with the current Maximum Y.  If the new value
        /// is greater, then it becomes MaxY instead of MinY.
        /// </summary>
        /// <returns>The minimum y-coordinate.</returns>
        public virtual double MinY
        {
            get
            {
                return miny;
            }
            set
            {
                if (value > maxy)
                {
                    miny = maxy;
                    maxy = value;
                }
                else
                {
                    miny = value;
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Maximum Y value.  By setting the value,
        /// the current Maximum Y is replaced, and the new value is
        /// compared with the current Minimum Y.  If the new value
        /// is less, then it becomes MinY instead of MaxY.
        /// </summary>
        /// <returns>The maximum y-coordinate.</returns>
        public virtual double MaxY
        {
            get
            {
                return maxy;
            }
            set
            {
                if (value < miny)
                {
                    maxy = miny;
                    miny = value;
                }
                else
                {
                    maxy = value;
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Minimum Z value.  By setting the value,
        /// the current Minimum Z is replaced, and the new value is
        /// compared with the current Maximum Z.  If the new value
        /// is greater, then it becomes MaxZ instead of MinZ.
        /// </summary>
        /// <returns>The maximum y-coordinate.</returns>
        public virtual double MinZ
        {
            get
            {
                return minz;
            }
            set
            {
                if (value > maxz)
                {
                    minz = maxz;
                    maxz = value;
                }
                else
                {
                    minz = value;
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Maximum Z value.  By setting the value,
        /// the current Maximum Z is replaced, and the new value is
        /// compared with the current Minimum Z.  If the new value
        /// is less, then it becomes MinZ instead of MaxZ.
        /// </summary>
        /// <returns>The maximum y-coordinate.</returns>
        public virtual double MaxZ
        {
            get
            {
                return maxz;
            }
            set
            {
                if (value < minz)
                {
                    maxz = minz;
                    minz = value;
                }
                else
                {
                    maxz = value;
                }
            }
        }



        /// <summary>
        /// Expands this envelope by a given distance in all directions.
        /// Both positive and negative distances are supported.
        /// </summary>
        /// <param name="distance">The distance to expand the envelope.</param>
        public void ExpandBy(double distance)
        {
            ExpandBy(distance, distance);
        }

        /// <summary>
        /// Expands this envelope by a given distance in all directions.
        /// Both positive and negative distances are supported.
        /// </summary>
        /// <param name="deltaX">The distance to expand the envelope along the the X axis.</param>
        /// <param name="deltaY">The distance to expand the envelope along the the Y axis.</param>
        public void ExpandBy(double deltaX, double deltaY)
        {
            if (IsNull) 
                return;

            minx -= deltaX;
            maxx += deltaX;
            miny -= deltaY;
            maxy += deltaY;

            // check for envelope disappearing
            if (minx > maxx || miny > maxy)
                SetToNull();
        }

        /// <summary>
        /// Enlarges the boundary of the <c>Envelope</c> so that it contains (p).
        /// Does nothing if (p) is already on or within the boundaries.
        /// </summary>
        /// <param name="p">The Coordinate.</param>
        public virtual void ExpandToInclude(ICoordinate p)
        {
            ExpandToInclude(p.X, p.Y);
        }

        /// <summary>
        /// Enlarges the boundary of the <c>Envelope</c> so that it contains
        /// (x,y). Does nothing if (x,y) is already on or within the boundaries.
        /// </summary>
        /// <param name="x">The value to lower the minimum x to or to raise the maximum x to.</param>
        /// <param name="y">The value to lower the minimum y to or to raise the maximum y to.</param>
        public virtual void ExpandToInclude(double x, double y)
        {
            if (IsNull)
            {
                minx = x;
                maxx = x;
                miny = y;
                maxy = y;
            }
            else
            {
                if (x < minx) minx = x;                
                if (x > maxx) maxx = x;
                if (y < miny) miny = y;
                if (y > maxy) maxy = y;
            }
        }

        /// <summary>
        /// Enlarges the boundary of the <c>Envelope</c> so that it contains
        /// <c>other</c>. Does nothing if <c>other</c> is wholly on or
        /// within the boundaries.
        /// </summary>
        /// <param name="other">the <c>Envelope</c> to merge with.</param>        
        public virtual void ExpandToInclude(IEnvelope other)
        {
            if (other.IsNull)
                return;            
            if (IsNull)
            {
                minx = other.MinX;
                maxx = other.MaxX;
                miny = other.MinY;
                maxy = other.MaxY;
            }
            else
            {
                if (other.MinX < minx)
                    minx = other.MinX;                
                if (other.MaxX > maxx)
                    maxx = other.MaxX;
                if (other.MinY < miny)
                    miny = other.MinY;
                if (other.MaxY > maxy)
                    maxy = other.MaxY;
            }
        }

        /// <summary>
        /// Translates this envelope by given amounts in the X and Y direction.
        /// </summary>
        /// <param name="transX">The amount to translate along the X axis.</param>
        /// <param name="transY">The amount to translate along the Y axis.</param>
        public virtual void Translate(double transX, double transY)
        {
            if (IsNull) 
                return;            
            Init(MinX + transX, MaxX + transX, MinY + transY, MaxY + transY);
        }

        /// <summary>
        /// Computes the coordinate of the centre of this envelope (as long as it is non-null).
        /// </summary>
        /// <returns>
        /// The centre coordinate of this envelope, 
        /// or <c>null</c> if the envelope is null.
        /// </returns>.
        public ICoordinate Centre
        {
            get
            {
                if (IsNull) return null;
                return new Coordinate((MinX + MaxX) / 2.0, (MinY + MaxY) / 2.0);
            }
        }

        /// <summary>
        /// Finds an envelope that represents the intersection between this
        /// envelope and the specified <c>IEnvelope</c>
        /// </summary>
        /// <param name="env">An <c>IEnvelope</c> to compare against</param>
        /// <returns>an <c>IEnvelope</c> that bounds the intersection area</returns>
        public IEnvelope Intersection(IEnvelope env)
        {
            if (IsNull || env.IsNull || !Intersects(env)) 
                return new Envelope();

            return new Envelope( Math.Max(MinX, env.MinX) ,
                                 Math.Min(MaxX, env.MaxX) ,
                                 Math.Max(MinY, env.MinY) ,
                                 Math.Min(MaxY, env.MaxY) );
        }        

        /// <summary> 
        /// Check if the region defined by <c>other</c>
        /// overlaps (intersects) the region of this <c>Envelope</c>.
        /// </summary>
        /// <param name="other"> the <c>Envelope</c> which this <c>Envelope</c> is
        /// being checked for overlapping.
        /// </param>
        /// <returns>        
        /// <c>true</c> if the <c>Envelope</c>s overlap.
        /// </returns>
        public virtual bool Intersects(IEnvelope other)
        {
            if (IsNull || other.IsNull)
                return false;            
            return !(other.MinX > maxx || other.MaxX < minx || other.MinY > maxy || other.MaxY < miny);
        }
       
        /// <summary>
        /// Use Intersects instead. In the future, Overlaps may be
        /// changed to be a true overlap check; that is, whether the intersection is
        /// two-dimensional.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [Obsolete("Use Intersects instead")]
        public virtual bool Overlaps(IEnvelope other)
        {
            return Intersects(other);
        }        

        /// <summary>
        /// Use Intersects instead.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [Obsolete("Use Intersects instead")]
        public virtual bool Overlaps(ICoordinate p)
        {
            return Intersects(p);
        }

        /// <summary>
        /// Use Intersects instead.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [Obsolete("Use Intersects instead")]
        public virtual bool Overlaps(double x, double y)
        {
            return Intersects(x, y);
        }

        /// <summary>  
        /// Check if the point <c>p</c> overlaps (lies inside) the region of this <c>Envelope</c>.
        /// </summary>
        /// <param name="p"> the <c>Coordinate</c> to be tested.</param>
        /// <returns><c>true</c> if the point overlaps this <c>Envelope</c>.</returns>
        public virtual bool Intersects(ICoordinate p)
        {
            return Intersects(p.X, p.Y);
        }

        /// <summary>  
        /// Check if the point <c>(x, y)</c> overlaps (lies inside) the region of this <c>Envelope</c>.
        /// </summary>
        /// <param name="x"> the x-ordinate of the point.</param>
        /// <param name="y"> the y-ordinate of the point.</param>
        /// <returns><c>true</c> if the point overlaps this <c>Envelope</c>.</returns>
        public virtual bool Intersects(double x, double y)
        {
            return !(x > maxx || x < minx || y > maxy || y < miny);
        }

        /// <summary>  
        /// Returns <c>true</c> if the given point lies in or on the envelope.
        /// </summary>
        /// <param name="p"> the point which this <c>Envelope</c> is
        /// being checked for containing.</param>
        /// <returns>    
        /// <c>true</c> if the point lies in the interior or
        /// on the boundary of this <c>Envelope</c>.
        /// </returns>                
        public virtual bool Contains(ICoordinate p)
        {
            return Contains(p.X, p.Y);
        }

        /// <summary>  
        /// Returns <c>true</c> if the given point lies in or on the envelope.
        /// </summary>
        /// <param name="x"> the x-coordinate of the point which this <c>Envelope</c> is
        /// being checked for containing.</param>
        /// <param name="y"> the y-coordinate of the point which this <c>Envelope</c> is
        /// being checked for containing.</param>
        /// <returns><c>true</c> if <c>(x, y)</c> lies in the interior or
        /// on the boundary of this <c>Envelope</c>.</returns>
        public virtual bool Contains(double x, double y)
        {
            return x >= minx && x <= maxx && y >= miny && y <= maxy;
        }

        /// <summary>  
        /// Returns <c>true</c> if the <c>Envelope other</c>
        /// lies wholely inside this <c>Envelope</c> (inclusive of the boundary).
        /// </summary>
        /// <param name="other"> the <c>Envelope</c> which this <c>Envelope</c> is being checked for containing.</param>
        /// <returns><c>true</c> if <c>other</c> is contained in this <c>Envelope</c>.</returns>
        public virtual bool Contains(IEnvelope other)
        {
            if (IsNull || other.IsNull)
                return false;            
            return  other.MinX >= minx && other.MaxX <= maxx && 
                other.MinY >= miny && other.MaxY <= maxy;
        }

        /// <summary> 
        /// Computes the distance between this and another
        /// <c>Envelope</c>.
        /// The distance between overlapping Envelopes is 0.  Otherwise, the
        /// distance is the Euclidean distance between the closest points.
        /// </summary>
        /// <returns>The distance between this and another <c>Envelope</c>.</returns>
        public virtual double Distance(IEnvelope env)
        {
            if (Intersects(env))
                return 0;

            double dx = 0.0;

            if (maxx < env.MinX)
                dx = env.MinX - maxx;
            if (minx > env.MaxX)
                dx = minx - env.MaxX;

            double dy = 0.0;

            if (maxy < env.MinY)
                dy = env.MinY - maxy;
            if (miny > env.MaxY)
                dy = miny - env.MaxY;

            // if either is zero, the envelopes overlap either vertically or horizontally
            if (dx == 0.0) return dy;
            if (dy == 0.0) return dx;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            if (!(other is Envelope)) 
                return false;            

            Envelope otherEnvelope = (Envelope)other;
            if (IsNull) 
                return otherEnvelope.IsNull;

            return  maxx == otherEnvelope.MaxX && maxy == otherEnvelope.MaxY &&
                    minx == otherEnvelope.MinX && miny == otherEnvelope.MinY;
        }
        
        /// <summary>
        /// See <see cref="Equals"/>
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator ==(Envelope obj1, Envelope obj2)
        {
            return Object.Equals(obj1, obj2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator !=(Envelope obj1, Envelope obj2)
        {
            return !(obj1 == obj2);
        }     

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Env[" + minx + " : " + maxx + ", " + miny + " : " + maxy + "]";
        }

      

        /* BEGIN ADDED BY MPAUL42: monoGIS team */
        
        /// <summary>
        /// Returns the area of the envelope.
        /// </summary>
        public virtual double Area
        {
            get
            {
                double area = 1;
                area = area * (maxx - minx);
                area = area * (maxy - miny);
                return area;
            }
        }
  
        /// <summary>
        /// Creates a copy of the current envelope.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return new Envelope(minx, maxx, miny, maxy);
        }


       

        /// <summary>
        /// Calculates the union of the current box and the given coordinate.
        /// </summary>
        public virtual IEnvelope Union(ICoordinate coord)
        {
            IEnvelope env = (IEnvelope)this.Clone();
            env.ExpandToInclude(coord);
            return env;
        }

       

        /// <summary>
        /// Calculates the union of the current box and the given box.
        /// </summary>
        public virtual IEnvelope Union(IEnvelope box)
        {
            if (box.IsNull)
                return this;
            if (this.IsNull)
                return new Envelope(box);

            return new Envelope( Math.Min(minx, box.MinX) ,
                                 Math.Max(maxx, box.MaxX) ,
                                 Math.Min(miny, box.MinY) ,
                                 Math.Max(maxy, box.MaxY) );
        }

        /// <summary>
        /// Moves the envelope to the indicated coordinate.
        /// </summary>
        /// <param name="centre">The new centre coordinate.</param>
        public virtual void SetCentre(ICoordinate centre)
        {
            SetCentre(centre, Width, Height);
        }

       
        /// <summary>
        /// Resizes the envelope to the indicated point.
        /// </summary>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        public virtual void SetCentre(double width, double height)
        {
            SetCentre(Centre, width, height);
        }

        

        /// <summary>
        /// Moves and resizes the current envelope.
        /// </summary>
        /// <param name="centre">The new centre coordinate.</param>
        /// <param name="width">The new width.</param>
        /// <param name="height">The new height.</param>
        public virtual void SetCentre(ICoordinate centre, double width, double height)
        {
            minx = centre.X - (width / 2);
            maxx = centre.X + (width / 2);
            miny = centre.Y - (height / 2);
            maxy = centre.Y + (height / 2);
        }

        /// <summary>
        /// Zoom the box. 
        /// Possible values are e.g. 50 (to zoom in a 50%) or -50 (to zoom out a 50%).
        /// </summary>
        /// <param name="perCent"> 
        /// Negative do Envelope smaller.
        /// Positive do Envelope bigger.
        /// </param>
        /// <example> 
        ///  perCent = -50 compact the envelope a 50% (make it smaller).
        ///  perCent = 200 enlarge envelope by 2.
        /// </example>
        public virtual void Zoom(double perCent)
        {
            double w = (this.Width * perCent / 100);
            double h = (this.Height * perCent / 100);
            SetCentre(w, h);
        }
        
        /* END ADDED BY MPAUL42: monoGIS team */

        /// <summary>
        /// Specifies that this is in fact an envelope
        /// </summary>
        
        public virtual GeometryTypes GeometryType
        {
            get
            {
                return GeometryTypes.Envelope;
            }
        }


        #region IEnvelope Members
  

        /// <summary>
        /// Despite the naming of the extents, this will force the larger of the two x values
        /// to become Xmax etc.
        /// </summary>
        /// <param name="minX">An X coordinate</param>
        /// <param name="minY">A Y coordinate</param>
        /// <param name="minZ">A Z coordinate</param>
        /// <param name="maxX">Another X coordinate</param>
        /// <param name="maxY">Another Y coordinate</param>
        /// <param name="maxZ">Another Z coordinate</param>
        public void SetExtents(double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
        {
            Init(minX, maxX, minY, maxY, minZ, maxZ);
        }

        /// <summary>
        /// The two dimensional overload for consistency with other code.
        /// Despite the names, this will force the smallest X coordinate given
        /// to become maxX.  
        /// </summary>
        /// <param name="minX">An X coordinate</param>
        /// <param name="minY">A Y coordinate</param>
        /// <param name="maxX">Another X coordinate</param>
        /// <param name="maxY">Another Y coordinate</param>
        public void SetExtents(double minX, double minY, double maxX, double maxY)
        {
            Init(minX, minY, maxX, maxY);
        }

        #endregion

        /// <summary>
        /// Technically an Evelope object is not actually a geometry.
        /// This creates a polygon from the extents.
        /// </summary>
        /// <returns>A Polygon, which technically qualifies as an IGeometry</returns>
        public virtual IPolygon ToPolygon()
        {
            Coordinate[] Coords = new Coordinate[5];
            Coords[0] = new Coordinate(minx, miny);
            Coords[1] = new Coordinate(maxx, miny);
            Coords[2] = new Coordinate(maxx, maxy);
            Coords[3] = new Coordinate(minx, maxy);
            Coords[4] = new Coordinate(minx, miny);
            LinearRing shell = new LinearRing(Coords);
            return new Polygon(shell);
            
        }
    }
}
