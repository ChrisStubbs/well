import {Injectable} from 'angular2/core';
import {Http, Response} from 'angular2/http';
import {Observable} from 'rxjs/Observable';
import {ICleanDelivery} from './cleanDelivery';

@Injectable()
export class CleanDeliveryService {

    private cleanDeliveriesUrl = '/Well.Api/';

    constructor(private http: Http) { }

    getCleanDeliveries(): Observable<ICleanDelivery> {

        return this.http.get(this.cleanDeliveriesUrl + 'clean')
            .map((response: Response) => <ICleanDelivery>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);

    }


    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}