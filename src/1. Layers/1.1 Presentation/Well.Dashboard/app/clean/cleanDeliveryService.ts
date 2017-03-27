import {Injectable} from '@angular/core';
import {Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {CleanDelivery} from './cleanDelivery';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';
import {HttpService} from '../shared/httpService';

@Injectable()
export class CleanDeliveryService {
    constructor(
        private http: HttpService,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) {
    }

    public getCleanDeliveries(): Observable<CleanDelivery[]> {

        const url = this.globalSettingsService.globalSettings.apiUrl + 'deliveries/clean';

        return this.http.get(url)
            .map((response: Response) => <CleanDelivery[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}