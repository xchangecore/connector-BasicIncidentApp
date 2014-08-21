using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BasicIncidentsApp
{
    public partial class IncidentEditForm : Form
    {
        private UICDS_Basics.IIncidentManagementService incidentManagementService;
        private DataTable incidentsTable;
        private DataRowView row;

        public IncidentEditForm(UICDS_Basics.IIncidentManagementService incidentManagementService, DataTable incidentsTable, DataRowView row)
        {
            InitializeComponent();

            this.incidentManagementService = incidentManagementService;
            this.incidentsTable = incidentsTable;
            this.row = row;

            textBoxName.Text = row["ActivityName"].ToString();
            textBoxID.Text = row["InterestGroup"].ToString();

            UICDS_Services.WorkProductService.WorkProduct workProduct = (UICDS_Services.WorkProductService.WorkProduct)row["workproduct"];
            UICDS_Services.IncidentManagementService.UICDSIncidentType incident = incidentManagementService.GetIncidentFromWorkProduct(workProduct);

            if (incident.ActivityCategoryText.Length > 0)
            {
                textBoxType.Text = incident.ActivityCategoryText[0].Value;
            }

            if (incident.ActivityDescriptionText.Length > 0)
            {
                textBoxDescription.Text = incident.ActivityDescriptionText[0].Value;
            }

            setLatitude(incident);
            setLongitude(incident);

            if (incident.Items != null && incident.Items.Length > 0)
            {
                if (incident.Items[0] is UICDS_Services.IncidentManagementService.DateType)
                {
                    UICDS_Services.IncidentManagementService.DateType date = (UICDS_Services.IncidentManagementService.DateType)incident.Items[0];
                    if (date.Items.Length > 0)
                    {
                        textBoxDate.Text = date.Items[0].Value.ToString();
                    }
                }
            }
        }

        private void setLatitude(UICDS_Services.IncidentManagementService.UICDSIncidentType incident)
        {
            if (incident.IncidentLocation != null && incident.IncidentLocation.Length > 0 && incident.IncidentLocation[0].LocationArea.Length > 0 &&
                incident.IncidentLocation[0].LocationArea[0].AreaCircularRegion.Length > 0 &&
                incident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate.Length > 0)
            {
                UICDS_Services.IncidentManagementService.TwoDimensionalGeographicCoordinateType circle = incident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0];
                StringBuilder sb = new StringBuilder();
                if (circle != null && circle.GeographicCoordinateLatitude != null)
                {
                    if (circle.GeographicCoordinateLatitude.LatitudeDegreeValue != null && circle.GeographicCoordinateLatitude.LatitudeDegreeValue.Length > 0)
                    {
                        sb.Append(circle.GeographicCoordinateLatitude.LatitudeDegreeValue[0].Value);
                    }
                    else
                    {
                        sb.Append("0");
                    }
                    sb.Append(":");

                    if (circle.GeographicCoordinateLatitude.LatitudeMinuteValue != null && circle.GeographicCoordinateLatitude.LatitudeMinuteValue.Length > 0)
                    {
                        sb.Append(circle.GeographicCoordinateLatitude.LatitudeMinuteValue[0].Value);
                    }
                    else
                    {
                        sb.Append("0");
                    }
                    sb.Append(":");

                    if (circle.GeographicCoordinateLatitude.LatitudeSecondValue != null && circle.GeographicCoordinateLatitude.LatitudeSecondValue.Length > 0)
                    {
                        sb.Append(circle.GeographicCoordinateLatitude.LatitudeSecondValue[0].Value);
                    }
                    else
                    {
                        sb.Append("0.0");
                    }
                }

                textBoxLatitude.Text = sb.ToString();
            }
        }

        private void setLongitude(UICDS_Services.IncidentManagementService.UICDSIncidentType incident)
        {
            if (incident.IncidentLocation != null && incident.IncidentLocation.Length > 0 && incident.IncidentLocation[0].LocationArea.Length > 0 &&
                incident.IncidentLocation[0].LocationArea[0].AreaCircularRegion.Length > 0 &&
                incident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate.Length > 0)
            {
                UICDS_Services.IncidentManagementService.TwoDimensionalGeographicCoordinateType circle = incident.IncidentLocation[0].LocationArea[0].AreaCircularRegion[0].CircularRegionCenterCoordinate[0];
                StringBuilder sb = new StringBuilder();
                if (circle != null && circle.GeographicCoordinateLongitude != null)
                {
                    if (circle.GeographicCoordinateLongitude.LongitudeDegreeValue != null && circle.GeographicCoordinateLongitude.LongitudeDegreeValue.Length > 0)
                    {
                        sb.Append(circle.GeographicCoordinateLongitude.LongitudeDegreeValue[0].Value);
                    }
                    else
                    {
                        sb.Append("0");
                    }
                    sb.Append(":");

                    if (circle.GeographicCoordinateLongitude.LongitudeMinuteValue != null && circle.GeographicCoordinateLongitude.LongitudeMinuteValue.Length > 0)
                    {
                        sb.Append(circle.GeographicCoordinateLongitude.LongitudeMinuteValue[0].Value);
                    }
                    else
                    {
                        sb.Append("0");
                    }
                    sb.Append(":");

                    if (circle.GeographicCoordinateLongitude.LongitudeSecondValue != null && circle.GeographicCoordinateLongitude.LongitudeSecondValue.Length > 0)
                    {
                        sb.Append(circle.GeographicCoordinateLongitude.LongitudeSecondValue[0].Value);
                    }
                    else
                    {
                        sb.Append("0.0");
                    }
                }

                textBoxLongitude.Text = sb.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void IncidentEditForm_Load(object sender, EventArgs e)
        {

        }

    }
}
