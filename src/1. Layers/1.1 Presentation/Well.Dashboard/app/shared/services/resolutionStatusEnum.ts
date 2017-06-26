export enum ResolutionStatusEnum {
    Imported = 1,
    DriverCompleted = 2,
    ActionRequired = 4,
    PendingSubmission = 8,
    PendingApproval = 16,
    Approved = 32,
    Credited = 64,
    Resolved = 128,
    ClosedDriverCompleted = 258,
    ClosedCredited = 320,
    ClosedResolved = 384
}