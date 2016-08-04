import { Component, OnInit, ViewChild}  from '@angular/core';
import {Router} from '@angular/router';
import { HTTP_PROVIDERS } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features

import {PaginatePipe, PaginationControlsCmp, PaginationService } from 'ng2-pagination';
import {OptionFilterComponent} from '../shared/optionfilter.component';
import {OptionFilterPipe } from '../shared/optionFilterPipe';
import {FilterOption} from "../shared/filterOption";
import {DropDownItem} from "../shared/dropDownItem";
import {ContactModal} from "../shared/contact-modal";
import {AccountService} from "../account/accountService";
import {IAccount} from "../account/account";
import {ExceptionDelivery} from "./exceptionDelivery";
import {ExceptionDeliveryService} from "./exceptionDeliveryService";
import {RefreshService} from '../shared/refreshService';

@Component({
    selector: 'ow-exceptions',
    templateUrl: './app/exceptions/exceptions-list.html',
    providers: [HTTP_PROVIDERS, GlobalSettingsService, ExceptionDeliveryService, PaginationService, AccountService],
    directives: [OptionFilterComponent, PaginationControlsCmp, ContactModal],
    pipes: [OptionFilterPipe, PaginatePipe]
})
export class ExceptionsComponent implements OnInit {
    refreshSubscription: any;
    errorMessage: string;
    exceptions: ExceptionDelivery[];
    rowCount: number = 10;
    filterOption: FilterOption = new FilterOption();
    options: DropDownItem[] = [
        new DropDownItem("Route", "routeNumber"),
        new DropDownItem("Drop", "dropId"),
        new DropDownItem("Invoice No", "invoiceNumber"),
        new DropDownItem("Account", "accountCode"),
        new DropDownItem("Account Name", "accountName"),
        new DropDownItem("Date", "dateTime")
    ];
    defaultAction: DropDownItem = new DropDownItem("Action");
    actions: DropDownItem[] = [
        new DropDownItem("Assign", "#"),
        new DropDownItem("Credit", "#"),
        new DropDownItem("Credit and Re-Order", "#"),
        new DropDownItem("Re-Plan", "#"),
        new DropDownItem("Future Re-plan", "#"),
        new DropDownItem("No Action", "#")
    ];
    account: IAccount;
    lastRefresh = Date.now();

    constructor(
        private exceptionDeliveryService: ExceptionDeliveryService,
        private accountService: AccountService,
        private router: Router,
        private refreshService: RefreshService) {
    }

    ngOnInit(): void {
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getExceptions());
        this.getExceptions();
    }

    ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    getExceptions() {
        this.exceptionDeliveryService.getExceptions()
            .subscribe(responseData => this.function1(responseData),
                error => this.function2(error));
    }

    function1(responseData) {
        this.exceptions = responseData;
        this.lastRefresh = Date.now();
    };

    function2(error) {
        if (error.status && error.status === 404) {
            this.lastRefresh = Date.now();
        }
    };

    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    deliverySelected(delivery): void {
        this.router.navigate(['/delivery', delivery.id]);
    }

    @ViewChild(ContactModal) modal = new ContactModal();

    openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => { this.account = account; this.modal.show(this.account); },
            error => this.errorMessage = <any>error);
    }

    setSelectedAction(delivery: ExceptionDelivery, action: DropDownItem): void {
        delivery.action = action.description;
    }

}