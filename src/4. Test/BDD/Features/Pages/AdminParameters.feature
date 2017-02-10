@WebDriverFeature
@RoleSuperUser
Feature: Administration Parameters
	In order to parameterise the well
	As a user
	I want to be able to set seasonal dates so that clean deliveries take these dates into account when getting cleared from the well
	And I want to be able to set credit threshold per branch
	And I want to be able to set the time clean deliveries are cleaned from the well
	And I want to be able to set widget warning levels per branch

#TODO BDD required around royalty exceptions when cleaning the Well

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
    | Description | FromDate   | ToDate     |
    |   test      | 29/02/2016 | 30/03/2016 |
	And all branches are selected for the seasonal date
	And the seasonal dates page is closed
	Then the seasonal dates are not saved


Scenario: Credit threshold Add, Edit, and Remove
	Given I have a clean database
	When I navigate to the branch parameters page
	And I select the credit threshold tab
	And I add a credit threshold
	| Level  | Threshold |
	| 1      | 1000      |
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
	| Level   | Threshold | Branches                                                   |
	| Level 1 | 2000      | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay | 
	When I remove the credit threshold
	Then it is removed from the credit threshold grid

Scenario:  Credit threshold negative inputs
	Given I have a clean database
	When I navigate to the branch parameters page
	And I select the credit threshold tab
	And I open the credit threshold input
	And I save the credit threshold
	Then warnings appear on the credit threshold page
	| Error                        |
	| Threshold level is required! |
	| Threshold is required!       |
	| Branch is required!          |
	When I change the credit threshold
    | Level | Threshold |
    | 1     | aaa       |
	And I save the credit threshold
	Then warnings appear on the credit threshold page
	| Error                        |
	| Threshold is required!       |
	| Branch is required!          |
	When I change the credit threshold
    | Level | Threshold |
    | 1     | -1        |
	And I save the credit threshold
	Then warnings appear on the credit threshold page
	| Error                             |
	| Threshold range is 1 to 1000000   |
	| Branch is required!               |
	When I change the credit threshold
    | Level | Threshold |
    | 1     | 0         |
	And I save the credit threshold
	Then warnings appear on the credit threshold page
	| Error                              |
	| Threshold range is 1 to 1000000    |
	| Branch is required!                |
	When I change the credit threshold
    | Level | Threshold |
    | 1     | 10000001  |
	And I save the credit threshold
	Then warnings appear on the credit threshold page
	| Error                              |
	| Threshold range is 1 to 1000000    |
	| Branch is required!                |
	When I change the credit threshold
    | Level | Threshold |
    | 1     | 10        |
	And I save the credit threshold
	Then warnings appear on the credit threshold page
	| Error                              |
	| Branch is required!                |

Scenario: Credit threshold applied all levels
	Given I have a clean database
	And I have loaded the Adam route data
	And I have imported a valid Epod update file named 'ePOD_30062016_Update.xml'
	And  3 deliveries have been marked as exceptions
	#need 3 deliveries with different credit levels
	When I navigate to the branch parameters page
	And I select the credit threshold tab
	And I add a credit threshold
	| Level | Threshold |
	| 1     | 1000      |
	And all branches are selected for the credit threshold
	And I save the credit threshold
	Then the credit threshold is saved
	| Level   | Threshold | Branches                                                   |
	| Level 1 | 1000      | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When I add a credit threshold
	| Level | Threshold |
	| 2     | 100       |
	And all branches are selected for the credit threshold
	And I save the credit threshold
	Then the credit threshold is saved
	| Level   | Threshold | Branches                                                   |
	| Level 1 | 1000      | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	| Level 2 | 100       | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When I add a credit threshold
	| Level | Threshold |
	| 3     | 10      |
	And all branches are selected for the credit threshold
	And I save the credit threshold
	Then the credit threshold is saved
	| Level   | Threshold | Branches                                                   |
	| Level 1 | 1000      | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	| Level 2 | 100       | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	| Level 3 | 10        | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When I navigate to the branches page
	And I select all the branches
	And I save the branch selection
	Then all the branches are saved
	#When I navigate to the user threshold levels page
	#And I search for the current user
	#And I select the current user from the results
	#And I select Level '2' from the dropdown list
	#And save the user threshold level
	#Then the threshold level is saved   
	#When I open the exception deliveries
	#When I assign the delivery on row 1 to myself
	#Then Only the execption within the threshold tolerance will be actionable
	#When I navigate to user threshold levels
	#And I search for the current user
	#And I select the current user from the results
	#And I select Level2 from the dropdown list
	#And save the user threshold level
	#Then the threshold level is saved
	#When I open the exception deliveries
	#Then Only the execption within the threshold tolerance will be actionable
	#When I navigate to user threshold levels
	#And I search for the current user
	#And I select the current user from the results
	#And I select Level3 from the dropdown list
	#And save the user threshold level
	#Then the threshold level is saved
	#When I open the exception deliveries
	#Then Only the execption within the threshold tolerance will be actionable


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






