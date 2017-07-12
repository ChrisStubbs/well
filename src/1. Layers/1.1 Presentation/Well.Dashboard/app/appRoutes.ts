import { Routes, RouterModule } from '@angular/router';
import {AccountComponent} from './account/accountComponent';
import {ApprovalsComponent} from './approvals/approvalsComponent';
import {AuditComponent} from './audit/auditComponent';
import {BranchSelectionComponent} from './branch/branchSelectionComponent';
import {DeliveryComponent} from './delivery/deliveryComponent';
import {DeliveryUpdateComponent} from './delivery/deliveryUpdateComponent';
import {NotificationsComponent} from './notifications/notificationsComponent';
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
import {SingleLocationComponent} from './locations/singleLocationComponent';

const routes: Routes = [
    { path: 'well/dashboard', redirectTo: '/widgets', pathMatch: 'full' },
    { path: '', redirectTo: '/widgets', pathMatch: 'full' },
    { path: 'unauthorised', component: UnauthorisedComponent },
    { path: 'approvals', component: ApprovalsComponent },
    { path: 'audits', component: AuditComponent },
    { path: 'account', component: AccountComponent },
    { path: 'branch', component: BranchSelectionComponent },
    { path: 'branch/:name/:domain', component: BranchSelectionComponent },
    { path: 'delivery/:id', component: DeliveryComponent },
    { path: 'delivery/:id/:line', component: DeliveryUpdateComponent },
    { path: 'notifications', component: NotificationsComponent },
    { path: 'routes', component: RoutesComponent },
    { path: 'widgets', component: WidgetComponent },
    { path: 'preferences', component: UserPreferenceComponent },
    { path: 'branch-role', component: BranchRoleComponent },
    { path: 'user-threshold', component: UserThresholdComponent },
    { path: 'user-threshold-level/:name', component: UserThresholdLevelComponent },
    { path: 'singleroute/:id', component: SingleRouteComponent },
    { path: 'stops/:id', component: StopComponent },
    { path: 'invoice/:number/:branchId', component: ActivityComponent },
    { path: 'singlelocation', component: SingleLocationComponent }
];

export const appRoutingProviders: any[] = [];
export const routing = RouterModule.forRoot(routes);