namespace AMS.ReportAutomation.SchedulerAdmin
{
    partial class AddClientSiteForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblClient = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCrawlerConfig = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTemplateScreamingFrog = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.btnTemplatePSI = new System.Windows.Forms.Button();
            this.btnPreviewCrawlerConfig = new System.Windows.Forms.Button();
            this.btnSelectFolderGC = new System.Windows.Forms.Button();
            this.btnLoginOauthGC = new System.Windows.Forms.Button();
            this.txtFileSecretPathGC = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBrandName = new System.Windows.Forms.TextBox();
            this.btnSelectFolderGA = new System.Windows.Forms.Button();
            this.btnLoginOauthGA = new System.Windows.Forms.Button();
            this.grvCrawlerConfig = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtSiteUrl = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFileSecretPathGA = new System.Windows.Forms.TextBox();
            this.txtLogoUrl = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboKey = new System.Windows.Forms.ComboBox();
            this.grvTools = new System.Windows.Forms.DataGridView();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewImageColumn();
            this.Delete = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnAddTool = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.cbReportActive = new System.Windows.Forms.CheckBox();
            this.txtReportName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnTemplateAll = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvCrawlerConfig)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvTools)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Client:";
            // 
            // lblClient
            // 
            this.lblClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClient.Location = new System.Drawing.Point(82, 16);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(289, 22);
            this.lblClient.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name: *";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(92, 47);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(294, 20);
            this.txtName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(389, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(452, 46);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(375, 20);
            this.txtDescription.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Crawler config:";
            // 
            // txtCrawlerConfig
            // 
            this.txtCrawlerConfig.Location = new System.Drawing.Point(92, 101);
            this.txtCrawlerConfig.Multiline = true;
            this.txtCrawlerConfig.Name = "txtCrawlerConfig";
            this.txtCrawlerConfig.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCrawlerConfig.Size = new System.Drawing.Size(431, 152);
            this.txtCrawlerConfig.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTemplateAll);
            this.groupBox1.Controls.Add(this.btnTemplateScreamingFrog);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.btnTemplatePSI);
            this.groupBox1.Controls.Add(this.btnPreviewCrawlerConfig);
            this.groupBox1.Controls.Add(this.btnSelectFolderGC);
            this.groupBox1.Controls.Add(this.btnLoginOauthGC);
            this.groupBox1.Controls.Add(this.txtFileSecretPathGC);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtBrandName);
            this.groupBox1.Controls.Add(this.btnSelectFolderGA);
            this.groupBox1.Controls.Add(this.btnLoginOauthGA);
            this.groupBox1.Controls.Add(this.grvCrawlerConfig);
            this.groupBox1.Controls.Add(this.txtSiteUrl);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtFileSecretPathGA);
            this.groupBox1.Controls.Add(this.txtLogoUrl);
            this.groupBox1.Controls.Add(this.lblClient);
            this.groupBox1.Controls.Add(this.txtCrawlerConfig);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(957, 316);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Client site";
            // 
            // btnTemplateScreamingFrog
            // 
            this.btnTemplateScreamingFrog.Location = new System.Drawing.Point(37, 181);
            this.btnTemplateScreamingFrog.Name = "btnTemplateScreamingFrog";
            this.btnTemplateScreamingFrog.Size = new System.Drawing.Size(49, 23);
            this.btnTemplateScreamingFrog.TabIndex = 23;
            this.btnTemplateScreamingFrog.Text = "SFrog";
            this.btnTemplateScreamingFrog.UseVisualStyleBackColor = true;
            this.btnTemplateScreamingFrog.Click += new System.EventHandler(this.btnTemplateScreamingFrog_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(34, 139);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 13);
            this.label13.TabIndex = 22;
            this.label13.Text = "Templates";
            // 
            // btnTemplatePSI
            // 
            this.btnTemplatePSI.Location = new System.Drawing.Point(37, 155);
            this.btnTemplatePSI.Name = "btnTemplatePSI";
            this.btnTemplatePSI.Size = new System.Drawing.Size(49, 23);
            this.btnTemplatePSI.TabIndex = 21;
            this.btnTemplatePSI.Text = "PSI";
            this.btnTemplatePSI.UseVisualStyleBackColor = true;
            this.btnTemplatePSI.Click += new System.EventHandler(this.btnTemplatePSI_Click);
            // 
            // btnPreviewCrawlerConfig
            // 
            this.btnPreviewCrawlerConfig.Location = new System.Drawing.Point(523, 100);
            this.btnPreviewCrawlerConfig.Name = "btnPreviewCrawlerConfig";
            this.btnPreviewCrawlerConfig.Size = new System.Drawing.Size(75, 23);
            this.btnPreviewCrawlerConfig.TabIndex = 11;
            this.btnPreviewCrawlerConfig.Text = "Preview =>";
            this.btnPreviewCrawlerConfig.UseVisualStyleBackColor = true;
            this.btnPreviewCrawlerConfig.Click += new System.EventHandler(this.btnPreviewCrawlerConfig_Click);
            // 
            // btnSelectFolderGC
            // 
            this.btnSelectFolderGC.Location = new System.Drawing.Point(756, 284);
            this.btnSelectFolderGC.Name = "btnSelectFolderGC";
            this.btnSelectFolderGC.Size = new System.Drawing.Size(34, 23);
            this.btnSelectFolderGC.TabIndex = 20;
            this.btnSelectFolderGC.Text = "...";
            this.btnSelectFolderGC.UseVisualStyleBackColor = true;
            this.btnSelectFolderGC.Click += new System.EventHandler(this.btnSelectFolderGC_Click);
            // 
            // btnLoginOauthGC
            // 
            this.btnLoginOauthGC.Enabled = false;
            this.btnLoginOauthGC.Location = new System.Drawing.Point(795, 285);
            this.btnLoginOauthGC.Name = "btnLoginOauthGC";
            this.btnLoginOauthGC.Size = new System.Drawing.Size(156, 22);
            this.btnLoginOauthGC.TabIndex = 19;
            this.btnLoginOauthGC.Text = "Login Oauth && Save Secret";
            this.btnLoginOauthGC.UseVisualStyleBackColor = true;
            this.btnLoginOauthGC.Click += new System.EventHandler(this.btnLoginOauthGC_Click);
            // 
            // txtFileSecretPathGC
            // 
            this.txtFileSecretPathGC.Enabled = false;
            this.txtFileSecretPathGC.Location = new System.Drawing.Point(293, 286);
            this.txtFileSecretPathGC.Name = "txtFileSecretPathGC";
            this.txtFileSecretPathGC.ReadOnly = true;
            this.txtFileSecretPathGC.Size = new System.Drawing.Size(463, 20);
            this.txtFileSecretPathGC.TabIndex = 18;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 289);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(274, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Google Search Console OAuth Secret saved folder path:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(389, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "Brand name:";
            // 
            // txtBrandName
            // 
            this.txtBrandName.Location = new System.Drawing.Point(462, 16);
            this.txtBrandName.Name = "txtBrandName";
            this.txtBrandName.Size = new System.Drawing.Size(284, 20);
            this.txtBrandName.TabIndex = 16;
            // 
            // btnSelectFolderGA
            // 
            this.btnSelectFolderGA.Location = new System.Drawing.Point(756, 258);
            this.btnSelectFolderGA.Name = "btnSelectFolderGA";
            this.btnSelectFolderGA.Size = new System.Drawing.Size(34, 23);
            this.btnSelectFolderGA.TabIndex = 14;
            this.btnSelectFolderGA.Text = "...";
            this.btnSelectFolderGA.UseVisualStyleBackColor = true;
            this.btnSelectFolderGA.Click += new System.EventHandler(this.btnSelectFolderGA_Click);
            // 
            // btnLoginOauthGA
            // 
            this.btnLoginOauthGA.Enabled = false;
            this.btnLoginOauthGA.Location = new System.Drawing.Point(795, 259);
            this.btnLoginOauthGA.Name = "btnLoginOauthGA";
            this.btnLoginOauthGA.Size = new System.Drawing.Size(156, 22);
            this.btnLoginOauthGA.TabIndex = 13;
            this.btnLoginOauthGA.Text = "Login Oauth && Save Secret";
            this.btnLoginOauthGA.UseVisualStyleBackColor = true;
            this.btnLoginOauthGA.Click += new System.EventHandler(this.btnLoginOauthGA_Click);
            // 
            // grvCrawlerConfig
            // 
            this.grvCrawlerConfig.AllowUserToAddRows = false;
            this.grvCrawlerConfig.AllowUserToDeleteRows = false;
            this.grvCrawlerConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grvCrawlerConfig.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.grvCrawlerConfig.Location = new System.Drawing.Point(598, 101);
            this.grvCrawlerConfig.Name = "grvCrawlerConfig";
            this.grvCrawlerConfig.ReadOnly = true;
            this.grvCrawlerConfig.Size = new System.Drawing.Size(353, 152);
            this.grvCrawlerConfig.TabIndex = 12;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Key";
            this.dataGridViewTextBoxColumn1.HeaderText = "Key";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 86;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Value";
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 230;
            // 
            // txtSiteUrl
            // 
            this.txtSiteUrl.Location = new System.Drawing.Point(452, 72);
            this.txtSiteUrl.Name = "txtSiteUrl";
            this.txtSiteUrl.Size = new System.Drawing.Size(375, 20);
            this.txtSiteUrl.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Logo Url:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(389, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Site Url: *";
            // 
            // txtFileSecretPathGA
            // 
            this.txtFileSecretPathGA.Enabled = false;
            this.txtFileSecretPathGA.Location = new System.Drawing.Point(293, 260);
            this.txtFileSecretPathGA.Name = "txtFileSecretPathGA";
            this.txtFileSecretPathGA.ReadOnly = true;
            this.txtFileSecretPathGA.Size = new System.Drawing.Size(463, 20);
            this.txtFileSecretPathGA.TabIndex = 6;
            // 
            // txtLogoUrl
            // 
            this.txtLogoUrl.Location = new System.Drawing.Point(92, 73);
            this.txtLogoUrl.Name = "txtLogoUrl";
            this.txtLogoUrl.Size = new System.Drawing.Size(294, 20);
            this.txtLogoUrl.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 263);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(241, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Google Analytics OAuth Secret saved folder path:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.cbReportActive);
            this.groupBox2.Controls.Add(this.txtReportName);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(13, 332);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(956, 235);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Report";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cboKey);
            this.groupBox3.Controls.Add(this.grvTools);
            this.groupBox3.Controls.Add(this.btnAddTool);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtValue);
            this.groupBox3.Location = new System.Drawing.Point(6, 44);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(944, 186);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Report sections: *";
            // 
            // cboKey
            // 
            this.cboKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboKey.FormattingEnabled = true;
            this.cboKey.Location = new System.Drawing.Point(50, 19);
            this.cboKey.Name = "cboKey";
            this.cboKey.Size = new System.Drawing.Size(239, 21);
            this.cboKey.TabIndex = 11;
            // 
            // grvTools
            // 
            this.grvTools.AllowUserToAddRows = false;
            this.grvTools.AllowUserToDeleteRows = false;
            this.grvTools.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grvTools.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Key,
            this.Value,
            this.Edit,
            this.Delete});
            this.grvTools.Location = new System.Drawing.Point(10, 49);
            this.grvTools.Name = "grvTools";
            this.grvTools.Size = new System.Drawing.Size(928, 131);
            this.grvTools.TabIndex = 11;
            this.grvTools.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvTools_CellContentClick);
            // 
            // Key
            // 
            this.Key.DataPropertyName = "Key";
            this.Key.HeaderText = "Key";
            this.Key.Name = "Key";
            this.Key.Width = 150;
            // 
            // Value
            // 
            this.Value.DataPropertyName = "Value";
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.Width = 230;
            // 
            // Edit
            // 
            this.Edit.HeaderText = "Edit";
            this.Edit.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.edit;
            this.Edit.Name = "Edit";
            this.Edit.Width = 50;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "Delete";
            this.Delete.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.delete;
            this.Delete.Name = "Delete";
            this.Delete.Width = 50;
            // 
            // btnAddTool
            // 
            this.btnAddTool.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.plus;
            this.btnAddTool.Location = new System.Drawing.Point(613, 16);
            this.btnAddTool.Name = "btnAddTool";
            this.btnAddTool.Size = new System.Drawing.Size(33, 23);
            this.btnAddTool.TabIndex = 10;
            this.btnAddTool.UseVisualStyleBackColor = true;
            this.btnAddTool.Click += new System.EventHandler(this.btnAddTool_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(321, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Value:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Key:";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(364, 18);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(243, 20);
            this.txtValue.TabIndex = 7;
            // 
            // cbReportActive
            // 
            this.cbReportActive.AutoSize = true;
            this.cbReportActive.Location = new System.Drawing.Point(327, 21);
            this.cbReportActive.Name = "cbReportActive";
            this.cbReportActive.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbReportActive.Size = new System.Drawing.Size(56, 17);
            this.cbReportActive.TabIndex = 5;
            this.cbReportActive.Text = "Active";
            this.cbReportActive.UseVisualStyleBackColor = true;
            // 
            // txtReportName
            // 
            this.txtReportName.Location = new System.Drawing.Point(57, 19);
            this.txtReportName.Name = "txtReportName";
            this.txtReportName.Size = new System.Drawing.Size(239, 20);
            this.txtReportName.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Name: *";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(810, 571);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(891, 571);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnTemplateAll
            // 
            this.btnTemplateAll.Location = new System.Drawing.Point(37, 210);
            this.btnTemplateAll.Name = "btnTemplateAll";
            this.btnTemplateAll.Size = new System.Drawing.Size(49, 23);
            this.btnTemplateAll.TabIndex = 24;
            this.btnTemplateAll.Text = "All";
            this.btnTemplateAll.UseVisualStyleBackColor = true;
            this.btnTemplateAll.Click += new System.EventHandler(this.btnTemplateAll_Click);
            // 
            // AddClientSiteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 597);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddClientSiteForm";
            this.ShowIcon = false;
            this.Text = "Client Site Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddClientSiteForm_FormClosing);
            this.Load += new System.EventHandler(this.AddClientSiteForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvCrawlerConfig)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvTools)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCrawlerConfig;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.CheckBox cbReportActive;
        private System.Windows.Forms.TextBox txtReportName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAddTool;
        private System.Windows.Forms.DataGridView grvTools;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewImageColumn Edit;
        private System.Windows.Forms.DataGridViewImageColumn Delete;
        private System.Windows.Forms.ComboBox cboKey;
        private System.Windows.Forms.TextBox txtSiteUrl;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtLogoUrl;
        private System.Windows.Forms.DataGridView grvCrawlerConfig;
        private System.Windows.Forms.Button btnLoginOauthGA;
        private System.Windows.Forms.TextBox txtFileSecretPathGA;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnSelectFolderGA;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtBrandName;
        private System.Windows.Forms.Button btnSelectFolderGC;
        private System.Windows.Forms.Button btnLoginOauthGC;
        private System.Windows.Forms.TextBox txtFileSecretPathGC;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Button btnPreviewCrawlerConfig;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnTemplatePSI;
        private System.Windows.Forms.Button btnTemplateScreamingFrog;
        private System.Windows.Forms.Button btnTemplateAll;
    }
}