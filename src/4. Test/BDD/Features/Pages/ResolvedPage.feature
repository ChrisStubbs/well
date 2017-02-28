@WebDriverFeature
@RoleSuperUser
Feature: Resolved Delivery Page
	As a user
	I wish to be able to view and filter resolved delivery information
	so that I can determine which deliveries have been resolved

Scenario: A user can view Resolved Delivery Information
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'
	And  3 deliveries have been marked as Resolved
	When I open the resolved deliveries page
	Then the following resolved deliveries will be displayed
	| Id | Route | Drop | InvoiceNo | Account   | AccountName          | Status   | Assigned    |
	| 1  | 001   | 1    | 94294343	| 49214.152 | CSG - must be CF van | Resolved | Unallocated |
	| 2  | 001   | 1    | 92545470  | 2874.033  | CSG - must be CF van | Resolved | Unallocated |
	| 3  | 001   | 2    | 92545470  | 2874.033  | RVS SHOP             | Resolved | Unallocated |
	When I view the account info modal for resolved row 2
	Then I can the following account info details - resolved
	| Account name         | Street              | Town   | Postcode | Contact name  | Phone       | Alt Phone   | Email           |
	| CSG - must be CF van | 112-114 Barrow Road | SILEBY | LE12 7LP | CSG Contact 1 | 01509815739 | 01234987654 | contact@csg.com |

Scenario: A user can filter Resolved Delivery information
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'
	And  All the deliveries are marked as Resolved
	When I open the resolved deliveries page
	And I filter the resolved delivery grid with the option 'Route' and value '006'
	Then the following resolved deliveries will be displayed
	| Id | Route | Drop | InvoiceNo  | Account   | AccountName          | Status   | Assigned    |
	| 5  | 006   | 1    | 91156028	| 43362.048 | WB - SHOP            | Resolved | Unallocated |
	| 6  | 006   | 1    | 92544765  | 2874.033  | WB - SHOP            | Resolved | Unallocated |
	| 7  | 006   | 2    | 94295479 | 54107.000 | WB - SHELL FORECOURT | Resolved | Unallocated |
	| 8  | 006   | 2    | 94294985 | 54107.000 | WB - SHELL FORECOURT | Resolved | Unallocated |
	When I filter the resolved delivery grid with the option 'Invoice No' and value '94294343'
	Then the following resolved deliveries will be displayed
	| Id | Route | Drop | InvoiceNo  | Account   | AccountName          | Status   | Action | Assigned    |
	| 1  | 001   | 1    | 94294343 | 49214.152 | CSG - must be CF van | Resolved |        | Unallocated |
	When I filter the resolved delivery grid with the option 'Account' and value '28398.080'
	Then the following resolved deliveries will be displayed
	| Id | Route | Drop | InvoiceNo  | Account   | AccountName   | Status   | Action | Assigned    |
	| 16 | 011   | 5    | 92545853 | 28398.080 | TESCO EXPRESS | Resolved |        | Unallocated |
	When I filter the resolved delivery grid with the option 'Account Name' and value 'WB - SHOP'
	Then the following resolved deliveries will be displayed
	| Id | Route | Drop | InvoiceNo  | Account   | AccountName | Status   | Action | Assigned    |
	| 5  | 006   | 1    | 91156028 | 43362.048 | WB - SHOP   | Resolved |        | Unallocated |
	| 6  | 006   | 1    | 92544765  | 2874.033  | WB - SHOP   | Resolved |        | Unallocated |

Scenario: A user can view Resolved Delivery Information and sort on updated date
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'
	And  3 deliveries have been marked as Resolved
	When I open the resolved deliveries page
	Then the following resolved deliveries will be displayed
	| Id | Route | Drop | InvoiceNo  | Account   | AccountName          | Status   | Assigned    | LastUpdatedDateTime     |
	| 1  | 001   | 1    | 94294343 | 49214.152 | CSG - must be CF van | Resolved | Unallocated | Sep 7, 2016, 1:27:17 PM |
	| 2  | 001   | 1    | 92545470  | 2874.033  | CSG - must be CF van | Resolved | Unallocated | Sep 7, 2016, 1:29:17 PM |
	| 3  | 001   | 2    | 92545470  | 2874.033  | RVS SHOP             | Resolved | Unallocated | Sep 7, 2016, 1:31:17 PM |
	When I click on the orderby Triangle image in the resolved deliveries grid
	Then The following resolved deliveries ordered by date will be displayed in 'desc' order
	| Id | Route | Drop | InvoiceNo  | Account   | AccountName          | Status   | Assigned    | LastUpdatedDateTime     |
	| 3  | 001   | 2    | 92545470  | 2874.033  | RVS SHOP             | Resolved | Unallocated | Sep 7, 2016, 1:31:17 PM |
	| 2  | 001   | 1    | 92545470  | 2874.033  | CSG - must be CF van | Resolved | Unallocated | Sep 7, 2016, 1:29:17 PM |
	| 1  | 001   | 1    | 94294343 | 49214.152 | CSG - must be CF van | Resolved | Unallocated | Sep 7, 2016, 1:27:17 PM |

Scenario: A user can page through Resolved Delivery information
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'
	And  All the deliveries are marked as Resolved
	When I open the resolved deliveries page
	Then '10' rows of resolved delivery data will be displayed
	And I will have 2 pages of resolved delivery data
	When I click on resolved delivery page 2
	Then '7' rows of resolved delivery data will be displayed
	When I click on resolved delivery page 1
	Then '10' rows of resolved delivery data will be displayed