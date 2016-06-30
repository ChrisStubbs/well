import { Component, OnInit, ChangeDetectorRef} from 'angular2/core';
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
        private router: Router, private routeParams: RouteParams, private changeDetectorRef: ChangeDetectorRef) { }

    ngOnInit() {
        this.getWidgetStats();


        var exceptionNotifications = $.connection.exceptionsHub;
        exceptionNotifications.qs = { 'version': '1.0' };

        exceptionNotifications.client.widgetExceptions = () => {
            this.getWidgetStats();
        };

        $.connection.hub.start().done((data) => {
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