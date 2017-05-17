import { Component, Input }                         from '@angular/core';
import { IObservableAlive }                         from '../shared/IObservableAlive';
import { EditExceptionService }                     from './editExceptionsService';
import { IEditLineItemException }                   from './editLineItemException';
import {LookupService, ILookupValue, LookupsEnum}   from '../shared/services/services';
import * as _                                       from 'lodash';
import 'rxjs/add/operator/mergeMap';

@Component({
    selector: 'ow-editExceptions',
    templateUrl: './app/exceptions/editExceptionsComponent.html',
    providers: [LookupService, EditExceptionService],
    styles: ['.groupRow { display: flex} ' +
    '.groupRow div { display: table-cell; padding-right: 9px; padding-left: 9px} ' +
    '.group1{ width: 11%} ' +
    '.group2{ width: 9%} ' +
    '.group3{ width: 7%; text-align: right} ' +
    '.group4{ width: 7%; text-align: right} ' +
    '.group5{ width: 7%; text-align: right} ' +
    '.group6{ width: 45%} ' +
    '.group7{ width: 14%} ']
})
export class EditExceptionsComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public source: Array<IEditLineItemException>;
    public exceptionTypes: Array<ILookupValue>;
    public selectedException: string;

    @Input() public ids: Array<number>;

    private allItems: Array<IEditLineItemException>;

     constructor(
         private lookupService: LookupService,
         private editExceptionService: EditExceptionService) { }

    public ngOnInit(): void
    {
        this.editExceptionService.get(this.ids)
            .takeWhile(() => this.isAlive)
            .subscribe((values: Array<IEditLineItemException>) =>
            {
                this.allItems = values;
                this.source = values
            });

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

    public totalPerGroup(perCol: string, id: number): number
    {
        return _.chain(this.source)
            .filter((current: IEditLineItemException) => current.id == id)
            .map(perCol)
            .sum()
            .value();
    }

    public addException(id: number): void
    {
        //
    }

    public filter(): void
    {
        if (_.isNil(this.selectedException))
        {
            this.source = this.allItems;
        }
        else
        {
            this.source = _.filter(
                this.allItems,
                (current: IEditLineItemException) => current.exception == this.selectedException);
        }
    }

    public editLine(line: IEditLineItemException): void
    {
        console.log(line);
        //i have to do something here
    }
}