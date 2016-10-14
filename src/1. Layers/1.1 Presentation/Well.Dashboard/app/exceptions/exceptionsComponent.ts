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
import {OrderArrowComponent} from '../shared/orderbyArrow';
import {CodComponent} from '../shared/codComponent';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {SecurityService} from '../shared/security/securityService';
import {UnauthorisedComponent} from '../unauthorised/unauthorisedComponent';
import * as lodash from 'lodash';
import * as jquery from 'jquery';

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
    deliveryCredits:any[];
    @ViewChild(AssignModal)
    private assignModal: AssignModal;
    value: string;
    creditTitle:string;
    confirmMessage: string;
    confirmModalIsVisible: boolean = false;
    selectGridBox: boolean = false;
    @ViewChild(ConfirmModal) private confirmModal: ConfirmModal;
    @ViewChild(ContactModal)
    private contactModal: ContactModal;

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
        this.currentConfigSort = '-dateTime';
        this.sortDirection(false);
        this.activatedRoute.queryParams.subscribe(params => {
            this.routeId = params['route'];
            this.assignee = params['assignee'];
            this.outstandingFilter = params['outstanding'] === 'true';
            this.deliveryCredits = [];
            this.getExceptions();    
        });
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
        this.deliveryCredits = [];
    }

    onOutstandingClicked(showOutstandingOnly: boolean) {
        this.outstandingFilter = showOutstandingOnly;
    }

    isChecked(exceptionid) {
        var creditListIndex = this.getCreditListIndex(exceptionid);

        if (creditListIndex === -1) {
            return '';
        } else {
            return'checked'; 
        }
    }

    onCheck(exceptionid) {
        var creditListIndex = this.getCreditListIndex(exceptionid);

        if (creditListIndex === -1) {
            this.addToCreditList(exceptionid, creditListIndex);
        } else {
            this.removeFromCreditList(exceptionid, creditListIndex);
        }
        this.creditTitle = this.deliveryCredits.length > 1 ? "Bulk Credit" : "Credit";
    }

    getCreditListIndex(exceptionid) {
        return this.deliveryCredits.indexOf(exceptionid);
    }

    selectAllCredits() {

        this.deliveryCredits = [];
        
        if (this.filterOption.dropDownItem.description === 'Credit Threshold' &&
            this.exceptions.length > 0 &&
            !isNaN(parseFloat(this.filterOption.filterText))) {

            var currentThreshold = parseFloat(this.filterOption.filterText);
            
            lodash.forEach(this.exceptions,
                value => {
                    if (value.totalCreditValueForThreshold <= currentThreshold && value.assigned !== 'Unallocated') {
                        this.deliveryCredits.push(value.id);
                    }
                });
        } else {

            lodash.forEach(this.exceptions,
                value => {
                    if (value.assigned !== 'Unallocated') {
                        this.deliveryCredits.push(value.id);
                    }
                });
        }
    }


    addToCreditList(exceptionId, index) {
        if (index === -1) {
            this.deliveryCredits.push(exceptionId);
      }
    }

    isGridCheckBoxDisabled(exceptionid) {
        var exceptionDelivery = lodash.find(this.exceptions, ['id', exceptionid]);
        if (exceptionDelivery.assigned !== 'Unallocated') {
            return '';
        }
        return 'disabled';
    }

    removeFromCreditList(exceptionId, index) {
        if (index !== -1) {
            this.deliveryCredits.splice(index, 1);
        }
    }

    checkExceptionsForCredit() {
        if (this.deliveryCredits !== []) {
            this.creditExceptions();
        } else {
            this.toasterService.pop('error', 'No Delivery line(s) selected for credit. Please select at least one Delivery line.', '');
        }
    }

    creditExceptions() {     
        this.confirmModal.isVisible = true;
        this.confirmModal.heading = this.creditTitle + " exceptions?";
        this.confirmModal.messageHtml =
            "You are about to " + this.creditTitle + " " + this.deliveryCredits.length + " exceptions " +
            "Are you sure you want to save your changes?";
        return;
    }

    creditConfirmed() {

        this.paginationCount();
        
        this.exceptionDeliveryService.creditLines(this.deliveryCredits)
            .subscribe((res: Response) => {

                this.httpResponse = JSON.parse(JSON.stringify(res));

                if (this.httpResponse.success) {
                    this.toasterService.pop('success', this.deliveryCredits.length + ' Delivery line(s) credited', '');
                    this.getExceptions();
                    this.deliveryCredits = [];
                } else {
                    this.toasterService.pop('error', 'Error crediting exceptions!', 'An error occured please contact support.');
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
        this.assignModal.show(delivery.id, delivery.branchId, delivery.accountCode);
    }

    openConfirmModal(delivery): void {
        
    }

    onAssigned($event) {

        var creditListIndex = this.getCreditListIndex($event.exceptionId);

        if (creditListIndex !== -1) {
            this.removeFromCreditList($event.exceptionId, creditListIndex);
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