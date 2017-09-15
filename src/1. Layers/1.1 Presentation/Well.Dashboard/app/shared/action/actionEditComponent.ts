import
{
    Component, ViewChild, ElementRef, EventEmitter, Output, ViewEncapsulation  
}                                  from '@angular/core';
import
{
    AbstractControl, FormGroup, FormArray, FormBuilder, Validators, ValidationErrors, Validator
}
                                    from '@angular/forms';
import { IObservableAlive }         from '../IObservableAlive';
import * as _                       from 'lodash';
import { ILookupValue }             from '../services/ILookupValue';
import { LookupsEnum }              from '../services/lookupsEnum';
import { LookupService }            from '../services/lookupService';
import { EditExceptionsService }    from '../../exceptions/editExceptionsService';
import { Observable }               from 'rxjs';
import { LineItemAction }           from '../../exceptions/lineItemAction';
import { EditLineItemException }    from '../../exceptions/editLineItemException';
import { LineItemActionComment }    from '../../exceptions/lineItemAction';
import {SecurityService}            from '../services/securityService';

@Component({
    selector: 'action-edit',
    templateUrl: 'app/shared/action/actionEditComponent.html',
    providers: [LookupService, EditExceptionsService],
    styleUrls: ['app/shared/action/actionEditComponent.css'],
    encapsulation: ViewEncapsulation.None
})
export class ActionEditComponent implements IObservableAlive
{
    public isAlive: boolean = true;
    public source: EditLineItemException = new EditLineItemException();

    @Output() public onSave = new EventEmitter<EditLineItemException>();
    @ViewChild('showEditActionsModal') public showModal: ElementRef;
    @ViewChild('closeModal') public closeModal: ElementRef;

    private deliveryActions: Array<ILookupValue>;
    private deliveryActionsWithFilter: Array<ILookupValue> = undefined;
    private sources: Array<ILookupValue>;
    private reasons: Array<ILookupValue>;
    private exceptionTypes: Array<ILookupValue>;
    private commentReasons: Array<ILookupValue>;
    private lineItemActions: Array<LineItemAction> = [];
    private creditActionValue = 1;
    private podActionValue = 3;
    private actionsForm: FormGroup;
    private actionsGroup: FormArray;
    private canEditExceptions: boolean = false;

    constructor(
        private lookupService: LookupService,
        private securityService: SecurityService,
        private editExceptionsService: EditExceptionsService,
        private formBuilder: FormBuilder) { }

