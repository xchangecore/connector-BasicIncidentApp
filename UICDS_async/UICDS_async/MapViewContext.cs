using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UICDS_async
{
    class MapViewContext : WorkProduct
    {
        static public String TYPE = "MapViewContext";

        public MapViewContext(WorkProduct workProduct) 
        {
            lastUpdateStatus = workProduct.GetLastUpdateStatus();
            workProductIdentification = workProduct.GetWorkProductIdentification();
            this.workProduct = workProduct.GetWorkProduct();
            this.structuredPayload = workProduct.GetLastFullStructuredPayload();
            type = TYPE;
        }

        public void AddLayer(XElement layer)
        {
            XElement viewContext = GetViewContext();
            IEnumerable<XElement> layers = structuredPayload.DescendantsAndSelf(ServiceProxy.contextNS + "LayerList");
            if (layers.Count() > 0)
            {
                layers.ElementAt(0).Add(layer);
            }
        }

        public XElement GetViewContext()
        {
            XElement payload = GetPayload(ServiceProxy.contextNS + "ViewContext");
            if (payload == null) throw new NullReferenceException("No payload found");
            return payload;
        }

        // Get an UpdateMapRequest mesage based on the values of this object
        public XElement GetUpdateRequest()
        {
            XElement updateMap = UpdateMapRequest();
            return updateMap;
        }

        private XElement UpdateMapRequest()
        {
            XElement request = new XElement(ServiceProxy.mapNS + "UpdateMapRequest",
                GetWorkProductIdentification(),
                GetPayload(ServiceProxy.contextNS + "ViewContext")
            );
            return request;
        }

        public bool processUpdateMapResponse(string response)
        {
            return processWorkProductProcessingResponse(response);
        }
    }
}
