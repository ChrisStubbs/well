import { Injectable } from '@angular/core';
import { Response } from '@angular/http'
import { Observable } from 'rxjs/Observable';
import { GlobalSettingsService } from '../shared/globalSettings';
import { HttpErrorService } from '../shared/httpErrorService';
import { HttpService } from '../shared/httpService';
import { Stop, StopItem } from './stop';

@Injectable()
export class StopService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public getStop(stopId: number): Observable<Stop>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'stops/' + stopId.toString();

        return this.http.get(url)
            .map((response: Response) =>
            {
                const stop = (response.json() as Stop);

                const items = (stop.items as any[]).map((obj) =>
                {
                    return Object.assign(new StopItem(), obj);
                }) as StopItem[];

                stop.items = items;

                return stop;

            })
            .catch(e => this.httpErrorService.handleError(e));
    }
}