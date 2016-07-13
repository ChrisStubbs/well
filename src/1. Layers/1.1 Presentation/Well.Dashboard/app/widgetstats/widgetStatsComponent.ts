import { Component, OnInit, ChangeDetectorRef} from '@angular/core';
import { RouteParams, Router } from '@angular/router-deprecated';
import {IWidgetStats} from './widgetstats'
import {WidgetStatsService} from './widgetstats-service'
declare var $: any;

@Component({
    templateUrl: './app/widgetstats/widgetstats.html'
})

export class WidgetStatsComponent implements OnInit {

    widgetstats: IWidgetStats;
    errorMessage: string;

    constructor(private widgetStatsService: WidgetStatsService,
        private router: Router, private routeParams: RouteParams, private changeDetectorRef: ChangeDetectorRef) { }

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