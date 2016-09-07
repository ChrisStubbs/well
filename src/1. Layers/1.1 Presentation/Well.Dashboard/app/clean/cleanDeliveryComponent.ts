import { Component, OnInit, ViewChild}  from '@angular/core';
import {GlobalSettingsService} from '../shared/globalSettings';
import {Router, ActivatedRoute} from '@angular/router';
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

@Component({
    selector: 'ow-clean',
    templateUrl: './app/clean/cleanDelivery-list.html',
    providers: [CleanDeliveryService, PaginationService, AccountService]
})
export class CleanDeliveryComponent implements OnInit {
    lastRefresh = Date.now();
    refreshSubscription: any;
    errorMessage: string;
    cleanDeliveries: CleanDelivery[];
    currentConfigSort: string;
    rowCount: number = 10;
    filterOption: FilterOption = new FilterOption();
    routeOption = new DropDownItem("Route", "routeNumber");
    options: DropDownItem[] = [
        this.routeOption,
        new DropDownItem("Invoice No", "invoiceNumber"),
        new DropDownItem("Account", "accountCode"),
        new DropDownItem("Account Name", "accountName"),
        new DropDownItem("Date", "dateTime")
    ];
    account: IAccount;
    routeId: string;
    selectedOption: DropDownItem;
    selectedFilter: string;

    constructor(
        private cleanDeliveryService: CleanDeliveryService,
        private accountService: AccountService,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private refreshService: RefreshService) { }

    ngOnInit(): void {
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getDeliveries());
        this.currentConfigSort = '-dateTime';
        this.activatedRoute.queryParams.subscribe(params => {
            this.routeId = params['route'];
            this.getDeliveries();
        });
    }

    ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    getDeliveries() {
        this.cleanDeliveryService.getCleanDeliveries()
            .subscribe(cleanDeliveries => {
                    this.cleanDeliveries = cleanDeliveries;
                    this.lastRefresh = Date.now();

                    if (this.routeId) {
                        this.filterOption = new FilterOption(this.routeOption, this.routeId);
                        this.selectedOption = this.routeOption;
                        this.selectedFilter = this.routeId;
                    }
                },
                error => this.lastRefresh = Date.now());
    }

    sortDirection(sortDirection): void {
        this.getDeliveries();
        this.currentConfigSort = sortDirection === true ? '+dateTime' : '-dateTime';
        
    }

    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    deliverySelected(delivery): void {
        this.router.navigate(['/delivery', delivery.id]);
    }

    @ViewChild(ContactModal) modal : ContactModal;

    openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => { this.account = account; this.modal.show(this.account);},
            error => this.errorMessage = <any>error);
    }
}
