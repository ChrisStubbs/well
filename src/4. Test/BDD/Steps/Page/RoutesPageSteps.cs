namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;
    using NUnit.Framework;
    using Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class RoutesPageSteps
    {

        private RoutesPage RoutesPage => new RoutesPage();

        [When(@"I open the routes page")]
        public void OpenTheRoutesPage()
        {
            this.RoutesPage.Open();
        }

        [When(@"I filter the grid with the option '(.*)' and value '(.*)'")]
        public void WhenIFilterTheGridWithTheOptionAndValue(string option, string value)
        {
            this.RoutesPage.Filter.Apply(option, value);
        }

        [When(@"I clear the filter")]
        public void WhenIClearTheFilter()
        {
            this.RoutesPage.Filter.Clear();
        }

        [When(@"I click on page (.*)")]
        public void WhenIClickOnPage(int pageNo)
        {
            this.RoutesPage.Pager.Click(pageNo);
        }


        [Then(@"The following routes will be displayed")]
        public void ThenTheFollowingRoutesWillBeDisplayed(Table table)
        {
            var pageRows = this.RoutesPage.RoutesGrid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(table.RowCount));
            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(pageRows[i].GetColumnValueByIndex((int)RoutesGrid.Route), Is.EqualTo(table.Rows[i]["Route"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)RoutesGrid.Driver), Is.EqualTo(table.Rows[i]["Driver"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)RoutesGrid.NoOfDrops), Is.EqualTo(table.Rows[i]["NoOfDrops"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)RoutesGrid.Exceptions), Is.EqualTo(table.Rows[i]["Exceptions"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)RoutesGrid.Clean), Is.EqualTo(table.Rows[i]["Clean"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)RoutesGrid.Status), Is.EqualTo(table.Rows[i]["Status"]));
            }
        }

        [Then(@"'(.*)' rows of data will be displayed")]
        public void CheckNoOfRows(int noOfRowsExpected)
        {
            var pageRows = this.RoutesPage.RoutesGrid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(noOfRowsExpected));
        }

        [Then(@"I will have (.*) pages of data")]
        public void CheckNoOfPages(int noOfPages)
        {
            Assert.That(RoutesPage.Pager.NoOfPages(), Is.EqualTo(noOfPages));
        }

    }
}
