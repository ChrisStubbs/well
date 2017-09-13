import { Injectable } from '@angular/core';
import { Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { HttpService } from '../services/httpService';
import { GlobalSettingsService } from '../globalSettings';
import { HttpErrorService } from '../services/httpErrorService';
import { ISubmitActionModel } from './submitActionModel';
import { IActionSubmitSummary } from './actionSubmitSummary';
import { ISubmitActionResult } from './submitActionModel';

@Injectable()
export class ActionService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public getPreSubmitSummary(
        jobIds: Array<number>,
        isStopLevel: boolean): Observable<IActionSubmitSummary>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'SubmitAction/PreSubmitSummary';

        return this.http.get(url, { params: { jobId: jobIds, isStopLevel: isStopLevel } })
            .map((response: Response) => <IActionSubmitSummary>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public post(action: ISubmitActionModel): Observable<ISubmitActionResult>
    {
        const body = JSON.stringify(action);
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });
        const url = this.globalSettingsService.globalSettings.apiUrl + 'SubmitAction';

        return this.http.post(url, body, options)
            .map((response: Response) => <ISubmitActionResult>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}