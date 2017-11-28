import {Injectable} from '@angular/core';
import {Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {GlobalSettingsService} from '../shared/globalSettings';
import {SeasonalDate} from './seasonalDate';
import {HttpService} from '../shared/services/httpService';

@Injectable()
export class SeasonalDateService {
    public headers: Headers = new Headers({ 'Content-Type': 'application/json' });
    public options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(private http: HttpService, private globalSettingsService: GlobalSettingsService) { }

    public getSeasonalDates(): Observable<SeasonalDate[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'seasonal-date')
            .map((res: Response) => 
            {    
                const seasons = <SeasonalDate[]>res.json();
                const result = [];

                seasons.forEach((item: SeasonalDate) => result.push(Object.assign(new SeasonalDate(), item)));

                return result;
            });
    }

    public removeSeasonalDate(seasonalDateId: number): Observable<any> {
        return this.http.delete(this.globalSettingsService.globalSettings.apiUrl + 'seasonal-date/' + seasonalDateId,
            this.options).map(res => res.json());
    }

    public saveSeasonalDate(seasonalDate: SeasonalDate): Observable<any> {
        const body = JSON.stringify(seasonalDate);

        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'seasonal-date',
            body,
            this.options)
            .map(res => res.json());
    }
}