using System;
using System.Collections;
using System.Text;
using MapWindow.Interfaces.Geometries;
namespace MapWinGeoProc.NTS.Topology.Geometries
{
   
    /// <summary>
    /// 
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Converts the location value to a location symbol, for example, <c>EXTERIOR => 'e'</c>.
        /// </summary>
        /// <param name="locationValue"></param>
        /// <returns>Either 'e', 'b', 'i' or '-'.</returns>
        public static char ToLocationSymbol(Locations locationValue)
        {
            switch (locationValue)
            {
                case Locations.Exterior:
                    return 'e';
                case Locations.Boundary:
                    return 'b';
                case Locations.Interior:
                    return 'i';
                case Locations.Null:
                    return '-';
            }
            throw new ArgumentException("Unknown location value: " + locationValue);
        }
    }   
}
