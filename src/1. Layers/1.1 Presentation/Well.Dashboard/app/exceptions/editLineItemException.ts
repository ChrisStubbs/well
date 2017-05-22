export class EditLineItemException
{
    constructor()
    {
        this.isSelected = false;
    }

    public id: number;
    public lineItemActionId?: number;
    public productNumber: string;
    public product: string;
    public originator: string;
    public exception: string;
    public invoiced?: number;
    public delivered?: number;
    public quantity: number;
    public action: string;
    public source: string;
    public reason: string;
    public erdd?: Date;
    public actionedBy: string;
    public approvedBy: string;
    public comments: Array<string>;
    public isSelected: boolean;
}
