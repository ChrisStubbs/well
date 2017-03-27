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
            FirstDamageQtyTextBox = new TextBox { Locator = By.Id("damage-qty-input0") };
            FirstDamageReasonSelect = new HtmlSelectElement() { Locator = By.Id("reason-select0") };
            FirstDamageActionSelect = new HtmlSelectElement() { Locator = By.Id("damage-action-select0") };
            DamagesGrid = new Grid<DamagesGridCols> {Locator = By.Id("damageTable"), RowLocator = By.ClassName("editable") };
            SaveButton = new Button { Locator = By.Id("save-button") };
            ConfirmButton = new Button { Locator = By.Id("confirm-modal-button") };
            AddActionButton = new Button() { Locator = By.Id("add-action-button") };
            ActionGrid = new Grid<ActionGridCols> { Locator = By.Id("actionTable"), RowLocator = By.ClassName("editable") };
            this.ShortSourceSelect = new HtmlSelectElement() { Locator = By.Id("short-source-select") };
            this.ShortReasonSelect = new HtmlSelectElement() { Locator = By.Id("short-reason-select") };
            this.ShortActionSelect = new HtmlSelectElement() { Locator = By.Id("short-reason-action") };
        }

        protected override string UrlSuffix => "delivery";

        public readonly TextBox ShortQtyTextBox;
        public readonly Button AddDamageButton;
        public readonly TextBox FirstDamageQtyTextBox;
        public readonly HtmlSelectElement FirstDamageReasonSelect;
        public readonly HtmlSelectElement FirstDamageActionSelect;
        public readonly Button SaveButton;
        public readonly Button ConfirmButton;
        public readonly HtmlSelectElement ShortSourceSelect;
        public readonly HtmlSelectElement ShortReasonSelect;
        public readonly HtmlSelectElement ShortActionSelect;
 public TextBox DamageQtyInput(int id)
        {
            return new TextBox() { Locator = By.Id("damage-qty-input" + id) };
        }

       
        public HtmlSelectElement DamageReasonSelect(int id)
        {
            return new HtmlSelectElement() { Locator = By.Id("damage-reason-select" + id) };
        }

        public HtmlSelectElement DamageSourceSelect(int id)
        {
            return new HtmlSelectElement() { Locator = By.Id("damage-source-select" + id) };
        }

        public HtmlSelectElement DamageActionSelect(int id)
        {
            return new HtmlSelectElement() { Locator = By.Id("damage-action-select" + id) };
        }

        public Button DamageRemoveButton(int id)
        {
            return new Button() { Locator = By.Id("remove-damage-button" + id) };
        }

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

        public Grid<DamagesGridCols> DamagesGrid { get; set; }

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

        public enum DamagesGridCols
        {
            Quantity = 0,
            Reason = 1,
            Remove = 2
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
