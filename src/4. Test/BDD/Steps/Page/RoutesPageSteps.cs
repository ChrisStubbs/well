﻿namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;
    using System.Threading;
    using Domain;
    using Framework.Extensions;
    using NUnit.Framework;
    using Pages;

    using PH.Well.BDD.Framework.Context;

    using TechTalk.SpecFlow;

    [Binding]
    public class RoutesPageSteps
    {
        private RoutesPage routesPage => new RoutesPage();
        private ExceptionDeliveriesPage exceptionPage => new ExceptionDeliveriesPage();
        private CleanDeliveriesPage cleanPage => new CleanDeliveriesPage();
        private FilterControl filter => new FilterControl();

        [When(@"I open the routes page")]
        public void OpenTheRoutesPage()
        {
            this.routesPage.Open();
        }

        [When(@"I filter the grid with the option '(.*)' and value '(.*)'")]
        public void WhenIFilterTheGridWithTheOptionAndValue(string option, string value)
        {
            this.routesPage.Filter.Apply(option, value);
        }

        [When(@"I select '(.*)' from the filter options")]
        public void WhenISelectRouteFromTheFilterOptions(string option)
        {
            this.routesPage.Filter.OptionDropDown.Select(option);
        }


        [When(@"I clear the filter")]
        public void WhenIClearTheFilter()
        {
            this.routesPage.Filter.Clear();
        }

        [When(@"I click on page '(.*)'")]
        public void WhenIClickOnPage(int pageNo)
        {
            this.routesPage.Pager.Click(pageNo);
        }

        [When(@"I click on the orderby Triangle image")]
        public void WhenIClickOnTheOrderByArrowImage()
        {
           this.routesPage.OrderByButton.Click();
        }


        [Then(@"The following routes will be displayed")]
        public void ThenTheFollowingRoutesWillBeDisplayed(Table table)
        {
            var result = this.routesPage.RoutesGrid.ContainsSpecFlowTable(table);
            Assert.That(result.HasError, Is.False, result.ErrorsDesc);
        }

        [Then(@"The exceptions count matches the exceptions page for that route")]
        public void TheExceptionsCountMatchesTheExceptionsForThatRoute()
        {
            var initialPageRows = this.routesPage.RoutesGrid.ReturnAllRows().ToList();
            var totalRowCount = initialPageRows.Count;

            for (int i = 0; i < totalRowCount; i++)
            {
                var rows = this.routesPage.RoutesGrid.ReturnAllRows().ToList();

                var expectedExceptionsCount = rows[i].GetColumnValueByIndex((int)RoutesGrid.Exceptions);

                if (int.Parse(expectedExceptionsCount) == 0)
                {
                    continue;
                }

                rows[i].Click();
                Thread.Sleep(500);
                this.routesPage.ExceptionButton.Click();
                Thread.Sleep(500);
                var exceptionsRows = this.exceptionPage.ExceptionsGrid.ReturnAllRows().ToList();

                Assert.That(exceptionsRows.Count, Is.EqualTo(int.Parse(expectedExceptionsCount)));

                this.routesPage.Back();
                Thread.Sleep(500);
            }
        }

        [Then(@"The clean count matches the clean page for that route")]
        public void TheCleanCountMatchesTheCleanForThatRoute()
        {
            var initialPageRows = this.routesPage.RoutesGrid.ReturnAllRows().ToList();
            var totalRowCount = initialPageRows.Count;

            for (int i = 0; i < totalRowCount; i++)
            {
                var rows = this.routesPage.RoutesGrid.ReturnAllRows().ToList();

                var expectedCount = rows[i].GetColumnValueByIndex((int)RoutesGrid.Clean);

                if (int.Parse(expectedCount) == 0)
                {
                    continue;
                }

                rows[i].Click();
                Thread.Sleep(500);
                this.routesPage.CleanButton.Click();
                Thread.Sleep(500);
                var cleanRows = this.cleanPage.Grid.ReturnAllRows().ToList();

                Assert.That(cleanRows.Count, Is.EqualTo(int.Parse(expectedCount)));

                this.routesPage.Back();
                Thread.Sleep(500);
            }
        }

        [Then(@"The following routes ordered by date will be displayed in '(.*)' order")]
        public void ThenTheFollowingRoutesOrderedByDateWillBeDisplayedInOrder(string direction, Table table)
        {
            var pageRows = this.routesPage.RoutesGrid.ReturnAllRows().ToList();

            Assert.That(pageRows.Count, Is.EqualTo(table.RowCount));
            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(pageRows[i].GetColumnValueByIndex((int) RoutesGrid.Route),
                    Is.EqualTo(table.Rows[i]["Route"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int) RoutesGrid.Driver),
                    Is.EqualTo(table.Rows[i]["Driver"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int) RoutesGrid.NoOfDrops),
                    Is.EqualTo(table.Rows[i]["Drops"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int) RoutesGrid.Exceptions),
                    Is.EqualTo(table.Rows[i]["Exceptions"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int) RoutesGrid.Clean),
                    Is.EqualTo(table.Rows[i]["Clean"]));

            }
        }



        [Then(@"'(.*)' rows of data will be displayed")]
        public void CheckNoOfRows(int noOfRowsExpected)
        {
            var pageRows = this.routesPage.RoutesGrid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(noOfRowsExpected));
        }

        [Then(@"I will have (.*) pages of data")]
        public void CheckNoOfPages(int noOfPages)
        {
            Assert.That(this.routesPage.Pager.NoOfPages(), Is.EqualTo(noOfPages));
        }

        [When(@"I select the first row of the route")]
        public void SelectFirstRowOfTheRoutesGrid()
        {
            var rows = this.routesPage.RoutesGrid.ReturnAllRows().ToList();

            var route = rows[0].GetColumnValueByIndex((int)RoutesGrid.Route);

            ScenarioContextWrapper.SetContextObject(ContextDescriptors.RouteNumber, route);

            rows[0].Click();
        }

        [When(@"I choose to view that routes exceptions")]
        public void SelectExceptionButton()
        {
            Thread.Sleep(1000);
            this.routesPage.ExceptionButton.Click();
        }

        [When(@"I choose to view that routes clean deliveries")]
        public void SelectCleanButton()
        {
            Thread.Sleep(1000);
            this.routesPage.CleanButton.Click();
        }

        [Then(@"I can see that routes exceptions")]
        public void SeeRoutesExceptionsOnly()
        {
            var rows = this.exceptionPage.ExceptionsGrid.ReturnAllRows().ToList();

            Assert.That(rows.Count, Is.GreaterThan(0));

            var routeNumberShouldBe = ScenarioContextWrapper.GetContextObject<string>(
                    ContextDescriptors.RouteNumber);

            foreach (var row in rows)
            {
                var routeNumber = row.GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Route);

                Assert.That(routeNumber, Is.EqualTo(routeNumberShouldBe));
            }
        }

        [Then(@"I can see that routes clean deliveries")]
        public void SeeRoutesCleanDeliveriesOnly()
        {
            var rows = this.cleanPage.Grid.ReturnAllRows().ToList();

            Assert.That(rows.Count, Is.GreaterThan(1));

            var routeNumberShouldBe = ScenarioContextWrapper.GetContextObject<string>(
                    ContextDescriptors.RouteNumber);

            foreach (var row in rows)
            {
                var routeNumber = row.GetColumnValueByIndex((int)CleanDeliveriesGrid.Route);

                Assert.That(routeNumber, Is.EqualTo(routeNumberShouldBe));
            }
        }

        [Then(@"the clean deliveries page will be opened")]
        public void ThenTheCleanDeliveriesPageWillBeOpened()
        {
            Assert.That(this.routesPage.IsElementPresent("CleanPage"));
        }


        [Then(@"the filter should be preset to route and route number")]
        public void FilterShouldBePresetToRouteAndRouteNumber()
        {
            var routeNumberShouldBe = ScenarioContextWrapper.GetContextObject<string>(
                    ContextDescriptors.RouteNumber);

            var filterText = this.filter.GetFilterText();

            Assert.That(filterText, Is.EqualTo(routeNumberShouldBe));

            var option = this.filter.GetSelectedOptionText();

            Assert.That(option, Is.EqualTo("Route"));
        }

        [Then(@"the routes page will be displayed")]
        public void ThenTheRoutesPageWillBeDisplayed()
        {
            Assert.That(this.routesPage.IsElementPresent("filter-option"));
        }

        [Then(@"page '(.*)' will be displayed")]
        public void ThenPageWillBeDisplayed(int pageNo)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I click the back button")]
        public void WhenIClickTheBackButton()
        {
            this.routesPage.Back();
            Thread.Sleep(1000);

        }

        [Then(@"the the previous filter should be cleared")]
        public void ThenTheThePreviousFilterShouldBeCleared()
        {
            Assert.That(this.routesPage.Filter.GetFilterText(), Is.EqualTo(null));
        }


    }
}
