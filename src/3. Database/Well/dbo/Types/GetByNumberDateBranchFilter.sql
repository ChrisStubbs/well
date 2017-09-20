CREATE TYPE GetByNumberDateBranchFilter AS TABLE
(
	RouteNumber     VarChar(12) NOT NULL,
	RouteDate       DateTime NULL,
	BranchId        Int NOT NULL
)