@WebDriverFeature
@RoleSuperUser
Feature: ExceptionPage
	As a user
	I wish to be able to view and filter exception delivery information
	so that I can determine	which deliveries have been unsuccesful

Background: 
	Given I have a clean database
	And I have loaded the Adam route data
	And I have imported a valid Epod update file named 'ePOD_30062016_Update.xml'

Scenario: A user can view Exception Delivery Information
	Given I have selected branch 22
	And  3 deliveries have been marked as exceptions
	When I open the exception deliveries
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | Status     |	
	| 001   | 01   | 949214.152 | 49214.152 | CSG - must be CF van | Incomplete |
	| 001   | 01   | 92874.033  | 2874.033  | CSG - must be CF van | Incomplete |
	| 001   | 02   | 92874.033  | 2874.033  | RVS SHOP             | Incomplete |

	When I view the account info modal for exception row 2
	Then I can the following account info details
	| Account name         | Street              | Town   | Postcode | Contact name  | Phone       | Alt Phone   | Email           |
	| CSG - must be CF van | 112-114 Barrow Road | SILEBY | LE12 7LP | CSG Contact 1 | 01509815739 | 01234987654 | contact@csg.com |

Scenario: A user can filter Exception Delivery information
	Given I have selected branch 22
	And  All the deliveries are marked as exceptions
	When I open the exception deliveries
	And I filter the exception delivery grid with the option 'Route' and value '006'
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | Status     |
	| 006   | 01   | 943362.048 | 43362.048 | WB - SHOP            | Incomplete |
	| 006   | 01   | 92874.033  | 2874.033  | WB - SHOP            | Incomplete |
	| 006   | 02   | 954107.000 | 54107.000 | WB - SHELL FORECOURT | Incomplete |
	| 006   | 02   | 954107.000 | 54107.000 | WB - SHELL FORECOURT | Incomplete |

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
	| 006   | 01   | 943362.048 | 43362.048 | WB - SHOP   | Incomplete |
	| 006   | 01   | 92874.033  | 2874.033  | WB - SHOP   | Incomplete |


Scenario: A user can view Exception Delivery Information and sort on updated date
	Given I have selected branch 22
	And  3 deliveries have been marked as exceptions
	When I open the exception deliveries
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | Status     | LastUpdatedDateTime     |
	| 001   | 01   | 949214.152 | 49214.152 | CSG - must be CF van | Incomplete | Sep 7, 2016, 1:28:16 PM |
	| 001   | 01   | 92874.033  | 2874.033  | CSG - must be CF van | Incomplete | Sep 7, 2016, 1:30:17 PM |
	| 001   | 02   | 92874.033  | 2874.033  | RVS SHOP             | Incomplete | Sep 7, 2016, 1:27:17 PM |

	When I click on the orderby Triangle image in the exceptions deliveries grid
	Then The following exceptions ordered by date will be displayed in 'desc' order
	| Route | Drop | InvoiceNo  | Account   | AccountName          | Status     | LastUpdatedDateTime     |
	| 001   | 01   | 949214.152 | 49214.152 | CSG - must be CF van | Incomplete | Sep 7, 2016, 1:28:16 PM |
	| 001   | 01   | 92874.033  | 2874.033  | CSG - must be CF van | Incomplete | Sep 7, 2016, 1:30:17 PM |
	| 001   | 02  | 92874.033  | 2874.033  | RVS SHOP             | Incomplete | Sep 7, 2016, 1:27:17 PM |
	
	
	



Scenario: A user can page through Exception Delivery information
	Given I have selected branch 22
	And  All the deliveries are marked as exceptions
	When I open the exception deliveries
	Then '10' rows of exception delivery data will be displayed
	And I will have 2 pages of exception delivery data
	When I click on exception delivery page 2
	Then '7' rows of exception delivery data will be displayed
	When I click on exception delivery page 1
	Then '10' rows of exception delivery data will be displayed

