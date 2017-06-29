import { Component, ViewChild, ElementRef, Input, Output, EventEmitter } from '@angular/core';
import { LookupService } from '../services/lookupService';
import { IObservableAlive } from '../IObservableAlive';
import { LookupsEnum } from '../services/lookupsEnum';
import { Observable } from 'rxjs';
import { ILookupValue } from '../services/ILookupValue';
import { IBulkEditSummary, IBulkEditItem, IBulkEditPatchRequest, IBulkEditResult } from './bulkEditItem';
import { BulkEditService } from './bulkEditService';
import { NgForm } from '@angular/forms';

@Component({
    selector: 'bulk-edit-action-modal',
    templateUrl: 'app/shared/action/bulkEditActionModal.html',
    providers: [LookupService, BulkEditService]
})
export class BulkEditActionModal implements IObservableAlive
{
    public isAlive: boolean = true;
    private deliveryActions: Array<ILookupValue> = [];
    private deliveryAction: number = 0;
    private sources: Array<ILookupValue> = [];
    private source: number = 0;
    private reasons: Array<ILookupValue> = [];
    private reason: number = 0;
    private message: string;

    private editItems: IBulkEditItem[] = [];
    @Input() public jobIds: number[] = [];
    @Input() public lineItemIds: number[] = [];
    @Output() public onSave = new EventEmitter<IBulkEditResult>();

    @ViewChild('showBulkAddModal')
    public showModal: ElementRef;
    @ViewChild('closeBulkAddModal')
    public closeModal: ElementRef;
    @ViewChild('bulkEditActionForm') private currentForm: NgForm;

    constructor(
        private lookupService: LookupService,
        private bulkEditService: BulkEditService)
    {
    }

    public ngOnInit()
    {
        Observable.forkJoin(
            this.lookupService.get(LookupsEnum.DeliveryAction),
            this.lookupService.get(LookupsEnum.JobDetailSource),
            this.lookupService.get(LookupsEnum.JobDetailReason)
        )
            .takeWhile(() => this.isAlive)
            .subscribe(value =>
            {
                this.deliveryActions = value[0];
                this.sources = value[1];
                this.reasons = value[2];
            });
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    public show()
    {
        this.showModal.nativeElement.click();
        this.populateEditItems();
    }

    private populateEditItems()
    {
        let observable: Observable<IBulkEditSummary>;
        if (this.lineItemIds.length > 0)
        {
            observable = this.bulkEditService.getSummaryForLineItems(this.lineItemIds);
        } else if (this.jobIds.length > 0)
        {
            observable = this.bulkEditService.getSummaryForJob(this.jobIds);
        }
        if (observable)
        {
            observable
                .takeWhile(() => this.isAlive)
                .subscribe((summary: IBulkEditSummary) =>
                {
                    this.message = summary.message;
                    this.editItems = summary.items;
                });
        }
    }

    public close(): void {
        this.deliveryAction = 0;
        this.source = 0;
        this.reason = 0;
        this.closeModal.nativeElement.click();
    }

    private getPatchRequest(): IBulkEditPatchRequest
    {
        return {
            deliveryAction: this.deliveryAction,
            source: this.source,
            reason: this.reason,
            jobIds: this.jobIds,
            lineItemIds: this.lineItemIds
        }
    }

    private save(): void
    {
        if (this.currentForm.form.valid)
        {
            this.bulkEditService.patch(this.getPatchRequest())
                .takeWhile(() => this.isAlive)
                .subscribe((responseData: IBulkEditResult) =>
                {
                    this.onSave.emit(responseData);
                    this.close();
                });
        }
    }

    private isFormValid(): boolean {
        return this.currentForm.form.valid
            && this.editItems.length > 0;
    }
}