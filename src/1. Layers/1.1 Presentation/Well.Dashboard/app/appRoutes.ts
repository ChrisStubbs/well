﻿import { Routes, RouterModule } from '@angular/router';

import {AccountComponent} from './account/accountComponent';
import {AuditComponent} from './audit/auditComponent';
import {BranchSelectionComponent} from './branch/branchSelectionComponent';
import {CleanDeliveryComponent} from './clean/cleanDeliveryComponent';
import {DeliveryComponent} from './delivery/deliveryComponent';
import {DeliveryUpdateComponent} from './delivery/deliveryUpdateComponent';
import {ExceptionsComponent} from './exceptions/exceptionsComponent';
import {NotificationsComponent} from './notifications/notificationsComponent';
import {ResolvedDeliveryComponent} from './resolved/resolvedDeliveryComponent';
import {UserPreferenceComponent} from './user_preferences/userPreferenceComponent';
import {RouteHeaderComponent} from './route_header/routeHeaderComponent';
import {WidgetComponent} from './home/widgetComponent';
import {UnauthorisedComponent} from './unauthorised/unauthorisedComponent';
import {BranchPreferenceComponent} from './shared/branch/branchPreferenceComponent';

const routes: Routes = [
    { path: 'well/dashboard', redirectTo: '/widgets', pathMatch: 'full' }, //for chrome
    { path: '', redirectTo: '/widgets', pathMatch: 'full' },               //for IE
    { path: 'unauthorised', component: UnauthorisedComponent },
    { path: 'audits', component: AuditComponent },
    { path: 'account', component: AccountComponent },
    { path: 'branch', component: BranchSelectionComponent },
    { path: 'branch/:name/:domain', component: BranchSelectionComponent },
    { path: 'clean', component: CleanDeliveryComponent },
    { path: 'delivery/:id', component: DeliveryComponent },
    { path: 'delivery/:id/:line', component: DeliveryUpdateComponent },
    { path: 'exceptions', component: ExceptionsComponent },
    { path: 'notifications', component: NotificationsComponent },
    { path: 'resolved', component: ResolvedDeliveryComponent },
    { path: 'routes', component: RouteHeaderComponent },
    { path: 'widgets', component: WidgetComponent },
    { path: 'preferences', component: UserPreferenceComponent },
    { path: 'branch-preference', component: BranchPreferenceComponent }
];

export const appRoutingProviders: any[] = [

];

export const routing = RouterModule.forRoot(routes);
//export const appRouterProviders = RouterModule.forRoot(routes);