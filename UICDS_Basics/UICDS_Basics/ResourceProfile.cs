using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICDS_Basics
{
    public class ResourceProfile : IResourceProfile
    {
        private String id;
        private List<String> interests;

        public String GetID()
        {
            return id;
        }

        internal void SetID(String id)
        {
            this.id = id;
        }

        public List<String> GetInterests()
        {
            return interests;
        }

        internal void SetInterests(List<String> interests)
        {
            this.interests = interests;
        }
    }
}
