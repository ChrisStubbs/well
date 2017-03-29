import { BaseDelivery } from '../shared/baseDelivery';

export class CleanDelivery extends BaseDelivery {
    routeNumber: string;
    routeDate: Date;
    dropId: string;
    invoiceNumber: number;
    accountName: string;
    jobStatus: string;
    action: string;
    assigned: string;
    deliveryDate: Date;
    accountId: string;
    canAction: boolean;
} 