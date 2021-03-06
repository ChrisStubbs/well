﻿namespace PH.Well.BDD.Pages
{
    using System;

    using Framework;
    using Framework.Context;
    using Framework.Extensions;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public abstract class Page
    {
        public IWebDriver Driver => DriverContext.CurrentDriver;

        protected abstract string UrlSuffix { get; }

        public void Open(string routing)
        {
            var url = UrlContext.CurrentUrl + this.UrlSuffix + routing;

            this.Driver.Manage().Window.Maximize();

            this.Driver.Navigate().GoToUrl(url);

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            wait.Until(d => d.Url.ToLowerInvariant().Contains(url.ToLowerInvariant()));
            
            this.Driver.WaitForJavascript();
        }

        public void OpenAbsolute(string routing)
        {
            var url = UrlContext.CurrentUrl + routing;

            this.Driver.Manage().Window.Maximize();

            this.Driver.Navigate().GoToUrl(url);

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            wait.Until(d => d.Url.ToLowerInvariant().Contains(url.ToLowerInvariant()));

            this.Driver.WaitForJavascript();
        }

        public void Open()
        {
            Open(string.Empty);
        }

        public void Back()
        {
            this.Driver.Navigate().Back();
        }
    }
}