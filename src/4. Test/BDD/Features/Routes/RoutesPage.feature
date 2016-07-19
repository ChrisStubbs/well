@WebDriverFeature
Feature: RoutesPage
	As a User
	I wish to be able to view and filter route information
	so that i can determine route progress

Scenario: A User can view Route information
	Given I have a clean database
	And I have loaded the Adam route data
	When I open the routes page
	Then The following routes will be displayed
	| Route | Driver         | NoOfDrops | Exceptions | Clean | Status      | 
	| 001   | HALL IAN       | 2         | 2          | 0     | Not Defined | 
	| 006   | RENTON MARK    | 2         | 2          | 0     | Not Defined | 
	| 011   | DUGDALE STEVEN | 4         | 4          | 0     | Not Defined | 



