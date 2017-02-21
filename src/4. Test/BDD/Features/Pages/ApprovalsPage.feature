﻿@WebDriverFeature
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
	| 1     | 100       |
	| 2     | 30        |
	| 3     | 5         |
	

Scenario: Approvals Browsing
	Given I have selected branch '22'
	And I am assigned to credit threshold 'Level 3'
	And 7 deliveries are waiting credit approval
	When I open the approval deliveries page
	And I open the widget page
	And I go back
	Then the following approval deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | CreditValue | Threshold | Assigned    |
	| 001   | 1    | 976549    | 49214.152 | CSG - must be CF van | 39.95       | Level 1   | Unallocated |
	| 001   | 1    | 976549    | 02874.033 | CSG - must be CF van | 22.41       | Level 2   | Unallocated |
	| 001   | 2    | 976541    | 02874.033 | RVS SHOP             | 39.95       | Level 1   | Unallocated |
	| 001   | 2    | 976542    | 02874.033 | RVS SHOP             | 19.23       | Level 2   | Unallocated |
	| 006   | 1    | 123123123 | 43362.048 | WB - SHOP              | 24.72       | Level 2   | Unallocated |
	| 006   | 1    | 223123123 | 02874.033 | WB - SHOP              | 80          | Level 1   | Unallocated |
	| 006   | 2    | 323123123 | 54107.000 | WB - SHELL FORECOURT   | 7.32        | Level 2   | Unallocated |
	When I view the account info modal for approval row 2 
	Then I can view the following account info details
	| Account name         | Street              | Town   | Postcode | Contact name  | Phone       | Alt Phone   | Email           |
	| CSG - must be CF van | 112-114 Barrow Road | SILEBY | LE12 7LP | CSG Contact 1 | 01509815739 | 01234987654 | contact@csg.com |

#Scenario: Approvals Filtering

#Scenario: Approvals Paging 

