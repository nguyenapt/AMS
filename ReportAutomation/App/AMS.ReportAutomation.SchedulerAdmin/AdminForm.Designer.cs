namespace AMS.ReportAutomation.SchedulerAdmin
{
    partial class AdminForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.SchedulerToolBar = new System.Windows.Forms.ToolStripButton();
            this.ClientToolBar = new System.Windows.Forms.ToolStripButton();
            this.ServiceDeskToolBar = new System.Windows.Forms.ToolStripButton();
            this.PingdomCheckToolBar = new System.Windows.Forms.ToolStripButton();
            this.ReportSubscriptionButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 739);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1084, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel.Text = "Status";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SchedulerToolBar,
            this.ClientToolBar,
            this.ServiceDeskToolBar,
            this.PingdomCheckToolBar,
            this.ReportSubscriptionButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1084, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "ToolStrip";
            // 
            // SchedulerToolBar
            // 
            this.SchedulerToolBar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SchedulerToolBar.Image = ((System.Drawing.Image)(resources.GetObject("SchedulerToolBar.Image")));
            this.SchedulerToolBar.ImageTransparentColor = System.Drawing.Color.Black;
            this.SchedulerToolBar.Name = "SchedulerToolBar";
            this.SchedulerToolBar.Size = new System.Drawing.Size(63, 22);
            this.SchedulerToolBar.Text = "Scheduler";
            this.SchedulerToolBar.ToolTipText = "Scheduler";
            this.SchedulerToolBar.Click += new System.EventHandler(this.SchedulerToolBar_Click);
            // 
            // ClientToolBar
            // 
            this.ClientToolBar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ClientToolBar.Image = ((System.Drawing.Image)(resources.GetObject("ClientToolBar.Image")));
            this.ClientToolBar.ImageTransparentColor = System.Drawing.Color.Black;
            this.ClientToolBar.Name = "ClientToolBar";
            this.ClientToolBar.Size = new System.Drawing.Size(42, 22);
            this.ClientToolBar.Text = "Client";
            this.ClientToolBar.Click += new System.EventHandler(this.ClientToolBar_Click);
            // 
            // ServiceDeskToolBar
            // 
            this.ServiceDeskToolBar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ServiceDeskToolBar.Image = ((System.Drawing.Image)(resources.GetObject("ServiceDeskToolBar.Image")));
            this.ServiceDeskToolBar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ServiceDeskToolBar.Name = "ServiceDeskToolBar";
            this.ServiceDeskToolBar.Size = new System.Drawing.Size(76, 22);
            this.ServiceDeskToolBar.Text = "Service Desk";
            this.ServiceDeskToolBar.Click += new System.EventHandler(this.ServiceDeskToolBar_Click);
            // 
            // PingdomCheckToolBar
            // 
            this.PingdomCheckToolBar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.PingdomCheckToolBar.Image = ((System.Drawing.Image)(resources.GetObject("PingdomCheckToolBar.Image")));
            this.PingdomCheckToolBar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PingdomCheckToolBar.Name = "PingdomCheckToolBar";
            this.PingdomCheckToolBar.Size = new System.Drawing.Size(112, 22);
            this.PingdomCheckToolBar.Text = "Pingdom check list";
            this.PingdomCheckToolBar.Click += new System.EventHandler(this.PingdomCheckToolBar_Click);
            // 
            // ReportSubscriptionButton
            // 
            this.ReportSubscriptionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ReportSubscriptionButton.Image = ((System.Drawing.Image)(resources.GetObject("ReportSubscriptionButton.Image")));
            this.ReportSubscriptionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ReportSubscriptionButton.Name = "ReportSubscriptionButton";
            this.ReportSubscriptionButton.Size = new System.Drawing.Size(114, 22);
            this.ReportSubscriptionButton.Text = "Report subscription";
            this.ReportSubscriptionButton.Click += new System.EventHandler(this.ReportSubscriptionButton_Click);
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 761);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "AdminForm";
            this.Text = "Admin Configuration";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripButton SchedulerToolBar;
        private System.Windows.Forms.ToolStripButton ClientToolBar;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton ServiceDeskToolBar;
        private System.Windows.Forms.ToolStripButton PingdomCheckToolBar;
        private System.Windows.Forms.ToolStripButton ReportSubscriptionButton;
    }
}



