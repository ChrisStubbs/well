export interface IManualCompletionRequest
{
    jobIds: number[];
    manualCompletionType: ManualCompletionType;
}
export enum ManualCompletionType
{
    CompleteAsClean,
    CompleteAsBypassed
}
