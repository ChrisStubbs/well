import {Component, OnInit}  from '@angular/core';
import {ROUTER_DIRECTIVES} from '@angular/router-deprecated';
import {PAGINATION_DIRECTIVES} from 'ng2-bootstrap';
import {IResolvedDelivery} from './resolvedDelivery';
import {ResolvedDeliveryService} from './ResolvedDeliveryService';
import {ResolvedDeliveryFilterPipe } from './resolvedDeliveryFilterPipe';
import {GlobalSettingsService} from '../shared/globalSettings';
import {MODAL_DIRECTIVES} from 'ng2-bootstrap';

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