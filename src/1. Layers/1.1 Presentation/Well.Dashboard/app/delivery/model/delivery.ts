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
            this.deliveryType = delivery.deliveryType;
            this.cashOnDelivery = delivery.cashOnDelivery;
            this.isException = delivery.isException;
            this.canAction = delivery.canAction;
            this.canSubmit = delivery.canSubmit;
            this.grnNumber = delivery.grnNumber;
            this.branchId = delivery.branchId;
            this.grnProcessType = delivery.grnProcessType;
            this.proofOfDelivery = delivery.proofOfDelivery;

            if (delivery.exceptionDeliveryLines)
            {
                for (const line of delivery.exceptionDeliveryLines)
                {
                    this.exceptionDeliveryLines.push(new DeliveryLine(line));
                }
            }

            if (delivery.cleanDeliveryLines)
            {
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
    public deliveryType: string;
    public cashOnDelivery: string;
    public isException: boolean;
    public canAction: boolean;
    public canSubmit: boolean;
    public grnNumber: string;
    public exceptionDeliveryLines: DeliveryLine[] = new Array<DeliveryLine>();
    public branchId: number;
    public grnProcessType: number;
    public proofOfDelivery: number;
    public cleanDeliveryLines: DeliveryLine[] = new Array<DeliveryLine>();

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