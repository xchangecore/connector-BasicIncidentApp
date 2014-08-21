using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace UICDS_async
{
    public class PolygonGeometry
    {
        private Dictionary<int, List<PointGeometryClass>> _rings = new Dictionary<int,List<PointGeometryClass>>();

        public PolygonGeometry() { }

        public PolygonGeometry(XElement geomElements)
        {
            //get linear ring elements
            IEnumerable<XElement> elements = geomElements.Descendants(ServiceProxy.gmlNS + "LinearRing");
            if (elements.Count() > 0)
            {
                //loop through and make rings to add to dictionary
                int cnt = 0;
                foreach (XElement elem in elements)
                {
                    //get all pos elements, loop and make vertices
                    IEnumerable<XElement> posElems = elem.Descendants(ServiceProxy.gmlNS + "pos");
                    if (posElems.Count() > 0)
                    {
                        List<PointGeometryClass> ptList = new List<PointGeometryClass>();
                        foreach(XElement pElem in posElems)
                        {
                            string xyString = pElem.Value;
                            string[] vals = xyString.Split(new char[] { ' ', ',' });
                            double x = double.Parse(vals[0]);
                            double y = double.Parse(vals[1]);
                            ptList.Add(new PointGeometryClass(x, y));
                        }
                        _rings.Add(cnt, ptList);
                    }
                }
            }
        }

        public void AddRing(int ringNumber, List<PointGeometryClass> ring)
        {
            _rings.Add(ringNumber, ring);
        }

        public Dictionary<int,List<PointGeometryClass>> GetRings()
        {
            return _rings;
        }
    }
}
