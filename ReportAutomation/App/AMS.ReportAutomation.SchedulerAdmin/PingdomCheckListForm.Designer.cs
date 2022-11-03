namespace AMS.ReportAutomation.SchedulerAdmin
{
    partial class PingdomCheckListForm
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
            this.grvPingdomCheck = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PingdomCheckName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HostName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grvPingdomCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // grvPingdomCheck
            // 
            this.grvPingdomCheck.AllowUserToAddRows = false;
            this.grvPingdomCheck.AllowUserToDeleteRows = false;
            this.grvPingdomCheck.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grvPingdomCheck.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.PingdomCheckName,
            this.HostName});
            this.grvPingdomCheck.Location = new System.Drawing.Point(13, 12);
            this.grvPingdomCheck.Name = "grvPingdomCheck";
            this.grvPingdomCheck.Size = new System.Drawing.Size(559, 306);
            this.grvPingdomCheck.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(497, 324);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.Width = 150;
            // 
            // PingdomCheckName
            // 
            this.PingdomCheckName.DataPropertyName = "Name";
            this.PingdomCheckName.HeaderText = "Name";
            this.PingdomCheckName.Name = "PingdomCheckName";
            this.PingdomCheckName.Width = 150;
            // 
            // HostName
            // 
            this.HostName.DataPropertyName = "HostName";
            this.HostName.HeaderText = "HostName";
            this.HostName.Name = "HostName";
            this.HostName.Width = 180;
            // 
            // PingdomCheckListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 372);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grvPingdomCheck);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PingdomCheckListForm";
            this.Text = "Pingdom Check List";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PingdomCheckListForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.grvPingdomCheck)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grvPingdomCheck;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn PingdomCheckName;
        private System.Windows.Forms.DataGridViewTextBoxColumn HostName;
    }
}