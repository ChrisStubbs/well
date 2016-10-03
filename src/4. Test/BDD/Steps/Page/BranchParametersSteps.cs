namespace PH.Well.BDD.Steps.Page
{
    using NUnit.Framework;

    using PH.Well.BDD.Pages;

    using TechTalk.SpecFlow;

    [Binding]
    public class BranchParametersSteps
    {
        private SeasonalDatesPage page => new SeasonalDatesPage();
        private CreditThresholdPage creditThresholdPage => new CreditThresholdPage();
        private BranchPage branchPage => new BranchPage();
        private CleanPreferencePage cleanPage => new CleanPreferencePage();

        [Given("I navigate to the branch parameters page")]
        [Then("I navigate to the branch parameters page")]
        public void NavigateToBranchParameters()
        {
            this.page.Open();
            this.page.AdminDropDown.SelectBranchParameters();
        }

        [When("I add a seasonal date")]
        public void AddSeasonalDate(Table table)
        {
            this.page.AddButton.Click();
            this.page.Description.EnterText(table.Rows[0]["Description"]);
            this.page.FromDate.EnterText(table.Rows[0]["FromDate"]);
            this.page.ToDate.EnterText(table.Rows[0]["ToDate"]);
        }

        [When("I add a credit threshold")]
        public void AddCreditThreshold(Table table)
        {
            this.creditThresholdPage.ClickThresholdTab();
            this.creditThresholdPage.AddButton.Click();
            this.creditThresholdPage.dropdown.SelectLevel1();
            this.creditThresholdPage.Threshold.EnterText(table.Rows[0]["Threshold"]);
        }

        [When("I add a clean parameter")]
        public void AddCleanParameter(Table table)
        {
            this.cleanPage.ClickCleanDeliveriesTab();
            this.cleanPage.Add.Click();
            this.cleanPage.Days.EnterText(table.Rows[0]["Days"]);
        }

        [When("I select the credit threshold tab")]
        public void SelectCreditThresholdTab()
        {
            this.creditThresholdPage.ClickThresholdTab();
        }

        [When("I select the clean parameter tab")]
        public void SelectCleanParameterTab()
        {
            this.cleanPage.ClickCleanDeliveriesTab();
        }

        [When("I edit a seasonal date")]
        public void EditSeasonalDate(Table table)
        {
            var grid = this.page.GetGrid(1);

            grid[0].Description.Click();

            this.page.Description.EnterText(table.Rows[0]["Description"]);
            this.page.FromDate.EnterText(table.Rows[0]["FromDate"]);
            this.page.ToDate.EnterText(table.Rows[0]["ToDate"]);
        }

        [When("I edit a credit threshold")]
        public void EditCreditThreshold(Table table)
        {
            var grid = this.creditThresholdPage.GetGridById(1);

            grid[0].Level.Click();

            this.creditThresholdPage.Threshold.EnterText(table.Rows[0]["Threshold"]);
        }

        [When("I edit a clean parameter")]
        public void EditCleanParameter(Table table)
        {
            var grid = this.cleanPage.GetGridById(1);

            grid[0].Days.Click();

            this.cleanPage.Days.EnterText(table.Rows[0]["Days"]);
        }

        [When("all branches are selected for the seasonal date")]
        [When("all branches are selected for the credit threshold")]
        [When("all branches are selected for the clean parameter")]
        public void AllBranchesForSeasonalDate()
        {
            this.branchPage.SelectAllBranchesCheckbox.Check();
        }

        [When("I save the seasonal date")]
        [When("I update the seasonal date")]
        public void SaveSeasonalDate()
        {
            this.page.SaveButton.Click();
        }

        [When("I save the credit threshold")]
        [When("I update the credit threshold")]
        public void SaveCreditThreshold()
        {
            this.creditThresholdPage.SaveButton.Click();
        }

        [When("I save the clean parameter")]
        [When("I update the clean parameter")]
        public void SaveCleanParameter()
        {
            this.cleanPage.Save.Click();
        }

        [Then("the seasonal date is saved")]
        public void SeasonalDateSaved(Table table)
        {
            var grid = this.page.GetGrid(1);

            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(grid[i].Description.Text, Is.EqualTo(table.Rows[i]["Description"]));
                Assert.That(grid[i].FromDate.Text, Is.EqualTo(table.Rows[i]["FromDate"]));
                Assert.That(grid[i].ToDate.Text, Is.EqualTo(table.Rows[i]["ToDate"]));
                Assert.That(grid[i].Branches.Text, Is.EqualTo(table.Rows[i]["Branches"]));
            }
        }

        [Then("the credit threshold is saved")]
        public void CreditThresholdSaved(Table table)
        {
            var grid = this.creditThresholdPage.GetGridById(1);

            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(grid[i].Level.Text, Is.EqualTo(table.Rows[i]["Level"]));
                Assert.That(grid[i].ThresholdAmount.Text, Is.EqualTo(table.Rows[i]["Threshold"]));
                Assert.That(grid[i].Branches.Text, Is.EqualTo(table.Rows[i]["Branches"]));
            }
        }

        [Then("the clean parameter is saved")]
        public void CleanParameterSaved(Table table)
        {
            var grid = this.cleanPage.GetGridById(1);

            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(grid[i].Days.Text, Is.EqualTo(table.Rows[i]["Days"]));
                Assert.That(grid[i].Branches.Text, Is.EqualTo(table.Rows[i]["Branches"]));
            }
        }

        [Then("the seasonal date is updated with id '(.*)'")]
        public void SeasonalDateUpdate(int id, Table table)
        {
            var grid = this.page.GetGridById(id);

            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(grid[i].Description.Text, Is.EqualTo(table.Rows[i]["Description"]));
                Assert.That(grid[i].FromDate.Text, Is.EqualTo(table.Rows[i]["FromDate"]));
                Assert.That(grid[i].ToDate.Text, Is.EqualTo(table.Rows[i]["ToDate"]));
                Assert.That(grid[i].Branches.Text, Is.EqualTo(table.Rows[i]["Branches"]));
            }
        }

        [Then("the credit threshold is updated with id '(.*)'")]
        public void CreditThresholdUpdate(int id, Table table)
        {
            var grid = this.creditThresholdPage.GetGridById(id);

            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(grid[i].Level.Text, Is.EqualTo(table.Rows[i]["Level"]));
                Assert.That(grid[i].ThresholdAmount.Text, Is.EqualTo(table.Rows[i]["Threshold"]));
                Assert.That(grid[i].Branches.Text, Is.EqualTo(table.Rows[i]["Branches"]));
            }
        }

        [Then("the clean parameter is updated with id '(.*)'")]
        public void CleanParameterUpdate(int id, Table table)
        {
            var grid = this.cleanPage.GetGridById(id);

            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(grid[i].Days.Text, Is.EqualTo(table.Rows[i]["Days"]));
                Assert.That(grid[i].Branches.Text, Is.EqualTo(table.Rows[i]["Branches"]));
            }
        }

        [When("I remove the seasonal date")]
        public void RemoveSeasonalDate()
        {
            var grid = this.page.GetGrid(1);

            grid[0].Remove.Click();

            this.page.RemoveConfirmButton.Click();
        }

        [Then("it is removed from the seasonal date grid")]
        public void SeasonalDateHasGoneFromGrid()
        {
            Assert.That(this.page.NoResults.Text, Is.EqualTo("No Seasonal Dates!"));
        }

        [When("I remove the credit threshold")]
        public void RemoveCreditThreshold()
        {
            var grid = this.creditThresholdPage.GetGridById(1);

            grid[0].Remove.Click();

            this.creditThresholdPage.RemoveConfirmButton.Click();
        }

        [Then("it is removed from the credit threshold grid")]
        public void CreditThresholdHasGoneFromGrid()
        {
            Assert.That(this.creditThresholdPage.NoResults.Text, Is.EqualTo("No Credit Thresholds!"));
        }

        [When("I remove the clean parameter")]
        public void RemoveCleanParameter()
        {
            var grid = this.cleanPage.GetGridById(1);

            grid[0].Remove.Click();

            this.cleanPage.Remove.Click();
        }

        [Then("it is removed from the clean parameter grid")]
        public void CleanParameterHasGoneFromGrid()
        {
            Assert.That(this.cleanPage.NoResults.Text, Is.EqualTo("No Clean Preferences!"));
        }
    }
}