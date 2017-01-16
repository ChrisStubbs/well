import { Component, OnInit, ViewChild}  from '@angular/core';
import {GlobalSettingsService} from '../shared/globalSettings';
import {Router, ActivatedRoute} from '@angular/router';
import 'rxjs/Rx';   // Load all features

import {CleanDelivery} from './cleanDelivery';
import {CleanDeliveryService} from './cleanDeliveryService';
import {AssignModal} from '../shared/assignModal';
import {FilterOption} from '../shared/filterOption';
import {DropDownItem} from '../shared/dropDownItem';
import {ContactModal} from '../shared/contactModal';
import {AccountService} from '../account/accountService';
import {IAccount} from '../account/account';
import {RefreshService} from '../shared/refreshService';
import {OrderArrowComponent} from '../shared/orderbyArrow';
import {SecurityService} from '../shared/security/securityService';
import {UnauthorisedComponent} from '../unauthorised/unauthorisedComponent';
import {CodComponent} from '../shared/codComponent';
import * as lodash from 'lodash';

@Component({
    selector: 'ow-clean',
    templateUrl: './app/clean/cleanDelivery-list.html',
    providers: [CleanDeliveryService]

})
export class CleanDeliveryComponent implements OnInit {
    public isLoading: boolean = true;
    public lastRefresh = Date.now();
    public refreshSubscription: any;
    public errorMessage: string;
    public cleanDeliveries: CleanDelivery[];
    public currentConfigSort: string;
    public rowCount: number = 10;
    public filterOption: FilterOption = new FilterOption();
    public routeOption = new DropDownItem('Route', 'routeNumber');
    public options: DropDownItem[] = [
        this.routeOption,
        new DropDownItem('Invoice No', 'invoiceNumber'),
        new DropDownItem('Account', 'accountCode'),
        new DropDownItem('Account Name', 'accountName'),
        new DropDownItem('Assignee', 'assigned'),
        new DropDownItem('Date', 'deliveryDate', false, 'date')
    ];
    public account: IAccount;
    public routeId: string;
    public selectedOption: DropDownItem;
    public selectedFilter: string;
    public isReadOnlyUser: boolean = false;

    @ViewChild(AssignModal) public assignModal: AssignModal;
    @ViewChild(ContactModal) public contactModal: ContactModal;

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private cleanDeliveryService: CleanDeliveryService,
        private accountService: AccountService,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private refreshService: RefreshService,
        private securityService: SecurityService) { }

    public ngOnInit(): void {
        this.securityService.validateUser(
            this.globalSettingsService.globalSettings.permissions,
            this.securityService.actionDeliveries);
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getDeliveries());
        this.activatedRoute.queryParams.subscribe(params => {
            this.routeId = params['route'];
            this.getDeliveries();
        });

        this.isReadOnlyUser = this.securityService
            .hasPermission(this.globalSettingsService.globalSettings.permissions, this.securityService.readOnly);
    }

    public ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    public getDeliveries() {
        this.cleanDeliveryService.getCleanDeliveries()
            .subscribe(cleanDeliveries => {
                this.cleanDeliveries = cleanDeliveries;
                this.lastRefresh = Date.now();

                if (this.routeId) {
                    this.filterOption = new FilterOption(this.routeOption, this.routeId);
                    this.selectedOption = this.routeOption;
                    this.selectedFilter = this.routeId;
                }
                this.isLoading = false;
            },
            error => {
                this.lastRefresh = Date.now();
                this.isLoading = false;
            });
    }

    public sortDirection(sortDirection): void {
        this.currentConfigSort = sortDirection === true ? '+deliveryDate' : '-deliveryDate';
        const sortString = this.currentConfigSort === '+dateTime' ? 'asc' : 'desc';
        this.getDeliveries();
        lodash.sortBy(this.cleanDeliveries, ['dateTime'], [sortString]);
    }

    public onSortDirectionChanged(isDesc: boolean) {
        this.sortDirection(isDesc);
    }

    public onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    public deliverySelected(delivery): void {
        this.router.navigate(['/delivery', delivery.id]);
    }

    public openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => { this.account = account; this.contactModal.show(this.account); },
            error => this.errorMessage = <any>error);
    }

    public allocateUser(delivery: CleanDelivery): void {
        this.assignModal.show(delivery);
    }

    public onAssigned(assigned: boolean) {
        this.getDeliveries();
    }
}