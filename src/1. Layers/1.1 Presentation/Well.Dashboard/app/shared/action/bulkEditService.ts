import { Injectable } from '@angular/core';
import { Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { HttpService } from '../services/httpService';
import { GlobalSettingsService } from '../globalSettings';
import { HttpErrorService } from '../services/httpErrorService';
import { IBulkEditPatchRequest, IBulkEditResult } from './bulkEditItem';
import { IPatchSummary } from '../models/patchSummary';

@Injectable()
export class BulkEditService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public getSummaryForJob(jobIds: Array<number>, deliveryAction: number): Observable<IPatchSummary>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'bulkedit/summary/jobs';
        return this.get(url, jobIds, deliveryAction);
    }

    public getSummaryForLineItems(lineitemIds: Array<number>, deliveryAction: number): Observable<IPatchSummary>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'bulkedit/summary/lineitems';
        return this.get(url, lineitemIds, deliveryAction);
    }

    private get(url: string, ids: Array<number>, deliveryAction: number): Observable<IPatchSummary>
    {
        return this.http.get(url, { params: { id: ids, deliveryAction: deliveryAction } })
            .map((response: Response) => <IPatchSummary>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public patch(item: IBulkEditPatchRequest): Observable<IBulkEditResult> 
    {
        const body = JSON.stringify(item);
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });
        const url = this.globalSettingsService.globalSettings.apiUrl + 'bulkedit';

        return this.http.patch(url, body, options)
            .map((response: Response) => <IBulkEditResult>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}
