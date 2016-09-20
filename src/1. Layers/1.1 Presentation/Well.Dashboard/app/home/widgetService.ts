import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http'
import {Observable} from 'rxjs/Observable';
import {Widget} from './widget';
import {GlobalSettingsService} from '../shared/globalSettings';
import {HttpErrorService} from '../shared/httpErrorService';
import {LogService} from '../shared/logService';

@Injectable()
export class WidgetService {

    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private logService: LogService) {
    }

    getWidgets(): Observable<Widget[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'widgets')
            .map((response: Response) => <Widget[]>response.json())
            .do(data => this.logService.log("All: " + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }
}