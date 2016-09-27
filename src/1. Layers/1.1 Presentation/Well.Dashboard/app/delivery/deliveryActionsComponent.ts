import {Component, ViewChild} from '@angular/core';
import {Action} from './model/action';
import {Delivery} from './model/delivery';
import {DeliveryLine} from './model/deliveryLine';
import {DeliveryLineAction} from './model/deliveryLineAction';
import {DeliveryService} from "./deliveryService";
import {Router} from '@angular/router';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import * as lodash from 'lodash';

@Component({
    templateUrl: './app/delivery/delivery-actions.html',
    selector: 'ow-delivery-actions',
})
export class DeliveryActionsComponent {
    deliveryId: number;
    deliveryLine: DeliveryLine = new DeliveryLine(undefined);
    actions: Action[] = new Array<Action>();

    constructor(private deliveryService: DeliveryService,
        private toasterService: ToasterService,
        private router: Router) {
    }

    ngOnInit(): void {
        this.deliveryService.getActions().subscribe(actions => { this.actions = actions; });
    }

    addAction() {
        var index = this.deliveryLine.actions.length;
        this.deliveryLine.actions.push(new DeliveryLineAction(index, 0, new Action()));
    }

    removeAction(index) {
        lodash.remove(this.deliveryLine.actions, { index: index });
    }

    update() {
        /*
        this.deliveryService.updateDeliveryLine(this.deliveryLine)
            .subscribe(() => {
                this.toasterService.pop('success', 'Delivery line updated', '');
                this.router.navigate(['/delivery', this.deliveryId]);
            });
        */
    }

    cancel() {
        this.router.navigate(['/delivery', this.deliveryId]);
    }
}