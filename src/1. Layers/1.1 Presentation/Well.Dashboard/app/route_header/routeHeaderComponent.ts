import { Component, OnInit }  from 'angular2/core';
import { ROUTER_DIRECTIVES } from 'angular2/router';

import {IRouteHeader} from './routeHeader';
import {RouteHeaderService} from './routeHeaderService';

@Component({
    templateUrl: './app/route_header/routeheader-list.html',
    providers: [RouteHeaderService],
    directives: [ROUTER_DIRECTIVES]

})

export class RouteHeaderComponent implements OnInit {
    errorMessage: string;
    routeheaders: IRouteHeader;


    constructor(private _routerHeaderService: RouteHeaderService) { }

    ngOnInit(): void {
        this._routerHeaderService.getRouteHeaders()
            .subscribe(routeheaders => this.routeheaders = routeheaders, error => this.errorMessage = <any>error);
    }

}



