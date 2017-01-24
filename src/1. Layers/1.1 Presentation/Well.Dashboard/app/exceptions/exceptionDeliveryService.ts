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
import { Threshold } from '../shared/threshold';
import { DeliveryLine } from '../delivery/model/deliveryLine';

@Injectable()
export class ExceptionDeliveryService {

   public headers: Headers = new Headers({ 'Content-Type': 'application/json' });
   public options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private logService: LogService) {
    }

    public getExceptions(): Observable<ExceptionDelivery[]> {

        const url = this.globalSettingsService.globalSettings.apiUrl + 'deliveries/exception';

        return this.http.get(url)
            .map((response: Response) => <ExceptionDelivery[]>response.json())
            //.do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getUsersForBranch(branchId): Observable<IUser[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'users-for-branch/' + branchId)
            .map((response: Response) => <IUser[]>response.json())
            .do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getConfirmationDetails(jobId: number): Observable<DeliveryLine[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'submission-confirm/' + jobId)
            .map(res => res.json());
    }

    public submitExceptionConfirmation(jobId: number): Observable<any> {
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'confirm-exceptions/' + jobId,
                JSON.stringify(''),
                this.options)
            .map(response => response.json())
            .do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    public creditLines(creditlines: any[]): Observable<any> {
        
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'credit-bulk/{creditlines}',
            JSON.stringify(creditlines),
            this.options)
            .map(res => res.json())
            .do (data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    public getUserCreditThreshold(): Observable<any> {

        const url = this.globalSettingsService.globalSettings.apiUrl + 'credit-threshold/getByUser';

        return this.http.get(url)
            .map((response: Response) => <any>response.json())
            .do(data => this.logService.log('All: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }

    private handleError(error: Response) {
        console.error(error);
        return Observable.throw(error.json().error || ' error');
    }
}