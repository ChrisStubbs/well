import { Component, OnInit } from 'angular2/core';
import { ROUTER_DIRECTIVES } from 'angular2/router';
//import { MODAL_DIRECTIVES } from '../../node_modules/ng2-bs3-modal/ng2-bs3-modal';

import {ICleanDelivery} from './cleanDelivery';
import {CleanDeliveryService} from './cleanDeliveryService';

@Component({

    templateUrl: './app/clean/cleanDelivery-list.html',
    providers: [CleanDeliveryService],
    directives: [ROUTER_DIRECTIVES]
    //directives: [ROUTER_DIRECTIVES, MODAL_DIRECTIVES]
    
})
export class CleanDeliveryComponent implements OnInit {
    errorMessage: string;
    cleanDeliveries: ICleanDelivery;

    constructor(private cleanDeliveryService: CleanDeliveryService) { }

    ngOnInit(): void {
        this.cleanDeliveryService.getCleanDeliveries()
            .subscribe(cleanDeliveries => this.cleanDeliveries = cleanDeliveries,
                error => this.errorMessage = <any>error);
    }
}
