import { Component, ViewChild, ElementRef } from '@angular/core';
import { LookupService } from '../services/lookupService';
import { IObservableAlive } from '../IObservableAlive';
import { EditExceptionsService } from '../../exceptions/editExceptionsService';
import { LookupsEnum } from '../services/lookupsEnum';
import { Observable } from 'rxjs';
import { ILookupValue } from '../services/ILookupValue';

@Component({
    selector: 'bulk-add-action-modal',
    templateUrl: 'app/shared/action/bulkAddActionModal.html',
    providers: [LookupService, EditExceptionsService]
})
export class BulkAddActionModal implements IObservableAlive
{
    public isAlive: boolean = true;

    private deliveryActions: Array<ILookupValue> = [];
    private sources: Array<ILookupValue> = [];
    private reasons: Array<ILookupValue> = [];
    private exceptionTypes: Array<ILookupValue> = [];

    @ViewChild('showBulkAddModal') public showModal: ElementRef;
    @ViewChild('closeBulkAddModal') public closeModal: ElementRef;

    constructor(
        private lookupService: LookupService,
        private editExceptionsService: EditExceptionsService) { }

    public ngOnInit()
    {

        Observable.forkJoin(
            this.lookupService.get(LookupsEnum.DeliveryAction),
            this.lookupService.get(LookupsEnum.ExceptionType),
            this.lookupService.get(LookupsEnum.JobDetailSource),
            this.lookupService.get(LookupsEnum.JobDetailReason)

        )
            .takeWhile(() => this.isAlive)
            .subscribe(value =>
            {
                this.deliveryActions = value[0];
                this.exceptionTypes = value[1];
                this.sources = value[2];
                this.reasons = value[3];
            });
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    public show()
    {
        this.showModal.nativeElement.click();
    }

    public close(): void
    {
        this.closeModal.nativeElement.click();
    }
}