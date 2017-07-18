﻿import {
    Component,
    ViewChild,
    ElementRef,
    EventEmitter,
    Output,
    ViewEncapsulation
    } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AbstractControl, FormGroup, FormArray, FormBuilder, Validators, ValidationErrors, Validator }
    from '@angular/forms';
import { IObservableAlive } from '../IObservableAlive';
import * as _ from 'lodash';
import { ILookupValue } from '../services/ILookupValue';
import { LookupsEnum } from '../services/lookupsEnum';
import { LookupService } from '../services/lookupService';
import { EditExceptionsService } from '../../exceptions/editExceptionsService';
import { Observable } from 'rxjs';
import { LineItemAction } from '../../exceptions/lineItemAction';
import { EditLineItemException } from '../../exceptions/editLineItemException';
import { LineItemActionComment } from '../../exceptions/lineItemAction';

@Component({
    selector: 'action-edit-refactor',
    templateUrl: 'app/shared/action/actionEditComponentRefactor.html',
    providers: [LookupService, EditExceptionsService],
    styleUrls: ['app/shared/action/actionEditComponent.css'],
    encapsulation: ViewEncapsulation.None
})
export class ActionEditComponentRefactor implements IObservableAlive {
    public isAlive: boolean = true;
    public source: EditLineItemException = new EditLineItemException();
    public originalLineItems: Array<LineItemAction> = [];

    @Output() public onSave = new EventEmitter<EditLineItemException>();
    @ViewChild('showEditActionsModal') public showModal: ElementRef;
    @ViewChild('closeModal') public closeModal: ElementRef;

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
        private editExceptionsService: EditExceptionsService,
        private formBuilder: FormBuilder) { }

    public ngOnInit() {
        this.fillLookups();

        this.createLineItemActionsForm(new EditLineItemException());
    }

    private fillLookups(): void {
        if (_.isNil(this.deliveryActions)) {
            this.deliveryActions = [];
            Observable.forkJoin(
                    this.lookupService.get(LookupsEnum.DeliveryAction),
                    this.lookupService.get(LookupsEnum.ExceptionType),
                    this.lookupService.get(LookupsEnum.JobDetailSource),
                    this.lookupService.get(LookupsEnum.JobDetailReason),
                    this.lookupService.get(LookupsEnum.CommentReason)

                )
                .takeWhile(() => this.isAlive)
                .subscribe(value => {
                    this.deliveryActions = value[0];
                    this.exceptionTypes = value[1];
                    this.sources = value[2];
                    this.reasons = value[3];
                    this.commentReasons = value[4];

                    this.creditAction = +this.deliveryActions.
                        find((current: ILookupValue) => current.value == 'Credit').key;

                    this.closeAction = +this.deliveryActions.
                        find((current: ILookupValue) => current.value == 'Close').key;
                });
        }
    }

    public ngOnDestroy() {
        this.isAlive = false;
    }

    public addAction(): void {
        const group = this.createLineItemActionFromGroup(new LineItemAction());
        this.actionsGroup.push(group);
    }

    public removeItem(index: number): void {
        this.actionsGroup.removeAt(index);
    }

    public show(editLineItemException: EditLineItemException) {
        this.loadSource(editLineItemException);

        this.createLineItemActionsForm(editLineItemException);

        this.showModal.nativeElement.click();
    }

    public close(): void {
        this.lineItemActions = [];
        this.closeModal.nativeElement.click();
    }

    public save(): void {
        const self = this;
        console.log(self);
        //if (this.currentForm.form.valid) {
        //    _.map(this.lineItemActions, (current: LineItemAction) => {
        //        if (current.deliveryAction == this.closeAction) {
        //            current.quantity = 0;
        //        }
        //    });

        //    this.editExceptionsService.patch(this.source)
        //        .takeWhile(() => this.isAlive)
        //        .subscribe((responseData: EditLineItemException) => {
        //            this.loadSource(responseData);
        //            this.onSave.emit(responseData);
        //            this.close();
        //        });
        //}
    }

    private loadSource(editLineItemException: EditLineItemException): void {
        this.source = editLineItemException;
        this.originalLineItems = JSON.parse(JSON.stringify(editLineItemException.lineItemActions));
        this.lineItemActions = this.source.lineItemActions || [];
    }
    
    private validateTotalQty(): void {
        //const form = this.currentForm.form;
        //const totalLineQty = _.sumBy(this.lineItemActions, x => x.quantity);

        //this.deleteError(form, this.errorInvoiceQty);
        //if (totalLineQty > this.source.invoiced) {
        //    this.setError(form, this.errorInvoiceQty);
        //}
    }

    public setError(ctl: any, error: string) {
        if (!ctl.errors) {
            ctl.setErrors({ key: error });
        }
        else {
            ctl.errors.key = ctl.errors.key + ', ' + error;
        }
    }

    public deleteError(ctl: any, error: string) {
        if (ctl.errors) {
            const array = ctl.errors.key.split(', ');
            _.filter(array, x => x === error);
            if (array.length === 0) {
                delete ctl.errors[error];

            } else {
                ctl.errors.key = array.join(', ');
            }
        }
    }

    private getFormValidationErrors() {

        //let errors = [];
        //const form = this.currentForm.form;
        //if (form.errors != undefined) {
        //    errors = form.errors.key.split(', ');
        //}
        //return errors;

        return [];
    }

    private isFormValid() {
        //return this.currentForm.form.valid;
    }

    private hasComments(item: LineItemAction): boolean {
        const existingComments = _.filter(item.comments, x => x.id !== 0);
        return existingComments.length > 0;
    }

    // Reactive form impl
    private actionsForm: FormGroup;
    private actionsGroup: FormArray;

    private createLineItemActionsForm(editLineItemException: EditLineItemException) {
        const self = this;

        this.actionsGroup = this.formBuilder.array(_.map(editLineItemException.lineItemActions,
                function(item: LineItemAction) {
                    return self.createLineItemActionFromGroup(item);
                }),
            (control) => this.validateTotalQuantity(control));

        this.actionsForm = this.formBuilder.group({
                actionsGroup: this.actionsGroup
            }
        );
    }

    private validateTotalQuantity(formArray: AbstractControl): ValidationErrors {
        const sum = _.sumBy(formArray.value,
            item => {
                return item.quantity || 0;
            });

        if (sum > this.source.invoiced) {
            return { totalQuantity: true, message: 'Quanty invalid ' + sum };
        }

        return undefined;
    }

    private createLineItemActionFromGroup(item: LineItemAction) {
        const validator = new LineItemActionValidator(item);

        const action = this.formBuilder.control(item.deliveryAction, Validators.required);
        const quantity = this.formBuilder.control(item.quantity, [Validators.pattern('^[0-9]+$')]);
        const commentReason = this.formBuilder.control({
            value: item.commentReason,
            disabled: true
        });
        const exceptionType = this.formBuilder.control(item.exceptionType);
        const source = this.formBuilder.control(item.source);
        const reason = this.formBuilder.control(item.reason);
        const originator = this.formBuilder.control(item.originator);

        action.valueChanges.subscribe((value) => {
            // When status is close - disable other fields
            if (Number(value) == 2) {
                quantity.disable();
                commentReason.disable();
                exceptionType.disable();
                source.disable();
                reason.disable();

            } else {
                quantity.enable();
                commentReason.enable();
                exceptionType.enable();
                source.enable();
                reason.enable();
            }
        });

        quantity.valueChanges.subscribe((value) => {
            if (!validator.isNewLineItemAction() && validator.quantityDifferentFromOriginal(value)) {
                commentReason.enable();
            } else {
                commentReason.disable();
            }
        });

        return this.formBuilder.group({
                action: action,
                quantity: quantity,
                commentReason: commentReason,
                exceptionType: exceptionType,
                source: source,
                reason: reason,
                originator
            },
            { validator: (control) => validator.validate(control) });
    }
}

