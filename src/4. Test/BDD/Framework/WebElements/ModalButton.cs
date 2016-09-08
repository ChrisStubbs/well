namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using Well.BDD.Framework;

    public class ModalButton : WebElement
    {
        public override void Click()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
            wait.Until(d => d.FindElement(By.Id("confirmationModal")).GetCssValue("opacity").Equals("1"));
            base.Click();
        }
    }
}
