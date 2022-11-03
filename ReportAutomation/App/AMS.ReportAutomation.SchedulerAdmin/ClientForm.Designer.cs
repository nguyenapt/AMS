namespace AMS.ReportAutomation.SchedulerAdmin
{
    partial class ClientsForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grvClient = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grvClientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientEdit = new System.Windows.Forms.DataGridViewImageColumn();
            this.Delete = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnAddClient = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddSite = new System.Windows.Forms.Button();
            this.grvClientSites = new System.Windows.Forms.DataGridView();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn4 = new System.Windows.Forms.DataGridViewImageColumn();
            this.ClientId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grvClientSiteName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Host = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewImageColumn();
            this.Config = new System.Windows.Forms.DataGridViewImageColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvClient)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvClientSites)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(284, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 46);
            this.label1.TabIndex = 1;
            this.label1.Text = "Client Management";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grvClient);
            this.groupBox1.Controls.Add(this.btnAddClient);
            this.groupBox1.Location = new System.Drawing.Point(12, 94);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(301, 380);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Clients";
            // 
            // grvClient
            // 
            this.grvClient.AllowUserToAddRows = false;
            this.grvClient.AllowUserToDeleteRows = false;
            this.grvClient.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grvClient.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.grvClientName,
            this.ClientEdit,
            this.Delete});
            this.grvClient.Location = new System.Drawing.Point(7, 20);
            this.grvClient.Name = "grvClient";
            this.grvClient.Size = new System.Drawing.Size(288, 325);
            this.grvClient.TabIndex = 0;
            this.grvClient.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvClient_CellClick);
            this.grvClient.SelectionChanged += new System.EventHandler(this.grvClient_SelectionChanged);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.Visible = false;
            // 
            // grvClientName
            // 
            this.grvClientName.DataPropertyName = "Name";
            this.grvClientName.HeaderText = "Name";
            this.grvClientName.Name = "grvClientName";
            this.grvClientName.Width = 155;
            // 
            // ClientEdit
            // 
            this.ClientEdit.HeaderText = "Edit";
            this.ClientEdit.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.edit;
            this.ClientEdit.Name = "ClientEdit";
            this.ClientEdit.Width = 45;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "Delete";
            this.Delete.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.delete;
            this.Delete.Name = "Delete";
            this.Delete.Width = 45;
            // 
            // btnAddClient
            // 
            this.btnAddClient.Location = new System.Drawing.Point(222, 350);
            this.btnAddClient.Name = "btnAddClient";
            this.btnAddClient.Size = new System.Drawing.Size(74, 23);
            this.btnAddClient.TabIndex = 1;
            this.btnAddClient.Text = "Add Client";
            this.btnAddClient.UseVisualStyleBackColor = true;
            this.btnAddClient.Click += new System.EventHandler(this.btnAddClient_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAddSite);
            this.groupBox2.Controls.Add(this.grvClientSites);
            this.groupBox2.Location = new System.Drawing.Point(319, 94);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(653, 380);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Client Sites";
            // 
            // btnAddSite
            // 
            this.btnAddSite.Enabled = false;
            this.btnAddSite.Location = new System.Drawing.Point(573, 350);
            this.btnAddSite.Name = "btnAddSite";
            this.btnAddSite.Size = new System.Drawing.Size(75, 23);
            this.btnAddSite.TabIndex = 1;
            this.btnAddSite.Text = "Add Site";
            this.btnAddSite.UseVisualStyleBackColor = true;
            this.btnAddSite.Click += new System.EventHandler(this.btnAddSite_Click);
            // 
            // grvClientSites
            // 
            this.grvClientSites.AllowUserToAddRows = false;
            this.grvClientSites.AllowUserToDeleteRows = false;
            this.grvClientSites.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grvClientSites.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ClientId,
            this.grvClientSiteName,
            this.Host,
            this.Description,
            this.Edit,
            this.Config});
            this.grvClientSites.Location = new System.Drawing.Point(6, 20);
            this.grvClientSites.Name = "grvClientSites";
            this.grvClientSites.ReadOnly = true;
            this.grvClientSites.Size = new System.Drawing.Size(641, 325);
            this.grvClientSites.TabIndex = 0;
            this.grvClientSites.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvClientSites_CellClick);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "Edit";
            this.dataGridViewImageColumn1.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.edit;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.Width = 45;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.HeaderText = "Configuration";
            this.dataGridViewImageColumn2.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.settings;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn2.Width = 45;
            // 
            // dataGridViewImageColumn3
            // 
            this.dataGridViewImageColumn3.HeaderText = "Edit";
            this.dataGridViewImageColumn3.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.edit;
            this.dataGridViewImageColumn3.Name = "dataGridViewImageColumn3";
            this.dataGridViewImageColumn3.ReadOnly = true;
            this.dataGridViewImageColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn3.Width = 45;
            // 
            // dataGridViewImageColumn4
            // 
            this.dataGridViewImageColumn4.HeaderText = "Delete";
            this.dataGridViewImageColumn4.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.delete;
            this.dataGridViewImageColumn4.Name = "dataGridViewImageColumn4";
            this.dataGridViewImageColumn4.ReadOnly = true;
            this.dataGridViewImageColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn4.Width = 45;
            // 
            // ClientId
            // 
            this.ClientId.DataPropertyName = "Id";
            this.ClientId.HeaderText = "Id";
            this.ClientId.Name = "ClientId";
            this.ClientId.ReadOnly = true;
            this.ClientId.Visible = false;
            // 
            // grvClientSiteName
            // 
            this.grvClientSiteName.DataPropertyName = "Name";
            this.grvClientSiteName.HeaderText = "Name";
            this.grvClientSiteName.Name = "grvClientSiteName";
            this.grvClientSiteName.ReadOnly = true;
            this.grvClientSiteName.Width = 150;
            // 
            // Host
            // 
            this.Host.DataPropertyName = "SiteUrl";
            this.Host.HeaderText = "SiteUrl";
            this.Host.Name = "Host";
            this.Host.ReadOnly = true;
            this.Host.Width = 150;
            // 
            // Description
            // 
            this.Description.DataPropertyName = "Description";
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 190;
            // 
            // Edit
            // 
            this.Edit.HeaderText = "Edit";
            this.Edit.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.edit;
            this.Edit.Name = "Edit";
            this.Edit.ReadOnly = true;
            this.Edit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Edit.Width = 45;
            // 
            // Config
            // 
            this.Config.HeaderText = "Delete";
            this.Config.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.delete;
            this.Config.Name = "Config";
            this.Config.ReadOnly = true;
            this.Config.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Config.Width = 45;
            // 
            // ClientsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 531);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "ClientsForm";
            this.ShowIcon = false;
            this.Text = "Client Administrator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientsForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grvClient)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grvClientSites)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAddSite;
        private System.Windows.Forms.DataGridView grvClientSites;
        private System.Windows.Forms.DataGridView grvClient;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.Button btnAddClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn grvClientName;
        private System.Windows.Forms.DataGridViewImageColumn ClientEdit;
        private System.Windows.Forms.DataGridViewImageColumn Delete;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn3;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientId;
        private System.Windows.Forms.DataGridViewTextBoxColumn grvClientSiteName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Host;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewImageColumn Edit;
        private System.Windows.Forms.DataGridViewImageColumn Config;
    }
}