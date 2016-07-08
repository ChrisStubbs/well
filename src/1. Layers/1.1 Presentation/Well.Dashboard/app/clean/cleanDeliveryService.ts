import {Injectable} from 'angular2/core';
import {Http, Response} from 'angular2/http';
import {Observable} from 'rxjs/Observable';
import {ICleanDelivery} from './cleanDelivery';
import Settings = require("../globalSettings");

@Injectable()
export class CleanDeliveryService {


    constructor(private http: Http, private globalSettings: Settings.GlobalSettings) { }

    getCleanDeliveries(): Observable<ICleanDelivery> {

        return this.http.get(this.globalSettings.WellApiUrl + 'clean')
            .map((response: Response) => <ICleanDelivery>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);

    }


    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}