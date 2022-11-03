using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.ViewModel;

namespace AMS.ReportAutomation.SchedulerAdmin
{
    public partial class EditReportSubscriptionForm : Form
    {
        private IReportSubscription_Service _reportSubscriptionService;
        private IReport_ReportsRepository _reportReportsRepository;
        public ReportSubscriptionViewModel ReportSubscription { get; set; } = new ReportSubscriptionViewModel();

        public EditReportSubscriptionForm(IReportSubscription_Service reportSubscriptionService, IReport_ReportsRepository reportReportsRepository)
        {
            _reportSubscriptionService = reportSubscriptionService;
            _reportReportsRepository = reportReportsRepository;
            InitializeComponent();
        }

        public void BindingData()
        {
            var reports = _reportReportsRepository.GetClientSiteReportViewModels();
            cbReport.DisplayMember = "ReportName";
            cbReport.ValueMember = "Id";
            cbReport.DataSource = reports;

            cbStartAtHour.DataSource = Enumerable.Range(0, 24).ToList();
            cbStartAtMinute.DataSource = Enumerable.Range(0, 60).ToList();

            cbHourlyEveryHour.DataSource = new List<int> { 0, 1, 2, 3, 4, 6, 12 };
            cbDailyEveryDay.DataSource = new List<int> { 1, 2, 3, 4, 5, 6 };

            var dayOfMonth = new List<int>();

            for (int i = 1; i <= 31; i++){
                dayOfMonth.Add(i);
            }

            cbMonthlyDay.DataSource = dayOfMonth;

            cbMonthlyEveryMonth.DataSource = new List<int> { 1, 2, 3, 4, 6 };

            if (ReportSubscription != null)
            {
                cbReport.SelectedValue = ReportSubscription.ReportId;
                txtEmails.Text = ReportSubscription.ReceiverEmails;
                cbSubscribedByUser.Checked = ReportSubscription.SubscribedByUser ?? false;
                txtSubscribedUser.Text = ReportSubscription.SubscribedUser ?? null;

                LoadConfigurationByTool();
            }
        }

