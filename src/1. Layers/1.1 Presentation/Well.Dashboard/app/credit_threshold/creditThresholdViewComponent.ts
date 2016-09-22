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
export class CreditThresholdViewComponent implements OnInit{
    credits: CreditThreshold[];

    constructor(private creditThresholdService: CreditThresholdService) {}

    ngOnInit(): void {
        this.loadCreditThreshold();
    }

    @ViewChild(CreditThresholdAddModalComponent) addModal: CreditThresholdAddModalComponent;
    @ViewChild(CreditThresholdRemoveModalComponent) removeModal: CreditThresholdRemoveModalComponent;
    @ViewChild(CreditThresholdEditModalComponent) editModal: CreditThresholdEditModalComponent;

    selectCredit(credit: CreditThreshold): void {
        this.editModal.show(credit);
    }

    loadCreditThreshold() {
        this.creditThresholdService.getCreditThresholds().subscribe(x => this.credits = x);
    }

    add() {
        this.addModal.show();
    }

    remove(creditThreshold: CreditThreshold): void {
        this.removeModal.show(creditThreshold);
    }

    onRemoved(creditThreshold: CreditThreshold) {
        lodash.remove(this.credits, creditThreshold);
    }
}