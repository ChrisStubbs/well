import {Component, OnInit, ViewChild}  from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import 'rxjs/Rx';   // Load all features
import {PaginationService } from 'ng2-pagination';
import {Delivery} from "./delivery";
import {DeliveryService} from "./deliveryService";
import {DropDownItem} from "../shared/dropDownItem";

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
    canAction: boolean;

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
        private deliveryService: DeliveryService,
        private route: ActivatedRoute,
        private router: Router) {
    }

    ngOnInit(): void {
        this.route.params.subscribe(params => { this.deliveryId = params['id'], this.canAction = params['canAction'] === 'true'; console.log(this.canAction); });

        this.deliveryService.getDelivery(this.deliveryId)
            .subscribe(delivery => this.delivery = new Delivery(delivery),
            error => this.errorMessage = <any>error);
    }

    onShowAllClicked() {
        this.showAll = !this.showAll;
    }

    lineClicked(line): void {
        this.router.navigate(['/delivery', this.delivery.id, '/line', line.lineNo]);
    }
}