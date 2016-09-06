import {Router} from '@angular/router';
import {Component} from '@angular/core';
import {Route} from './route';

@Component({
    selector: 'delivery-selection-modal',
    templateUrl: './app/route_header/delivery-selection-modal.html'
})
export class DeliverySelectionModal {
    isVisible = false;
    route: Route;

    constructor(private router: Router) { }

    show(route) {
        this.route = route;
        this.isVisible = true;
    }

    hide() {
        this.isVisible = false;
    }

    viewException() {
        this.router.navigate(['/exceptions', this.route.route]);
    }

    viewClean() {
        this.router.navigate(['/clean', this.route.route]);
    }
}