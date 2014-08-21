using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UICDS_async
{
    class WorkProduct
    {
        public enum ProcessingStatus {ACCEPTED, PENDING, REJECTED};
        protected ProcessingStatus lastUpdateStatus;
        protected string lastUpdatedBy;

        protected XElement workProductIdentification;

        protected XElement workProduct;

        protected XElement structuredPayload;

        protected XElement digest;

        public String identifier;
        public String incidentID;
        public String type;

        public ProcessingStatus GetLastUpdateStatus()
        {
            return lastUpdateStatus;
        }

        public XElement GetWorkProductIdentification()
        {
            return workProductIdentification;
        }

        public void setWorkProductIdentification(XElement identifier)
        {
            workProductIdentification = identifier;
        }

        public XElement GetWorkProduct()
        {
            return workProduct;
        }

        public XElement GetLastFullStructuredPayload()
        {
            return structuredPayload;
        }

        public XElement GetDigest()
        {
            return digest;
        }

        public WorkProduct()
        {
        }

        public WorkProduct(XElement workProductElement)
        {
            // Assume accepted
            lastUpdateStatus = ProcessingStatus.ACCEPTED;

//            System.Diagnostics.Debug.WriteLine(getProductResponse);
            processWorkProductElement(workProductElement);
        }

        public WorkProduct(XElement workProductElement, String status)
        {
            if (status.Equals("Accepted"))
            {
                lastUpdateStatus = WorkProduct.ProcessingStatus.ACCEPTED;
            }
            else if (status.Equals("Rejected"))
            {
                lastUpdateStatus = WorkProduct.ProcessingStatus.REJECTED;
            }
            else
            {
                lastUpdateStatus = WorkProduct.ProcessingStatus.PENDING;
            }

            processWorkProductElement(workProductElement);
        }

        private string GetIdentifierFromWorkProductIdentification(XElement workProductIdentification)
        {
            IEnumerable<XElement> elements = workProductIdentification.Descendants(ServiceProxy.precisbNS + "Identifier");
            if (elements.Count() > 0)
            {
                return elements.ElementAt(0).Value;
            }
            return "";
        }

        private string GetIncidentIDFromWorkProductProperties(XElement workProductProperties)
        {
            IEnumerable<XElement> elements = workProductProperties.Descendants(ServiceProxy.precisbNS + "Identifier");
            if (elements.Count() > 0)
            {
                return elements.ElementAt(0).Value;
            }

            return "";
        }

        private string GetLastUpdatedByFromWorkProductProperties(XElement workProductProperties)
        {
            IEnumerable<XElement> elements = workProductProperties.Descendants(ServiceProxy.precisbNS + "LastUpdatedBy");
            if (elements.Count() > 0)
            {
                return elements.ElementAt(0).Value;
            }
            return "";
        }

        private string GetTypeFromWorkProductIdentification(XElement workProductIdentification)
        {
            IEnumerable<XElement> elements = workProductIdentification.Descendants(ServiceProxy.precisbNS + "Type");
            if (elements.Count() > 0)
            {
                return elements.ElementAt(0).Value;
            }
            return "";
        }

        public XElement GetPayload(XName elementName)
        {
            // Get the structured payload from the WorkProduct
            //System.Diagnostics.Debug.WriteLine("GetPayload ICS: " + workProduct);
            if (structuredPayload != null)
            {
                IEnumerable<XElement> elements = structuredPayload.DescendantsAndSelf(ServiceProxy.ulexNS + "StructuredPayload");
                if (elements.Count() > 0)
                {
                    // Get the first child Organization element 
                    XElement payload = elements.ElementAt(0).Element(elementName);
                    return payload;
                }
            }
            return null;
        }

        public bool processWorkProductProcessingResponse(string response)
        {
            // parse the response
            XElement responseXML = XElement.Parse(response);

            // Get the status of the response
            lastUpdateStatus = WorkProduct.GetStatusFromWorkProductProcessingStatus(responseXML);

            processWorkProductElement(responseXML);

            if (lastUpdateStatus != WorkProduct.ProcessingStatus.ACCEPTED)
            {
                System.Diagnostics.Debug.WriteLine("Work prodcut update not accepted: " + lastUpdateStatus.ToString());
            }

            return (lastUpdateStatus == WorkProduct.ProcessingStatus.ACCEPTED);
        }

        protected void processWorkProductElement(XElement workProductElement)
        {
            // Set the full contents of the WorkProduct
            IEnumerable<XElement> elements = workProductElement.Descendants(ServiceProxy.precissNS + "WorkProduct");
            if (elements.Count() > 0)
            {
                workProduct = elements.ElementAt(0);
                processStructuredPayload(workProduct);
            }

            // Get the work product identification
            elements = workProductElement.Descendants(ServiceProxy.precissNS + "WorkProductIdentification");
            if (elements.Count() > 0)
            {
                workProductIdentification = elements.ElementAt(0);
                type = GetTypeFromWorkProductIdentification(workProductIdentification);
                identifier = GetIdentifierFromWorkProductIdentification(workProductIdentification);
            }

            // Get the associated interest group (incident) identifier
            elements = workProductElement.Descendants(ServiceProxy.precissNS + "WorkProductProperties");
            if (elements.Count() > 0)
            {
                incidentID = GetIncidentIDFromWorkProductProperties(elements.ElementAt(0));
                lastUpdatedBy = GetLastUpdatedByFromWorkProductProperties(elements.ElementAt(0));
            }

            // Get the digest
            elements = workProductElement.Descendants(ServiceProxy.ucoreNS + "Digest");
            if (elements.Count() > 0)
            {
                digest = elements.ElementAt(0);
            }
        }

        private void processStructuredPayload(XElement workProduct)
        {
            IEnumerable<XElement> elements = workProduct.Descendants(ServiceProxy.ulexNS + "StructuredPayload");
            if (elements.Count() > 0)
            {
                structuredPayload = elements.ElementAt(0);
            }
        }

        // Get the work product processing status from the input UICDS web service response message
        public static WorkProduct.ProcessingStatus GetStatusFromWorkProductProcessingStatus(XElement responseXML)
        {
            //System.Diagnostics.Debug.WriteLine("Response: " + responseXML);
            WorkProduct.ProcessingStatus status = WorkProduct.ProcessingStatus.REJECTED;
            IEnumerable<XElement> elements = responseXML.Descendants(ServiceProxy.precisbNS + "Status");
            if (elements.Count() > 0)
            {
                String statusString = elements.ElementAt(0).Value;
                if (statusString.Equals("Accepted"))
                {
                    status = WorkProduct.ProcessingStatus.ACCEPTED;
                }
                else if (statusString.Equals("Rejected"))
                {
                    status = WorkProduct.ProcessingStatus.REJECTED;
                }
                else
                {
                    status = WorkProduct.ProcessingStatus.PENDING;
                }
            }
            return status;
        }
    }
}
