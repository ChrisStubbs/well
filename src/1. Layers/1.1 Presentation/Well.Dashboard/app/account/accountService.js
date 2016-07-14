System.register(['@angular/core', '@angular/http', 'rxjs/Observable', 'rxjs/add/operator/map', '../shared/globalSettings'], function(exports_1, context_1) {
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
    var core_1, http_1, Observable_1, globalSettings_1;
    var AccountService;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (Observable_1_1) {
                Observable_1 = Observable_1_1;
            },
            function (_1) {},
            function (globalSettings_1_1) {
                globalSettings_1 = globalSettings_1_1;
            }],
        execute: function() {
            AccountService = (function () {
                function AccountService(http, globalSettingsService) {
                    this.http = http;
                    this.globalSettingsService = globalSettingsService;
                }
                AccountService.prototype.getAccountByStopId = function () {
                    return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'account')
                        .map(function (response) { return response.json(); })
                        .do(function (data) { return console.log("All: " + JSON.stringify(data)); })
                        .catch(this.handleError);
                };
                AccountService.prototype.handleError = function (error) {
                    console.log(error);
                    return Observable_1.Observable.throw(error.json().error || 'Server error');
                };
                AccountService = __decorate([
                    core_1.Injectable(), 
                    __metadata('design:paramtypes', [(typeof (_a = typeof http_1.Http !== 'undefined' && http_1.Http) === 'function' && _a) || Object, globalSettings_1.GlobalSettingsService])
                ], AccountService);
                return AccountService;
                var _a;
            }());
            exports_1("AccountService", AccountService);
        }
    }
});
//# sourceMappingURL=accountService.js.map