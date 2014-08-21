using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;
using System.Net;

namespace UICDS_async
{
    class IncidentManagementService : ServiceProxy 
    {

        // key = UICDS incident id
        private Hashtable incidents = new Hashtable();

        // reference to the work product service proxy
        private WorkProductService workProductService;

        public IncidentManagementService(WorkProductService workProductService)
        {
            this.workProductService = workProductService;
        }

        public List<IncidentDigest> GetIncidentList()
        {
            List<IncidentDigest> list = new List<IncidentDigest>();

            XElement requestElement = new XElement(ServiceProxy.imsNS + "GetIncidentListRequest");

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

        // Create an incident on the UCIDS core
        internal void CreateIncident(Incident incident)
        {
            // Get the CreateIncidentRequest message for the incident
            String request = WrapInSOAP(incident.GetCreateRequest()).ToString();
            //System.Diagnostics.Debug.WriteLine(request);

            // Post the CreateIncidentRequest to the core
            try
            {
                String response = POST(request);
                //System.Diagnostics.Debug.WriteLine(response);

                // Set the reference to the WorkProductService for the incident
                incident.setWorkProductService(workProductService);

                // Have the local representation of the incident process the CreateIncidentResponse
                incident.processCreateIncidentResponse(response);

                // Keep a list of the incidents we create
                incidents.Add(incident.GetIncidentID(), incident);

                System.Diagnostics.Debug.WriteLine("Incident was created");
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("CreateIncident exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                }
            }
        }

        public Dictionary<string, SpatialIncident> GetActiveIncidents()
        {
            // key = UICDS incident id
            Dictionary<string, SpatialIncident> incidents = new Dictionary<string, SpatialIncident>();

            try
            {

                XElement requestElement = new XElement(ServiceProxy.imsNS + "GetIncidentListRequest");

                // Get the UpdateIncidentRequest message
                String request = WrapInSOAP(requestElement).ToString();
                //System.Diagnostics.Debug.WriteLine(request);

                XElement inctListXML; 

                // Post the GetIncidentListRequest to the core
                try
                {
                    String response = POST(request);
                    //System.Diagnostics.Debug.WriteLine(response);

                    inctListXML = XElement.Parse(response);

                }
                catch (WebException ex)
                {
                    System.Diagnostics.Debug.Write("GetIncidentList exception: " + ex.Status + ": ");
                    if (ex.Response != null && ex.Response is HttpWebResponse)
                    {
                        System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                    }
                    return incidents;
                }

                //get enumeration of WorkProducts
                IEnumerable<XElement> workProductXMLElements = inctListXML.Descendants(ServiceProxy.precissNS + "WorkProduct");
                //System.Windows.Forms.MessageBox.Show("After getting workProductXMLElements");
                if (workProductXMLElements.Count() > 0)
                {
                    //System.Windows.Forms.MessageBox.Show("The number of Work Product elements is " + workProductXMLElements.Count().ToString());

                    foreach (XElement wpElem in workProductXMLElements)
                    {
                        try
                        {
                            //get basic info

                            //get type and state
                            XElement workProductIdentification = WorkProductUtilities.GetWorkProductIdentification(wpElem);
                            string inctType = WorkProductUtilities.GetStringValueFromFirstElement(ServiceProxy.precisbNS + "Type", workProductIdentification);
                            string state = WorkProductUtilities.GetStringValueFromFirstElement(ServiceProxy.precisbNS + "State", workProductIdentification);
                            string workProdID = WorkProductUtilities.GetStringValueFromFirstElement(ServiceProxy.precisbNS + "Identifier", workProductIdentification);
                            //System.Windows.Forms.MessageBox.Show("After getting type and state");

                            //get Incident ID
                            XElement workProductProps = WorkProductUtilities.GetWorkProductProperties(wpElem);
                            string inctID = WorkProductUtilities.GetStringValueFromFirstElement(ServiceProxy.precisbNS + "AssociatedGroups", workProductProps);
                            //System.Windows.Forms.MessageBox.Show("The incident ID is " + inctID);

                            //get descriptive info
                            XElement eventInfo = WorkProductUtilities.GetEventInfo(wpElem);
                            string description = WorkProductUtilities.GetStringValueFromFirstElement(ucoreNS + "Descriptor", eventInfo);
                            string inctName = WorkProductUtilities.GetStringValueFromFirstElement(ucoreNS + "Identifier", eventInfo);
                            string inctCategory = WorkProductUtilities.GetStringAttributeFromFirstElement(ucoreNS + "What", eventInfo, ucoreNS + "code");
                            //System.Windows.Forms.MessageBox.Show("After getting descriptive info");

                            //System.Windows.Forms.MessageBox.Show("Incident Name is " + inctName);

                            //get location info
                            XElement locationinfo = WorkProductUtilities.GetLocationInfo(wpElem);
                            //System.Windows.Forms.MessageBox.Show("The incident ID is " + inctID);
                            //System.Windows.Forms.MessageBox.Show("After getting location info");

                            object objGeom = null;
                            if (locationinfo != null)
                            {
                                //make geometry
                                IEnumerable<XElement> geomElements = locationinfo.Descendants();
                                if (geomElements.ElementAt(0).Name.LocalName.ToLower() == "circlebycenterpoint")
                                {
                                    //make circlebuffer geometry
                                    PointBufferGeometry circleGeom = new PointBufferGeometry(geomElements.ElementAt(0));
                                    objGeom = circleGeom;
                                }
                                else if (geomElements.ElementAt(0).Name.LocalName.ToLower() == "polygon")
                                {
                                    PolygonGeometry polyGeom = new PolygonGeometry(geomElements.ElementAt(0));
                                    objGeom = polyGeom;
                                }
                            }

                            //make a spatial incident and add it to the hash table
                            SpatialIncident spatIncident = new SpatialIncident()
                            {
                                incidentID = inctID,
                                workProductIdentity = workProdID,
                                status = state,
                                incidentType = inctType,
                                description = description,
                                name = inctName,
                                category = inctCategory,
                                shapeGeometry = objGeom
                            };

                            incidents.Add(inctID, spatIncident);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Error creating Spatial Incident: " + ex.Message);
                            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                        }
                    }

                }
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error creating Spatial Incident: " + err.Message);
                System.Diagnostics.Debug.WriteLine(err.StackTrace);
            }

            return incidents;
        }

        // Update an incident on the UICDS core
        internal void UpdateIncident(Incident incident)
        {
            // Get the UpdateIncidentRequest message
            String request = WrapInSOAP(incident.GetUpdateRequest()).ToString();
            //System.Diagnostics.Debug.WriteLine(request);

            // Post the UpdateIncidentRequest to the core
            try
            {
                String response = POST(request);
                //System.Diagnostics.Debug.WriteLine(response);

                // Have the local representation of the incident process the UpdateIncidentResponse
                if (incident.processUpdateIncidentResponse(response))
                {
                    System.Diagnostics.Debug.WriteLine("Update was accepted");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Should poll endpoint for notifcation of accept or reject after updating");
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("UpdateIncident exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                }
            }
        }

        // Close an incident on the UICDS core
        internal void CloseIncident(Incident incident)
        {
            // Get the CloseIncidentRequest message
            String request = WrapInSOAP(incident.GetCloseRequest()).ToString();

            // Post the CloseIncidentRequest message to the core
            try
            {
                String response = POST(request);

                // Have the local representation of the incident process the CloseIncidentResponse
                if (incident.processCloseIncidentResponse(response))
                {
                    System.Diagnostics.Debug.WriteLine("Close was accepted");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Should poll endpoint for notifcation of accept or reject after closing");
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("CloseIncident exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                }
            }
        }

        // Archive an incident on the UICDS core
        internal void ArchiveIncident(Incident incident)
        {
            // Get the ArchiveIncidentRequest message
            String request = WrapInSOAP(incident.GetArchiveRequest()).ToString();

            // Post the ArchiveIncidentMessage to the core
            try
            {
                String response = POST(request);

                // Have the local representation of the incident process the ArchiveIncidentResponse
                if (incident.processArchiveIncidentResponse(response))
                {
                    System.Diagnostics.Debug.WriteLine("Archive was accepted");

                    // remove from the list of incidents we know about
                    incidents.Remove(incident.GetIncidentID());
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Should poll endpoint for notifcation of accept or reject after archiving");
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
        }
    }
}
