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
var userPreferenceService_1 = require('./userPreferenceService');
var ng2_pagination_1 = require('ng2-pagination');
var userPreferenceModalComponent_1 = require('./userPreferenceModalComponent');
var UserPreferenceComponent = (function () {
    function UserPreferenceComponent(userPreferenceService) {
        this.userPreferenceService = userPreferenceService;
        this.users = [];
        this.rowCount = 10;
        this.modal = new userPreferenceModalComponent_1.UserPreferenceModal();
    }
    UserPreferenceComponent.prototype.find = function () {
        var _this = this;
        this.userPreferenceService.getUsers(this.userText)
            .subscribe(function (users) { return _this.users = users; });
    };
    UserPreferenceComponent.prototype.userSelected = function (user) {
        this.modal.show(user);
    };
    __decorate([
        core_1.ViewChild(userPreferenceModalComponent_1.UserPreferenceModal), 
        __metadata('design:type', Object)
    ], UserPreferenceComponent.prototype, "modal", void 0);
    UserPreferenceComponent = __decorate([
        core_1.Component({
            selector: 'ow-user-preferences',
            templateUrl: './app/user_preferences/user-preferences.html',
            providers: [http_1.HTTP_PROVIDERS, userPreferenceService_1.UserPreferenceService, globalSettings_1.GlobalSettingsService, ng2_pagination_1.PaginationService],
            directives: [ng2_pagination_1.PaginationControlsCmp, userPreferenceModalComponent_1.UserPreferenceModal],
            pipes: [ng2_pagination_1.PaginatePipe]
        }), 
        __metadata('design:paramtypes', [userPreferenceService_1.UserPreferenceService])
    ], UserPreferenceComponent);
    return UserPreferenceComponent;
}());
exports.UserPreferenceComponent = UserPreferenceComponent;
