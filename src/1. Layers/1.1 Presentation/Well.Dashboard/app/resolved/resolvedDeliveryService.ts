import {Injectable} from '@angular/core';
import {Http, Response} from '@angular/http'
import {Observable} from 'rxjs/Observable';
import {IResolvedDelivery} from './resolvedDelivery';
import {GlobalSettingsService} from '../shared/globalSettings';


@Injectable()
export class ResolvedDeliveryService {
    
    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    getResolvedDeliveries(): Observable<IResolvedDelivery[]> {
        
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'deliveries/resolved')
            .map((response: Response) => <IResolvedDelivery[]>response.json())
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}