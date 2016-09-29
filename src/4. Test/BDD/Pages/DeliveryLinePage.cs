namespace PH.Well.BDD.Pages
{
    using System.Collections.Generic;
    using System.Linq;
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

            AddActionButton = new Button() { Locator = By.Id("add-action-button") };
            ActionGrid = new Grid<ActionGridCols> { Locator = By.Id("actionTable"), RowLocator = By.ClassName("editable") };
        }

        protected override string UrlSuffix => "delivery";

     
        public TextBox ShortQtyTextBox;
        public readonly Button AddDamageButton;
       
        public TextBox DamageQtyTextBox;
        public HtmlSelectElement DamageReasonSelect;
        public readonly Button SaveButton;
        public readonly Button ConfirmButton;

        public IWebElement ActionsTab
        {
            get
            {
                var tabsList = new List() { Locator = By.ClassName("nav-tabs") };
                var buttons = tabsList.GetElement().FindElements(By.ClassName("btn"));
                return buttons[1];
            }
        }

        public Button AddActionButton { get; set; }

        public List<Button> GetRemoveDamageButtons(int buttonCount)
        {
            var buttons = new List<Button>();
            for (int i = buttonCount-1; i >= 0; i--)
            {
                buttons.Add(new Button { Locator = By.Id("remove-damage-button" + i) });
            }
            return buttons;
        }

        public Grid<ActionGridCols> ActionGrid { get; set; }

        public TextBox NewActionQuantityTextBox
        {
            get
            {
                var rows = ActionGrid.ReturnAllRows().Count();
                return new TextBox() { Locator = By.Id($"action-qty-input{rows-1}") };
            }
        }

        public HtmlSelectElement NewActionDropDown
        {
            get
            {
                var rows = ActionGrid.ReturnAllRows().Count();
                return new HtmlSelectElement() { Locator = By.Id($"action-select{rows - 1}") };
            }
        }

        public enum ActionGridCols
        {
            Quantity = 0,
            Action = 1,
            Status = 2,
            Remove = 3
        }
    }

}
