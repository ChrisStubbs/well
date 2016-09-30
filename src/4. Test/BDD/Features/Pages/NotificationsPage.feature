@WebDriverFeature
Feature: NotificationsPage
	As a well user
	I wish to be able to view and archive notifications 
	so that I can take action in the ADAM system

@mytag
Scenario: A user can page through notifications
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22 
	And 10 notifications have been made
	When I navigate to the notifications page
	Then I will have 4 pages of notification data
	When I click on notification page 4
	Then '1' rows of notification data will be displayed on page 4
	And the following notifications with a rowcount of '1' will be displayed on page 4 
	| Heading       | Account     | PicklistReference | InvoiceNumber | Contact       | Reason                        |
	| Credit failed | 0/2874.033  | 2544765           |               | Rhys Williams  | Credit failed ADAM validation |
	When I click on notification page 1
	Then the following notifications with a rowcount of '3' will be displayed on page 1 
	| Heading       | Account     | PicklistReference | InvoiceNumber | Contact       | Reason                        |
	| Credit failed | 0/49214.152 | 4294343           |               | CSG Contact 1 | Credit failed ADAM validation |
	| Credit failed | 0/2874.033  | 2545470           |               | CSG Contact 1 | Credit failed ADAM validation |
	| Credit failed | 0/2874.033  | 2545470           |               | GEN HOSPITAL  | Credit failed ADAM validation |
	When I click on notification page 2
	Then '3' rows of notification data will be displayed on page 2



Scenario: A user can archive a notification
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22 
	And 1 notifications have been made
	When I navigate to the notifications page
	Then I will have 1 pages of notification data
	When I archive the notification 1 from rowcount 1 on page 1
	Then I can see the following notification detail
	| ModalTitle											           |
	| Are you sure you want to archive the notification for 49214.152   | 
	When I click 'Yes' on the archive modal
	Then '0' rows of notification data will be displayed on page 1




