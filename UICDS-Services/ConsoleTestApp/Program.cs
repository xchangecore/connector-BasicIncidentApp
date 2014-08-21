using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Net;
using System.Xml.Serialization;
using System.IO;

namespace ConsoleTestApp
{
    class Program
    {
        public const String INCIDENT1_NAME = "Incident1";
        public const String INCIDENT1_DESC = "Description1";
        public const double INCIDENT1_LATITUDE = 38.7;
        public const double INCIDENT1_LONGITUDE = -76.5;
        public static DateTime INCIDENT1_DATE = System.DateTime.Now;
        public const String INCIDENT1_TYPE = "Traffic";

        private static UICDS_Services.WorkProductService.WorkProductServiceService workProductSerivce;

        static void Main(string[] args)
        {
            UICDS_Services.IncidentManagementService.IncidentManagementServiceService incidentManagementService = new UICDS_Services.IncidentManagementService.IncidentManagementServiceService();
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
            credentials.UserName = "user1";
            credentials.Password = "user1";

            UICDS_Services.IncidentManagementService.IncidentManagementServiceService service = new UICDS_Services.IncidentManagementService.IncidentManagementServiceService();
            service.Credentials = credentials;
            System.Net.ServicePointManager.Expect100Continue = false;

            workProductSerivce = new UICDS_Services.WorkProductService.WorkProductServiceService();
            workProductSerivce.Credentials = credentials;

            UICDS_Services.IncidentManagementService.UICDSIncidentType incident = GetSimpleCreateIncident(INCIDENT1_NAME, INCIDENT1_DESC,
                INCIDENT1_LATITUDE, INCIDENT1_LONGITUDE, INCIDENT1_DATE, INCIDENT1_TYPE);

            StringWriter incStream = new StringWriter();
            XmlSerializer incSer = new XmlSerializer(typeof(UICDS_Services.IncidentManagementService.UICDSIncidentType));
            incSer.Serialize(incStream, incident);
            System.Diagnostics.Debug.WriteLine(incStream.ToString());
            incStream.Close();
            incStream.Dispose();


            UICDS_Services.IncidentManagementService.WorkProductPublicationResponseType createResponse = CreateIncident(service, incident);
            System.Diagnostics.Debug.WriteLine(createResponse.ToString());

            //StringWriter outStream = new StringWriter();
            //XmlSerializer responseSer = new XmlSerializer(typeof(UICDS_Services.IncidentManagementService.WorkProductPublicationResponseType));
            //responseSer.Serialize(outStream, createResponse);
            //System.Diagnostics.Debug.WriteLine(outStream.ToString());
            //outStream.Close();
            //outStream.Dispose();

        }

        public static UICDS_Services.IncidentManagementService.UICDSIncidentType GetSimpleCreateIncident(string name, string description, double latitude, double longitude, System.DateTime date, string type)
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

            coords.GeographicCoordinateLatitude = new UICDS_Services.IncidentManagementService.LatitudeCoordinateType();
            coords.GeographicCoordinateLatitude.LatitudeDegreeValue = new UICDS_Services.IncidentManagementService.LatitudeDegreeType[1];
            UICDS_Services.IncidentManagementService.LatitudeDegreeType latitudeDegrees = new UICDS_Services.IncidentManagementService.LatitudeDegreeType();
            latitudeDegrees.Value = new decimal(38);
            coords.GeographicCoordinateLatitude.LatitudeDegreeValue.SetValue(latitudeDegrees, 0);

            coords.GeographicCoordinateLatitude.LatitudeMinuteValue = new UICDS_Services.IncidentManagementService.AngularMinuteType[1];
            UICDS_Services.IncidentManagementService.AngularMinuteType latitudeMinutes = new UICDS_Services.IncidentManagementService.AngularMinuteType();
            latitudeMinutes.Value = new decimal(57);
            coords.GeographicCoordinateLatitude.LatitudeMinuteValue.SetValue(latitudeMinutes, 0);

            coords.GeographicCoordinateLatitude.LatitudeSecondValue = new UICDS_Services.IncidentManagementService.AngularSecondType[1];
            UICDS_Services.IncidentManagementService.AngularSecondType latitudeSeconds = new UICDS_Services.IncidentManagementService.AngularSecondType();
            latitudeSeconds.Value = new decimal(0.82);
            coords.GeographicCoordinateLatitude.LatitudeSecondValue.SetValue(latitudeSeconds, 0);

            coords.GeographicCoordinateLongitude = new UICDS_Services.IncidentManagementService.LongitudeCoordinateType();
            coords.GeographicCoordinateLongitude.LongitudeDegreeValue = new UICDS_Services.IncidentManagementService.LongitudeDegreeType[1];
            UICDS_Services.IncidentManagementService.LongitudeDegreeType longitudeDegrees = new UICDS_Services.IncidentManagementService.LongitudeDegreeType();
            longitudeDegrees.Value = new decimal(-76);
            coords.GeographicCoordinateLongitude.LongitudeDegreeValue.SetValue(longitudeDegrees, 0);

            coords.GeographicCoordinateLatitude.LatitudeMinuteValue = new UICDS_Services.IncidentManagementService.AngularMinuteType[1];
            UICDS_Services.IncidentManagementService.AngularMinuteType longitudeMinutes = new UICDS_Services.IncidentManagementService.AngularMinuteType();
            longitudeMinutes.Value = new decimal(59);
            coords.GeographicCoordinateLatitude.LatitudeMinuteValue.SetValue(longitudeMinutes, 0);

