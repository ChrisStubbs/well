namespace PH.Well.BDD.Steps.Page
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Domain.Enums;
    using Domain.Extensions;
    using Framework.WebElements;
    using NUnit.Framework;
    using OpenQA.Selenium;
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
            page.ActionsTab.Click();
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
            deliveryPageSteps.WhenIOpenDelivery(1);
            deliveryPageSteps.ClickSubmitActions();
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

        [When(@"I add a damage qty of '(.*)' and reason '(.*)'")]
        public void WhenIAddADamageQtyAndReason(string damageQuantity, string reasonCode)
        {
            page.AddDamageButton.Click();
            page.DamageQtyTextBox.EnterText(damageQuantity);
            page.DamageReasonSelect.Select(reasonCode);
        }

        [When(@"I remove all damaages")]
        public void WhenIRemoveAllDamaages()
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
            Thread.Sleep(2000);
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

                ExceptionAction expectedAction =
                    EnumExtensions.GetValueFromDescription<ExceptionAction>(table.Rows[i]["Action"]);
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
                Assert.IsNull(pageRows[i].GetItemInRowById($"remove-action-button{i}"));
            }
        }
    }
}