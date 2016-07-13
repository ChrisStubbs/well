import {Injectable} from 'angular2/core';
import {Http, Response} from 'angular2/http';
import {Observable} from 'rxjs/Observable';
import {IAccount} from './account';
import Setting = require("../globalSettings");

@Injectable()
export class AccountService {

    constructor(private http: Http, private globalSettings: Setting.GlobalSettings) {}

    getAccountByStopId(): Observable<IAccount> {

        return this.http.get(this.globalSettings.WellApiUrl + 'account')
            .map((response: Response) => <IAccount>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}