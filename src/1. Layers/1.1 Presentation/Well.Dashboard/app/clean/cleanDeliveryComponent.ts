import { Component } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router-deprecated';

@Component({

    templateUrl: './app/clean/cleanDelivery-list.html',
    directives: [ROUTER_DIRECTIVES]
    
})
export class CleanDeliveryComponent {
    public pageTitle: string = 'Welcome';
}
