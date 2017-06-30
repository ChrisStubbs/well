import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { HttpService } from '../shared/httpService';
import { GlobalSettingsService } from '../shared/globalSettings';
import { HttpErrorService } from '../shared/httpErrorService';
import { Approval } from './approval';

@Injectable()
export class ApprovalsService
{
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) { }

    public get(): Observable<Approval[]>
    {
        const url = this.globalSettingsService.globalSettings.apiUrl + 'approval/';

        return this.http.get(url)
            .map((response: Response) =>
            {
                const resp = (response.json() as Approval[]);

                return (resp as any[]).map((obj) =>
                {
                    return Object.assign(new Approval(), obj);
                }) as Approval[];
            })
            .catch(e => this.httpErrorService.handleError(e));
    }
}