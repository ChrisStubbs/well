import { Component, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { BranchService } from '../branch/branchService';
import { GlobalSettingsService } from '../globalSettings';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
import { DriverService } from '../../driver/driverService';
import { IAppSearchResultSummary } from './iAppSearchResultSummary';
import { AppSearchParameters } from './appSearchParameters';
import { AppSearchService } from './appSearchService'
import * as _ from 'lodash';
import { LookupService, LookupsEnum, ILookupValue } from '../services/services';
import { IObservableAlive } from '../IObservableAlive';
import { Observable } from 'rxjs';
import 'rxjs/add/operator/takeWhile';
import 'rxjs/add/observable/forkJoin';

@Component({
    selector: 'ow-appSearch',
    templateUrl: 'app/shared/appSearch/appSearchComponent.html',
    providers: [BranchService, DriverService, AppSearchService]
})
export class AppSearch implements IObservableAlive
{
    public branches: Array<[string, string]>;
    public jobStatus: ILookupValue[];
    public jobTypes: ILookupValue[];
    public searchForm: FormGroup;
    public showMoreFilters = false;
    public drivers: ILookupValue[];
    public isAlive: boolean = true;
    @Output() public onSearch = new EventEmitter();

    public constructor(
        private lookupService: LookupService,
        private branchService: BranchService,
        private globalSettingsService: GlobalSettingsService,
        private fb: FormBuilder,
        private appSearchService: AppSearchService,
        private router: Router) { }

    public ngOnInit(): void
    {
        this.searchForm = this.fb.group(
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

        this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName)
            .takeWhile(() => this.isAlive)
            .subscribe(branches =>
            {
                this.branches = <any>branches;
                this.setDefaultBranch();
            });

        Observable.forkJoin(
            this.lookupService.get(LookupsEnum.JobType),
            //this.lookupService.get(LookupsEnum.JobStatus),
            this.lookupService.get(LookupsEnum.RouteStatus),
            this.lookupService.get(LookupsEnum.WellStatus),
            this.lookupService.get(LookupsEnum.Driver)
        )
            .takeWhile(() => this.isAlive)
            .subscribe(res =>
            {
                this.jobTypes = res[0];
                this.jobStatus = res[1];
                this.jobStatus = res[2];
                this.drivers = res[3];
            });
    }

    private setDefaultBranch()
    {
        if (_.isNil(this.branches))
        {
            this.searchForm.value.branch = undefined;
            return;
        }

        this.searchForm.value.branch = this.branches[0] ? this.branches[0][0] : undefined;
    }
    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public resetSearch(): void
    {
        this.searchForm.reset();
        this.setDefaultBranch();
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
        parameters.routeIds = [];

        this.appSearchService.Search(parameters)
            .takeWhile(() => this.isAlive)
            .subscribe((result: IAppSearchResultSummary) =>
            {
                if (result.stopIds.length === 1)
                {
                    this.router.navigateByUrl('/stops/' + result.stopIds[0]);
                    return;
                }

                if (result.routeIds.length === 1)
                {
                    this.router.navigateByUrl('/singleroute/' + result.routeIds[0]);
                    return;
                }

                if (this.isNonFilterSearch(parameters))
                {
                    parameters.routeIds = (result.routeIds.length > 0) ? result.routeIds : [-1];
                }
                this.router.navigate(['/routes'], { queryParams: parameters });
                this.onSearch.emit();

                return;

            });
    }

    public isNonFilterSearch(searchParams: AppSearchParameters): boolean
    {
        if (searchParams.account || searchParams.invoice || searchParams.deliveryType)
        {
            return true;
        }
        return false;
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