import { Injectable }               from '@angular/core';
import { Response }                 from '@angular/http';
import { GlobalSettingsService }    from '../shared/globalSettings';
import { HttpErrorService }         from '../shared/httpErrorService';
import { HttpService }              from '../shared/httpService';
import { Observable }               from 'rxjs';
import { IEditLineItemException }   from './editLineItemException';
import * as _                       from 'lodash';
import 'rxjs/add/operator/map';

@Injectable()
export class EditExceptionService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) {}

    public get(ids: Array<number>): Observable<Array<IEditLineItemException>> {

        const params: URLSearchParams = new URLSearchParams();
        const url = this.globalSettingsService.globalSettings.apiUrl + 'Exception/PerLineItem';

        _.each(ids, (current: number) => params.set('id', current.toString()));

        return this.http.get(url, {search: params})
            .map((response: Response) => <IEditLineItemException>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}
