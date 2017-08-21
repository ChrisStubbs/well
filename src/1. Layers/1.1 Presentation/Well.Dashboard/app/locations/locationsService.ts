import { Injectable }                           from '@angular/core';
import { Response }                             from '@angular/http';
import { Observable }                                       from 'rxjs/Observable';
import { GlobalSettingsService }                            from '../shared/globalSettings';
import { HttpErrorService }                                 from '../shared/httpErrorService';
import { HttpService }                                      from '../shared/httpService';
import { SingleLocationHeader, SingleLocation, Locations }   from './singleLocation';
import * as _                                               from 'lodash';

@Injectable()
export class LocationsService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public getLocations(branchId: number): Observable<Array<Locations>>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'Location/' + branchId;

        return this.http.get(url)
            .map((response: Response) =>
            {
                const locations: Locations[] = (response.json() as any[]).map((obj) =>
                {
                    return Object.assign(new Locations(), obj);
                }) as Locations[];

                return locations;
            })
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getSingleLocation(locationId: number): Observable<SingleLocationHeader> {
        const url: string = this.globalSettingsService.globalSettings.apiUrl +
            'SingleLocation/' + locationId;

        return this.http.get(url)
            .map((response: Response) =>
            {
                const act = (response.json() as SingleLocationHeader);

                const items = (act.details as any[]).map((obj) =>
                {
                    return Object.assign(new SingleLocation(), obj);
                }) as SingleLocation[];

                act.details = items;

                return act;

            })
            .catch(e => this.httpErrorService.handleError(e));
    }
}