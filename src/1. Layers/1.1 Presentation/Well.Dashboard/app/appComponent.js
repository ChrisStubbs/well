"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var http_1 = require('@angular/http');
//import { ROUTER_PROVIDERS, ROUTER_DIRECTIVES, RouteConfig } from "@angular2/router";
require('rxjs/Rx'); // Load all features
var accountComponent_1 = require('./account/accountComponent');
var branchSelectionComponent_1 = require('./branch/branchSelectionComponent');
var cleanDeliveryComponent_1 = require('./clean/cleanDeliveryComponent');
var deliveryComponent_1 = require('./delivery/deliveryComponent');
var exceptionsComponent_1 = require('./exceptions/exceptionsComponent');
var notificationsComponent_1 = require('./notifications/notificationsComponent');
var resolved_deliveryComponent_1 = require('./resolved/resolved-deliveryComponent');
var routeHeaderComponent_1 = require('./route_header/routeHeaderComponent');
var widgetStatsComponent_1 = require('./home/widgetStatsComponent');
var AppComponent = (function () {
    function AppComponent() {
    }
    AppComponent = __decorate([
        core_1.Component({
            selector: 'ow-app',
            template: "<ow-widgetstats></ow-widgetstats>\n                ",
            providers: [http_1.HTTP_PROVIDERS],
            directives: [accountComponent_1.AccountComponent, branchSelectionComponent_1.BranchSelectionComponent, cleanDeliveryComponent_1.CleanDeliveryComponent, deliveryComponent_1.DeliveryComponent,
                exceptionsComponent_1.ExceptionsComponent, notificationsComponent_1.NotificationsComponent, resolved_deliveryComponent_1.ResolvedDeliveryComponent, routeHeaderComponent_1.RouteHeaderComponent, widgetStatsComponent_1.WidgetStatsComponent]
        }), 
        __metadata('design:paramtypes', [])
    ], AppComponent);
    return AppComponent;
}());
exports.AppComponent = AppComponent;
//# sourceMappingURL=appComponent.js.map