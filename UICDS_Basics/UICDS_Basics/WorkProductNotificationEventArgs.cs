using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace UICDS_Basics
{
    public class WorkProductNotificationEventArgs
    {
        private UICDS_Services.WorkProductService.WorkProduct workProduct;

        public WorkProductNotificationEventArgs(UICDS_Services.NotificationService.NotificationMessageHolderType notification)
        {
            MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(notification.Message.OuterXml));
            try
            {
                this.workProduct = (UICDS_Services.WorkProductService.WorkProduct)WorkProductService.GetWorkProductSerializer().Deserialize(memStream);
            }
            catch (InvalidOperationException e)
            {
                this.workProduct = null;
            }
        }

        public UICDS_Services.WorkProductService.WorkProduct getNotification()
        {
            return workProduct;
        }

    }
}
