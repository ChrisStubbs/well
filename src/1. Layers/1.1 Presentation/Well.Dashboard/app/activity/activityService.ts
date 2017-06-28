import { Injectable }                               from '@angular/core';
import { Response }                                 from '@angular/http'
import { Observable }                               from 'rxjs/Observable';
import { GlobalSettingsService }                    from '../shared/globalSettings';
import { HttpErrorService }                         from '../shared/httpErrorService';
import { HttpService }                              from '../shared/httpService';
import { ActivitySource, ActivitySourceDetail }     from './activitySource';

@Injectable()
export class ActivityService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService)
    {
    }

    public get(invoice: string, branchId: number): Observable<ActivitySource>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'Activity/' +
            encodeURIComponent(invoice) + '/' + branchId.toString();

        return this.http.get(url)
            .map((response: Response) =>
            {
                const act = (response.json() as ActivitySource);

                const items = (act.details as any[]).map((obj) =>
                {
                    return Object.assign(new ActivitySourceDetail(), obj);
                }) as ActivitySourceDetail[];

                act.details = items;

                return act;

            })
            .catch(e => this.httpErrorService.handleError(e));
    }
}