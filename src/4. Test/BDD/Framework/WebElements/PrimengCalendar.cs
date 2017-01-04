using OpenQA.Selenium.Support.UI;
using System;
using OpenQA.Selenium;

namespace PH.Well.BDD.Framework.WebElements
{
    public class PrimengCalendar : WebElement
    {
        private readonly string id;
        public PrimengCalendar(string cssSelector)
        {
            if (string.IsNullOrWhiteSpace(cssSelector))
            {
                throw new ArgumentNullException(cssSelector);
            }
            base.Locator = By.CssSelector(string.Format($@"{cssSelector}  span input"));
        }

        public override By Locator
        {
            get
            {
                return base.Locator;
            }
            set
            {
                throw new NotSupportedException("Locator can only be set via parameter constructor");
            }
        }

        public DateTime Date
        { set
            {
                var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

                wait.Until(d => this.GetElement().Displayed);
                wait.Until(d => this.GetElement().Enabled);

                this.GetElement().Clear();

                this.GetElement().SendKeys(value.ToString("dd/MM/yyyy"));
            }
        }
    }
}
