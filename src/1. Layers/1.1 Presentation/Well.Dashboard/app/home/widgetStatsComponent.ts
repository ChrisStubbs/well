import { Component, OnInit}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features
import {IWidgetStats} from './widgetstats';
import {WidgetStatsService} from './widgetstats-service';
import {RefreshService} from '../shared/refreshService';
declare var $: any;

@Component({
    templateUrl: './app/home/widgetstats.html',
    providers: [HTTP_PROVIDERS, GlobalSettingsService, WidgetStatsService],
    selector:'ow-widgetstats'
})

export class WidgetStatsComponent implements OnInit {

    widgetstats: IWidgetStats;
    errorMessage: string;
    refreshSubscription: any;

    constructor(private widgetStatsService: WidgetStatsService,
        private refreshService: RefreshService)
    {
        this.refreshSubscription = this.refreshService.dataRefreshed$.subscribe(r => this.getWidgetStats());
    }

    ngOnInit() {
        this.getWidgetStats();
    }

    ngOnDestroy() {
        this.refreshSubscription.unsubscribe();
    }

    getWidgetStats() {
        this.widgetStatsService.getWidgetStats()
            .subscribe(widgetstats => this.widgetstats = widgetstats, error => this.errorMessage = <any>error);
    }
}