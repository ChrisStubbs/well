﻿import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';

import {Notification} from './notification';

@Injectable()
export class NotificationsService {

    headers: Headers = new Headers({ 'Content-Type': 'application/json' });
    options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) {
    }

    getNotifications(): Observable<Notification[]> {

        var url = this.globalSettingsService.globalSettings.apiUrl + 'notification';

        return this.http.get(url)
            .map((response: Response) => <Notification[]>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    archiveNotification(id: number): Observable<any> {

        return this.http.put(this.globalSettingsService.globalSettings.apiUrl + 'notification/archive/' + id,
            this.options)
            .map(res => res.json())
            .catch(e => this.httpErrorService.handleError(e));
    }


}