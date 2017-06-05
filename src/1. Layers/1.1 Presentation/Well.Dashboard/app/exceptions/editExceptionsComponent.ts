import { Component, Input, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { IObservableAlive } from '../shared/IObservableAlive';
import { EditExceptionsService } from './editExceptionsService';
import { EditLineItemException } from './editLineItemException';
import { LookupService, ILookupValue, LookupsEnum } from '../shared/services/services';
import * as _ from 'lodash';
import 'rxjs/add/operator/mergeMap';

@Component({
    selector: 'ow-editExceptions',
    templateUrl: './app/exceptions/editExceptionsComponent.html',
    providers: [LookupService, EditExceptionsService, CurrencyPipe],
    styles: ['.group1{ width: 3%} ' +
        '.group2{ width: 6%} ' +
        '.group3{ width: 9%} ' +
        '.group4{ width: 7%; text-align: right} ' +
        '.group5{ width: 7%; text-align: right} ' +
        '.group6{ width: 7%; text-align: right} ' +
        '.group7{ width: 47%} ' +
        '.group8{ width: 14%} ' +
        '.group9{ width: 38px; text-align: right; padding-right: 2px }']
})
export class EditExceptionsComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public source: Array<EditLineItemException>;
    public exceptionTypes: Array<ILookupValue>;

    @Input() public set ids(value: Array<number>)
    {
        if (_.isNil(this.source))
        {
            this.source = [];
            this.editExceptionService.get(value)
                .takeWhile(() => this.isAlive)
                .subscribe((values: Array<EditLineItemException>) =>
                {
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
        private editExceptionService: EditExceptionsService,
        private currencyPipe: CurrencyPipe) { }

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

        event.preventDefault();
    }

    public closeEdit(): void
    {
        this.close.emit(undefined);
        this.source = undefined;
    }

    public selectLineItems(select: boolean, id?: number): void
    {
        let filterToApply = function (item: EditLineItemException): boolean { return true; };

        if (!_.isNull(id))
        {
            //if it is not null it means the user click on a group
            filterToApply = function (item: EditLineItemException): boolean { return item.id == id; };
        }

        _.chain(this.source)
            .filter(filterToApply)
            .map(current => current.isSelected = select)
            .value();
    }

    public allChildrenSelected(id?: number): boolean
    {
        let filterToApply = function (item: EditLineItemException): boolean { return true; };

        if (!_.isNull(id))
        {
            //if it is not null it means the user click on a group
            filterToApply = function (item: EditLineItemException): boolean { return item.id == id; };
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

    private mergeNewException(data: EditLineItemException)
    {
        //the server is sending back all the exceptions for this lineitem
        //so first thing i need to do is;
        //remove existing lineItem
        let isExpanded: boolean = false;
        _.remove(this.source, current =>
        {

            if (current.id == data.id)
            {
                isExpanded = current.isExpanded;
                return true;
            }

            return false;
        });

        //now re-add it
        data.isExpanded = isExpanded;
        this.source.push(data);
    }

    private exceptionSaved(data: EditLineItemException): void
    {
        this.closeAddExceptionModal();
        this.mergeNewException(data);
    }

    private cancelExeption()
    {
        this.closeAddExceptionModal();
    }

    public selectedItems(): Array<EditLineItemException>
    {
        return _.filter(this.source, x => x.isSelected);
    }

    public areAllExpanded(): boolean
    {
        let value: boolean = true;

        _.forEach(this.source, current => value = value && current.isExpanded);

        return value;
    }

    public expandGroup(event: any): void
    {
        const action: boolean = !this.areAllExpanded();

        _.map(this.source, current => current.isExpanded = action);
        event.preventDefault();
    }

    public getActionSummaryData(): string
    {
        //TODO: Hook up the userThresholdValue
        //TODO: get the value not tthe quantity
        const totalCreditValue = this.currencyPipe.transform(
            _.sumBy(this.selectedItems(), x => x.quantity)
            , 'GBP', true);
        const usersThreshold = this.currencyPipe.transform(1000.00, 'GBP', true);
        const summary = `The total to be actioned for the selection is ${totalCreditValue}. 
                        The maximum you are allowed to credit is ${usersThreshold}. 
                        Any items over your threshold will be sent for approval`;
        return summary;
    }
}