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
	And I have selected branch 22
	And  3 deliveries have been marked as exceptions
	And I open the exception deliveries
	And I select the assigned link on the first row
	And I assign the delivery to myself
	When I view the widgets page
	Then there are 3 exceptions, 1 assigned, 3 outstanding and 0 notifications