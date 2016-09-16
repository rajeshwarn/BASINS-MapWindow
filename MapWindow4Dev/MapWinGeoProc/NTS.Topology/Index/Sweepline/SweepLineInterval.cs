using System;
using System.Collections;
using System.Text;

namespace MapWinGeoProc.NTS.Topology.Index.Sweepline
{
    /// <summary>
    /// 
    /// </summary>
    public class SweepLineInterval
    {
        private double min, max;
        private object item;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public SweepLineInterval(double min, double max) : this(min, max, null) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="item"></param>
        public SweepLineInterval(double min, double max, object item)
        {
            this.min = min < max ? min : max;
            this.max = max > min ? max : min;
            this.item = item;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual double Min { get { return min; } }
        
        /// <summary>
        /// 
        /// </summary>
        public virtual double Max { get { return max; } }

        /// <summary>
        /// 
        /// </summary>
        public virtual object Item { get { return item; } }
    }
}
