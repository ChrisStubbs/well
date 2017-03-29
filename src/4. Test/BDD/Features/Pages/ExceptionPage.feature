@WebDriverFeature
@RoleSuperUser
Feature: ExceptionPage
	As a user
	I wish to be able to view and filter exception delivery information
	so that I can determine	which deliveries have been unsuccesful

Background: 
	Given I have a clean database
	And I have loaded the MultiDate Adam route data

Scenario: View Exceptions
	Given I have selected branch '22'
	And 3 deliveries have been marked as exceptions
	When I open the exception deliveries
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | Status     | TBA |
	| 001   | 1    | 976549    | 49214.152 | CSG - must be CF van | Incomplete | 0   |
	| 001   | 1    | 1000123   | 02874.033 | CSG - must be CF van | Incomplete | 0   |
	| 001   | 2    | 976541    | 02874.033 | RVS SHOP             | Incomplete | 0   |
	When I view the account info modal for exception row 2 
	Then I can the following account info details
	| Account name         | Street              | Town   | Postcode | Contact name  | Phone       | Alt Phone   | Email           |
	| CSG - must be CF van | 112-114 Barrow Road | SILEBY | LE12 7LP | CSG Contact 1 | 01509815739 | 01234987654 | contact@csg.com |


Scenario: Filter exceptions
	Given I have selected branches '22' and '2'
	And  All the deliveries are marked as exceptions
	When I open the exception deliveries
	And I filter the exception delivery grid with the option 'Route' and value '006'
	Then the following exception deliveries will be displayed
	| Route | Branch | Drop | InvoiceNo | Account   | AccountName          | Status     | TBA |
	| 006   | 22     | 1    | 123123123 | 43362.048 | WB - SHOP            | Incomplete | 0   |
	| 006   | 22     | 1    | 223123123 | 02874.033 | WB - SHOP            | Incomplete | 0   |
	| 006   | 22     | 2    | 323123123 | 54107.000 | WB - SHELL FORECOURT | Incomplete | 0   |
	| 006   | 22     | 2    | 423123123 | 54107.000 | WB - SHELL FORECOURT | Incomplete | 0   |
	When I filter the exception delivery grid with the option 'Invoice No' and value '423123123'
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | Status     | TBA |
	| 006   | 2    | 423123123 | 54107.000 | WB - SHELL FORECOURT | Incomplete | 0   |
	When I filter the exception delivery grid with the option 'Account' and value '28398.080'
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName   | Status     | TBA |
	| 011   | 5    | 1000131    | 28398.080 | TESCO EXPRESS | Incomplete | 0   |
	| 011   | 5    | 1000140    | 28398.080 | TESCO EXPRESS | Incomplete | 0   |
	When I filter the exception delivery grid with the option 'Account Name' and value 'WB - SHOP'
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName | Status     | TBA |
	| 006   | 1    | 123123123 | 43362.048 | WB - SHOP   | Incomplete | 0   |
	| 006   | 1    | 223123123 | 02874.033 | WB - SHOP   | Incomplete | 0   |


