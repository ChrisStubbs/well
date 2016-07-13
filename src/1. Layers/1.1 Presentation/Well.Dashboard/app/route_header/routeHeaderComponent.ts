﻿import { Component, OnInit }  from '@angular/core';
import { ROUTER_DIRECTIVES} from '@angular/router';
import {IRoute} from './route';
import {RouteHeaderService} from './routeHeaderService';

import {GlobalSettingsService} from '../shared/globalSettings';
import {OptionFilterComponent} from '../shared/optionfilter.component';
import {OptionFilterPipe } from '../shared/optionFilterPipe';
import {FilterOption} from "../shared/filterOption";
import {DropDownItem} from "../shared/DropDownItem";

@Component({
    templateUrl: './app/route_header/routeheader-list.html',
    providers: [RouteHeaderService, GlobalSettingsService],
    directives: [ROUTER_DIRECTIVES],
    pipes: [RouteFilterPipe]
})
export class RouteHeaderComponent implements OnInit {
    errorMessage: string;
    routes: IRoute[];
    rowCount: number = 10;
    filterOption: FilterOption = new FilterOption();
    options: DropDownItem[] = [
        new DropDownItem("Route", "route"),        
        new DropDownItem("Driver", "driverName"),
        new DropDownItem("No of Drops", "totalDrops"),
        new DropDownItem("Exceptions", "deliveryExceptionCount"),
        new DropDownItem("Clean", "deliveryCleanCount"),
        new DropDownItem("Status", "routeStatus"),
        new DropDownItem("Date", "dateTimeUpdated")
    ];

    constructor(private routerHeaderService: RouteHeaderService) {}

    ngOnInit() {

        this.routerHeaderService.getRouteHeaders()
            .subscribe(routes => this.routes = routes, error => this.errorMessage = <any>error);
    }

    routeSelected(route): void {
        console.log(route.driverName);
    }

    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }
  
}



