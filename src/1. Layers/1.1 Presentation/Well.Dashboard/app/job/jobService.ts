import { Injectable }               from '@angular/core';
import { JobStatus }                from './JobStatus';
import { HttpErrorService }         from '../shared/httpErrorService';
import { Observable }               from 'rxjs/Observable';
import { Response }                 from '@angular/http';
import { HttpService }              from '../shared/httpService';
import { GlobalSettingsService }    from '../shared/globalSettings';
import 'rxjs/add/operator/map';

@Injectable()
export class JobService {
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService)
    {
    }

    public JobStatus(): Observable<JobStatus[]>
    {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'JobStatus')
            .map((response: Response) => <JobStatus[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }

    public JobTypes(): Observable<JobStatus[]>
    {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'JobType')
            .map((response: Response) => <JobStatus[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}