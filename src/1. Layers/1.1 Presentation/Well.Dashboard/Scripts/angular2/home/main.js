"use strict";
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var core_1 = require("@angular/core");
var widgetStatsComponent_1 = require('./widgetStatsComponent');
function runApplication(config) {
    platform_browser_dynamic_1.bootstrap(widgetStatsComponent_1.WidgetStatsComponent, [core_1.provide('global.settings', { useValue: config })]);
}
exports.runApplication = runApplication;
