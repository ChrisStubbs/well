import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {ICleanDelivery} from './cleanDelivery';
import {GlobalSettingsService} from '../shared/globalSettings';

@Injectable()
export class CleanDeliveryService {


    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    getCleanDeliveries(): Observable<ICleanDelivery> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'clean')
            .map((response: Response) => <ICleanDelivery>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);

    }


    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}