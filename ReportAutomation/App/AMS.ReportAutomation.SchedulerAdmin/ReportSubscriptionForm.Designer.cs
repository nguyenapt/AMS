namespace AMS.ReportAutomation.SchedulerAdmin
{
    partial class ReportSubscriptionForm
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
            this.grvReportSubscription = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReportId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReportName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceiverEmails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Frequency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubscribedByUser = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewImageColumn();
            this.Delete = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grvReportSubscription)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(291, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(372, 46);
            this.label1.TabIndex = 2;
            this.label1.Text = "Report Subscription";
            // 
            // grvReportSubscription
            // 
            this.grvReportSubscription.AllowUserToAddRows = false;
            this.grvReportSubscription.AllowUserToDeleteRows = false;
            this.grvReportSubscription.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grvReportSubscription.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.ReportId,
            this.ReportName,
            this.ReceiverEmails,
            this.Frequency,
            this.SubscribedByUser,
            this.Edit,
            this.Delete});
            this.grvReportSubscription.Location = new System.Drawing.Point(12, 108);
            this.grvReportSubscription.Name = "grvReportSubscription";
            this.grvReportSubscription.Size = new System.Drawing.Size(910, 326);
            this.grvReportSubscription.TabIndex = 1;
            this.grvReportSubscription.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvReportSubscription_CellClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(820, 440);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(102, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add Subscription";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
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
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // ReportId
            // 
            this.ReportId.DataPropertyName = "ReportId";
            this.ReportId.HeaderText = "Report Id";
            this.ReportId.Name = "ReportId";
            this.ReportId.ReadOnly = true;
            // 
            // ReportName
            // 
            this.ReportName.DataPropertyName = "ReportName";
            this.ReportName.FillWeight = 150F;
            this.ReportName.HeaderText = "Report Name";
            this.ReportName.Name = "ReportName";
            this.ReportName.ReadOnly = true;
            this.ReportName.Width = 150;
            // 
            // ReceiverEmails
            // 
            this.ReceiverEmails.DataPropertyName = "ReceiverEmails";
            this.ReceiverEmails.HeaderText = "Receiver Emails";
            this.ReceiverEmails.Name = "ReceiverEmails";
            this.ReceiverEmails.ReadOnly = true;
            this.ReceiverEmails.Width = 150;
            // 
            // Frequency
            // 
            this.Frequency.DataPropertyName = "Frequency";
            this.Frequency.HeaderText = "Frequency";
            this.Frequency.Name = "Frequency";
            this.Frequency.ReadOnly = true;
            // 
            // SubscribedByUser
            // 
            this.SubscribedByUser.DataPropertyName = "SubscribedByUser";
            this.SubscribedByUser.HeaderText = "Subscribed By User";
            this.SubscribedByUser.Name = "SubscribedByUser";
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
            // ReportSubscriptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 531);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.grvReportSubscription);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "ReportSubscriptionForm";
            this.ShowIcon = false;
            this.Text = "Report subscription";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReportSubscriptionForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.grvReportSubscription)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView grvReportSubscription;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReportId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReportName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceiverEmails;
        private System.Windows.Forms.DataGridViewTextBoxColumn Frequency;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SubscribedByUser;
        private System.Windows.Forms.DataGridViewImageColumn Edit;
        private System.Windows.Forms.DataGridViewImageColumn Delete;
    }
}