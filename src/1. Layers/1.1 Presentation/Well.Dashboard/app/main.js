System.register(['@angular/platform-browser-dynamic', "@angular/core", 'angular2-modal/platform-browser/index', './appComponent'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var platform_browser_dynamic_1, core_1, index_1, appComponent_1;
    function runApplication(config) {
        platform_browser_dynamic_1.bootstrap(appComponent_1.AppComponent, [core_1.provide('global.settings', { useValue: config }), index_1.MODAL_BROWSER_PROVIDERS]);
    }
    exports_1("runApplication", runApplication);
    return {
        setters:[
            function (platform_browser_dynamic_1_1) {
                platform_browser_dynamic_1 = platform_browser_dynamic_1_1;
            },
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (index_1_1) {
                index_1 = index_1_1;
            },
            function (appComponent_1_1) {
                appComponent_1 = appComponent_1_1;
            }],
        execute: function() {
        }
    }
});
//# sourceMappingURL=main.js.map