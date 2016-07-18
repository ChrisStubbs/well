import { Component, OnInit } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router-deprecated';
import {PaginatePipe, PaginationControlsCmp, PaginationService } from 'ng2-pagination';
//import {MODAL_DIRECTIVES, ModalComponent} from 'ng2-bs3-modal/ng2-bs3-modal';
import {ICleanDelivery} from './cleanDelivery';
import {CleanDeliveryService} from './cleanDeliveryService';
import {OptionFilterComponent} from '../shared/optionfilter.component';
import {OptionFilterPipe } from '../shared/optionFilterPipe';
import {FilterOption} from "../shared/filterOption";
import {DropDownItem} from "../shared/DropDownItem";
import {AccountComponent} from "../account/AccountComponent";

@Component({

    templateUrl: './app/clean/cleanDelivery-list.html',
    providers: [CleanDeliveryService, PaginationService],
    directives: [ROUTER_DIRECTIVES, OptionFilterComponent, PaginationControlsCmp, AccountComponent],//, MODAL_DIRECTIVES],
    pipes: [OptionFilterPipe, PaginatePipe]
})
export class CleanDeliveryComponent implements OnInit {
    errorMessage: string;
    cleanDeliveries: ICleanDelivery;
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

    constructor(private cleanDeliveryService: CleanDeliveryService) { }

    ngOnInit(): void {
        this.cleanDeliveryService.getCleanDeliveries()
            .subscribe(cleanDeliveries => this.cleanDeliveries = cleanDeliveries,
                error => this.errorMessage = <any>error);
    }

    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

    openModal(delivery: ICleanDelivery): void {
    }
}
