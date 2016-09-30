namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;
    using System.Threading;

    using NUnit.Framework;
    using Pages;

    using PH.Well.BDD.Framework.Context;
    using Repositories.Contracts;
    using StructureMap;
    using TechTalk.SpecFlow;

    [Binding]
    public class ExceptionPageSteps
    {
        private ExceptionDeliveriesPage ExceptionDeliveriesPage => new ExceptionDeliveriesPage();
        private IJobRepository jobRepository;
        private readonly IContainer container;

        [Given(@"I open the exception deliveries")]
        [When(@"I open the exception deliveries")]
        public void WhenIOpenTheExceptionDeliveries()
        {
            
            ExceptionDeliveriesPage.Open();
        }

        [Given(@"an exception with 20 invoiced items")]
        public void GivenAnExceptionWithInvoicedItems()
        {
            var dbSteps = new DatabaseSteps();
            dbSteps.GivenIHaveSelectedBranch(22);
        }

        [Given(@"an exception with 20 invoiced items is assigned to me")]
        public void GivenAnExceptionWithInvoicedItemsIsAssignedToMe()
        {
            var dbSteps = new DatabaseSteps();
            dbSteps.GivenIHaveSelectedBranch(22);
            WhenIOpenTheExceptionDeliveries();
            SelectAssignLink();
            AssignToMe();
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


        [Then(@"there are (.*) exception deliveries will be displayed")]
        public void ThenThereAreExceptionDeliveriesWillBeDisplayed(int currentExceptions)
        {
            var hasNoCurrentExceptions = currentExceptions <= 0;
            var displayed = this.ExceptionDeliveriesPage.IsElementPresent("no-exceptions");
            var noExceptions = displayed == hasNoCurrentExceptions;
            Assert.That(noExceptions, Is.EqualTo(noExceptions));
        }





        [When(@"I click on the orderby Triangle image in the exceptions deliveries grid")]
        public void WhenIClickOnTheOrderbyTriangleImageInTheExceptionsDeliveriesGrid()
        {
            this.ExceptionDeliveriesPage.OrderByButton.Click();
        }

        [Then(@"The following exceptions ordered by date will be displayed in '(.*)' order")]
        public void ThenTheFollowingExceptionsOrderedByDateWillBeDisplayedInOrder(string p0, Table table)
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

        [Then(@"I can the following account info details")]
        public void ThenICanTheFollowingAccountInfoDetails(Table table)
        {
            var modal = ExceptionDeliveriesPage.AccountModal;
            AccountModalSteps.CompareModal(table, modal);
        }

        [When(@"I click on exception row (.*)")]
        public void ClickExceptionDetail(int row)
        {
            var rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();
            rows[row-1].GetItemInRowByClass("first-cell").Click();
        }

        [When(@"I view the account info modal for exception row (.*)")]
        public void ViewAccountModal(int row)
        {
            var rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();
            rows[row-1].GetItemInRowByClass("contact-info").Click();
        }

        [Given(@"I select the assigned link on the first row")]
        [When(@"I select the assigned link on the first row")]
        public void SelectAssignLink()
        {
            var rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();
            var assignAnchor = rows[0].GetItemInRowByClass("assign");
            assignAnchor.Click();
        }

        [When(@"I select the exception row")]
        public void DrillIntoTheException()
        {
            var rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();

            rows[0].GetItemInRowByClass("first-cell").Click();
        }

        [When(@"I select an unassigned exception row")]
        public void DrillIntoTheExceptionViaUnassignedRow()
        {
            var rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();

            rows[1].GetItemInRowByClass("first-cell").Click();
        }

        [Then(@"All the exception detail rows can not be updated")]
        public void TheExceptionDetailRowsCanNotBeUpdated()
        {
            Thread.Sleep(2000);

            var updateableRows = this.ExceptionDeliveriesPage.GetCountOfElements("update-enabled");

            Assert.That(updateableRows, Is.EqualTo(0));
        }

        [Then(@"All the exception detail rows can be updated")]
        public void TheExceptionDetailRowsCanBeUpdated()
        {
            Thread.Sleep(2000);

            var rows = this.ExceptionDeliveriesPage.ExceptionsDrillDownGrid.ReturnAllRows().ToList();

            rows[0].Click();

            Thread.Sleep(2000);

            var updateable = this.ExceptionDeliveriesPage.DeliveryUpdateDrillDown;

            Assert.IsNotNull(updateable);
        }

        [Given(@"I assign the delivery to myself")]
        [When(@"I assign the delivery to myself")]
        public void AssignToMe()
        {
            Thread.Sleep(2000);
            var element = this.ExceptionDeliveriesPage.GetLoggedInAssignUserFromModal();
            ScenarioContextWrapper.SetContextObject(ContextDescriptors.AssignName, element.Text);

            element.Click();
            Thread.Sleep(2000);
        }

        public void SelectUserToAssign(string username)
        {
            Thread.Sleep(2000);
            var element = this.ExceptionDeliveriesPage.GetUserFromModal(username);
            ScenarioContextWrapper.SetContextObject(ContextDescriptors.AssignName, element.Text);

            element.Click();
            Thread.Sleep(2000);
        }

        [Then(@"the user is assigned to that exception")]
        public void UserIsAssignedToTheCorrectException()
        {
            var rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();

            var assignAnchor = rows[0].GetItemInRowByClass("assign");

            var name = ScenarioContextWrapper.GetContextObject<string>(ContextDescriptors.AssignName);

            Assert.That(assignAnchor.Text, Is.EqualTo(name));
            Assert.That(assignAnchor.Text, Is.Not.EqualTo("Unallocated"));

            // refresh the page to ensure name is still the same
            this.ExceptionDeliveriesPage.Open();

            rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();

            assignAnchor = rows[0].GetItemInRowByClass("assign");

            Assert.That(assignAnchor.Text, Is.EqualTo(name));
            Assert.That(assignAnchor.Text, Is.Not.EqualTo("Unallocated"));
        }

        [Then(@"the user can action the exception")]
        public void UserCanActionTheException()
        {
            var element = this.ExceptionDeliveriesPage.EnabledButton;

            Assert.IsNotNull(element);
        }

        [Then(@"all other actions are disabled")]
        public void AllOtherActionsDisabled()
        {
            var rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();

            var disabledCount = this.ExceptionDeliveriesPage.GetCountOfElements("disabled-action");

            Assert.That(rows.Count() - 1, Is.EqualTo(disabledCount));
        }
    }
}
