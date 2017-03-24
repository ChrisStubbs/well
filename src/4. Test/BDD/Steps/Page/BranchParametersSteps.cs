using System;
using PH.Well.Domain.Enums;
using PH.Well.BDD.Framework.Extensions;

namespace PH.Well.BDD.Steps.Page
{
    using NUnit.Framework;

    using PH.Well.BDD.Pages;

    using TechTalk.SpecFlow;
    using System.Threading;


    [Binding]
    public class BranchParametersSteps
    {
        private SeasonalDatesPage page => new SeasonalDatesPage();
        private CreditThresholdPage creditThresholdPage => new CreditThresholdPage();
        private BranchPage branchPage => new BranchPage();
        private CleanPreferencePage cleanPage => new CleanPreferencePage();
        private WidgetWarningPage widgetWarningPage => new WidgetWarningPage();

        [Given("I navigate to the branch parameters page")]
        [Then("I navigate to the branch parameters page")]
        [When("I navigate to the branch parameters page")]
        public void NavigateToBranchParameters()
        {
            this.page.Open();
            Thread.Sleep(1000);
            this.page.AdminDropDown.SelectBranchParameters();
        }

        [When("I navigate to the user threshold levels page")]
        public void NavigateToUserThreshold()
        {
            this.creditThresholdPage.AdminDropDown.SelectUserThreshold();

        }


        [When("I add a seasonal date")]
        public void AddSeasonalDate(Table table)
        {
            var fromDate = DateTime.Now.AddDays(int.Parse(table.Rows[0]["FromDate"]));
            var toDate = DateTime.Now.AddDays(int.Parse(table.Rows[0]["ToDate"]));
            this.page.AddButton.Click();
            this.page.Description.EnterText(table.Rows[0]["Description"]);
            this.page.FromDate.Date = fromDate;
            this.page.ToDate.Date = toDate;
        }

        [When("I change the seasonal date")]
        public void ChangeSeasonalDate(Table table)
        {
            this.page.Description.EnterText(table.Rows[0]["Description"]);
            this.page.FromDate.Date = table.Rows[0]["FromDate"].ParseBritishDate();
            this.page.ToDate.Date = table.Rows[0]["ToDate"].ParseBritishDate();
        }

        [When("I add a credit threshold")]
        public void AddCreditThreshold(Table table)
        {
            this.creditThresholdPage.AddButton.Click();

            var thresholdLevel = (ThresholdLevel)int.Parse(table.Rows[0]["Level"]);

            this.creditThresholdPage.dropdown.SelectThresholdLevel(thresholdLevel);
            this.creditThresholdPage.Threshold.EnterText(table.Rows[0]["Threshold"]);
        }

        [When(@"I open the credit threshold input")]
        public void WhenIOpenTheCreditThresholdInput()
        {
            this.creditThresholdPage.AddButton.Click();
        }


        [When(@"I change the credit threshold")]
        public void WhenIChangeTheCreditThreshold(Table table)
        {
            var thresholdLevel = (ThresholdLevel)int.Parse(table.Rows[0]["Level"]);

            this.creditThresholdPage.dropdown.SelectThresholdLevel(thresholdLevel);
            this.creditThresholdPage.Threshold.EnterText(table.Rows[0]["Threshold"]);
        }


        [When("I add a clean parameter")]
        public void AddCleanParameter(Table table)
        {
            this.cleanPage.ClickCleanDeliveriesTab();
            this.cleanPage.Add.Click();
            this.cleanPage.Days.EnterText(table.Rows[0]["Days"]);
        }