    public ngOnInit()
    {
        this.canEditExceptions = this.securityService.userHasPermission(SecurityService.editExceptions);
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
                });
        }
    }

    private getDeliveryActions(): Array<ILookupValue>
    {
        if (_.isUndefined(this.deliveryActionsWithFilter))
        {
            const self = this;
            if (this.source.isProofOfDelivery) {
                this.deliveryActionsWithFilter = _.filter(this.deliveryActions,
                    (action: ILookupValue) => {
                        return Number(action.key) != self.creditActionValue;
                    });
            } else {
                this.deliveryActionsWithFilter = _.filter(this.deliveryActions,
                    (action: ILookupValue) => {
                        return Number(action.key) != self.podActionValue;
                    });
            }
        }

        return this.deliveryActionsWithFilter;
    }

    private getDeliveryActionDescription(id: number): string
    {
        return _.find(this.getDeliveryActions(), (current: ILookupValue) => {
            return +current.key == id;
        }).value;
    }

    private getExceptionTypeDescription(id: number): string
    {
        const result = _.find(this.exceptionTypes, (current: ILookupValue) => {
            return +current.key == id;
        });

        if (_.isUndefined(result))
        {
            return 'Bypass';
        }

        return result.value;
    }

    private getSourceDescription(id: number): string
    {
        return _.find(this.sources, (current: ILookupValue) => {
            return +current.key == id;
        }).value;
    }

    private getReasonDescription(id: number): string
    {
        return _.find(this.reasons, (current: ILookupValue) => {
            return +current.key == id;
        }).value;
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public addAction(): void {
        const lineItemAction = new LineItemAction();
        this.lineItemActions.push(lineItemAction);
        const group = this.createLineItemActionFromGroup(lineItemAction);
        this.actionsGroup.push(group);
    }

    public removeItem(index: number): void
    {
        this.lineItemActions.splice(index, 1);
        this.actionsGroup.removeAt(index);
    }

    public show(editLineItemException: EditLineItemException): void
    {
        this.loadSource(editLineItemException);

        this.createLineItemActionsForm(editLineItemException);

        this.showModal.nativeElement.click();
    }

    public close(): void
    {
        this.closeModal.nativeElement.click();
    }

    public save(): void
    {
        if (!this.canEditExceptions)
        {
            return;
        }

        if (this.actionsForm.valid) {

            _.each(this.actionsForm.controls['actionsGroup'].value,
                (value: any, index: number) => {
                    const lineItemAction = this.lineItemActions[index];
                    // patch line item actions
                    lineItemAction.deliveryAction = Number(value.action);
                    lineItemAction.quantity = (value.quantity) ? Number(value.quantity) : 0;
                    lineItemAction.source = (value.source) ? Number(value.source) : 0;
                    lineItemAction.reason = (value.reason) ? Number(value.reason) : 0;

                    if (!LineItemActionValidator.isBypassExceptionType(lineItemAction.exceptionType)) {
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

    private loadSource(editLineItemException: EditLineItemException): void
    {
        this.source = editLineItemException;
        this.canEditExceptions = this.canEditExceptions && this.source.canEditActions;
        this.lineItemActions = this.source.lineItemActions || [];
    }

    private createLineItemActionsForm(editLineItemException: EditLineItemException)
    {
        const self = this;

        this.actionsGroup = this.formBuilder.array(_.map(editLineItemException.lineItemActions,
            function (item: LineItemAction) {

                return self.createLineItemActionFromGroup(item);
            }),
            (control) => this.validateOverallQuantity(control));

        this.actionsForm = this.formBuilder.group({
            actionsGroup: this.actionsGroup
        });
    }

    private createLineItemActionFromGroup(item: LineItemAction): FormGroup
    {
        const validator = new LineItemActionValidator(item);
        const isCloseAction = Number(item.deliveryAction) == LineItemActionValidator.closeActionValue;

        const action = this.formBuilder.control({
            value: item.deliveryAction,
            disabled: !this.source.canEditActions && !this.canEditExceptions
        },
            Validators.required);

        const quantity = this.formBuilder.control({
            value: item.quantity,
            disabled: (!this.source.canEditActions || isCloseAction)
        },
            [Validators.pattern('^[1-9]{1}[0-9]*$')]);

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

        const group = this.formBuilder.group({
            action: action,
            quantity: quantity,
            commentReason: commentReason,
            exceptionType: exceptionType,
            source: source,
            reason: reason
        });

        validator.applyToLineItemActionFrom(group);

        return group;
    }

    private shouldErrorsDivBeVisible(): boolean
    {

        const result: boolean = this.actionsForm.enabled
            && !this.actionsForm.valid
            && (
                this.actionsForm.controls.actionsGroup.errors
                && this.actionsForm.controls.actionsGroup.errors.length > 0
            );

        if (result == undefined) {
            return false;
        }
        return result;
    }

    private validateOverallQuantity(formArray: AbstractControl): ValidationErrors {
        const errors = new Array<ValidationErrors>();
        const sum = _.sumBy(formArray.value,
            item => {
                return item.quantity || 0;
            });

        if (sum > this.source.invoiced) {
            const totalError = {
                totalQuantity: true,
                message: 'Total quantity (' + sum + ') is greater than invoiced (' + this.source.invoiced + ')'
            };

            errors.push(totalError);
        }

        if (_.find(formArray.value, item => item.quantity < 0)) {
            const negativeQuantityError = {
                totalQuantity: true,
                message: 'Quantity field should not be a negative number'
            };

            errors.push(negativeQuantityError);
        }

        if (errors.length > 0) {
            return errors;
        } else {
            return undefined;
        }
    }

    private hasComments(item: LineItemAction): boolean {
        const existingComments = _.filter(item.comments, x => x.id !== 0);
        return existingComments.length > 0;
    }

    private isBypassExceptionType(item: LineItemAction): boolean {
        return LineItemActionValidator.isBypassExceptionType(item);
    }
}

// This class along with createLineItemActionFromGroup should probably be refactored into component
class LineItemActionValidator implements Validator {
    public static bypassValue = 2;
    public static closeActionValue = 2;
    private isBypassExceptionType: boolean;

    constructor(private lineItemAction: LineItemAction) {
        this.isBypassExceptionType = LineItemActionValidator.isBypassExceptionType(lineItemAction.exceptionType);
    }

    public validate(group: FormGroup): ValidationErrors {
        const actionValue = group.value.action;

        switch (Number(actionValue)) {
            case 0:
                return this.validateAction(group);
            case 1:
                return this.validateAction(group);
            case 2:
                return this.validateCloseAction(group);
            case 3:
                return this.validatePodAction(group);
            default:
                throw new Error('Unknown action type : ' + actionValue);
        }
    }

    private validateAction(group: FormGroup): ValidationErrors {
        this.validateQuantity(group);
        this.validateComment(group);
        this.validateExceptionType(group);
        
        // Source required
        const sourceCtrl = group.controls['source'];
        if (Number(sourceCtrl.value | 0 ) == 0) {
            sourceCtrl.setErrors({ required: true });
        }

        // Reason required
        const reasonCtrl = group.controls['reason'];
        if (Number(reasonCtrl.value | 0 ) == 0) {
            reasonCtrl.setErrors({ required: true });
        }

        return undefined;
    }

    private validateCloseAction(group: FormGroup): ValidationErrors {
        // No validation
        return undefined;
    }

    private validatePodAction(group: FormGroup): ValidationErrors {
        this.validateQuantity(group);
        this.validateComment(group);
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
        if (!this.isBypassExceptionType) {
            // Exception required
            const exceptionTypeCtrl = group.controls['exceptionType'];
            if (Number(exceptionTypeCtrl.value | 0) == 0) {
                exceptionTypeCtrl.setErrors({ required: true });
            }
        }
    }

    public quantityDifferentFromOriginal(value: number): boolean {
        return value != this.lineItemAction.quantity;
    }

    public isNewLineItemAction(): boolean {
        return (!this.lineItemAction.id || this.lineItemAction.id == 0);
    }

    public applyToLineItemActionFrom(group: FormGroup) {
        group.setValidators((control: FormGroup) => this.validate(control));

        const action = group.controls['action'];
        const quantity = group.controls['quantity'];
        const commentReason = group.controls['commentReason'];
        const exceptionType = group.controls['exceptionType'];
        const source = group.controls['source'];
        const reason = group.controls['reason'];

        action.valueChanges.subscribe((value) => {
            // When status is close - disable other fields
            const actionValue = Number(value);
            if (actionValue == LineItemActionValidator.closeActionValue) {
                quantity.disable();
                exceptionType.disable();
                source.disable();
                reason.disable();
                //Set default values
                quantity.setValue(undefined);
                source.setValue(undefined);
                reason.setValue(undefined);

                if (!this.isBypassExceptionType) {
                    exceptionType.setValue(undefined);
                }

            } else {
                quantity.enable();
                source.enable();
                reason.enable();

                if (!this.isBypassExceptionType) {
                    exceptionType.enable();
                }
            }
        });

        quantity.valueChanges.subscribe((value) => {
            if (quantity.disabled) {
                commentReason.disable();
                return;
            }

            if ((!this.isNewLineItemAction() && this.quantityDifferentFromOriginal(value))) {
                commentReason.enable();
            } else {
                commentReason.setValue(undefined);
                commentReason.disable();
            }
        });
    }

    public static isBypassExceptionType(value: any): boolean {
        return Number(value) == this.bypassValue;
    }
}