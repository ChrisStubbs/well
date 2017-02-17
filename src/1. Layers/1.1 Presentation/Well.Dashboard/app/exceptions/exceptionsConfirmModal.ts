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
    public disableSave: boolean = false;

    @Output() public onSave = new EventEmitter();

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
        this.disableSave = false;
        this.isVisible = false; 
    }

    public save() {
        this.disableSave = true;
        this.exceptionDeliveryService.submitExceptionConfirmation(this.deliveryLines[0].jobId)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.notAcceptable) {
                    this.toasterService.pop('error', this.httpResponse.message, '');
                }
                else if (this.httpResponse.adamdown) {
                    this.toasterService.pop('info',
                        'ADAM is currently offline!',
                        'You will receive a notification once the credit has taken place!');
                }
                else if (this.httpResponse.success) {
                    this.toasterService.pop('success',
                        'Delivery line actions completed!',
                        '');

                    this.onSave.emit();
                    this.hide();
                }
            });
    }
}