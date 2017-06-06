export class EditLineItemException
{
    constructor()
    {
        this.isSelected = false;
        this.isExpanded = false;
    }

    public isSelected: boolean;
    public id: number;
    public productNumber: string;
    public product: string;
    public invoiced?: number;
    public delivered?: number;
    public quantity: number;
    public exceptions: Array<EditLineItemExceptionDetail>
    public isExpanded: boolean;
}

export class EditLineItemExceptionDetail
{
    public id: number;
    public lineItemId: number;
    public quantity: number;
    public originator: string;
    public exception: string;
    public action: string;
    public source: string;
    public reason: string;
    public erdd?: Date;
    public actionedby: string;
    public approvedby: string;
    public comments: Array<string>
}