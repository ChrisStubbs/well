import { Component, OnInit, ViewChild}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features

import {PaginatePipe, PaginationControlsCmp, PaginationService } from 'ng2-pagination';
import {Delivery} from "./delivery";
import {DeliveryService} from "./deliveryService";
import {ExceptionsFilterPipe} from "./exceptionsFilterPipe";

import {DropDownItem} from "../shared/dropDownItem";
@Component({
    selector: 'ow-delivery',
    templateUrl: './app/delivery/delivery.html',
    providers: [HTTP_PROVIDERS, GlobalSettingsService, DeliveryService, PaginationService],
    directives: [PaginationControlsCmp],
    pipes: [ExceptionsFilterPipe,PaginatePipe]
})

export class DeliveryComponent implements OnInit {
    errorMessage: string;
    delivery: Delivery = new Delivery();
    rowCount: number = 10;
    showAll: boolean = false;

    options: DropDownItem[] = [
        new DropDownItem("Exceptions", "isException"),
        new DropDownItem("Line", "lineNo"),
        new DropDownItem("Product", "productCode"),
        new DropDownItem("Description", "productDescription"),
        new DropDownItem("Reason", "reason"),
        new DropDownItem("Status", "status"),
        new DropDownItem("Action", "action")
    ];

    constructor(private deliveryService: DeliveryService, private globalSettingsService: GlobalSettingsService) { }

    ngOnInit(): void {
       
        this.deliveryService.getDelivery(1) //TODO - Fix
            .subscribe(delivery => { this.delivery = delivery; console.log(this.delivery.id) },
            error => this.errorMessage = <any>error);
    }

    onShowAllClicked() {
        this.showAll = !this.showAll;
    }

}