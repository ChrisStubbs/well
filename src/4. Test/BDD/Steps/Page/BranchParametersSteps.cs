namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;
    using System.Threading;

    using NUnit.Framework;

    using PH.Well.BDD.Pages;

    using TechTalk.SpecFlow;

    [Binding]
    public class BranchParametersSteps
    {
        private BranchParametersPage page => new BranchParametersPage();
        private BranchPage branchPage => new BranchPage();

        [Given("I navigate to the branch parameters page")]
        public void NavigateToBranchParameters()
        {
            this.page.Open();
            this.page.AdminDropDown.SelectBranchParameters();
        }

        [When("I add a seasonal date")]
        public void AddSeasonalDate()
        {
            this.page.AddButton.Click();
            this.page.Description.EnterText("New year");
            this.page.FromDate.EnterText("24/12/2016");
            this.page.ToDate.EnterText("03/01/2017");
        }

        [When("all branches are selected for the seasonal date")]
        public void AllBranchesForSeasonalDate()
        {
            this.branchPage.SelectAllBranchesCheckbox.Check();
        }
    }
}