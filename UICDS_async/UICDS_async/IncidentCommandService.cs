using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace UICDS_async
{
    class IncidentCommandService : ServiceProxy
    {
        // reference to the work product service proxy
        private WorkProductService workProductService;

        public IncidentCommandService(WorkProductService workProductService)
        {
            this.workProductService = workProductService;
        }

        // Update an ICS on the UICDS core
        internal void UpdateICS(IncidentCommandStructure ics)
        {
            // Get the UpdateIncidentRequest message
            String request = WrapInSOAP(ics.GetUpdateRequest()).ToString();
            System.Diagnostics.Debug.WriteLine("Update ICS Request: ");

            // Post the UpdateIncidentRequest to the core
            try
            {
                String response = POST(request);
                //System.Diagnostics.Debug.WriteLine(response);

                // Have the local representation of the incident process the UpdateIncidentResponse
                if (ics.processUpdateICSResponse(response))
                {
                    System.Diagnostics.Debug.WriteLine("Update ICS was accepted");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Should poll endpoint for notifcation of accept or reject after updating of ICS");
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("UpdateICS exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                }
            }
        }
    }
}
