import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {IBranch} from './branch';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';

@Injectable()
export class BranchService {

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    getBranches(): Observable<IBranch[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'branch')
            .map((response: Response) => <IBranch[]>response.json())
            .catch(this.handleError);
    }

    saveBranches(branches: IBranch[]): Observable<any> {
        let body = JSON.stringify(branches);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({headers: headers});

        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'branch', body, options)
            .map(res => res.json());
    }

    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}