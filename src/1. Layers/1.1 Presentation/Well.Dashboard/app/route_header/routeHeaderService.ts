import {Injectable} from 'angular2/core';
import {Http, Response} from 'angular2/http'
import {Observable} from 'rxjs/Observable';
import {IRoute} from './route';
import Settings = require("../globalSettings");

@Injectable()
export class RouteHeaderService {
    
    constructor(private http: Http, private globalSettings: Settings.GlobalSettings) { }

    getRouteHeaders(): Observable<IRoute> {
        console.log(this.globalSettings.WellApiUrl);
        return this.http.get(this.globalSettings.WellApiUrl + 'routeheaders')
            .map((response: Response) => <IRoute>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}