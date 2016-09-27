@WebDriverFeature
Feature: NotificationsPage
	As a well user
	I wish to be able to view and archive notifications 
	so that I can take action in the ADAM system

@mytag
Scenario: A user can page through notifications
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22 
	And 10 notifications have been made
	When I navigate to the notifications page
	Then I will have 3 pages of notification data
	And '3' rows of notification data will be displayed



