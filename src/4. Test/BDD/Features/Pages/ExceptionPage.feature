@WebDriverFeature
Feature: ExceptionPage
	As a user
	I wish to be able to view and filter exception delivery information
	so that I can determine which deliveries have been unsuccesful

Scenario: A user can view Exception Delivery Information
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22
	And  3 deliveries have been marked as exceptions
	When I open the exception deliveries
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | Status     |
	| 001   | 02   | 92874.033  | 2874.033  | RVS SHOP             | Incomplete |
	| 001   | 01   | 92874.033  | 2874.033  | CSG - must be CF van | Incomplete |
	| 001   | 01   | 949214.152 | 49214.152 | CSG - must be CF van | Incomplete |


Scenario: A user can filter Exception Delivery information
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22
	And  All the deliveries are marked as exceptions
	When I open the exception deliveries
	And I filter the exception delivery grid with the option 'Route' and value '006'
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | Status		|
	| 006   | 02   | 954107.000 | 54107.000 | WB - SHELL FORECOURT | Incomplete |
	| 006   | 02   | 954107.000 | 54107.000 | WB - SHELL FORECOURT | Incomplete |
	| 006   | 01   | 92874.033  | 2874.033  | WB - SHOP            | Incomplete |
	| 006   | 01   | 943362.048 | 43362.048 | WB - SHOP            | Incomplete |


	When I filter the exception delivery grid with the option 'Invoice No' and value '949214.152'
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | Status     |
	| 001   | 01   | 949214.152 | 49214.152 | CSG - must be CF van | Incomplete |
	When I filter the exception delivery grid with the option 'Account' and value '28398.080'
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName   | Status     |
	| 011   | 05   | 928398.080 | 28398.080 | TESCO EXPRESS | Incomplete |
	When I filter the exception delivery grid with the option 'Account Name' and value 'WB - SHOP'
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName | Status     |
	| 006   | 01   | 92874.033  | 2874.033  | WB - SHOP   | Incomplete |
	| 006   | 01   | 943362.048 | 43362.048 | WB - SHOP   | Incomplete |
	


Scenario: A user can page through Exception Delivery information
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22
	And  All the deliveries are marked as exceptions
	When I open the exception deliveries
	Then '10' rows of exception delivery data will be displayed
	And I will have 2 pages of exception delivery data
	When I click on exception delivery page 2
	Then '7' rows of exception delivery data will be displayed
	When I click on exception delivery page 1
	Then '10' rows of exception delivery data will be displayed

Scenario: View exception details at lower level
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22
	And  All the deliveries are marked as exceptions
	When I open the exception deliveries
	And I click on a exception row
	Then I am shown the exception detail
	| LineNo | Product | Description              | Value | InvoiceQuantity | DeliveryQuantity | DamagedQuantity | ShortQuantity |
	| 1      | 4237    | Maltesers Tube 75g       | 80    | 0               | 0                | 0               | 0             |
	| 2      | 7605    | Bass Sherbet Lemons 200g | 32    | 0               | 0                | 0               | 0             |
	| 3      | 41957   | Bournville Std 45g       | 84    | 0               | 0                | 0               | 0             |
	| 4      | 3319    | C.D.M Std 45g            | 125   | 0               | 0                | 0               | 0             |
	| 5      | 9135    | Wispa Duo 51g            | 395   | 0               | 0                | 0               | 0             |


	