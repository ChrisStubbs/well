import {IJobIdResolutionStatus} from '../models/jobIdResolutionStatus';

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
    statuses: IJobIdResolutionStatus[];
    lineItemIds:number[];
}

