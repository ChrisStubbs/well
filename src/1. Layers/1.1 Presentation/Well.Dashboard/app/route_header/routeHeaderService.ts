import {Injectable} from 'angular2/core';
import {Http, Response} from 'angular2/http'
import {Observable} from 'rxjs/Observable';
import {IRouteHeader} from './routeHeader';

@Injectable()
export class RouteHeaderService {
    private _exceptionsUrl = '/Well.Api/';

    constructor(private _http: Http) { }

    getRouteHeaders(): Observable<IRouteHeader> {

        return this._http.get(this._exceptionsUrl + 'routeheaders')
            .map((response: Response) => <IRouteHeader>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}