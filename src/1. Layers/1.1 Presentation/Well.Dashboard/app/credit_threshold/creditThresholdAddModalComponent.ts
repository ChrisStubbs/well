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
    public isVisible: boolean = false;
    public creditThreshold: CreditThreshold = new CreditThreshold();
    public httpResponse: HttpResponse = new HttpResponse();
    public errors: string[];
    @Output() public onCreditThresholdSave = new EventEmitter<CreditThreshold>();

    constructor(private creditThresholdService: CreditThresholdService, private toasterService: ToasterService) { }
    
    @ViewChild(BranchCheckboxComponent) public branch: BranchCheckboxComponent;

    public show() {
        this.clear();
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

    public save() {
        //this.creditThreshold.branches = this.branch.selectedBranches;

        this.creditThresholdService.saveCreditThreshold(this.creditThreshold, false)
            .subscribe((res: Response) => {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Credit threshold has been saved', '');
                    this.isVisible = false;
                    this.clear();
                    this.onCreditThresholdSave.emit(this.creditThreshold);
                }
                if (this.httpResponse.failure) {
                    this.toasterService.pop(
                        'error',
                        'Credit threshold could not be saved at this time',
                        'Please try again later!');
                    this.isVisible = false;
                }
                if (this.httpResponse.notAcceptable) {
                    this.errors = this.httpResponse.errors;
                }
            });
    }

    public setSelectedLevel(level) {
        this.creditThreshold.thresholdLevel = level;
    }
}