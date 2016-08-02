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
var router_1 = require('@angular/router');
var globalSettings_1 = require('./shared/globalSettings');
require('rxjs/Rx'); // Load all features
var AppComponent = (function () {
    function AppComponent(globalSettingsService) {
        var _this = this;
        this.globalSettingsService = globalSettingsService;
        this.version = "";
        this.branches = "";
        this.globalSettingsService.getVersion().subscribe(function (version) { return _this.version = version; });
        this.globalSettingsService.getBranches().subscribe(function (branches) { return _this.branches = branches; });
    }
    AppComponent = __decorate([
        // Load all features
        core_1.Component({
            selector: 'ow-app',
            templateUrl: 'home/applayout',
            providers: [http_1.HTTP_PROVIDERS, globalSettings_1.GlobalSettingsService],
            directives: [router_1.ROUTER_DIRECTIVES]
        }), 
        __metadata('design:paramtypes', [globalSettings_1.GlobalSettingsService])
    ], AppComponent);
    return AppComponent;
}());
exports.AppComponent = AppComponent;
//# sourceMappingURL=appComponent.js.map