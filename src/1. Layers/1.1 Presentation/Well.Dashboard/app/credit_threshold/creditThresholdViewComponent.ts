import {Component, OnInit, ViewChild} from '@angular/core';
import {CreditThresholdService} from './creditThresholdService';
import {CreditThresholdAddModalComponent} from './creditThresholdAddModalComponent';
import {CreditThresholdRemoveModalComponent} from './creditThresholdRemoveModalComponent';
import {CreditThresholdEditModalComponent} from './creditThresholdEditModalComponent';
import {CreditThreshold} from './creditThreshold';
import * as lodash from 'lodash';

@Component({
    selector: 'ow-credit-threshold-view',
    templateUrl: './app/credit_threshold/credit-threshold-view.html'
})
export class CreditThresholdViewComponent implements OnInit {
    public credits: CreditThreshold[];

    constructor(private creditThresholdService: CreditThresholdService) {}

    public ngOnInit(): void {
        this.loadCreditThreshold();
    }

    @ViewChild(CreditThresholdAddModalComponent) public addModal: CreditThresholdAddModalComponent;
    @ViewChild(CreditThresholdRemoveModalComponent) public removeModal: CreditThresholdRemoveModalComponent;
    @ViewChild(CreditThresholdEditModalComponent) public editModal: CreditThresholdEditModalComponent;

    public selectCredit(credit: CreditThreshold): void {
        this.editModal.show(credit);
    }

    public loadCreditThreshold() {
        this.creditThresholdService.getCreditThresholds().subscribe(x => this.credits = x);
    }

    public add() {
        this.addModal.show();
    }

    public remove(creditThreshold: CreditThreshold): void {
        this.removeModal.show(creditThreshold);
    }

    public onRemoved(creditThreshold: CreditThreshold) {
        lodash.remove(this.credits, creditThreshold);
    }
}