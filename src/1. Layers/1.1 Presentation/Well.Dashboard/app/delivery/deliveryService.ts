import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {Delivery} from './model/delivery';
import {DamageReason} from './model/damageReason';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';

@Injectable()
export class DeliveryService {

    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) {
    }

    getDelivery(deliveryId: number): Observable<Delivery> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'deliveries/' + deliveryId)
            .map((response: Response) => <Delivery>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    getDamageReasons(): Observable<DamageReason[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'damage-reasons/')
            .map((response: Response) => <DamageReason[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    updateDeliveryLine(line): Observable<any> {
        let body = JSON.stringify(line);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.put(this.globalSettingsService.globalSettings.apiUrl + 'delivery-line/',
                body,
                options)
            .catch(e => this.httpErrorService.handleError(e));
    }
}