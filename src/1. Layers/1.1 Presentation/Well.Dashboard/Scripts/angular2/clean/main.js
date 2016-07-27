"use strict";
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var core_1 = require("@angular/core");
var cleanDeliveryComponent_1 = require('./cleanDeliveryComponent');
function runApplication(config) {
    platform_browser_dynamic_1.bootstrap(cleanDeliveryComponent_1.CleanDeliveryComponent, [core_1.provide('global.settings', { useValue: config })]);
}
exports.runApplication = runApplication;
