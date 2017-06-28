export interface IBulkEditSummary
{
    message: string;
    items: IBulkEditItem[];
}

export interface IBulkEditItem
{
    jobId: number;
    invoice: string;
    type: string;
    account: string;
    shortQuantity: number;
    damageQuantity: number;
    bypassQuantity: number;
    totalValue: number;
}

export interface IBulkEditPatchRequest
{
    deliveryAction: number;
    source: number;
    reason: number;
    jobIds: number[];
    lineItemIds: number[];
}

export interface IBulkEditResult
{
    statuses: IBulkEditResolutionStatus[];
    lineItemIds:number[];
}

export interface IBulkEditResolutionStatus
{
    jobId: number;
    status: any;
}