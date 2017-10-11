export class UserJobs {
    public userId: number;
    public jobIds: number[];
    public allocatePendingApprovalJobs: boolean;
}

export class AssignJobResult {
    public success: boolean;
    public message: string;
}