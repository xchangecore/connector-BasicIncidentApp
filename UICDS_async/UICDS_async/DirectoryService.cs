using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;

namespace UICDS_async
{
    class DirectoryService : ServiceProxy
    {
        public List<IncidentDigest> GetIncidentList()
        {
            List<IncidentDigest> list = new List<IncidentDigest>();

            XElement requestElement = new XElement(ServiceProxy.dirNS + "GetIncidentListRequest");

            // Get the UpdateIncidentRequest message
            String request = WrapInSOAP(requestElement).ToString();
            //System.Diagnostics.Debug.WriteLine(request);

            // Post the GetIncidentListRequest to the core
            try
            {
                String response = POST(request);
                //System.Diagnostics.Debug.WriteLine(response);

                list = CreateIncidentListFromResponse(response);

            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("GetIncidentList exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                }
            }

            return list;
        }

        private List<IncidentDigest> CreateIncidentListFromResponse(string response)
        {
            List<IncidentDigest> list = new List<IncidentDigest>();

            // parse the response
            XElement responseXML = XElement.Parse(response);

            IEnumerable<XElement> elements = responseXML.Descendants(ServiceProxy.precissNS + "WorkProduct");
            if (elements.Count() > 0)
            {
                IEnumerator<XElement> items = elements.GetEnumerator();

                while (items.MoveNext())
                {
                    WorkProduct wp = new WorkProduct(items.Current);

                    IncidentDigest digest = null;
                    if (wp.GetDigest() != null)
                    {
                        digest = new IncidentDigest(wp.identifier, wp.incidentID, wp.GetDigest());

                    }
                    list.Add(digest);
                }
            }

            return list;
        }
    }
}
