@UITests
Feature: Smoke
	In order to check the app is running
	As a user
	I want to see the homepage

@SomeUserRole
Scenario: User can load homepage
	When I view the homepage
	Then I can see the message "Hello Well!"
