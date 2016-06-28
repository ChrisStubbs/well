﻿
import {Injectable} from 'angular2/core';
import {Http, Response} from 'angular2/http'
import {Observable} from 'rxjs/Observable';
import {IRouteException} from './route-exceptions'

@Injectable()
export class RouteExceptionService {
    private _exceptionsUrl = '/Well.Api/';

    constructor(private _http: Http) { }

    getExceptions(): Observable<IRouteException> {

       return this._http.get(this._exceptionsUrl + 'exceptions')
            .map((response: Response) => <IRouteException> response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);
    }


    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }



}