@WebDriverFeature
@RoleSuperUser
Feature: Approvals Page
	As a user
	I wish to be able to view and filter deliveries waiting credit approval
	so that I can find and approve credits

Background: 
	Given I have a clean database
	And I have loaded the MultiDate Adam route data	
	And 11 deliveries have been marked as exceptions	
	And I have the following credit thresholds setup for all branches
	| Level | Threshold |
	| 1     | 5000      |
	| 2     | 50        |
	| 3     | 5         |	

Scenario: Approvals Browsing and Paging
	Given I have selected branch '22'
	And I am assigned to credit threshold 'Level 3'
	And 11 deliveries are waiting credit approval
	When I open the approval deliveries page
	Then the following approval deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | CreditValue | Threshold | Assigned    | DeliveryDate |
	| 001   | 1    | 976549    | 49214.152 | CSG - must be CF van | 79.9        | Level 1   | Unallocated | 01/08/2016    |
	| 001   | 1    | 1000123   | 02874.033 | CSG - must be CF van | 44.82       | Level 2   | Unallocated | 01/08/2016    |
	| 001   | 2    | 976541    | 02874.033 | RVS SHOP             | 79.9        | Level 1   | Unallocated | 01/08/2016    |
	| 001   | 2    | 976542    | 02874.033 | RVS SHOP             | 41.71       | Level 2   | Unallocated | 01/08/2016    |
	| 011   | 1    | 1000124    | 43362.048 | CSG - COSTCUTTER     | 329.02      | Level 1   | Unallocated | 01/07/2016    |
	| 011   | 1    | 1000125    | 02874.033 | CSG - COSTCUTTER     | 717.55      | Level 1   | Unallocated | 01/07/2016    |
	| 011   | 2    | 1000126    | 54107.000 | TESCO - EXPRESS      | 13.52       | Level 2   | Unallocated | 01/07/2016    |
	| 006   | 1    | 123123123 | 43362.048 | WB - SHOP            | 329.02      | Level 1   | Unallocated | 01/06/2016    |
	| 006   | 1    | 223123123 | 02874.033 | WB - SHOP            | 717.55      | Level 1   | Unallocated | 01/06/2016    |
	| 006   | 2    | 323123123 | 54107.000 | WB - SHELL FORECOURT | 13.52       | Level 2   | Unallocated | 01/06/2016    |
	When I click on the orderby Triangle image in the approval deliveries grid
	Then the following approval deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | CreditValue | Threshold | Assigned    | DeliveryDate |
	| 006   | 1    | 123123123 | 43362.048 | WB - SHOP            | 329.02      | Level 1   | Unallocated | 01/06/2016    |
	| 006   | 1    | 223123123 | 02874.033 | WB - SHOP            | 717.55      | Level 1   | Unallocated | 01/06/2016    |
	| 006   | 2    | 323123123 | 54107.000 | WB - SHELL FORECOURT | 13.52       | Level 2   | Unallocated | 01/06/2016    |
	| 006   | 2    | 423123123 | 54107.000 | WB - SHELL FORECOURT | 644.18      | Level 1   | Unallocated | 01/06/2016    |
	| 011   | 1    | 1000124    | 43362.048 | CSG - COSTCUTTER     | 329.02      | Level 1   | Unallocated | 01/07/2016    |
	| 011   | 1    | 1000125    | 02874.033 | CSG - COSTCUTTER     | 717.55      | Level 1   | Unallocated | 01/07/2016    |
	| 011   | 2    | 1000126    | 54107.000 | TESCO - EXPRESS      | 13.52       | Level 2   | Unallocated | 01/07/2016    |
	| 001   | 1    | 976549    | 49214.152 | CSG - must be CF van | 79.9        | Level 1   | Unallocated | 01/08/2016    |
	| 001   | 1    | 1000123    | 02874.033 | CSG - must be CF van | 44.82       | Level 2   | Unallocated | 01/08/2016    |
	| 001   | 2    | 976541    | 02874.033 | RVS SHOP             | 79.9        | Level 1   | Unallocated | 01/08/2016    |
	When I click on approvals page 2
	Then the following approval deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName | CreditValue | Threshold | Assigned    | DeliveryDate |
	| 001   | 2    | 976542    | 02874.033 | RVS SHOP    | 41.71       | Level 2   | Unallocated | 01/08/2016    |
	When I view the account info modal for approval row 1 
	Then I can view the following account info details
	| Account name | Street       | Town      | Postcode | Contact name | Phone       | Alt Phone | Email |
	| RVS SHOP     | BROAD AVENUE | LEICESTER | LE5 4PW  | GEN HOSPITAL | 01162584229 |           |       |

Scenario: Threshold Filtering
	Given I have selected branch '22'
	And I am assigned to credit threshold 'Level 3'
	And 11 deliveries are waiting credit approval	
	When I open the approval deliveries page
	And I filter for threshold level 2 
	Then the following approval deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | CreditValue | Threshold | Assigned    |
	| 001   | 1    | 1000123   | 02874.033 | CSG - must be CF van | 44.82       | Level 2   | Unallocated |
	| 001   | 2    | 976542    | 02874.033 | RVS SHOP             | 41.71       | Level 2   | Unallocated |
	| 011   | 2    | 1000126   | 54107.000 | TESCO - EXPRESS      | 13.52       | Level 2   | Unallocated |
	| 006   | 2    | 323123123 | 54107.000 | WB - SHELL FORECOURT | 13.52       | Level 2   | Unallocated |
	And I filter for threshold level 1 
	Then the following approval deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | CreditValue | Threshold | Assigned    |
	| 001   | 1    | 976549    | 49214.152 | CSG - must be CF van | 79.9        | Level 1   | Unallocated |
	| 001   | 2    | 976541    | 02874.033 | RVS SHOP             | 79.9        | Level 1   | Unallocated |
	| 011   | 1    | 1000124   | 43362.048 | CSG - COSTCUTTER     | 329.02      | Level 1   | Unallocated |
	| 011   | 1    | 1000125   | 02874.033 | CSG - COSTCUTTER     | 717.55      | Level 1   | Unallocated |
	| 006   | 1    | 123123123 | 43362.048 | WB - SHOP            | 329.02      | Level 1   | Unallocated |
	| 006   | 1    | 223123123 | 02874.033 | WB - SHOP            | 717.55      | Level 1   | Unallocated |
	| 006   | 2    | 423123123 | 54107.000 | WB - SHELL FORECOURT | 644.18      | Level 1   | Unallocated |


Scenario: Can approve credit as I have a credit threshold higher than the deliveries threshold
	Given I have selected branch '22'
	And I am assigned to credit threshold 'Level 3'
	And 1 deliveries are waiting credit approval
	When I open the approval deliveries page
	Then I am not allowed to assign the delivery
	And I cannot submit the delivery
	When I am assigned to credit threshold 'Level 2'
	And I open the approval deliveries page
	Then I am not allowed to assign the delivery
	And I cannot submit the delivery
	When I am assigned to credit threshold 'Level 1'
	And I open the approval deliveries page
	Then I cannot submit the delivery
	When I assign the approved delivery to myself
	Then I can submit the approval delivery
