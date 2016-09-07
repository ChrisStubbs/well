﻿import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http'
import {Observable} from 'rxjs/Observable';
import {Widget} from './widget';
import {GlobalSettingsService} from '../shared/globalSettings';
import {HttpErrorService} from '../shared/httpErrorService';

@Injectable()
export class WidgetService {

    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) {
    }

    getWidgets(): Observable<Widget[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'widgets')
            .map((response: Response) => <Widget[]>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }
}