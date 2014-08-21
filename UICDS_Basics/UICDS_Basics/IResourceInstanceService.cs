using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICDS_Basics
{
    /// <summary>
    /// Provides methods to register, get, and unregister UICDS resource instances.
    /// </summary>
    public interface IResourceInstanceService
    {
        /// <summary>
        /// Register a UICDS resource instance.
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="resourceProfile"></param>
        /// <returns></returns>
        IResourceInstance RegisterResourceInstance(String resourceID, String resourceProfile);

        /// <summary>
        /// Get a UICDS resource instance.
        /// </summary>
        /// <param name="resourceID"></param>
        /// <returns></returns>
        IResourceInstance GetResourceInstance(String resourceID);

        /// <summary>
        /// Unregister a resource instance.
        /// </summary>
        /// <param name="resourceID"></param>
        void UnregisterResourceInstance(String resourceID);
    }
}
