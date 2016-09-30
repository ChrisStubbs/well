@WebDriverFeature
@RoleSuperUser
Feature: Admininistration Parameters
	In order to parameterise the well
	As a user
	I want to be able to set seasonal dates so that clean deliveries take these dates into account when getting cleared from the well
	And I want to be able to set credit threshold per branch
	And I want to be able to set the time clean deliveries are cleaned from the well

Scenario: Seasonal dates add new
	Given I have a clean database
	And I navigate to the branch parameters page
	When I add a seasonal date
	And all branches are selected for the seasonal date
	And I save the seasonal date
	Then the seasonal date is saved
	And I navigate to the branch parameters page
	And the seasonal date is saved