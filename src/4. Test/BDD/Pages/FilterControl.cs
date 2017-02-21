namespace PH.Well.BDD.Pages
{
    using Framework.WebElements;
    using OpenQA.Selenium;
    using System;

    public class FilterControl
    {
        public ButtonDropDown OptionDropDown { get; set; }

        private TextBox FilterText { get; set; }

        private PrimengCalendar FilterDate { get; set; }

        public Button ApplyFilter { get; set; }

        public Button ClearFilter { get; set; }

        public FilterControl()
        {
            this.OptionDropDown = new ButtonDropDown { Locator = By.Id("filter-option") };
            this.FilterText = new TextBox { Locator = By.Id("filter-text") };
            this.ApplyFilter = new Button { Locator = By.Id("filter-apply") };
            this.ClearFilter = new Button { Locator = By.Id("filter-clear") };
            this.FilterDate = new PrimengCalendar("#filter-text");
        }

        public void Apply<T>(string filterOption, T filterValue) where T : struct
        {
            if (filterValue is DateTime)
            {
                this.Apply(filterOption, () => this.FilterDate.Date = (DateTime)(object)filterValue);
            }
            else
            {
                this.Apply(filterOption, filterValue.ToString());
            }
        }
        
        private void Apply(string filterOption, Action writer)
        {
            Clear();
            this.OptionDropDown.Select(filterOption);
            writer();
            this.ApplyFilter.Click();
        }

        public void Apply(string filterOption, string filterValue)
        {
            this.Apply(filterOption, () => this.FilterText.EnterText(filterValue));
        }

        public void Select(string filterOption)
        {
            this.OptionDropDown.Select(filterOption);
        }


        public void Clear()
        {
            if (this.ClearFilter.GetElement().Enabled)
            {
                this.ClearFilter.Click();
            }
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
