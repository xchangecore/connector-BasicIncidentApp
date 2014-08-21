using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICDS_Basics
{
    /// <summary>
    /// Represents a UICDS resource instance.
    /// </summary>
    public interface IResourceInstance
    {
        /// <summary>
        /// Get the web resource generated class that represents the UICDS resource instance.
        /// </summary>
        void GetResourceInstance();

        /// <summary>
        /// Get the endpoint for this resource instance
        /// </summary>
        String GetResourceInstanceEndpoint();

        /// <summary>
        /// Get the resource profile name that this resource instance was created with.
        /// </summary>
        void GetResourceProfile();

        /// <summary>
        /// Get the name this resource instance was registered with.
        /// </summary>
       String GetResourceInstanceID();

    }
}
