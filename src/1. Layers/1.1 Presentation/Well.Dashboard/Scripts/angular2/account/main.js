"use strict";
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var core_1 = require("@angular/core");
var accountComponent_1 = require('./accountComponent');
function runApplication(config) {
    platform_browser_dynamic_1.bootstrap(accountComponent_1.AccountComponent, [core_1.provide('global.settings', { useValue: config })]);
}
exports.runApplication = runApplication;
