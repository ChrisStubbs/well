/* tslint:disable:max-line-length */
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { ToasterModule, ToasterService } from 'angular2-toaster';
import {Ng2PaginationModule} from 'ng2-pagination';
import {ChartsModule} from 'ng2-charts/ng2-charts';
import {TabsModule} from 'ng2-tabs'; 
import {AppComponent} from './appComponent';
import {routing, appRoutingProviders} from './appRoutes';
import {ApprovalsComponent} from './approvals/approvalsComponent';
import {AccountComponent} from './account/accountComponent'; 
import {BranchSelectionComponent} from './branch/branchSelectionComponent';
import {CleanDeliveryComponent} from './clean/cleanDeliveryComponent';
import {DeliveryComponent} from './delivery/deliveryComponent';
import {DeliveryUpdateComponent} from './delivery/deliveryUpdateComponent';
import {DeliveryIssuesComponent} from './delivery/deliveryIssuesComponent';
import {ExceptionsComponent} from './exceptions/exceptionsComponent';
import {NotificationsComponent} from './notifications/notificationsComponent';
import {ResolvedDeliveryComponent} from './resolved/resolvedDeliveryComponent';
import {RouteHeaderComponent} from './route_header/routeHeaderComponent';
import {UserPreferenceComponent} from './user_preferences/userPreferenceComponent';
import {UserPreferenceModal} from './user_preferences/userPreferenceModalComponent';
import {WidgetComponent} from './home/widgetComponent';
import {UnauthorisedComponent} from './unauthorised/unauthorisedComponent';
import {WidgetGraphComponent} from './home/widgetGraphComponent';
import {BranchCheckboxComponent} from './shared/branch/branchCheckboxComponent';
import {BranchRoleComponent} from './branch-role/branchRoleComponent';
import {UserThresholdComponent} from './user_threshold/userThresholdComponent';
import {UserThresholdLevelComponent} from './user_threshold/userThresholdLevelComponent';
import {CleanPreferenceComponent} from './clean_preferences/cleanPreferenceComponent';
import {CleanPreferenceAddModalComponent} from './clean_preferences/cleanPreferenceAddModalComponent';
import {CleanPreferenceRemoveModalComponent} from './clean_preferences/cleanPreferenceRemoveModalComponent';
import { CleanPreferenceEditModalComponent } from './clean_preferences/cleanPreferenceEditModalComponent';
import {AssignModal} from './shared/assignModal';
import {ConfirmModal} from './shared/confirmModal';
import {ContactModal} from './shared/contactModal';
import {OptionFilterComponent} from './shared/optionFilterComponent';
import {CustomDatePipe} from './shared/customDatePipe';
import {OptionFilterPipe} from './shared/optionFilterPipe';
import {OrderByDatePipe} from './shared/orderByDatePipe';
import {OutstandingPipe} from './shared/outstandingPipe';
import {DeliverySelectionModal} from './route_header/delivery-selection-modal';
import {AccountFlagsComponent} from './shared/accountFlagsComponent';
import {OrderArrowComponent} from './shared/orderbyArrow';
import {SeasonalDatesEditModalComponent} from './seasonal_dates/seasonalDatesEditModalComponent';
import {SpinnerComponent} from './shared/spinnerComponent';
import {SeasonalDatesAddModalComponent} from './seasonal_dates/seasonalDatesAddModalComponent';
import {SeasonalDatesRemoveModalComponent} from './seasonal_dates/seasonalDatesRemoveModalComponent';
import {SeasonalDatesViewComponent} from './seasonal_dates/seasonalDatesViewComponent';
import {CreditThresholdViewComponent} from './credit_threshold/creditThresholdViewComponent';
import {CreditThresholdRemoveModalComponent} from './credit_threshold/creditThresholdRemoveModalComponent';
import {CreditThresholdAddModalComponent} from './credit_threshold/creditThresholdAddModalComponent';
import {CreditThresholdEditModalComponent} from './credit_threshold/creditThresholdEditModalComponent';
import {NotificationModalComponent} from './notifications/notificationModalComponent';
import AppRoutes = require('./appRoutes');
import {WidgetWarningsViewComponent} from './widget_warnings/widgetWarningsViewComponent';
import {WidgetWarningAddModalComponent} from './widget_warnings/widgetWarningAddModalComponent';
import {WidgetWarningRemoveModalComponent} from './widget_warnings/widgetWarningRemoveModalComponent';
import {WidgetWarningEditModalComponent} from './widget_warnings/widgetWarningEditModalComponent';

