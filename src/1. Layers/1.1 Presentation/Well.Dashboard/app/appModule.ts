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

import {AssignModal} from "./shared/assignModal";
import {ConfirmModal} from "./shared/confirmModal";
import {ContactModal} from "./shared/contactModal";
import {OptionFilterComponent} from './shared/optionFilterComponent';
import {OptionFilterPipe} from './shared/optionFilterPipe';
import {OrderByDatePipe} from "./shared/orderByDatePipe";
import {OutstandingPipe} from "./shared/outstandingPipe";
import {DeliverySelectionModal} from './route_header/delivery-selection-modal';
import {OrderArrowComponent} from './shared/orderbyArrow';
import {SeasonalDatesComponent} from './seasonal_dates/seasonalDatesComponent';
import {SeasonalDatesViewComponent} from './seasonal_dates/seasonalDatesViewComponent';
import {CreditThresholdComponent} from './credit_threshold/creditThresholdComponent';
import {CreditThresholdViewComponent} from './credit_threshold/creditThresholdViewComponent';
import AppRoutes = require("./appRoutes");

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

@NgModule({
    declarations: [
        OptionFilterComponent, OptionFilterPipe, OutstandingPipe, PaginationControlsCmp, PaginatePipe, OrderByDatePipe,
        AssignModal, ConfirmModal, ContactModal, DeliverySelectionModal, BranchRoleComponent, 
        UserPreferenceModal, DeliveryUpdateComponent, WidgetGraphComponent, SeasonalDatesComponent, SeasonalDatesViewComponent,
        AccountComponent, AuditComponent, BranchSelectionComponent, CleanDeliveryComponent, DeliveryComponent, ExceptionsComponent,
        NotificationsComponent, BranchCheckboxComponent, CreditThresholdComponent, CreditThresholdViewComponent,
        ResolvedDeliveryComponent, RouteHeaderComponent, UserPreferenceComponent, WidgetComponent,
        AppComponent, OrderArrowComponent, UnauthorisedComponent
    ],
    imports: [
        ChartsModule, ToasterModule, BrowserModule, FormsModule, HttpModule, RouterModule, TabsModule, routing
    ],
    providers: [
        GlobalSettingsService, HttpErrorService, ToasterService, AccountService, AuditService, BranchService, PaginationService,
        RefreshService, WidgetService, SecurityService, LogService, appRoutingProviders,
    {
        provide: APP_INITIALIZER,
        useFactory: (settingsService: GlobalSettingsService) => () => settingsService.getSettings(),
        deps: [GlobalSettingsService],
        multi: true
    },
        ],
    bootstrap: [AppComponent]
})
export class AppModule {

}