import {Injectable} from '@angular/core';
import {Response} from '@angular/http';
import 'rxjs/Rx';
import 'rxjs/add/operator/map';
import {Observable} from 'rxjs/Observable';
import {IAccount} from './account';
import {GlobalSettingsService} from '../shared/globalSettings';
import {HttpErrorService} from '../shared/httpErrorService';
import {LogService} from '../shared/logService';
import {HttpService} from '../shared/httpService';

@Injectable()
export class AccountService {

    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private logService: LogService) {
    }
    
    public getAccountByAccountId(accountId: number): Observable<IAccount> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'account/' + accountId)
            .map((response: Response) => <IAccount>response.json())
            .do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }
}