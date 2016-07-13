import { Component } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router-deprecated';
import {PaginatePipe, PaginationControlsCmp, PaginationService} from 'ng2-pagination';
import {ICleanDelivery} from './cleanDelivery';
import {CleanDeliveryService} from './cleanDeliveryService';
import {OptionFilterComponent} from '../shared/optionfilter.component';
import {OptionFilterPipe } from '../shared/optionFilterPipe';
import {FilterOption} from "../shared/filterOption";
import {DropDownItem} from "../shared/DropDownItem";

@Component({

    templateUrl: './app/clean/cleanDelivery-list.html',
    providers: [CleanDeliveryService, PaginationService],
    directives: [ROUTER_DIRECTIVES, PaginationControlsCmp, OptionFilterComponent],
    pipes: [PaginatePipe, OptionFilterPipe]
    
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

}
