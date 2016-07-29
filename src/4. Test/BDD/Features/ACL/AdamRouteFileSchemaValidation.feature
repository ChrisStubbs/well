Feature: AdamRouteFileSchemaValidation
	In order to import correctly formed ADAM route files
	As a math idiot
	I want to be able to validate exsisting ADAM route files against a pre defined schema

Scenario: Import ADAM route file with the CompanyID node missing from the first route header node
	Given I have an invalid ADAM route file 'PH_ROUTES_MissingCompanyNode.xml' with a 'RouteHeader' node at position '0' with the 'CompanyID' node missing
	When I import the route file 'PH_ROUTES_MissingCompanyNode.xml' into the well
	Then The schema validation error should be "file PH_ROUTES_MissingCompanyNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'RouteHeader' has invalid child element 'RouteNumber'. List of possible elements expected: 'CompanyID'."

Scenario: Import ADAM route file with the PlannedStopNumber missing from the first stop node
	Given I have an invalid ADAM route file 'PH_ROUTES_MissingPlannedStopNumberNode.xml' with a 'Stop' node at position '0' with the 'PlannedStopNumber' node missing
	When I import the route file 'PH_ROUTES_MissingPlannedStopNumberNode.xml' into the well
	Then The schema validation error should be "file PH_ROUTES_MissingPlannedStopNumberNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'Stop' has invalid child element 'PlannedArriveTime'. List of possible elements expected: 'PlannedStopNumber'."

Scenario: Import ADAM route file with the Code missing from the Account node child of the first stop node
	Given I have an invalid ADAM route file 'PH_ROUTES_MissingCodeFromAccountNode.xml' with a 'Account' node at position '0' with the 'Code' node missing
	When I import the route file 'PH_ROUTES_MissingCodeFromAccountNode.xml' into the well
	Then The schema validation error should be "file PH_ROUTES_MissingCodeFromAccountNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'Account' has invalid child element 'AccountTypeCode'. List of possible elements expected: 'Code'."

Scenario: Import ADAM route file with the JobRef1 missing from the Job node child of the first Jobs node
	Given I have an invalid ADAM route file 'PH_ROUTES_MissingJobRef1FromJobNode.xml' with a 'Job' node at position '0' with the 'JobRef1' node missing
	When I import the route file 'PH_ROUTES_MissingJobRef1FromJobNode.xml' into the well
	Then The schema validation error should be "file PH_ROUTES_MissingJobRef1FromJobNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'Job' has invalid child element 'JobRef2'. List of possible elements expected: 'JobRef1'."

Scenario: Import ADAM route file with the Barcode missing from the JobDetail node child of the first JobDetails node
	Given I have an invalid ADAM route file 'PH_ROUTES_MissingBarcodeFromJobDetailNode.xml' with a 'JobDetail' node at position '0' with the 'Barcode' node missing
	When I import the route file 'PH_ROUTES_MissingBarcodeFromJobDetailNode.xml' into the well
	Then The schema validation error should be "file PH_ROUTES_MissingBarcodeFromJobDetailNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'JobDetail' has invalid child element 'OriginalDespatchQty'. List of possible elements expected: 'Barcode'."
