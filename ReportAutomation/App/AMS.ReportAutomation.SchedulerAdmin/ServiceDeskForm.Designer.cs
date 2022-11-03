namespace AMS.ReportAutomation.SchedulerAdmin
{
    partial class ServiceDeskForm
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
            this.grvServiceDesk = new System.Windows.Forms.DataGridView();
            this.btnAddServiceDesk = new System.Windows.Forms.Button();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceDeskUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrganizationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrgNameForProjectIfNotProvided = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Edit = new System.Windows.Forms.DataGridViewImageColumn();
            this.Delete = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grvServiceDesk)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(333, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(255, 46);
            this.label1.TabIndex = 2;
            this.label1.Text = "Service Desk";
            // 
            // grvServiceDesk
            // 
            this.grvServiceDesk.AllowUserToAddRows = false;
            this.grvServiceDesk.AllowUserToDeleteRows = false;
            this.grvServiceDesk.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grvServiceDesk.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.ServiceDeskUrl,
            this.ProjectKey,
            this.OrganizationName,
            this.OrgNameForProjectIfNotProvided,
            this.Edit,
            this.Delete});
            this.grvServiceDesk.Location = new System.Drawing.Point(12, 108);
            this.grvServiceDesk.Name = "grvServiceDesk";
            this.grvServiceDesk.Size = new System.Drawing.Size(910, 326);
            this.grvServiceDesk.TabIndex = 1;
            this.grvServiceDesk.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvServiceDesk_CellClick);
            // 
            // btnAddServiceDesk
            // 
            this.btnAddServiceDesk.Location = new System.Drawing.Point(820, 440);
            this.btnAddServiceDesk.Name = "btnAddServiceDesk";
            this.btnAddServiceDesk.Size = new System.Drawing.Size(102, 23);
            this.btnAddServiceDesk.TabIndex = 2;
            this.btnAddServiceDesk.Text = "Add Service Desk";
            this.btnAddServiceDesk.UseVisualStyleBackColor = true;
            this.btnAddServiceDesk.Click += new System.EventHandler(this.btnAddServiceDesk_Click);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // ServiceDeskUrl
            // 
            this.ServiceDeskUrl.DataPropertyName = "ServiceDeskUrl";
            this.ServiceDeskUrl.HeaderText = "ServiceDeskUrl";
            this.ServiceDeskUrl.Name = "ServiceDeskUrl";
            this.ServiceDeskUrl.ReadOnly = true;
            this.ServiceDeskUrl.Width = 200;
            // 
            // ProjectKey
            // 
            this.ProjectKey.DataPropertyName = "ProjectKey";
            this.ProjectKey.HeaderText = "Project Key";
            this.ProjectKey.Name = "ProjectKey";
            this.ProjectKey.ReadOnly = true;
            this.ProjectKey.Width = 90;
            // 
            // OrganizationName
            // 
            this.OrganizationName.DataPropertyName = "OrganizationName";
            this.OrganizationName.HeaderText = "Organization Name";
            this.OrganizationName.Name = "OrganizationName";
            this.OrganizationName.ReadOnly = true;
            this.OrganizationName.Width = 130;
            // 
            // OrgNameForProjectIfNotProvided
            // 
            this.OrgNameForProjectIfNotProvided.DataPropertyName = "OrgNameForProjectIfNotProvided";
            this.OrgNameForProjectIfNotProvided.HeaderText = "Organization Name for Project if not Provided";
            this.OrgNameForProjectIfNotProvided.Name = "OrgNameForProjectIfNotProvided";
            this.OrgNameForProjectIfNotProvided.ReadOnly = true;
            this.OrgNameForProjectIfNotProvided.Width = 250;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "Edit";
            this.dataGridViewImageColumn1.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.edit;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Width = 45;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.HeaderText = "Delete";
            this.dataGridViewImageColumn2.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.delete;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.ReadOnly = true;
            this.dataGridViewImageColumn2.Width = 45;
            // 
            // Edit
            // 
            this.Edit.HeaderText = "Edit";
            this.Edit.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.edit;
            this.Edit.Name = "Edit";
            this.Edit.ReadOnly = true;
            this.Edit.Width = 45;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "Delete";
            this.Delete.Image = global::AMS.ReportAutomation.SchedulerAdmin.Properties.Resources.delete;
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Width = 45;
            // 
            // ServiceDeskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 531);
            this.Controls.Add(this.btnAddServiceDesk);
            this.Controls.Add(this.grvServiceDesk);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "ServiceDeskForm";
            this.ShowIcon = false;
            this.Text = "Service Desk";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServiceDeskForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.grvServiceDesk)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView grvServiceDesk;
        private System.Windows.Forms.Button btnAddServiceDesk;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceDeskUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrganizationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrgNameForProjectIfNotProvided;
        private System.Windows.Forms.DataGridViewImageColumn Edit;
        private System.Windows.Forms.DataGridViewImageColumn Delete;
    }
}