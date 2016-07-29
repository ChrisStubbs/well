"use strict";
var router_1 = require('@angular/router');
var routeHeaderComponent_1 = require('./route_header/routeHeaderComponent');
var widgetStatsComponent_1 = require('./home/widgetStatsComponent');
var routes = [
    { path: 'widgets', component: widgetStatsComponent_1.WidgetStatsComponent },
    { path: 'routes', component: routeHeaderComponent_1.RouteHeaderComponent }
];
exports.appRouterProviders = [
    router_1.provideRouter(routes)
];
//# sourceMappingURL=appRouter.js.map