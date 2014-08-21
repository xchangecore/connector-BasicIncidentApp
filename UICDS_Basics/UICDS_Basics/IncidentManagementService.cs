using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Net;
using System.Xml.Serialization;
using System.IO;

namespace UICDS_Basics
{
    class IncidentManagementService : ServiceProxy, IIncidentManagementService
    {
        private UICDS_Services.IncidentManagementService.IncidentManagementServiceService service;

        private static XmlSerializer incidentSerializer;

        public IncidentManagementService(ServerConfiguration serverConfig)
        {
            this.SetCredentials(serverConfig.getCredentials());
            service = new UICDS_Services.IncidentManagementService.IncidentManagementServiceService();
            service.Credentials = credentials;

            service.Url = getServiceUrl(serverConfig, service.Url);
        }

        public UICDS_Services.IncidentManagementService.UICDSIncidentType GetSimpleCreateIncident(string name, string description, double latitude, double longitude, System.DateTime date, string type)
        {
            UICDS_Services.IncidentManagementService.UICDSIncidentType incident = new UICDS_Services.IncidentManagementService.UICDSIncidentType();

            incident.ActivityCategoryText = new UICDS_Services.IncidentManagementService.TextType[1];
            UICDS_Services.IncidentManagementService.TextType incidentType = new UICDS_Services.IncidentManagementService.TextType();
            incidentType.Value = type;
            incident.ActivityCategoryText.SetValue(incidentType, 0);

            incident.ActivityName = new UICDS_Services.IncidentManagementService.TextType[1];
            UICDS_Services.IncidentManagementService.TextType incidentName = new UICDS_Services.IncidentManagementService.TextType();
            incidentName.Value = name;
            incident.ActivityName.SetValue(incidentName, 0);

            incident.ActivityDescriptionText = new UICDS_Services.IncidentManagementService.TextType[1];
            UICDS_Services.IncidentManagementService.TextType desc = new UICDS_Services.IncidentManagementService.TextType();
            desc.Value = description;
            incident.ActivityDescriptionText.SetValue(desc, 0);

            UICDS_Services.IncidentManagementService.DateType incidentDate = new UICDS_Services.IncidentManagementService.DateType();
            incidentDate.Items = new UICDS_Services.IncidentManagementService.dateTime[1];
            UICDS_Services.IncidentManagementService.dateTime d = new UICDS_Services.IncidentManagementService.dateTime();
            d.Value = date;
            incidentDate.Items.SetValue(d, 0);
            incident.Items = new UICDS_Services.IncidentManagementService.DateType[1];
            incident.Items.SetValue(incidentDate, 0);

            incident.IncidentLocation = new UICDS_Services.IncidentManagementService.LocationType[1];
            UICDS_Services.IncidentManagementService.LocationType location = new UICDS_Services.IncidentManagementService.LocationType();
            location.LocationArea = new UICDS_Services.IncidentManagementService.AreaType[1];
            UICDS_Services.IncidentManagementService.AreaType area = new UICDS_Services.IncidentManagementService.AreaType();
            area.AreaCircularRegion = new UICDS_Services.IncidentManagementService.CircularRegionType[1];
            UICDS_Services.IncidentManagementService.CircularRegionType circle = new UICDS_Services.IncidentManagementService.CircularRegionType();
            circle.CircularRegionCenterCoordinate = new UICDS_Services.IncidentManagementService.TwoDimensionalGeographicCoordinateType[1];
            UICDS_Services.IncidentManagementService.TwoDimensionalGeographicCoordinateType coords = new UICDS_Services.IncidentManagementService.TwoDimensionalGeographicCoordinateType();

            Utilities.DegreesMinutesSeconds latdms = Utilities.DDtoDMS(latitude);

            coords.GeographicCoordinateLatitude = new UICDS_Services.IncidentManagementService.LatitudeCoordinateType();
            coords.GeographicCoordinateLatitude.LatitudeDegreeValue = new UICDS_Services.IncidentManagementService.LatitudeDegreeType[1];
            UICDS_Services.IncidentManagementService.LatitudeDegreeType latitudeDegrees = new UICDS_Services.IncidentManagementService.LatitudeDegreeType();
            latitudeDegrees.Value = latdms.degrees;
            coords.GeographicCoordinateLatitude.LatitudeDegreeValue.SetValue(latitudeDegrees, 0);

            coords.GeographicCoordinateLatitude.LatitudeMinuteValue = new UICDS_Services.IncidentManagementService.AngularMinuteType[1];
            UICDS_Services.IncidentManagementService.AngularMinuteType latitudeMinutes = new UICDS_Services.IncidentManagementService.AngularMinuteType();
            latitudeMinutes.Value = latdms.minutes;
            coords.GeographicCoordinateLatitude.LatitudeMinuteValue.SetValue(latitudeMinutes, 0);

            coords.GeographicCoordinateLatitude.LatitudeSecondValue = new UICDS_Services.IncidentManagementService.AngularSecondType[1];
            UICDS_Services.IncidentManagementService.AngularSecondType latitudeSeconds = new UICDS_Services.IncidentManagementService.AngularSecondType();
            latitudeSeconds.Value = latdms.seconds;
            coords.GeographicCoordinateLatitude.LatitudeSecondValue.SetValue(latitudeSeconds, 0);

            Utilities.DegreesMinutesSeconds longdms = Utilities.DDtoDMS(longitude);

            coords.GeographicCoordinateLongitude = new UICDS_Services.IncidentManagementService.LongitudeCoordinateType();
            coords.GeographicCoordinateLongitude.LongitudeDegreeValue = new UICDS_Services.IncidentManagementService.LongitudeDegreeType[1];
            UICDS_Services.IncidentManagementService.LongitudeDegreeType longitudeDegrees = new UICDS_Services.IncidentManagementService.LongitudeDegreeType();
            longitudeDegrees.Value = longdms.degrees;
            coords.GeographicCoordinateLongitude.LongitudeDegreeValue.SetValue(longitudeDegrees, 0);

            coords.GeographicCoordinateLongitude.LongitudeMinuteValue = new UICDS_Services.IncidentManagementService.AngularMinuteType[1];
            UICDS_Services.IncidentManagementService.AngularMinuteType longitudeMinutes = new UICDS_Services.IncidentManagementService.AngularMinuteType();
            longitudeMinutes.Value = longdms.minutes;
            coords.GeographicCoordinateLongitude.LongitudeMinuteValue.SetValue(longitudeMinutes, 0);

            coords.GeographicCoordinateLongitude.LongitudeSecondValue = new UICDS_Services.IncidentManagementService.AngularSecondType[1];
            UICDS_Services.IncidentManagementService.AngularSecondType longitudeSeconds = new UICDS_Services.IncidentManagementService.AngularSecondType();
            longitudeSeconds.Value = longdms.seconds;
            coords.GeographicCoordinateLongitude.LongitudeSecondValue.SetValue(longitudeSeconds, 0);

            circle.CircularRegionRadiusLengthMeasure = new UICDS_Services.IncidentManagementService.LengthMeasureType[1];
            UICDS_Services.IncidentManagementService.LengthMeasureType radius = new UICDS_Services.IncidentManagementService.LengthMeasureType();
            radius.LengthUnitCode = new UICDS_Services.IncidentManagementService.LengthCodeType();
            radius.LengthUnitCode.Value = UICDS_Services.IncidentManagementService.LengthCodeSimpleType.SMI;
            UICDS_Services.IncidentManagementService.MeasurePointValueType radiusValue = new UICDS_Services.IncidentManagementService.MeasurePointValueType();
            radiusValue.Value = new decimal(0.0);
            radius.Items = new UICDS_Services.IncidentManagementService.MeasurePointValueType[1];
            radius.Items.SetValue(radiusValue, 0);

            circle.CircularRegionCenterCoordinate.SetValue(coords, 0);
            location.LocationArea.SetValue(area, 0);
            area.AreaCircularRegion.SetValue(circle, 0);
            incident.IncidentLocation.SetValue(location, 0);

            return incident;
        }

