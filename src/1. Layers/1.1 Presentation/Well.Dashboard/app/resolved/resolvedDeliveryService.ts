import {Injectable} from '@angular/core';
import {Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {ResolvedDelivery} from './resolvedDelivery';
import {GlobalSettingsService} from '../shared/globalSettings';
import {HttpErrorService} from '../shared/httpErrorService';
import {HttpService} from '../shared/httpService';

@Injectable()
export class ResolvedDeliveryService {

    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) {
    }

    public getResolvedDeliveries(): Observable<ResolvedDelivery[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'deliveries/resolved')
            .map((response: Response) => <ResolvedDelivery[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}