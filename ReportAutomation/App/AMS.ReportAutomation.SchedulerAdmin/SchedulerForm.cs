using AMS.ReportAutomation.Data.Services.Interfaces;
using System;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Collections.Generic;
using System.Linq;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Common.Base;
using System.Configuration;

namespace AMS.ReportAutomation.SchedulerAdmin
{
    public partial class SchedulerForm : Form
    {
        public AutomationReport_Scheduler SchedulerConfiguration { get; set; }

        public SchedulerType SchedulerType
        {
            get {
                if (rbCrawler.Checked)
                    return SchedulerType.CrawData;
                 return SchedulerType.ProcessorData;
            }
        }

        public string ToolName
        {
            get
            {
                return cboTools.Text;
            }
        }

        public List<string> ToolsName { get; set; } = ConfigurationManager.AppSettings["toolNames"].Split(',').ToList();

        private IAutomationReport_SchedulerService _automationReport_SchedulerService;
        public SchedulerForm(IAutomationReport_SchedulerService automationReport_SchedulerService)
        {
            _automationReport_SchedulerService = automationReport_SchedulerService;
            InitializeComponent();
            BindingValue();
        }

        public void BindingValue()
        {
            cbStartAtHour.DataSource = Enumerable.Range(0, 24).ToList();
            cbStartAtMinute.DataSource = Enumerable.Range(0, 60).ToList();

            cbHourlyEveryHour.DataSource = new List<int> { 0, 1, 2, 3, 4, 6, 12 };
            cbDailyEveryDay.DataSource = new List<int> { 1, 2, 3, 4, 5, 6 };

            //TODO: Think more, should get in app config or both
            //var listDBTools = _automationReport_SchedulerService.GetTools();
            //if (listDBTools != null && listDBTools.Any()) {
            //    ToolsName.AddRange(listDBTools);
            //    ToolsName = ToolsName.Distinct().ToList();
            //}
            
            cboTools.DataSource = ToolsName;
            cboTools.SelectedIndex = 0;

            LoadConfigurationByTool(ToolName, SchedulerType);
        }

        private void LoadConfigurationByTool(string toolName, SchedulerType schedulerType)
        {
            if (schedulerType == SchedulerType.CrawData)
            {
                SchedulerConfiguration = _automationReport_SchedulerService.GetCrawlerConfigurationByTool(toolName);
            }
            else
            {
                SchedulerConfiguration = _automationReport_SchedulerService.GetProcessorConfigurationByTool(toolName);
            }

            //Load current configuration in DB

            if (SchedulerConfiguration == null)
            {
                SchedulerConfiguration = new AutomationReport_Scheduler();

                cbStartAtHour.SelectedItem = SchedulerConfiguration.StartAtHour.HasValue ? SchedulerConfiguration.StartAtHour.Value : 0;
                cbStartAtMinute.SelectedItem = SchedulerConfiguration.StartAtMinute.HasValue ? SchedulerConfiguration.StartAtMinute.Value : 0;
            }
            else
            {
                cbStartAtHour.SelectedItem = SchedulerConfiguration.StartAtHour.HasValue ? SchedulerConfiguration.StartAtHour.Value : 0;
                cbStartAtMinute.SelectedItem = SchedulerConfiguration.StartAtMinute.HasValue ? SchedulerConfiguration.StartAtMinute.Value : 0;

                if (SchedulerConfiguration.IsHourly.HasValue && SchedulerConfiguration.IsHourly.Value)
                {
                    tcSchedulerType.SelectedTab = tcSchedulerType.TabPages[0];
                    if (SchedulerConfiguration.HourlyEveryHour.HasValue && SchedulerConfiguration.HourlyEveryHour.Value != 0)
                    {
                        cbHourlyEveryHour.SelectedItem = SchedulerConfiguration.HourlyEveryHour;
                    }
                }
                else if (SchedulerConfiguration.IsDaily.HasValue && SchedulerConfiguration.IsDaily.Value)
                {
                    tcSchedulerType.SelectedTab = tcSchedulerType.TabPages[1];

                    if (SchedulerConfiguration.DailyEveryDay.HasValue && SchedulerConfiguration.DailyEveryDay.Value)
                    {
                        rbDailyEveryDay.Checked = true;
                        cbDailyEveryDay.SelectedItem = SchedulerConfiguration.DailyEveryDayNumber;
                    }
                    else
                    {
                        rbDailyEveryWeekday.Checked = true;
                    }
                }
                else if (SchedulerConfiguration.IsWeekly.HasValue && SchedulerConfiguration.IsWeekly.Value)
                {
                    tcSchedulerType.SelectedTab = tcSchedulerType.TabPages[2];

                    var days = SchedulerConfiguration.WeeklyDays.Split(',').ToList();
                    cbWeeklyMon.Checked = days.Contains("MON");
                    cbWeeklyTue.Checked = days.Contains("TUE");
                    cbWeeklyWed.Checked = days.Contains("WED");
                    cbWeeklyThu.Checked = days.Contains("THU");
                    cbWeeklyFri.Checked = days.Contains("FRI");
                    cbWeeklySat.Checked = days.Contains("SAT");
                    cbWeeklySun.Checked = days.Contains("SUN");
                }

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //TODO: Take the ToolName and ExecutionType from UI?
            GenerateAndSaveConfiguration(ToolName, SchedulerType);
        }

        private void btnSaveAndRestart_Click(object sender, EventArgs e)
        {
            //TODO: Take the ToolName and ExecutionType from UI?
            GenerateAndSaveConfiguration(ToolName, SchedulerType);

            var serviceName = "";

            if(SchedulerType == SchedulerType.CrawData)                
            {
                serviceName = "AMSCrawlerService";
            }
            else
            {
                serviceName = "AMSProcessorService";
            }

            ServiceController sc = new ServiceController(serviceName);
            if (sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped);
                txtStatus.Text = $"{serviceName} Stopped";
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
                txtStatus.Text = txtStatus.Text + Environment.NewLine + $"{serviceName} Started";
            }
            else
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
                txtStatus.Text = txtStatus.Text + Environment.NewLine + $"{serviceName} Started";
            }
        }

