import { ActivatedRoute }                               from '@angular/router';
import { Component }                                    from '@angular/core';
import { GlobalSettingsService }                        from '../shared/globalSettings';
import { Route }                                        from './route';
import { RouteFilter }                                  from './routeFilter';
import { RoutesService }                                from './routesService';
import { BranchService }                                from '../shared/branch/branchService';
import { AppSearchParameters }                          from '../shared/appSearch/appSearch';
import { AssignModel, AssignModalResult }               from '../shared/components/components';
import { Branch }                                       from '../shared/branch/branch';
import { IObservableAlive }                             from '../shared/IObservableAlive';
import { LookupService, LookupsEnum, ILookupValue }     from '../shared/services/services';
import { Router }                                       from '@angular/router';
import * as _                                           from 'lodash';
import { Observable }                                   from "rxjs/Observable";
import 'rxjs/Rx';
import { GridHelpersFunctions }                         from '../shared/gridHelpers/gridHelpersFunctions';

@Component({
    selector: 'ow-route',
    templateUrl: './app/routes/routesComponent.html',
    providers: [RoutesService]
})
export class RoutesComponent implements IObservableAlive
{
    public errorMessage: string;
    public routes: Route[];
    public gridSource: Route[];
    public isReadOnlyUser: boolean = false;
    public branches: Array<[string, string]>;
    public routeStatus: Array<ILookupValue>;
    public isAlive: boolean = true;
    private routeNumbers: Array<string> = [];

    private routeFilter: RouteFilter;

    constructor(
        private lookupService: LookupService,
        protected globalSettingsService: GlobalSettingsService,
        private routeService: RoutesService,
        private activatedRoute: ActivatedRoute,
        private branchService: BranchService,
        private router: Router) { }

    public ngOnInit()
    {
        this.activatedRoute.queryParams
            .flatMap(params =>
            {
                this.routeFilter = RouteFilter.toRouteFilter(<AppSearchParameters>params);

                return Observable.forkJoin(
                    this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName),
                    this.lookupService.get(LookupsEnum.RouteStatus)
                );
            })
            .takeWhile(() => this.isAlive)
            .subscribe(res =>
            {
                this.branches = res[0];
                if (this.branches.length === 0)
                {
                    // no branches set up
                    this.router.navigateByUrl('/branch');
                    return;
                }

                if (!this.routeFilter.branchId)
                {
                    this.routeFilter.branchId = +this.branches[0][0];
                }

                this.getRoutesByBranch();
                this.routeStatus = res[1];
            });
    }

    public ngOnDestroy()
    {
        this.isAlive = false;
    }

    private getRoutesByBranch(): void
    {
        this.routeService.getRoutesByBranch(this.routeFilter.branchId)
            .takeWhile(() => this.isAlive)
            .subscribe((result: Route[]) =>
            {
                this.routes = result;
                this.fillGridSource();

                _.forEach(this.routes, (current: Route) =>
                {
                    this.routeNumbers.push(current.routeNumber);
                });

                this.routeNumbers = _.uniq(this.routeNumbers);
            });
    }

    private fillGridSource()
    {
        this.gridSource = GridHelpersFunctions.applyGridFilter<Route, RouteFilter>(this.routes, this.routeFilter);

    }

    public clearFilters(): void
    {
        const branchId = this.routeFilter.branchId;

        this.routeFilter = new RouteFilter();
        this.routeFilter.branchId = branchId;
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
        this.routeFilter.branchId = +event.target.value;
        this.getRoutesByBranch();
    }
}