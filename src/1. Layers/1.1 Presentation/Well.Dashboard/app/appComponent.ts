/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
import {bootstrap} from 'angular2/platform/browser';
import {Component, OnInit} from 'angular2/core';
import { HTTP_PROVIDERS } from 'angular2/http';
import 'rxjs/Rx';   // Load all features
import {IRouteException} from './route_exceptions/route-exceptions';
import {RouteExceptionService} from './route_exceptions/route-exception.service';
declare var $: any;

@Component({
    selector: 'ow-app',
    templateUrl: './app/main.html',
    providers: [RouteExceptionService, HTTP_PROVIDERS]


})


export class AppComponent  implements OnInit { 
    exception: IRouteException;
    errorMessage: string;
    exceptionNumber:number;
    

    constructor(private routeExceptionService: RouteExceptionService) { }
 
    ngOnInit() {
        var self = this;

        this.getExceptions();     


        var exceptionNotifications =  $.connection.exceptionsHub;
        exceptionNotifications.qs = { 'version': '1.0' };

        exceptionNotifications.client.widgetExceptions = function () {
            self.getExceptions();  
        };

        $.connection.hub.start().done((data) => {
        });

    }

    getExceptions() {
        this.routeExceptionService.getExceptions()
            .subscribe(exception => this.exception = exception, error => this.errorMessage = <any>error);
    }

}

