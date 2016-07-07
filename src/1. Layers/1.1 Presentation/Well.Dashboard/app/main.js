System.register(['angular2/platform/browser', './globalSettings', './appComponent'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var browser_1, globalSettings_1, appComponent_1;
    return {
        setters:[
            function (browser_1_1) {
                browser_1 = browser_1_1;
            },
            function (globalSettings_1_1) {
                globalSettings_1 = globalSettings_1_1;
            },
            function (appComponent_1_1) {
                appComponent_1 = appComponent_1_1;
            }],
        execute: function() {
            browser_1.bootstrap(appComponent_1.AppComponent, [globalSettings_1.GlobalSettings]);
        }
    }
});
//# sourceMappingURL=main.js.map