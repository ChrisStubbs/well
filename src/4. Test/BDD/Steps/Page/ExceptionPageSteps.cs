namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;
    using System.Threading;

    using NUnit.Framework;
    using Pages;

    using PH.Well.BDD.Framework.Context;

    using TechTalk.SpecFlow;

    [Binding]
    public class ExceptionPageSteps
    {
        private ExceptionDeliveriesPage ExceptionDeliveriesPage => new ExceptionDeliveriesPage();

        [When(@"I open the exception deliveries")]
        public void WhenIOpenTheExceptionDeliveries()
        {
            ExceptionDeliveriesPage.Open();
        }

        [Then(@"the following exception deliveries will be displayed")]
        public void ThenTheFollowingExceptionDeliveriesWillBeDisplayed(Table table)
        {
            var pageRows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(table.RowCount));
            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Route), Is.EqualTo(table.Rows[i]["Route"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Drop), Is.EqualTo(table.Rows[i]["Drop"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.InvoiceNo), Is.EqualTo(table.Rows[i]["InvoiceNo"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Account), Is.EqualTo(table.Rows[i]["Account"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.AccountName), Is.EqualTo(table.Rows[i]["AccountName"]));
                Assert.That(pageRows[i].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Status), Is.EqualTo(table.Rows[i]["Status"]));
            }
        }
        
        [When(@"I filter the exception delivery grid with the option '(.*)' and value '(.*)'")]
        public void WhenIFilterTheExceptionDeliveryGridWithTheOptionAndValue(string option, string value)
        {
            this.ExceptionDeliveriesPage.Filter.Apply(option, value);
        }

        [When(@"I click on exception delivery page (.*)")]
        public void WhenIClickOnExceptionDeliveryPage(int pageNo)
        {
            this.ExceptionDeliveriesPage.Pager.Click(pageNo);
        }
        
        [Then(@"'(.*)' rows of exception delivery data will be displayed")]
        public void ThenRowsOfExceptionDeliveryDataWillBeDisplayed(int noOfRowsExpected)
        {
            var pageRows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();
            Assert.That(pageRows.Count, Is.EqualTo(noOfRowsExpected));
        }

        [Then(@"I will have (.*) pages of exception delivery data")]
        public void CheckNoOfPages(int noOfPages)
        {
            Assert.That(ExceptionDeliveriesPage.Pager.NoOfPages(), Is.EqualTo(noOfPages));
        }

        [When(@"I click on a exception row")]
        public void ClickExceptionDetail()
        {
            this.ExceptionDeliveriesPage.GetFirstCell().Click();
        }

        [When(@"I select the assigned link on the first row")]
        public void SelectAssignLink()
        {
            var rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();

            var unallocated = rows[0].GetColumnValueByIndex((int)ExceptionDeliveriesGrid.Assigned);

            Assert.That(unallocated, Is.EqualTo("Unallocated"));
        }

        [When(@"I select a user to assign")]
        public void SelectUserToAssign()
        {
            Thread.Sleep(2000);
            var element = this.ExceptionDeliveriesPage.GetFirstAssignUserFromModal();

            element.Click();

            ScenarioContextWrapper.SetContextObject(ContextDescriptors.AssignName, element.Text);
        }

        [Then(@"the user is assigned to that exception")]
        public void UserIsAssignedToTheCorrectException()
        {
            /*var element = this.ExceptionDeliveriesPage.GetFirstAssignAnchor();

            var name = ScenarioContextWrapper.GetContextObject<string>(ContextDescriptors.AssignName);

            Assert.That(element.Text, Is.EqualTo(name));
            Assert.That(element.Text, Is.Not.EqualTo("Unallocated"));

            // refresh the page to ensure name is still the same
            this.ExceptionDeliveriesPage.Open();

            // element = this.ExceptionDeliveriesPage.GetFirstAssignAnchor();

            Assert.That(element.Text, Is.EqualTo(name));
            Assert.That(element.Text, Is.Not.EqualTo("Unallocated"));*/
        }

        [Then(@"the user can action the exception")]
        public void UserCanActionTheException()
        {
            var element = this.ExceptionDeliveriesPage.EnabledButton;

            Assert.IsNotNull(element);
        }
    }
}
