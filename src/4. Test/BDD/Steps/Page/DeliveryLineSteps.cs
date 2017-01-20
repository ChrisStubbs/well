namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;
    using System.Threading;
    using Domain.Enums;
    using Domain.Extensions;
    using NUnit.Framework;
    using PH.Well.BDD.Pages;
    using TechTalk.SpecFlow;

    [Binding]
    public class DeliveryLineSteps
    {
        private DeliveryLinePage page => new DeliveryLinePage();

        [Given(@"I view the Actions for line '(.*)' of Delivery '(.*)'")]
        [When(@"I view the Actions for line '(.*)' of Delivery '(.*)'")]
        public void GivenIViewTheActionsForLineOfDelivery(int line, int delivery)
        {
            string routing = "/" + delivery + "/" + line;
            page.Open(routing);

            Thread.Sleep(1000);

            page.ActionsTab.Click();
        }

        [When(@"I view the Issues for line '(.*)' of Delivery '(.*)'")]
        public void GivenIViewTheIssuesForLineOfDelivery(int line, int delivery)
        {
            string routing = "/" + delivery + "/" + line;
            page.Open(routing);
        }

        [When(@"I open delivery '(.*)'")]
        public void WhenIOpenDelivery(int delivery)
        {
            string routing = "/" + delivery ;
            page.Open(routing);
        }


        [Given(@"an exception with a submitted action")]
        public void GivenAnExceptionWithASubmittedAction()
        {
            GivenAnExceptionWithSubmittedActionIsAssignedToMe();
            var exceptionPageSteps = new ExceptionPageSteps();
            exceptionPageSteps.WhenIOpenTheExceptionDeliveries();
            exceptionPageSteps.SelectUserToAssign("Unallocated");
        }

        [Given(@"an exception with a draft action")]
        public void GivenAnExceptionWithDraftAction()
        {
            var exceptionPageSteps = new ExceptionPageSteps();
            exceptionPageSteps.GivenAnExceptionWithInvoicedItemsIsAssignedToMe();
            GivenIViewTheActionsForLineOfDelivery(1, 1);
            WhenIAddTheActionToItem("Credit", "1");
            WhenISaveTheDeliveryLineUpdates();
            exceptionPageSteps.WhenIOpenTheExceptionDeliveries();
            exceptionPageSteps.SelectUserToAssign("Unallocated");
        }

        [Given(@"an exception with a submitted action is assigned to me")]
        public void GivenAnExceptionWithSubmittedActionIsAssignedToMe()
        {
            var exceptionPageSteps = new ExceptionPageSteps();
            exceptionPageSteps.GivenAnExceptionWithInvoicedItemsIsAssignedToMe();
            GivenIViewTheActionsForLineOfDelivery(1, 1);
            WhenIAddTheActionToItem("Credit", "1");
            WhenISaveTheDeliveryLineUpdates();
            var deliveryPageSteps = new DeliveryDetailSteps();
            deliveryPageSteps.WhenIOpenTheExceptionDelivery(1);
            deliveryPageSteps.ClickSubmitActions();
        }

        [Given(@"an exception with a submitted action is assigned to identity: '(.*)', name: '(.*)'")]
        public void GivenAnExceptionWithASubmittedActionIsAssignedToIdentityName(string userIdentity, string name)
        {
            GivenAnExceptionWithSubmittedActionIsAssignedToMe();

            var dbSteps = new DatabaseSteps();
            dbSteps.GivenIHaveSelectedBranch(22, userIdentity);

            var exceptionPageSteps = new ExceptionPageSteps();
            exceptionPageSteps.WhenIOpenTheExceptionDeliveries();
            exceptionPageSteps.SelectUserToAssign(name);
        }


        [When(@"I add the '(.*)' action to (.*) item")]
        public void WhenIAddTheActionToItem(string action, string quantity)
        {
            page.AddActionButton.Click();
            page.NewActionQuantityTextBox.EnterText(quantity);
            page.NewActionDropDown.Select(action);
        }

        [When(@"I enter a short quantity of '(.*)'")]
        public void WhenIEnterAShortQuantityOf(string shortQty)
        {
            page.ShortQtyTextBox.EnterText(shortQty);
        }

        [When(@"I select a short action of '(.*)'")]
        public void SelectShortAction(string shortsAction)
        {
            this.page.ShortActionSelect.Select(shortsAction);
        }

        [When(@"I select a short reason of '(.*)'")]
        public void SelectShortReason(string shortsReason)
        {
            this.page.ShortReasonSelect.Select(shortsReason);
        }

        [When(@"I select a short source of '(.*)'")]
        public void SelectShortSource(string shortsSource)
        {
            this.page.ShortSourceSelect.Select(shortsSource);
        }

        [When(@"I enter a damage qty of '(.*)' for id '(.*)'")]
        public void EnterDamageQuantity(string qty, int id)
        {
            this.page.DamageQtyInput(id).EnterText(qty);
        }

        [When(@"I enter a damage reason of '(.*)' for id '(.*)'")]
        public void EnterDamageReason(string reason, int id)
        {
            this.page.DamageReasonSelect(id).Select(reason);
        }

        [When(@"I enter a damage source of '(.*)' for id '(.*)'")]
        public void EnterDamageSource(string source, int id)
        {
            this.page.DamageSourceSelect(id).Select(source);
        }

        [When(@"click add damage button")]
        public void ClickAddDamageButton()
        {
            page.AddDamageButton.Click();
        }

        [When(@"I add a damage qty of '(.*)' and reason '(.*)'")]
        public void WhenIAddADamageQtyAndReason(string damageQuantity, string reasonCode)
        {
            page.AddDamageButton.Click();
            page.FirstDamageQtyTextBox.EnterText(damageQuantity);
            page.FirstDamageReasonSelect.Select(reasonCode);
        }

        [When(@"I remove all damages")]
        public void WhenIRemoveAllDamages()
        {
            var buttons = page.GetRemoveDamageButtons(2);
            foreach (var button in buttons)
            {
                button.Click();
            }
        }

        [When(@"I save the delivery line updates")]
        public void WhenISaveTheDeliveryLineUpdates()
        {
            page.SaveButton.Click();
            Thread.Sleep(1000);
        }

        [When(@"I confirm the delivery line update")]
        public void WhenIConfirmTheDeliveryLineUpdate()
        {
            page.ConfirmButton.Click();
        }

        [Then(@"the following actions are shown on the delivery items")]
        public void ThenTheFollowingActionsAreShownOnTheDeliveryItems(Table table)
        {
            var pageRows = this.page.ActionGrid.ReturnAllRows().ToList();

            Assert.That(pageRows.Count, Is.EqualTo(table.RowCount));

            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.AreEqual(table.Rows[i]["Quantity"], pageRows[i].GetItemInRowById($"action-qty-input{i}").GetAttribute("value"));

                DeliveryAction expectedAction =
                    EnumExtensions.GetValueFromDescription<DeliveryAction>(table.Rows[i]["Action"]);
                int actionSelectValue =
                    int.Parse(pageRows[i].GetItemInRowById($"action-select{i}").GetAttribute("value"));
                Assert.AreEqual((int) expectedAction, actionSelectValue);
                Assert.AreEqual( table.Rows[i]["Status"], pageRows[i].GetItemInRowById($"action-status{i}").Text);
            }
        }

        [Then(@"I can not edit any action")]
        public void ThenICanNotEditAnyAction()
        {
            var pageRows = page.ActionGrid.ReturnAllRows().ToList();

            for (int i = 0; i < pageRows.Count; i++)
            {
                Assert.AreEqual("true", pageRows[i].GetItemInRowById($"action-qty-input{i}").GetAttribute("disabled"));
                Assert.AreEqual("true", pageRows[i].GetItemInRowById($"action-select{i}").GetAttribute("disabled"));
                Assert.AreEqual("true", pageRows[i].GetItemInRowById($"remove-action-button{i}").GetAttribute("disabled"));
            }
        }

        [Then(@"I cannot add or edit any shorts or damages")]
        public void ThenICannotEditAnyShortsOrDamages()
        {
            Assert.AreEqual("true", page.ShortQtyTextBox.GetElement().GetAttribute("disabled"));

            var pageRows = page.DamagesGrid.ReturnAllRows().ToList();

            for (int i = 0; i < pageRows.Count; i++)
            {
                Assert.AreEqual("true", pageRows[i].GetItemInRowById($"damage-qty-input{i}").GetAttribute("disabled"));
                Assert.AreEqual("true", pageRows[i].GetItemInRowById($"damage-reason-select{i}").GetAttribute("disabled"));
                Assert.AreEqual("true", pageRows[i].GetItemInRowById($"damage-source-select{i}").GetAttribute("disabled"));
                Assert.AreEqual("true", pageRows[i].GetItemInRowById($"remove-damage-button{i}").GetAttribute("disabled"));
            }

            Assert.AreEqual("true", page.AddDamageButton.GetElement().GetAttribute("disabled"));
        }

        [Then(@"I can not add any action to the delivery")]
        public void ThenICanNotAddAnyActionToTheDelivery()
        {
            Assert.AreEqual("true", page.AddActionButton.GetElement().GetAttribute("disabled"));
        }

        [Then(@"an error is returned '(.*)'")]
        public void ThenAnErrorIsReturned(string error)
        {
            ScenarioContext.Current.Pending();
        }
    }
}