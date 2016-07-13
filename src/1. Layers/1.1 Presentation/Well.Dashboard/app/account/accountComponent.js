System.register(['@angular/core', './accountService'], function(exports_1, context_1) {
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
    var core_1, accountService_1;
    var AccountComponent;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (accountService_1_1) {
                accountService_1 = accountService_1_1;
            }],
        execute: function() {
            AccountComponent = (function () {
                function AccountComponent(accountService) {
                    this.accountService = accountService;
                }
                AccountComponent.prototype.ngOnInit = function () {
                    var _this = this;
                    this.accountService.getAccountByStopId()
                        .subscribe(function (account) { return _this.account = account; }, function (error) { return _this.errorMessage = error; });
                };
                AccountComponent = __decorate([
                    core_1.Component({
                        templateUrl: './app/account/accountModal.html',
                        providers: [accountService_1.AccountService]
                    }), 
                    __metadata('design:paramtypes', [accountService_1.AccountService])
                ], AccountComponent);
                return AccountComponent;
            }());
            exports_1("AccountComponent", AccountComponent);
        }
    }
});
//# sourceMappingURL=accountComponent.js.map