import {
        Component,
        ViewChild,
        ElementRef,
        EventEmitter,
        Output,
        ViewEncapsulation
}                                           from '@angular/core';
import { NgForm }                           from '@angular/forms';
import { IObservableAlive }                 from '../IObservableAlive';
import * as _                               from 'lodash';
import { ILookupValue }                     from '../services/ILookupValue';
import { LookupsEnum }                      from '../services/lookupsEnum';
import { LookupService }                    from '../services/lookupService';
import { EditExceptionsService }            from '../../exceptions/editExceptionsService';
import { Observable }                       from 'rxjs';
import { LineItemAction }                   from '../../exceptions/lineItemAction';
import { EditLineItemException }            from '../../exceptions/editLineItemException';
import { LineItemActionComment }            from '../../exceptions/lineItemAction';

@Component({
    selector: 'action-edit',
    templateUrl: 'app/shared/action/actionEditComponent.html',
    providers: [LookupService, EditExceptionsService],
    styleUrls: ['app/shared/action/actionEditComponent.css'],
    encapsulation: ViewEncapsulation.None,
})
export class ActionEditComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public source: EditLineItemException = new EditLineItemException();
    public originalLineItems: Array<LineItemAction> = [];

    @Output() public onSave = new EventEmitter<EditLineItemException>();
    @ViewChild('showModal') public showModal: ElementRef;
    @ViewChild('closeModal') public closeModal: ElementRef;
    @ViewChild('actionEditForm') private currentForm: NgForm;

    private deliveryActions: Array<ILookupValue>;
    private sources: Array<ILookupValue>;
    private reasons: Array<ILookupValue>;
    private exceptionTypes: Array<ILookupValue>;
    private commentReasons: Array<ILookupValue>;
    private lineItemActionsToRemove: Array<LineItemAction> = [];
    private lineItemActions: Array<LineItemAction> = [];
    private errorInvoiceQty: string = 'The total exception quantity cannot exceed the invoiced quantity';
    private errorCommentRequired: string = 'When editing a quantity a comment is required';
    private creditAction: number;
    private closeAction: number;

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
                this.lookupService.get(LookupsEnum.JobDetailReason),
                this.lookupService.get(LookupsEnum.CommentReason)

            )
                .takeWhile(() => this.isAlive)
                .subscribe(value =>
                {
                    this.deliveryActions = value[0];
                    this.exceptionTypes = value[1];
                    this.sources = value[2];
                    this.reasons = value[3];
                    this.commentReasons = JSON.parse(JSON.stringify(value[4]));
                    this.commentReasons.unshift({ key: undefined, value: undefined });

                    this.creditAction = +this.deliveryActions.
                        find((current: ILookupValue) => current.value == 'Credit').key;

                    this.closeAction = +this.deliveryActions.
                    find((current: ILookupValue) => current.value == 'Close').key;
                });
        }
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    public addAction(): void
    {
        this.lineItemActions.push(new LineItemAction());
    }

    public removeItem(index: number): void
    {
        const item = this.lineItemActions.splice(index, 1);
        this.lineItemActionsToRemove.push(item[0]);
    }

    public show(editLineItemException: EditLineItemException)
    {
        this.loadSource(editLineItemException);
        this.showModal.nativeElement.click();
    }

    public close(): void
    {
        this.lineItemActions = [];
        this.closeModal.nativeElement.click();
    }

    public save(): void
    {
        if (this.currentForm.form.valid)
        {
            _.map(this.lineItemActions, (current: LineItemAction) => {
                if (current.deliveryAction == this.closeAction)
                {
                    current.quantity = 0;
                }
            });

            this.editExceptionsService.patch(this.source)
                .takeWhile(() => this.isAlive)
                .subscribe((responseData: EditLineItemException) =>
                {
                    this.loadSource(responseData);
                    this.onSave.emit(responseData);
                    this.close();
                });
        }
    }

    private loadSource(editLineItemException: EditLineItemException): void
    {
        this.source = editLineItemException;
        this.originalLineItems = JSON.parse(JSON.stringify(editLineItemException.lineItemActions));
        this.lineItemActions = this.source.lineItemActions || [];
    }

    private actionClose: number = 2;
    private qtyChanged(item: LineItemAction, index: number): void
    {
        if (this.isOriginalQuantity(item))
        {
            item.commentReason = undefined;
        }

        this.validate(item, index);
    }

    private validate(item: LineItemAction, index: number): void
    {
        this.validateTotalQty();
        this.validateComment(item, index);
    }

    private validateTotalQty(): void
    {
        const form = this.currentForm.form;
        const totalLineQty = _.sumBy(this.lineItemActions, x => x.quantity);

        this.deleteError(form, this.errorInvoiceQty);
        if (totalLineQty > this.source.invoiced)
        {
            this.setError(form, this.errorInvoiceQty);

        }
    }

    public setError(ctl: any, error: string) 
    {
        if (!ctl.errors)
        {
            ctl.setErrors({ key: error });
        }
        else
        {
            ctl.errors.key = ctl.errors.key + ', ' + error;
        }
    }

    public deleteError(ctl: any, error: string)
    {
        if (ctl.errors)
        {
            const array = ctl.errors.key.split(', ');
            _.filter(array, x => x === error);
            if (array.length === 0)
            {
                delete ctl.errors[error];

            } else
            {
                ctl.errors.key = array.join(', ');
            }
        }
    }

    private getFormValidationErrors()
    {
        let errors = [];
        const form = this.currentForm.form;
        if (form.errors != undefined)
        {
            errors = form.errors.key.split(', ');
        }
        return errors;
    }

    private getOriginalItem(id: number): LineItemAction
    {
        return _.find(this.originalLineItems, x => x.id === id);
    }

    private isOriginalQuantity(item: LineItemAction): boolean
    {
        const originalItem = this.getOriginalItem(item.id);
        return !_.isNil(originalItem) && originalItem.quantity === item.quantity;
    }

    private isCommentMandatory(item: LineItemAction): boolean
    {
        return !this.isOriginalQuantity(item)
            && item.deliveryAction != this.actionClose
            && item.id != 0;
    }

    private validateComment(item: LineItemAction, index: number)
    {
        const form = this.currentForm.form;
        if (item.id !== 0)
        {
            const commentCtl = form.controls['commentReasonId' + index];

            this.deleteError(form, this.errorCommentRequired);
            this.deleteError(commentCtl, this.errorCommentRequired);

            if (!this.isOriginalQuantity(item) && (!commentCtl.value || commentCtl.value === 'undefined'))
            {
                this.setError(form, this.errorCommentRequired);
                this.setError(commentCtl, this.errorCommentRequired);
            }
        }
    }

    private deliveryActionChange(item: LineItemAction, index: number): void
    {
        if (item.deliveryAction === this.actionClose)
        {
            item.quantity = 0;
            this.validate(item, index);
        }
    }

    private commentChange(item: LineItemAction, event, index: number): void
    {
        let newComment = _.find(item.comments, x => x.id === 0);
        if (_.isNil(newComment))
        {
            if (!item.comments)
            {
                item.comments = [];
            }

            newComment = new LineItemActionComment();
            item.comments.push(newComment);
        }
        newComment.commentReasonId = +event.target.value;

        if (event.target.value === 'undefined')
        {
            item.comments.pop();
        }

        this.validate(item, index);
    }

    private isFormValid()
    {
        return this.currentForm.form.valid;
    }

    private hasComments(item: LineItemAction): boolean {
        const existingComments = _.filter(item.comments, x => x.id !== 0);
        return existingComments.length > 0;
    }
}