@WebDriverFeature
@RoleSuperUser
Feature: Admininistration Parameters
	In order to parameterise the well
	As a user
	I want to be able to set seasonal dates so that clean deliveries take these dates into account when getting cleared from the well
	And I want to be able to set credit threshold per branch
	And I want to be able to set the time clean deliveries are cleaned from the well
	And I want to be able to set widget warning levels per branch

Scenario: Seasonal dates applied all branches
#Add, edit
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'
	And All the deliveries are marked as clean 
	And The clean deliveries are '-2' days old 
	And I navigate to the branch parameters page 
	When I add a seasonal date
	| Description | FromDate | ToDate |
	| New Year    | -5       | 0      |
	And all branches are selected for the seasonal date
	And I save the seasonal date
	Then the seasonal date is saved
	| Description | FromDate | ToDate | Branches                                                   |
	| New Year    | -5       | 0      | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When The clean task runs
	And I open the clean deliveries
	Then '10' rows of clean delivery data will be displayed
	When I navigate to the branch parameters page
	And I edit a seasonal date
	| Description | FromDate | ToDate |
	| New Year    | -5       | -2      |
	And I update the seasonal date
	Then the seasonal date is updated with id '2'
	| Description | FromDate | ToDate | Branches                                                  |
    |New Year     | -5       | -2      | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay|
	When The clean task runs
	And I open the clean deliveries
	Then No clean deliveries will be displayed

Scenario: Seasonal dates applied one branch
#Add, remove
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branches '22' and '2' 
	And All the deliveries are marked as clean
	And '2' clean deliveries are updated to branch '2' 
	And The clean deliveries are '-3' days old 
	When I open the clean deliveries
	Then '10' rows of clean delivery data will be displayed
	When I navigate to the branch parameters page 
	And I add a seasonal date
	| Description | FromDate | ToDate |
	| New Year    | -5       | 0      | 
	And 'Medway' is selected for the seasonal date
	And I save the seasonal date
	Then the seasonal date is saved
	| Description | FromDate | ToDate | Branches |
	| New Year    | -5       | 0      | med      |
	When The clean task runs
	And I open the clean deliveries
	Then At least '1' rows of clean delivery data will be displayed
	When I navigate to the branches page
	And I deselect branch 'Medway'
	And I save my branches
	And I open the clean deliveries
	Then No clean deliveries will be displayed 
	When I navigate to the branches page
	And I deselect branch 'Medway'
	And I select branch 'Birtley'
	And I save my branches
	And I open the clean deliveries
	Then At least '1' rows of clean delivery data will be displayed
	When I navigate to the branch parameters page
	And I select the seasonal dates tab
	And I remove the seasonal date
	Then it is removed from the seasonal date grid
	When The clean task runs
	And I open the clean deliveries
	Then No clean deliveries will be displayed


Scenario: Credit threshold add new
	Given I have a clean database
	And I navigate to the branch parameters page
	When I add a credit threshold
	| Level  | Threshold |
	| Level1 | 1000      |
	And all branches are selected for the credit threshold
	And I save the credit threshold
	Then the credit threshold is saved
	| Level   | Threshold | Branches                                                   |
	| Level 1 | 1000      | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	And I navigate to the branch parameters page
	When I select the credit threshold tab
	Then the credit threshold is saved
	| Level   | Threshold | Branches                                                   |
	| Level 1 | 1000      | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |

Scenario: Credit threshold remove
	Given I have a clean database
	And I navigate to the branch parameters page
	When I add a credit threshold
	| Level  | Threshold |
	| Level1 | 1000      |
	And all branches are selected for the seasonal date
	And I save the credit threshold
	Then the credit threshold is saved
	| Level   | Threshold | Branches                                                   |
	| Level 1 | 1000      | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When I remove the credit threshold
	Then it is removed from the credit threshold grid

