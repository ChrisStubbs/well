import { Injectable } from '@angular/core';
import { Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { GlobalSettingsService } from '../shared/globalSettings';
import { WidgetWarning } from './widgetWarning';
import {HttpService} from '../shared/httpService';

@Injectable()
export class WidgetWarningService {
    public headers: Headers = new Headers({ 'Content-Type': 'application/json' });
    public options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(private http: HttpService, private globalSettingsService: GlobalSettingsService) { }
    
    public getWidgetWarnings(): Observable<WidgetWarning[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'widgetsWarnings')
            .map((res: Response) => <WidgetWarning[]>res.json());
    }

    public saveWidgetWarning(widgetWarning: WidgetWarning, isUpdate: boolean): Observable<any> {
        const body = JSON.stringify(widgetWarning);

        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'widgetWarning/' + isUpdate,
            body,
            this.options)
            .map(res => res.json());
    }

    public removeWidgetWarning(widgetWarningId: number): Observable<any> {
        return this.http.delete(this.globalSettingsService.globalSettings.apiUrl + 'widgetWarning/' + widgetWarningId,
            this.options).map(res => res.json());
    }
}