import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TabsModule, Tabset, Tab} from 'ng2-tabs';
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
export class DeliveryComponent implements OnInit, AfterViewInit {
    public errorMessage: string;
    public delivery: Delivery = new Delivery(undefined);
    public rowCount: number = 10;
    public showAll: boolean = false;
    public deliveryId: number;
    private defaultTab = 'Exceptions';
    public exceptionsPage: 0;
    public cleanPage: 0;

    @ViewChild(Tabset)
    private tabset: Tabset;

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
        this.route.params.subscribe(params =>
        {
            this.deliveryId = params['id'];
            if (params['tab'])
            {
                this.defaultTab = params['tab'];
            }
        });

        this.deliveryService.getDelivery(this.deliveryId)
            .subscribe(delivery => { this.delivery = new Delivery(delivery), console.log(delivery.branchId); },
                error => this.errorMessage = <any>error);
    }

    public ngAfterViewInit(): void
    {
        const tab:Tab = this.tabset.tabs.find(t => t.title === this.defaultTab);
        tab.active = true;
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
                this.toasterService.pop('success', 'GRN saved', '');
            });
    }

    public disableGrnSave(): boolean
    {
        return !this.delivery.grnNumber || this.delivery.grnNumber.length === 0;
    }
}