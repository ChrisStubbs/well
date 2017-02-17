import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import {Action} from './model/action';
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

    public getDelivery(deliveryId: number): Observable<Delivery> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'deliveries/' + deliveryId)
            .map((response: Response) => <Delivery>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getSources(): Observable<JobDetailSource[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'job-detail-source/')
            .map((response: Response) => <JobDetailSource[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getDamageReasons(): Observable<JobDetailReason[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'damage-reasons/')
            .map((response: Response) => <JobDetailReason[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getActions(): Observable<Action[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'delivery-actions/')
            .map((response: Response) => <Action[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public updateDeliveryLine(line): Observable<any> {
        const body = JSON.stringify(line);
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.put(this.globalSettingsService.globalSettings.apiUrl + 'DeliveryLine/',
                body,
                options)
            .catch(e => this.httpErrorService.handleError(e));
    }

    public updateDeliveryLineActions(request): Observable<any> {
        const body = JSON.stringify(request);
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'delivery-line-actions/',
                body,
                options)
            .catch(e => this.httpErrorService.handleError(e));
    }

    public submitActions(deliveryId): Observable<any> {
        const options = new RequestOptions({ headers: new Headers({ 'Content-Type': 'application/json' }) });
        const url = this.globalSettingsService.globalSettings.apiUrl + 'deliveries/' + deliveryId + '/submit-actions';
        return this.http.post(url, '', options).catch(e => this.httpErrorService.handleError(e));
    }

    public saveGrn(delivery: Delivery): Observable<any> {
        const options = new RequestOptions({ headers: new Headers({ 'Content-Type': 'application/json' }) });
        const body = JSON.stringify(delivery);
        const url = this.globalSettingsService.globalSettings.apiUrl + 'deliveries/grn';

        return this.http.post(url, body, options).catch(e => this.httpErrorService.handleError(e));
    }
}