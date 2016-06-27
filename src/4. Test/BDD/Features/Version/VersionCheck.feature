Feature: VersionCheck
	In order to know which API version I'm using
	As a user
	I want to query the API version

@mytag
Scenario: Check API version
	When I get the API version
	Then the response code is '200' OK
	And a version number is returned
