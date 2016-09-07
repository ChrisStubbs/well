﻿namespace PH.Well.BDD.Pages
{
    using System;
    using System.Linq;

    using Framework.WebElements;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    using PH.Well.BDD.Framework;
    using PH.Well.BDD.Framework.Extensions;

    public class ExceptionDeliveriesPage : Page
    {
        public ExceptionDeliveriesPage()
        {
            this.ExceptionsGrid = new Grid<ExceptionDeliveriesGrid> { Locator = By.Id("tableExceptionDeliveries"), RowLocator = By.ClassName("grid-row") };
            this.ExceptionsDrillDownGrid = new Grid<ExceptionDrilldownGrid> { Locator = By.Id("tableDelivery"), RowLocator = By.ClassName("grid-row") };
            this.Filter = new FilterControl();
            this.Pager = new PagerControl();
            this.EnabledButton = new Button { Locator = By.ClassName("enabled-action") };
        }

        protected override string UrlSuffix => "exceptions";

        public Grid<ExceptionDeliveriesGrid> ExceptionsGrid { get; set; }

        public Grid<ExceptionDrilldownGrid> ExceptionsDrillDownGrid { get; set; }

        public FilterControl Filter { get; set; }

        public PagerControl Pager { get; set; }

        public Button EnabledButton { get; set; }

        public IWebElement GetFirstCell()
        {
            this.Driver.WaitForAjax();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            var elements = wait.Until(d => d.FindElements(By.ClassName("first-cell")));

            return elements.First();
        }

        public IWebElement GetFirstAssignUserFromModal()
        {
            this.Driver.WaitForAjax();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            var elements = wait.Until(d => d.FindElements(By.ClassName("assign-user")));

            return elements.First();
        }

        public int GetCountOfElements(string className)
        {
            this.Driver.WaitForAjax();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            var elements = wait.Until(d => d.FindElements(By.ClassName(className)));

            return elements.Count();
        }
    }

    public enum ExceptionDeliveriesGrid
    {
        Route = 0,
        Drop = 1,
        InvoiceNo = 2,
        Account = 3,
        AccountName= 4,
        Contact= 5,
        InfoButton = 6,
        Status = 7,
        Reason = 8,
        Assigned = 9,
        Action = 10,
        LastUpdatedDateTime = 11
    }

    public enum ExceptionDrilldownGrid
    {
        LineNumber = 0,
        Product = 1,
        Description = 2,
        Value = 3,
        InvoiceQuantity = 4,
        DeliveredQuantity = 5,
        DamagedQuantity = 6,
        ShortQuantity = 7,
        Update = 8
    }
}
