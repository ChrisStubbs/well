/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
import {bootstrap} from 'angular2/platform/browser';
import {Component, OnInit, ChangeDetectorRef} from 'angular2/core';
import { HTTP_PROVIDERS } from 'angular2/http';
import 'rxjs/Rx';   // Load all features
import {IRouteExceptions} from './route_exceptions/route-exceptions';
import {RouteExceptionService} from './route_exceptions/route-exception.service';
declare var $: any;

@Component({
    selector: 'ow-app',
    templateUrl: './app/main.html',
    providers: [RouteExceptionService, HTTP_PROVIDERS]
})

export class AppComponent  implements OnInit { 
    exception: IRouteExceptions;
    errorMessage: string;
    exceptionNumber: number;
    

    constructor(private changeDetectorRef: ChangeDetectorRef, private routeExceptionService: RouteExceptionService) { }
 
    ngOnInit(): void {
        var self = this;

        this.getExceptions();     


        var exceptionNotifications =  $.connection.exceptionsHub;
        exceptionNotifications.qs = { 'version': '1.0' };

        exceptionNotifications.client.widgetExceptions = function () {
            console.log("widgetExceptions triggered");
            self.getExceptions();
        };

        $.connection.hub.start().done((data) => {
            console.log("Hub Started");
        });

    }

    handleExceptions(exception): void {
        this.exception = exception;
        this.changeDetectorRef.detectChanges();
    }

    getExceptions(): void {
        this.routeExceptionService.getExceptions()
            .subscribe(response => this.handleExceptions(response), error => this.errorMessage = <any>error);
    }

}

