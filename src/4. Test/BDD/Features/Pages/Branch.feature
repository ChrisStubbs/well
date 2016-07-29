﻿@WebDriverFeature
Feature: Branch
	In order to view the branch deliveries
	As a well user
	I want to be able to determine what branches I can see

Scenario: A user sets all branches to themselves
	Given I have a clean database
	When I navigate to the branches page
	And I select all the branches
	And I save my branches
    And select branches selection
    Then all the branches are selected
