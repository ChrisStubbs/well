import { Injectable } from '@angular/core';
import { Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { GlobalSettingsService } from '../shared/globalSettings';
import { HttpErrorService } from '../shared/services/httpErrorService';
import { HttpService } from '../shared/services/httpService';
import {ToasterService} from 'angular2-toaster/angular2-toaster';

@Injectable()
export  class JobService {
    constructor(
        private globalSettingsService: GlobalSettingsService,
        private http: HttpService,
        private httpErrorService: HttpErrorService,
        private toasterService: ToasterService) { }

    public setGrnForJob(jobId: number, grn: string): Observable<any>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'job/SetGrn';
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(url, JSON.stringify({ id: jobId, grn: grn }), options)
            .map((response: Response) => { return response; })
            .catch((e: Response) =>
            {
                if (e.status == 400)
                {
                    const msg = e.json().message ? e.json().message : undefined;

                    if (msg)
                    {
                        const data = JSON.parse(msg);

                        if (data.customError)
                        {
                            this.toasterService.pop('error', data.Message, '');
                            return Observable.throw(data.Message);
                        }
                    }
                }

                return this.httpErrorService.handleError(e);
            });
    }
}