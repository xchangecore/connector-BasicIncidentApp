using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICDS_Basics
{
    /// <summary>
    /// Access to the configuration parameters for the UICDS core requests.
    /// </summary>
    public class ServerConfiguration
    {

        private static System.Net.NetworkCredential credentials;

        public ServerConfiguration(String protocol, String server, String username, String password)
        {
            Username = username;
            Password = password;
            Server = server;
            Protocol = protocol;

            credentials = new System.Net.NetworkCredential();
            credentials.UserName = username;
            credentials.Password = password;
        }

        /// <summary>
        /// Get the System.Net.Credentials for this server configuration.
        /// </summary>
        /// <returns></returns>
        public System.Net.NetworkCredential getCredentials()
        {
            return credentials;
        }

        /// <summary>
        /// Get the username to be used on requests to the core.
        /// </summary>
        public String Username { get; private set; }

        /// <summary>
        /// Get the password to be used on requests to the core.
        /// </summary>
        public String Password { get; private set; }

        /// <summary>
        /// Get the fully qualified name of the server to be used in the URL to the core.  (i.e. uicds.company.com)
        /// </summary>
        public String Server { get; private set; }

        /// <summary>
        /// Get the protocol of the server to be used in the URL to the core. (valid values are https or http)
        /// </summary>
        public String Protocol { get; private set; }

    }
}
