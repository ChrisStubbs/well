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
import { ExceptionDeliveryService }                 from '../exceptions/exceptionDeliveryService';
import { RefreshService }                           from '../shared/refreshService';
import { AssignModal }                              from '../shared/assignModal';
import { ConfirmModal }                             from '../shared/confirmModal';
import { ExceptionsConfirmModal }                   from '../exceptions/exceptionsConfirmModal';
import { ToasterService }                           from 'angular2-toaster/angular2-toaster';
import { SecurityService }                          from '../shared/security/securityService';
import * as lodash                                  from 'lodash';
import { BaseComponent }                            from '../shared/BaseComponent';
import 'rxjs/Rx';
import {DeliveryLine} from '../delivery/model/deliveryLine';
import {BaseDelivery} from '../shared/baseDelivery';
import {DeliveryAction} from '../delivery/model/deliveryAction';

@Component({
    templateUrl: './app/approvals/approvals-list.html'
})
export class ApprovalsComponent extends BaseComponent implements OnInit, OnDestroy {
    public isLoading: boolean = true;
    private refreshSubscription: any;
    public errorMessage: string;
    public approvals = new Array<ApprovalDelivery>();
    public level: number;
    public assigneeOption = new DropDownItem('Assignee', 'assigned');
    public account: IAccount;
    public lastRefresh = Date.now();
    @ViewChild(AssignModal)
    private assignModal: AssignModal;
    public value: string;
    public confirmModalIsVisible: boolean = false;
    public selectGridBox: boolean = false;
    public thresholdFilterOption = new FilterOption();

    @ViewChild(ConfirmModal) private confirmModal: ConfirmModal;
    @ViewChild(ContactModal) private contactModal: ContactModal;
    @ViewChild(ExceptionsConfirmModal) private exceptionConfirmModal: ExceptionsConfirmModal;
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
            this.assigneeOption
        ];
    }

    public ngOnInit(): void {
        super.ngOnInit();

        this.securityService.validateUser(this.globalSettingsService.globalSettings.permissions,
            this.securityService.actionDeliveries);
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getApprovals());

        this.activatedRoute.queryParams.subscribe(params => 
        {
            this.getApprovals();
        });

        this.isReadOnlyUser = this.securityService
            .hasPermission(this.globalSettingsService.globalSettings.permissions, this.securityService.readOnly);
    }

    public ngOnDestroy() {
        super.ngOnDestroy();
        this.refreshSubscription.unsubscribe();
    }

    public getApprovals() {
        this.approvalsService.getApprovals()
            .subscribe(responseData => {
                    this.approvals = responseData || new Array<ApprovalDelivery>();
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

    public onSortDirectionChanged(isDesc: boolean)
    {
        super.onSortDirectionChanged(isDesc);
        this.approvals = lodash.orderBy(this.approvals, ['deliveryDate'], [super.getSort()]);
    }

    public onFilterClicked(filterOption: FilterOption) {
        super.onFilterClicked(filterOption);
    }

    public onThresholdClicked(level: number) {
        this.level = level;
        this.thresholdFilterOption = new FilterOption(
            new DropDownItem('Level', 'creditThresholdLevel'),
            level == undefined ? '' : 'Level ' + level);
    } 

    public openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => {
                    this.account = account;
                    this.contactModal.show(this.account);
                }, 
                error => this.errorMessage = <any>error);
    }

    public onAssigned($event) {
        this.getApprovals();
    }

    public allocateUser(delivery: ApprovalDelivery): void {
        this.assignModal.show(delivery);
    }

    public deliverySelected(delivery): void {
        this.router.navigate(['/delivery', delivery.id]);
    }

    public canSubmit(canSubmitDelivery: boolean): boolean
    {
        return canSubmitDelivery &&
            this.securityService.hasPermission(this.globalSettingsService.globalSettings.permissions,
                this.securityService.actionDeliveries);
    }

    public submit(delivery: BaseDelivery): void {
        this.exceptionDeliveryService.getConfirmationDetails(delivery.id)
            .subscribe((deliveryAction: DeliveryAction) => {
                this.exceptionConfirmModal.show(deliveryAction);
            });
    }
}