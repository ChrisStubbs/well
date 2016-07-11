System.register(['angular2/platform/browser', "angular2/core", './shared/globalSettings', './appComponent'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var browser_1, core_1, globalSettings_1, appComponent_1;
    function runApplication(config) {
        var globalSettings = new globalSettings_1.GlobalSettings();
        globalSettings.apiUrl = config.apiUrl;
        browser_1.bootstrap(appComponent_1.AppComponent, [core_1.provide('global.settings', { useValue: globalSettings })]);
    }
    exports_1("runApplication", runApplication);
    return {
        setters:[
            function (browser_1_1) {
                browser_1 = browser_1_1;
            },
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (globalSettings_1_1) {
                globalSettings_1 = globalSettings_1_1;
            },
            function (appComponent_1_1) {
                appComponent_1 = appComponent_1_1;
            }],
        execute: function() {
        }
    }
});
//# sourceMappingURL=main.js.map