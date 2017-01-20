import { Component } from '@angular/core';
import { ExceptionDelivery } from '../exceptions/exceptionDelivery';
import { PendingCreditDetail } from './pendingCreditDetail';
import { PendingCreditService } from './pendingCreditService';

@Component({
    selector: 'ow-pending-credit-detail-modal',
    templateUrl: './app/pending_credit/pending-credit-detail-modal.html'
})
export class PendingCreditDetailModal {
    public isVisible: boolean;
    public pendingCredit: ExceptionDelivery;
    public details: PendingCreditDetail[];

    constructor(private pendingCreditService: PendingCreditService) {}

    public show(pendingCredit: ExceptionDelivery) {
        this.pendingCredit = pendingCredit;
        this.pendingCreditService.getPendingCreditDetail(pendingCredit.id)
            .subscribe(x => {
                this.details = x;
                this.isVisible = true;
                console.log(this.details);
            });
    }

    public hide() {
        this.isVisible = false;
    }
}