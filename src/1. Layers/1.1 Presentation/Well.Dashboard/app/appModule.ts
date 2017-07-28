/* tslint:disable:max-line-length */
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { ToasterModule, ToasterService } from 'angular2-toaster';
import { Ng2PaginationModule } from 'ng2-pagination';
import { ChartsModule } from 'ng2-charts/ng2-charts';
import { TabsModule } from 'ng2-tabs';
import { AppComponent } from './appComponent';
import { routing, appRoutingProviders } from './appRoutes';
import { ApprovalsComponent } from './approvals/approvalsComponent';
import { AccountComponent } from './account/accountComponent';
import { BranchSelectionComponent } from './branch/branchSelectionComponent';
import { LoadingComponent } from './shared/loadingComponent';
import { NotificationsComponent } from './notifications/notificationsComponent';
import { UserPreferenceComponent } from './user_preferences/userPreferenceComponent';
import { UserPreferenceModal } from './user_preferences/userPreferenceModalComponent';
import { WidgetComponent } from './home/widgetComponent';
import { UnauthorisedComponent } from './unauthorised/unauthorisedComponent';
import { WidgetGraphComponent } from './home/widgetGraphComponent';
import { BranchCheckboxComponent } from './shared/branch/branchCheckboxComponent';
import { BranchRoleComponent } from './branch-role/branchRoleComponent';
import { UserThresholdComponent } from './user_threshold/userThresholdComponent';
import { UserThresholdLevelComponent } from './user_threshold/userThresholdLevelComponent';
import { ConfirmModal } from './shared/confirmModal';
import { ContactModal } from './shared/contactModal';
import { AppSearch, MenuBarAppSearchComponent } from './shared/appSearch/appSearch';
import { CustomDatePipe } from './shared/customDatePipe';
import { OptionFilterPipe } from './shared/optionFilterPipe';
import { OrderByDatePipe } from './shared/orderByDatePipe';
import { OrderByPipe } from './shared/orderByPipe';
import { OutstandingPipe } from './shared/outstandingPipe';
import { AccountFlagsComponent, AssignModal } from './shared/components/components';
import { SeasonalDatesEditModalComponent } from './seasonal_dates/seasonalDatesEditModalComponent';
import { SeasonalDatesAddModalComponent } from './seasonal_dates/seasonalDatesAddModalComponent';
import { SeasonalDatesRemoveModalComponent } from './seasonal_dates/seasonalDatesRemoveModalComponent';
import { SeasonalDatesViewComponent } from './seasonal_dates/seasonalDatesViewComponent';
import { CreditThresholdViewComponent } from './credit_threshold/creditThresholdViewComponent';
import { CreditThresholdRemoveModalComponent } from './credit_threshold/creditThresholdRemoveModalComponent';
import { CreditThresholdAddModalComponent } from './credit_threshold/creditThresholdAddModalComponent';
import { CreditThresholdEditModalComponent } from './credit_threshold/creditThresholdEditModalComponent';
import { NotificationModalComponent } from './notifications/notificationModalComponent';
import { WidgetWarningsViewComponent } from './widget_warnings/widgetWarningsViewComponent';
import { WidgetWarningAddModalComponent } from './widget_warnings/widgetWarningAddModalComponent';
import { WidgetWarningRemoveModalComponent } from './widget_warnings/widgetWarningRemoveModalComponent';
import { WidgetWarningEditModalComponent } from './widget_warnings/widgetWarningEditModalComponent';
import { ApprovalsService } from './approvals/approvalsService';
import { AccountService } from './account/accountService';
import { BranchService } from './shared/branch/branchService';
import { GlobalSettingsService } from './shared/globalSettings';
import { HttpService } from './shared/httpService';
import { HttpErrorService } from './shared/httpErrorService';
import { LogService } from './shared/logService';
import { RefreshService } from './shared/refreshService';
import { SecurityService } from './shared/security/securityService';
import { WidgetService } from './home/widgetService';
import { SeasonalDateService } from './seasonal_dates/seasonalDateService';
import { CreditThresholdService } from './credit_threshold/creditThresholdService';
import { UserService } from './shared/userService';
import { WidgetWarningService } from './widget_warnings/widgetWarningService';
import { UserPreferenceService } from './user_preferences/userPreferenceService';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ButtonDropDownComponent } from './shared/shared';
import { RoutesComponent } from './routes/routesComponent';
import { SingleRouteComponent } from './routes/singleRouteComponent';
import { StopComponent } from './stops/stopComponent';
import { SubmitActionModal } from './shared/action/submitActionModal';
import { EditExceptionsComponent } from './exceptions/editExceptionsComponent';
import { EditExceptionsModal } from './exceptions/editExceptionsModal';
import { Ng2Webstorage } from 'ngx-webstorage';
import { LookupService } from './shared/services/lookupService';
import { ActionEditComponent } from './shared/action/actionEditComponent';
import { TooltipModule } from 'ngx-tooltip';
import { ActivityComponent, ActivityService } from './activity/activity';
import { JobService, AssignGrnModal } from './job/job';
import AppRoutes = require('./appRoutes');
import { BulkEditActionModal } from './shared/action/bulkEditActionModal';
import { SingleLocationComponent, LocationsService, LocationsComponent } from './locations/locations';
import { CrmLinkController } from './shared/crm/CrmLinkController';
import { ManualCompletionModal } from './shared/manualCompletion/manualCompletionModal';
import { BranchNameComponent } from './shared/branch/branchNameComponent';
import { BranchDateThresholdComponent, BranchDateThresholdService } from './branch-role/branchDateThresholdModule';

