@WebDriverFeature
@RoleSuperUser
Feature: Audit viewing
	As a user
	I wish to be able to view and filter audits
	so that I can see what activities users have performed in The Well

Background: 
	Given I have a clean database

Scenario: Audit paging
	Given 25 audit entries have been made
	When I open the audits page
	Then '10' rows of audit data will be displayed	
	When I click on audit page 3
	Then '5' rows of audit data will be displayed

Scenario: Audit filtering
	Given 5 audit entries have been made
	When I open the audits page	
	Then the following audit entries are shown
	| Entry     | Type               | InvoiceNo | Account | DeliveryDate |
	| Audit 123 | DeliveryLineUpdate | 987654    | 123456  | 20/01/2016   |
	| Audit 123 | DeliveryLineUpdate | 987654    | 123456  | 20/01/2016   |
	| Audit 456 | DeliveryLineUpdate | 55555     | 88888   | 15/05/2016   |
	| Audit 456 | DeliveryLineUpdate | 55555     | 88888   | 15/05/2016   |
	| Audit 456 | DeliveryLineUpdate | 55555     | 88888   | 15/05/2016   |
	
	When I filter the audits grid with the option 'Invoice No' and value '987654'
	Then the following audit entries are shown
	| Entry     | Type               | InvoiceNo | Account | DeliveryDate |
	| Audit 123 | DeliveryLineUpdate | 987654    | 123456  | 20/01/2016   |
	| Audit 123 | DeliveryLineUpdate | 987654    | 123456  | 20/01/2016   |

		
	When I filter the audits grid with the option 'Account' and value '88888'
	Then the following audit entries are shown
	| Entry     | Type               | InvoiceNo | Account | DeliveryDate |
	| Audit 456 | DeliveryLineUpdate | 55555     | 88888   | 15/05/2016   |
	| Audit 456 | DeliveryLineUpdate | 55555     | 88888   | 15/05/2016   |
	| Audit 456 | DeliveryLineUpdate | 55555     | 88888   | 15/05/2016   |

	When I filter the audits grid with the option 'Delivery Date' and value '15/05/2016'
	Then the following audit entries are shown
	| Entry     | Type               | InvoiceNo | Account | DeliveryDate |
	| Audit 456 | DeliveryLineUpdate | 55555     | 88888   | 15/05/2016   |
	| Audit 456 | DeliveryLineUpdate | 55555     | 88888   | 15/05/2016   |
	| Audit 456 | DeliveryLineUpdate | 55555     | 88888   | 15/05/2016   |

