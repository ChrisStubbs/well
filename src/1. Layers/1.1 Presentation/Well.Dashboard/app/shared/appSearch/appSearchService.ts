import { Injectable }               from '@angular/core';
import { HttpErrorService }         from '../services/httpErrorService';
import { Observable }               from 'rxjs/Observable';
import { Response }                 from '@angular/http';
import { HttpService }              from '../services/httpService';
import { GlobalSettingsService }    from '../globalSettings';
import 'rxjs/add/operator/map';
import * as _ from 'lodash';
import { IAppSearchResult, AppSearchParameters } from './appSearch';

@Injectable()
export class AppSearchService {
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) {}

    public Search(parameters: AppSearchParameters): Observable<IAppSearchResult>
    {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'AppSearch'
            , { params: _.omitBy(parameters, _.isNil) })
            .map((response: Response) => <IAppSearchResult>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}