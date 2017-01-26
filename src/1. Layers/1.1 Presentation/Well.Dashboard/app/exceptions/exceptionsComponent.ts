import { Component, OnInit, ViewChild }     from '@angular/core';
import { Router, ActivatedRoute}            from '@angular/router';
import { Response }                         from '@angular/http';
import { GlobalSettingsService }            from '../shared/globalSettings';
import { LogService }                       from '../shared/logService';
import 'rxjs/Rx';   // Load all features
import {BranchService}                      from '../shared/branch/branchService';
import {FilterOption}                       from '../shared/filterOption';
import {DropDownItem}                       from '../shared/dropDownItem';
import {ContactModal}                       from '../shared/contactModal';
import {AccountService}                     from '../account/accountService';
import {IAccount}                           from '../account/account';
import {ExceptionDelivery}                  from './exceptionDelivery';
import {ExceptionDeliveryService}           from './exceptionDeliveryService';
import {RefreshService}                     from '../shared/refreshService';
import {HttpResponse}                       from '../shared/httpResponse';
import {AssignModal}                        from '../shared/assignModal';
import {ConfirmModal}                       from '../shared/confirmModal';
import {IUser}                              from '../shared/user';
import {CreditItem}                         from '../shared/creditItem';
import {ToasterService}                     from 'angular2-toaster/angular2-toaster';
import {SecurityService}                    from '../shared/security/securityService';
import { Threshold } from '../shared/threshold';
import { DeliveryLine } from '../delivery/model/deliveryLine'; 
import { ExceptionsConfirmModal } from './exceptionsConfirmModal';
import * as lodash                          from 'lodash';

@Component({
    selector: 'ow-exceptions',
    templateUrl: './app/exceptions/exceptions-list.html',
    providers: [ExceptionDeliveryService]
})

