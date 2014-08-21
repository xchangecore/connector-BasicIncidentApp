using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace UICDS_async
{
    public class PointBufferGeometry
    {
        private PointGeometryClass _point;
        private double _buffDist = 0.0;
        private string _units = "SMI";

        public PointBufferGeometry(double x, double y, double buffer, string unitType)
        {
            _point = new PointGeometryClass(x, y);
            _buffDist = buffer;
            _units = unitType;
        }

        public PointBufferGeometry(XElement geometryElements)
        {
            //get point xy
            string xyString = WorkProductUtilities.GetStringValueFromFirstElement(ServiceProxy.gmlNS + "pos", geometryElements);
            ConvertPosValueToXY(xyString);

            //get units and distance of buffer
            string units = WorkProductUtilities.GetStringAttributeFromFirstElement(ServiceProxy.gmlNS + "radius", geometryElements, ServiceProxy.gmlNS + "uom");
            if (!string.IsNullOrEmpty(units))
                _units = units;

            string numUnits = WorkProductUtilities.GetStringValueFromFirstElement(ServiceProxy.gmlNS + "radius", geometryElements);
            double dist = 0;
            if (double.TryParse(numUnits, out dist))
                _buffDist = dist;
        }

        private void ConvertPosValueToXY(string posValue)
        {
            if (!String.IsNullOrEmpty(posValue))
            {
                string[] vals = posValue.Split(' ');
                double xCoord = double.Parse(vals[1]);
                double yCoord = double.Parse(vals[0]);
                _point = new PointGeometryClass(xCoord, yCoord);
            }
        }

        public PointGeometryClass CenterPoint
        {
            get { return _point; }
        }

        public double BufferDistance
        {
            get { return _buffDist; }
        }

        public string BufferUnits
        {
            get { return _units; }
        }
    }
}
