import { ActivatedRoute } from '@angular/router';
import { Component, OnDestroy, OnInit, ViewChildren, QueryList } from '@angular/core';
import { NavigateQueryParametersService } from '../shared/NavigateQueryParametersService';
import { BaseComponent } from '../shared/BaseComponent';
import { GlobalSettingsService } from '../shared/globalSettings';
import { Route, RouteFilter, RoutesService } from './routes';
import { RefreshService } from '../shared/refreshService';
import { SecurityService } from '../shared/security/securityService';
import { BranchService } from '../shared/branch/branchService';
import { Branch } from '../shared/branch/branch';
import { JobService, JobStatus, JobType } from '../job/job';
import { AppSearchParameters } from '../shared/appSearch/appSearch';
import { DataTable } from 'primeng/primeng';
import * as _ from 'lodash';
import 'rxjs/Rx';

@Component({
    selector: 'ow-route',
    templateUrl: './app/routes/route-list.html',
    providers: [RoutesService]
})
export class RoutesComponent extends BaseComponent implements OnInit, OnDestroy
{
    public isLoading: boolean = true;
    public refreshSubscription: any;
    public errorMessage: string;
    public routes: Route[];
    public lastRefresh = Date.now();
    public isReadOnlyUser: boolean = false;
    public branches: Array<[string, string]>;
    public jobStatus: Array<[string, string]>;
    public selectedRoutes: Route[];

    private alive: boolean = true;
    private actions: string[] = ['Assign'];
    private rowsPerPageOptions: number[] = [10, 20, 30, 40];
    private appSearchParams: AppSearchParameters = new AppSearchParameters();
    private selectedBranch: string = '';
    private selectedStatus: string = '';
    private selectedExceptionFilterItem: string = '';
    private selectedRouteDate?: Date;
    
    private exceptionFilterItems: Array<[string, string]> = [['', 'All'], ['true', 'Yes'], ['false', 'No']];
    @ViewChildren('dt') public dataTable: QueryList<DataTable>;

    constructor(
        protected globalSettingsService: GlobalSettingsService,
        private routeService: RoutesService,
        private refreshService: RefreshService,
        private activatedRoute: ActivatedRoute,
        protected securityService: SecurityService,
        private nqps: NavigateQueryParametersService,
        private branchService: BranchService,
        private jobService: JobService)
    {
        super(nqps, globalSettingsService, securityService);
    }

    public ngOnInit()
    {
        super.ngOnInit();

        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getRoutes());
        this.activatedRoute.queryParams.subscribe(params =>
        {
            this.appSearchParams = <AppSearchParameters>params;
            this.getRoutes();
        });

        this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName)
            .takeWhile(() => this.alive)
            .subscribe(
            (branches: Array<[string, string]>) =>
            {
                this.branches = branches;
                if (this.appSearchParams.branchId) {
                    this.selectedBranch = this.appSearchParams.branchId.toString();
                }
            });

        this.jobService.getStatusValueList()
            .takeWhile(() => this.alive)
            .subscribe((jobStatus: Array<[string, string]>) => {
                this.jobStatus = jobStatus;
                if (this.appSearchParams.status) {
                    this.selectedStatus = this.appSearchParams.status.toString();
                }
            });

        this.selectedRouteDate = this.appSearchParams.date;
    }

    public ngOnDestroy()
    {
        super.ngOnDestroy();
        this.alive = false;
        this.refreshSubscription.unsubscribe();
    }

    public getRoutes(): void
    {
        this.routeService.getRoutes()
            .takeWhile(() => this.alive)
            .subscribe((result: Route[]) =>
            {
                this.routes = result;
                this.lastRefresh = Date.now();
                this.isLoading = false;
                this.dataTable.first.filters = <any>RouteFilter.toRouteFilter(this.appSearchParams);
            },
            error =>
            {
                this.lastRefresh = Date.now();
                this.isLoading = false;
            });
    }

    public clearFilters(): void
    {
        this.selectedBranch = '';
        this.selectedStatus = '';
        this.selectedRouteDate = undefined;
        this.selectedExceptionFilterItem = '';
        this.dataTable.first.filters = <any>new RouteFilter();
        this.dataTable.first.reset();
    }

}