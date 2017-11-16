import { NavigateQueryParametersService }               from './../shared/services/navigateQueryParametersService';
import { ActivatedRoute }                               from '@angular/router';
import { Component }                                    from '@angular/core';
import { Router }                                       from '@angular/router';
import { GlobalSettingsService }                        from '../shared/globalSettings';
import { Route }                                        from './route';
import { RouteFilter }                                  from './routeFilter';
import { RoutesService }                                from './routesService';
import { BranchService }                                from '../shared/branch/branchService';
import { AssignModel, AssignModalResult }               from '../shared/components/components';
import { Branch }                                       from '../shared/branch/branch';
import { IObservableAlive }                             from '../shared/IObservableAlive';
import { LookupService, LookupsEnum, ILookupValue }     from '../shared/services/services';
import * as _                                           from 'lodash';
import { Observable }                                   from 'rxjs/Observable';
import { GridHelpersFunctions }                         from '../shared/gridHelpers/gridHelpersFunctions';
import { SecurityService }                              from '../shared/services/securityService';
import { Assignee }                                     from './../shared/models/assignee';
import 'rxjs/Rx';
import {GlobalSettings} from '../shared/globalSettings';

@Component({
    selector: 'ow-route',
    templateUrl: './app/routes/routesComponent.html',
    providers: [RoutesService, NavigateQueryParametersService, GlobalSettings]
})
export class RoutesComponent implements IObservableAlive {
    public routes: Route[];
    public gridSource: Route[] = [];
    public branches: Array<[string, string]>;
    public routeStatus: Array<ILookupValue>;
    public jobIssueType: Array<ILookupValue>;
    public isAlive: boolean = true;
    public rowCount = 10;

    private routeNumbers: Array<string> = [];
    private drivers: Array<string> = [];
    private routeFilter: RouteFilter;
    private assignees: Array<string> = [];

    constructor(
        private securityService: SecurityService,
        private lookupService: LookupService,
        protected globalSettingsService: GlobalSettingsService,
        private routeService: RoutesService,
        private activatedRoute: ActivatedRoute,
        private branchService: BranchService,
        private router: Router,
        private historyNavigate: NavigateQueryParametersService) { }

    public ngOnInit() 
    {
        this.globalSettingsService.setCurrentBranchFromUrl(this.activatedRoute);

        this.historyNavigate.BrowserNavigation
            .takeWhile(() => this.isAlive)
            .subscribe((p: string) =>
            {
                const params = this.historyNavigate.GetQueryStringObject();
                this.handleQueryString(params);
            });

        this.activatedRoute.queryParams
            .flatMap(params => {
                this.handleQueryString(params);

                return Observable.forkJoin(
                    this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName),
                    this.lookupService.get(LookupsEnum.RouteStatus),
                    this.lookupService.get(LookupsEnum.JobIssueType)
                );
            })
            .takeWhile(() => this.isAlive)
            .subscribe(res => {
                this.branches = res[0];
                if (this.branches.length === 0) {
                    // no branches set up
                    this.router.navigateByUrl('/branch');
                    return;
                }

                if (!this.globalSettingsService.currentBranchId ) {
                    this.globalSettingsService.currentBranchId = +this.branches[0][0];
                    this.routeFilter.branchId = this.globalSettingsService.currentBranchId;
                }

                this.getRoutesByBranch();
                this.routeStatus = res[1];
                this.jobIssueType = res[2];
            });
    }

    private handleQueryString(qs: any): void {
        this.routeFilter = RouteFilter.toRouteFilter(this.globalSettingsService.currentBranchId, qs); 
    }

    public ngOnDestroy() {
        this.isAlive = false;
    }

    private getRoutesByBranch(): void {
        
        this.routeService.getRoutesByBranch(this.globalSettingsService.currentBranchId)
            .takeWhile(() => this.isAlive)
            .subscribe((result: Route[]) => {
                this.routes = _.orderBy(result, [function (route: Route) {
                    return route.routeDate;
                }, 'routeNumber'], ['desc', 'asc']);

                this.fillGridSource();
                this.routeNumbers = [];
                this.drivers = []; 
                this.assignees = [];

                _.forEach(this.routes, (current: Route) => {
                    if (!current.assignee) {
                        current.assignee = 'Unallocated';
                    }

                    this.routeNumbers.push(current.routeNumber);
                    this.drivers.push(current.driverName);
                    
                    if (!_.isNil(current.assignees))
                    {
                        current.assignees.forEach((current: Assignee) => this.assignees.push(current.name));
                    }
                });

                this.routeNumbers = this.sortAndUniq(this.routeNumbers);
                this.drivers = this.sortAndUniq(this.drivers);
                this.assignees = this.sortAndUniq(this.assignees);
            });
    }

    private sortAndUniq(values: Array<string>): Array<string> {
        return _.chain(values)
            .uniq()
            .sortBy()
            .value();
    }

    private applyFilter()
    {
        this.fillGridSource();
        this.historyNavigate.Save(this.routeFilter);
    }

    private fillGridSource() {
        this.gridSource = GridHelpersFunctions.applyGridFilter<Route, RouteFilter>(this.routes, this.routeFilter);
    }

    public clearFilters(): void {
        this.routeFilter = new RouteFilter();
        this.routeFilter.branchId = this.globalSettingsService.currentBranchId;
    }

    public clearFiltersAndApply() {
        this.clearFilters();
        this.applyFilter();
    }

    public getAssignModel(route: Route): AssignModel {
        const branch = { id: route.branchId } as Branch;
        return new AssignModel(route.assignee, branch, route.jobIds, route, false);
    }

    public onAssigned($event): void {
        const result = $event as AssignModalResult;
        if (result.assigned) {
            const route = _.find(this.routes, (x) => x.id === result.source.id) as Route;
            if (route) {
                route.assignee = (result.newUser) ? result.newUser.name : 'Unallocated';
            }
        }
    }
    
    public refreshData(): void {
        this.getRoutesByBranch();
        this.historyNavigate.Save(this.routeFilter);
    }

    public changeBranch(event): void {
        if (event) {
            this.globalSettingsService.currentBranchId = +event.target.value;
            this.router.navigateByUrl(`/routes/${this.globalSettingsService.currentBranchId}`);
            this.clearFilters();
            this.refreshData();
        }
    }

    public getJobIssueTypeDescription() {
        const result = _.filter(
            this.jobIssueType,
            (current: ILookupValue) => +current.key == this.routeFilter.jobIssueType);

        if (!_.isEmpty(result)) {
            return result[0].value;
        }

        return '';
    }

     private changePage(p: number)
     {
        this.routeFilter.pageNumber = p;
        this.historyNavigate.Save(this.routeFilter);
     }
}