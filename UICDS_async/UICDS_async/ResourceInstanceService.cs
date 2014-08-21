using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.IO;

namespace UICDS_async
{
    class ResourceInstanceService : ServiceProxy
    {
        // Register this application with the core 
        internal bool RegisterApplication(string applicationID, string localID, string applicationProfileID)
        {
            bool registered = false;

            // If the resource already exists then return true
            if (ResourceExists(applicationID))
            {
                return true;
            }

            // Get the CreateIncidentRequest message for the incident
            String request = WrapInSOAP(GetRegisterRequest(applicationID, localID, applicationProfileID)).ToString();
            System.Diagnostics.Debug.WriteLine(request);

            // Post the CreateIncidentRequest to the core
            try
            {
                String response = POST(request);
                registered = true;
                //System.Diagnostics.Debug.WriteLine(response);

            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("registerApplication exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    HttpWebResponse webResponse = (HttpWebResponse)ex.Response;
                    StreamReader loResponseStream = new StreamReader(webResponse.GetResponseStream());
                    string response = loResponseStream.ReadToEnd();
                    String faultElement = MessageIfSoapFault(response);
                    if (faultElement != null)
                    {
                        System.Diagnostics.Debug.WriteLine(faultElement);
                        registered = false;
                    }
                    else
                    {
                        registered = true;
                    }
                }
            }
            return registered;
        }

        // Check the list of current resources to see if the one we want has alread registered with the core
        private bool ResourceExists(string applicationID)
        {
            bool exists = false;
            // get the list of resource instances and see if it exists
            String request = WrapInSOAP(GetResourceInstanceList()).ToString();

            try
            {
                String response = POST(request);
                //System.Diagnostics.Debug.WriteLine(response);

                XElement responseXML = XElement.Parse(response);
                IEnumerable<XElement> instances = responseXML.Descendants(resourceInstanceNS + "ResourceInstance");
                if (instances.Count() > 0)
                {
                    foreach (XElement instance in instances)
                    {
                        IEnumerable<XElement> ids = instance.Descendants(resourceInstanceNS + "ID");
                        if (ids.Count() > 0)
                        {
                            if (ids.First().Value.Equals(applicationID))
                            {
                                exists = true;
                            }
                        }
                    }
                }

            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("registerApplication exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                }
            }

            return exists;
        }

        // Get a resource instance
        public ResourceInstance GetResourceInstance(String applicationID)
        {
            ResourceInstance instance = null;

            // Create a request for a work product
            XElement getResourceInstanceRequest = new XElement(resourceInstanceNS + "GetResourceInstanceRequest",
                new XElement(resourceInstanceNS + "ID", applicationID)
            );

            try
            {
                String response = POST(WrapInSOAP(getResourceInstanceRequest).ToString());

                // Parse the resuls
                XElement responseXML = XElement.Parse(response);

                instance = new ResourceInstance();
                IEnumerable<XElement> ids = responseXML.Descendants(resourceInstanceNS + "ID");
                if (ids.Count() > 0)
                {
                    instance.ID = ids.First().Value;
                }
                IEnumerable<XElement> endpoints = responseXML.Descendants(resourceInstanceNS + "Endpoint");
                if (endpoints.Count() > 0)
                {
                    instance.endpoint = endpoints.First().Value;
                }


            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("ArchiveIncident exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                }
            }

            return instance;
        }


        // Get a Regester resource instance request
        public XElement GetRegisterRequest(string applicationID, string localID, string applicationProfileID)
        {
            // Create a request for a work product
            XElement getRegisterRequest = new XElement(resourceInstanceNS + "RegisterRequest",
                new XElement(resourceInstanceNS + "ID", applicationID),
                new XElement(resourceInstanceNS + "LocalResourceID", localID),
                new XElement(resourceInstanceNS + "ResourceProfileID", applicationProfileID)
            );

            return getRegisterRequest;
        }

        // Get a ResoruceInstanceList request
        public XElement GetResourceInstanceList()
        {
            // Create a request for a work product
            XElement getResourceInstanceListRequest = new XElement(resourceInstanceNS + "GetResourceInstanceListRequest",
                new XElement(resourceInstanceNS + "QueryString")
            );

            return getResourceInstanceListRequest;
        }


    }
}
