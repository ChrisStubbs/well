Feature: WellClean
	In order to import keep the most up to date data within the well
	As a user
	I want the system to be able to purge the well daily of any records that have been resolved

Background:
	 Given I have loaded the Adam route data


Scenario: Import epod file one job with 2 clean and 2 exception lines and resolve all
	Given I have imported a valid Epod update file named 'ePOD__20160701_10452212189454' with 2 clean and 2 exceptions
	And I resolve all of the exceptions with a JobId of 1
	When I start the ACL Well Clean process
	Then there should be 0 exception lines left for a Job with and Id or 1
