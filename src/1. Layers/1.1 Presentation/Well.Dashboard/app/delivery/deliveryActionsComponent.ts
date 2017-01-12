import {Component, ViewChild} from '@angular/core';
import {Action} from './model/action';
import {ActionStatus} from './model/actionStatus';
import {Delivery} from './model/delivery';
import {DeliveryLine} from './model/deliveryLine';
import {DeliveryLineAction} from './model/deliveryLineAction';
import {DeliveryService} from './deliveryService';
import {Router} from '@angular/router';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import * as lodash from 'lodash';

@Component({
    templateUrl: './app/delivery/delivery-actions.html',
    selector: 'ow-delivery-actions',
})
export class DeliveryActionsComponent {
    public deliveryId: number;
    public canAction: boolean = false;
    public deliveryLine: DeliveryLine = new DeliveryLine(undefined);
    public actions: Action[] = new Array<Action>();

    constructor(
        private deliveryService: DeliveryService,
        private toasterService: ToasterService,
        private router: Router) {
    }

    public ngOnInit(): void {
        this.deliveryService.getActions().subscribe(actions => { this.actions = actions; });
    }

    public addAction() {
        this.deliveryLine.actions.push(new DeliveryLineAction(
            this.deliveryLine.actions.length,
            0,
            1,
            'Credit',
            1,
            'Draft'));
    }

    public removeAction(index) {
        lodash.remove(this.deliveryLine.actions, { index: index });
    }

    public update() {
        const request = {
            jobDetailId: this.deliveryLine.jobDetailId,
            draftActions: lodash.filter(this.deliveryLine.actions, { status: 1})
        }
        
        this.deliveryService.updateDeliveryLineActions(request)
            .subscribe(() => {
                this.toasterService.pop('success', 'Delivery line actions updated', '');
                this.router.navigate(['/delivery', this.deliveryId]);
            });
    }

    public cancel() {
        this.router.navigate(['/delivery', this.deliveryId]);
    }
}