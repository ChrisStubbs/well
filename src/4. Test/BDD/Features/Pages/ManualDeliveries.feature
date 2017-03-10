@WebDriverFeature
@RoleSuperUser
Feature: ManualDeliveries
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'


Scenario: Standard Delivery with shorts shown as exception
	Given I have imported a valid Epod update file named 'ePOD_30062016_Update.xml'
	When I open the exception deliveries 
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | Status     | TBA |
	| 001   | 1    | 94294343  | 49214.152 | CSG - must be CF van | Incomplete | 0   |


Scenario: Manual Delivery with shorts NOT shown as exception
	Given I have imported a valid Epod update file named 'ePOD_30062016_ManualDelivery.xml'
	When I open the exception deliveries 
	Then no exceptions are displayed 


