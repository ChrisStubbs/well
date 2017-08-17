import { Component, EventEmitter, Output, Input} from '@angular/core';
import { JobService } from './jobService';

@Component({
    selector: 'assignGrn-Modal',
    templateUrl: 'app/job/assignGrnModal.html',
    providers: [JobService]
})
export class AssignGrnModal {
    @Input() public model: IGrnAssignable;
    @Output() public onGrnAssigned = new EventEmitter<IGrnAssignable>();
    private isVisible: boolean = false;
    public grnNumber: string;
    private notRequired: string = 'Not required';

    constructor(private jobService: JobService) {

    }

    public ngOnInit() {
        this.grnNumber = this.model.grnNumber;
    }

    public show = (): void => {
        this.isVisible = true;
    }

    private submit(): void
    {
        this.jobService.setGrnForJob(this.model.jobId, this.grnNumber)
            .subscribe(data =>
            {
                this.model.grnNumber = this.grnNumber;
                this.onGrnAssigned.emit(this.model);
                this.close();
            });
    }

    private close = (): void => {
        this.isVisible = false;
    }

    private linkText = (): string => {
        if (this.model.grnNumber) {
            return this.model.grnNumber;
        } else {
            return (GrnHelpers.isGrnRequired(this.model)) ? 'Required' : this.notRequired;
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