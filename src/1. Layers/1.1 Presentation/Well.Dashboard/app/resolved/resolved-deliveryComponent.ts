import {Component, OnInit}  from 'angular2/core';
import {ROUTER_DIRECTIVES} from 'angular2/router';
import {PaginatePipe, PaginationControlsCmp, PaginationService} from 'ng2-pagination';
import {IResolvedDelivery} from './resolvedDelivery';
import {ResolvedDeliveryService} from './ResolvedDeliveryService';
import {OptionFilterComponent} from '../shared/optionfilter.component';
import {OptionFilterPipe } from '../shared/optionFilterPipe';
import {FilterOption} from "../shared/filterOption";
import {DropDownItem} from "../shared/DropDownItem";

@Component({
    templateUrl: './app/resolved/resolveddelivery-list.html',
    providers: [ResolvedDeliveryService, PaginationService],
    directives: [ROUTER_DIRECTIVES, PaginationControlsCmp, OptionFilterComponent],
    pipes: [PaginatePipe, OptionFilterPipe]
})
export class ResolvedDeliveryComponent implements OnInit {
    errorMessage: string;
    deliveries: IResolvedDelivery[];
    rowCount: number = 10;
    filterOption: FilterOption = new FilterOption();
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