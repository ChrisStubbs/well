import {Injectable, EventEmitter, NgZone} from '@angular/core';
import {LogService} from './logService';
declare var $: any;

@Injectable()
export class RefreshService {
    public dataRefreshed$ = new EventEmitter();

    constructor(private zone: NgZone,
        private logService: LogService) {
        this.initSignalr();
    }

    initSignalr(): void {
        var hub = $.connection.refreshHub;
        hub.client.dataRefreshed = () => {
            this.zone.run(() => {
                //This is inside zone.run to trigger the Angular automagical change detection shizzle! 
                this.logService.log("Awoooga! Data refreshed.");
                this.dataRefreshed$.emit(undefined);
            });
        };

        $.connection.hub.start().done((data) => {
            console.log("Hub started");
        });
    }
}