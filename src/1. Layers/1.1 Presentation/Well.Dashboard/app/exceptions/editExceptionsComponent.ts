import { Component, Input }                         from '@angular/core';
import { IObservableAlive }                         from '../shared/IObservableAlive';
import { EditExceptionsService }                     from './editExceptionsService';
import { IEditLineItemException }                   from './editLineItemException';
import {LookupService, ILookupValue, LookupsEnum}   from '../shared/services/services';

import 'rxjs/add/operator/mergeMap';

@Component({
    selector: 'ow-editExceptions',
    templateUrl: './app/exceptions/editExceptionsComponent.html',
     providers: [LookupService]
})
export class EditExceptionsComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    // public source: Array<IEditLineItemException>;
    public exceptionTypes: Array<ILookupValue>;
    //
    // @Input() public ids: Array<number>;
    //
     constructor(
         private lookupService: LookupService) { }

    public ngOnInit()
    {
        // this.editExceptionService.get(this.ids)
        //     .takeWhile(() => this.isAlive)
        //     .subscribe((values: Array<IEditLineItemException>) => this.source = values);

        this.lookupService.get(LookupsEnum.ExceptionType)
            .takeWhile(() => this.isAlive)
            .subscribe((value: Array<ILookupValue>) =>
            {
                this.exceptionTypes = value;
                console.log(value);
            });
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public editLine(line: IEditLineItemException): void
    {
        console.log(line);
        //i have to do something here
    }
}