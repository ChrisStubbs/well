﻿

namespace PH.Well.BDD.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Globalization;
    using Framework;
    using Framework.Context;
    using Framework.Extensions;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public abstract class Page
    {
        public IWebDriver Driver
        {
            get
            {
                return DriverContext.CurrentDriver;
            }
        }

        protected abstract string UrlSuffix { get; }

        public void Open()
        {
            var url = UrlContext.CurrentUrl + this.UrlSuffix;

            this.Driver.Manage().Window.Maximize();

            this.Driver.Navigate().GoToUrl(url);

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            wait.Until(d => d.Url.ToString(CultureInfo.InvariantCulture).Contains(url));

            this.Driver.WaitForAjax();
        }
    }
}
