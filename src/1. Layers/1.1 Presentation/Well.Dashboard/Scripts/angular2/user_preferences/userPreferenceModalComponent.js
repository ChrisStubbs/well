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
var UserPreferenceModal = (function () {
    function UserPreferenceModal() {
        this.isVisible = false;
    }
    UserPreferenceModal.prototype.show = function (user) {
        this.user = user;
        this.isVisible = true;
    };
    UserPreferenceModal.prototype.hide = function () {
        this.isVisible = false;
    };
    UserPreferenceModal.prototype.setBranches = function () {
        window.location.href = encodeURI('./user-preferences/branches/' + this.user.friendlyName + '/' + this.user.domain);
    };
    UserPreferenceModal = __decorate([
        core_1.Component({
            selector: 'ow-user-preference-modal',
            templateUrl: './app/user_preferences/user-preference-modal.html'
        }), 
        __metadata('design:paramtypes', [])
    ], UserPreferenceModal);
    return UserPreferenceModal;
}());
exports.UserPreferenceModal = UserPreferenceModal;
