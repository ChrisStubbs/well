﻿import { Component, OnInit}  from '@angular/core';
import {NavigationExtras, Router} from '@angular/router';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {Widget} from './widget';
import {WidgetService} from './widgetService';
import {RefreshService} from '../shared/refreshService';

@Component({
    templateUrl: './app/home/widgets.html',
    providers: [WidgetService]
})

export class WidgetComponent implements OnInit {

    widgets: Widget[] = new Array<Widget>();
    errorMessage: string;
    refreshSubscription: any;

    constructor(
        private widgetService: WidgetService,
        private refreshService: RefreshService,
        private router: Router) {
    }

    ngOnInit() {
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getWidgets());
        this.getWidgets();
    }

    ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    getWidgets() {
        this.widgetService.getWidgets()
            .subscribe(widgets => this.widgets = widgets, error => this.errorMessage = <any>error);
    }

    widgetLinkClicked(widgetLink: string, widgetName: string) {

        let navigationExtras: NavigationExtras;
        let link: string = '';
        switch(widgetName) {
            case 'Assigned':
            {
                navigationExtras = {
                    queryParams: { 'assigned': 'michael hook' } //TODO - Name must be dynamic
                };
                break;
            }
        }

        this.router.navigate([widgetLink], navigationExtras);
    }
}