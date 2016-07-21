namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using Common.Contracts;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using StructureMap;
    using Well.BDD.Framework.Context;
    using Well.BDD.Framework.Extensions;

    public abstract class WebElement
    {
        protected WebElement()
        {
            this.container = FeatureContextWrapper.GetContextObject<IContainer>(ContextDescriptors.StructureMapContainer);
            this.logger = this.container.GetInstance<ILogger>();
        }

        public IWebDriver Driver => DriverContext.CurrentDriver;

        private readonly IContainer container;

        protected ILogger logger;

        public string Id { get; set; }

        public IWebElement Element { get; set; }

        public By Locator { get; set; }

        public bool WaitForAjax { get; set; }

        public virtual IWebElement GetElement()
        {
            try
            {
                this.Driver.WaitForAjax();

                var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

                this.Element = wait.Until(d => d.FindElement(this.Locator));

                this.Driver.ExecuteJavaScript($"window.scrollTo(0, {this.Element.Location.Y});");

                logger.LogDebug($"Found Element Locator: {Locator} Text: {Element.Text} Enable: {Element.Enabled} Displayed: {Element.Displayed}"  );
                return this.Element;
            }
            catch (NoSuchElementException ex)
            {
                logger.LogError($"Could Not Find Element {Locator}", ex);
                return null;
            }
            catch (NoSuchWindowException ex)
            {
                logger.LogError($"Could Not Find Window {Locator}", ex);
                return null;
            }
        }

        private bool DoClick()
        {
            try
            {
                this.GetElement().Click();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual void Click()
        {
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
            wait.Until(d => DoClick());
        }
    }
}