System.register(['angular2/core', 'angular2/router', 'ng2-pagination', './routeHeaderService', './routeFilterPipe', '../shared/globalSettings'], function(exports_1, context_1) {
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
    var core_1, router_1, ng2_pagination_1, routeHeaderService_1, routeFilterPipe_1, globalSettings_1;
    var RouteHeaderComponent;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (ng2_pagination_1_1) {
                ng2_pagination_1 = ng2_pagination_1_1;
            },
            function (routeHeaderService_1_1) {
                routeHeaderService_1 = routeHeaderService_1_1;
            },
            function (routeFilterPipe_1_1) {
                routeFilterPipe_1 = routeFilterPipe_1_1;
            },
            function (globalSettings_1_1) {
                globalSettings_1 = globalSettings_1_1;
            }],
        execute: function() {
            RouteHeaderComponent = (function () {
                function RouteHeaderComponent(routerHeaderService) {
                    this.routerHeaderService = routerHeaderService;
                    this.rowCount = 10;
                }
                RouteHeaderComponent.prototype.ngOnInit = function () {
                    var _this = this;
                    this.routerHeaderService.getRouteHeaders()
                        .subscribe(function (routes) { return _this.routes = routes; }, function (error) { return _this.errorMessage = error; });
                };
                RouteHeaderComponent.prototype.routeSelected = function (route) {
                    console.log(route.driverName);
                };
                RouteHeaderComponent.prototype.clearFilterText = function () {
                    this.filterText = '';
                };
                RouteHeaderComponent.prototype.foo = function () {
                    console.log(this.filterText);
                };
                RouteHeaderComponent = __decorate([
                    core_1.Component({
                        templateUrl: './app/route_header/routeheader-list.html',
                        providers: [routeHeaderService_1.RouteHeaderService, ng2_pagination_1.PaginationService, globalSettings_1.GlobalSettingsService],
                        directives: [router_1.ROUTER_DIRECTIVES, ng2_pagination_1.PaginationControlsCmp],
                        pipes: [ng2_pagination_1.PaginatePipe, routeFilterPipe_1.RouteFilterPipe]
                    }), 
                    __metadata('design:paramtypes', [routeHeaderService_1.RouteHeaderService])
                ], RouteHeaderComponent);
                return RouteHeaderComponent;
            }());
            exports_1("RouteHeaderComponent", RouteHeaderComponent);
        }
    }
});
//# sourceMappingURL=routeHeaderComponent.js.map