using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace UICDS_async
{
    class ServiceProxy
    {
        // SOAP
        public static XNamespace soapEnvNS = "http://schemas.xmlsoap.org/soap/envelope/";

        // UICDS Services
        public static XNamespace imsNS = "http://uicds.org/IncidentManagementService";
        public static XNamespace workProductNS = "http://uicds.org/WorkProductService";
        public static XNamespace resourceInstanceNS = "http://uicds.org/ResourceInstanceService";
        public static XNamespace resourceProfileNS = "http://uicds.org/ResourceProfileService";
        public static XNamespace notificationNS = "http://uicds.org/NotificationService";
        public static XNamespace icsNS = "http://uicds.org/IncidentCommandStructureService";
        public static XNamespace dirNS = "http://uicds.org/DirectoryService";
        public static XNamespace mapNS = "http://uicds.org/MapService";

        // Incident types
        public static XNamespace incNS = "http://uicds.org/incident";

        // ICS types
        public static XNamespace organizationNS = "http://uicds.org/OrganizationElement";

        // UICDS Infrastructure
        public static XNamespace precisbNS = "http://www.saic.com/precis/2009/06/base";
        public static XNamespace precissNS = "http://www.saic.com/precis/2009/06/structures";
        public static XNamespace precispNS = "http://www.saic.com/precis/2009/06/payloads";

        // NIEM
        public static XNamespace niemCoreNS = "http://niem.gov/niem/niem-core/2.0";
        public static XNamespace niemStructuresNS = "http://niem.gov/niem/structures/2.0";

        // UCORE
        public static XNamespace ulexNS = "ulex:message:structure:1.0";
        public static XNamespace ucoreNS = "http://ucore.gov/ucore/2.0";
        public static XNamespace ucoreGMLNS = "http://www.opengis.net/gml/3.2";
        public static XNamespace ucoreCodeSpaceNS = "http://ucore.gov/ucore/2.0/codespace/";

        // DDMS
        public static XNamespace ddmsNS = "http://metadata.dod.mil/mdr/ns/DDMS/2.0/";

        // WS
        public static XNamespace wsAddressingNS = "http://www.w3.org/2005/08/addressing";
        public static XNamespace xlinkNS = "http://www.w3.org/1999/xlink";

        // OGC
        public static XNamespace contextNS = "http://www.opengis.net/context";

        // GML
        public static XNamespace gmlNS = "http://www.opengis.net/gml/3.2";

        public String endpoint
        {
            get;
            set;
        }

        public NetworkCredential credentials
        {
            get;
            set;
        }


        // convenience method for performing an HTTP POST
        // method returns "String" which will catch xml response or errors messages
        public String POST(String data)
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(endpoint);

                byte[] buffer = Encoding.UTF8.GetBytes(data);

                httpRequest.KeepAlive = false;

                httpRequest.Method = "POST";
                httpRequest.Credentials = credentials;
                httpRequest.ContentType = "text/xml";

                httpRequest.ContentLength = buffer.Length;

                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Close();

                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                StreamReader responseReader = new StreamReader(httpResponse.GetResponseStream());
                string response = responseReader.ReadToEnd();

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("POST error: " + ex.Message);
                throw;
            }

        }

        // Wrap an XElement in SOAP 
        public XElement WrapInSOAP(XElement content)
        {
            XElement message = new XElement(soapEnvNS + "Envelope",
                new XAttribute(XNamespace.Xmlns + "SOAP-ENV", soapEnvNS.NamespaceName),
                new XElement(soapEnvNS + "Body",
                    new XElement(content)
                )
            );
            return message;
        }

        protected static Boolean TrueIfSoapFault(string responseFailure)
        {
            try
            {
                ThrowIfSoapFault(responseFailure);
                return false;
            }
            catch (Exception e)
            {
                return true;
            }
        }

        protected static String MessageIfSoapFault(string responseFailure)
        {
            try
            {
                ThrowIfSoapFault(responseFailure);
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        protected static void ThrowIfSoapFault(string responseFailure)
        {
            XDocument doc = ExtractDocumentFromResponse(responseFailure);
            if (doc == null)
            {
                // The response did not include an XML document
                return;
            }

            IEnumerable<XElement> elements = doc.Descendants(soapEnvNS + "Fault");
            if (elements.Count() > 0)
            {
                System.Diagnostics.Debug.WriteLine("Found fault; "+elements.ElementAt(0).ToString());
                throw new Exception(elements.ElementAt(0).ToString());
            }
            return;
        }

        internal static XDocument ExtractDocumentFromResponse(string response)
        {
            // The namespace prefix might not be s: or soap:, so match any non-white-space 
            // content before Envelope.
            Match match = Regex.Match(response, @"<\S*:Envelope");
            if (match.Success)
            {
                XDocument doc = XDocument.Parse(response);
                return doc;
            }
            return null;
        }
    }
}
