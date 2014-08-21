using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UICDS_Basics
{
    /// <summary>
    /// Provides methods to create, update, close, and archive incidents, get a list of incidents, and
    /// get the current Incident work product for an incident.
    /// </summary>
    public interface IIncidentManagementService
    {
        /// <summary>
        /// Get a simple UICDSIncident that can be used to call CreateIncident
        /// </summary>
        /// <param name="name">Name of the incident (ActivityName).</param>
        /// <param name="description">Description of the incident (ActivityDescription).</param>
        /// <param name="latitude">WGS84 latitude in decimal degrees.</param>
        /// <param name="longitude">WGS84 longitude in decimal degress.</param>
        /// <param name="date">Date of the incident (ActivityDate).</param>
        /// <param name="type">Type of the incident (ActivityCategoryText).</param>
        UICDS_Services.IncidentManagementService.UICDSIncidentType GetSimpleCreateIncident(string name, string description, double latitude, double longitude, System.DateTime date, string type);

        /// <summary>
        /// Calls the CreateIncident method on the UICDS core.
        /// </summary>
        UICDS_Services.IncidentManagementService.WorkProductPublicationResponseType CreateIncident(UICDS_Services.IncidentManagementService.UICDSIncidentType incident);

        /// <summary>
        /// Calls the UpdateIncident method on the UICDS core.
        /// </summary>
        UICDS_Services.IncidentManagementService.WorkProductPublicationResponseType UpdateIncident(
            UICDS_Services.IncidentManagementService.IdentificationType1 workProductIdentification,
            UICDS_Services.IncidentManagementService.UICDSIncidentType incident);

        /// <summary>
        /// Calls the CloseIncident method on the UICDS core.
        /// </summary>
        UICDS_Services.IncidentManagementService.ProcessingStatusType CloseIncident(String incidentID);

        /// <summary>
        /// Calls the ArchiveIncident method on the UICDS core.
        /// </summary>
        UICDS_Services.IncidentManagementService.ProcessingStatusType ArchiveIncident(String incidentID);

        /// <summary>
        /// Gets a list of incidents from the UICDS core.
        /// </summary>
        UICDS_Services.IncidentManagementService.WorkProductList GetIncidentList();

        /// <summary>
        /// Gets the current Incident work product for the given incident.
        /// </summary>
        /// <param name="incidentID">Interest group identifier for the incident.</param>
        UICDS_Services.IncidentManagementService.WorkProduct GetIncidentWorkProduct(UICDS_Services.IncidentManagementService.IdentificationType1 workProductIdentification);

        /// <summary>
        /// Returns an XmlSerializer specific to the Incident work product document.
        /// </summary>
        /// <returns></returns>
        XmlSerializer GetIncidentSerializer();

        /// <summary>
        /// Get the Incident work product document from a Work Product Service WorkProduct element.
        /// </summary>
        /// <param name="fullWorkProduct"></param>
        /// <returns></returns>
        UICDS_Services.IncidentManagementService.UICDSIncidentType GetIncidentFromWorkProduct(UICDS_Services.WorkProductService.WorkProduct fullWorkProduct);

        String GetInterestGroupIDFromIncidentWorkProduct(UICDS_Services.IncidentManagementService.IncidentType incident);

        String GetIncidentNameFromIncidentWorkProduct(UICDS_Services.IncidentManagementService.IncidentType incident);
    }
}
