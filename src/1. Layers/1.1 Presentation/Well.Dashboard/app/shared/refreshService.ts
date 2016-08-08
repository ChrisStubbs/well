import {Injectable, EventEmitter, NgZone} from '@angular/core';
declare var $: any;

@Injectable()
export class RefreshService {
    public dataRefreshed$ = new EventEmitter();

    constructor(private zone: NgZone) {
        this.initSignalr();
    }

    initSignalr(): void {
        var hub = $.connection.refreshHub;
        hub.client.dataRefreshed = () => {
            this.zone.run(() => {
                //This is inside zone.run to trigger the Angular automagical change detection shizzle! 
                //console.log("Awoooga! Data refreshed.");
                this.dataRefreshed$.emit(null);
            });
        };

        $.connection.hub.start().done((data) => {
            //console.log("Hub started");
        });
    }
}