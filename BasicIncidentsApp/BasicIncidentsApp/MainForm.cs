using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Services.Protocols;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Threading;

namespace BasicIncidentsApp
{
    public partial class MainForm : Form
    {
        private UICDS_Basics.IResourceProfileService resourceProfileService;

        private UICDS_Basics.IResourceInstanceService resourceInstanceService;

        private UICDS_Basics.INotificationsService notificationService;

        private UICDS_Basics.IIncidentManagementService incidentManagementService;

        private UICDS_Basics.IWorkProductService workProductService;

        private UICDS_Basics.WorkProductNotificationHandler notificationHandler;

        private UICDS_Basics.WorkProductDeletedNotificationHandler deleteHandler;

        private DataSet incidentDataSet;
        private DataTable incidentsTable;
        BindingSource incidentsTableBindingSource;

        private readonly string ACTIVITYNAME_FIELD = "ActivityName";
        private readonly string IG_ID_FIELD = "InterestGroup";
        private readonly string WORKPRODUCT_ID_FIELD = "WorkProductID";
        private readonly string WORKPRODUCT_FIELD = "workproduct";

        private UICDS_Basics.IResourceProfile profile;

        private UICDS_Basics.IResourceInstance resourceInstance;

        private delegate void incidentTableUpdateDelegate(DataTable table, string name, string igid, UICDS_Services.WorkProductService.WorkProduct fullWorkProduct);

        private delegate void incidentTableRemoveDelegate(DataTable table, string igid);

        private incidentTableUpdateDelegate checkedListBoxUpdateDelegate;

        private incidentTableRemoveDelegate checkedListBoxRemoveDelegate;

        public MainForm()
        {
            InitializeComponent();

            checkedListBoxUpdateDelegate = new incidentTableUpdateDelegate(CreateOrUpdateIncidentsTable);

            checkedListBoxRemoveDelegate = new incidentTableRemoveDelegate(RemoveRowFromIncidentsTable);

            CreateDataTable();
        }

