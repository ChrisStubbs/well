@WebDriverFeature
@RoleSuperUser
Feature: BypassedDeliveries

Background: 
	Given I have a clean database
	And I have loaded the MultiDate Adam route data
	And I have imported a valid Epod update file named 'ePOD_30062016_BypassedDelivery.xml'
	And I have selected branch '22'

Scenario: Bypassed Delivery shown on Exceptions page
	When I open the exception deliveries 
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | Status   | TBA |
	| 001   | 1    | 976549    | 49214.152 | CSG - must be CF van | Bypassed | 0   |
	| 001   | 1    | 1000123   | 02874.033 | CSG - must be CF van | Bypassed | 0   |

Scenario: Bypassed deliveries with exceptions can be resolved 
	Given 1 delivery has all its lines set to close
	And I open the exception deliveries
	When I select the exception submit button
	And I confirm the exception submit
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | Status   | TBA |
	| 001   | 1    | 1000123   | 02874.033 | CSG - must be CF van | Bypassed | 0   |

Scenario: Bypassed deliveries without exceptions can be resolved 
	Given I open the exception deliveries
	When I assign the delivery on row 2 to myself
	And I select the exception submit button on Row '2'
	And I confirm the exception submit
	Then the following exception deliveries will be displayed
	| Route | Drop | InvoiceNo | Account   | AccountName          | Status   | TBA |
	| 001   | 1    | 976549    | 49214.152 | CSG - must be CF van | Bypassed | 0   |
