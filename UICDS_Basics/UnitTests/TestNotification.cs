using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests
{
    class TestNotification : TestBase
    {
        private const String TESTNAME = "Notification";

        UICDS_Basics.INotificationsService notificationService;

        //static UICDS_Services.NotificationService.NotificationMessageHolderType notification;

        static UICDS_Services.WorkProductService.WorkProduct notification;

        public TestNotification() { }

        public void NotificationHandler(object sender, UICDS_Basics.WorkProductNotificationEventArgs args)
        {
            notification = args.getNotification();
            System.Diagnostics.Debug.WriteLine(notification);
        }



        public void run()
        {
            startTest(TESTNAME);

            //setup();

            notificationService = UICDS_Basics.UICDSProxyFactory.GetNotificationsService();
            UICDS_Basics_UnitTests.assertTrue("incidentService is null", notificationService != null);

            //UICDS_Basics_UnitTests.assertTrue("incident is null", incident != null);

            
            //cleanup();

            endTest(TESTNAME);

        }
    }
}
