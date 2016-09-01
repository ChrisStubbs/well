import { Component, OnInit, ViewChild}  from '@angular/core';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {PaginationService } from 'ng2-pagination';
import {IRoute} from './route';
import {RouteHeaderService} from './routeHeaderService';
import {DropDownItem} from "../shared/dropDownItem";
import Option = require("../shared/filterOption");
import FilterOption = Option.FilterOption;
import {WellModal} from "../shared/well-modal";
import {RefreshService} from "../shared/refreshService";

@Component({
    selector: 'ow-routes',
    templateUrl: './app/route_header/routeheader-list.html',
    providers: [GlobalSettingsService, RouteHeaderService, PaginationService],
    directives: [WellModal]
})
export class RouteHeaderComponent implements OnInit {
    refreshSubscription: any;
    errorMessage: string;
    routes: IRoute[];
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
        private routerHeaderService: RouteHeaderService,
        private refreshService: RefreshService){
    }

    @ViewChild(WellModal) modal = new WellModal();

    ngOnInit() {
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getRoutes());
        this.getRoutes();
        this.currentConfigSort = '-dateTimeUpdated';
    }

    ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    sortDirection(sortDirection): void {
        this.currentConfigSort = sortDirection === true ? '+dateTimeUpdated' : '-dateTimeUpdated';
        this.getRoutes();
    }

    getRoutes(): void {
        this.routerHeaderService.getRouteHeaders()
            .subscribe(routes => {
                    this.routes = routes;
                    this.lastRefresh = Date.now();
                },
                error => this.lastRefresh = Date.now());
    }

    routeSelected(route): void {
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



