import {Injectable} from '@angular/core';
import {Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {Widget} from './widget';
import {GlobalSettingsService} from '../shared/globalSettings';
import {HttpErrorService} from '../shared/httpErrorService';
import {LogService} from '../shared/logService';
import {HttpService} from '../shared/httpService';

@Injectable()
export class WidgetService {

    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private logService: LogService) {
    } 

    public getWidgets(): Observable<Widget[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'widgets')
            .map((response: Response) => <Widget[]>response.json())
            .do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }
}