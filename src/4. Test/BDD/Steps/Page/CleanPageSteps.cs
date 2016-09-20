namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;
    using NUnit.Framework;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class CleanPageSteps
    {
        private CleanDeliveriesPage CleanDeliveriesPage => new CleanDeliveriesPage();

        [When(@"I open the clean deliveries")]
        public void WhenIOpenTheCleanDeliveries()
        {
            CleanDeliveriesPage.Open();
        }

        [When(@"I filter the clean delivery grid with the option '(.*)' and value '(.*)'")]
        public void WhenIFilterTheCleanDeliveryGridWithTheOptionAndValue(string option, string value)
        {
            this.CleanDeliveriesPage.Filter.Apply(option, value);
        }


        [Then(@"the following clean deliveries will be displayed")]
        public void ThenTheFollowingCleanDeliveriesWillBeDisplayed(Table table)
        {
            var pageRows = this.CleanDeliveriesPage.RoutesGrid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(table.RowCount));
            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.Route), Is.EqualTo(table.Rows[i]["Route"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.Drop), Is.EqualTo(table.Rows[i]["Drop"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.InvoiceNo), Is.EqualTo(table.Rows[i]["InvoiceNo"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.Account), Is.EqualTo(table.Rows[i]["Account"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.AccountName), Is.EqualTo(table.Rows[i]["AccountName"]));
               // Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.Status), Is.EqualTo(table.Rows[i]["Status"]));
            }
        }

        [When(@"I click on clean delivery page (.*)")]
        public void WhenIClickOnCleanDeliveryPage(int pageNo)
        {
            this.CleanDeliveriesPage.Pager.Click(pageNo);
        }


        [Then(@"'(.*)' rows of clean delivery data will be displayed")]
        public void ThenRowsOfCleanDeliveryDataWillBeDisplayed(int noOfRowsExpected)
        {
            var pageRows = this.CleanDeliveriesPage.RoutesGrid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(noOfRowsExpected));
        }

        [Then(@"I will have (.*) pages of clean delivery data")]
        public void CheckNoOfPages(int noOfPages)
        {
            Assert.That(CleanDeliveriesPage.Pager.NoOfPages(), Is.EqualTo(noOfPages));
        }

        [When(@"I view the account info modal for clean row (.*)")]
        public void WhenIViewTheAccountInfoModalForResolvedRow(int row)
        {
            var rows = CleanDeliveriesPage.RoutesGrid.ReturnAllRows().ToList();
            rows[row - 1].GetItemInRowByClass("contact-info").Click();
        }


        [Then(@"I can the following account info details - clean")]
        public void ThenICanTheFollowingAccountInfoDetails(Table table)
        {
            var modal = CleanDeliveriesPage.AccountModal;
            AccountModalSteps.CompareModal(table, modal);
        }

    }
}
