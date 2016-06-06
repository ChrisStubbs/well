namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using OpenQA.Selenium.Support.UI;

    public class TextBox : WebElement
    {
        public void Click()
        {
            this.GetElement().Click();
        }

        public void EnterText(string textToPopulate)
        {
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            wait.Until(d => this.GetElement().Displayed);
            wait.Until(d => this.GetElement().Enabled);

            this.GetElement().SendKeys(string.Empty);

            this.GetElement().SendKeys(textToPopulate);
        }
    }
}