import {Injectable} from '@angular/core';
import {Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {Branch} from './branch/branch';
import {IUser} from './iuser';
import {GlobalSettingsService} from './globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from './httpErrorService';
import {LogService} from './logService';
import {UserJobs} from './userJobs';
import {HttpService} from './httpService';

@Injectable()
export class UserService {

    constructor(
        public globalSettingsService: GlobalSettingsService,
        private logService: LogService,
        private httpErrorService: HttpErrorService,
        private http: HttpService) {
    }

    public getUsersForBranch(branchId): Observable<IUser[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'users-for-branch/' + branchId)
            .map((response: Response) => <IUser[]>response.json())
            .do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    public assign(userJobs: UserJobs): Observable<any> {
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'assign-user-to-jobs',
            JSON.stringify(userJobs),
            this.globalSettingsService.jsonOptions)
            .map(res => res.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public unassign(jobIds: number[]): Observable<any> {
        return this.http.post(
            this.globalSettingsService.globalSettings.apiUrl + 'unassign-user-from-jobs',
            JSON.stringify(jobIds),
            this.globalSettingsService.jsonOptions)
            .map(res => res.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}