import { Component } from '@angular/core';

@Component({
    selector: 'ow-pending-credit-confirmation',
    templateUrl: './app/pending_credit/pending-credit-confirmation.html'
})
export class PendingCreditConfirmationModal {
    isVisible: boolean;

    show(): void {
        this.isVisible = true;
    }
}