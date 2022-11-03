using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using AMS.Selenium.PageMethods;
using AMS.Selenium.Config;
using AMS.Selenium.DynoMapper.Interfaces;
using NUnit.Framework;
using log4net;
using System.Collections.Generic;

namespace AMS.Selenium.DynoMapper
{
    public class DynomapperDownloadReport : DynomapperBase
    {
        ILog log = LogManager.GetLogger(typeof(DynomapperDownloadReport));
        private IWebDriver driver;
        public WorkSpaceFolder WorkSpaceFolder { get; set; }
        public DynomapperDownloadReport(string userName, string password, WorkSpaceFolder workSpaceFolder) : base(userName, password)
        {
            this.WorkSpaceFolder = workSpaceFolder;
        }

        public void DownloadChartForProject(string projectName, Guid clientSiteId)
        {
            try
            {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddUserProfilePreference("download.default_directory", WorkSpaceFolder.DownloadFilePath);
                chromeOptions.AddUserProfilePreference("intl.accept_languages", "nl");
                chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                chromeOptions.AddArgument("--headless");//hide browser

                driver = new ChromeDriver(WorkSpaceFolder.ChromePath, chromeOptions);
                driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 3, 20);
                driver.Manage().Window.Maximize();

                var loginPage = new LoginPage(driver, this.UserName, this.Password);

                loginPage.LoginByAdminAccount();
                Assert.IsTrue(loginPage.VerifyDashboard());

                var homepage = new Homepage(driver);
                homepage.ClickOnLeftMenuItem("Accessibility");

                var accessibilityPage = new AccessibilityPage(driver);

                bool isProjectFound = accessibilityPage.SelectAProjectBySearchName(projectName);
                if (!isProjectFound)
                {
                    log.Error(projectName + ": FAILED, message: PROJECT IS NOT FOUND.");
                }
                else
                {
                    bool isTestFound = accessibilityPage.CheckProjectNoTestFound();

                    if (isTestFound)
                    {
                        log.Error(projectName + ": FAILED, message: No test found.");
                    }
                    else
                    {
                        accessibilityPage.RemoveUnusedItems(driver);
                        accessibilityPage.DownloadBoardChart(driver, clientSiteId.ToString(), WorkSpaceFolder);
                        log.Info(projectName + ": OK");
                    }
                }
                driver.Quit();
            }
            catch (Exception e)
            {
                log.Error(projectName + ": FAILED, message: " + e.Message);
            }
        }

        public List<string> GetListingProjects()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");

            driver = new ChromeDriver(WorkSpaceFolder.ChromePath,chromeOptions);
            driver.Manage().Window.Maximize();

            var loginPage = new LoginPage(driver,this.UserName,this.Password);
            var homepage = new Homepage(driver);
            var accessibilityPage = new AccessibilityPage(driver);

            loginPage.LoginByAdminAccount();
            Assert.IsTrue(loginPage.VerifyDashboard());
            homepage.ClickOnLeftMenuItem("Accessibility");

