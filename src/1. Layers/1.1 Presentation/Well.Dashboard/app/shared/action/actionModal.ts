import { Component, EventEmitter, Output, Input, ViewChild, ElementRef } from '@angular/core';
import { IObservableAlive } from '../IObservableAlive';
import { ActionService } from './actionService';
import { IActionSubmitSummary } from './actionSubmitSummary';
import { ISubmitActionModel, ISubmitActionResult } from './submitActionModel';
import { ToasterService } from 'angular2-toaster';
import * as _ from 'lodash';
import { ILookupValue } from '../services/ILookupValue';
import { LookupsEnum } from '../services/lookupsEnum';
import { LookupService } from '../services/lookupService';
import { ISubmitActionResult as ISubmitActionResult1 } from './submitActionModel';

@Component({
    selector: 'action-Modal',
    templateUrl: 'app/shared/action/actionModal.html',
    providers: [ActionService, LookupService]
})
export class ActionModal implements IObservableAlive
{
    @Input() public disabled: boolean = false;
    @Input() public isStopLevel: boolean = false;
    @Output() public onActionClicked: EventEmitter<ILookupValue> = new EventEmitter<ILookupValue>();
    @Input() public jobIds: number[] = [];
    @ViewChild('btnClose') private btnClose: ElementRef;

    private summaryData: IActionSubmitSummary = {} as IActionSubmitSummary;
    public isAlive: boolean = true;
    private deliveryActions: Array<ILookupValue>;
    private defaultAction: ILookupValue = { key: '0', value: 'Action' };
    private selectedAction: ILookupValue = this.defaultAction;

    constructor(
        private lookupService: LookupService,
        private actionService: ActionService,
        private toasterService: ToasterService)
    {

    }

    public ngOnInit()
    {
        this.lookupService.get(LookupsEnum.DeliveryAction)
            .takeWhile(() => this.isAlive)
            .subscribe(res => {
                this.deliveryActions = _.filter(res, x => +x.key !== 0);
            });
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    private submit()
    {
        const submitAction: ISubmitActionModel = {
            action: this.selectedAction.key,
            jobIds: this.summaryData.jobIds
        };

        this.actionService.post(submitAction)
            .takeWhile(() => this.isAlive)
            .subscribe((res: ISubmitActionResult) => this.handleResponse(res));
    }

    private handleResponse(res: ISubmitActionResult1): void
    {
        if (res.isValid)
        {
            const warningMessages = _.map(res.warnings).join(', ');

            if (warningMessages)
            {
                this.toasterService.pop('warning', res.message, warningMessages);
            } else
            {
                this.toasterService.pop('success', res.message, '');
            }

        } else
        {
            this.toasterService.pop('error', res.message, '');
        }

        this.closeModal();

    }

    private closeModal()
    {
        this.selectedAction = this.defaultAction;
        this.summaryData = {} as IActionSubmitSummary;
        this.btnClose.nativeElement.click();
    }

    public actionClicked(action: ILookupValue): void
    {

        this.selectedAction = action;

        this.actionService.getPreSubmitSummary(this.jobIds, +this.selectedAction.key, this.isStopLevel)
            .takeWhile(() => this.isAlive)
            .subscribe(summaryData =>
            {
                this.summaryData = summaryData as IActionSubmitSummary;
            });

        this.onActionClicked.emit(action);
    }

    private hasItemsToSubmit(): boolean
    {
        if (!this.summaryData.jobIds) {
             return false;
        }

        return this.summaryData.jobIds.length > 0;
    }
}