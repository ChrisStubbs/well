import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {CleanDelivery} from './cleanDelivery';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';

@Injectable()
export class CleanDeliveryService {

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    getCleanDeliveries(): Observable<CleanDelivery[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'deliveries/clean')
            .map((response: Response) => <CleanDelivery>response.json())
            //.do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);

    }


    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}