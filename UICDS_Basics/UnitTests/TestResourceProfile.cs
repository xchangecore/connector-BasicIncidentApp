using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests
{
    class TestResourceProfile : TestBase
    {
        private const String TESTNAME = "ResourceProfile";

        public UICDS_Basics.IResourceProfileService resourceProfileService;

        public UICDS_Basics.IResourceInstanceService resourceInstanceService;

        public TestResourceProfile() { }

        public void run() {

            setup();

            startTest(TESTNAME);

            UICDS_Basics_UnitTests.assertTrue("server set but shouldn't be", UICDS_Basics.UICDSProxyFactory.isServerConfigurationSet() == false);

            UICDS_Basics.UICDSProxyFactory.CreateServerConfiguration(UICDS_Basics_UnitTests.protocol, UICDS_Basics_UnitTests.server, UICDS_Basics_UnitTests.username, UICDS_Basics_UnitTests.password);
            UICDS_Basics_UnitTests.assertTrue("server should be set", UICDS_Basics.UICDSProxyFactory.isServerConfigurationSet() == true);

            resourceProfileService = UICDS_Basics.UICDSProxyFactory.GetResourceProfileService();
            UICDS_Basics_UnitTests.assertTrue("resourceProfileService is null", resourceProfileService != null);

            UICDS_Basics.IResourceProfile noProfile = resourceProfileService.GetProfile(RESOURCE_PROFILE_ID);
            UICDS_Basics_UnitTests.assertTrue("profile is not null", noProfile == null);

            List<String> interests = new List<String>();
            interests.Add(INCIDENT_WP_TYPE);

            UICDS_Basics.IResourceProfile profile = resourceProfileService.CreateProfile(RESOURCE_PROFILE_ID, interests);
            UICDS_Basics_UnitTests.assertTrue("resourceProfileService is null", profile != null);
            UICDS_Basics_UnitTests.assertTrue("profile id is null", profile.GetID() != null);

            endTest(TESTNAME);

            cleanup();
        }

        private void setup()
        {
        }

        private void cleanup()
        {
            // remove the test profiles and resource instances (assumes the remove methods work)
            resourceProfileService.DeleteProfile(RESOURCE_PROFILE_ID);

            resourceInstanceService = null;
            resourceProfileService = null;
        }
    }

}
