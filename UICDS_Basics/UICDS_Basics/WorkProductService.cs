using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Net;
using System.Xml.Serialization;

namespace UICDS_Basics
{
    class WorkProductService : ServiceProxy, IWorkProductService
    {
        private static XmlSerializer workProductSerializer;

        private UICDS_Services.WorkProductService.WorkProductServiceService service;

        public WorkProductService(ServerConfiguration serverConfig)
        {
            this.SetCredentials(serverConfig.getCredentials());
            service = new UICDS_Services.WorkProductService.WorkProductServiceService();
            service.Credentials = credentials;

            service.Url = getServiceUrl(serverConfig, service.Url);
        }


        UICDS_Services.WorkProductService.WorkProduct IWorkProductService.GetWorkProduct(UICDS_Services.WorkProductService.IdentificationType workProductIdentification)
        {
            UICDS_Services.WorkProductService.GetProductRequest request = new UICDS_Services.WorkProductService.GetProductRequest();
            request.WorkProductIdentification = workProductIdentification;

            try
            {
                UICDS_Services.WorkProductService.GetProductResponse response = service.GetProduct(request);
                return response.WorkProduct;

            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("ResourceInstanceService:Register", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("ResourceInstanceService:Register", webEx);
            }

            return null;
        }

        void IWorkProductService.GetAssociatedWorkProducts(string interestGroupID)
        {
            throw new NotImplementedException();
        }

        public static XmlSerializer GetWorkProductSerializer()
        {

            if (workProductSerializer == null)
            {
                // Create the XmlAttributes and XmlAttributeOverrides objects.
                XmlAttributes attrs = new XmlAttributes();
                XmlAttributeOverrides xOver = new XmlAttributeOverrides();

                XmlRootAttribute xRoot = new XmlRootAttribute();

                // Set a new Namespace and ElementName for the root element.
                xRoot.Namespace = "http://www.saic.com/precis/2009/06/structures";
                xRoot.ElementName = "WorkProduct";
                attrs.XmlRoot = xRoot;

                /* Add the XmlAttributes object to the XmlAttributeOverrides. 
                   No  member name is needed because the whole class is 
                   overridden. */
                xOver.Add(typeof(UICDS_Services.WorkProductService.WorkProduct), attrs);

                workProductSerializer = new XmlSerializer(typeof(UICDS_Services.WorkProductService.WorkProduct), xOver);
            }

            return workProductSerializer;
        }

    }
}