        private void CreateDataTable()
        {
            incidentDataSet = new DataSet();
            incidentsTable = new DataTable("incidents");
            incidentsTable.Columns.Add(ACTIVITYNAME_FIELD, typeof(string));
            incidentsTable.Columns.Add(IG_ID_FIELD, typeof(string));
            incidentsTable.Columns.Add(WORKPRODUCT_ID_FIELD, typeof(string));
            incidentsTable.Columns.Add(WORKPRODUCT_FIELD, typeof(UICDS_Services.WorkProductService.WorkProduct));

            incidentDataSet.Tables.Add(incidentsTable);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            textBoxServer.Text = Properties.Settings.Default.SERVER;
            textBoxUser.Text = Properties.Settings.Default.USERNAME;
            textBoxPassword.Text = Properties.Settings.Default.PASSWORD;
            if (Properties.Settings.Default.PROTOCOL.Equals("http"))
            {
                radioButtonHTTP.Checked = true;
            }
            else
            {
                radioButtonHTTPS.Checked = true;
            }

            buttonStop.Enabled = false;
            buttonCreate.Enabled = false;
            buttonEditIncident.Enabled = false;
            buttonClose.Enabled = false;
            buttonRefresh.Enabled = false;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.SERVER = textBoxServer.Text;
            Properties.Settings.Default.USERNAME = textBoxUser.Text;
            Properties.Settings.Default.PASSWORD = textBoxPassword.Text;
            if (radioButtonHTTP.Checked)
            {
                Properties.Settings.Default.PROTOCOL = "http";
            }
            else
            {
                Properties.Settings.Default.PROTOCOL = "https";
            }
            Properties.Settings.Default.Save();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            // save the current settings
            SaveSettings();

            // intialize service proxies
            initializeServiceProxies();

            // create profile if necessary
            CreateResourceProfile();

            if (profile != null)
            {
                // create resource instance if necessary
                CreateResourceInstance();

                // register event delegate

                if (resourceInstance != null)
                {
                    // get initial messages
                    List<UICDS_Services.NotificationService.IdentificationType> workProductIDs = notificationService.GetInitialUICDSEvents(resourceInstance.GetResourceInstanceID());

                    // populate the incident list
                    PopulateIncidentList();

                    // register callback
                    if (notificationHandler == null)
                    {
                        notificationHandler = new UICDS_Basics.WorkProductNotificationHandler(this.WorkProductNotificationHandler);
                    }
                    notificationService.updateNotifications += notificationHandler;

                    if (deleteHandler == null)
                    {
                        deleteHandler = new UICDS_Basics.WorkProductDeletedNotificationHandler(this.WorkProductDeletedNotificationHandler);
                    }
                    notificationService.deleteNotifications += deleteHandler;

                    // disable button
                    buttonStart.Enabled = false;
                    buttonStop.Enabled = true;
                    buttonCreate.Enabled = true;
                    buttonEditIncident.Enabled = true;
                    buttonClose.Enabled = true;
                    buttonRefresh.Enabled = true;

                    // Start the background worker thread to poll for notifications
                    if (resourceInstance != null)
                    {
                        backgroundPoller.RunWorkerAsync(null);
                    }
                    else
                    {
                        MessageBox.Show("Cannot start background polling because the resource instance is null");
                    }
                }
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            // unregister resource instance
            resourceInstanceService.UnregisterResourceInstance(resourceInstance.GetResourceInstanceID());

            // stop background thread
            backgroundPoller.CancelAsync();

            // unregister notification callback
            notificationService.updateNotifications -= notificationHandler;

        }
        private void initializeServiceProxies()
        {
            // Configure the UICDS Proxy factory with connection information
            if (!UICDS_Basics.UICDSProxyFactory.isServerConfigurationSet())
            {
                String protocol = "https";
                if (radioButtonHTTP.Checked)
                {
                    protocol = "http";
                }

                UICDS_Basics.UICDSProxyFactory.CreateServerConfiguration(protocol, textBoxServer.Text, textBoxUser.Text, textBoxPassword.Text);
            }

            // Create the Resource Profile Service Proxy
            if (resourceProfileService == null)
            {
                resourceProfileService = UICDS_Basics.UICDSProxyFactory.GetResourceProfileService();
                if (resourceProfileService == null)
                {
                    MessageBox.Show("ResourceProfileService proxy could not be created.");
                }
            }

            // Create the Resource Instance Service Proxy
            if (resourceInstanceService == null)
            {
                resourceInstanceService = UICDS_Basics.UICDSProxyFactory.GetResourceInstanceService();
                if (resourceInstanceService == null)
                {
                    MessageBox.Show("ResourceInstanceService proxy could not be created.");
                }
            }

            // Create the Notification Service Proxy
            if (notificationService == null)
            {
                notificationService = UICDS_Basics.UICDSProxyFactory.GetNotificationsService();
                if (notificationService == null)
                {
                    MessageBox.Show("NotificationService proxy could not be created.");
                }
            }

            // Create the Incident Management Service Proxy
            if (incidentManagementService == null)
            {
                incidentManagementService = UICDS_Basics.UICDSProxyFactory.GetIncidentManagementService();
                if (incidentManagementService == null)
                {
                    MessageBox.Show("IncidentManagementService proxy could not be created.");
                }
            }

            // Create the Work Product Service Proxy
            if (workProductService == null)
            {
                workProductService = UICDS_Basics.UICDSProxyFactory.GetWorkProductService();
                if (workProductService == null)
                {
                    MessageBox.Show("WorkProductService proxy could not be created.");
                }
            }
        }

        private void CreateResourceProfile()
        {
            List<String> interests = new List<String>();
            interests.Add("Incident");
            try
            {
                profile = resourceProfileService.CreateProfile("IncidentListener", interests);
            }
            catch (SoapHeaderException soapEx)
            {
                MessageBox.Show(soapEx.Message);
            }
            catch (WebException webEx)
            {
                MessageBox.Show(webEx.Status + ":" + webEx.Message);
            }
            if (profile == null)
            {
                MessageBox.Show("Profile IncidentListener was not created");
            }
        }

        private void CreateResourceInstance()
        {
            try
            {
                resourceInstance = resourceInstanceService.RegisterResourceInstance("BasicsIncidentsApp", "IncidentListener");
            }
            catch (SoapHeaderException soapEx)
            {
                MessageBox.Show(soapEx.Message);
            }
            catch (WebException webEx)
            {
                MessageBox.Show(webEx.Status + ":" + webEx.Message);
            }
            if (resourceInstance == null)
            {
                MessageBox.Show("Resouce instance not registered");
            }
        }

        private void PopulateIncidentList()
        {
            try
            {
                List<UICDS_Services.NotificationService.IdentificationType> workProductIDList = notificationService.GetInitialUICDSEvents(resourceInstance.GetResourceInstanceID());
                if (workProductIDList != null)
                {
                    IEnumerable<UICDS_Services.NotificationService.IdentificationType> workProductIDEnumerable = workProductIDList.AsEnumerable<UICDS_Services.NotificationService.IdentificationType>();
                    foreach (UICDS_Services.NotificationService.IdentificationType wpid in workProductIDEnumerable)
                    {
                        UICDS_Services.WorkProductService.IdentificationType workProductID = notificationService.GetWorkProductServiceID(wpid);
                        DataRow row = incidentsTable.NewRow();
                        UICDS_Services.WorkProductService.WorkProduct fullWorkProduct = workProductService.GetWorkProduct(workProductID);

                        UICDS_Services.IncidentManagementService.IncidentType incident = incidentManagementService.GetIncidentFromWorkProduct(fullWorkProduct);

                        String name = incidentManagementService.GetIncidentNameFromIncidentWorkProduct(incident);

                        String igid = incidentManagementService.GetInterestGroupIDFromIncidentWorkProduct(incident);

                        CreateOrUpdateIncidentsTable(incidentsTable, name, igid, fullWorkProduct);
                    }

                    if (incidentsTableBindingSource == null)
                    {
                        incidentsTableBindingSource = new BindingSource();
                    }
                    if (incidentsTableBindingSource.DataSource == null)
                    {
                        //incidentsTableBindingSource.DataSource = incidentsTable;
                        DataView sortedView = new DataView(incidentsTable);
                        sortedView.Sort = ACTIVITYNAME_FIELD;
                        incidentsTableBindingSource.DataSource = sortedView;
                        checkedListBoxIncidents.DataSource = incidentsTableBindingSource;
                        checkedListBoxIncidents.DisplayMember = ACTIVITYNAME_FIELD;
                    }

                }
            }
            catch (SoapHeaderException soapEx)
            {
                MessageBox.Show(soapEx.Message);
            }
            catch (WebException webEx)
            {
                MessageBox.Show(webEx.Status + ":" + webEx.Message);
            }

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            // get the list of incidents that are selected and close and archive them
            StringBuilder sb = new StringBuilder();
            foreach (DataRowView incidentView in checkedListBoxIncidents.CheckedItems)
            {
                Object obj = incidentView.Row[WORKPRODUCT_FIELD];
                if (obj is UICDS_Services.WorkProductService.WorkProduct)
                {
                    UICDS_Services.WorkProductService.WorkProduct wp = (UICDS_Services.WorkProductService.WorkProduct)obj;
                    sb.Append("Close ");
                    sb.Append(incidentView.Row[ACTIVITYNAME_FIELD]);
                    sb.Append(" : ");
                    sb.Append(wp.PackageMetadata.WorkProductProperties.AssociatedGroups[0].Value);
                    sb.Append("?\n");
                    DialogResult result = MessageBox.Show(sb.ToString(), "Confirm delete", MessageBoxButtons.YesNo);
                    sb.Clear();
                    if (result.Equals(DialogResult.Yes))
                    {
                        try
                        {
                            UICDS_Services.IncidentManagementService.ProcessingStatusType closeResult = incidentManagementService.CloseIncident(wp.PackageMetadata.WorkProductProperties.AssociatedGroups[0].Value);
                            if (closeResult.Status.Equals(UICDS_Services.IncidentManagementService.ProcessingStateType.Accepted))
                            {
                                UICDS_Services.IncidentManagementService.ProcessingStatusType archiveResult = incidentManagementService.ArchiveIncident(wp.PackageMetadata.WorkProductProperties.AssociatedGroups[0].Value);
                                if (archiveResult.Status.Equals(UICDS_Services.IncidentManagementService.ProcessingStateType.Rejected))
                                {
                                    MessageBox.Show("Archive was rejected: " + archiveResult.Message);
                                }
                                else if (archiveResult.Status.Equals(UICDS_Services.IncidentManagementService.ProcessingStateType.Pending))
                                {
                                    MessageBox.Show("Archive is pending: " + archiveResult.Message);
                                }
                            }
                            else if (closeResult.Status.Equals(UICDS_Services.IncidentManagementService.ProcessingStateType.Rejected))
                            {
                                MessageBox.Show("Close was rejected: " + closeResult.Message);
                            }
                            else if (closeResult.Status.Equals(UICDS_Services.IncidentManagementService.ProcessingStateType.Pending))
                            {
                                MessageBox.Show("Close is pending: " + closeResult.Message);
                            }
                            else if (closeResult.Message.Value.Contains("does not exist")) {
                                MessageBox.Show("Selected incident does not exist");
                            }
                        }
                        catch (SoapHeaderException soapEx)
                        {
                            MessageBox.Show("IncidentManagementService:closing or archiving", soapEx.Message);
                        }
                        catch (WebException webEx)
                        {
                            MessageBox.Show("IncidentManagementService:closing or archiving", webEx.Message);
                        }
                    }
                }
            }

        }

        private void buttonPoll_Click(object sender, EventArgs e)
        {
            checkedListBoxIncidents.DataSource = null;
            checkedListBoxIncidents.DataSource = incidentsTableBindingSource;
            checkedListBoxIncidents.DisplayMember = ACTIVITYNAME_FIELD;
        }

        public void WorkProductNotificationHandler(object sender, UICDS_Basics.WorkProductNotificationEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Received notification");
            UICDS_Services.WorkProductService.WorkProduct workProductNotification = args.getNotification();

            UICDS_Services.WorkProductService.WorkProduct fullWorkProduct = null;
            try
            {
                if (workProductNotification.PackageMetadata.WorkProductIdentification.State.Equals(UICDS_Services.WorkProductService.StateType.Active))
                {
                    fullWorkProduct = workProductService.GetWorkProduct(workProductNotification.PackageMetadata.WorkProductIdentification);
                }
            }
            catch (SoapHeaderException soapEx)
            {
                MessageBox.Show("IncidentManagementService:GetIncidentWorkProduct", soapEx.Message);
            }
            catch (WebException webEx)
            {
                MessageBox.Show("IncidentManagementService:GetIncidentWorkProduct", webEx.Message);
            }


            if (fullWorkProduct != null)
            {
                String workProductType = fullWorkProduct.PackageMetadata.WorkProductIdentification.Type.Value;
                if (workProductType.Equals("Incident"))
                {
                    UICDS_Services.IncidentManagementService.IncidentType incident = incidentManagementService.GetIncidentFromWorkProduct(fullWorkProduct);

                    String name = incidentManagementService.GetIncidentNameFromIncidentWorkProduct(incident);

                    String igid = incidentManagementService.GetInterestGroupIDFromIncidentWorkProduct(incident);

                    if (checkedListBoxIncidents.InvokeRequired)
                    {
                        checkedListBoxIncidents.Invoke(checkedListBoxUpdateDelegate, new Object[] { incidentsTable, name, igid, fullWorkProduct });
                    }
                    else
                    {
                        CreateOrUpdateIncidentsTable(incidentsTable, name, igid, fullWorkProduct);
                    }

                }
            }
        }


        public void WorkProductDeletedNotificationHandler(object sender, UICDS_Basics.WorkProductDeletedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Received delete notification");
            UICDS_Services.NotificationService.WorkProductDeletedNotification notification = args.getNotification();
            if (notification != null)
            {
                if (checkedListBoxIncidents.InvokeRequired)
                {
                    checkedListBoxIncidents.Invoke(checkedListBoxRemoveDelegate, new Object[] { incidentsTable, notification.WorkProductIdentification.Identifier.Value });
                }
                else
                {
                    RemoveRowFromIncidentsTable(incidentsTable, notification.WorkProductIdentification.Identifier.Value);
                }
                
            }
            else
            {
                MessageBox.Show("Received a work product deleted notification but it was null");
            }
        }

        private void CreateOrUpdateIncidentsTable(DataTable table, string name, string igid, UICDS_Services.WorkProductService.WorkProduct fullWorkProduct)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(IG_ID_FIELD);
            sb.Append("='");
            sb.Append(igid);
            sb.Append("'");
            DataRow[] rows = table.Select(sb.ToString());
            if (rows.Length == 1)
            {
                rows[0][ACTIVITYNAME_FIELD] = name;
                rows[0][IG_ID_FIELD] = igid;
                rows[0][WORKPRODUCT_ID_FIELD] = fullWorkProduct.PackageMetadata.WorkProductIdentification.Identifier.Value;
                rows[0][WORKPRODUCT_FIELD] = fullWorkProduct;
            }
            else if (rows.Length == 0)
            {
                DataRow row = table.NewRow();
                row[ACTIVITYNAME_FIELD] = name;
                row[IG_ID_FIELD] = igid;
                row[WORKPRODUCT_ID_FIELD] = fullWorkProduct.PackageMetadata.WorkProductIdentification.Identifier.Value;
                row[WORKPRODUCT_FIELD] = fullWorkProduct;
                table.Rows.Add(row);
            }

        }

