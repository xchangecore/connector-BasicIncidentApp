using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UICDS_async
{
    class IncidentDigest
    {
        public String workProductIdentifier;
        public String incidentID;
        public String name;
        public String type;
        public String description;
        public double latitude;
        public double longitude;

        public IncidentDigest(String identifier, String incidentID, XElement digest)
        {
            this.workProductIdentifier = identifier;
            this.incidentID = incidentID;

            parseEvent(digest);
            parseLocation(digest);
        }

        private void parseEvent(XElement digest)
        {
            IEnumerable<XElement> elements = digest.Descendants(ServiceProxy.ucoreNS + "Event");
            if (elements.Count() > 0)
            {
                XElement descriptor = elements.ElementAt(0).Element(ServiceProxy.ucoreNS + "Descriptor");
                if (descriptor != null)
                {
                    description = descriptor.Value;
                }
                XElement identifier = elements.ElementAt(0).Element(ServiceProxy.ucoreNS + "Identifier");
                if (identifier != null)
                {
                    name = identifier.Value;
                }
                XElement what = elements.ElementAt(0).Element(ServiceProxy.ucoreNS + "What");
                if (what != null)
                {
                    XAttribute typeAttribute = what.Attribute(ServiceProxy.ucoreNS + "code");
                    if (typeAttribute != null)
                    {
                        type = typeAttribute.Value;
                    }
                }   
            }
        }

        private void parseLocation(XElement digest)
        {
            IEnumerable<XElement> elements = digest.Descendants(ServiceProxy.ucoreNS + "GeoLocation");
            if (elements.Count() > 0)
            {
                
                IEnumerable<XElement> circles = elements.ElementAt(0).Descendants(ServiceProxy.ucoreGMLNS + "CircleByCenterPoint");
                if (circles.Count() > 0)
                {
                    parseCircleByCenterPoint(circles);
                }

                IEnumerable<XElement> polygons = elements.ElementAt(0).Descendants(ServiceProxy.ucoreGMLNS + "Polygon");
                if (polygons.Count() > 0)
                {
                    parsePolygons(polygons);
                }
            }
        }

        private void parseCircleByCenterPoint(IEnumerable<XElement> circles)
        {
            XElement pos = circles.ElementAt(0).Element(ServiceProxy.ucoreGMLNS + "pos");
            parsePos(pos);
        }

        private void parsePos(XElement pos)
        {
            char splitChar = ' ';
            if (pos.Value.Contains(",")) 
            {
                splitChar = ',';
            }

            String[] values = pos.Value.Split(splitChar);
            if (values.Length == 2)
            {
                latitude = Double.Parse(values[0]);
                longitude = Double.Parse(values[1]);
            }
                
        }

        private void parsePolygons(IEnumerable<XElement> polygons)
        {
            IEnumerable<XElement> linearRings = polygons.ElementAt(0).Descendants(ServiceProxy.ucoreGMLNS + "LinearRing");
            if (linearRings.Count() > 0)
            {
                // Just use the first position.
                // For real code it would be best to find the centroid of the polygon
                XElement pos = linearRings.ElementAt(0).Element(ServiceProxy.ucoreGMLNS + "pos");
                parsePos(pos);
            }
        }

    }
}
