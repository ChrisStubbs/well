namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using Framework;
    using OpenQA.Selenium.Support.UI;

    public class CheckBox : WebElement
    {
        public void Check()
        {
            this.GetElement();
            this.Element.Click();
        }

        public void UnCheck()
        {
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
            wait.Until(d => this.GetElement().Displayed);

            if (this.Element.Selected)
            {
                this.Check();
            }
        }
    }
}