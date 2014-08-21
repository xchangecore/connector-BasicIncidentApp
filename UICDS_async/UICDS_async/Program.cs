using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;

namespace UICDS_async
{
    // Example of Create, Update, Close, and Archive of incidents on a UICDS core
    class Program
    {

        internal class AcceptAllCertificatePolicy : ICertificatePolicy
        {

            public AcceptAllCertificatePolicy()
            {
            }

            public bool CheckValidationResult(ServicePoint sPoint,
               X509Certificate cert, WebRequest wRequest, int certProb)
            {
                // Always accept

                return true;
            }
        }

        private static String host = "https://localhost/uicds/core/ws/services/";
        private static String workProductServiceURL = host + "WorkProductService";
        private static String incidentManagementServiceURL = host + "IncidentManagementService";
        private static String resourceInstanceServiceURL = host + "ResourceInstanceService";
        private static String resourceProfileServiceURL = host + "ResourceProfileService";
        private static String notificationServiceURL = host + "NotificationService";
        private static String incidentCommandServiceURL = host + "IncidentCommandService";
        private static String directoryServiceURL = host + "DirectoryService";
        private static String mapServiceURL = host + "MapService";

        // A unique label for this application on this core
        private static String applicationID = "dotNetApplication";

        // A label that the local application may be known as
        private static String localID = "Incident Application #1";

        // The UICDS resource profile that will be applied to this application when it registers
        // with the core.  The resource profile maintains a list of interests for work product 
        // notifications.  For this client to function correctly this profile should have 
        // interests in Incident, ICS, and MapViewContext work products.
        private static String applicationProfileID = "DotNetClient";

