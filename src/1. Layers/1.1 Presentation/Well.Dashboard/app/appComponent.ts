import { Component, OnInit, ChangeDetectorRef}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
//import { ROUTER_PROVIDERS, ROUTER_DIRECTIVES, RouteConfig } from "@angular2/router";
import 'rxjs/Rx';   // Load all features

import {AccountComponent} from './account/accountComponent';
import {BranchSelectionComponent} from './branch/branchSelectionComponent';
import {CleanDeliveryComponent} from './clean/cleanDeliveryComponent';
import {DeliveryComponent} from './delivery/deliveryComponent';
import {ExceptionsComponent} from './exceptions/exceptionsComponent';
import {NotificationsComponent} from './notifications/notificationsComponent';
import {ResolvedDeliveryComponent} from './resolved/resolved-deliveryComponent';
import {RouteHeaderComponent} from './route_header/routeHeaderComponent';
import {WidgetStatsComponent} from './home/widgetStatsComponent';

@Component({
    selector: 'ow-app',
    template: `<ow-widgetstats></ow-widgetstats>
                `,
    providers: [HTTP_PROVIDERS],
    directives: [AccountComponent, BranchSelectionComponent, CleanDeliveryComponent, DeliveryComponent,
        ExceptionsComponent, NotificationsComponent, ResolvedDeliveryComponent, RouteHeaderComponent, WidgetStatsComponent]
})
// TODO - Implement routing to switch between components
export class AppComponent {
    
}