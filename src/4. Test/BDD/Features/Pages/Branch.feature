@WebDriverFeature
@RoleSuperUser
Feature: Branch
	In order to view the branch deliveries
	As a well user
	I want to be able to determine what branches I can see

Scenario: A user sets all branches to themselves
	Given I have a clean database
	When I navigate to the branches page
	And I select all the branches
	And I save my branches
    And select branches selection
    Then all the branches are selected

Scenario: A user sets medway and birtley as their branches
	Given I have a clean database
	When I navigate to the branches page
	And I select branch 'Birtley' 
	And I select branch 'Medway'
	And I save my branches
	And select branches selection
	Then branch is selected Birtley
	And branch is selected Medway
	And branch is not selected Coventry
	And branch is not selected Fareham
	And branch is not selected Dunfermline
	And branch is not selected Leeds
	And branch is not selected Hemel
	And branch is not selected Belfast
	And branch is not selected Brandon
	And branch is not selected Plymouth
	And branch is not selected Bristol
	And branch is not selected Haydock

Scenario: A user sets up branches on another users behalf
	Given I have a clean database
	When I navigate to the user preferences page
	And I search for user pond
	Then the user Fiona Pond is returned in the search results
	When I select the row for Fiona Pond
	And I select Yes on the popup user preference modal
	Then I select all the branches
	And I save my branches
	When I navigate to the user preferences page
	And I search for user pond
	Then the user Fiona Pond is returned in the search results
	When I select the row for Fiona Pond
	And I select Yes on the popup user preference modal
	Then all the branches are selected