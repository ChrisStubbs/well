import { Injectable } from '@angular/core';
import { Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { HttpService } from '../httpService';
import { GlobalSettingsService } from '../globalSettings';
import { HttpErrorService } from '../httpErrorService';
import { IBulkEditSummary, IBulkEditPatchRequest, IBulkEditResult } from './bulkEditItem';

@Injectable()
export class BulkEditService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public getSummaryForJob(jobIds: Array<number>): Observable<IBulkEditSummary>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'bulkedit/summary/jobs';
        return this.get(url, jobIds);
    }

    public getSummaryForLineItems(lineitemIds: Array<number>): Observable<IBulkEditSummary>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'bulkedit/summary/lineitems';
        return this.get(url, lineitemIds);
    }

    private get(url: string, ids: Array<number>): Observable<IBulkEditSummary>
    {
        return this.http.get(url, { params: { id: ids } })
            .map((response: Response) => <IBulkEditSummary>response.json())
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
