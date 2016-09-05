@WebDriverFeature
Feature: Edit delivery line
	As a customer service agent
	I wish to be able to edit short qtys and damages on deliveries 
	so that I can fix any discrepancies raised by customers

Background: 
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22
	#And I have imported a valid Epod update file named 'ePOD_30062016_Update.xml'

Scenario: Add short qty and damages to clean delivery
	And 1 deliveries have been marked as clean
	Given I open delivery '1'
	And I click on the first delivery line
	When I add a short quantity of '5'
	And I add a damage qty of '2' and reason 'CAR01 - Dented'
	And I save the delivery line updates
	And I confirm the delivery line update
	Then the line '1' Short Qty is '5' and Damaged Qty is '2' Del Qty is '13'
	And the delivery status is 'Incomplete'

#Scenario: Remove short qty and damages to exception delivery
