namespace AMS.ReportAutomation.SchedulerAdmin
{
    partial class EditServiceDeskForm
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
            this.txtProjectKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtServiceDeskUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOrgName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOrgNameIfNotProvided = new System.Windows.Forms.TextBox();
            this.btnSaveServiceDesk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtProjectKey
            // 
            this.txtProjectKey.Location = new System.Drawing.Point(192, 78);
            this.txtProjectKey.Name = "txtProjectKey";
            this.txtProjectKey.Size = new System.Drawing.Size(294, 20);
            this.txtProjectKey.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Project Key:";
            // 
            // txtServiceDeskUrl
            // 
            this.txtServiceDeskUrl.Location = new System.Drawing.Point(192, 45);
            this.txtServiceDeskUrl.Name = "txtServiceDeskUrl";
            this.txtServiceDeskUrl.Size = new System.Drawing.Size(294, 20);
            this.txtServiceDeskUrl.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Service Desk Url:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Organization Name:";
            // 
            // txtOrgName
            // 
            this.txtOrgName.Location = new System.Drawing.Point(192, 113);
            this.txtOrgName.Name = "txtOrgName";
            this.txtOrgName.Size = new System.Drawing.Size(294, 20);
            this.txtOrgName.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(168, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "OrgNameForProjectIfNotProvided:";
            // 
            // txtOrgNameIfNotProvided
            // 
            this.txtOrgNameIfNotProvided.Location = new System.Drawing.Point(192, 146);
            this.txtOrgNameIfNotProvided.Name = "txtOrgNameIfNotProvided";
            this.txtOrgNameIfNotProvided.Size = new System.Drawing.Size(294, 20);
            this.txtOrgNameIfNotProvided.TabIndex = 4;
            // 
            // btnSaveServiceDesk
            // 
            this.btnSaveServiceDesk.Location = new System.Drawing.Point(330, 173);
            this.btnSaveServiceDesk.Name = "btnSaveServiceDesk";
            this.btnSaveServiceDesk.Size = new System.Drawing.Size(75, 23);
            this.btnSaveServiceDesk.TabIndex = 5;
            this.btnSaveServiceDesk.Text = "Save";
            this.btnSaveServiceDesk.UseVisualStyleBackColor = true;
            this.btnSaveServiceDesk.Click += new System.EventHandler(this.btnSaveServiceDesk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(411, 173);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // EditServiceDeskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSaveServiceDesk);
            this.Controls.Add(this.txtOrgNameIfNotProvided);
            this.Controls.Add(this.txtProjectKey);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtOrgName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtServiceDeskUrl);
            this.Controls.Add(this.label1);
            this.Name = "EditServiceDeskForm";
            this.Text = "Service Desk";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditServiceDeskForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtProjectKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServiceDeskUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOrgName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOrgNameIfNotProvided;
        private System.Windows.Forms.Button btnSaveServiceDesk;
        private System.Windows.Forms.Button btnCancel;
    }
}