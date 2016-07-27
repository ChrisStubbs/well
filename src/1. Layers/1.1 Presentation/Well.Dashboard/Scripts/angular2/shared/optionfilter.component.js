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
var core_1 = require("@angular/core");
var filterOption_1 = require("./filterOption");
var dropDownItem_1 = require("./dropDownItem");
var OptionFilterComponent = (function () {
    function OptionFilterComponent() {
        this.defaultOption = new dropDownItem_1.DropDownItem("Option", "");
        this.selectedOption = this.defaultOption;
        this.filterClicked = new core_1.EventEmitter();
    }
    OptionFilterComponent.prototype.ngOnChanges = function () {
        console.log("onchange");
    };
    OptionFilterComponent.prototype.clearFilterText = function () {
        this.filterText = '';
        this.selectedOption = this.defaultOption;
        this.applyFilter();
    };
    OptionFilterComponent.prototype.applyFilter = function () {
        this.filterClicked.emit(new filterOption_1.FilterOption(this.selectedOption, this.filterText));
    };
    OptionFilterComponent.prototype.setSelectedOption = function (option) {
        this.selectedOption = option;
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], OptionFilterComponent.prototype, "options", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', core_1.EventEmitter)
    ], OptionFilterComponent.prototype, "filterClicked", void 0);
    OptionFilterComponent = __decorate([
        core_1.Component({
            selector: "ow-optionfilter",
            templateUrl: "app/shared/optionfilter.component.html"
        }), 
        __metadata('design:paramtypes', [])
    ], OptionFilterComponent);
    return OptionFilterComponent;
}());
exports.OptionFilterComponent = OptionFilterComponent;
