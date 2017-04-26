import { Injectable }               from '@angular/core';
import { JobStatus }                from './jobStatus';
import { HttpErrorService }         from '../shared/httpErrorService';
import { Observable }               from 'rxjs/Observable';
import { Response }                 from '@angular/http';
import { HttpService }              from '../shared/httpService';
import { GlobalSettingsService }    from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import * as _ from 'lodash';

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

    public getBranchesValueList(): Observable<Array<[string, string]>>
    {
        return this.JobStatus()
            .map((branches: JobStatus[]) =>
            {
                const values = new Array<[string, string]>();

                values.push([undefined, 'All']);
                _.map(branches, (current: JobStatus) => {
                    values.push([current.id.toString(), current.description]);
                });

                return values;
            })
            .catch(e => this.httpErrorService.handleError(e));
    }

    public JobTypes(): Observable<JobStatus[]>
    {
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'JobType')
            .map((response: Response) => <JobStatus[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}