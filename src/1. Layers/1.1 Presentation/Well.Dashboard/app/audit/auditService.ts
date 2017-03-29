import {Injectable} from '@angular/core';
import {Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {Audit} from './audit';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';
import {HttpService} from '../shared/httpService';

@Injectable()
export class AuditService {
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) {
    }

    public getAudits(): Observable<Audit[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'audits')
            .map((response: Response) => <Audit[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}