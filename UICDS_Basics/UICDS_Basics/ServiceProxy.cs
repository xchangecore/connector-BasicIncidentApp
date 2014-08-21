using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Web.Services.Protocols;
using System.Net;

namespace UICDS_Basics
{
    /// <summary>
    /// Base class for UICDS web service proxies.  Provides support for namespace definitions.
    /// </summary>
    public class ServiceProxy
    {
        protected System.Net.NetworkCredential credentials;

        protected void SetCredentials(System.Net.NetworkCredential credentials)
        {
            if (this.credentials == null)
            {
                this.credentials = credentials;
            }
        }

        public String getServiceUrl(ServerConfiguration serverConfig, String currentUrl)
        {
            Uri newHostUri = new Uri(serverConfig.Protocol + "://" + serverConfig.Server);
            Uri oldHostUri = new Uri(currentUrl);
            Uri uri = new Uri(newHostUri, oldHostUri.AbsolutePath);
            return uri.ToString();
        }

        public void SerializeToDebugConsole(object obj, System.Type type)
        {
            XmlSerializer responseSer = new XmlSerializer(type);
            StringWriter outStream = new StringWriter();
            responseSer.Serialize(outStream, obj);
            System.Diagnostics.Debug.WriteLine(outStream.ToString());
            outStream.Close();
            outStream.Dispose();
        }

        public void handleSoapHeaderException(String identifier, SoapHeaderException soapEx)
        {
            System.Diagnostics.Debug.WriteLine(identifier + ":" + soapEx.Message);
            throw soapEx;
        }

        public void handleWebException(String identifier, WebException webEx)
        {
            if (webEx.Status == System.Net.WebExceptionStatus.ConnectFailure)
            {
                System.Diagnostics.Debug.WriteLine(identifier + ": cannot connect to server: " + webEx.Status + ":" + webEx.Message);
                throw webEx;
            }
            else if (webEx.Status == System.Net.WebExceptionStatus.NameResolutionFailure)
            {
                System.Diagnostics.Debug.WriteLine(identifier + ": cannot find server: " + webEx.Status + ":" + webEx.Message);
                throw webEx;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(identifier + ": network error: " + webEx.Status + ":" + webEx.Message);
                throw webEx;
            }

        }
    }
}
