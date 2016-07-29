﻿import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {Delivery} from './delivery';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';

@Injectable()
export class DeliveryService {

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    getDelivery(deliveryId: number): Observable<Delivery> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'deliveries/' + deliveryId)
            .map((response: Response) => <Delivery>response.json())
            //.do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    


    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}