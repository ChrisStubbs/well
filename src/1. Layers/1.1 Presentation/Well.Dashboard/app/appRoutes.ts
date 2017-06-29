import { Routes, RouterModule } from '@angular/router';
import {AccountComponent} from './account/accountComponent';
import {ApprovalsComponent} from './approvals/approvalsComponent';
import {AuditComponent} from './audit/auditComponent';
import {BranchSelectionComponent} from './branch/branchSelectionComponent';
import {CleanDeliveryComponent} from './clean/cleanDeliveryComponent';
import {DeliveryComponent} from './delivery/deliveryComponent';
import {DeliveryUpdateComponent} from './delivery/deliveryUpdateComponent';
import {ExceptionsComponent} from './exceptions/exceptionsComponent';
import {NotificationsComponent} from './notifications/notificationsComponent';
import {ResolvedDeliveryComponent} from './resolved/resolvedDeliveryComponent';
import {UserPreferenceComponent} from './user_preferences/userPreferenceComponent';
import {RoutesComponent } from './routes/routesComponent';
import {WidgetComponent} from './home/widgetComponent';
import {UnauthorisedComponent} from './unauthorised/unauthorisedComponent';
import {BranchRoleComponent} from './branch-role/branchRoleComponent';
import {UserThresholdComponent} from './user_threshold/userThresholdComponent';
import { UserThresholdLevelComponent } from './user_threshold/userThresholdLevelComponent';
import { SingleRouteComponent } from './routes/singleRouteComponent';
import {StopComponent} from './stops/stopComponent';
import {ActivityComponent} from './activity/activityComponent';

const routes: Routes = [
    { path: 'well/dashboard', redirectTo: '/widgets', pathMatch: 'full' },
    { path: '', redirectTo: '/widgets', pathMatch: 'full' },
    { path: 'unauthorised', component: UnauthorisedComponent },
    { path: 'approvals', component: ApprovalsComponent },
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
    { path: 'routes', component: RoutesComponent },
    { path: 'widgets', component: WidgetComponent },
    { path: 'preferences', component: UserPreferenceComponent },
    { path: 'branch-role', component: BranchRoleComponent },
    { path: 'user-threshold', component: UserThresholdComponent },
    { path: 'user-threshold-level/:name', component: UserThresholdLevelComponent },
    { path: 'singleroute/:id', component: SingleRouteComponent },
    { path: 'stops/:id', component: StopComponent },
    { path: 'invoice', component: ActivityComponent }
];

export const appRoutingProviders: any[] = [];
export const routing = RouterModule.forRoot(routes);