@NgModule({
    declarations: [LoadingComponent,
        AppSearch, MenuBarAppSearchComponent, CustomDatePipe, OptionFilterPipe, OutstandingPipe, OrderByDatePipe, OrderByPipe,
        AssignModal, ConfirmModal, ContactModal, BranchRoleComponent, UserPreferenceModal, WidgetGraphComponent,
        SeasonalDatesEditModalComponent, SeasonalDatesRemoveModalComponent, SeasonalDatesViewComponent, SeasonalDatesAddModalComponent,
        AccountComponent, ApprovalsComponent, BranchSelectionComponent, NotificationsComponent, BranchCheckboxComponent, CreditThresholdViewComponent,
        CreditThresholdRemoveModalComponent, CreditThresholdAddModalComponent, CreditThresholdEditModalComponent, RoutesComponent, SingleRouteComponent,
        UserPreferenceComponent, WidgetComponent, AppComponent, UnauthorisedComponent, NotificationModalComponent, UserThresholdComponent,
        UserThresholdLevelComponent, WidgetWarningRemoveModalComponent, WidgetWarningEditModalComponent, AccountFlagsComponent, ButtonDropDownComponent,
        StopComponent, SubmitActionModal, EditExceptionsComponent, EditExceptionsModal, ActionEditComponent,
        ActivityComponent, AssignGrnModal, BulkEditActionModal, SingleLocationComponent, ManualCompletionModal,
        CrmLinkController, LocationsComponent, ManualCompletionModal, BranchNameComponent, BranchDateThresholdComponent
    ],
    imports: [
        ChartsModule, ToasterModule, BrowserModule, FormsModule, HttpModule, RouterModule, TabsModule, routing, Ng2PaginationModule,
        ReactiveFormsModule, BrowserAnimationsModule, Ng2Webstorage, TooltipModule
    ],
    providers: [
        ApprovalsService, GlobalSettingsService, HttpService, HttpErrorService, ToasterService, AccountService, BranchService,
        SeasonalDateService, RefreshService, WidgetService, SecurityService, LogService, appRoutingProviders, CreditThresholdService,
        UserService, WidgetWarningService, LookupService, UserPreferenceService, ActivityService, JobService, LocationsService, BranchDateThresholdService,
        {
            provide: APP_INITIALIZER,
            useFactory: (settingsService: GlobalSettingsService) => () => settingsService.initApp(),
            deps: [GlobalSettingsService],
            multi: true
        }],
    bootstrap: [AppComponent]
})

export class AppModule { }