﻿import {Component} from '@angular/core';
import {Delivery} from './delivery';
import {DeliveryLine} from './deliveryLine';
import {Damage} from './damage';
import {DamageReason} from './damageReason';
import {DeliveryService} from "./deliveryService";
import {ROUTER_DIRECTIVES, ActivatedRoute, Router} from '@angular/router';
import * as lodash from 'lodash';

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
    reasons: DamageReason[] = new Array<DamageReason>();

    constructor(private deliveryService: DeliveryService,
        private route: ActivatedRoute,
        private router: Router) {
        route.params.subscribe(params => { this.deliveryId = parseInt(params['id'],10), this.lineNo = parseInt(params['line'],10) });
    }

    ngOnInit(): void {

        this.deliveryService.getDamageReasons()
            .subscribe(reasons => {
                this.reasons = reasons;
                //console.log(reasons);
            });

        this.deliveryService.getDelivery(this.deliveryId)
            .subscribe(delivery => {
                this.delivery = new Delivery(delivery);
                this.deliveryLine = lodash.find(this.delivery.deliveryLines, { lineNo: this.lineNo });
                console.log(this.deliveryLine);
            });
    }

    addDamage() {
        var index = this.deliveryLine.damages.length;
        this.deliveryLine.damages.push(new Damage(index, 0, "Notdef"));
    }

    removeDamage(index) {
        console.log("Removing index: " + index);        
        lodash.remove(this.deliveryLine.damages, { index: index }); 
        console.log(this.deliveryLine.damages);      
    }

    update() {
        //TODO - POSTBACK Update
    }

    cancel() {
        this.router.navigate(['/delivery', this.delivery.id]);
    }
}