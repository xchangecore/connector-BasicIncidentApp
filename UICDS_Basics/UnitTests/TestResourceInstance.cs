using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;

namespace UnitTests
{
    class TestResourceInstance : TestBase
    {
        private const String TESTNAME = "ResourceInstance";

        public UICDS_Basics.IResourceProfileService resourceProfileService;

        public UICDS_Basics.IResourceInstanceService resourceInstanceService;

        public UICDS_Basics.INotificationsService notificationService;

        public TestResourceInstance() { }

        public void run()
        {

            startTest(TESTNAME);

            setup();

            resourceInstanceService = UICDS_Basics.UICDSProxyFactory.GetResourceInstanceService();
            UICDS_Basics_UnitTests.assertTrue("resourceInstanceService is null", resourceInstanceService != null);

            UICDS_Basics.IResourceInstance noResource = resourceInstanceService.GetResourceInstance(RESOURCE_ID);
            UICDS_Basics_UnitTests.assertTrue("noResource is not null", noResource == null);

            UICDS_Basics.IResourceInstance resource = resourceInstanceService.RegisterResourceInstance(RESOURCE_ID, RESOURCE_PROFILE_ID);
            UICDS_Basics_UnitTests.assertTrue("resource is null", resource != null);
            UICDS_Basics_UnitTests.assertTrue("resource id is null", resource.GetResourceInstanceID() != null);
            if (resource.GetResourceInstanceID() != null)
            {
                UICDS_Basics_UnitTests.assertTrue("", resource.GetResourceInstanceID().Equals(RESOURCE_ID));
            }

            String endpoint = resource.GetResourceInstanceEndpoint();
            notificationService.GetUICDSEvents(endpoint);

            resourceInstanceService.UnregisterResourceInstance(RESOURCE_ID);

            cleanup();

            endTest(TESTNAME);
        }

        private void setup()
        {
            resourceProfileService = UICDS_Basics.UICDSProxyFactory.GetResourceProfileService();
            UICDS_Basics_UnitTests.assertTrue("resourceProfileService is null", resourceProfileService != null);

            UICDS_Basics.IResourceProfile noProfile = resourceProfileService.GetProfile(RESOURCE_PROFILE_ID);
            UICDS_Basics_UnitTests.assertTrue("profile is not null", noProfile == null);

            List<String> interests = new List<String>();
            interests.Add(INCIDENT_WP_TYPE);

            UICDS_Basics.IResourceProfile profile = resourceProfileService.CreateProfile(RESOURCE_PROFILE_ID, interests);
            UICDS_Basics_UnitTests.assertTrue("resourceProfileService is null", profile != null);
            UICDS_Basics_UnitTests.assertTrue("profile id is null", profile.GetID() != null);

            notificationService = UICDS_Basics.UICDSProxyFactory.GetNotificationsService();
            UICDS_Basics_UnitTests.assertTrue("notificationService is null", notificationService != null);
        }

        private void cleanup()
        {
            // remove the test profiles and resource instances (assumes the remove methods work)
            resourceProfileService.DeleteProfile(RESOURCE_PROFILE_ID);
            try {
            resourceInstanceService.UnregisterResourceInstance(RESOURCE_ID);
            }
            catch (SoapHeaderException soapEx)
            {
                System.Diagnostics.Debug.WriteLine("ResourceInstanceService:Unregister:" + soapEx.Message);
            }

            resourceInstanceService = null;
            resourceProfileService = null;
        }

    }
}
