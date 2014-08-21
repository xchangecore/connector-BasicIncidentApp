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
    public partial class IncidentCreateForm : Form
    {
        private UICDS_Basics.IIncidentManagementService incidentManagementService;

        public IncidentCreateForm(UICDS_Basics.IIncidentManagementService incidentManagementService)
        {
            InitializeComponent();

            this.incidentManagementService = incidentManagementService;
        }

        private void IncidentEditForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            double latitude = GetDoubleFromTextBox(textBoxLatitude);
            double longitude = GetDoubleFromTextBox(textBoxLongitude);
            DateTime date = DateTime.Now;

            UICDS_Services.IncidentManagementService.UICDSIncidentType incident = incidentManagementService.GetSimpleCreateIncident(textBoxName.Text,textBoxDescription.Text,latitude,longitude,date,textBoxType.Text);

            UICDS_Services.IncidentManagementService.WorkProductPublicationResponseType response = incidentManagementService.CreateIncident(incident);

            if (response.WorkProductProcessingStatus.Status.Equals(UICDS_Services.IncidentManagementService.ProcessingStateType.Rejected)) {
                MessageBox.Show("Error creating incident: " + response.WorkProductProcessingStatus.Message);
            }

            this.Close();
        }

        private double GetDoubleFromTextBox(TextBox textBox)
        {
            double d;
            if ( !double.TryParse(textBox.Text, out d) )
            {
                d = 0.0;
            }
            return d;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
