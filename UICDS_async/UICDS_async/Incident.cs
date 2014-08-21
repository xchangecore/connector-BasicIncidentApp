using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections;

namespace UICDS_async
{
    // Represents an incident on a UICDS core
    // TODO: should derive from WorkProduct like IncidentCommandStructure and move common methods to WorkProduct class
    class Incident
    {
        // UICDS Incident Management Service
        private XNamespace imsNS = "http://uicds.org/IncidentManagementService";

        // Incident types
        private XNamespace incNS = "http://uicds.org/incident";

        // UICDS Infrastructure
        private XNamespace precisbNS = "http://www.saic.com/precis/2009/06/base";
        private XNamespace precissNS = "http://www.saic.com/precis/2009/06/structures";
        private XNamespace precispNS = "http://www.saic.com/precis/2009/06/payloads";

        // NIEM
        private XNamespace niemCoreNS = "http://niem.gov/niem/niem-core/2.0";

        // The incident work product from the core
        private WorkProduct workProduct;

        // key = UICDS work product id
        private Dictionary<String, WorkProduct> associatedWorkProducts = new Dictionary<string, WorkProduct>();

        public String name;
        public String dateTime;
        public String type;
        public String description;
        public int latitudeDegrees;
        public int latitudeMinutes;
        public double latitudeSeconds;
        public int longitudeDegrees;
        public int longitudeMinutes;
        public double longitudeSeconds;

        // Values in the incident document maintained by the core so
        // there is no accessors to change these values
        private String workProductID = null;
        private String incidentID = null;
        private String sharedCoreName;
        private String owningCore;

        // Incident is open on UICDS core
        private bool open;

        // Referece to the UICDS work product service proxy
        private WorkProductService workProductService;

        public Incident()
        {
        }

        public Incident(WorkProductService proxy)
        {
            workProductService = proxy;
        }

        public Incident(WorkProductService proxy, string incidentID)
        {
            workProductService = proxy;


            // Find and process the Incident work product first
            List<WorkProduct> workProducts = workProductService.GetAssociatedWorkProducts(incidentID);
            foreach (WorkProduct workProduct in workProducts)
            {
                if (workProduct.type.Equals("Incident"))
                {
                    // Get the work product identification structure from the document
                    XElement workProductIdentification = workProduct.GetWorkProductIdentification();

                    // Get the full work product and update all the fields of this object accordingly
                    if (workProductIdentification != null)
                    {
                        this.workProduct = workProductService.GetWorkProduct(workProductIdentification);
                        //System.Diagnostics.Debug.WriteLine(workProduct.ToString());

                        // Update the incident properties based on the final work product the core created
                        updateIncidentProperties();

                    }

                    // Set incident status as open
                    open = true;

                    // remove the incident work product from the list that need to be processed
                    workProducts.Remove(workProduct);
                    break;
                }
            }

            // Process all the associated work products
            foreach (WorkProduct workProduct in workProducts)
            {
                // Get the work product identification structure from the document
                XElement workProductIdentification = workProduct.GetWorkProductIdentification();

                // Get the full work product and update all the fields of this object accordingly
                if (workProductIdentification != null)
                {
                    WorkProduct fullWorkProduct = workProductService.GetWorkProduct(workProductIdentification);
                    //System.Diagnostics.Debug.WriteLine(workProduct.ToString());

                    processAssociatedWorkProduct(fullWorkProduct);

                }
            }
        }

        public bool isOpen()
        {
            return open;
        }

        public void setWorkProductService(WorkProductService workProductServiceProxy)
        {
            this.workProductService = workProductServiceProxy;
        }

        public void updateFromWorkProduct(WorkProduct workProduct)
        {
            this.workProduct = workProduct;
            updateIncidentProperties();
        }

        public void archiveIncident()
        {
            System.Diagnostics.Debug.WriteLine(" Archiving incident: " + incidentID);
        }

        public String GetIncidentID()
        {
            return incidentID;
        }