            return accessibilityPage.GetAllProjectsName();
        }

        public void DownloadReportForProject(string projectName, Guid clientSiteId)
        {
            try
            {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddUserProfilePreference("download.default_directory", WorkSpaceFolder.DownloadFilePath);
                chromeOptions.AddUserProfilePreference("intl.accept_languages", "nl");
                chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                chromeOptions.AddArgument("--headless");

                driver = new ChromeDriver(WorkSpaceFolder.ChromePath, chromeOptions);
                driver.Manage().Window.Maximize();

                var loginPage = new LoginPage(driver,this.UserName,this.Password);
                var homepage = new Homepage(driver);
                var accessibilityPage = new AccessibilityPage(driver);

                loginPage.LoginByAdminAccount();
                Assert.IsTrue(loginPage.VerifyDashboard());

                homepage.ClickOnLeftMenuItem("Accessibility");

                bool selectAble = accessibilityPage.SelectAProjectBySearchName(projectName);
                if (!selectAble)
                {
                    log.Error(projectName + ": FAILED, message: PROJECT IS NOT FOUND.");
                }
                else
                {
                    bool isTestFound = accessibilityPage.CheckProjectNoTestFound();

                    if (isTestFound)
                    {
                        log.Error(projectName + ": FAILED, message: No test found.");
                    }
                    else
                    {
                        var currentRundate = accessibilityPage.GetLatestRunDateSuccessfully();
                        var rundate = accessibilityPage.GetDateFromString(currentRundate);         
                        var homePageReportName = clientSiteId + "_HomePage.pdf";
                        var summaryReportName = clientSiteId + "_SummaryReport.pdf";

                        accessibilityPage.ViewTheLatestSuccessfulReport();
                        accessibilityPage.DownloadSummaryReportAndSaveToFolder(WorkSpaceFolder, clientSiteId.ToString(), summaryReportName);
                        driver.Navigate().Refresh();
                        accessibilityPage.DownloadHomepageReportAndSaveToFolder(WorkSpaceFolder, clientSiteId.ToString(), homePageReportName);

                        log.Info(projectName + ": OK");

                        accessibilityPage.CloseTheReportFile();

                        accessibilityPage.BackToAccessibilityPage();
                    }
                }

                driver.Quit();
            }
            catch (Exception e)
            {
                log.Error(projectName + ": FAILED, message: " + e.Message);
            }
        }

        public void DownloadBoardChartForProject(string projectName, Guid clientSiteId)
        {
            try
            {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddUserProfilePreference("download.default_directory", WorkSpaceFolder.DownloadFilePath);
                chromeOptions.AddUserProfilePreference("intl.accept_languages", "nl");
                chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");
                chromeOptions.AddArgument("--headless");//hide browser

                driver = new ChromeDriver(WorkSpaceFolder.ChromePath, chromeOptions);
                driver.Manage().Timeouts().PageLoad = new TimeSpan(0, 3, 20);
                driver.Manage().Window.Maximize();

                var loginPage = new LoginPage(driver, this.UserName, this.Password);

                loginPage.LoginByAdminAccount();
                Assert.IsTrue(loginPage.VerifyDashboard());

                var homepage = new Homepage(driver);
                homepage.ClickOnLeftMenuItem("Accessibility");

                var accessibilityPage = new AccessibilityPage(driver);

                bool isProjectFound = accessibilityPage.SelectAProjectBySearchName(projectName);
                if (!isProjectFound)
                {
                    log.Error(projectName + ": FAILED, message: PROJECT IS NOT FOUND.");
                }
                else
                {
                    bool isTestFound = accessibilityPage.CheckProjectNoTestFound();

                    if (isTestFound)
                    {
                        log.Error(projectName + ": FAILED, message: No test found.");
                    }
                    else
                    {
                        accessibilityPage.RemoveUnusedItems(driver);
                        accessibilityPage.DownloadBoardChart(driver, clientSiteId.ToString(), WorkSpaceFolder);
                        log.Info(projectName + ": OK");
                    }
                }
                driver.Quit();
            }
            catch (Exception e)
            {
                log.Error(projectName + ": FAILED, message: " + e.Message);
            }
        }

        public string GetLatestRunDate(string projectName)
        {
            try
            {
                var options = new ChromeOptions();
                options.AddArgument("--headless");

                driver = new ChromeDriver(WorkSpaceFolder.ChromePath, options);
                driver.Manage().Window.Maximize();

                var loginPage = new LoginPage(driver, this.UserName,this.Password);
                var homepage = new Homepage(driver);
                var accessibilityPage = new AccessibilityPage(driver);

                loginPage.LoginByAdminAccount();
                Assert.IsTrue(loginPage.VerifyDashboard());
                homepage.ClickOnLeftMenuItem("Accessibility");

                bool isProjectFound = accessibilityPage.SelectAProjectBySearchName(projectName);
                if (!isProjectFound)
                {
                    log.Error(projectName + ": FAILED, message: PROJECT IS NOT FOUND.");
                }
                else
                {
                    bool isTestFound = accessibilityPage.CheckProjectNoTestFound();

                    if (isTestFound)
                    {
                        log.Error(projectName + ": FAILED, message: No test found.");
                    }
                    else
                    {
                        log.Info(projectName + ": OK");

                        return accessibilityPage.GetLatestRunDateSuccessfully();
                    }
                }
            }
            catch (Exception e)
            {
                log.Error("FAILED, message: " + e.Message);
            }

            return "";
        }
    }
}
