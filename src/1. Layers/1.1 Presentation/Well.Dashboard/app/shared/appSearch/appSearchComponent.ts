import { Component, OnDestroy, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { BranchService } from '../branch/branchService';
import { GlobalSettingsService } from '../globalSettings';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
import { JobService, JobStatus, JobType } from '../../job/job';
import { DriverService } from '../../driver/driverService';
import { IAppSearchResult } from './iAppSearchResult';
import { AppSearchParameters } from './appSearchParameters';
import { AppSearchService } from './appSearchService'
import * as _ from 'lodash';
import 'rxjs/add/operator/takeWhile';

//http://stackoverflow.com/questions/32896407/redirect-within-component-angular-2
/* tslint:disable:max-line-length */
//https://www.google.co.uk/search?num=20&newwindow=1&espv=2&q=angular+2+redirect+to+route&oq=angular+2+redirect+&gs_l=serp.3.1.0l10.26462.33032.0.35808.15.15.0.0.0.0.170.1167.14j1.15.0....0...1.1.64.serp..0.15.1161...0i67k1j0i131k1j35i39k1j0i20k1j0i10k1.lLydsRjGk6M
//https://angular.io/resources/live-examples/router/ts/eplnkr.html
@Component({
    selector: 'ow-appSearch',
    templateUrl: 'app/shared/appSearch/appSearchView.html',
    providers: [JobService, BranchService, GlobalSettingsService, DriverService, AppSearchService]
})
export class AppSearch implements OnDestroy
{
    public branches: Array<[string, string]>;
    public jobStatus: JobStatus[];
    public jobTypes: JobType[];
    public searchForm: FormGroup;
    public showMoreFilters = false;
    public drivers: string[];
    @Output() public onSearch = new EventEmitter();

    private alive: boolean = true;

    public constructor(
        private jobService: JobService,
        private branchService: BranchService,
        private globalSettingsService: GlobalSettingsService,
        private fb: FormBuilder,
        private driverService: DriverService,
        private appSearchService: AppSearchService,
        private router: Router)
    {
        this.driverService.drivers()
            .takeWhile(() => this.alive)
            .subscribe(d =>
            {
                this.drivers = d;
                this.drivers.unshift('All');

            });

        this.branchService.getBranchesValueList(globalSettingsService.globalSettings.userName)
            .takeWhile(() => this.alive)
            .subscribe(branches =>
            {
                this.branches = branches;
            });

        this.jobService.JobStatus()
            .takeWhile(() => this.alive)
            .subscribe(status =>
            {
                const emptyState = new JobStatus();

                this.jobStatus = status;
                emptyState.description = 'All';
                emptyState.id = undefined;
                this.jobStatus.unshift(emptyState);
            });

        this.jobService.JobTypes()
            .takeWhile(() => this.alive)
            .subscribe(types =>
            {
                const emptyType = new JobType();

                this.jobTypes = types;
                emptyType.description = 'All';
                emptyType.id = undefined;
                this.jobTypes.unshift(emptyType);
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

    public resetSearch()
    {
        this.searchForm.reset();
    }

    public search(event: any): void
    {
        event.stopPropagation();

        if (this.isEmptySearch())
        {
            return;
        }
        const formData = this.searchForm.value;
        const parameters = new AppSearchParameters();

        parameters.branchId = formData.branch;
        parameters.date = formData.date;
        parameters.account = formData.account;
        parameters.invoice = formData.invoice;
        parameters.route = formData.route;
        parameters.driver = formData.driver;
        parameters.deliveryType = formData.deliveryType;
        parameters.status = formData.status;

        this.appSearchService.Search(parameters)
            .takeWhile(() => this.alive)
            .subscribe((result: IAppSearchResult) =>
            {
                if (!_.isNil(result.stopId))
                {
                    this.router.navigateByUrl('/stops/' + result.stopId );
                    return;
                }

                if (!_.isNil(result.routeId))
                {
                    this.router.navigateByUrl('/singleroute/' + result.routeId );
                    return;
                }

                this.router.navigate(['/routes'], { queryParams: parameters });
                this.onSearch.emit();
                return;
            });
    }

    public isEmptySearch(): boolean
    {
        const formData = this.searchForm.value;
        let result: boolean = true;

        if (!_.isNil(formData.branch))
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