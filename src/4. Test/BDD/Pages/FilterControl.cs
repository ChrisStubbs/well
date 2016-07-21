namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;

    public class FilterControl
    {
        private readonly ButtonDropDown optionDropDown;
        private readonly TextBox filterText;
        private readonly Button applyFilter;
        private readonly Button clearFilter;

        public FilterControl()
        {
            this.optionDropDown = new ButtonDropDown { Locator = By.Id("filter-option") };
            this.filterText = new TextBox { Locator = By.Id("filter-text") };
            this.applyFilter = new Button { Locator = By.Id("filter-apply") };
            this.clearFilter = new Button { Locator = By.Id("filter-clear") };
        }

        public void Apply(string filterOption, string filterValue)
        {
            Clear();
            this.optionDropDown.Select(filterOption);
            this.filterText.EnterText(filterValue);
            this.applyFilter.Click();
        }

        public void Clear()
        {
            this.clearFilter.Click();
        }


    }
}
