import { NavigateQueryParametersService }           from '../shared/NavigateQueryParametersService';
import { BaseComponent }                            from '../shared/BaseComponent';
import { Component, OnDestroy, OnInit, ViewChild }  from '@angular/core';
import { Router, ActivatedRoute }                   from '@angular/router';
import {GlobalSettingsService}                      from '../shared/globalSettings';
import {Route}                                      from './route';
import {RouteHeaderService}                         from './routeHeaderService';
import {DropDownItem}                               from '../shared/dropDownItem';
import { FilterOption }                             from '../shared/filterOption';
import {RefreshService}                             from '../shared/refreshService';
import {DeliverySelectionModal}                     from './delivery-selection-modal';
import {OrderArrowComponent}                        from '../shared/orderbyArrow';
import {SecurityService}                            from '../shared/security/securityService';
import {UnauthorisedComponent}                      from '../unauthorised/unauthorisedComponent';
import * as lodash                                  from 'lodash';
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
    public isReadOnlyUser: boolean = false;

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private routerHeaderService: RouteHeaderService,
        private refreshService: RefreshService,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private securityService: SecurityService,
        private nqps: NavigateQueryParametersService ) {

            super(nqps);
            this.options = [
                new DropDownItem('Route', 'route'),
                new DropDownItem('Account', 'account', true),
                new DropDownItem('Invoice', 'invoice', true),
                new DropDownItem('Assignee', 'assignee', true)
            ];
    }

    @ViewChild(DeliverySelectionModal) public deliverySelectionModal: DeliverySelectionModal;

    public ngOnInit() {
        super.ngOnInit();

        this.securityService.validateUser(
            this.globalSettingsService.globalSettings.permissions,
            this.securityService.actionDeliveries);

        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getRoutes());
        this.activatedRoute.queryParams.subscribe(params => {
            this.getRoutes();
        });
        this.getRoutes();
    }

    public ngOnDestroy() {
        super.ngOnDestroy();
        this.refreshSubscription.unsubscribe();
    }

    private sortDirection(sortDirection): void {
        const sortString = sortDirection ? 'asc' : 'desc';
        this.routes = lodash.orderBy(this.routes, ['routeDate'], [sortString]);
        super.onSortDirectionChanged(sortDirection);
    }

    public onSortDirectionChanged(isDesc: boolean) {
        this.sortDirection(isDesc);
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