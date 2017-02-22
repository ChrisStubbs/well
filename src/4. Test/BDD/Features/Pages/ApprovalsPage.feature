@WebDriverFeature
@RoleSuperUser
Feature: Approvals Page
	As a user
	I wish to be able to view and filter deliveries waiting credit approval
	so that I can find and approve credits

Background: 
	Given I have a clean database
	And I have loaded the Adam route data
	And I have loaded the MultiDate Adam route data
	And I have the following credit thresholds setup for all branches
	| Level | Threshold |
	| 1     | 500       |
	| 2     | 30        |
	| 3     | 5         |	

Scenario: Approvals Browsing and Paging
	Given I have selected branch '22'
	And I am assigned to credit threshold 'Level 3'
	And 11 deliveries are waiting credit approval
	When I open the approval deliveries page
	Then the following approval deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | CreditValue | Threshold | Assigned    |
	| 001   | 1    | 976549    | 49214.152 | CSG - must be CF van | 39.95       | Level 1   | Unallocated |
	| 001   | 1    | 976549    | 02874.033 | CSG - must be CF van | 22.41       | Level 2   | Unallocated |
	| 001   | 2    | 976541    | 02874.033 | RVS SHOP             | 39.95       | Level 1   | Unallocated |
	| 001   | 2    | 976542    | 02874.033 | RVS SHOP             | 19.23       | Level 2   | Unallocated |
	| 006   | 1    | 123123123 | 43362.048 | WB - SHOP            | 24.72       | Level 2   | Unallocated |
	| 006   | 1    | 223123123 | 02874.033 | WB - SHOP            | 80          | Level 1   | Unallocated |
	| 006   | 2    | 323123123 | 54107.000 | WB - SHELL FORECOURT | 7.32        | Level 2   | Unallocated |
	| 006   | 2    | 423123123 | 54107.000 | WB - SHELL FORECOURT | 176.7       | Level 1   | Unallocated |
	| 011   | 1    | 976549    | 43362.048 | CSG - COSTCUTTER     | 24.72       | Level 2   | Unallocated |
	| 011   | 1    | 976549    | 02874.033 | CSG - COSTCUTTER     | 80          | Level 1   | Unallocated |
	When I view the account info modal for approval row 2 
	Then I can view the following account info details
	| Account name         | Street              | Town   | Postcode | Contact name  | Phone       | Alt Phone   | Email           |
	| CSG - must be CF van | 112-114 Barrow Road | SILEBY | LE12 7LP | CSG Contact 1 | 01509815739 | 01234987654 | contact@csg.com |
	When I click on approvals page 2
	Then the following approval deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName     | CreditValue | Threshold | Assigned    |
	| 011   | 2    | 976549    | 54107.000 | TESCO - EXPRESS | 7.32        | Level 2   | Unallocated |

Scenario: Threshold Filtering
	Given I have selected branch '22'
	And I am assigned to credit threshold 'Level 3'
	And 11 deliveries are waiting credit approval	
	When I open the approval deliveries page
	And I filter for threshold level 2 
	Then the following approval deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | CreditValue | Threshold | Assigned    |
	| 001   | 1    | 976549    | 02874.033 | CSG - must be CF van | 22.41       | Level 2   | Unallocated |
	| 001   | 2    | 976542    | 02874.033 | RVS SHOP             | 19.23       | Level 2   | Unallocated |
	| 006   | 1    | 123123123 | 43362.048 | WB - SHOP            | 24.72       | Level 2   | Unallocated |
	| 006   | 2    | 323123123 | 54107.000 | WB - SHELL FORECOURT | 7.32        | Level 2   | Unallocated |
	| 011   | 1    | 976549    | 43362.048 | CSG - COSTCUTTER     | 24.72       | Level 2   | Unallocated |
	| 011   | 2    | 976549    | 54107.000 | TESCO - EXPRESS      | 7.32        | Level 2   | Unallocated |
	And I filter for threshold level 1 
	Then the following approval deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | CreditValue | Threshold | Assigned    |
	| 001   | 1    | 976549    | 49214.152 | CSG - must be CF van | 39.95       | Level 1   | Unallocated |
	| 001   | 2    | 976541    | 02874.033 | RVS SHOP             | 39.95       | Level 1   | Unallocated |
	| 006   | 1    | 223123123 | 02874.033 | WB - SHOP            | 80          | Level 1   | Unallocated |
	| 006   | 2    | 423123123 | 54107.000 | WB - SHELL FORECOURT | 176.7       | Level 1   | Unallocated |
	| 011   | 1    | 976549    | 02874.033 | CSG - COSTCUTTER     | 80          | Level 1   | Unallocated |