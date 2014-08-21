using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICDS_async
{
    public class SpatialIncident
    {
        public string incidentID { get; set; }
        public string status { get; set; }
        public string incidentType { get; set; }
        public string workProductIdentity { get; set; }

        public string description { get; set; }
        public string name { get; set; }
        public string category { get; set; }

        public object shapeGeometry { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
