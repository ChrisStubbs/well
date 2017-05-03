import { Injectable } from '@angular/core';
import { Response } from '@angular/http'
import { Observable } from 'rxjs/Observable';
import { Route } from '../routes/route';
import { GlobalSettingsService } from '../shared/globalSettings';
import { HttpErrorService } from '../shared/httpErrorService';
import { HttpService } from '../shared/httpService';
import { SingleRoute } from './singleRoute';

@Injectable()
export class RoutesService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService)
    {
    }

    public getRoutes(): Observable<Route[]>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'routes/all';

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

    public getSingleRoute(routeId: number): Observable<Array<SingleRoute>>
    {
            const url = this.globalSettingsService.globalSettings.apiUrl + 'SingleRoute/' + routeId.toString();

            return this.http.get(url)
                .map((response: Response) => <SingleRoute[]>response.json())
                .catch(e => this.httpErrorService.handleError(e));
    }
}