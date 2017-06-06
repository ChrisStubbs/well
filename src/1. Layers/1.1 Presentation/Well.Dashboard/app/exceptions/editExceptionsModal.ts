import { Component, Input, ViewChild, EventEmitter, Output }                from '@angular/core';
import { NgForm }                                                           from '@angular/forms';
import { IObservableAlive }                                                 from '../shared/IObservableAlive';
import { LookupService, ILookupValue, LookupsEnum }                         from '../shared/services/services';
import { EditLineItemException, EditLineItemExceptionDetail }               from './editLineItemException';
import { LineItemAction }                                                   from './lineItemAction';
import { EditExceptionsService }                                            from './editExceptionsService';
import * as _                                                               from 'lodash';
import { Observable }                                                       from 'rxjs'

@Component({
    selector: 'edit-exceptions-modal',
    templateUrl: './app/exceptions/editExceptionsModal.html',
    providers: [LookupService, EditExceptionsService]
})
export class EditExceptionsModal implements IObservableAlive
{
    private mItem: EditLineItemExceptionDetail;
    @Input() public set item(value: EditLineItemExceptionDetail)
    {
        this.mItem = value;
        this.mapToLineItemAction(value);
    };

    public get item(): EditLineItemExceptionDetail
    {
        return this.mItem;
    }
    public isAlive: boolean = true;

    @Input() public isEditMode: boolean = false;
    @Output() public onSave = new EventEmitter<EditLineItemException>();
    @Output() public onCancel = new EventEmitter();
    @ViewChild('exceptionModalForm') private currentForm: NgForm;

    private deliveryActions: Array<ILookupValue>;
    private sources: Array<ILookupValue>;
    private reasons: Array<ILookupValue>;
    private exceptionTypes: Array<ILookupValue>;
    private lineItemAction: LineItemAction = new LineItemAction();

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
                .subscribe(value => {
                    this.deliveryActions = value[0];
                    this.exceptionTypes = value[1];
                    this.sources = value[2];
                    this.reasons = value[3];
                });
        }
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public save(): void
    {
        if (this.currentForm.form.valid) {

            const exceptionServiceObservable = (this.isEditMode)
                ? this.editExceptionsService.put(this.lineItemAction)
                : this.editExceptionsService.post(this.lineItemAction);

            exceptionServiceObservable
                .takeWhile(() => this.isAlive)
                .subscribe((responseData: EditLineItemException) => {
                    this.onSave.emit(responseData);
                });
        }
    }

    private mapToLineItemAction(ex: EditLineItemExceptionDetail): void
    {
        if (!_.isNil(ex))
        {
            this.fillLookups();

            this.lineItemAction.id = ex.id;
            this.lineItemAction.lineItemId = ex.lineItemId;
            this.lineItemAction.deliverAction = this.findKeyByValue(this.deliveryActions, ex.action);
            this.lineItemAction.exceptionType = this.findKeyByValue(this.exceptionTypes, ex.exception);
            this.lineItemAction.quantity = ex.quantity | 0;
            this.lineItemAction.source = this.findKeyByValue(this.sources, ex.source);
            this.lineItemAction.reason = this.findKeyByValue(this.reasons, ex.reason);
        }
    }

    private findKeyByValue(lookups: Array<ILookupValue>, value: string): number | undefined
    {
        if (!_.isNil(value)) {
            const lu = _.find(lookups, ['value', value]);
            if (!_.isNil(lu)) {
                return +lu.key;
            }
        }
        return undefined;
    }

    public close(): void
    {
        this.lineItemAction = new LineItemAction();
        this.onCancel.emit();
    }
}