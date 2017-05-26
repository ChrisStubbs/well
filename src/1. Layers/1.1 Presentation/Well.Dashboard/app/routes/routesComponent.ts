import { ActivatedRoute }                           from '@angular/router';
import { Component, ViewChild }                     from '@angular/core';
import { GlobalSettingsService }                    from '../shared/globalSettings';
import { Route }                                    from './route';
import { RouteFilter }                              from './routeFilter';
import { RoutesService }                            from './routesService';
import { RefreshService }                           from '../shared/refreshService';
import { SecurityService }                          from '../shared/security/securityService';
import { BranchService }                            from '../shared/branch/branchService';
import { AppSearchParameters }                      from '../shared/appSearch/appSearch';
import { DataTable }                                from 'primeng/primeng';
import { AssignModel, AssignModalResult }           from '../shared/components/components';
import { Branch }                                   from '../shared/branch/branch';
import { AppDefaults }                              from '../shared/defaults/defaults';
import { IObservableAlive }                         from '../shared/IObservableAlive';
import { LookupService, LookupsEnum, ILookupValue}  from '../shared/services/services';
import * as _                                       from 'lodash';
import 'rxjs/Rx';

@Component({
    selector: 'ow-route',
    templateUrl: './app/routes/route-list.html',
    providers: [RoutesService]
})
export class RoutesComponent implements IObservableAlive
{
    public isLoading: boolean = true;
    public refreshSubscription: any;
    public errorMessage: string;
    public routes: Route[];
    public lastRefresh = Date.now();
    public isReadOnlyUser: boolean = false;
    public branches: Array<[string, string]>;
    public jobStatus: Array<ILookupValue>;

    public isAlive: boolean = true;
    private actions: string[] = ['Re-Plan'];
    public rowCount = AppDefaults.Paginator.rowCount();
    public pageLinks = AppDefaults.Paginator.pageLinks();
    public rowsPerPageOptions = AppDefaults.Paginator.rowsPerPageOptions();

    private routeFilter: RouteFilter;
    private exceptionFilterItems: Array<[string, string]> = [['', 'All'], ['true', 'Yes'], ['false', 'No']];

    @ViewChild('dt') public dataTable: DataTable;

    constructor(
        private lookupService: LookupService,
        protected globalSettingsService: GlobalSettingsService,
        private routeService: RoutesService,
        private refreshService: RefreshService,
        private activatedRoute: ActivatedRoute,
        protected securityService: SecurityService,
        private branchService: BranchService) { }

    public ngOnInit()
    {
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getRoutes());
        this.activatedRoute.queryParams
            .takeWhile(() => this.isAlive)
            .subscribe(params =>
            {
                this.routeFilter = RouteFilter.toRouteFilter(<AppSearchParameters>params);
                this.getRoutes();
            });

        this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName)
            .takeWhile(() => this.isAlive)
            .subscribe(
            (branches: Array<[string, string]>) =>
            {
                this.branches = branches;
            });

        this.lookupService.get(LookupsEnum.JobStatus)
            .takeWhile(() => this.isAlive)
            .subscribe((value: Array<ILookupValue>) => this.jobStatus = value);
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
        this.refreshSubscription.unsubscribe();
    }

    private getRoutes(): void
    {
        this.routeService.getRoutes()
            .takeWhile(() => this.isAlive)
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
        return new AssignModel(route.assignee, branch, route.jobIds, this.isReadOnlyUser, route);
    }

    public onAssigned($event)
    {
        const result = $event as AssignModalResult;
        if (result.assigned)
        {
            const route = _.find(this.routes, (x) => x.id === result.source.id) as Route;
            if (route)
            {
                route.assignee = (result.newUser) ? result.newUser.name : 'Unallocated';
            }
        }
    }
}