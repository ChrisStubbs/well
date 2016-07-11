import {Component, OnInit}  from 'angular2/core';
import {ROUTER_DIRECTIVES} from 'angular2/router';
import {PaginatePipe, PaginationControlsCmp, PaginationService} from 'ng2-pagination';
import {IResolvedDelivery} from './resolvedDelivery';
import {ResolvedDeliveryService} from './ResolvedDeliveryService';
import {ResolvedDeliveryFilterPipe } from './resolvedDeliveryFilterPipe';
import {GlobalSettingsService} from '../shared/globalSettings';

@Component({
    templateUrl: './app/resolved/resolveddelivery-list.html',
    providers: [ResolvedDeliveryService, PaginationService, GlobalSettingsService],
    directives: [ROUTER_DIRECTIVES, PaginationControlsCmp],
    pipes: [PaginatePipe, ResolvedDeliveryFilterPipe]
})
export class ResolvedDeliveryComponent implements OnInit {
    errorMessage: string;
    deliveries: IResolvedDelivery[];
    rowCount: number = 10;
    filterText: string;

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


    foo(): void {
        console.log(this.filterText);
    }
}