        // Get a CreateIncidentRequest mesage based on the values of this object
        public XElement GetCreateRequest()
        {
            XElement createIncident = CreateIncidentRequest();
            return createIncident;
        }

        // Get an UpdateIncidentRequest mesage based on the values of this object
        public XElement GetUpdateRequest()
        {
            XElement updateIncident = UpdateIncidentRequest();
            return updateIncident;
        }

        // Get a CloseIncidentRequest mesage based on the values of this object
        public XElement GetCloseRequest()
        {
            XElement closeIncident = CloseIncidentRequest();
            return closeIncident;
        }

        // Get an ArchiveIncidentRequest mesage based on the values of this object
        public XElement GetArchiveRequest()
        {
            XElement archiveIncident = ArchiveIncidentRequest();
            return archiveIncident;
        }

        // Builds the CreateIncidentRequest from this object
        private XElement CreateIncidentRequest()
        {
            XElement request = new XElement(imsNS + "CreateIncidentRequest",
                BuildUICDSIncidentTypeElement(incNS, "Incident")
            );
            return request;
        }

        // Builds the UpdateIncidentRequest from this object
        private XElement UpdateIncidentRequest()
        {
            // TODO
            // We should put our properties back into the full work product
            // because if someone else changed it using GetUICDSIncidentType 
            // could lose their changes
            XElement request = new XElement(imsNS + "UpdateIncidentRequest",
                workProduct.GetWorkProductIdentification(),
                BuildUICDSIncidentTypeElement(incNS, "Incident")
            );
            return request;
        }

        // Builds the CloseIncidentRequest from this object
        private XElement CloseIncidentRequest()
        {
            return new XElement(imsNS + "CloseIncidentRequest",
                                new XElement(imsNS + "IncidentID", incidentID)
                                );
        }

        // Builds the ArchiveIncidentRequest from this object
        private XElement ArchiveIncidentRequest()
        {
            return new XElement(imsNS + "ArchiveIncidentRequest",
                                new XElement(imsNS + "IncidentID", incidentID)
                                );
        }

        // Builds the UICDSIncidentType XML element from this object
        private XElement BuildUICDSIncidentTypeElement(XNamespace incidentElementNS, String incidentElementName)
        {
            XElement incident = new XElement(incidentElementNS + incidentElementName,
                                new XElement(niemCoreNS + "ActivityCategoryText", type),
                                new XElement(niemCoreNS + "ActivityDate",
                                    new XElement(niemCoreNS + "DateTime", dateTime)
                                ),
                                new XElement(niemCoreNS + "ActivityDescriptionText", description),
                                new XElement(niemCoreNS + "ActivityName", name),
                                new XElement(niemCoreNS + "IncidentLocation",
                                    new XElement(niemCoreNS + "LocationArea",
                                        new XElement(niemCoreNS + "AreaCircularRegion",
                                            new XElement(niemCoreNS + "CircularRegionCenterCoordinate",
                                                new XElement(niemCoreNS + "GeographicCoordinateLatitude",
                                                    new XElement(niemCoreNS + "LatitudeDegreeValue", latitudeDegrees),
                                                    new XElement(niemCoreNS + "LatitudeMinuteValue", latitudeMinutes),
                                                    new XElement(niemCoreNS + "LatitudeSecondValue", latitudeSeconds)
                                                ),
                                                new XElement(niemCoreNS + "GeographicCoordinateLongitude",
                                                    new XElement(niemCoreNS + "LongitudeDegreeValue", latitudeDegrees),
                                                    new XElement(niemCoreNS + "LongitudeMinuteValue", latitudeMinutes),
                                                    new XElement(niemCoreNS + "LongitudeSecondValue", latitudeSeconds)
                                                )
                                             ),
                                             new XElement(niemCoreNS + "CircularRegionRadiusLengthMeasure",
                                                 new XElement(niemCoreNS + "MeasurePointValue", 0.0),
                                                 new XElement(niemCoreNS + "LengthUnitCode", "SMI")
                                             )
                                        )
                                    )

                                ),
                                new XElement(niemCoreNS + "IncidentLocation"),
                                new XElement(niemCoreNS + "IncidentJurisdictionalOrganization"),
                                new XElement(niemCoreNS + "IncidentObservationText")
                            );
            if (incidentID != null)
            {
                incident.AddFirst(new XElement(niemCoreNS + "ActivityIdentification",
                    new XElement(niemCoreNS + "IdentificationID", incidentID)
                        )
                    );
            }
            return incident;
        }

