import {BaseDelivery} from '../shared/baseDelivery' 

export class ResolvedDelivery extends BaseDelivery{
    routeNumber: string;
    dropId: string;
    invoiceNumber: number;
    accountName: string;
    jobStatus: string;
    action: string;
    assigned: string;
    deliveryDate: Date;
    accountId: string;
    canAction: boolean;
    cod: string;
}