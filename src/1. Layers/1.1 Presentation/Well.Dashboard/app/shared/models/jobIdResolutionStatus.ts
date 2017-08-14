import {IResolutionStatus} from './resolutionStatus';

export interface IJobIdResolutionStatus
{
    jobId: number;
    status: IResolutionStatus;
}