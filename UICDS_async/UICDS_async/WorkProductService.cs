using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Net;

namespace UICDS_async
{
    class WorkProductService : ServiceProxy
    {

        // Get a full work product from the UICDS core
        public WorkProduct GetWorkProduct(XElement workProductIndentification)
        {
            // Create a request for a work product
            XElement getWorkProductRequest = new XElement(workProductNS + "GetProductRequest",
                workProductIndentification
            );

            // Wrap request in SOAP
            XElement requestElement = WrapInSOAP(getWorkProductRequest);

            // Make the request
            WorkProduct workProduct = null;
            try
            {
                String response = POST(requestElement.ToString());

                // Parse the resuls
                XElement responseXML = XElement.Parse(response);

                // Create an object to represent the work product
                workProduct = new WorkProduct(responseXML);
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("ArchiveIncident exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                }
            }

            return workProduct;
        }

        public Boolean publishWorkProduct(WorkProduct workProduct)
        {
            Boolean ok = false;

            // Create a request for a work product
            XElement publishWorkProductRequest = new XElement(workProductNS + "PublishProductRequest",
                workProduct.GetWorkProduct()
            );

            System.Diagnostics.Debug.WriteLine(publishWorkProductRequest.ToString());

            // Wrap request in SOAP
            XElement requestElement = WrapInSOAP(publishWorkProductRequest);

            try
            {
                String response = POST(requestElement.ToString());

                // Process the results
                bool accepted = workProduct.processWorkProductProcessingResponse(response);
                if (accepted)
                {
                    ok = true;
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("PublishProductRequest exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                }
            }

            return ok;
        }

        public void AssociateWorkProductToIncident(WorkProduct workProduct, string incidentID)
        {
            XElement associateRequest = new XElement(workProductNS + "AssociateWorkProductToInterestGroupRequest",
                new XElement(workProductNS + "WorkProductID", workProduct.identifier),
                new XElement(workProductNS + "IncidentID", incidentID)
            );

            // Wrap request in SOAP
            XElement requestElement = WrapInSOAP(associateRequest);

            // Make the request
            try
            {
                String response = POST(requestElement.ToString());
                workProduct.incidentID = incidentID;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("AssociateWorkProductToInterestGroupRequest exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                }
            }
        }


        // Get a full work product from the UICDS core
        public List<WorkProduct> GetAssociatedWorkProducts(string incidentID)
        {
            // Create a request for a work product
            XElement getAssociatedWorkProductRequest = new XElement(workProductNS + "GetAssociatedWorkProductListRequest",
                new XElement(precisbNS + "Identifier", incidentID)
            );

            // Wrap request in SOAP
            XElement requestElement = WrapInSOAP(getAssociatedWorkProductRequest);

            // Make the request
            try
            {
                String response = POST(requestElement.ToString());

                // Parse the resuls
                XElement responseXML = XElement.Parse(response);

                // Create an object to represent the work product
                return processGetAssociatedWorkProductsResponse(responseXML);
    }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.Write("ArchiveIncident exception: " + ex.Status + ": ");
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    System.Diagnostics.Debug.WriteLine(((HttpWebResponse)ex.Response).StatusDescription);
                }
            }

            return new List<WorkProduct>();
        }

        private List<WorkProduct> processGetAssociatedWorkProductsResponse(XElement responseXML)
        {
            List<WorkProduct> workProducts = new List<WorkProduct>();
            IEnumerable<XElement> workProductElements = responseXML.Descendants(precissNS + "WorkProduct");
            if (workProductElements.Count() > 0)
            {
                foreach (XElement workProductElement in workProductElements) 
                {
                    WorkProduct workProduct = new WorkProduct(workProductElement);
                    workProducts.Add(workProduct);
                }
            }
            return workProducts;
        }
    }

}
