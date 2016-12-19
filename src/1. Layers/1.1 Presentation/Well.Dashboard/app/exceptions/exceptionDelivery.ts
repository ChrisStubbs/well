import {BaseDelivery} from '../shared/baseDelivery';

export class ExceptionDelivery extends BaseDelivery {
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
    totalCreditValueForThreshold: number;
    formattedDeliveryDate: string;
    formattedPendingCreditCreatedBy: string;
    cashOnDelivery: string;
    totalCredit: string;
    isPending: boolean;
}