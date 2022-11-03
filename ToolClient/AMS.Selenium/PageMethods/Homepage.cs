using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace AMS.Selenium.PageMethods
{
    public class Homepage
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        public Homepage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            PageFactory.InitElements(driver, this);
            //wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }

        String leftMenuElement = "//div[@id='sidebar-menu']/ul/li/a[text()[contains(.,'menuItem')]]";

        public void ClickOnLeftMenuItem(String menuName)
        {
            leftMenuElement = leftMenuElement.Replace("menuItem", menuName);
            driver.FindElement(By.XPath(leftMenuElement)).Click();
            leftMenuElement = leftMenuElement.Replace(menuName, "menuItem");
        }
    }
}
