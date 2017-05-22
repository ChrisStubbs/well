import { Injectable } from '@angular/core';
import { Response } from '@angular/http'
import { Observable } from 'rxjs/Observable';
import {HttpService} from '../httpService';
import {GlobalSettingsService} from '../globalSettings';
import {HttpErrorService} from '../httpErrorService';

@Injectable()
export class ActionService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService)
    {
    }

    //public actionLineItemActions(): Observable<any>
    //{
    //    //const body = JSON.stringify(bulkCredit);

    //    //return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'bulk-credit/',
    //    //        body,
    //    //        this.options)
    //    //    .map(res => res.json())
    //    //    .do(data => this.logService.log('All: ' + JSON.stringify(data)))
    //    //    .catch(e => this.httpErrorService.handleError(e));
    //}

    //public getRoutes(): Observable<Route[]>
    //{
    //    const url = this.globalSettingsService.globalSettings.apiUrl + 'routes/all';

    //    return this.http.get(url)
    //        .map((response: Response) =>
    //        {
    //            const routes: Route[] = (response.json() as any[]).map((obj) =>
    //            {
    //                return Object.assign(new Route(), obj);
    //            }) as Route[];

    //            return routes;
    //        })
    //        .catch(e => this.httpErrorService.handleError(e));
    //}

    //public getSingleRoute(routeId: number): Observable<SingleRoute>
    //{
    //        const url = this.globalSettingsService.globalSettings.apiUrl + 'SingleRoute/' + routeId.toString();

    //        return this.http.get(url)
    //            .map((response: Response) => <SingleRoute>response.json())
    //            .catch(e => this.httpErrorService.handleError(e));
    //}
}