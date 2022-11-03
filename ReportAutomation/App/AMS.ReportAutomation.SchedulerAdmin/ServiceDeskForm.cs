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
    public partial class ServiceDeskForm : Form
    {
        private IClient_ManagementService _client_ManagementService;
        private EditServiceDeskForm _editServiceDeskForm;

        public ServiceDeskForm(IClient_ManagementService client_ManagementService, EditServiceDeskForm editServiceDeskForm)
        {
            _client_ManagementService = client_ManagementService;
            _editServiceDeskForm = editServiceDeskForm;
            InitializeComponent();
            //Allow copying the Id value
            grvServiceDesk.Columns[0].ReadOnly = false;
            grvServiceDesk.AutoGenerateColumns = false;
            _editServiceDeskForm.VisibleChanged += _editServiceDeskForm_VisibleChanged;
        }

        private void _editServiceDeskForm_VisibleChanged(object sender, EventArgs e)
        {
            EditServiceDeskForm editServiceDeskForm = sender as EditServiceDeskForm;
            if (!editServiceDeskForm.Visible)
            {
                BindingData();
            }
        }

        public void BindingData()
        {
            var serviceDesks = _client_ManagementService.GetReport_ClientJiraServiceDesks();
            grvServiceDesk.DataSource = serviceDesks;
        }

        private void btnAddServiceDesk_Click(object sender, EventArgs e)
        {
            _editServiceDeskForm.MdiParent = this.ParentForm;
            _editServiceDeskForm.Report_ClientJiraServiceDesk = new Report_ClientJiraServiceDesks();
            _editServiceDeskForm.Show();
        }

        private void grvServiceDesk_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var serviceDesk = grvServiceDesk.Rows[e.RowIndex].DataBoundItem as Report_ClientJiraServiceDesks;

            if (e.ColumnIndex == 5)
            {
                _editServiceDeskForm.Report_ClientJiraServiceDesk = serviceDesk;
                _editServiceDeskForm.BindingData();
                _editServiceDeskForm.MdiParent = this.ParentForm;
                _editServiceDeskForm.Show();
            }
            if (e.ColumnIndex == 6)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this Service Desk ?",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    _client_ManagementService.DeleteServiceDesk(serviceDesk.Id);

                    BindingData();
                }
            }
        }

        private void ServiceDeskForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
