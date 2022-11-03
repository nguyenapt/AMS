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
    public partial class AdminForm : Form
    {
        private int childFormNumber = 0;
        private SchedulerForm _mainForm;
        private ClientsForm _clientForm;
        private ServiceDeskForm _serviceDeskForm;
        private PingdomCheckListForm _pingdomCheckListForm;
        private ReportSubscriptionForm _reportSubscriptionForm;
        public AdminForm(SchedulerForm mainForm, ClientsForm clientForm, ServiceDeskForm serviceDeskForm, PingdomCheckListForm pingdomCheckListForm, ReportSubscriptionForm reportSubscriptionForm)
        {
            _mainForm = mainForm;
            _clientForm = clientForm;
            _serviceDeskForm = serviceDeskForm;
            _pingdomCheckListForm = pingdomCheckListForm;
            _reportSubscriptionForm = reportSubscriptionForm;
            InitializeComponent();
            _mainForm.MdiParent = this;
            _mainForm.Show();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SchedulerToolBar_Click(object sender, EventArgs e)
        {
            _mainForm.MdiParent = this;
            _mainForm.Show();
        }

        private void ClientToolBar_Click(object sender, EventArgs e)
        {
            _clientForm.MdiParent = this;
            _clientForm.Show();
        }

        private void ServiceDeskToolBar_Click(object sender, EventArgs e)
        {
            _serviceDeskForm.MdiParent = this;
            _serviceDeskForm.BindingData();
            _serviceDeskForm.Show();
        }

        private void PingdomCheckToolBar_Click(object sender, EventArgs e)
        {
            _pingdomCheckListForm.MdiParent = this;
            _pingdomCheckListForm.BindingData();
            _pingdomCheckListForm.Show();
        }

        private void ReportSubscriptionButton_Click(object sender, EventArgs e)
        {
            _reportSubscriptionForm.MdiParent = this;
            _reportSubscriptionForm.BindingData();
            _reportSubscriptionForm.Show();
        }
    }
}