Scenario: Credit threshold edit
	Given I have a clean database
	And I navigate to the branch parameters page
	When I add a credit threshold
	| Level  | Threshold |
	| Level1 | 1000      |
	And all branches are selected for the seasonal date
	And I save the credit threshold
	Then the credit threshold is saved
	| Level   | Threshold | Branches                                                   |
	| Level 1 | 1000      | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When I edit a credit threshold
	| Threshold |
	| 2000      |
	And I update the credit threshold
	Then the credit threshold is updated with id '2'
	| Level  | Threshold | Branches                                                   |
	| Level 1 | 2000      | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |


Scenario: Clean parameters applied all branches
#Add, edit
 	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'
	And All the deliveries are marked as clean 
	And The clean deliveries are '-2' days old 
	When I open the clean deliveries
	Then '10' rows of clean delivery data will be displayed
	When  I navigate to the branch parameters page   
	And I add a clean parameter
	| Days |
	| 3    |
	And all branches are selected for the clean parameter
	And I save the clean parameter
	Then the clean parameter is saved
	| Days | Branches                                                   |
	| 3    | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When The clean task runs
	And I open the clean deliveries
	Then '10' rows of clean delivery data will be displayed
	When I navigate to the branch parameters page
	And I select the clean parameter tab
	And I edit a clean parameter
	| Days | 
	| 2    |
	And I update the clean parameter
	Then the clean parameter is updated with id '2'
	| Days | Branches                                                   |
	| 2    | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When The clean task runs
	And I open the clean deliveries
	Then the clean deliveries are removed from the well

Scenario: Clean parameters applied one branch
#Add, remove
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branches '2' and '22'
	And All the deliveries are marked as clean 
	And The clean deliveries are '-2' days old 
	And '2' clean deliveries are updated to branch '2'
	When I open the clean deliveries
	Then '10' rows of clean delivery data will be displayed
	When  I navigate to the branch parameters page   
	And I add a clean parameter
	| Days |
	| 3    |
	And 'Medway' is selected for the clean parameter
	And I save the clean parameter
	Then the clean parameter is saved
	| Days | Branches |
	| 3    | med      |
	When The clean task runs
	And I navigate to the branches page
	And I deselect branch 'Medway'
	And I save my branches 
	And I open the clean deliveries 
	Then No clean deliveries will be displayed
	When I navigate to the branches page
	And I select branch 'Medway'
	And I deselect branch 'Birtley'
	And I save my branches
	And I open the clean deliveries
	Then At least '1' rows of clean delivery data will be displayed
	When I navigate to the branch parameters page
	And I select the clean parameter tab
	And I remove the clean parameter
	Then it is removed from the clean parameter grid
	When The clean task runs
	And I open the clean deliveries
	Then No clean deliveries will be displayed

Scenario: Clean parameter negative inputs 
	Given I have a clean database
	And I navigate to the branch parameters page
	When I select the clean parameter tab
	And I click the add parameter button
	And I save the clean parameter
	Then warnings appear on the clean input page 
	| Error               |
	| Days is required!   |
	| Branch is required! |
	When I update clean parameter values
	| Days |
	| 0    |
	And I save the clean parameter
	Then warnings appear on the clean input page 
	| Error					  |
    | Days range is 1 to 100! |
	| Branch is required!     |
	When I update clean parameter values
	| Days |
	| 101  |
	And I save the clean parameter
	Then warnings appear on the clean input page 
	| Error					  |
    | Days range is 1 to 100! |
	| Branch is required!     |
	When I update clean parameter values
	| Days |
	| -1   |
	And I save the clean parameter
	Then warnings appear on the clean input page 
	| Error					  |
    | Days range is 1 to 100! |
	| Branch is required!     |
	When I update clean parameter values
	| Days |
	| abc  |
	And I save the clean parameter
	Then warnings appear on the clean input page 
	| Error					 |
    | Days is required!      |
	| Branch is required!    |
	When I update clean parameter values
	| Days |
	| 1    |
	And I save the clean parameter
	Then warnings appear on the clean input page 
	| Error					 |
	| Branch is required!    |
	When I update clean parameter values
	| Days |
	| 100  |
	And I save the clean parameter
	Then warnings appear on the clean input page 
	| Error					 |
	| Branch is required!    |
	When I select all the branches
	And I click the Close button
	Then the clean parameter is not saved


