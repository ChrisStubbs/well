import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {GlobalSettingsService} from '../shared/globalSettings';
import {SeasonalDate} from './seasonalDate';

@Injectable()
export class SeasonalDateService {
    headers: Headers = new Headers({ 'Content-Type': 'application/json' });
    options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    getSeasonalDates(): Observable<SeasonalDate[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'seasonaldate')
            .map((res: Response) => <SeasonalDate[]>res.json());
    }

    removeSeasonalDate(seasonalDateId: number): Observable<any> {
        return this.http.delete(this.globalSettingsService.globalSettings.apiUrl + 'seasonal-date/' + seasonalDateId,
            this.options).map(res => res.json());
    }

    saveSeasonalDate(seasonalDate: SeasonalDate): Observable<any> {
        let body = JSON.stringify(seasonalDate);

        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'seasonal-date',
            body,
            this.options)
            .map(res => res.json());
    }
}