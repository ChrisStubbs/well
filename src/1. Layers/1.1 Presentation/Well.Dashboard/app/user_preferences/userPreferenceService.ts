import { Injectable } from '@angular/core';
import { Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { User } from './user';
import { GlobalSettingsService } from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import { HttpErrorService } from '../shared/httpErrorService';
import { HttpService } from '../shared/httpService';

@Injectable()
export class UserPreferenceService {

    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public getUsers(name: string): Observable<User[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'users/' + name)
            .map((response: Response) => <User[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));

    }

    public getUser(name: string): Observable<User> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'user/' + name)
            .map((response: Response) => <User>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}