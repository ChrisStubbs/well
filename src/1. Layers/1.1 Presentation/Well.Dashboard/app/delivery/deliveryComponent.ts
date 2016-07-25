import { Component, OnInit, ViewChild}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features

import {PaginatePipe, PaginationControlsCmp, PaginationService } from 'ng2-pagination';
import {Delivery} from "./delivery";
import {DeliveryService} from "./deliveryService";

@Component({
    selector: 'ow-delivery',
    templateUrl: './app/delivery/delivery.html',
    providers: [HTTP_PROVIDERS, GlobalSettingsService, DeliveryService, PaginationService],
    directives: [PaginationControlsCmp],
    pipes: [PaginatePipe]
})

export class DeliveryComponent implements OnInit {
    errorMessage: string;
    delivery: Delivery = new Delivery();
    rowCount: number = 10;

    constructor(private deliveryService: DeliveryService, private globalSettingsService: GlobalSettingsService) { }

    ngOnInit(): void {
       
        this.deliveryService.getDelivery(this.globalSettingsService.globalSettings.deliveryId)
            .subscribe(delivery => { this.delivery = delivery; console.log(this.delivery.id) },
            error => this.errorMessage = <any>error);
    }

}