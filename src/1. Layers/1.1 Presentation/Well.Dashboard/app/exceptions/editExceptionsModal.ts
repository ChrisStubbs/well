import { Component, Input, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { IObservableAlive } from '../shared/IObservableAlive';
import { LookupService, ILookupValue, LookupsEnum } from '../shared/services/services';
import { IEditLineItemException } from './editLineItemException';
import { LineItemAction } from './lineItemAction';

@Component({
    selector: 'edit-exceptions-modal',
    templateUrl: './app/exceptions/editExceptionsModal.html'
})
export class EditExceptionsModal implements IObservableAlive
{
    @Input() public items: Array<IEditLineItemException> = [];
    @Input() public isEditMode: boolean = false;

    public isAlive: boolean = true;
    private title: string = this.isEditMode ? 'Edit Exceptions' : 'Add Exceptions';
    private deliveryActions: Array<ILookupValue> = [];
    private sources: Array<ILookupValue> = [];
    private reasons: Array<ILookupValue> = [];
    private exceptionTypes: Array<ILookupValue> = [];
    private lineItemAction: LineItemAction = new LineItemAction();
    
    constructor(private lookupService: LookupService) { }

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
        console.log(this.lineItemAction);
    }

    public cancel(): void
    {
        this.lineItemAction = new LineItemAction();
    }

}