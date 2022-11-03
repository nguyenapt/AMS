using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;
using AMS.ReportAutomation.Data.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AMS.ReportAutomation.Data.Services.Crawler
{
    public class ScreamingFrog_Service : IScreamingFrog_Service
    {
        protected readonly IAMSLogger _logger;
        private IReport_ClientSitesRepository _report_ClientSitesRepository;

        public string SpiderProgramFolder { get; set; }
        public string SpiderConfigFile { get; set; }
        public string SpiderOutputFolder { get; set; }

        public ScreamingFrog_Service(IReport_ClientSitesRepository report_ClientSitesRepository, IAMSLogger logger)
        {
            _logger = logger;
            _report_ClientSitesRepository = report_ClientSitesRepository;
        }

        public void Crawl()
        {
            if (string.IsNullOrWhiteSpace(SpiderProgramFolder) || string.IsNullOrWhiteSpace(SpiderOutputFolder))
                throw new Exception("You must configure SpiderProgramFolder and SpiderOutputFolder before crawling.");

            var clientSites = GetClientSites();
            if (clientSites != null && clientSites.Any())
            {
                foreach (var clientSite in clientSites)
                {
                    string outputFolder = Path.Combine(SpiderOutputFolder, clientSite.CrawlerConfig.SfUrlWithoutProtocol.Replace("/","_").Replace(@"\", "_") + " " + clientSite.ClientSiteId);
                    _logger.Information($"Getting spider stats for the site {clientSite.CrawlerConfig.ScreamingFrogUrl} to folder {outputFolder}");                    
                    try
                    {                        
                        if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);
                        string spiderProgramExePath = Path.Combine(SpiderProgramFolder, "ScreamingFrogSEOSpiderCli.exe");

                        //Note: If the command line program crashs (i.e. the commandArgs has wrong syntax), it will crash the Crawler Windows Service that trigger it, so it is important to get this right.
                        string commandArgs = "--crawl " + clientSite.CrawlerConfig.ScreamingFrogUrl +
                            " --headless --save-crawl --output-folder \"" + outputFolder + "\"  --timestamped-output --overwrite" +
                            " --export-format xlsx --export-tabs \"Internal:All,Response Codes:All,Page Titles:All,Meta Description:All,Meta Keywords:All,H1:All,H2:All,Content:All,Images:All\" -task-name 247-automation";
                        if (!string.IsNullOrWhiteSpace(SpiderConfigFile))
                        {
                            commandArgs += $" --config \"{SpiderConfigFile}\"";
                        }

                        _logger.Information($"\"{spiderProgramExePath}\" {commandArgs}");
                        _logger.Information(RunSpiderCli(spiderProgramExePath, commandArgs));
                    }
                    catch (Exception ex)
                    {
                        string errMessage = $"{ex.Message} {(ex.InnerException != null ? "InnerException: " + ex.InnerException.Message : string.Empty)} StackTrace: {ex.StackTrace}";
                        _logger.Error(errMessage);

                        //Continue to the next site/check
                        continue;
                    }
                }
            }
        }

        public List<ScreamingFrogCheckSite> GetClientSites()
        {
            //Use the url from CrawlerConfig value
            var data = _report_ClientSitesRepository.FindBy(x => x.CrawlerConfig != null && x.CrawlerConfig != string.Empty);

            var list = new List<ScreamingFrogCheckSite>();

            if (data != null)
            {
                foreach (var site in data)
                {
                    var client = new ScreamingFrogCheckSite
                    {
                        ClientSiteId = site.Id,
                        CrawlerConfig = JsonConvert.DeserializeObject<ScreamingFrogCrawlerConfig>(site.CrawlerConfig)
                    };

                    if (!string.IsNullOrWhiteSpace(client.CrawlerConfig.ScreamingFrogUrl)) list.Add(client);
                }
            }

            return list;
        }

        private string RunSpiderCli(string spiderFilePath, string arguments = null)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = spiderFilePath;
                if (!string.IsNullOrEmpty(arguments))
                {
                    process.StartInfo.Arguments = arguments;
                }

                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.UseShellExecute = false;

                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;

                //// Use AppendLine rather than Append since args.Data is one line of output, not including the newline character.
                //var stdOutput = new StringBuilder();
                //process.OutputDataReceived += (sender, args) => { if (!args.Data.Contains("SpiderProgress")) stdOutput.AppendLine(args.Data); };
                process.OutputDataReceived += (sender, args) => { if (args != null && args.Data != null && !args.Data.Contains("SpiderProgress")) _logger.Information($"- {args.Data}"); };

                string stdError = null;
                try
                {
                    process.Start();
                    process.BeginOutputReadLine();
                    stdError = process.StandardError.ReadToEnd();
                    process.WaitForExit();
                }
                catch (Exception e)
                {
                    throw new Exception($"OS error while executing \"{spiderFilePath} {arguments}\": {e.Message} {e}");
                }

                if (process.ExitCode == 0)
                {
                    //return stdOutput.ToString();
                    return $"\"{spiderFilePath} {arguments}\" completed successfully!";
                }
                else
                {
                    var sbMessage = new StringBuilder();

                    if (!string.IsNullOrEmpty(stdError))
                    {
                        sbMessage.AppendLine(stdError);
                    }

                    //if (stdOutput.Length != 0)
                    //{
                    //    sbMessage.AppendLine("Std output:");
                    //    sbMessage.AppendLine(stdOutput.ToString());
                    //}

                    throw new Exception($"\"{spiderFilePath} {arguments}\" finished with exit code = " + process.ExitCode + ": " + sbMessage);
                }
            }
        }

        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime);
            
            //Scan folder SpiderOutputFolder and delete old data files
            var directories = Directory.GetDirectories(SpiderOutputFolder);
            if (directories != null)
            {
                foreach (var dir in directories)
                {
                    var subDirectories = Directory.GetDirectories(dir);
                    //BatchDelete(Directory.GetCreationTimeUtc(subDir) < retentionTimeCalc);
                    foreach (var subDir in subDirectories)
                    {
                        //_logger.Information(subDir + " " + Directory.GetCreationTimeUtc(subDir));
                        if (Directory.GetCreationTimeUtc(subDir) < retentionTimeCalc)
                        {
                            Directory.Delete(subDir, true);
                        }
                    }
                }
            }            
        }
    }
}