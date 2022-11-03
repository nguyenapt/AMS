namespace AMS.ReportAutomation.SchedulerAdmin
{
    partial class EditReportSubscriptionForm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSubscribedByUser = new System.Windows.Forms.CheckBox();
            this.cbReport = new System.Windows.Forms.ComboBox();
            this.txtEmails = new System.Windows.Forms.TextBox();
            this.txtSubscribedUser = new System.Windows.Forms.TextBox();
            this.tpMonthly = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.cbMonthlyDay = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbMonthlyEveryMonth = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tcSchedulerType.SuspendLayout();
            this.tpHourly.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tpDaily.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tpWeekly.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tpMonthly.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(729, 383);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(810, 383);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cbStartAtMinute);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cbStartAtHour);
            this.groupBox1.Controls.Add(this.tcSchedulerType);
            this.groupBox1.Location = new System.Drawing.Point(19, 117);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(875, 245);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Frequency";
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
            this.tcSchedulerType.Controls.Add(this.tpMonthly);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Report:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Receiver Emails: ";
            // 
            // cbSubscribedByUser
            // 
            this.cbSubscribedByUser.AutoSize = true;
            this.cbSubscribedByUser.Location = new System.Drawing.Point(30, 76);
            this.cbSubscribedByUser.Name = "cbSubscribedByUser";
            this.cbSubscribedByUser.Size = new System.Drawing.Size(162, 17);
            this.cbSubscribedByUser.TabIndex = 10;
            this.cbSubscribedByUser.Text = "Subscribed By User, UserId=";
            this.cbSubscribedByUser.UseVisualStyleBackColor = true;
            // 
            // cbReport
            // 
            this.cbReport.FormattingEnabled = true;
            this.cbReport.Location = new System.Drawing.Point(126, 16);
            this.cbReport.Name = "cbReport";
            this.cbReport.Size = new System.Drawing.Size(381, 21);
            this.cbReport.TabIndex = 11;
            // 
            // txtEmails
            // 
            this.txtEmails.Location = new System.Drawing.Point(126, 44);
            this.txtEmails.Name = "txtEmails";
            this.txtEmails.Size = new System.Drawing.Size(753, 20);
            this.txtEmails.TabIndex = 12;
            // 
            // txtSubscribedUser
            // 
            this.txtSubscribedUser.Location = new System.Drawing.Point(189, 74);
            this.txtSubscribedUser.Name = "txtSubscribedUser";
            this.txtSubscribedUser.Size = new System.Drawing.Size(82, 20);
            this.txtSubscribedUser.TabIndex = 13;
            // 
            // tpMonthly
            // 
            this.tpMonthly.Controls.Add(this.panel4);
            this.tpMonthly.Location = new System.Drawing.Point(4, 22);
            this.tpMonthly.Name = "tpMonthly";
            this.tpMonthly.Padding = new System.Windows.Forms.Padding(3);
            this.tpMonthly.Size = new System.Drawing.Size(855, 154);
            this.tpMonthly.TabIndex = 3;
            this.tpMonthly.Text = "Monthly";
            this.tpMonthly.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.cbMonthlyEveryMonth);
            this.panel4.Controls.Add(this.cbMonthlyDay);
            this.panel4.Location = new System.Drawing.Point(6, 16);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(843, 100);
            this.panel4.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(137, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "of every";
            // 
            // cbMonthlyDay
            // 
            this.cbMonthlyDay.FormattingEnabled = true;
            this.cbMonthlyDay.Location = new System.Drawing.Point(67, 0);
            this.cbMonthlyDay.Name = "cbMonthlyDay";
            this.cbMonthlyDay.Size = new System.Drawing.Size(61, 21);
            this.cbMonthlyDay.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(26, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Day";
            // 
            // cbMonthlyEveryMonth
            // 
            this.cbMonthlyEveryMonth.FormattingEnabled = true;
            this.cbMonthlyEveryMonth.Location = new System.Drawing.Point(188, 0);
            this.cbMonthlyEveryMonth.Name = "cbMonthlyEveryMonth";
            this.cbMonthlyEveryMonth.Size = new System.Drawing.Size(59, 21);
            this.cbMonthlyEveryMonth.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(253, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "month(s)";
            // 
            // EditReportSubscriptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 437);
            this.Controls.Add(this.txtSubscribedUser);
            this.Controls.Add(this.txtEmails);
            this.Controls.Add(this.cbReport);
            this.Controls.Add(this.cbSubscribedByUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Name = "EditReportSubscriptionForm";
            this.Text = "Report subscription";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditServiceDeskForm_FormClosing);
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
            this.tpMonthly.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbStartAtMinute;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbStartAtHour;
        private System.Windows.Forms.TabControl tcSchedulerType;
        private System.Windows.Forms.TabPage tpHourly;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbHourlyEveryHour;
        private System.Windows.Forms.TabPage tpDaily;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbDailyEveryDay;
        private System.Windows.Forms.RadioButton rbDailyEveryWeekday;
        private System.Windows.Forms.RadioButton rbDailyEveryDay;
        private System.Windows.Forms.TabPage tpWeekly;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox cbWeeklyThu;
        private System.Windows.Forms.CheckBox cbWeeklySun;
        private System.Windows.Forms.CheckBox cbWeeklyWed;
        private System.Windows.Forms.CheckBox cbWeeklySat;
        private System.Windows.Forms.CheckBox cbWeeklyTue;
        private System.Windows.Forms.CheckBox cbWeeklyFri;
        private System.Windows.Forms.CheckBox cbWeeklyMon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbSubscribedByUser;
        private System.Windows.Forms.ComboBox cbReport;
        private System.Windows.Forms.TextBox txtEmails;
        private System.Windows.Forms.TextBox txtSubscribedUser;
        private System.Windows.Forms.TabPage tpMonthly;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbMonthlyEveryMonth;
        private System.Windows.Forms.ComboBox cbMonthlyDay;
    }
}