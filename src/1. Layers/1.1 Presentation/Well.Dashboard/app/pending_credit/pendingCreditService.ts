import { Injectable } from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { GlobalSettingsService } from '../shared/globalSettings';
import { ExceptionDelivery } from '../exceptions/exceptionDelivery';
import { PendingCreditDetail } from './pendingCreditDetail';

@Injectable()
export class PendingCreditService {
    public headers: Headers = new Headers({ 'Content-Type': 'application/json' });
    public options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    public getPendingCredits(): Observable<ExceptionDelivery[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'pending-credits')
            .map((res: Response) => <ExceptionDelivery[]>res.json());
    }

    public getPendingCreditDetail(jobId: number): Observable<PendingCreditDetail[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'pending-credit-detail/' + jobId)
            .map((res: Response) => <PendingCreditDetail[]>res.json());
    }
}