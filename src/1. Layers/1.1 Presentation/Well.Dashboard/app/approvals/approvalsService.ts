import {Injectable} from '@angular/core';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';
import {LogService} from '../shared/logService';
import {ApprovalDelivery} from './approvalDelivery';

@Injectable()
export class ApprovalsService {

   public headers: Headers = new Headers({ 'Content-Type': 'application/json' });
   public options: RequestOptions = new RequestOptions({ headers: this.headers });

    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private logService: LogService) {
    }

    public getApprovals(): Observable<ApprovalDelivery[]> {

        const url = this.globalSettingsService.globalSettings.apiUrl + 'deliveries/approval';

        return this.http.get(url)
            .map((response: Response) => <ApprovalDelivery[]>response.json())
            .do(data => this.logService.log('Approval: ' + JSON.stringify(data)))
            .catch(e => this.httpErrorService.handleError(e));
    }
}