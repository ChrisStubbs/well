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
var branchService_1 = require('./branchService');
var http_response_1 = require('../shared/http-response');
var angular2_toaster_1 = require('angular2-toaster/angular2-toaster');
var BranchSelectionComponent = (function () {
    function BranchSelectionComponent(branchService, toasterService) {
        this.branchService = branchService;
        this.toasterService = toasterService;
        this.selectedBranches = [];
        this.httpResponse = new http_response_1.HttpResponse();
    }
    BranchSelectionComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.selectAllCheckbox = false;
        this.branchService.getBranches()
            .subscribe(function (branches) {
            _this.branches = branches;
            _this.branches.forEach(function (branch) { if (branch.selected)
                _this.selectedBranches.push(branch); });
            if (_this.branches.every(function (x) { return x.selected; }))
                _this.selectAllCheckbox = true;
        }, function (error) { return _this.errorMessage = error; });
    };
    BranchSelectionComponent.prototype.selectAll = function () {
        var _this = this;
        var selected = !this.selectAllCheckbox;
        this.branches.forEach(function (branch) {
            var index = _this.selectedBranches.indexOf(branch, 0);
            if (index > -1)
                _this.selectedBranches.splice(index, 1);
            branch.selected = selected;
            if (selected) {
                _this.selectedBranches.push(branch);
            }
            else {
                _this.selectedBranches = [];
            }
        });
    };
    BranchSelectionComponent.prototype.selectBranch = function (branch) {
        var index = this.selectedBranches.indexOf(branch, 0);
        var selected = !branch.selected;
        if (index > -1 && selected === false) {
            this.selectedBranches.splice(index, 1);
        }
        else {
            this.selectedBranches.push(branch);
        }
        if (this.selectedBranches.length > 1 && this.selectedBranches.length === this.branches.length) {
            this.selectAllCheckbox = true;
        }
        else {
            this.selectAllCheckbox = false;
        }
    };
    BranchSelectionComponent.prototype.save = function () {
        var _this = this;
        this.branchService.saveBranches(this.selectedBranches)
            .subscribe(function (res) {
            _this.httpResponse = JSON.parse(JSON.stringify(res));
            if (_this.httpResponse.success)
                _this.toasterService.pop('success', 'Branches have been saved!', '');
            if (_this.httpResponse.failure)
                _this.toasterService.pop('error', 'Branches could not be saved at this time!', 'Please try again later!');
            if (_this.httpResponse.notAcceptable)
                _this.toasterService.pop('warning', 'Please select at least one branch!', '');
        });
    };
    BranchSelectionComponent = __decorate([
        core_1.Component({
            selector: 'ow-branch',
            templateUrl: './app/branch/branch-list.html',
            directives: [angular2_toaster_1.ToasterContainerComponent],
            providers: [http_1.HTTP_PROVIDERS, globalSettings_1.GlobalSettingsService, branchService_1.BranchService, angular2_toaster_1.ToasterService]
        }), 
        __metadata('design:paramtypes', [branchService_1.BranchService, angular2_toaster_1.ToasterService])
    ], BranchSelectionComponent);
    return BranchSelectionComponent;
}());
exports.BranchSelectionComponent = BranchSelectionComponent;
