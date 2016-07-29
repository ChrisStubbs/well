namespace PH.Well.BDD.Steps.Page
{
    using System.Threading;

    using NUnit.Framework;

    using PH.Well.BDD.Pages;

    using TechTalk.SpecFlow;

    [Binding]
    public class BranchSteps
    {
        private BranchPage branchPage => new BranchPage();

        [When("I navigate to the branches page")]
        public void NavigateToBranches()
        {
            this.branchPage.Open();
            this.branchPage.AdminDropDown.Select();
        }

        [When("select branches selection")]
        public void ClickBranchsSelection()
        {
            this.branchPage.AdminDropDown.Select();
        }

        [When("I select all the branches")]
        public void SelectAllBranches()
        {
            Thread.Sleep(2000);
            this.branchPage.SelectAllBranchesCheckbox.Check();
        }

        [When("I save my branches")]
        public void SaveBranches()
        {
            this.branchPage.SaveButton.Click();
        }

        [Then("all the branches are selected")]
        public void AllBranchesSelected()
        {
            Thread.Sleep(2000);
            var checkboxes = this.branchPage.GetBranchCheckboxElements();

            Assert.That(checkboxes.Count, Is.EqualTo(12));

            foreach (var box in checkboxes)
            {
                Assert.IsTrue(box.Selected);
            }
        }
    }
}