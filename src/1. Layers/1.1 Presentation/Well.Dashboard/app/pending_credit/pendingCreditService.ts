import { Injectable } from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { GlobalSettingsService } from '../shared/globalSettings';
import { PendingCredit } from './pendingCredit';
import { PendingCreditDetail } from './pendingCreditDetail';

@Injectable()
export class PendingCreditService {
    headers: Headers = new Headers({ 'Content-Type': 'application/json' });
    options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    getPendingCredits(): Observable<PendingCredit[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'pending-credits')
            .map((res: Response) => <PendingCredit[]>res.json());
    }

    getPendingCreditDetail(jobId: number): Observable<PendingCreditDetail[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'pending-credit-detail/' + jobId)
            .map((res: Response) => <PendingCreditDetail[]>res.json());
    }
}