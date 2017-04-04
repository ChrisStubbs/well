﻿import { NavigateQueryParametersService }           from '../shared/NavigateQueryParametersService';
import { BaseComponent }                            from '../shared/BaseComponent';
import { Component, OnInit, ViewChild, OnDestroy}   from '@angular/core';
import {GlobalSettingsService}                      from '../shared/globalSettings';
import { Router, ActivatedRoute}                    from '@angular/router';
import {ResolvedDelivery}                           from './resolvedDelivery';
import {ResolvedDeliveryService}                    from './ResolvedDeliveryService';
import {DropDownItem}                               from '../shared/dropDownItem';
import {AssignModal}                                from '../shared/assignModal';
import {ContactModal}                               from '../shared/contactModal';
import {AccountService}                             from '../account/accountService';
import {IAccount}                                   from '../account/account';
import {RefreshService}                             from '../shared/refreshService';
import {SecurityService}                            from '../shared/security/securityService';
import { OrderByExecutor }                          from '../shared/OrderByExecutor';
import 'rxjs/Rx';

@Component({
    selector: 'ow-resolved',
    templateUrl: './app/resolved/resolveddelivery-list.html',
    providers: [ResolvedDeliveryService]

})
export class ResolvedDeliveryComponent extends BaseComponent implements OnInit, OnDestroy {
    public isLoading: boolean = true;
    public lastRefresh = Date.now();
    public refreshSubscription: any;
    public deliveries = new Array<ResolvedDelivery>();
    public currentConfigSort: string;
    public account: IAccount;
    private orderBy: OrderByExecutor = new OrderByExecutor();

    @ViewChild(ContactModal) public contactModal: ContactModal;
    @ViewChild(AssignModal) public assignModal: AssignModal;

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private resolvedDeliveryService: ResolvedDeliveryService,
        private accountService: AccountService,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private refreshService: RefreshService,
        private securityService: SecurityService,
        private nqps: NavigateQueryParametersService ) {

            super(nqps);
            this.options = [
                new DropDownItem('Route', 'routeNumber'),
                new DropDownItem('Branch', 'branchId', false, 'number'),
                new DropDownItem('Invoice No', 'invoiceNumber'),
                new DropDownItem('Account', 'accountCode'),
                new DropDownItem('Account Name', 'accountName'),
                new DropDownItem('Status', 'jobStatus'),
                new DropDownItem('Assigned', 'assigned'),
                new DropDownItem('Date', 'deliveryDate', false, 'date')
            ];
            this.sortField = 'deliveryDate';
        }

    public ngOnInit() {
        super.ngOnInit();
        this.securityService.validateUser(
            this.globalSettingsService.globalSettings.permissions,
            this.securityService.actionDeliveries);
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getDeliveries());
        this.activatedRoute.queryParams.subscribe(params => {
            this.getDeliveries();
        });
        
    }

    public ngOnDestroy() {
        super.ngOnDestroy();
        this.refreshSubscription.unsubscribe();
    }

    public getDeliveries() {
        this.resolvedDeliveryService.getResolvedDeliveries()
            .subscribe(deliveries => {
                    this.deliveries = deliveries || new Array<ResolvedDelivery>();
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

    public onSortDirectionChanged(isDesc: boolean)
    {   
        super.onSortDirectionChanged(isDesc);
        this.deliveries = this.orderBy.Order(this.deliveries, this);
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