System.register(['@angular/core', '@angular/http', 'rxjs/Rx', '@angular/router-deprecated', './widgetstats/widgetstats-service', './widgetstats/widgetStatsComponent', './route_header/routeHeaderComponent', './clean/cleanDeliveryComponent', './resolved/resolved-deliveryComponent', './notifications/notificationsComponent', './shared/globalSettings'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var core_1, http_1, router_deprecated_1, widgetstats_service_1, widgetStatsComponent_1, routeHeaderComponent_1, cleanDeliveryComponent_1, resolved_deliveryComponent_1, notificationsComponent_1, globalSettings_1;
    var AppComponent;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (_1) {},
            function (router_deprecated_1_1) {
                router_deprecated_1 = router_deprecated_1_1;
            },
            function (widgetstats_service_1_1) {
                widgetstats_service_1 = widgetstats_service_1_1;
            },
            function (widgetStatsComponent_1_1) {
                widgetStatsComponent_1 = widgetStatsComponent_1_1;
            },
            function (routeHeaderComponent_1_1) {
                routeHeaderComponent_1 = routeHeaderComponent_1_1;
            },
            function (cleanDeliveryComponent_1_1) {
                cleanDeliveryComponent_1 = cleanDeliveryComponent_1_1;
            },
            function (resolved_deliveryComponent_1_1) {
                resolved_deliveryComponent_1 = resolved_deliveryComponent_1_1;
            },
            function (notificationsComponent_1_1) {
                notificationsComponent_1 = notificationsComponent_1_1;
            },
            function (accountComponent_1_1) {
                accountComponent_1 = accountComponent_1_1;
            },
            function (globalSettings_1_1) {
                globalSettings_1 = globalSettings_1_1;
            }],
        execute: function() {
            AppComponent = (function () {
                function AppComponent(router, changeDetectorRef, globalSettings) {
                    this.router = router;
                    this.changeDetectorRef = changeDetectorRef;
                    this.globalSettings = globalSettings;
                }
                //re-direct to widget stats on load
                AppComponent.prototype.ngOnInit = function () {
                    this.router.navigate(['WidgetStats']);
                };
                AppComponent = __decorate([
                    core_1.Component({
                        selector: 'ow-app',
                        templateUrl: './app/main.html',
                        directives: [router_deprecated_1.ROUTER_DIRECTIVES],
                        providers: [widgetstats_service_1.WidgetStatsService, http_1.HTTP_PROVIDERS, router_deprecated_1.ROUTER_PROVIDERS, globalSettings_1.GlobalSettingsService]
                    }),
                    router_deprecated_1.RouteConfig([
                        { path: '/widgetstats', name: 'WidgetStats', component: widgetStatsComponent_1.WidgetStatsComponent, useAsDefault: true },
                        { path: '/routes', name: 'Routes', component: routeHeaderComponent_1.RouteHeaderComponent },
                        { path: '/clean', name: 'Clean', component: cleanDeliveryComponent_1.CleanDeliveryComponent },
                        { path: '/resolved', name: 'Resolved', component: resolved_deliveryComponent_1.ResolvedDeliveryComponent },
                        { path: '/notifications', name: 'Notifications', component: notificationsComponent_1.NotificationsComponent },
                        { path: '/account', name: 'Account', component: accountComponent_1.AccountComponent }
                    ]), 
                    __metadata('design:paramtypes', [router_deprecated_1.Router, core_1.ChangeDetectorRef, globalSettings_1.GlobalSettingsService])
                ], AppComponent);
                return AppComponent;
            }());
            exports_1("AppComponent", AppComponent);
        }
    }
});
//# sourceMappingURL=appComponent.js.map