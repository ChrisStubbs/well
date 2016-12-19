export class ExceptionDelivery {
    id: number;
    routeNumber: string;
    dropId: string;
    invoiceNumber: number;
    accountCode: string;
    accountName: string;
    jobStatus: string;
    action: string;
    assigned: string;
    deliveryDate: Date;
    accountId: string;
    branchId: number;
    canAction: boolean;
    totalCreditValueForThreshold: number;
    formattedDeliveryDate: string;
    formattedPendingCreditCreatedBy: string;
    cashOnDelivery: string;
    totalCredit: string;
    totalOutersShort: number;
}