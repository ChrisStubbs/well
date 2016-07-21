System.register(['@angular/core', '@angular/http', '../shared/globalSettings', 'rxjs/Rx', 'ng2-pagination', './cleanDeliveryService', '../shared/optionfilter.component', '../shared/optionFilterPipe', "../shared/filterOption", "../shared/DropDownItem", "../shared/contact-modal", "../account/accountService"], function(exports_1, context_1) {
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
    var core_1, http_1, globalSettings_1, ng2_pagination_1, cleanDeliveryService_1, optionfilter_component_1, optionFilterPipe_1, filterOption_1, DropDownItem_1, contact_modal_1, accountService_1;
    var CleanDeliveryComponent;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (globalSettings_1_1) {
                globalSettings_1 = globalSettings_1_1;
            },
            function (_1) {},
            function (ng2_pagination_1_1) {
                ng2_pagination_1 = ng2_pagination_1_1;
            },
            function (cleanDeliveryService_1_1) {
                cleanDeliveryService_1 = cleanDeliveryService_1_1;
            },
            function (optionfilter_component_1_1) {
                optionfilter_component_1 = optionfilter_component_1_1;
            },
            function (optionFilterPipe_1_1) {
                optionFilterPipe_1 = optionFilterPipe_1_1;
            },
            function (filterOption_1_1) {
                filterOption_1 = filterOption_1_1;
            },
            function (DropDownItem_1_1) {
                DropDownItem_1 = DropDownItem_1_1;
            },
            function (contact_modal_1_1) {
                contact_modal_1 = contact_modal_1_1;
            },
            function (accountService_1_1) {
                accountService_1 = accountService_1_1;
            }],
        execute: function() {
            CleanDeliveryComponent = (function () {
                function CleanDeliveryComponent(cleanDeliveryService, accountService) {
                    this.cleanDeliveryService = cleanDeliveryService;
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
                    this.modal = new contact_modal_1.ContactModal();
                }
                CleanDeliveryComponent.prototype.ngOnInit = function () {
                    var _this = this;
                    this.cleanDeliveryService.getCleanDeliveries()
                        .subscribe(function (cleanDeliveries) { return _this.cleanDeliveries = cleanDeliveries; }, function (error) { return _this.errorMessage = error; });
                };
                CleanDeliveryComponent.prototype.onFilterClicked = function (filterOption) {
                    this.filterOption = filterOption;
                };
                CleanDeliveryComponent.prototype.deliverySelected = function (delivery) {
                    console.log(delivery.accountName);
                };
                CleanDeliveryComponent.prototype.openModal = function (accountId) {
                    var _this = this;
                    this.accountService.getAccountByAccountId(accountId)
                        .subscribe(function (account) { _this.account = account; _this.modal.show(_this.account); }, function (error) { return _this.errorMessage = error; });
                };
                __decorate([
                    core_1.ViewChild(contact_modal_1.ContactModal), 
                    __metadata('design:type', Object)
                ], CleanDeliveryComponent.prototype, "modal", void 0);
                CleanDeliveryComponent = __decorate([
                    core_1.Component({
                        selector: 'ow-clean',
                        templateUrl: './app/clean/cleanDelivery-list.html',
                        providers: [http_1.HTTP_PROVIDERS, globalSettings_1.GlobalSettingsService, cleanDeliveryService_1.CleanDeliveryService, ng2_pagination_1.PaginationService, accountService_1.AccountService],
                        directives: [optionfilter_component_1.OptionFilterComponent, ng2_pagination_1.PaginationControlsCmp, contact_modal_1.ContactModal],
                        pipes: [optionFilterPipe_1.OptionFilterPipe, ng2_pagination_1.PaginatePipe]
                    }), 
                    __metadata('design:paramtypes', [cleanDeliveryService_1.CleanDeliveryService, accountService_1.AccountService])
                ], CleanDeliveryComponent);
                return CleanDeliveryComponent;
            }());
            exports_1("CleanDeliveryComponent", CleanDeliveryComponent);
        }
    }
});
//# sourceMappingURL=cleanDeliveryComponent.js.map