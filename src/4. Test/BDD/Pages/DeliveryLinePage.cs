﻿namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class DeliveryLinePage : Page
    {
        public DeliveryLinePage()
        {
            ShortQtyTextBox = new TextBox { Locator = By.Id("short-qty-input") };

            AddDamageButton = new Button { Locator = By.Id("add-damage-button") };
            DamageQtyTextBox = new TextBox { Locator = By.Id("damage-qty-input0") };
            DamageReasonSelect = new HtmlSelectElement() { Locator = By.Id("reason-select0") };

            SaveButton = new Button { Locator = By.Id("save-button") };
            ConfirmButton = new Button { Locator = By.Id("confirm-modal-button") };
        }

        protected override string UrlSuffix => "delivery";

     
        public TextBox ShortQtyTextBox;
        public readonly Button AddDamageButton;
        public TextBox DamageQtyTextBox;
        public HtmlSelectElement DamageReasonSelect;
        public readonly Button SaveButton;
        public readonly Button ConfirmButton;
    }

}
