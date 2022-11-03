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
    public partial class EditClientForm : Form
    {
        private IClient_ManagementService _client_ManagementService;

        public Report_Clients Report_Client { get; set; }

        public EditClientForm(IClient_ManagementService client_ManagementService)
        {
            _client_ManagementService = client_ManagementService;
            InitializeComponent();
        }

        public void BindingData()
        {
            if (Report_Client != null)
            {
                txtClientName.Text = Report_Client.Name;
                txtLogoUrl.Text = Report_Client.LogoUrl;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Report_Client.Id == 0)
            {
                _client_ManagementService.AddClient(txtClientName.Text, txtLogoUrl.Text);
            }
            else
            {
                _client_ManagementService.UpdateClient(Report_Client.Id, txtClientName.Text, txtLogoUrl.Text);
            }
            this.Hide();
        }

        private void EditClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
