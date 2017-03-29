namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;
    using System.Threading;
    using Framework.Context;
    using Framework.Extensions;
    using Framework.WebElements;
    using NUnit.Framework;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class CleanPageSteps
    {
        private CleanDeliveriesPage Page => new CleanDeliveriesPage();
        private DeliveryDetailsPage DeliveryDetailsPage => new DeliveryDetailsPage();

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

        [Given(@"I assign the POD delivery to myself")]
        public void AssignPODToMe()
        {
            var podRow = GetPodRow();
            SelectAssignLink(podRow);
        }

        [Given(@"I click on the first POD delivery")]
        public void GivenIClickOnTheFirstPODDelivery()
        {
            var podRow = GetPodRow();
            podRow?.GetItemInRowById("isPod").Click();
        }

        public void SelectAssignLink(GridRow<CleanDeliveriesGrid> row)
        {
            var assignAnchor = row.GetItemInRowByClass("assign");
            assignAnchor.Click();

            Thread.Sleep(1000);
            var element = this.Page.GetLoggedInAssignUserFromModal();
            ScenarioContextWrapper.SetContextObject(ContextDescriptors.AssignName, element.Text);

            element.Click();
        }

        [Given(@"I assign the clean delivery to myself")]
        public void AssignToMe()
        {
            var rows = this.Page.Grid.ReturnAllRows().ToList();
            SelectAssignLink(rows[0]);
        }

        [Then(@"the following clean deliveries will be displayed")]
        public void ThenTheFollowingCleanDeliveriesWillBeDisplayed(Table table)
        {
            var result = this.Page.Grid.ContainsSpecFlowTable(table);
            Assert.That(result.HasError, Is.False, result.ErrorsDesc);
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

        [Then(@"the first clean delivery line is COD \(Cash on Delivery\)")]
        public void ThenTheFirstDeliveryLineIsCODCashOnDelivery()
        {
            var pageRow = this.Page.Grid.ReturnAllRows().First();
            Assert.IsNotNull(pageRow.GetItemInRowById("isCod"));
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

        private GridRow<CleanDeliveriesGrid> GetPodRow()
        {
            var rows = Page.Grid.ReturnAllRows();
            return rows.FirstOrDefault(r => r.GetItemInRowById("isPod") != null);
        }

        [When(@"I click on each of the clean deliveries on each page there will be no exception delivery lines")]
        public void WhenIClickOnEachOfTheCleanDeliveriesOnPageThereWillBeNoExceptionDeliveryLines()
        {
            var noOfPages = this.Page.Pager.NoOfPages();
            for (int pageNo = 1; pageNo <= noOfPages; pageNo++)
            {

                this.Page.Pager.Click(pageNo);

                var initialPageRows = this.Page.Grid.ReturnAllRows().ToList();
                var totalRowCount = initialPageRows.Count;

                for (int i = 0; i < totalRowCount; i++)
                {
                    var rows = this.Page.Grid.ReturnAllRows().ToList();
                    var row = rows[i];
                    row.GetItemInRowByClass("first-cell").Click();
                    this.DeliveryDetailsPage.ClickExceptionsTab();
                       
                    Assert.IsTrue(this.DeliveryDetailsPage.NoExceptions.GetElement().Text.Contains("No exceptions"));
                    this.DeliveryDetailsPage.Back();
                }

            }

        }

    }
}
