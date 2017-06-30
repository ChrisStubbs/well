import {Component, OnInit, ViewChild}   from '@angular/core';
import {NavigationExtras, Router}       from '@angular/router';
import {GlobalSettingsService}          from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {Widget}                         from './widget';
import {WidgetService}                  from './widgetService';
import {RefreshService}                 from '../shared/refreshService';
import {SecurityService}                from '../shared/security/securityService';
import {WidgetGraphComponent}           from './widgetGraphComponent';
import { UnauthorisedComponent }        from '../unauthorised/unauthorisedComponent';
import * as lodash                      from 'lodash';

@Component({
    templateUrl: './app/home/widgets.html'
})

export class WidgetComponent implements OnInit {
    public widgets: Widget[] = new Array<Widget>();
    public refreshSubscription: any;
    @ViewChild(WidgetGraphComponent) public widgetGraph: WidgetGraphComponent;

    constructor(
        public globalSettingsService: GlobalSettingsService,
        private widgetService: WidgetService,
        private refreshService: RefreshService,
        private securityService: SecurityService,
        private router: Router) {
    }

    public ngOnInit() {
        this.securityService.validateUser(
            this.globalSettingsService.globalSettings.permissions,
            this.securityService.landingPage);
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getWidgets());
        this.getWidgets();
    }

    public ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    public getWidgets() {
        this.widgetService.getWidgets()
            .subscribe(widgets => {
                const graphWidgets = lodash.filter(widgets, w => w.showOnGraph === true);
                this.widgets = widgets;
                const graphlabels: string[] = graphWidgets.map(widget => { return widget.name; });
                const graphData: any[] = graphWidgets.map(widget => { return widget.count; });
                const graphWarnings = graphWidgets.map(widget => { return widget.showWarning; });
                this.widgetGraph.init(graphlabels, graphData, graphWarnings, new Date());
            });
    }

    public widgetLinkClicked(widgetName: string, link: string) {

        let navigationExtras: NavigationExtras;
        switch (widgetName) {
        case 'Assigned':
        {
            navigationExtras = {
                queryParams: { 'filter.assigned': this.globalSettingsService.globalSettings.userName }
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
        case 'Notifications':
            {
                navigationExtras = {
                    queryParams: { 'notifications': true }
                };
                break;
            }
        }
       
        this.router.navigate([link], navigationExtras);
    }
}