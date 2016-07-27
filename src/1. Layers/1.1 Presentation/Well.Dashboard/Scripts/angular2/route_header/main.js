"use strict";
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var core_1 = require("@angular/core");
var routeHeaderComponent_1 = require('./routeHeaderComponent');
function runApplication(config) {
    platform_browser_dynamic_1.bootstrap(routeHeaderComponent_1.RouteHeaderComponent, [core_1.provide('global.settings', { useValue: config })]);
}
exports.runApplication = runApplication;
