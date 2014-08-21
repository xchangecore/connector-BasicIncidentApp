using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICDS_Basics
{
    /// <summary>
    /// Service proxy for the UICDS Resource Profile Service.
    /// </summary>
    public interface IResourceProfileService
    {
        /// <summary>
        /// Create a new profile with the given name and interests or return a profile wth the given name if it exists.
        /// Does not update a profile if it exists with the input interests.
        /// </summary>
        /// <param name="applicationProfileID"></param>
        /// <param name="interests"></param>
        IResourceProfile CreateProfile(string id, List<string> interests);

        /// <summary>
        /// Get a resource profile.
        /// </summary>
        /// <param name="applicationProfileID"></param>
        IResourceProfile GetProfile(string id);

        /// <summary>
        /// Remove a resource profile.
        /// </summary>
        /// <param name="applicationProfileID"></param>
        void DeleteProfile(string id);
    }
}
