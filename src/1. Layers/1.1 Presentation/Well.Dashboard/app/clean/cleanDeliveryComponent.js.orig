System.register(['angular2/core', 'angular2/router', './cleanDeliveryService'], function(exports_1, context_1) {
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
    var core_1, router_1, cleanDeliveryService_1;
    var CleanDeliveryComponent;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (cleanDeliveryService_1_1) {
                cleanDeliveryService_1 = cleanDeliveryService_1_1;
            }],
        execute: function() {
            CleanDeliveryComponent = (function () {
                function CleanDeliveryComponent(cleanDeliveryService) {
                    this.cleanDeliveryService = cleanDeliveryService;
                }
                CleanDeliveryComponent.prototype.ngOnInit = function () {
                    var _this = this;
                    this.cleanDeliveryService.getCleanDeliveries()
                        .subscribe(function (cleanDeliveries) { return _this.cleanDeliveries = cleanDeliveries; }, function (error) { return _this.errorMessage = error; });
                };
                CleanDeliveryComponent = __decorate([
                    core_1.Component({
                        templateUrl: './app/clean/cleanDelivery-list.html',
                        providers: [cleanDeliveryService_1.CleanDeliveryService],
                        directives: [router_1.ROUTER_DIRECTIVES]
                    }), 
                    __metadata('design:paramtypes', [cleanDeliveryService_1.CleanDeliveryService])
                ], CleanDeliveryComponent);
                return CleanDeliveryComponent;
            }());
            exports_1("CleanDeliveryComponent", CleanDeliveryComponent);
        }
    }
});
//# sourceMappingURL=cleanDeliveryComponent.js.map