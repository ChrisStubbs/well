import { Component, ViewChild } from '@angular/core';
import { Delivery } from './model/delivery';
import { DeliveryIssuesComponent } from './deliveryIssuesComponent';
import { DeliveryLine } from './model/deliveryLine';
import { DeliveryService } from './deliveryService';
import { ActivatedRoute } from '@angular/router';
import * as lodash from 'lodash';

@Component({
    templateUrl: './app/delivery/delivery-update.html',
    providers: [DeliveryService]
})
export class DeliveryUpdateComponent
{
    public deliveryId: number;
    public lineNo: number;
    public delivery: Delivery;
    public deliveryLine: DeliveryLine;

    constructor(private deliveryService: DeliveryService, private route: ActivatedRoute)
    {
        route.params.subscribe(params =>
        {
            this.deliveryId = parseInt(params['id'], 10), this.lineNo = parseInt(params['line'], 10)
        });
    }

    public ngOnInit(): void
    {
        this.deliveryService.getDelivery(this.deliveryId)
            .subscribe(deliveryResponse => this.initDelivery(new Delivery(deliveryResponse)));
    }

    public initDelivery(delivery: Delivery)
    {
        this.delivery = delivery;

        let line = lodash.find(this.delivery.exceptionDeliveryLines, { lineNo: this.lineNo });
        if (!line) {
            line = lodash.find(this.delivery.cleanDeliveryLines, { lineNo: this.lineNo });
        }

        this.deliveryLine = line;
    }

    public invalidShortDamagesQty(): boolean
    {
        return this.deliveryLine.totalQtyOfShortsAndDamages() > this.deliveryLine.invoicedQuantity;
    }

    public invalidDeliveryQty(): boolean
    {
        return this.deliveryLine.totalQtyOfShortsAndDamages() > this.deliveryLine.invoicedQuantity;
    }
}