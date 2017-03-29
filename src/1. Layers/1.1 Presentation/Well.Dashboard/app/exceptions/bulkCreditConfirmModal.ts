import {Component, EventEmitter, Input, Output} from '@angular/core';
import * as DetailReason from '../delivery/model/jobDetailReason';
import * as DetailSource from '../delivery/model/jobDetailSource';

@Component({
    selector: 'bulk-credit-confirm-modal',
    templateUrl: './app/exceptions/bulkCreditConfirmModal.html'
})
export class BulkCreditConfirmModal {
    @Input() public isVisible: boolean = false;
    @Output() public confirmed = new EventEmitter<any>();
    public reasons: DetailReason.JobDetailReason[] = new Array<DetailReason.JobDetailReason>();
    public sources: DetailSource.JobDetailSource[] = new Array<DetailSource.JobDetailSource>();
    public source: number = 0;
    public reason: number = 0;

    public show() {
        this.isVisible = true;
    }

    public hide() {
        this.isVisible = false;
    }

    public confirm()
    {
        this.confirmed.emit({ source: this.source, reason: this.reason });
        this.isVisible = false;
    }
}