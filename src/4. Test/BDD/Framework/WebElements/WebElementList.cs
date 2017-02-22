namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Contracts;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using StructureMap;
    using Well.BDD.Framework.Context;
    using Well.BDD.Framework.Extensions;

    public abstract class WebElementList
    {
        protected ILogger Logger;
        

        protected WebElementList()
        {
            var container = FeatureContextWrapper.GetContextObject<IContainer>(ContextDescriptors.StructureMapContainer);
            this.Logger = container.GetInstance<ILogger>();
        }

        public IWebDriver Driver => DriverContext.CurrentDriver;
        
        public virtual By Locator { get; set; }

        public virtual IEnumerable<IWebElement> GetElements()
        {
            try
            {
                this.Driver.WaitForJavascript();

                var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

                return wait.Until(d => d.FindElements(this.Locator));
            }
            catch (NoSuchElementException ex)
            {
                Logger.LogError($"Could Not Find Element {Locator}", ex);
                return null;
            }
            catch (NoSuchWindowException ex)
            {
                Logger.LogError($"Could Not Find Window {Locator}", ex);
                return null;
            }
        }

        private bool DoClick(string value)
        {
            try
            {
                var singleOrDefault = this.GetElements().SingleOrDefault(r => r.GetAttribute("value") == value);
                singleOrDefault?.Click();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual void Click(string value)
        {
            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
            wait.Until(d => DoClick(value));
        }
    }
}