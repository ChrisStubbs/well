import {Component, Input, OnInit, OnDestroy, NgZone} from '@angular/core';
import {GlobalSettingsService} from './globalSettings';
import {HttpService} from './services/httpService';
import {Subscription} from 'rxjs/Subscription';
import {LogService} from './services/logService';

@Component({
    selector: 'ow-loading',
    templateUrl: 'app/shared/loading.html'
})
export class LoadingComponent implements OnDestroy {

    private isTimerRunning: boolean = false;
    private timeout: any;
    private blocked: boolean = false;

    private isLoadingSubscription: Subscription;

    constructor(private httpService: HttpService, private logService: LogService)
    {
        this.isLoadingSubscription = this.httpService.isHttpLoading
            .subscribe(isHttpLoading => this.isHttpLoading(isHttpLoading));
    }

    public isHttpLoading(isLoading: boolean)
    {
        if (isLoading)
        {
            this.loadingStarted();
        } else
        {
            this.loadingStopped();
        }
    }

    public loadingStarted()
    {
        if (this.isTimerRunning === false)
        {
            this.timeout = setTimeout(this.blockUI(), 500);
            this.isTimerRunning = true;
        }
    }

    public loadingStopped()
    {
        if (this.timeout)
        {
            clearTimeout(this.timeout);
        }
        
        this.isTimerRunning = false;
        this.blocked = false;
    }

    public blockUI()
    {
        this.blocked = true;
    }

    public ngOnDestroy(): void
    {
        this.isLoadingSubscription.unsubscribe();
    }

}