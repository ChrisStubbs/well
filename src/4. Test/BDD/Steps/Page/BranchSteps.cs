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
            this.branchPage.AdminDropDown.SelectBranchSelection();
        }
        
        [When("select branches selection")]
        public void ClickBranchsSelection()
        {
            this.branchPage.Open();
            this.branchPage.AdminDropDown.SelectBranchSelection();
        }

        [When("I select all the branches")]
        [Then("I select all the branches")]
        public void SelectAllBranches()
        {
            Thread.Sleep(2000);
            this.branchPage.SelectAllBranchesCheckbox.Check();
        }

        [When("I save my branches")]
        [Then("I save my branches")]
        [When("I save the branch selection")]
        public void SaveBranches()
        {
            this.branchPage.SaveButton.Click();
        }

        [When("I select branch '(.*)'")]
        [When("I deselect branch '(.*)'")]
        public void SelectBranch(string branch)
        {
            this.branchPage.GetCheckBox(branch).Click();
        }
        
        [Then("branch is selected (.*)")]
        public void BranchIsSelected(string branch)
        {
            var branchCheckbox = this.branchPage.GetCheckBox(branch);

            Assert.IsTrue(branchCheckbox.Selected);
        }

        [Then("branch is not selected (.*)")]
        public void BranchIsNotSelected(string branch)
        {
            var branchCheckbox = this.branchPage.GetCheckBox(branch);

            Assert.IsFalse(branchCheckbox.Selected);
        }

        [Then("all the branches are selected")]
        [Then("all the branches are saved")]
        public void AllBranchesSelected()
        {
            Thread.Sleep(1500);
            var checkboxes = this.branchPage.GetBranchCheckboxElements();

            Assert.That(checkboxes.Count, Is.EqualTo(12));

            foreach (var box in checkboxes)
            {
                Assert.IsTrue(box.Selected);
            }
        }
    }
}