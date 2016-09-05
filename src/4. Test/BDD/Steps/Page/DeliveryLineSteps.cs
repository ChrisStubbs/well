namespace PH.Well.BDD.Steps.Page
{
    using System.Collections.Generic;
    using System.Linq;
    using Framework.WebElements;
    using NUnit.Framework;

    using PH.Well.BDD.Pages;

    using TechTalk.SpecFlow;

    [Binding]
    public class DeliveryLineSteps
    {
        private DeliveryLinePage page => new DeliveryLinePage();

        [When(@"I add a short quantity of '(.*)'")]
        public void WhenIAddAShortQuantityOf(string shortQty)
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

        [When(@"I save the delivery line updates")]
        public void WhenISaveTheDeliveryLineUpdates()
        {
            page.SaveButton.Click();
        }

        [When(@"I confirm the delivery line update")]
        public void WhenIConfirmTheDeliveryLineUpdate()
        {
            page.ConfirmButton.Click();
        }





    }
}