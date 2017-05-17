import { Component, Input, ViewChild, EventEmitter, Output, ElementRef } from '@angular/core';
import { NgForm } from '@angular/forms';
import { IObservableAlive } from '../shared/IObservableAlive';
import { LookupService, ILookupValue, LookupsEnum } from '../shared/services/services';
import { IEditLineItemException } from './editLineItemException';
import { LineItemAction } from './lineItemAction';
import { EditExceptionsService } from './editExceptionsService';
import * as _ from 'lodash';

@Component({
    selector: 'edit-exceptions-modal',
    templateUrl: './app/exceptions/editExceptionsModal.html',
    providers: [LookupService, EditExceptionsService]
})
export class EditExceptionsModal implements IObservableAlive
{
    @Input() public items: Array<IEditLineItemException> = [];
    @Input() public isEditMode: boolean = false;
    @Output() public onSave = new EventEmitter();
    public isAlive: boolean = true;

    private title: string = this.isEditMode ? 'Edit Exceptions' : 'Add Exceptions';
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
        this.lookupService.get(LookupsEnum.DeliveryAction)
            .takeWhile(() => this.isAlive)
            .subscribe((value: Array<ILookupValue>) =>
            {
                this.deliveryActions = value;
            });

        this.lookupService.get(LookupsEnum.ExceptionType)
            .takeWhile(() => this.isAlive)
            .subscribe((value: Array<ILookupValue>) =>
            {
                this.exceptionTypes = value;
            });

        this.lookupService.get(LookupsEnum.JobDetailSource)
            .takeWhile(() => this.isAlive)
            .subscribe((value: Array<ILookupValue>) =>
            {
                this.sources = value;
            });

        this.lookupService.get(LookupsEnum.JobDetailReason)
            .takeWhile(() => this.isAlive)
            .subscribe((value: Array<ILookupValue>) =>
            {
                this.reasons = value;
            });
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    public save(): void
    {
        this.lineItemAction.ids = _.uniq(_.map(this.items, 'id'));

        if (this.currentForm.form.valid && this.lineItemAction.ids.length > 0)
        {
            if (this.isEditMode)
            {
                this.editExceptionsService.put(this.lineItemAction);
            }
            else
            {
                this.editExceptionsService.post(this.lineItemAction);
            }
        }
        this.onSave.emit({});
        this.close();
    }

    public close(): void
    {
        this.lineItemAction = new LineItemAction();
        this.cancelButton.nativeElement.click();
    }

}