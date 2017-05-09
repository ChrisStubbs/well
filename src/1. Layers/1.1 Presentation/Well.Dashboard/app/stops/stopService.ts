import { Injectable }               from '@angular/core';
import { Response }                 from '@angular/http'
import { Observable }               from 'rxjs/Observable';
import { GlobalSettingsService }    from '../shared/globalSettings';
import { HttpErrorService }         from '../shared/httpErrorService';
import { HttpService }              from '../shared/httpService';
import { Stop }                     from './stop';

@Injectable()
export class StopService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService)
    {
    }

    public getStop(stopId: number): Observable<Array<Stop>>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'stops/' + stopId.toString();

        return this.http.get(url)
            .map((response: Response) =>
            {
                return (response.json() as any[]).map((obj) =>
                {
                    return Object.assign(new Stop(), obj);
                }) as Stop[];
            })
            .catch(e => this.httpErrorService.handleError(e));
    }
}