        // Process the CreateIncidentResponse message
        public void processCreateIncidentResponse(String response)
        {
            // parse the response
            XElement responseXML = XElement.Parse(response);

            // Get the work product identification structure from the document
            XElement workProductIdentification = null;
            workProductIdentification = WorkProductUtilities.GetWorkProductIdentification(responseXML);

            // Get the full work product and update all the fields of this object accordingly
            if (workProductIdentification != null)
            {
                workProduct = workProductService.GetWorkProduct(workProductIdentification);
                //System.Diagnostics.Debug.WriteLine(workProduct.ToString());

                // Update the incident properties based on the final work product the core created
                updateIncidentProperties();

            }

            // Set incident status as open
            open = true;
        }

        // Process the UpdateIncidentResponse message if it was accepted
        public bool processUpdateIncidentResponse(string response)
        {
            bool accepted = false;

            // parse the response
            XElement responseXML = XElement.Parse(response);

            // Get the status of the response
            WorkProduct.ProcessingStatus status = WorkProductUtilities.GetStatusFromWorkProductProcessingStatus(responseXML);
            IEnumerable<XElement> elements = responseXML.Descendants(precisbNS + "Status");
            if (elements.Count() > 0)
            {
                String statusString = elements.ElementAt(0).Value;
                if (statusString.Equals("Accepted"))
                {
                    status = WorkProduct.ProcessingStatus.ACCEPTED;
                }
                else if (statusString.Equals("Rejected"))
                {
                    status = WorkProduct.ProcessingStatus.REJECTED;
                }
                else
                {
                    status = WorkProduct.ProcessingStatus.PENDING;
                }
            }

            // Check the status of the request (else handle PENDING or REJECTED in caller)
            if (status.Equals(WorkProduct.ProcessingStatus.ACCEPTED))
            {
                // Get the latest work product identification
                XElement workProductIdentification = WorkProductUtilities.GetWorkProductIdentification(responseXML);

                if (workProductIdentification != null)
                {
                    // Get the work product
                    workProduct = workProductService.GetWorkProduct(workProductIdentification);

                    // Update the incident properties based on the final work product the core created
                    updateIncidentProperties();
                }

                accepted = true;
            }

            return accepted;
        }

        public void processAssociatedWorkProduct(WorkProduct workProduct)
        {
            // Make sure this work product is for this incident
            if (workProduct.incidentID.Equals(incidentID))
            {
                //System.Diagnostics.Debug.WriteLine("Processing work product of type: " + workProduct.type);
                if (associatedWorkProducts.ContainsKey(workProduct.type))
                {
                    //System.Diagnostics.Debug.WriteLine("      Updating work product: " + workProduct.GetWorkProductIdentification());
                    associatedWorkProducts[workProduct.type] = GetTypedWorkProduct(workProduct);
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine("      Adding work product of type: " + workProduct.GetWorkProductIdentification());
                    associatedWorkProducts.Add(workProduct.type, GetTypedWorkProduct(workProduct));
                }
            }
        }

        public WorkProduct GetAssociatedWorkProduct(String type)
        {
            if (associatedWorkProducts.ContainsKey(type))
            {
                return associatedWorkProducts[type];
            }
            return null;
        }

        private WorkProduct GetTypedWorkProduct(WorkProduct workProduct)
        {
            if (workProduct.type.Equals(IncidentCommandStructure.TYPE))
            {
                IncidentCommandStructure wp = new IncidentCommandStructure(workProduct);
                return wp;
            }
            else if (workProduct.type.Equals(MapViewContext.TYPE))
            {
                MapViewContext wp = new MapViewContext(workProduct);
                return wp;
            }
            return workProduct;
        }

