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
using AMS.ReportAutomation.Data.ViewModel;

namespace AMS.ReportAutomation.SchedulerAdmin
{
    public partial class ReportSubscriptionForm : Form
    {
        private IReportSubscription_Service _reportSubscriptionService;
        private EditReportSubscriptionForm _editReportSubscriptionForm;

        public ReportSubscriptionForm(IReportSubscription_Service reportSubscriptionService, EditReportSubscriptionForm editReportSubscriptionForm)
        {
            _reportSubscriptionService = reportSubscriptionService;
            _editReportSubscriptionForm = editReportSubscriptionForm;
            InitializeComponent();
            //Allow copying the Id value
            grvReportSubscription.Columns[0].ReadOnly = false;
            grvReportSubscription.AutoGenerateColumns = false;
            _editReportSubscriptionForm.VisibleChanged += _editReportSubscriptionForm_VisibleChanged;
        }

        private void _editReportSubscriptionForm_VisibleChanged(object sender, EventArgs e)
        {
            EditReportSubscriptionForm editReportSubscriptionForm = sender as EditReportSubscriptionForm;
            if (!editReportSubscriptionForm.Visible)
            {
                BindingData();
            }
        }

        public void BindingData()
        {
            var subscriptions = _reportSubscriptionService.GetReportSubscriptionConfigurations();
            grvReportSubscription.DataSource = subscriptions;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _editReportSubscriptionForm.MdiParent = this.ParentForm;
            _editReportSubscriptionForm.ReportSubscription = new ReportSubscriptionViewModel();
            _editReportSubscriptionForm.BindingData();
            _editReportSubscriptionForm.Show();
        }

        private void grvReportSubscription_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var reportSubscription = grvReportSubscription.Rows[e.RowIndex].DataBoundItem as ReportSubscriptionViewModel;

            if (e.ColumnIndex == 6)
            {
                _editReportSubscriptionForm.ReportSubscription = reportSubscription;
                _editReportSubscriptionForm.BindingData();
                _editReportSubscriptionForm.MdiParent = this.ParentForm;
                _editReportSubscriptionForm.Show();
            }
            if (e.ColumnIndex == 7)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this subscription ?",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    _reportSubscriptionService.DeleteReportSubscriptionConfiguration(reportSubscription.Id);

                    BindingData();
                }
            }
        }

        private void ReportSubscriptionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
