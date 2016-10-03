@WebDriverFeature
@RoleSuperUser
Feature: Admininistration Parameters
	In order to parameterise the well
	As a user
	I want to be able to set seasonal dates so that clean deliveries take these dates into account when getting cleared from the well
	And I want to be able to set credit threshold per branch
	And I want to be able to set the time clean deliveries are cleaned from the well

Scenario: Seasonal dates add new
	Given I have a clean database
	And I navigate to the branch parameters page
	When I add a seasonal date
	| Description | FromDate   | ToDate     |
	| New Year    | 24/12/2016 | 04/01/2017 |
	And all branches are selected for the seasonal date
	And I save the seasonal date
	Then the seasonal date is saved
	| Description | FromDate   | ToDate     | Branches                                                   |
	| New Year    | 24/12/2016 | 04/01/2017 | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	And I navigate to the branch parameters page
	And the seasonal date is saved
	| Description | FromDate   | ToDate     | Branches                                                   |
	| New Year    | 24/12/2016 | 04/01/2017 | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |

Scenario: Seasonal dates remove
	Given I have a clean database
	And I navigate to the branch parameters page
	When I add a seasonal date
	| Description | FromDate   | ToDate     |
	| New Year    | 24/12/2016 | 04/01/2017 |
	And all branches are selected for the seasonal date
	And I save the seasonal date
	Then the seasonal date is saved
	| Description | FromDate   | ToDate     | Branches                                                   |
	| New Year    | 24/12/2016 | 04/01/2017 | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When I remove the seasonal date
	Then it is removed from the seasonal date grid

Scenario: Seasonal dates edit
	Given I have a clean database
	And I navigate to the branch parameters page
	When I add a seasonal date
	| Description | FromDate   | ToDate     |
	| New Year    | 24/12/2016 | 04/01/2017 |
	And all branches are selected for the seasonal date
	And I save the seasonal date
	Then the seasonal date is saved
	| Description | FromDate   | ToDate     | Branches                                                   |
	| New Year    | 24/12/2016 | 04/01/2017 | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When I edit a seasonal date
	| Description | FromDate   | ToDate     |
	| New Years Eve    | 25/12/2016 | 02/01/2017 |
	And I update the seasonal date
	Then the seasonal date is updated with id '2'
	| Description   | FromDate   | ToDate     | Branches                                                   |
	| New Years Eve | 25/12/2016 | 02/01/2017 | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |

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