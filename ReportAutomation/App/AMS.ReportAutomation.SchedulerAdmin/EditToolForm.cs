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
    public partial class EditToolForm : Form
    {
        public ToolModel ToolModel { get; set; }

        public EditToolForm()
        {
            InitializeComponent();
        }

        public void BindingData() { 
            if(ToolModel != null)
            {
                txtKey.Text = ToolModel.Key;
                txtValue.Text = ToolModel.Value;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.ToolModel.Key = txtKey.Text;
            this.ToolModel.Value = txtValue.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
