import { NavigateQueryParametersService }           from '../shared/NavigateQueryParametersService';
import { BaseComponent }                            from '../shared/BaseComponent';
import { Component, OnDestroy, OnInit, ViewChild }  from '@angular/core';
import { ActivatedRoute }                           from '@angular/router';
import {GlobalSettingsService}                      from '../shared/globalSettings';
import {Route}                                      from '../routes/route';
import {RouteHeaderService}                         from './routeHeaderService';
import {DropDownItem}                               from '../shared/dropDownItem';
import {RefreshService}                             from '../shared/refreshService';
import {DeliverySelectionModal}                     from './delivery-selection-modal';
import {SecurityService}                            from '../shared/security/securityService';
import { OrderByExecutor }                          from '../shared/OrderByExecutor';
import 'rxjs/Rx';

@Component({
    selector: 'ow-routes',
    templateUrl: './app/route_header/routeheader-list.html',
    providers: [RouteHeaderService]
})
export class RouteHeaderComponent extends BaseComponent implements OnInit, OnDestroy {
    public isLoading: boolean = true;
    public refreshSubscription: any;
    public errorMessage: string;
    public routes: Route[];
    public lastRefresh = Date.now();
    private orderBy: OrderByExecutor = new OrderByExecutor();

    constructor(
        protected globalSettingsService: GlobalSettingsService,
        private routerHeaderService: RouteHeaderService,
        private refreshService: RefreshService,
        private activatedRoute: ActivatedRoute,
        protected securityService: SecurityService,
        private nqps: NavigateQueryParametersService )
    {
        super(nqps, globalSettingsService, securityService);
        this.options = [
            new DropDownItem('Route', 'route'),
            new DropDownItem('Branch', 'routeOwnerId', false, 'number')
        ];
        this.sortField = 'routeDate';
    }

    @ViewChild(DeliverySelectionModal) public deliverySelectionModal: DeliverySelectionModal;

    public ngOnInit() {
        super.ngOnInit();

        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getRoutes());
        this.activatedRoute.queryParams.subscribe(params =>
        {
            this.getRoutes();
        });
    }

    public ngOnDestroy() {
        super.ngOnDestroy();
        this.refreshSubscription.unsubscribe();
    }

    public onSortDirectionChanged(isDesc: boolean)
    {
        super.onSortDirectionChanged(isDesc);
        this.routes = this.orderBy.Order(this.routes, this);
    }

    public getRoutes(): void {
        this.routerHeaderService.getRouteHeaders()
            .subscribe(routes => {
                    this.routes = routes;
                    this.lastRefresh = Date.now();
                    this.isLoading = false;
                },
                error => {
                    this.lastRefresh = Date.now();
                    this.isLoading = false;
                });
    }

    public routeSelected(route): void {
        this.deliverySelectionModal.show(route);
    }
}