        static void Main(string[] args)
        {
            // Credentials to access the core
            String username = "user1";
            String password = "user1";

            // Accept all certificates since we are using a self-signed right now
            ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();

            // Create a proxy for the work product service
            WorkProductService workProductServiceProxy = CreateWorkProductService(workProductServiceURL, username, password);

            // Create a proxy for the incident management service
            IncidentManagementService incidentManagementService =
                CreateIncidentManagementServiceProxy(incidentManagementServiceURL, username, password, workProductServiceProxy);

            // Create a proxy for the resource instance service
            ResourceInstanceService resourceInstanceService = 
                CreateResourceInstanceServiceProxy(resourceInstanceServiceURL, username, password);

            // Create a proxy for the resource profile service
            ResourceProfileService resourceProfileService =
                CreateResourceProfileServiceProxy(resourceProfileServiceURL, username, password);

            // Create a proxy for the notification service
            NotificationService notificationService = 
                CreateNotificationServiceProxy(notificationServiceURL, username, password);

            // Create a proxy for the Incident Command service
            IncidentCommandService incidentCommandService =
                CreateIncidentCommandServiceProxy(incidentCommandServiceURL, username, password, workProductServiceProxy);

            // Create a proxy for the Incident Command service
            DirectoryService directoryService =
                CreateDirectoryServiceProxy(directoryServiceURL, username, password);

            // Create a proxy for the Map service
            MapService mapService =
                CreateMapServiceProxy(directoryServiceURL, username, password, workProductServiceProxy);


            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("ESN", "123");
            data.Add("Destination", "Warehouse 3");
            data.Add("latitude", "37.92");
            data.Add("longitude", "-77.09");
            Feature feature = new Feature("title", "id", "37.92", "-77.09", "2009-04-15T12:00:00GMT", data);
            //System.Diagnostics.Debug.WriteLine(feature.GetWorkProduct());

            if (workProductServiceProxy.publishWorkProduct(feature))
            {
                System.Diagnostics.Debug.WriteLine("Feature was published");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Feature publish failed");
                System.Environment.Exit(1);
            }

            data["latitude"] = "37.95";
            feature.updateFeature("title", "id", "37.95", "-77.09", "2009-04-15T12:00:00GMT", data);
            if (workProductServiceProxy.publishWorkProduct(feature))
            {
                System.Diagnostics.Debug.WriteLine("Feature was published");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Feature publish failed");
                System.Environment.Exit(1);
            }


            // Get the resource profile (create if it doesn't exist)
            List<string> interests = new List<string>();
            interests.Add("Incident");
            interests.Add("ICS");
            interests.Add("MapViewContext");
            bool profileExists = resourceProfileService.CreateOrGetProfile(applicationProfileID, interests);
            if (!profileExists)
            {
                // Leave if we can't get our resource from the core
                System.Diagnostics.Debug.WriteLine("Cannot get or create resource profile " + applicationProfileID);
                return;
            }


            // Register our application with UICDS core
            ResourceInstance resourceInstance = null;
            if (!resourceInstanceService.RegisterApplication(applicationID, localID, applicationProfileID))
            {
                // Leave if we can't regster with the core
                System.Diagnostics.Debug.WriteLine("Cannot register with the core to get notifications");
                return;
            }
            else
            {
                // Get our resource instance from the core
                resourceInstance = resourceInstanceService.GetResourceInstance(applicationID);
                if (resourceInstance == null)
                {
                    // Leave if we can't get our resource from the core
                    System.Diagnostics.Debug.WriteLine("Cannot get resource instance " + applicationID);
                    return;
                }
            }

            // Clear out any left over notifications
            ClearNotifications(5, notificationService, resourceInstance);

            Dictionary<string, SpatialIncident> incidents = incidentManagementService.GetActiveIncidents();

            System.Diagnostics.Debug.WriteLine("Incident count: " + incidents.Count());


            // Get a list of current incidents and list them
            //List<IncidentDigest> incidentList = directoryService.GetIncidentList();
            List<IncidentDigest> incidentList = incidentManagementService.GetIncidentList();
            IEnumerator<IncidentDigest> digests = incidentList.GetEnumerator();
            List<Incident> currentIncidents = new List<Incident>();
            while (digests.MoveNext())
            {
                if (digests.Current != null)
                {
                    Incident currentIncident = new Incident(workProductServiceProxy, digests.Current.incidentID);
                    currentIncidents.Add(currentIncident);
                    System.Diagnostics.Debug.WriteLine("Incident: " + digests.Current.type + " " +  digests.Current.name + " at " + digests.Current.latitude + "," + digests.Current.longitude);
                }
            }

            // Set the incident commander of one of the current incidents
            UpdateIncidentCommmander(currentIncidents, incidentCommandService);

            // Create a local representation of the incident
            Incident incident = createIncident();

            // Create the incident on the core
            incidentManagementService.CreateIncident(incident);
            PollForNotifications(5, incident, resourceInstance, notificationService, incidentManagementService, workProductServiceProxy);


            // If the incident was opened then continue else exit with error
            if (incident.isOpen())
            {

                // Update the local representation of the incident
                incident.description = "The flood on the river has covered downtown";

                // Post the update to the core
                incidentManagementService.UpdateIncident(incident);

                // Process notifications from the core until we run out of run time
                int pollingSeconds = 5;
                PollForNotifications(pollingSeconds, incident, resourceInstance, notificationService, incidentManagementService, workProductServiceProxy);

                incident.name = "Big Flood";
                incidentManagementService.UpdateIncident(incident);
                PollForNotifications(pollingSeconds, incident, resourceInstance, notificationService, incidentManagementService, workProductServiceProxy);

                // Add a layer to the map
                System.Diagnostics.Debug.WriteLine("Adding a layer to the map");
                AddMapLayer(incident, mapService);
                PollForNotifications(pollingSeconds, incident, resourceInstance, notificationService, incidentManagementService, workProductServiceProxy);

                // Update the ICS
                System.Diagnostics.Debug.WriteLine("Updating the ICS");
                UpdateICS(incident, incidentCommandService);
                PollForNotifications(pollingSeconds, incident, resourceInstance, notificationService, incidentManagementService, workProductServiceProxy);

                // Update the ICS
                System.Diagnostics.Debug.WriteLine("Updating the ICS by removing the Fire Engine");
                RemoveFireEngineFromICS(incident, incidentCommandService);
                PollForNotifications(pollingSeconds, incident, resourceInstance, notificationService, incidentManagementService, workProductServiceProxy);

                // Associate the features
                workProductServiceProxy.AssociateWorkProductToIncident(feature, incident.GetIncidentID());

                // Close the incident
                incidentManagementService.CloseIncident(incident);
                PollForNotifications(pollingSeconds, incident, resourceInstance, notificationService, incidentManagementService, workProductServiceProxy);

                // Archive the incident (remove it from the core)
                incidentManagementService.ArchiveIncident(incident);
                // PollForNotifications(3, incident, resourceInstance, notificationService, incidentManagementService, workProductServiceProxy);


                // Process notifications to get the close and archive messages
                IEnumerable<XElement> finalNotifications = notificationService.GetMessages(resourceInstance);
                System.Diagnostics.Debug.WriteLine("Got " + finalNotifications.Count() + " notifications");
                // ProcessNotifications(incident, finalNotifications, notificationService, incidentManagementService, workProductServiceProxy);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ERROR: incident was not created");
            }
        }

