import { Component, EventEmitter, Output, Input, ViewChild, ElementRef } from '@angular/core';
import { IObservableAlive } from '../IObservableAlive';
import { ActionService } from './actionService';
import { IActionSubmitSummary, IActionSubmitSummaryItem } from './actionSubmitSummary';
import { ISubmitActionModel, ISubmitActionResult } from './submitActionModel';
import { ToasterService } from 'angular2-toaster';
import * as _ from 'lodash';
import { ILookupValue } from '../services/ILookupValue';
import { LookupsEnum } from '../services/lookupsEnum';
import { LookupService } from '../services/lookupService';

@Component({
    selector: 'submit-action-Modal',
    templateUrl: 'app/shared/action/submitActionModal.html',
    providers: [ActionService, LookupService]
})

export class SubmitActionModal implements IObservableAlive
{
    @Input() public disabled: boolean = false;
    @Input() public isStopLevel: boolean = false;
    @Output() public onActionClicked: EventEmitter<ILookupValue> = new EventEmitter<ILookupValue>();
    @Output() public onSubmitted = new EventEmitter<ISubmitActionResult>();
    @Input() public jobIds: number[] = [];
    @ViewChild('btnClose') private btnClose: ElementRef;

    private summaryData: IActionSubmitSummary = {} as IActionSubmitSummary;
    public isAlive: boolean = true;

    constructor(
        private lookupService: LookupService,
        private actionService: ActionService,
        private toasterService: ToasterService) { }

    public ngOnInit()
    {
       //
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    private submit()
    {
        const submitAction: ISubmitActionModel = {
            jobIds: this.summaryData.jobIds
        };

        this.actionService.post(submitAction)
            .takeWhile(() => this.isAlive)
            .subscribe((res: ISubmitActionResult) => this.handleResponse(res));
    }

    private handleResponse(res: ISubmitActionResult): void
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
        this.onSubmitted.emit(res);
        this.closeModal();
    }

    private closeModal()
    {
        this.summaryData = {} as IActionSubmitSummary;
        this.btnClose.nativeElement.click();
    }

    public actionClicked(action: ILookupValue): void
    {
        this.actionService.getPreSubmitSummary(this.jobIds, this.isStopLevel)
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

    public getIdentifierText(summaryItem: IActionSubmitSummaryItem): string {
        let type: string;
        if (this.isStopLevel) {
            type = 'Stop';
        } else {
            type = _.includes(summaryItem.jobType.toLowerCase(), 'uplift') ? 'Uplift' : 'Invoice';
        }
       
        return type + ' ' + summaryItem.identifier;
    }
}