Scenario: Sort exceptions
	Given I have selected branch '22'
	And 9 deliveries have been marked as exceptions
	When I open the exception deliveries
	Then The following exceptions ordered by date will be displayed in 'desc' order
	| Route | Branch | Drop | InvoiceNo | Account   | AccountName          | Status     | TBA | DeliveryDate |
	| 001   | 22     | 1    | 976549    | 49214.152 | CSG - must be CF van | Incomplete | 0   | 01/08/2016   |
	| 001   | 22     | 1    | 1000123   | 02874.033 | CSG - must be CF van | Incomplete | 0   | 01/08/2016   |
	| 001   | 22     | 2    | 976541    | 02874.033 | RVS SHOP             | Incomplete | 0   | 01/08/2016   |
	| 001   | 22     | 2    | 976542    | 02874.033 | RVS SHOP             | Incomplete | 0   | 01/08/2016   |
	| 011   | 22     | 1    | 1000124   | 43362.048 | CSG - COSTCUTTER     | Incomplete | 0   | 01/07/2016   |
	| 006   | 22     | 1    | 123123123 | 43362.048 | WB - SHOP            | Incomplete | 0   | 01/06/2016   |
	| 006   | 22     | 1    | 223123123 | 02874.033 | WB - SHOP            | Incomplete | 0   | 01/06/2016   |
	| 006   | 22     | 2    | 323123123 | 54107.000 | WB - SHELL FORECOURT | Incomplete | 0   | 01/06/2016   |
	| 006   | 22     | 2    | 423123123 | 54107.000 | WB - SHELL FORECOURT | Incomplete | 0   | 01/06/2016   |
	When I click on the orderby Triangle image in the exceptions deliveries grid
	Then The following exceptions ordered by date will be displayed in 'asc' order
	| Route | Branch | Drop | InvoiceNo | Account   | AccountName          | Status     | TBA | DeliveryDate |
	| 006   | 22     | 1    | 123123123 | 43362.048 | WB - SHOP            | Incomplete | 0   | 01/06/2016   |
	| 006   | 22     | 1    | 223123123 | 02874.033 | WB - SHOP            | Incomplete | 0   | 01/06/2016   |
	| 006   | 22     | 2    | 323123123 | 54107.000 | WB - SHELL FORECOURT | Incomplete | 0   | 01/06/2016   |
	| 006   | 22     | 2    | 423123123 | 54107.000 | WB - SHELL FORECOURT | Incomplete | 0   | 01/06/2016   |
	| 011   | 22     | 1    | 1000124   | 43362.048 | CSG - COSTCUTTER     | Incomplete | 0   | 01/07/2016   |
	| 001   | 22     | 1    | 976549    | 49214.152 | CSG - must be CF van | Incomplete | 0   | 01/08/2016   |
	| 001   | 22     | 1    | 1000123   | 02874.033 | CSG - must be CF van | Incomplete | 0   | 01/08/2016   |
	| 001   | 22     | 2    | 976541    | 02874.033 | RVS SHOP             | Incomplete | 0   | 01/08/2016   |
	| 001   | 22     | 2    | 976542    | 02874.033 | RVS SHOP             | Incomplete | 0   | 01/08/2016   |

Scenario: Page exceptions
	Given I have selected branch '22'
	And  All the deliveries are marked as exceptions
	When I open the exception deliveries
	Then '10' rows of exception delivery data will be displayed
	And I will have 2 pages of exception delivery data
	When I click on exception delivery page 2
	Then '7' rows of exception delivery data will be displayed
	When I click on exception delivery page 1
	Then '10' rows of exception delivery data will be displayed

Scenario: View exception details
	Given I have selected branch '22'
	And  All the deliveries are marked as exceptions
	When I open the exception deliveries
	And I click on exception row 4
	Then I am shown the exception detail
	| LineNo | Product | Description              | Value | InvoiceQuantity | DeliveryQuantity | DamagedQuantity | ShortQuantity |
	| 1      | 6987    | Choc Teacakes Tunnock    | 19.23 | 1               | 0                | 0               | 1             |
	| 2      | 49179   | Ginger Nuts 250g         | 4.88  | 1               | 0                | 0               | 1             |
	| 3      | 21633   | Kiddies Super Mix 220gPM | 3.60  | 1               | 0                | 0               | 1             |
	| 4      | 4244    | Milkybar Btns Giant PM   | 5.60  | 1               | 0                | 0               | 1             |
	| 5      | 7621    | Fruit Past Tube 52.5g    | 8.40  | 1               | 0                | 0               | 1             |

Scenario: Exception assigned to a user
	Given I have selected branch '22'
	And All the deliveries are marked as exceptions
	When I open the exception deliveries
	And I assign the delivery to myself
	Then the user is assigned to that exception

Scenario: Submit an exception
	Given I have selected branch '22'
	And I have the following credit thresholds setup for all branches
	| Level | Threshold |
	| 1     | 5000      |
	| 2     | 50        |
	| 3     | 5         |	
	And I am assigned to credit threshold 'Level 1'
	And 1 deliveries have been marked as exceptions
	And 1 delivery has all its lines set to credit
	And I open the exception deliveries
	When I select the exception submit button
	And I confirm the exception submit
	Then no exceptions are displayed

Scenario: Assigned user can update exception lines
	Given I have selected branch '22'
	And All the deliveries are marked as exceptions
	When I open the exception deliveries
	And I assign the delivery to myself
	And I select the exception row
	Then All the exception detail rows can be updated

