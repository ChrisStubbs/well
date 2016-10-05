namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Well.BDD.Framework;
    using Well.BDD.Framework.Extensions;

    public class CreditThresholdButtonDropDown : WebElement
    {
        public void SelectLevel1()
        {
            this.Driver.WaitForJavascript();

            var button = this.Driver.FindElement(By.Id("credit-threshold-dropdown"));

            button.Click();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
            
            var dropItem = wait.Until((d) => d.FindElement(By.Id("credit-threshold-level1")));

            dropItem.Click();
        }
    }
}