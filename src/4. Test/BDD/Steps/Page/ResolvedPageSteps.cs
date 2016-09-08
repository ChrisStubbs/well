namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;
    using NUnit.Framework;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class ResolvedPageSteps
    {
        private ResolvedDeliveriesPage ResolvedDeliveriesPage => new ResolvedDeliveriesPage();

        [When(@"I open the resolved deliveries page")]
        public void WhenIOpenTheResolvedDeliveries()
        {
            ResolvedDeliveriesPage.Open();
        }

       [When(@"I filter the resolved delivery grid with the option '(.*)' and value '(.*)'")]
        public void WhenIFilterTheResolvedDeliveryGridWithTheOptionAndValue(string option, string value)
        {
            this.ResolvedDeliveriesPage.Filter.Apply(option, value);
        }

        [When(@"I click on the orderby Triangle image in the resolved deliveries grid")]
        public void WhenIClickOnTheOrderbyTriangleImageInTheResolvedDeliveriesGrid()
        {
            this.ResolvedDeliveriesPage.OrderByButton.Click();
        }

        [Then(@"The following resolved deliveries ordered by date will be displayed in '(.*)' order")]
        public void ThenTheFollowingResolvedDeliveriesOrderedByDateWillBeDisplayedInOrder(string direction, Table table)
        {
            var pageRows = this.ResolvedDeliveriesPage.RoutesGrid.ReturnAllRows().ToList();

            pageRows.Reverse(0, pageRows.Count);

            Assert.That(pageRows.Count, Is.EqualTo(table.RowCount));
            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.Route), Is.EqualTo(table.Rows[i]["Route"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.Drop), Is.EqualTo(table.Rows[i]["Drop"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.InvoiceNo), Is.EqualTo(table.Rows[i]["InvoiceNo"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.Account), Is.EqualTo(table.Rows[i]["Account"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.AccountName), Is.EqualTo(table.Rows[i]["AccountName"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.Status), Is.EqualTo(table.Rows[i]["Status"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.Action), Is.EqualTo(table.Rows[i]["Action"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.Assigned), Is.EqualTo(table.Rows[i]["Assigned"]));
            }
        }



        [Then(@"the following resolved deliveries will be displayed")]
        public void ThenTheFollowingResolvedDeliveriesWillBeDisplayed(Table table)
        {
            var pageRows = this.ResolvedDeliveriesPage.RoutesGrid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(table.RowCount));
            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.Route), Is.EqualTo(table.Rows[i]["Route"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.Drop), Is.EqualTo(table.Rows[i]["Drop"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.InvoiceNo), Is.EqualTo(table.Rows[i]["InvoiceNo"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.Account), Is.EqualTo(table.Rows[i]["Account"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.AccountName), Is.EqualTo(table.Rows[i]["AccountName"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.Status), Is.EqualTo(table.Rows[i]["Status"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.Action), Is.EqualTo(table.Rows[i]["Action"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ResolvedDeliveriesGrid.Assigned), Is.EqualTo(table.Rows[i]["Assigned"]));
            }
        }

        [When(@"I click on resolved delivery page (.*)")]
        public void WhenIClickOnDeliveryPage(int pageNo)
        {
            this.ResolvedDeliveriesPage.Pager.Click(pageNo);
        }

        [Then(@"'(.*)' rows of resolved delivery data will be displayed")]
        public void ThenRowsOfDeliveryDataWillBeDisplayed(int noOfRowsExpected)
        {
            var pageRows = this.ResolvedDeliveriesPage.RoutesGrid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(noOfRowsExpected));
        }

        [Then(@"I will have (.*) pages of resolved delivery data")]
        public void CheckNoOfPages(int noOfPages)
        {
            Assert.That(ResolvedDeliveriesPage.Pager.NoOfPages(), Is.EqualTo(noOfPages));
        }

    }
}
