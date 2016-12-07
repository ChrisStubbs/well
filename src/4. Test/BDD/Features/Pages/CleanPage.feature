@WebDriverFeature
@RoleSuperUser
Feature: CleanPage
	As a user
	I wish to be able to view and filter clean delivery information
	so that I can determine which deliveries have been succesful


Scenario: A user can view Clean Delivery Information
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'
	And  3 deliveries have been marked as clean
	When I open the clean deliveries 
	Then the following clean deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          |
	| 001   | 1    | 949214.152 | 49214.152 | CSG - must be CF van |
	| 001   | 1    | 92874.033  | 2874.033  | CSG - must be CF van |
	| 001   | 2    | 92874.033  | 2874.033  | RVS SHOP             |
	When I view the account info modal for clean row 2
	Then I can the following account info details - clean
	| Account name         | Street              | Town   | Postcode | Contact name  | Phone       | Alt Phone   | Email           |
	| CSG - must be CF van | 112-114 Barrow Road | SILEBY | LE12 7LP | CSG Contact 1 | 01509815739 | 01234987654 | contact@csg.com |


Scenario: A user can filter Clean Delivery information
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'
	And  All the deliveries are marked as clean
	When I open the clean deliveries
	And I filter the clean delivery grid with the option 'Route' and value '006'
	Then the following clean deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          |
	| 006   | 1   | 943362.048 | 43362.048 | WB - SHOP            |
	| 006   | 1   | 92874.033  | 2874.033  | WB - SHOP            |
	| 006   | 2   | 954107.000 | 54107.000 | WB - SHELL FORECOURT |
	| 006   | 2   | 954107.000 | 54107.000 | WB - SHELL FORECOURT |	

	When I filter the clean delivery grid with the option 'Invoice No' and value '949214.152'
	Then the following clean deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | 
	| 001   | 1   | 949214.152 | 49214.152 | CSG - must be CF van | 
	When I filter the clean delivery grid with the option 'Account' and value '28398.080'
	Then the following clean deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName   | 
	| 011   | 5   | 928398.080 | 28398.080 | TESCO EXPRESS | 
	When I filter the clean delivery grid with the option 'Account Name' and value 'WB - SHOP'
	Then the following clean deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName |
	| 006   | 1   | 943362.048 | 43362.048 | WB - SHOP   |  
	| 006   | 1   | 92874.033  | 2874.033  | WB - SHOP   | 

Scenario: A user can view Clean Delivery Information and sort on updated date
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'
	And  3 deliveries have been marked as clean
	When I open the clean deliveries 
	Then the following clean deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | LastUpdatedDateTime     |
	| 001   | 1   | 949214.152 | 49214.152 | CSG - must be CF van | Sep 7, 2016, 1:27:17 PM |
	| 001   | 1   | 92874.033  | 2874.033  | CSG - must be CF van | Sep 7, 2016, 1:27:16 PM |
	| 001   | 2   | 92874.033  | 2874.033  | RVS SHOP             | Sep 7, 2016, 1:27:15 PM |
	When I click on the orderby Triangle image in the clean deliveries grid
	Then The following clean deliveries ordered by date will be displayed in 'desc' order
	| Route | Drop | InvoiceNo  | Account   | AccountName          | LastUpdatedDateTime     |
	| 001   | 2   | 92874.033  | 2874.033  | RVS SHOP             | Sep 7, 2016, 1:27:15 PM |
	| 001   | 1   | 92874.033  | 2874.033  | CSG - must be CF van | Sep 7, 2016, 1:27:16 PM |
	| 001   | 1   | 949214.152 | 49214.152 | CSG - must be CF van | Sep 7, 2016, 1:27:17 PM |

Scenario: A user can page through Clean Delivery information
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

Scenario: A user can view Clean Delivery Information with cash on delivery icons displayed
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'
	And  3 deliveries have been marked as clean
	And the first 'clean' delivery is not a cash on delivery customer
	When I open the clean deliveries 
	Then the cod delivery icon is not displayed in row 1
	
