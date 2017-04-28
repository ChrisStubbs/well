import { Injectable }               from '@angular/core';
import { HttpErrorService }         from '../httpErrorService';
import { Observable }               from 'rxjs/Observable';
import { Response }                 from '@angular/http';
import { HttpService }              from '../httpService';
import { GlobalSettingsService }    from '../globalSettings';
import { IAppSearchResult }         from './iAppSearchResult';
import { AppSearchParameters }      from './appSearchParameters';
import 'rxjs/add/operator/map';

@Injectable()
export class AppSearchService {
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) {}

    public Search(parameters: AppSearchParameters): Observable<IAppSearchResult>
    {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'AppSearch', {params: parameters})
            .map((response: Response) => <IAppSearchResult>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}