@WebDriverFeature
@RoleSuperUser
Feature: DocumentDeliveries

Background: 
	Given I have a clean database
	And I have loaded the Adam document delivery route data
	And I have imported a valid Epod update file named 'ePOD_30062016_DocumentDelivery.xml'
	And I have selected branch '22'

Scenario: Document Delivery NOT shown
	When I open the exception deliveries 
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo | Account  | AccountName          | Status     | TBA |
	| 001   | 1    | 976549    | 2874.033 | CSG - must be CF van | Incomplete | 0   |


