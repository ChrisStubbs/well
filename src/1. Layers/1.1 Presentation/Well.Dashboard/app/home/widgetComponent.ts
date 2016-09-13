import { Component, OnInit, ViewChild}  from '@angular/core';
import {NavigationExtras, Router} from '@angular/router';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {Widget} from './widget';
import {WidgetService} from './widgetService';
import {RefreshService} from '../shared/refreshService';
import {SecurityService} from '../shared/security/securityService';
import {WidgetGraphComponent} from './widgetGraphComponent';
import {UnauthorisedComponent} from '../unauthorised/unauthorisedComponent';

@Component({
    templateUrl: './app/home/widgets.html'
})

export class WidgetComponent implements OnInit {

    widgets: Widget[] = new Array<Widget>();
    errorMessage: string;
    refreshSubscription: any;
    @ViewChild(WidgetGraphComponent) widgetGraph: WidgetGraphComponent;

    constructor(
        private globalSettingsService: GlobalSettingsService,
        private widgetService: WidgetService,
        private refreshService: RefreshService,
        private securityService: SecurityService,
        private router: Router) {
    }

    ngOnInit() {
        this.securityService.validateUser(this.globalSettingsService.globalSettings.permissions, this.securityService.landingPage);
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getWidgets());
        this.getWidgets();
    }

    ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    getWidgets() {
        this.widgetService.getWidgets()
            .subscribe(widgets => {
                this.widgets = widgets;

                let graphlabels: string[] = widgets.map(widget => { return widget.name; } );
                let graphData: any[] = widgets.map(widget => { return widget.count; });
                let graphWarnings = widgets.map(widget => { return widget.showWarning; });
                this.widgetGraph.init(graphlabels, graphData, graphWarnings, new Date());
            }, error => this.errorMessage = <any>error);
    }

    widgetLinkClicked(widgetName: string) {

        let navigationExtras: NavigationExtras;
        switch (widgetName) {
        case 'Assigned':
        {
            navigationExtras = {
                queryParams: { 'assignee': this.globalSettingsService.globalSettings.userName }
            };
            break;
        }
        case 'Outstanding':
        {
            navigationExtras = {
                queryParams: { 'outstanding': true }
            };
            break;
        }
        }
        let link: string = this.widgets.filter(widget => { return widget.name === widgetName; })[0].link;
        this.router.navigate([link], navigationExtras);
    }

    graphBarClicked(barName: string) {
        this.widgetLinkClicked(barName);
    }
}