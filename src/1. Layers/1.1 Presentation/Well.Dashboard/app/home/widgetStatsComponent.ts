import { Component, OnInit, ChangeDetectorRef}  from '@angular/core';
import { HTTP_PROVIDERS } from '@angular/http';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/Rx';   // Load all features


import {IWidgetStats} from './widgetstats'
import {WidgetStatsService} from './widgetstats-service'
declare var $: any;

@Component({
    templateUrl: './app/home/widgetstats.html',
    providers: [HTTP_PROVIDERS, GlobalSettingsService, WidgetStatsService ],
    selector:'ow-widgetstats'
})

export class WidgetStatsComponent implements OnInit {

    widgetstats: IWidgetStats;
    errorMessage: string;

    constructor(private widgetStatsService: WidgetStatsService, private changeDetectorRef: ChangeDetectorRef) { }

    ngOnInit() {
        this.getWidgetStats();

        this.initSignalr();
    }

    initSignalr(): void {

        var hub = $.connection.refreshHub;
        hub.client.dataRefreshed = () => {
            //console.log("Refreshing data...");
            this.getWidgetStats();
        };

        $.connection.hub.start().done((data) => {
            //console.log("Hub started");
        });
    }

    handleExceptions(widgetstats): void {
        this.widgetstats = widgetstats;
        this.changeDetectorRef.detectChanges();
    }

    getWidgetStats() {
        this.widgetStatsService.getWidgetStats()
            .subscribe(widgetstats => this.handleExceptions(widgetstats), error => this.errorMessage = <any>error);
    }
}