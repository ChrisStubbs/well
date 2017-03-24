import {Component, Input, OnDestroy} from '@angular/core';
import {GlobalSettingsService} from './globalSettings';
import {HttpService} from './httpService';
import {Subscription} from 'rxjs/Subscription';
import {LogService} from './logService';

@Component({
    selector: 'ow-spinner',
    templateUrl: 'app/shared/spinner.html'
})
export class SpinnerComponent implements OnDestroy {
    private currentTimeout: number;
    private isItRunning: boolean = false;
    private isHttpLoading: boolean = false;
    private isLoadingSubscription: Subscription;

    constructor(private httpService : HttpService, private logService : LogService)
    {
       
    }

    public ngOnInit()
    {
        this.isLoadingSubscription = this.httpService.isHttpLoading
            .subscribe(isHttpLoading =>
            {
                this.logService.log("IsHttpLoading: " + isHttpLoading);
                this.isHttpLoading = isHttpLoading;
                var x = (window as any).getAllAngularTestabilities();
            });
    }

    @Input()
    public delay: number = 0;

    @Input()
    public set isRunning(value: boolean) {
        if (!value) {
            this.cancelTimeout();
            this.isItRunning = false;
            return;
        }

        if (this.currentTimeout) {
            return;
        }

        this.currentTimeout = window.setTimeout(() => {
            this.isItRunning = value;
            this.cancelTimeout();
        }, this.delay);
    }

    private cancelTimeout(): void {
        clearTimeout(this.currentTimeout);
        this.currentTimeout = undefined;
    }

    public ngOnDestroy(): void {
        this.cancelTimeout();
        this.isLoadingSubscription.unsubscribe();
    }
}