        // Process the CloseIncidentResponse message if it was accepted
        public bool processCloseIncidentResponse(string response)
        {
            // Check the status of the request
            bool accepted = false;
            XElement responseXML = XElement.Parse(response);

            // Get the status of the response
            WorkProduct.ProcessingStatus status = WorkProductUtilities.GetStatusFromWorkProductProcessingStatus(responseXML);

            // Check the status of the request (else handle PENDING or REJECTED in caller)
            if (status.Equals(WorkProduct.ProcessingStatus.ACCEPTED))
            {
                // Get the latest work product identification
                XElement workProductIdentification = WorkProductUtilities.GetWorkProductIdentification(responseXML);

                if (workProductIdentification != null)
                {
                    // Get the work product
                    workProduct = workProductService.GetWorkProduct(workProductIdentification);

                    // Update the incident properties based on the final work product the core created
                    updateIncidentProperties();
                }

                // Set incident as closed
                open = false;

                accepted = true;
            }

            return accepted;
        }

        // Process the ArchiveIncidentResponse message if it was accepted
        public bool processArchiveIncidentResponse(string response)
        {
            // Check the status of the request
            bool accepted = false;
            XElement responseXML = XElement.Parse(response);

            // Get the status of the response
            WorkProduct.ProcessingStatus status = WorkProductUtilities.GetStatusFromWorkProductProcessingStatus(responseXML);

            // Check the status of the request (else handle PENDING or REJECTED in caller)
            if (status.Equals(WorkProduct.ProcessingStatus.ACCEPTED))
            {
                accepted = true;
            }

            return accepted;
        }

        // Update the properties of this object based on the values from the latest UICDS work product
        private void updateIncidentProperties()
        {
            // Get the work product identifier
            XName idAttribute;
            idAttribute = ServiceProxy.niemStructuresNS + "id";
            IEnumerable<XAttribute> attributes = workProduct.GetWorkProduct().Descendants(incNS + "Incident").Attributes(idAttribute);
            if (attributes.Count() > 0)
            {
                workProductID = attributes.ElementAt(0).Value;
            }

            // Get the properties from the work product
            incidentID = WorkProductUtilities.GetStringValueFromFirstElement(workProduct, niemCoreNS + "IdentificationID");
            type = WorkProductUtilities.GetStringValueFromFirstElement(workProduct, niemCoreNS + "ActivityCategoryText");
            dateTime = WorkProductUtilities.GetStringValueFromFirstElement(workProduct, niemCoreNS + "DateTime");
            description = WorkProductUtilities.GetStringValueFromFirstElement(workProduct, niemCoreNS + "ActivityDescriptionText");
            name = WorkProductUtilities.GetStringValueFromFirstElement(workProduct, niemCoreNS + "ActivityName");
            latitudeDegrees = WorkProductUtilities.GetIntValueFromFirstElement(workProduct, niemCoreNS + "LatitudeDegreeValue");
            latitudeMinutes = WorkProductUtilities.GetIntValueFromFirstElement(workProduct, niemCoreNS + "LatitudeMinuteValue");
            latitudeSeconds = WorkProductUtilities.GetDoubleValueFromFirstElement(workProduct, niemCoreNS + "LatitudeSecondValue");
            longitudeDegrees = WorkProductUtilities.GetIntValueFromFirstElement(workProduct, niemCoreNS + "LongitudeDegreeValue");
            longitudeMinutes = WorkProductUtilities.GetIntValueFromFirstElement(workProduct, niemCoreNS + "LongitudeMinuteValue");
            longitudeSeconds = WorkProductUtilities.GetDoubleValueFromFirstElement(workProduct, niemCoreNS + "LongitudeSecondValue");
            sharedCoreName = WorkProductUtilities.GetStringValueFromFirstElement(workProduct, incNS + "SharedCoreName");
            owningCore = WorkProductUtilities.GetStringValueFromFirstElement(workProduct, incNS + "OwningCore");
        }
    }
}
