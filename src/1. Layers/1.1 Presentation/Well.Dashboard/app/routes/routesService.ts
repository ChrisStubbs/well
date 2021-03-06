import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Route } from '../routes/route';
import { GlobalSettingsService } from '../shared/globalSettings';
import { HttpErrorService } from '../shared/services/httpErrorService';
import { HttpService } from '../shared/services/httpService';
import {SingleRoute, SingleRouteItem} from './singleRoute';

@Injectable()
export class RoutesService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public getRoutesByBranch(branchId: number): Observable<Route[]>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'routes?branchId=' + branchId;

        return this.http.get(url)
            .map((response: Response) =>
            {
                const routes: Route[] = (response.json() as any[]).map((obj) =>
                {
                    return Object.assign(new Route(), obj);
                }) as Route[];

                return routes;
            })
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getSingleRoute(routeId: number): Observable<SingleRoute>
    {
            const url = this.globalSettingsService.globalSettings.apiUrl + 'SingleRoute/' + routeId.toString();

            return this.http.get(url)
                .map((response: Response) =>
                {
                    const obj: SingleRoute = <SingleRoute>response.json();

                    const items: Array<SingleRouteItem> = obj.items.map(current =>
                    {
                        return Object.assign(new SingleRouteItem(), current);
                    });

                    obj.items = items;

                    return obj;
                })
                .catch(e => this.httpErrorService.handleError(e));
    }
}