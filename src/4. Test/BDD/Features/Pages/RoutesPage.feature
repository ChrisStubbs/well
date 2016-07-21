@WebDriverFeature
Feature: RoutesPage
	As a User
	I wish to be able to view and filter route information
	so that i can determine route progress

Scenario: A user can view Route information
	Given I have a clean database
	And I have loaded the Adam route data
	When I open the routes page
	Then The following routes will be displayed
	| Route | Driver         | NoOfDrops | Exceptions | Clean | Status      | 
	| 001   | HALL IAN       | 2         | 2          | 0     | Not Defined | 
	| 006   | RENTON MARK    | 2         | 2          | 0     | Not Defined | 
	| 011   | DUGDALE STEVEN | 4         | 4          | 0     | Not Defined | 

Scenario: A user can filter Route information
	Given I have a clean database
	And I have loaded the Adam route data
	When I open the routes page
	And I filter the grid with the option 'Route' and value '001'
	Then The following routes will be displayed
	| Route | Driver   | NoOfDrops | Exceptions | Clean | Status      |
	| 001   | HALL IAN | 2         | 2          | 0     | Not Defined |
	When I clear the filter 
	Then The following routes will be displayed
	| Route | Driver         | NoOfDrops | Exceptions | Clean | Status      |
	| 001   | HALL IAN       | 2         | 2          | 0     | Not Defined |
	| 006   | RENTON MARK    | 2         | 2          | 0     | Not Defined |
	| 011   | DUGDALE STEVEN | 4         | 4          | 0     | Not Defined |

#TODO Add more filter scenarios when the additional filters have been implemented

Scenario: A user can page through Route information
	Given I have a clean database
	And I have loaded the Adam route data that has 21 lines
	When I open the routes page
	Then '10' rows of data will be displayed
	And I will have 3 pages of data
	When I click on page 2
	Then '10' rows of data will be displayed
	When I click on page 3
	Then '1' rows of data will be displayed
	When I click on page 1
	Then '10' rows of data will be displayed