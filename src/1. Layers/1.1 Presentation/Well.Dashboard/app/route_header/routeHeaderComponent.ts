import { Component, OnInit }  from 'angular2/core';
import { ROUTER_DIRECTIVES} from 'angular2/router';

import {IRoute} from './route';
import {RouteHeaderService} from './routeHeaderService';

@Component({
    templateUrl: './app/route_header/routeheader-list.html',
    providers: [RouteHeaderService],
    directives: [ROUTER_DIRECTIVES]

})

export class RouteHeaderComponent implements OnInit {
    errorMessage: string;
    routes: IRoute[];

    constructor(private routerHeaderService: RouteHeaderService) { }

    ngOnInit() {

        this.routerHeaderService.getRouteHeaders()
            .subscribe(routes => this.routes = routes, error => this.errorMessage = <any>error);
    }

}



