using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using CsQuery.Utility;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using AMS.Selenium.Config;

namespace AMS.Selenium.PageMethods
{
    public class AccessibilityPage
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private WebDriverWait waitQueued;

        public AccessibilityPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            waitQueued = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            PageFactory.InitElements(driver, this);
            //wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }

        [FindsBy(How = How.XPath, Using = "//div/form[@action='/accessibility']")]
        private IWebElement chosenProjectElement;

        [FindsBy(How = How.XPath, Using = "//div[@class='chosen-search']/input[@type='text']")]
        private IWebElement searchProjectTextboxElement;

        [FindsBy(How = How.XPath, Using = "//a[contains(@class,'btn-run-test')]")]
        private IWebElement runTestBtnElement;

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'text-right')]/button[@type='submit']")]
        private IWebElement runTestPopupBtnElement;


        [FindsBy(How = How.Id, Using = "guide-filter")]
        private IWebElement chooseGuideSelectboxElement;

        // report that not running or false
        [FindsBy(How = How.XPath, Using = "//div[@data-pending and string-length(@data-pending)=0 and not(.//div[@class='infobox error-bg mrg0B mrg10T'])][1]//a[@title='View Report']")]
        private IWebElement firstViewSuccessfulReportBtnElement;

        [FindsBy(How = How.XPath, Using = "//div[@class='test-result-list-wrap']/table/tbody/tr[1]//a[text()='Review']")]
        private IWebElement firstReviewLinkElement;

        [FindsBy(How = How.XPath, Using = "//a/span[text()='PDF']")]
        private IWebElement pdfBtnElement;

        [FindsBy(How = How.XPath, Using = "//div[@class='test-item-row']//span[@class='link-progress']")]
        private IWebElement testInProgressElement;

        String reportDate = "//div[@class='test-item-row complete'][1]//div[@class='date' and contains(text(),'dateDefault')]";

        [FindsBy(How = How.XPath, Using = "//ul[@class='chosen-results']//li")]
        private IList<IWebElement> projectListULLI;

        [FindsBy(How = How.XPath, Using = "//ul[@class='chosen-results']/li[@class='no-results']")]
        private IWebElement projectListULLINull;

        // display queue button and in queue to run test
        [FindsBy(How = How.XPath, Using = "//span[@class='label primary-bg link-progress-label' and text()='Queued']")]
        private IWebElement QueuedProgressElement;

        // run successfully: not pending and not failsed
        [FindsBy(How = How.XPath, Using = "//div[@data-pending and string-length(@data-pending)=0 and not(.//div[@class='infobox error-bg mrg0B mrg10T'])][1]//div[@class='date']")]
        private IWebElement runSuccessfullyDate_firshElement;

        // running and display progress bar
        [FindsBy(How = How.XPath, Using = "//div[@data-pending='1'][1]//div[@class='date']")]
        private IWebElement runningDateElement_firshElement;


        // run failed and display error message: 
        [FindsBy(How = How.XPath, Using = "//div[@data-pending and string-length(@data-pending)=0][1]/div[@class='infobox error-bg mrg0B mrg10T']")]
        private IWebElement runFalsedMessage_firshElement;

        // run failed, display error message and date: 
        [FindsBy(How = How.XPath, Using = "//div[@data-pending and string-length(@data-pending)=0][1]/div[@class='infobox error-bg mrg0B mrg10T']/..//div[@class='date']")]
        private IWebElement runFalsedDate_firshElement;

        [FindsBy(How = How.XPath, Using = "//a[@class='chosen-single']/span")]
        private IWebElement selectedProjectNameElement;

        string projectlistindexLI = "//ul[@class='chosen-results']//li[index]";

        [FindsBy(How = How.XPath, Using = "//div[@id='accessibility-result-popup']//h3[@class='content-header clearfix']//i[contains(@class,'glyph-icon icon-remove')]")]
        private IWebElement closeTheReportFileElement;

        [FindsBy(How = How.XPath, Using = "//span[contains(text(),'Back')]")]
        private IWebElement backToAccessibilityPageElement;

        [FindsBy(How = How.XPath, Using = "//span[contains(text(),'Export')]")]
        private IWebElement exportButton;

        [FindsBy(How = How.XPath, Using = "//a[contains(text(),'PDF')]")]
        private IWebElement exportPDFLink;

        [FindsBy(How = How.XPath, Using = "//*[@id='accessibility-wrap']//div[@class='list-wrap test-list-wrap']")]
        private IWebElement boardChartContent;

        [FindsBy(How = How.XPath, Using = "//div[@id='accessibility-wrap']//div[@class='alert-box']/h3[text()='No test found.']")]
        private IWebElement noTestFoundElement;
        

        public bool SelectAProjectBySearchName(String projectName)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(chosenProjectElement));
            chosenProjectElement.Click();
            searchProjectTextboxElement.Clear();
            searchProjectTextboxElement.SendKeys(projectName);
            System.Threading.Thread.Sleep(1000);
            searchProjectTextboxElement.SendKeys(Keys.Enter);
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            // check if "no results match" then close the dropdownbox for the next loop.
            bool ProjectNotFound = false;
            try
            {
                ProjectNotFound = projectListULLINull.Enabled;
                if (ProjectNotFound)
                {
                    chosenProjectElement.Click();
                    return !ProjectNotFound;
                }
            // Below case is not available all the time
            //else {
            //    Console.WriteLine("***************  PROJECT FOUND");
            //    wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            //    Console.WriteLine(" 0_9 {0}", ProjectNotFound);
            //}

            }
            catch (Exception)
            {
               // In case can't get projectListULLINull object 
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
                return !ProjectNotFound;
            }

            return ProjectNotFound;
        }

        public bool CheckProjectNoTestFound()
        {
            bool isNotFound = false;
            try
            {
                isNotFound = noTestFoundElement.Displayed;
                return isNotFound;  // true
            } catch (Exception)
            {
                return isNotFound;  // false
            }
           
        }

        public void SelectALevel(String levelName)
        {
            SelectElement select = new SelectElement(chooseGuideSelectboxElement);
            select.SelectByText(levelName);
        }
        public void ClickRunTestButton()
        {
            runTestBtnElement.Click();
            System.Threading.Thread.Sleep(2000);
            runTestPopupBtnElement.Click();
        }
        public void ClickToSelectProject()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(chosenProjectElement));
            chosenProjectElement.Click();
            System.Threading.Thread.Sleep(2000);
        }

        public void ViewTheLatestSuccessfulReport()
        {
            firstViewSuccessfulReportBtnElement.Click();
        }
        public void ReviewHomepageReport()
        {
            firstReviewLinkElement.Click();
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }
        public void ClickPdfButton()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(pdfBtnElement));
            pdfBtnElement.Click();
        }

        public void DownloadHomepageReportAndSaveToFolder(WorkSpaceFolder folder, string projectName, String reportFileName)
        {
            // 4. =========== check if default file exits then delete it  
            folder.DeleteDefaultFile("pdf.crdownload");
            folder.DeleteDefaultFile("accessibility_review_");

            // 5. =========== download the latest file
            ReviewHomepageReport();
            ClickPdfButton();
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            System.Threading.Thread.Sleep(3000);

            // 6. =========== wait for downloading successfully
            folder.WaitForFileDownloadSuccessfully();

            // 7. =========== remane file by current project name : 
            folder.RenameFileDowload(projectName, "accessibility_review_", reportFileName);
        }

        public void DownloadHomepageReportFromProjectDetailPage(WorkSpaceFolder folder, string projectName)
        {
            // 4. =========== check if default file exits then delete it  
            folder.DeleteDefaultFile("pdf.crdownload");
            folder.DeleteDefaultFile("accessibility_review_");

            string currentRundate = GetLatestRunDateSuccessfully();  // December 15, 2020 4:28 PM
            string rundate = "";
            rundate = currentRundate.Replace(", ", "_");  // December 15_2020 4:28 PM
            rundate = rundate.Replace(":", "_");                  // December 15_2020 4_28 PM
            rundate = rundate.Replace(" ", "_");                  // December_15_2020_4_28_PM
            string newName = projectName + "_Homepage_" + rundate + ".pdf";

            // 5. =========== download the latest file                 

            ReviewHomepageReport();
            ClickPdfButton();
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            System.Threading.Thread.Sleep(3000);

            // 6. =========== wait for downloading successfully
            folder.WaitForFileDownloadSuccessfully();

            // 7. =========== remane file by current project name : 
            folder.RenameFileDowload(projectName, "accessibility_review_", newName);
        }

        public void DownloadSummaryReportAndSaveToFolder(WorkSpaceFolder folder, string clientSiteId, string reportFileName)
        {
            // 4. =========== check if default file exits then delete it   
            folder.DeleteDefaultFile("pdf.crdownload");
            folder.DeleteDefaultFile("accessibility_report_");

            // 5. =========== download the latest file                             
            exportButton.Click();
            exportPDFLink.Click();
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            System.Threading.Thread.Sleep(3000);

            // 6. =========== wait for downloading successfully
            folder.WaitForFileDownloadSuccessfully();

            // 7. =========== remane file by current project name : 
            try
            {
                folder.DeleteDefaultFile(reportFileName);
            }
            catch
            {

            }

            folder.RenameFileDowload(clientSiteId, "accessibility_report_", reportFileName);
        }

        public string GetLatestRunDateSuccessfully()
        {
            string successfulDate = runSuccessfullyDate_firshElement.Text;
            return successfulDate;
        }
        public string GetLatestRunDateStatus()
        {
            string message = "";
            // check the latest running date : 

            try
            {
                bool isRunning = runningDateElement_firshElement.Displayed;

                if (isRunning)
                {
                    var runningDate = runningDateElement_firshElement.Text;
                    message = runningDate + ": is running.";
                }
            }
            catch (Exception) {

            }

            try
            {
                // check the latest falsed run date : 
                bool isFalsed = runFalsedMessage_firshElement.Displayed;

                if (isFalsed)
                {
                    var falsedDate = runFalsedDate_firshElement.Text;
                    var falsedMessage = runFalsedMessage_firshElement.Text;
                    message = message + "\n" + falsedDate + ": is falsed, and message: " + falsedMessage;
                }

            }
            catch (Exception) { 
            }
            try
            {
                // check the latest run successfully date :
                bool isSuccessful = runSuccessfullyDate_firshElement.Displayed;
                if (isSuccessful)
                {
                    var successfulDate = runSuccessfullyDate_firshElement.Text;
                    message = message + "\n" + successfulDate + ": is successful.";
                }
            }
            catch (Exception) {

            }

            return message;
        }
        public void WaitUntilRunTestSuccessfully(string currentDate)
        {
            System.Threading.Thread.Sleep(5000);

            // wait for queque bar disable and progress bar display.
            waitQueued.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//span[@class='label primary-bg link-progress-label' and text()='Queued']")));
            // wait for testing complete.
            bool display = testInProgressElement.Displayed;

            try
            {
                while (display)
                {

                    System.Threading.Thread.Sleep(1000 * 60);  // Milliseconds(1000) * Seconds(60) * Minutes(10) = sleep in 1 minute.
                    display = testInProgressElement.Displayed;
                }
            }
            catch (Exception)
            {
                // verify downloaded file: 
                reportDate = reportDate.Replace("dateDefault", currentDate);
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(driver.FindElement(By.XPath(reportDate))));
            }

        }
        public void SelectAProjectByIndex(string i)
        {
            projectlistindexLI = projectlistindexLI.Replace("index", i);
            System.Threading.Thread.Sleep(2000);
            IWebElement selectedProject = driver.FindElement(By.XPath(projectlistindexLI));
            //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(selectedProject));            
            selectedProject.Click();
            projectlistindexLI = projectlistindexLI.Replace(i, "index");
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

        }
        public List<string> GetAllProjectsName()
        {
            List<string> projectNameList = new List<string>();

            // 3. =========== get the amount of project : 
            ClickToSelectProject();
            var projectNumber = GetAmountOfProject();

            // 4. =========== get all projects name : 
            for (int i = 1; i < projectNumber + 1; i++)
            {
                // 2. get project name : 
                string number = i.ToString();
                projectlistindexLI = projectlistindexLI.Replace("index", number);
                System.Threading.Thread.Sleep(1000);
                IWebElement selectedProject = driver.FindElement(By.XPath(projectlistindexLI));
                string projectName = selectedProject.Text;
                // get the project name : www.electroluxarabia.com/ (8 tests)  => www.electroluxarabia.com
                var projectNameSplit = projectName.Split('/');
                var projectNameSplit2 = projectNameSplit[0].Split('(');
                string projectNameFirst = projectNameSplit2[0];

                projectNameList.Add(projectNameFirst);
                projectlistindexLI = projectlistindexLI.Replace(number, "index");
            }

            return projectNameList;
        }
        public int GetAmountOfProject()
        {
            int amount = projectListULLI.Count;
            return amount;
        }
        public string GetSelectedProjectName()
        {
            string projectName = selectedProjectNameElement.Text;
            var projectNameSplit = projectName.Split('/');
            var projectNameSplit2 = projectNameSplit[0].Split('(');
            string projectNameFirst = projectNameSplit2[0];
            //Console.WriteLine("=========== projectNameFirst: " + projectNameFirst);
            return projectNameFirst;

        }
        public void CloseTheReportFile()
        {
            closeTheReportFileElement.Click();
        }
        public void BackToAccessibilityPage()
        {
            backToAccessibilityPageElement.Click();
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }
        public void RunTestSuccessfullyForAProject(string projectName, string guidelines, string reportDateFile)
        {
            SelectAProjectBySearchName(projectName);
            SelectALevel(guidelines);
            ClickRunTestButton();
            WaitUntilRunTestSuccessfully(reportDateFile);
        }

        public string GetDateFromString(string date)
        {
            DateTime parsedDateTime;
            string strDateTime = "";
            if (DateTime.TryParse(date, out parsedDateTime))
            {
                strDateTime = parsedDateTime.ToString("yyyyMMdd");
            }
            else
            {
                strDateTime = date;
            }
            Console.WriteLine(strDateTime);
            return strDateTime;
        }

        public void RemoveUnusedItems(IWebDriver driver)
        {
            string pageHeader_ID = "page-header";
            string pageSidebar_ID = "page-sidebar";
            string pageTitle_ID = "page-title";
            string rowHeader_Class = "row";
            string verticalLine_Class = "divider";
            string testItemRowList_Class = "test-item-row";

            IJavaScriptExecutor jsExecute = (IJavaScriptExecutor)driver;

            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            // remove page Header
            jsExecute.ExecuteScript("return document.getElementById('" + pageHeader_ID + "').remove();");
            System.Threading.Thread.Sleep(1000);
            jsExecute.ExecuteScript("return document.getElementById('" + pageSidebar_ID + "').remove();");
            System.Threading.Thread.Sleep(1000);
            jsExecute.ExecuteScript("return document.getElementById('" + pageTitle_ID + "').remove();");
            System.Threading.Thread.Sleep(1000);
            jsExecute.ExecuteScript("return document.getElementsByClassName('" + rowHeader_Class + "')[0].remove();");
            System.Threading.Thread.Sleep(1000);
            jsExecute.ExecuteScript("return document.getElementsByClassName('" + verticalLine_Class + "')[0].remove();");
            System.Threading.Thread.Sleep(1000);
            jsExecute.ExecuteScript("return document.getElementsByClassName('" + verticalLine_Class + "')[0].remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementsByClassName('result-gauges')[0].remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementById('accessibility-test-popup').remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementById('accessibility-schedule-popup').remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementById('accessibility-notification-popup').remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementById('sitemap-creation-popup').remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementById('sitemap-crawler-popup').remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementById('import-xml-popup').remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementById('import-text-popup').remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementById('scratch-sitemap-popup').remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementById('edit-sitemap-popup').remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementById('sitemap-duplicate-popup').remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementById('sitemap-merge-popup').remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementsByClassName('chart-toolbar')[0].remove();");
            System.Threading.Thread.Sleep(1000);

            jsExecute.ExecuteScript("return document.getElementsByClassName('daterangepicker')[0].remove();");
            System.Threading.Thread.Sleep(1000);

            IList<IWebElement> listResultRow = driver.FindElements(By.ClassName(testItemRowList_Class));

            for (int i = 0; i < listResultRow.Count; i++)
            {
                jsExecute.ExecuteScript("return document.getElementsByClassName('" + testItemRowList_Class + "')[0].remove();");
                System.Threading.Thread.Sleep(1000);
            }

        }


        public void DownloadBoardChart(IWebDriver driver, string clientSiteId, WorkSpaceFolder f)
        {
            // fileChart : ....DynoMapper_Selenium\DownloadReport\20210107\www.greenfieldlancaster.com.html            
            string fileChart = f.DownloadFilePath + "\\" + clientSiteId + ".html";
            //Console.WriteLine("============= fileChart " + fileChart);

            // save file : 
            string PageSourceHTML = driver.PageSource;
            System.Threading.Thread.Sleep(10000);
            System.IO.File.WriteAllText(fileChart, PageSourceHTML);

            // re-format html file : 
            ReformatHtmlFile(fileChart);
            f.RenameFileDowload(clientSiteId, clientSiteId + ".html", clientSiteId + ".html");

        }

        public void ReformatHtmlFile(string htmlFile)
        {
            // read file until end : 
            StreamReader sourceFile = new StreamReader(htmlFile);
            string htmlContent = sourceFile.ReadToEnd();

            // original files : 
            string font_awesome_min_css_DEFAULT = "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css";
            string css_DEFAULT = "https://fonts.googleapis.com/css?family=Open+Sans";
            string style_css_DEFAULT = "/css/style.css?r=1607578509";
            string layout_css_DEFAULT = "/css/layout.css?r=1607578509";
            string app_css_DEFAULT = "https://s3-us-west-2.amazonaws.com/dynomapper-static/3b6af1c902c36a27bf76/app.css";
            string gtm_goal_js_DEFAULT = "<script src=\"https://connect.facebook.net/signals/config/428328831329889?v=2.9.31&amp;r=stable\" async=\"\"></script><script async=\"\" src=\"https://connect.facebook.net/en_US/fbevents.js\"></script><script type=\"text/javascript\" async=\"\" src=\"https://snap.licdn.com/li.lms-analytics/insight.min.js\"></script><script type=\"text/javascript\" async=\"\" src=\"https://www.google-analytics.com/analytics.js\"></script><script async=\"\" src=\"https://www.googletagmanager.com/gtm.js?id=GTM-WMG9K49\"></script><script type=\"text/javascript\" async=\"\" defer=\"\" src=\"https://s3.amazonaws.com/downloads.mailchimp.com/js/goal.min.js\"></script><script type=\"text/javascript\">";
            string app_build_js_DEFAULT = "/js/app-build.js?v=1607578509";
            string vendor_js_DEFAULT = "https://s3-us-west-2.amazonaws.com/dynomapper-static/3b6af1c902c36a27bf76/vendor.js";

            // original files : 
            string font_awesome_min_css_NEW = "/boardChart_cssJSFile/font-awesome.min.css";
            string css_NEW = "./boardChart_cssJSFile/css";
            string style_css_NEW = "./boardChart_cssJSFile/style.css";
            string layout_css_NEW = "./boardChart_cssJSFile/layout.css";
            string app_css_NEW = "./boardChart_cssJSFile/app.css";
            string gtm_goal_js_NEW = "<script async=\"\" src=\"./boardChart_cssJSFile/gtm.js.download\"></script><script type=\"text/javascript\" async=\"\" defer=\"\" src=\"./boardChart_cssJSFile/goal.min.js.download\"></script><script type=\"text/javascript\">";
            string app_build_js_NEW = "./boardChart_cssJSFile/app-build.js.download";
            string vendor_js_NEW = "./boardChart_cssJSFile/vendor.js";

            htmlContent = htmlContent.Replace(font_awesome_min_css_DEFAULT, font_awesome_min_css_NEW);
            htmlContent = htmlContent.Replace(css_DEFAULT, css_NEW);
            htmlContent = htmlContent.Replace(style_css_DEFAULT, style_css_NEW);
            htmlContent = htmlContent.Replace(layout_css_DEFAULT, layout_css_NEW);
            htmlContent = htmlContent.Replace(app_css_DEFAULT, app_css_NEW);
            htmlContent = htmlContent.Replace(gtm_goal_js_DEFAULT, gtm_goal_js_NEW);
            htmlContent = htmlContent.Replace(app_build_js_DEFAULT, app_build_js_NEW);
            htmlContent = htmlContent.Replace(vendor_js_DEFAULT, vendor_js_NEW);

            sourceFile.Close();
            System.Threading.Thread.Sleep(10000);
            System.IO.File.WriteAllText(htmlFile, htmlContent);
        }
    }
}

