import { DeliveryLine } from './deliveryLine';


export class DeliveryAction {
    public jobId : number;
    public totalCreditThreshold : number;

 public lines: DeliveryLine[] = new Array<DeliveryLine>();
}