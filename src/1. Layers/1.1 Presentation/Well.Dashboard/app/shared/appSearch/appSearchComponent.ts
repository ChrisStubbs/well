import { Component, Output, EventEmitter }  from '@angular/core';
import { Router }                           from '@angular/router';
import { BranchService }                    from '../branch/branchService';
import { GlobalSettingsService }            from '../globalSettings';
import {
    FormGroup,
    FormControl,
    FormBuilder,
    Validators
}                                           from '@angular/forms';
import {
    LookupService,
    LookupsEnum,
    ILookupValue
}                                           from '../services/services';
import { IObservableAlive }                 from '../IObservableAlive';
import { Observable }                       from 'rxjs';
import { ToasterService }                   from 'angular2-toaster/angular2-toaster';
import * as _                               from 'lodash';
import {
    IAppSearchResult,
    IAppSearchItem,
    AppSearchItemType,
    AppSearchParameters,
    AppSearchInvoiceItem,
    AppSearchLocationItem,
    AppSearchRouteItem
} from './appSearch';
import { AppSearchService }                 from './appSearchService';
import 'rxjs/add/operator/takeWhile';
import 'rxjs/add/observable/forkJoin';

@Component({
    selector: 'ow-appSearch',
    templateUrl: 'app/shared/appSearch/appSearchComponent.html',
    providers: [AppSearchService]
})
export class AppSearch implements IObservableAlive {
    public branches: Array<[string, string]>;
    public jobStatus: ILookupValue[];
    public jobTypes: ILookupValue[];
    public searchForm: FormGroup;
    public showMoreFilters = false;
    public drivers: ILookupValue[];
    public isAlive: boolean = true;
    @Output() public onSearch: EventEmitter<IAppSearchResult> = new EventEmitter<IAppSearchResult>();
    private currentResult: IAppSearchResult;
    private appSearchItemType;

    public constructor(
        private lookupService: LookupService,
        private branchService: BranchService,
        private globalSettingsService: GlobalSettingsService,
        private fb: FormBuilder,
        private appSearchService: AppSearchService,
        private router: Router,
        private toasterService: ToasterService) {

        // Instance of enum
        this.appSearchItemType = AppSearchItemType;
    }

    public ngOnInit(): void {
        this.searchForm = this.fb.group(
            {
                'branch': new FormControl('', Validators.required),
                'date': new FormControl(),
                'account': new FormControl(),
                'invoice': new FormControl(),
                'route': new FormControl(),
                'driver': new FormControl(),
                'deliveryType': new FormControl(),
                'status': new FormControl(),
                'upliftInvoiceNumber': new FormControl()
            });

        this.fillBranches();

        this.branchService.userBranchesChanged$
            .takeWhile(() => this.isAlive)
            .subscribe(b => this.fillBranches());

        Observable.forkJoin(
            this.lookupService.get(LookupsEnum.JobType),
            this.lookupService.get(LookupsEnum.WellStatus),
            this.lookupService.get(LookupsEnum.Driver)
        )
            .takeWhile(() => this.isAlive)
            .subscribe(res => {
                this.jobTypes = res[0];
                this.jobStatus = res[1];
                this.drivers = res[2];
            });
    }

    private setDefaultBranch(): void {
        if (_.isNil(this.branches)) {
            this.searchForm.value.branch = undefined;
            return;
        }

        if (this.branches.length == 1) {
            this.searchForm.value.branch = this.branches[0] ? this.branches[0][0] : undefined;
        }
    }

    private fillBranches(): void {
        this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName)
            .takeWhile(() => this.isAlive)
            .subscribe(branches => {
                this.branches = branches;
                this.setDefaultBranch();
            });
    }

    public ngOnDestroy(): void {
        this.isAlive = false;
    }

    public resetSearch(): void {
        this.searchForm.reset();
        this.setDefaultBranch();
        this.currentResult = undefined;
    }

    public search(event: any): void {
        event.stopPropagation();

        if (this.isEmptySearch()) {
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
        parameters.upliftInvoiceNumber = formData.upliftInvoiceNumber;

        this.globalSettingsService.globalSettings.currentBranchId = parameters.branchId;

        this.appSearchService.Search(parameters)
            .takeWhile(() => this.isAlive)
            .subscribe((result: IAppSearchResult) => {
                // If no locations matched
                if (result.items.length === 0) {
                    this.toasterService.pop('warning', 'No results found for your search criteria');
                    this.onSearch.emit();
                    return;
                }

                // Single result found so can navigate
                if (result.items.length === 1) {
                    this.navigateToItem(result.items[0]);
                    return;
                }

                if (this.isNonFilterSearch(parameters)) {
                    const routeIds = _.map(_.filter(result.items, { itemType: 3 }), (item) => { return item.id; });
                    parameters.routeIds = (routeIds.length > 0) ? routeIds : [-1];
                }

                //this.router.navigate(['/routes'], { queryParams: parameters });
                this.currentResult = result;
                this.onSearch.emit(result);
                return;
            });
    }

    public isNonFilterSearch(searchParams: AppSearchParameters): boolean {
        if (searchParams.account || searchParams.invoice /* || searchParams.deliveryType */) {
            return true;
        }
        return false;
    }

    public isEmptySearch(): boolean {
        return !this.searchForm.valid;
    }

    public showMore(): void {
        this.showMoreFilters = !this.showMoreFilters;
    }

    private navigateToItem(item: IAppSearchItem) {
        switch (item.itemType) {
            case AppSearchItemType.Invoice:
                this.navigateToInvoice(<AppSearchInvoiceItem>item);
                return;

            case AppSearchItemType.Location:
                this.navigateToLocation(<AppSearchLocationItem>item);
                return;
                
            case AppSearchItemType.Route:
                this.navigateToSingleRoute(<AppSearchRouteItem>item);
                return;
        }
    }

    private navigateToInvoice(item: AppSearchInvoiceItem) {
        this.router.navigateByUrl('/invoice/' + item.id);
    }

    private navigateToLocation(item: AppSearchLocationItem) {
        this.router.navigate(['/singlelocation'], { queryParams: {locationId: item.id}});
    }

    private navigateToSingleRoute(item: AppSearchRouteItem) {
        this.router.navigateByUrl('/singleroute/' + item.id);
    }
}