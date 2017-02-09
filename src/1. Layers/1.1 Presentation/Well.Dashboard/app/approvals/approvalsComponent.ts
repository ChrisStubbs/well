import {Component, OnInit, ViewChild, OnDestroy}    from '@angular/core';
import { Router, ActivatedRoute}                    from '@angular/router';
import { Response }                                 from '@angular/http';
import { GlobalSettingsService }                    from '../shared/globalSettings';
import { NavigateQueryParametersService }           from '../shared/NavigateQueryParametersService';
import { FilterOption }                             from '../shared/filterOption';
import { DropDownItem }                             from '../shared/dropDownItem';
import { ContactModal }                             from '../shared/contactModal';
import { AccountService }                           from '../account/accountService';
import { IAccount }                                 from '../account/account';
import { ApprovalDelivery }                         from './approvalDelivery';
import { ApprovalsService }                         from './approvalsService';
import {ExceptionDelivery}                   from '../exceptions/exceptionDelivery';
import {ExceptionDeliveryService}                   from '../exceptions/exceptionDeliveryService';
import { RefreshService }                           from '../shared/refreshService';
import { AssignModal }                              from '../shared/assignModal';
import { ConfirmModal }                             from '../shared/confirmModal';
import { ToasterService }                           from 'angular2-toaster/angular2-toaster';
import { SecurityService }                          from '../shared/security/securityService';
import * as lodash                                  from 'lodash';
import { BaseComponent }                            from '../shared/BaseComponent';
import 'rxjs/Rx';   // Load all features

@Component({
    templateUrl: './app/approvals/approvals-list.html'
})
export class ApprovalsComponent extends BaseComponent implements OnInit, OnDestroy {
    public isLoading: boolean = true;
    private refreshSubscription: any;
    public errorMessage: string;
    public approvals: ExceptionDelivery[];
    public level: number;
    public assigneeOption = new DropDownItem('Assignee', 'assigned');
    public account: IAccount;
    public lastRefresh = Date.now();
    @ViewChild(AssignModal)
    private assignModal: AssignModal;
    public value: string;
    public confirmModalIsVisible: boolean = false;
    public selectGridBox: boolean = false;
    @ViewChild(ConfirmModal) private confirmModal: ConfirmModal;
    @ViewChild(ContactModal) private contactModal: ContactModal;
    public isReadOnlyUser: boolean = false;

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private accountService: AccountService,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private refreshService: RefreshService,
        private toasterService: ToasterService,
        private securityService: SecurityService,
        private nqps: NavigateQueryParametersService,
        private approvalsService: ApprovalsService,
        private exceptionDeliveryService: ExceptionDeliveryService) {

        super(nqps);

        this.options = [
            this.assigneeOption,
            new DropDownItem('Date', 'deliveryDate', false, 'date'),
            new DropDownItem('Credit Threshold', 'totalCreditValueForThreshold', false, 'numberLessThanOrEqual')
        ];
    }

    public ngOnInit(): void {
        super.ngOnInit();

        this.securityService.validateUser(this.globalSettingsService.globalSettings.permissions,
            this.securityService.actionDeliveries);
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(this.getApprovals());

        this.isReadOnlyUser = this.securityService
            .hasPermission(this.globalSettingsService.globalSettings.permissions, this.securityService.readOnly);
    }

    public ngOnDestroy() {
        super.ngOnDestroy();
        this.refreshSubscription.unsubscribe();
    }

    public getApprovals() {
        //TODO - Switch for approvalsService once API is implemented
        this.exceptionDeliveryService.getExceptions()
            .subscribe(responseData => {
                    this.approvals = responseData;
                    this.lastRefresh = Date.now();
                    this.isLoading = false;
                },
                error => {
                    if (error.status && error.status === 404) {
                        this.lastRefresh = Date.now();
                    }
                    this.isLoading = false;
                });
    }

    private sortDirection(sortDirection): void {
        const sortString = sortDirection ? 'asc' : 'desc';
        this.approvals = lodash.orderBy(this.approvals, ['deliveryDate'], [sortString]);
        super.onSortDirectionChanged(sortDirection);
    }

    public onSortDirectionChanged(isDesc: boolean) {
        this.sortDirection(isDesc);
    }
    
    public onFilterClicked(filterOption: FilterOption) {
        super.onFilterClicked(filterOption);
    }

    public onThresholdClicked(level: number) {
        this.level = level;
    }

    public openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => {
                    this.account = account;
                    this.contactModal.show(this.account);
                },
                error => this.errorMessage = <any>error);
    }

    public allocateUser(delivery: ApprovalDelivery): void {
        this.assignModal.show(delivery);
    }
}