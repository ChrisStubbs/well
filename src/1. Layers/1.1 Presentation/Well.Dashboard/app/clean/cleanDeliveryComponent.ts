﻿import { Component } from 'angular2/core';
import { ROUTER_DIRECTIVES } from 'angular2/router';

@Component({

    templateUrl: './app/clean/cleanDelivery-list.html',
    directives: [ROUTER_DIRECTIVES]
    
})
export class CleanDeliveryComponent {
    public pageTitle: string = 'Welcome';
}