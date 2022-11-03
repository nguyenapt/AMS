using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Services.Interfaces;
using AMS.ReportAutomation.Data.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace AMS.ReportAutomation.SchedulerAdmin
{
    public partial class AddClientSiteForm : Form
    {
        public Report_Clients Report_Client { get; set; }

        public Report_ClientSiteViewModel Report_ClientSite { get; set; }

        private IClient_ManagementService _client_ManagementService;

        public AddClientSiteForm(IClient_ManagementService client_ManagementService)
        {
            _client_ManagementService = client_ManagementService;
            InitializeComponent();
            grvTools.AutoGenerateColumns = false;
            txtFileSecretPathGA.Text = ConfigurationManager.AppSettings["googleanalytics:CredentialsFolder"];
            txtFileSecretPathGC.Text = ConfigurationManager.AppSettings["googlesearch:CredentialsFolder"];
        }

        public void BindingData()
        {
            if (Report_Client != null)
            {
                lblClient.Text = Report_Client.Name;
            }

            if (Report_ClientSite != null)
            {
                txtBrandName.Text = Report_ClientSite.BrandName;
                txtName.Text = Report_ClientSite.Name;
                txtDescription.Text = Report_ClientSite.Description;
                txtLogoUrl.Text = Report_ClientSite.LogoUrl;
                txtSiteUrl.Text = Report_ClientSite.SiteUrl;
                txtCrawlerConfig.Text = Report_ClientSite.CrawlerConfig;
                
                BindingGrvCrawlerConfig();
                
                if (Report_ClientSite.Report_ClientSiteViewModels != null && Report_ClientSite.Report_ClientSiteViewModels.Any())
                {
                    var report = Report_ClientSite.Report_ClientSiteViewModels.First();
                    txtReportName.Text = report.ReportName;
                    cbReportActive.Checked = report.IsActive.HasValue ? report.IsActive.Value : false;
                    grvTools.DataSource = (from row in report.ToolIds select new ToolModel { Key = row.Key, Value = row.Value }).ToList();
                }
                else
                {
                    txtReportName.Text = string.Empty;
                    txtValue.Text = string.Empty;
                    cbReportActive.Checked = false;
                    grvTools.DataSource = new List<ToolModel>();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var newId = Guid.NewGuid();
            Report_ClientSite.ClientId = Report_Client.Id;
            Report_ClientSite.BrandName = txtBrandName.Text;
            Report_ClientSite.Name = txtName.Text;
            Report_ClientSite.Description = txtDescription.Text;
            Report_ClientSite.LogoUrl = txtLogoUrl.Text;
            Report_ClientSite.SiteUrl = txtSiteUrl.Text;
            Report_ClientSite.CrawlerConfig = txtCrawlerConfig.Text;
            grvCrawlerConfig.DataSource = string.IsNullOrWhiteSpace(txtCrawlerConfig.Text) ?
                null : JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(txtCrawlerConfig.Text).Select(x => new ToolModel { Key = x.Key, Value = x.Value })
                .ToList();

            var toolIds = new Dictionary<string, string>();
            if (grvTools.Rows.Count > 0)
            {
                try
                {
                    toolIds = ((List<ToolModel>)grvTools.DataSource).ToDictionary(x => x.Key, x => (string)x.Value);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }

            if (Report_ClientSite.Report_ClientSiteViewModels == null || !Report_ClientSite.Report_ClientSiteViewModels.Any())
            {
                Report_ClientSite.Report_ClientSiteViewModels = new List<ClientSite_ReportViewModel>(){
                    new ClientSite_ReportViewModel() {
                        Id = Guid.NewGuid(),
                        ClientSiteId = newId,
                        IsActive = cbReportActive.Checked,
                        ReportName = txtReportName.Text,
                        ToolIds = toolIds
                    }
                };
            }
            else
            {
                Report_ClientSite.Report_ClientSiteViewModels.First().ReportName = txtReportName.Text;
                Report_ClientSite.Report_ClientSiteViewModels.First().IsActive = cbReportActive.Checked;
                Report_ClientSite.Report_ClientSiteViewModels.First().ToolIds = toolIds;
            }

            if (Report_ClientSite.Id == Guid.Empty)
            {
                Report_ClientSite.Id = newId;
                _client_ManagementService.AddClientSite(Report_ClientSite);
            }
            else
            {
                _client_ManagementService.UpdateClientSite(Report_ClientSite);
            }

            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void AddClientSiteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void btnAddTool_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cboKey.Text) && !string.IsNullOrEmpty(txtValue.Text))
            {
                var tools = ((List<ToolModel>)grvTools.DataSource);
                tools.Add(new ToolModel() { Key = cboKey.Text, Value = txtValue.Text });
                grvTools.DataSource = null;
                grvTools.DataSource = tools;
            }
        }


        private void grvTools_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (grvTools.Columns[e.ColumnIndex].Name == "Edit")
            {
                var tools = ((List<ToolModel>)grvTools.DataSource);

                getResult(this.ParentForm, tools[e.RowIndex], (ToolModel result) =>
                {
                    if (result != null)
                    {
                        tools[e.RowIndex] = result;

                        grvTools.DataSource = null;
                        grvTools.DataSource = tools;
                    }
                });
            }

            if (grvTools.Columns[e.ColumnIndex].Name == "Delete")
            {
                var tools = ((List<ToolModel>)grvTools.DataSource);
                tools.RemoveAt(e.RowIndex);
                grvTools.DataSource = null;
                grvTools.DataSource = tools;
            }
        }

        private void getResult(Form frmParent,ToolModel model, Action<ToolModel> callback)
        {
            EditToolForm frmDialog = new EditToolForm();
            frmDialog.ToolModel = model;
            frmDialog.BindingData();
            frmDialog.FormClosed += (object closeSender, FormClosedEventArgs closeE) =>
            {
                frmDialog.Dispose();
                frmDialog = null;
            };

            frmDialog.MdiParent = frmParent;

            frmDialog.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                if (frmDialog.DialogResult == DialogResult.OK)
                    callback(frmDialog.ToolModel);
                else
                    callback(null);
            };

            frmDialog.Show();
        }

        private void AddClientSiteForm_Load(object sender, EventArgs e)
        {
            cboKey.DataSource = "PingdomCheckId,ClientJiraServiceDeskId,PSIPerformanceEnabled,PSIAccessibilityEnabled,PSISeoEnabled,PSIPwaEnabled,GoogleAnalyticEnabled,GoogleSearchEnabled,ScreamingFrogEnabled".Split(',').ToList();
            cboKey.SelectedIndex = 0;
        }

        private void btnSelectFolderGA_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtFileSecretPathGA.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnSelectFolderGC_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtFileSecretPathGC.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnLoginOauthGA_Click(object sender, EventArgs e)
        {
            var dataSources = string.IsNullOrWhiteSpace(txtCrawlerConfig.Text)
                ? null
                : JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(txtCrawlerConfig.Text)
                    .Select(x => new ToolModel { Key = x.Key, Value = x.Value }).ToList();

            if (dataSources != null)
            {
                var gaObj = dataSources.FirstOrDefault(x => x.Key == "GoogleAnalytics");
                if (gaObj != null)
                {
                    string gaObjString = gaObj.Value.ToString();
                    var googleConfig = JsonConvert.DeserializeObject<GoogleCrawlerConfig>(gaObjString);
                    (new GoogleAnalytic.GoogleAnalytic()).AuthenticateOauth(
                        System.IO.Path.Combine(txtFileSecretPathGA.Text, googleConfig.GoogleOauthAccSecretJsonFile), googleConfig.GoogleOauthAccEmail,
                        false);
                }
            }
        }

        private void btnLoginOauthGC_Click(object sender, EventArgs e)
        {
            var dataSources = string.IsNullOrWhiteSpace(txtCrawlerConfig.Text)
                ? null
                : JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(txtCrawlerConfig.Text)
                    .Select(x => new ToolModel { Key = x.Key, Value = x.Value }).ToList();

            if (dataSources != null)
            {
                var gcObj = dataSources.FirstOrDefault(x => x.Key == "GoogleSearch");
                if (gcObj != null)
                {
                    string gcObjString = gcObj.Value.ToString();
                    var googleConfig = JsonConvert.DeserializeObject<GoogleCrawlerConfig>(gcObjString);
                    (new GoogleSearch.GoogleSearch()).AuthenticateOauth(
                        System.IO.Path.Combine(txtFileSecretPathGC.Text, googleConfig.GoogleOauthAccSecretJsonFile), googleConfig.GoogleOauthAccEmail,
                        false);
                }
            }
        }

        private void btnPreviewCrawlerConfig_Click(object sender, EventArgs e)
        {
            BindingGrvCrawlerConfig();
        }

        private void BindingGrvCrawlerConfig()
        {
            var dataSources = string.IsNullOrWhiteSpace(txtCrawlerConfig.Text)
                ? null
                : JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(txtCrawlerConfig.Text)
                    .Select(x => new ToolModel { Key = x.Key, Value = x.Value }).ToList();
            grvCrawlerConfig.DataSource = dataSources;

            bool isGAOauthLoginAvailable = false;
            bool isGCOauthLoginAvailable = false;
            if (dataSources != null)
            {
                var gaObj = dataSources.FirstOrDefault(x => x.Key == "GoogleAnalytics");
                if (gaObj != null)
                {
                    string gaObjString = gaObj.Value.ToString();
                    var googleConfig = JsonConvert.DeserializeObject<GoogleCrawlerConfig>(gaObjString);
                    if (!string.IsNullOrEmpty(googleConfig.GoogleOauthAccSecretJsonFile) &&
                        !string.IsNullOrEmpty(googleConfig.GoogleOauthAccEmail))
                    {
                        isGAOauthLoginAvailable = true;
                    }
                }

                var gcObj = dataSources.FirstOrDefault(x => x.Key == "GoogleSearch");
                if (gcObj != null)
                {
                    string gcObjString = gcObj.Value.ToString();
                    var googleConfig = JsonConvert.DeserializeObject<GoogleCrawlerConfig>(gcObjString);
                    if (!string.IsNullOrEmpty(googleConfig.GoogleOauthAccSecretJsonFile) &&
                        !string.IsNullOrEmpty(googleConfig.GoogleOauthAccEmail))
                    {
                        isGCOauthLoginAvailable = true;
                    }
                }
            }
            btnLoginOauthGA.Enabled = isGAOauthLoginAvailable;
            txtFileSecretPathGA.Enabled = isGAOauthLoginAvailable;
            btnLoginOauthGC.Enabled = isGCOauthLoginAvailable;
            txtFileSecretPathGC.Enabled = isGCOauthLoginAvailable;
        }

        private void btnTemplatePSI_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCrawlerConfig.Text))
            {
                txtCrawlerConfig.Text = string.Format(@"{{PsiUrl: ""{0}"", PsiUrlDeepCrawl: false, PsiSitemapUrl: """"}}", string.IsNullOrWhiteSpace(txtSiteUrl.Text) ? "https://yourUrlToCheck" : txtSiteUrl.Text);
            }
            else
            {
                MessageBox.Show("You already had a config value. To use the template, backup yours and clear it first!");
            }
        }

        private void btnTemplateScreamingFrog_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCrawlerConfig.Text))
            {
                txtCrawlerConfig.Text = string.Format(@"{{ScreamingFrogUrl: ""{0}""}}", string.IsNullOrWhiteSpace(txtSiteUrl.Text) ? "https://yourUrlToCheck" : txtSiteUrl.Text);
            }
            else
            {
                MessageBox.Show("You already had a config value. To use the template, backup yours and clear it first!");
            }
        }

        private void btnTemplateAll_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCrawlerConfig.Text))
            {
                txtCrawlerConfig.Text = string.Format(@"{{PsiUrl: ""{0}"", PsiUrlDeepCrawl: false, PsiSitemapUrl: """", ScreamingFrogUrl: ""{0}""}}", string.IsNullOrWhiteSpace(txtSiteUrl.Text) ? "https://yourUrlToCheck" : txtSiteUrl.Text);
            }
            else
            {
                MessageBox.Show("You already had a config value. To use the template, backup yours and clear it first!");
            }
        }
    }

    public class ToolModel
    {
        public string Key { get; set; }
        public dynamic Value { get; set; }
    }
}
