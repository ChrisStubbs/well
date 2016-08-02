"use strict";
var router_1 = require('@angular/router');
var accountComponent_1 = require('./account/accountComponent');
var branchSelectionComponent_1 = require('./branch/branchSelectionComponent');
var cleanDeliveryComponent_1 = require('./clean/cleanDeliveryComponent');
var deliveryComponent_1 = require('./delivery/deliveryComponent');
var exceptionsComponent_1 = require('./exceptions/exceptionsComponent');
var notificationsComponent_1 = require('./notifications/notificationsComponent');
var resolved_deliveryComponent_1 = require('./resolved/resolved-deliveryComponent');
var userPreferenceComponent_1 = require('./user_preferences/userPreferenceComponent');
var routeHeaderComponent_1 = require('./route_header/routeHeaderComponent');
var widgetStatsComponent_1 = require('./home/widgetStatsComponent');
var routes = [
    { path: '', redirectTo: '/widgets', pathMatch: 'full' },
    { path: 'account', component: accountComponent_1.AccountComponent },
    { path: 'branch', component: branchSelectionComponent_1.BranchSelectionComponent },
    { path: 'clean', component: cleanDeliveryComponent_1.CleanDeliveryComponent },
    { path: 'delivery', component: deliveryComponent_1.DeliveryComponent },
    { path: 'exceptions', component: exceptionsComponent_1.ExceptionsComponent },
    { path: 'notifications', component: notificationsComponent_1.NotificationsComponent },
    { path: 'resolved', component: resolved_deliveryComponent_1.ResolvedDeliveryComponent },
    { path: 'routes', component: routeHeaderComponent_1.RouteHeaderComponent },
    { path: 'widgets', component: widgetStatsComponent_1.WidgetStatsComponent },
    { path: 'preferences', component: userPreferenceComponent_1.UserPreferenceComponent }
];
exports.appRouterProviders = [
    router_1.provideRouter(routes)
];
//# sourceMappingURL=appRoutes.js.map