import { Component, EventEmitter, Output } from '@angular/core';
import { Response } from '@angular/http';
import { ExceptionDelivery } from '../exceptions/exceptionDelivery';
import { ExceptionDeliveryService } from "../exceptions/exceptionDeliveryService";
import { ToasterService } from 'angular2-toaster/angular2-toaster';
import { HttpResponse } from '../shared/httpResponse';

@Component({
    selector: 'ow-pending-credit-confirmation',
    templateUrl: './app/pending_credit/pending-credit-confirmation.html'
})
export class PendingCreditConfirmationModal {
    isVisible: boolean;
    pendingCredit: ExceptionDelivery;
    httpResponse: HttpResponse = new HttpResponse();
    @Output() onAccepted = new EventEmitter<ExceptionDelivery>();

    constructor(private toasterService: ToasterService, private exceptionDeliveryService: ExceptionDeliveryService) {}

    show(pendingCredit: ExceptionDelivery): void {
        this.pendingCredit = pendingCredit;
        this.isVisible = true;
    }

    hide(): void {
        this.isVisible = false;
    }

    yes() {
        this.exceptionDeliveryService.credit(this.pendingCredit)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) this.toasterService.pop('success', 'Exception has been credited!', '');
                if (this.httpResponse.notAcceptable) this.toasterService.pop('error', this.httpResponse.message, '');
                if (this.httpResponse.adamdown) this.toasterService.pop('error', 'ADAM is currently offline!', 'You will receive a notification once the credit has taken place!');

                this.isVisible = false;

                this.onAccepted.emit(this.pendingCredit);
            });
    }
}