Scenario: View exception details at lower level
	Given I have selected branch 22
	And  All the deliveries are marked as exceptions
	When I open the exception deliveries
	And I click on exception row 4
	Then I am shown the exception detail
	| LineNo | Product | Description              | Value | InvoiceQuantity | DeliveryQuantity | DamagedQuantity | ShortQuantity |
	| 1      | 6987    | Choc Teacakes Tunnock    | 19    | 1               | 1                | 0               | 0             |
	| 2      | 49179   | Ginger Nuts 250g         | 4     | 1               | 1                | 0               | 0             |
	| 3      | 21633   | Kiddies Super Mix 220gPM | 3     | 1               | 1                | 0               | 0             |
	| 4      | 4244    | Milkybar Btns Giant PM   | 5     | 1               | 1                | 0               | 0             |
	| 5      | 7621    | Fruit Past Tube 52.5g    | 8     | 1               | 1                | 0               | 0             |

Scenario: Exception assigned to a user
	Given I have selected branch 22
	And All the deliveries are marked as exceptions
	When I open the exception deliveries
	And I assign the delivery to myself
	Then the user is assigned to that exception

Scenario: Assigned user to an exception can action it
	Given I have selected branch 22
	And All the deliveries are marked as exceptions
	When I open the exception deliveries
	And I assign the delivery to myself
	Then the user is assigned to that exception
	And the user can action the exception
	And all other actions are disabled

Scenario: Assigned user to an exception drills to details and can update
	Given I have selected branch 22
	And All the deliveries are marked as exceptions
	When I open the exception deliveries
	And I assign the delivery to myself
	And I select the exception row
	Then All the exception detail rows can be updated

Scenario: UnAssigned user to an exception drills to details and can not update
	Given I have selected branch 22
	And All the deliveries are marked as exceptions
	When I open the exception deliveries
	And I assign the delivery to myself
	And I select an unassigned exception row
	Then All the exception detail rows can not be updated

Scenario: Credit check boxes are not enabled till exceptions are assigned
	Given I have selected branch 22
	And All the deliveries are marked as exceptions
	When I open the exception deliveries
	Then the 'credit' and 'selectAll' button is not visible
	When I assign the delivery on row 2 to myself
	And click the first credit checkbox
	Then the 'credit' and 'selectAll' button are visible

Scenario: Select all button will check all allowcated job lines
	Given I have selected branch 22
	And All the deliveries are marked as exceptions
	When I open the exception deliveries
	Then the 'credit' and 'selectAll' button is not visible
	When I assign the delivery on row 1 to myself
	When I assign the delivery on row 2 to myself
	And click the first credit checkbox
	Then the 'credit' and 'selectAll' button are visible
	When I click the 'selectAll' button
	Then the first 2 checkboxes are checked



Scenario: A user cannot view Exception Delivery Information without a valid invoice number
	Given I have a clean database
	And I have loaded the Adam route data
	And I have imported a valid Epod update file named 'ePOD__MissingInvoiceNumbers.xml'
	And I have selected branch 22
	When I open the exception deliveries
	Then there are 0 exception deliveries will be displayed
	Given  3 deliveries have been marked as exceptions
	When valid invoice numbers are assigned to jobs
	When I open the exception deliveries
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | Status     |
	| 001   | 01   | 949214.152 | 49214.152 | CSG - must be CF van | Incomplete |
	| 001   | 01   | 92874.033  | 2874.033  | CSG - must be CF van | Incomplete |
	| 001   | 02   | 92874.033  | 2874.033  | RVS SHOP             | Incomplete |

Scenario: A user can view Exception Delivery Information with cash on delivery icons displayed
	Given I have selected branch 22
	And All the deliveries are marked as exceptions
    And the first 'exception' delivery is not a cash on delivery customer
	When I open the exception deliveries
	Then the exception cod delivery icon is not displayed in row 1



