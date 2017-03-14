@WebDriverFeature
@RoleSuperUser
Feature: CompletedOnPaperDeliveries

Background: 
	Given I have a clean database
	And I have loaded the MultiDate Adam route data
	And I have imported a valid Epod update file named 'ePOD_30062016_CompletedOnPaperDelivery.xml'
	And I have selected branch '22'

Scenario: Deliveries completed on paper are shown as exceptions 
	When I open the exception deliveries
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | Status             | TBA |
	| 001   | 1    | 976549    | 49214.152 | CSG - must be CF van | Completed On Paper | 0   |
	| 001   | 1    | 1000123   | 02874.033 | CSG - must be CF van | Completed On Paper | 0   |
