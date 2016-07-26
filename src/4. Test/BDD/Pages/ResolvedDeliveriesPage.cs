﻿namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class ResolvedDeliveriesPage : Page
    {
        public ResolvedDeliveriesPage()
        {
            this.RoutesGrid = new Grid<CleanDeliveriesGrid> { Locator = By.Id("tableResolvedDeliveries"), RowLocator = By.ClassName("grid-row") };
            this.Filter = new FilterControl();
            this.Pager = new PagerControl();
        }
        protected override string UrlSuffix => "Resolved";

        public Grid<CleanDeliveriesGrid> RoutesGrid { get; set; }
        public FilterControl Filter { get; set; }
        public PagerControl Pager { get; set; }
    }

    public enum ResolvedDeliveriesGrid
    {
        Route = 0,
        Drop = 1,
        InvoiceNo = 2,
        Account = 3,
        AccountName= 4,
        Contact= 5,
        InfoButton = 6,
        Status = 7,
        Action = 8,
        Assigned = 9,
        LastUpdatedDateTime = 10
    }
}
