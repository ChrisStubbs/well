﻿import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {ExceptionDelivery} from './exceptionDelivery';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';
import {IUser} from '../shared/user';
import {LogService} from '../shared/logService';
import { Threshold } from '../shared/threshold';
import { DeliveryLine } from '../delivery/model/deliveryLine';
import {DeliveryAction} from '../delivery/model/deliveryAction';
import {BulkCredit} from './bulkCredit';

@Injectable()
export class ExceptionDeliveryService {

   public headers: Headers = new Headers({ 'Content-Type': 'application/json' });
   public options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private logService: LogService) {
    }

    public getExceptions(): Observable<ExceptionDelivery[]> {

        const url = this.globalSettingsService.globalSettings.apiUrl + 'deliveries/exception';

        return this.http.get(url)
            .map((response: Response) => <ExceptionDelivery[]>response.json())
            //.do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getUsersForBranch(branchId): Observable<IUser[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'users-for-branch/' + branchId)
            .map((response: Response) => <IUser[]>response.json())
            .do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getConfirmationDetails(jobId: number): Observable<DeliveryAction> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'delivery-line-actions/' + jobId)
            .map(res => res.json());  
    }

    public submitExceptionConfirmation(jobId: number): Observable<any> {
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'confirm-delivery-lines/' + jobId,
                JSON.stringify(''),
                this.options)
            .map(response => response.json())
            .do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    public creditLines(bulkCredit: BulkCredit): Observable<any>
    {
        const body = JSON.stringify(bulkCredit);
            
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'bulk-credit/',
            body,
            this.options)
            .map(res => res.json())
            .do (data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getUserCreditThreshold(): Observable<any> {

        const url = this.globalSettingsService.globalSettings.apiUrl + 'credit-threshold/getByUser';

        return this.http.get(url)
            .map((response: Response) => <any>response.json())
            .do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || ' error');
    }
}