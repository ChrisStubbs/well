"use strict";
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var core_1 = require("@angular/core");
var notificationsComponent_1 = require('./notificationsComponent');
function runApplication(config) {
    platform_browser_dynamic_1.bootstrap(notificationsComponent_1.NotificationsComponent, [core_1.provide('global.settings', { useValue: config })]);
}
exports.runApplication = runApplication;
