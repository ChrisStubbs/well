import { Component, Input, Output, EventEmitter, ViewChild, ElementRef }    from '@angular/core';
import { IObservableAlive }                                                 from '../shared/IObservableAlive';
import { EditExceptionsService }                                            from './editExceptionsService';
import { EditLineItemException }                                            from './editLineItemException';
import { LookupService, ILookupValue, LookupsEnum }                         from '../shared/services/services';
import * as _                                                               from 'lodash';
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
    '.group7{ width: 14%} ' +
    '.group8{ width: 38px; text-align: right; padding-right: 2px }']
})
export class EditExceptionsComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public source: Array<EditLineItemException>;
    public exceptionTypes: Array<ILookupValue>;

    @Input() public set ids(value: Array<number>)
    {
        if (_.isNil(this.source)) {
            this.source = [];
            this.editExceptionService.get(value)
                .takeWhile(() => this.isAlive)
                .subscribe((values: Array<EditLineItemException>) => {
                    this.source = values
                });
        }
    };

    @Output() public close: EventEmitter<any> = new EventEmitter(undefined);
    @ViewChild('closeExceptionsModal') public closeBtn: ElementRef;

    private openModal: boolean = false;
    private lineItemToHandle: EditLineItemException;
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
            });
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public totalPerGroup(perCol: string, id: number): number
    {
        return _.chain(this.source)
            .filter((current: EditLineItemException) => current.id == id)
            .map(perCol)
            .sum()
            .value();
    }

    public addException(line: EditLineItemException): void
    {
        this.openModal = true;
        this.isEditMode = false;
        this.lineItemToHandle = line;
    }

    public editLine(line: EditLineItemException, event: any): void
    {
        this.openModal = true;
        this.isEditMode = true;
        this.lineItemToHandle = line;

        event.preventDefault()
    }

    public closeEdit(): void
    {
        this.close.emit(undefined);
        this.source = undefined;
    }

    public selectLineItens(select: boolean, id?: number): void
    {
        let filterToApply = function(item: EditLineItemException): boolean { return true; };

        if (!_.isNull(id))
        {
            //if it is not null it means the user click on a group
            filterToApply = function(item: EditLineItemException): boolean { return item.id == id; };
        }

        _.chain(this.source)
            .filter(filterToApply)
            .map(current => current.isSelected = select)
            .value();
    }

    public allChildrenSelected(id?: number): boolean
    {
        let filterToApply = function(item: EditLineItemException): boolean { return true; };

        if (!_.isNull(id))
        {
            //if it is not null it means the user click on a group
            filterToApply = function(item: EditLineItemException): boolean { return item.id == id; };
        }

        return _.every(
            _.filter(this.source, filterToApply),
            current => current.isSelected);
    }

    private closeAddExceptionModal(): void
    {
        this.openModal = false;
        this.closeBtn.nativeElement.click();
    }

    private mergeNewException(data: Array<EditLineItemException>)
    {
        //the server is sending back all the exceptions for this lineitem
        //so first thing i need to do is;
        //remove existing lineItem
        _.remove(this.source, current => current.id == data[0].id);

        //now re-add it
        _.map(data, current => this.source.push(current));
    }

    private exceptionSaved(data: Array<EditLineItemException>): void
    {
        this.closeAddExceptionModal();
        this.mergeNewException(data);
    }

    private cancelExeption()
    {
        this.closeAddExceptionModal();
    }
}