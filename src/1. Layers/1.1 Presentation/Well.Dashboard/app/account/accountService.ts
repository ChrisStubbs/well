import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {IAccount} from './account';
import {GlobalSettingsService} from '../shared/globalSettings';

@Injectable()
export class AccountService {

    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) {}

    getAccountByAccountId(accountId: number): Observable<IAccount> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'account/' + accountId)
            .map((response: Response) => <IAccount>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}