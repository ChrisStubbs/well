export class UserJobs {
    public userName: string;
    public jobIds: number[];
    public allocatePendingApprovalJobs: boolean;
}

export class AssignJobResult {
    public success: boolean;
    public message: string;
}