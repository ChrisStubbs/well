import { Component, OnInit}  from '@angular/core';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {Widget} from './widget';
import {WidgetService} from './widgetService';
import {RefreshService} from '../shared/refreshService';

@Component({
    templateUrl: './app/home/widgets.html',
    providers: [GlobalSettingsService, WidgetService]
})

export class WidgetComponent implements OnInit {

    widgets: Widget[] = new Array<Widget>();
    errorMessage: string;
    refreshSubscription: any;

    constructor(
        private widgetService: WidgetService,
        private refreshService: RefreshService) {
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
}