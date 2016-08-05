import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http'
import {Observable} from 'rxjs/Observable';
import {IRoute} from './route';
import {GlobalSettingsService} from '../shared/globalSettings';
import {HttpErrorService} from '../shared/httpErrorService';
import {ToasterService} from 'angular2-toaster/angular2-toaster';

@Injectable()
export class RouteHeaderService {

    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private toasterService: ToasterService) {
    }

    getRouteHeaders(searchField: string = '', searchTerm: string = ''): Observable<IRoute[]> {

        var url = this.globalSettingsService.globalSettings.apiUrl +
            'routes?searchField=' +
            searchField +
            '&searchTerm=' +
            searchTerm;

        return this.http.get(url)
            .map((response: Response) => <IRoute[]>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e, this.toasterService));
    }
}