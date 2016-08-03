import {Injectable, EventEmitter} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {Branch} from './branch';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';

@Injectable()
export class BranchService {
    public userBranchesChanged$: EventEmitter<Branch[]>;

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) {
        this.userBranchesChanged$ = new EventEmitter<Branch[]>();
    }

    getBranches(username): Observable<Branch[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'branch?username=' + username)
            .map((response: Response) => <Branch[]>response.json())
            .catch(this.handleError);
    }

    saveBranches(branches: Branch[], username, domain): Observable<any> {
        let body = JSON.stringify(branches);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({headers: headers});

        if (username) {
            return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'save-branches-on-behalf-of-user?username=' + username + '&domain=' + domain,
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

    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}