@WebDriverFeature
@RoleSuperUser
Feature: Approvals Page
	As a user
	I wish to be able to view and filter credits waiting approval
	so that I can find and approve credits

Background: 
	Given I have a clean database
	And I have loaded the Adam route data
	And I have imported a valid Epod update file named 'ePOD_30062016_Update.xml'
	And I have the following credit thresholds setup for all branches
	| Level | Threshold |
	| 1     | 200       |
	| 2     | 100       |
	| 3     | 10        |
	

Scenario: A user can view Deliveries waiting credit approval
	Given I have selected branch '22'
	And I am assigned to credit threshold 'Level 3'
	And 3 deliveries are waiting credit approval
	When I open the approval deliveries page
	Then the following approval deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | CreditValue | Status     | Threshold |
	| 001   | 1    | 1787878   | 49214.152 | CSG - must be CF van | 123.6       | Incomplete | Level 1   |
	| 001   | 1    | 976549    | 02874.033 | CSG - must be CF van | 22.41       | Incomplete | Level 2   |
	| 001   | 2    | 976541    | 02874.033 | RVS SHOP             | 39.95       | Incomplete | Level 2   |
	When I view the account info modal for approval row 2 
	Then I can view the following account info details
	| Account name         | Street              | Town   | Postcode | Contact name  | Phone       | Alt Phone   | Email           |
	| CSG - must be CF van | 112-114 Barrow Road | SILEBY | LE12 7LP | CSG Contact 1 | 01509815739 | 01234987654 | contact@csg.com |
