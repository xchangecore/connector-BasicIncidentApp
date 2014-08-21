using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Collections;
using System.Net;

namespace UICDS_Basics
{
    class ResourceProfileService : ServiceProxy, IResourceProfileService
    {
        private UICDS_Services.ResourceProfileService.ResourceProfileServiceService service;

        public ResourceProfileService(ServerConfiguration serverConfig)
        {
            this.SetCredentials(serverConfig.getCredentials());
            service = new UICDS_Services.ResourceProfileService.ResourceProfileServiceService();
            service.Credentials = credentials;

            service.Url = getServiceUrl(serverConfig, service.Url);
        }

        public IResourceProfile CreateProfile(String id, List<String> interests)
        {
            // If the resource profile already exists then return true
            IResourceProfile profile = ProfileExists(id);
            if (profile != null)
            {
                return profile;
            }

            UICDS_Services.ResourceProfileService.CreateProfileRequest request = new UICDS_Services.ResourceProfileService.CreateProfileRequest();
            request.Profile = new UICDS_Services.ResourceProfileService.ResourceProfile();
            request.Profile.ID = new UICDS_Services.ResourceProfileService.IdentifierType();
            request.Profile.ID.Value = id;
            request.Profile.Interests = new UICDS_Services.ResourceProfileService.Interest[1];
            for (int i = 0; i < interests.Count; i++)
            {
                UICDS_Services.ResourceProfileService.Interest interest = new UICDS_Services.ResourceProfileService.Interest();
                interest.TopicExpression = interests[i];
                request.Profile.Interests.SetValue(interest, i);
            }

            try
            {
                UICDS_Services.ResourceProfileService.CreateProfileResponse response = service.CreateProfile(request);
                if (response.Profile == null || response.Profile.ID == null)
                {
                    return null;
                }
                return CreateProfileFromResponse(response.Profile);

            }
            catch (SoapHeaderException soapEx)
            {
                System.Diagnostics.Debug.WriteLine("ResourceProfileService:CreateProfile:" + soapEx.Message);
                throw soapEx;
            }
        }

        // Check the list of current resource profiles to see if the one we want was already created
        private IResourceProfile ProfileExists(string profileID)
        {
            // get the list of resource instances and see if it exists
            UICDS_Services.ResourceProfileService.GetProfileListRequest request = new UICDS_Services.ResourceProfileService.GetProfileListRequest();
            request.QueryString = "";

            try
            {
                UICDS_Services.ResourceProfileService.GetProfileListResponse response = service.GetProfileList(request);
                //SerializeToDebugConsole(response, typeof(UICDS_Services.ResourceProfileService.GetProfileListResponse));

                IEnumerator e = response.ProfileList.GetEnumerator();
                while (e.MoveNext())
                {
                    //System.Diagnostics.Debug.WriteLine("Type: " + e.Current.GetType().ToString());
                    if (e.Current is UICDS_Services.ResourceProfileService.ResourceProfile)
                    {
                        UICDS_Services.ResourceProfileService.ResourceProfile profile = (UICDS_Services.ResourceProfileService.ResourceProfile)e.Current;
                        if (profile.ID.Value.Equals(profileID))
                        {
                            return CreateProfileFromResponse(profile);
                        }
                    }
                }
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("ResourceProfileService:GetProfileList", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("ResourceProfileService:GetProfileList", webEx);
            }

            return null;
        }

        private IResourceProfile CreateProfileFromResponse(UICDS_Services.ResourceProfileService.ResourceProfile profile)
        {
            if (profile == null || profile.ID == null)
            {
                return null;
            }
            ResourceProfile resourceProfile = new ResourceProfile();
            resourceProfile.SetID(profile.ID.Value);

            List<String> interests = new List<String>();
            IEnumerable<UICDS_Services.ResourceProfileService.Interest> responseInterests = profile.Interests.AsEnumerable<UICDS_Services.ResourceProfileService.Interest>();
            if (responseInterests != null)
            {
                foreach (UICDS_Services.ResourceProfileService.Interest interest in responseInterests)
                {
                    interests.Add(interest.TopicExpression);
                }
            }
            resourceProfile.SetInterests(interests);

            return resourceProfile;
        }

        public IResourceProfile GetProfile(string id)
        {
            UICDS_Services.ResourceProfileService.GetProfileRequest request = new UICDS_Services.ResourceProfileService.GetProfileRequest();
            request.ID = new UICDS_Services.ResourceProfileService.IdentifierType();
            request.ID.Value = id;

            try
            {
                UICDS_Services.ResourceProfileService.GetProfileResponse response = service.GetProfile(request);
                if (response.Profile != null || response.Profile.ID != null)
                {
                    return CreateProfileFromResponse(response.Profile);
                }
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("ResourceProfileService:GetProfile", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("ResourceProfileService:GetProfile", webEx);
            }

            return null;
        }

        public void DeleteProfile(string id)
        {
            UICDS_Services.ResourceProfileService.DeleteProfileRequest request = new UICDS_Services.ResourceProfileService.DeleteProfileRequest();
            request.ID = new UICDS_Services.ResourceProfileService.IdentifierType();
            request.ID.Value = id;

            try
            {
                UICDS_Services.ResourceProfileService.DeleteProfileResponse response = service.DeleteProfile(request);
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("ResourceProfileService:DeleteProfile", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("ResourceProfileService:DeleteProfile", webEx);
            }

        }
    }
}
