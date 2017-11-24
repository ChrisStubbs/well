import { Component }                            from '@angular/core';
import { FormGroup, FormBuilder, Validators }   from '@angular/forms';
import { SecurityService }                      from '../shared/services/securityService';
import { BranchDateThreshold }                  from './branchDateThreshold';
import { BranchDateThresholdService }           from './branchDateThresholdService';
import * as _                                   from 'lodash';
import {IObservableAlive}                       from '../shared/IObservableAlive';

@Component({
    selector: 'ow-branch-date-threshold',
    templateUrl: './app/branch-role/branchDateThresholdComponent.html'
})
export class BranchDateThresholdComponent implements IObservableAlive
{
    public isAlive: boolean = true;

    private branchThresholdsForm: FormGroup;

    constructor(
        private securityService: SecurityService,
        private branchDateThresholdService: BranchDateThresholdService,
        private formBuilder: FormBuilder) { }

    public ngOnInit(): void
    {
        const self = this;

        this.securityService.validateAccess(SecurityService.adminPages);

        const branchThresholdsArray = this.formBuilder.array([]);
        this.branchThresholdsForm = this.formBuilder.group({
            branchThresholdsArray: branchThresholdsArray
        });

        this.branchDateThresholdService.getBranchDateThresholds()
            .takeWhile(() => this.isAlive)
            .subscribe((values) => {
                _.each(values,
                    (branchThreshold: BranchDateThreshold) => {
                        const branchThresholdFormGroup = self.formBuilder.group({
                            branchId: self.formBuilder.control(branchThreshold.branchId),
                            numberOfDays: self.formBuilder.control(branchThreshold.numberOfDays,
                                [Validators.required, Validators.pattern('[2-9]')])
                        });

                        branchThresholdsArray.push(branchThresholdFormGroup);
                    });
            });
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public onSubmit(): void {      
        if (this.branchThresholdsForm.valid) {
            const data = this.branchThresholdsForm.value.branchThresholdsArray;
            this.branchDateThresholdService.updateBranchDateThresholds(data).subscribe();
        }
    }
}
