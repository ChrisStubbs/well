﻿import { Component, OnInit }  from '@angular/core';
import { ROUTER_DIRECTIVES} from '@angular/router';
import {IRoute} from './route';
import {RouteHeaderService} from './routeHeaderService';
import { RouteFilterPipe } from './routeFilterPipe';
import {GlobalSettingsService} from '../shared/globalSettings';

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
    filterText: string;

    constructor(private routerHeaderService: RouteHeaderService) {}

    ngOnInit() {

        this.routerHeaderService.getRouteHeaders()
            .subscribe(routes => this.routes = routes, error => this.errorMessage = <any>error);
    }

    routeSelected(route): void {
        console.log(route.driverName);
    }

    clearFilterText(): void {
        this.filterText = '';
    }

    foo(): void {
        console.log(this.filterText);
    }
}



