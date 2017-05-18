import {Component, Input, Output, EventEmitter}     from '@angular/core';
import { IObservableAlive }                         from '../shared/IObservableAlive';
import { EditExceptionsService }                    from './editExceptionsService';
import { IEditLineItemException }                   from './editLineItemException';
import {LookupService, ILookupValue, LookupsEnum}   from '../shared/services/services';
import * as _                                       from 'lodash';
import 'rxjs/add/operator/mergeMap';

@Component({
    selector: 'ow-editExceptions',
    templateUrl: './app/exceptions/editExceptionsComponent.html',
    providers: [LookupService, EditExceptionsService],
    styles: ['.groupRow { display: flex} ' +
    '.groupRow div { display: table-cell; padding-right: 9px; padding-left: 9px} ' +
    '.group1{ width: 9%} ' +
    '.group2{ width: 9%} ' +
    '.group3{ width: 7%; text-align: right} ' +
    '.group4{ width: 7%; text-align: right} ' +
    '.group5{ width: 7%; text-align: right} ' +
    '.group6{ width: 47%} ' +
    '.group7{ width: 14%} ']
})
export class EditExceptionsComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public source: Array<IEditLineItemException>;
    public exceptionTypes: Array<ILookupValue>;

    @Input() public set ids(value: Array<number>)
    {
        if (_.isNil(this.source)) {
            this.source = [];
            this.editExceptionService.get(value)
                .takeWhile(() => this.isAlive)
                .subscribe((values: Array<IEditLineItemException>) => this.source = values);
        }
    };

    @Output() public close: EventEmitter<any> = new EventEmitter(undefined);

    private openModal: boolean = false;
    private lineItemToHandle: IEditLineItemException;
    private isEditMode: boolean;

    constructor(
         private lookupService: LookupService,
         private editExceptionService: EditExceptionsService) { }

    public ngOnInit(): void
    {
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

    public addException(line: IEditLineItemException): void
    {
        this.openModal = true;
        this.isEditMode = false;
        this.lineItemToHandle = line;
    }

    public editLine(line: IEditLineItemException): void
    {
        this.openModal = true;
        this.isEditMode = true;
        this.lineItemToHandle = line;
    }

    public lineItemsToHandle(): Array<IEditLineItemException>
    {
        return [this.lineItemToHandle];
    }

    public closeEdit(): void
    {
        this.close.emit(undefined);
        this.source = undefined;
    }
    //
    // public x(): void
    // {
    //     console.log('test');
    //
    // }
}