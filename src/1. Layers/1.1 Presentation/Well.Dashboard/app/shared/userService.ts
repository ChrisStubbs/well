import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {Branch} from './branch/branch';
import {IUser} from './user';
import {GlobalSettingsService} from './globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from './httpErrorService';
import {LogService} from './logService';
import {UserJob} from './userJob';

@Injectable()
export class UserService {

    constructor(public globalSettingsService: GlobalSettingsService,
        private logService: LogService,
        private httpErrorService: HttpErrorService,
        private http: Http) {
    }

    getUsersForBranch(branchId): Observable<IUser[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'users-for-branch/' + branchId)
            .map((response: Response) => <IUser[]>response.json())
            .do(data => this.logService.log("All: " + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    assign(userJob: UserJob): Observable<any>{
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'assign-user-to-job',
            JSON.stringify(userJob),
            this.globalSettingsService.jsonOptions)
            .map(res => res.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    unassign(jobId): Observable<any> {
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'unassign-user-from-job?jobId=' + jobId,
            '',
            this.globalSettingsService.jsonOptions)
            .map(res => res.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}