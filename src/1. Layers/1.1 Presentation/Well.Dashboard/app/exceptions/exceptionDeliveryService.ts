import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {IExceptionDelivery} from './exceptionDelivery';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';

@Injectable()
export class ExceptionDeliveryService {

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    getExceptions(): Observable<IExceptionDelivery[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'deliveries/exception')
            .map((response: Response) => <IExceptionDelivery>response.json())
            //.do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);

    }


    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}