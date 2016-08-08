import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http'
import {Observable} from 'rxjs/Observable';
import {ResolvedDelivery} from './resolvedDelivery';
import {GlobalSettingsService} from '../shared/globalSettings';
import {HttpErrorService} from '../shared/httpErrorService';
import {ToasterService} from 'angular2-toaster/angular2-toaster';

@Injectable()
export class ResolvedDeliveryService {

    constructor(
        private http: Http,
        private globalSettingsService: GlobalSettingsService,
        private httpErrorService: HttpErrorService,
        private toasterService: ToasterService) {
    }

    getResolvedDeliveries(): Observable<ResolvedDelivery[]> {

        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'deliveries/resolved')
            .map((response: Response) => <ResolvedDelivery[]>response.json())
            .catch(e => this.httpErrorService.handleError(e, this.toasterService));
    }
}