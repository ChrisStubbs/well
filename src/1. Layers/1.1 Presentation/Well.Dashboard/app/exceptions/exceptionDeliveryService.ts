import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {ExceptionDelivery} from './exceptionDelivery';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';
import {ToasterService} from 'angular2-toaster/angular2-toaster';

@Injectable()
export class ExceptionDeliveryService {

    constructor(private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private toasterService: ToasterService) {
    }

    getExceptions(): Observable<ExceptionDelivery[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'deliveries/exception')
            .map((response: Response) => <ExceptionDelivery>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e, this.toasterService));
    }

    credit(exception: ExceptionDelivery): Observable<any> {
        let body = JSON.stringify(exception);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'credit',
            body,
            options)
            .map(res => res.json());
    }
}