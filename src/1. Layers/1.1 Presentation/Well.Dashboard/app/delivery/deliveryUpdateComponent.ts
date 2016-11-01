import {Component, ViewChild} from '@angular/core';
import {Delivery} from './model/delivery';
import {DeliveryIssuesComponent} from './deliveryIssuesComponent';
import {DeliveryActionsComponent} from './deliveryActionsComponent';
import {DeliveryLine} from './model/deliveryLine';
import {DeliveryService} from "./deliveryService";
import {ActivatedRoute} from '@angular/router';
import * as lodash from 'lodash';

@Component({
    templateUrl: './app/delivery/delivery-update.html',
    providers: [DeliveryService]
})
export class DeliveryUpdateComponent {
    deliveryId: number;
    lineNo: number;
    delivery: Delivery = new Delivery(undefined);
    deliveryLine: DeliveryLine = new DeliveryLine(undefined);

    @ViewChild(DeliveryIssuesComponent) private deliveryIssues: DeliveryIssuesComponent;
    @ViewChild(DeliveryActionsComponent) private deliveryActions: DeliveryActionsComponent;

    constructor(private deliveryService: DeliveryService,
        private route: ActivatedRoute) {
        route.params.subscribe(params => {
            this.deliveryId = parseInt(params['id'], 10), this.lineNo = parseInt(params['line'], 10)
        });
    }

    ngOnInit(): void {
        this.deliveryService.getDelivery(this.deliveryId)
            .subscribe(deliveryResponse => this.initDelivery(new Delivery(deliveryResponse)));
    }

    initDelivery(delivery: Delivery) {
        this.delivery = delivery;

        var line = lodash.find(this.delivery.exceptionDeliveryLines, { lineNo: this.lineNo });

        if (!line) line = lodash.find(this.delivery.cleanDeliveryLines, { lineNo: this.lineNo });

        this.deliveryLine = line;
        this.deliveryIssues.delivery = this.delivery;
        this.deliveryIssues.deliveryLine = this.deliveryLine;
        this.deliveryActions.deliveryId = this.delivery.id;
        this.deliveryActions.deliveryLine = this.deliveryLine;
        this.deliveryActions.canAction = this.delivery.canAction;
    }

}