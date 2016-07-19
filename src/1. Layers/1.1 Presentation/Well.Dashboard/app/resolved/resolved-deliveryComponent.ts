import { Component, OnInit}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features

import {PaginatePipe, PaginationControlsCmp, PaginationService } from 'ng2-pagination';
import {IResolvedDelivery} from './resolvedDelivery';
import {ResolvedDeliveryService} from './ResolvedDeliveryService';
import {OptionFilterComponent} from '../shared/optionfilter.component';
import {OptionFilterPipe } from '../shared/optionFilterPipe';
import {DropDownItem} from "../shared/DropDownItem";
import Option = require("../shared/filterOption");
import FilterOption = Option.FilterOption;

@Component({
    selector: 'ow-resolved',
    templateUrl: './app/resolved/resolveddelivery-list.html',
    providers: [HTTP_PROVIDERS, GlobalSettingsService, ResolvedDeliveryService, PaginationService],
    directives: [OptionFilterComponent, PaginationControlsCmp],
    pipes: [OptionFilterPipe, PaginatePipe]
})
export class ResolvedDeliveryComponent implements OnInit {
    errorMessage: string;
    deliveries: IResolvedDelivery[];
    rowCount: number = 10;
    filterOption: Option.FilterOption = new FilterOption();
    options: DropDownItem[] = [
        new DropDownItem("Route", "routeNumber"),
        new DropDownItem("Drop", "dropId"),
        new DropDownItem("Invoice No", "invoiceNumber"),
        new DropDownItem("Account", "accountCode"),
        new DropDownItem("Account Name", "accountName"),
        new DropDownItem("Status", "jobStatus"),
        new DropDownItem("Action", "action"),
        new DropDownItem("Assigned", "assigned"),
        new DropDownItem("Date", "dateTime")
    ];
    constructor(private resolvedDeliveryService: ResolvedDeliveryService) { }

    ngOnInit() {

        this.resolvedDeliveryService.getResolvedDeliveries()
            .subscribe(deliveries => this.deliveries = deliveries, error => this.errorMessage = <any>error);
    }

    deliverySelected(delivery: IResolvedDelivery): void {
        console.log(delivery.accountName);
    }

    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

}