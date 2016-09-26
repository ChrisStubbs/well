﻿import {Component, EventEmitter, Output} from '@angular/core';
import {Response} from '@angular/http';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {HttpResponse} from '../shared/httpResponse';
import {CreditThresholdService} from './creditThresholdService';
import {CreditThreshold} from './creditThreshold';

@Component({
    selector: 'ow-credit-threshold-remove-modal',
    templateUrl: './app/credit_threshold/credit-threshold-remove-modal.html'
})
export class CreditThresholdRemoveModalComponent {
    isVisible: boolean = false;
    creditThreshold: CreditThreshold;
    httpResponse: HttpResponse = new HttpResponse();
    @Output() onCreditThresholdRemoved = new EventEmitter<CreditThreshold>();

    constructor(private creditThresholdService: CreditThresholdService, private toasterService: ToasterService) { }

    show(creditThreshold: CreditThreshold) {
        this.creditThreshold = creditThreshold;
        this.isVisible = true;
    }

    hide() {
        this.isVisible = false;
    }

    yes() {
        this.creditThresholdService.removeCreditThreshold(this.creditThreshold.id)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) this.toasterService.pop('success', 'Credit threshold has been removed!', '');
                if (this.httpResponse.failure) this.toasterService.pop('error', 'Credit threshold could not be deleted at this time!', 'Please try again later!');

                this.isVisible = false;

                this.onCreditThresholdRemoved.emit(this.creditThreshold);
            });
    }
}