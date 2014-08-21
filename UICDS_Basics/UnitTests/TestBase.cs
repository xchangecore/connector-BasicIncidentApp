using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests
{
    class TestBase
    {
        public const String RESOURCE_ID = "testing";
        public const String RESOURCE_PROFILE_ID = "testProfile";
        public const String INCIDENT_WP_TYPE = "Incident";

        public const String INCIDENT1_NAME = "Incident1";
        public const String INCIDENT1_DESC = "Description1";
        public const double INCIDENT1_LATITUDE = 38.7;
        public const double INCIDENT1_LONGITUDE = -76.5;
        public DateTime INCIDENT1_DATE = System.DateTime.Now;
        public const String INCIDENT1_TYPE = "Traffic";

        public void startTest(String testName)
        {
            UICDS_Basics_UnitTests.info(testName);
        }

        public void endTest(String testName)
        {
            UICDS_Basics_UnitTests.info("END Test: " + testName+ " - " + (UICDS_Basics_UnitTests.failed ? "FAIL" : "PASS"));
            UICDS_Basics_UnitTests.failed = false;
        }
    }
}
