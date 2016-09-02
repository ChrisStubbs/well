import { Component, OnInit, ViewChild}  from '@angular/core';
import {GlobalSettingsService} from '../shared/globalSettings';
import {Router} from '@angular/router';
import 'rxjs/Rx';   // Load all features

import {PaginationService } from 'ng2-pagination';
import {CleanDelivery} from './cleanDelivery';
import {CleanDeliveryService} from './cleanDeliveryService';
import {FilterOption} from "../shared/filterOption";
import {DropDownItem} from "../shared/dropDownItem";
import {ContactModal} from "../shared/contact-modal";
import {AccountService} from "../account/accountService";
import {IAccount} from "../account/account";
import {RefreshService} from '../shared/refreshService';
import {OrderArrowComponent} from '../shared/orderby-arrow';
import * as lodash from 'lodash';

@Component({
    selector: 'ow-clean',
    templateUrl: './app/clean/cleanDelivery-list.html',
    providers: [GlobalSettingsService, CleanDeliveryService, PaginationService, AccountService]
    directives: [ContactModal, OrderArrowComponent]
})
export class CleanDeliveryComponent implements OnInit {
    lastRefresh = Date.now();
    refreshSubscription: any;
    errorMessage: string;
    cleanDeliveries: CleanDelivery[];
    currentConfigSort: string;
    rowCount: number = 10;
    filterOption: FilterOption = new FilterOption();
    options: DropDownItem[] = [
        new DropDownItem("Route", "routeNumber"),
        new DropDownItem("Invoice No", "invoiceNumber"),
        new DropDownItem("Account", "accountCode"),
        new DropDownItem("Account Name", "accountName"),
        new DropDownItem("Date", "dateTime")
    ];
    account: IAccount;

    constructor(
        private cleanDeliveryService: CleanDeliveryService,
        private accountService: AccountService,
        private router: Router,
        private refreshService: RefreshService) { }

    ngOnInit(): void {
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getDeliveries());
        this.getDeliveries();
        this.currentConfigSort = '-dateTime';
        this.sortDirection(false);
    }

    ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    getDeliveries() {
        this.cleanDeliveryService.getCleanDeliveries()
            .subscribe(cleanDeliveries => {
                    this.cleanDeliveries = cleanDeliveries;
                    this.lastRefresh = Date.now();
                },
                error => this.lastRefresh = Date.now());
    }

    sortDirection(sortDirection): void {    
        this.currentConfigSort = sortDirection === true ? '+dateTime' : '-dateTime';
        var sortString = this.currentConfigSort === '+dateTime' ? 'asc' : 'desc';
        this.getDeliveries();
        lodash.sortBy(this.cleanDeliveries, ['dateTime'], [sortString]);   
    }

    
    onSortDirectionChanged(isDesc: boolean) {
        this.sortDirection(isDesc);
    }

    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    deliverySelected(delivery): void {
        this.router.navigate(['/delivery', delivery.id]);
    }

    @ViewChild(ContactModal) modal = new ContactModal();

    openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => { this.account = account; this.modal.show(this.account);},
            error => this.errorMessage = <any>error);
    }


}
