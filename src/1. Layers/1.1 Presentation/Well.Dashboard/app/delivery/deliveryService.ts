import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import {Action} from './model/action';
import {ActionStatus} from './model/actionStatus';
import {Delivery} from './model/delivery';
import {JobDetailReason} from './model/jobDetailReason';
import {JobDetailSource} from './model/jobDetailSource';
import {GlobalSettingsService} from '../shared/globalSettings';
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

    getSources(): Observable<JobDetailSource[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'job-detail-source/')
            .map((response: Response) => <JobDetailSource[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    getDamageReasons(): Observable<JobDetailReason[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'damage-reasons/')
            .map((response: Response) => <JobDetailReason[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    getActions(): Observable<Action[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'delivery-actions/')
            .map((response: Response) => <Action[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    getActionStatuses(): Observable<ActionStatus[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'action-statuses/')
            .map((response: Response) => <ActionStatus[]>response.json())
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

    updateDeliveryLineActions(request): Observable<any> {
        let body = JSON.stringify(request);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'delivery-line-actions/',
                body,
                options)
            .catch(e => this.httpErrorService.handleError(e));
    }

    submitActions(deliveryId): Observable<any> {
        let options = new RequestOptions({ headers: new Headers({ 'Content-Type': 'application/json' }) });
        var url = this.globalSettingsService.globalSettings.apiUrl + 'deliveries/' + deliveryId + '/submit-actions';
        return this.http.post(url, "", options).catch(e => this.httpErrorService.handleError(e));
    }

    saveGrn(delivery: Delivery): Observable<any> {
        let options = new RequestOptions({ headers: new Headers({ 'Content-Type': 'application/json' }) });
        let body = JSON.stringify(delivery);
        var url = this.globalSettingsService.globalSettings.apiUrl + 'deliveries/grn';

        return this.http.post(url, body, options).catch(e => this.httpErrorService.handleError(e));
    }
}