class LineItemActionValidator implements Validator {
    constructor(private lineItemAction: LineItemAction) {}

    public validate(group: FormGroup): ValidationErrors {
        const actionValue = group.value.action;

        switch (Number(actionValue)) {
        case 0:
            return this.validateNotDefinedAction(group);
        case 1:
            return this.validateCreditAction(group);
        case 2:
            return this.validateCloseAction(group);
        default:
            throw new Error('Unknown action type : ' + actionValue);
        }
    }

    private validateNotDefinedAction(group: FormGroup): ValidationErrors {
        this.validateQuantity(group);
        this.validateComment(group);
        this.validateExceptionType(group);

        // May want to aggregate errors here for whole group
        return undefined;
    }

    private validateCreditAction(group: FormGroup): ValidationErrors {
        this.validateQuantity(group);
        this.validateComment(group);   
        this.validateExceptionType(group);

        // Source required
        const sourceCtrl = group.controls['source'];
        if (Number(sourceCtrl.value) == 0) {
            sourceCtrl.setErrors({ required: true });
        }

        // Reason required
        const reasonCtrl = group.controls['reason'];
        if (Number(reasonCtrl.value) == 0) {
            reasonCtrl.setErrors({ required: true });
        }

        return undefined;
    }

    private validateCloseAction(group: FormGroup): ValidationErrors {
        console.log('validateCloseAction');
        // Set default values here ?

        return undefined;
    }

    private validateQuantity(group: FormGroup): void {
        // Quantity required
        const quantityCtrl = group.controls['quantity'];
        const quantityRequired = Validators.required(quantityCtrl);
        if (quantityRequired) {
            quantityCtrl.setErrors(quantityRequired);
        }
    }

    private validateComment(group: FormGroup): void {   
        // Comment required for new or when quantity changes
        if (this.isNewLineItemAction() ||
            this.quantityDifferentFromOriginal(group.value.quantity)) {

            const commentReasonCtrl = group.controls['commentReason'];
            const commentRequired = Validators.required(commentReasonCtrl);
            if (commentRequired) {
                commentReasonCtrl.setErrors(commentRequired);
            }
        }
    }

    private validateExceptionType(group: FormGroup): void {
        // Exception type required
        const exceptionTypeCtrl = group.controls['exceptionType'];
        const exceptionTypeRequired = Validators.required(exceptionTypeCtrl);
        if (exceptionTypeRequired) {
            exceptionTypeCtrl.setErrors(exceptionTypeRequired);
        }
    }

    public quantityDifferentFromOriginal(value: number): boolean {
        return value != this.lineItemAction.quantity;
    }

    public isNewLineItemAction(): boolean {
        return (!this.lineItemAction.id || this.lineItemAction.id == 0);
    }
}