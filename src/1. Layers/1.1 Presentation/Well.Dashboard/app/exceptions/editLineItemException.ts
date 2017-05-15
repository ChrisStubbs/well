export interface IEditLineItemException
{
    id: number;
    productNumber: string;
    product: string;
    originator: string;
    exception: string;
    invoiced?: number;
    delivered?: number;
    quantity: number;
    action: string;
    source: string;
    reason: string;
    erdd?: Date;
    actionedBy: string;
    approvedBy: string;
    comments: Array<string>;
}