        private void RemoveRowFromIncidentsTable(DataTable table, string igid)
        {
            DataRow[] rows = incidentsTable.Select(WORKPRODUCT_ID_FIELD + " = '" + igid + "'");
            if (rows.Length == 1)
            {
                incidentsTable.Rows.Remove(rows[0]);
            }
            else
            {
                if (rows.Length > 0)
                {
                    MessageBox.Show("Found more than one entry for Work Product ID: " + igid);
                    for (int i = 0; i < rows.Length; i++)
                    {
                        incidentsTable.Rows.Remove(rows[i]);
                    }
                }
            }
        }

        private void backgroundPoller_DoWork(object sender, DoWorkEventArgs e)
        {
            int delay = 1000; // 1 second
            int interval = 10000;
            int elapsed = 0;
            BackgroundWorker worker = sender as BackgroundWorker;

            while (!worker.CancellationPending)
            {
                elapsed = 0;
                System.Diagnostics.Debug.WriteLine("Checking for Notifications");
                notificationService.GetUICDSEvents(resourceInstance.GetResourceInstanceEndpoint());

                while (elapsed < interval && !worker.CancellationPending)
                {
                    Thread.Sleep(delay);
                    elapsed += delay;
                }
            }

            e.Cancel = true;
        }

        private void backgroundPoller_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonStart.Enabled = true;
            buttonStop.Enabled = false;
            buttonCreate.Enabled = false;
            buttonEditIncident.Enabled = false;
            buttonClose.Enabled = false;
            buttonRefresh.Enabled = false;

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
        }

        private void buttonEditIncident_Click(object sender, EventArgs e)
        {
            if (checkedListBoxIncidents.CheckedItems.Count != 1) {
                MessageBox.Show("Please select only one incident to edit");
            }
            else {
                foreach (DataRowView incidentView in checkedListBoxIncidents.CheckedItems)
                {
                    IncidentEditForm form = new IncidentEditForm(incidentManagementService, incidentsTable, incidentView);
                    form.Show();
                    break;
                }
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            IncidentCreateForm form = new IncidentCreateForm(incidentManagementService);
            form.Show();
        }


    }
}
