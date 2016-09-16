using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

using MapWinGeoProc.NTS.Topology.Geometries;
using MapWindow.Interfaces.Geometries;

namespace MapWinGeoProc.NTS.Topology.IO.GML2
{
    /// <summary>
    /// Reads a GML document and creates a representation of the features based or Topology model.
    /// Uses GML 2.1.1 <c>Geometry.xsd</c> as schema for features.
    /// </summary>
    public class GMLReader
    {
        private GeometryFactory factory = null;
        private XmlReaderSettings settings = new XmlReaderSettings();

        /// <summary>
        /// <c>Geometry</c> builder.
        /// </summary>
        protected virtual GeometryFactory Factory
        {
            get { return factory; }            
        }

        /// <summary>
        /// Initialize reader with a standard <see cref="GeometryFactory" />. 
        /// </summary>
        /// <seealso cref="GeometryFactory.Default" />
        public GMLReader() : this(new GeometryFactory()) { }

        /// <summary>
        /// Initialize reader with the given <see cref="GeometryFactory" />.
        /// </summary>
        /// <param name="factory"></param>
        public GMLReader(GeometryFactory factory)
        {
            this.factory = factory;
            settings.IgnoreWhitespace = true;
        }

        /// <summary>
        /// Read a GML document and returns relative <see cref="Geometry" />.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual IGeometry Read(XmlDocument document)
        {            
            return Read(new StringReader(document.InnerXml));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlText"></param>
        /// <returns></returns>
        public virtual IGeometry Read(string xmlText)
        {            
            return Read(new StringReader(xmlText));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringReader"></param>
        /// <returns></returns>
        public virtual IGeometry Read(StringReader stringReader)
        {
            XmlReader reader = XmlReader.Create(stringReader, settings);
            if (reader.IsStartElement("Point"))
                return ReadPoint(reader);
            else if (reader.IsStartElement("LineString"))
                return ReadLineString(reader);
            else if (reader.IsStartElement("Polygon"))
                return ReadPolygon(reader);
            else if (reader.IsStartElement("MultiPoint"))
                return ReadMultiPoint(reader);
            else if (reader.IsStartElement("MultiLineString"))
                return ReadMultiLineString(reader);
            else if (reader.IsStartElement("MultiPolygon"))
                return ReadMultiPolygon(reader);
            else if (reader.IsStartElement("MultiGeometry"))
                return ReadGeometryCollection(reader);
            throw new ArgumentException("ShouldNeverReachHere!");
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual ICoordinate ReadCoordinate(XmlReader reader)
        {            
            double x = 0, y = 0;
            while (reader.Read())
            {
                switch (reader.NodeType)
                { 
                    case XmlNodeType.Element:                        
                        if (reader.Name == "X")
                        {
                            reader.Read();      // Jump to X value
                            x = XmlConvert.ToDouble(reader.Value);
                        }
                        else if (reader.Name == "Y")
                        {
                            reader.Read();      // Jump to Y value
                            y = XmlConvert.ToDouble(reader.Value);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name == "coord")
                            return new Coordinate(x, y);
                        break;
                    default:
                        break;
                }
            }
            throw new ArgumentException("ShouldNeverReachHere!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IPoint ReadPoint(XmlReader reader)
        {            
            while (reader.Read())
            {
                switch (reader.NodeType)
                { 
                    case XmlNodeType.Element:
                        if (reader.Name == "coord")
                            return Factory.CreatePoint(ReadCoordinate(reader));
                        break;
                    default:
                        break;
                }
            }
            throw new ArgumentException("ShouldNeverReachHere!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadLineString(XmlReader reader)
        {
            ArrayList coordinates = new ArrayList();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "coord")
                            coordinates.Add(ReadCoordinate(reader));
                        break;
                    case XmlNodeType.EndElement:
                        return Factory.CreateLineString((Coordinate[])coordinates.ToArray(typeof(Coordinate)));
                    default:
                        break;
                }
            }
            throw new ArgumentException("ShouldNeverReachHere!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadLinearRing(XmlReader reader)
        {            
            return Factory.CreateLinearRing(ReadLineString(reader).Coordinates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadPolygon(XmlReader reader)
        {
            LinearRing exterior = null;
            ArrayList interiors = new ArrayList();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "outerBoundaryIs")
                            exterior = ReadLinearRing(reader) as LinearRing;
                        else if (reader.Name == "innerBoundaryIs")
                            interiors.Add(ReadLinearRing(reader));
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name == "Polygon")
                            return Factory.CreatePolygon(exterior, (ILinearRing[])interiors.ToArray(typeof(ILinearRing)));
                        break;
                    default:
                        break;
                }
            }
            throw new ArgumentException("ShouldNeverReachHere!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadMultiPoint(XmlReader reader)
        {
            ArrayList points = new ArrayList();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "pointMember")
                            points.Add(ReadPoint(reader));
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name == "MultiPoint")                        
                            return Factory.CreateMultiPoint((Point[])points.ToArray(typeof(Point)));
                        break;
                    default:
                        break;
                }
            }
            throw new ArgumentException("ShouldNeverReachHere!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadMultiLineString(XmlReader reader)
        {
            ArrayList lines = new ArrayList();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "lineStringMember")
                            lines.Add(ReadLineString(reader));
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name == "MultiLineString")                       
                            return Factory.CreateMultiLineString((LineString[])lines.ToArray(typeof(LineString)));
                        break;
                    default:
                        break;
                }
            }
            throw new ArgumentException("ShouldNeverReachHere!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadMultiPolygon(XmlReader reader)
        {
            ArrayList polygons = new ArrayList();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "polygonMember")
                            polygons.Add(ReadPolygon(reader));
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name == "MultiPolygon")
                            return Factory.CreateMultiPolygon((Polygon[])polygons.ToArray(typeof(Polygon)));
                        break;
                    default:
                        break;
                }
            }
            throw new ArgumentException("ShouldNeverReachHere!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual IGeometry ReadGeometryCollection(XmlReader reader)
        {
            ArrayList collection = new ArrayList();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "Point")
                            collection.Add(ReadPoint(reader));
                        else if (reader.Name == "LineString")
                            collection.Add(ReadLineString(reader));                        
                        else if (reader.Name == "Polygon")
                            collection.Add(ReadPolygon(reader));
                        else if (reader.Name == "MultiPoint")
                            collection.Add(ReadMultiPoint(reader));
                        else if (reader.Name == "MultiLineString")
                            collection.Add(ReadMultiLineString(reader));
                        else if (reader.Name == "MultiPolygon")
                            collection.Add(ReadMultiPolygon(reader));
                        else if (reader.Name == "MultiGeometry")
                            collection.Add(ReadGeometryCollection(reader));
                        break;
                    case XmlNodeType.EndElement:
                        if (reader.Name == "MultiGeometry")
                            return Factory.CreateGeometryCollection((Geometry[])collection.ToArray(typeof(Geometry)));
                        break;
                    default:
                        break;
                }
            }
            throw new ArgumentException("ShouldNeverReachHere!");
        }
    }
}