        private void GenerateAndSaveConfiguration(string toolName, SchedulerType executionType)
        {
            var schedulerExpression = GenerateConfigurationExpression();

            if (!string.IsNullOrEmpty(schedulerExpression))
            {
                SchedulerConfiguration.ExecutionTool = toolName;
                SchedulerConfiguration.ExecutionType = (int)executionType;
                SchedulerConfiguration.ExecutionExpression = schedulerExpression;
                SchedulerConfiguration.IsHourly = false;
                SchedulerConfiguration.IsDaily = false;
                SchedulerConfiguration.IsWeekly = false;
                //set current configuration to DB
                SchedulerConfiguration.StartAtHour = cbStartAtHour.SelectedValue == null ? 0 : (int)cbStartAtHour.SelectedValue;
                SchedulerConfiguration.StartAtMinute = cbStartAtMinute.SelectedValue == null ? 0 : (int)cbStartAtMinute.SelectedValue;
                if (tcSchedulerType.SelectedTab.Text == "Hourly")
                {
                    SchedulerConfiguration.IsHourly = true;

                    int cbHourlyEveryHourSelectedValue;
                    if (cbHourlyEveryHour.SelectedValue != null &&  int.TryParse(cbHourlyEveryHour.SelectedValue.ToString(), out cbHourlyEveryHourSelectedValue))
                    {
                        SchedulerConfiguration.HourlyEveryHour = cbHourlyEveryHourSelectedValue;
                    }
                    else
                    {
                        SchedulerConfiguration.HourlyEveryHour = 0;
                    }
                }
                else if (tcSchedulerType.SelectedTab.Text == "Daily")
                {
                    SchedulerConfiguration.IsDaily = true;

                    SchedulerConfiguration.DailyEveryDay = rbDailyEveryDay.Checked;
                    SchedulerConfiguration.DailyEveryDayNumber = (int)cbDailyEveryDay.SelectedItem;
                    SchedulerConfiguration.DailyEveryWeekDay = rbDailyEveryWeekday.Checked;
                }
                else if (tcSchedulerType.SelectedTab.Text == "Weekly")
                {
                    SchedulerConfiguration.IsWeekly = true;

                    var days = new List<string>();
                    if (cbWeeklyMon.Checked) days.Add("MON");
                    if (cbWeeklyTue.Checked) days.Add("TUE");
                    if (cbWeeklyWed.Checked) days.Add("WED");
                    if (cbWeeklyThu.Checked) days.Add("THU");
                    if (cbWeeklyFri.Checked) days.Add("FRI");
                    if (cbWeeklySat.Checked) days.Add("SAT");
                    if (cbWeeklySun.Checked) days.Add("SUN");

                    SchedulerConfiguration.WeeklyDays = string.Join(",", days);
                }

                _automationReport_SchedulerService.SaveConfiguration(SchedulerConfiguration);
            }
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
            return string.Empty;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Disable text typing
            cbStartAtHour.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStartAtMinute.DropDownStyle = ComboBoxStyle.DropDownList;
            cbHourlyEveryHour.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDailyEveryDay.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void rbCrawler_CheckedChanged(object sender, EventArgs e)
        {
            LoadConfigurationByTool(ToolName, SchedulerType);
        }

        private void rbProcessor_CheckedChanged(object sender, EventArgs e)
        {
            LoadConfigurationByTool(ToolName, SchedulerType);
        }

        private void cboTools_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadConfigurationByTool(ToolName, SchedulerType);
        }       

        private void btnDelete_Click(object sender, EventArgs e)
        {
            _automationReport_SchedulerService.DeleteConfiguration(SchedulerConfiguration);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }

    public class DayOfWeek
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
