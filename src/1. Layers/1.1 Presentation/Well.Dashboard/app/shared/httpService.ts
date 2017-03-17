import {Injectable} from '@angular/core';
import {Http, XHRBackend, RequestOptions, Request, RequestOptionsArgs, Response, Headers} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';

@Injectable()
export class HttpService extends Http {

    private isHttpLoadingSubject = new BehaviorSubject<boolean>(false);
    public isHttpLoading = this.isHttpLoadingSubject.asObservable();

    constructor(backend: XHRBackend, options: RequestOptions)
    {
        super(backend, options);
    }

    
    public request(url: string|Request, options?: RequestOptionsArgs): Observable<Response>
    {
        this.isHttpLoadingSubject.next(true);
        return super.request(url, options).do(x =>
        {
            this.isHttpLoadingSubject.next(false);
        });
    }
    
}