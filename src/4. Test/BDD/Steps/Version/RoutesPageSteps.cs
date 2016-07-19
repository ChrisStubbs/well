namespace PH.Well.BDD.Steps.Version
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


    }
}
