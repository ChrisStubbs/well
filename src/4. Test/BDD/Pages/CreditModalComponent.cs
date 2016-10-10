using System;
using System.Collections.Generic;
using System.Linq;
namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class CreditModalComponent
    {
        public CreditModalComponent()
        {
            ModalTitle = new Heading() { Locator = By.ClassName("modal-title") };
            ConfirmButton = new Button() { Locator = By.Id("confirm-modal-button") };
            CancelButton = new Button() { Locator = By.Id("cancel-modal-button") };
        }

        public Heading ModalTitle { get; set; }
        public Button ConfirmButton { get; set; }
        public Button CancelButton { get; set; }
    }
}
