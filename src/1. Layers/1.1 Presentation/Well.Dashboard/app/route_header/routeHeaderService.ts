import {Injectable} from 'angular2/core';
import {Http, Response} from 'angular2/http'
import {Observable} from 'rxjs/Observable';
import {IRoute} from './route';
import {GlobalSettingsService} from '../shared/globalSettings';


@Injectable()
export class RouteHeaderService {
    
    constructor(private http: Http, private globalSettingsService: GlobalSettingsService) { }

    getRouteHeaders(): Observable<IRoute[]> {
        
        return this.http.get(this.globalSettingsService.globalSettings.apiUrl + 'routes')
            .map((response: Response) => <IRoute[]>response.json())
            .do(data => console.log("All: " + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private handleError(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}