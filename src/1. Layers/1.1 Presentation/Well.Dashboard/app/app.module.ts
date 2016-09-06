import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { ToasterModule } from 'angular2-toaster/angular2-toaster';
import {PaginatePipe, PaginationControlsCmp} from 'ng2-pagination';

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

import {AssignModal} from "./shared/assign-Modal";
import {ConfirmModal} from "./shared/confirmModal";
import {ContactModal} from "./shared/contact-modal";
import {OptionFilterComponent} from './shared/optionfilter.component';
import {OptionFilterPipe} from './shared/optionFilterPipe';
import {OrderBy} from "./shared/orderBy";
import {WellModal} from "./shared/well-modal";

@NgModule({
    declarations: [OptionFilterComponent, OptionFilterPipe, PaginationControlsCmp, PaginatePipe, OrderBy,
        AssignModal, ConfirmModal, ContactModal, WellModal,
        UserPreferenceModal, DeliveryUpdateComponent,
        AccountComponent, BranchSelectionComponent, CleanDeliveryComponent, DeliveryComponent, ExceptionsComponent, NotificationsComponent,
        ResolvedDeliveryComponent, RouteHeaderComponent, UserPreferenceComponent, WidgetComponent,
        AppComponent],
    imports: [ToasterModule, BrowserModule, FormsModule, HttpModule, RouterModule, appRouterProviders],
    bootstrap: [AppComponent]
})
export class AppModule {
}