        private static void UpdateIncidentCommmander(List<Incident> currentIncidents, IncidentCommandService incidentCommandService)
        {
            if (currentIncidents.Count() > 0)
            {
                Incident incident = currentIncidents.ElementAt(0);
                IncidentCommandStructure ics = incident.GetAssociatedWorkProduct("ICS") as IncidentCommandStructure;
                if (ics != null)
                {
                    try
                    {
                        ics.UpdateOrganizationPersonInCharge("Incident Commander", "Incident Commander", "Incident Commander", "Sally Commander");
                        incidentCommandService.UpdateICS(ics);
                    }
                    catch (ArgumentException ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error adding to ICS: " + ex.Message);
                    }
                    catch (NullReferenceException ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error adding to ICS: " + ex.Message);
                    }
                }
            }
        }

        private static void UpdateICS(Incident incident, IncidentCommandService incidentCommandService)
        {
            WorkProduct workProduct = incident.GetAssociatedWorkProduct(IncidentCommandStructure.TYPE);
            if (workProduct != null)
            {
                IncidentCommandStructure ics = workProduct as IncidentCommandStructure;
                if (ics != null)
                {
                    ics.SetIncidentCommander("Chief McGillicutty");
                    try
                    {
                        ics.AddSubordinateOrganziationElement("Incident Commander", "Incident Commander", "Operations", "Section", "Operation Section Chief", "Joe Ops");
                        ics.AddSubordinateOrganziationElement("Incident Commander", "Incident Commander", "Planning", "Section", "Planning Section Chief", "Joe Planner");
                        ics.AddSubordinateOrganziationElement("Incident Commander", "Incident Commander", "Logistics", "Section", "Logistics Section Chief", "Joe Log");
                        ics.AddSubordinateOrganziationElement("Operations", "Section", "Fire Suppression", "Branch", "Fire Chief", "Joe Fire");
                        ics.AddSubordinateOrganziationElement("Fire Suppression", "Branch", "Engine Units", "Group", "Unit Commander", "Joe Fire Suppression");
                        ics.AddSubordinateOrganziationElement("Engine Units", "Group", "Engine 7", "Resource", "Engine Commander", "Joe Engine");
                        ics.AddStaff("Engine 7", "Resource", "Engine Commander", "Joe Engine");
                        ics.AddStaff("Engine 7", "Resource", "Fireman", "Joe Fireman");
                    }
                    catch (ArgumentException ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error adding to ICS: " + ex.Message);
                    }

                    incidentCommandService.UpdateICS(ics);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("WorkProduct found is not of type IncidentCommandStructure: " + workProduct.type);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No ICS work product available to update");
            }
        }

        private static void RemoveFireEngineFromICS(Incident incident, IncidentCommandService incidentCommandService)
        {
            WorkProduct workProduct = incident.GetAssociatedWorkProduct(IncidentCommandStructure.TYPE);
            if (workProduct != null)
            {
                IncidentCommandStructure ics = workProduct as IncidentCommandStructure;
                if (ics != null)
                {
                    try
                    {
                        ics.RemoveOrganizationElement("Engine 7", "Resource");
                    }
                    catch (ArgumentException ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error removing organization element: " + ex.Message);
                    }

                    incidentCommandService.UpdateICS(ics);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("WorkProduct found is not of type IncidentCommandStructure: " + workProduct.type);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No ICS work product available to update");
            }
        }

        private static void AddMapLayer(Incident incident, MapService mapService)
        {
            WorkProduct workProduct = incident.GetAssociatedWorkProduct(MapViewContext.TYPE);
            if (workProduct != null)
            {
                MapViewContext map = workProduct as MapViewContext;
                if (map != null)
                {
                    try
                    {
                        XElement layer = LayerViewContext.CreateLayerElement("Service Title", "OGC:WMS", "1.1.0", "http://wms.server.map", "LayerName", "New Layer Title", "EPSG:4326");

                        map.AddLayer(layer);

                        // Add the new layer as a standalone Layer work product also (is not required)
                        mapService.AddLayerWorkProduct(layer, incident.GetIncidentID());
                    }
                    catch (ArgumentException ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error adding layer work product: " + ex.Message);
                    }

                    mapService.UpdateMap(map);

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("WorkProduct found is not of type MapViewContext: " + workProduct.type);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No Map work product available to update");
            }
        }

        private static void ClearNotifications(int seconds, NotificationService notificationService, ResourceInstance resourceInstance)
        {
            int count = 0;
            while (count < seconds)
            {
                try
                {
                    System.Threading.Thread.Sleep(1000);
                    count++;
                    IEnumerable<XElement> notifications = notificationService.GetMessages(resourceInstance);
                }
                catch (System.Threading.ThreadInterruptedException)
                {
                    break;
                }
            }
        }

        private static void PollForNotifications(int seconds, Incident incident, ResourceInstance resourceInstance,
            NotificationService notificationService, IncidentManagementService incidentManagementService,
            WorkProductService workProductService)
        {
                int count = 0;
            while (count < seconds)
                {
                    try
                    {
                        System.Threading.Thread.Sleep(1000);
                        count++;
                        IEnumerable<XElement> notifications = notificationService.GetMessages(resourceInstance);
                        System.Diagnostics.Debug.WriteLine("Got " + notifications.Count() + " notifications");
                    ProcessNotifications(incident, notifications, notificationService, incidentManagementService, workProductService);
                    }
                    catch (System.Threading.ThreadInterruptedException)
                    {
                        break;
                    }
                }
        }

        private static void ProcessNotifications(Incident incident, IEnumerable<XElement> notifications,
            NotificationService notificationService, IncidentManagementService incidentManagementService,
            WorkProductService workProductService)
        {
            IEnumerator<XElement> enumerator = notifications.GetEnumerator();
            while (enumerator.MoveNext())
            {
                // Get the work product identification from the notification 
                XElement workProductIdentification = notificationService.getWorkProductIdentification(enumerator.Current);

                // If the work product identification is not null then process it
                if (workProductIdentification != null)
                {
                    // Get the full work product and update the appropriate object
                    WorkProduct wp = workProductService.GetWorkProduct(workProductIdentification);
                    if (wp != null && wp.type != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Notification for work product of type: " + wp.type);
                    }
                    if (wp != null)
                    {
                        // At this point a real implementation would find the correct object to handle this 
                        // updated work product based on its type and/or the incident it is associated with.
                        // But this is just an example so we know it is for our only incident
                        if (wp.type.Equals("Incident"))
                        {
                            incident.updateFromWorkProduct(wp);
                        }
                        else
                        {
                            incident.processAssociatedWorkProduct(wp);
                        }
                    }
                }
                else
                {
                    XElement workProductDeleted = notificationService.getWorkProductDeleted(enumerator.Current);
                    if (workProductDeleted != null)
                    {
                        incident.archiveIncident();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("UNKNOWN Notification type");
                    }
                }
            }
        }

        private static Incident createIncident()
        {
            Incident incident = new Incident();
            incident.name = "Flood";
            incident.dateTime = "2009-10-14T09:55:22";
            incident.description = "A flood on the river";
            incident.latitudeDegrees = 38;
            incident.latitudeMinutes = 17;
            incident.latitudeSeconds = 50.00;
            incident.longitudeDegrees = -77;
            incident.longitudeMinutes = 17;
            incident.longitudeSeconds = 50.00;
            incident.type = "Env";
            return incident;
        }

        private static WorkProductService CreateWorkProductService(String serviceEndpoint, String username, String password)
        {
            WorkProductService serviceProxy = new WorkProductService();
            NetworkCredential credentials = new NetworkCredential(username, password);

            serviceProxy.credentials = credentials;
            serviceProxy.endpoint = serviceEndpoint;
            return serviceProxy;
        }

        private static IncidentManagementService CreateIncidentManagementServiceProxy(String serviceEndpoint, 
            String username, String password, WorkProductService workProductService)
        {
            IncidentManagementService serviceProxy = new IncidentManagementService(workProductService);
            NetworkCredential credentials = new NetworkCredential(username, password);

            serviceProxy.credentials = credentials;
            serviceProxy.endpoint = serviceEndpoint;
            return serviceProxy;
        }

        private static ResourceInstanceService CreateResourceInstanceServiceProxy(String serviceEndpoint,
            String username, String password)
        {
            ResourceInstanceService serviceProxy = new ResourceInstanceService();
            NetworkCredential credentials = new NetworkCredential(username, password);

            serviceProxy.credentials = credentials;
            serviceProxy.endpoint = serviceEndpoint;
            return serviceProxy;
        }

        private static ResourceProfileService CreateResourceProfileServiceProxy(String serviceEndpoint,
            String username, String password)
        {
            ResourceProfileService serviceProxy = new ResourceProfileService();
            NetworkCredential credentials = new NetworkCredential(username, password);

            serviceProxy.credentials = credentials;
            serviceProxy.endpoint = serviceEndpoint;
            return serviceProxy;
        }

        private static NotificationService CreateNotificationServiceProxy(String serviceEndpoint,
            String username, String password)
        {
            NotificationService serviceProxy = new NotificationService();
            NetworkCredential credentials = new NetworkCredential(username, password);

            serviceProxy.credentials = credentials;
            serviceProxy.endpoint = serviceEndpoint;
            return serviceProxy;
        }

        private static IncidentCommandService CreateIncidentCommandServiceProxy(String serviceEndpoint,
            String username, String password, WorkProductService workProductService)
        {
            IncidentCommandService serviceProxy = new IncidentCommandService(workProductService);
            NetworkCredential credentials = new NetworkCredential(username, password);

            serviceProxy.credentials = credentials;
            serviceProxy.endpoint = serviceEndpoint;
            return serviceProxy;
        }

        private static DirectoryService CreateDirectoryServiceProxy(string serviceEndpoint, string username, string password)
        {
            DirectoryService serviceProxy = new DirectoryService();
            NetworkCredential credentials = new NetworkCredential(username, password);

            serviceProxy.credentials = credentials;
            serviceProxy.endpoint = serviceEndpoint;
            return serviceProxy;
        }

        private static MapService CreateMapServiceProxy(string serviceEndpoint, string username, string password,
             WorkProductService workProductService)
        {
            MapService serviceProxy = new MapService(workProductService);
            NetworkCredential credentials = new NetworkCredential(username, password);

            serviceProxy.credentials = credentials;
            serviceProxy.endpoint = serviceEndpoint;
            return serviceProxy;
        }

    }
}
