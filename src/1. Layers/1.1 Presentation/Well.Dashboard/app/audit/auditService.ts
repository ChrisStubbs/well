import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {Audit} from './audit';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';

@Injectable()
export class AuditService {
    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) {
    }

    getAudits(): Observable<Audit[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'audits')
            .map((response: Response) => <Audit[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}