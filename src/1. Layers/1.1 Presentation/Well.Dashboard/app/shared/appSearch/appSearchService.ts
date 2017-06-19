import { Injectable }               from '@angular/core';
import { HttpErrorService }         from '../httpErrorService';
import { Observable }               from 'rxjs/Observable';
import { Response }                 from '@angular/http';
import { HttpService }              from '../httpService';
import { GlobalSettingsService }    from '../globalSettings';
import { IAppSearchResultSummary }  from './iAppSearchResultSummary';
import { AppSearchParameters }      from './appSearchParameters';
import 'rxjs/add/operator/map';
import * as _ from 'lodash';

@Injectable()
export class AppSearchService {
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) {}

    public Search(parameters: AppSearchParameters): Observable<IAppSearchResultSummary>
    {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'AppSearch'
            , { params: _.omitBy(parameters, _.isNil) })
            .map((response: Response) => <IAppSearchResultSummary>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}