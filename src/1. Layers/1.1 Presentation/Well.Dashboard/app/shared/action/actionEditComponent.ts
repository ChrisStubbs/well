import { Component, ViewChild, ElementRef, EventEmitter, Output } from '@angular/core';
import { NgForm } from '@angular/forms';
import { IObservableAlive } from '../IObservableAlive';
import * as _ from 'lodash';
import { ILookupValue } from '../services/ILookupValue';
import { LookupsEnum } from '../services/lookupsEnum';
import { LookupService } from '../services/lookupService';
import { EditExceptionsService } from '../../exceptions/editExceptionsService';
import { Observable } from 'rxjs'
import { LineItemAction } from '../../exceptions/lineItemAction';
import { EditLineItemException } from '../../exceptions/editLineItemException';

@Component({
    selector: 'action-edit',
    templateUrl: 'app/shared/action/actionEditComponent.html',
    providers: [LookupService, EditExceptionsService]
})
export class ActionEditComponent implements IObservableAlive
{
    public isAlive: boolean = true;

    private deliveryActions: Array<ILookupValue>;
    private sources: Array<ILookupValue>;
    private reasons: Array<ILookupValue>;
    private exceptionTypes: Array<ILookupValue>;
    private lineItemActionsToRemove: Array<LineItemAction> = [];
    public source: EditLineItemException = new EditLineItemException();
    private lineItemActions: Array<LineItemAction> = [];

    @Output() public onSave = new EventEmitter<EditLineItemException>();
    @ViewChild('showModal') public showModal: ElementRef;
    @ViewChild('actionEditForm') private currentForm: NgForm;

    constructor(
        private lookupService: LookupService,
        private editExceptionsService: EditExceptionsService) { }

    public ngOnInit()
    {
        this.fillLookups();
    }

    private fillLookups(): void
    {
        if (_.isNil(this.deliveryActions))
        {
            this.deliveryActions = [];
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
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    public addAction(): void
    {
        this.lineItemActions.push(new LineItemAction());
    }

    public removeItem(index: number): void
    {
        const item = this.lineItemActions.splice(index, 1);
        this.lineItemActionsToRemove.push(item[0]);
    }

    public show(editLineItemException: EditLineItemException)
    {
        this.loadSource(editLineItemException);
        this.showModal.nativeElement.click();
    }

    public close(): void
    {
        this.lineItemActions = [];
    }

    public save(): void
    {
        if (this.currentForm.form.valid)
        {
            this.source.lineItemActions = this.lineItemActions;
            this.editExceptionsService.patch(this.source)
                .takeWhile(() => this.isAlive)
                .subscribe((responseData: EditLineItemException) =>
                {
                    this.loadSource(responseData);
                    this.onSave.emit(responseData);
                });
        }
    }

    private loadSource(editLineItemException: EditLineItemException): void
    {
        this.source = editLineItemException;
        this.lineItemActions = this.source.lineItemActions || [];
    }

    private  actionClose: number = 2;
    public qtyChanged(item: LineItemAction): void
    {
       
        if (item.quantity === 0)
        {
            item.deliveryAction = this.actionClose;
        }
    }

    public deliveryActionChange(item: LineItemAction): void
    {
        if (+item.deliveryAction === this.actionClose) {
            item.quantity = 0;
        }
    }
}