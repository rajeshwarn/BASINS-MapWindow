using System;
using System.Collections;
using System.IO;

using MapWinGeoProc.NTS.Topology.Geometries;
using MapWindow.Interfaces.Geometries;

namespace MapWinGeoProc.NTS.Topology.IO
{
    /// <summary>
    /// Writes a Well-Known Binary byte data representation of a <c>Geometry</c>.
    /// </summary>
    public class WKBWriter
    {
        private ByteOrder encodingType;

        /// <summary>
        /// Standard byte size for each complex point.
        /// Each complex point (LineString, Polygon, ...) contains:
        ///     1 byte for ByteOrder and
        ///     4 bytes for WKBType.      
        /// </summary>
        protected const int InitCount = 5;        

        /// <summary>
        /// Initializes writer with LittleIndian byte order.
        /// </summary>
        public WKBWriter() : this(ByteOrder.LittleIndian) { }

        /// <summary>
        /// Initializes writer with the specified byte order.
        /// </summary>
        /// <param name="encodingType">Encoding type</param>
        public WKBWriter(ByteOrder encodingType)
        {
            this.encodingType = encodingType;
        }

        /// <summary>
        /// Writes a WKB representation of a given point.
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public virtual byte[] Write(IGeometry geometry)
        {
            byte[] bytes = GetBytes(geometry);
            Write(geometry, new MemoryStream(bytes));
            return bytes;
        }

