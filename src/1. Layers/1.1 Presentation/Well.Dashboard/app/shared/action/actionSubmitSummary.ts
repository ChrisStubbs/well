export interface IActionSubmitSummary {
    summary: string;
    items: IActionSubmitSummaryItem[];
    jobIds: number[];
}

export interface IActionSubmitSummaryItem
{
    identifier: string;
    noOfItems: number;
    qty: number;
    value: number;
}