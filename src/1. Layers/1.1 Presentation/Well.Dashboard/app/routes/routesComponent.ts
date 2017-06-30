import { ActivatedRoute } from '@angular/router';
import { Component, ViewChild } from '@angular/core';
import { GlobalSettingsService } from '../shared/globalSettings';
import { Route } from './route';
import { RouteFilter } from './routeFilter';
import { RoutesService } from './routesService';
import { RefreshService } from '../shared/refreshService';
import { SecurityService } from '../shared/security/securityService';
import { BranchService } from '../shared/branch/branchService';
import { AppSearchParameters } from '../shared/appSearch/appSearch';
import { DataTable } from 'primeng/primeng';
import { AssignModel, AssignModalResult } from '../shared/components/components';
import { Branch } from '../shared/branch/branch';
import { IObservableAlive } from '../shared/IObservableAlive';
import { LookupService, LookupsEnum, ILookupValue } from '../shared/services/services';
import { Router } from '@angular/router';
import * as _ from 'lodash';
import 'rxjs/Rx';

@Component({
    selector: 'ow-route',
    templateUrl: './app/routes/routesComponent.html',
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
    public routeStatus: Array<ILookupValue>;
    public isAlive: boolean = true;

    public rowCount = 10;
    public pageLinks = 3;
    public rowsPerPageOptions = [10, 20, 30, 40, 50];

    private routeFilter: RouteFilter;
    private yesNoFilterItems: Array<[string, string]> = [['', 'All'], ['true', 'Yes'], ['false', 'No']];

    @ViewChild('dt') public dataTable: DataTable;

    constructor(
        private lookupService: LookupService,
        protected globalSettingsService: GlobalSettingsService,
        private routeService: RoutesService,
        private refreshService: RefreshService,
        private activatedRoute: ActivatedRoute,
        protected securityService: SecurityService,
        private branchService: BranchService,
        private router: Router) { }

    public ngOnInit()
    {
        this.activatedRoute.queryParams
            .takeWhile(() => this.isAlive)
            .subscribe(params =>
            {
                this.routeFilter = RouteFilter.toRouteFilter(<AppSearchParameters>params);

                this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName)
                    .takeWhile(() => this.isAlive)
                    .subscribe(
                    (branches: Array<[string, string]>) =>
                    {
                        this.branches = branches;
                        if (branches.length === 0)
                        {
                            // no branches set up
                            this.router.navigateByUrl('/branch');
                            return;
                        }
                        if (!this.routeFilter.branchId.value)
                        {
                            this.routeFilter.branchId.value = branches[0][0];
                        }

                        this.getRoutesByBranch();

                        this.refreshSubscription = this.refreshService.dataRefreshed$
                            .takeWhile(() => this.isAlive)
                            .subscribe(r => this.getRoutesByBranch());

                    });

                this.lookupService.get(LookupsEnum.RouteStatus)
                    .takeWhile(() => this.isAlive)
                    .subscribe((value: Array<ILookupValue>) => this.routeStatus = value);
            });
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    private getFilteredBranchId(): number
    {
        return this.routeFilter.branchId.value as number;
    }

    private getRoutesByBranch(): void
    {
        const branchId =
            this.routeService.getRoutesByBranch(this.getFilteredBranchId())
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
        this.routeFilter = new RouteFilter(this.getFilteredBranchId());
        this.dataTable.filters = <any>this.routeFilter;
        this.dataTable.filter(undefined, undefined, undefined);
    }

    public getAssignModel(route: Route): AssignModel
    {
        const branch = { id: route.branchId } as Branch;
        return new AssignModel(route.assignee, branch, route.jobIds, this.isReadOnlyUser, route);
    }

    public onAssigned($event): void
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

    public refreshData(event): void
    {
        this.routeFilter.branchId.value = event.target.value;
        this.getRoutesByBranch();
    }
}