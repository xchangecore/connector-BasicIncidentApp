using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICDS_Basics
{
    public class UICDSProxyFactory
    {
        private static ServerConfiguration serverConfiguration;

        private static IResourceInstanceService resourceInstanceService;

        private static IResourceProfileService resourceProfileService;

        private static IIncidentManagementService incidentManagementService;

        private static INotificationsService notificationService;

        private static IWorkProductService workProductService;

        public static void CreateServerConfiguration(String protocol, String server, String username, String password) {
            serverConfiguration = new ServerConfiguration(protocol, server, username, password);
            System.Net.ServicePointManager.Expect100Continue = false;
        }

        public static bool isServerConfigurationSet()
        {
            return (serverConfiguration != null);
        }

        public static IResourceInstanceService GetResourceInstanceService()
        {
            if (resourceInstanceService == null)
            {
                resourceInstanceService = new ResourceInstanceService(serverConfiguration);
            }
            return resourceInstanceService;
        }

        public static IResourceProfileService GetResourceProfileService()
        {
            if (resourceProfileService == null) {
                resourceProfileService = new ResourceProfileService(serverConfiguration);
            }
            return resourceProfileService;
        }

        public static IIncidentManagementService GetIncidentManagementService()
        {
            if (incidentManagementService == null)
            {
                incidentManagementService = new IncidentManagementService(serverConfiguration);
            }
            return incidentManagementService;
        }

        public static INotificationsService GetNotificationsService()
        {
            if (notificationService == null)
            {
                notificationService = new NotificationService(serverConfiguration);
            }
            return notificationService;
        }

        public static IWorkProductService GetWorkProductService()
        {
            if (workProductService == null)
            {
                workProductService = new WorkProductService(serverConfiguration);
            }
            return workProductService;
        }
    }
}
