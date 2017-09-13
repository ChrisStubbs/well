import { Injectable } from '@angular/core';
import { Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { HttpService } from '../shared/services/httpService';
import { GlobalSettingsService } from '../shared/globalSettings';
import { HttpErrorService } from '../shared/services/httpErrorService';
import * as _ from 'lodash';
import { BranchDateThreshold } from './branchDateThreshold';

@Injectable()
export class BranchDateThresholdService {
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public getBranchDateThresholds(): Observable<BranchDateThreshold[]> {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'branchDateThreshold')
            .map((response: Response) => <BranchDateThreshold[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public updateBranchDateThresholds(branchDateThresholds: Array<BranchDateThreshold>) {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });
        return this.http.post(this.globalSettingsService.globalSettings.apiUrl + 'updateBranchDateThreshold',
            JSON.stringify(branchDateThresholds), options)
            .catch(e => this.httpErrorService.handleError(e));
    }
}