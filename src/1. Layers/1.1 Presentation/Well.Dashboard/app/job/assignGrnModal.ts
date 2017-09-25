import { Component, EventEmitter, Output, Input}    from '@angular/core';
import { JobService }                               from './jobService';
import {SecurityService}                            from '../shared/services/securityService';
import {IObservableAlive}                           from '../shared/IObservableAlive';

@Component({
    selector: 'assignGrn-Modal',
    templateUrl: 'app/job/assignGrnModal.html',
    providers: [JobService]
})
export class AssignGrnModal implements IObservableAlive
{
    public isAlive: boolean = true;
    public grnNumber: string;

    @Input() public model: IGrnAssignable;
    @Output() public onGrnAssigned = new EventEmitter<IGrnAssignable>();

    private notRequired: string = 'Not required';
    private required: string = 'Required';
    private canSubmitMissingGRN: boolean = false;
    private isVisible: boolean = false;

    constructor(private jobService: JobService, private securityService: SecurityService) {}

    public ngOnInit()
    {
        this.grnNumber = this.model.grnNumber;
        this.canSubmitMissingGRN = this.securityService.userHasPermission(SecurityService.submitMissingGRN);
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public show(): void
    {
        this.isVisible = this.canSubmitMissingGRN;
    }

    private submitGrn(): void
    {
        this.jobService.setGrnForJob(this.model.jobId, this.grnNumber)
            .subscribe(data => {
                this.model.grnNumber = this.grnNumber;
                this.onGrnAssigned.emit(this.model);
                this.close();
            });
    }

    private close(): void
    {
        this.isVisible = false;
    }

    private linkText (): string
    {
        if (this.model.grnNumber) {
            return this.model.grnNumber;
        } else {
            return (GrnHelpers.isGrnRequired(this.model)) ? this.required : this.notRequired;
        }
    }
}

export class GrnHelpers {
    public static isGrnRequired(item: IGrnAssignable) {
        return item.grnProcessType == 1;
    }
}

export interface IGrnAssignable {
    jobId: number;
    grnNumber: string;
    grnProcessType: number;
}