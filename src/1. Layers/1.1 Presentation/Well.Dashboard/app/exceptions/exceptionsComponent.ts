import { Component, OnInit, ViewChild }  from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
import { Response } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import {LogService} from '../shared/logService';
import 'rxjs/Rx';   // Load all features

import {BranchService} from "../shared/branch/branchService";
import {FilterOption} from "../shared/filterOption";
import {DropDownItem} from "../shared/dropDownItem";
import {ContactModal} from "../shared/contactModal";
import {AccountService} from "../account/accountService";
import {IAccount} from "../account/account";
import {ExceptionDelivery} from "./exceptionDelivery";
import {ExceptionDeliveryService} from "./exceptionDeliveryService";
import {RefreshService} from '../shared/refreshService';
import {HttpResponse} from '../shared/httpResponse';
import {AssignModal} from "../shared/assignModal";
import {ConfirmModal} from "../shared/confirmModal";
import {IUser} from "../shared/user";
import {CreditItem} from "../shared/creditItem";
import {OrderArrowComponent} from '../shared/orderbyArrow';
import {CodComponent} from '../shared/codComponent';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {SecurityService} from '../shared/security/securityService';
import {UnauthorisedComponent} from '../unauthorised/unauthorisedComponent';
import {Threshold} from '../shared/threshold';
import * as lodash from 'lodash';

@Component({
    selector: 'ow-exceptions',
    templateUrl: './app/exceptions/exceptions-list.html',
    providers: [ExceptionDeliveryService]
})

export class ExceptionsComponent implements OnInit {
    isLoading: boolean = true;
    refreshSubscription: any;
    errorMessage: string;
    exceptions: ExceptionDelivery[];
    currentConfigSort: string;
    rowCount: number = 10;
    filterOption: FilterOption = new FilterOption();
    routeOption = new DropDownItem("Route", "routeNumber");
    assigneeOption = new DropDownItem("Assignee", "assigned");
    options: DropDownItem[] = [
        this.routeOption,
        new DropDownItem("Invoice No", "invoiceNumber"),
        new DropDownItem("Account", "accountCode"),
        new DropDownItem("Account Name", "accountName"),
        this.assigneeOption,
        new DropDownItem("Date", "deliveryDate", false, "date"),
        new DropDownItem("Credit Threshold", "totalCreditValueForThreshold", false, "numberLessThanOrEqual")
    ];
    defaultAction: DropDownItem = new DropDownItem("Action");
    actions: DropDownItem[] = [
        new DropDownItem("Credit", "credit"),
        new DropDownItem("Credit and Re-Order", "credit-reorder"),
        new DropDownItem("Re-plan in TranSend", "replan-transcend"),
        new DropDownItem("Re-plan in Roadnet", "replan-roadnet"),
        new DropDownItem("Re-plan in Queue", "replan-queue"),
        new DropDownItem("Reject - No Action", "reject")
    ];
    account: IAccount;
    lastRefresh = Date.now();
    httpResponse: HttpResponse = new HttpResponse();
    users: IUser[];
    delivery: ExceptionDelivery;
    routeId: string;
    assignee: string;
    selectedOption: DropDownItem;
    selectedFilter: string;
    outstandingFilter: boolean = false;
    bulkCredits: ExceptionDelivery[];
    threshold:number;
    @ViewChild(AssignModal)
    private assignModal: AssignModal;
    value: string;
    confirmMessage: string;
    confirmModalIsVisible: boolean = false;
    selectGridBox: boolean = false;
    @ViewChild(ConfirmModal) private confirmModal: ConfirmModal;
    @ViewChild(ContactModal) private contactModal: ContactModal;
    thresholdLimit: Threshold;
    isReadOnlyUser: boolean = false;

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

