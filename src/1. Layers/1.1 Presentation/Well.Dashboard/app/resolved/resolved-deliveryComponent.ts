import {Component, OnInit}  from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router-deprecated';
import {PAGINATION_DIRECTIVES} from 'ng2-bootstrap';
import {IResolvedDelivery} from './resolvedDelivery';
import {ResolvedDeliveryService} from './ResolvedDeliveryService';
import {OptionFilterComponent} from '../shared/optionfilter.component';
import {OptionFilterPipe } from '../shared/optionFilterPipe';
import {MODAL_DIRECTIVES} from 'ng2-bootstrap';
import {DropDownItem} from "../shared/DropDownItem";

@Component({
    templateUrl: './app/resolved/resolveddelivery-list.html',
    providers: [ResolvedDeliveryService, GlobalSettingsService],
    directives: [ROUTER_DIRECTIVES],
    pipes: [ResolvedDeliveryFilterPipe]
})
export class ResolvedDeliveryComponent implements OnInit {
    errorMessage: string;
    deliveries: IResolvedDelivery[];
    rowCount: number = 10;
    filterOption: FilterOption = new FilterOption();
    options: DropDownItem[] = [
        new DropDownItem("Route", "route"),
        new DropDownItem("Drop", "drop"),
        new DropDownItem("Invoice No", "invoiceNo"),
        new DropDownItem("Account", "account"),
        new DropDownItem("Account Name", "accountName"),
        new DropDownItem("Status", "status"),
        new DropDownItem("Action", "action"),
        new DropDownItem("Assigned", "assigned"),
        new DropDownItem("Date", "dateTime")
    ];
    constructor(private resolvedDeliveryService: ResolvedDeliveryService) { }

    ngOnInit() {

        this.resolvedDeliveryService.getResolvedDeliveries()
            .subscribe(deliveries => this.deliveries = deliveries, error => this.errorMessage = <any>error);
    }

    clearFilterText(): void {
        this.filterText = '';
    }

    deliverySelected(delivery : IResolvedDelivery): void {
        console.log(delivery.accountName);
    }

    onFilterClicked(filterOption: FilterOption) {
        this.filterOption = filterOption;
    }

}