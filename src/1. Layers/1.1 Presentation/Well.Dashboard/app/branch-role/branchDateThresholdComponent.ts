import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';
import { SecurityService } from '../shared/security/securityService';
import { GlobalSettingsService } from '../shared/globalSettings';
import { BranchDateThreshold } from './branchDateThreshold';
import { BranchDateThresholdService } from './branchDateThresholdService';
import * as _ from 'lodash';

@Component({
    selector: 'ow-branch-date-threshold',
    templateUrl: './app/branch-role/branchDateThresholdComponent.html'
})
export class BranchDateThresholdComponent implements OnInit {
    private branchThresholds: Array<BranchDateThreshold> = [];
    private branchThresholdsForm: FormGroup;
    constructor(
        private securityService: SecurityService,
        private globalSettingsService: GlobalSettingsService,
        private branchDateThresholdService: BranchDateThresholdService,
        private formBuilder: FormBuilder) { }

    public ngOnInit(): void {
        this.securityService.validateUser(
            this.globalSettingsService.globalSettings.permissions,
            this.securityService.branchSelection);

        const self = this;
        const branchThresholdsArray = this.formBuilder.array([]);
        this.branchThresholdsForm = this.formBuilder.group({
            branchThresholdsArray: branchThresholdsArray
        });

        this.branchDateThresholdService.getBranchDateThresholds().subscribe((values) => {
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

    public onSubmit(): void {      
        if (this.branchThresholdsForm.valid) {
            const data = this.branchThresholdsForm.value.branchThresholdsArray;
            this.branchDateThresholdService.updateBranchDateThresholds(data).subscribe();
        }
    }
}
