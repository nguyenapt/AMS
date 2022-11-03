using NUnit.Framework;
using System;
using AMS.Selenium.Config;
using AMS.Selenium.DynoMapper.Interfaces;
using AMS.Selenium.PageMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using log4net;

namespace AMS.Selenium.DynoMapper
{
    public class DynomapperRunningTest : DynomapperBase
    {
        ILog log = LogManager.GetLogger(typeof(DynomapperRunningTest));

        private IWebDriver driver;
        public string Guidelines { get; set; } = "WCAG 2.1 (Level AA)";

        // create date for report file.
        public static String monthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
        public static String day = DateTime.Now.Day.ToString();
        public static String year = DateTime.Now.Year.ToString();
        public static String reportDateFile = monthName + " " + day + ", " + year;  // December 8, 2020
        public WorkSpaceFolder WorkSpaceFolder { get; set; }

        public DynomapperRunningTest(string userName, string password, WorkSpaceFolder workSpaceFolder) : base(userName, password)
        {
            this.WorkSpaceFolder = workSpaceFolder;
        }

        public void RunningTestForProject(string projectName = "greenfieldlancaster")
        {
            try
            {
                // 1. =========== Start browser and open Accessibility page : 
                String chromePath = WorkSpaceFolder.ChromePath;
                driver = new ChromeDriver(chromePath);
                driver.Manage().Window.Maximize();

                var loginPage = new LoginPage(driver, this.UserName, this.Password);
                var homepage = new Homepage(driver);
                var accessibilityPage = new AccessibilityPage(driver);

                loginPage.LoginByAdminAccount();
                Assert.IsTrue(loginPage.VerifyDashboard());
                homepage.ClickOnLeftMenuItem("Accessibility");

                accessibilityPage.SelectAProjectBySearchName(projectName);
                accessibilityPage.SelectALevel(Guidelines);
                accessibilityPage.ClickRunTestButton();
                accessibilityPage.WaitUntilRunTestSuccessfully(reportDateFile);
                log.Info(projectName + ": OK");
            }
            catch (Exception e)
            {
                log.Error(projectName + ": FAILED, message: " + e.Message);
            }
        }

        public void ParallelRunningTestForProjects(string projectName = "nordicbeautyhouse.com")
        {
            try
            {
              
                String chromePath = WorkSpaceFolder.ChromePath;
                driver = new ChromeDriver(chromePath);
                driver.Manage().Window.Maximize();

                // 2. =========== initial the objects : 
                var loginPage = new LoginPage(driver, UserName, Password);
                var homepage = new Homepage(driver);
                var accessibilityPage = new AccessibilityPage(driver);

                loginPage.LoginByAdminAccount();
                Assert.IsTrue(loginPage.VerifyDashboard());
                homepage.ClickOnLeftMenuItem("Accessibility");

                // 5. =========== run test and wait for completed : 
                try
                {
                    bool selectAble = accessibilityPage.SelectAProjectBySearchName(projectName);
                    if (!selectAble)
                    {
                        log.Error(projectName + ": FAILED, message: PROJECT IS NOT FOUND.");
                    }
                    else
                    {
                        accessibilityPage.SelectALevel(Guidelines);
                        accessibilityPage.ClickRunTestButton();
                        System.Threading.Thread.Sleep(15000);
                        log.Info(projectName + ": RUNNING !");
                    }
                }
                catch (Exception e)
                {
                    log.Error(projectName + ": FAILED, message: " + e.Message);
                }
            }
            catch (Exception e)
            {
                log.Error("FAILED, message: " + e.Message);
            }
        }

        public void SequentialRunningTestForProjects(string projectName = "www.grantthornton.co.uk")
        {
            try
            {
               
                String chromePath = WorkSpaceFolder.ChromePath;
                driver = new ChromeDriver(chromePath);
                driver.Manage().Window.Maximize();

                // 2. =========== initial the objects : 
                var loginPage = new LoginPage(driver, UserName, Password);
                var homepage = new Homepage(driver);
                var accessibilityPage = new AccessibilityPage(driver);

                loginPage.LoginByAdminAccount();
                Assert.IsTrue(loginPage.VerifyDashboard());
                homepage.ClickOnLeftMenuItem("Accessibility");

                // 5. =========== run test and wait for completed : 
                try
                {
                    bool selectAble = accessibilityPage.SelectAProjectBySearchName(projectName);
                    if (!selectAble)
                    {
                        log.Error(projectName + ": FAILED, message: No test found.");
                    }
                    else
                    {
                        accessibilityPage.SelectALevel(Guidelines);
                        accessibilityPage.ClickRunTestButton();
                        accessibilityPage.WaitUntilRunTestSuccessfully(WorkSpaceFolder.GetReportDateFile());
                        log.Info(projectName + ": OK");
                    }
                }
                catch (Exception e)
                {
                    log.Error(projectName + ": FAILED, message: " + e.Message);
                }

            }
            catch (Exception e)
            {
                log.Error("FAILED, message: " + e.Message);
            }
        }
    }


}
