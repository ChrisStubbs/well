import {Component, ViewChild} from '@angular/core';
import {Delivery} from './delivery';
import {DeliveryLine} from './deliveryLine';
import {Damage} from './damage';
import {DamageReason} from './damageReason';
import {DeliveryService} from "./deliveryService";
import {ConfirmModal} from "../shared/confirmModal";
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
    confirmMessage: string;
    confirmModalIsVisible: boolean = false;
    @ViewChild(ConfirmModal) private confirmModal: ConfirmModal;

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
        if (this.delivery.isCleanOnInit() && this.delivery.isClean() === false) {
            //Changing a Clean to an Exception
            this.confirmModal.isVisible = true;
            this.confirmModal.message =
                "You have added shorts or damages for this delivery, this will make the delivery dirty. " +
                "Are you sure you want to save your changes?";
            return;
        }
        if (this.delivery.isCleanOnInit() == false && this.delivery.isClean()) {
            ///Changing an Exception to a clean
            this.confirmModal.isVisible = true;
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
                this.toasterService.pop('success', 'Delivery line updated', '');
                this.router.navigate(['/delivery', this.delivery.id]);
            });
    }
    
    cancel() {
        this.router.navigate(['/delivery', this.delivery.id]);
    }
}