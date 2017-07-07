import { Injectable }                           from '@angular/core';
import { Response }                             from '@angular/http';
import { Observable }                           from 'rxjs/Observable';
import { GlobalSettingsService }                from '../shared/globalSettings';
import { HttpErrorService }                     from '../shared/httpErrorService';
import { HttpService }                          from '../shared/httpService';
import { SingleLocationHeader, SingleLocation } from './singleLocation';
import * as _                                   from 'lodash';

@Injectable()
export class LocationsService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public getSingleRoute(locationId?: string, account?: string, branchId?: number): Observable<SingleLocationHeader>
    {
        let url: string = this.globalSettingsService.globalSettings.apiUrl + 'SingleLocation?';

        if (_.isNil(locationId)) {
            url = url + 'accountNumber=' + encodeURIComponent(account) + '&branchId=' + branchId.toString();
        }
        else {
            url = url + 'locationId=' + locationId.toString();
        }

        return this.http.get(url)
            .map((response: Response) =>
            {
                let a: number;
                a = 1;
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