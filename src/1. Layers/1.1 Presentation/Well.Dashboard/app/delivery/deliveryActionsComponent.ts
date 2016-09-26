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
    //TODO - Populate from backend action list 
    actions: Action[] = [
        { id: 1, description: "Credit" },
        { id: 2, description: "Credit And Reorder" },
        { id: 3, description: "Replan In Roadnet" },
        { id: 4, description: "Replan In TranSend" },
        { id: 5, description: "Replan In The Queue" },
        { id: 6, description: "Reject" },
    ];

    constructor(private deliveryService: DeliveryService,
        private toasterService: ToasterService,
        private router: Router) {
    }

    ngOnInit(): void {

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