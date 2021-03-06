export interface ISubmitActionModel 
{
    submit: boolean;
    jobIds: number[];
}

export interface ISubmitActionResult {
    message: string;
    isValid: boolean;
    warnings: string[];
    details: ISubmitActionResultDetails[];
}

export interface ISubmitActionResultDetails
{
    jobId: number;
    resolutionStatusId: number;
    resolutionStatusDescription: string;
}