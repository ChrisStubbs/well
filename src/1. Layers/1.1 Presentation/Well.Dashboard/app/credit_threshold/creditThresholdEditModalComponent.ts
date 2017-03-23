import {Component, ViewChild, EventEmitter, Output} from '@angular/core';
import {Response} from '@angular/http';
import {CreditThreshold} from './creditThreshold';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {BranchCheckboxComponent} from '../shared/branch/branchCheckboxComponent';
import {CreditThresholdService} from './creditThresholdService';
import {HttpResponse} from '../shared/httpResponse';

@Component({
    selector: 'ow-credit-threshold-edit-modal',
    templateUrl: './app/credit_threshold/credit-threshold-edit-modal.html'
})
export class CreditThresholdEditModalComponent {
    public isVisible: boolean = false;
    public creditThreshold: CreditThreshold;
    public httpResponse: HttpResponse = new HttpResponse();
    public errors: string[];
    @Output() public onCreditThresholdUpdate = new EventEmitter<CreditThreshold>();

    constructor(private creditThresholdService: CreditThresholdService, private toasterService: ToasterService) { }

    @ViewChild(BranchCheckboxComponent) public branch: BranchCheckboxComponent;
    
    public show(creditThreshold: CreditThreshold) {
        this.clear();
        this.creditThreshold = creditThreshold;
        this.isVisible = true;
    }

    public hide() {
        this.isVisible = false;
        this.clear();
    }

    public clear() {
        this.creditThreshold = new CreditThreshold();
        this.errors = [];
    }

    public update() {
        this.creditThresholdService.saveCreditThreshold(this.creditThreshold, true)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Credit threshold has been updated', '');
                    this.isVisible = false;
                    this.onCreditThresholdUpdate.emit(this.creditThreshold);
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop(
                        'error',
                        'Credit threshold could not be updated at this time',
                        'Please try again later!');
                    this.isVisible = false;
                }
                if (this.httpResponse.notAcceptable) {
                    this.errors = this.httpResponse.errors;
                }
            });
    }
}