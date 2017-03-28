namespace PH.Well.BDD.Steps.Page
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using Domain.Enums;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using Pages;

    using PH.Well.BDD.Framework.Context;
    using Repositories.Contracts;
    using StructureMap;
    using TechTalk.SpecFlow;
    using PH.Well.BDD.Framework.Extensions;

    [Binding]
    public class ExceptionPageSteps
    {
        private ExceptionDeliveriesPage ExceptionDeliveriesPage => new ExceptionDeliveriesPage();
        private UserCreditThresholdPage UserCreditThresholdPage => new UserCreditThresholdPage();
        private DeliveryDetailsPage DeliveryDetailsPage => new DeliveryDetailsPage();

        private IJobRepository jobRepository;
        private readonly IContainer container;

        private Dictionary<string, int> bulkSourceMap = new Dictionary<string, int>
        {
            {"Not Defined", 0},
            {"Input", 1 }
        };

        private Dictionary<string, int> bulkReasonMap = new Dictionary<string, int>
        {
            {"Not Defined", 0},
            {"No Credit", 1 }
        };


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
            AssignToMe();
        }

        [Given(@"the exception is assigned to identity: '(.*)', name: '(.*)'")]
        public void GivenTheExceptionIsAssignedToIdentityName(string userIdentity, string userName)
        {

            var dbSteps = new DatabaseSteps();
            dbSteps.GivenIHaveSelectedBranch(22, userIdentity);
            WhenIOpenTheExceptionDeliveries();
            SelectUserToAssign(userName);
        }

        [Given(@"(.*) delivery has all its lines set to credit")]
        public void GivenDeliveryHasAllItsLinesSetToCredit(int noOfDeliveries)
        {
            var setupDeliveryLineUpdate = new SetupDeliveryLineUpdate();
            setupDeliveryLineUpdate.SetDeliveriesToAction(noOfDeliveries, false, DeliveryAction.Credit);
        }

        [Given(@"(.*) delivery has all its lines set to close")]
        public void GivenDeliveryHasAllItsLinesSetToClose(int noOfDeliveries)
        {
            var setupDeliveryLineUpdate = new SetupDeliveryLineUpdate();
            setupDeliveryLineUpdate.SetDeliveriesToAction(noOfDeliveries, false, DeliveryAction.Close);
        }

        [Then(@"the following exception deliveries will be displayed")]
        public void ThenTheFollowingExceptionDeliveriesWillBeDisplayed(Table table)
        {
            var result = this.ExceptionDeliveriesPage.ExceptionsGrid.ContainsSpecFlowTable(table);
            Assert.That(result.HasError, Is.False, result.ErrorsDesc);
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
            var result = this.ExceptionDeliveriesPage.ExceptionsGrid.ContainsSpecFlowTable(table);
            Assert.That(result.HasError, Is.False, result.ErrorsDesc);
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

        [Then(@"no exceptions are displayed")]
        public void ThenNoExceptionsAreDisplayed()
        {
            var noExceptions = ExceptionDeliveriesPage.NoExceptionsDiv.GetElement();
            Assert.IsNotNull(noExceptions);
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

        [Then(@"I click on exception row (.*)")]
        [When(@"I click on exception row (.*)")]
        public void ClickExceptionDetail(int row)
        {
            var rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();
            rows[row - 1].GetItemInRowByClass("first-cell").Click();
        }

        [When(@"I view the account info modal for exception row (.*)")]
        public void ViewAccountModal(int row)
        {
            var rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();
            rows[row - 1].GetItemInRowByClass("contact-info").Click();
        }

        public void SelectAssignLink(int row)
        {
            var rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();
            var assignAnchor = rows[row].GetItemInRowByClass("assign");
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
            var updateableRows = this.ExceptionDeliveriesPage.GetCountOfElements("update-enabled");

            Assert.That(updateableRows, Is.EqualTo(0));
        }

        [Then(@"All the exception detail rows can be updated")]
        public void TheExceptionDetailRowsCanBeUpdated()
        {
            var rows = this.ExceptionDeliveriesPage.ExceptionsDrillDownGrid.ReturnAllRows().ToList();

            rows[0].Click();

            var updateable = this.ExceptionDeliveriesPage.DeliveryUpdateDrillDown;

            Assert.IsNotNull(updateable);
        }

        [Given(@"I assign the delivery to myself")]
        [When(@"I assign the delivery to myself")]
        public void AssignToMe()
        {
            const int firstRow = 0;
            AssignException(firstRow);
        }

        public void AssignException(int rowIndex)
        {
            SelectAssignLink(rowIndex);
            var element = this.ExceptionDeliveriesPage.GetLoggedInAssignUserFromModal();
            ScenarioContextWrapper.SetContextObject(ContextDescriptors.AssignName, element.Text);

            element.Click();
        }

        [Given(@"I assign the following exception lines to myself")]
        public void GivenIAssignTheFollowingExceptionLinesToMyself(Table table)
        {
            foreach (var row in table.Rows)
            {
                var rowIndex = int.Parse(row["LineNo"]) - 1;
                AssignException(rowIndex);
                Thread.Sleep(100);
            }
        }

        [When(@"I assign the delivery on row (.*) to myself")]
        public void WhenIAssignTheDeliveryOnRowToMyself(int row)
        {
            var selectedRow = row - 1;
            SelectAssignLink(selectedRow);

            var element = this.ExceptionDeliveriesPage.GetLoggedInAssignUserFromModal();
            ScenarioContextWrapper.SetContextObject(ContextDescriptors.AssignName, element.Text);

            element.Click();
        }

        [When(@"I assign the delivery on rows (.*) and (.*) to myself")]
        public void WhenIAssignTheDeliveryOnRowsAndToMyself(int firstRow, int secondRow)
        {
            var selectedRow = firstRow - 1;
            SelectAssignLink(selectedRow);

            selectedRow = secondRow - 1;
            SelectAssignLink(selectedRow);

            var element = this.ExceptionDeliveriesPage.GetLoggedInAssignUserFromModal();
            ScenarioContextWrapper.SetContextObject(ContextDescriptors.AssignName, element.Text);

            element.Click();
        }

        public void SelectUserToAssign(string username)
        {
            var firstRow = 0;
            SelectAssignLink(firstRow);

            var element = this.ExceptionDeliveriesPage.AssignModal.GetUserFromModal(username);
            ScenarioContextWrapper.SetContextObject(ContextDescriptors.AssignName, element.Text);

            element.Click();
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

        [When(@"I submit the exception")]
        public void UserCanActionTheException()
        {
            var element = this.ExceptionDeliveriesPage.EnabledButton;
            element.Click();


        }

        [Then(@"the '(.*)' and '(.*)' button is not visible")]
        public void ThenTheAndButtonIsNotVisible(string creditButton, string selectAllButton)
        {
            Assert.That(this.ExceptionDeliveriesPage.IsElementPresent(creditButton), Is.EqualTo(false));
            Assert.That(this.ExceptionDeliveriesPage.IsElementPresent(selectAllButton), Is.EqualTo(false));
        }

        [When(@"click the first credit checkbox")]
        public void WhenClickTheFirstCreditCheckbox()
        {
            var firstCheckbox = this.ExceptionDeliveriesPage.CreditCheckBox(0).GetElement();
            firstCheckbox.Click();
        }

        [Given(@"I click the credit checkbox on the following lines")]
        public void GivenIClickTheCreditCheckboxOnTheFollowingLines(Table table)
        {
            foreach (var row in table.Rows)
            {
                var lineIdx = int.Parse(row["LineNo"]) - 1;
                var chkBox = this.ExceptionDeliveriesPage.CreditCheckBox(lineIdx).GetElement();
                chkBox.Click();
            }
        }

        [Then(@"The credit check box on line (.*) is disabled")]
        public void ThenTheCreditCheckBoxOnLineIsDisabled(int lineNo)
        {
            var lineIdx = lineNo - 1;
            var chkBox = this.ExceptionDeliveriesPage.CreditCheckBox(lineIdx).GetElement();
            Assert.IsFalse(chkBox.Enabled);
        }

        [When(@"I click the Bulk Credit button")]
        public void WhenIClickTheBulkCreditButton()
        {
            var button = this.ExceptionDeliveriesPage.BulkCreditButton.GetElement();
            button.Click();
        }

        [When(@"I click the Select All button")]
        public void WhenIClickTheSelectAllButton()
        {
            var button = this.ExceptionDeliveriesPage.SelectAllButton.GetElement().FindElement(By.Id("selectAll"));
            button.Click();
        }

        [When(@"I click the bulk modal Confirm button")]
        public void WhenIClickTheBulkModalConfirmButton()
        {
            this.ExceptionDeliveriesPage.BulkModalComponent.ConfirmButton.Click();
        }

        [Then(@"the exception deliveries page will show No exceptions found")]
        public void ThenTheExceptionDeliveriesPageWillShowNoExceptionsFound()
        {
            Assert.IsNotNull(this.ExceptionDeliveriesPage.NoExceptionsDiv);
        }


        [Then(@"the '(.*)' and '(.*)' button are visible")]
        public void ThenTheAndButtonAreVisible(string creditButton, string selectAllButton)
        {
            Assert.That(this.ExceptionDeliveriesPage.IsElementPresent(creditButton), Is.EqualTo(true));
            Assert.That(this.ExceptionDeliveriesPage.IsElementPresent(selectAllButton), Is.EqualTo(true));
        }

        [Then(@"the first (.*) checkboxes are checked")]
        public void ThenTheFirstCheckboxesAreChecked(int numberofCheckboxes)
        {
            var rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();

            for (var i = 0; i < numberofCheckboxes - 1; i++)
            {
                var creditCheckbox = rows[i].GetItemInRowByClass("exceptionCheckbox");
                Assert.That(creditCheckbox.Selected, Is.EqualTo(true));
            }
        }

        [When(@"click the confirm button on the modal popup")]
        public void WhenClickTheConfirmButtonOnTheModalPopup()
        {
            this.ExceptionDeliveriesPage.BulkModalComponent.ConfirmButton.Click();
        }

        [When(@"Select the Sources as '(.*)' and reason as '(.*)'")]
        public void WhenSelectTheSourcesAsAndReasonAs(string source, string reason)
        {
            var sourceSelection = this.ExceptionDeliveriesPage.BulkModalComponent.Source(bulkSourceMap[source]);
            sourceSelection.Click();

            var reasonSelection = this.ExceptionDeliveriesPage.BulkModalComponent.Reason(bulkReasonMap[reason]);
            reasonSelection.Click();
        }

        [Then(@"the first delivery line is COD \(Cash on Delivery\)")]
        public void ThenTheFirstDeliveryLineIsCODCashOnDelivery()
        {
            var pageRow = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().First();
            Assert.IsNotNull(pageRow.GetItemInRowById("isCod"));
        }

        [Then(@"the exception cod delivery icon is not displayed in row (.*)")]
        public void ThenTheExceptionCodDeliveryIconIsNotDisplayedInRow(int firstRow)
        {
            var row = firstRow - 1;
            var pageRows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();
            var cashOnDeliveryIcon = pageRows[row].GetItemInRowByClass("fakeCod").Text;
            Assert.That(cashOnDeliveryIcon, Is.Empty);
        }

        [Then(@"the delivery checked icon is not displayed in row (.*)")]
        public void ThenTheDeliveryCheckedIconIsNotDisplayedInRow(int firstRow)
        {
            var row = firstRow - 1;
            var pageRows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();
            var deliveryCheckedIcon = pageRows[row].GetColumnValueByIndex(8);
            Assert.That(deliveryCheckedIcon, !Is.Empty);
        }

        [When(@"I click the Save button")]
        public void WhenIClickTheSaveButton()
        {
            this.UserCreditThresholdPage.SaveButton.Click();
        }

        [Then(@"the Credit Confirm modal is displayed")]
        public void ThenTheCreditConfirmModalIsDisplayed(Table table)
        {
            var modal = ExceptionDeliveriesPage.BulkModalComponent;
            CreditModalSteps.CompareModal(table, modal);
        }

        [When(@"I click the save button on the modal")]
        public void WhenIClickTheSaveButtonOnTheModal()
        {
            var modal = ExceptionDeliveriesPage.BulkModalComponent;
            modal.ConfirmButton.Click();
        }

        [When(@"I select the exception submit button")]
        public void SelectExceptionSubmitButton()
        {
            ExceptionDeliveriesPage.FirstRowSubmitButton.Click();
        }

        [When(@"I select the exception submit button on Row '(.*)'")]
        public void SelectExceptionSubmitButtonRowX(int rowNumber)
        {
            var row = ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList()[rowNumber - 1];
            var button = row.GetItemInRowById($"submit{rowNumber}");
            button.Click();
        }

        [When(@"I confirm the exception submit")]
        public void ConfirmSubmitButton()
        {
            this.ExceptionDeliveriesPage.ConfirmModal.ConfirmButton.Click();
        }

        [Then(@"I can see the product information '(.*)'")]
        public void ViewProductInformation(string productInformation)
        {
            Assert.AreEqual(this.ExceptionDeliveriesPage.ProductInformation.Text, productInformation);
        }

        [Then(@"I can see the shortage quantity of '(.*)'")]
        public void ViewShortageQuantity(string quantity)
        {
            Assert.AreEqual(this.ExceptionDeliveriesPage.ShortQty.Text, quantity);
        }

        [Then(@"I can see the shortage reason of '(.*)'")]
        public void ViewShortageReason(string reason)
        {
            Assert.AreEqual(this.ExceptionDeliveriesPage.ShortReason.Text, reason);
        }

        [Then(@"I can see the shortage source of '(.*)'")]
        public void ViewShortageSource(string source)
        {
            Assert.AreEqual(this.ExceptionDeliveriesPage.ShortSource.Text, source);
        }

        [Then(@"I can see the shortage action of '(.*)'")]
        public void ViewShortageAction(string action)
        {
            Assert.AreEqual(this.ExceptionDeliveriesPage.ShortAction.Text, action);
        }

        [Then(@"I can see the damage quantity of '(.*)'")]
        public void ViewDamageQuantity(string quantity)
        {
            Assert.AreEqual(this.ExceptionDeliveriesPage.DamageQty.Text, quantity);
        }

        [Then(@"I can see the damage reason of '(.*)'")]
        public void ViewDamageReason(string reason)
        {
            Assert.AreEqual(this.ExceptionDeliveriesPage.DamageReason.Text, reason);
        }

        [Then(@"I can see the damage source of '(.*)'")]
        public void ViewDamageSource(string source)
        {
            Assert.AreEqual(this.ExceptionDeliveriesPage.DamageSource.Text, source);
        }

        [Then(@"I can see the damage action of '(.*)'")]
        public void ViewDamageAction(string action)
        {
            Assert.AreEqual(this.ExceptionDeliveriesPage.DamageAction.Text, action);
        }

        [When(@"I click on each of the deliveries on page (.*) there will be at least one exception delivery line")]
        public void WhenIClickOnEachOfTheDeliveriesOnPageThereWillBeAtLeastOneDeliveryLine(int pageNo)
        {
            this.ExceptionDeliveriesPage.Pager.Click(pageNo);
            var initialPageRows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();
            var totalRowCount = initialPageRows.Count;

            for (int i = 0; i < totalRowCount; i++)
            {
                var rows = this.ExceptionDeliveriesPage.ExceptionsGrid.ReturnAllRows().ToList();
                var row = rows[i];
                row.GetItemInRowByClass("first-cell").Click();
                this.DeliveryDetailsPage.ClickExceptionsTab();
                Assert.That(this.DeliveryDetailsPage.Grid.ReturnAllRows().Count, Is.GreaterThan(0));
                this.DeliveryDetailsPage.Back();
            }
        }


    }
}
