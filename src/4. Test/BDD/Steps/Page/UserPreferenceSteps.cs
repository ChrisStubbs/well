namespace PH.Well.BDD.Steps.Page
{
    using System.Linq;
    using System.Threading;

    using NUnit.Framework;

    using PH.Well.BDD.Pages;

    using TechTalk.SpecFlow;
    using PH.Well.Domain.Enums;


    [Binding]
    public class UserPreferenceSteps
    {
        private UserPreferencesPage userPreferencesPage => new UserPreferencesPage();
        private UserCreditThresholdPage userCreditThresholdPage => new UserCreditThresholdPage();

        [When("I navigate to the user preferences page")]
        public void NavigateToUserPreferences()
        {
            this.userPreferencesPage.Open();
            this.userPreferencesPage.UserPreferencesDropDown.Select();
        }


        [When("I search for user (.*)")]
        public void SearchForUser(string user)
        {
            Thread.Sleep(1000);
            this.userPreferencesPage.FindBox.EnterText(user);
            this.userPreferencesPage.FindButton.Click();
        }

        [Then("the user (.*) is returned in the search results")]
        public void TheUserIsReturnedInTheGrid(string username)
        {
            var rows = this.userPreferencesPage.Grid.ReturnAllRows().ToList();

            bool userExistsInGrid = false;

            foreach (var row in rows)
            {
                var name = row.GetColumnValueByIndex((int)UserPreferenceGrid.Name);

                if (name == username)
                {
                    userExistsInGrid = true;
                    break;
                }
            }

            Assert.IsTrue(userExistsInGrid);
        }

        [When("I select the row for (.*)")]
        public void SelectRowForGivenUser(string username)
        {
            var rows = this.userPreferencesPage.Grid.ReturnAllRows().ToList();

            foreach (var row in rows)
            {
                var name = row.GetColumnValueByIndex((int)UserPreferenceGrid.Name);

                if (name == username)
                {
                    row.Click();
                    break;
                }
            }
        }

        [When("I select Yes on the popup user preference modal")]
        public void ClickYesOnModal()
        {
            this.userPreferencesPage.ModalYesButton.Click();
        }

        [When(@"I search for the current user")]
        public void WhenISearchForTheCurrentUser()
        {
            var username = this.userCreditThresholdPage.Username.GetElement().Text;
            this.userCreditThresholdPage.SearchTextBox.EnterText(username.Substring(0, username.IndexOf(" ")));
            this.userCreditThresholdPage.FindButton.Click();

        }

        [When(@"I select the current user from the results")]
        public void WhenISelectTheCurrentUserFromTheResults()
        {
            var username = this.userCreditThresholdPage.Username.GetElement().Text;
            var rows = this.userPreferencesPage.Grid.ReturnAllRows().ToList();

            foreach (var row in rows)
            {
                var name = row.GetColumnValueByIndex((int)UserPreferenceGrid.Name);

                if (name == username)
                {
                    row.Click();
                    break;
                }
            }

            this.userPreferencesPage.ModalPreferenceYesButton.Click();

        }

        [When(@"I select Level '(.*)' from the dropdown list")]
        public void WhenISelectFromTheDropdownList(string level)
        {
            var thresholdLevel = (ThresholdLevel) int.Parse(level);
            this.userPreferencesPage.CreditThresholdDropDown.SelectThresholdLevel(thresholdLevel);

        }

        [When(@"save the user threshold level")]
        public void WhenSaveTheUserThresholdLevel()
        {
            this.userCreditThresholdPage.SaveButton.Click();
        }

        [Then(@"the threshold level is saved")]
        public void ThenTheThresholdLevelIsSaved()
        {
            var text = this.userPreferencesPage.ToasterSucess.GetElement().Text;
            StringAssert.Contains("Threshold level has been saved", text);
        }


    }
}