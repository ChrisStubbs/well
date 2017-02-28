import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TabsModule } from 'ng2-tabs';
import { GlobalSettingsService } from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {Delivery} from './model/delivery';  
import { DeliveryService } from './deliveryService';
import { DropDownItem } from '../shared/dropDownItem';
import { SecurityService } from '../shared/security/securityService';
import { ToasterService } from 'angular2-toaster/angular2-toaster';
import { UnauthorisedComponent } from '../unauthorised/unauthorisedComponent';
import * as lodash from 'lodash';

@Component({
    selector: 'ow-delivery',
    templateUrl: './app/delivery/delivery.html',
    providers: [DeliveryService]
})
export class DeliveryComponent implements OnInit {
    public errorMessage: string;
    public delivery: Delivery = new Delivery(undefined);
    public rowCount: number = 10;
    public showAll: boolean = false;
    public deliveryId: number;

    public options: DropDownItem[] = [
        new DropDownItem('Exceptions', 'isException'),
        new DropDownItem('Line', 'lineNo'),
        new DropDownItem('Product', 'productCode'),
        new DropDownItem('Description', 'productDescription'),
        new DropDownItem('Reason', 'reason'),
        new DropDownItem('Status', 'status'),
        new DropDownItem('Action', 'action')
    ];

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private deliveryService: DeliveryService,
        private route: ActivatedRoute,
        private router: Router,
        private securityService: SecurityService,
        private toasterService: ToasterService)
    {
    }

    public ngOnInit(): void
    {
        this.securityService.validateUser(
            this.globalSettingsService.globalSettings.permissions,
            this.securityService.actionDeliveries);
        this.route.params.subscribe(params => { this.deliveryId = params['id'] });

        this.deliveryService.getDelivery(this.deliveryId)
            .subscribe(delivery => { this.delivery = new Delivery(delivery), console.log(delivery.branchId); },
                error => this.errorMessage = <any>error);
    }

    public onShowAllClicked()
    {
        this.showAll = !this.showAll;
    }

    public lineClicked(line): void
    {
        this.router.navigate(['/delivery', this.delivery.id, line.lineNo]);
    }

    public saveGrn(): void
    {
        this.deliveryService.saveGrn(this.delivery)
            .subscribe(() =>
            {
                this.toasterService.pop('success', 'GRN saved...', '');
            });
    }

    public disableGrnSave(): boolean
    {
        return !this.delivery.grnNumber || this.delivery.grnNumber.length === 0;
    }
}