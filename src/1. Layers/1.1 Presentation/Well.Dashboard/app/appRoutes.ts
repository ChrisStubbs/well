import { Routes, RouterModule } from '@angular/router';
import {AccountComponent} from './account/accountComponent';
import {ApprovalsComponent} from './approvals/approvalsComponent';
import {BranchSelectionComponent} from './branch/branchSelectionComponent';
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
import {SingleLocationComponent, LocationsComponent} from './locations/locations';

const routes: Routes = [
    { path: 'well/dashboard', redirectTo: '/widgets', pathMatch: 'full' },
    { path: '', redirectTo: '/widgets', pathMatch: 'full' },
    { path: 'unauthorised', component: UnauthorisedComponent },
    { path: 'approvals', component: ApprovalsComponent },
    { path: 'account', component: AccountComponent },
    { path: 'branch', component: BranchSelectionComponent },
    { path: 'branch/:name/:domain', component: BranchSelectionComponent },
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
    { path: 'singlelocation', component: SingleLocationComponent },
    { path: 'locations', component: LocationsComponent }
];

export const appRoutingProviders: any[] = [];
export const routing = RouterModule.forRoot(routes);