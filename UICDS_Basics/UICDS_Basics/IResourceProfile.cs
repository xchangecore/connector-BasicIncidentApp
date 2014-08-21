using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICDS_Basics
{
    /// <summary>
    /// Represents a UICDS Resource Profile.
    /// </summary>
    public interface IResourceProfile
    {
        String GetID();
        List<String> GetInterests();
    }
}
