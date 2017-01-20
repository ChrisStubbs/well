import {Injectable, EventEmitter} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {Branch} from './branch';
import {IUser} from '../user';
import {GlobalSettingsService} from '../globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../httpErrorService';
import {LogService} from '../logService';

@Injectable()
export class BranchService {
    public userBranchesChanged$ = new EventEmitter<Branch[]>();

    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private logService: LogService) {
    }

    public getBranches(username): Observable<Branch[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'branch?username=' + username)
            .map((response: Response) => <Branch[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getBranchesWithSeasonalDate(seasonalDateId): Observable<Branch[]> {

        return this.http.get(
                this.globalSettingsService.globalSettings.apiUrl + 'branch-season?seasonalDateId=' + seasonalDateId)
            .map((response: Response) => <Branch[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getBranchesWithCreditThreshold(creditThresholdId): Observable<Branch[]> {

        return this.http.get(
                this.globalSettingsService.globalSettings.apiUrl
                + 'branch-credit-threshold?creditThresholdId='
                + creditThresholdId)
            .map((response: Response) => <Branch[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getBranchesWithCleanPreference(cleanPreferenceId): Observable<Branch[]> {

        return this.http.get(
                this.globalSettingsService.globalSettings.apiUrl
                + 'branch-clean-preference?cleanPreferenceId='
                + cleanPreferenceId)
            .map((response: Response) => <Branch[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public saveBranches(branches: Branch[], username, domain): Observable<any> {
        const body = JSON.stringify(branches);
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({headers: headers});

        if (username) {
            return this.http.post(this.globalSettingsService.globalSettings.apiUrl
                    + 'save-branches-on-behalf-of-user?username='
                    + username
                    + '&domain='
                    + domain,
                body,
                options)
                .map(res => res.json());
        } else {
            return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'branch',
                    body,
                    options)
                .map(res => {
                    this.userBranchesChanged$.emit(branches);
                    return res.json();
                });
        }
    }
}