import {ApprovalsService} from './approvals/approvalsService';
import {AccountService} from './account/accountService';
import {BranchService} from './shared/branch/branchService';
import {GlobalSettingsService} from './shared/globalSettings';
import {HttpErrorService} from './shared/httpErrorService';
import {LogService} from './shared/logService';
import {RefreshService} from './shared/refreshService';
import {SecurityService} from './shared/security/securityService';
import {WidgetService} from './home/widgetService';
import {SeasonalDateService} from './seasonal_dates/seasonalDateService';
import {CreditThresholdService} from './credit_threshold/creditThresholdService';
import {CleanPreferenceService} from './clean_preferences/cleanPreferenceService';
import { UserService } from './shared/userService';
import {WidgetWarningService} from './widget_warnings/widgetWarningService';
import { ExceptionDeliveryService } from './exceptions/exceptionDeliveryService';
import { DeliveryService } from './delivery/deliveryService';
import { NavigateQueryParametersService } from './shared/NavigateQueryParametersService';
import {CalendarModule } from './shared/primeng/primeng';
import { AuditComponent } from './audit/auditComponent';
import { AuditService } from './audit/auditService';
import { ExceptionsConfirmModal } from './exceptions/exceptionsConfirmModal';
import {BulkCreditConfirmModal} from './exceptions/bulkCreditConfirmModal';
import { UserPreferenceService } from './user_preferences/userPreferenceService';

@NgModule({
    declarations: [SpinnerComponent,
        OptionFilterComponent, CustomDatePipe, OptionFilterPipe, OutstandingPipe, OrderByDatePipe,
        AssignModal, ConfirmModal, ContactModal, DeliverySelectionModal, BranchRoleComponent, 
        UserPreferenceModal, DeliveryUpdateComponent, WidgetGraphComponent, SeasonalDatesEditModalComponent, SeasonalDatesRemoveModalComponent,
        SeasonalDatesViewComponent, SeasonalDatesAddModalComponent, CleanPreferenceEditModalComponent,
        DeliveryIssuesComponent, CleanPreferenceRemoveModalComponent, 
        AccountComponent, ApprovalsComponent, AuditComponent, BranchSelectionComponent, CleanDeliveryComponent, DeliveryComponent, 
        ExceptionsComponent, NotificationsComponent, BranchCheckboxComponent, CreditThresholdViewComponent, CreditThresholdRemoveModalComponent,
        CreditThresholdAddModalComponent, CreditThresholdEditModalComponent, CleanPreferenceComponent, CleanPreferenceAddModalComponent,
        ResolvedDeliveryComponent, RouteHeaderComponent, UserPreferenceComponent, WidgetComponent, 
        AppComponent, OrderArrowComponent, UnauthorisedComponent, NotificationModalComponent, UserThresholdComponent, UserThresholdLevelComponent,
        WidgetWarningsViewComponent, WidgetWarningAddModalComponent, WidgetWarningRemoveModalComponent, WidgetWarningEditModalComponent,
        AccountFlagsComponent, ExceptionsConfirmModal, BulkCreditConfirmModal
    ],
    imports: [ 
        ChartsModule, ToasterModule, BrowserModule, FormsModule, HttpModule, RouterModule, TabsModule, routing, Ng2PaginationModule, CalendarModule
    ],
    providers: [
        ApprovalsService, GlobalSettingsService, HttpErrorService, ToasterService, AccountService, AuditService, BranchService,
        SeasonalDateService, RefreshService, WidgetService, SecurityService, LogService, appRoutingProviders, CreditThresholdService,
        CleanPreferenceService, UserService, WidgetWarningService, DeliveryService,
        CleanPreferenceService, UserService, ExceptionDeliveryService, NavigateQueryParametersService, UserPreferenceService,
    {
        provide: APP_INITIALIZER,
        useFactory: (settingsService: GlobalSettingsService) => () => settingsService.initApp(),
        deps: [GlobalSettingsService],
        multi: true
    }],
    bootstrap: [AppComponent]
})

export class AppModule {}