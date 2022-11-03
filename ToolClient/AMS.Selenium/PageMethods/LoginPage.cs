using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace AMS.Selenium.PageMethods
{
    public class LoginPage
    {
        private WebDriverWait wait;
        private string adminName;
        private string adminPassword;

        public LoginPage(IWebDriver driver, string userName, string password)
        {
            this.adminName = userName;
            this.adminPassword = password;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            PageFactory.InitElements(driver, this);

            driver.Navigate().GoToUrl("https://app.dynomapper.com/user/login");
            System.Threading.Thread.Sleep(5000);
            driver.Manage().Window.Maximize();
        }

        [FindsBy(How = How.Id, Using = "login-username")]
        private IWebElement nameElement;

        [FindsBy(How = How.Id, Using = "login-password")]
        private IWebElement passwordElement;

        [FindsBy(How = How.XPath, Using = "//button[@type='submit']")]
        private IWebElement loginElement;

        [FindsBy(How = How.Id, Using = "sidebar-menu")]
        private IWebElement menuDashboard;        

        public void TypeName(string name)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(nameElement));
            nameElement.SendKeys(name);
        }
        public void TypePassword(string password)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(passwordElement));
            passwordElement.SendKeys(password);
        }
        public void ClickLogin()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(loginElement));
            loginElement.Click();
        }
        public void Login(string name, string password)
        {
            TypeName(name);
            TypePassword(password);
            ClickLogin();
        }       
        public void LoginByAdminAccount()
        {
            TypeName(adminName);
            TypePassword(adminPassword);
            ClickLogin();
        }        
        public Boolean VerifyDashboard()
        {
            Boolean res = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(menuDashboard)).Displayed;
            return res;
        }
    }
}
