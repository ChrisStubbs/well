import { ActivatedRoute } from '@angular/router';
import { Component, OnDestroy, OnInit, ViewChild, QueryList } from '@angular/core';
import { NavigateQueryParametersService } from '../shared/NavigateQueryParametersService';
import { BaseComponent } from '../shared/BaseComponent';
import { GlobalSettingsService } from '../shared/globalSettings';
import { Route } from './route';
import { RouteFilter } from './routeFilter';
import { RoutesService } from './routesService';
import { RefreshService } from '../shared/refreshService';
import { SecurityService } from '../shared/security/securityService';
import { BranchService } from '../shared/branch/branchService';
import { JobService } from '../job/job';
import { AppSearchParameters } from '../shared/appSearch/appSearch';
import { DataTable } from 'primeng/primeng';
import 'rxjs/Rx';
import { AssignModal } from '../shared/assignModal';
import { AssignModel } from '../shared/assignModel';
import { Branch } from '../shared/branch/branch';
import { AppDefaults } from '../shared/defaults/defaults';

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
    private actions: string[] = ['Re-Plan'];
    public rowCount = AppDefaults.Paginator.rowCount();
    public pageLinks = AppDefaults.Paginator.pageLinks();
    public rowsPerPageOptions = AppDefaults.Paginator.rowsPerPageOptions();

    private routeFilter: RouteFilter;
    private exceptionFilterItems: Array<[string, string]> = [['', 'All'], ['true', 'Yes'], ['false', 'No']];

    @ViewChild('dt') public dataTable: DataTable;
    @ViewChild(AssignModal) private assignModal: AssignModal;

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
        this.activatedRoute.queryParams
            .takeWhile(() => this.alive)
            .subscribe(params =>
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
            .subscribe((jobStatus: Array<[string, string]>) =>
            {
                this.jobStatus = jobStatus;
            });
    }

    public ngOnDestroy()
    {
        super.ngOnDestroy();
        this.alive = false;
        this.refreshSubscription.unsubscribe();
    }

    private getRoutes(): void
    {
        this.routeService.getRoutes()
            .takeWhile(() => this.alive)
            .subscribe((result: Route[]) =>
            {
                this.routes = result;
                this.lastRefresh = Date.now();
                this.isLoading = false;
                this.dataTable.filters = <any>this.routeFilter;
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
        this.dataTable.filters = <any>this.routeFilter;
        this.dataTable.filter(undefined, undefined, undefined);
    }

    public getAssignModel(route: Route): AssignModel
    {
        const branch = { id: route.branchId } as Branch;
        return new AssignModel(route.assignee, branch, route.jobIds, this.isReadOnlyUser);
    }

    public onAssigned($event)
    {
        this.getRoutes();
    }
}