@WebDriverFeature
@RoleSuperUser
Feature: Bulk Credit Feature
	As a user I need to be able to bulk credit Delievry Exceptions

Background: 
	Given I have a clean database
	And I have selected branch '55'
	And I import the route file 'ROUTE_PLYM_BulkCredit.xml' into the well
	And I have loaded the order file 'ORDER_PLY_BulkCredit.xml' into the well
	And I have imported the following valid Epod files
	| Filename              |
	| ePOD__BulkCredit1.xml |
	| ePOD__BulkCredit2.xml |
	| ePOD__BulkCredit3.xml |
	And I have the following credit threshold levels set in the database
	| Level | Value | Branch |
	| 1     | 1000  | 55     |
	| 2     | 30    | 55     |
	| 3     | 10    | 55     |
	When I open the exception deliveries
	Then the following exception deliveries will be displayed
	| Route | Branch | Drop | InvoiceNo | Account   | AccountName          | CreditValue | Status     | TBA |
	| 111   | 55     | 1    | 4800011   | 45649.000 | SHELL - TRERULEFOOT  | 5.4         | Incomplete | 0   |
	| 111   | 55     | 3    | 4800013   | 37432.000 | SHELL - KINGSLEY VIL | 5.4         | Incomplete | 0   |
	| 111   | 55     | 4    | 2845610   | 47020.053 | COSTCUTTER           | 158.46      | Incomplete | 0   |
	| 111   | 55     | 4    | 4800016   | 47663.040 | COSTCUTTER           | 25.32       | Incomplete | 0   |
	

	Scenario: A user with sufficient credit threshold set can bulk credit multiple
	Given I am assigned to credit threshold 'Level 1'
	And I assign the following exception lines to myself
	| LineNo |
	| 1      |
	| 2      |
	| 3      |
	| 4      |
	And I click the credit checkbox on the following lines
	| LineNo |
	| 1      |
	| 2      |
	| 3      |
	| 4      |
	When I click the Bulk Credit button
	And Select the Sources as 'Not Defined' and reason as 'Not Defined'
	And I click the bulk modal Confirm button
	Then the exception deliveries page will show No exceptions found
	When I open the resolved deliveries page
	Then the following resolved deliveries grid will be displayed
	| Route | Branch | Drop | InvoiceNo | Account   | AccountName          | Status   |
	| 111   | 55     | 1    | 4800011   | 45649.000 | SHELL - TRERULEFOOT  | Resolved |
	| 111   | 55     | 3    | 4800013   | 37432.000 | SHELL - KINGSLEY VIL | Resolved |
	| 111   | 55     | 4    | 2845610   | 47020.053 | COSTCUTTER           | Resolved |
	| 111   | 55     | 4    | 4800016   | 47663.040 | COSTCUTTER           | Resolved |

	

