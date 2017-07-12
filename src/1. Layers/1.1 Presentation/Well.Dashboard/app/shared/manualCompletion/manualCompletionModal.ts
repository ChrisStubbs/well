import { Component, EventEmitter, Output, Input, ViewChild, ElementRef } from '@angular/core';
import { IObservableAlive } from '../IObservableAlive';
import { ManualCompletionService } from './manualCompletionService';
import { IJobIdResolutionStatus } from '../models/jobIdResolutionStatus';
import { IPatchSummary } from '../models/patchSummary';
import { IManualCompletionRequest } from './manualCompletionRequest';
import { ManualCompletionType } from './manualCompletionRequest';
import * as _ from 'lodash';

@Component({
    selector: 'manual-completion-Modal',
    templateUrl: 'app/shared/manualCompletion/manualCompletionModal.html',
    providers: [ManualCompletionService]
})

export class ManualCompletionModal implements IObservableAlive
{
    @Input() public jobIds: number[] = [];
    public manualCompletionType: ManualCompletionType = ManualCompletionType.CompleteAsClean;
    @Output() public onSubmitted = new EventEmitter<IJobIdResolutionStatus[]>();
    @ViewChild('btnClose') private btnClose: ElementRef;
    @ViewChild('showManualCompletionModal') private showModal: ElementRef;

    private summary: IPatchSummary = {} as IPatchSummary;
    public isAlive: boolean = true;

    constructor(private manualCompletionService: ManualCompletionService) { }

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
        const completionRequest: IManualCompletionRequest = {
            jobIds: this.jobIds,
            manualCompletionType: this.manualCompletionType
        };

        this.manualCompletionService.patch(completionRequest)
            .takeWhile(() => this.isAlive)
            .subscribe((res: IJobIdResolutionStatus[]) =>
            {
                this.onSubmitted.emit((res));
                this.closeModal();
            });
    }

    public show(manualCompletionType: ManualCompletionType): void
    {
        this.manualCompletionType = manualCompletionType;
        this.showModal.nativeElement.click();
        this.manualCompletionService.getSummary(this.jobIds)
            .takeWhile(() => this.isAlive)
            .subscribe(summaryData =>
            {
                this.summary = summaryData as IPatchSummary;
            });
    }

    private closeModal()
    {
        this.summary = {} as IPatchSummary;
        this.btnClose.nativeElement.click();
    }

    private hasItemsToSubmit(): boolean
    {
        return this.summary.noOfJobs > 0;
    }

    private manualCompletionTypeDescription(): string
    {
        switch (this.manualCompletionType)
        {
            case ManualCompletionType.CompleteAsBypassed:
                return 'Complete as Bypassed';
            default:
                return 'Complete as Clean';
        }
    }

}