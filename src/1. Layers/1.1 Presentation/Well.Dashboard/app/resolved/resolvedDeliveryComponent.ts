﻿import { Component, OnInit, ViewChild}  from '@angular/core';
import {GlobalSettingsService} from '../shared/globalSettings';
import {Router} from '@angular/router';
import 'rxjs/Rx';   // Load all features

import {ResolvedDelivery} from './resolvedDelivery';
import {ResolvedDeliveryService} from './ResolvedDeliveryService';
import {DropDownItem} from "../shared/dropDownItem";
import Option = require("../shared/filterOption");
import FilterOption = Option.FilterOption;
import {AssignModal} from "../shared/assignModal";
import {ContactModal} from "../shared/contactModal";
import {AccountService} from "../account/accountService";
import {IAccount} from "../account/account";
import {RefreshService} from '../shared/refreshService';
import {OrderArrowComponent} from '../shared/orderbyArrow';
import {CodComponent} from '../shared/codComponent';
import {SecurityService} from '../shared/security/securityService';
import {UnauthorisedComponent} from '../unauthorised/unauthorisedComponent';
import * as lodash from 'lodash';

@Component({
    selector: 'ow-resolved',
    templateUrl: './app/resolved/resolveddelivery-list.html',
    providers: [ResolvedDeliveryService]

})
export class ResolvedDeliveryComponent implements OnInit {
    isLoading: boolean = true;
    lastRefresh = Date.now();
    refreshSubscription: any;
    deliveries: ResolvedDelivery[];
    currentConfigSort: string;
    rowCount: number = 10;
    filterOption: Option.FilterOption = new FilterOption();
    options: DropDownItem[] = [
        new DropDownItem("Route", "routeNumber"),
        new DropDownItem("Invoice No", "invoiceNumber"),
        new DropDownItem("Account", "accountCode"),
        new DropDownItem("Account Name", "accountName"),
        new DropDownItem("Status", "jobStatus"),
        new DropDownItem("Action", "action"),
        new DropDownItem("Assigned", "assigned"),
        new DropDownItem("Date", "deliveryDate", false, "date")
    ];
    account: IAccount;

    @ViewChild(ContactModal) contactModal : ContactModal;
    @ViewChild(AssignModal) assignModal: AssignModal;

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private resolvedDeliveryService: ResolvedDeliveryService,
        private accountService: AccountService,
        private router: Router,
        private refreshService: RefreshService,
        private securityService: SecurityService) { }

    ngOnInit() {
        this.securityService.validateUser(this.globalSettingsService.globalSettings.permissions, this.securityService.actionDeliveries);
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getDeliveries());
        this.getDeliveries();
    }

    ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    getDeliveries() {
        this.resolvedDeliveryService.getResolvedDeliveries()
            .subscribe(deliveries => {
                    this.deliveries = deliveries;
                    this.lastRefresh = Date.now();
                    this.isLoading = false;
                },
                error => {
                    this.lastRefresh = Date.now();
                    this.isLoading = false;
                });
    }

    deliverySelected(delivery): void {
        this.router.navigate(['/delivery', delivery.id]);
    }

    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    sortDirection(sortDirection): void {
        this.currentConfigSort = sortDirection === true ? '+deliveryDate' : '-deliveryDate';
        var sortString = this.currentConfigSort === '+dateTime' ? 'asc' : 'desc';
        lodash.sortBy(this.deliveries, ['dateTime'], [sortString]);
    }

    onSortDirectionChanged(isDesc: boolean) {      
        this.sortDirection(isDesc);
    }

    openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => {
                this.account = account;
                this.contactModal.show(this.account);
            });
    }

    allocateUser(delivery: ResolvedDelivery): void {
        this.assignModal.show(delivery);
    }

    onAssigned(assigned: boolean) {
        this.getDeliveries();
    }

}