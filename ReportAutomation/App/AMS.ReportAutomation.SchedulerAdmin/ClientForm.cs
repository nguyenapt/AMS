using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Services.Interfaces;
using AMS.ReportAutomation.Data.ViewModel;
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
    public partial class ClientsForm : Form
    {
        private IClient_ManagementService _client_ManagementService;
        private AddClientSiteForm _addClientSiteForm;
        private EditClientForm _editClientForm;
        public ClientsForm(IClient_ManagementService client_ManagementService, AddClientSiteForm addClientSiteForm, EditClientForm editClientForm)
        {
            _client_ManagementService = client_ManagementService;
            _addClientSiteForm = addClientSiteForm;
            _editClientForm = editClientForm;
            InitializeComponent();
            grvClient.AutoGenerateColumns = false;
            grvClientSites.AutoGenerateColumns = false;
            BindingData();
            _editClientForm.VisibleChanged += _editClientForm_VisibleChanged;
            _addClientSiteForm.VisibleChanged += _addClientSiteForm_VisibleChanged;
        }

        private void _addClientSiteForm_VisibleChanged(object sender, EventArgs e)
        {
            AddClientSiteForm addClientSiteForm = sender as AddClientSiteForm;
            if (!addClientSiteForm.Visible)
            {
                var clientId = (int)grvClient.SelectedRows[0].Cells[0].Value;
                var clientSites = _client_ManagementService.GetReport_ClientSitesByClient(clientId);
                grvClientSites.DataSource = null;
                grvClientSites.DataSource = clientSites;
            }
        }

        private void _editClientForm_VisibleChanged(object sender, EventArgs e)
        {
            EditClientForm clientsForm = sender as EditClientForm;
            if (!clientsForm.Visible)
            {
                BindingData();
            }
        }

        private void BindingData()
        {
            var clients = _client_ManagementService.GetReport_Clients();
            grvClient.DataSource = null;
            grvClient.DataSource = clients;

            grvClientSites.DataSource = null;
        }

        private void grvClient_SelectionChanged(object sender, EventArgs e)
        {
            if (grvClient.SelectedRows.Count > 0)
            {
                var clientId = (int)grvClient.SelectedRows[0].Cells[0].Value;
                var clientSites = _client_ManagementService.GetReport_ClientSitesByClient(clientId);
                grvClientSites.DataSource = clientSites;
                btnAddSite.Enabled = true;
            }
            else
            {
                btnAddSite.Enabled = false;
            }
        }

        private void btnAddClient_Click(object sender, EventArgs e)
        {
            _editClientForm.Report_Client = new Report_Clients();
            _editClientForm.MdiParent = this.ParentForm;
            _editClientForm.Show();
        }

        private void btnAddSite_Click(object sender, EventArgs e)
        {
            var clientId = (int)grvClient.SelectedRows[0].Cells[0].Value;
            var client = _client_ManagementService.GetReport_Client(clientId);
            _addClientSiteForm.Report_Client = client;
            _addClientSiteForm.Report_ClientSite = new Report_ClientSiteViewModel();
            _addClientSiteForm.BindingData();
            _addClientSiteForm.MdiParent = this.ParentForm;
            _addClientSiteForm.Show();
        }

        private void grvClientSites_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;


            var client = grvClient.SelectedRows[0].DataBoundItem as Report_Clients;

            var clientSiteViewModel = grvClientSites.Rows[e.RowIndex].DataBoundItem as Report_ClientSiteViewModel;

            if (e.ColumnIndex == 4)
            {
                _addClientSiteForm.Report_Client = client;
                _addClientSiteForm.Report_ClientSite = clientSiteViewModel;
                _addClientSiteForm.BindingData();
                _addClientSiteForm.MdiParent = this.ParentForm;
                _addClientSiteForm.Show();
            }
            if (e.ColumnIndex == 5)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this Client Site ?",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    _client_ManagementService.DeleteClientSite(clientSiteViewModel.Id);

                    var clientId = (int)grvClient.SelectedRows[0].Cells[0].Value;
                    var clientSites = _client_ManagementService.GetReport_ClientSitesByClient(clientId);
                    grvClientSites.DataSource = null;
                    grvClientSites.DataSource = clientSites;
                }
            }
        }

        private void ClientsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void grvClient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            grvClient.Rows[e.RowIndex].Selected = true;

            var client = grvClient.Rows[e.RowIndex].DataBoundItem as Report_Clients;

            if (e.ColumnIndex == 2)
            {
                _editClientForm.Report_Client = client;
                _editClientForm.BindingData();
                _editClientForm.MdiParent = this.ParentForm;
                _editClientForm.Show();
            }
            if (e.ColumnIndex == 3)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this Client ?",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    _client_ManagementService.DeleteClient(client.Id);

                    var clients = _client_ManagementService.GetReport_Clients();
                    grvClient.DataSource = null;
                    grvClient.DataSource = clients;
                }
            }
        }
    }
}
