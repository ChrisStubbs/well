import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {User} from './user';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';
import {ToasterService} from 'angular2-toaster/angular2-toaster';

@Injectable()
export class UserPreferenceService {

    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private toasterService: ToasterService) { }

    getUsers(name: string): Observable<User[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'users/' + name)
            .map((response: Response) => <User[]>response.json())
            .catch(e => this.httpErrorService.handleError(e, this.toasterService));

    }
}