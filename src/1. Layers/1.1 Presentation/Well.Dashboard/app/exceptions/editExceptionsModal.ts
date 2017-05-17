import { Component } from '@angular/core';
import { IObservableAlive } from '../shared/IObservableAlive';
import { LookupService, ILookupValue, LookupsEnum } from '../shared/services/services';

@Component({
    selector: 'edit-exceptions-modal',
    templateUrl: './app/exceptions/editExceptionsModal.html'
})
export class EditExceptionsModal implements IObservableAlive
{
    public isAlive: boolean = true;
    private isEditMode: boolean = false;
    private title: string = this.isEditMode ? 'Edit Exceptions' : 'Add Exceptions';
    private deliveryActions: Array<ILookupValue> = [];
    private sources: Array<ILookupValue> = [];
    private reasons: Array<ILookupValue> = [];
    private exceptionTypes: Array<ILookupValue> = [];

    constructor(
        private lookupService: LookupService) { }

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
}