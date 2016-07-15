import { Component, OnInit, ViewContainerRef }  from '@angular/core';
import { DialogRef } from 'angular2-modal';
import { Modal, BS_MODAL_PROVIDERS } from 'angular2-modal/plugins/bootstrap';
import { ROUTER_DIRECTIVES} from '@angular/router';
import {PaginatePipe, PaginationControlsCmp, PaginationService } from 'ng2-pagination';
import {IRoute} from './route';
import {RouteHeaderService} from './routeHeaderService';
import {OptionFilterComponent} from '../shared/optionfilter.component';
import {OptionFilterPipe } from '../shared/optionFilterPipe';
import {DropDownItem} from "../shared/DropDownItem";
import Option = require("../shared/filterOption");
import FilterOption = Option.FilterOption;

@Component({
    templateUrl: './app/route_header/routeheader-list.html',
    providers: [RouteHeaderService, PaginationService, BS_MODAL_PROVIDERS],
    directives: [ROUTER_DIRECTIVES, OptionFilterComponent, PaginationControlsCmp],
    pipes: [OptionFilterPipe, PaginatePipe]
})
export class RouteHeaderComponent implements OnInit {
    errorMessage: string;
    routes: IRoute[];
    rowCount: number = 10;
    lastRefresh: string = '01 january 1666 13:05';
    filterOption: Option.FilterOption = new FilterOption();
    options: DropDownItem[] = [
        new DropDownItem("Route", "route"),        
        new DropDownItem("Account", "account", true),
        new DropDownItem("Invoice", "invoice", true),
        new DropDownItem("Assignee", "assignee", true)
    ];

    constructor(private routerHeaderService: RouteHeaderService, public modal: Modal, viewContainer: ViewContainerRef) {
        modal.defaultViewContainer = viewContainer;
    }

    ngOnInit() {
        this.routerHeaderService.getRouteHeaders("lee", "foo")
            .subscribe(routes => this.routes = routes, error => this.errorMessage = <any>error);
    }

    routeSelected(route): void {
        this.modal.confirm()
            .isBlocking(true)
            .showClose(true)
            .keyboard(27)
            .title("A title")
            .body("<p>hi lee</p><p>hi paul</p>")
            .open();
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



