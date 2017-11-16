export interface IManualCompletionRequest
{
    jobIds: number[];
    manualCompletionType: ManualCompletionType;
}
export enum ManualCompletionType
{
    CompleteAsClean = 1,
    CompleteAsBypassed = 2
}
