import { ActivatedRoute } from '@angular/router';
import { Component, OnDestroy, OnInit, ViewChildren, QueryList } from '@angular/core';
import { NavigateQueryParametersService } from '../shared/NavigateQueryParametersService';
import { BaseComponent } from '../shared/BaseComponent';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { GlobalSettingsService } from '../shared/globalSettings';
import { Route, RouteFilter, RoutesService } from './routes';
import { RefreshService } from '../shared/refreshService';
import { SecurityService } from '../shared/security/securityService';
import { BranchService } from '../shared/branch/branchService';
import { JobService } from '../job/job';
import { AppSearchParameters } from '../shared/appSearch/appSearch';
import { DataTable } from 'primeng/primeng';
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
    private routeFilter: RouteFilter;
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
            this.routeFilter = RouteFilter.toRouteFilter(<AppSearchParameters>params);
            this.getRoutes();
        });

        this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName)
            .takeWhile(() => this.alive)
            .subscribe(
            (branches: Array<[string, string]>) =>
            {
                this.branches = branches;
            });

        this.jobService.getStatusValueList()
            .takeWhile(() => this.alive)
            .subscribe((jobStatus: Array<[string, string]>) => {
                this.jobStatus = jobStatus;
            });
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
                this.dataTable.first.filters = <any>this.routeFilter;
            },
            error =>
            {
                this.lastRefresh = Date.now();
                this.isLoading = false;
            });
    }

    public clearFilters(): void
    {
        this.routeFilter = new RouteFilter();
        this.dataTable.first.filters = <any>this.routeFilter;
        this.dataTable.first.filter(undefined, undefined, undefined);
    }
}