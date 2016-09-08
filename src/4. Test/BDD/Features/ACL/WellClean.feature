Feature: WellClean
	In order to import keep the most up to date data within the well
	As a user
	I want the system to be able to purge the well daily of any records that have been resolved



Scenario: Import epod file one job with 2 clean and 2 exception lines and resolve all for soft delete
	Given I have a database with Adam/Epod data
	And I resolve all of the exceptions with a JobId of 1
	When I start the ACL Well Clean process for a soft delete
	Then there should be 0 exception lines left for a Job with an Id of 1

Scenario: Import epod file one job with 2 clean and 2 exception lines and resolve one for soft delete
	Given I have a database with Adam/Epod data
	And I resolve one of the exceptions with a JobId of 1
	When I start the ACL Well Clean process for a soft delete
	Then there should be 1 exception lines left for a Job with an Id of 1

Scenario: Import epod file one job with 2 clean and 2 exception lines and resolve all for hard delete
	Given I have a database with Adam/Epod data
	And I resolve all of the exceptions with a JobId of 1
	When I start the ACL Well Clean process for a date 4 months from today
	Then there should be 0 lines left for a Job with an Id of 1

Scenario: Import epod file one job with 2 clean and 2 exception lines and resolve one for hard delete
	Given I have a database with Adam/Epod data
	And I resolve one of the exceptions with a JobId of 1
	When I start the ACL Well Clean process for a date 4 months from today
	Then there should be 1 lines left for a Job with an Id of 1

Scenario: Import epod file one job with current exception royalty for soft delete
	Given I have a database with Adam/Epod data
	And I have an exception royalty of 5 days for royalty 1871
	When I start the ACL Well Clean process for a soft delete
	Then there should be 2 exception lines left for a Job with an Id of 1



