import {Component} from '@angular/core';
import {Route} from './route';

@Component({
    selector: 'delivery-selection-modal',
    templateUrl: './app/route_header/delivery-selection-modal.html'
})
export class DeliverySelectionModal {
    isVisible = false;
    route: Route;

    constructor() { }

    show(route) {
        this.route = route;
        this.isVisible = true;
    }

    hide() {
        this.isVisible = false;
    }
}