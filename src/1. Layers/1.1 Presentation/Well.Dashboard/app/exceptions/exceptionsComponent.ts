import { Component, OnInit, ViewChild}  from '@angular/core';
import {Router} from '@angular/router';
import { Response } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features

import {PaginationService } from 'ng2-pagination';
import {FilterOption} from "../shared/filterOption";
import {DropDownItem} from "../shared/dropDownItem";
import {ContactModal} from "../shared/contact-modal";
import {AccountService} from "../account/accountService";
import {IAccount} from "../account/account";
import {ExceptionDelivery} from "./exceptionDelivery";
import {ExceptionDeliveryService} from "./exceptionDeliveryService";
import {RefreshService} from '../shared/refreshService';
import {HttpResponse} from '../shared/http-response';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {AssignModal} from "../shared/assign-Modal";
import {IUser} from "../shared/user";

@Component({
    selector: 'ow-exceptions',
    templateUrl: './app/exceptions/exceptions-list.html',
    providers: [GlobalSettingsService, ExceptionDeliveryService, PaginationService, AccountService]
})

export class ExceptionsComponent implements OnInit {
    refreshSubscription: any;
    errorMessage: string;
    exceptions: ExceptionDelivery[];
    currentConfigSort: string;
    rowCount: number = 10;
    filterOption: FilterOption = new FilterOption();
    options: DropDownItem[] = [
        new DropDownItem("Route", "routeNumber"),
        new DropDownItem("Invoice No", "invoiceNumber"),
        new DropDownItem("Account", "accountCode"),
        new DropDownItem("Account Name", "accountName"),
        new DropDownItem("Assignee", "assigned"),
        new DropDownItem("Date", "dateTime")
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

    constructor(
        private exceptionDeliveryService: ExceptionDeliveryService,
        private accountService: AccountService,
        private router: Router,
        private refreshService: RefreshService,
        private toasterService: ToasterService) {
    }

    ngOnInit(): void {
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getExceptions());
        this.getExceptions();
        this.currentConfigSort = '-dateTimeUpdated';
    }

    ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    getExceptions() {
        this.exceptionDeliveryService.getExceptions()
            .subscribe(responseData => {
                this.exceptions = responseData;
                this.lastRefresh = Date.now();
            },
                error => {
                    if (error.status && error.status === 404) {
                        this.lastRefresh = Date.now();
                    }
                });
    }

    sortDirection(sortDirection): void {
        console.log(sortDirection);
        this.currentConfigSort = sortDirection === true ? '+dateTime' : '-dateTime';
        console.log(this.currentConfigSort);
        this.getExceptions();
    }

    
    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    deliverySelected(delivery): void {
        this.router.navigate(['/delivery', delivery.id, delivery.canAction]);
    }

    @ViewChild(ContactModal) modal = new ContactModal();

    openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => { this.account = account; this.modal.show(this.account); },
            error => this.errorMessage = <any>error);
    }

    
    @ViewChild(AssignModal) assignModal = new AssignModal(this.exceptionDeliveryService,this.router, this.toasterService);

    openAssignModal(delivery): void {

        this.exceptionDeliveryService.getUsersForBranch(delivery.branchId)
            .subscribe(users => {
                this.users = users;
                this.assignModal.show(this.users, delivery);
            }, 
            error => this.errorMessage = <any>error);
    }

    onAssigned(assigned: boolean) {
        this.getExceptions();
    }

    allocateUser(delivery: ExceptionDelivery): void {
        this.openAssignModal(delivery);
    }

    setSelectedAction(delivery: ExceptionDelivery, action: DropDownItem): void {
        switch (action.value) {
            case 'credit':
                this.exceptionDeliveryService.credit(delivery)
                    .subscribe((res: Response) => {
                        this.httpResponse = JSON.parse(JSON.stringify(res));

                        if (this.httpResponse.success) this.toasterService.pop('success', 'Exception has been credited!', '');
                        if (this.httpResponse.adamdown) this.toasterService.pop('error', 'ADAM is currently offline!', 'You will receive a notification once the credit has taken place!');
                    });
                break;
            case 'credit-reorder':
                // do something els
                break;
        }
    }

}