using System;
using System.Collections.Generic;
using System.Linq;
namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class ConfirmModal
    {
        public ConfirmModal()
        {
            ConfirmButton = new Button() { Locator = By.Id("exception-confirm-save") };
        }
       
        public Button ConfirmButton { get; set; }
    }
}
