import { NavigateQueryParametersService } from '../shared/NavigateQueryParametersService';
import { BaseComponent } from '../shared/BaseComponent';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GlobalSettingsService } from '../shared/globalSettings';
import { Route } from './route';
import { RoutesService } from './routesService';
import { RefreshService } from '../shared/refreshService';
import { SecurityService } from '../shared/security/securityService';
import { BranchService } from '../shared/branch/branchService';
import { JobService } from '../job/job';
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
        super(nqps, globalSettingsService, securityService );
    }

    public ngOnInit()
    {
        super.ngOnInit();

        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getRoutes());
        this.activatedRoute.queryParams.subscribe(params =>
        {
            this.getRoutes();
        });

        this.branchService.getBranchesValueList(this.globalSettingsService.globalSettings.userName)
            .takeWhile(() => this.alive)
            .subscribe((branches: Array<[string, string]>) => this.branches = branches);

        this.jobService.getBranchesValueList()
            .takeWhile(() => this.alive)
            .subscribe((jobStatus: Array<[string, string]>) => this.jobStatus = jobStatus);

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
            .subscribe(routes =>
            {
                this.routes = <Route[]>routes;
                this.lastRefresh = Date.now();
                this.isLoading = false;
            },
            error =>
            {
                this.lastRefresh = Date.now();
                this.isLoading = false;
            });
    }

}