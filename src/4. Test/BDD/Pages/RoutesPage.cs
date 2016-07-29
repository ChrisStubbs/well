﻿namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using NUnit.Framework.Interfaces;
    using OpenQA.Selenium;

    public class RoutesPage : Page
    {
        public RoutesPage()
        {
            this.RoutesGrid = new Grid<RoutesGrid> { Locator = By.Id("tableRoutes"), RowLocator = By.ClassName("grid-row") };
            this.Filter = new FilterControl();
            this.Pager = new PagerControl();
        }

        protected override string UrlSuffix => "Routes";
        public Grid<RoutesGrid> RoutesGrid { get; set; }
        public FilterControl Filter { get; set; }
        public PagerControl Pager { get; set; }
    }

    public enum RoutesGrid
    {
        Route,
        Driver,
        NoOfDrops,
        Exceptions,
        Clean,
        Status,
        LastUpdatedDateTime
    }
}