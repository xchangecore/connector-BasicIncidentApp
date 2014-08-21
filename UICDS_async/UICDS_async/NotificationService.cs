using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UICDS_async
{
    class NotificationService : ServiceProxy
    {
        // Get messages for the input resource instance
        public IEnumerable<XElement> GetMessages(ResourceInstance resourceInstance)
        {
            XElement request = GetMessagesRequest(resourceInstance.endpoint);
//            System.Diagnostics.Debug.WriteLine(request.ToString());
            String response = POST(WrapInSOAP(request).ToString());
            XElement responseXML = XElement.Parse(response);
//            System.Diagnostics.Debug.WriteLine(response);
            IEnumerable<XElement> messages = responseXML.Descendants(notificationNS + "NotificationMessage");
            return messages;
        }


        public XElement GetMessagesRequest(String endpoint)
        {
            // Create a request for a work product
            XElement getMessagesRequest = new XElement(notificationNS + "GetMessagesRequest",
                new XElement(notificationNS + "MaximumNumber", "1"),
                new XElement(wsAddressingNS + "To", endpoint)
            );

            return getMessagesRequest;
        }

        public XElement getWorkProductIdentification(XElement notificationMessage)
        {
            IEnumerable<XElement> elements = notificationMessage.Descendants(ServiceProxy.precissNS + "WorkProductIdentification");
            if (elements.Count() > 0)
            {
                return elements.ElementAt(0);
            }
            return null;
        }


        internal XElement getWorkProductDeleted(XElement notificationMessage)
        {
            IEnumerable<XElement> elements = notificationMessage.Descendants(ServiceProxy.notificationNS + "WorkProductDeletedNotification");
            if (elements.Count() > 0)
            {
                return elements.ElementAt(0);
            }
            return null;
        }
    }
}
