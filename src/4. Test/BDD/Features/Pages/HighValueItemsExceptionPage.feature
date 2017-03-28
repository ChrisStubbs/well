@WebDriverFeature
@RoleSuperUser
Feature: HighValueItemsExceptionPage
	As a user
	I wish to be able to view and filter exception delivery information
	so that I can determine	which deliveries have been unsuccessful

Background: 
	Given I have a clean database
	And I have loaded the Adam high value route data
	And I have loaded the Adam order data with high value lines
	And I have imported a valid Epod update file named 'ePOD__20170317_HIGHVALUE.xml'

	## 
#Scenario: A user can view Exception Delivery Information with shorts to be advised displayed
#	Given I have selected branch '55'
	##And  2 deliveries have been marked as exceptions with shorts to be advised
	#When I open the exception deliveries
	#Then the following exception deliveries will be displayed
	#| Route | Drop | InvoiceNo | Account   | AccountName          | Status     | TBA |
	#| 001   | 1    | 94294343    | 49214.152 | CSG - must be CF van | Incomplete | 2   |
	#| 001   | 1    | 976549    | 2874.033  | CSG - must be CF van | Incomplete | 2   |
	#When I open the exception deliveries
	#And I click on exception row 1
	#Then I am shown the exception detail
 #  | LineNo | Product | Description            | Value | InvoiceQuantity | DeliveryQuantity | DamagedQuantity | ShortQuantity |
 #  | 1      | 50035   | Ind Potato Gratin 400g | 39.95 | 1               | -3               | 2               | 2             |
   
Scenario: View exception details at lower level with delivery check icon displayed
	Given I have selected branch '55'
	When I open the exception deliveries
	And I click on exception row 3
	Then I am shown the exception detail
	| LineNo | Product | Description              | Value | InvoiceQuantity | DeliveryQuantity | DamagedQuantity | ShortQuantity | Checked |
	| 1      | 69603   | Carlsberg Exp 4.8% 4Pk   | 23.33 | 1               | 0                | 0               | 1             | true    |
	And I am shown the high value check