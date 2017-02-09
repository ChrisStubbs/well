namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;
    using System.Threading;
    using Framework.Context;
    using Framework.Extensions;
    using NUnit.Framework;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class CleanPageSteps
    {
        private CleanDeliveriesPage Page => new CleanDeliveriesPage();

        [Given(@"I open the clean deliveries")]
        [When(@"I open the clean deliveries")]
        public void WhenIOpenTheCleanDeliveries()
        {
            Page.Open();
        }

        [When(@"I filter the clean delivery grid with the option '(.*)' and value '(.*)'")]
        public void WhenIFilterTheCleanDeliveryGridWithTheOptionAndValue(string option, string value)
        {
            this.Page.Filter.Apply(option, value);
        }

        [When(@"I click on the orderby Triangle image in the clean deliveries grid")]
        public void WhenIClickOnTheOrderbyTriangleImageInTheCleanDeliveries()
        {
            this.Page.OrderByButton.Click();
        }

        public void SelectAssignLink()
        {
            var rows = this.Page.Grid.ReturnAllRows().ToList();
            var assignAnchor = rows[0].GetItemInRowByClass("assign");
            assignAnchor.Click();
        }

        [Given(@"I assign the clean delivery to myself")]
        public void AssignToMe()
        {
            SelectAssignLink();

            Thread.Sleep(1000);
            var element = this.Page.GetLoggedInAssignUserFromModal();
            ScenarioContextWrapper.SetContextObject(ContextDescriptors.AssignName, element.Text);

            element.Click();
        }


        [Then(@"The following clean deliveries ordered by date will be displayed in '(.*)' order")]
        public void ThenTheFollowingCleanDeliveriesOrderedByDateWillBeDisplayedInOrder(string direction, Table table)
        {
             var pageRows = this.Page.Grid.ReturnAllRows().ToList();

            pageRows.Reverse(0, pageRows.Count);

            Assert.That(pageRows.Count, Is.EqualTo(table.RowCount));
            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.Route), Is.EqualTo(table.Rows[i]["Route"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.Drop), Is.EqualTo(table.Rows[i]["Drop"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.InvoiceNo), Is.EqualTo(table.Rows[i]["InvoiceNo"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.Account), Is.EqualTo(table.Rows[i]["Account"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.AccountName), Is.EqualTo(table.Rows[i]["AccountName"]));
            }
        }


        [Then(@"the following clean deliveries will be displayed")]
        public void ThenTheFollowingCleanDeliveriesWillBeDisplayed(Table table)
        {
            var result = this.Page.Grid.ContainsSpecFlowTable(table);
            Assert.That(result.HasError, Is.False);
        }

        [Then(@"the following clean with cash on delivery deliveries will be displayed")]
        public void ThenTheFollowingCleanWithCashOnDeliveryDeliveriesWillBeDisplayed(Table table)
        {
            var pageRows = this.Page.Grid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(table.RowCount));
            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.Route), Is.EqualTo(table.Rows[i]["Route"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.Drop), Is.EqualTo(table.Rows[i]["Drop"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.InvoiceNo), Is.EqualTo(table.Rows[i]["InvoiceNo"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.Account), Is.EqualTo(table.Rows[i]["Account"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.AccountName), Is.EqualTo(table.Rows[i]["AccountName"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.CashOnDelivery), Is.EqualTo(table.Rows[i]["Status"]));
            }
        }

        [Then(@"the cod delivery icon is not displayed in row (.*)")]
        public void ThenTheCodDeliveryIconIsNotDisplayedInRow(int firstRow)
        {
            var row = firstRow - 1;
            var pageRows = this.Page.Grid.ReturnAllRows().ToList();
            var cashOnDeliveryIcon = pageRows[row].GetColumnValueByIndex(6);
            Assert.That(cashOnDeliveryIcon, Is.Empty);
        }

        [When(@"I click on clean delivery page (.*)")]
        public void WhenIClickOnCleanDeliveryPage(int pageNo)
        {
            this.Page.Pager.Click(pageNo);
        }


        [Then(@"'(.*)' rows of clean delivery data will be displayed")]
        public void ThenRowsOfCleanDeliveryDataWillBeDisplayed(int noOfRowsExpected)
        {
            var pageRows = this.Page.Grid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(noOfRowsExpected));
        }

        [Then(@"At least '(.*)' rows of clean delivery data will be displayed")]
        public void ThenAtLeastRowsOfCleanDeliveryDataWillBeDisplayed(int noOfRowsExpected)
        {
            var pageRows = this.Page.Grid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.GreaterThanOrEqualTo(noOfRowsExpected));
        }

        [Then(@"No clean deliveries will be displayed")]
        public void ThenNoCleanDeliveriesWillBeDisplayed()
        {
            Assert.That(Page.IsElementPresent("noDeliveries"));
        }


        [Then(@"I will have (.*) pages of clean delivery data")]
        public void CheckNoOfPages(int noOfPages)
        {
            Assert.That(Page.Pager.NoOfPages(), Is.EqualTo(noOfPages));
        }

        [When(@"I view the account info modal for clean row (.*)")]
        public void WhenIViewTheAccountInfoModalForResolvedRow(int row)
        {
            var rows = Page.Grid.ReturnAllRows().ToList();
            rows[row - 1].GetItemInRowByClass("contact-info").Click();
        }


        [Then(@"I can the following account info details - clean")]
        public void ThenICanTheFollowingAccountInfoDetails(Table table)
        {
            var modal = Page.AccountModal;
            AccountModalSteps.CompareModal(table, modal);
        }

    }
}
