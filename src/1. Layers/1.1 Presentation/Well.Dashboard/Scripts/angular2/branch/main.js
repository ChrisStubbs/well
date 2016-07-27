"use strict";
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var core_1 = require("@angular/core");
var branchSelectionComponent_1 = require('./branchSelectionComponent');
function runApplication(config) {
    platform_browser_dynamic_1.bootstrap(branchSelectionComponent_1.BranchSelectionComponent, [core_1.provide('global.settings', { useValue: config })]);
}
exports.runApplication = runApplication;
