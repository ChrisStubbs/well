@Ignore
Feature: WellClean
	To prevent system performance from degrading over time 
	As a Well Application
	I will regularly purge clean and resolved deliveries from the Well

#Amend this to include deliveries in all statuses that should be deleted 'Complete, Resolved, Authorised ByPass, Non Authorised ByPass'
Scenario: All resolved deliveries are SOFT deleted
	Given I have a database with Adam/Epod data
	And I resolve all of the exceptions with a JobId of 1
	When I start the ACL Well Clean process for a soft delete
	Then there should be 0 exception lines left for a Job with an Id of 1

#Amend this to include deliveries in all statuses that should not be deleted 'Not Arrived, Not Done, Incomplete, Not Defined'
Scenario: Delivery exceptions are not SOFT deleted
	Given I have a database with Adam/Epod data
	And I resolve one of the exceptions with a JobId of 1
	When I start the ACL Well Clean process for a soft delete
	Then there should be 1 exception lines left for a Job with an Id of 1

#Amend this to include deliveries in all statuses, so it tests 'All soft deleted deliveries are hard deleted after 3 months'
Scenario: All resolved deliveries are HARD deleted after 4 months
	Given I have a database with Adam/Epod data
	And I resolve all of the exceptions with a JobId of 1
	When I start the ACL Well Clean process for a date 4 months from today
	Then there should be 0 lines left for a Job with an Id of 1

Scenario: Delivery exceptions over 3 months old are not HARD deleted
	Given I have a database with Adam/Epod data
	And I resolve one of the exceptions with a JobId of 1
	When I start the ACL Well Clean process for a date 4 months from today
	Then there should be 1 lines left for a Job with an Id of 1

Scenario: Resolved Deliveries created within with royalty exception time are not soft deleted
	Given I have a database with Adam/Epod data
	And I have an exception royalty of 5 days for royalty 1871
	When I start the ACL Well Clean process for a soft delete
	Then there should be 2 exception lines left for a Job with an Id of 1

#Scenario: Resolved Deliveries created outside the royalty exception time are soft deleted

