import {Component} from '@angular/core';
import {Delivery} from './delivery';
import {DeliveryLine} from './deliveryLine';
import {DeliveryService} from "./deliveryService";
import {ROUTER_DIRECTIVES, ActivatedRoute, Router} from '@angular/router';
import * as _ from 'lodash';

@Component({
    templateUrl: './app/delivery/delivery-update.html',
    providers: [DeliveryService],
    directives: [ROUTER_DIRECTIVES]
})
export class DeliveryUpdateComponent {
    deliveryId: number;
    lineNo: number;
    delivery: Delivery = new Delivery(null);
    deliveryLine: DeliveryLine = new DeliveryLine(null);

    constructor(private deliveryService: DeliveryService,
        private route: ActivatedRoute,
        private router: Router) {
        route.params.subscribe(params => { this.deliveryId = parseInt(params['id'],10), this.lineNo = parseInt(params['line'],10) });
    }

    ngOnInit(): void {

        this.deliveryService.getDelivery(this.deliveryId)
            .subscribe(delivery => {
                this.delivery = new Delivery(delivery);
                this.deliveryLine = _.find(this.delivery.deliveryLines, { lineNo: this.lineNo });
                console.log(this.delivery);
            });
    }

    update() {
        //TODO - POSTBACK Update
    }

    cancel() {
        this.router.navigate(['/delivery', this.delivery.id]);
    }
}