import { Component, Output, EventEmitter }              from '@angular/core';
import { Router }                                       from '@angular/router';
import { BranchService }                                from '../branch/branchService';
import { GlobalSettingsService }                        from '../globalSettings';
import { FormGroup, FormControl, FormBuilder }          from '@angular/forms';
import { DriverService }                                from '../../driver/driverService';
import { IAppSearchResult }                             from './iAppSearchResult';
import { AppSearchParameters }                          from './appSearchParameters';
import { AppSearchService }                             from './appSearchService'
import * as _                                           from 'lodash';
import { LookupService, LookupsEnum, ILookupValue}      from '../services/services';
import { IObservableAlive }                             from '../IObservableAlive';
import { Observable }                                   from 'rxjs';
import 'rxjs/add/operator/takeWhile';
import 'rxjs/add/observable/forkJoin'

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
        private router: Router) {}

    public ngOnInit(): void
    {
        this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName)
            .takeWhile(() => this.isAlive)
            .subscribe(branches =>
            {
                this.branches = branches;
            });

        Observable.forkJoin(
            this.lookupService.get(LookupsEnum.JobType),
            this.lookupService.get(LookupsEnum.JobStatus),
            this.lookupService.get(LookupsEnum.Driver)
        )
            .takeWhile(() => this.isAlive)
            .subscribe(res =>
            {
                this.jobTypes = res[0];
                this.jobStatus = res[1];
                this.drivers = res[2];
            });

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
    }

    public ngOnDestroy(): void
    {
        this.isAlive = false;
    }

    public resetSearch(): void
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
            .takeWhile(() => this.isAlive)
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