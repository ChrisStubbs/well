@ignore
Feature: EpodRouteFileSchemaValidation
	In order to import correctly formed Epod route files
	As a math idiot
	I want to be able to validate exsisting Epod route files against a pre defined schema

Background:
	 Given I have loaded the Adam route data


Scenario: Import Epod route file with the JobDetailID child node missing from the first JobDetailDamage node
	Given I have an invalid Epod route file 'ePOD_MissingJobDetailIdNode.xml' with a 'JobDetailDamage' node at position '0' with the 'JobDetailID' node missing
	When I import the route file 'ePOD_MissingJobDetailIdNode.xml' into the well
	Then The schema validation error should be "file ePOD_MissingJobDetailIdNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'JobDetailDamage' has invalid child element 'DamageReasonID'. List of possible elements expected: 'JobDetailID'."

Scenario: Import Epod route file with the Qty child node missing from the first JobDetailDamage node
	Given I have an invalid Epod route file 'ePOD_MissingQtyNode.xml' with a 'JobDetailDamage' node at position '0' with the 'Qty' node missing
	When I import the route file 'ePOD_MissingQtyNode.xml' into the well
	Then The schema validation error should be "file ePOD_MissingQtyNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'JobDetailDamage' has invalid child element 'Deleted'. List of possible elements expected: 'Qty'."

Scenario: Import Epod route file with the ReasonCode child node missing from the first Reason node
	Given I have an invalid Epod route file 'ePOD_MissingReasonCodeNode.xml' with a 'Reason' node at position '0' with the 'ReasonCode' node missing
	When I import the route file 'ePOD_MissingReasonCodeNode.xml' into the well
	Then The schema validation error should be "file ePOD_MissingReasonCodeNode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'Reason' has invalid child element 'Description'. List of possible elements expected: 'ReasonCode'."

Scenario: Import Epod route file with a duplicate ReasonCode node added to the first first JobDamageDetail node
	Given I have an invalid Epod route file 'ePOD__AdditionalDamageReasonCode.xml' with a 'Reason' node at position '0' with a 'ReasonCode' node added with a value of 'CAR01'
	When I import the route file 'ePOD__AdditionalDamageReasonCode.xml' into the well
	Then The schema validation error should be "file ePOD__AdditionalDamageReasonCode.xml failed schema validation with the following: System.Xml.XsdValidatingReader:	The element 'Reason' has invalid child element 'ReasonCode'."