            coords.GeographicCoordinateLongitude.LongitudeSecondValue = new UICDS_Services.IncidentManagementService.AngularSecondType[1];
            UICDS_Services.IncidentManagementService.AngularSecondType longitudeSeconds = new UICDS_Services.IncidentManagementService.AngularSecondType();
            longitudeSeconds.Value = new decimal(16.19);
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

        public static UICDS_Services.IncidentManagementService.WorkProductPublicationResponseType CreateIncident(UICDS_Services.IncidentManagementService.IncidentManagementServiceService service,
            UICDS_Services.IncidentManagementService.UICDSIncidentType incident)
        {
            UICDS_Services.IncidentManagementService.CreateIncidentRequest request = new UICDS_Services.IncidentManagementService.CreateIncidentRequest();
            request.Incident = incident;

            StringWriter outStream = new StringWriter();
            XmlSerializer responseSer = new XmlSerializer(typeof(UICDS_Services.IncidentManagementService.CreateIncidentRequest));
            responseSer.Serialize(outStream, request);
            System.Diagnostics.Debug.WriteLine(outStream.ToString());
            outStream.Close();
            outStream.Dispose();

            try
            {
                UICDS_Services.IncidentManagementService.CreateIncidentResponse response = service.CreateIncident(request);
                GetIncidentDocument(service, response.WorkProductPublicationResponse);
                return response.WorkProductPublicationResponse;
            }
            catch (SoapHeaderException soapEx)
            {
                handleSoapHeaderException("ResourceInstanceService:Unregister", soapEx);
            }
            catch (WebException webEx)
            {
                handleWebException("ResourceInstanceService:Unregister", webEx);
            }
            return null;
        }

        private static void GetIncidentDocument(UICDS_Services.IncidentManagementService.IncidentManagementServiceService service,
            UICDS_Services.IncidentManagementService.WorkProductPublicationResponseType workProductPublicationResponseType)
        {
            try
            {
                UICDS_Services.WorkProductService.GetProductRequest request = new UICDS_Services.WorkProductService.GetProductRequest();
                request.WorkProductIdentification = new UICDS_Services.WorkProductService.IdentificationType();
                request.WorkProductIdentification.Checksum = new UICDS_Services.WorkProductService.ChecksumType();
                request.WorkProductIdentification.Checksum.Value = workProductPublicationResponseType.WorkProduct.PackageMetadata.WorkProductIdentification.Checksum.Value;

                request.WorkProductIdentification.Identifier = new UICDS_Services.WorkProductService.IdentifierType();
                request.WorkProductIdentification.Identifier.Value = workProductPublicationResponseType.WorkProduct.PackageMetadata.WorkProductIdentification.Identifier.Value;

                request.WorkProductIdentification.State = new UICDS_Services.WorkProductService.StateType();
                request.WorkProductIdentification.State = UICDS_Services.WorkProductService.StateType.Active;

                request.WorkProductIdentification.Type = new UICDS_Services.WorkProductService.CodespaceValueType();
                request.WorkProductIdentification.Type.Value = workProductPublicationResponseType.WorkProduct.PackageMetadata.WorkProductIdentification.Type.Value;

                request.WorkProductIdentification.Version = new UICDS_Services.WorkProductService.VersionType();
                request.WorkProductIdentification.Version.Value = workProductPublicationResponseType.WorkProduct.PackageMetadata.WorkProductIdentification.Version.Value;

                UICDS_Services.WorkProductService.GetProductResponse response = workProductSerivce.GetProduct(request);
                StringWriter outStream = new StringWriter();
                XmlSerializer responseSer = new XmlSerializer(typeof(UICDS_Services.WorkProductService.GetProductResponse));
                responseSer.Serialize(outStream, response);
                System.Diagnostics.Debug.WriteLine(outStream.ToString());
                outStream.Close();
                outStream.Dispose();

                UICDS_Services.IncidentManagementService.UICDSIncidentType incident = GetIncidentFromWorkProduct(response.WorkProduct);
                StringWriter incStream = new StringWriter();
                XmlSerializer incSer = GetIncidentSerializer();

                incSer.Serialize(incStream, incident);
                System.Diagnostics.Debug.WriteLine("Incident: " + incStream.ToString());
                incStream.Close();
                incStream.Dispose();

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

        public static UICDS_Services.IncidentManagementService.UICDSIncidentType GetIncidentFromWorkProduct(UICDS_Services.WorkProductService.WorkProduct fullWorkProduct)
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

        public static XmlSerializer GetIncidentSerializer()
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

            XmlSerializer incidentSerializer = new XmlSerializer(typeof(UICDS_Services.IncidentManagementService.UICDSIncidentType), xOver);

            return incidentSerializer;
        }

        public static void handleSoapHeaderException(String identifier, SoapHeaderException soapEx)
        {
            System.Diagnostics.Debug.WriteLine(identifier + ":" + soapEx.Message);
            throw soapEx;
        }

        public static void handleWebException(String identifier, WebException webEx)
        {
            if (webEx.Status == System.Net.WebExceptionStatus.ConnectFailure)
            {
                System.Diagnostics.Debug.WriteLine(identifier + ": cannot connect to server: " + webEx.Status + ":" + webEx.Message);
                throw webEx;
            }
            else if (webEx.Status == System.Net.WebExceptionStatus.NameResolutionFailure)
            {
                System.Diagnostics.Debug.WriteLine(identifier + ": cannot find server: " + webEx.Status + ":" + webEx.Message);
                throw webEx;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(identifier + ": network error: " + webEx.Status + ":" + webEx.Message);
                throw webEx;
            }

        }


    }
}
