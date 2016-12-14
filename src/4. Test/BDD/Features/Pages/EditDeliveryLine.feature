@WebDriverFeature
@RoleSuperUser
Feature: Edit delivery line
	As a customer service agent
	I wish to be able to edit short qtys and damages on deliveries 
	so that I can fix any discrepancies raised by customers

Background: 
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch '22'

Scenario: Add short qty and damages to clean delivery
	Given 1 deliveries have been marked as clean	
	And I open the clean deliveries
	And I assign the clean delivery to myself
	And I open the clean delivery '1'
	And I click on the first delivery line
	When I enter a short quantity of '5'
	And I add a damage qty of '2' and reason 'Picking Error'
	And I save the delivery line updates
	And I confirm the delivery line update
	Then the line '1' Short Qty is '5' and Damaged Qty is '2' Del Qty is '13'
	And the delivery status is 'Incomplete'
	When I open the audits page
	Then the following audit entries are shown
	| Entry                                                                                                    | Type               | InvoiceNo  | Account                          | DeliveryDate |
	| Product: 50035 - Ind Potato Gratin 400g. Short Qty changed from 0 to 5. Damages added PickingError - 2. | DeliveryLineUpdate | 949214.152 | 49214.152 - CSG - must be CF van | 07/01/2016   |


Scenario: Remove short qty and damages from exception delivery
	Given I have imported a valid Epod update file named 'ePOD_one_exception.xml'
	And 1 deliveries have been marked as exceptions
	And I open the exception deliveries
	And I assign the delivery to myself
	And I open the exception delivery '1'
	And I click on the first delivery line
	When I enter a short quantity of '0'
	And I save the delivery line updates
	And I confirm the delivery line update
	And I open the clean tab
	Then the line '1' Short Qty is '0' and Damaged Qty is '0' Del Qty is '20'
	And the delivery status is 'Resolved'
	When I open the audits page
	Then the following audit entries are shown
	| Entry                                                                                                                                | Type               | InvoiceNo  | Account                          | DeliveryDate |
	| Product: 50035 - Ind Potato Gratin 400g. Short Qty changed from 1 to 0. | DeliveryLineUpdate | 949214.152 | 49214.152 - CSG - must be CF van | 07/01/2016   | 

Scenario: Can not edit unassigned delivery line
	Given I have imported a valid Epod update file named 'ePOD_30062016_Update.xml'
	And 1 deliveries have been marked as exceptions	
	When I view the Issues for line '1' of Delivery '1'
	Then I cannot add or edit any shorts or damages

Scenario: Can not edit delivery line assigned to another user
	Given I have imported a valid Epod update file named 'ePOD_30062016_Update.xml'
	And 1 deliveries have been marked as exceptions
	And the exception is assigned to identity: 'palmerharvey\Bruno.Dobson', name: 'Bruno Dobson'
	When I view the Issues for line '1' of Delivery '1'
	Then I cannot add or edit any shorts or damages