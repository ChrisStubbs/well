import { provideRouter, RouterConfig } from '@angular/router';

import {AccountComponent} from './account/accountComponent';
import {BranchSelectionComponent} from './branch/branchSelectionComponent';
import {CleanDeliveryComponent} from './clean/cleanDeliveryComponent';
import {DeliveryComponent} from './delivery/deliveryComponent';
import {ExceptionsComponent} from './exceptions/exceptionsComponent';
import {NotificationsComponent} from './notifications/notificationsComponent';
import {ResolvedDeliveryComponent} from './resolved/resolved-deliveryComponent';
import {UserPreferenceComponent} from './user_preferences/userPreferenceComponent';
import {RouteHeaderComponent} from './route_header/routeHeaderComponent';
import {WidgetStatsComponent} from './home/widgetStatsComponent';

const routes: RouterConfig = [
    { path: '', redirectTo: '/widgets', pathMatch: 'full' },
    { path: 'account', component: AccountComponent },
    { path: 'branch', component: BranchSelectionComponent },
    { path: 'clean', component: CleanDeliveryComponent },
    { path: 'delivery', component: DeliveryComponent },
    { path: 'exceptions', component: ExceptionsComponent },
    { path: 'notifications', component: NotificationsComponent },
    { path: 'resolved', component: ResolvedDeliveryComponent },
    { path: 'routes', component: RouteHeaderComponent },
    { path: 'widgets', component: WidgetStatsComponent },
    { path: 'preferences', component: UserPreferenceComponent }
];

export const appRouterProviders = [
    provideRouter(routes)
];