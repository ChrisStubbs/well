import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {GlobalSettingsService} from '../shared/globalSettings';
import {CreditThreshold} from './creditThreshold';

@Injectable()
export class CreditThresholdService {
    headers: Headers = new Headers({ 'Content-Type': 'application/json' });
    options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    getCreditThresholds(): Observable<CreditThreshold[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'credit-threshold')
            .map((res: Response) => <CreditThreshold[]>res.json());
    }

    removeCreditThreshold(creditThresholdId: number): Observable<any> {
        return this.http.delete(this.globalSettingsService.globalSettings.apiUrl + 'credit-threshold/' + creditThresholdId,
            this.options).map(res => res.json());
    }

    saveCreditThreshold(creditThreshold: CreditThreshold, isUpdate: boolean): Observable<any> {
        let body = JSON.stringify(creditThreshold);

        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'credit-threshold/' + isUpdate,
            body,
            this.options)
            .map(res => res.json());
    }

    saveThresholdLevel(threshold: string, username: string): Observable<any> {
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl +
                'threshold-level?threshold=' +
                threshold +
                '&username=' +
                username,
                null,
                this.options)
            .map(res => res.json());
    }
}