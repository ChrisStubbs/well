"use strict";
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var core_1 = require("@angular/core");
var deliveryComponent_1 = require('./deliveryComponent');
function runApplication(config) {
    platform_browser_dynamic_1.bootstrap(deliveryComponent_1.DeliveryComponent, [core_1.provide('global.settings', { useValue: config })]);
}
exports.runApplication = runApplication;
