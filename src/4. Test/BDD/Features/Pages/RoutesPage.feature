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
	And I have selected branch '22'
	When I open the routes page
	Then The following routes will be displayed
	| Route | Branch | RouteDate   | Driver         | NoOfDrops | Exceptions | Clean | Status |
	| 001   | 22     | Jul 1, 2016 | HALL IAN       | 2         | 3          | 1     |   Not Started     |
	| 006   | 22     | Jul 1, 2016 | RENTON MARK    | 2         | 0          | 4     |   Not Started     |
	| 011   | 22     | Jul 1, 2016 | DUGDALE STEVEN | 4         | 0          | 9     |   Not Started     |
	
	

Scenario: A user can filter Route information
	Given I have a clean database
	And I have loaded the MultiDate Adam route data
	And  All the deliveries are marked as clean
	And  3 deliveries have been marked as exceptions
	And I have selected branches '22' and '2'
	When I open the routes page
	And I filter the grid with the option 'Route' and value '011'
	Then The following routes will be displayed
	| Route | Branch | RouteDate   | Driver         | NoOfDrops | Exceptions | Clean | Status |
	| 011   | 22     | Jul 1, 2016 | DUGDALE STEVEN | 4         | 0          | 9     |   Not Started     |
	| 011   | 2      | Jul 1, 2016 | DUGDALE STEVEN | 4         | 0          | 9     |   Not Started     |
	When I clear the filter 
	Then The following routes will be displayed
	| Route | Branch | RouteDate   | Driver         | NoOfDrops | Exceptions | Clean | Status      |
	| 001   | 22     | Aug 1, 2016 | HALL IAN       | 2         | 3          | 1     | Not Started |
	| 011   | 22     | Jul 1, 2016 | DUGDALE STEVEN | 4         | 0          | 9     | Not Started |
	| 011   | 2      | Jul 1, 2016 | DUGDALE STEVEN | 4         | 0          | 9     | Not Started |
	| 006   | 22     | Jun 1, 2016 | RENTON MARK    | 2         | 0          | 4     | Not Started |
	When I filter the grid with the option 'Branch' and value '2'
	Then The following routes will be displayed
    | Route | Branch | RouteDate   | Driver         | NoOfDrops | Exceptions | Clean | Status |
    | 011   | 2      | Jul 1, 2016 | DUGDALE STEVEN | 4         | 0          | 9     | Not Started       |
	When I select 'Route' from the filter options
	Then the the previous filter should be cleared 
	And The following routes will be displayed
	| Route | Branch | RouteDate   | Driver         | NoOfDrops | Exceptions | Clean | Status      |
	| 001   | 22     | Aug 1, 2016 | HALL IAN       | 2         | 3          | 1     | Not Started |
	| 011   | 22     | Jul 1, 2016 | DUGDALE STEVEN | 4         | 0          | 9     | Not Started |
	| 011   | 2      | Jul 1, 2016 | DUGDALE STEVEN | 4         | 0          | 9     | Not Started |
	| 006   | 22     | Jun 1, 2016 | RENTON MARK    | 2         | 0          | 4     | Not Started |

Scenario: A user can view Route information and sort on route date
	Given I have a clean database
	And I have loaded the MultiDate Adam route data
	And I have selected branch '22'
	When I open the routes page
	Then The following routes will be displayed
	| Route | Branch | RouteDate   | Driver         | NoOfDrops | Exceptions | Clean | Status      |
	| 001   | 22     | Aug 1, 2016 | HALL IAN       | 2         | 0          | 0     | Not Started |
	| 011   | 22     | Jul 1, 2016 | DUGDALE STEVEN | 4         | 0          | 0     | Not Started |
	| 006   | 22     | Jun 1, 2016 | RENTON MARK    | 2         | 0          | 0     | Not Started |
	When I click on the orderby Triangle image
	Then The following routes ordered by date will be displayed in 'asc' order
	| Route | Branch | Route Date  | Driver         | NoOfDrops | Exceptions | Clean | Status      |
	| 006   | 22     | Jun 1, 2016 | RENTON MARK    | 2         | 0          | 0     | Not Started |
	| 011   | 22     | Jul 1, 2016 | DUGDALE STEVEN | 4         | 0          | 0     | Not Started |
	| 001   | 22     | Aug 1, 2016 | HALL IAN       | 2         | 0          | 0     | Not Started | 
	When I click on the orderby Triangle image
	Then The following routes ordered by date will be displayed in 'desc' order
	| Route | Branch | Route Date  | Driver         | NoOfDrops | Exceptions | Clean | Status      |
	| 001   | 22     | Aug 1, 2016 | HALL IAN       | 2         | 0          | 0     | Not Started |
	| 011   | 22     | Jul 1, 2016 | DUGDALE STEVEN | 4         | 0          | 0     | Not Started |
	| 006   | 22     | Jun 1, 2016 | RENTON MARK    | 2         | 0          | 0     | Not Started | 


#TODO Add more filter scenarios when the additional filters have been implemented

Scenario: A user can page through Route information
	Given I have a clean database
	And I have loaded the Adam route data that has 21 lines
	And I have selected branch '2'
	When I open the routes page
	Then '10' rows of data will be displayed
	And I will have 3 pages of data
	When I click on page '2'
	Then '10' rows of data will be displayed
	When I click on page '3'
	Then '1' rows of data will be displayed
	When I click on page '1'
	Then '10' rows of data will be displayed
	When I click on page '3' 
	And I select the first row of the route
	And I choose to view that routes clean deliveries
	Then the clean deliveries page will be opened
	When I click the back button
	Then the routes page will be displayed
	And '1' rows of data will be displayed


 
Scenario: A user can drill into a Route to view exceptions
	Given I have a clean database
	And I have loaded the Adam route data
	And All the deliveries are marked as exceptions
	And I have selected branch '22'
	When I open the routes page
	And I select the first row of the route    
	And I choose to view that routes exceptions
	Then I can see that routes exceptions
	And the filter should be preset to route and route number

 
Scenario: A user can drill into a Route to view clean deliveries
	Given I have a clean database
	And I have loaded the Adam route data
	And All the deliveries are marked as clean
	And I have selected branch '22'
	When I open the routes page
	And I select the first row of the route
	And I choose to view that routes clean deliveries
	Then I can see that routes clean deliveries
	And the filter should be preset to route and route number

