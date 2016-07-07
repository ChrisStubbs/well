/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
//common
import {bootstrap} from 'angular2/platform/browser';
import {Component, OnInit, ChangeDetectorRef} from 'angular2/core';
import { HTTP_PROVIDERS } from 'angular2/http';
import 'rxjs/Rx';   // Load all features
import { ROUTER_PROVIDERS, RouteConfig, ROUTER_DIRECTIVES, Router } from 'angular2/router';
import * as lodash from 'lodash';

//widget stats
import {WidgetStatsService} from './widgetstats/widgetstats-service';
import {WidgetStatsComponent} from './widgetstats/widgetStatsComponent';
//routes
import {RouteHeaderComponent} from './route_header/routeHeaderComponent';
//clean
import {CleanRoutesComponent} from './clean/cleanRoutesComponent';
//resolved
import {ResolvedRoutesComponent} from './resolved/resolved-routesComponent';
//notifications
import {NotificationsComponent} from './notifications/notificationsComponent';

declare var $: any;

@Component({
    selector: 'ow-app',
    templateUrl: './app/main.html',
    directives: [ROUTER_DIRECTIVES],
    providers: [WidgetStatsService, HTTP_PROVIDERS, ROUTER_PROVIDERS]
})

    @RouteConfig([
        { path: '/widgetstats', name: 'WidgetStats', component: WidgetStatsComponent, useAsDefault: true },
        { path: '/routes', name: 'Routes', component: RouteHeaderComponent },
        { path: '/clean', name: 'Clean', component: CleanRoutesComponent },
        { path: '/resolved', name: 'Resolved', component: ResolvedRoutesComponent },
        { path: '/notifications', name: 'Notifications', component: NotificationsComponent }
])

export class AppComponent implements OnInit  {

    constructor(private router: Router, private changeDetectorRef: ChangeDetectorRef ) { }

    //re-direct to widget stats on load
    ngOnInit() {
        this.router.navigate(['WidgetStats']);
    }
        

}



