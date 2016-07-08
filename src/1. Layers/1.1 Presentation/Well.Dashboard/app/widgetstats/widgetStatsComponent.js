System.register(['angular2/core', 'angular2/router', './widgetstats-service'], function(exports_1, context_1) {
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
    var core_1, router_1, widgetstats_service_1;
    var WidgetStatsComponent;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (widgetstats_service_1_1) {
                widgetstats_service_1 = widgetstats_service_1_1;
            }],
        execute: function() {
            WidgetStatsComponent = (function () {
                function WidgetStatsComponent(widgetStatsService, router, routeParams, changeDetectorRef) {
                    this.widgetStatsService = widgetStatsService;
                    this.router = router;
                    this.routeParams = routeParams;
                    this.changeDetectorRef = changeDetectorRef;
                }
                WidgetStatsComponent.prototype.ngOnInit = function () {
                    this.getWidgetStats();
                    //this.widgetStatsService.autoUpdateDisabled()
                    //    .subscribe(isAutoUpdateDisabled => this.initSignalr(isAutoUpdateDisabled),
                    //        error => this.errorMessage = <any>error);
                    //    .subscribe(isAutoUpdateDisabled => this.initSignalr(isAutoUpdateDisabled),
                    //        error => this.errorMessage = <any>error);
                };
                WidgetStatsComponent.prototype.initSignalr = function (isAutoUpdateDisabled) {
                    var _this = this;
                    if (isAutoUpdateDisabled === true)
                        return; //We can get rid of this once signalr is using webSockets
                    var exceptionNotifications = $.connection.exceptionsHub;
                    exceptionNotifications.qs = { 'version': '1.0' };
                    exceptionNotifications.client.widgetExceptions = function () {
                        _this.getWidgetStats();
                    };
                    $.connection.hub.start().done(function (data) {
                        console.log("Hub started");
                    });
                };
                WidgetStatsComponent.prototype.handleExceptions = function (widgetstats) {
                    this.widgetstats = widgetstats;
                    //this.changeDetectorRef.detectChanges();
                };
                WidgetStatsComponent.prototype.getWidgetStats = function () {
                    var _this = this;
                    this.widgetStatsService.getWidgetStats()
                        .subscribe(function (widgetstats) { return _this.handleExceptions(widgetstats); }, function (error) { return _this.errorMessage = error; });
                };
                WidgetStatsComponent = __decorate([
                    core_1.Component({
                        templateUrl: './app/widgetstats/widgetstats.html'
                    }), 
                    __metadata('design:paramtypes', [widgetstats_service_1.WidgetStatsService, router_1.Router, router_1.RouteParams, core_1.ChangeDetectorRef])
                ], WidgetStatsComponent);
                return WidgetStatsComponent;
            }());
            exports_1("WidgetStatsComponent", WidgetStatsComponent);
        }
    }
});
//# sourceMappingURL=widgetStatsComponent.js.map