import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {ExceptionDelivery} from './exceptionDelivery';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';
import {IUser} from '../shared/user';
import {UserJob} from '../shared/userJob';
import {LogService} from '../shared/logService';

@Injectable()
export class ExceptionDeliveryService {

    headers: Headers = new Headers({ 'Content-Type': 'application/json' });
    options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private logService: LogService) {
    }

    getExceptions(): Observable<ExceptionDelivery[]> {

        var url = this.globalSettingsService.globalSettings.apiUrl + 'deliveries/exception';

        return this.http.get(url)
            .map((response: Response) => <ExceptionDelivery[]>response.json())
            .do(data => this.logService.log("All: " + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    getUsersForBranch(branchId): Observable<IUser[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'users-for-branch/' + branchId)
            .map((response: Response) => <IUser[]>response.json())
            .do(data => this.logService.log("All: " + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }


    credit(exception: ExceptionDelivery): Observable<any> {
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'credit',
            JSON.stringify(exception),
            this.options)
            .map(res => res.json());
    }

    creditLines(creditlines: any[]): Observable<any> {
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'deliveries-creditlines/{creditlines}',
            JSON.stringify(creditlines),
            this.options)
            .map(res => res.json())
            .do (data => this.logService.log("All: " + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || ' error');
    }
}