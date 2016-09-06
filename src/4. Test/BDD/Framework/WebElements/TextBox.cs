namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using Framework;
    using OpenQA.Selenium.Support.UI;

    public class TextBox : WebElement
    {
        public void EnterText(string textToPopulate)
        {
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            wait.Until(d => this.GetElement().Displayed);
            wait.Until(d => this.GetElement().Enabled);

            this.GetElement().Clear();

            this.GetElement().SendKeys(textToPopulate);
        }
    }
}