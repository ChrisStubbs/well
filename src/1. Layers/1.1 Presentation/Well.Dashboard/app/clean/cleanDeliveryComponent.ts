import { Component, OnInit, ViewChild}  from '@angular/core';
import {GlobalSettingsService} from '../shared/globalSettings';
import {Router, ActivatedRoute} from '@angular/router';
import 'rxjs/Rx';   // Load all features

import {CleanDelivery} from './cleanDelivery';
import {CleanDeliveryService} from './cleanDeliveryService';
import {FilterOption} from "../shared/filterOption";
import {DropDownItem} from "../shared/dropDownItem";
import {ContactModal} from "../shared/contactModal";
import {AccountService} from "../account/accountService";
import {IAccount} from "../account/account";
import {RefreshService} from '../shared/refreshService';
import {OrderArrowComponent} from '../shared/orderbyArrow';
import {SecurityService} from '../shared/security/securityService';
import {UnauthorisedComponent} from '../unauthorised/unauthorisedComponent';
import * as lodash from 'lodash';

@Component({
    selector: 'ow-clean',
    templateUrl: './app/clean/cleanDelivery-list.html',
    providers: [CleanDeliveryService]

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
        new DropDownItem("Date", "deliveryDate")
    ];
    account: IAccount;
    routeId: string;
    selectedOption: DropDownItem;
    selectedFilter: string;

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private cleanDeliveryService: CleanDeliveryService,
        private accountService: AccountService,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private refreshService: RefreshService,
        private securityService: SecurityService) { }

    ngOnInit(): void {
        this.securityService.validateUser(this.globalSettingsService.globalSettings.permissions, this.securityService.actionDeliveries);
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getDeliveries());
        this.currentConfigSort = '-deliveryDate';
        this.sortDirection(false);
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
        this.currentConfigSort = sortDirection === true ? '+deliveryDate' : '-deliveryDate';
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

    @ViewChild(ContactModal) modal : ContactModal;

    openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => { this.account = account; this.modal.show(this.account);},
            error => this.errorMessage = <any>error);
    }


}
