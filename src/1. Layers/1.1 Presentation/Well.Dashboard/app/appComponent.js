System.register(['angular2/core', 'angular2/http', 'rxjs/Rx', './route_exceptions/route-exception.service'], function(exports_1, context_1) {
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
    var core_1, http_1, route_exception_service_1;
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
            function (route_exception_service_1_1) {
                route_exception_service_1 = route_exception_service_1_1;
            }],
        execute: function() {
            AppComponent = (function () {
                function AppComponent(changeDetectorRef, routeExceptionService) {
                    this.changeDetectorRef = changeDetectorRef;
                    this.routeExceptionService = routeExceptionService;
                }
                AppComponent.prototype.ngOnInit = function () {
                    var self = this;
                    this.getExceptions();
                    var exceptionNotifications = $.connection.exceptionsHub;
                    exceptionNotifications.qs = { 'version': '1.0' };
                    exceptionNotifications.client.widgetExceptions = function () {
                        console.log("widgetExceptions triggered");
                        self.getExceptions();
                    };
                    $.connection.hub.start().done(function (data) {
                        console.log("Hub Started");
                    });
                };
                AppComponent.prototype.handleExceptions = function (exception) {
                    this.exception = exception;
                    this.changeDetectorRef.detectChanges();
                };
                AppComponent.prototype.getExceptions = function () {
                    var _this = this;
                    this.routeExceptionService.getExceptions()
                        .subscribe(function (response) { return _this.handleExceptions(response); }, function (error) { return _this.errorMessage = error; });
                };
                AppComponent = __decorate([
                    core_1.Component({
                        selector: 'ow-app',
                        templateUrl: './app/main.html',
                        providers: [route_exception_service_1.RouteExceptionService, http_1.HTTP_PROVIDERS]
                    }), 
                    __metadata('design:paramtypes', [core_1.ChangeDetectorRef, route_exception_service_1.RouteExceptionService])
                ], AppComponent);
                return AppComponent;
            }());
            exports_1("AppComponent", AppComponent);
        }
    }
});
//# sourceMappingURL=appComponent.js.map