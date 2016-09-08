import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { ToasterModule, ToasterService } from 'angular2-toaster/angular2-toaster';
import {PaginatePipe, PaginationControlsCmp} from 'ng2-pagination';
import {ChartsModule} from 'ng2-charts/ng2-charts';

import {AppComponent} from './appComponent';
import {appRouterProviders} from './appRoutes';

import {AccountComponent} from './account/accountComponent';
import {BranchSelectionComponent} from './branch/branchSelectionComponent';
import {CleanDeliveryComponent} from './clean/cleanDeliveryComponent';
import {DeliveryComponent} from './delivery/deliveryComponent';
import {DeliveryUpdateComponent} from './delivery/deliveryUpdateComponent';
import {ExceptionsComponent} from './exceptions/exceptionsComponent';
import {NotificationsComponent} from './notifications/notificationsComponent';
import {ResolvedDeliveryComponent} from './resolved/resolvedDeliveryComponent'
import {RouteHeaderComponent} from './route_header/routeHeaderComponent';
import {UserPreferenceComponent} from './user_preferences/userPreferenceComponent';
import {UserPreferenceModal} from './user_preferences/userPreferenceModalComponent';
import {WidgetComponent} from './home/widgetComponent';
import {WidgetGraphComponent} from './home/widgetGraphComponent';

import {AssignModal} from "./shared/assign-Modal";
import {ConfirmModal} from "./shared/confirmModal";
import {ContactModal} from "./shared/contact-modal";
import {OptionFilterComponent} from './shared/optionFilterComponent';
import {OptionFilterPipe} from './shared/optionFilterPipe';
import {OrderByDatePipe} from "./shared/orderByDatePipe";
import {OutstandingPipe} from "./shared/outstandingPipe";
import {DeliverySelectionModal} from './route_header/delivery-selection-modal';
import {OrderArrowComponent} from './shared/orderby-arrow';
import AppRoutes = require("./appRoutes");

import {BranchService} from './branch/branchService';
import {GlobalSettingsService} from './shared/globalSettings';
import {HttpErrorService} from './shared/httpErrorService';
import {RefreshService} from './shared/refreshService';
import {WidgetService} from './home/widgetService';



@NgModule({
    declarations: [OptionFilterComponent, OptionFilterPipe, OutstandingPipe, PaginationControlsCmp, PaginatePipe, OrderByDatePipe,
        AssignModal, ConfirmModal, ContactModal, DeliverySelectionModal,
        UserPreferenceModal, DeliveryUpdateComponent, WidgetGraphComponent,
        AccountComponent, BranchSelectionComponent, CleanDeliveryComponent, DeliveryComponent, ExceptionsComponent, NotificationsComponent,
        ResolvedDeliveryComponent, RouteHeaderComponent, UserPreferenceComponent, WidgetComponent,
        AppComponent, OrderArrowComponent],
    imports: [ChartsModule, ToasterModule, BrowserModule, FormsModule, HttpModule, RouterModule, AppRoutes.appRouterProviders],
    providers: [GlobalSettingsService, HttpErrorService, ToasterService, BranchService, RefreshService, WidgetService],
    bootstrap: [AppComponent]
})
export class AppModule {
}