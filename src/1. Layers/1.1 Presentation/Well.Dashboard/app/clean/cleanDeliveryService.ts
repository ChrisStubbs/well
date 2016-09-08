import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import {CleanDelivery} from './cleanDelivery';
import {GlobalSettingsService} from '../shared/globalSettings';
import 'rxjs/add/operator/map';
import {HttpErrorService} from '../shared/httpErrorService';

@Injectable()
export class CleanDeliveryService {
    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService) {
    }

    getCleanDeliveries(): Observable<CleanDelivery[]> {

        var url = this.globalSettingsService.globalSettings.apiUrl + 'deliveries/clean';

        return this.http.get(url)
            .map((response: Response) => <CleanDelivery[]>response.json())
            .catch(e => this.httpErrorService.handleError(e));
    }
}