namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Well.BDD.Framework;
    using Well.BDD.Framework.Extensions;

    public class UserPreferencesButtonDropDown : WebElement
    {
        public void Select()
        {
            this.Driver.WaitForJavascript();
            this.GetElement().FindElement(By.Id("admin-dropdown-anchor")).Click();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
            
            var dropItem = wait.Until((d) => d.FindElement(By.Id("user-preference-selection")));

            dropItem.Click();
        }
    }
}