@WebDriverFeature
@RoleSuperUser
Feature: HighValueItemsExceptionPage
	As a user
	I wish to be able to view and filter exception delivery information
	so that I can determine	which deliveries have been unsuccesful

Background: 
	Given I have a clean database
	And I have loaded the Adam route data
	And I have imported a valid Epod update file named 'ePOD_30062016_Update.xml'

	
Scenario: A user can view Exception Delivery Information with shorts to be advised displayed
	Given I have selected branch '22'
	And  2 deliveries have been marked as exceptions with shorts to be advised
	When I open the exception deliveries
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | Status     | TBA |
	| 001   | 1    | 94294343	| 49214.152 | CSG - must be CF van | Incomplete | 2   |
	| 001   | 1    | 92545470	| 2874.033  | CSG - must be CF van | Incomplete | 2   |
	When I open the exception deliveries
	And I click on exception row 1
	Then I am shown the exception detail
   | LineNo | Product | Description            | Value | InvoiceQuantity | DeliveryQuantity | DamagedQuantity | ShortQuantity |
   | 1      | 50035   | Ind Potato Gratin 400g | 39.95 | 1               | -3               | 2               | 2             |
   
Scenario: View exception details at lower level with delivery check icon displayed
	Given I have selected branch '22'
	And All the deliveries are marked as exceptions
	And All delivery lines are flagged with line delivery status 'Exception'
	When I open the exception deliveries
	And I click on exception row 4
	Then I am shown the exception detail
	| LineNo | Product | Description              | Value | InvoiceQuantity | DeliveryQuantity | DamagedQuantity | ShortQuantity | Checked |
	| 1      | 6987    | Choc Teacakes Tunnock    | 19.23 | 1               | 0               | 0               | 1             | true    |
	| 2      | 49179   | Ginger Nuts 250g         | 4.88  | 1               | 0               | 0               | 1             | true    |
	| 3      | 21633   | Kiddies Super Mix 220gPM | 3.60  | 1               | 0               | 0               | 1             | true    |
	| 4      | 4244    | Milkybar Btns Giant PM   | 5.60  | 1               | 0               | 0               | 1             | true    |
	| 5      | 7621    | Fruit Past Tube 52.5g    | 8.40  | 1               | 0               | 0               | 1             | true    |
	And I am shown the high value check