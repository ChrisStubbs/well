import {
    Component,
    ViewChild,
    ElementRef,
    EventEmitter,
    Output,
    ViewEncapsulation
} from '@angular/core';
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
    selector: 'action-edit',
    templateUrl: 'app/shared/action/actionEditComponent.html',
    providers: [LookupService, EditExceptionsService],
    styleUrls: ['app/shared/action/actionEditComponent.css'],
    encapsulation: ViewEncapsulation.None
})
export class ActionEditComponent implements IObservableAlive {
    @Output() public onSave = new EventEmitter<EditLineItemException>();
    @ViewChild('showEditActionsModal') public showModal: ElementRef;
    @ViewChild('closeModal') public closeModal: ElementRef;
    public isAlive: boolean = true;
    public source: EditLineItemException = new EditLineItemException();
    private deliveryActions: Array<ILookupValue>;
    private sources: Array<ILookupValue>;
    private reasons: Array<ILookupValue>;
    private exceptionTypes: Array<ILookupValue>;
    private commentReasons: Array<ILookupValue>;
    private lineItemActions: Array<LineItemAction> = [];
    private creditAction: number;
    private closeAction: number;
    private bypassValue = 2;
    private closeActionValue = 2;
    private creditActionValue = 1;
    private actionsForm: FormGroup;
    private actionsGroup: FormArray;

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

    private getDeliveryActions() {
        const self = this;
        if (this.source.isProofOfDelivery) {
            return _.filter(this.deliveryActions,
                (action: ILookupValue) => {
                    return Number(action.key) != self.creditActionValue;
                });
        } else {
            return this.deliveryActions;
        }
    }

    public ngOnDestroy() {
        this.isAlive = false;
    }

    public addAction(): void {
        const lineItemAction = new LineItemAction();
        this.lineItemActions.push(lineItemAction);
        const group = this.createLineItemActionFromGroup(lineItemAction);
        this.actionsGroup.push(group);
    }

    public removeItem(index: number): void {
        this.lineItemActions.splice(index, 1);
        this.actionsGroup.removeAt(index);
    }

    public show(editLineItemException: EditLineItemException) {
        this.loadSource(editLineItemException);

        this.createLineItemActionsForm(editLineItemException);

        this.showModal.nativeElement.click();
    }

    public close(): void {
        this.closeModal.nativeElement.click();
    }

    public save(): void {
        if (this.actionsForm.valid) {

            _.each(this.actionsForm.controls['actionsGroup'].value,
                (value: any, index: number) => {
                    const lineItemAction = this.lineItemActions[index];
                    // patch line item actions
                    lineItemAction.deliveryAction = Number(value.action);
                    lineItemAction.quantity = (value.quantity) ? Number(value.quantity) : 0;
                    lineItemAction.source = (value.source) ? Number(value.source) : 0;
                    lineItemAction.reason = (value.reason) ? Number(value.reason) : 0;

                    if (!this.isBypassExceptionType(lineItemAction.exceptionType)) {
                        lineItemAction.exceptionType = (value.exceptionType) ? Number(value.exceptionType) : 0;
                    }

                    if (value.commentReason) {
                        lineItemAction.commentReason = value.commentReason;
                        const newComment = new LineItemActionComment();
                        newComment.commentReasonId = Number(value.commentReason);
                        lineItemAction.comments.push(newComment);
                    }
                });

            this.editExceptionsService.patch(this.source)
                .takeWhile(() => this.isAlive)
                .subscribe((responseData: EditLineItemException) => {
                    this.loadSource(responseData);
                    this.onSave.emit(responseData);
                    this.close();
                });
        }
    }

    private loadSource(editLineItemException: EditLineItemException): void {
        this.source = editLineItemException;
        this.lineItemActions = this.source.lineItemActions || [];
    }

    private createLineItemActionsForm(editLineItemException: EditLineItemException) {
        const self = this;

        this.actionsGroup = this.formBuilder.array(_.map(editLineItemException.lineItemActions,
            function (item: LineItemAction) {

                return self.createLineItemActionFromGroup(item);
                //if (Number(item.exceptionType) == self.bypassValue) {
                //    return self.createBypassLineItemActionFormGroup(item);
                //} else {
                //    return self.createLineItemActionFromGroup(item);
                //}
            }),
            (control) => this.validateTotalQuantity(control));

        this.actionsForm = this.formBuilder.group({
            actionsGroup: this.actionsGroup
        });
    }

