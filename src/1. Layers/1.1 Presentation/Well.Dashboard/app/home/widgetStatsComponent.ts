import { Component, OnInit}  from '@angular/core';
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

    constructor(private widgetStatsService: WidgetStatsService) { }

    ngOnInit() {
        this.getWidgetStats();

        //this.widgetStatsService.autoUpdateDisabled()
        //    .subscribe(isAutoUpdateDisabled => this.initSignalr(isAutoUpdateDisabled),
        //        error => this.errorMessage = <any>error);
        //    .subscribe(isAutoUpdateDisabled => this.initSignalr(isAutoUpdateDisabled),
        //        error => this.errorMessage = <any>error);
    }

    initSignalr(isAutoUpdateDisabled): void {
        if (isAutoUpdateDisabled === true) return; //We can get rid of this once signalr is using webSockets

        var exceptionNotifications = $.connection.exceptionsHub;
        exceptionNotifications.qs = { 'version': '1.0' };

        exceptionNotifications.client.widgetExceptions = () => {
            this.getWidgetStats();
        };

        $.connection.hub.start().done((data) => {
            console.log("Hub started");
        });
    }

    handleExceptions(widgetstats): void {
        this.widgetstats = widgetstats;
      
    }

    getWidgetStats() {
        this.widgetStatsService.getWidgetStats()
            .subscribe(widgetstats => this.handleExceptions(widgetstats), error => this.errorMessage = <any>error);
    }

   
}