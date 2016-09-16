// An example of comments for enumerated constants

namespace MapWinGIS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    /// <summary>
    /// Sets drawing behavior when overlapping labels and charts are present on map.
    /// </summary>
    public enum tkCollisionMode
    {
        /// <summary>
        /// Overlaps of labels (charts) are allowed. The subsequent labels (charts) will be drawn atop of the prior ones , thus hiding them.
        /// </summary>
        AllowCollisions = 0,

        /// <summary>
        /// The overlaps of labels (charts) of the same layer are prevented.
        /// But overlaps of labels (charts) of different layers are allowed.
        /// </summary>
        LocalList = 1,

        /// <summary>
        /// No overlaps of labels (charts) are allowed for both inside the layer and around layers.
        /// </summary>
        GlobalList = 2,
    }
}
