import { Component, OnInit, ViewChild}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {PaginatePipe, PaginationControlsCmp, PaginationService } from 'ng2-pagination';
import {IRoute} from './route';
import {RouteHeaderService} from './routeHeaderService';
import {OptionFilterComponent} from '../shared/optionfilter.component';
import {OptionFilterPipe } from '../shared/optionFilterPipe';
import {DropDownItem} from "../shared/dropDownItem";
import Option = require("../shared/filterOption");
import FilterOption = Option.FilterOption;
import {WellModal} from "../shared/well-modal";
import {RefreshService} from "../shared/refreshService";

@Component({
    selector: 'ow-routes',
    templateUrl: './app/route_header/routeheader-list.html',
    providers: [HTTP_PROVIDERS, GlobalSettingsService, RouteHeaderService, PaginationService],
    directives: [OptionFilterComponent, PaginationControlsCmp, WellModal],
    pipes: [OptionFilterPipe, PaginatePipe]
})
export class RouteHeaderComponent implements OnInit {
    refreshSubscription: any;
    errorMessage: string;
    routes: IRoute[];
    rowCount: number = 10;
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
    }

    ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
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



