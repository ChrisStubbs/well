"use strict";
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var core_1 = require("@angular/core");
var userPreferenceComponent_1 = require('./userPreferenceComponent');
function runApplication(config) {
    platform_browser_dynamic_1.bootstrap(userPreferenceComponent_1.UserPreferenceComponent, [core_1.provide('global.settings', { useValue: config })]);
}
exports.runApplication = runApplication;
