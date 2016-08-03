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
var http_1 = require('@angular/http');
var Observable_1 = require('rxjs/Observable');
var GlobalSettingsService = (function () {
    function GlobalSettingsService(_http) {
        this._http = _http;
        var configuredApiUrl = "#{OrderWellApi}"; //This variable can be replaced by Octopus during deployment :)
        this.globalSettings =
            {
                apiUrl: (configuredApiUrl[0] !== "#") ? configuredApiUrl : "http://localhost/well/api/"
            };
    }
    GlobalSettingsService.prototype.getVersion = function () {
        return this._http.get(this.globalSettings.apiUrl + 'version')
            .map(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    GlobalSettingsService.prototype.getBranches = function () {
        return this._http.get(this.globalSettings.apiUrl + 'user-branches')
            .map(function (response) { return response.json(); })
            .catch(this.handleError);
    };
    GlobalSettingsService.prototype.handleError = function (error) {
        console.log(error);
        return Observable_1.Observable.throw(error.json().error || 'Server error');
    };
    GlobalSettingsService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], GlobalSettingsService);
    return GlobalSettingsService;
}());
exports.GlobalSettingsService = GlobalSettingsService;
//# sourceMappingURL=globalSettings.js.map