import {Component, ViewChild, EventEmitter, Output} from '@angular/core';
import {Response} from '@angular/http';
import {CreditThreshold} from './creditThreshold';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {BranchCheckboxComponent} from '../shared/branch/branchCheckboxComponent';
import {CreditThresholdService} from './creditThresholdService';
import {HttpResponse} from '../shared/httpResponse';

@Component({
    selector: 'ow-credit-threshold-add-modal',
    templateUrl: './app/credit_threshold/credit-threshold-add-modal.html'
})
export class CreditThresholdAddModalComponent {
    isVisible: boolean = false;
    creditThreshold: CreditThreshold = new CreditThreshold();
    httpResponse: HttpResponse = new HttpResponse();
    errors: string[];
    @Output() onCreditThresholdSave = new EventEmitter<CreditThreshold>();

    constructor(private creditThresholdService: CreditThresholdService, private toasterService: ToasterService) { }
    
    @ViewChild(BranchCheckboxComponent) branch: BranchCheckboxComponent;

    show() {
        this.isVisible = true;
        this.creditThreshold.thresholdLevel = 'Level';
    }

    hide() {
        this.isVisible = false;
        this.clear();
    }

    clear() {
        this.creditThreshold = new CreditThreshold();
    }

    save() {
        this.creditThreshold.branches = this.branch.selectedBranches;

        this.creditThresholdService.saveCreditThreshold(this.creditThreshold)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Credit threshold has been saved!', '');
                    this.isVisible = false;
                    this.clear();
                    this.onCreditThresholdSave.emit(this.creditThreshold);
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop('error', 'Credit threshold could not be saved at this time!', 'Please try again later!');
                    this.isVisible = false;
                }
                if (this.httpResponse.notAcceptable) {
                    this.errors = this.httpResponse.errors;
                }
            });
    }

    setSelectedLevel(level) {
        this.creditThreshold.thresholdLevel = level;
    }
}