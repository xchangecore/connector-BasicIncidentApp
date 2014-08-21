using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICDS_Basics
{

    ///// <summary>
    ///// delegate declaration for handling work product update notification events.
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="args"></param>
    public delegate void WorkProductNotificationHandler(object sender, WorkProductNotificationEventArgs args);

    /// <summary>
    /// delegate declaration for handling work product deleted notification events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void WorkProductDeletedNotificationHandler(object sender, WorkProductDeletedEventArgs args);

    /// <summary>
    /// Provides methods to register and unregister delegates to handle UICDS notifications and to call 
    /// GetMatchingMessages and GetMessages as .NET Events.  GetIntialUICDSEvents will call GetMatchingMessages
    /// which then triggers Events for each notification returned.  These Events represent the current
    /// state of the UICDS core with respect to each registered delegates profile interests.  GetUICDSEvents
    /// will call GetMessages which will get the current notifications and trigger Events.
    /// </summary>
        ///// <summary>
        ///// delegate declaration for handling work product notification events.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="args"></param>
        //delegate void WorkProductNotificationHandler(object sender, WorkProductNotificationEventArgs args);
    public interface INotificationsService
    {

        /// <summary>
        /// EventHandler for work product update notifications.
        /// </summary>
        event WorkProductNotificationHandler updateNotifications;

        event WorkProductDeletedNotificationHandler deleteNotifications;

        /// <summary>
        /// Call GetMatchingMessages on the UICDS core to get the initial state of the core delivered as events
        /// to each delegate.
        /// </summary>
        List<UICDS_Services.NotificationService.IdentificationType> GetInitialUICDSEvents(String resourceID);

        /// <summary>
        /// Call GetMessages on the UICDS core to deliver notifications as events to each delegate
        /// </summary>
        void GetUICDSEvents(String resourceEndpoint);

        /// <summary>
        /// Translate a Notification Service Work Product Identifier into a Work Product Service Identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UICDS_Services.WorkProductService.IdentificationType GetWorkProductServiceID(UICDS_Services.NotificationService.IdentificationType id);
    }
}
