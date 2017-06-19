export interface IActionSubmitSummary {
    summary: string;
    items: IActionSubmitSummaryItem[];
    jobIds: number[];
}

export interface IActionSubmitSummaryItem
{
    identifier: string;
    totalCreditValue: number;
    totalActionValue: number;
    totalCreditQty: number;
    totalQty: number;
}