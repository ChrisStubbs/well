namespace PH.Well.BDD.Pages
{
    using System;
    using System.Collections.Generic;
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
            this.RoutesGrid = new Grid<ExceptionDeliveriesGrid> { Locator = By.Id("tableExceptionDeliveries"), RowLocator = By.ClassName("grid-row") };
            this.Filter = new FilterControl();
            this.Pager = new PagerControl();
        }
        protected override string UrlSuffix => "exceptions";

        public Grid<ExceptionDeliveriesGrid> RoutesGrid { get; set; }
        public FilterControl Filter { get; set; }
        public PagerControl Pager { get; set; }

        public IWebElement GetFirstCell()
        {
            this.Driver.WaitForAjax();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            var elements = wait.Until(d => d.FindElements(By.ClassName("first-cell")));

            return elements.First();
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
}
