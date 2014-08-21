using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Net;

namespace UICDS_Basics
{
    class ResourceInstanceService : ServiceProxy, IResourceInstanceService
    {
        private UICDS_Services.ResourceInstanceService.ResourceInstanceServiceService service;

        public ResourceInstanceService(ServerConfiguration serverConfig)
        {
            this.SetCredentials(serverConfig.getCredentials());
            service = new UICDS_Services.ResourceInstanceService.ResourceInstanceServiceService();
            service.Credentials = credentials;

            service.Url = getServiceUrl(serverConfig, service.Url);
        }

        public IResourceInstance RegisterResourceInstance(String resourceID, String resourceProfile)
        {
            UICDS_Services.ResourceInstanceService.RegisterRequest request = new UICDS_Services.ResourceInstanceService.RegisterRequest();
            request.ID = new UICDS_Services.ResourceInstanceService.IdentifierType();
            request.ID.Value = resourceID;
            request.ResourceProfileID = new UICDS_Services.ResourceInstanceService.IdentifierType();
            request.ResourceProfileID.Value = resourceProfile;

            try
            {
                UICDS_Services.ResourceInstanceService.RegisterResponse response = service.Register(request);
                if (response.ResourceInstance != null)
                {
                    IResourceInstance resource = CreateResourceInstanceFromResponse(response.ResourceInstance);
                    return resource;
                }
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("ResourceInstanceService:Register", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("ResourceInstanceService:Register", webEx);
            }
            return null;
        }

        private IResourceInstance CreateResourceInstanceFromResponse(UICDS_Services.ResourceInstanceService.ResourceInstance resource)
        {
            if (resource == null || resource.ID == null || resource.Endpoints == null || resource.Endpoints.Length == 0)
            {
                return null;
            }
            ResourceInstance resourceInstance = new ResourceInstance();
            resourceInstance.SetResourceInstanceID(resource.ID.Value);
            resourceInstance.SetResourceInstanceEndpoint(resource.Endpoints[0].Address.Value);
            return resourceInstance;
        }

        public IResourceInstance GetResourceInstance(String resourceID)
        {
            UICDS_Services.ResourceInstanceService.GetResourceInstanceRequest request = new UICDS_Services.ResourceInstanceService.GetResourceInstanceRequest();
            request.ID = new UICDS_Services.ResourceInstanceService.IdentifierType();
            request.ID.Value = resourceID;

            try
            {
                UICDS_Services.ResourceInstanceService.GetResourceInstanceResponse response = service.GetResourceInstance(request);
                if (response.ResourceInstance != null)
                {
                    IResourceInstance resource = CreateResourceInstanceFromResponse(response.ResourceInstance);
                    return resource;
                }
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("ResourceInstanceService:GetResourceInstance", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("ResourceInstanceService:GetResourceInstance", webEx);
            }
            return null;
        }

        public void UnregisterResourceInstance(String resourceID)
        {
            UICDS_Services.ResourceInstanceService.UnregisterRequest request = new UICDS_Services.ResourceInstanceService.UnregisterRequest();
            request.ID = new UICDS_Services.ResourceInstanceService.IdentifierType();
            request.ID.Value = resourceID;

            try
            {
                UICDS_Services.ResourceInstanceService.UnregisterResponse response = service.Unregister(request);
                if (response.ID != null)
                {
                    return;
                }
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("ResourceInstanceService:Unregister", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("ResourceInstanceService:Unregister", webEx);
            }
        }

    }
}
