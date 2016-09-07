namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class FilterControl
    {
        public ButtonDropDown OptionDropDown { get; set; }

        public TextBox FilterText { get; set; }

        public Button ApplyFilter { get; set; }

        public Button ClearFilter { get; set; }

        public FilterControl()
        {
            this.OptionDropDown = new ButtonDropDown { Locator = By.Id("filter-option") };
            this.FilterText = new TextBox { Locator = By.Id("filter-text") };
            this.ApplyFilter = new Button { Locator = By.Id("filter-apply") };
            this.ClearFilter = new Button { Locator = By.Id("filter-clear") };
        }

        public void Apply(string filterOption, string filterValue)
        {
            Clear();
            this.OptionDropDown.Select(filterOption);
            this.FilterText.EnterText(filterValue);
            this.ApplyFilter.Click();
        }

        public void Clear()
        {
            this.ClearFilter.Click();
        }

        public string GetFilterText()
        {
            var attribute = this.FilterText.GetElement().GetAttribute("ng-reflect-model");

            return attribute;
        }

        public string GetSelectedOptionText()
        {
            return this.OptionDropDown.GetElement().Text;
        }
    }
}
