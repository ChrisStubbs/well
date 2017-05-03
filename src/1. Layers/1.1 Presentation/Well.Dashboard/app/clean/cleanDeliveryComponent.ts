import { NavigateQueryParametersService } from '../shared/NavigateQueryParametersService';
import { BaseComponent } from '../shared/BaseComponent';
import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { GlobalSettingsService } from '../shared/globalSettings';
import { Router, ActivatedRoute } from '@angular/router';
import { CleanDelivery } from './cleanDelivery';
import { CleanDeliveryService } from './cleanDeliveryService';
import { AssignModal } from '../shared/assignModal';
import { AssignModel } from '../shared/assignModel';
import { DropDownItem } from '../shared/dropDownItem';
import { ContactModal } from '../shared/contactModal';
import { AccountService } from '../account/accountService';
import { IAccount } from '../account/account';
import { RefreshService } from '../shared/refreshService';
import { SecurityService } from '../shared/security/securityService';
import * as _ from 'lodash';
import { OrderByExecutor } from '../shared/OrderByExecutor';
import { Branch } from '../shared/branch/branch';
import 'rxjs/Rx';   // Load all features

@Component({
    selector: 'ow-clean',
    templateUrl: './app/clean/cleanDelivery-list.html',
    providers: [CleanDeliveryService]

})
export class CleanDeliveryComponent extends BaseComponent implements OnInit, OnDestroy
{
    public isLoading: boolean = true;
    public lastRefresh = Date.now();
    public refreshSubscription: any;
    public errorMessage: string;
    public cleanDeliveries = new Array<CleanDelivery>();
    public routeOption = new DropDownItem('Route', 'routeNumber');
    public account: IAccount;
    public isReadOnlyUser: boolean = false;
    public routeDate: Date;
    private orderBy: OrderByExecutor = new OrderByExecutor();
    public branchId: number;
    public routeNumber: string;

    @ViewChild(AssignModal) public assignModal: AssignModal;
    @ViewChild(ContactModal) public contactModal: ContactModal;

    constructor(
        protected globalSettingsService: GlobalSettingsService,
        private cleanDeliveryService: CleanDeliveryService,
        private accountService: AccountService,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private refreshService: RefreshService,
        protected securityService: SecurityService,
        private nqps: NavigateQueryParametersService) 
    {

        super(nqps, globalSettingsService, securityService);
        this.options = [
            this.routeOption,
            new DropDownItem('Branch', 'branchId', false, 'number'),
            new DropDownItem('Invoice No', 'invoiceNumber'),
            new DropDownItem('Account', 'accountCode'),
            new DropDownItem('Account Name', 'accountName'),
            new DropDownItem('Assignee', 'assigned'),
            new DropDownItem('Date', 'deliveryDate', false, 'date')
        ];
        this.sortField = 'deliveryDate';
        this.orderBy = new OrderByExecutor();
    }

    public ngOnInit(): void
    {
        super.ngOnInit();

        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getDeliveries());
        this.activatedRoute.queryParams.subscribe(params =>
        {
            this.routeDate = params['routeDate'];
            this.branchId = params['branchId'];
            this.routeNumber = params['filter.routeNumber'];
            this.getDeliveries();
        });

    }

    public ngOnDestroy()
    {
        super.ngOnDestroy();
        this.refreshSubscription.unsubscribe();
    }

    public getDeliveries()
    {
        this.cleanDeliveryService.getCleanDeliveries()
            .subscribe(cleanDeliveries =>
            {
                this.cleanDeliveries = cleanDeliveries || new Array<CleanDelivery>();

                if (!_.isUndefined(this.routeDate))
                {
                    this.cleanDeliveries = _.filter(this.cleanDeliveries,
                        x =>
                        {
                            return x.routeDate === this.routeDate && x.branchId === Number(this.branchId)
                                && x.routeNumber === this.routeNumber;
                        });
                }

                this.lastRefresh = Date.now();
                this.isLoading = false;
            },
            error =>
            {
                this.lastRefresh = Date.now();
                this.isLoading = false;
            });
    }

    public onSortDirectionChanged(isDesc: boolean)
    {
        super.onSortDirectionChanged(isDesc);
        this.cleanDeliveries = this.orderBy.Order(this.cleanDeliveries, this);
    }

    public deliverySelected(delivery): void
    {
        this.router.navigate(['/delivery', delivery.id, { 'tab': 'Clean' }]);
    }

    public openModal(accountId): void
    {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => { this.account = account; this.contactModal.show(this.account); },
            error => this.errorMessage = <any>error);
    }

    public allocateUser(delivery: CleanDelivery): void
    {
        const branch: Branch = { id: delivery.branchId } as Branch;
        this.assignModal.show(new AssignModel(delivery.assigned, branch, [delivery.id] as number[]));
    }

    public onAssigned(assigned: boolean)
    {
        this.getDeliveries();
    }
}