    ngOnInit(): void {
        this.securityService.validateUser(this.globalSettingsService.globalSettings.permissions, this.securityService.actionDeliveries);
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

    ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    getExceptions() {
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

    getThresholdLimit() {

        this.exceptionDeliveryService.getUserCreditThreshold(this.globalSettingsService.globalSettings.userName)
            .subscribe(responseData => {
                this.threshold = responseData[0];
               
            });
    }

    sortDirection(sortDirection): void {
        this.currentConfigSort = sortDirection === true ? '+deliveryDate' : '-deliveryDate';
        var sortString = this.currentConfigSort === '+dateTime' ? 'asc' : 'desc';
        this.getExceptions();
        lodash.sortBy(this.exceptions, ['dateTime'], [sortString]);
    }

    onSortDirectionChanged(isDesc: boolean) {
        this.sortDirection(isDesc);
    }
    
    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
        this.bulkCredits = [];
    }

    onOutstandingClicked(showOutstandingOnly: boolean) {
        this.outstandingFilter = showOutstandingOnly;
    }

    isAboveThresholdLimit(amount) {
        return parseFloat(amount) > this.threshold;
    }

    isChecked(exceptionid) {
        var creditListIndex = this.getCreditListIndex(exceptionid);
        
        if (creditListIndex === -1) {
            return '';
        } else {
            return'checked'; 
        }
    }

    creditListlength() {
        return this.bulkCredits.length;
    }

    onCheck(exception) {
        var creditListIndex = this.getCreditListIndex(exception.id);

        if (creditListIndex === -1) {
            this.addToCreditList(exception, creditListIndex);
        } else {
            this.removeFromCreditList(exception);
        }
    }

    getCreditListIndex(exceptionid) {
        return lodash.findIndex(this.bulkCredits, { id: exceptionid});
    }

    /*selectAllCredits() {

        this.bulkCredits = [];
        var creditListIndex = -1;

        if (this.filterOption.dropDownItem.description === 'Credit Threshold' &&
            this.exceptions.length > 0 &&
            !isNaN(parseFloat(this.filterOption.filterText))) {

            var currentThreshold = parseFloat(this.filterOption.filterText);
            
            lodash.forEach(this.exceptions,
                value => {
                    if (value.totalCreditValueForThreshold <= currentThreshold && value.assigned !== 'Unallocated') {
                        creditListIndex = this.getCreditListIndex(value.id);
                        this.addToCreditList(value, creditListIndex);
                    }
                });
        } else {

            lodash.forEach(this.exceptions,
                value => {
                    if (value.assigned !== 'Unallocated') {
                        creditListIndex = this.getCreditListIndex(value.id);
                        this.addToCreditList(value, creditListIndex);
                    }
                });
        }
    }*/
    
    addToCreditList(exception, index) {

        var isAboveThesholdLimit = this.isAboveThresholdLimit(exception.totalCreditValueForThreshold);

        if (index === -1) {
            exception.isPending = isAboveThesholdLimit;
            this.bulkCredits.push(exception);
        }       
    }

    removeFromCreditList(index) {

        if (index !== -1) {
            this.bulkCredits.splice(index, 1);
        }
    }

    isGridCheckBoxDisabled(exceptionid) {
        var exceptionDelivery = lodash.find(this.exceptions, ['id', exceptionid]);

        if (exceptionDelivery.assigned === this.globalSettingsService.globalSettings.userName) {
            return '';
        }

        return 'disabled';
    }

    checkExceptionsForCredit() {
        if (this.bulkCredits !== []) {
            this.creditExceptions();
        } else {
            this.toasterService.pop('error', 'No Delivery line(s) selected for credit. Please select at least one Delivery line.', '');
        }
    }

    creditExceptions() {

        var pendingLength = lodash.filter(this.bulkCredits, o => { if (o.isPending === true) return o }).length;
        var creditLength = lodash.filter(this.bulkCredits, o => { if (o.isPending === false) return o }).length;

        var approvalConfirm = pendingLength > 0
            ? " and " + pendingLength + " pending exceptions "
            : "";

        this.confirmModal.isVisible = true;
        this.confirmModal.heading = "Bulk credit exceptions?";
        this.confirmModal.messageHtml =
            "You are about to bulk credit " + creditLength + " exceptions " + approvalConfirm +
            "Are you sure you want to save your changes?";
        return;
    }

    creditConfirmed() {

        this.paginationCount();
        
        this.exceptionDeliveryService.creditLines(this.bulkCredits)
            .subscribe((res: Response) => {

                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', this.bulkCredits.length + ' Delivery line(s) credited', '');

                    this.getExceptions();
                    this.bulkCredits = [];
                } else if (this.httpResponse.adamdown) {
                    if (this.httpResponse.adamdown) this.toasterService.pop('error', 'ADAM is currently offline!', 'You will receive a notification once the credit has taken place!');
                } else if (this.httpResponse.notAcceptable) {
                    this.toasterService.pop('error', this.httpResponse.message, '');
                }
            });
    }

    cancel() {
        this.router.navigate(['/delivery', this.delivery.id]);
    }

    deliverySelected(delivery): void {
        this.router.navigate(['/delivery', delivery.id]);
    }

    openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => {
                    this.account = account;
                    this.contactModal.show(this.account);
                },
                error => this.errorMessage = <any>error);
    }

    allocateUser(delivery: ExceptionDelivery): void {
        this.assignModal.show(delivery);
    }

    openConfirmModal(delivery): void {
        
    }

    onAssigned($event) {

        if ($event.delivery) {
            var creditListIndex = this.getCreditListIndex($event.delivery.id);

            var exceptionDelivery = lodash.find(this.exceptions, ['id', $event.delivery.id]);

            if (creditListIndex !== -1) {
                this.removeFromCreditList($event.delivery);
            }
        } 
        this.getExceptions();
    }

    paginationCount() {
        var isLastExceptionOnPage = this.exceptions.length % this.rowCount === 1;

        if (isLastExceptionOnPage) {
            location.reload();
        }       

    }

    setSelectedAction(delivery: ExceptionDelivery, action: DropDownItem): void {
        switch (action.value) {
            case 'credit':
                this.exceptionDeliveryService.credit(delivery)
                    .subscribe((res: Response) => {
                        this.httpResponse = JSON.parse(JSON.stringify(res));

                        if (this.httpResponse.success) this.toasterService.pop('success', 'Exception has been credited!', '');
                        if (this.httpResponse.notAcceptable) this.toasterService.pop('error', this.httpResponse.message, '');
                        if (this.httpResponse.adamdown) this.toasterService.pop('error', 'ADAM is currently offline!', 'You will receive a notification once the credit has taken place!');
                    });
                break;
            case 'credit-reorder':
                // do something els
                break;
        }
    }

}