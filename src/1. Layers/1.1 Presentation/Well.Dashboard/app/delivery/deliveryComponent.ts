﻿import {Component, OnInit, ViewChild}  from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {Delivery} from "./model/delivery";
import {DeliveryService} from "./deliveryService";
import {DropDownItem} from "../shared/dropDownItem";
import {SecurityService} from '../shared/security/securityService';
import {SubmitConfirmModal} from './submitConfirmModal';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {UnauthorisedComponent} from '../unauthorised/unauthorisedComponent';
import {SubmitLine} from './model/submitLine';
import * as lodash from 'lodash';

@Component({
    selector: 'ow-delivery',
    templateUrl: './app/delivery/delivery.html',
    providers: [DeliveryService]
})
export class DeliveryComponent implements OnInit {
    errorMessage: string;
    delivery: Delivery = new Delivery(undefined);
    rowCount: number = 10;
    showAll: boolean = false;
    deliveryId: number;
    @ViewChild(SubmitConfirmModal) private submitConfirmModal: SubmitConfirmModal;

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
        private securityService: SecurityService,
        private toasterService: ToasterService) {
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

    submitActions(): void {
        let submitLines: SubmitLine[] = new Array<SubmitLine>();
        for (let line of this.delivery.deliveryLines) {
            var draftActions = lodash.filter(line.actions, { status: 1 });
            if (draftActions && draftActions.length > 0) {
                submitLines.push(new SubmitLine(line.productCode, line.productDescription, draftActions));
            }
        }

        this.submitConfirmModal.submitLines = submitLines;
        this.submitConfirmModal.show();
    }

    submitActionsConfirmed(): void {
        this.deliveryService.submitActions(this.delivery.id).subscribe(() => {
            this.toasterService.pop('success', 'Delivery actions submitted.', '');
        });
    }
}