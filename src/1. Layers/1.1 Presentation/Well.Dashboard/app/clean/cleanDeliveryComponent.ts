import { Component, OnInit, ViewChild}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import {Router} from '@angular/router';
import 'rxjs/Rx';   // Load all features

import {PaginatePipe, PaginationControlsCmp, PaginationService } from 'ng2-pagination';
import {CleanDelivery} from './cleanDelivery';
import {CleanDeliveryService} from './cleanDeliveryService';
import {OptionFilterComponent} from '../shared/optionfilter.component';
import {OptionFilterPipe } from '../shared/optionFilterPipe';
import {FilterOption} from "../shared/filterOption";
import {DropDownItem} from "../shared/dropDownItem";
import {ContactModal} from "../shared/contact-modal";
import {AccountService} from "../account/accountService";
import {IAccount} from "../account/account";

@Component({
    selector: 'ow-clean',
    templateUrl: './app/clean/cleanDelivery-list.html',
    providers: [HTTP_PROVIDERS, GlobalSettingsService, CleanDeliveryService, PaginationService, AccountService],
    directives: [OptionFilterComponent, PaginationControlsCmp, ContactModal],
    pipes: [OptionFilterPipe, PaginatePipe]

})
export class CleanDeliveryComponent implements OnInit {
    errorMessage: string;
    cleanDeliveries: CleanDelivery[];
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
    account: IAccount;

    constructor(
        private cleanDeliveryService: CleanDeliveryService,
        private accountService: AccountService,
        private router: Router) { }

    ngOnInit(): void {
        this.cleanDeliveryService.getCleanDeliveries()
            .subscribe(cleanDeliveries => this.cleanDeliveries = cleanDeliveries,
            error => this.errorMessage = <any>error);
    }

    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    deliverySelected(delivery): void {
        this.router.navigate(['/delivery', delivery.id]);
    }

    @ViewChild(ContactModal) modal = new ContactModal();

    openModal(accountId): void {
        this.accountService.getAccountByAccountId(accountId)
            .subscribe(account => { this.account = account; this.modal.show(this.account);},
            error => this.errorMessage = <any>error);
    }
}
