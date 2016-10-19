import { NgModule, APP_INITIALIZER } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { ToasterModule, ToasterService } from 'angular2-toaster/angular2-toaster';
import {PaginatePipe, PaginationControlsCmp} from 'ng2-pagination';
import {ChartsModule} from 'ng2-charts/ng2-charts';
import {TabsModule} from 'ng2-tabs';
import {AppComponent} from './appComponent';
import {routing, appRoutingProviders} from './appRoutes';

import {AccountComponent} from './account/accountComponent';
import {AuditComponent} from './audit/auditComponent';
import {BranchSelectionComponent} from './branch/branchSelectionComponent';
import {CleanDeliveryComponent} from './clean/cleanDeliveryComponent';
import {DeliveryComponent} from './delivery/deliveryComponent';
import {DeliveryUpdateComponent} from './delivery/deliveryUpdateComponent';
import {DeliveryIssuesComponent} from './delivery/deliveryIssuesComponent';
import {DeliveryActionsComponent} from './delivery/deliveryActionsComponent';
import {SubmitConfirmModal} from './delivery/submitConfirmModal';
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
import {CleanPreferenceEditModalComponent} from './clean_preferences/cleanPreferenceEditModalComponent';

import {AssignModal} from "./shared/assignModal";
import {ConfirmModal} from "./shared/confirmModal";
import {ContactModal} from "./shared/contactModal";
import {OptionFilterComponent} from './shared/optionFilterComponent';
import {OptionFilterPipe} from './shared/optionFilterPipe';
import {OrderByDatePipe} from "./shared/orderByDatePipe";
import {OutstandingPipe} from "./shared/outstandingPipe";
import {DeliverySelectionModal} from './route_header/delivery-selection-modal';
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
import AppRoutes = require("./appRoutes");
import {WidgetWarningsViewComponent} from './widget_warnings/widgetWarningsViewComponent';
import {WidgetWarningAddModalComponent} from './widget_warnings/widgetWarningAddModalComponent';
import {WidgetWarningRemoveModalComponent} from './widget_warnings/widgetWarningRemoveModalComponent';
import {WidgetWarningEditModalComponent} from './widget_warnings/widgetWarningEditModalComponent';

import {AccountService} from './account/accountService';
import {AuditService} from './audit/auditService';
import {BranchService} from './shared/branch/branchService';
import {GlobalSettingsService} from './shared/globalSettings';
import {HttpErrorService} from './shared/httpErrorService';
import {LogService} from './shared/logService';
import {PaginationService } from 'ng2-pagination';
import {RefreshService} from './shared/refreshService';
import {SecurityService} from './shared/security/securityService';
import {WidgetService} from './home/widgetService';
import {SeasonalDateService} from './seasonal_dates/seasonalDateService';
import {CreditThresholdService} from './credit_threshold/creditThresholdService';
import {CleanPreferenceService} from './clean_preferences/cleanPreferenceService';
import {UserService} from './shared/userService';
import {WidgetWarningService} from './widget_warnings/widgetWarningService';

@NgModule({
    declarations: [SpinnerComponent,
        OptionFilterComponent, OptionFilterPipe, OutstandingPipe, PaginationControlsCmp, PaginatePipe, OrderByDatePipe,
        AssignModal, ConfirmModal, ContactModal, DeliverySelectionModal, BranchRoleComponent, 
        UserPreferenceModal, DeliveryUpdateComponent, WidgetGraphComponent, SeasonalDatesEditModalComponent, SeasonalDatesRemoveModalComponent,
        SeasonalDatesViewComponent, SeasonalDatesAddModalComponent, CleanPreferenceEditModalComponent, SubmitConfirmModal,
        DeliveryIssuesComponent, DeliveryActionsComponent, CleanPreferenceRemoveModalComponent,
        AccountComponent, AuditComponent, BranchSelectionComponent, CleanDeliveryComponent, DeliveryComponent, ExceptionsComponent,
        NotificationsComponent, BranchCheckboxComponent, CreditThresholdViewComponent, CreditThresholdRemoveModalComponent,
        CreditThresholdAddModalComponent, CreditThresholdEditModalComponent, CleanPreferenceComponent, CleanPreferenceAddModalComponent,
        ResolvedDeliveryComponent, RouteHeaderComponent, UserPreferenceComponent, WidgetComponent, 
        AppComponent, OrderArrowComponent, UnauthorisedComponent, NotificationModalComponent, UserThresholdComponent, UserThresholdLevelComponent,
        WidgetWarningsViewComponent, WidgetWarningAddModalComponent, WidgetWarningRemoveModalComponent, WidgetWarningEditModalComponent
    ],
    imports: [
        ChartsModule, ToasterModule, BrowserModule, FormsModule, HttpModule, RouterModule, TabsModule, routing
    ],
    providers: [
        GlobalSettingsService, HttpErrorService, ToasterService, AccountService, AuditService, BranchService, PaginationService,
        SeasonalDateService, RefreshService, WidgetService, SecurityService, LogService, appRoutingProviders, CreditThresholdService,
        CleanPreferenceService, UserService, WidgetWarningService,
    {
        provide: APP_INITIALIZER,
        useFactory: (settingsService: GlobalSettingsService) => () => settingsService.initApp(),
        deps: [GlobalSettingsService],
        multi: true
    }
        ],
    bootstrap: [AppComponent]
})
export class AppModule {

}