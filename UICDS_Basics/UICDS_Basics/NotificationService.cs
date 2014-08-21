using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace UICDS_Basics
{
    public class NotificationService : ServiceProxy, INotificationsService
    {

        private static XmlSerializer workProductDeletedSerializer;

        private UICDS_Services.NotificationService.NotificationServiceService service;

        public event WorkProductNotificationHandler updateNotifications;

        public event WorkProductDeletedNotificationHandler deleteNotifications;

        public NotificationService(ServerConfiguration serverConfig)
        {
            this.SetCredentials(serverConfig.getCredentials());
            service = new UICDS_Services.NotificationService.NotificationServiceService();
            service.Credentials = credentials;

            service.Url = getServiceUrl(serverConfig, service.Url);
        }

        public List<UICDS_Services.NotificationService.IdentificationType> GetInitialUICDSEvents(String resourceID)
        {
            UICDS_Services.NotificationService.GetMatchingMessagesRequest request = new UICDS_Services.NotificationService.GetMatchingMessagesRequest();
            request.ID = new UICDS_Services.NotificationService.IdentifierType();
            request.ID.Value = resourceID;

            try
            {
                UICDS_Services.NotificationService.GetMatchingMessagesResponse response = service.GetMatchingMessages(request);

                IEnumerable<UICDS_Services.NotificationService.IdentificationType> notificationList = response.WorkProductIdentificationList.AsEnumerable<UICDS_Services.NotificationService.IdentificationType>();
                return notificationList.ToList<UICDS_Services.NotificationService.IdentificationType>();
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("NotificationService:GetMatchingMessages", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("NotificationService:GetMatchingMessages", webEx);
            }

            return null;
        }

        public void GetUICDSEvents(String resourceEndpoint)
        {
            UICDS_Services.NotificationService.GetMessagesRequest request = new UICDS_Services.NotificationService.GetMessagesRequest();
            request.MaximumNumber = "1";
            request.Any = new XmlElement[1];
            XmlDocument doc = new XmlDocument();
            XmlElement endpoint = doc.CreateElement("wsa","To","http://www.w3.org/2005/08/addressing");
            endpoint.AppendChild(doc.CreateTextNode(resourceEndpoint));
            request.Any[0] = endpoint;

            try
            {
                UICDS_Services.NotificationService.GetMessagesResponse response = service.GetMessages(request);

                IEnumerable<UICDS_Services.NotificationService.NotificationMessageHolderType> notificationList = 
                    response.NotificationMessage.AsEnumerable<UICDS_Services.NotificationService.NotificationMessageHolderType>();
                if (notificationList != null)
                {
                    foreach (UICDS_Services.NotificationService.NotificationMessageHolderType notification in notificationList)
                    {
                        if (notification.Message.HasChildNodes)
                        {
                            if (notification.Message.NamespaceURI.Equals("http://www.saic.com/precis/2009/06/structures") &&
                                notification.Message.LocalName.Equals("WorkProduct"))
                            {
                                WorkProductNotificationEventArgs args = new WorkProductNotificationEventArgs(notification);
                                if (updateNotifications != null)
                                {
                                    updateNotifications(this, args);
                                }
                            }
                            else if (notification.Message.NamespaceURI.Equals("http://uicds.org/NotificationService") &&
                                notification.Message.LocalName.Equals("WorkProductDeletedNotification"))
                            {
                                WorkProductDeletedEventArgs args = new WorkProductDeletedEventArgs(notification);
                                if (deleteNotifications != null)
                                {
                                    deleteNotifications(this, args);
                                }
                            }
                        }
                    }
                }
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("NotificationService:GetMessages", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("NotificationService:GetMessages", webEx);
            }

        }

        UICDS_Services.WorkProductService.IdentificationType INotificationsService.GetWorkProductServiceID(UICDS_Services.NotificationService.IdentificationType id)
        {
            UICDS_Services.WorkProductService.IdentificationType wpID = new UICDS_Services.WorkProductService.IdentificationType();
            wpID.Checksum = new UICDS_Services.WorkProductService.ChecksumType();
            wpID.Checksum.Value = new String(id.Checksum.Value.ToCharArray());

            wpID.Identifier = new UICDS_Services.WorkProductService.IdentifierType();
            wpID.Identifier.Value = new String(id.Identifier.Value.ToCharArray());

            wpID.State = new UICDS_Services.WorkProductService.StateType();
            wpID.State = UICDS_Services.WorkProductService.StateType.Active;

            wpID.Type = new UICDS_Services.WorkProductService.CodespaceValueType();
            wpID.Type.Value = new String(id.Type.Value.ToCharArray());

            wpID.Version = new UICDS_Services.WorkProductService.VersionType();
            wpID.Version.Value = new String(id.Version.Value.ToCharArray());

            return wpID;
        }

        public static XmlSerializer GetWorkProductDeletedSerializer()
        {
            if (workProductDeletedSerializer == null)
            {
                // Create the XmlAttributes and XmlAttributeOverrides objects.
                XmlAttributes attrs = new XmlAttributes();
                XmlAttributeOverrides xOver = new XmlAttributeOverrides();

                XmlRootAttribute xRoot = new XmlRootAttribute();

                // Set a new Namespace and ElementName for the root element.
                xRoot.Namespace = "http://uicds.org/NotificationService";
                xRoot.ElementName = "WorkProductDeletedNotification";
                attrs.XmlRoot = xRoot;

                /* Add the XmlAttributes object to the XmlAttributeOverrides. 
                   No  member name is needed because the whole class is 
                   overridden. */
                xOver.Add(typeof(UICDS_Services.NotificationService.WorkProductDeletedNotification), attrs);

                workProductDeletedSerializer = new XmlSerializer(typeof(UICDS_Services.NotificationService.WorkProductDeletedNotification), xOver);
            }

            return workProductDeletedSerializer;
        }


    }
}
