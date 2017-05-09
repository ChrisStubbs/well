import { Injectable }               from '@angular/core';
import { HttpErrorService }         from '../shared/httpErrorService';
import { Observable }               from 'rxjs/Observable';
import { Response }                 from '@angular/http';
import { HttpService }              from '../shared/httpService';
import { GlobalSettingsService }    from '../shared/globalSettings';
import 'rxjs/add/operator/map';

@Injectable()
export class DriverService {
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService)
    {
    }

    public drivers(): Observable<string[]>
    {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'Driver')
            .map((response: Response) => <string[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}