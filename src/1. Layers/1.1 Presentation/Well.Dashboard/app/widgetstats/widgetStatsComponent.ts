import { Component, OnInit } from 'angular2/core';
import { RouteParams, Router } from 'angular2/router';
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
        private router: Router, private routeParams: RouteParams    ) { }

    ngOnInit() {
        this.getWiidgetStats();


        var exceptionNotifications = $.connection.exceptionsHub;
        exceptionNotifications.qs = { 'version': '1.0' };

        exceptionNotifications.client.widgetExceptions = () => {
            this.getWiidgetStats();
        };

        $.connection.hub.start().done((data) => {
        });

    }

    getWiidgetStats() {
        this.widgetStatsService.getWidgetStats()
            .subscribe(widgetstats => this.widgetstats = widgetstats, error => this.errorMessage = <any>error);
    }

   
}