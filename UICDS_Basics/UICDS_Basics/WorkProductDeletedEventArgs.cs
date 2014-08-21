using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UICDS_Basics
{
    public class WorkProductDeletedEventArgs
    {
        private UICDS_Services.NotificationService.WorkProductDeletedNotification workProductDeletedNotification;

        public WorkProductDeletedEventArgs(UICDS_Services.NotificationService.NotificationMessageHolderType notification)
        {

            MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(notification.Message.OuterXml));
            try
            {
                this.workProductDeletedNotification = (UICDS_Services.NotificationService.WorkProductDeletedNotification)
                    NotificationService.GetWorkProductDeletedSerializer().Deserialize(memStream);
            }
            catch (InvalidOperationException e)
            {
                this.workProductDeletedNotification = null;
                System.Diagnostics.Debug.WriteLine("Error deserializing work product deleted notification: " + e.Message);
                System.Diagnostics.Debug.WriteLine("Error deserializing work product deleted notification: " + e.InnerException.Message);
                System.Diagnostics.Debug.WriteLine(notification.Message.OuterXml);
            }
        }

        public UICDS_Services.NotificationService.WorkProductDeletedNotification getNotification()
        {
            return workProductDeletedNotification;
        }
    }
}
