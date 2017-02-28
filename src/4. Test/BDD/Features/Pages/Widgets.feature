@WebDriverFeature
@RoleSuperUser
Feature: Widgets
	In order to prioritise my work
	As a customer service user
	I want to see an overview of deliveries in the Well

Scenario: User stats are shown on widgets
	Given I have a clean database
	And I have loaded the Adam route data
	And I have imported a valid Epod update file named 'ePOD_30062016_Update.xml'
	And I have the following credit thresholds setup for all branches
	| Level | Threshold |
	| 1     | 200       |
	| 2     | 100       |
	| 3     | 10        |
	And I have selected branch '22'
	And 3 deliveries have been marked as exceptions
	And I am assigned to credit threshold 'Level 3'
	And 1 deliveries are waiting credit approval
	And I open the exception deliveries
	And I assign the delivery to myself
	And 1 notifications have been made 
	When I view the widgets page
	Then there are the following widget stats
	| Unsubmitted exceptions | Unapproved exceptions | Unsubmitted assigned | Unapproved assigned | Unsubmitted outstanding | Unapproved outstanding | Notifications |
	| 2                      | 1                     | 1                    | 0                   | 2                       | 1                      | 1             |