        private void LoadConfigurationByTool()
        {
            
            //Load current configuration in DB

            if (ReportSubscription != null)
            {
                cbStartAtHour.SelectedItem = ReportSubscription.StartAtHour.HasValue ? ReportSubscription.StartAtHour.Value : 0;
                cbStartAtMinute.SelectedItem = ReportSubscription.StartAtMinute.HasValue ? ReportSubscription.StartAtMinute.Value : 0;

                if (ReportSubscription.IsHourly.HasValue && ReportSubscription.IsHourly.Value)
                {
                    tcSchedulerType.SelectedTab = tcSchedulerType.TabPages[0];
                    if (ReportSubscription.HourlyEveryHour.HasValue && ReportSubscription.HourlyEveryHour.Value != 0)
                    {
                        cbHourlyEveryHour.SelectedItem = ReportSubscription.HourlyEveryHour;
                    }
                }
                else if (ReportSubscription.IsDaily.HasValue && ReportSubscription.IsDaily.Value)
                {
                    tcSchedulerType.SelectedTab = tcSchedulerType.TabPages[1];

                    if (ReportSubscription.DailyEveryDay.HasValue && ReportSubscription.DailyEveryDay.Value)
                    {
                        rbDailyEveryDay.Checked = true;
                        cbDailyEveryDay.SelectedItem = ReportSubscription.DailyEveryDayNumber;
                    }
                    else
                    {
                        rbDailyEveryWeekday.Checked = true;
                    }
                }
                else if (ReportSubscription.IsWeekly.HasValue && ReportSubscription.IsWeekly.Value)
                {
                    tcSchedulerType.SelectedTab = tcSchedulerType.TabPages[2];

                    var days = ReportSubscription.WeeklyDays.Split(',').ToList();
                    cbWeeklyMon.Checked = days.Contains("MON");
                    cbWeeklyTue.Checked = days.Contains("TUE");
                    cbWeeklyWed.Checked = days.Contains("WED");
                    cbWeeklyThu.Checked = days.Contains("THU");
                    cbWeeklyFri.Checked = days.Contains("FRI");
                    cbWeeklySat.Checked = days.Contains("SAT");
                    cbWeeklySun.Checked = days.Contains("SUN");
                }
                else if (ReportSubscription.IsMonthly.HasValue && ReportSubscription.IsMonthly.Value)
                {
                    tcSchedulerType.SelectedTab = tcSchedulerType.TabPages[3];

                    if (ReportSubscription.MonthlyDay.HasValue && ReportSubscription.MonthlyDay.Value != 0)
                    {
                        cbMonthlyDay.SelectedItem = ReportSubscription.MonthlyDay;
                    }
                    if (ReportSubscription.MonthlyEveryMonth.HasValue && ReportSubscription.MonthlyEveryMonth.Value != 0)
                    {
                        cbMonthlyEveryMonth.SelectedItem = ReportSubscription.MonthlyEveryMonth;
                    }
                }
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void EditServiceDeskForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var expression = GenerateConfigurationExpression();

            ReportSubscription.ReportId = Guid.Parse(cbReport.SelectedValue.ToString());
            ReportSubscription.ReceiverEmails = txtEmails.Text;
            ReportSubscription.SubscribedByUser = cbSubscribedByUser.Checked;
            ReportSubscription.SubscribedUser = string.IsNullOrWhiteSpace(txtSubscribedUser.Text) ? null : txtSubscribedUser.Text.Trim();
            ReportSubscription.Frequency = expression;
            ReportSubscription.IsHourly = false;
            ReportSubscription.IsDaily = false;
            ReportSubscription.IsWeekly = false;
            ReportSubscription.IsMonthly = false;
            //set current configuration to DB
            ReportSubscription.StartAtHour = cbStartAtHour.SelectedValue == null ? 0 : (int)cbStartAtHour.SelectedValue;
            ReportSubscription.StartAtMinute = cbStartAtMinute.SelectedValue == null ? 0 : (int)cbStartAtMinute.SelectedValue;
            if (tcSchedulerType.SelectedTab.Text == "Hourly")
            {
                ReportSubscription.IsHourly = true;

                int cbHourlyEveryHourSelectedValue;
                if (cbHourlyEveryHour.SelectedValue != null && int.TryParse(cbHourlyEveryHour.SelectedValue.ToString(), out cbHourlyEveryHourSelectedValue))
                {
                    ReportSubscription.HourlyEveryHour = cbHourlyEveryHourSelectedValue;
                }
                else
                {
                    ReportSubscription.HourlyEveryHour = 0;
                }
            }
            else if (tcSchedulerType.SelectedTab.Text == "Daily")
            {
                ReportSubscription.IsDaily = true;

                ReportSubscription.DailyEveryDay = rbDailyEveryDay.Checked;
                ReportSubscription.DailyEveryDayNumber = (int)cbDailyEveryDay.SelectedItem;
                ReportSubscription.DailyEveryWeekDay = rbDailyEveryWeekday.Checked;
            }
            else if (tcSchedulerType.SelectedTab.Text == "Weekly")
            {
                ReportSubscription.IsWeekly = true;

                var days = new List<string>();
                if (cbWeeklyMon.Checked) days.Add("MON");
                if (cbWeeklyTue.Checked) days.Add("TUE");
                if (cbWeeklyWed.Checked) days.Add("WED");
                if (cbWeeklyThu.Checked) days.Add("THU");
                if (cbWeeklyFri.Checked) days.Add("FRI");
                if (cbWeeklySat.Checked) days.Add("SAT");
                if (cbWeeklySun.Checked) days.Add("SUN");

                ReportSubscription.WeeklyDays = string.Join(",", days);
            }
            else if (tcSchedulerType.SelectedTab.Text == "Monthly")
            {
                ReportSubscription.IsMonthly = true;

                ReportSubscription.MonthlyDay = (int)cbMonthlyDay.SelectedItem;
                ReportSubscription.MonthlyEveryMonth = (int)cbMonthlyEveryMonth.SelectedItem;
            }

            _reportSubscriptionService.SaveReportSubscriptionConfiguration(ReportSubscription);

            this.Hide();
        }

        private string GenerateConfigurationExpression()
        {
            if (tcSchedulerType.SelectedTab.Text == "Hourly")
            {
                return $"0 {cbStartAtMinute.Text} {cbStartAtHour.Text} 1/1 * ? *";
            }
            else if (tcSchedulerType.SelectedTab.Text == "Daily")
            {
                if (rbDailyEveryDay.Checked)
                {
                    int cbDailyEveryDaySelectedValue;
                    if (int.TryParse(cbDailyEveryDay.SelectedValue.ToString(), out cbDailyEveryDaySelectedValue))
                    {
                        return $"0 {cbStartAtMinute.Text} {cbStartAtHour.Text} */{cbDailyEveryDaySelectedValue} * ? *";
                    }
                    else
                    {
                        return $"0 {cbStartAtMinute.Text} {cbStartAtHour.Text} 1/1 * ? *";
                    }
                }
                else
                {
                    return $"0 {cbStartAtMinute.Text} {cbStartAtHour.Text} ? * MON-FRI *";
                }
            }
            else if (tcSchedulerType.SelectedTab.Text == "Weekly")
            {
                var days = new List<string>();
                if (cbWeeklyMon.Checked) days.Add("MON");
                if (cbWeeklyTue.Checked) days.Add("TUE");
                if (cbWeeklyWed.Checked) days.Add("WED");
                if (cbWeeklyThu.Checked) days.Add("THU");
                if (cbWeeklyFri.Checked) days.Add("FRI");
                if (cbWeeklySat.Checked) days.Add("SAT");
                if (cbWeeklySun.Checked) days.Add("SUN");

                if (days.Count == 0)
                    return $"0 {cbStartAtMinute.Text} {cbStartAtHour.Text} ? * * *";

                return $"0 {cbStartAtMinute.Text} {cbStartAtHour.Text} ? * {string.Join(",", days)} *";
            }
            else if (tcSchedulerType.SelectedTab.Text == "Monthly")
            {
                if (int.TryParse(cbMonthlyDay.SelectedValue.ToString(), out var cbMonthlyDaySelectedValue) && int.TryParse(cbMonthlyEveryMonth.SelectedValue.ToString(), out var cbMonthlyMonthSelectedValue))
                {
                    return $"0 {cbStartAtMinute.Text} {cbStartAtHour.Text} {cbMonthlyDaySelectedValue} 1/{cbMonthlyMonthSelectedValue} ? *";
                }
                else
                {
                    return $"0 {cbStartAtMinute.Text} {cbStartAtHour.Text} 1/1 * ? *";
                }
                
            }
            return string.Empty;
        }
    }
}
