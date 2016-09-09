import {Component, OnInit, ViewChild}  from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {PaginationService } from 'ng2-pagination';
import {Delivery} from "./delivery";
import {DeliveryService} from "./deliveryService";
import {DropDownItem} from "../shared/dropDownItem";
import {SecurityService} from '../shared/security/security-service';
import {UnauthorisedComponent} from '../unauthorised/unauthorisedComponent';

@Component({
    selector: 'ow-delivery',
    templateUrl: './app/delivery/delivery.html',
    providers: [DeliveryService, PaginationService]
})
export class DeliveryComponent implements OnInit {
    errorMessage: string;
    delivery: Delivery = new Delivery(null);
    rowCount: number = 10;
    showAll: boolean = false;
    deliveryId: number;

    options: DropDownItem[] = [
        new DropDownItem("Exceptions", "isException"),
        new DropDownItem("Line", "lineNo"),
        new DropDownItem("Product", "productCode"),
        new DropDownItem("Description", "productDescription"),
        new DropDownItem("Reason", "reason"),
        new DropDownItem("Status", "status"),
        new DropDownItem("Action", "action")
    ];

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private deliveryService: DeliveryService,
        private route: ActivatedRoute,
        private router: Router,
        private securityService: SecurityService) {
    }

    ngOnInit(): void {
        this.securityService.validateUser(this.globalSettingsService.globalSettings.permissions, this.securityService.actionDeliveries);
        this.route.params.subscribe(params => { this.deliveryId = params['id'] });

        this.deliveryService.getDelivery(this.deliveryId)
            .subscribe(delivery => this.delivery = new Delivery(delivery),
            error => this.errorMessage = <any>error);
    }

    onShowAllClicked() {
        this.showAll = !this.showAll;
    }

    lineClicked(line): void {
        this.router.navigate(['/delivery', this.delivery.id, line.lineNo]);
    }
}