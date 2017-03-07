import {BaseDelivery} from '../shared/baseDelivery';
 
export class ExceptionDelivery extends BaseDelivery {
    public  routeNumber: string;
    public  dropId: string;
    public  invoiceNumber: number;
    public  accountName: string;
    public  jobStatus: string;
    public  action: string;
    public  assigned: string;
    public  deliveryDate: Date;
    public  accountId: string;
    public  canAction: boolean;
    public  canSubmit: boolean;
    public  totalCreditValueForThreshold: number;
    public  formattedDeliveryDate: string;
    public  formattedPendingCreditCreatedBy: string;
    public  cashOnDelivery: string;
    public  isCashOnDelivery: boolean;
    public  totalCredit: string;
    public  isPending: boolean;
    public  totalOutersShort: number;
}