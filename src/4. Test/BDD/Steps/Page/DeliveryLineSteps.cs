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
        }

        [When(@"I confirm the delivery line update")]
        public void WhenIConfirmTheDeliveryLineUpdate()
        {
            page.ConfirmButton.Click();
        }





    }
}