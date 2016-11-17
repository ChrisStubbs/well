Feature: AdamRouteFileSchemaValidation
	In order to import correctly formed ADAM route files
	As a user
	I want the system to be able to validate exsisting ADAM route files against a pre defined schema

@Ignore
Scenario: Import ADAM route file with the CompanyID node missing from the first route header node
	Given I have an invalid ADAM route file 'PH_ROUTES_MissingCompanyNode.xml' with a 'RouteHeader' node at position '0' with the 'CompanyID' node missing
	When I import the route file 'PH_ROUTES_MissingCompanyNode.xml' into the well
	Then The schema validation error should be "file PH_ROUTES_MissingCompanyNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'RouteHeader' has invalid child element 'RouteNumber'. List of possible elements expected: 'CompanyID'."

@Ignore
Scenario: Import ADAM route file with the PlannedStopNumber missing from the first stop node
	Given I have an invalid ADAM route file 'PH_ROUTES_MissingPlannedStopNumberNode.xml' with a 'Stop' node at position '0' with the 'PlannedStopNumber' node missing
	When I import the route file 'PH_ROUTES_MissingPlannedStopNumberNode.xml' into the well
	Then The schema validation error should be "file PH_ROUTES_MissingPlannedStopNumberNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'Stop' has invalid child element 'PlannedArriveTime'. List of possible elements expected: 'PlannedStopNumber'."
@Ignore
Scenario: Import ADAM route file with the Code missing from the Account node child of the first stop node
	Given I have an invalid ADAM route file 'PH_ROUTES_MissingCodeFromAccountNode.xml' with a 'Account' node at position '0' with the 'Code' node missing
	When I import the route file 'PH_ROUTES_MissingCodeFromAccountNode.xml' into the well
	Then The schema validation error should be "file PH_ROUTES_MissingCodeFromAccountNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'Account' has incomplete content. List of possible elements expected: 'Code'."
@Ignore
Scenario: Import ADAM route file with the JobRef1 missing from the Job node child of the first Jobs node
	Given I have an invalid ADAM route file 'PH_ROUTES_MissingJobRef1FromJobNode.xml' with a 'Job' node at position '0' with the 'PHAccount' node missing
	When I import the route file 'PH_ROUTES_MissingJobRef1FromJobNode.xml' into the well
	Then The schema validation error should be "file PH_ROUTES_MissingJobRef1FromJobNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'Job' has invalid child element 'PickListRef'. List of possible elements expected: 'PHAccount'."

@Ignore
Scenario: Import ADAM route file with the Barcode missing from the JobDetail node child of the first JobDetails node
	Given I have an invalid ADAM route file 'PH_ROUTES_MissingBarcodeFromJobDetailNode.xml' with a 'JobDetail' node at position '0' with the 'PHProductCode' node missing
	When I import the route file 'PH_ROUTES_MissingBarcodeFromJobDetailNode.xml' into the well
	Then The schema validation error should be "file PH_ROUTES_MissingBarcodeFromJobDetailNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'JobDetail' has invalid child element 'OriginalDespatchQty'. List of possible elements expected: 'PHProductCode'."
@Ignore
Scenario: Import ADAM route file with the additional CompanyName node added to the first first route header node
	Given I have an invalid ADAM route file 'PH_ROUTES_AdditionalCompanyNameNode.xml' with a 'RouteHeader' node at position '0' with a 'CompanyName2' node added with a value of 'NewCompanyName'
	When I import the route file 'PH_ROUTES_AdditionalCompanyNameNode.xml' into the well
	Then The schema validation error should be "file PH_ROUTES_AdditionalCompanyNameNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'RouteHeader' has invalid child element 'CompanyName2'."
@Ignore
Scenario: Import ADAM route file with a duplicate TransportOrderRef node added to the first first Stop node
	Given I have an invalid ADAM route file 'PH_ROUTES_30062016_02_AddedTransportRef.xml' with a 'Stop' node at position '0' with a 'TransportOrderRef' node added with a value of '001 01 49214.152 01/07/2016'
	When I import the route file 'PH_ROUTES_30062016_02_AddedTransportRef.xml' into the well
	Then The schema validation error should be "file PH_ROUTES_30062016_02_AddedTransportRef.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'Stop' has invalid child element 'TransportOrderRef'."
@Ignore
Scenario: Import ADAM route file with a duplicate JobRef1 node added to the first first job node
	Given I have an invalid ADAM route file 'PH_ROUTES_30062016_02_AddedJobRef.xml' with a 'Job' node at position '0' with a 'PHAccount' node added with a value of '49214.152'
	When I import the route file 'PH_ROUTES_30062016_02_AddedJobRef.xml' into the well
	Then The schema validation error should be "file PH_ROUTES_30062016_02_AddedJobRef.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'Job' has invalid child element 'PHAccount'."
