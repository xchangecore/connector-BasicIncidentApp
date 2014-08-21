using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Net;

namespace UnitTests
{
    class UICDS_Basics_UnitTests
    {

        public const String protocol = "http";
        public const String server = "localhost";
        public const String username = "user1";
        public const String password = "user1";

        private static bool oneFailed = false;
        public static bool failed = false;

        static void Main(string[] args)
        {
            try
            {
                TestResourceProfile resourceProfileTest = new TestResourceProfile();
                resourceProfileTest.run();

                TestResourceInstance resourceInstanceTest = new TestResourceInstance();
                resourceInstanceTest.run();

                TestIncidentManagement incidentManagementTest = new TestIncidentManagement();
                incidentManagementTest.run();
            }
            catch (SoapHeaderException soapEx)
            {
                oneFailed = true;
                System.Diagnostics.Debug.WriteLine("ResourceProfileService:GetProfile:" + soapEx.Message);
            }
            catch (WebException webEx)
            {
                oneFailed = true;
                if (webEx.Status == System.Net.WebExceptionStatus.ConnectFailure)
                {
                    System.Diagnostics.Debug.WriteLine("Cannot connect to server: " + server + ".  The error is: " + webEx.Status + ":" + webEx.Message);
                }
                else if (webEx.Status == System.Net.WebExceptionStatus.NameResolutionFailure)
                {
                    System.Diagnostics.Debug.WriteLine("A computer with the name " + server + " cannot be found on the network.");
                }
                else if (webEx.Response != null && webEx.Response is HttpWebResponse)
                {
                    HttpWebResponse httpEx = (HttpWebResponse)webEx.Response;
                    if (httpEx.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        System.Diagnostics.Debug.WriteLine("The request caused an internal server error.  Please check the UICDS server log for more details.");
                    }
                    else if (httpEx.StatusCode == HttpStatusCode.RequestTimeout)
                    {
                        System.Diagnostics.Debug.WriteLine("The request to the server timed out.  Please check the connectivity to the UICDS server.");
                    }
                    else if (httpEx.StatusCode == HttpStatusCode.NotFound)
                    {
                        System.Diagnostics.Debug.WriteLine("The requested service was not found on UICDS server.  Please check that the request is a valid UICDS service request.");
                    }
                    else if (httpEx.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        System.Diagnostics.Debug.WriteLine("The username and password specified are not authorized on this server");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("The server returned the following status code: " + httpEx.StatusCode + "/n" + httpEx.StatusDescription);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("WebException: " + webEx.Status + ":" + webEx.Message);
                }
            }

            if (oneFailed)
            {
                info("FAILED - tests have finished but with failures");
            }
            else
            {
                info("PASSED - tests have finished successfully");
            }
        }

        static public void assertTrue(String message, bool assert)
        {
            if (!assert)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + message);
                failed = true;
                oneFailed = true;
            }
        }

        static public void info(String message)
        {
            System.Diagnostics.Debug.WriteLine("INFO: " + message);
        }

    }
}
