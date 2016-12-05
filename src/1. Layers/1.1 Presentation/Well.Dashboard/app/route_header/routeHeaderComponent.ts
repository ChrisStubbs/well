import { Component, OnInit, ViewChild}  from '@angular/core';
import { Router } from '@angular/router';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {Route} from './route';
import {RouteHeaderService} from './routeHeaderService';
import {DropDownItem} from '../shared/dropDownItem';
import Option = require('../shared/filterOption');
import FilterOption = Option.FilterOption;
import {RefreshService} from '../shared/refreshService';
import {DeliverySelectionModal} from './delivery-selection-modal';
import {OrderArrowComponent} from '../shared/orderbyArrow';
import {SecurityService} from '../shared/security/securityService';
import {UnauthorisedComponent} from '../unauthorised/unauthorisedComponent';
import * as lodash from 'lodash';

@Component({
    selector: 'ow-routes',
    templateUrl: './app/route_header/routeheader-list.html',
    providers: [RouteHeaderService]
})
export class RouteHeaderComponent implements OnInit {
    isLoading: boolean = true;
    refreshSubscription: any;
    errorMessage: string;
    routes: Route[];
    rowCount: number = 10;
    currentConfigSort: string;
    lastRefresh = Date.now();
    filterOption: Option.FilterOption = new FilterOption();
    options: DropDownItem[] = [
        new DropDownItem("Route", "route"),
        new DropDownItem("Account", "account", true),
        new DropDownItem("Invoice", "invoice", true),
        new DropDownItem("Assignee", "assignee", true)
    ];

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private routerHeaderService: RouteHeaderService,
        private refreshService: RefreshService,
        private router: Router,
        private securityService: SecurityService) {
    }

    @ViewChild(DeliverySelectionModal) deliverySelectionModal : DeliverySelectionModal;

    ngOnInit() {
        this.securityService.validateUser(this.globalSettingsService.globalSettings.permissions, this.securityService.actionDeliveries);
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getRoutes());
        this.getRoutes();
        this.currentConfigSort = '+dateTimeUpdated';
        this.sortDirection(false);
    }

    ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    sortDirection(sortDirection): void {
        this.currentConfigSort = sortDirection === true ? '+dateTimeUpdated' : '-dateTimeUpdated';
        var sortString = this.currentConfigSort === '+dateTimeUpdated' ? 'asc' : 'desc';
        lodash.sortBy(this.routes, ['dateTimeUpdated'], [sortString]);
    }

    onSortDirectionChanged(isDesc: boolean) {
        this.sortDirection(isDesc);
    }

    getRoutes(): void {
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

    routeSelected(route): void {
        this.deliverySelectionModal.show(route);
    }

    onFilterClicked(filterOption: FilterOption) {

        if (filterOption.dropDownItem.requiresServerCall) {
            this.routerHeaderService.getRouteHeaders(filterOption.dropDownItem.value, filterOption.filterText)
                .subscribe(routes => this.routes = routes, error => this.errorMessage = <any>error);
        } else {
            this.filterOption = filterOption;
        }
    }
}



