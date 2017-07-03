export interface  IAppSearchResultSummary
{
    stopIds: number[];
    routeIds: number[];
    invoices: IInvoiceSearchResult[];
}

export interface IInvoiceSearchResult  {
    branchId: number;
    invoiceNumber: string;
}
