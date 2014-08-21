using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;

namespace UICDS_async
{
    class MapService : ServiceProxy
    {
        // reference to the work product service proxy
        private WorkProductService workProductService;

        public MapService(WorkProductService workProductService)
        {
            this.workProductService = workProductService;
        }

        public void UpdateMap(MapViewContext map)
        {
            // Get the UpdateMapRequest message
            String request = WrapInSOAP(map.GetUpdateRequest()).ToString();
            //System.Diagnostics.Debug.WriteLine("Update Map: " + request);

            // Post the UpdateIncidentRequest to the core
            try
            {
                String response = POST(request);
                //System.Diagnostics.Debug.WriteLine(response);

                // Have the local representation of the incident process the UpdateIncidentResponse
                if (map.processUpdateMapResponse(response))
                {
                    System.Diagnostics.Debug.WriteLine("Update Map was accepted");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Should poll endpoint for notifcation of accept or reject after updating");
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("UpdateMap exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                }
            }
        }

        internal void AddLayerWorkProduct(XElement layer, String incidentID)
        {
            String request = WrapInSOAP(LayerViewContext.GetSubmitLayerRequest(layer, incidentID)).ToString();
            //System.Diagnostics.Debug.WriteLine("Create Layer: " + request);

            // Post the SubmitLayerRequest to the core
            try
            {
                String response = POST(request);
                //System.Diagnostics.Debug.WriteLine(response);

                // Have the local representation of the incident process the UpdateIncidentResponse
                XElement responseXML = XElement.Parse(response);
                WorkProduct.ProcessingStatus status = WorkProduct.GetStatusFromWorkProductProcessingStatus(responseXML);
                if (status  == WorkProduct.ProcessingStatus.ACCEPTED)
                {
                    System.Diagnostics.Debug.WriteLine("Layer creation was accepted");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Should poll endpoint for notifcation of accept or reject after creating layer");
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("SubmitLayer exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                }
            }

        }
    }
}
