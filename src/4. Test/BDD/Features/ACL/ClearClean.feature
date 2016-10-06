Feature: Clearout clean deliveries via a nightly task
	In order to clear the well from clean deliveries
	I want to be able to see the clean deliveries removed from the well
	When the clean delivery has been stored a certain number of configurable days

Scenario: Clean deliveries removed after the default of one day
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22
	And 3 deliveries have been marked as clean
	And The clean deliveries are -1 days old
	When The clean task runs
	Then the clean deliveries are removed from the well
