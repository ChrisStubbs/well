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
import { ExceptionDelivery }                        from './exceptionDelivery';
import { ExceptionDeliveryService }                 from './exceptionDeliveryService';
import { RefreshService }                           from '../shared/refreshService';
import { HttpResponse }                             from '../shared/httpResponse';
import { AssignModal }                              from '../shared/assignModal';
import { ConfirmModal }                             from '../shared/confirmModal';
import { IUser }                                    from '../shared/iuser';
import { ToasterService }                           from 'angular2-toaster/angular2-toaster';
import { SecurityService }                          from '../shared/security/securityService';
import { ExceptionsConfirmModal }                   from './exceptionsConfirmModal';
import * as _                                       from 'lodash';
import { BaseComponent }                            from '../shared/BaseComponent';
import {DeliveryAction}                             from '../delivery/model/deliveryAction';
import {BulkCredit}                                 from './bulkCredit';
import {DeliveryService}                            from '../delivery/deliveryService';
import {BulkCreditConfirmModal}                     from './bulkCreditConfirmModal';
import { OrderByExecutor }                          from '../shared/OrderByExecutor';
import 'rxjs/Rx';

@Component({
    selector: 'ow-exceptions',
    templateUrl: './app/exceptions/exceptions-list.html',
    providers: [ExceptionDeliveryService]
})
export class ExceptionsComponent extends BaseComponent implements OnInit, OnDestroy {
    public isLoading: boolean = true;
    private refreshSubscription: any;
    public errorMessage: string;
    public exceptions = new Array<ExceptionDelivery>();
    public routeOption = new DropDownItem('Route', 'routeNumber');
    public assigneeOption = new DropDownItem('Assignee', 'assigned');
    public account: IAccount;
    public lastRefresh = Date.now();
    public httpResponse: HttpResponse = new HttpResponse();
    public users: IUser[];
    public delivery: ExceptionDelivery;
    public outstandingFilter: boolean = false;
    public bulkCredits = new Array<number>();
    public threshold: number;
    @ViewChild(AssignModal)
    private assignModal: AssignModal;
    public value: string;
    public confirmModalIsVisible: boolean = false;
    public selectGridBox: boolean = false;
    @ViewChild(ConfirmModal)
    //private confirmModal: ConfirmModal;
    @ViewChild(ContactModal)
    private contactModal: ContactModal;
    @ViewChild(ExceptionsConfirmModal)
    private exceptionConfirmModal: ExceptionsConfirmModal;
    public isReadOnlyUser: boolean = false;
    @ViewChild(BulkCreditConfirmModal)
    private bulkCreditConfirmModal: BulkCreditConfirmModal;
    public routeDate: Date;
    private orderBy: OrderByExecutor;

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private exceptionDeliveryService: ExceptionDeliveryService,
        private accountService: AccountService,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private refreshService: RefreshService,
        private toasterService: ToasterService,
        private securityService: SecurityService,
        private nqps: NavigateQueryParametersService,
        private deliveryService: DeliveryService)
    {
        super(nqps);

        this.options = [
            this.routeOption,
            new DropDownItem('Branch', 'branchId', false, 'number'),
            new DropDownItem('Invoice No', 'invoiceNumber'),
            new DropDownItem('Account', 'accountCode'),
            new DropDownItem('Account Name', 'accountName'),
            this.assigneeOption,
            new DropDownItem('Date', 'deliveryDate', false, 'date'),
            new DropDownItem('Credit Value', 'totalCreditValueForThreshold', false, 'numberLessThanOrEqual')
        ];
        this.sortField = 'deliveryDate'
        this.orderBy = new OrderByExecutor();
    }

    public ngOnInit(): void
    {
        super.ngOnInit();

        this.securityService.validateUser(
            this.globalSettingsService.globalSettings.permissions,
            this.securityService.actionDeliveries);
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getExceptions());
        this.activatedRoute.queryParams.subscribe(params =>
        {
            this.routeDate = params['routeDate'];
            this.outstandingFilter = params['outstanding'] === 'true';
            this.getExceptions();
            this.getThresholdLimit();
        });

        this.isReadOnlyUser = this.securityService
            .hasPermission(this.globalSettingsService.globalSettings.permissions, this.securityService.readOnly);
        
        this.deliveryService.getDamageReasons()
            .subscribe(r => { this.bulkCreditConfirmModal.reasons = r; });

        this.deliveryService.getSources()
            .subscribe(s => { this.bulkCreditConfirmModal.sources = s });
    }

    public ngOnDestroy()
    {
        super.ngOnDestroy();
        this.refreshSubscription.unsubscribe();
    }

    public deliveryLinesSaved()
    {
        this.getExceptions();
    }

    public getExceptions()
    {
        this.exceptionDeliveryService.getExceptions()
            .subscribe(responseData =>
                {
                    this.exceptions = responseData || new Array<ExceptionDelivery>();

                    if (!_.isUndefined(this.routeDate)) {
                        this.exceptions = _.filter(this.exceptions,
                            x => {
                                return x.routeDate === this.routeDate;
                            }
                        );
                    }
                    
                    this.lastRefresh = Date.now();
                    this.isLoading = false;
                },
                error =>
                {
                    if (error.status && error.status === 404)
                    {
                        this.lastRefresh = Date.now();
                    }
                    this.isLoading = false;
                });
    }

    public getThresholdLimit()
    {
        this.exceptionDeliveryService.getUserCreditThreshold()
            .subscribe(responseData =>
            {
                this.threshold = responseData[0];
            });
    }
    public onSortDirectionChanged(isDesc: boolean)
    {   
        super.onSortDirectionChanged(isDesc);
        this.exceptions = this.orderBy.Order(this.exceptions, this);
    }

    public onFilterClicked(filterOption: FilterOption)
    {
        this.bulkCredits = new Array<number>();
        super.onFilterClicked(filterOption);
    }

    public onOutstandingClicked(showOutstandingOnly: boolean)
    {
        this.outstandingFilter = showOutstandingOnly;
    }

    public isAboveThresholdLimit(amount)
    {
        return parseFloat(amount) > this.threshold;
    }

    public isChecked(exceptionid)
    {
        if (this.getCreditListIndex(exceptionid) == -1)
        {
            return '';
        }

        return'checked';
    }

    public creditListlength()
    {
        return this.bulkCredits.length;
    }

    public onCheck(exception)
    {
        const creditListIndex = this.getCreditListIndex(exception.id);

        if (creditListIndex === -1)
        {
            this.bulkCredits.push(exception.id);
        } else
        {
            this.removeFromCreditList(creditListIndex);
        }
    }

    public addToCreditList(exception, index)
    {
        if (index === -1)
        {
            exception.isPending = this.isAboveThresholdLimit(exception.totalCreditValueForThreshold);
            this.bulkCredits.push(exception);
        }
    }

    public isGridCheckBoxDisabled(exceptionid)
    {
        const exceptionDelivery = _.find(this.exceptions, ['id', exceptionid]);

        if (exceptionDelivery.assigned == this.globalSettingsService.globalSettings.userName
            && exceptionDelivery.canBulkCredit)
        {
            return '';
        }

        return 'disabled';
    }

    public checkExceptionsForCredit()
    {
        if (this.bulkCredits !== [])
        {
            this.creditExceptions();
        } else
        {
            this.toasterService.pop(
                'error',
                'No Delivery line(s) selected for credit. Please select at least one Delivery line.',
                '');
        }
    }

    public creditExceptions()
    {
        this.bulkCreditConfirmModal.isVisible = true;
    }

    public creditConfirmed(model: any)
    {
        const bulkCredit = new BulkCredit();
        bulkCredit.jobIds = this.bulkCredits;
        bulkCredit.reason = model.reason;
        bulkCredit.source = model.source;

        this.exceptionDeliveryService.creditLines(bulkCredit)
            .subscribe((res: Response) =>
            {
                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success)
                {
                    this.toasterService.pop('success', this.bulkCredits.length + ' Delivery line(s) credited', '');
                } else if (this.httpResponse.notAcceptable)
                {
                    this.toasterService.pop('error', this.httpResponse.message, '');
                }

                this.getExceptions();
                this.bulkCredits = [];
            });
    }

    public cancel()
    {
        this.router.navigate(['/delivery', this.delivery.id]);
    }

    public deliverySelected(delivery): void
    {
        this.router.navigate(['/delivery', delivery.id]);
    }

    public openModal(accountId): void
    {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account =>
                {
                    this.account = account;
                    this.contactModal.show(this.account);
                },
                error => this.errorMessage = <any>error);
    }

    public allocateUser(delivery: ExceptionDelivery): void
    {
        this.assignModal.show(delivery);
    }

    public onAssigned($event)
    {
        if ($event.delivery)
        {
            const creditListIndex = this.getCreditListIndex($event.delivery.id);

            if (creditListIndex !== -1)
            {
                this.removeFromCreditList($event.delivery);
            }
        }
        this.getExceptions();
    }

    public submit(delivery: ExceptionDelivery): void
    {
        this.exceptionDeliveryService.getConfirmationDetails(delivery.id)
            .subscribe((deliveryAction: DeliveryAction) =>
            {
                this.exceptionConfirmModal.show(deliveryAction);
            });
    }

    public canSubmit(canSubmitDelivery: boolean): boolean
    {
        return canSubmitDelivery &&
            this.securityService.hasPermission(this.globalSettingsService.globalSettings.permissions,
                this.securityService.actionDeliveries);
    }

    public getCreditListIndex(exceptionid)
    {
        return _.findIndex(this.bulkCredits, { id: exceptionid });
    }

    public removeFromCreditList(index)
    {
        if (index !== -1)
        {
            this.bulkCredits.splice(index, 1);
        }
    }
}