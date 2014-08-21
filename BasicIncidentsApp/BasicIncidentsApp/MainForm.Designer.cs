namespace BasicIncidentsApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkedListBoxIncidents = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonStop = new System.Windows.Forms.Button();
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxProtocol = new System.Windows.Forms.GroupBox();
            this.radioButtonHTTPS = new System.Windows.Forms.RadioButton();
            this.radioButtonHTTP = new System.Windows.Forms.RadioButton();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.backgroundPoller = new System.ComponentModel.BackgroundWorker();
            this.buttonEditIncident = new System.Windows.Forms.Button();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBoxProtocol.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkedListBoxIncidents
            // 
            this.checkedListBoxIncidents.CheckOnClick = true;
            this.checkedListBoxIncidents.FormattingEnabled = true;
            this.checkedListBoxIncidents.Location = new System.Drawing.Point(553, 32);
            this.checkedListBoxIncidents.Name = "checkedListBoxIncidents";
            this.checkedListBoxIncidents.Size = new System.Drawing.Size(371, 409);
            this.checkedListBoxIncidents.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(550, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Incidents";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonStop);
            this.groupBox1.Controls.Add(this.textBoxServer);
            this.groupBox1.Controls.Add(this.buttonStart);
            this.groupBox1.Controls.Add(this.textBoxUser);
            this.groupBox1.Controls.Add(this.textBoxPassword);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.groupBoxProtocol);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(484, 233);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "UICDS Server Information";
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(269, 193);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 5;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // textBoxServer
            // 
            this.textBoxServer.Location = new System.Drawing.Point(76, 88);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.Size = new System.Drawing.Size(381, 20);
            this.textBoxServer.TabIndex = 1;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(158, 193);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 4;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textBoxUser
            // 
            this.textBoxUser.Location = new System.Drawing.Point(76, 117);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(187, 20);
            this.textBoxUser.TabIndex = 2;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(76, 149);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(187, 20);
            this.textBoxPassword.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "User";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Server";
            // 
            // groupBoxProtocol
            // 
            this.groupBoxProtocol.Controls.Add(this.radioButtonHTTPS);
            this.groupBoxProtocol.Controls.Add(this.radioButtonHTTP);
            this.groupBoxProtocol.Location = new System.Drawing.Point(6, 19);
            this.groupBoxProtocol.Name = "groupBoxProtocol";
            this.groupBoxProtocol.Size = new System.Drawing.Size(138, 47);
            this.groupBoxProtocol.TabIndex = 0;
            this.groupBoxProtocol.TabStop = false;
            this.groupBoxProtocol.Text = "Protocol";
            // 
            // radioButtonHTTPS
            // 
            this.radioButtonHTTPS.AutoSize = true;
            this.radioButtonHTTPS.Location = new System.Drawing.Point(70, 20);
            this.radioButtonHTTPS.Name = "radioButtonHTTPS";
            this.radioButtonHTTPS.Size = new System.Drawing.Size(61, 17);
            this.radioButtonHTTPS.TabIndex = 1;
            this.radioButtonHTTPS.TabStop = true;
            this.radioButtonHTTPS.Text = "https://";
            this.radioButtonHTTPS.UseVisualStyleBackColor = true;
            // 
            // radioButtonHTTP
            // 
            this.radioButtonHTTP.AutoSize = true;
            this.radioButtonHTTP.Location = new System.Drawing.Point(7, 20);
            this.radioButtonHTTP.Name = "radioButtonHTTP";
            this.radioButtonHTTP.Size = new System.Drawing.Size(56, 17);
            this.radioButtonHTTP.TabIndex = 0;
            this.radioButtonHTTP.TabStop = true;
            this.radioButtonHTTP.Text = "http://";
            this.radioButtonHTTP.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(553, 447);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(187, 23);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Close and Archive Selected";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(849, 447);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 4;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonPoll_Click);
            // 
            // backgroundPoller
            // 
            this.backgroundPoller.WorkerReportsProgress = true;
            this.backgroundPoller.WorkerSupportsCancellation = true;
            this.backgroundPoller.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundPoller_DoWork);
            this.backgroundPoller.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundPoller_RunWorkerCompleted);
            // 
            // buttonEditIncident
            // 
            this.buttonEditIncident.Location = new System.Drawing.Point(746, 447);
            this.buttonEditIncident.Name = "buttonEditIncident";
            this.buttonEditIncident.Size = new System.Drawing.Size(97, 23);
            this.buttonEditIncident.TabIndex = 5;
            this.buttonEditIncident.Text = "View Selected";
            this.buttonEditIncident.UseVisualStyleBackColor = true;
            this.buttonEditIncident.Click += new System.EventHandler(this.buttonEditIncident_Click);
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(206, 352);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(113, 23);
            this.buttonCreate.TabIndex = 6;
            this.buttonCreate.Text = "Create Incident";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 480);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.buttonEditIncident);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedListBoxIncidents);
            this.Name = "MainForm";
            this.Text = "Basic Incidents App";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxProtocol.ResumeLayout(false);
            this.groupBoxProtocol.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxIncidents;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxProtocol;
        private System.Windows.Forms.RadioButton radioButtonHTTPS;
        private System.Windows.Forms.RadioButton radioButtonHTTP;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonRefresh;
        private System.ComponentModel.BackgroundWorker backgroundPoller;
        private System.Windows.Forms.Button buttonEditIncident;
        private System.Windows.Forms.Button buttonCreate;
    }
}

