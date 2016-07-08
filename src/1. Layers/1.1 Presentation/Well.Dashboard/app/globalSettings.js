System.register([], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var GlobalSettings;
    return {
        setters:[],
        execute: function() {
            GlobalSettings = (function () {
                function GlobalSettings() {
                    this.WellApiUrl = 'http://localhost/Well/Api/';
                }
                return GlobalSettings;
            }());
            exports_1("GlobalSettings", GlobalSettings);
        }
    }
});
//# sourceMappingURL=globalSettings.js.map