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
var globalSettings_1 = require('../shared/globalSettings');
require('rxjs/Rx'); // Load all features
var ng2_pagination_1 = require('ng2-pagination');
var delivery_1 = require("./delivery");
var deliveryService_1 = require("./deliveryService");
var exceptionsFilterPipe_1 = require("./exceptionsFilterPipe");
var DropDownItem_1 = require("../shared/DropDownItem");
var DeliveryComponent = (function () {
    function DeliveryComponent(deliveryService, globalSettingsService) {
        this.deliveryService = deliveryService;
        this.globalSettingsService = globalSettingsService;
        this.delivery = new delivery_1.Delivery();
        this.rowCount = 10;
        this.showAll = false;
        this.options = [
            new DropDownItem_1.DropDownItem("Exceptions", "isException"),
            new DropDownItem_1.DropDownItem("Line", "lineNo"),
            new DropDownItem_1.DropDownItem("Product", "productCode"),
            new DropDownItem_1.DropDownItem("Description", "productDescription"),
            new DropDownItem_1.DropDownItem("Reason", "reason"),
            new DropDownItem_1.DropDownItem("Status", "status"),
            new DropDownItem_1.DropDownItem("Action", "action")
        ];
    }
    DeliveryComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.deliveryService.getDelivery(this.globalSettingsService.globalSettings.deliveryId)
            .subscribe(function (delivery) { _this.delivery = delivery; console.log(_this.delivery.id); }, function (error) { return _this.errorMessage = error; });
    };
    DeliveryComponent.prototype.onShowAllClicked = function () {
        this.showAll = !this.showAll;
    };
    DeliveryComponent = __decorate([
        core_1.Component({
            selector: 'ow-delivery',
            templateUrl: './app/delivery/delivery.html',
            providers: [http_1.HTTP_PROVIDERS, globalSettings_1.GlobalSettingsService, deliveryService_1.DeliveryService, ng2_pagination_1.PaginationService],
            directives: [ng2_pagination_1.PaginationControlsCmp],
            pipes: [exceptionsFilterPipe_1.ExceptionsFilterPipe, ng2_pagination_1.PaginatePipe]
        }), 
        __metadata('design:paramtypes', [deliveryService_1.DeliveryService, globalSettings_1.GlobalSettingsService])
    ], DeliveryComponent);
    return DeliveryComponent;
}());
exports.DeliveryComponent = DeliveryComponent;
