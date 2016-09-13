import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http'
import {Observable} from 'rxjs/Observable';
import {Route} from './route';
import {GlobalSettingsService} from '../shared/globalSettings';
import {HttpErrorService} from '../shared/httpErrorService';
import {LogService} from '../shared/logService';

@Injectable()
export class RouteHeaderService {

    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private logService: LogService) {
    }

    getRouteHeaders(searchField: string = '', searchTerm: string = ''): Observable<Route[]> {

        var url = this.globalSettingsService.globalSettings.apiUrl +
            'routes?searchField=' +
            searchField +
            '&searchTerm=' +
            searchTerm;

        return this.http.get(url)
            .map((response: Response) => <Route[]>response.json())
            .do(data => this.logService.log("All: " + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }
}