export class ExceptionsComponent implements OnInit {
    public isLoading: boolean = true;
    public refreshSubscription: any;
    public errorMessage: string;
    public exceptions: ExceptionDelivery[];
    public currentConfigSort: string;
    public rowCount: number = 10;
    public filterOption: FilterOption = new FilterOption();
    public routeOption = new DropDownItem('Route', 'routeNumber');
    public assigneeOption = new DropDownItem('Assignee', 'assigned');
    public options: DropDownItem[] = [
        this.routeOption,
        new DropDownItem('Invoice No', 'invoiceNumber'),
        new DropDownItem('Account', 'accountCode'),
        new DropDownItem('Account Name', 'accountName'),
        this.assigneeOption,
        new DropDownItem('Date', 'deliveryDate', false, 'date'),
        new DropDownItem('Credit Threshold', 'totalCreditValueForThreshold', false, 'numberLessThanOrEqual')
    ];
    public account: IAccount;
    public lastRefresh = Date.now();
    public httpResponse: HttpResponse = new HttpResponse();
    public users: IUser[];
    public delivery: ExceptionDelivery;
    public routeId: string;
    public assignee: string;
    public selectedOption: DropDownItem;
    public selectedFilter: string;
    public outstandingFilter: boolean = false;
    public bulkCredits: ExceptionDelivery[];
    public threshold: number;
    @ViewChild(AssignModal)
    private assignModal: AssignModal;
    public value: string;
    public confirmMessage: string;
    public confirmModalIsVisible: boolean = false;
    public selectGridBox: boolean = false;
    @ViewChild(ConfirmModal) private confirmModal: ConfirmModal;
    @ViewChild(ContactModal) private contactModal: ContactModal;
    @ViewChild(ExceptionsConfirmModal) private exceptionConfirmModal: ExceptionsConfirmModal;
    public thresholdLimit: Threshold;
    public isReadOnlyUser: boolean = false;

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private exceptionDeliveryService: ExceptionDeliveryService,
        private accountService: AccountService,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private refreshService: RefreshService,
        private toasterService: ToasterService,
        private securityService: SecurityService,
        private logService: LogService,
        private branchService: BranchService ) {
    }

    public ngOnInit(): void {
        this.securityService.validateUser(
            this.globalSettingsService.globalSettings.permissions,
            this.securityService.actionDeliveries);
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getExceptions());
        this.activatedRoute.queryParams.subscribe(params => {
            this.routeId = params['route'];
            this.assignee = params['assignee'];
            this.outstandingFilter = params['outstanding'] === 'true';
            this.getExceptions();
            this.getThresholdLimit();
            this.bulkCredits = new Array<ExceptionDelivery>();
        });

        this.isReadOnlyUser = this.securityService
            .hasPermission(this.globalSettingsService.globalSettings.permissions, this.securityService.readOnly);
    }

    public ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    public getExceptions() {
        this.exceptionDeliveryService.getExceptions()
            .subscribe(responseData => {
                    this.exceptions = responseData;
                    this.lastRefresh = Date.now();

                    if (this.routeId) {
                        this.filterOption = new FilterOption(this.routeOption, this.routeId);
                        this.selectedOption = this.routeOption;
                        this.selectedFilter = this.routeId;
                    }
                    if (this.assignee) {
                        this.filterOption = new FilterOption(this.assigneeOption, this.assignee);
                        this.selectedOption = this.assigneeOption;
                        this.selectedFilter = this.assignee;
                    }

                    this.isLoading = false;
                },
                error => {
                    if (error.status && error.status === 404) {
                        this.lastRefresh = Date.now();
                    }
                    this.isLoading = false;
                });
    }

    public getThresholdLimit() {

        this.exceptionDeliveryService.getUserCreditThreshold()
            .subscribe(responseData => {
                this.threshold = responseData[0];
            });
    }

    public sortDirection(sortDirection): void {
        this.currentConfigSort = sortDirection === true ? '+deliveryDate' : '-deliveryDate';
        const sortString = this.currentConfigSort === '+dateTime' ? 'asc' : 'desc';
        this.getExceptions();
        lodash.sortBy(this.exceptions, ['dateTime'], [sortString]);
    }

    public onSortDirectionChanged(isDesc: boolean) {
        this.sortDirection(isDesc);
    }
    
    public onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
        this.bulkCredits = [];
    }

    public onOutstandingClicked(showOutstandingOnly: boolean) {
        this.outstandingFilter = showOutstandingOnly;
    }

    public isAboveThresholdLimit(amount) {
        return parseFloat(amount) > this.threshold;
    }

    public isChecked(exceptionid) {

        if (this.getCreditListIndex(exceptionid) == -1) {
            return '';
        }

        return'checked'; 
    }

    public creditListlength() {
        return this.bulkCredits.length;
    }

    public onCheck(exception) {
        const creditListIndex = this.getCreditListIndex(exception.id);

        if (creditListIndex === -1) {
            this.addToCreditList(exception, creditListIndex);
        } else {
            this.removeFromCreditList(exception);
        }
    }

    public getCreditListIndex(exceptionid) {
        return lodash.findIndex(this.bulkCredits, { id: exceptionid});
    }
    
    public addToCreditList(exception, index) {

        if (index === -1) {
            exception.isPending = this.isAboveThresholdLimit(exception.totalCreditValueForThreshold);
            this.bulkCredits.push(exception);
        }       
    }

    public removeFromCreditList(index) {

        if (index !== -1) {
            this.bulkCredits.splice(index, 1);
        }
    }

    public isGridCheckBoxDisabled(exceptionid) {
        const exceptionDelivery = lodash.find(this.exceptions, ['id', exceptionid]);

        if (exceptionDelivery.assigned === this.globalSettingsService.globalSettings.userName) {
            return '';
        }

        return 'disabled';
    }

    public checkExceptionsForCredit() {
        if (this.bulkCredits !== []) {
            this.creditExceptions();
        } else {
            this.toasterService.pop(
                'error',
                'No Delivery line(s) selected for credit. Please select at least one Delivery line.',
                '');
        }
    }

    public creditExceptions() {

        const pendingLength = lodash.filter(this.bulkCredits, o => {
            if (o.isPending === true) {
                return o
            }
        }).length;

        const creditLength = lodash.filter(this.bulkCredits, o => {
            if (o.isPending === false) {
                return o
            }
        }).length;

        const approvalConfirm = pendingLength > 0
            ? ' and ' + pendingLength + ' pending exceptions '
            : '';

        this.confirmModal.isVisible = true;
        this.confirmModal.heading = 'Bulk credit exceptions?';
        this.confirmModal.messageHtml =
            'You are about to bulk credit the exceptions of ' + creditLength + ' invoices ' + approvalConfirm +
            'Are you sure you want to continue?';
        return;
    }

    public creditConfirmed() {

        this.paginationCount();
        
        this.exceptionDeliveryService.creditLines(this.bulkCredits)
            .subscribe((res: Response) => {

                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', this.bulkCredits.length + ' Delivery line(s) credited', '');

                    this.getExceptions();
                    this.bulkCredits = [];
                } else if (this.httpResponse.adamdown) {
                    if (this.httpResponse.adamdown) {
                        this.toasterService.pop(
                            'error',
                            'ADAM is currently offline!',
                            'You will receive a notification once the credit has taken place!');
                    }
                } else if (this.httpResponse.notAcceptable) {
                    this.toasterService.pop('error', this.httpResponse.message, '');
                } else if (this.httpResponse.adamPartProcessed) {
                    this.toasterService.pop(
                        'error',
                        'the credit has been part processed!',
                        'You will receive a notification once the credit is complete!');
                }
            });
    }

    public cancel() {
        this.router.navigate(['/delivery', this.delivery.id]);
    }

    public deliverySelected(delivery): void {
        this.router.navigate(['/delivery', delivery.id]);
    }

    public openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => {
                    this.account = account;
                    this.contactModal.show(this.account);
                },
                error => this.errorMessage = <any>error);
    }

    public allocateUser(delivery: ExceptionDelivery): void {
        this.assignModal.show(delivery);
    }
    
    public onAssigned($event) {

        if ($event.delivery) {
            const creditListIndex = this.getCreditListIndex($event.delivery.id);
            
            if (creditListIndex !== -1) {
                this.removeFromCreditList($event.delivery);
            }
        } 
        this.getExceptions();
    }

    public paginationCount() {

        if (this.exceptions.length % this.rowCount === 1) {
            location.reload();
        }       

    }

    public submit(delivery: ExceptionDelivery): void {
        this.exceptionDeliveryService.getConfirmationDetails(delivery.id)
            .subscribe((deliveryLines: DeliveryLine[]) => {
                this.exceptionConfirmModal.show(deliveryLines);
                /*this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', 'Exception has been credited!', '');
                }
                if (this.httpResponse.notAcceptable) {
                    this.toasterService.pop('error', this.httpResponse.message, '');
                }
                if (this.httpResponse.adamdown) {
                    this.toasterService.pop(
                        'error',
                        'ADAM is currently offline!',
                        'You will receive a notification once the credit has taken place!');
                }*/
            });
    }
}