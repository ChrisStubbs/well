import { Component, Input, ViewChild, EventEmitter, Output, ElementRef } from '@angular/core';
import { NgForm } from '@angular/forms';
import { IObservableAlive } from '../shared/IObservableAlive';
import { LookupService, ILookupValue, LookupsEnum } from '../shared/services/services';
import { IEditLineItemException } from './editLineItemException';
import { LineItemAction } from './lineItemAction';
import { EditExceptionsService } from './editExceptionsService';
import * as _ from 'lodash';
import { Observable } from 'rxjs'

@Component({
    selector: 'edit-exceptions-modal',
    templateUrl: './app/exceptions/editExceptionsModal.html',
    providers: [LookupService, EditExceptionsService]
})
export class EditExceptionsModal implements IObservableAlive {
    private mItem: IEditLineItemException;

    @Input() public set item(value: IEditLineItemException)
    {
        this.mItem = value;
        this.mapToLineItemAction(value);
    };

    public get item(): IEditLineItemException
    {
        return this.mItem;
    }

    @Input() public isEditMode: boolean = false;
    @Output() public onSave = new EventEmitter();
    public isAlive: boolean = true;

    private deliveryActions: Array<ILookupValue> = [];
    private sources: Array<ILookupValue> = [];
    private reasons: Array<ILookupValue> = [];
    private exceptionTypes: Array<ILookupValue> = [];
    private lineItemAction: LineItemAction = new LineItemAction();
    @ViewChild('exceptionModalForm') private currentForm: NgForm;
    @ViewChild('cancelButton') private cancelButton: ElementRef;

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
        ).takeWhile(() => this.isAlive)
            .subscribe(value => {
                this.deliveryActions = value[0];
                this.exceptionTypes = value[1];
                this.sources = value[2];
                this.reasons = value[3];
            });
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
                .subscribe(responseData => {
                    this.onSave.emit(responseData);
                    this.close();
                });
        }
    }

    private mapToLineItemAction(ex: IEditLineItemException): void
    {
        if (!_.isNil(ex)) {
            this.lineItemAction.id = ex.lineItemActionId;
            this.lineItemAction.lineItemId = ex.id;
            this.lineItemAction.deliverAction = this.findKeyByValue(this.deliveryActions, ex.action);
            this.lineItemAction.exceptionType = this.findKeyByValue(this.exceptionTypes, ex.exception);
            this.lineItemAction.quantity = ex.quantity;
            this.lineItemAction.source = this.findKeyByValue(this.sources, ex.source);
            this.lineItemAction.reason = this.findKeyByValue(this.reasons, ex.reason);
        }
    }

    private findKeyByValue(lookups: Array<ILookupValue>, value: string): number
    {
        let key = 0;
        if (!_.isNil(value)) {
            const lu = _.find(lookups, ['value', value]);
            if (!_.isNil(lu)) {
                key = +lu.key;
            }
        }
        return key;
    }

    public close(): void
    {
        this.lineItemAction = new LineItemAction();
        this.cancelButton.nativeElement.click();
    }
}