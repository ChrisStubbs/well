import { Injectable } from '@angular/core';
import { Response } from '@angular/http'
import { Observable } from 'rxjs/Observable';
import { Route } from '../routes/route';
import { GlobalSettingsService } from '../shared/globalSettings';
import { HttpErrorService } from '../shared/httpErrorService';
import { LogService } from '../shared/logService';
import { HttpService } from '../shared/httpService';

@Injectable()
export class RoutesService
{

    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private logService: LogService)
    {
    }

    public getRoutes(): Observable<Route[]>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'routes/all';

        return this.http.get(url)
            .map((response: Response) => <Route[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}