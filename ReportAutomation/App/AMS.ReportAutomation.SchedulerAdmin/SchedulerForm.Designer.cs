namespace AMS.ReportAutomation.SchedulerAdmin
{
    partial class SchedulerForm
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
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbStartAtMinute = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbStartAtHour = new System.Windows.Forms.ComboBox();
            this.tcSchedulerType = new System.Windows.Forms.TabControl();
            this.tpHourly = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbHourlyEveryHour = new System.Windows.Forms.ComboBox();
            this.tpDaily = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.cbDailyEveryDay = new System.Windows.Forms.ComboBox();
            this.rbDailyEveryWeekday = new System.Windows.Forms.RadioButton();
            this.rbDailyEveryDay = new System.Windows.Forms.RadioButton();
            this.tpWeekly = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cbWeeklyThu = new System.Windows.Forms.CheckBox();
            this.cbWeeklySun = new System.Windows.Forms.CheckBox();
            this.cbWeeklyWed = new System.Windows.Forms.CheckBox();
            this.cbWeeklySat = new System.Windows.Forms.CheckBox();
            this.cbWeeklyTue = new System.Windows.Forms.CheckBox();
            this.cbWeeklyFri = new System.Windows.Forms.CheckBox();
            this.cbWeeklyMon = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboTools = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSaveAndRestart = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbCrawler = new System.Windows.Forms.RadioButton();
            this.rbProcessor = new System.Windows.Forms.RadioButton();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tcSchedulerType.SuspendLayout();
            this.tpHourly.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tpDaily.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tpWeekly.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(344, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(258, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "Configuration";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cbStartAtMinute);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cbStartAtHour);
            this.groupBox1.Controls.Add(this.tcSchedulerType);
            this.groupBox1.Location = new System.Drawing.Point(24, 152);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(875, 245);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Run the crawler/processor";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 43);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Recurrence:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Start:";
            // 
            // cbStartAtMinute
            // 
            this.cbStartAtMinute.FormattingEnabled = true;
            this.cbStartAtMinute.Location = new System.Drawing.Point(115, 19);
            this.cbStartAtMinute.Name = "cbStartAtMinute";
            this.cbStartAtMinute.Size = new System.Drawing.Size(57, 21);
            this.cbStartAtMinute.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(104, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(10, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = ":";
            // 
            // cbStartAtHour
            // 
            this.cbStartAtHour.FormattingEnabled = true;
            this.cbStartAtHour.Location = new System.Drawing.Point(46, 19);
            this.cbStartAtHour.Name = "cbStartAtHour";
            this.cbStartAtHour.Size = new System.Drawing.Size(56, 21);
            this.cbStartAtHour.TabIndex = 6;
            // 
            // tcSchedulerType
            // 
            this.tcSchedulerType.Controls.Add(this.tpHourly);
            this.tcSchedulerType.Controls.Add(this.tpDaily);
            this.tcSchedulerType.Controls.Add(this.tpWeekly);
            this.tcSchedulerType.Location = new System.Drawing.Point(7, 59);
            this.tcSchedulerType.Name = "tcSchedulerType";
            this.tcSchedulerType.SelectedIndex = 0;
            this.tcSchedulerType.Size = new System.Drawing.Size(863, 180);
            this.tcSchedulerType.TabIndex = 0;
            // 
            // tpHourly
            // 
            this.tpHourly.Controls.Add(this.panel1);
            this.tpHourly.Location = new System.Drawing.Point(4, 22);
            this.tpHourly.Name = "tpHourly";
            this.tpHourly.Padding = new System.Windows.Forms.Padding(3);
            this.tpHourly.Size = new System.Drawing.Size(855, 154);
            this.tpHourly.TabIndex = 0;
            this.tpHourly.Text = "Hourly";
            this.tpHourly.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cbHourlyEveryHour);
            this.panel1.Location = new System.Drawing.Point(6, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(843, 100);
            this.panel1.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Recur every:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(185, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Hour(s)";
            // 
            // cbHourlyEveryHour
            // 
            this.cbHourlyEveryHour.FormattingEnabled = true;
            this.cbHourlyEveryHour.Location = new System.Drawing.Point(90, 2);
            this.cbHourlyEveryHour.Name = "cbHourlyEveryHour";
            this.cbHourlyEveryHour.Size = new System.Drawing.Size(89, 21);
            this.cbHourlyEveryHour.TabIndex = 1;
            // 
            // tpDaily
            // 
            this.tpDaily.Controls.Add(this.panel2);
            this.tpDaily.Location = new System.Drawing.Point(4, 22);
            this.tpDaily.Name = "tpDaily";
            this.tpDaily.Padding = new System.Windows.Forms.Padding(3);
            this.tpDaily.Size = new System.Drawing.Size(855, 154);
            this.tpDaily.TabIndex = 1;
            this.tpDaily.Text = "Daily";
            this.tpDaily.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.cbDailyEveryDay);
            this.panel2.Controls.Add(this.rbDailyEveryWeekday);
            this.panel2.Controls.Add(this.rbDailyEveryDay);
            this.panel2.Location = new System.Drawing.Point(6, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(843, 100);
            this.panel2.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(203, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "day(s)";
            // 
            // cbDailyEveryDay
            // 
            this.cbDailyEveryDay.FormattingEnabled = true;
            this.cbDailyEveryDay.Location = new System.Drawing.Point(108, 0);
            this.cbDailyEveryDay.Name = "cbDailyEveryDay";
            this.cbDailyEveryDay.Size = new System.Drawing.Size(89, 21);
            this.cbDailyEveryDay.TabIndex = 6;
            // 
            // rbDailyEveryWeekday
            // 
            this.rbDailyEveryWeekday.AutoSize = true;
            this.rbDailyEveryWeekday.Location = new System.Drawing.Point(33, 26);
            this.rbDailyEveryWeekday.Name = "rbDailyEveryWeekday";
            this.rbDailyEveryWeekday.Size = new System.Drawing.Size(98, 17);
            this.rbDailyEveryWeekday.TabIndex = 0;
            this.rbDailyEveryWeekday.Text = "Every weekday";
            this.rbDailyEveryWeekday.UseVisualStyleBackColor = true;
            // 
            // rbDailyEveryDay
            // 
            this.rbDailyEveryDay.AutoSize = true;
            this.rbDailyEveryDay.Checked = true;
            this.rbDailyEveryDay.Location = new System.Drawing.Point(33, 3);
            this.rbDailyEveryDay.Name = "rbDailyEveryDay";
            this.rbDailyEveryDay.Size = new System.Drawing.Size(52, 17);
            this.rbDailyEveryDay.TabIndex = 0;
            this.rbDailyEveryDay.TabStop = true;
            this.rbDailyEveryDay.Text = "Every";
            this.rbDailyEveryDay.UseVisualStyleBackColor = true;
            // 
            // tpWeekly
            // 
            this.tpWeekly.Controls.Add(this.label3);
            this.tpWeekly.Controls.Add(this.panel3);
            this.tpWeekly.Location = new System.Drawing.Point(4, 22);
            this.tpWeekly.Name = "tpWeekly";
            this.tpWeekly.Size = new System.Drawing.Size(855, 154);
            this.tpWeekly.TabIndex = 2;
            this.tpWeekly.Text = "Weekly";
            this.tpWeekly.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Recur every week on:";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cbWeeklyThu);
            this.panel3.Controls.Add(this.cbWeeklySun);
            this.panel3.Controls.Add(this.cbWeeklyWed);
            this.panel3.Controls.Add(this.cbWeeklySat);
            this.panel3.Controls.Add(this.cbWeeklyTue);
            this.panel3.Controls.Add(this.cbWeeklyFri);
            this.panel3.Controls.Add(this.cbWeeklyMon);
            this.panel3.Location = new System.Drawing.Point(9, 48);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(843, 60);
            this.panel3.TabIndex = 3;
            // 
            // cbWeeklyThu
            // 
            this.cbWeeklyThu.AutoSize = true;
            this.cbWeeklyThu.Location = new System.Drawing.Point(316, 3);
            this.cbWeeklyThu.Name = "cbWeeklyThu";
            this.cbWeeklyThu.Size = new System.Drawing.Size(70, 17);
            this.cbWeeklyThu.TabIndex = 6;
            this.cbWeeklyThu.Text = "Thursday";
            this.cbWeeklyThu.UseVisualStyleBackColor = true;
            // 
            // cbWeeklySun
            // 
            this.cbWeeklySun.AutoSize = true;
            this.cbWeeklySun.Location = new System.Drawing.Point(221, 27);
            this.cbWeeklySun.Name = "cbWeeklySun";
            this.cbWeeklySun.Size = new System.Drawing.Size(62, 17);
            this.cbWeeklySun.TabIndex = 6;
            this.cbWeeklySun.Text = "Sunday";
            this.cbWeeklySun.UseVisualStyleBackColor = true;
            // 
            // cbWeeklyWed
            // 
            this.cbWeeklyWed.AutoSize = true;
            this.cbWeeklyWed.Location = new System.Drawing.Point(221, 4);
            this.cbWeeklyWed.Name = "cbWeeklyWed";
            this.cbWeeklyWed.Size = new System.Drawing.Size(83, 17);
            this.cbWeeklyWed.TabIndex = 6;
            this.cbWeeklyWed.Text = "Wednesday";
            this.cbWeeklyWed.UseVisualStyleBackColor = true;
            // 
            // cbWeeklySat
            // 
            this.cbWeeklySat.AutoSize = true;
            this.cbWeeklySat.Location = new System.Drawing.Point(127, 27);
            this.cbWeeklySat.Name = "cbWeeklySat";
            this.cbWeeklySat.Size = new System.Drawing.Size(68, 17);
            this.cbWeeklySat.TabIndex = 6;
            this.cbWeeklySat.Text = "Saturday";
            this.cbWeeklySat.UseVisualStyleBackColor = true;
            // 
            // cbWeeklyTue
            // 
            this.cbWeeklyTue.AutoSize = true;
            this.cbWeeklyTue.Location = new System.Drawing.Point(127, 4);
            this.cbWeeklyTue.Name = "cbWeeklyTue";
            this.cbWeeklyTue.Size = new System.Drawing.Size(67, 17);
            this.cbWeeklyTue.TabIndex = 6;
            this.cbWeeklyTue.Text = "Tuesday";
            this.cbWeeklyTue.UseVisualStyleBackColor = true;
            // 
            // cbWeeklyFri
            // 
            this.cbWeeklyFri.AutoSize = true;
            this.cbWeeklyFri.Location = new System.Drawing.Point(35, 27);
            this.cbWeeklyFri.Name = "cbWeeklyFri";
            this.cbWeeklyFri.Size = new System.Drawing.Size(54, 17);
            this.cbWeeklyFri.TabIndex = 6;
            this.cbWeeklyFri.Text = "Friday";
            this.cbWeeklyFri.UseVisualStyleBackColor = true;
            // 
            // cbWeeklyMon
            // 
            this.cbWeeklyMon.AutoSize = true;
            this.cbWeeklyMon.Location = new System.Drawing.Point(35, 4);
            this.cbWeeklyMon.Name = "cbWeeklyMon";
            this.cbWeeklyMon.Size = new System.Drawing.Size(64, 17);
            this.cbWeeklyMon.TabIndex = 6;
            this.cbWeeklyMon.Text = "Monday";
            this.cbWeeklyMon.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Tool:";
            // 
            // cboTools
            // 
            this.cboTools.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTools.FormattingEnabled = true;
            this.cboTools.Location = new System.Drawing.Point(70, 107);
            this.cboTools.Name = "cboTools";
            this.cboTools.Size = new System.Drawing.Size(126, 21);
            this.cboTools.TabIndex = 6;
            this.cboTools.SelectedIndexChanged += new System.EventHandler(this.cboTools_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(517, 403);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveAndRestart
            // 
            this.btnSaveAndRestart.Location = new System.Drawing.Point(598, 403);
            this.btnSaveAndRestart.Name = "btnSaveAndRestart";
            this.btnSaveAndRestart.Size = new System.Drawing.Size(181, 23);
            this.btnSaveAndRestart.TabIndex = 3;
            this.btnSaveAndRestart.Text = "Save && Restart Service";
            this.btnSaveAndRestart.UseVisualStyleBackColor = true;
            this.btnSaveAndRestart.Click += new System.EventHandler(this.btnSaveAndRestart_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(28, 449);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(875, 69);
            this.txtStatus.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 433);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Status";
            // 
            // rbCrawler
            // 
            this.rbCrawler.AutoSize = true;
            this.rbCrawler.Checked = true;
            this.rbCrawler.Location = new System.Drawing.Point(35, 84);
            this.rbCrawler.Name = "rbCrawler";
            this.rbCrawler.Size = new System.Drawing.Size(60, 17);
            this.rbCrawler.TabIndex = 6;
            this.rbCrawler.TabStop = true;
            this.rbCrawler.Text = "Crawler";
            this.rbCrawler.UseVisualStyleBackColor = true;
            this.rbCrawler.CheckedChanged += new System.EventHandler(this.rbCrawler_CheckedChanged);
            // 
            // rbProcessor
            // 
            this.rbProcessor.AutoSize = true;
            this.rbProcessor.Location = new System.Drawing.Point(121, 84);
            this.rbProcessor.Name = "rbProcessor";
            this.rbProcessor.Size = new System.Drawing.Size(72, 17);
            this.rbProcessor.TabIndex = 7;
            this.rbProcessor.TabStop = true;
            this.rbProcessor.Text = "Processor";
            this.rbProcessor.UseVisualStyleBackColor = true;
            this.rbProcessor.CheckedChanged += new System.EventHandler(this.rbProcessor_CheckedChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(785, 403);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(118, 23);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // SchedulerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 531);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.rbProcessor);
            this.Controls.Add(this.rbCrawler);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnSaveAndRestart);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cboTools);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "SchedulerForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scheduler Administrator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tcSchedulerType.ResumeLayout(false);
            this.tpHourly.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tpDaily.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tpWeekly.ResumeLayout(false);
            this.tpWeekly.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSaveAndRestart;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tcSchedulerType;
        private System.Windows.Forms.TabPage tpHourly;
        private System.Windows.Forms.TabPage tpDaily;
        private System.Windows.Forms.TabPage tpWeekly;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbHourlyEveryHour;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbDailyEveryDay;
        private System.Windows.Forms.RadioButton rbDailyEveryWeekday;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox cbWeeklyThu;
        private System.Windows.Forms.CheckBox cbWeeklySun;
        private System.Windows.Forms.CheckBox cbWeeklyWed;
        private System.Windows.Forms.CheckBox cbWeeklySat;
        private System.Windows.Forms.CheckBox cbWeeklyTue;
        private System.Windows.Forms.CheckBox cbWeeklyFri;
        private System.Windows.Forms.CheckBox cbWeeklyMon;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbStartAtMinute;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbStartAtHour;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbDailyEveryDay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboTools;
        private System.Windows.Forms.RadioButton rbCrawler;
        private System.Windows.Forms.RadioButton rbProcessor;
        private System.Windows.Forms.Button btnDelete;
    }
}

