import {Component} from '@angular/core';
import {Route} from '../routes/route';

@Component({
    selector: 'delivery-selection-modal',
    templateUrl: './app/route_header/delivery-selection-modal.html'
})
export class DeliverySelectionModal {
    public isVisible = false;
    public route: Route;
    
    public show(route) {
        this.route = route;
        this.isVisible = true;
    }

    public hide() {
        this.isVisible = false;
    }
}