        public UICDS_Services.IncidentManagementService.WorkProductPublicationResponseType CreateIncident(UICDS_Services.IncidentManagementService.UICDSIncidentType incident)
        {
            UICDS_Services.IncidentManagementService.CreateIncidentRequest request = new UICDS_Services.IncidentManagementService.CreateIncidentRequest();
            request.Incident = incident;

            try
            {
                UICDS_Services.IncidentManagementService.CreateIncidentResponse response = service.CreateIncident(request);
                return response.WorkProductPublicationResponse;
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("IncidentManagementService:CreateIncident", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("IncidentManagementService:CreateIncident", webEx);
            }
            return null;
        }

        public UICDS_Services.IncidentManagementService.WorkProductPublicationResponseType UpdateIncident(
            UICDS_Services.IncidentManagementService.IdentificationType1 workProductIdentification,
            UICDS_Services.IncidentManagementService.UICDSIncidentType incident)
        {
            UICDS_Services.IncidentManagementService.UpdateIncidentRequest request = new UICDS_Services.IncidentManagementService.UpdateIncidentRequest();
            request.WorkProductIdentification = workProductIdentification;
            request.Incident = incident;

            try
            {
                UICDS_Services.IncidentManagementService.UpdateIncidentResponse response = service.UpdateIncident(request);
                return response.WorkProductPublicationResponse;
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("IncidentManagementService:UpdateIncident", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("IncidentManagementService:UpdateIncident", webEx);
            }
            return null;
        }

        public UICDS_Services.IncidentManagementService.ProcessingStatusType CloseIncident(String incidentID)
        {
            UICDS_Services.IncidentManagementService.CloseIncidentRequest request = new UICDS_Services.IncidentManagementService.CloseIncidentRequest();
            request.IncidentID = incidentID;

            try
            {
                UICDS_Services.IncidentManagementService.CloseIncidentResponse response = service.CloseIncident(request);
                return response.WorkProductProcessingStatus;
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("IncidentManagementService:CloseIncident", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("IncidentManagementService:CloseIncident", webEx);
            }
            return null;
        }

        public UICDS_Services.IncidentManagementService.ProcessingStatusType ArchiveIncident(String incidentID)
        {
            UICDS_Services.IncidentManagementService.ArchiveIncidentRequest request = new UICDS_Services.IncidentManagementService.ArchiveIncidentRequest();
            request.IncidentID = incidentID;

            try
            {
                UICDS_Services.IncidentManagementService.ArchiveIncidentResponse response = service.ArchiveIncident(request);
                return response.WorkProductProcessingStatus;
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("IncidentManagementService:ArchiveIncident", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("IncidentManagementService:ArchiveIncident", webEx);
            }
            return null;
        }

        public UICDS_Services.IncidentManagementService.WorkProductList GetIncidentList()
        {
            try
            {
                UICDS_Services.IncidentManagementService.GetIncidentListResponse response = service.GetIncidentList(new object());
                return response.WorkProductList;
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("IncidentManagementService:GetIncidentList", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("IncidentManagementService:GetIncidentList", webEx);
            }
            return null;
        }

        public UICDS_Services.IncidentManagementService.WorkProduct GetIncidentWorkProduct(UICDS_Services.IncidentManagementService.IdentificationType1 workProductIdentification)
        {
            UICDS_Services.IncidentManagementService.GetIncidentRequest request = new UICDS_Services.IncidentManagementService.GetIncidentRequest();
            request.WorkProductIdentification = workProductIdentification;

            try
            {
                UICDS_Services.IncidentManagementService.GetIncidentResponse response = service.GetIncident(request);
                return response.WorkProduct;
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("IncidentManagementService:GetIncidentWorkProduct", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("IncidentManagementService:GetIncidentWorkProduct", webEx);
            }
            return null;
        }

        public XmlSerializer GetIncidentSerializer()
        {

            if (incidentSerializer == null)
            {
                // Create the XmlAttributes and XmlAttributeOverrides objects.
                XmlAttributes attrs = new XmlAttributes();
                XmlAttributeOverrides xOver = new XmlAttributeOverrides();

                XmlRootAttribute xRoot = new XmlRootAttribute();

                // Set a new Namespace and ElementName for the root element.
                xRoot.Namespace = "http://uicds.org/incident";
                xRoot.ElementName = "Incident";
                attrs.XmlRoot = xRoot;

                /* Add the XmlAttributes object to the XmlAttributeOverrides. 
                   No  member name is needed because the whole class is 
                   overridden. */
                xOver.Add(typeof(UICDS_Services.IncidentManagementService.UICDSIncidentType), attrs);

                incidentSerializer = new XmlSerializer(typeof(UICDS_Services.IncidentManagementService.UICDSIncidentType), xOver);
            }

            return incidentSerializer;
        }

        public UICDS_Services.IncidentManagementService.UICDSIncidentType GetIncidentFromWorkProduct(UICDS_Services.WorkProductService.WorkProduct fullWorkProduct)
        {
            if (fullWorkProduct.StructuredPayload.Length > 0)
            {
                UICDS_Services.WorkProductService.StructuredPayloadType payload = fullWorkProduct.StructuredPayload[0];
                MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(payload.Any.OuterXml));
                XmlSerializer ser = GetIncidentSerializer();
                try
                {
                    UICDS_Services.IncidentManagementService.UICDSIncidentType incident = (UICDS_Services.IncidentManagementService.UICDSIncidentType)ser.Deserialize(memStream);
                    return incident;
                }
                catch (InvalidOperationException e)
                {
                    System.Diagnostics.Debug.WriteLine("Invalid operation error getting incident work product : " + e.InnerException.Message);
                    return null;
                }
            }
            return null;
        }

        public String GetInterestGroupIDFromIncidentWorkProduct(UICDS_Services.IncidentManagementService.IncidentType incident)
        {
            String igid = "Unknown id";
            if (incident != null && incident.ActivityIdentification.Length > 0 && incident.ActivityIdentification[0].IdentificationID.Length > 0)
            {
                igid = incident.ActivityIdentification[0].IdentificationID[0].Value;
            }
            return igid;
        }

        public String GetIncidentNameFromIncidentWorkProduct(UICDS_Services.IncidentManagementService.IncidentType incident)
        {
            String name = "Unknown Incident";
            if (incident != null && incident.ActivityName.Length > 0)
            {
                name = incident.ActivityName[0].Value;
            }
            return name;
        }


    }
}
