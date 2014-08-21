using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests
{
    class TestIncidentManagement : TestBase
    {
        private const String TESTNAME = "IncidentManagement";

        UICDS_Basics.IIncidentManagementService incidentService;

        UICDS_Basics.IWorkProductService workProductService;

        public TestIncidentManagement() { }

        public void run()
        {
            startTest(TESTNAME);

            setup();

            incidentService = UICDS_Basics.UICDSProxyFactory.GetIncidentManagementService();
            UICDS_Basics_UnitTests.assertTrue("incidentService is null", incidentService != null);

            workProductService = UICDS_Basics.UICDSProxyFactory.GetWorkProductService();
            UICDS_Basics_UnitTests.assertTrue("workProductService is null", workProductService != null);

            UICDS_Services.IncidentManagementService.UICDSIncidentType incident = incidentService.GetSimpleCreateIncident(INCIDENT1_NAME, INCIDENT1_DESC, 
                INCIDENT1_LATITUDE, INCIDENT1_LONGITUDE, INCIDENT1_DATE, INCIDENT1_TYPE);
            UICDS_Basics_UnitTests.assertTrue("incident is null", incident != null);
            if (incident != null)
            {
                checkIncident1(incident);

                UICDS_Services.IncidentManagementService.WorkProductPublicationResponseType createResponse = incidentService.CreateIncident(incident);
                UICDS_Basics_UnitTests.assertTrue("createResponse is null", createResponse != null);
                UICDS_Basics_UnitTests.assertTrue("createResponse.WorkProductProcessingStatus is null", createResponse.WorkProductProcessingStatus != null);
                if (createResponse.WorkProductProcessingStatus != null)
                {
                    UICDS_Basics_UnitTests.assertTrue("create not accepted",createResponse.WorkProductProcessingStatus.Status.Equals(UICDS_Services.IncidentManagementService.ProcessingStateType.Accepted));

                    if (createResponse.WorkProductProcessingStatus.Status.Equals(UICDS_Services.IncidentManagementService.ProcessingStateType.Accepted))
                    {
                        UICDS_Basics_UnitTests.assertTrue("no wpid", createResponse.WorkProduct.PackageMetadata.WorkProductIdentification != null);
                        UICDS_Basics_UnitTests.assertTrue("no properties", createResponse.WorkProduct.PackageMetadata.WorkProductProperties != null);

                        UICDS_Services.IncidentManagementService.IdentifierType createdID = createResponse.WorkProduct.PackageMetadata.WorkProductIdentification.Identifier;

                        // Get the latest work product id because if the incident was shared then the version will be incremented.
                        UICDS_Services.IncidentManagementService.IdentificationType1 latestWPID = null;

                        UICDS_Services.IncidentManagementService.WorkProductList incidentList = incidentService.GetIncidentList();
                        IEnumerable<UICDS_Services.IncidentManagementService.WorkProduct> workProductList = incidentList.WorkProduct.AsEnumerable<UICDS_Services.IncidentManagementService.WorkProduct>();
                        foreach (UICDS_Services.IncidentManagementService.WorkProduct workProduct in workProductList) 
                        {
                            if (workProduct.PackageMetadata.WorkProductIdentification.Identifier.Value.Equals(createdID.Value)) {
                                latestWPID = workProduct.PackageMetadata.WorkProductIdentification;
                            }
                        }

                        UICDS_Basics_UnitTests.assertTrue("cannot find incident work product", latestWPID != null);

                        UICDS_Services.WorkProductService.IdentificationType wpID = new UICDS_Services.WorkProductService.IdentificationType();
                        wpID.Checksum = new UICDS_Services.WorkProductService.ChecksumType();
                        wpID.Checksum.Value = new String(latestWPID.Checksum.Value.ToCharArray());

                        wpID.Identifier = new UICDS_Services.WorkProductService.IdentifierType();
                        wpID.Identifier.Value = new String(latestWPID.Identifier.Value.ToCharArray());

                        wpID.State = new UICDS_Services.WorkProductService.StateType();
                        wpID.State = UICDS_Services.WorkProductService.StateType.Active;

                        wpID.Type = new UICDS_Services.WorkProductService.CodespaceValueType();
                        wpID.Type.Value = new String(latestWPID.Type.Value.ToCharArray());

                        wpID.Version = new UICDS_Services.WorkProductService.VersionType();
                        wpID.Version.Value = new String(latestWPID.Version.Value.ToCharArray());

                        UICDS_Services.WorkProductService.WorkProduct wp = workProductService.GetWorkProduct(wpID);
                        UICDS_Basics_UnitTests.assertTrue("cannot find work product", wp != null);

                        UICDS_Services.IncidentManagementService.IncidentType retrievedIncident = incidentService.GetIncidentFromWorkProduct(wp);
                        UICDS_Basics_UnitTests.assertTrue("retrievedIncident.IncidentLocation is null", retrievedIncident.IncidentLocation != null);

                        UICDS_Services.IncidentManagementService.TextType type = incident.ActivityCategoryText.ElementAt<UICDS_Services.IncidentManagementService.TextType>(0);
                        type.Value = "Crash";
                        UICDS_Services.IncidentManagementService.WorkProductPublicationResponseType updateResponse = incidentService.UpdateIncident(latestWPID, incident);
                        UICDS_Basics_UnitTests.assertTrue("updateResponse.WorkProductProcessingStatus is null", updateResponse.WorkProductProcessingStatus != null);

                        UICDS_Services.IncidentManagementService.IdentifierType incidentID = 
                            updateResponse.WorkProduct.PackageMetadata.WorkProductProperties.AssociatedGroups.ElementAt<UICDS_Services.IncidentManagementService.IdentifierType>(0);

                        UICDS_Services.IncidentManagementService.ProcessingStatusType closeStatus = incidentService.CloseIncident(incidentID.Value);
                        UICDS_Basics_UnitTests.assertTrue("closeStatus is null", closeStatus != null);

                        UICDS_Services.IncidentManagementService.ProcessingStatusType archiveStatus = incidentService.ArchiveIncident(incidentID.Value);
                        UICDS_Basics_UnitTests.assertTrue("archiveStatus is null", archiveStatus != null);
                    }
                }

            }

            
            cleanup();

            endTest(TESTNAME);

        }

        private void checkIncident1(UICDS_Services.IncidentManagementService.UICDSIncidentType incident)
        {
            UICDS_Basics_UnitTests.assertTrue("no incident name", incident.ActivityName[0] != null);
            //System.Diagnostics.Debug.WriteLine(incident.ActivityName[0].Value);
            UICDS_Basics_UnitTests.assertTrue("incident name is wrong", incident.ActivityName[0].Value.Equals(INCIDENT1_NAME));

            UICDS_Basics_UnitTests.assertTrue("no incident description", incident.ActivityDescriptionText[0] != null);
            UICDS_Basics_UnitTests.assertTrue("incident description is wrong", incident.ActivityDescriptionText[0].Value.Equals(INCIDENT1_DESC));

            UICDS_Basics_UnitTests.assertTrue("no incident type", incident.ActivityCategoryText[0] != null);
            UICDS_Basics_UnitTests.assertTrue("incident type is wrong", incident.ActivityCategoryText[0].Value.Equals(INCIDENT1_TYPE));

            UICDS_Basics_UnitTests.assertTrue("no incident location", incident.IncidentLocation != null);
            UICDS_Basics_UnitTests.assertTrue("no incident location", incident.IncidentLocation.Length > 0);
            UICDS_Basics_UnitTests.assertTrue("no incident location area", incident.IncidentLocation[0].LocationArea != null);
            UICDS_Basics_UnitTests.assertTrue("no incident location area", incident.IncidentLocation[0].LocationArea.Length > 0);
            UICDS_Basics_UnitTests.assertTrue("no incident location circle", incident.IncidentLocation[0].LocationArea[0].AreaCircularRegion != null);
            UICDS_Basics_UnitTests.assertTrue("no incident location circle", incident.IncidentLocation[0].LocationArea[0].AreaCircularRegion.Length > 0);
            UICDS_Basics_UnitTests.assertTrue("no incident location circle", incident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate != null);
            UICDS_Basics_UnitTests.assertTrue("no incident location circle", incident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate.Length > 0);
            UICDS_Services.IncidentManagementService.TwoDimensionalGeographicCoordinateType circle = incident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0];
            UICDS_Basics_UnitTests.assertTrue("no lat degree", circle.GeographicCoordinateLatitude.LatitudeDegreeValue != null);
            UICDS_Basics_UnitTests.assertTrue("no lat degree", circle.GeographicCoordinateLatitude.LatitudeDegreeValue.Length > 0);
            UICDS_Basics_UnitTests.assertTrue("no lat minutes", circle.GeographicCoordinateLatitude.LatitudeMinuteValue != null);
            UICDS_Basics_UnitTests.assertTrue("no lat minutes", circle.GeographicCoordinateLatitude.LatitudeMinuteValue.Length > 0);
            UICDS_Basics_UnitTests.assertTrue("no lat seconds", circle.GeographicCoordinateLatitude.LatitudeSecondValue != null);
            UICDS_Basics_UnitTests.assertTrue("no lat seconds", circle.GeographicCoordinateLatitude.LatitudeSecondValue.Length > 0);
            UICDS_Basics_UnitTests.assertTrue("no long degree", circle.GeographicCoordinateLongitude.LongitudeDegreeValue != null);
            UICDS_Basics_UnitTests.assertTrue("no long degree", circle.GeographicCoordinateLongitude.LongitudeDegreeValue.Length > 0);
            UICDS_Basics_UnitTests.assertTrue("no long minutes", circle.GeographicCoordinateLongitude.LongitudeMinuteValue != null);
            UICDS_Basics_UnitTests.assertTrue("no long minutes", circle.GeographicCoordinateLongitude.LongitudeMinuteValue.Length > 0);
            UICDS_Basics_UnitTests.assertTrue("no long seconds", circle.GeographicCoordinateLongitude.LongitudeSecondValue != null);
            UICDS_Basics_UnitTests.assertTrue("no long seconds", circle.GeographicCoordinateLongitude.LongitudeSecondValue.Length > 0);
        }

        public void setup()
        {
        }

        public void cleanup()
        {
        }

    }
}
