namespace PH.Well.BDD.Steps.Page
{
    using Framework.Extensions;
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
            for (int i = 0; i < table.RowCount; i++)
            {
                var row = this.ResolvedDeliveriesPage.GetGridRow(int.Parse(table.Rows[i]["Id"]));

                Assert.That(row.Route.Text, Is.EqualTo(table.Rows[i]["Route"]));
                Assert.That(row.Drop.Text, Is.EqualTo(table.Rows[i]["Drop"]));
                Assert.That(row.Invoice.Text, Is.EqualTo(table.Rows[i]["InvoiceNo"]));
                Assert.That(row.Code.Text, Is.EqualTo(table.Rows[i]["Account"]));
                Assert.That(row.Name.Text, Is.EqualTo(table.Rows[i]["AccountName"]));
                Assert.That(row.Job.Text, Is.EqualTo(table.Rows[i]["Status"]));
                Assert.That(row.Assigned.Text, Is.EqualTo(table.Rows[i]["Assigned"]));
            }
        }
        
        [Then(@"the following resolved deliveries will be displayed")]
        public void ThenTheFollowingResolvedDeliveriesWillBeDisplayed(Table table)
        {
            for (int i = 0; i < table.RowCount; i++)
            {
                var row = this.ResolvedDeliveriesPage.GetGridRow(int.Parse(table.Rows[i]["Id"]));

                Assert.That(row.Route.Text, Is.EqualTo(table.Rows[i]["Route"]));
                Assert.That(row.Drop.Text, Is.EqualTo(table.Rows[i]["Drop"]));
                Assert.That(row.Invoice.Text, Is.EqualTo(table.Rows[i]["InvoiceNo"]));
                Assert.That(row.Code.Text, Is.EqualTo(table.Rows[i]["Account"]));
                Assert.That(row.Name.Text, Is.EqualTo(table.Rows[i]["AccountName"]));
                Assert.That(row.Job.Text, Is.EqualTo(table.Rows[i]["Status"]));
                Assert.That(row.Assigned.Text, Is.EqualTo(table.Rows[i]["Assigned"]));
            }
        }

        [Then(@"the following resolved deliveries grid will be displayed")]
        public void ThenTheFollowingResolvedDeliveriesGridWillBeDisplayed(Table table)
        {
            var result = this.ResolvedDeliveriesPage.DeliveriesGrid.ContainsSpecFlowTable(table);
            Assert.That(result.HasError, Is.False, result.ErrorsDesc);
        }


        [When(@"I view the account info modal for resolved row (.*)")]
        public void WhenIViewTheAccountInfoModalForResolvedRow(int row)
        {
            ResolvedDeliveriesPage.GetGridRow(row).Contact.Click();
        }


        [Then(@"I can the following account info details - resolved")]
        public void ThenICanTheFollowingAccountInfoDetails(Table table)
        {
            var modal = ResolvedDeliveriesPage.AccountModal;
            AccountModalSteps.CompareModal(table, modal);
        }

        [When(@"I click on resolved delivery page (.*)")]
        public void WhenIClickOnDeliveryPage(int pageNo)
        {
            this.ResolvedDeliveriesPage.Pager.Click(pageNo);
        }

        [Then(@"'(.*)' rows of resolved delivery data will be displayed")]
        public void ThenRowsOfDeliveryDataWillBeDisplayed(int noOfRowsExpected)
        {
            /*var pageRows = this.ResolvedDeliveriesPage.RoutesGrid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(noOfRowsExpected));*/
        }

        [Then(@"I will have (.*) pages of resolved delivery data")]
        public void CheckNoOfPages(int noOfPages)
        {
            Assert.That(ResolvedDeliveriesPage.Pager.NoOfPages(), Is.EqualTo(noOfPages));
        }

    }
}
