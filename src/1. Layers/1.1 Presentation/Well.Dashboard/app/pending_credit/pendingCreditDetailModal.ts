import { Component } from '@angular/core';
import { ExceptionDelivery } from '../exceptions/exceptionDelivery';
import { PendingCreditDetail } from './pendingCreditDetail';
import { PendingCreditService } from './pendingCreditService';

@Component({
    selector: 'ow-pending-credit-detail-modal',
    templateUrl: './app/pending_credit/pending-credit-detail-modal.html'
})
export class PendingCreditDetailModal {
    isVisible: boolean;
    pendingCredit: ExceptionDelivery;
    details: PendingCreditDetail[];

    constructor(private pendingCreditService: PendingCreditService) {}

    show(pendingCredit: ExceptionDelivery) {
        this.pendingCredit = pendingCredit;
        this.pendingCreditService.getPendingCreditDetail(pendingCredit.id)
            .subscribe(x => { this.details = x; this.isVisible = true;
                console.log(this.details);
            });
    }

    hide() {
        this.isVisible = false;
    }
}