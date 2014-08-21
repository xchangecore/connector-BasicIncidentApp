using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;
using System.IO;

namespace UICDS_async
{
    class ResourceProfileService : ServiceProxy
    {
        // Register this application with the core 
        internal bool CreateOrGetProfile(string applicationProfileID, List<string> interests)
        {
            bool created = false;

            // If the resource profile already exists then return true
            if (ProfileExists(applicationProfileID))
            {
                return true;
            }

            // Get the CreateProfileRequest message for the incident
            String request = WrapInSOAP(GetCreateProfileRequest(applicationProfileID, interests)).ToString();
            //System.Diagnostics.Debug.WriteLine(request);

            // Post the CreateIncidentRequest to the core
            try
            {
                String response = POST(request);
                created = true;
                //System.Diagnostics.Debug.WriteLine(response);

            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("CreateOrGetProfile exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    HttpWebResponse webResponse = (HttpWebResponse)ex.Response;
                    StreamReader loResponseStream = new StreamReader(webResponse.GetResponseStream());
                    string response = loResponseStream.ReadToEnd();
                    String faultElement = MessageIfSoapFault(response);
                    if (faultElement != null)
                    {
                        System.Diagnostics.Debug.WriteLine(faultElement);
                        created = false;
                    }
                    else
                    {
                        created = true;
                    }
                }
            }
            return created;
        }

        // Check the list of current resource profiles to see if the one we want was already created
        private bool ProfileExists(string profileID)
        {
            bool exists = false;
            // get the list of resource instances and see if it exists
            String request = WrapInSOAP(GetResourceProfileList()).ToString();
            //System.Diagnostics.Debug.WriteLine(request);

            try
            {
                String response = POST(request);
                //System.Diagnostics.Debug.WriteLine(response);

                XElement responseXML = XElement.Parse(response);
                IEnumerable<XElement> profiles = responseXML.Descendants(resourceProfileNS + "ResourceProfile");
                if (profiles.Count() > 0)
                {
                    foreach (XElement profile in profiles) {
                        IEnumerable<XElement> ids = profile.Descendants(resourceProfileNS + "ID");
                        if (ids.Count() > 0)
                        {
                            if (ids.First().Value.Equals(profileID)) 
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
        public XElement GetCreateProfileRequest(string profileID, List<string> interests)
        {
            // Create interests element
            XElement interestsElement = new XElement(resourceProfileNS + "Interests");
            foreach (string interest in interests) {
                interestsElement.Add(new XElement(resourceProfileNS + "Interest",
                                         new XElement(resourceProfileNS + "TopicExpression", interest)
                                     ));
            }

            // Create a request for a work product
            XElement getRegisterRequest = new XElement(resourceProfileNS + "CreateProfileRequest",
                new XElement(resourceProfileNS + "Profile",
                    new XElement(resourceProfileNS + "ID", profileID),
                    new XElement(resourceProfileNS + "Description", "DotNet Example Application Profile"),
                    interestsElement
                )
            );

            return getRegisterRequest;
        }

        // Get a ResoruceInstanceList request
        public XElement GetResourceProfileList()
        {
            // Create a request for a work product
            XElement getResourceProfileListRequest = new XElement(resourceProfileNS + "GetProfileListRequest",
                new XElement(resourceInstanceNS + "QueryString")
            );

            return getResourceProfileListRequest;
        }


    }
}
