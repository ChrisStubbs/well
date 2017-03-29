@WebDriverFeature
@RoleSuperUser
Feature: CleanPage
	As a user
	I wish to be able to view and filter clean delivery information
	so that I can determine which deliveries have been succesful


Scenario: View Clean Deliveries
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'
	And  3 deliveries have been marked as clean
	When I open the clean deliveries 
	Then the following clean deliveries will be displayed
	| Route | Branch | Drop | InvoiceNo | Account   | AccountName          |
	| 001   | 22     | 1    | 94294343  | 49214.152 | CSG - must be CF van |
	| 001   | 22     | 1    | 92545470  | 2874.033  | CSG - must be CF van |
	| 001   | 22     | 2    | 92545470  | 2874.033  | RVS SHOP             |
	When I view the account info modal for clean row 2
	Then I can the following account info details - clean
	| Account name         | Street              | Town   | Postcode | Contact name  | Phone       | Alt Phone   | Email           |
	| CSG - must be CF van | 112-114 Barrow Road | SILEBY | LE12 7LP | CSG Contact 1 | 01509815739 | 01234987654 | contact@csg.com |


Scenario: Filter Clean Deliveries
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'
	And  All the deliveries are marked as clean
	When I open the clean deliveries
	And I filter the clean delivery grid with the option 'Route' and value '006'
	Then the following clean deliveries will be displayed
	| Route | Branch | Drop | InvoiceNo | Account    | AccountName          |
	| 006   | 22     | 1    | 91156028  | 43362.048  | WB - SHOP            |
	| 006   | 22     | 1    | 92544765  | 2874.033  | WB - SHOP            |
	| 006   | 22     | 2    | 94295479  | 54107.000  | WB - SHELL FORECOURT |
	| 006   | 22     | 2    | 94294985  | 54107.000  | WB - SHELL FORECOURT |	

	When I filter the clean delivery grid with the option 'Invoice No' and value '94294343'
	Then the following clean deliveries will be displayed
	| Route | Branch | Drop | InvoiceNo | Account   | AccountName          |
	| 001   | 22     |1     | 94294343  | 49214.152 | CSG - must be CF van | 
	When I filter the clean delivery grid with the option 'Account' and value '28398.080'
	Then the following clean deliveries will be displayed
	| Route | Branch | Drop | InvoiceNo | Account   | AccountName   |
	| 011   | 22     |5     | 92545853  | 28398.080 | TESCO EXPRESS | 
	When I filter the clean delivery grid with the option 'Account Name' and value 'WB - SHOP'
	Then the following clean deliveries will be displayed
	| Route | Branch | Drop | InvoiceNo | Account   | AccountName |
	| 006   | 22     | 1    | 91156028  | 43362.048 | WB - SHOP   |
	| 006   | 22     | 1    | 92544765  | 2874.033 | WB - SHOP   | 

Scenario: Sort Clean Deliveries
	Given I have a clean database
	And I have loaded the MultiDate Adam route data
	And I have selected branch '22'
	And 5 deliveries have been marked as clean
	When I open the clean deliveries 
	#Ascending Order
	Then the following clean deliveries will be displayed		
	| Route | Branch | Drop | InvoiceNo | Account   | AccountName          | DeliveryDate |
	| 001   | 22     | 1    | 94294343  | 49214.152 | CSG - must be CF van | 01/08/2016   |
	| 001   | 22     | 1    | 92545470  | 02874.033 | CSG - must be CF van | 01/08/2016   |
	| 001   | 22     | 2    | 92545470  | 02874.033 | RVS SHOP             | 01/08/2016   |
	| 001   | 22     | 2    | 92545419  | 02874.033 | RVS SHOP             | 01/08/2016   |
	| 006   | 22     | 1    | 91156028  | 43362.048 | WB - SHOP            | 01/06/2016   |
	#Descending Order
	When I click on the orderby Triangle image in the clean deliveries grid
	Then the following clean deliveries will be displayed
	| Route | Branch | Drop | InvoiceNo | Account   | AccountName          | DeliveryDate |
	| 006   | 22     | 1    | 91156028  | 43362.048 | WB - SHOP            | 01/06/2016   |
	| 001   | 22     | 1    | 94294343  | 49214.152 | CSG - must be CF van | 01/08/2016   |
	| 001   | 22     | 1    | 92545470  | 02874.033 | CSG - must be CF van | 01/08/2016   |
	| 001   | 22     | 2    | 92545470  | 02874.033 | RVS SHOP             | 01/08/2016   |
	| 001   | 22     | 2    | 92545419  | 02874.033 | RVS SHOP             | 01/08/2016   |

Scenario: Page Clean Deliveries
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'
	And  All the deliveries are marked as clean
	When I open the clean deliveries
	Then '10' rows of clean delivery data will be displayed
	And I will have 2 pages of clean delivery data
	When I click on clean delivery page 2
	Then '7' rows of clean delivery data will be displayed
	When I click on clean delivery page 1
	Then '10' rows of clean delivery data will be displayed

Scenario: View cash on delivery icon
	Given I have a clean database
	And I have loaded the MultiDate Adam route data
	And I have selected branch '22'
	And 3 deliveries have been marked as clean
	When I open the clean deliveries 
	Then the first clean delivery line is COD (Cash on Delivery)

Scenario: Clean deliveries wil have no exception delivery lines
	Given I have a clean database
	And I have loaded the MultiDate Adam route data
	And I have loaded the Adam route data
	And I have selected branches '22' and '2'
	And  All the deliveries are marked as clean
	And I open the clean deliveries 
	When I click on each of the clean deliveries on each page there will be no exception delivery lines
	
