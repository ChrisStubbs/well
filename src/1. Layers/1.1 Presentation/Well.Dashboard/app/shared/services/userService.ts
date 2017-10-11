import {Injectable}             from '@angular/core';
import {Response}               from '@angular/http';
import {Observable}             from 'rxjs/Observable';
import {IUser}                  from '../models/iuser';
import {GlobalSettingsService}  from '../globalSettings';
import {HttpErrorService}       from './httpErrorService';
import {LogService}             from './logService';
import { UserJobs, AssignJobResult}               from '../models/userJobs';
import {HttpService}            from './httpService';
import 'rxjs/add/operator/map';

@Injectable()
export class UserService {

    constructor(
        public globalSettingsService: GlobalSettingsService,
        private logService: LogService,
        private httpErrorService: HttpErrorService,
        private http: HttpService) { }

    public getUsers(): Observable<IUser[]>
    {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'User')
            .map((response: Response) => <IUser[]>response.json())
            .do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getUsersForBranch(branchId): Observable<IUser[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'users-for-branch/' + branchId)
            .map((response: Response) => <IUser[]>response.json())
            .do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    public assign(userJobs: UserJobs): Observable<AssignJobResult> {
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'assign-user-to-jobs',
            JSON.stringify(userJobs),
            this.globalSettingsService.jsonOptions)
            .map(res => res.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public unassign(jobIds: number[]): Observable<AssignJobResult> {
        return this.http.post(
            this.globalSettingsService.globalSettings.apiUrl + 'unassign-user-from-jobs',
            JSON.stringify(jobIds),
            this.globalSettingsService.jsonOptions)
            .map(res => res.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}