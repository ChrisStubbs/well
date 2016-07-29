﻿@WebDriverFeature
Feature: Resolved Delivery Page
	As a user
	I wish to be able to view and filter resolved delivery information
	so that I can determine which deliveries have been resolved

Scenario: A user can view Resolved Delivery Information
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22
	And  3 deliveries have been marked as Resolved
	When I open the resolved deliveries page
	Then the following resolved deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | Status   | Action | Assigned |
	| 001   | 01   | 949214.152 | 49214.152 | CSG - must be CF van | Resolved |        |          |
	| 001   | 01   | 92874.033  | 2874.033  | CSG - must be CF van | Resolved |        |          |
	| 001   | 02   | 92874.033  | 2874.033  | RVS SHOP             | Resolved |        |          |

Scenario: A user can filter Resolved Delivery information
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22
	And  All the deliveries are marked as Resolved
	When I open the resolved deliveries page
	And I filter the resolved delivery grid with the option 'Route' and value '006'
	Then the following resolved deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | Status   | Action | Assigned |
	| 006   | 01   | 943362.048 | 43362.048 | WB - SHOP            | Resolved |        |          |
	| 006   | 01   | 92874.033  | 2874.033  | WB - SHOP            | Resolved |        |          |
	| 006   | 02   | 954107.000 | 54107.000 | WB - SHELL FORECOURT | Resolved |        |          |
	| 006   | 02   | 954107.000 | 54107.000 | WB - SHELL FORECOURT | Resolved |        |          |
	When I filter the resolved delivery grid with the option 'Drop' and value '03'
	Then the following resolved deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName        | Status   | Action | Assigned |
	| 011   | 03   | 954107.000 | 54107.000 | WB - WAITROSE SHOP | Resolved |        |          |
	| 011   | 03   | 954107.000 | 54107.000 | WB - WAITROSE SHOP | Resolved |        |          |
	When I filter the resolved delivery grid with the option 'Invoice No' and value '949214.152'
	Then the following resolved deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName          | Status   | Action | Assigned |
	| 001   | 01   | 949214.152 | 49214.152 | CSG - must be CF van | Resolved |        |          |
	When I filter the resolved delivery grid with the option 'Account' and value '28398.080'
	Then the following resolved deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName   | Status   | Action | Assigned |
	| 011   | 05   | 928398.080 | 28398.080 | TESCO EXPRESS | Resolved |        |          |
	When I filter the resolved delivery grid with the option 'Account Name' and value 'WB - SHOP'
	Then the following resolved deliveries will be displayed
	| Route | Drop | InvoiceNo  | Account   | AccountName | Status   | Action | Assigned |
	| 006   | 01   | 943362.048 | 43362.048 | WB - SHOP   | Resolved |        |          |
	| 006   | 01   | 92874.033  | 2874.033  | WB - SHOP   | Resolved |        |          |

Scenario: A user can page through Resolved Delivery information
	Given I have a clean database
	And I have loaded the Adam route data
	And I have selected branch 22
	And  All the deliveries are marked as Resolved
	When I open the resolved deliveries page
	Then '10' rows of resolved delivery data will be displayed
	And I will have 2 pages of resolved delivery data
	When I click on resolved delivery page 2
	Then '7' rows of resolved delivery data will be displayed
	When I click on resolved delivery page 1
	Then '10' rows of resolved delivery data will be displayed