    private createBypassLineItemActionFormGroup(item: LineItemAction) {

        const action = this.formBuilder.control({
            value: item.deliveryAction,
            disabled: true
        }, Validators.required);
        const quantity = this.formBuilder.control({
            value: item.quantity,
            disabled: true
        }, [Validators.pattern('^[0-9]+$')]);
        const commentReason = this.formBuilder.control({
            value: item.commentReason,
            disabled: true
        });
        const exceptionType = this.formBuilder.control({
            value: item.exceptionType,
            disabled: true
        });
        const source = this.formBuilder.control({
            value: item.source,
            disabled: true
        });
        const reason = this.formBuilder.control({
            value: item.reason,
            disabled: true
        });
        const originator = this.formBuilder.control({
            value: item.originator,
            disabled: true
        });

        return this.formBuilder.group({
            action: action,
            quantity: quantity,
            commentReason: commentReason,
            exceptionType: exceptionType,
            source: source,
            reason: reason,
            originator
        });
    }

    private createLineItemActionFromGroup(item: LineItemAction) {
        const isBypassExceptionType = this.isBypassExceptionType(item.exceptionType);
        const validator = new LineItemActionValidator(item, isBypassExceptionType);
        const isCloseAction = Number(item.deliveryAction) == this.closeActionValue;

        const action = this.formBuilder.control({
            value: item.deliveryAction,
            disabled: !this.source.canEditActions
        },
            Validators.required);
        const quantity = this.formBuilder.control({
            value: item.quantity,
            disabled: (!this.source.canEditActions || isCloseAction)
        },
            [Validators.pattern('^[0-9]+$')]);
        const commentReason = this.formBuilder.control({
            value: item.commentReason,
            disabled: true
        });
        const exceptionType = this.formBuilder.control({
            value: item.exceptionType,
            disabled: (!this.source.canEditActions || isCloseAction)
        });
        const source = this.formBuilder.control({
            value: item.source,
            disabled: (!this.source.canEditActions || isCloseAction)
        });
        const reason = this.formBuilder.control({
            value: item.reason,
            disabled: (!this.source.canEditActions || isCloseAction)
        });

        action.valueChanges.subscribe((value) => {
            // When status is close - disable other fields
            const actionValue = Number(value);
            if (actionValue == this.closeActionValue) {
                quantity.disable();
                commentReason.disable();
                exceptionType.disable();
                source.disable();
                reason.disable();
                //Set default values
                quantity.setValue(undefined);
                commentReason.setValue(undefined);
                source.setValue(undefined);
                reason.setValue(undefined);

                if (!isBypassExceptionType) {
                    exceptionType.setValue(undefined);
                }

            } else {
                quantity.enable();
                source.enable();
                reason.enable();

                if (!isBypassExceptionType) {
                    exceptionType.enable();
                }
            }

            // Enable comment when action is different than previous
            if (actionValue == this.closeAction || actionValue == item.deliveryAction) {
                commentReason.disable();
            } else {
                commentReason.enable();
            }

        });

        quantity.valueChanges.subscribe((value) => {
            if ((!validator.isNewLineItemAction() && validator.quantityDifferentFromOriginal(value)) ||
                Number(action.value) != item.deliveryAction) {
                commentReason.enable();
            } else {
                commentReason.setValue(undefined);
                commentReason.disable();
            }
        });

        return this.formBuilder.group({
            action: action,
            quantity: quantity,
            commentReason: commentReason,
            exceptionType: exceptionType,
            source: source,
            reason: reason
        },
            { validator: (control) => validator.validate(control) });
    }

    private validateTotalQuantity(formArray: AbstractControl): ValidationErrors {
        const sum = _.sumBy(formArray.value,
            item => {
                return item.quantity || 0;
            });

        if (sum > this.source.invoiced) {
            return {
                totalQuantity: true,
                message: 'Total quantity (' + sum + ') is greater than invoiced (' + this.source.invoiced + ')'
            };
        }

        return undefined;
    }

    private hasComments(item: LineItemAction): boolean {
        const existingComments = _.filter(item.comments, x => x.id !== 0);
        return existingComments.length > 0;
    }

    private isBypassExceptionType(value: any): boolean {
        return Number(value) == this.bypassValue;
    }

    private hasBypassActions() {
        const bypassActions = _.filter(this.source.lineItemActions,
            (item: LineItemAction) => {
                if (this.isBypassExceptionType(item.exceptionType)) {
                    return item;
                }
            });

        return bypassActions.length > 0;
    }
}

class LineItemActionValidator implements Validator {
    constructor(private lineItemAction: LineItemAction, private shouldValidateExceptionType: boolean) { }

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
        // No validation
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
        if (this.shouldValidateExceptionType) {
            const exceptionTypeCtrl = group.controls['exceptionType'];
            const exceptionTypeRequired = Validators.required(exceptionTypeCtrl);
            if (exceptionTypeRequired) {
                exceptionTypeCtrl.setErrors(exceptionTypeRequired);
            }
        }
      
    }

    public quantityDifferentFromOriginal(value: number): boolean {
        return value != this.lineItemAction.quantity;
    }

    public isNewLineItemAction(): boolean {
        return (!this.lineItemAction.id || this.lineItemAction.id == 0);
    }
}