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

namespace AMS.ReportAutomation.SchedulerAdmin
{
    public partial class EditServiceDeskForm : Form
    {
        private IClient_ManagementService _client_ManagementService;
        public Report_ClientJiraServiceDesks Report_ClientJiraServiceDesk { get; set; } = new Report_ClientJiraServiceDesks();

        public EditServiceDeskForm(IClient_ManagementService client_ManagementService)
        {
            _client_ManagementService = client_ManagementService;
            InitializeComponent();
        }

        public void BindingData()
        {
            if (Report_ClientJiraServiceDesk != null)
            {
                txtServiceDeskUrl.Text = Report_ClientJiraServiceDesk.ServiceDeskUrl;
                txtProjectKey.Text = Report_ClientJiraServiceDesk.ProjectKey;
                txtOrgName.Text = Report_ClientJiraServiceDesk.OrganizationName;
                txtOrgNameIfNotProvided.Text = Report_ClientJiraServiceDesk.OrgNameForProjectIfNotProvided;
            }
        }

        private void btnSaveServiceDesk_Click(object sender, EventArgs e)
        {
            Report_ClientJiraServiceDesk.ServiceDeskUrl = txtServiceDeskUrl.Text;
            Report_ClientJiraServiceDesk.ProjectKey = txtProjectKey.Text;
            Report_ClientJiraServiceDesk.OrganizationName = txtOrgName.Text;
            Report_ClientJiraServiceDesk.OrgNameForProjectIfNotProvided = txtOrgNameIfNotProvided.Text;

            if (Report_ClientJiraServiceDesk.Id != Guid.Empty)
            {                
                _client_ManagementService.UpdateServiceDesk(Report_ClientJiraServiceDesk);
            }
            else
            {
                Report_ClientJiraServiceDesk.Id = Guid.NewGuid();
                _client_ManagementService.AddServiceDesk(Report_ClientJiraServiceDesk);                
            }
            this.Hide();
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
    }
}
