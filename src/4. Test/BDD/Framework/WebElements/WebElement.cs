namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using Context;
    using Extensions;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public abstract class WebElement
    {
        public IWebDriver Driver {
            get
            {
                return DriverContext.CurrentDriver;
            }
        }

        public string Id { get; set; }

        public IWebElement Element { get; set; }

        public By Locator { get; set; }

        public bool WaitForAjax { get; set; }

        public virtual IWebElement GetElement(int seconds = 10)
        {
            try
            {
                this.Driver.WaitForAjax();

                var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(seconds));

                this.Element = wait.Until(d => d.FindElement(this.Locator));

                this.Driver.ExecuteJavaScript(string.Format("window.scrollTo(0, {0});", this.Element.Location.Y));

                return this.Element;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (NoSuchWindowException)
            {
                return null;
            }
        }
    }
}