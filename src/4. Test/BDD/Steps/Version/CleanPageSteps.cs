namespace PH.Well.BDD.Steps.Version
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
                Assert.That(pageRows[i].GetColumnValueByIndex((int)CleanDeliveriesGrid.Status), Is.EqualTo(table.Rows[i]["Status"]));
            }
        }
    }
}
