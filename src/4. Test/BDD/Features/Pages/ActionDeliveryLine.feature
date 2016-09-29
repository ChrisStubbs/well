@WebDriverFeature
@RoleSuperUser
Feature: Action Delivery Line
	As a customer service agent
	I want to add actions to delivery lines
	so that exceptions on deliveries are resolved

Background: 
	Given I have a clean database
	And I have loaded the Adam route data
	And I have imported a valid Epod update file named 'ePOD_30062016_Update.xml'

Scenario: Add Actions to delivery line	
	Given an exception with 20 invoiced items is assigned to me
	And I view the Actions for line '1' of Delivery '1'
	When I add the 'Credit' action to 1 item
	And I add the 'Credit And Reorder' action to 1 item
	And I add the 'Replan In TranSend' action to 1 item
	And I add the 'Replan In Roadnet' action to 1 item
	And I add the 'Replan In The Queue' action to 1 item
	And I add the 'Reject' action to 1 item
	And I save the delivery line updates
	And I view the Actions for line '1' of Delivery '1'
	Then the following actions are shown on the delivery items
	| Quantity | Action              | Status |
	| 1        | Credit              | Draft  |
	| 1        | Credit And Reorder  | Draft  |
	| 1        | Replan In TranSend  | Draft  |
	| 1        | Replan In Roadnet   | Draft  |
	| 1        | Replan In The Queue | Draft  |
	| 1        | Reject              | Draft  |

Scenario: Submitted actions can not be changed
	Given an exception with a submitted action is assigned to me
	When I view the Actions for line '1' of Delivery '1' 
	Then I can not edit any action
