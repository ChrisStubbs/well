import { LineItemAction } from './lineItemAction';

export class EditLineItemException {
    constructor() {
        this.isSelected = false;
        this.isExpanded = false;
    }

    public isSelected: boolean;
    public id: number;
    public jobId: number;
    public resolutionId: number;
    public resolutionStatus: string;
    public accountCode: string;
    public invoice: string;
    public jobTypeDescription: string;
    public jobType: number;
    public productNumber: string;
    public driverReason: string;
    public product: string;
    public value?: number;
    public invoiced?: number;
    public delivered?: number;
    public damages: number;
    public shorts: number;
    public bypass: number;
    public exceptions: Array<EditLineItemExceptionDetail>;
    public lineItemActions: Array<LineItemAction>;
    public isExpanded: boolean;
    public canEditActions: boolean;
    public canEditActionsReason: string;
    public hasUnresolvedActions: boolean;
    public readonly isProofOfDelivery: boolean;
    public upliftAction: number;
}

export class EditLineItemExceptionDetail {
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
    public comments: Array<string>;
}