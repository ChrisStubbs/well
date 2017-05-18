import { Injectable }                          from '@angular/core';
import { Response, Headers, RequestOptions }   from '@angular/http';
import { GlobalSettingsService }               from '../shared/globalSettings';
import { HttpErrorService }                    from '../shared/httpErrorService';
import { HttpService }                         from '../shared/httpService';
import { Observable }                          from 'rxjs';
import { IEditLineItemException }              from './editLineItemException';
import { LineItemAction }                      from './lineItemAction';
import 'rxjs/add/operator/map';

@Injectable()
export class EditExceptionsService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public get(ids: Array<number>): Observable<Array<IEditLineItemException>>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'Exception/PerLineItem';

        return this.http.get(url, { params: {id: ids}})
            .map((response: Response) => <IEditLineItemException>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public put(item: LineItemAction): Observable<any>
    {
        const body = JSON.stringify(item);
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });
        const url = this.globalSettingsService.globalSettings.apiUrl + 'LineItemAction';

        return this.http.put(url, body, options)
            .catch(e => this.httpErrorService.handleError(e));
    }

    public post(item: LineItemAction): Observable<any>
    {
        const body = JSON.stringify(item);
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });
        const url = this.globalSettingsService.globalSettings.apiUrl + 'LineItemAction';

        return this.http.post(url, body, options)
            .catch(e => this.httpErrorService.handleError(e));
    }
}