Scenario: UnAssigned user can not update exception lines
	Given I have selected branch '22'
	And All the deliveries are marked as exceptions
	When I open the exception deliveries
	And I assign the delivery to myself
	And I select an unassigned exception row
	Then All the exception detail rows can not be updated

@Ignore
Scenario: Credit check boxes are not enabled till exceptions are assigned
	Given I have selected branch '22'
	And All the deliveries are marked as exceptions
	When I open the exception deliveries
	Then the 'credit' and 'selectAll' button is not visible
	When I assign the delivery on row 2 to myself
	And click the first credit checkbox
	Then the 'credit' and 'selectAll' button are visible

@Ignore
Scenario: Select all button will check all allocated job lines
	Given I have selected branch '22'
	And All the deliveries are marked as exceptions
	When I open the exception deliveries
	Then the 'credit' and 'selectAll' button is not visible
	When I assign the delivery on row 1 to myself
	When I assign the delivery on row 2 to myself
	And click the first credit checkbox
	Then the 'credit' and 'selectAll' button are visible
	When I click the Select All button
	Then the first 2 checkboxes are checked
	
Scenario: Exceptions without invoice numbers are not shown
	Given I have a clean database
	And I have loaded the Adam route data
	And I have imported a valid Epod update file named 'ePOD__MissingInvoiceNumbers.xml'
	And I have selected branch '22'
	When I open the exception deliveries
	Then there are 0 exception deliveries will be displayed
	Given 3 deliveries have been marked as exceptions
	When valid invoice numbers are assigned to jobs
	When I open the exception deliveries
	Then the following exception deliveries will be displayed
	| Route | Branch | Drop | InvoiceNo | Account   | AccountName          | Status     | TBA |
	| 001   | 22     | 1    | 976549    | 49214.152 | CSG - must be CF van | Incomplete | 0   |
	| 001   | 22     | 1    | 976549    | 2874.033  | CSG - must be CF van | Incomplete | 0   |
	| 001   | 22     | 2    | 976541    | 2874.033  | RVS SHOP             | Incomplete | 0   |

Scenario: View cash on delivery icon
	Given I have selected branch '22'
	And 2 deliveries have been marked as exceptions
	When I open the exception deliveries
	Then the first delivery line is COD (Cash on Delivery)
		
Scenario: A user can view Exception Delivery Information with shorts to be advised displayed
	Given I have selected branch '22'
	And  2 deliveries have been marked as exceptions with shorts to be advised
	When I open the exception deliveries
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | Status     | TBA |
	| 001   | 1    | 976549    | 49214.152 | CSG - must be CF van | Incomplete | 2   |
	| 001   | 1    | 1000123   | 02874.033 | CSG - must be CF van | Incomplete | 2   |


Scenario: A user can view Exception Delivery Information with shorts to be advised when DETSHORT and TOTSHORT are set in epod files
	Given I have a clean database
	And I have selected branch '55'
	And I import the route file 'ROUTE_PLYM_BulkCredit.xml' into the well
	And I have loaded the order file 'ORDER_PLY_BulkCredit.xml' into the well
	And I have imported the following valid Epod files
	| Filename       |
	| ePOD__TBA1.xml |
	When I open the exception deliveries
	Then the following exception deliveries will be displayed
	| Route | Branch | Drop | InvoiceNo | Account   | AccountName | CreditValue | Status     | TBA |
	| 111   | 55     | 4    | 1715069   | 47020.053 | COSTCUTTER  | 0           | Incomplete | 3   |
	| 111   | 55     | 4    | 2845610   | 47020.053 | COSTCUTTER  | 158.46      | Incomplete | 0   |
	| 111   | 55     | 4    | 4800016   | 47663.040 | COSTCUTTER  | 25.32       | Incomplete | 0   |

Scenario: Each exception delivery should have at least one exception delivery line
   Given I have loaded the Adam route data
   And I have selected branches '22' and '2'
   And  All the deliveries are marked as clean
   And 20 deliveries have been marked as exceptions
   And I open the exception deliveries
   When I click on each of the deliveries on page 1 there will be at least one exception delivery line
   And I click on each of the deliveries on page 1 there will be at least one exception delivery line

