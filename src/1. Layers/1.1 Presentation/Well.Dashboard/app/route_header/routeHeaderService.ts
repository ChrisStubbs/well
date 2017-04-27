import {Injectable} from '@angular/core';
import {Response} from '@angular/http'
import {Observable} from 'rxjs/Observable';
import {Route} from '../routes/route';
import {GlobalSettingsService} from '../shared/globalSettings';
import {HttpErrorService} from '../shared/httpErrorService';
import {LogService} from '../shared/logService';
import {HttpService} from '../shared/httpService';

@Injectable()
export class RouteHeaderService {

    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private logService: LogService) {
    }

    public getRouteHeaders(searchField = '', searchTerm = ''): Observable<Route[]> {

        const url = this.globalSettingsService.globalSettings.apiUrl +
            'routes?searchField=' +
            searchField +
            '&searchTerm=' +
            searchTerm;

        return this.http.get(url)
            .map((response: Response) => <Route[]>response.json())
            //.do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }
}