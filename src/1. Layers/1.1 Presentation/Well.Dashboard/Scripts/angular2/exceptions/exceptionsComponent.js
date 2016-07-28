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
var optionfilter_component_1 = require('../shared/optionfilter.component');
var optionFilterPipe_1 = require('../shared/optionFilterPipe');
var filterOption_1 = require("../shared/filterOption");
var DropDownItem_1 = require("../shared/DropDownItem");
var contact_modal_1 = require("../shared/contact-modal");
var accountService_1 = require("../account/accountService");
var exceptionDeliveryService_1 = require("./exceptionDeliveryService");
var ExceptionsComponent = (function () {
    function ExceptionsComponent(exceptionDeliveryService, accountService) {
        this.exceptionDeliveryService = exceptionDeliveryService;
        this.accountService = accountService;
        this.rowCount = 10;
        this.filterOption = new filterOption_1.FilterOption();
        this.options = [
            new DropDownItem_1.DropDownItem("Route", "routeNumber"),
            new DropDownItem_1.DropDownItem("Drop", "dropId"),
            new DropDownItem_1.DropDownItem("Invoice No", "invoiceNumber"),
            new DropDownItem_1.DropDownItem("Account", "accountCode"),
            new DropDownItem_1.DropDownItem("Account Name", "accountName"),
            new DropDownItem_1.DropDownItem("Date", "dateTime")
        ];
        this.defaultAction = new DropDownItem_1.DropDownItem("Action");
        this.actions = [
            new DropDownItem_1.DropDownItem("Assign", "#"),
            new DropDownItem_1.DropDownItem("Credit", "#"),
            new DropDownItem_1.DropDownItem("Credit and Re-Order", "#"),
            new DropDownItem_1.DropDownItem("Re-Plan", "#"),
            new DropDownItem_1.DropDownItem("Future Re-plan", "#"),
            new DropDownItem_1.DropDownItem("No Action", "#")
        ];
        this.modal = new contact_modal_1.ContactModal();
    }
    ExceptionsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.exceptionDeliveryService.getExceptions()
            .subscribe(function (exceptions) { return _this.exceptions = exceptions; }, function (error) { return _this.errorMessage = error; });
    };
    ExceptionsComponent.prototype.onFilterClicked = function (filterOption) {
        this.filterOption = filterOption;
    };
    ExceptionsComponent.prototype.deliverySelected = function (delivery) {
        window.location.href = './Exceptions/Delivery/' + delivery.id;
    };
    ExceptionsComponent.prototype.openModal = function (accountId, event) {
        var _this = this;
        event.stopPropagation();
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(function (account) { _this.account = account; _this.modal.show(_this.account); }, function (error) { return _this.errorMessage = error; });
    };
    ExceptionsComponent.prototype.setSelectedAction = function (delivery, action) {
        delivery.action = action.description;
    };
    __decorate([
        core_1.ViewChild(contact_modal_1.ContactModal), 
        __metadata('design:type', Object)
    ], ExceptionsComponent.prototype, "modal", void 0);
    ExceptionsComponent = __decorate([
        core_1.Component({
            selector: 'ow-exceptions',
            templateUrl: './app/exceptions/exceptions-list.html',
            providers: [http_1.HTTP_PROVIDERS, globalSettings_1.GlobalSettingsService, exceptionDeliveryService_1.ExceptionDeliveryService, ng2_pagination_1.PaginationService, accountService_1.AccountService],
            directives: [optionfilter_component_1.OptionFilterComponent, ng2_pagination_1.PaginationControlsCmp, contact_modal_1.ContactModal],
            pipes: [optionFilterPipe_1.OptionFilterPipe, ng2_pagination_1.PaginatePipe]
        }), 
        __metadata('design:paramtypes', [exceptionDeliveryService_1.ExceptionDeliveryService, accountService_1.AccountService])
    ], ExceptionsComponent);
    return ExceptionsComponent;
}());
exports.ExceptionsComponent = ExceptionsComponent;
