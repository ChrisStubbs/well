import { DeliveryLine } from './deliveryLine';

export class Delivery
{
    constructor(delivery: Delivery)
    {
        if (delivery)
        {
            this.id = delivery.id;
            this.accountCode = delivery.accountCode;
            this.outerCount = delivery.outerCount;
            this.outerDiscrepancyFound = delivery.outerDiscrepancyFound;
            this.totalOutersShort = delivery.totalOutersShort;
            this.accountName = delivery.accountName;
            this.accountAddress = delivery.accountAddress;
            this.invoiceNumber = delivery.invoiceNumber;
            this.contactName = delivery.contactName;
            this.phoneNumber = delivery.phoneNumber;
            this.mobileNumber = delivery.mobileNumber;
            this.jobStatus = delivery.jobStatus;
            this.cashOnDelivery = delivery.cashOnDelivery;
            this.isCashOnDelivery = delivery.isCashOnDelivery;
            this.canAction = delivery.canAction;
            this.canSubmit = delivery.canSubmit;
            this.isPendingCredit = delivery.isPendingCredit;
            this.grnNumber = delivery.grnNumber;
            this.branchId = delivery.branchId;
            this.grnProcessType = delivery.grnProcessType;
            this.proofOfDelivery = delivery.proofOfDelivery;
            this.isProofOfDelivery = delivery.isProofOfDelivery;
            this.canBulkCredit = delivery.canBulkCredit;

            if (delivery.exceptionDeliveryLines)
            {
                delivery.jobStatus = 'Exception';
                for (const line of delivery.exceptionDeliveryLines)
                {
                    this.exceptionDeliveryLines.push(new DeliveryLine(line));
                }
            }

            if (delivery.cleanDeliveryLines)
            {
                delivery.jobStatus = 'Clean';
                for (const line of delivery.cleanDeliveryLines)
                {
                    this.cleanDeliveryLines.push(new DeliveryLine(line));
                }
            }
        }
    }
    public id: number;
    public accountCode: string;
    public outerCount: number;  
    public outerDiscrepancyFound: boolean;
    public totalOutersShort: number;
    public accountName: string;
    public accountAddress: string;
    public invoiceNumber: string;
    public contactName: string;
    public phoneNumber: string;
    public mobileNumber: string;
    public jobStatus: string;
    public cashOnDelivery: string;
    public isCashOnDelivery: boolean;
    public canAction: boolean;
    public canSubmit: boolean;
    public isPendingCredit: boolean;
    public grnNumber: string;
    public exceptionDeliveryLines: DeliveryLine[] = new Array<DeliveryLine>();
    public branchId: number;
    public grnProcessType: number;
    public proofOfDelivery: number;
    public isProofOfDelivery: boolean;
    public cleanDeliveryLines: DeliveryLine[] = new Array<DeliveryLine>();
    public canBulkCredit: boolean;

    public isCleanOnInit(): boolean
    {
        let clean = true;

        for (const line of this.exceptionDeliveryLines)
        {
            if (line.isCleanOnInit === false)
            {
                clean = false;
            }
        }

        for (const line of this.cleanDeliveryLines)
        {
            if (line.isCleanOnInit === false)
            {
                clean = false;
            }
        }

        return clean;
    };

    public isClean(): boolean
    {
        let clean = true;

        for (const line of this.exceptionDeliveryLines)
        {
            if (line.isClean() === false)
            {
                clean = false;
            }
        }

        for (const line of this.cleanDeliveryLines)
        {
            if (line.isClean() === false)
            {
                clean = false;
            }
        }

        return clean;
    }
}