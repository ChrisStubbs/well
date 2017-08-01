import { Injectable } from '@angular/core';
import { Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { GlobalSettingsService } from '../shared/globalSettings';
import { HttpErrorService } from '../shared/httpErrorService';
import { LogService } from '../shared/logService';
import { HttpService } from '../shared/httpService';

@Injectable()
export  class JobService {
    constructor(
        private globalSettingsService: GlobalSettingsService,
        private http: HttpService,
        private httpErrorService: HttpErrorService,
        private logService: LogService) {
        
    }

    public setGrnForJob(jobId: number, grn: string): Observable<any> {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'job/SetGrn';
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });
        return this.http.post(url, JSON.stringify({ id: jobId, grn: grn }), options)
            .map((response: Response) => { return response; })
            .do(data => this.logService.log(data))
            .catch(e => this.httpErrorService.handleError(e));
    }
}