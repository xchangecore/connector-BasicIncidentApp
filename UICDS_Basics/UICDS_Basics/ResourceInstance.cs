using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICDS_Basics
{
    public class ResourceInstance : IResourceInstance
    {
        String id;
        String endpoint;

        public void GetResourceInstance()
        { 
        }

        public String GetResourceInstanceEndpoint()
        {
            return endpoint;
        }

        internal void SetResourceInstanceEndpoint(String endpoint)
        {
            this.endpoint = endpoint;
        }

        public void GetResourceProfile()
        {
        }

        public String GetResourceInstanceID()
        {
            return id;
        }

        internal void SetResourceInstanceID(String id)
        {
            this.id = id;
        }
    }
}
