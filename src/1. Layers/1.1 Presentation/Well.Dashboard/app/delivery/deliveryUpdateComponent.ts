import {Component} from '@angular/core';
import {Delivery} from './delivery';
import {DeliveryLine} from './deliveryLine';
import {Damage} from './damage';
import {DamageReason} from './damageReason';
import {DeliveryService} from "./deliveryService";
import {ActivatedRoute, Router} from '@angular/router';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import * as lodash from 'lodash';

@Component({
    templateUrl: './app/delivery/delivery-update.html',
    providers: [DeliveryService]
})
export class DeliveryUpdateComponent {
    deliveryId: number;
    lineNo: number;
    delivery: Delivery = new Delivery(null);
    deliveryLine: DeliveryLine = new DeliveryLine(null);
    reasons: DamageReason[] = new Array<DamageReason>();

    constructor(private deliveryService: DeliveryService,
        private toasterService: ToasterService,
        private route: ActivatedRoute,
        private router: Router) {
        route.params.subscribe(params => { this.deliveryId = parseInt(params['id'],10), this.lineNo = parseInt(params['line'],10) });
    }

    ngOnInit(): void {

        this.deliveryService.getDamageReasons()
            .subscribe(reasons => { this.reasons = reasons; });

        this.deliveryService.getDelivery(this.deliveryId)
            .subscribe(delivery => {
                this.delivery = new Delivery(delivery);
                this.deliveryLine = lodash.find(this.delivery.deliveryLines, { lineNo: this.lineNo });
            });
    }

    addDamage() {
        var index = this.deliveryLine.damages.length;
        this.deliveryLine.damages.push(new Damage(index, 0, "Notdef"));
    }

    removeDamage(index) {
        lodash.remove(this.deliveryLine.damages, { index: index }); 
    }

    update() {
        this.deliveryService.updateDeliveryLine(this.deliveryLine)
            .subscribe(() => {
                this.toasterService.pop('success', 'Delivery line updated', '');
                this.router.navigate(['/delivery', this.delivery.id]);
            });
    }

    cancel() {
        this.router.navigate(['/delivery', this.delivery.id]);
    }
}