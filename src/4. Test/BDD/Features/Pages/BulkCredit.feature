Feature: Bulk Credit Feature
	As a user I need to be able to bulk credit Delievry Exceptions

Background: 
	Given I have a clean database
	And I import the route file 'ROUTE_PLYM_BulkCredit.xml' into the well
	And I have loaded the order file 'ORDER_PLY_BulkCredit.xml' into the well
	And I have imported the following valid Epod files
	| Filename              |
	| ePOD__BulkCredit4.xml |
	| ePOD__BulkCredit3.xml |
	| ePOD__BulkCredit2.xml |
	| ePOD__BulkCredit1.xml |

	Scenario: A user with sufficient credit threshold set can bulk credit
