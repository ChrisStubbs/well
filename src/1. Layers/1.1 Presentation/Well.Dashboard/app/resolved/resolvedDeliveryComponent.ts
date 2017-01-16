import { Component, OnInit, ViewChild}  from '@angular/core';
import {GlobalSettingsService} from '../shared/globalSettings';
import {Router} from '@angular/router';
import 'rxjs/Rx';   // Load all features

import {ResolvedDelivery} from './resolvedDelivery';
import {ResolvedDeliveryService} from './ResolvedDeliveryService';
import {DropDownItem} from '../shared/dropDownItem';
import Option = require('../shared/filterOption');
import FilterOption = Option.FilterOption;
import {AssignModal} from '../shared/assignModal';
import {ContactModal} from '../shared/contactModal';
import {AccountService} from '../account/accountService';
import {IAccount} from '../account/account';
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
    public isLoading: boolean = true;
    public lastRefresh = Date.now();
    public refreshSubscription: any;
    public deliveries: ResolvedDelivery[];
    public currentConfigSort: string;
    public rowCount: number = 10;
    public filterOption: Option.FilterOption = new FilterOption();
    public options: DropDownItem[] = [
        new DropDownItem('Route', 'routeNumber'),
        new DropDownItem('Invoice No', 'invoiceNumber'),
        new DropDownItem('Account', 'accountCode'),
        new DropDownItem('Account Name', 'accountName'),
        new DropDownItem('Status', 'jobStatus'),
        new DropDownItem('Action', 'action'),
        new DropDownItem('Assigned', 'assigned'),
        new DropDownItem('Date', 'deliveryDate', false, 'date')
    ];
    public account: IAccount;

    @ViewChild(ContactModal) public contactModal: ContactModal;
    @ViewChild(AssignModal) public assignModal: AssignModal;

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private resolvedDeliveryService: ResolvedDeliveryService,
        private accountService: AccountService,
        private router: Router,
        private refreshService: RefreshService,
        private securityService: SecurityService) { }

    public ngOnInit() {
        this.securityService.validateUser(
            this.globalSettingsService.globalSettings.permissions,
            this.securityService.actionDeliveries);
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getDeliveries());
        this.getDeliveries();
    }

    public ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    public getDeliveries() {
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

    public deliverySelected(delivery): void {
        this.router.navigate(['/delivery', delivery.id]);
    }

    public onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    public sortDirection(sortDirection): void { 
        this.currentConfigSort = sortDirection === true ? '+deliveryDate' : '-deliveryDate';
        const sortString = this.currentConfigSort === '+dateTime' ? 'asc' : 'desc';
        lodash.sortBy(this.deliveries, ['dateTime'], [sortString]);
    }

    public onSortDirectionChanged(isDesc: boolean) {      
        this.sortDirection(isDesc);
    }

    public openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => {
                this.account = account;
                this.contactModal.show(this.account);
            });
    }

    public allocateUser(delivery: ResolvedDelivery): void {
        this.assignModal.show(delivery);
    }

    public onAssigned(assigned: boolean) {
        this.getDeliveries();
    }
}