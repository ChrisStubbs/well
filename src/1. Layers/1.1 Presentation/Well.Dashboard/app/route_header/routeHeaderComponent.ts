import { Component, OnInit, ViewChild}  from '@angular/core';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {PaginationService } from 'ng2-pagination';
import {IRoute} from './route';
import {RouteHeaderService} from './routeHeaderService';
import {DropDownItem} from '../shared/dropDownItem';
import Option = require('../shared/filterOption');
import FilterOption = Option.FilterOption;
import {WellModal} from '../shared/well-modal';
import {RefreshService} from '../shared/refreshService';
import {ContextMenuItem} from './contextMenuItem';

@Component({
    selector: 'ow-routes',
    templateUrl: './app/route_header/routeheader-list.html',
    providers: [GlobalSettingsService, RouteHeaderService, PaginationService]
})
export class RouteHeaderComponent implements OnInit {
    refreshSubscription: any;
    errorMessage: string;
    routes: IRoute[];
    rowCount: number = 10;
    currentConfigSort: string;
    isContextMenuVisible = false;
    lastRefresh = Date.now();
    filterOption: Option.FilterOption = new FilterOption();
    options: DropDownItem[] = [
        new DropDownItem("Route", "route"),
        new DropDownItem("Account", "account", true),
        new DropDownItem("Invoice", "invoice", true),
        new DropDownItem("Assignee", "assignee", true)
    ];
    items: ContextMenuItem[];

    private mouseLocation: { left: number, top: number } = { left: 0, top: 0 };

    constructor(
        private routerHeaderService: RouteHeaderService,
        private refreshService: RefreshService) {
    }

    @ViewChild(WellModal) modal = new WellModal();

    ngOnInit() {
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getRoutes());
        this.getRoutes();
        this.currentConfigSort = '-dateTimeUpdated';
        this.items = [{ name: 'View exceptions' },{ name: 'View clean' }];
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

    routeSelected($event: MouseEvent, route): void {
        this.showMenu($event);
    }

    onFilterClicked(filterOption: FilterOption) {

        if (filterOption.dropDownItem.requiresServerCall) {
            this.routerHeaderService.getRouteHeaders(filterOption.dropDownItem.value, filterOption.filterText)
                .subscribe(routes => this.routes = routes, error => this.errorMessage = <any>error);
        } else {
            this.filterOption = filterOption;
        }
    }

    get locationCss() {
        return {
            position: 'fixed',
            border: '1px solid #ddd',
            display: this.isContextMenuVisible ? 'block' : 'none',
            left: this.mouseLocation.left + 'px',
            top: this.mouseLocation.top + 'px'
        };
    }

    showMenu(event) {
        this.isContextMenuVisible = true;
        this.mouseLocation = {
            left: event.clientX,
            top: event.clientY
        }
    }

    closeMenu() {
        this.isContextMenuVisible = false;
    }
}



