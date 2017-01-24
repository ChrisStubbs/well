import { Component, EventEmitter, Output } from '@angular/core';
import { Response } from '@angular/http';
import { DeliveryLine } from '../delivery/model/deliveryLine';
import { ToasterService } from 'angular2-toaster/angular2-toaster';
import { ExceptionDeliveryService } from './exceptionDeliveryService';
import { HttpResponse } from '../shared/httpResponse';

@Component({
    selector: 'ow-exceptions-confirm-modal',
    templateUrl: './app/exceptions/exceptions-confirm-modal.html'
})
export class ExceptionsConfirmModal {
    public isVisible: boolean = false; 
    public deliveryLines: DeliveryLine[];
    public httpResponse: HttpResponse = new HttpResponse();
    public userThreshold: number = 0.00;  

    //@Output() public onSave = new EventEmitter<DeliveryLine[]>();

    constructor(private exceptionDeliveryService: ExceptionDeliveryService, private toasterService: ToasterService) { }

    public show(deliveryLines: DeliveryLine[]) {
        this.deliveryLines = deliveryLines;

        this.exceptionDeliveryService.getUserCreditThreshold()
            .subscribe(responseData => {
                this.userThreshold = responseData[0];
            });

        this.isVisible = true;
    }

    public hide() {
        this.isVisible = false;
    }

    public save() {
        this.exceptionDeliveryService.submitExceptionConfirmation(this.deliveryLines[0].jobId)
            .subscribe(response => console.log(response));
    }
}