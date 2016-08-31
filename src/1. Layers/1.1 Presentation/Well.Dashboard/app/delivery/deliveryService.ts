import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {Delivery} from './delivery';
import {DamageReason} from './damageReason';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';
import {ToasterService} from 'angular2-toaster/angular2-toaster';

@Injectable()
export class DeliveryService {

    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private toasterService: ToasterService) {
    }

    getDelivery(deliveryId: number): Observable<Delivery> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'deliveries/' + deliveryId)
            .map((response: Response) => <Delivery>response.json())
            .catch(e => this.httpErrorService.handleError(e, this.toasterService));
    }

    getDamageReasons(): Observable<DamageReason[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'damage-reasons/')
            .map((response: Response) => <DamageReason[]>response.json())
            .catch(e => this.httpErrorService.handleError(e, this.toasterService));
    }

    updateDeliveryLine(line): Observable<any> {
        let body = JSON.stringify(line);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.put(this.globalSettingsService.globalSettings.apiUrl + 'delivery-line/',
                body,
                options)
            .catch(e => this.httpErrorService.handleError(e, this.toasterService));
    }
}