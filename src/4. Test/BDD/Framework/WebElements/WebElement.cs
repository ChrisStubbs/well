namespace PH.Well.BDD.Framework.WebElements
{
    using System;
    using System.Collections.Generic;
    using Common.Contracts;
    using Microsoft.SqlServer.Management.Sdk.Sfc;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using StructureMap;
    using Well.BDD.Framework.Context;
    using Well.BDD.Framework.Extensions;

    public abstract class WebElement
    {
        protected ILogger Logger;
        

        protected WebElement()
        {
            var container = FeatureContextWrapper.GetContextObject<IContainer>(ContextDescriptors.StructureMapContainer);
            this.Logger = container.GetInstance<ILogger>();
        }

        public IWebDriver Driver => DriverContext.CurrentDriver;
        
        public virtual By Locator { get; set; }

        public virtual IWebElement GetElement()
        {
            try
            {
                this.Driver.WaitForJavascript();

                var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                var element = wait.Until(d => d.FindElement(this.Locator));

                this.Driver.ExecuteJavaScript($"window.scrollTo(0, {element.Location.Y});");

                Logger.LogDebug($"Found Element Locator: {Locator} Text: {element.Text} Enable: {element.Enabled} Displayed: {element.Displayed}"  );
               return element;
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