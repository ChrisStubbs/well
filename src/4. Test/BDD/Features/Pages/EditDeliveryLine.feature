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
	And I select a short source of 'Checker'
	And I select a short reason of 'Minimum Drop Charge'
	And I select a short action of 'Reject'
	And click add damage button
	And I enter a damage qty of '2' for id '0'
	And I enter a damage reason of 'Picking Error' for id '0'
	And I enter a damage source of 'Customer' for id '0'
	And I enter a damage action of 'Credit' for id '0'
	And I save the delivery line updates
	And I confirm the delivery line update
	Then the line '1' Short Qty is '5' and Damaged Qty is '2' Del Qty is '13'
	And the delivery status is 'Incomplete'
	When I open the audits page
	Then the following audit entries are shown
	| Entry                                                                                                                                                 | Type               | InvoiceNo | Account                          | DeliveryDate |
	| Product: 50035 - Ind Potato Gratin 400g. Short Qty changed from 0 to 5. Damages added Reason - Picking Error, Source - Customer, Action - Credit - 2. | DeliveryLineUpdate | 94294343  | 49214.152 - CSG - must be CF van | 07/01/2016   |
	
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
	| Product: 50035 - Ind Potato Gratin 400g. Short Qty changed from 1 to 0. | DeliveryLineUpdate | 94294343 | 49214.152 - CSG - must be CF van | 07/01/2016   | 

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

Scenario: Add short qty and damages to exception delivery
	Given 1 deliveries have been marked as exceptions	
	And I open the exception deliveries
	And I assign the delivery to myself
	And I open the exception delivery '1'
	And I click on the first delivery line
	When I enter a short quantity of '5'
	And I select a short source of 'Checker'
	And I select a short reason of 'Minimum Drop Charge'
	And I select a short action of 'Reject'
	And click add damage button
	And I enter a damage qty of '2' for id '0'
	And I enter a damage reason of 'Picking Error' for id '0'
	And I enter a damage source of 'Customer' for id '0'
	And I enter a damage action of 'Credit' for id '0'
	And I save the delivery line updates
	And I open the exception deliveries
	And I select the exception submit button
	Then I can see the product information '50035 Ind Potato Gratin 400g'
	And I can see the shortage quantity of '5'
	And I can see the shortage reason of 'Minimum Drop Charge'
	And I can see the shortage source of 'Checker'
	And I can see the shortage action of 'Reject'
	And I can see the damage quantity of '2'
	And I can see the damage reason of 'Picking Error'
	And I can see the damage source of 'Customer'
	And I can see the damage action of 'Credit'
