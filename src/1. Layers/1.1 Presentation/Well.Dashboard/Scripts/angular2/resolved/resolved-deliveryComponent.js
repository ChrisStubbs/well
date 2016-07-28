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
var ResolvedDeliveryService_1 = require('./ResolvedDeliveryService');
var optionfilter_component_1 = require('../shared/optionfilter.component');
var optionFilterPipe_1 = require('../shared/optionFilterPipe');
var DropDownItem_1 = require("../shared/DropDownItem");
var Option = require("../shared/filterOption");
var FilterOption = Option.FilterOption;
var contact_modal_1 = require("../shared/contact-modal");
var accountService_1 = require("../account/accountService");
var ResolvedDeliveryComponent = (function () {
    function ResolvedDeliveryComponent(resolvedDeliveryService, accountService) {
        this.resolvedDeliveryService = resolvedDeliveryService;
        this.accountService = accountService;
        this.rowCount = 10;
        this.filterOption = new FilterOption();
        this.options = [
            new DropDownItem_1.DropDownItem("Route", "routeNumber"),
            new DropDownItem_1.DropDownItem("Drop", "dropId"),
            new DropDownItem_1.DropDownItem("Invoice No", "invoiceNumber"),
            new DropDownItem_1.DropDownItem("Account", "accountCode"),
            new DropDownItem_1.DropDownItem("Account Name", "accountName"),
            new DropDownItem_1.DropDownItem("Status", "jobStatus"),
            new DropDownItem_1.DropDownItem("Action", "action"),
            new DropDownItem_1.DropDownItem("Assigned", "assigned"),
            new DropDownItem_1.DropDownItem("Date", "dateTime")
        ];
        this.modal = new contact_modal_1.ContactModal();
    }
    ResolvedDeliveryComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.resolvedDeliveryService.getResolvedDeliveries()
            .subscribe(function (deliveries) { return _this.deliveries = deliveries; }, function (error) { return _this.errorMessage = error; });
    };
    ResolvedDeliveryComponent.prototype.deliverySelected = function (delivery) {
        window.location.href = './Resolved/Delivery/' + delivery.id;
    };
    ResolvedDeliveryComponent.prototype.onFilterClicked = function (filterOption) {
        this.filterOption = filterOption;
    };
    ResolvedDeliveryComponent.prototype.openModal = function (accountId, event) {
        var _this = this;
        event.stopPropagation();
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(function (account) { _this.account = account; _this.modal.show(_this.account); }, function (error) { return _this.errorMessage = error; });
    };
    __decorate([
        core_1.ViewChild(contact_modal_1.ContactModal), 
        __metadata('design:type', Object)
    ], ResolvedDeliveryComponent.prototype, "modal", void 0);
    ResolvedDeliveryComponent = __decorate([
        core_1.Component({
            selector: 'ow-resolved',
            templateUrl: './app/resolved/resolveddelivery-list.html',
            providers: [http_1.HTTP_PROVIDERS, globalSettings_1.GlobalSettingsService, ResolvedDeliveryService_1.ResolvedDeliveryService, ng2_pagination_1.PaginationService, accountService_1.AccountService],
            directives: [optionfilter_component_1.OptionFilterComponent, ng2_pagination_1.PaginationControlsCmp, contact_modal_1.ContactModal],
            pipes: [optionFilterPipe_1.OptionFilterPipe, ng2_pagination_1.PaginatePipe]
        }), 
        __metadata('design:paramtypes', [ResolvedDeliveryService_1.ResolvedDeliveryService, accountService_1.AccountService])
    ], ResolvedDeliveryComponent);
    return ResolvedDeliveryComponent;
}());
exports.ResolvedDeliveryComponent = ResolvedDeliveryComponent;
