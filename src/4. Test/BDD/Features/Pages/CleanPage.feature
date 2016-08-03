@WebDriverFeature
Feature: CleanPage
	As a user
	I wish to be able to view and filter clean delivery information
	so that I can determine which deliveries have been succesful


Scenario: A user can view Clean Delivery Information
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22
	And  3 deliveries have been marked as clean
	When I open the clean deliveries 
	Then the following clean deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | Status   |
	| 001   | 01   | 949214.152 | 49214.152 | CSG - must be CF van | Complete |
	| 001   | 01   | 92874.033  | 2874.033  | CSG - must be CF van | Complete |
	| 001   | 02   | 92874.033  | 2874.033  | RVS SHOP             | Complete |

