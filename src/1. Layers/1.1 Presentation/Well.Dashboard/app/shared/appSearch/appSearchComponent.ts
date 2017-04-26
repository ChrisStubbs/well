import { Component }                            from '@angular/core';
import { BranchService }                        from '../branch/branchService';
import { Branch }                               from '../branch/branch';
import { GlobalSettingsService }                from '../globalSettings';
import { FormGroup, FormControl, FormBuilder }  from '@angular/forms';
import { JobService, JobStatus, JobType }       from '../../job/job';
import 'rxjs/add/operator/takeWhile';
import * as _ from 'lodash';

//http://stackoverflow.com/questions/32896407/redirect-within-component-angular-2
/* tslint:disable:max-line-length */
//https://www.google.co.uk/search?num=20&newwindow=1&espv=2&q=angular+2+redirect+to+route&oq=angular+2+redirect+&gs_l=serp.3.1.0l10.26462.33032.0.35808.15.15.0.0.0.0.170.1167.14j1.15.0....0...1.1.64.serp..0.15.1161...0i67k1j0i131k1j35i39k1j0i20k1j0i10k1.lLydsRjGk6M
@Component({
    selector: 'ow-appSearch',
    templateUrl: 'app/shared/appSearch/appSearchView.html'
})
export class AppSearch {
    public branches: Branch[];
    public jobStatus: JobStatus[];
    public jobTypes: JobType[];
    public searchForm: FormGroup;
    public showMoreFilters = false;

    private alive: boolean = true;

    public constructor(
        private jobService: JobService,
        private branchService: BranchService,
        private globalSettingsService: GlobalSettingsService,
        private fb: FormBuilder)
    {
        this.branchService.getBranches(globalSettingsService.globalSettings.userName)
            .takeWhile(() => this.alive)
            .subscribe(branches =>
            {
                const emptyBranch = new Branch();

                this.branches = branches;
                emptyBranch.name = 'All';
                emptyBranch.id = undefined;
                this.branches.unshift(emptyBranch)
            });

        this.jobService.JobStatus()
            .takeWhile(() => this.alive)
            .subscribe(status =>
            {
                const emptyState = new JobStatus();

                this.jobStatus = status;
                emptyState.description = 'All';
                emptyState.id = undefined;
                this.jobStatus.unshift(emptyState)
            });

        this.jobService.JobTypes()
            .takeWhile(() => this.alive)
            .subscribe(types =>
            {
                const emptyType = new JobType();

                this.jobTypes = types;
                emptyType.description = 'All';
                emptyType.id = undefined;
                this.jobTypes.unshift(emptyType)
            });

        this.searchForm = fb.group(
            {
                'branch': new FormControl(),
                'date': new FormControl(),
                'account': new FormControl(),
                'invoice': new FormControl(),
                'route': new FormControl(),
                'driver': new FormControl(),
                'deliveryType': new FormControl(),
                'status': new FormControl()
            });
    }
    public ngOnDestroy()
    {
        this.alive = false;
    }

    public search(event: any): void
    {
        if (this.isEmptySearch())
        {
            return;
        }

        console.log(event);
    }

    public isEmptySearch(): boolean
    {
        const formData = this.searchForm.value;
        let result: boolean = true;

        if (!_.isNil(formData.branch) && formData.branch != 'undefined')
        {
            result = false;
        }

        if (!_.isNil(formData.date))
        {
            result = false;
        }

        if (!_.isNil(formData.account))
        {
            result = false;
        }

        if (!_.isNil(formData.invoice))
        {
            result = false;
        }

        if (!_.isNil(formData.route))
        {
            result = false;
        }

        if (!_.isNil(formData.driver))
        {
            result = false;
        }

        if (!_.isNil(formData.deliveryType) && formData.deliveryType != 'undefined')
        {
            result = false;
        }

        if (!_.isNil(formData.status) && formData.status != 'undefined')
        {
            result = false;
        }

        return result;
    }

    public showMore(): void
    {
        this.showMoreFilters = !this.showMoreFilters;
    }
}