namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using Extensions;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class WidgetButtonDropDown : WebElement
    {
        public void SelectWidgetWarningException()
        {

            this.Driver.WaitForJavascript();
            this.GetElement().FindElement(By.Id("widget-warning-dropdown")).Click();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            var dropItem = wait.Until((d) => d.FindElement(By.Id("widget-warning-1")));

            dropItem.Click();
        }
    }
}
