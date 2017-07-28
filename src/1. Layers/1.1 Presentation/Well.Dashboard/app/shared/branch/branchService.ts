import { Injectable, EventEmitter } from '@angular/core';
import { Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Branch } from './branch';
import { GlobalSettingsService } from '../globalSettings';
import { HttpErrorService } from '../httpErrorService';
import { HttpService } from '../httpService';
import * as _ from 'lodash';

import 'rxjs/add/operator/map';

@Injectable()
export class BranchService
{
    public userBranchesChanged$ = new EventEmitter<Branch[]>();

    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public getBranches(username): Observable<Branch[]>
    {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'branch?username=' + username)
            .map((response: Response) => <Branch[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getById(id: number): Observable<Branch> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'branch/' + id)
            .map((response: Response) => <Branch>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getBranchesValueList(username: string): Observable<Array<[string, string]>>
    {
        return this.getBranches(username)
            .map((branches: Branch[]) =>
            {
                const values = new Array<[string, string]>();

                _.map(branches, (current: Branch) =>
                {
                    if (current.selected)
                    {
                        values.push([current.id.toString(), current.name + ' (' + current.id.toString() + ')']);
                    }
                });

                return values;
            })
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getBranchesWithSeasonalDate(seasonalDateId): Observable<Branch[]>
    {

        return this.http.get(
            this.globalSettingsService.globalSettings.apiUrl + 'branch-season?seasonalDateId=' + seasonalDateId)
            .map((response: Response) => <Branch[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getBranchesWithCreditThreshold(creditThresholdId): Observable<Branch[]>
    {

        return this.http.get(
            this.globalSettingsService.globalSettings.apiUrl
            + 'branch-credit-threshold?creditThresholdId='
            + creditThresholdId)
            .map((response: Response) => <Branch[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public saveBranches(branches: Branch[], username, domain): Observable<any>
    {
        const body = JSON.stringify(branches);
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        if (username)
        {
            return this.http.post(this.globalSettingsService.globalSettings.apiUrl
                + 'save-branches-on-behalf-of-user?username='
                + username
                + '&domain='
                + domain,
                body,
                options)
                .map(res => res.json());
        } else
        {
            return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'branch',
                body,
                options)
                .map(res =>
                {
                    this.userBranchesChanged$.emit(branches);
                    return res.json();
                });
        }
    }
}