        [When(@"I update clean parameter values")]
        public void WhenIUpdateCleanParameterValues(Table table)
        {
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

        [When(@"I select the seasonal dates tab")]
        public void WhenISelectTheSeasonalDatesTab()
        {
            this.cleanPage.ClickSeasonalDatesTab();
        }



        [When("I edit a seasonal date")]
        public void EditSeasonalDate(Table table)
        {
            var grid = this.page.GetGrid(1);

            grid[0].Description.Click();

            var fromDate = DateTime.Now.AddDays(int.Parse(table.Rows[0]["FromDate"]));
            var toDate = DateTime.Now.AddDays(int.Parse(table.Rows[0]["ToDate"]));
            this.page.Description.EnterText(table.Rows[0]["Description"]);
            this.page.FromDate.Date  = fromDate;
            this.page.ToDate.Date = toDate;

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
        [When("all branches are selected for the widget warning parameter")]
        public void AllBranchesForSeasonalDate()
        {
            this.branchPage.SelectAllBranchesCheckbox.Check();
        }

        [When(@"'(.*)' is selected for the clean parameter")]
        [When(@"'(.*)' is selected for the seasonal date")]
        public void OneBranchCleanParameter(string branch)
        {
            this.branchPage.GetCheckBox(branch).Click();
        }


        [When("I save the seasonal date")]
        [When("I update the seasonal date")]
        public void SaveSeasonalDate()
        {
            this.page.SaveButton.Click();
        }

        [When(@"the seasonal dates page is closed")]
        public void SeasonalDatesPageIsClosed()
        {
            this.page.CloseButton.Click();
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

        [When(@"I click the Close button")]
        public void WhenIClickTheCloseButton()
        {
            this.cleanPage.Close.Click();
        }


        [Then("the seasonal date is saved")]
        public void SeasonalDateSaved(Table table)
        {
            var grid = this.page.GetGrid(1);

            for (int i = 0; i < table.RowCount; i++)
            {
                var fromDate = DateTime.Now.AddDays(int.Parse(table.Rows[0]["FromDate"]));
                var toDate = DateTime.Now.AddDays(int.Parse(table.Rows[0]["ToDate"]));
                Assert.That(grid[i].Description.Text, Is.EqualTo(table.Rows[i]["Description"]));
                Assert.That(grid[i].FromDate.Text, Is.EqualTo(fromDate.ToString("dd/MM/yyyy")));
                Assert.That(grid[i].ToDate.Text, Is.EqualTo(toDate.ToString("dd/MM/yyyy")));
                Assert.That(grid[i].Branches.Text, Is.EqualTo(table.Rows[i]["Branches"]));

            }
        }

        [Then("the credit threshold is saved")]
        public void CreditThresholdSaved(Table table)
        {
            Thread.Sleep(1000);

            var grid = this.creditThresholdPage.GetGrid(table.RowCount);

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
                var fromDate = DateTime.Now.AddDays(int.Parse(table.Rows[0]["FromDate"]));
                var toDate = DateTime.Now.AddDays(int.Parse(table.Rows[0]["ToDate"]));
                Assert.That(grid[i].Description.Text, Is.EqualTo(table.Rows[i]["Description"]));
                Assert.That(grid[i].FromDate.Text, Is.EqualTo(fromDate.ToString("dd/MM/yyyy")));
                Assert.That(grid[i].ToDate.Text, Is.EqualTo(toDate.ToString("dd/MM/yyyy")));
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
        [Then("the seasonal dates are not saved")]
        public void SeasonalDateHasGoneFromGrid()
        {
            Assert.That(this.page.NoResults.Text, Is.EqualTo("No Seasonal Dates"));
        }

        [When("I remove the credit threshold")]
        public void RemoveCreditThreshold()
        {
            var grid = this.creditThresholdPage.GetGridById(2);

            grid[0].Remove.Click();

            this.creditThresholdPage.RemoveConfirmButton.Click();
        }

        [Then("it is removed from the credit threshold grid")]
        public void CreditThresholdHasGoneFromGrid()
        {
            Assert.That(this.creditThresholdPage.NoResults.Text, Is.EqualTo("No credit thresholds"));
        }

        [When("I remove the clean parameter")]
        public void RemoveCleanParameter()
        {
            var grid = this.cleanPage.GetGridById(1);

            grid[0].Remove.Click();

            this.cleanPage.Remove.Click();
        }

        [Then("it is removed from the clean parameter grid")]
        [Then("the clean parameter is not saved")]
        public void CleanParameterHasGoneFromGrid()
        {
            Assert.That(this.cleanPage.NoResults.Text, Is.EqualTo("No clean preferences"));
        }

        [When("I add a widget warning parameter")]
        public void AddWidgetWarningParameter(Table table)
        {
            this.widgetWarningPage.ClickWidgetWarningTab();
            this.widgetWarningPage.Add.Click();
            this.widgetWarningPage.Level.EnterText(table.Rows[0]["Level"]);
            this.widgetWarningPage.Description.EnterText(table.Rows[0]["Description"]);
            this.widgetWarningPage.WidgetButtonDropDown.SelectWidgetWarningException();
        }

        [When("I select the widget warning tab")]
        public void SelectWidgetWarningTab()
        {
            this.widgetWarningPage.ClickWidgetWarningTab();
        }

        [When("I save the widget warning parameter")]
        [When("I update the widget warning parameter")]
        public void SaveWidgetWarning()
        {
            this.widgetWarningPage.Save.Click();
        }


        [Then("the widget warning parameter is saved")]
        public void WidgetWarningParameterSaved(Table table)
        {
            var grid = this.widgetWarningPage.GetGridById(1);

            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(grid[i].Level.Text, Is.EqualTo(table.Rows[i]["Level"]));
                Assert.That(grid[i].WidgetType.Text, Is.EqualTo(table.Rows[i]["Widget"]));
                Assert.That(grid[i].Branches.Text, Is.EqualTo(table.Rows[i]["Branches"]));
            }
        }


        [When("I remove the widget warning parameter")]
        public void RemoveWidgetWarningParameter()
        {
            var grid = this.widgetWarningPage.GetGridById(1);

            grid[0].Remove.Click();

            this.widgetWarningPage.Remove.Click();
        }

        [Then("it is removed from the widget warning grid")]
        public void WidgetWarningParameterHasGoneFromGrid()
        {
            Assert.That(this.widgetWarningPage.NoResults.Text, Is.EqualTo("No Widget Warnings"));
        }

        [When("I edit a widget warning parameter")]
        public void EditWidgetWarningParameter(Table table)
        {
            var grid = this.widgetWarningPage.GetGridById(1);

            grid[0].Level.Click();

            this.widgetWarningPage.Level.EnterText(table.Rows[0]["Level"]);
        }

        [Then("the widget warning parameter is updated with id '(.*)'")]
        public void WidgetWarningParameterUpdate(int id, Table table)
        {
            var grid = this.widgetWarningPage.GetGridById(id);

            for (int i = 0; i < table.RowCount; i++)
            {
                Assert.That(grid[i].Level.Text, Is.EqualTo(table.Rows[i]["Level"]));
                Assert.That(grid[i].Branches.Text, Is.EqualTo(table.Rows[i]["Branches"]));
            }
        }

        [When(@"I open the seasonal date input")]
        public void OpenTheSeasonalDateInput()
        {
            this.page.AddButton.Click();
        }

        [When(@"I click the add parameter button")]
        public void WhenIClickTheAddButton()
        {
            this.cleanPage.Add.Click();
        }

        [Then(@"warnings appear on the clean input page")]
        public void ThenWarningsAppearOnTheCleanInputPage(Table table)
        {
            Thread.Sleep(100);
            var errors = this.cleanPage.GetErrors();

            foreach (var row in table.Rows)
            {
                Assert.That(errors.Contains(row["Error"]));
            }
        }

        [Then(@"warnings appear in the seasonal input page")]
        public void ThenWarningsAppearInTheSeasonalInputPage(Table table)
        {
            var errors = this.page.GetErrors();

            foreach (var row in table.Rows)
            {
                Assert.That(errors.Contains(row["Error"]));
            }
            errors.Clear();

        }

        [Then(@"warnings appear on the credit threshold page")]
        public void ThenWarningsAppearOnTheCreditThresholdPage(Table table)
        {
            var errors = this.creditThresholdPage.GetErrors();

            foreach (var row in table.Rows)
            {
                Assert.That(errors.Contains(row["Error"]));
            }
            errors.Clear();
        }


    }
}