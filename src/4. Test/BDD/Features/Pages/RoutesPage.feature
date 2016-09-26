@WebDriverFeature
@RoleSuperUser
Feature: RoutesPage
	As a User
	I wish to be able to view and filter route information
	so that i can determine route progress

Scenario: A user can view Route information
	Given I have a clean database
	And I have loaded the Adam route data
	And  All the deliveries are marked as clean
	And  3 deliveries have been marked as exceptions
	And I have selected branch 22
	When I open the routes page
	Then The following routes will be displayed
	| Route | Driver         | NoOfDrops | Exceptions | Clean | Status      | 
	| 011   | DUGDALE STEVEN | 4         | 0          | 9     | Not Defined |
	| 006   | RENTON MARK    | 2         | 0          | 4     | Not Defined |
	| 001   | HALL IAN       | 2         | 3          | 1     | Not Defined |

Scenario: A user can filter Route information
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22
	When I open the routes page
	And I filter the grid with the option 'Route' and value '001'
	Then The following routes will be displayed
	| Route | Driver   | NoOfDrops | Exceptions | Clean | Status      |
	| 001   | HALL IAN | 2         | 0          | 0     | Not Defined |
	When I clear the filter 
	Then The following routes will be displayed
	| Route | Driver         | NoOfDrops | Exceptions | Clean | Status      |
	| 011   | DUGDALE STEVEN | 4         | 0          | 0     | Not Defined |
	| 006   | RENTON MARK    | 2         | 0          | 0     | Not Defined |
	| 001   | HALL IAN       | 2         | 0          | 0     | Not Defined |

Scenario: A user can view Route information and sort on updated date
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22
	When I open the routes page
	Then The following routes will be displayed
	| Route | Driver         | NoOfDrops | Exceptions | Clean | Status      | LastUpdatedDateTime     |
	| 011   | DUGDALE STEVEN | 4         | 0          | 0     | Not Defined | Sep 8, 2016, 1:27:17 PM |
	| 006   | RENTON MARK    | 2         | 0          | 0     | Not Defined | Sep 8, 2016, 1:27:17 PM |
	| 001   | HALL IAN       | 2         | 0          | 0     | Not Defined | Sep 8, 2016, 1:27:16 PM |
	When I click on the orderby Triangle image
	Then The following routes ordered by date will be displayed in 'desc' order
	| Route | Driver         | NoOfDrops | Exceptions | Clean | Status      | LastUpdatedDateTime     |
	| 001   | HALL IAN       | 2         | 0          | 0     | Not Defined | Sep 8, 2016, 1:27:16 PM |
	| 006   | RENTON MARK    | 2         | 0          | 0     | Not Defined | Sep 8, 2016, 1:27:17 PM |
	| 011   | DUGDALE STEVEN | 4         | 0          | 0     | Not Defined | Sep 8, 2016, 1:27:17 PM |
	When I click on the orderby Triangle image
	Then The following routes ordered by date will be displayed in 'asc' order
	| Route | Driver         | NoOfDrops | Exceptions | Clean | Status      | LastUpdatedDateTime     |
	| 011   | DUGDALE STEVEN | 4         | 0          | 0     | Not Defined | Sep 8, 2016, 1:27:17 PM |
	| 006   | RENTON MARK    | 2         | 0          | 0     | Not Defined | Sep 8, 2016, 1:27:17 PM |
	| 001   | HALL IAN       | 2         | 0          | 0     | Not Defined | Sep 8, 2016, 1:27:16 PM |

#TODO Add more filter scenarios when the additional filters have been implemented

Scenario: A user can page through Route information
	Given I have a clean database
	And I have loaded the Adam route data that has 21 lines
	And I have selected branch 22
	When I open the routes page
	Then '10' rows of data will be displayed
	And I will have 3 pages of data
	When I click on page 2
	Then '10' rows of data will be displayed
	When I click on page 3
	Then '1' rows of data will be displayed
	When I click on page 1
	Then '10' rows of data will be displayed

Scenario: A user can drill into a Route to view exceptions
	Given I have a clean database
	And I have loaded the Adam route data
	And All the deliveries are marked as exceptions
	And I have selected branch 22
	When I open the routes page
	And I select the first row of the route
	And I choose to view that routes exceptions
	Then I can see that routes exceptions
	And the filter should be preset to route and route number

Scenario: A user can drill into a Route to view clean deliveries
	Given I have a clean database
	And I have loaded the Adam route data
	And All the deliveries are marked as clean
	And I have selected branch 22
	When I open the routes page
	And I select the first row of the route
	And I choose to view that routes clean deliveries
	Then I can see that routes clean deliveries
	And the filter should be preset to route and route number