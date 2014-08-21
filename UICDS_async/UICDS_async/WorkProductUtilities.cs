using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UICDS_async
{
	class WorkProductUtilities
	{
        // Get the work product identification element from a web service response
        public static XElement GetWorkProductIdentification(XElement responseXML)
        {
            XElement workProductIdentification = null;
            IEnumerable<XElement> elements = responseXML.Descendants(ServiceProxy.precissNS + "WorkProductIdentification");
            if (elements.Count() > 0)
            {
                workProductIdentification = elements.ElementAt(0);
            }
            return workProductIdentification;
        }


        // Get the work product properties element from a web service response
        public static XElement GetWorkProductProperties(XElement responseXML)
        {
            XElement workProductProps = null;
            IEnumerable<XElement> elements = responseXML.Descendants(ServiceProxy.precissNS + "WorkProductProperties");
            if (elements.Count() > 0)
            {
                workProductProps = elements.ElementAt(0);
            }
            return workProductProps;
        }

        // Get the full package metadata from the work product
        public static XElement GetWorkProductPackageMetadata(XElement workProduct)
        {
            XElement packageMetadata = null;
            IEnumerable<XElement> elements = workProduct.Descendants(ServiceProxy.ulexNS + "PackageMetadata");
            if (elements.Count() > 0)
            {
                packageMetadata = elements.ElementAt(0);
            }
            return packageMetadata;
        }

        // Get the work product processing status from the input UICDS web service response message
        public static WorkProduct.ProcessingStatus GetStatusFromWorkProductProcessingStatus(XElement responseXML)
        {
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

        // Get the descriptive attributes of the incident
        public static XElement GetEventInfo(XElement responseXML)
        {
            XElement eventInfo = null;
            IEnumerable<XElement> elements = responseXML.Descendants(ServiceProxy.ucoreNS + "Event");
            if (elements.Count() > 0)
            {
                eventInfo = elements.ElementAt(0);
            }
            return eventInfo;
        }

        //get the location tags of the incident
        public static XElement GetLocationInfo(XElement responseXML)
        {
            XElement locationInfo = null;
            IEnumerable<XElement> elements = responseXML.Descendants(ServiceProxy.ucoreNS + "Location");
            if (elements.Count() > 0)
            {
                locationInfo = elements.ElementAt(0);
            }

            if (locationInfo != null)
            {
                elements = locationInfo.Descendants();
                if (elements.ElementAt(0).Name.LocalName.ToLower() == "geolocation")
                    return elements.ElementAt(0);
                else
                    return null;
            }
            else
            {
                return null;
            }
        }

        // Get the string value of the first element that has the input element name
        public static String GetStringValueFromFirstElement(WorkProduct workProduct, XName elementName)
        {
            String value = null;
            IEnumerable<XElement> elements = workProduct.GetWorkProduct().Descendants(elementName);
            if (elements.Count() > 0)
            {
                value = elements.ElementAt(0).Value;
            }
            return value;
        }

        public static String GetStringValueFromFirstElement(XName elementName, XElement elementToSearch)
        {
            String value = null;
            IEnumerable<XElement> elements = elementToSearch.Descendants(elementName);
            if (elements.Count() > 0)
            {
                value = elements.ElementAt(0).Value;
            }
            return value;
        }

        //get attribute from element
        public static String GetStringAttributeFromFirstElement(XName elementName, XElement elementToSearch, XName attribute)
        {
            string value = null;
            IEnumerable<XElement> elements = elementToSearch.Descendants(elementName);
            if (elements.Count() > 0)
            {
                XElement e = elements.ElementAt(0);
                IEnumerable<XAttribute> attrs = e.Attributes(attribute);
                if (attrs.Count() > 0)
                {
                    value = attrs.ElementAt(0).Value;
                }
                //value = elements.ElementAt(0).Attributes(attribute).First().Value;
                else
                {
                    attrs = e.Attributes("" + attribute.LocalName);
                    if (attrs.Count() > 0)
                    {
                        value = attrs.ElementAt(0).Value;
                    }
                }
            }
            return value;
        }

        // Get the int value of the first element that has the input element name
        public static int GetIntValueFromFirstElement(WorkProduct workProduct, XName elementName)
        {
            int value = 0;
            IEnumerable<XElement> elements = workProduct.GetWorkProduct().Descendants(elementName);
            if (elements.Count() > 0)
            {
                String valueString = elements.ElementAt(0).Value;
                value = int.Parse(valueString);
            }
            return value;
        }

        // Get the int value of the first element that has the input element name
        public static int GetIntValueFromFirstElement(XName elementName, XElement elementToSearch)
        {
            int value = 0;
            IEnumerable<XElement> elements = elementToSearch.Descendants(elementName);
            if (elements.Count() > 0)
            {
                String valueString = elements.ElementAt(0).Value;
                value = int.Parse(valueString);
            }
            return value;
        }

        // Get the double value of the first element that has the input element name
        public static double GetDoubleValueFromFirstElement(WorkProduct workProduct, XName elementName)
        {
            double value = 0.0;
            IEnumerable<XElement> elements = workProduct.GetWorkProduct().Descendants(elementName);
            if (elements.Count() > 0)
            {
                String valueString = elements.ElementAt(0).Value;
                value = double.Parse(valueString);
            }
            return value;
        }

        // Get the double value of the first element that has the input element name
        public static double GetDoubleValueFromFirstElement(XName elementName, XElement elementToSearch)
        {
            double value = 0.0;
            IEnumerable<XElement> elements = elementToSearch.Descendants(elementName);
            if (elements.Count() > 0)
            {
                String valueString = elements.ElementAt(0).Value;
                value = double.Parse(valueString);
            }
            return value;
        }
    }
}

