import {Injectable} from '@angular/core';
import {Http, XHRBackend, RequestOptions, Request, RequestOptionsArgs, Response, Headers} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import {LogService} from './logService';

@Injectable()
export class HttpService extends Http {

    private isHttpLoadingSubject = new BehaviorSubject<boolean>(false);
    public isHttpLoading = this.isHttpLoadingSubject.asObservable();
    public loadingCount: number = 0;

    constructor(backend: XHRBackend, options: RequestOptions, private logService: LogService)
    {
        super(backend, options);
    }
    
    public request(url: string|Request, options?: RequestOptionsArgs): Observable<Response>
    {
        this.loadingIncrement();
        return super.request(url, options)
            .do(x => { this.loadingDecrement(); })
            .catch(e => this.handleError(e));
    }

    private handleError(error: Response): Observable<any>
    {
        this.loadingDecrement();
        return Observable.throw(error);
    }

    private loadingIncrement()
    {
        this.loadingCount = this.loadingCount + 1;
        //this.logService.log('Loading start: ' + this.loadingCount);
        this.isHttpLoadingSubject.next(true);
    }

    private loadingDecrement()
    {
        this.loadingCount = this.loadingCount - 1;
        //this.logService.log('Loading stop: ' + this.loadingCount);
        if (this.loadingCount === 0)
        {
            this.isHttpLoadingSubject.next(false);
        }
    }
}