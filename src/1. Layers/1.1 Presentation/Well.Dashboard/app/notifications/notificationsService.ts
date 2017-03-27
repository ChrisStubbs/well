import {Injectable} from '@angular/core';
import {Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';

import {Notification} from './notification';
import {HttpService} from '../shared/httpService';

@Injectable()
export class NotificationsService {

    public headers: Headers = new Headers({ 'Content-Type': 'application/json' });
    public options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public getNotifications(): Observable<Notification[]> {

        const url = this.globalSettingsService.globalSettings.apiUrl + 'notification';

        return this.http.get(url)
            .map((response: Response) => <Notification[]>response.json())
            .do(data => console.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    public archiveNotification(id: number): Observable<any> {

        return this.http.put(this.globalSettingsService.globalSettings.apiUrl + 'notification/archive/' + id,
            this.options)
            .map(res => res.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}