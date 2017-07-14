export interface IPatchSummary
{
    message: string;
    manuallyCompleteMessage: string;
    items: IPatchSummaryItem[];
    noOfJobs: number;
    totalDispatchedQuantity: number;
    totalDispatchedValue: number;
}

export interface IPatchSummaryItem
{
    jobId: number;
    invoice: string;
    type: string;
    account: string;
    shortQuantity: number;
    damageQuantity: number;
    bypassQuantity: number;
    totalExceptionValue: number;
    totalDispatchedQuantity: number;
    totalDispatchedValue: number;
}