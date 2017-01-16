import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {GlobalSettingsService} from '../shared/globalSettings';
import {CleanPreference} from './cleanPreference';

@Injectable()
export class CleanPreferenceService {
    public headers: Headers = new Headers({ 'Content-Type': 'application/json' });
    public options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    public getCleanPreference(): Observable<CleanPreference[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'clean-preference')
            .map((res: Response) => <CleanPreference[]>res.json());
    }

    public removeCleanPreference(cleanPreferenceId: number): Observable<any> {
        return this.http.delete(
            this.globalSettingsService.globalSettings.apiUrl + 'clean-preference/' + cleanPreferenceId,
            this.options).map(res => res.json());
    }

    public saveCleanPreference(cleanPreference: CleanPreference, isUpdate: boolean): Observable<any> {
        const body = JSON.stringify(cleanPreference);

        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'clean-preference/' + isUpdate,
            body,
            this.options)
            .map(res => res.json());
    }
}