import { Injectable } from '@angular/core';
import { Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { HttpService } from '../httpService';
import { GlobalSettingsService } from '../globalSettings';
import { HttpErrorService } from '../httpErrorService';
import { IPatchSummary } from '../models/patchSummary';
import {IManualCompletionRequest} from './manualCompletionRequest';
import {IBulkEditResult} from '../action/bulkEditItem';
import {IJobIdResolutionStatus} from '../models/jobIdResolutionStatus';

@Injectable()
export class ManualCompletionService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public getSummary(jobIds: Array<number>): Observable<IPatchSummary>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'manualcompletion/summary';
        return this.http.get(url, { params: { ids: jobIds } })
            .map((response: Response) => <IPatchSummary>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public patch(item: IManualCompletionRequest): Observable<IJobIdResolutionStatus[]> 
    {
        const body = JSON.stringify(item);
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });
        const url = this.globalSettingsService.globalSettings.apiUrl + 'manualcompletion';

        return this.http.patch(url, body, options)
            .map((response: Response) => <IBulkEditResult>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}
