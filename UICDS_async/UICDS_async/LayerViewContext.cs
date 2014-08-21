using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UICDS_async
{
    class LayerViewContext : WorkProduct
    {
        static public String TYPE = "LayerViewContext";

        public LayerViewContext(WorkProduct workProduct)
        {
            lastUpdateStatus = workProduct.GetLastUpdateStatus();
            workProductIdentification = workProduct.GetWorkProductIdentification();
            this.workProduct = workProduct.GetWorkProduct();
            this.structuredPayload = workProduct.GetLastFullStructuredPayload();
            type = TYPE;
        }

        public static XElement CreateLayerElement(String serviceTitle, String serviceType, String serviceVersion, 
            String onlineResourceHref, String layerName, String layerTitle, String layerSRS)
        {
            XElement layer = new XElement(ServiceProxy.contextNS + "Layer",
                new XAttribute("hidden", "false"),
                new XAttribute("queryable", "false"),
                new XElement(ServiceProxy.contextNS + "Server",
                    new XAttribute("service", serviceType),
                    new XAttribute("title", serviceTitle),
                    new XAttribute("version", serviceVersion),
                new XElement(ServiceProxy.contextNS + "OnlineResource",
                    new XAttribute(ServiceProxy.xlinkNS + "href", onlineResourceHref)
                )
            ),
            new XElement(ServiceProxy.contextNS + "Name", layerName),
            new XElement(ServiceProxy.contextNS + "Title", layerTitle),
            new XElement(ServiceProxy.contextNS + "SRS", layerSRS)
            );

            return layer;
        }

        public XElement GetLayer()
        {
            XElement payload = GetPayload(ServiceProxy.contextNS + "Layer");
            if (payload == null) throw new NullReferenceException("No payload found");
            return payload;
        }

        public bool processUpdateLayerResponse(string response)
        {
            return processWorkProductProcessingResponse(response);
        }

        internal static XElement GetSubmitLayerRequest(XElement layer, String incidentID)
        {
            XElement request = new XElement(ServiceProxy.mapNS + "SubmitLayerRequest",
                new XElement(ServiceProxy.mapNS + "IncidentId", incidentID),
                layer
            );
            return request;
        }
    }
}
