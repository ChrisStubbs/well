import { Injectable }               from '@angular/core';
import { Response }                 from '@angular/http';
import { GlobalSettingsService }    from '../globalSettings';
import { HttpErrorService }         from '../httpErrorService';
import { HttpService }              from '../httpService';
import { Observable }               from 'rxjs';
import * as _                       from 'lodash';
import { SessionStorageService }    from 'ngx-webstorage'
import {LookupsEnum}                from './lookupsEnum';
import 'rxjs/add/operator/map';

@Injectable()
export class LookupService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private storageService: SessionStorageService) {}

    public get(lookupName: LookupsEnum): Observable<Array<[string, string]>>
    {
        
        //storageService

        // const params: URLSearchParams = new URLSearchParams();
        // const url = this.globalSettingsService.globalSettings.apiUrl + 'Exception/PerLineItem';
        //
        // _.each(ids, (current: number) => params.set('id', current.toString()));
        //
        // return this.http.get(url, {search: params})
        //     .map((response: Response) => <IEditLineItemException>response.json())
        //     .catch(e => this.httpErrorService.handleError(e));
    }
}
