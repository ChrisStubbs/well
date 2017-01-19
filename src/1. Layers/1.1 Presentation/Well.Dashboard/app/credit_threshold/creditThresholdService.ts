﻿import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {GlobalSettingsService} from '../shared/globalSettings';
import {CreditThreshold} from './creditThreshold';

@Injectable()
export class CreditThresholdService {
    public headers: Headers = new Headers({ 'Content-Type': 'application/json' });
    public options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    public getCreditThresholds(): Observable<CreditThreshold[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'credit-threshold')
            .map((res: Response) => <CreditThreshold[]>res.json());
    }

    public removeCreditThreshold(creditThresholdId: number): Observable<any> {
        return this.http.delete(
            this.globalSettingsService.globalSettings.apiUrl + 'credit-threshold/' + creditThresholdId,
            this.options).map(res => res.json());
    }

    public saveCreditThreshold(creditThreshold: CreditThreshold, isUpdate: boolean): Observable<any> {
        const body = JSON.stringify(creditThreshold);

        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'credit-threshold/' + isUpdate,
            body,
            this.options)
            .map(res => res.json());
    }

    public saveThresholdLevel(threshold: string, username: string): Observable<any> {
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl +
                'threshold-level?threshold=' +
                threshold +
                '&username=' +
                username,
                undefined,
                this.options)
            .map(res => res.json());
    }
}