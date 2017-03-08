@WebDriverFeature
@RoleSuperUser
Feature: AdamImportPage
	As a user
	I wish to be able to view and filter exception delivery information
	so that I can determine	which deliveries have been unsuccesful

Background: 
	Given I have a clean database
	And I have loaded the second Adam route data to check data to ADAM
	And I have loaded the Adam order data to check data to ADAM
	And I have imported a valid Epod update file named 'ePOD_Hay_Update2.xml'