Scenario: Seasonal dates negative inputs
	Given I have a clean database
	And I navigate to the branch parameters page
	When I open the seasonal date input
	And I save the seasonal date
	Then warnings appear in the seasonal input page
	| Error					   |
	| Description is required! |
	| From date is required!   |
	| To date is required!     |
	| Select a branch!         |
	When I change the seasonal date
    | Description | FromDate | ToDate |
    |   test      | aaa		 | aaa    |
	And I save the seasonal date
	Then warnings appear in the seasonal input page
	| Error					         |
	| From date is not a valid date! |
	| To date is not a valid date!   |
	| Select a branch!               |
	When I change the seasonal date
    | Description | FromDate | ToDate   |
    |   test      | 01012016 | 01012016 |
	And I save the seasonal date
	Then warnings appear in the seasonal input page
	| Error					         |
	| From date is not a valid date! |
	| To date is not a valid date!   |
	| Select a branch!               |
	When I change the seasonal date
    | Description | FromDate   | ToDate     |
    |   test      | 2016/01/31 | 2016/31/01 |
	And I save the seasonal date
	Then warnings appear in the seasonal input page
	| Error					         |
	| To date is not a valid date!   |
	| Select a branch!               |
	When I change the seasonal date
    | Description | FromDate   | ToDate   |
    |   test      | 29/02/2016 | -1       |
	And I save the seasonal date
	Then warnings appear in the seasonal input page
	| Error					         |
	| To date is not a valid date!   |
	| Select a branch!               |
	When I change the seasonal date
    | Description | FromDate   | ToDate     |
    |   test      | 29/02/2016 | 30/03/2016 |
	And all branches are selected for the seasonal date
	And the seasonal dates page is closed
	Then the seasonal dates are not saved
	
	Scenario: Widget warning parameter add new
	Given I have a clean database
	And I navigate to the branch parameters page
	When I add a widget warning parameter
	| Level | Widget     | Description |
	| 5     | Exceptions | 'Test'      |
	And all branches are selected for the widget warning parameter
	And I save the widget warning parameter
	Then the widget warning parameter is saved
	| Level | Widget	 | Branches													 |
	| 5     | Exceptions |med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	And I navigate to the branch parameters page
	When I select the widget warning tab
	Then the widget warning parameter is saved
	| Level | Widget     | Branches													 |
	| 5     | Exceptions |med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |

Scenario: Widget warning parameter remove
	Given I have a clean database
	And I navigate to the branch parameters page
	When I add a widget warning parameter
	| Level | Widget     | Description |
	| 5     | Exceptions | 'Test'      |
	And all branches are selected for the widget warning parameter
	And I save the widget warning parameter
	Then the widget warning parameter is saved
	| Level | Widget	 | Branches													 |
	| 5     | Exceptions |med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When I remove the widget warning parameter
	Then it is removed from the widget warning grid

Scenario: Widget warning parameter edit
	Given I have a clean database
	And I navigate to the branch parameters page
	When I add a widget warning parameter
	| Level | Widget     | Description |
	| 5     | Exceptions | 'Test'      |
	And all branches are selected for the widget warning parameter
	And I save the widget warning parameter
	Then the widget warning parameter is saved
	| Level | Widget	 | Branches													 |
	| 5     | Exceptions |med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When I edit a widget warning parameter
	| Level | Widget	 | Branches													 |
	| 2     | Exceptions |med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	And I update the widget warning parameter
	Then the widget warning parameter is updated with id '2'
	| Level | Branches                                                   |
	| 2     | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |






