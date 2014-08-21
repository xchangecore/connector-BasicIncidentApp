using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UICDS_Basics
{
    /// <summary>
    /// Provides access to the UICDS Work Product Service.
    /// </summary>
    public interface IWorkProductService
    {
        /// <summary>
        /// Get a particular work product.
        /// </summary>
        /// <param name="workProductIdentification"></param>
        /// <returns></returns>
        UICDS_Services.WorkProductService.WorkProduct GetWorkProduct(UICDS_Services.WorkProductService.IdentificationType workProductIdentification);

        /// <summary>
        /// Get a list of work products associated with a particular interest group.
        /// </summary>
        /// <param name="interestGroupID"></param>
        void GetAssociatedWorkProducts(String interestGroupID);
    }
}
