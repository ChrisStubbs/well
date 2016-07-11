System.register(['angular2/core', 'angular2/router', 'ng2-pagination', './ResolvedDeliveryService', './resolvedDeliveryFilterPipe', '../shared/globalSettings'], function(exports_1, context_1) {
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
    var core_1, router_1, ng2_pagination_1, ResolvedDeliveryService_1, resolvedDeliveryFilterPipe_1, globalSettings_1;
    var ResolvedDeliveryComponent;
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
            function (ResolvedDeliveryService_1_1) {
                ResolvedDeliveryService_1 = ResolvedDeliveryService_1_1;
            },
            function (resolvedDeliveryFilterPipe_1_1) {
                resolvedDeliveryFilterPipe_1 = resolvedDeliveryFilterPipe_1_1;
            },
            function (globalSettings_1_1) {
                globalSettings_1 = globalSettings_1_1;
            }],
        execute: function() {
            ResolvedDeliveryComponent = (function () {
                function ResolvedDeliveryComponent(resolvedDeliveryService) {
                    this.resolvedDeliveryService = resolvedDeliveryService;
                    this.rowCount = 10;
                }
                ResolvedDeliveryComponent.prototype.ngOnInit = function () {
                    var _this = this;
                    this.resolvedDeliveryService.getResolvedDeliveries()
                        .subscribe(function (deliveries) { return _this.deliveries = deliveries; }, function (error) { return _this.errorMessage = error; });
                };
                ResolvedDeliveryComponent.prototype.clearFilterText = function () {
                    this.filterText = '';
                };
                ResolvedDeliveryComponent.prototype.deliverySelected = function (delivery) {
                    console.log(delivery.accountName);
                };
                ResolvedDeliveryComponent.prototype.foo = function () {
                    console.log(this.filterText);
                };
                ResolvedDeliveryComponent = __decorate([
                    core_1.Component({
                        templateUrl: './app/resolved/resolveddelivery-list.html',
                        providers: [ResolvedDeliveryService_1.ResolvedDeliveryService, ng2_pagination_1.PaginationService, globalSettings_1.GlobalSettingsService],
                        directives: [router_1.ROUTER_DIRECTIVES, ng2_pagination_1.PaginationControlsCmp],
                        pipes: [ng2_pagination_1.PaginatePipe, resolvedDeliveryFilterPipe_1.ResolvedDeliveryFilterPipe]
                    }), 
                    __metadata('design:paramtypes', [ResolvedDeliveryService_1.ResolvedDeliveryService])
                ], ResolvedDeliveryComponent);
                return ResolvedDeliveryComponent;
            }());
            exports_1("ResolvedDeliveryComponent", ResolvedDeliveryComponent);
        }
    }
});
//# sourceMappingURL=resolved-deliveryComponent.js.map