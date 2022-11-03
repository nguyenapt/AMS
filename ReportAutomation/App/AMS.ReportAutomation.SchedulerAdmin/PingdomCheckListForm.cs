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
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;

namespace AMS.ReportAutomation.SchedulerAdmin
{
    public partial class PingdomCheckListForm : Form
    {
        private IPingdom_CheckService _pingdom_CheckService;

        public PingdomCheckListForm(IPingdom_CheckService pingdom_CheckService)
        {
            _pingdom_CheckService = pingdom_CheckService;
            InitializeComponent();
            //Allow copying the Id value
            grvPingdomCheck.Columns[0].ReadOnly = false;
            grvPingdomCheck.Columns[1].ReadOnly = true;
            grvPingdomCheck.Columns[2].ReadOnly = true;
            grvPingdomCheck.AutoGenerateColumns = false;
        }

        public void BindingData()
        {
            var pingdoms = _pingdom_CheckService.GetAll().ToList();
            grvPingdomCheck.DataSource = pingdoms;
        }

        private void PingdomCheckListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
