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
var routeHeaderService_1 = require('./routeHeaderService');
var optionfilter_component_1 = require('../shared/optionfilter.component');
var optionFilterPipe_1 = require('../shared/optionFilterPipe');
var DropDownItem_1 = require("../shared/DropDownItem");
var Option = require("../shared/filterOption");
var FilterOption = Option.FilterOption;
var well_modal_1 = require("../shared/well-modal");
var RouteHeaderComponent = (function () {
    function RouteHeaderComponent(routerHeaderService) {
        this.routerHeaderService = routerHeaderService;
        this.rowCount = 10;
        this.lastRefresh = '01 january 1666 13:05';
        this.filterOption = new FilterOption();
        this.options = [
            new DropDownItem_1.DropDownItem("Route", "route"),
            new DropDownItem_1.DropDownItem("Account", "account", true),
            new DropDownItem_1.DropDownItem("Invoice", "invoice", true),
            new DropDownItem_1.DropDownItem("Assignee", "assignee", true)
        ];
        this.modal = new well_modal_1.WellModal();
    }
    RouteHeaderComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.routerHeaderService.getRouteHeaders()
            .subscribe(function (routes) { return _this.routes = routes; }, function (error) { return _this.errorMessage = error; });
    };
    RouteHeaderComponent.prototype.routeSelected = function (route) {
    };
    RouteHeaderComponent.prototype.onFilterClicked = function (filterOption) {
        var _this = this;
        if (filterOption.dropDownItem.requiresServerCall) {
            this.routerHeaderService.getRouteHeaders(filterOption.dropDownItem.value, filterOption.filterText)
                .subscribe(function (routes) { return _this.routes = routes; }, function (error) { return _this.errorMessage = error; });
        }
        else {
            this.filterOption = filterOption;
        }
    };
    __decorate([
        core_1.ViewChild(well_modal_1.WellModal), 
        __metadata('design:type', Object)
    ], RouteHeaderComponent.prototype, "modal", void 0);
    RouteHeaderComponent = __decorate([
        core_1.Component({
            selector: 'ow-routes',
            templateUrl: './app/route_header/routeheader-list.html',
            providers: [http_1.HTTP_PROVIDERS, globalSettings_1.GlobalSettingsService, routeHeaderService_1.RouteHeaderService, ng2_pagination_1.PaginationService],
            directives: [optionfilter_component_1.OptionFilterComponent, ng2_pagination_1.PaginationControlsCmp, well_modal_1.WellModal],
            pipes: [optionFilterPipe_1.OptionFilterPipe, ng2_pagination_1.PaginatePipe]
        }), 
        __metadata('design:paramtypes', [routeHeaderService_1.RouteHeaderService])
    ], RouteHeaderComponent);
    return RouteHeaderComponent;
}());
exports.RouteHeaderComponent = RouteHeaderComponent;
