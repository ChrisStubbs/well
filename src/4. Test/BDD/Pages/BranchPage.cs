namespace PH.Well.BDD.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    using PH.Well.BDD.Framework;
    using PH.Well.BDD.Framework.Extensions;
    using PH.Well.BDD.Framework.WebElements;

    public class BranchPage : Page
    {
        public readonly AdminButtonDropDown AdminDropDown;

        public readonly CheckBox SelectAllBranchesCheckbox;

        public readonly Button SaveButton;

        public BranchPage()
        {
            this.AdminDropDown = new AdminButtonDropDown { Locator = By.Id("admin-dropdown") };
            this.SelectAllBranchesCheckbox = new CheckBox { Locator = By.Id("select-all-branches") };
            this.SaveButton = new Button { Locator = By.Id("save") };
        }

        protected override string UrlSuffix { get; }

        public List<IWebElement> GetBranchCheckboxElements()
        {
            this.Driver.WaitForAjax();

            var wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(Configuration.DriverTimeoutSeconds));

            var elements = wait.Until(d => d.FindElements(By.ClassName("branch-checkboxes")));

            return elements.ToList();
        }
    }
}