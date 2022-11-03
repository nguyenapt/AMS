using System;
using System.Globalization;
using System.IO;

namespace AMS.Selenium.Config
{
    public class WorkSpaceFolder
    {
        public string DownloadFilePath { get; set; }
        public string DataFilePath { get; set; }

        public string ChromePath
        {
            get
            {
                return DataFilePath + "\\chromedriver_latest";
            }
        }

        public WorkSpaceFolder(string dataFilePath, string downloadFilePath)
        {
            this.DataFilePath = dataFilePath;
            this.DownloadFilePath = downloadFilePath; //+ "\\" + DateTime.UtcNow.Date.ToString("yyyyMMdd");
        }

        public void CreateCssJSFile(string sourcePath, string destinationPath)
        {
            Directory.CreateDirectory(destinationPath);

            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
        }
        
        public string GetReportDateFile()
        {
            var date = DateTime.Now;
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month) + " " + date.Day + ", " + date.Year;      
        }

        public void DeleteDefaultFile(string fileNameContains)
        {
            string[] filePaths = Directory.GetFiles(DownloadFilePath);
            foreach (string fileName in filePaths)
            {
                if (fileName.Contains(fileNameContains))
                {
                    File.Delete(fileName);
                }
            }
        }

        public void WaitForFileDownloadSuccessfully()
        {

            while (IsDownloadingFile())
            {
                System.Threading.Thread.Sleep(1000 * 20);  // wait 20s
            }
        }

        public bool IsDownloadingFile()
        {
            string[] filePathsNew = Directory.GetFiles(DownloadFilePath);

            foreach (string fileName in filePathsNew)
            {
                if (fileName.Contains("pdf.crdownload"))
                {
                    return true;
                }
            }

            return false;
        }

        public void RenameFileDowload(string projectName, string containsName, string newName)
        {
            string[] filePathsNew = Directory.GetFiles(DownloadFilePath);

            Directory.CreateDirectory(DownloadFilePath + "\\" + projectName +"\\"+ DateTime.UtcNow.Date.ToString("yyyyMMdd"));

            string newDownloadFilePath = DownloadFilePath + "\\" + projectName + "\\" + DateTime.UtcNow.Date.ToString("yyyyMMdd") + "\\" + newName;

            foreach (string fileName in filePathsNew)
            {
                if (fileName.Contains(containsName))
                {
                    File.Move(fileName, newDownloadFilePath);
                }
            }
        }
    }
}
