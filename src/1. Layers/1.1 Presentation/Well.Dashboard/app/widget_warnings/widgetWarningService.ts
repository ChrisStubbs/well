import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { GlobalSettingsService } from '../shared/globalSettings';
import { WidgetWarning } from './widgetWarning';

@Injectable()
export class WidgetWarningService {
    headers: Headers = new Headers({ 'Content-Type': 'application/json' });
    options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }


    getWidgetWarnings(): Observable<WidgetWarning[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'widgetsWarnings')
            .map((res: Response) => <WidgetWarning[]>res.json());
    }

    saveWidgetWarning(widgetWarning: WidgetWarning, isUpdate: boolean): Observable<any> {
        let body = JSON.stringify(widgetWarning);

        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'widgetWarning/' + isUpdate,
            body,
            this.options)
            .map(res => res.json());
    }

    removeWidgetWarning(widgetWarningId: number): Observable<any> {
        return this.http.delete(this.globalSettingsService.globalSettings.apiUrl + 'widgetWarning/' + widgetWarningId,
            this.options).map(res => res.json());
    }
}