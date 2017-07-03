import { BaseDelivery } from '../shared/baseDelivery';

export class CleanDelivery extends BaseDelivery {
    public routeNumber: string;
    public routeDate: Date;
    public dropId: string;
    public invoiceNumber: number;
    public accountName: string;
    public jobStatus: string;
    public action: string;
    public deliveryDate: Date;
    public accountId: string;
    public canAction: boolean;
} 