﻿import {Component, ViewChild} from '@angular/core';
import {Delivery} from './model/delivery';
import {DeliveryLine} from './model/deliveryLine';
import {Damage} from './model/damage';
import {DamageReason} from './model/damageReason';
import {ConfirmModal} from "../shared/confirmModal";
import {DeliveryService} from "./deliveryService";
import {Router} from '@angular/router';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import * as lodash from 'lodash';

@Component({
    templateUrl: './app/delivery/delivery-issues.html',
    selector: 'ow-delivery-issues',
})
export class DeliveryIssuesComponent {
    delivery: Delivery = new Delivery(undefined);
    deliveryLine: DeliveryLine = new DeliveryLine(undefined);
    reasons: DamageReason[] = new Array<DamageReason>();
    confirmMessage: string;
    confirmModalIsVisible: boolean = false;
    @ViewChild(ConfirmModal) private confirmModal: ConfirmModal;

    constructor(private deliveryService: DeliveryService,
        private toasterService: ToasterService,
        private router: Router) {
    }

    ngOnInit(): void {
        this.deliveryService.getDamageReasons()
            .subscribe(reasons => { this.reasons = reasons; });
    }

    addDamage() {
        var index = this.deliveryLine.damages.length;
        this.deliveryLine.damages.push(new Damage(index, 0, "Notdef"));
    }

    removeDamage(index) {
        lodash.remove(this.deliveryLine.damages, { index: index });
    }

    update() {
        if (this.delivery.isCleanOnInit() && this.delivery.isClean() === false) {
            //Changing a Clean to an Exception
            this.confirmModal.isVisible = true;
            this.confirmModal.heading = "Make delivery dirty?";
            this.confirmModal.message =
                "You have added shorts or damages for this delivery, this will make the delivery dirty. " +
                "Are you sure you want to save your changes?";
            return;
        }
        if (this.delivery.isCleanOnInit() === false && this.delivery.isClean()) {
            ///Changing an Exception to a clean
            this.confirmModal.isVisible = true;
            this.confirmModal.heading = "Resolve delivery?";
            this.confirmModal.message =
                "You have removed all shorts and damages for this delivery, this will resolve the delivery. " +
                "Are you sure you want to save your changes?";
            return;
        }

        this.updateConfirmed();
    }

    updateConfirmed() {
        this.deliveryService.updateDeliveryLine(this.deliveryLine)
            .subscribe(() => {
                this.toasterService.pop('success', 'Delivery line issues updated', '');
                this.router.navigate(['/delivery', this.delivery.id]);
            });
    }

    cancel() {
        this.router.navigate(['/delivery', this.delivery.id]);
    }
}