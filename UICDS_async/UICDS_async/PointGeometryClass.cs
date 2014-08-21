using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICDS_async
{
    public class PointGeometryClass
    {
        public double xCoord { get; set; }
        public double yCoord { get; set; }

        public PointGeometryClass(double x, double y)
        {
            xCoord = x;
            yCoord = y;
        }
    }
}
