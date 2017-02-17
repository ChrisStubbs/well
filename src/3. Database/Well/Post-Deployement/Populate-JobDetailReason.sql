SET IDENTITY_INSERT dbo.JobDetailReason ON

MERGE INTO JobDetailReason AS Target
USING	(VALUES	(0,'NotDefined'),
				(1,'NoCredit'),
				(2,'DamagedGoods'),
				(3,'ShortDelivered'),
				(4,'BookingError'),
				(5,'PickingError'),
				(6,'OtherError'),
				(7,'Administration'),
				(8,'AccumulatedDamages'),
				(9,'RecallProduct'),
				(10,'CustomerDamaged'),
				(11,'ShortDated'),
				(12,'Vouchers'),
				(13,'SignedShort'),
				(14,'OutOfDateStock'),
				(15,'ShortTBA'),
				(16,'AvailabilityGuarantee'),
				(17,'FreezerChillerBreakdown'),
				(18,'NotEnoughRoom'),
				(19,'OutOfTemp'),
				(20,'DuplicateOrder'),
				(21,'NotOrdered'),
				(22,'ShopClosedNoStaff'),
				(23,'MinimumDropCharge')
		)
AS Source ([Id], [Description])
	ON Target.[Id] = Source.[Id]

WHEN NOT MATCHED BY TARGET THEN
	INSERT ([Id],[Description])
	VALUES ([Id],[Description]);

SET IDENTITY_INSERT JobDetailReason OFF