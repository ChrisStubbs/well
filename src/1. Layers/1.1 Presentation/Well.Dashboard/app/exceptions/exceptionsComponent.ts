﻿import { Component, OnInit, ViewChild }  from '@angular/core';
import {Router, ActivatedRoute} from '@angular/router';
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
import {AssignModal} from "../shared/assign-Modal";
import {IUser} from "../shared/user";
import {ToasterService} from 'angular2-toaster/angular2-toaster';

@Component({
    selector: 'ow-exceptions',
    templateUrl: './app/exceptions/exceptions-list.html',
    providers: [ExceptionDeliveryService, PaginationService, AccountService]
})

export class ExceptionsComponent implements OnInit {
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
        new DropDownItem("Date", "deliveryDate")
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

    constructor(
        private exceptionDeliveryService: ExceptionDeliveryService,
        private accountService: AccountService,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private refreshService: RefreshService,
        private toasterService: ToasterService) {
    }

    ngOnInit(): void {
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getExceptions());
        this.currentConfigSort = '-deliveryDate';

        this.activatedRoute.queryParams.subscribe(params => {
            this.routeId = params['route'];
            this.assignee = params['assignee'];
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
                },
                error => {
                    if (error.status && error.status === 404) {
                        this.lastRefresh = Date.now();
                    }
                });
    }

    sortDirection(sortDirection): void {
        console.log(sortDirection);
        this.currentConfigSort = sortDirection === true ? '+deliveryDate' : '-deliveryDate';
        console.log(this.currentConfigSort);
        this.getExceptions();
    }
    
    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    deliverySelected(delivery): void {
        this.router.navigate(['/delivery', delivery.id]);
    }

    @ViewChild(ContactModal) modal : ContactModal;

    openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => { this.account = account; this.modal.show(this.account); },
            error => this.errorMessage = <any>error);
    }
    
    @ViewChild(AssignModal) assignModal : AssignModal;

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