"use strict";
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var core_1 = require("@angular/core");
var resolved_deliveryComponent_1 = require('./resolved-deliveryComponent');
function runApplication(config) {
    platform_browser_dynamic_1.bootstrap(resolved_deliveryComponent_1.ResolvedDeliveryComponent, [core_1.provide('global.settings', { useValue: config })]);
}
exports.runApplication = runApplication;
