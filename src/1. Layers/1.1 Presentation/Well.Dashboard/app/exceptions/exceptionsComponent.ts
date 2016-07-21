import { Component, OnInit, ViewChild}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features

import {PaginatePipe, PaginationControlsCmp, PaginationService } from 'ng2-pagination';
import {OptionFilterComponent} from '../shared/optionfilter.component';
import {OptionFilterPipe } from '../shared/optionFilterPipe';
import {FilterOption} from "../shared/filterOption";
import {DropDownItem} from "../shared/DropDownItem";
import {ContactModal} from "../shared/contact-modal";
import {AccountService} from "../account/accountService";
import {IAccount} from "../account/account";
import {IExceptionDelivery} from "./exceptionDelivery";
import {ExceptionDeliveryService} from "./exceptionDeliveryService";

@Component({
    selector: 'ow-exceptions',
    templateUrl: './app/exceptions/exceptions-list.html',
    providers: [HTTP_PROVIDERS, GlobalSettingsService, ExceptionDeliveryService, PaginationService, AccountService],
    directives: [OptionFilterComponent, PaginationControlsCmp, ContactModal],
    pipes: [OptionFilterPipe, PaginatePipe]
})
export class ExceptionsComponent implements OnInit {
    errorMessage: string;
    exceptions: IExceptionDelivery[];
    rowCount: number = 10;
    filterOption: FilterOption = new FilterOption();
    options: DropDownItem[] = [
        new DropDownItem("Route", "routeNumber"),
        new DropDownItem("Drop", "dropId"),
        new DropDownItem("Invoice No", "invoiceNumber"),
        new DropDownItem("Account", "accountCode"),
        new DropDownItem("Account Name", "accountName"),
        new DropDownItem("Date", "dateTime")
    ];
    defaultAction: DropDownItem = new DropDownItem("Action");
    actions: DropDownItem[] = [
        new DropDownItem("Assign", "#"),
        new DropDownItem("Credit", "#"),
        new DropDownItem("Credit and Re-Order", "#"),
        new DropDownItem("Re-Plan", "#"),
        new DropDownItem("Future Re-plan", "#"),
        new DropDownItem("No Action", "#")
    ];
    account: IAccount;

    constructor(private exceptionDeliveryService: ExceptionDeliveryService, private accountService: AccountService) { }
       
    ngOnInit(): void {
        this.exceptionDeliveryService.getExceptions()
            .subscribe(exceptions => this.exceptions = exceptions,
            error => this.errorMessage = <any>error);
    }

    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    deliverySelected(delivery: IExceptionDelivery): void {
        console.log(delivery.accountName);
    }

    @ViewChild(ContactModal) modal = new ContactModal();

    openModal(accountId): void {

        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => { this.account = account; this.modal.show(this.account); },
            error => this.errorMessage = <any>error);
    }

    setSelectedAction(delivery: IExceptionDelivery, action: DropDownItem): void {
        delivery.action = action.description;
        //this.selectedAction = action;
    }

}