        /// <summary>
        /// Writes a WKB representation of a given point.
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public virtual void Write(IGeometry geometry, Stream stream)
        {
            BinaryWriter writer = null;
            try
            {
                if (encodingType == ByteOrder.LittleIndian)
                     writer = new BinaryWriter(stream);
                else writer = new BEBinaryWriter(stream);
                Write(geometry, writer);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IGeometry geometry, BinaryWriter writer)
        {
            if (geometry is IPoint)
                Write(geometry as IPoint, writer);
            else if (geometry is ILineString)
                Write(geometry as ILineString, writer);
            else if (geometry is IPolygon)
                Write(geometry as IPolygon, writer);
            else if (geometry is IMultiPoint)
                Write(geometry as IMultiPoint, writer);
            else if (geometry is IMultiLineString)
                Write(geometry as IMultiLineString, writer);
            else if (geometry is IMultiPolygon)
                Write(geometry as IMultiPolygon, writer);
            else if (geometry is IGeometryCollection)
                Write(geometry as IGeometryCollection, writer);
            else throw new ArgumentException("Geometry not recognized: " + geometry.ToString());
        }

        /// <summary>
        /// Writes LittleIndian ByteOrder.
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void WriteByteOrder(BinaryWriter writer)
        {
            writer.Write((byte)ByteOrder.LittleIndian);
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IPoint point, BinaryWriter writer)
        {
            WriteByteOrder(writer);     // LittleIndian
            writer.Write((int)WKBGeometryTypes.WKBPoint);
            Write(point, writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineString"></param>
        /// <param name="writer"></param>
        protected virtual void Write(LineString lineString, BinaryWriter writer)
        {
            WriteByteOrder(writer);     // LittleIndian
            writer.Write((int)WKBGeometryTypes.WKBLineString);
            writer.Write((int)lineString.NumPoints);
            for(int i = 0; i < lineString.Coordinates.Length; i++)
                Write(lineString.Coordinates[i], writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IPolygon polygon, BinaryWriter writer)
        {
            WriteByteOrder(writer);     // LittleIndian
            writer.Write((int)WKBGeometryTypes.WKBPolygon);
            writer.Write((int)polygon.NumInteriorRings + 1);
            Write(polygon.ExteriorRing as ILinearRing, writer);
            for (int i = 0; i < polygon.NumInteriorRings; i++)
                Write(polygon.InteriorRings[i] as ILinearRing, writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="multiPoint"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IMultiPoint multiPoint, BinaryWriter writer)
        {
            WriteByteOrder(writer);     // LittleIndian
            writer.Write((int)WKBGeometryTypes.WKBMultiPoint);
            writer.Write((int)multiPoint.NumGeometries);
            for (int i = 0; i < multiPoint.NumGeometries; i++)
                Write(multiPoint.Geometries[i] as Point, writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="multiLineString"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IMultiLineString multiLineString, BinaryWriter writer)
        {
            WriteByteOrder(writer);     // LittleIndian
            writer.Write((int)WKBGeometryTypes.WKBMultiLineString);
            writer.Write((int)multiLineString.NumGeometries);
            for (int i = 0; i < multiLineString.NumGeometries; i++)
                Write(multiLineString.Geometries[i] as LineString, writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="multiPolygon"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IMultiPolygon multiPolygon, BinaryWriter writer)
        {
            WriteByteOrder(writer);     // LittleIndian
            writer.Write((int)WKBGeometryTypes.WKBMultiPolygon);
            writer.Write((int)multiPolygon.NumGeometries);
            for (int i = 0; i < multiPolygon.NumGeometries; i++)
                Write(multiPolygon.Geometries[i] as Polygon, writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geomCollection"></param>
        /// <param name="writer"></param>
        protected virtual void Write(IGeometryCollection geomCollection, BinaryWriter writer)
        {
            WriteByteOrder(writer);     // LittleIndian
            writer.Write((int)WKBGeometryTypes.WKBGeometryCollection);
            writer.Write((int)geomCollection.NumGeometries);
            for (int i = 0; i < geomCollection.NumGeometries; i++)
                Write(geomCollection.Geometries[i], writer); ;                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="writer"></param>
        protected virtual void Write(ICoordinate coordinate, BinaryWriter writer)
        {
            writer.Write((double)coordinate.X);
            writer.Write((double)coordinate.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ring"></param>
        /// <param name="writer"></param>
        protected virtual void Write(ILinearRing ring, BinaryWriter writer)
        {
            writer.Write((int)ring.NumPoints);
            for(int i = 0; i < ring.Coordinates.Length; i++)
                Write(ring.Coordinates[i], writer);
        }

        /// <summary>
        /// Sets corrent length for Byte Stream.
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual byte[] GetBytes(IGeometry geometry)
        {
            if (geometry is IPoint)
                return new byte[SetByteStream(geometry as IPoint)];
            else if (geometry is ILineString)
                return new byte[SetByteStream(geometry as ILineString)];
            else if (geometry is IPolygon)
                return new byte[SetByteStream(geometry as IPolygon)];
            else if (geometry is IMultiPoint)
                return new byte[SetByteStream(geometry as IMultiPoint)];
            else if (geometry is IMultiLineString)
                return new byte[SetByteStream(geometry as IMultiLineString)];
            else if (geometry is IMultiPolygon)
                return new byte[SetByteStream(geometry as IMultiPolygon)];
            else if (geometry is IGeometryCollection)
                return new byte[SetByteStream(geometry as IGeometryCollection)];
            else throw new ArgumentException("ShouldNeverReachHere");
        }

        /// <summary>
        /// Sets corrent length for Byte Stream.
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IGeometry geometry)
        {
            if (geometry is Point)
                return SetByteStream(geometry as Point);
            else if (geometry is LineString)
                return SetByteStream(geometry as LineString);
            else if (geometry is Polygon)
                return SetByteStream(geometry as Polygon);
            else if (geometry is MultiPoint)
                return SetByteStream(geometry as MultiPoint);
            else if (geometry is MultiLineString)
                return SetByteStream(geometry as MultiLineString);
            else if (geometry is MultiPolygon)
                return SetByteStream(geometry as MultiPolygon);
            else if (geometry is GeometryCollection)
                return SetByteStream(geometry as GeometryCollection);
            else throw new ArgumentException("ShouldNeverReachHere");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IGeometryCollection geometry)
        {            
            int count = InitCount;
            count += 4;
            foreach (Geometry geom in geometry.Geometries)
                count += SetByteStream(geom);
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IMultiPolygon geometry)
        {            
            int count = InitCount;
            count += 4;
            foreach (Polygon geom in geometry.Geometries)
                count += SetByteStream(geom);
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IMultiLineString geometry)
        {                       
            int count = InitCount;
            count += 4;
            foreach (LineString geom in geometry.Geometries)
                count += SetByteStream(geom);
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IMultiPoint geometry)
        {                        
            int count = InitCount;
            count += 4;     // NumPoints
            foreach (Point geom in geometry.Geometries)
                count += SetByteStream(geom);            
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IPolygon geometry)
        {                
            int count = InitCount;
            count += 4 + 4;                                 // NumRings + NumPoints
            count += 4 * (geometry.NumInteriorRings + 1);   // Index parts
            count += geometry.NumPoints * 16;               // Points in exterior and interior rings
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(ILineString geometry)
        {
            int numPoints = geometry.NumPoints;
            int count = InitCount;
            count += 4;                             // NumPoints
            count += 16 * numPoints;            
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        protected virtual int SetByteStream(IPoint geometry)
        {
            return 21;
        }
    }
}
