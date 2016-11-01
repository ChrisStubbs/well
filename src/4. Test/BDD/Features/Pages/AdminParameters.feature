@WebDriverFeature
@RoleSuperUser
Feature: Admininistration Parameters
	In order to parameterise the well
	As a user
	I want to be able to set seasonal dates so that clean deliveries take these dates into account when getting cleared from the well
	And I want to be able to set credit threshold per branch
	And I want to be able to set the time clean deliveries are cleaned from the well
	And I want to be able to set widget warning levels per branch

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

Scenario: Clean parameter add new
	Given I have a clean database
	And I navigate to the branch parameters page
	When I add a clean parameter
	| Days |
	| 1    |
	And all branches are selected for the clean parameter
	And I save the clean parameter
	Then the clean parameter is saved
	| Days | Branches                                                   |
	| 1    | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	And I navigate to the branch parameters page
	When I select the clean parameter tab
	Then the clean parameter is saved
	| Days | Branches                                                   |
	| 1    | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |

Scenario: Clean parameters remove
	Given I have a clean database
	And I navigate to the branch parameters page
	When I add a clean parameter
	| Days |
	| 1    |
	And all branches are selected for the clean parameter
	And I save the clean parameter
	Then the clean parameter is saved
	| Days | Branches                                                   |
	| 1    | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When I remove the clean parameter
	Then it is removed from the clean parameter grid

Scenario: Clean parameters edit
	Given I have a clean database
	And I navigate to the branch parameters page
	When I add a clean parameter
	| Days |
	| 1    |
	And all branches are selected for the clean parameter
	And I save the clean parameter
	Then the clean parameter is saved
	| Days | Branches                                                   |
	| 1    | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |
	When I edit a clean parameter
	| Days |
	| 2    |
	And I update the clean parameter
	Then the clean parameter is updated with id '2'
	| Days | Branches                                                   |
	| 2    | med, cov, far, dun, lee, hem, bir, bel, bra, ply, bri, hay |

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





