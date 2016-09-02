import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {ExceptionDelivery} from './exceptionDelivery';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';
import {ToasterService} from 'angular2-toaster/angular2-toaster';
import {IUser} from '../shared/user';
import {UserJob} from '../shared/userJob';

@Injectable()
export class ExceptionDeliveryService {

    headers: Headers = new Headers({ 'Content-Type': 'application/json' });
    options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private toasterService: ToasterService) {
    }

    getExceptions(): Observable<ExceptionDelivery[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'deliveries/exception')
            .map((response: Response) => <ExceptionDelivery[]>response.json())
            //.do(data => console.log("All: " + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e, this.toasterService));
    }

    getUsersForBranch(branchId): Observable<IUser[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'users-for-branch/' + branchId)
            .map((response: Response) => <IUser[]>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e, this.toasterService));
    }


    credit(exception: ExceptionDelivery): Observable<any> {
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'credit',
            JSON.stringify(exception),
            this.options)
            .map(res => res.json());
    }

    assign(userJob: UserJob): Observable<any>{
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'assign-user-to-job',
            JSON.stringify(userJob),
            this.options)
            .map(res => res.json())
            .catch(this.handleError);
    }

    unassign(jobId): Observable<any> {
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'unassign-user-from-job?jobId=' + jobId,
            '',
            this.options)
            .map(res => res.json())
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || ' error');
    }


}