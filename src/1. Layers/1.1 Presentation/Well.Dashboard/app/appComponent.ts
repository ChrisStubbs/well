/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
//common
import {bootstrap} from '@angular/platform-browser-dynamic';
import {Component, OnInit, ChangeDetectorRef} from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import 'rxjs/Rx';   // Load all features
import { ROUTER_PROVIDERS, RouteConfig, ROUTER_DIRECTIVES, Router } from '@angular/router-deprecated';
import * as lodash from 'lodash';

//widget stats
import {WidgetStatsService} from './widgetstats/widgetstats-service';
import {WidgetStatsComponent} from './widgetstats/widgetStatsComponent';
//routes
import {RouteHeaderComponent} from './route_header/routeHeaderComponent';
//clean
import {CleanDeliveryComponent} from './clean/cleanDeliveryComponent';
//resolved
import {ResolvedDeliveryComponent} from './resolved/resolved-deliveryComponent';
//notifications
import {NotificationsComponent} from './notifications/notificationsComponent';
//account
import {AccountComponent} from './account/accountComponent';
//configuration
import {GlobalSettingsService} from './shared/globalSettings';

declare var $: any;

@Component({
    selector: 'ow-app',
    templateUrl: './app/main.html',
    directives: [ROUTER_DIRECTIVES],
    providers: [WidgetStatsService, HTTP_PROVIDERS, ROUTER_PROVIDERS, GlobalSettingsService]
})

    @RouteConfig([
        { path: '/widgetstats', name: 'WidgetStats', component: WidgetStatsComponent, useAsDefault: true },
        { path: '/routes', name: 'Routes', component: RouteHeaderComponent },
        { path: '/clean', name: 'Clean', component: CleanDeliveryComponent },
        { path: '/resolved', name: 'Resolved', component: ResolvedDeliveryComponent },
        { path: '/notifications', name: 'Notifications', component: NotificationsComponent },
        { path: '/account', name: 'Account', component: AccountComponent }
])

export class AppComponent implements OnInit  {

    constructor(private router: Router, private changeDetectorRef: ChangeDetectorRef, private globalSettings: GlobalSettingsService  ) { }

    //re-direct to widget stats on load
    ngOnInit() {
        this.router.navigate(['WidgetStats']);
    }
        

}



