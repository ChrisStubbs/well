namespace PH.Well.BDD.Steps.Page
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Framework.WebElements;
    using NUnit.Framework;

    using PH.Well.BDD.Pages;

    using TechTalk.SpecFlow;

    [Binding]
    public class AuditSteps
    {
        private AuditPage page => new AuditPage();
    
        [When(@"I open the audits page")]
        public void WhenIOpenTheAuditsPage()
        {
            page.Open();
        }

        [When(@"I click on audit page (.*)")]
        public void WhenIClickOnExceptionDeliveryPage(int pageNo)
        {
            this.page.Pager.Click(pageNo);
        }

        [When(@"I filter the audits grid with the option '(.*)' and value '(.*)'")]
        public void WhenIFilterTheExceptionDeliveryGridWithTheOptionAndValue(string option, string value)
        {
            this.page.Filter.Apply(option, value);
        }

        [Then(@"'(.*)' rows of audit data will be displayed")]
        public void ThenRowsOfExceptionDeliveryDataWillBeDisplayed(int noOfRowsExpected)
        {
            var pageRows = this.page.Grid.ReturnAllRows().ToList();
            Assert.AreEqual(pageRows.Count, noOfRowsExpected);
        }

        [Then(@"the following audit entries are shown")]
        public void ThenTheFollowingAuditIsCreated(Table table)
        {
            IEnumerable<GridRow<AuditGrid>> pageRows = page.Grid.ReturnAllRows();
            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.AreEqual(table.Rows[i]["Entry"],  pageRows.ElementAt(i).GetColumnValueByIndex((int)AuditGrid.Entry));
                Assert.AreEqual(table.Rows[i]["Type"],  pageRows.ElementAt(i).GetColumnValueByIndex((int)AuditGrid.Type));
                Assert.AreEqual(table.Rows[i]["InvoiceNo"],  pageRows.ElementAt(i).GetColumnValueByIndex((int)AuditGrid.InvoiceNo));
                Assert.AreEqual(table.Rows[i]["Account"],  pageRows.ElementAt(i).GetColumnValueByIndex((int)AuditGrid.Account));
               // Assert.AreEqual(table.Rows[i]["DeliveryDate"],  pageRows.ElementAt(i).GetColumnValueByIndex((int)AuditGrid.DeliveryDate));
            }
        }

    }
}