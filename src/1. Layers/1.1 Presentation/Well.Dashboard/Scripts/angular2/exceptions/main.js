"use strict";
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var core_1 = require("@angular/core");
var exceptionsComponent_1 = require('./exceptionsComponent');
function runApplication(config) {
    platform_browser_dynamic_1.bootstrap(exceptionsComponent_1.ExceptionsComponent, [core_1.provide('global.settings', { useValue: config })]);
}
exports.runApplication = runApplication;
