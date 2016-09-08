namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Well.BDD.Framework;
    using Well.BDD.Framework.Extensions;

    public class AdminButtonDropDown : WebElement
    {
        public void Select()
        {
            this.Driver.WaitForAngular2();
            this.GetElement();
            this.Element.FindElement(By.Id("admin-dropdown-anchor")).Click();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
            
            var dropItem = wait.Until((d) => d.FindElement(By.Id("branch-selection")));

            dropItem.Click();
        }
    }
}